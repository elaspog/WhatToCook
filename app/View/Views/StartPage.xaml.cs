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

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace WhatToCook.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        //private NavigationParameter navParam = null;

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

        public StartPage()
        {
            this.InitializeComponent(); 
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
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
            this.DefaultViewModel["CategoryListViewModel"] = WhatToCook.DataModel.AppCache.GetCategories();
        }

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var group = (sender as FrameworkElement).DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            this.Frame.Navigate(typeof(RecipesPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(RecipeDetailsPage), itemId);
        }

        void CategoryView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var category = e.ClickedItem as CategoryViewModel;
            this.Frame.Navigate(typeof(RecipesPage), new RecipesPageViewModel() 
            { 
                Title = category.Name,
                TitleForegroundColor = category.BackgroundColor,
                Recipes = category.RelatedRecipes,
                Id=category.Id.ToString()
            });
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
            switch(button.Label)
            {
                case "Menü":
                    this.Frame.Navigate(typeof(FoodMenuPage));
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

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            var text = args.QueryText;
            if (text.Length > 2)
            {
                this.Frame.Navigate(typeof(RecipesPage), new RecipesPageViewModel()
                {
                    Title = "Találatok erre: \"" + text + "\"",
                    Recipes = AppCache.Search(text)
                });
            }
            else
            {
                new Windows.UI.Popups.MessageDialog("Kérjük legalább 3 karaktert adj meg!").ShowAsync();
            }
        }
    }
}