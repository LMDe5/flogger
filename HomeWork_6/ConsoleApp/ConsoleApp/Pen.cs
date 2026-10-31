using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class Pen
    {
        public string name { get; set; }
        public string colour { get; set; }
        public string widht { get; set; }
        public decimal price { get; set; }

        public Pen(string name, string colour, string widht, decimal price)
        {
            this.name = name;
            this.colour = colour;
            this.widht = widht;
            this.price = price;
        }
    }
}
