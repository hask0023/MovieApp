using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.TMDb;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace MovieApp
{
    /// <summary>
    /// Detailed movie information
    /// Authors: Steve Haskins & Nune Vardanyan
    /// </summary>
    public sealed partial class DetailsPage : Page
    {

        MovieInfo detailsPageMovieInfo;

        public DetailsPage()
        {
            this.InitializeComponent();

            detailsPageMovieInfo = new MovieInfo();
        }


        // specific movie title is sent to this page from the main page
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += backButton_Tapped;

            if (e.Parameter != null)
            {
                string movieToSearch = (string)e.Parameter;

                //call MovieInfo class to get info to populate details of selected movie
                await detailsPageMovieInfo.searchSpecificMovie(movieToSearch);
                Plot.Text = detailsPageMovieInfo.plot;
                Director.Text = detailsPageMovieInfo.movieDirector;
                Cast.Text = detailsPageMovieInfo.fullCast;
                Poster.Source = detailsPageMovieInfo.posterBitMap;
                Genre.Text = detailsPageMovieInfo.allGenres;
                RunTime.Text = detailsPageMovieInfo.runtime;
                Movie_Title.Text = detailsPageMovieInfo.title;


            }

            base.OnNavigatedTo(e);
        }

        #region commandbar controls
        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DetailsPage));
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }
        #endregion


    }
}
