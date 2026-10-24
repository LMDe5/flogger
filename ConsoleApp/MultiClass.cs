using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class MultiClass
    {
        public string name { get; set; }
        public bool gender { get; set; }
        public List<Pen> items { get ; set; }

        public MultiClass(string name, bool gender, List<Pen> items)
        {
            this.name = name;
            this.gender = gender;
            this.items = items;
        }
    }
}