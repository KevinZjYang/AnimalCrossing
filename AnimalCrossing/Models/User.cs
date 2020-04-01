using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCrossing.Models
{
    [Table("Insect")]
    public class UserInsect
    {
        [PrimaryKey]
        public string Name { get; set; }
        public bool Owned { get; set; }
        public bool MuseumHave { get; set; }
    }

    [Table("Fish")]
    public class UserFish
    {
        [PrimaryKey]
        public string Name { get; set; }
        public bool Owned { get; set; }
        public bool MuseumHave { get; set; }
    }
}
