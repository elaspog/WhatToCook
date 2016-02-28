using Model.Entities;
using Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToCook.ViewModels
{
    public class RecipeStepViewModel : Notifier
    {
        private RecipeStep _step;
        private MenuItem _menuItem;

        public bool CurrentStepIndexChanged { get; set; }

        public RecipeStepViewModel(RecipeStep step)
        {
            this._step = step;
        }

        public MenuItem MenuItem
        {
            get
            {
                return this._menuItem;
            }
            set
            {
                if (_step.RecipeId != value.RecipeId)
                {
                    throw new Exception("Invalid input parameters");
                }
                this._menuItem = value;
            }
        }

        public String Step
        {
            get
            {
                return this._step.Step;
            }
        }

        public int CurrentStepIndex
        {
            get
            {
                return this._menuItem.CurrentStepIndex;
            }

            set
            {
                this._menuItem.CurrentStepIndex = value;
            }
        }

        public bool Done
        {
            get 
            {
                if (this._menuItem != null)
                {
                    return this._step.Index <= this._menuItem.CurrentStepIndex;
                }
                return false;
            }
            set
            {
                CurrentStepIndexChanged = true;
                if (value == true)
                {
                    this._menuItem.CurrentStepIndex = this._step.Index;
                }
                else
                {
                    this._menuItem.CurrentStepIndex = this._step.Index-1;
                }

                OnPropertyChanged("Done");
            }
        }


    }
}
