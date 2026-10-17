using HttpServer.Framework.HttpResponses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace HttpServer.Framework
{
    internal class ControllerBase
    {
        protected HttpListenerContext Context { get; private set; }

        internal void SetContext(HttpListenerContext Context)
        {
            this.Context = Context;
        } 

        protected JsonResult Json(object obj) => new JsonResult(obj);
        protected HtmlResult Html(string html) => new HtmlResult(html);
    }
}
