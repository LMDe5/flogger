using HttpServer.Controllers;
using HttpServer.Framework.Attributes;
using HttpServer.Framework.HttpResponses;

namespace MiniHttpServer.Controllers
{
    [HttpController("rules")]
    internal class RuleController : ControllerBase
    {

        [Get("")]
        public IHttpResponseTypeResult Rules()
        {
            return Html("semestrovaya/rulesPage.html", new { });
        }

    }
}
