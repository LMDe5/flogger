using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.IO;

namespace HttpServer.framework.core
{
    internal class HTTPServer
    {
        private HttpListener server = new HttpListener();
        private bool serverRuns = false;
        private Settings settings;

        public HTTPServer()
        {
            ReadJson();
        }

        private void ReadJson()
        {
            string jsonStrings = File.ReadAllText("settings.json");
            settings = JsonSerializer.Deserialize<Settings>(jsonStrings);

            if (settings == null)
            {
                Console.WriteLine("не смогли считать settings.json");
                return;
            }

            string prefix = $"http://{settings.Host}:{settings.Port}/";

            server.Prefixes.Add(prefix);
        }

        public void Start()
        {
            serverRuns = true;

            server.Start();
        }

        public void Stop()
        {
            serverRuns = false;

            server.Stop();
        }

        public void manageServer()
        {
            CycleRequest();

            Console.WriteLine("Сервер запущен");
            while (true)
            {
                Console.WriteLine("Для остановки напишите 'stop'");
                string str = Console.ReadLine();
                if (str == "stop")
                {
                    server.Stop();
                    break;
                }
            }
        }

        public async Task CycleRequest()
        {
            while (serverRuns)
            {
                HttpListenerContext context;
                try
                {
                    context = await server.GetContextAsync();
                }
                catch (HttpListenerException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }



                var request = context.Request;
                var response = context.Response;

                string path = request.Url.LocalPath;
                string prefix = $"/{settings.Path}";
                string relativePath = path.Substring(prefix.Length);

                if (relativePath == "/Login_Form")
                {
                    relativePath = "/Login_Form/login.html";
                }

                if (string.IsNullOrEmpty(relativePath) || relativePath == "/")
                {
                    relativePath = "/search.html";
                }
                else if (relativePath == "/satisfactory")
                {
                    relativePath = "/copyfactory.html";
                }
                else if (relativePath == "/steam")
                {
                    relativePath = "/steam.html";
                }


                string filePath = Directory.GetCurrentDirectory() + $"/static{relativePath}";
                FileInfo fileInfo = new FileInfo(filePath);


                if (fileInfo.Extension == "")
                {
                    string[] extensions = { ".html", ".png" };
                    foreach (string extension in extensions)
                    {
                        if (File.Exists(filePath + extension))
                        {
                            filePath += extension;
                            fileInfo = new FileInfo(filePath);
                            break;
                        }
                    }
                }

                if (!fileInfo.Exists)
                {
                    string staticRoot = Directory.GetCurrentDirectory() + "/static";
                    string[] hits = Directory.GetFiles(staticRoot, Path.GetFileName(filePath), SearchOption.AllDirectories);

                    if (hits.Length > 0)
                    {
                        filePath = hits[0];
                        fileInfo = new FileInfo(filePath);
                    }
                }

                if (!fileInfo.Exists)
                {
                    response.StatusCode = 404;
                    relativePath = "/404.html";
                    filePath = Directory.GetCurrentDirectory() + $"/static{relativePath}";
                    fileInfo = new FileInfo(filePath);
                }

                switch (fileInfo.Extension)
                {
                    case ".html":
                        response.ContentType = "text/html; charset=utf-8";
                        break;
                    case ".css":
                        response.ContentType = "text/css; charset=utf-8";
                        break;
                    case ".js":
                        response.ContentType = "text/javascript; charset=utf-8";
                        break;
                    case ".png":
                        response.ContentType = "image/png";
                        break;
                    case ".ico":
                        response.ContentType = "image/x-icon";
                        break;
                    case ".svg":
                        response.ContentType = "image/svg+xml";
                        break;
                    case ".jpg":
                        response.ContentType = "image/jpeg";
                        break;
                }

                byte[] buffer = await File.ReadAllBytesAsync(filePath);
                response.ContentLength64 = buffer.Length;

                using Stream output = response.OutputStream;
                await output.WriteAsync(buffer);
                await output.FlushAsync();

                Console.WriteLine($"Запрос отправлен: {relativePath}");
                response.Close();
            }
        }
    }
}