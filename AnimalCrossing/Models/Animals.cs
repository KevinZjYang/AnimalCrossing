using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCrossing.Models
{
    [Table("Insect")]
    public class AnimalsInsect
    {
        [PrimaryKey]
        public string Name { get; set; }

        public string Image { get; set; }
        public int Number { get; set; }
        public string English { get; set; }
        public string Japanese { get; set; }
        public string Position { get; set; }
        public string Weather { get; set; }
        public string North { get; set; }
        public string South { get; set; }
        public string Time { get; set; }
        public int Price { get; set; }
    }

    [Table("Fish")]
    public class AnimalsFish
    {
        [PrimaryKey]
        public string Name { get; set; }

        public string Image { get; set; }
        public int Number { get; set; }
        public string English { get; set; }
        public string Japanese { get; set; }
        public string Position { get; set; }
        public string Shape { get; set; }
        public string North { get; set; }
        public string South { get; set; }
        public string Time { get; set; }
        public int Price { get; set; }
    }
}
