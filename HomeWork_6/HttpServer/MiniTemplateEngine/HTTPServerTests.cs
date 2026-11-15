namespace MiniTemplateEngine;
using HttpServer;
using HttpServer.Framework.Handlers;
using HttpServer.Framework.Http;
using System.Net;
using System.Net.Sockets;

[TestClass]
[DoNotParallelize]
public class HTTPServerTests
{
    private static string directory;
    private static string testRoot;

    private HTTPServer server;
    private HttpClient client;
    [ClassInitialize]
    public static void ClassSetup(TestContext context)
    {
        directory = Environment.CurrentDirectory;
        testRoot = Path.Combine(Path.GetTempPath(), "httpserver-tests-" + Guid.NewGuid());

        var staticDir = Path.Combine(testRoot, "static");
        Directory.CreateDirectory(staticDir);

        File.WriteAllText(Path.Combine(staticDir, "search.html"), "<h1>search</h1>");
        File.WriteAllText(Path.Combine(staticDir, "index.html"), "<h1>index</h1>");
        File.WriteAllText(Path.Combine(staticDir, "404.html"), "<h1>404</h1>");
        File.WriteAllText(Path.Combine(staticDir, "style.css"), "body { background: white; }");
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Environment.CurrentDirectory = directory;
        try { Directory.Delete(testRoot, true); }
        catch { };
    }

    [TestInitialize]
    public void SetUp()
    {
        Environment.CurrentDirectory = testRoot;

        int port = GetFreePort();
        WriteSettings(port);

        server = new HTTPServer();
        client = new HttpClient()
        { 
            BaseAddress = new Uri($"http://127.0.0.1:{port}/"),
            Timeout = TimeSpan.FromSeconds(5)
        };
    }

    private static int GetFreePort()
    {
        var l = new TcpListener(IPAddress.Loopback, 0);
        l.Start();
        int port = ((IPEndPoint)l.LocalEndpoint).Port;
        l.Stop();
        return port;
    }

    private static void WriteSettings(int port)
    {
        string json = $"{{\"Port\":\"{port}\",\"Host\":\"127.0.0.1\",\"Path\":\"connection\"}}";
        File.WriteAllText(Path.Combine(testRoot, "settings.json"), json);
    }

    private async Task<(HttpResponseMessage response, string body)> SendAsync(
            Func<HttpClient, Task<HttpResponseMessage>> send)
    {
        server.Start();
        Task loop = Task.Run( () => server.CycleRequest());

        try
        {
            var response = await send(client);
            string body = await response.Content.ReadAsStringAsync();
            return (response, body);
        }
        finally
        {
            server.Stop();
            await Task.WhenAny(loop, Task.Delay(2000));

            if (loop.IsFaulted)
            {
                Console.WriteLine($"CycleRequest упал: {loop.Exception?.GetBaseException().Message}");
            }
        }
    }

    //Tests
    [TestMethod]
    public async Task Server_ReturnSearch_Test()
    {
        var (response, body) = await SendAsync(c => c.GetAsync("/"));

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("<h1>search</h1>", body);
    }
    [TestMethod]
    public async Task Static_Css_ContentType_Test()
    {
        var (response, body) = await SendAsync(c => c.GetAsync("style.css"));

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("text/css", response.Content.Headers.ContentType!.MediaType);
    }
    [TestMethod]
    public async Task Server_ExtensionLess_GettingBachHtml_Test()
    {
        var (response, body) = await SendAsync(c => c.GetAsync("/index"));

        Assert.AreEqual("<h1>index</h1>", body);
    }
    [TestMethod]
    public async Task Server_Return404_Test()
    {
        var (response, body) = await SendAsync(c => c.GetAsync("meow"));

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        Assert.AreEqual("<h1>404</h1>", body);
    }
}