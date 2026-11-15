using System;
using System.Collections.Generic;
using System.Text;

namespace controlnyaMeow.Core.Attributes
{
    public class HttpControllerAttribute : Attribute
    {
        public string Route { get; init; }

        public HttpControllerAttribute(string route)
        {
            Route = route;
        }
    }
}
