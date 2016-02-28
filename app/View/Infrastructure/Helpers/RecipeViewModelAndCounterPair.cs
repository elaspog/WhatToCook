using Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToCook.ViewModels;

namespace WhatToCook.Infrastructure.Helpers
{
    public class RecipeViewModelAndCounterPair : Notifier
    {
        private RecipeViewModel recipe;
        public RecipeViewModel Recipe 
        { 
            get { return recipe; }
            set 
            {
                recipe = value;
                OnPropertyChanged("Recipe");
            }         
        }


        private int counter;
        public int Counter
        {
            get { return counter; }
            set
            {
                if (value >= 0)
                {
                    counter = value;
                    OnPropertyChanged("Counter");
                }
            }
        }

    }
}
