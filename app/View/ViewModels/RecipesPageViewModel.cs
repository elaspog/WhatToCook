using Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToCook.ViewModels
{
    class RecipesPageViewModel : Notifier
    {
        private string _title;
        private string _titleForegroundColor;
        private RecipeListViewModel _recipes;
        private string _id;

        public RecipesPageViewModel()
        {
            this.Recipes = new RecipeListViewModel();
        }

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                this._id = value;
            }
        }

        public string Title
        {
            get { return this._title; }
            set
            {
                this._title = value;
                OnPropertyChanged("Title");
            }
        }

        public string TitleForegroundColor
        {
            get { return this._titleForegroundColor; }
            set
            {
                this._titleForegroundColor = value;
                OnPropertyChanged("TitleForegroundColor");
            }
        }

        public RecipeListViewModel Recipes
        {
            get { return this._recipes; }
            set
            {
                this._recipes = value;
                OnPropertyChanged("Recipes");
            }
        }
    }
}
