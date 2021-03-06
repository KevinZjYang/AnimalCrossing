﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace AnimalCrossing.Models
{
    [Table("LittleAnimal")]
    public class LittleAnimal
    {
        [PrimaryKey, NotNull]
        public string Name { get; set; }

        public string Image { get; set; }
        public string Gender { get; set; }
        public string Brithday { get; set; }
        public string Character { get; set; }
        public string Mantra { get; set; }
        public string Goal { get; set; }
        public string Motto { get; set; }
        public string PersonalStyle { get; set; }
        public string LikeColor { get; set; }
        public string Pitch { get; set; }
        public string ForeignName { get; set; }
        public string HomePic { get; set; }
    }
}
