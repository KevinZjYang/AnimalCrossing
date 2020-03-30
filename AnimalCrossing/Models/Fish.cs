using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCrossing.Models
{
    public class Fish
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string English { get; set; }
        public string Japanese { get; set; }
        public string Price { get; set; }
        public string Position { get; set; }
        public string Shape { get; set; }
        public string Time { get; set; }
        public Hemisphere Hemisphere { get; set; }
    }
}
