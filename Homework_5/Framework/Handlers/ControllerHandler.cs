using HttpServer.Framework.Attributes;
using HttpServer.Framework.HttpResponses;
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
                //[0]="/"; [1]="auth/"; [2]="login/"
                string[] strParams = context.Request.Url
                   .Segments
                   .Skip(2)
                   .Select(s => s.Replace("/", ""))
                   .ToArray();

                var assembly = Assembly.GetExecutingAssembly();

                //ну а тут я получаю auth, steam и тд 
                string controllerName = context.Request.Url.Segments[1].Replace("/", "").ToLower();
                var controller = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(HttpControllerAttribute))).FirstOrDefault(c => { var attr = c.GetCustomAttribute<HttpControllerAttribute>(); return attr != null && attr.Route.ToLower() == controllerName; });

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

                //получаем тут типы методов "логин", "жсон", "хтмл" или другие
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
                
                //создаем экземпляр authControllera (сейчас :з)
                var controllerInstance = Activator.CreateInstance(controller);

                //после этого в ControllerBase доступен context
                if (controllerInstance is ControllerBase controllerBase)
                {
                    //даем доступ к запросу/ответу
                    controllerBase.SetContext(context);
                }

                Console.WriteLine($"[Controller] context передан: {(controllerInstance is ControllerBase ? "да" : "нет (не ControllerBase)")}");

                //раскладываем пришедшую инфу на ключ-значение
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

                //собираем значения для параметров метод (login="admin", password="123")
                object[] queryParams = method.GetParameters()
                    .Select(p => Convert.ChangeType(
                        source.TryGetValue(p.Name!, out var v) ? v : "",
                        p.ParameterType))
                    .ToArray();

                //вызываем все методы в authController (или в экземпляре с context)
                var ret = method.Invoke(controllerInstance, queryParams);

                if (ret is IHttpResponseTypeResult httpResponseTypeResult)
                {
                    //имеем html/json и отправляем байты

                    byte[] buffer = httpResponseTypeResult.Execute(context);
                    response.ContentLength64 = buffer.Length;

                    using Stream output = response.OutputStream;
                    await output.WriteAsync(buffer);
                    await output.FlushAsync();
                    response.Close();
                }
                else
                {
                    response.StatusCode = 200;
                    response.Close();
                }
            }
            else if (Successor != null)
            {
                await Successor.HandleRequest(context);
            }
        }
    }
}
