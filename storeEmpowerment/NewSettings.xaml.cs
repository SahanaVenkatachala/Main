using QrCodeDetector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace storeEmpowerment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewSettings : Page, ILHMView
    {
        private LHMPresenter lhmPresenter = new LHMPresenter();
        public static ProgressRing pring;
        public NewSettings()
        {
            this.InitializeComponent();
            GoogleAnalytics.EasyTracker.GetTracker().SendView("Settings Page");
            DrawerLayout.InitializeDrawerLayout();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            lhmPresenter.Add(this);
        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Double widthScreen = Window.Current.Bounds.Width;
            Canvas.SetLeft(searchIcon, 0.8 * widthScreen);
            Canvas.SetLeft(homeIcon, .12 * widthScreen);
            Canvas.SetLeft(scanIcon, 0.9 * widthScreen);
            App.localSettings.Values["currentPage"] = "setting";
            pring = p_ring;
            
            try
            {
                menuName.Text = App.localSettings.Values["storename"].ToString().ToUpper();
            }
            catch (Exception)
            {

            }
            standardcanvashorizontal.Width = widthScreen;
            casesandsingleshorizontal.Width = widthScreen;
            friendlycanvashorizontal.Width = widthScreen;
            singleshorizontal.Width = widthScreen;

            Canvas.SetLeft(tick1, 0.80 * widthScreen);
            Canvas.SetLeft(tick2, 0.80 * widthScreen);
            Canvas.SetLeft(tick3, 0.80 * widthScreen);
            Canvas.SetLeft(tick4, 0.80 * widthScreen);
            Canvas.SetLeft(SettingsText, (widthScreen - SettingsText.Width) / 2);
            if ((string)App.localSettings.Values["mode"] == "cs")
            {
                tick1.Visibility = Visibility.Collapsed;
                tick2.Visibility = Visibility.Visible;
            }
            else
            {
                tick1.Visibility = Visibility.Visible;
                tick2.Visibility = Visibility.Collapsed;
            }

            if ((string)App.localSettings.Values["locationName"] == "friendly")
            {
                tick4.Visibility = Visibility.Collapsed;
                tick3.Visibility = Visibility.Visible;
            }
            else
            {
                tick4.Visibility = Visibility.Visible;
                tick3.Visibility = Visibility.Collapsed;
            }

            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        private void singlesTapped(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("CheckBox", "Singles selected", null, 0);
            App.localSettings.Values["mode"] = "s";
            tick1.Visibility = Visibility.Visible;
            tick2.Visibility = Visibility.Collapsed;

        }

        private void casesandsinglesTapped(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("CheckBox", "casesandsingles selected", null, 0);
            Debug.WriteLine("went inside cases and singles :" + App.localSettings.Values["mode"]);
            App.localSettings.Values["mode"] = "cs";
            tick1.Visibility = Visibility.Collapsed;
            tick2.Visibility = Visibility.Visible;
        }
        private void friendlyTapped(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("CheckBox", "friendly selected", null, 0);
          
            App.localSettings.Values["locationName"] = "friendly";
            tick3.Visibility = Visibility.Visible;
            tick4.Visibility = Visibility.Collapsed;
        }
        private void standardTapped(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("CheckBox", "Standard selected", null, 0);
          
            App.localSettings.Values["locationName"] = "standard";
            tick3.Visibility = Visibility.Collapsed;
            tick4.Visibility = Visibility.Visible;
        }

        private void DrawerIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "SideMenu icon - Headerbar", 0);

            Debug.WriteLine("inside taooed");
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            else
                DrawerLayout.OpenDrawer();
        }

        private async void Scan_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Scan icon - Headerbar", 0);

            App.localSettings.Values["barcode"] = "false";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(MainPage)); });
        }

        private async void searchIconClick(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Search icon - Headerbar", 0);
            App.localSettings.Values["tab"] = "recentItems";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
        }


        public async void NavigateToLoginPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(LoginPage)); });
        }

        public async void NavigateToSelectStorePage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(StoreList)); });
        }

        public async void NavigateToSettingsPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(NewSettings)); });
        }

        public async void NavigateToAboutPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(Settings)); });
        }
        
        public async void NavigateToComplianceRoutinePage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(storeRoutine)); });
        }
        public async void NavigateToTeamAccessPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(TeamAccess)); });
        }
        private void logoutAction(object sender, RoutedEventArgs e)
        {

            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            if (pring.IsActive == false)
            {
                lhmPresenter.Logout();
            }
        }
        private void complainceAction(object sender, RoutedEventArgs e)
        {

            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();

            lhmPresenter.complianceRoutine();

        }
        private void about(object sender, RoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();

            lhmPresenter.about();

        }
        private void changeStore(object sender, RoutedEventArgs e)
        {
            //changeCall();
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            if (pring.IsActive == false)
            {
                lhmPresenter.ChangeStore();
            }
        }

        private void settings(object sender, RoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            if (pring.IsActive == false)
            {
                lhmPresenter.settings();
            }
        }

       private void teamAccess(object sender, TappedRoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();

               lhmPresenter.teamAccess();
            

        }
        private async void home_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Home icon - Headerbar", 0);


            App.localSettings.Values["tab"] = "recentItems";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });

        }

       

        
    }
}
