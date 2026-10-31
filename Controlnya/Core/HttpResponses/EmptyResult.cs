using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace controlnyaMeow.Core.HttpResponses
{
    public class EmptyResult : IHttpResponseTypeResult
    {
        private readonly int code;
        private readonly string location;

        public EmptyResult(string location, int code)
        {
            this.location = location;
            this.code = code;
        }

        public byte[] Execute(HttpListenerContext context)
        {
            context.Response.StatusCode = code;
            context.Response.RedirectLocation = location;

            return Array.Empty<byte>();
        }
    }
}

