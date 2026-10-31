using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConsoleApp
{
    public class User
    {
        public string name { get; set; }
        public string group { get; set; }
        public int age { get; set; }
        public bool sex { get; set; }

        public User(string name, string group, int age, bool sex)
        {
            this.name = name;
            this.group = group;
            this.age = age;
            this.sex = sex;
        }
    }
}
