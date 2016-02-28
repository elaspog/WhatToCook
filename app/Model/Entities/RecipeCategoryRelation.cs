using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class RecipeCategoryRelation
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int RecipeId { get; set; }
    }
}
