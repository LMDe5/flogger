using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class Book
    {
        public string name { get; set; }
        public string author { get; set; }
        public User realiseDate { get; set; }

        public Book(string name, string author, User realiseDate)
        {
            this.name = name;
            this.author = author;
            this.realiseDate = realiseDate;
        }
    }
}
