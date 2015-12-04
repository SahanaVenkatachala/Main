using QrCodeDetector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    /// 

    public sealed partial class StoreList : Page, IStoreList, ILHMView
    {
        private LHMPresenter lhmPresenter = new LHMPresenter();
        public static ProgressRing pring;
        private StoreListPresenter storePresenter = new StoreListPresenter();
        private static StoreList storeListPointer;
        public static int storeFlag = 0;
        public static string[] menuItems;
        public static string[] storName;
        private List<storeDetails> searchList;
        public StoreList()
        {
            
            this.NavigationCacheMode = NavigationCacheMode.Required;
            lhmPresenter.Add(this);
            this.InitializeComponent();
            GoogleAnalytics.EasyTracker.GetTracker().SendView("StoreList Page");

            DrawerLayout.InitializeDrawerLayout();
            storeListPointer = this;
            storePresenter.Add(this);
        }
        public StoreList(int n)
        {

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
        private async void complainceAction(object sender, RoutedEventArgs e)
        {

            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            if (App.localSettings.Values["stores"] == null)
            {
                DrawerLayout.CloseDrawer();
                popupBox popup = new popupBox("Please Select Store First");
                await popup.ShowDialog();
                
            }
            else if (pring.IsActive == false )
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

        public void SearchArrange()
        {
            var width = Window.Current.Bounds.Width;
            searchText.Width = width - 20;
            Canvas.SetLeft(searchText, 10);
            Canvas.SetLeft(myOrders1, (width - myOrders1.Width) / 2);
            //Canvas.SetLeft(empowImage2, (width - myOrders1.Width - empowImage2.Width) / 2);
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /// 

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(App.localSettings.Values["storename"]==null)
            {
                searchIcon.Visibility = Visibility.Collapsed;
                scanIcon.Visibility = Visibility.Collapsed;
                ic_DrawerImage.Visibility = Visibility.Collapsed;
                homeIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                searchIcon.Visibility = Visibility.Visible;
                scanIcon.Visibility = Visibility.Visible;
                ic_DrawerImage.Visibility = Visibility.Visible;
                homeIcon.Visibility = Visibility.Visible;
            }
            searchText.Text="";
            Double widthScreen = Window.Current.Bounds.Width;
            Canvas.SetLeft(searchIcon, 0.8 * widthScreen);
            Canvas.SetLeft(homeIcon, .12 * widthScreen);
            Canvas.SetLeft(scanIcon, 0.9 * widthScreen);
            App.localSettings.Values["currentPage"] = "store";
            try
            {
                menuName.Text = App.localSettings.Values["storename"].ToString().ToUpper();
            }
            catch (Exception)
            {

            }
            pring = p_ring;
           
            SearchArrange();
            if (StoreEmpowermentModel.stores.Count < 2)
            {
                Debug.WriteLine("675273649832*********");
                storePresenter.FetchStoresFromFile();

            }
            else
            {

                SetListSource(StoreEmpowermentModel.stores);
            }
            storeFlag = 1;
            Debug.WriteLine(StoreEmpowermentModel.stores.Count);
            //storeList.ItemsSource = StoreEmpowermentModel.stores;

        }

      /*  private void logoutAction(object sender, RoutedEventArgs e)
        {
            if (messageStore.Visibility == Visibility.Collapsed)
            {
                storeFlag = 0;
                App.localSettings.Values["refresh"] = null;
                App.localSettings.Values["token"] = null;
                App.localSettings.Values["expires"] = null;
                Frame.Navigate(typeof(LoginPage));
            }
        }*/

        private async void Scan_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Scan icon - Headerbar", 0);
            App.localSettings.Values["barcode"] = "false";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(MainPage)); });
        }
        private async void listView_SelectionChanged(object sender, RoutedEventArgs e)
        {
           
            if (storeList.SelectedIndex != -1)
            {
                App.localSettings.Values["tab"] = "recentItems";
                    if (searchList == null)
                    {
                        App.localSettings.Values["stores"] = StoreEmpowermentModel.stores[storeList.SelectedIndex].storeNumber;
                        App.localSettings.Values["storename"] = StoreEmpowermentModel.stores[storeList.SelectedIndex].storeName;
                        message.Text = "You have successfully\nchanged to store " + StoreEmpowermentModel.stores[storeList.SelectedIndex].storeNumber + "\n" + StoreEmpowermentModel.stores[storeList.SelectedIndex].storeName;
                    }
                    else
                    {
                        App.localSettings.Values["stores"] = searchList[storeList.SelectedIndex].storeNumber;
                        App.localSettings.Values["storename"] = searchList[storeList.SelectedIndex].storeName;
                        message.Text = "You have successfully\nchanged to store " + searchList[storeList.SelectedIndex].storeNumber + "\n" + searchList[storeList.SelectedIndex].storeName;
                    }
                    messageStore.Visibility = Visibility.Visible;
                    await putDelay();
                    messageStore.Visibility = Visibility.Collapsed;
                    storeFlag = 0;
                    Frame.Navigate(typeof(productSearch));
                
            }
            
        }
        private void searchListener(object sender, TextChangedEventArgs e)
        {
            searchList = new List<storeDetails>();
            storePresenter.StoreSearch(searchList, searchText.Text);
            storeList.ItemsSource = searchList;
        }


        public void SetListSource(List<storeDetails> stores)
        {
            stores.Sort();
            storeList.ItemsSource = stores;
            
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

        private void storeList_SelectionChanged(object sender, RoutedEventArgs e)
        {
           // GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "tap", "select store tap", 0);
            Debug.WriteLine("went insideeeeeeeeeeeeeeeee");
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
        async Task putDelay()
        {
            await Task.Delay(2000);
        }


        public async void NavigateToTeamPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(TeamAccess)); });
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
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(storeRoutine)); }); 
        }
        public async void NavigateToTeamAccessPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(TeamAccess)); });
        }
        public void SetSearchByNameListSource(List<SearchByNameResponse> searchresponse)
        {
            throw new NotImplementedException();
        }

        private async void home_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Home icon - Headerbar", 0);
            App.localSettings.Values["tab"] = "recentItems";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });

        }
        public async void NavigateToHomePage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); }); 
        }

        private async void searchIconClick(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Search icon - Headerbar", 0);
            App.localSettings.Values["tab"] = "recentItems";
            App.localSettings.Values["searchCheck"] = "active";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
        }

        
    }
}
