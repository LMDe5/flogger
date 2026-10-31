using System;
using System.Collections.Generic;
using System.Text;

namespace controlnyaMeow.Core.Attributes
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
