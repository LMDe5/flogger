using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class Book
    {
        public string name;
        public string author;
        public string realiseDate;

        public Book(string name, string author, string realiseDate)
        {
            this.name = name;
            this.author = author;
            this.realiseDate = realiseDate;
        }
    }
}
