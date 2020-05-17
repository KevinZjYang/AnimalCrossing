using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCrossing.Models
{
    public class NormalAlbum
    {
        public string Name { get; set; }

        public int Number { get; set; }
        public string ForeignName { get; set; }
        public string Cover { get; set; }
        public string Source { get; set; }
        public string BuyPrice { get; set; }
        public string SalePrice { get; set; }

        public bool Owned { get; set; }
    }
}
