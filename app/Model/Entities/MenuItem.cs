using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int CurrentStepIndex { get; set; }
        public int Portion { get; set; }
    }
}
