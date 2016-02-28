using Model.Infrastructure;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToCook.ViewModels
{
    public class RecipeIngredientViewModel : Notifier
    {
        private Ingredient _ingredient;
        private RecipeIngredientRelation _relation;

        public RecipeIngredientViewModel()
        {
            this._ingredient = new Ingredient();
            this._relation = new RecipeIngredientRelation();
        }
        public RecipeIngredientViewModel(Ingredient ingredient, RecipeIngredientRelation relation)
        {
            if (ingredient.Id != relation.IngredientId)
            {
                throw new Exception("Invalid input parameters");
            }
            this._ingredient = ingredient;
            this._relation = relation;
        }

        public RecipeIngredientViewModel(RecipeIngredientViewModel other, int portion)
        {
            this._ingredient = other._ingredient;
            this._relation = new RecipeIngredientRelation {IngredientId = other.IngredientId, Quantity = other.Quantity *portion, Unit = other.Unit };
        }

        public int Id
        {
            get { return this._relation.Id; }
            set
            {
                this._relation.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public int IngredientId
        {
            get { return this._ingredient.Id; }
            set
            {
                this._ingredient.Id = value;
                this._relation.IngredientId = value;
                OnPropertyChanged("IngredientId");
            }
        }
        public int RecipeId
        {
            get { return this._relation.RecipeId; }
            set
            {
                this._relation.RecipeId = value;
                this._relation.RecipeId = value;
                OnPropertyChanged("RecipeId");
            }
        }
        public string Name
        {
            get { return this._ingredient.Name; }
            set
            {
                this._ingredient.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public double Quantity
        {
            get { return this._relation.Quantity; }
            set
            {
                this._relation.Quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        public string Unit
        {
            get { return this._relation.Unit; }
            set
            {
                this._relation.Unit = value;
                OnPropertyChanged("Unit");
            }
        }
        public int Index
        {
            get { return this._relation.Index; }
            set
            {
                this._relation.Index = value;
                OnPropertyChanged("Index");
            }
        }

        private bool _hasBought;

        public bool HasBought
        {
            get
            {
                return _hasBought;
            }

            set
            {
                _hasBought = value;
                OnPropertyChanged("HasBought");
            }
        }

        public override string ToString()
        {
            if (this.Quantity > 0 && !String.IsNullOrEmpty(this.Unit))
            {
                return String.Format("{0} {1} {2}",
                    this.Quantity == 0.5 ? "fél" : this.Quantity.ToString(),
                    this.Unit,
                    this.Name);
            }
            else
            {
                return this.Name;
            }

        }
    }
}
