using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConsoleApp
{
    public class User
    {
        public string name = "Ваня";
        public string group = "11-541";
        public int age = 22;
        public bool sex = true;

        public User(string name, string group, int age, bool sex)
        {
            this.name = name;
            this.group = group;
            this.age = age;
            this.sex = sex;
        }
    }
}
