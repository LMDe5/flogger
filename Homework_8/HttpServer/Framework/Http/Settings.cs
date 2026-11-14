using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.Framework.Http
{
    public class Settings
    {
        public string Port { get; set; } = "8888";
        public string Host { get; set; } = "127.0.0.1";
        public string Path { get; set; } = "connection";
        public string Connection { get; set; } = "Host=localhost;Port=5432;Database=myDataBase;Username=postgres;Password=piskoed228";
    }
}
