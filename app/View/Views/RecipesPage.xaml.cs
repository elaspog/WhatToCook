using WhatToCook.Common;
using WhatToCook.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WhatToCook.ViewModels;
using WhatToCook.Infrastructure;
using WhatToCook.DataModel;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace WhatToCook.Views
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class RecipesPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }


        public RecipesPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            Loaded += (sender, e) => FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
            Loaded += (sender, e) => FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.DefaultViewModel["ViewModel"] = e.NavigationParameter as RecipesPageViewModel;
        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            //var itemId = ((RecipeViewModel)e.ClickedItem).Id;
            //this.Frame.Navigate(typeof(RecipeDetailsPage), itemId);

            this.Frame.Navigate(typeof(RecipeDetailsPage), (RecipeViewModel)e.ClickedItem);

        }

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
            }
            //TODO: remove this 
            new Windows.UI.Popups.MessageDialog(msg).ShowAsync();
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

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            var text = args.QueryText;
            if (text.Length > 2)
            {
                var model =  this.DefaultViewModel["ViewModel"] as RecipesPageViewModel;
                this.Frame.Navigate(typeof(RecipesPage), new RecipesPageViewModel()
                {                    
                    Title = "Találatok erre: \"" + text + "\" a(z) " + model.Title + " kategóriában",
                    Recipes = AppCache.Search(text,model.Id)
                });
            }
            else
            {
                new Windows.UI.Popups.MessageDialog("Kérjük legalább 3 karaktert adj meg!").ShowAsync();
            }
        }
    }
}