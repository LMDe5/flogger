using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace HttpServer.Framework.HttpResponses
{
    internal class HtmlResult : IHttpResponseTypeResult
    {
        private readonly string html;

        public HtmlResult(string html)
        {
            this.html = html;
        }

        public byte[] Execute(HttpListenerContext context)
        {
            var html = this.html;
            context.Response.ContentType = "text/html";

            byte[] byffer = Encoding.UTF8.GetBytes(html);
            return byffer;
        }
    }
}
