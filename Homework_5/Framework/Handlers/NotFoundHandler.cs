using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpServer.Framework.Handlers
{
    internal class NotFoundHandler : Handler
    {
        public override async Task HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            response.StatusCode = 404;
            response.ContentType = "text/html; charset=utf-8";

            string path = Directory.GetCurrentDirectory() + $"/static/{"404.html"}";

            if (File.Exists(path))
            {
                byte[] buffer = await File.ReadAllBytesAsync(path);
                response.ContentLength64 = buffer.Length;

                using Stream output = response.OutputStream;
                await output.WriteAsync(buffer);
                await output.FlushAsync();
            }
            response.Close();
        }
    }
}
