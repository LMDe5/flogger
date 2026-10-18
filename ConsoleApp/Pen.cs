using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class Pen
    {
        public string name;
        public string colour;
        public string mm;
        public decimal price;

        public Pen(string name, string colour, string mm, decimal price)
        {
            this.name = name;
            this.colour = colour;
            this.mm = mm;
            this.price = price;
        }
    }
}
