using System;
using System.Collections.Generic;
using System.Text;

namespace controlnyaMeow.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GetAttribute : Attribute
    {
        public string Route { get; init; }

        public GetAttribute(string route)
        {
            Route = route;
        }
    }
}
