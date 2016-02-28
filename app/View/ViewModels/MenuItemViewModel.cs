using Model.Infrastructure;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WhatToCook.Infrastructure.Helpers;

namespace WhatToCook.ViewModels
{
    public class MenuItemViewModel : Notifier
    {
        private MenuItem _menuItem;
        private RecipeViewModel _recipe;

        public MenuItemViewModel()
        {
            this._menuItem = new MenuItem();
            this._recipe = new RecipeViewModel();
            this._optionList = new ObservableCollection<int>(new int[] { 1,2,3,4,5,6,7,8,9 });
        }
        public MenuItemViewModel(MenuItem menuItem)
        {
            this._menuItem = menuItem;
            this._recipe = new RecipeViewModel();
            this._optionList = new ObservableCollection<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }

        public int Id
        {
            get { return this._menuItem.Id; }
            set
            {
                this._menuItem.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public int Portion
        {
            get { return this._menuItem.Portion; }
            set
            {
                this._menuItem.Portion = value;

                using (var context = new Model.Entities.RecipeDbContext())
                {
                    context.MenuItems.Update(this._menuItem);
                    context.SaveChanges();
                }

                OnPropertyChanged("Portion");
                OnPropertyChanged("Recipe");
            }
        }

        public int RecipeId
        {
            get { return this._menuItem.RecipeId; }
            set
            {
                this._menuItem.RecipeId = value;
                OnPropertyChanged("RecipeId");
            }
        }

        public int CurrentStepIndex
        {
            get { return this._menuItem.CurrentStepIndex; }
            set
            {
                this._menuItem.CurrentStepIndex = value;
                OnPropertyChanged("CurrentStepIndex");
            }
        }

        public RecipeViewModel Recipe
        {
            get { return this._recipe; }
            set
            {
                if(this.RecipeId != value.Id)
                {
                    throw new Exception("Mismatching ids");
                }
                this._recipe = value;
                OnPropertyChanged("Recipe");
            }
        }

        private ObservableCollection<int> _optionList;

        public ObservableCollection<int> OptionList
        {
            get
            {
                return this._optionList;
            }
            set
            {
                this._optionList = value;
            }
        }

           

    }
}
