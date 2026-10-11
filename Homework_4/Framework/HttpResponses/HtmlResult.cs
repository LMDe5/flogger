using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace HttpServer.Framework.HttpResponses
{
    internal class HtmlResult : HttpResponseTypeResult
    {
        private readonly object data;

        public HtmlResult(object data)
        {
            this.data = data;
        }

        public byte[] Execute(HttpListenerContext context)
        {
            var json = JsonSerializer.Serialize(data);
            context.Response.ContentType = "text/html";

            byte[] byffer = Encoding.UTF8.GetBytes(json);
            return byffer;
        }
    }
}
