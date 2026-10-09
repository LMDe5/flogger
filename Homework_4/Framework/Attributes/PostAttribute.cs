using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PostAttribute : Attribute
    {
        public string Route { get; init; }

        public PostAttribute(string route)
        {
            Route = route;
        }
    }
}
