using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace controlnyaMeow.Core.HttpResponses
{
    internal interface IHttpResponseTypeResult
    {
        byte[] Execute(HttpListenerContext context);
    }
}
