using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieApp
{
    /// <summary>
    /// Page showing information about authors
    /// Authors: Steve Haskins & Nune Vardanyan
    /// </summary>
    public sealed partial class About : Page
    {
        public About()
        {
            this.InitializeComponent();
            Search_Icon.Background = new SolidColorBrush();
            About_Icon.Background = new SolidColorBrush(Colors.Gray);
        }
        #region navigation & state management
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += backButton_Tapped;


            base.OnNavigatedTo(e);
        }

        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            //issue with the hardware back button and go back function
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }

        }
        #endregion

        #region commandbar navigation
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }


        private void About_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }
        #endregion

        #region e-mail launcher methods
        private async void Nune_Email_Button_Click(object sender, RoutedEventArgs e)
        {
            Uri email = new Uri("mailto:vard0020@algonquinlive.com");
            await Windows.System.Launcher.LaunchUriAsync(email);
        }

        private async void Steve_Email_Button_Click(object sender, RoutedEventArgs e)
        {
            Uri email = new Uri("mailto:hask0023@algonquinlive.com");
            await Windows.System.Launcher.LaunchUriAsync(email);
        }
        #endregion
    }
}
