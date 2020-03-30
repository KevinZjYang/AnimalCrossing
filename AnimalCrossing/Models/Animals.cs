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
        public string Data { get; set; }
    }

    [Table("Fish")]
    public class AnimalsFish
    {
        [PrimaryKey]
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
