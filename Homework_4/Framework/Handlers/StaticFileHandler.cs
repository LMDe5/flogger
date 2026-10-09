using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpServer.Framework.Handlers
{
    class StaticFileHandler : Handler
    {
        public override async Task HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string path = request.Url.LocalPath;
            string relativePath = path;

            bool isFile = false;

            if (!isFile)
            {
                if (string.IsNullOrEmpty(relativePath) || relativePath == "/")
                {
                    relativePath = "/search.html";
                }
                else if (relativePath == "/Login_Form")
                {
                    relativePath = "/Login_Form/login.html";
                }
                else if (relativePath == "/satisfactory")
                {
                    relativePath = "/copyfactory.html";
                }
                else if (relativePath == "/steam")
                {
                    relativePath = "/steam.html";
                }

                string filePath = Directory.GetCurrentDirectory() + $"/static{relativePath}";
                FileInfo fileInfo = new FileInfo(filePath);

                if (fileInfo.Extension == "")
                {
                    if (File.Exists(filePath + ".html"))
                    {
                        filePath += ".html";
                        fileInfo = new FileInfo(filePath);
                    }
                }

                if (!fileInfo.Exists)
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

                switch (fileInfo.Extension)
                {
                    case ".html":
                        response.ContentType = "text/html; charset=utf-8";
                        break;
                    case ".css":
                        response.ContentType = "text/css; charset=utf-8";
                        break;
                    case ".js":
                        response.ContentType = "text/javascript; charset=utf-8";
                        break;
                    case ".png":
                        response.ContentType = "image/png";
                        break;
                    case ".ico":
                        response.ContentType = "image/x-icon";
                        break;
                    case ".svg":
                        response.ContentType = "image/svg+xml";
                        break;
                    case ".jpg":
                        response.ContentType = "image/jpeg";
                        break;
                }

                byte[] buffer = await File.ReadAllBytesAsync(filePath);
                response.ContentLength64 = buffer.Length;

                using Stream output = response.OutputStream;
                await output.WriteAsync(buffer);
                await output.FlushAsync();

                //Console.WriteLine($"Запрос отправлен: {relativePath}");
                response.Close();
            }
            else if (Successor != null)
            {
                await Successor.HandleRequest(context);
            }
            else
            {
                response.StatusCode = 404;
                response.Close();
            }
        }
    }
}
