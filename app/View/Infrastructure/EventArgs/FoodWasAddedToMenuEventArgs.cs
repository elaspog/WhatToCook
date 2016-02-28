using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToCook.ViewModels;

namespace WhatToCook.Infrastructure.EventArgs
{
    public class FoodWasAddedToMenuEventArgs
    {
        public RecipeViewModel Recipe { get; set; }
    }
}
