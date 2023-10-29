using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KYRSOVA
{
    public class Game
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }

        public Game(string name, string price, string imageUrl)
        {
            this.Name = name;
            Price = price;
            ImageUrl = imageUrl;
        }
    }
}
