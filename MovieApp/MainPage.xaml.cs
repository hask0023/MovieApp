using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.TMDb;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace MovieApp
{
    /// <summary>
    /// Main page where search bar is located.
    /// Authors: Steve Haskins & Nune Vardanyan
    /// </summary>
    public sealed partial class MainPage : Page
    {
       
        MovieInfo mainPageMovieInfo;
       
        public MainPage()
        {
            this.InitializeComponent();

            mainPageMovieInfo = new MovieInfo();          
            Search_Icon.Background = new SolidColorBrush(Colors.Gray);
            About_Icon.Background = new SolidColorBrush();

        }



        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
           
        }

       
        #region navigation & saving state
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            currentView.BackRequested -= backButton_Tapped;

            //saving application data whenever the uer leaves the search for movie page
            var composite = new ApplicationDataCompositeValue();
            composite["movieSearch"] = SearchBox.Text;
            ApplicationData.Current.LocalSettings.Values["mainPage data"] = composite;

            base.OnNavigatedTo(e);
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //load
            //var composite = new ApplicationDataCompositeValue();
            //composite["movieSearch"] = SearchBox.Text;

            //ApplicationData.Current.LocalSettings.Values["mainPage data"] = composite;

            var composite = ApplicationData.Current.LocalSettings.Values["mainPage data"] as ApplicationDataCompositeValue;

            if (composite != null)
            {
                SearchBox.Text = composite["movieSearch"].ToString();
                // Checks whether the entire result is null OR
                // contains no resulting records.
            }

            base.OnNavigatedTo(e);
        }
        #endregion

        #region commandbar controls
        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }
      

        private void About_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }
        #endregion

        // method for getting autosuggestbox items source
        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (SearchBox.Text.Length >= 1)
            {
                await mainPageMovieInfo.SearchAllMovies(SearchBox.Text);
                sender.ItemsSource = mainPageMovieInfo.asbList;
                OptionsBox.ItemsSource = mainPageMovieInfo.asbList;
            }            
        }

        // choosing one of the movie options takes you to detailed page for that movie
        private void OptionsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedMovie = OptionsBox.SelectedItem.ToString();

            Frame.Navigate(typeof(DetailsPage), selectedMovie);
        }

       
    }
}
