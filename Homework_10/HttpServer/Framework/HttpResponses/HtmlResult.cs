using System.Net;
using System.Text;

namespace HttpServer.Framework.HttpResponses
{
    internal class HtmlResult : IHttpResponseTypeResult
    {
        private readonly string html;

        public HtmlResult(string html)
        {
            this.html = html;
        }

        public byte[] Execute(HttpListenerContext context)
        {
            var html = this.html;
            context.Response.ContentType = "text/html; charset=utf-8";

            byte[] byffer = Encoding.UTF8.GetBytes(html);
            return byffer;
        }
    }
}
