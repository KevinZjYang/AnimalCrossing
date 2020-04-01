using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCrossing.Models
{
    public class NormalAnimals
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Number { get; set; }
        public string English { get; set; }
        public string Japanese { get; set; }
        public int Price { get; set; }
        public string Position { get; set; }
        public string ShapeOrWeather { get; set; }
        public string Time { get; set; }
        public List<string> AppearMonth { get; set; }

        public bool Owned { get; set; }
        public bool MuseumHave { get; set; }

    }
}
