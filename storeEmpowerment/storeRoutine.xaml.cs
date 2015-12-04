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
using Windows.Storage;
using Windows.UI;
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
    public sealed partial class storeRoutine : Page, ILHMView
    {
        public static int tabSelected;
        public static storeRoutine storePointer;
        public static int flag = 0;
        private static ListViewController listViewController;
        public static ProgressRing pring;
        private static int errorFlag = 0;
        private static int quarterFlag = 1;
        private int count = 0;
        private LHMPresenter lhmPresenter = new LHMPresenter();

        Windows.UI.ViewManagement.StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();


        public storeRoutine()
        {
            this.InitializeComponent();
            GoogleAnalytics.EasyTracker.GetTracker().SendView("storeRoutine Page");
            DrawerLayout.InitializeDrawerLayout();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            storePointer = this;
            lhmPresenter.Add(this);
            listViewController = new ListViewController(listView);

        }

        public async void NavigateToLoginPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(LoginPage)); });
        }

        public async void NavigateToSettingsPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(NewSettings)); });
        }

        public async void NavigateToAboutPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(Settings)); });
        }

        public async void NavigateToSelectStorePage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(StoreList)); });
        }

        public async void NavigateToComplianceRoutinePage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(storeRoutine)); });
        }
        public async void NavigateToTeamAccessPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(TeamAccess)); });
        }

        public static void successHandleforRoutineData(RoutineData routinedata)
        {
            try
            {
                ResponseHandler.storeRoutineFlag = 0;
                StoreEmpowermentModel.kpiData = routinedata;
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!" + StoreEmpowermentModel.kpiData.this_quarter.update_timestamp);
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!" + StoreEmpowermentModel.kpiData.last_quarter.update_timestamp);
                storePointer.storeName.Text = "-" + routinedata.Store_Name;
                Debug.WriteLine(quarterFlag + "*********");
                listViewController.populateList(quarterFlag);
                storePointer.renderButtons(quarterFlag);
                storePointer.listView.Visibility = Visibility.Visible;
                pring.IsActive = false;
                storeRoutine.storePointer.IsEnabled = true;
                

            }
            catch (Exception e)
            {

            }
        }

        private void changeMargin(Thickness margin, int bottomMargin, Control uiControl)
        {
            margin.Top = 0;
            margin.Bottom = bottomMargin;
            uiControl.Margin = margin;
        }
        private void renderButtons(int flag)
        {

            string green = "#0B780B";
            string red = "#C10B0B";
            string grey = "#696969";
            if (flag == 1)
            {
                ThisQuarter quarter = StoreEmpowermentModel.kpiData.this_quarter;
                overallScoreText.Text = (Int32)(Double.Parse(quarter.overall_score) * 100) + " %";
                if (quarter.overall_RAG == "G")
                {
                    overallScore.Background = GetColorFromHexa(green);
                }
                else
                {
                    overallScore.Background = GetColorFromHexa(grey);
                }

                if (quarter.is_approved == "Y")
                {
                    rejectedText.Text = "Approved";
                    rejected.Background = GetColorFromHexa(green);
                }
                else
                {
                    Debug.WriteLine("grey color");
                    rejectedText.Text = "Not Approved";
                    rejected.Background = GetColorFromHexa(grey);
                }
            }
            else
            {
                ThisQuarter quarter = StoreEmpowermentModel.kpiData.last_quarter;
                Debug.WriteLine(quarter.is_approved + "____________________________________");

                overallScoreText.Text = (Int32)(Double.Parse(quarter.overall_score) * 100) + " %";
                if (quarter.overall_RAG == "G")
                {
                    overallScore.Background = GetColorFromHexa(green);
                }
                else
                {
                    overallScore.Background = GetColorFromHexa(grey);
                }

                if (quarter.is_approved == "Y")
                {
                    rejectedText.Text = "Approved";
                    rejected.Background = GetColorFromHexa(green);
                }
                else
                {
                    rejectedText.Text = "Not Approved";
                    rejected.Background = GetColorFromHexa(grey);
                }
            }


        }
        private void changeMarginText(Thickness margin, int bottomMargin, TextBlock uiControl)
        {


            margin.Top = 0;
            margin.Bottom = bottomMargin;
            uiControl.Margin = margin;
        }

        private double uiArrange()
        {
            Debug.WriteLine("1");
            var widthScreen = Window.Current.Bounds.Width;
            var heightScreen = Window.Current.Bounds.Height;
            Debug.WriteLine("2");
            Canvas.SetLeft(overallScore, widthScreen - overallScore.Width - 20);
            Canvas.SetLeft(rejected, widthScreen - rejected.Width - 20);
            Canvas.SetLeft(homeIcon, .12 * widthScreen);
            var length = myOrders1.Width * .7;
            Canvas.SetLeft(myOrders1, (widthScreen - length) / 2);
            Canvas.SetLeft(searchIcon, widthScreen * 0.8);
            Debug.WriteLine("3");
            Thickness margin = bottomText.Margin;
            margin.Top = heightScreen - 150.00;
            bottomText.Margin = margin;
            Double temp = (heightScreen - 667.00) / 10.0;
            Double height;
            height = (heightScreen - listView.Margin.Top - bottomText.Height - 30 - temp) / 5;
            Debug.WriteLine(bottomText.Height + "   " + height);
            return height;


        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /// 

        public static async void getData()
        {
           // storeRoutine.init();
           // storeRoutine StoreRoutine = new storeRoutine();
            //StoreRoutine.init();
            Debug.WriteLine("inside getData");

            Debug.WriteLine("inside getData1");
            // pring.IsActive = true;
            //storeRoutine.storePointer.IsEnabled = false;
            if (App.localSettings.Values["stores"] == null)
            {
                ResponseHandler.storeRoutineFlag = 0;

                Debug.WriteLine("Inside Change Store");

                storePointer.Frame.Navigate(typeof(StoreList));
            }
            else
            {
                ResponseHandler.storeRoutineFlag++;
                NetworkManager manager = NetworkManager.Instance;
                Task<RoutineData> routineDataTask = manager.RoutineDataAsync((string)App.localSettings.Values["username"], (string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["stores"]);
                await routineDataTask;
            }
           // await StoreRoutine.statusBar.HideAsync();

        }


        private void init()
        {
            qtrToDate.Foreground = new SolidColorBrush(Colors.White);
            lastQtr.Foreground = new SolidColorBrush(Colors.LightGray);
            color.Background = new SolidColorBrush(Colors.CornflowerBlue);
            colorlstQtr.Background = new SolidColorBrush(Colors.Transparent);
            Double widthScreen = Window.Current.Bounds.Width;
            qtrToDateabove.Width = widthScreen / 2;
            qtrToDateabove.Height = myCanvas.Height;
            lastQtrabove.Width = widthScreen / 2;
            lastQtrabove.Height = myCanvas.Height;
            Canvas.SetLeft(lastQtrabove, widthScreen / 2);
            lastQtr.Width = widthScreen / 2;
            qtrToDate.Width = widthScreen / 2;
            Canvas.SetLeft(lastQtr, widthScreen / 2);
            color.Width = widthScreen / 2;
            colorlstQtr.Width = widthScreen / 2;
            Canvas.SetLeft(colorlstQtr, 0.5 * widthScreen);
            Canvas.SetLeft(searchIcon, 0.8 * widthScreen);
            Canvas.SetLeft(scanIcon, 0.9 * widthScreen);
        }
        private void DrawerIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "SideMenu icon - Headerbar", 0);
            Debug.WriteLine("inside taooed");
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            else
            {
                DrawerLayout.OpenDrawer();

            }
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            init();
            // flag = 1;
            App.localSettings.Values["currentPage"] = "kpi";
            App.localSettings.Values["currentTab"] = "Last 6 weeks";
            Debug.WriteLine("inside navigation");
            try
            {
                menuName.Text = App.localSettings.Values["storename"].ToString().ToUpper();
            }
            catch (Exception)
            {

            }
            pring = p_ring;
            Debug.WriteLine("inside navigation1");
            //uiArrange();
            var height = uiArrange();
            if (count++ == 0)
            {
                Debug.WriteLine("goes in");
                itemStyle.Setters.Add(new Setter(ListViewItem.HeightProperty, height));
            }
            Debug.WriteLine("inside navigation2");
            if (listView.Items.Count == 1)
            {
                listView.Items.RemoveAt(0);
            }
            //listView.Items.RemoveAt(0);

            tabSelected = 0;
            Debug.WriteLine(tabSelected + "@@@@@@@@@@@@@@@@@@@@@@");
            if (tabSelected == 0)
            {
                this_quarter();
            }
            else if (tabSelected == 1)
            {
                last_quarter();
            }
            Debug.WriteLine("inside navigation3");
            p_ring.IsActive = true;
            storeRoutine.storePointer.IsEnabled = false;
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
                Task<RoutineData> routineDataTask = manager.RoutineDataAsync((string)App.localSettings.Values["username"], (string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["stores"]);
                await routineDataTask;
            }

            await statusBar.HideAsync();
        }

        public void last_quarter()
        {
            tabSelected = 0;
            qtrToDate.Foreground = new SolidColorBrush(Colors.LightGray);
            lastQtr.Foreground = new SolidColorBrush(Colors.White);
            color.Background = new SolidColorBrush(Colors.Transparent);
            colorlstQtr.Background = new SolidColorBrush(Colors.CornflowerBlue);
            if (quarterFlag == 1 && StoreEmpowermentModel.kpiData.last_quarter != null)
            {
                quarterFlag = 0;
                listViewController.clearlist();
                listViewController.populateList(quarterFlag);
                renderButtons(quarterFlag);
                //qtrToDate.Background = GetColorFromHexa("#76787C");
                // lastQtr.Background = GetColorFromHexa("#387EF5");
            }
            App.localSettings.Values["currentTab"] = "Last Quarter";

        }

        public void this_quarter()
        {
            tabSelected = 1;
            qtrToDate.Foreground = new SolidColorBrush(Colors.White);
            lastQtr.Foreground = new SolidColorBrush(Colors.LightGray);
            color.Background = new SolidColorBrush(Colors.CornflowerBlue);
            colorlstQtr.Background = new SolidColorBrush(Colors.Transparent);
            if (quarterFlag == 0 && StoreEmpowermentModel.kpiData.this_quarter != null)
            {
                quarterFlag = 1;
                listViewController.clearlist();
                listViewController.populateList(quarterFlag);
                renderButtons(quarterFlag);
                //lastQtr.Background = GetColorFromHexa("#76787C");
                //qtrToDate.Background = GetColorFromHexa("#387EF5");
            }
            quarterFlag = 1;
            App.localSettings.Values["currentTab"] = "Last 6 weeks";

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        private void ListView_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void listView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
        private void qtrToDate_Click(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Tab", "Click", "Last 6 weeks tab", 0);
            App.localSettings.Values["currentTab"] = "Last 6 weeks";
            this_quarter();
        }

        private async void Scan_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Scan icon - Headerbar", 0);
            App.localSettings.Values["barcode"] = "false";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(MainPage)); });
        }
        private void lastQtr_Click(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Tab", "Click", "Last Quarter", 0);
            App.localSettings.Values["currentTab"] = "Last Quarter";
            last_quarter();
        }
        public static SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            byte R = Convert.ToByte(hexaColor.Substring(1, 2), 16);
            byte G = Convert.ToByte(hexaColor.Substring(3, 2), 16);
            byte B = Convert.ToByte(hexaColor.Substring(5, 2), 16);
            SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(0xFF, R, G, B));
            return scb;
        }
        private async void home_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Home icon - Headerbar", 0);
            App.localSettings.Values["tab"] = "recentItems";

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });

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
        public async void NavigateToTeamPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(TeamAccess)); });
        }
        private async void searchIconClick(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Search icon - Headerbar", 0);
            App.localSettings.Values["tab"] = "recentItems";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
        }




    }
}
