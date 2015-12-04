using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
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
    public sealed partial class searchPage : Page, ISearchPage
    {

        public static SearchPresenter presenter = new SearchPresenter();
        public searchPage()
        {
            presenter.add(this);
            this.InitializeComponent();
            //GoogleAnalytics.EasyTracker.GetTracker().SendView("MainPage");

        }

        public async void NavigateToHomePage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
        }
        public async void NavigateToOverview()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(ProductOverview)); });
        }

        public void ShowRing(){
            ring.IsActive = true;
        }
        public void HideRing()
        {
            ring.IsActive = false;
        }
        /* private void buttonPosition(Control button, Thickness margin)
         {
             Double widthScreen = Window.Current.Bounds.Width;
             margin.Left = 0.7 * Width;
             button.Margin = margin;
            

         }*/


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            //searchText.Focus(Windows.UI.Xaml.FocusState.Keyboard);
               
            App.localSettings.Values["currentPage"] = "searchPg";
            var width = Window.Current.Bounds.Width;
            Debug.WriteLine(width);
            if (width > 410)
            {
                canvas1.Width = 0.2 * width;
                cancel.Width = 0.2 * width;
                Canvas.SetLeft(cancel, 20);
            }
            searchText.Width = width * 0.75;
            //canvas1.Width = 0.20 * width;
            //cancel.Width = canvas1.Width;
            //Canvas.SetLeft(searchText, 0.025 * width);
            Canvas.SetLeft(SearchIcon, (0.73 * width) + 8 - (0.07 * width));
            // Canvas.SetLeft(Cancel, 0.75 * width);
            Canvas.SetLeft(canvas1, 0.79 * width);
        }

      

        private async void cancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
        }

        private async void searchText_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(searchText.Text==null)
            {
                Debug.WriteLine("");
            }
            
                if (e.Key == VirtualKey.Enter)
                {
                    ShowRing();
                    double output;
                    NetworkManager manager = NetworkManager.Instance;

                    if (Double.TryParse(searchText.Text, out output))
                    {
                        Debug.WriteLine("nos");
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
            else if(e.Key.Equals(" "))
                {
                    NavigateToOverview();
                }
            }
           
        }





       
    }

