using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServer.framework.core
{
    public class Settings
    {
        public string Port { get; set; } = "8888";
        public string Host { get; set; } = "127.0.0.1";
        public string Path { get; set; } = "connection";
    }
}