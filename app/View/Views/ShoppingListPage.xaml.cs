using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WhatToCook.Common;
using WhatToCook.DataModel;
using WhatToCook.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhatToCook.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShoppingListPage : Page
    {
        public ShoppingListPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            Loaded += (sender, e) => FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
        }

          private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var menuList = WhatToCook.DataModel.AppCache.GetMenuList();
            this.DefaultViewModel["Menu"] = menuList;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarButton;
            var msg = "TODO: open " + button.Label + " page";
            switch (button.Label)
            {
                case "Menü":
                    this.Frame.Navigate(typeof(FoodMenuPage));
                    return;
                case "Kezdőoldal":
                    this.Frame.Navigate(typeof(StartPage));
                    return;
                case "Kedvencek":
                    this.Frame.Navigate(typeof(RecipesPage), new RecipesPageViewModel()
                    {
                        Title = "Kedvencek",
                        Recipes = AppCache.GetFavourites()
                    });
                    return;
                case "Mi van itthon?":
                    //TODO: 
                    break;
                case "Menü ürítése":
                    var menuList = this.DefaultViewModel["Menu"] as MenuListViewModel;
                    menuList.Clear();
                    return;
            }
            //TODO: remove this 
            new Windows.UI.Popups.MessageDialog(msg).ShowAsync();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            var menuList = this.DefaultViewModel["Menu"] as MenuListViewModel;
            foreach (var item in menuList) 
            {
                item.Recipe.SetIsInMenu(false);
            }
            menuList.Clear();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

      
    }
}
