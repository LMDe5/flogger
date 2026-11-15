

namespace controlnyaMeow.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PutAttribute : Attribute
    {
        public string Route { get; init; }

        public PutAttribute(string route)
        {
            Route = route;
        }
    }
}
