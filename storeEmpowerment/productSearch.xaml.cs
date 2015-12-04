using QrCodeDetector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace storeEmpowerment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class productSearch : Page, ILHMView, ISearchPage, ISearchByNameList
    {
        public static int tabSelected = 0;
        public static productSearch homePointer;
        private LHMPresenter lhmPresenter = new LHMPresenter();
        public static SearchProductbyNamePresenter searchbynamepresenter = new SearchProductbyNamePresenter();
        public static SearchPresenter presenter = new SearchPresenter();
        public static ProgressRing pring;
       // public static productSearch homePointer;
        Windows.UI.ViewManagement.StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

        public productSearch()
        {
            this.InitializeComponent();
            GoogleAnalytics.EasyTracker.GetTracker().SendView("productSearch Page");
            DrawerLayout.InitializeDrawerLayout();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            lhmPresenter.Add(this);
            presenter.add(this);
            pring = this.p_ring;
            homePointer = this;
        }


        /*private void SetSearchSource()
        {
            for (int i = 0; i < StoreEmpowermentModel.productNames.Count; i++)
            {
                if (StoreEmpowermentModel.productNames[i].ImageURL.Length < 2)
                {
                    ListViewItem item = new ListViewItem();
                    item.Content = StoreEmpowermentModel.productNames[i].Description;
                    item.Background = new SolidColorBrush(Colo)
                }
                else
                {
                    storeList.Items[i] = StoreEmpowermentModel.productNames[i];
                }
            }
        }*/

        public async void NavigateToOverview()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(ProductOverview)); });
        }

        public void ShowRing()
        {
            p_ring.IsActive = true;
        }
        public void HideRing()
        {
            p_ring.IsActive = false;
        }

        private void uiArrange()
        {

            var widthScreen = Window.Current.Bounds.Width;
            RecentItems.Foreground = new SolidColorBrush(Colors.White);
            horizontal.Width = widthScreen;
            searchResults.Foreground = new SolidColorBrush(Colors.LightGray);
            /*Searchabove.Width = widthScreen / 2;
           // Searchabove.Height = myCanvas.Height;
            Recentabove.Width = widthScreen / 2;
            //Recentabove.Height = myCanvas.Height;
            Canvas.SetLeft(Searchabove, widthScreen / 2);*/
            color.Width = widthScreen / 2;
            colorScan.Width = widthScreen / 2;
            //Search.Width = widthScreen / 2;
            //Search.TextAlignment = TextAlignment.Center;
            color.Background = new SolidColorBrush(Colors.CornflowerBlue);
            colorScan.Background = new SolidColorBrush(Colors.Transparent);
            Canvas.SetLeft(colorScan, 0.5 * widthScreen);
            //Canvas.SetLeft(Search, 0.25 * widthScreen);
            Canvas.SetLeft(searchImage, 0.14 * widthScreen);
            Canvas.SetLeft(Scan, 0.55 * widthScreen); //0.75
            Canvas.SetLeft(scanImage, 0.65 * widthScreen);
            //Canvas.SetLeft(RecentItems, 0.15 * widthScreen);
            Canvas.SetLeft(searchResults, 0.60 * widthScreen);
            Search.Width = widthScreen / 2;
            Scan.Width = widthScreen / 2;
            RecentItems.Width = widthScreen / 2;
            searchResults.Width = widthScreen / 2;
            //RecentItems.Height = myCanvas.Height;
            //searchResults.Height = myCanvas.Height;
            Canvas.SetLeft(myOrders1, (widthScreen - myOrders1.Width) / 2);

        }

        private void changeButtonMargins(Thickness margin, TextBlock button)
        {
            Double widthScreen = Window.Current.Bounds.Width;
            if (button == Search)
                margin.Left = 0;
            else
                margin.Left = widthScreen * 0.5;
            margin.Top = 50;
            button.Margin = margin;
        }

        private void changecanvasmargin(Thickness margin, Canvas canvas)
        {
            Double widthScreen = Window.Current.Bounds.Width;
            margin.Left = widthScreen / 2;
            margin.Right = widthScreen / 2;

            canvas.Margin = margin;

        }

        private void Align(Thickness margin, TextBlock textblock)
        {
            Double widthScreen = Window.Current.Bounds.Width;
            margin.Left = (widthScreen - textblock.Width) / 2;
            textblock.Margin = margin;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            //GoogleAnalytics.EasyTracker.GetTracker().SendView("Home Page");
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            var width = Window.Current.Bounds.Width;
            myCanvas.Height = 0.25 * width;
            //vertical.Height = 0.9 * myCanvas.Height;
            changecanvasmargin(vertical.Margin, vertical);
            vertical.Visibility = Visibility.Visible;


            // Search.Height = 0.125 * width;

            //searchImage.Height = 0.125 * width;
            // Scan.Height = 0.125 * width;
            //scanImage.Height = 0.125 * width;
            //backArrow.Width = width - searchText.Width;
            Canvas.SetLeft(backArrow, 0.90 * width);
            nosearch.Visibility = Visibility.Collapsed;
            // AlignProgressRing(pring.Margin, pring);
            myCanvas.Visibility = Visibility.Visible;
            canvas1.Visibility = Visibility.Collapsed;


            Debug.WriteLine(width);
            if (width > 410)
            {
                // canvas2.Width = 0.2 * width;
                // cancel.Width = 0.2 * width;
                // Canvas.SetLeft(backArrow, 10);
            }
            searchText.Width = width * 0.85;
            // Canvas.SetLeft()
            //canvas1.Width = 0.20 * width;
            //cancel.Width = canvas1.Width;
            //Canvas.SetLeft(searchText, 0.025 * width);
            Canvas.SetLeft(SearchIcon, (0.85 * width) + 8 - (0.065 * width));
            //  Canvas.SetLeft(backArrow, 0.75 * width);
            // Canvas.SetLeft(canvas2, 0.79 * width);

            try
            {
                menuName.Text = App.localSettings.Values["storename"].ToString().ToUpper();
            }
            catch (Exception)
            {

            }
            Align(norecent.Margin, norecent);
            Align(nosearch.Margin, nosearch);
            horizontal.Visibility = Visibility.Visible;
            uiArrange();
            Debug.WriteLine("tabselected : " + tabSelected);
            if (tabSelected == 0)
            {
                SelectRecent();
                RenderRecent();
            }
            else if (tabSelected == 1)
            {
                SelectSearch();
                RenderSearch();
            }
            App.localSettings.Values["currentPage"] = "psearch";

            try
            {
                menuName.Text = App.localSettings.Values["storename"].ToString().ToUpper();
            }
            catch (Exception)
            {

            }

            try
            {
                if (App.localSettings.Values["barcode"].ToString() == "found")
                {
                    App.localSettings.Values["barcode"] = "false";
                    NetworkManager manager = NetworkManager.Instance;

                    ShowRing();


                    Task GetProductTask = manager.GetProductAsync(App.localSettings.Values["bar_value"].ToString());

                    await GetProductTask;

                }
            }
            catch (Exception)
            {

            }
            try
            {
                Debug.WriteLine(App.localSettings.Values["searchCheck"]);
                if ((string)App.localSettings.Values["searchCheck"] == "active")
                {
                    Debug.WriteLine("went inside active");
                    ShowSearch();
                    searchText.Text = "";
                    //searchText.is
                    searchText.Focus(Windows.UI.Xaml.FocusState.Keyboard);
                    App.localSettings.Values["searchCheck"] = "inactive";
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if(App.localSettings.Values["tab"].Equals("recentItems"))
                {

                    SelectRecent();
                    RenderRecent();
                    
                }
                if (App.localSettings.Values["tab"].Equals("searchResult"))
                {
                    SelectSearch();
                    RenderSearch();

                }
            }
            catch(Exception ex)
            {

            }
        }

        private void RenderRecent()
        {
            if (StoreEmpowermentModel.recentItems.Count == 0)
            {
                try
                {
                    recentList.Items.RemoveAt(0);
                }
                catch (Exception)
                {

                }
                Debug.WriteLine("went inside to checkn search");
                nosearch.Visibility = Visibility.Collapsed;
                norecent.Visibility = Visibility.Visible;
            }
            else
            {
                nosearch.Visibility = Visibility.Collapsed;
                norecent.Visibility = Visibility.Collapsed;
                Debug.WriteLine("went inside to check search and scan" + StoreEmpowermentModel.recentItems.Count);
                recentList.ItemsSource = ((string[])App.localSettings.Values["recent"]).ToList();

                recentList.UpdateLayout();

            }
        }

        private void RenderSearch()
        {


            //SelectSearch();
            if (StoreEmpowermentModel.productNames.Count == 0)
            {
                //StoreEmpowermentModel.recentItems.RemoveAt(0);
                Debug.WriteLine("went inside to checkn search");
                nosearch.Visibility = Visibility.Visible;
                norecent.Visibility = Visibility.Collapsed;
            }
            else
            {

                storeList.ItemsSource = null;
                nosearch.Visibility = Visibility.Collapsed;
                norecent.Visibility = Visibility.Collapsed;
                Debug.WriteLine("went inside to check search and scan" + StoreEmpowermentModel.productNames.Count + StoreEmpowermentModel.productNames[0].Description + StoreEmpowermentModel.productNames[1].Description);

                storeList.Visibility = Visibility.Visible;
                //storeList.ItemsSource = null;
                //SelectSearch();
                storeList.ItemsSource = StoreEmpowermentModel.productNames;


            }
        }


        public static async void getData(String productId)
        {
            Debug.WriteLine("inside getData");

            Debug.WriteLine("inside getData1");
            // pring.IsActive = true;
            //storeRoutine.storePointer.IsEnabled = false;
            if (App.localSettings.Values["stores"] == null)
            {
                ResponseHandler.searchFlag = 0;

                Debug.WriteLine("Inside Change Store");

                homePointer.Frame.Navigate(typeof(StoreList));
            }
            else
            {
                ResponseHandler.searchFlag++;
                NetworkManager manager = NetworkManager.Instance;
                 Task GetProductTask = manager.GetProductAsync(productId);
                 await GetProductTask;
            }

            homePointer.Frame.Navigate(typeof(productSearch));
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

        private void logoutAction(object sender, RoutedEventArgs e)
        {

            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            if (p_ring.IsActive == false)
            {
                lhmPresenter.Logout();
            }
        }
        private void complainceAction(object sender, RoutedEventArgs e)
        {

            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            if (p_ring.IsActive == false)
            {
                lhmPresenter.complianceRoutine();
            }

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
            if (p_ring.IsActive == false)
            {
                lhmPresenter.ChangeStore();
            }
        }

       private void teamAccess(object sender, TappedRoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            lhmPresenter.teamAccess();

        }


        private void settings(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("went to settings");
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            if (p_ring.IsActive == false)
            {
                lhmPresenter.settings();
            }
        }
        private void SelectRecent()
        {
            storeList.Visibility = Visibility.Collapsed;
            tabSelected = 0;
            if (StoreEmpowermentModel.recentItems.Count == 0)
            {
                norecent.Visibility = Visibility.Visible;
            }
            else
            {
                recentList.Visibility = Visibility.Visible;
                //recentList.ItemsSource = ((string[])App.localSettings.Values["recent"]).ToList();
            }
            nosearch.Visibility = Visibility.Collapsed;
            RecentItems.Foreground = new SolidColorBrush(Colors.White);
            searchResults.Foreground = new SolidColorBrush(Colors.LightGray);
            color.Background = new SolidColorBrush(Colors.CornflowerBlue);
            colorScan.Background = new SolidColorBrush(Colors.Transparent);
        }
        private void RecentItemsAction(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Tab", "Click", "Recent Items", 0);
            App.localSettings.Values["tab"] = "recentItems";
            SelectRecent();
            RenderRecent();
        }

        private void SelectSearch()
        {
            //storeList.Visibility = Visibility.Visible;
            recentList.Visibility = Visibility.Collapsed;
            tabSelected = 0;
            if (StoreEmpowermentModel.productsearchResult.Count == 0)
            {
                nosearch.Visibility = Visibility.Visible;
            }
            else
            {
                nosearch.Visibility = Visibility.Collapsed;
                storeList.Visibility = Visibility.Visible;
                //searchbynamepresenter.populateResult();
            }

            norecent.Visibility = Visibility.Collapsed;
            nosearch.Visibility = Visibility.Visible;
            RecentItems.Foreground = new SolidColorBrush(Colors.LightGray);
            searchResults.Foreground = new SolidColorBrush(Colors.White);
            color.Background = new SolidColorBrush(Colors.Transparent);
            colorScan.Background = new SolidColorBrush(Colors.CornflowerBlue);
        }

        private void SearchResultAction(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Tab", "Click", "Search Result", 0);
            App.localSettings.Values["tab"] = "searchResult";
            SelectSearch();
            RenderSearch();
            //tabSelected = 0;
        }


        public async void NavigateToHomePage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
        }
        public async void NavigateToLoginPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(LoginPage)); }); ;
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
        private void Search_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Button", "Click", "Search Button", 0);
                //NavigateToHomePage();         
               App.localSettings.Values["tab"] = "searchResult";
               ShowSearch();
        }
        public void checkState()
        {
            if(searchText.Text=="")
            {
                NavigateToHomePage();
            }
            else
            {
               
            }
        }

        private void ShowSearch()
        {
            searchText.Text = "";
            searchText.Focus(Windows.UI.Xaml.FocusState.Keyboard);

            vertical.Visibility = Visibility.Collapsed;
            //searchText.Focus();
            // horizontal.Visibility = Visibility.Collapsed;
            // await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(searchPage)); });
            myCanvas.Visibility = Visibility.Collapsed;
            canvas1.Visibility = Visibility.Visible;
        }
        private void clearText(object sender, TappedRoutedEventArgs e)
        {
            searchText.Text = "";
        }
        private void cancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Button", "Click", "Cancel button", 0);
            // await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
            canvas1.Visibility = Visibility.Collapsed;
            vertical.Visibility = Visibility.Visible;
            horizontal.Visibility = Visibility.Visible;
            myCanvas.Visibility = Visibility.Visible;
        }

        private async void Scan_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Button", "Click", "Barcode Scan", 0);
            App.localSettings.Values["barcode"] = "false";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(MainPage)); });
        }
        private async void searchText_KeyDown(object sender, KeyRoutedEventArgs e)
        {          
            if (e.Key == VirtualKey.Enter)
            {

                //AlignProgressRing(pring.Margin, pring);
                ShowRing();
                double output;
                NetworkManager manager = NetworkManager.Instance;

                if (Double.TryParse(searchText.Text, out output))
                {
                    Debug.WriteLine("nos");
                    App.localSettings.Values["tab"] = "recentItems";
                    Task GetProductTask = manager.GetProductAsync(searchText.Text);
                    await GetProductTask;
                }
                else
                {
                    Debug.WriteLine("alpha");
                    Task GetProductByName = manager.GetProductByName(searchText.Text);
                    Debug.WriteLine(searchText.Text);
                    await GetProductByName;
                    HideRing();
                    productSearch.tabSelected = 1;
                    //if(StoreEmpowermentModel.productNames[0].)
                    NavigateToHomePage();
                }

            }
          
          
        }

        private async void SearchItemsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NetworkManager manager = NetworkManager.Instance;
            if (storeList.SelectedIndex != -1 && StoreEmpowermentModel.productNames[storeList.SelectedIndex].ImageURL != "")
            {
                ShowRing();
                try
                {
                    //Task GetProductTask = manager.GetProductAsync("5000436589457");
                    Task GetProductTask = manager.GetProductAsync(StoreEmpowermentModel.productNames[storeList.SelectedIndex].EAN);
                    await GetProductTask;
                }
                catch (Exception)
                {

                }

            }
        }

        private async void RecentItemsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("index : " + recentList.SelectedIndex);
            NetworkManager manager = NetworkManager.Instance;
            if (recentList.SelectedIndex != -1)
            {
                ShowRing();

                //Task GetProductTask = manager.GetProductAsync();
                String temp = StoreEmpowermentModel.recentItems[recentList.SelectedIndex];
                temp = temp.Substring(temp.Length - 13, 13);
                Debug.WriteLine(temp);
                Task GetProductTask = manager.GetProductAsync(temp);

                await GetProductTask;

            }
        }



        public void SetSearchByNameListSource()
        {

            HideRing();
            NavigateToHomePage();
            Debug.WriteLine("before");

            for (int i = 0; i < StoreEmpowermentModel.productsearchResult.Count; ++i)
            {
                for (int j = 0; j < StoreEmpowermentModel.productsearchResult[i].Products.Count; ++j)
                {
                    storeList.ItemsSource = StoreEmpowermentModel.productsearchResult[i].Products[j].Description;
                }
            }
            //StoreEmpowermentModel.productsearchResult
            // storeList.ItemsSource = searchresponse;
            Debug.WriteLine("After");

        }

    }
}
