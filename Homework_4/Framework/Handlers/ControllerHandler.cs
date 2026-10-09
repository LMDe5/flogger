using HttpServer.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;

namespace HttpServer.Framework.Handlers
{
    internal class ControllerHandler : Handler
    {
        public override async Task HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string path = request.Url.LocalPath.Trim('/');

            bool isFile = path.Contains(".");

            if (!isFile) 
            {
                string[] strParams = context.Request.Url
                   .Segments
                   .Skip(2)
                   .Select(s => s.Replace("/", ""))
                   .ToArray();

                var assembly = Assembly.GetExecutingAssembly();

                string controllerName = context.Request.Url.Segments[1].Replace("/", "").ToLower();
                var controller = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(HttpControllerAttribute))).FirstOrDefault(c => c.Name.ToLower() == controllerName+"controller");

                if (controller == null)
                {
                    if (Successor != null)
                    {
                        await Successor.HandleRequest(context);
                    }
                    else
                    {
                        response.StatusCode = 404;
                        response.Close();
                    }
                    return;
                }

                string methodRoute = context.Request.Url.Segments.Length > 2 ?
                    context.Request.Url.Segments[2].Replace("/", "").ToLower()
                    : "";
                var method = controller.GetMethods()
                    .Where(t =>
                    {
                        string httpMethod = context.Request.HttpMethod;
                        foreach (var attr in t.GetCustomAttributes(true))
                        {
                            string name = attr.GetType().Name.Replace("Attribute", "");
                            if (!name.Equals(httpMethod, StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }
                            var routeProp = attr.GetType().GetProperty("Route");
                            string route = routeProp?.GetValue(attr)?.ToString()?.ToLower() ?? "";
                            if (route == methodRoute)
                            {
                                return true;
                            }
                        }
                        return false;
                    })
                    .FirstOrDefault();

                if (method == null)
                { 
                    if (Successor != null)
                    {
                        await Successor.HandleRequest(context);
                    }
                    else
                    {
                        response.StatusCode = 404;
                        response.Close();
                    }
                    return;
                }

                var source = new Dictionary<string, string>();

                if (request.HttpMethod == "POST" && request.HasEntityBody)
                {
                    var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    string body = reader.ReadToEnd();
                    foreach (var pair in body.Split('&', StringSplitOptions.RemoveEmptyEntries))
                    {
                        var kv = pair.Split('=', 2);
                        string key = Uri.UnescapeDataString(kv[0].Replace('+', ' '));
                        string val = kv.Length > 1
                            ? Uri.UnescapeDataString(kv[1].Replace('+', ' '))
                            : "";
                        source[key] = val;
                    }
                }
                else
                {
                    foreach (string key in request.QueryString.AllKeys)
                    {
                        if (key != null)
                        {
                            source[key] = request.QueryString[key] ?? "";
                        }
                    }
                }

                object[] queryParams = method.GetParameters()
                    .Select(p => Convert.ChangeType(
                        source.TryGetValue(p.Name!, out var v) ? v : "",
                        p.ParameterType))
                    .ToArray();

                var ret = method.Invoke(Activator.CreateInstance(controller), queryParams);

                response.StatusCode = 200;
                response.Close();
            }
            else if (Successor != null)
            {
                await Successor.HandleRequest(context);
            }
        }
    }
}
