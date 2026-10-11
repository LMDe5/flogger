using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpServer.Framework.HttpResponses
{
    internal interface HttpResponseTypeResult
    {
        byte[] Execute(HttpListenerContext context);
    }
}
