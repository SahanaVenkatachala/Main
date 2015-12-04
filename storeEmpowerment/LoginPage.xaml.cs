using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System;
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
    public sealed partial class LoginPage : Page
    {
        public static LoginPage pagePoint;
        public static LoginResponse loginres;
        public static ProgressRing pring;
        public static int domainFlag;
        public LoginPage()
        {
            this.InitializeComponent();
            GoogleAnalytics.EasyTracker.GetTracker().SendView("Login Page");
            loginres = new LoginResponse();
            pagePoint = this;
        }

        private void imageMargin()
        {
            Double heightScreen = Window.Current.Bounds.Height;
            Thickness margin;
            margin = empowimg.Margin;
            margin.Top = heightScreen * 0.16125;
            empowimg.Margin = margin;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(storeRoutine));
        }

        private void changeButtonMargins(Thickness margin, Control button)
        {
            Double widthScreen = Window.Current.Bounds.Width;
            var width = widthScreen * 0.125;
            margin.Left = width;
            margin.Right = width;
            button.Margin = margin;
        }



        private void changeArrowPosition(Thickness margin, Image image)
        {
            Double widthScreen = Window.Current.Bounds.Width;
            var width = widthScreen * 0.15;
            margin.Right = width;
            image.Margin = margin;
        }

        //private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            App.localSettings.Values["currentPage"] = "login";
            imageMargin();
            changeArrowPosition(downArrow.Margin, downArrow);
            pring = ring;
            changeButtonMargins(signIn.Margin, signIn);
            changeButtonMargins(signInAbove.Margin, signInAbove);
            changeButtonMargins(domain.Margin, domain);
            changeButtonMargins(userName.Margin, userName);
            changeButtonMargins(passWord.Margin, passWord);
            if (App.localSettings.Values["domain"] != null)
            {
                string domainSelected = (string)App.localSettings.Values["domain"];
                int index = 0;
                if (domainSelected == "TSL")
                {
                    index = 1;
                }
                if (domainSelected == "Tescoglobal")
                {
                    index = 2;
                }
                if (domainSelected == "UKROI")
                {
                    index = 3;
                }
                domain.SelectedIndex = index;
            }
            else
            {
                domain.SelectedIndex = 0;
            }
            signInAbove.Visibility = Visibility.Collapsed;
            Debug.WriteLine("inside navigation loginnn" + App.localSettings.Values["newToken"]);


        }

        private async void password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                validation();
            }
        }

        private async void validation()
        {
            string domainselected = ((ComboBoxItem)domain.SelectedItem).Content.ToString();
            if (domainselected != "Domain" && domainFlag == 1)
            {
                App.localSettings.Values["domain"] = domainselected;
                App.localSettings.Values["username"] = userName.Text;
            }
            if (userName.Text.Length != 0 && passWord.Password.Length != 0 && ((domainFlag == 1 && domainselected != "Domain") || domainFlag == 0))
            {
                this.IsEnabled = false;
                ring.IsActive = true;
                NetworkManager manager = NetworkManager.Instance;
                Task loginResponseTask = manager.loginRequestAsync(userName.Text, passWord.Password, domainselected);
                await loginResponseTask;

            }
            else
            {
                if (userName.Text.Length == 0 || passWord.Password.Length == 0)
                {
                    popupBox popup = new popupBox("Please Enter Username/Password");
                    await popup.ShowDialog();
                }
                else if (domainselected == "Domain")
                {
                    if (domainFlag == 1)
                    {
                        popupBox popup = new popupBox("Please Choose Your Domain");
                        await popup.ShowDialog();

                    }
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Button", "signin", null, 0);
            validation();

        }

        private void usernameListener(object sender, TextChangedEventArgs e)
        {
            if (userName.Text.Contains('@'))
            {
                domain.Visibility = Visibility.Collapsed;
                domainFlag = 0;
                signIn.Visibility = Visibility.Collapsed;
                signInAbove.Visibility = Visibility.Visible;
                downArrow.Visibility = Visibility.Collapsed;
            }
            else
            {
                signInAbove.Visibility = Visibility.Collapsed;
                domain.Visibility = Visibility.Visible;
                domainFlag = 1;
                downArrow.Visibility = Visibility.Visible;
                signIn.Visibility = Visibility.Visible;
            }

        }

        private void userName_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                passWord.Focus(FocusState.Keyboard);
            }
        }

    }
}