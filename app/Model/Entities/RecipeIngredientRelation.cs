using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class RecipeIngredientRelation
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public int Index { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        
    }
}
