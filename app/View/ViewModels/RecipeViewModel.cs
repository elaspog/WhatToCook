using Model.Entities;
using Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToCook.Common;
using WhatToCook.DataModel;
using WhatToCook.Infrastructure;
using WhatToCook.Infrastructure.Helpers;

namespace WhatToCook.ViewModels
{
    public class RecipeViewModel : Notifier
    {
        private Recipe _recipe;
        private ObservableCollection<RecipeIngredientViewModel> _ingredients;
        private TrulyObservableCollection<RecipeStepViewModel> _steps;
        private bool _isFullyLoaded;
        private bool _isInMenu;

        private bool _menuItemAdded;

        public RecipeViewModel()
        {
            this._recipe = new Recipe();
            this.Steps = new TrulyObservableCollection<RecipeStepViewModel>();
            this.Ingredients = new ObservableCollection<RecipeIngredientViewModel>();
        }

        public RecipeViewModel(Recipe recipe)
        {
            this._recipe = recipe;
            this.Steps = new TrulyObservableCollection<RecipeStepViewModel>();
            this.Ingredients = new ObservableCollection<RecipeIngredientViewModel>();
        }

        public void LoadDetails()
        {
            // make sure it is initialized only once
            if(!this._isFullyLoaded)
            {
                this.Steps.Clear();
                this.Ingredients.Clear();
                using(var context = new Model.Entities.RecipeDbContext())
                {
                    // load steps
                    var steps = from step in context.RecipeSteps
                                where step.RecipeId == this.Id
                                orderby step.Index ascending
                                select step;
                    foreach(var step in steps)
                    {                       
                        this.Steps.Add(new RecipeStepViewModel(step));
                    }

                    // load ingredients
                    var ingredientNodes = context.RecipeIngredientRelations
                                               .Where(r => r.RecipeId == this.Id)
                                               .Join(
                                                context.Ingredients, 
                                                r => r.IngredientId, 
                                                i => i.Id,
                                                (r, i) => new {Relation = r, Ingredient = i}
                                                )
                                                .Select(item => item)
                                                .ToList();

                    foreach (var ingredientNode in ingredientNodes)
                    {
                        this.Ingredients.Add(new RecipeIngredientViewModel(ingredientNode.Ingredient, ingredientNode.Relation));
                    }

                    this._isInMenu = context.MenuItems.Where(item => item.RecipeId == this.Id).Any();
                }
                this._isFullyLoaded = true;
            }
        }

        public void AddMenuItem(MenuItem menuItem)
        {
            foreach (var step in _steps)
            {
                step.MenuItem = menuItem;
            }
        }

        public void SetIsInMenu(bool isInMenu)
        {
            if(this.IsInMenu != isInMenu)
            {
                using (var context = new Model.Entities.RecipeDbContext())
                {
                    if(!this.IsInMenu)
                    {
                        context.MenuItems.Add(new MenuItem
                        {
                            Id = context.GenerateNewId(),
                            RecipeId = this.Id,
                            Portion = 1,
                            CurrentStepIndex = 2
                        });
                        context.SaveChanges();
                    }
                    else
                    {
                        var menuItem = (from menu in context.MenuItems
                                       where menu.RecipeId == this.Id
                                       select menu).FirstOrDefault();
                        if(menuItem != null)
                        {
                            context.MenuItems.Remove(menuItem);
                            context.SaveChanges();
                        }
                    }
                    this._isInMenu = isInMenu;
                }
            }
        }



        public int Id
        {
            get { return this._recipe.Id; }
            set
            {
                this._recipe.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return this._recipe.Name; }
            set
            {
                this._recipe.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get { return this._recipe.Description; }
            set
            {
                this._recipe.Description = value;
                OnPropertyChanged("Description");
            }
        }
        public string ImagePath
        {
            get { return this._recipe.ImagePath; }
            set
            {
                this._recipe.ImagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }
        public int UnitPrice
        {
            get { return this._recipe.UnitPrice; }
            set
            {
                this._recipe.UnitPrice = value;
                OnPropertyChanged("UnitPrice");
                OnPropertyChanged("DisplayUnitPrice");
            }
        }
        public string DisplayUnitPrice
        {
            get { return this._recipe.UnitPrice + " Ft/adag"; }
        }

        public int PreparationTime
        {
            get { return this._recipe.PreparationTime; }
            set
            {
                this._recipe.PreparationTime = value;
                OnPropertyChanged("PreparationTime");
                OnPropertyChanged("DisplayPreparationTime");
            }
        }

        public string DisplayPreparationTime
        {
            get
            {
                return String.Format("~ {0} perc", this.PreparationTime);
            }
        }

        public int Difficulty
        {
            get { return this._recipe.Difficulty; }
            set
            {
                this._recipe.Difficulty = value;
                OnPropertyChanged("Difficulty");
                OnPropertyChanged("DisplayDifficulty");
            }
        }

        public string DisplayDifficulty
        {
            get
            {
                switch(this.Difficulty)
                {
                    case 1:
                        return "Nagyon könnyű";
                    case 2:
                        return "Könnyű";
                    case 3:
                        return "Közepes";
                    case 4:
                        return "Nehéz";
                    case 5:
                        return "Nagyon nehéz";
                    default:
                        return "Ismeretlen";
                }
            }
        }

        public int Stars
        {
            get { return this._recipe.Stars; }
            set
            {
                this._recipe.Stars = value;
                OnPropertyChanged("Stars");
            }
        }
        public bool IsFavourite
        {
            get { return this._recipe.IsFavourite; }
            set
            {
                this._recipe.IsFavourite = value;
                using (var context = new Model.Entities.RecipeDbContext())
                {
                    context.Recipes.Update(this._recipe);
                    context.SaveChanges();
                }   
                OnPropertyChanged("IsFavourite");
            }
        }
        public bool IsInMenu
        {
            get { return this._isInMenu; }
        }
        public ObservableCollection<RecipeIngredientViewModel> Ingredients
        {
            get 
            {
                return this._ingredients; 
            }
            set
            {
                this._ingredients = value;
                OnPropertyChanged("Ingredients");
            }
        }
        public TrulyObservableCollection<RecipeStepViewModel> Steps
        {
            get
            {
                return this._steps;
            }
            set
            {
                this._steps = value;
                this._steps.CollectionChanged+= _steps_CollectionChanged;
                OnPropertyChanged("Steps");
            }
        }

        private void _steps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action.Equals(NotifyCollectionChangedAction.Reset))
            {

                var changed = Steps.FirstOrDefault(s => s.CurrentStepIndexChanged.Equals(true));
                if (changed != null)
                {
                    changed.CurrentStepIndexChanged = false;
                    foreach (var step in Steps)
                    {
                        step.CurrentStepIndex = changed.CurrentStepIndex;
                    }
                }


            }
        }
    }
}
