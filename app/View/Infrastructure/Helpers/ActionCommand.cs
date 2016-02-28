using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WhatToCook.Infrastructure.Helpers
{
    public class ActionCommand<TParam> : ICommand
    {
        Action<TParam> action;
        public ActionCommand(Action<TParam> action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action((TParam)parameter);
        }
    }
}
