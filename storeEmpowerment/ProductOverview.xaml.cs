
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace storeEmpowerment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductOverview : Page, ILHMView
    {
        public double width = Window.Current.Bounds.Width;
        public static ProductOverview prodPointer;
        public static WebView web;
        private LHMPresenter lhmPresenter = new LHMPresenter();
        private static int flag = 0;
        private static int wFlag = 0;
        //private static readonly Uri HomeUri = new Uri("http://www.google.com");
        private static readonly Uri HomeUri = new Uri("ms-appx-web:///Html/resp.html", UriKind.Absolute);
        // private static readonly Uri HomeUri = new Uri("ms-appdata:///local/NavigateToState/resp.html", UriKind.Absolute);

        public ProductOverview()
        {
            prodPointer = this;
            this.InitializeComponent();
             GoogleAnalytics.EasyTracker.GetTracker().SendView("ProductOverview");
            DrawerLayout.InitializeDrawerLayout();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            lhmPresenter.Add(this);
            WebViewControl.NavigationCompleted += WebViewControl_NavigationCompleted;
            //Debug.WriteLine(StoreEmpowermentModel.htmlResponse);
            WebViewControl.ScriptNotify += WebViewControl_ScriptNotify;
            web = WebViewControl;
            DrawerLayout.Visibility = Visibility.Collapsed;
        }
        async void WebViewControl_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {

            //p_ring.IsActive = true;
            NetworkManager manager = NetworkManager.Instance;
            Task<string> GetCalendarTask = manager.GetCalendarAsync(StoreEmpowermentModel.tpnb);
            StoreEmpowermentModel.calendarResponse = await GetCalendarTask;
            Debug.WriteLine("webview data set");
            await this.WebViewControl.InvokeScriptAsync("SetData", new string[] { (string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["stores"], StoreEmpowermentModel.tpnb, StoreEmpowermentModel.details.calAccess });
            Debug.WriteLine("webview calendar set");
            if (StoreEmpowermentModel.calendarResponse != "")
            {
                Debug.WriteLine(StoreEmpowermentModel.calendarResponse );
                await this.WebViewControl.InvokeScriptAsync("SetCal", new string[] { StoreEmpowermentModel.calendarResponse });
                try
                {
                    double count = StoreEmpowermentModel.details.Offers.Count;
                    Debug.WriteLine("outside count"+StoreEmpowermentModel.details.Offers.Count);
                    Debug.WriteLine("the statre" + StoreEmpowermentModel.details.Offers[0].StartDate);
                    await this.WebViewControl.InvokeScriptAsync("setOffer", new string[] { StoreEmpowermentModel.details.Offers.Count.ToString() });
                    for (int i = 0; i < count; i++)
                    {
                       // Debug.WriteLine(StoreEmpowermentModel.details.Offers[1].StartDate);
                        Debug.WriteLine("startdate"+StoreEmpowermentModel.details.Offers[i].StartDate);
                        Debug.WriteLine("enddate"+StoreEmpowermentModel.details.Offers[i].EndDate);
                        await this.WebViewControl.InvokeScriptAsync("SetOffersStartDate", new string[] { StoreEmpowermentModel.details.Offers[i].StartDate.ToString() });
                        await this.WebViewControl.InvokeScriptAsync("setOffersEndDate", new string[] { StoreEmpowermentModel.details.Offers[i].EndDate.ToString() });
                    }
                }
                catch(Exception ex)
                {

                }
            }
            Debug.WriteLine("calendar response is  : " + StoreEmpowermentModel.calendarResponse);
            //WebViewControl.Visibility = Visibility.Visible;
            // p_ring.IsActive = false;

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

        private async void Scan_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Scan icon - Headerbar", 0);
            App.localSettings.Values["barcode"] = "false";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(MainPage)); });
        }

        public async void NavigateToSelectStorePage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(StoreList)); });
        }
        public async void NavigateToTeamPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(TeamAccess)); });
        }
        public async void NavigateToComplianceRoutinePage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(storeRoutine)); });
        }
        public async void NavigateToTeamAccessPage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(TeamAccess)); });
        }

        private void Browser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (!args.IsSuccess)
            {
                //Debug.WriteLine("Navigation to this page failed, check your internet connection.");
            }
        }
        private void DrawerIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "SideMenu icon - Headerbar", 0);
            if (DrawerLayout.IsDrawerOpen)
            {
                DrawerLayout.Visibility = Visibility.Collapsed;
                MainFragment.Visibility = Visibility.Collapsed;
                DrawerLayout.CloseDrawer();

            }

            else
            {
                DrawerLayout.Visibility = Visibility.Visible;
                DrawerLayout.OpenDrawer();
                //DrawerLayout.Visibility = Visibility.Visible;
            }

        }

        async void WebViewControl_ScriptNotify(object sender, NotifyEventArgs e)
        {
            Debug.WriteLine(++flag + " :     " + e.Value);
            if (e.Value.StartsWith("get"))
            {
                Debug.WriteLine("went inside get ");
                NetworkManager manager = NetworkManager.Instance;
                Task<string> GetCalendarTask = manager.GetCalendarAsync(StoreEmpowermentModel.tpnb);
                StoreEmpowermentModel.calendarResponse = await GetCalendarTask;
                //Debug.WriteLine("response : " + StoreEmpowermentModel.calendarResponse);
                //WebViewControl.Refresh();
                if (StoreEmpowermentModel.calendarResponse != "")
                    await this.WebViewControl.InvokeScriptAsync("SetCal", new string[] { StoreEmpowermentModel.calendarResponse });
                //Debug.WriteLine("response sent");
            }
            else if (e.Value.StartsWith("https") && e.Value.Contains("OrderDate"))
            {
                DrawerLayout.Visibility = Visibility.Visible;
                //MainFragment.Visibility = Visibility.Visible;
                p_ring.IsActive = true;
                Debug.WriteLine("check save : " + e.Value);
                Debug.WriteLine("the save string is" + e.Value.ToString() + " " + e.Value.IndexOf("{") + e.Value.Length);
                int index = e.Value.IndexOf("{");
                int index1 = index - 1;
                string temp1 = e.Value.Substring(index, (e.Value.Length - 1) - index);
                Debug.WriteLine("temp1" + " " + temp1);
                string temp2 = "," + "\"StoreOrderReactive\"" + ":" + "\"YES\"" + "," + "\"User\"" + ":" + "\"" + App.localSettings.Values["username"] + "\"" + "}";
                Debug.WriteLine("temp2" + " " + temp2);
                string temp = temp1 + temp2;
                Debug.WriteLine("temp" + temp);
                string url = e.Value.Remove(index, e.Value.Length - index);
                Debug.WriteLine("url" + url);
                NetworkManager manager = NetworkManager.Instance;
                Task<string> SaveCalendarTask = manager.SaveCalendarAsync(url, temp);
                await SaveCalendarTask;
                Debug.WriteLine("data " + temp + "\nUrl " + url);
                Task<string> GetCalendarTask = manager.GetCalendarAsync(StoreEmpowermentModel.tpnb);
                StoreEmpowermentModel.calendarResponse = await GetCalendarTask;
                Debug.WriteLine("calendar data set inside save");
                if (StoreEmpowermentModel.calendarResponse != "")
                    await this.WebViewControl.InvokeScriptAsync("SetCal", new string[] { StoreEmpowermentModel.calendarResponse });
                WebViewControl.Refresh();
                //await this.WebViewControl.InvokeScriptAsync("SetRev", new string[] { });
                p_ring.IsActive = false;
                //MainFragment.Visibility = Visibility.Collapsed;
                DrawerLayout.Visibility = Visibility.Collapsed;
            }
            else if (e.Value.StartsWith("https"))
            {
                Debug.WriteLine("came here");
                DrawerLayout.Visibility = Visibility.Visible;
                p_ring.IsActive = true;
                NetworkManager manager = NetworkManager.Instance;
                //  int index = e.Value.IndexOf("{");
                //Debug.WriteLine(e.Value.Length+ e.Value.ToString());
                Debug.WriteLine("the save string is" + e.Value.ToString() + " " + e.Value.IndexOf("{") + e.Value.Length);
                Debug.WriteLine("the order date" + e.Value.Substring(e.Value.Length - 10, 10));
                string temp3 = e.Value.Substring(e.Value.Length - 10, 10);
                string temp1 = "{" + "\"OrderDate\"" + ":" + "\"" + temp3 + "\"" + "," + "\"Quantity\"" + ":" + 0;
                string temp2 = "," + "\"StoreOrderReactive\"" + ":" + "\"YES\"" + "," + "\"User\"" + ":" + "\"" + App.localSettings.Values["username"] + "\"" + "}";
                string temp = temp1 + temp2;
                Debug.WriteLine("the temp is" + temp);
                string url = e.Value.Remove(e.Value.Length - 10, 10);
                Debug.WriteLine("the url is " + url);
                Task<string> RevertCalendarTask = manager.RevertCalendarAsync(url, temp);
                await RevertCalendarTask;
                Debug.WriteLine("came here 1");
                Debug.WriteLine("came here 2");
                Task<string> GetCalendarTask = manager.GetCalendarAsync(StoreEmpowermentModel.tpnb);
                StoreEmpowermentModel.calendarResponse = await GetCalendarTask;

                Debug.WriteLine("calendar data set inside revert");
                if (StoreEmpowermentModel.calendarResponse != "")
                    await this.WebViewControl.InvokeScriptAsync("SetCal", new string[] { StoreEmpowermentModel.calendarResponse });
                WebViewControl.Refresh();
                DrawerLayout.Visibility = Visibility.Collapsed;
                p_ring.IsActive = false;
                //await this.WebViewControl.InvokeScriptAsync("SetRev", new string[] { });
            }

            //Debug.WriteLine(++flag + " :     " + e.Value);

        }

        private void borderArrange(Border border)
        {
            //var width = Window.Current.Bounds.Width;
            border.Width = .95 * width;
            //border.Height = .35 * width;
        }
        private void borderPosition(Border border)
        {
            Canvas.SetLeft(border, .025 * width);

        }

        private void canvasArrange(Canvas canvas)
        {
            canvas.Height = .07 * width;
            canvas.Width = .95 * width;

        }

        private void marginSetter(Border border, int value)
        {
            Thickness marg = new Thickness();
            marg = border.Margin;
            marg.Top = 20;
            border.Margin = marg;
        }
        private string GetLocation(int index)
        {
            string location = "Aisle";
            location = location + StoreEmpowermentModel.details.ProductLocation[index].Aisle.ToString() + ", ";
            if (StoreEmpowermentModel.details.ProductLocation[index].AisleSide == "L")
            {
                location = location + "Left, Mod " + StoreEmpowermentModel.details.ProductLocation[index].Module + ", Shelf ";
            }
            else if (StoreEmpowermentModel.details.ProductLocation[index].AisleSide == "R")
            {
                location = location + "Right, Mod " + StoreEmpowermentModel.details.ProductLocation[index].Module + ", Shelf ";

            }
            else if (StoreEmpowermentModel.details.ProductLocation[index].AisleSide == "E")
            {
                location = location + "End, Mod " + StoreEmpowermentModel.details.ProductLocation[index].Module + ", Shelf ";

            }
            else
            {
                location = location + "Side " + StoreEmpowermentModel.details.ProductLocation[index].AisleSide + ", Mod " + StoreEmpowermentModel.details.ProductLocation[index].Module + ", Shelf ";

            }
            location = location + StoreEmpowermentModel.details.ProductLocation[index].Shelf_Reference;
            return location;

        }

        private string GetLocationSingles(int index)
        {
            string location = "";
            location = location + StoreEmpowermentModel.details.ProductLocation[index].Aisle.ToString() + "";
            if (StoreEmpowermentModel.details.ProductLocation[index].AisleSide == "L")
            {
                location = location + "L" + StoreEmpowermentModel.details.ProductLocation[index].Module + "";
            }
            else if (StoreEmpowermentModel.details.ProductLocation[index].AisleSide == "R")
            {

                location = location + "R" + StoreEmpowermentModel.details.ProductLocation[index].Module + "";

            }
            else if (StoreEmpowermentModel.details.ProductLocation[index].AisleSide == "E")
            {
                location = location + "E" + StoreEmpowermentModel.details.ProductLocation[index].Module + "";
            }
            else
            {
                location = location + "S" + StoreEmpowermentModel.details.ProductLocation[index].AisleSide + "" + StoreEmpowermentModel.details.ProductLocation[index].Module + "";

            }
            location = location + StoreEmpowermentModel.details.ProductLocation[index].Shelf_Reference;
            location = location + StoreEmpowermentModel.details.ProductLocation[index].Sequence_Number;
            return location;

        }
        private String converter(String date)
        {
            String mon = date.Substring(5, 2);
            Debug.WriteLine("month  :" + mon);
            String month = null;
            if (mon.Equals("01"))
            {
                month = "Jan";
            }
            else if (mon.Equals("02"))
            {
                month = "Feb";
            }
            else if (mon.Equals("03"))
            {
                month = "Mar";
            }
            else if (mon.Equals("04"))
            {
                month = "Apr";
            }
            else if (mon.Equals("05"))
            {
                month = "May";
            }
            else if (mon.Equals("06"))
            {
                month = "Jun";
            }
            else if (mon.Equals("07"))
            {
                month = "Jul";
            }
            else if (mon.Equals("08"))
            {
                month = "Aug";
            }
            else if (mon.Equals("09"))
            {
                month = "Sep";
            }
            else if (mon.Equals("10"))
            {
                month = "Oct";
            }
            else if (mon.Equals("11"))
            {
                month = "Nov";
            }
            else if (mon.Equals("12"))
            {
                month = "Dec";
            }

            String d = date.Substring(8, 2);
            Debug.WriteLine("date : " + d);
            String day = null;
            switch (d)
            {
                case "01":
                    day = "1st";
                    break;
                case "02":
                    day = "2nd";
                    break;
                case "03":
                    day = "3rd";
                    break;
                case "04":
                    day = "4th";
                    break;
                case "05":
                    day = "5th";
                    break;
                case "06":
                    day = "6th";
                    break;
                case "07":
                    day = "7th";
                    break;
                case "08":
                    day = "8th";
                    break;
                case "09":
                    day = "9th";
                    break;
                case "10":
                    day = "10th";
                    break;
                case "11":
                    day = "11th";
                    break;
                case "12":
                    day = "12th";
                    break;
                case "13":
                    day = "13th";
                    break;
                case "14":
                    day = "14th";
                    break;
                case "15":
                    day = "15th";
                    break;
                case "16":
                    day = "16th";
                    break;
                case "17":
                    day = "17th";
                    break;
                case "18":
                    day = "18th";
                    break;
                case "19":
                    day = "19th";
                    break;
                case "20":
                    day = "20th";
                    break;
                case "21":
                    day = "21st";
                    break;
                case "22":
                    day = "22nd";
                    break;
                case "23":
                    day = "23rd";
                    break;
                case "24":
                    day = "24th";
                    break;
                case "25":
                    day = "25th";
                    break;
                case "26":
                    day = "26th";
                    break;
                case "27":
                    day = "27th";
                    break;
                case "28":
                    day = "28th";
                    break;
                case "29":
                    day = "29th";
                    break;
                case "30":
                    day = "30th";
                    break;
                case "31":
                    day = "31st";
                    break;

            }
            return day + " " + month;
        }
        private int convertStringToInt(String str)
        {
            int a = Int32.Parse(str);
            return a;
        }

        private string GetUnit(int unit)
        {
            string unitString = "";
            if (unit < 10)
            {
                unitString = "000" + unit.ToString();
            }
            else if (unit < 100)
            {
                unitString = "00" + unit.ToString();
            }
            else if (unit < 1000)
            {
                unitString = "0" + unit.ToString();
            }
            else
            {
                unitString = unit.ToString();
            }
            return unitString;
        }
        private string convertDecimalToString(double Price)
        {
            //decimal decimalNumber;
            //decimalNumber = 126.89889M;
            string value;

            value = (Price).ToString("0.00");
            Debug.WriteLine(value);
            //inputText.Text = value;
            return value;
            //Debug.WriteLine(inputText.Text);
        }


        private void labelBinding()
        {

            Debug.WriteLine("the login token is " + App.localSettings.Values["newToken"]);
            ClearList();
            DateTime dt = DateTime.Today;
            productName.Text = StoreEmpowermentModel.details.Product.Description;
            Debug.WriteLine("unit size is @@@@@@@@@@@@@@@@@@@@@@@@@ " + GetUnit((int)float.Parse(StoreEmpowermentModel.details.Product.UnitSize)));
            productId.Text = StoreEmpowermentModel.details.Product.TPNB + " \\ " + GetUnit((int)float.Parse(StoreEmpowermentModel.details.Product.UnitSize)) + "\n" + StoreEmpowermentModel.details.Product.EAN;
            if(StoreEmpowermentModel.details.PriceAPI.variants[0].currency.Equals("GBP"))
            {
                productPrice.Text = "\u00A3 " + StoreEmpowermentModel.details.PriceAPI.variants[0].authPrice;
            }
            else if (StoreEmpowermentModel.details.PriceAPI.variants[0].currency.Equals("EUR"))
            {
                productPrice.Text = "\u20AC" + StoreEmpowermentModel.details.PriceAPI.variants[0].authPrice;
            }
           /* if (StoreEmpowermentModel.details.PriceAPI.variants[0].currency.Equals("GBP"))
            {
                Debug.WriteLine(StoreEmpowermentModel.details.PriceAPI.variants.Count);
                if (StoreEmpowermentModel.details.PriceAPI.variants.Count==1)
                {
                    productPrice.Text = "\u00A3 " + StoreEmpowermentModel.details.PriceAPI.variants[0].authPrice;
                }
                else {
                    double price=0;
                    for(int i=0;i<StoreEmpowermentModel.details.PriceAPI.variants.Count;i++)
                    {
                        
                      
                        if( Double.Parse(StoreEmpowermentModel.details.PriceAPI.variants[i+1].authPrice)>Double.Parse(StoreEmpowermentModel.details.PriceAPI.variants[i].authPrice))
                        {
                            double t1 = Double.Parse(StoreEmpowermentModel.details.PriceAPI.variants[i].authPrice); 
                            double t2 = Double.Parse(StoreEmpowermentModel.details.PriceAPI.variants[i + 1].authPrice);
                            price =t1;
                            t1 = t2;
                            t2 = price;
                          

                        }
                        
                    }
                    productPrice.Text = "\u00A3 " + convertDecimalToString(price);
                }
            }
            if (StoreEmpowermentModel.details.PriceAPI.variants[0].currency.Equals("GBP"))
            {
                Debug.WriteLine(StoreEmpowermentModel.details.PriceAPI.variants.Count);
                if (StoreEmpowermentModel.details.PriceAPI.variants.Count == 1)
                {
                    productPrice.Text = "\u20AC " + StoreEmpowermentModel.details.PriceAPI.variants[0].authPrice;
                }
                else
                {
                    double price = 0;
                    for (int i = 0; i < StoreEmpowermentModel.details.PriceAPI.variants.Count; i++)
                    {


                        if (Double.Parse(StoreEmpowermentModel.details.PriceAPI.variants[i + 1].authPrice) > Double.Parse(StoreEmpowermentModel.details.PriceAPI.variants[i].authPrice))
                        {
                            double t1 = Double.Parse(StoreEmpowermentModel.details.PriceAPI.variants[i].authPrice);
                            double t2 = Double.Parse(StoreEmpowermentModel.details.PriceAPI.variants[i + 1].authPrice);
                            price = t1;
                            t1 = t2;
                            t2 = price;


                        }

                    }
                    productPrice.Text = "\u20AC " + convertDecimalToString(price);
                }
            }*/
            //productPrice.Text = "\u00A3 " + convertDecimalToString(StoreEmpowermentModel.details.PriceAPI.variants[0]);
           // productPrice.Text = "\u20AC" + convertDecimalToString(StoreEmpowermentModel.details.Price.SellingPrice);
            Debug.WriteLine("dasdadas" + StoreEmpowermentModel.details.BookStock.Quantity);
            productImage.Source = new BitmapImage(new Uri(ImageSource()));

            if (StoreEmpowermentModel.details.Range.EndDate != "")
            {
                if (Convert.ToDateTime(StoreEmpowermentModel.details.Range.EndDate) != DateTime.MaxValue)
                {
                    stockedUntilCanvas.Visibility = Visibility.Visible;
                    String str = Convert.ToDateTime(StoreEmpowermentModel.details.Range.EndDate).ToString();
                    stockedUntil.Visibility = Visibility.Visible;
                    stockedUntil.Text = "    Stocked Until" + " " + str.Substring(0, 2) + "/" + str.Substring(3, 2) + "/" + str.Substring(6, 4);

                    blank1.Visibility = Visibility.Visible;
                }
            }

            if (StoreEmpowermentModel.details.BookStock.Quantity < 0)
            {
                negativeStockCanvas.Visibility = Visibility.Visible;
                negativeStock.Visibility = Visibility.Visible;
                blank2.Visibility = Visibility.Visible;
            }
            else if (StoreEmpowermentModel.details.BookStock.Quantity == 0)
            {
                zeroStockCanvas.Visibility = Visibility.Visible;
                zeroStock.Visibility = Visibility.Visible;
                blank3.Visibility = Visibility.Visible;
            }
            else
            {
                Debug.WriteLine("in product overview bottom");
            }
            if ((convertStringToInt(StoreEmpowermentModel.details.ShelfCapacity.Facing) > 0) && (StoreEmpowermentModel.details.ShelfCapacity.Capacity < 1))
            {
                noShelfCapacityCanvas.Visibility = Visibility.Visible;
                noShelfCapacity.Visibility = Visibility.Visible;
            }
            if ((StoreEmpowermentModel.details.Product.SellByWeight.ToUpper().Equals("W")))
            {
                currentStockTextBlock.Text = "Current Stock (KG)";
            }
            else
            {
                currentStockTextBlock.Text = "Current Stock (Cases/Singles)";
            }

            //stock.Text = StoreEmpowermentModel.details.BookStock.QuantityInCasesAndSingles;
            if (StoreEmpowermentModel.details.BookStock.QuantityInCasesAndSingles != "")
            {
                currentStockBorder.Visibility = Visibility.Visible;
                currentPanel.Visibility = Visibility.Visible;

                currentStockListView.Items.Add(new StockList() { stock = StoreEmpowermentModel.details.BookStock.QuantityInCasesAndSingles, stockName = "Stock Record" });
                Debug.WriteLine("no of items is " + currentStockListView.Items.Count);
            }

            if (StoreEmpowermentModel.details.ShelfCapacity.CapacityAsCasesAndSingles != "")
            {
                shelfDetailsBorder.Visibility = Visibility.Visible;
                shelfPanel.Visibility = Visibility.Visible;
                Debug.WriteLine("CapacityForOffer" + StoreEmpowermentModel.details.ShelfCapacity.CapacityForOffer);
                ListView1.Items.Add(new ShelfList() { shelfName = "Standard", capacity = StoreEmpowermentModel.details.ShelfCapacity.CapacityAsCasesAndSingles, facing = StoreEmpowermentModel.details.ShelfCapacity.Facing });
                if (StoreEmpowermentModel.details.ShelfCapacity.CapacityForOffer != 0)
                {
                    if (StoreEmpowermentModel.details.Offers.Count == 1)
                    {
                        if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles.Equals("0/000"))
                        {
                            Debug.WriteLine("inside additional zero");
                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles), facing = "" });
                        }
                    }
                    else if (StoreEmpowermentModel.details.Offers.Count == 2)
                    {
                        if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles.Equals("0/000"))
                        {
                            Debug.WriteLine("inside additional zero");
                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles, facing = "" });
                        }
                        if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles.Equals("0/000"))
                        {
                            Debug.WriteLine("inside additional zero");
                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles, facing = "" });
                        }
                    }
                    else if (StoreEmpowermentModel.details.Offers.Count == 3)
                    {
                        if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles.Equals("0/000"))
                        {
                            Debug.WriteLine("inside additional zero");
                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles, facing = "" });

                        }
                        if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles.Equals("0/000"))
                        {
                            Debug.WriteLine("inside additional zero");
                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles, facing = "" });
                        }
                        if (StoreEmpowermentModel.details.Offers[2].OfferShelfCapacityAsCasesAndSingles.Equals("0/000"))
                        {
                            Debug.WriteLine("inside additional zero");
                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[2].OfferShelfCapacityAsCasesAndSingles, facing = "" });
                        }

                    }
                    else if (StoreEmpowermentModel.details.Offers.Count == 4)
                    {
                        if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles.Equals("0/000"))
                        {
                            Debug.WriteLine("inside additional zero");
                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles, facing = "" });
                        }
                        if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles.Equals("0/000"))
                        {
                            Debug.WriteLine("inside additional zero");
                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles, facing = "" });
                        }
                        if (StoreEmpowermentModel.details.Offers[2].OfferShelfCapacityAsCasesAndSingles.Equals("0/000"))
                        {
                            Debug.WriteLine("inside additional zero");
                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[2].OfferShelfCapacityAsCasesAndSingles, facing = "" });
                        }
                        if (StoreEmpowermentModel.details.Offers[3].OfferShelfCapacityAsCasesAndSingles.Equals("0/000"))
                        {
                            Debug.WriteLine("inside additional zero");
                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[3].OfferShelfCapacityAsCasesAndSingles, facing = "" });
                        }
                    }
                    else
                    {
                        Debug.WriteLine("inside else of additional");
                    }

                }
                else
                {
                    Debug.WriteLine("inside outer else of additon");
                }
            }
            
            if (StoreEmpowermentModel.details.Deliveries.Count != 0)
            {
                deliveryBorder.Visibility = Visibility.Visible;
                deliveryPanel.Visibility = Visibility.Visible;
                if ((StoreEmpowermentModel.details.Product.SellByWeight.ToUpper().Equals("W")))
                {
                    ListView.Header = new DeliverList() { delivery = "Delivery(" + StoreEmpowermentModel.details.BookStock.UOM + ")", date = "Date", quantity = "Quantity" };
                }
                ListView.Header = new DeliverList() { delivery = "Delivery (C/S)", date = "Date", quantity = "Quantity" };

                ListView.Items.Add(new DeliverList() { delivery = "Last Delivery", date = StoreEmpowermentModel.details.Deliveries[0].ActualDeliveryDate + " " + StoreEmpowermentModel.details.Deliveries[0].ActualDeliveryTime, quantity = StoreEmpowermentModel.details.Deliveries[0].QuantityAsCasesAndSingles });

                ListView.Items.Add(new DeliverList() { delivery = "Next Delivery", date = ((StoreEmpowermentModel.details.Deliveries[1].ActualDeliveryDate) == "") ? (StoreEmpowermentModel.details.Deliveries[1].RequestDeliveryDate + " " + StoreEmpowermentModel.details.Deliveries[1].RequestDeliveryTime) : (StoreEmpowermentModel.details.Deliveries[1].ActualDeliveryDate + " " + StoreEmpowermentModel.details.Deliveries[1].ActualDeliveryTime), quantity = ((StoreEmpowermentModel.details.Deliveries[1].QuantityAsCasesAndSingles.Equals("0/000")) ? "" : StoreEmpowermentModel.details.Deliveries[1].QuantityAsCasesAndSingles) });
                Debug.WriteLine("check date" + StoreEmpowermentModel.details.Deliveries[1].ActualDeliveryDate + StoreEmpowermentModel.details.Deliveries[1].ActualDeliveryTime);
                Debug.WriteLine("check req date" + StoreEmpowermentModel.details.Deliveries[1].RequestDeliveryDate + StoreEmpowermentModel.details.Deliveries[1].RequestDeliveryTime);
            }
            if (StoreEmpowermentModel.details.Counting.LastGapScanDate != "")
            {
                gapScanBorder.Visibility = Visibility.Visible;
                gapPanel.Visibility = Visibility.Visible;
                gapScanListView.Items.Add(new CountingList() { gap = "Last Gap Scan", desc = StoreEmpowermentModel.details.Counting.LastGapScanDate + " " + StoreEmpowermentModel.details.Counting.LastGapScanTime });
                gapScanListView.Items.Add(new CountingList() { gap = "Gap Scan Status", desc = StoreEmpowermentModel.details.Counting.LastGapScanStatus });
            }
            if (StoreEmpowermentModel.details.Sales.Count != 0)
            {
                saleBorder.Visibility = Visibility.Visible;
                salePanel.Visibility = Visibility.Visible;
                Debug.WriteLine("sell by weight" + StoreEmpowermentModel.details.Product.SellByWeight);
                // Debug.WriteLine("after condition"+StoreEmpowermentModel.)
                if ((StoreEmpowermentModel.details.Product.SellByWeight.ToUpper().Equals("W")))
                {
                    saleListView.Header = new SaleList() { day = "Sales(" + StoreEmpowermentModel.details.BookStock.UOM + ")", expected = "Expected", actual = "Actual" };

                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[1].Type), expected = (StoreEmpowermentModel.details.Sales[1].Expected).ToString(), actual = (StoreEmpowermentModel.details.Sales[1].Actual).ToString() });
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[0].Type), expected = (StoreEmpowermentModel.details.Sales[0].Expected).ToString(), actual = (StoreEmpowermentModel.details.Sales[0].Actual).ToString() });
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[2].Type), expected = (StoreEmpowermentModel.details.Sales[2].Expected).ToString(), actual = "" });

                }
                else
                {
                    saleListView.Header = new SaleList() { day = "Sales (C/S)", expected = "Expected", actual = "Actual" };
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[1].Type), expected = StoreEmpowermentModel.details.Sales[1].ExpectedAsCasesAndSingles, actual = StoreEmpowermentModel.details.Sales[1].ActualAsCasesAndSingles });
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[0].Type), expected = StoreEmpowermentModel.details.Sales[0].ExpectedAsCasesAndSingles, actual = StoreEmpowermentModel.details.Sales[0].ActualAsCasesAndSingles });
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[2].Type), expected = StoreEmpowermentModel.details.Sales[2].ExpectedAsCasesAndSingles, actual = "" });

                }

            }
            if (StoreEmpowermentModel.details.calAccess.ToUpper().Equals("Y") || StoreEmpowermentModel.details.calAccess.ToUpper().Equals("N"))
            {
                Debug.WriteLine(StoreEmpowermentModel.details.calAccess.ToUpper());
            }
            else
            {
                WebViewControl.Visibility = Visibility.Collapsed;
            }
            int promotionCount = StoreEmpowermentModel.details.Offers.Count;
            if (promotionCount == 1)
            {
                promotionBorder.Visibility = Visibility.Visible;
                promo.Visibility = Visibility.Visible;
                //DateTime dt = DateTime.Today;
                promotionColorBlock.Text = StoreEmpowermentModel.details.Offers[0].Description;
                promotionListView.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[0].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[0].EndDate) });
                if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles != "")
                {
                    Debug.WriteLine("promotion value" + StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles);
                    //Debug.WriteLine("parsed" + Int32.Parse(StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles));
                    if ((StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles).Equals("0/000"))
                    {
                        if (dt < Convert.ToDateTime(StoreEmpowermentModel.details.Offers[0].StartDate))
                        {
                            promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                        }
                    }
                    else
                    {

                        promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                    }
                }
            }
            if (promotionCount == 2)
            {
                promotionBorder.Visibility = Visibility.Visible;
                promo.Visibility = Visibility.Visible;
                Debug.WriteLine("offerdate" + Convert.ToDateTime(StoreEmpowermentModel.details.Offers[0].StartDate));

                //DateTime dt = DateTime.Today;
                DateTime dt1 = DateTime.UtcNow;
                Debug.WriteLine(dt + " " + dt1);
                promotionColorBlock.Text = StoreEmpowermentModel.details.Offers[0].Description;
                promotionListView.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[0].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[0].EndDate) });
                if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles != "")
                {
                    Debug.WriteLine("promotion value" + StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles);
                    // promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                    if ((StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles).Equals("0/000"))
                    {
                        if (dt < Convert.ToDateTime(StoreEmpowermentModel.details.Offers[0].StartDate))
                        {
                            promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                        }
                    }
                    else
                    {
                        promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                    }
                }

                promotionBorder1.Visibility = Visibility.Visible;
                promo1.Visibility = Visibility.Visible;
                promotionColorBlock1.Text = StoreEmpowermentModel.details.Offers[1].Description;
                promotionListView1.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[1].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[1].EndDate) });
                if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles != "")
                {

                    Debug.WriteLine("promotion value" + StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles);
                    promotionListView1.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles });


                }
            }
            if (promotionCount == 3)
            {
                promotionBorder.Visibility = Visibility.Visible;
                promo.Visibility = Visibility.Visible;
                promotionColorBlock.Text = StoreEmpowermentModel.details.Offers[0].Description;
                promotionListView.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[0].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[0].EndDate) });
                if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles != "")
                {
                    Debug.WriteLine("promotion value" + StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles);
                    //promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                    if ((StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles).Equals("0/000"))
                    {
                        if (dt < Convert.ToDateTime(StoreEmpowermentModel.details.Offers[0].StartDate))
                        {
                            promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                        }
                    }
                    else
                    {
                        promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                    }
                }

                promotionBorder1.Visibility = Visibility.Visible;
                promo1.Visibility = Visibility.Visible;
                promotionColorBlock1.Text = StoreEmpowermentModel.details.Offers[1].Description;
                promotionListView1.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[1].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[1].EndDate) });
                if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles != "")
                {

                    promotionListView1.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles });


                }

                promotionBorder2.Visibility = Visibility.Visible;
                promo2.Visibility = Visibility.Visible;
                promotionColorBlock2.Text = StoreEmpowermentModel.details.Offers[2].Description;
                promotionListView2.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[2].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[2].EndDate) });
                if (StoreEmpowermentModel.details.Offers[2].OfferShelfCapacityAsCasesAndSingles != "")
                {

                    promotionListView2.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[2].OfferShelfCapacityAsCasesAndSingles });


                }
            }

            if (promotionCount == 4)
            {
                promotionBorder.Visibility = Visibility.Visible;
                promo.Visibility = Visibility.Visible;
                promotionColorBlock.Text = StoreEmpowermentModel.details.Offers[0].Description;
                promotionListView.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[0].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[0].EndDate) });
                if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles != "")
                {

                    //promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                    if ((StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles).Equals("0/000"))
                    {
                        if (dt < Convert.ToDateTime(StoreEmpowermentModel.details.Offers[0].StartDate))
                        {
                            promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                        }
                    }
                    else
                    {
                        promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacityAsCasesAndSingles });
                    }
                }

                promotionBorder1.Visibility = Visibility.Visible;
                promo1.Visibility = Visibility.Visible;
                promotionColorBlock1.Text = StoreEmpowermentModel.details.Offers[1].Description;
                promotionListView1.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[1].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[1].EndDate) });
                if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles != "")
                {

                    promotionListView1.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles });


                }

                promotionBorder2.Visibility = Visibility.Visible;
                promo2.Visibility = Visibility.Visible;
                promotionColorBlock2.Text = StoreEmpowermentModel.details.Offers[2].Description;
                promotionListView2.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[2].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[2].EndDate) });
                if (StoreEmpowermentModel.details.Offers[2].OfferShelfCapacityAsCasesAndSingles != "")
                {

                    promotionListView2.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[2].OfferShelfCapacityAsCasesAndSingles });


                }

                promotionBorder3.Visibility = Visibility.Visible;
                promo3.Visibility = Visibility.Visible;
                promotionColorBlock3.Text = StoreEmpowermentModel.details.Offers[3].Description;
                promotionListView3.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[3].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[3].EndDate) });
                if (StoreEmpowermentModel.details.Offers[3].OfferShelfCapacityAsCasesAndSingles != "")
                {

                    promotionListView3.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[3].OfferShelfCapacityAsCasesAndSingles });


                }
            }
            if (StoreEmpowermentModel.details.ProductLocation.Count < 1)
            {
                noLocationsDefinedCanvas.Visibility = Visibility.Visible;
                noLocationsDefined.Visibility = Visibility.Visible;
                blank4.Visibility = Visibility.Visible;
            }

            if (StoreEmpowermentModel.details.ProductLocation.Count != 0)
            {
                locationBorder.Visibility = Visibility.Visible;
                locationPanel.Visibility = Visibility.Visible;
                if (App.localSettings.Values["locationName"].Equals("friendly"))
                {

                    for (int i = 1; i <= StoreEmpowermentModel.details.ProductLocation.Count; i++)
                    {
                        locationListView.Items.Add(new LocationList() { index = i.ToString(), loc = GetLocation(i - 1) });
                    }
                }
                else
                {
                    for (int i = 1; i <= StoreEmpowermentModel.details.ProductLocation.Count; i++)
                    {
                        locationListView.Items.Add(new LocationList() { index = i.ToString(), loc = GetLocationSingles(i - 1) });
                    }

                }
            }

        }

        private void labelBindingSingles()
        {

            ClearList();
            DateTime dt = DateTime.Today;
            productName.Text = StoreEmpowermentModel.details.Product.Description;
            productId.Text = StoreEmpowermentModel.details.Product.TPNB + " \\ " + GetUnit((int)float.Parse(StoreEmpowermentModel.details.Product.UnitSize)) + "\n" + StoreEmpowermentModel.details.Product.EAN;
            if (StoreEmpowermentModel.details.PriceAPI.variants[0].currency.Equals("GBP"))
            {
                productPrice.Text = "\u00A3 " + StoreEmpowermentModel.details.PriceAPI.variants[0].authPrice;
            }
            else if (StoreEmpowermentModel.details.PriceAPI.variants[0].currency.Equals("EUR"))
            {
                productPrice.Text = "\u20AC" + StoreEmpowermentModel.details.PriceAPI.variants[0].authPrice;
            }
           
           // productPrice.Text = "\u00A3 " + convertDecimalToString(StoreEmpowermentModel.details.Price.SellingPrice);
            productImage.Source = new BitmapImage(new Uri(ImageSource()));

            if (StoreEmpowermentModel.details.Range.EndDate != "")
            {
                if (Convert.ToDateTime(StoreEmpowermentModel.details.Range.EndDate) != DateTime.MaxValue)
                {
                    stockedUntilCanvas.Visibility = Visibility.Visible;
                    String str = Convert.ToDateTime(StoreEmpowermentModel.details.Range.EndDate).ToString();
                    stockedUntil.Visibility = Visibility.Visible;
                    stockedUntil.Text = "Stocked Until" + " " + str.Substring(0, 2) + "/" + str.Substring(3, 2) + "/" + str.Substring(6, 4);

                    blank1.Visibility = Visibility.Visible;
                }
            }

            if (StoreEmpowermentModel.details.BookStock.Quantity < 0)
            {
                negativeStockCanvas.Visibility = Visibility.Visible;
                negativeStock.Visibility = Visibility.Visible;
                blank2.Visibility = Visibility.Visible;
            }
            else if (StoreEmpowermentModel.details.BookStock.Quantity == 0)
            {
                zeroStockCanvas.Visibility = Visibility.Visible;
                zeroStock.Visibility = Visibility.Visible;
                blank3.Visibility = Visibility.Visible;
            }
            else
            {
                Debug.WriteLine("in product overview bottom");
            }
            if ((convertStringToInt(StoreEmpowermentModel.details.ShelfCapacity.Facing) > 0) && (StoreEmpowermentModel.details.ShelfCapacity.Capacity < 1))
            {
                noShelfCapacityCanvas.Visibility = Visibility.Visible;
                noShelfCapacity.Visibility = Visibility.Visible;
            }

            if ((StoreEmpowermentModel.details.Product.SellByWeight.ToUpper().Equals("W")))
            {
                currentStockTextBlock.Text = "Current Stock (KG)";
            }
            else
            {
                currentStockTextBlock.Text = "Current Stock (Singles)";
            }
            //stock.Text = StoreEmpowermentModel.details.BookStock.QuantityInCasesAndSingles;
            if (StoreEmpowermentModel.details.BookStock.QuantityInCasesAndSingles != "")
            {
                currentStockBorder.Visibility = Visibility.Visible;
                currentPanel.Visibility = Visibility.Visible;
                currentStockListView.Items.Add(new StockList() { stock = StoreEmpowermentModel.details.BookStock.Quantity.ToString(), stockName = "Stock Record" });
                Debug.WriteLine("no of items is " + currentStockListView.Items.Count);
            }

            if (StoreEmpowermentModel.details.ShelfCapacity.CapacityAsCasesAndSingles != "")
            {
                shelfDetailsBorder.Visibility = Visibility.Visible;
                shelfPanel.Visibility = Visibility.Visible;
                ListView1.Items.Add(new ShelfList() { shelfName = "Standard", capacity = StoreEmpowermentModel.details.ShelfCapacity.Capacity.ToString(), facing = StoreEmpowermentModel.details.ShelfCapacity.Facing });
                if (StoreEmpowermentModel.details.ShelfCapacity.CapacityForOffer != 0)
                {
                    if (StoreEmpowermentModel.details.Offers.Count == 1)
                    {
                        if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString().Equals("0"))
                        {

                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString(), facing = "" });
                        }
                    }
                    else if (StoreEmpowermentModel.details.Offers.Count == 2)
                    {
                        if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString().Equals("0"))
                        {

                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString(), facing = "" });

                        }
                        if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacity.ToString().Equals("0"))
                        {

                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacity.ToString(), facing = "" });
                        }
                    }
                    else if (StoreEmpowermentModel.details.Offers.Count == 3)
                    {
                        if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString().Equals("0"))
                        {

                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString(), facing = "" });
                        }
                        if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacity.ToString().Equals("0"))
                        {

                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacity.ToString(), facing = "" });
                        }
                        if (StoreEmpowermentModel.details.Offers[2].OfferShelfCapacity.ToString().Equals("0"))
                        {

                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[2].OfferShelfCapacity.ToString(), facing = "" });
                        }

                    }
                    else if (StoreEmpowermentModel.details.Offers.Count == 4)
                    {
                        if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString().Equals("0"))
                        {

                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString(), facing = "" });

                        }
                        if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacity.ToString().Equals("0"))
                        {

                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacity.ToString(), facing = "" });
                        }
                        if (StoreEmpowermentModel.details.Offers[2].OfferShelfCapacity.ToString().Equals("0"))
                        {

                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[2].OfferShelfCapacity.ToString(), facing = "" });

                        }
                        if (StoreEmpowermentModel.details.Offers[3].OfferShelfCapacity.ToString().Equals("0"))
                        {

                        }
                        else
                        {
                            ListView1.Items.Add(new ShelfList() { shelfName = "Additional", capacity = StoreEmpowermentModel.details.Offers[3].OfferShelfCapacity.ToString(), facing = "" });
                        }
                    }
                    else
                    {
                        Debug.WriteLine("inside else of additional");
                    }

                }
                else
                {
                    Debug.WriteLine("inside outer else of additon");
                }

            }
            

            if (StoreEmpowermentModel.details.Deliveries.Count != 0)
            {
                deliveryBorder.Visibility = Visibility.Visible;
                deliveryPanel.Visibility = Visibility.Visible;
                if ((StoreEmpowermentModel.details.Product.SellByWeight.ToUpper().Equals("W")))
                {
                    ListView.Header = new DeliverList() { delivery = "Delivery (" + StoreEmpowermentModel.details.BookStock.UOM + ")", date = "Date", quantity = "Quantity" };
                }
                else
                {
                    ListView.Header = new DeliverList() { delivery = "Delivery (S)", date = "Date", quantity = "Quantity" };
                }
                ListView.Items.Add(new DeliverList() { delivery = "Last Delivery", date = StoreEmpowermentModel.details.Deliveries[0].ActualDeliveryDate + " " + StoreEmpowermentModel.details.Deliveries[0].ActualDeliveryTime, quantity = StoreEmpowermentModel.details.Deliveries[0].Quantity.ToString() });
                ListView.Items.Add(new DeliverList() { delivery = "Next Delivery", date = ((StoreEmpowermentModel.details.Deliveries[1].ActualDeliveryDate) == "") ? (StoreEmpowermentModel.details.Deliveries[1].RequestDeliveryDate + " " + StoreEmpowermentModel.details.Deliveries[1].RequestDeliveryTime) : (StoreEmpowermentModel.details.Deliveries[1].ActualDeliveryDate + " " + StoreEmpowermentModel.details.Deliveries[1].ActualDeliveryTime), quantity = ((StoreEmpowermentModel.details.Deliveries[1].Quantity.ToString().Equals("0")) ? "" : StoreEmpowermentModel.details.Deliveries[1].Quantity.ToString()) });

            }
            if (StoreEmpowermentModel.details.Counting.LastGapScanDate != "")
            {
                gapScanBorder.Visibility = Visibility.Visible;
                gapPanel.Visibility = Visibility.Visible;
                gapScanListView.Items.Add(new CountingList() { gap = "Last Gap Scan", desc = StoreEmpowermentModel.details.Counting.LastGapScanDate + " " + StoreEmpowermentModel.details.Counting.LastGapScanTime });
                gapScanListView.Items.Add(new CountingList() { gap = "Gap Scan Status", desc = StoreEmpowermentModel.details.Counting.LastGapScanStatus });
            }
            if (StoreEmpowermentModel.details.Sales.Count != 0)
            {
                //int expected1, expected1, expected1;
                //int actual1, actual2, actual3;
                saleBorder.Visibility = Visibility.Visible;
                salePanel.Visibility = Visibility.Visible;
                if ((StoreEmpowermentModel.details.Product.SellByWeight.ToUpper().Equals("W")))
                {
                    saleListView.Header = new SaleList() { day = "Sales (" + StoreEmpowermentModel.details.BookStock.UOM + ")", expected = "Expected", actual = "Actual" };
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[1].Type), expected = (StoreEmpowermentModel.details.Sales[1].Expected).ToString(), actual = (StoreEmpowermentModel.details.Sales[1].Actual).ToString() });
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[0].Type), expected = (StoreEmpowermentModel.details.Sales[0].Expected).ToString(), actual = (StoreEmpowermentModel.details.Sales[0].Actual).ToString() });
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[2].Type), expected = (StoreEmpowermentModel.details.Sales[2].Expected).ToString(), actual = "" });


                }
                else
                {
                    saleListView.Header = new SaleList() { day = "Sales (S)", expected = "Expected", actual = "Actual" };
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[1].Type), expected = convertToInt(StoreEmpowermentModel.details.Sales[1].Expected).ToString(), actual = convertToInt(StoreEmpowermentModel.details.Sales[1].Actual).ToString() });
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[0].Type), expected = convertToInt(StoreEmpowermentModel.details.Sales[0].Expected).ToString(), actual = convertToInt(StoreEmpowermentModel.details.Sales[0].Actual).ToString() });
                    saleListView.Items.Add(new SaleList() { day = GetDay(StoreEmpowermentModel.details.Sales[2].Type), expected = convertToInt(StoreEmpowermentModel.details.Sales[2].Expected).ToString(), actual = "" });

                }

            }
            if (StoreEmpowermentModel.details.calAccess.ToUpper().Equals("Y") || StoreEmpowermentModel.details.calAccess.ToUpper().Equals("N"))
            {
                WebViewControl.Visibility = Visibility.Visible;
            }
            else
            {
                WebViewControl.Visibility = Visibility.Collapsed;
            }
            int promotionCount = StoreEmpowermentModel.details.Offers.Count;
            if (promotionCount == 1)
            {
                promotionBorder.Visibility = Visibility.Visible;
                promo.Visibility = Visibility.Visible;
                promotionColorBlock.Text = StoreEmpowermentModel.details.Offers[0].Description;
                promotionListView.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[0].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[0].EndDate) });
                Debug.WriteLine(StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString());
                if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString().Equals("0"))
                {
                    if (dt < Convert.ToDateTime(StoreEmpowermentModel.details.Offers[0].StartDate))
                    {
                        promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString() });
                    }
                }
                else
                {
                    promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString() });
                }

            }
            if (promotionCount == 2)
            {
                promotionBorder.Visibility = Visibility.Visible;
                promo.Visibility = Visibility.Visible;
                promotionColorBlock.Text = StoreEmpowermentModel.details.Offers[0].Description;
                promotionListView.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[0].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[0].EndDate) });

                if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString().Equals("0"))
                {
                    if (dt < Convert.ToDateTime(StoreEmpowermentModel.details.Offers[0].StartDate))
                    {

                        promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString() });
                    }
                }
                else
                {
                    promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString() });
                }


                promotionBorder1.Visibility = Visibility.Visible;
                promo1.Visibility = Visibility.Visible;
                promotionColorBlock1.Text = StoreEmpowermentModel.details.Offers[1].Description;
                promotionListView1.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[1].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[1].EndDate) });
                if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles != "")
                {
                    Debug.WriteLine("promotion value" + StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString());
                    promotionListView1.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacity.ToString() });
                }
            }
            if (promotionCount == 3)
            {
                promotionBorder.Visibility = Visibility.Visible;
                promo.Visibility = Visibility.Visible;
                promotionColorBlock.Text = StoreEmpowermentModel.details.Offers[0].Description;
                promotionListView.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[0].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[0].EndDate) });

                if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString().Equals("0"))
                {
                    if (dt < Convert.ToDateTime(StoreEmpowermentModel.details.Offers[0].StartDate))
                    {
                        promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString() });
                    }
                }
                else
                {
                    promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString() });
                }

                promotionBorder1.Visibility = Visibility.Visible;
                promo1.Visibility = Visibility.Visible;
                promotionColorBlock1.Text = StoreEmpowermentModel.details.Offers[1].Description;
                promotionListView1.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[1].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[1].EndDate) });
                if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles != "")
                {
                    promotionListView1.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacity.ToString() });
                }

                promotionBorder2.Visibility = Visibility.Visible;
                promo2.Visibility = Visibility.Visible;
                promotionColorBlock2.Text = StoreEmpowermentModel.details.Offers[2].Description;
                promotionListView2.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[2].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[2].EndDate) });
                if (StoreEmpowermentModel.details.Offers[2].OfferShelfCapacityAsCasesAndSingles != "")
                {
                    promotionListView2.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[2].OfferShelfCapacity.ToString() });
                }
            }

            if (promotionCount == 4)
            {
                promotionBorder.Visibility = Visibility.Visible;
                promo.Visibility = Visibility.Visible;
                promotionColorBlock.Text = StoreEmpowermentModel.details.Offers[0].Description;
                promotionListView.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[0].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[0].EndDate) });

                if (StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString().Equals("0"))
                {
                    if (dt < Convert.ToDateTime(StoreEmpowermentModel.details.Offers[0].StartDate))
                    {
                        promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString() });
                    }
                }
                else
                {
                    promotionListView.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[0].OfferShelfCapacity.ToString() });
                }

                promotionBorder1.Visibility = Visibility.Visible;
                promo1.Visibility = Visibility.Visible;
                promotionColorBlock1.Text = StoreEmpowermentModel.details.Offers[1].Description;
                promotionListView1.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[1].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[1].EndDate) });
                if (StoreEmpowermentModel.details.Offers[1].OfferShelfCapacityAsCasesAndSingles != "")
                {
                    promotionListView1.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[1].OfferShelfCapacity.ToString() });
                }

                promotionBorder2.Visibility = Visibility.Visible;
                promo2.Visibility = Visibility.Visible;
                promotionColorBlock2.Text = StoreEmpowermentModel.details.Offers[2].Description;
                promotionListView2.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[2].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[2].EndDate) });
                if (StoreEmpowermentModel.details.Offers[2].OfferShelfCapacityAsCasesAndSingles != "")
                {
                    promotionListView2.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[2].OfferShelfCapacity.ToString() });
                }

                promotionBorder3.Visibility = Visibility.Visible;
                promo3.Visibility = Visibility.Visible;
                promotionColorBlock3.Text = StoreEmpowermentModel.details.Offers[3].Description;
                promotionListView3.Items.Add(new PromoList() { column1 = "Dates", column2 = converter(StoreEmpowermentModel.details.Offers[3].StartDate) + " to " + converter(StoreEmpowermentModel.details.Offers[3].EndDate) });
                if (StoreEmpowermentModel.details.Offers[3].OfferShelfCapacityAsCasesAndSingles != "")
                {
                    promotionListView3.Items.Add(new PromoList() { column1 = "Shelf Capacity For Promotion", column2 = StoreEmpowermentModel.details.Offers[3].OfferShelfCapacity.ToString() });
                }
            }
            if (StoreEmpowermentModel.details.ProductLocation.Count != 0)
            {
                locationBorder.Visibility = Visibility.Visible;
                locationPanel.Visibility = Visibility.Visible;
                if (App.localSettings.Values["locationName"].Equals("friendly"))
                {

                    for (int i = 1; i <= StoreEmpowermentModel.details.ProductLocation.Count; i++)
                    {
                        locationListView.Items.Add(new LocationList() { index = i.ToString(), loc = GetLocation(i - 1) });
                    }
                }
                else
                {
                    for (int i = 1; i <= StoreEmpowermentModel.details.ProductLocation.Count; i++)
                    {
                        locationListView.Items.Add(new LocationList() { index = i.ToString(), loc = GetLocationSingles(i - 1) });
                    }

                }
            }
           
            if (StoreEmpowermentModel.details.ProductLocation.Count < 1)
            {
                noLocationsDefinedCanvas.Visibility = Visibility.Visible;
                noLocationsDefined.Visibility = Visibility.Visible;
                blank4.Visibility = Visibility.Visible;
            }
            
        }
        private string ImageSource()
        {
            string source = "http://img.tesco.com/Groceries/pi/";
            string ean = StoreEmpowermentModel.details.Product.EAN;
            int count = ean.Count();
            source = source + ean[count - 3] + ean[count - 2] + ean[count - 1] + "/" + ean + "/IDShot_90x90.jpg";
            Debug.WriteLine(source);
            return source;
        }

        public double convertToInt(double value)
        {
            return Math.Round(value);
        }


        private string GetDay(string inp)
        {
            if (inp.Contains("Tod"))
                return "Today";
            else if (inp.Contains("Tom"))
                return "Tomorrow";
            else return "Yesterday";
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically  used to configure the page.</param>
        /// 
        public int convert(String s)
        {
            return Int32.Parse(s);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            HideList();
            ClearBorder();
            TitleBar.UpdateLayout();
            //ListView.Header = 
            Debug.WriteLine("went inside cases and singles :" + App.localSettings.Values["mode"]);
            try
            {
                if ((string)App.localSettings.Values["mode"] == "cs")
                    labelBinding();
                else
                    labelBindingSingles();
            }
            catch (Exception ex)
            {

            }
            /*StackPanel spanel = new StackPanel();
            spanel = promo;
            spanel.Name = "promo1";*/
            //outer.Children.Add(spanel);
            try
            {

                Double widthScreen = Window.Current.Bounds.Width;
                Canvas.SetLeft(searchIcon, 0.8 * widthScreen);
                Canvas.SetLeft(backImage, .12 * widthScreen);
                Canvas.SetLeft(scanIcon, 0.9 * widthScreen);
                Canvas.SetLeft(myOrders1, (widthScreen - myOrders1.Width) / 2);
                DrawerLayout.Visibility = Visibility.Collapsed;
                menuName.Text = (string)App.localSettings.Values["storename"];
                //popupBox popup2 = new popupBox("length : "+StoreEmpowermentModel.productHtml.Length);
                //await popup2.ShowDialog();
                //Debug.WriteLine(StoreEmpowermentModel.productHtml);
                //WebViewControl.NavigateToString(StoreEmpowermentModel.productHtml);
                //WebViewControl.NavigateToString("");
                ///WebViewControl.Navigate(new Uri(url));
                Debug.WriteLine("doing");
                WebViewControl.Navigate(HomeUri);
                promotioncolorCanvas.Width = .94 * widthScreen;
                promotionColorBlock1.Width = .94 * widthScreen;
                promotionColorBlock2.Width = .94 * widthScreen;
                promotionColorBlock3.Width = .94 * widthScreen;
                // Canvas.SetLeft(promotionColorBlock, .01 * widthScreen);
                //Canvas.SetLeft(promotionColorBlock1, .01 * widthScreen);
                //Canvas.SetLeft(promotionColorBlock2, .01 * widthScreen);
                //Canvas.SetLeft(promotionColorBlock3, .01 * widthScreen);
                //Canvas.SetLeft(productName, 1 * widthScreen);
                //Canvas.SetLeft(productPrice, 1 * widthScreen);
                //Canvas.SetLeft(productId, 1 * widthScreen);
                // Canvas.SetLeft(currentStockTextBlock,)



                //WebViewControlCalendar.Navigate(HomeUri);
                //Debug.WriteLine("checkthis : "+StoreEmpowermentModel.htmlResponse);
            }
            catch (Exception error)
            {
                //Crittercism.LogHandledException(error);
                //popupBox popup1 = new popupBox(el.ToString());
                //popup1.ShowDialog();
            }
            UIArrange();
            Debug.WriteLine("finished");
        }

        private void HideList()
        {
            stockedUntilCanvas.Visibility = Visibility.Collapsed;
            noLocationsDefinedCanvas.Visibility = Visibility.Collapsed;
            zeroStockCanvas.Visibility = Visibility.Collapsed;
            noShelfCapacityCanvas.Visibility = Visibility.Collapsed;
            negativeStockCanvas.Visibility = Visibility.Collapsed;
            promo.Visibility = Visibility.Collapsed;
            promo1.Visibility = Visibility.Collapsed;
            promo2.Visibility = Visibility.Collapsed;
            promo3.Visibility = Visibility.Collapsed;
            salePanel.Visibility = Visibility.Collapsed;
            gapPanel.Visibility = Visibility.Collapsed;
            deliveryPanel.Visibility = Visibility.Collapsed;
            shelfPanel.Visibility = Visibility.Collapsed;
        }
        private void UIArrange()
        {
            //productDetail.Width = width;
            //productDetail.Height = .07 * width;

            //setting text in center
            //Double textWidth = productDetailTextBlock.ActualWidth;

            //Double Center = ((width * 0.35) - (textWidth * 0.5));
            //Canvas.SetLeft(productDetailTextBlock, Center);
            Thickness margin = new Thickness();
            margin = productImage.Margin;
            margin.Left = width - 290;
            productImage.Margin = margin;

            //Arranging border
            borderArrange(productOverviewBoorder);
            borderArrange(currentStockBorder);
            borderArrange(promotionBorder);
            borderArrange(promotionBorder1);
            borderArrange(promotionBorder2);
            borderArrange(promotionBorder3);
            borderArrange(locationBorder);
            borderArrange(gapScanBorder);
            borderArrange(deliveryBorder);
            borderArrange(shelfDetailsBorder);
            borderArrange(saleBorder);
            //Border Postioning
            borderPosition(productOverviewBoorder);
            borderPosition(currentStockBorder);
            borderPosition(promotionBorder);
            borderPosition(promotionBorder1);
            borderPosition(promotionBorder2);
            borderPosition(promotionBorder3);
            borderPosition(locationBorder);
            borderPosition(gapScanBorder);
            borderPosition(saleBorder);
            Canvas.SetTop(productOverviewBoorder, .1 * width);
            Canvas.SetTop(productOverviewCanvas, 0);
            //Arranging canvas
            canvasArrange(productOverviewCanvas);
            canvasArrange(currentStock);
            canvasArrange(promotionCanvas);
            canvasArrange(promotionCanvas1);
            canvasArrange(promotionCanvas2);
            canvasArrange(promotionCanvas3);
            //  canvasArrange(promotioncolorCanvas);
            // canvasArrange(promotioncolorCanvas1);
            //canvasArrange(On2);
            //canvasArrange(promotioncolorCanvas3);
            canvasArrange(locationCanvas);
            canvasArrange(gapScanCanvas);
            Canvas.SetLeft(productImage, 0.7 * width);
            productImage.Width = .3 * width;
            productImage.Height = .2 * width;
            //Canvas.SetLeft(productImage, 100);
            //Canvas.SetTop(productImage, 20);
            // textAlignment(productDetail, productDetailTextBlock);

            //Margin setter
            /*marginSetter(productOverviewBoorder, 30);
            marginSetter(currentStockBorder, 250);
            marginSetter(promotionBorder, 475);
            marginSetter(deliveryBorder, 625);
            marginSetter(shelfDetailsBorder, 770);
            marginSetter(locationBorder, 830);
            marginSetter(gapScanBorder, 890);*/
            Debug.WriteLine("width is" + width);
        }
        private void ClearList()
        {
            currentStockListView.Items.Clear();
            ListView.Items.Clear();
            ListView1.Items.Clear();
            gapScanListView.Items.Clear();
            locationListView.Items.Clear();
            promotionListView.Items.Clear();
            promotionListView1.Items.Clear();
            promotionListView2.Items.Clear();
            promotionListView3.Items.Clear();
            saleListView.Items.Clear();
        }
        private void ClearBorder()
        {
            currentStockBorder.Visibility = Visibility.Collapsed;
            promotionBorder.Visibility = Visibility.Collapsed;
            promotionBorder1.Visibility = Visibility.Collapsed;
            promotionBorder2.Visibility = Visibility.Collapsed;
            promotionBorder3.Visibility = Visibility.Collapsed;
            deliveryBorder.Visibility = Visibility.Collapsed;
            shelfDetailsBorder.Visibility = Visibility.Collapsed;
            locationBorder.Visibility = Visibility.Collapsed;
            saleBorder.Visibility = Visibility.Collapsed;
            gapScanBorder.Visibility = Visibility.Collapsed;
        }
        private void logoutAction(object sender, RoutedEventArgs e)
        {

            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();

            lhmPresenter.Logout();

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

            lhmPresenter.ChangeStore();

        }
        private void teamAccess(object sender, TappedRoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            lhmPresenter.teamAccess();

        }

        private void about(object sender, RoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            lhmPresenter.about();
        }

        private void settings(object sender, RoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            lhmPresenter.settings();
        }
        private async void searchIconClick(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Search icon - Headerbar", 0);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
        }

        private async void backImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Back icon - Headerbar", 0);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
        }


    }
}
