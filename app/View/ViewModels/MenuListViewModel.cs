using Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToCook.Common;
using WhatToCook.Infrastructure;
using WhatToCook.Infrastructure.EventArgs;
using WhatToCook.Infrastructure.Helpers;

namespace WhatToCook.ViewModels
{
    public class MenuListViewModel : TrulyObservableCollection<MenuItemViewModel>, INotifyPropertyChanged
    {

        public MenuListViewModel()
        {
            this.CollectionChanged += collectionChanged;
        }

        private void collectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            loadIngredients();
            OnPropertyChanged("Ingredients");
        }


        ActionCommand<RecipeViewModel> _removeItemCommand;          //ActionCommand derivides from ICommand
        public ActionCommand<RecipeViewModel> RemoveItemCommand
        {
            get
            {
                _removeItemCommand = new ActionCommand<RecipeViewModel>(param =>
                {
                    var itemToRemove = this.FirstOrDefault(x => x.RecipeId.Equals(param.Id));
                    itemToRemove.Recipe.SetIsInMenu(false);
                    this.Remove(itemToRemove);
                });
                return _removeItemCommand;
            }
        }

        private ObservableCollection<RecipeIngredientViewModel> _ingredients;

        private ObservableCollection<RecipeIngredientViewModel> loadIngredients()
        {
            _ingredients = new ObservableCollection<RecipeIngredientViewModel>();
            foreach (var menuItem in this)
            {
                int portion = menuItem.Portion;
                foreach (var ingredient in menuItem.Recipe.Ingredients)
                {
                    var previous = _ingredients.FirstOrDefault(x => x.IngredientId.Equals(ingredient.IngredientId));
                    if (previous != null)
                    {
                        previous.Quantity += portion * ingredient.Quantity;
                    }
                    else
                    {
                        RecipeIngredientViewModel newIngredient = new RecipeIngredientViewModel(ingredient, portion);
                        _ingredients.Add(newIngredient);
                    }
                }
            }
            return _ingredients;
        }

        public ObservableCollection<RecipeIngredientViewModel> Ingredients
        {
            get
            {
                _ingredients = loadIngredients() ;
                return _ingredients;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
