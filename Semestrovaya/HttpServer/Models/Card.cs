using System;
using System.Collections.Generic;
using System.Text;

namespace MiniHttpServer.Models
{
    public class Card
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Alignment { get; set; }

        public string Ability { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Card() { }
    }
}
