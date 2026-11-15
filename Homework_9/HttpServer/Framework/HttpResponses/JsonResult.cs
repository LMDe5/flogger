using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace HttpServer.Framework.HttpResponses
{
    internal class JsonResult : IHttpResponseTypeResult
    {
        private readonly object data;

        public JsonResult(object data)
        {
            this.data = data;
        }

        public byte[] Execute(HttpListenerContext context)
        {
            var json = JsonSerializer.Serialize(data);
            context.Response.ContentType = "application/json";

            byte[] byffer = Encoding.UTF8.GetBytes(json);
            return byffer;
        }
    }
}
