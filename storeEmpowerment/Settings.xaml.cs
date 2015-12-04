using QrCodeDetector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;
using System.Globalization;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace storeEmpowerment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page,ILHMView
    {
        
        public static ProgressRing pring;
        //public static ProgressRing pgring;
        public static Settings settingptr;
        private LHMPresenter lhmPresenter = new LHMPresenter();
        public Settings()
        {

            this.InitializeComponent();
            GoogleAnalytics.EasyTracker.GetTracker().SendView("About Page");
            DrawerLayout.InitializeDrawerLayout();
            settingptr = this;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            lhmPresenter.Add(this);

        }
        

       
       
        private void imageMargin()
        {
            Double heightScreen = Window.Current.Bounds.Height;
            Thickness margin;
            margin = empowimg.Margin;
            margin.Top = heightScreen * 0.16125;
            empowimg.Margin = margin;

        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            Double widthScreen = Window.Current.Bounds.Width;
            Canvas.SetLeft(searchIcon, 0.8 * widthScreen);
            Canvas.SetLeft(scanIcon, 0.9 * widthScreen);
            Canvas.SetLeft(homeIcon, .12 * widthScreen);
            App.localSettings.Values["currentPage"] = "setting";
            pring = p_ring;
           // p_ring.IsActive = true;
            Debug.WriteLine("inside onn in settings");
            Debug.WriteLine(App.localSettings.Values["storename"].ToString().ToUpper());
            Settings.settingptr.IsEnabled = false;
            try
            {
                menuName.Text = App.localSettings.Values["storename"].ToString().ToUpper();
            }
            catch (Exception)
            {

            }
            //Windows.ApplicationModel.Package.Current.Id;
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion versionId = packageId.Version;
            Debug.WriteLine("app version" + versionId.Major);           
            Debug.WriteLine("app version"+versionId.Minor);
            Debug.WriteLine("app version" + versionId.Revision);
            Version.Text = versionId.Major + "." + versionId.Minor + "." + versionId.Revision;
            pring.IsActive = true;
            //p_ring.IsActive = true;
            Settings.settingptr.IsEnabled = false;
            imageMargin();
           // Version.Text = StoreEmpowermentModel.AppVersionResp.LatestApplicationVersion;
            Canvas.SetLeft(SettingsText, (widthScreen - SettingsText.Width) / 2);
            if (App.localSettings.Values["stores"] == null)
            {
                ResponseHandler.refreshFlag = 1;

                Debug.WriteLine("Inside Change Store");

                Frame.Navigate(typeof(StoreList));
            }
            else
            {
                ResponseHandler.refreshFlag = 11;
                NetworkManager manager = NetworkManager.Instance;
                Task<RoutineData> routineDataTask = manager.RoutineDataSettingsAsync((string)App.localSettings.Values["username"], (string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["stores"]);
                await routineDataTask;
            }         
        }
        public static async void getData()
        {
            Debug.WriteLine("inside getData");

            Debug.WriteLine("inside getData1");
            if (App.localSettings.Values["stores"] == null)
            {
                ResponseHandler.settingsFlag = 0;

                Debug.WriteLine("Inside Change Store");

                settingptr.Frame.Navigate(typeof(StoreList));
            }
            else
            {
                ResponseHandler.settingsFlag++;
                NetworkManager manager = NetworkManager.Instance;
                Task<RoutineData> routineDataTask = manager.RoutineDataSettingsAsync((string)App.localSettings.Values["username"], (string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["stores"]);
                await routineDataTask;
            }
        }


        private async void Scan_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Scan icon - Headerbar", 0);
            App.localSettings.Values["barcode"] = "false";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(MainPage)); });
        }
       

        public static void successHandlesettingsCall(RoutineData routinedata)
        {
            ResponseHandler.settingsFlag = 0;
            StoreEmpowermentModel.kpiData = routinedata;
            
            if ((StoreEmpowermentModel.kpiData.this_quarter.update_timestamp != "") && (StoreEmpowermentModel.kpiData.last_quarter.update_timestamp != ""))
            {
               // pgring.IsActive = false;
                //if(DateTime.ParseExact(StoreEmpowermentModel.kpiData.this_quarter.update_timestamp,"dd/MM/yyyy",CultureInfo.InvariantCulture)<DateTime.ParseExact(StoreEmpowermentModel.kpiData.last_quarter.update_timestamp,"dd/MM/yyyy",CultureInfo.InvariantCulture))
                if (DateTime.Parse(StoreEmpowermentModel.kpiData.this_quarter.update_timestamp) < DateTime.Parse(StoreEmpowermentModel.kpiData.last_quarter.update_timestamp))
                {
                    Settings.settingptr.Date.Text = (String)StoreEmpowermentModel.kpiData.last_quarter.update_timestamp;
                }

                else if (DateTime.Parse(StoreEmpowermentModel.kpiData.this_quarter.update_timestamp) > DateTime.Parse(StoreEmpowermentModel.kpiData.last_quarter.update_timestamp))
                {
                    Settings.settingptr.Date.Text = StoreEmpowermentModel.kpiData.this_quarter.update_timestamp;
                }
                else

                    Settings.settingptr.Date.Text = StoreEmpowermentModel.kpiData.this_quarter.update_timestamp;
            }

            else
                Settings.settingptr.Date.Text = StoreEmpowermentModel.kpiData.cronTime;

           // pgring.IsActive = false;
            Debug.WriteLine(Settings.settingptr.Date.Text + "Date@@@@@@@@@@@@");
            //Settings.settingptr.Date.Visibility = Visibility.Visible;
            pring.IsActive = false;
           // pgring.IsActive = false;
            Settings.settingptr.IsEnabled = true;
           

        }
        private void DrawerIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "SideMenu icon - Headerbar", 0);
            ListFragment.Visibility = Visibility.Visible;
            Debug.WriteLine("inside taooed");
            if (DrawerLayout.IsDrawerOpen)
            {
                DrawerLayout.CloseDrawer();
            }
            else
            {
                DrawerLayout.OpenDrawer();
                //canvas.Opacity = .5;
                

            }
            
        }

        private async void home_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Home icon - Headerbar", 0);
            App.localSettings.Values["tab"] = "recentItems";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });

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
        private void about(object sender, RoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();

            lhmPresenter.about();

        }
        private void teamAccess(object sender, TappedRoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            lhmPresenter.teamAccess();

        }
        private void complainceAction(object sender, RoutedEventArgs e)
        {

            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            if (pring.IsActive == false)
            {
                lhmPresenter.complianceRoutine();
            }

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
        private void Back(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(storeRoutine));
        }

        public async void NavigateToLoginPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(LoginPage)); }); 
        }

        public async void NavigateToSelectStorePage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(StoreList)); }); ;
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
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(storeRoutine)); }); ;
        }

        public async void NavigateToTeamAccessPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(TeamAccess)); });
        }
        private async void searchIconClick(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Search icon - Headerbar", 0);
            App.localSettings.Values["tab"] = "recentItems";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
        }
        public async void NavigateToTeamPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(TeamAccess)); });
        }

        private async void complainceAction(object sender, TappedRoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(storeRoutine)); });
        }



        public static async void failureHandleForKpi(string p)
        {
            Settings.settingptr.Date.Text = " ";
            popupBox popup = new popupBox(p);
            await popup.ShowDialog();

        }

        
    }
}
