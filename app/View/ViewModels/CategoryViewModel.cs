using Model.Entities;
using Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToCook.DataModel;
using WhatToCook.Infrastructure;
using Windows.UI;

namespace WhatToCook.ViewModels
{
    public class CategoryViewModel : Notifier
    {
        private Category _category;
        private RecipeListViewModel _relatedRecipes;


        public CategoryViewModel()
        {
            this._category = new Category();
            this.RelatedRecipes = new RecipeListViewModel();
        }
        public CategoryViewModel(Category category)
        {
            this._category = category;
            this.RelatedRecipes = new RecipeListViewModel();
        }
        public int Id
        {
            get { return this._category.Id; }
            set
            {
                this._category.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return this._category.Name; }
            set
            {
                this._category.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get { return this._category.Description; }
            set
            {
                this._category.Description = value;
                OnPropertyChanged("Description");
            }
        }
        public string IconPath
        {
            get { return this._category.IconPath; }
            set
            {
                this._category.IconPath = value;
                OnPropertyChanged("IconPath");
            }
        }
        public string BackgroundColor
        {
            get { return this._category.BackgroundColor; }
            set
            {
                this._category.BackgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }
        public RecipeListViewModel RelatedRecipes
        {
            get { return this._relatedRecipes; }
            set
            {
                this._relatedRecipes = value;
                OnPropertyChanged("RelatedRecipes");
            }
        }
    }
}
