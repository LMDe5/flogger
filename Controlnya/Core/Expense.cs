using System;
using System.Collections.Generic;
using System.Text;

namespace controlnyaMeow.Core
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; } = "pending";
        public int Count { get; set; }
    }
}
