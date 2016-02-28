using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WhatToCook.Infrastructure.Helpers
{
    public class StatusToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var Availability = (bool)value;
            var color = Availability ? new SolidColorBrush(Colors.Green) : (SolidColorBrush)Application.Current.Resources["ListViewItemOverlayBackgroundThemeBrush"]; ;
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
