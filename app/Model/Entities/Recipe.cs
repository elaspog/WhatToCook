using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Recipe
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int UnitPrice { get; set; }
        public int PreparationTime { get; set; }
        public int Difficulty {get; set; }
        public bool IsFavourite { get; set; }
        public int Stars { get; set; }
    }
}
