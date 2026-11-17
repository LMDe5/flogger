using HttpServer;
using HttpServer.Framework.Handlers;
using MiniHttpServer.Services.Authentication;
using ServiceStack;
using System.Net;
using System.Net.Sockets;

namespace MiniTemplateEngine
{
    [TestClass]
    [DoNotParallelize]
    public sealed class HttpServerHandlersTest
    {
        private static string directory;
        private static string testRoot;

        private HttpListener listener;
        private HttpClient client;

        [ClassInitialize]
        public static void ClassSetup(TestContext context)
        {
            directory = Environment.CurrentDirectory;
            testRoot = Path.Combine(Path.GetTempPath(), "orm-tests-" + Guid.NewGuid());

            var staticDir = Path.Combine(testRoot, "static");
            Directory.CreateDirectory(staticDir);
            Directory.CreateDirectory(Path.Combine(staticDir, "Login_Form"));

            File.WriteAllText(Path.Combine(staticDir, "search.html"), "<h1>search</h1>");
            File.WriteAllText(Path.Combine(staticDir, "index.html"), "<h1>index</h1>");
            File.WriteAllText(Path.Combine(staticDir, "404.html"), "<h1>404</h1>");
            File.WriteAllText(Path.Combine(staticDir, "Login_Form", "login.html"), "<h1>login-form</h1>");
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
            listener = new HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{port}/");
            listener.Start();

            client = new HttpClient() { BaseAddress = new Uri($"http://127.0.0.1:{port}/") };
        }

        private static int GetFreePort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        private async Task<HttpResponseMessage> SendAsync(Handler handler,
            Func<HttpClient, Task<HttpResponseMessage>> send)
        {
            var serverTask = Task.Run(async () =>
            {
                var context = await listener.GetContextAsync();
                try
                {
                    await handler.HandleRequest(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ощибка хендлера: {ex.Message}");
                }
            });

            var response = await send(client);
            await Task.WhenAny(serverTask, Task.Delay(2000));
            return response;
        }


        //staticHandler
        [TestMethod]
        public async Task Static_ReturnSearch_Test()
        {
            var responce = await SendAsync(new StaticFileHandler(), c => c.GetAsync("/"));
            Assert.AreEqual(HttpStatusCode.OK, responce.StatusCode);
            Assert.AreEqual("<h1>search</h1>", await responce.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task Static_Css_ContentType_Test()
        {
            var response = await SendAsync(new StaticFileHandler(), c => c.GetAsync("/style.css"));
            Assert.AreEqual("text/css", response.Content.Headers.ContentType!.MediaType);
        }
        [TestMethod]
        public async Task Static_ExtensionLess_GettingBachHtml_Test()
        {
            var response = await SendAsync(new StaticFileHandler(), c => c.GetAsync("/index"));
            Assert.AreEqual("<h1>index</h1>", await response.Content.ReadAsStringAsync());
        }
        [TestMethod]
        public async Task Static_LoginForm_Test()
        {
            var response = await SendAsync(new StaticFileHandler(), c => c.GetAsync("/Login_Form"));
            Assert.AreEqual("<h1>login-form</h1>", await response.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task Static_404_Test()
        {
            var response = await SendAsync(new StaticFileHandler(), c => c.GetAsync("/meow.html"));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        //NotFoundHandler
        [TestMethod]
        public async Task NotFound_404_Test()
        {
            var response = await SendAsync(new NotFoundHandler(), c => c.GetAsync("/meow"));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("<h1>404</h1>", await response.Content.ReadAsStringAsync());
        }

        //ControllerHandler
        [TestMethod]
        public async Task Controller_AuthLogin_Test()
        {
            var sessionService = new UserSessionService();
            var response = await SendAsync(new ControllerHandler(sessionService),
                c => c.PostAsync("/auth/login", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("login", "student@g.com"),
                    new KeyValuePair<string, string>("password", "test123")
                }))); 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod]
        public async Task Controller_Authorize_WrondData_Return401()
        {
            var sessionService = new UserSessionService();
            var response = await SendAsync(new ControllerHandler(sessionService),
                c => c.PostAsync("/auth/login", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("login", "meow"),
                    new KeyValuePair<string, string>("password", "meow")
                })));
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [TestMethod]
        public async Task Controller_Authorize_WithoutAttribute_Return200()
        {
            var sessionService = new UserSessionService();
            var response = await SendAsync(new ControllerHandler(sessionService),
                c => c.GetAsync("/auth/test"));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Controller_Authorize_WithoutSession_Return401()
        {
            var sessionService = new UserSessionService();
            var response = await SendAsync(new ControllerHandler(sessionService),
                c => c.GetAsync("/auth/profile"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Controller_Authorize_WithSession_Returns200()
        {
            var sessionService = new UserSessionService();
            var response = await SendAsync(new ControllerHandler(sessionService),
                c => c.PostAsync("/auth/login", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("login", "student@g.com"),
                    new KeyValuePair<string, string>("password", "test123")
                })));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var profileResponse = await SendAsync(new ControllerHandler(sessionService),
                c => c.GetAsync("/auth/profile"));
            Assert.AreEqual(HttpStatusCode.OK, profileResponse.StatusCode);
        }
    }
}