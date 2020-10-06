using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderBySpeechWebApp.Models
{
    public class OrderResponse
    {
        public OrderResponse()
        {
            Person = new List<string>();
            Location = new List<string>();
            Product = new List<string>();
        }

        public List<string> Person { get; set; }
        public List<string> Location { get; set; }
        public List<string> Product { get; set; }
    }
}
