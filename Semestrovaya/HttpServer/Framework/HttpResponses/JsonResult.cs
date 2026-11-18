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
        private readonly int statusCode;

        public JsonResult(object data, int statusCode = 200)
        {
            this.data = data;
            this.statusCode = statusCode;
        }

        public byte[] Execute(HttpListenerContext context)
        {
            var json = JsonSerializer.Serialize(data);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = statusCode;

            byte[] byffer = Encoding.UTF8.GetBytes(json);
            return byffer;
        }
    }
}
