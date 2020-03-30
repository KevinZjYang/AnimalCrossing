using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCrossing.Models
{
    public class Insect
    {       
        public string Name { get; set; }
        public string Number { get; set; }
        public string English { get; set; }
        public string Japanese { get; set; }
        public string Price { get; set; }
        public string Position { get; set; }
        public string Weather { get; set; }
        public string Time { get; set; }
        public Hemisphere Hemisphere { get; set; }

    }
    public class Hemisphere
    {
        public North North { get; set; }
        public South South { get; set; }
    }

    public class North
    {
        public Month Month { get; set; }
    }

    public class South
    {
        public Month Month { get; set; }
    }

    public class Month
    {
        public List<string> AppearMonth { get; set; }
    }
}
