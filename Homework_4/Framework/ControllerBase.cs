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
        protected HttpListener context { get; set; }

        internal void SetContext(HttpListener context)
        {
            this.context = context;
        } 

        protected JsonResult Json(object obj) => new JsonResult(obj);
        protected JsonResult Json(string str) => new JsonResult(str);
    }
}
