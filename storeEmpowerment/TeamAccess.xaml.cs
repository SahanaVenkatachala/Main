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
    public sealed partial class TeamAccess : Page, ILHMView
    {
        public string calResponse;
        public static TeamAccess team;
        double screenWidth = Window.Current.Bounds.Width;
        public int tappedCount;
        private int cancelCount;
        private int minusCount;
        private static int firstTimeFlag = 1;
        public static ProgressRing pring;
        public static int failureFlag;
        public static TeamAccess teamAccessptr;
        public static List<Delegatee0> delegateeList;
        private List<string> names;
        private List<string> states;
        private LHMPresenter lhmPresenter = new LHMPresenter();
        public TeamAccess()
        {
            this.InitializeComponent();
            GoogleAnalytics.EasyTracker.GetTracker().SendView("TeamAccess Page");
            DrawerLayout.InitializeDrawerLayout();
            #region Initialize

            states = new List<string>();
            names = (List<string>)App.localSettings.Values["names"];
            states = (List<string>)App.localSettings.Values["states"];

            team = this;
            #endregion
            lhmPresenter.Add(this);



        }



        #region LHM Functions
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

            lhmPresenter.Logout();

        }
        private void about(object sender, RoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();

            lhmPresenter.about();

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

        private void settings(object sender, RoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();

            lhmPresenter.settings();

        }
        private void teamAccess(object sender, RoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            if (p_ring.IsActive == false)
            {
                lhmPresenter.teamAccess();

            }

        }
        private void teamAccessPage(object sender, TappedRoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            if (p_ring.IsActive == false)
            {
                lhmPresenter.teamAccess();

            }

        }
        private async void Scan_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Scan icon - Headerbar", 0);
            App.localSettings.Values["barcode"] = "false";
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(MainPage)); });
        }
        #endregion


        #region Navigation Functions
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

        private async void home_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Home icon - Headerbar", 0);
            App.localSettings.Values["tab"] = "recentItems";

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });

        }

        #endregion


        private async void searchIconClick(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Icon", "Click", "Search icon - Headerbar", 0);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(productSearch)); });
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //TeamAccessDelete.Navigate(HomeUri);
            #region UI Arrange

            var teamAccessTextBlockWidth = headerText.ActualWidth;
            var center = (screenWidth * .35) - (teamAccessTextBlockWidth * .5);
            Canvas.SetLeft(headerText, center);
            Canvas.SetLeft(secondaryText, .02 * screenWidth);
            Canvas.SetLeft(messageStore, .4 * screenWidth);
            Canvas.SetLeft(minusImage, .88 * screenWidth);
            Canvas.SetLeft(addImage, .77 * screenWidth);
            Canvas.SetLeft(searchIcon, .8 * screenWidth);
            Canvas.SetLeft(scanIcon, .9 * screenWidth);
            Canvas.SetLeft(homeIcon, .1 * screenWidth);
            messageStore.Width = 350;
            messageStore.Height = 100;
            menuName.Text = (string)App.localSettings.Values["storename"];
            pring = p_ring;

            //hiding datalist grid
            hide(firstNameTextBox, firstToggle, firstRowCancelButton);
            hide(secondNameTextBox, secondToggle, secondRowCancelButton);
            hide(thirdNameTextBox, thirdToggle, thirdRowCancelButton);
            hide(fourthNameTextBox, fourthToggle, fourthRowCancelButton);

            //Making server call to get list
            p_ring.IsActive = true;
            disableUI();
            ResponseHandler.refreshFlag = 11;
            NetworkManager manager = NetworkManager.Instance;
            Task GetTeamAccessResponse = manager.TeamAccessAsyncGetList();
            await GetTeamAccessResponse;

            Debug.WriteLine("failure flag is " + failureFlag);
            if (failureFlag == 1)
            {
                p_ring.IsActive = false;
                DisplayList();
                failureFlag = 0;
                firstTimeFlag = 0;
            }
            try
            {
                if (delegateeList.Count == 0)
                {
                    dataList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    dataList.Visibility = Visibility.Visible;
                }

            }
            catch (Exception)
            {

            }



            Debug.WriteLine("failure flag is " + failureFlag);
            if (StoreEmpowermentModel.listResponse.storeManager == false)
            {
                
                addImage.Visibility = Visibility.Collapsed;
                minusImage.Visibility = Visibility.Collapsed;
                disableUI();

            }
            if (StoreEmpowermentModel.listResponse.storeManager == true)
            {
                Debug.WriteLine(StoreEmpowermentModel.listResponse.storeManagerName);
                reenableUI();
            }

            #endregion

            //reenableUI();
        }

        //hide function
        private void hide(TextBlock textblock1, ToggleSwitch toggle, Image img)
        {
            textblock1.Visibility = Visibility.Collapsed;
            toggle.Visibility = Visibility.Collapsed;
            img.Visibility = Visibility.Collapsed;
        }

        //visibility function
        private void visible(TextBlock textblock1, ToggleSwitch toggle)
        {
            textblock1.Visibility = Visibility.Visible;
            //toggle.IsOn = true;
            toggle.Visibility = Visibility.Visible;


        }
        //clicking on add image
        private async void addImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            tappedCount = delegateeListCount();
            if (tappedCount < 4)
            {
                Debug.WriteLine("inside addImage_tapped");
                secondaryAccessHeader.Opacity = .45;
                headerCanvas.Opacity = .45;
                Text.Opacity = .45;
                dataList.Visibility = Visibility.Collapsed;
                popUpGrid.Visibility = Visibility.Visible;
                minusImage.Visibility = Visibility.Visible;
            }
            else
            {
                popupBox popup = new popupBox("No more users can be added");
                await popup.ShowDialog();

            }
        }

        //Cancel button action on pop-up while adding delegatee
        public void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Text.Opacity = 1;
            employeeIdTextBox.Text = "";
            secondaryAccessHeader.Opacity = 1;
            headerCanvas.Opacity = 1;
            Debug.WriteLine("on click of cancel button" + cancelCount);
            tappedCount = delegateeListCount();
            if (tappedCount == 0)
            {
                Debug.WriteLine("on click of cancel button first time" + cancelCount);

                dataList.Visibility = Visibility.Collapsed;
                popUpGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                Debug.WriteLine("on click of cancel button next time" + cancelCount);

                dataList.Visibility = Visibility.Visible;
                popUpGrid.Visibility = Visibility.Collapsed;
            }


        }

        protected async void messageBox(string msg)
        {
            var msgDlg = new Windows.UI.Popups.MessageDialog(msg);
            await msgDlg.ShowAsync();
        }

        //Adding delegatee to list
        private async void addButton_Click(object sender, RoutedEventArgs e)
        {

            Debug.WriteLine("tapped count : " + tappedCount);
            Text.Opacity = 1;
            secondaryAccessHeader.Opacity = 1;
            headerCanvas.Opacity = 1;
            popUpGrid.Visibility = Visibility.Collapsed;
            dataList.Visibility = Visibility.Visible;
            if (string.IsNullOrWhiteSpace(employeeIdTextBox.Text))
            {
                Debug.WriteLine("tappedCount after =" + tappedCount);
                tappedCount = delegateeListCount();
                Debug.WriteLine("inside try block");
                if (tappedCount == 0)
                {
                    messageBox("Blank spaces are not allowed");
                    dataList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    dataList.Visibility = Visibility.Visible;
                    popupBox popup = new popupBox("Blank spaces are not allowed");
                    await popup.ShowDialog();
                }
            }
            else
            {
                Debug.WriteLine(employeeIdTextBox.Text);
                p_ring.IsActive = true;
                disableUI();
                Debug.WriteLine("inside the call");
                NetworkManager manager = NetworkManager.Instance;
                Task GetNameRequest = manager.GetNameAsync(employeeIdTextBox.Text);
                await GetNameRequest;
                tappedCount = delegateeListCount();
                Debug.WriteLine("***********************************" + tappedCount + "*************************************");
                if (tappedCount == 1)
                {
                    try
                    {
                        firstTimeFlag = 2;
                        firstNameTextBox.Text = employeeIdTextBox.Text;
                        visible(firstNameTextBox, firstToggle);
                        Debug.WriteLine("" + firstTimeFlag);
                        Debug.WriteLine(firstTimeFlag);
                        firstToggle.IsOn = true;
                        firstTimeFlag = 0;

                    }
                    catch (Exception e1)
                    {

                    }

                }


                else if (tappedCount == 2)
                {
                    try
                    {
                        firstTimeFlag = 2;
                        secondNameTextBox.Text = employeeIdTextBox.Text;
                        visible(secondNameTextBox, secondToggle);
                        Debug.WriteLine(firstTimeFlag);

                        Debug.WriteLine(firstTimeFlag);
                        secondToggle.IsOn = true;
                        firstTimeFlag = 0;

                    }
                    catch (Exception e2)
                    {

                    }

                }

                else if (tappedCount == 3)
                {
                    try
                    {
                        firstTimeFlag = 2;
                        Debug.WriteLine(firstTimeFlag);
                        thirdNameTextBox.Text = employeeIdTextBox.Text;
                        visible(thirdNameTextBox, thirdToggle);
                        Debug.WriteLine(firstTimeFlag);
                        Debug.WriteLine(firstTimeFlag);
                        thirdToggle.IsOn = true;
                        firstTimeFlag = 0;


                    }
                    catch (Exception e3)
                    {

                    }
                }

                else if (tappedCount == 4)
                {
                    try
                    {
                        firstTimeFlag = 2;
                        Debug.WriteLine(firstTimeFlag);
                        fourthNameTextBox.Text = employeeIdTextBox.Text;
                        visible(fourthNameTextBox, fourthToggle);
                        Debug.WriteLine(firstTimeFlag);
                        Debug.WriteLine(firstTimeFlag);
                        fourthToggle.IsOn = true;
                        firstTimeFlag = 0;

                    }
                    catch (Exception e4)
                    {

                    }
                }

                employeeIdTextBox.Text = "";
                ++cancelCount;
                reenableUI();


                Debug.WriteLine("inside adbutton_clicked");

                popUpGrid.Visibility = Visibility.Collapsed;
                dataList.Visibility = Visibility.Visible;

            }

        }


        //On click of minus image on datalist
        private void minusImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ++minusCount;
            if (minusCount == 1)
            {
                Debug.WriteLine(minusCount);
                try
                {
                    if (!string.IsNullOrWhiteSpace(firstNameTextBox.Text))
                    {
                        firstRowCancelButton.Visibility = Visibility.Visible;

                    }
                    if (!string.IsNullOrWhiteSpace(secondNameTextBox.Text))
                    {
                        secondRowCancelButton.Visibility = Visibility.Visible;

                    } if (!string.IsNullOrWhiteSpace(thirdNameTextBox.Text))
                    {
                        thirdRowCancelButton.Visibility = Visibility.Visible;

                    } if (!string.IsNullOrWhiteSpace(fourthNameTextBox.Text))
                    {
                        fourthRowCancelButton.Visibility = Visibility.Visible;

                    }


                }
                catch (Exception ex)
                {

                }

            }

            cancelIconClicked();
            Debug.WriteLine(minusCount);
            minusCount = 0;
            Debug.WriteLine(minusCount);
        }



        //Delete delegatee action
        private async void firstRowCancelButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //server call to delete delegatee
            p_ring.IsActive = true;
            disableUI();
            NetworkManager manager = NetworkManager.Instance;
            Task GetCancelAction = manager.GetCancelRequestAsync(App.localSettings.Values["stores"].ToString(), delegateeList[0].user);
            await GetCancelAction;

            if (firstRowCancelButton.IsTapEnabled)
            {
                Debug.WriteLine("first button clicked");
                try
                {
                    tappedCount = delegateeListCount() - 1;

                    Debug.WriteLine("tapped count************************" + tappedCount);
                    if (tappedCount == 3)
                    {
                        firstNameTextBox.Text = delegateeList[1].user;
                        delegateeList[0].access = delegateeList[1].access;
                        if (delegateeList[0].access.Equals("y"))
                        {
                            firstTimeFlag = 1;
                            firstToggle.IsOn = true;
                        }
                        Debug.WriteLine(firstNameTextBox.Text);
                        secondNameTextBox.Text = delegateeList[2].user;
                        delegateeList[1].access = delegateeList[2].access;
                        if (delegateeList[1].access.Equals("y"))
                        {
                            firstTimeFlag = 1;
                            secondToggle.IsOn = true;
                        } Debug.WriteLine(secondNameTextBox.Text);
                        thirdNameTextBox.Text = delegateeList[3].user;
                        delegateeList[2].access = delegateeList[3].access;
                        Debug.WriteLine(thirdNameTextBox.Text);
                        if (delegateeList[2].access.Equals("y"))
                        {
                            firstTimeFlag = 1;
                            thirdToggle.IsOn = true;
                        } fourthNameTextBox.Text = " ";
                        hide(fourthNameTextBox, fourthToggle, fourthRowCancelButton);


                    }
                    else if (tappedCount == 2)
                    {
                        firstNameTextBox.Text = delegateeList[1].user;
                        delegateeList[0].access = delegateeList[1].access;
                        if (delegateeList[0].access.Equals("y"))
                        {
                            firstTimeFlag = 1;
                            firstToggle.IsOn = true;
                        }
                        Debug.WriteLine(firstNameTextBox.Text);
                        secondNameTextBox.Text = delegateeList[2].user;
                        delegateeList[1].access = delegateeList[2].access;
                        if (delegateeList[1].access.Equals("y"))
                        {
                            firstTimeFlag = 1;
                            secondToggle.IsOn = true;
                        }
                        Debug.WriteLine(secondNameTextBox.Text);
                        thirdNameTextBox.Text = "";
                        Debug.WriteLine(thirdNameTextBox.Text);
                        fourthNameTextBox.Text = " ";
                        hide(thirdNameTextBox, thirdToggle, thirdRowCancelButton);


                    }
                    else if (tappedCount == 1)
                    {
                        firstNameTextBox.Text = delegateeList[1].user;
                        delegateeList[0].access = delegateeList[1].access;
                        Debug.WriteLine(delegateeList[0].access);
                        if (delegateeList[0].access.Equals("y"))
                        {
                            firstTimeFlag = 1;
                            firstToggle.IsOn = true;
                        }
                        Debug.WriteLine(firstNameTextBox.Text);
                        secondNameTextBox.Text = "";
                        Debug.WriteLine(secondNameTextBox.Text);
                        thirdNameTextBox.Text = "";
                        Debug.WriteLine(thirdNameTextBox.Text);
                        fourthNameTextBox.Text = " ";
                        hide(secondNameTextBox, secondToggle, secondRowCancelButton);


                    }
                    else if (tappedCount == 0)
                    {
                        firstNameTextBox.Text = "";
                        Debug.WriteLine(firstNameTextBox.Text);
                        secondNameTextBox.Text = "";
                        Debug.WriteLine(secondNameTextBox.Text);
                        thirdNameTextBox.Text = "";
                        Debug.WriteLine(thirdNameTextBox.Text);
                        fourthNameTextBox.Text = " ";
                        hide(firstNameTextBox, firstToggle, firstRowCancelButton);
                        minusImage.Visibility = Visibility.Collapsed;
                        dataList.Visibility = Visibility.Collapsed;
                    }

                }
                catch (Exception ex)
                {

                }
                firstTimeFlag = 0;
                delegateeList.RemoveAt(0);
                cancelCount--;
                cancelIconClicked();
                reenableUI();
            }
        }
        private async void secondRowCancelButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //server call to delete delegatee
            p_ring.IsActive = true;
            disableUI();
            NetworkManager manager = NetworkManager.Instance;
            Task GetCancelAction = manager.GetCancelRequestAsync((string)App.localSettings.Values["stores"], delegateeList[1].user);
            await GetCancelAction;

            if (secondRowCancelButton.IsTapEnabled)
            {
                tappedCount = delegateeListCount() - 1;
                if (tappedCount == 3)
                {
                    secondNameTextBox.Text = delegateeList[2].user;
                    delegateeList[1].access = delegateeList[2].access;
                    if (delegateeList[1].access.Equals("y"))
                    {
                        firstTimeFlag = 1;
                        secondToggle.IsOn = true;
                    }
                    thirdNameTextBox.Text = delegateeList[3].user;
                    delegateeList[2].access = delegateeList[3].access;
                    if (delegateeList[2].access.Equals("y"))
                    {
                        firstTimeFlag = 1;
                        thirdToggle.IsOn = true;
                    }
                    fourthNameTextBox.Text = "";
                    hide(fourthNameTextBox, fourthToggle, fourthRowCancelButton);


                }
                else if (tappedCount == 2)
                {
                    Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + delegateeListCount());
                    secondNameTextBox.Text = delegateeList[2].user;
                    delegateeList[1].access = delegateeList[2].access;
                    if (delegateeList[1].access.Equals("y"))
                    {
                        firstTimeFlag = 1;
                        secondToggle.IsOn = true;
                    }
                    thirdNameTextBox.Text = "";

                    fourthNameTextBox.Text = "";
                    hide(thirdNameTextBox, thirdToggle, thirdRowCancelButton);



                }
                else if (tappedCount == 1)
                {
                    secondNameTextBox.Text = "";
                    thirdNameTextBox.Text = "";

                    fourthNameTextBox.Text = "";
                    hide(secondNameTextBox, secondToggle, secondRowCancelButton);


                }
                firstTimeFlag = 0;
                delegateeList.RemoveAt(1);
                cancelCount--;
                cancelIconClicked();
                reenableUI();
            }
        }

        private async void thirdRowCancelButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //server call to delete delegatee
            p_ring.IsActive = true;
            disableUI();
            NetworkManager manager = NetworkManager.Instance;
            Task GetCancelAction = manager.GetCancelRequestAsync((string)App.localSettings.Values["stores"], delegateeList[2].user);
            await GetCancelAction;
            if (thirdRowCancelButton.IsTapEnabled)
            {
                tappedCount = delegateeListCount() - 1;
                Debug.WriteLine("########################" + tappedCount);
                if (tappedCount == 3)
                {
                    thirdNameTextBox.Text = delegateeList[3].user; ;
                    delegateeList[2].access = delegateeList[3].access;
                    if (delegateeList[2].access.Equals("y"))
                    {
                        firstTimeFlag = 1;
                        thirdToggle.IsOn = true;
                    } fourthNameTextBox.Text = "";
                    hide(fourthNameTextBox, fourthToggle, fourthRowCancelButton);


                }
                else if (tappedCount == 2)
                {
                    thirdNameTextBox.Text = "";
                    fourthNameTextBox.Text = "";
                    hide(thirdNameTextBox, thirdToggle, thirdRowCancelButton);

                }
                firstTimeFlag = 0;
                delegateeList.RemoveAt(2);
                cancelCount--;
                cancelIconClicked();
                reenableUI();
            }
        }

        private async void fourthRowCancelButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //server call to delete delegatee
            p_ring.IsActive = true;
            disableUI();
            NetworkManager manager = NetworkManager.Instance;
            Task GetCancelAction = manager.GetCancelRequestAsync((string)App.localSettings.Values["stores"], delegateeList[3].user);
            await GetCancelAction;
            if (fourthRowCancelButton.IsTapEnabled)
            {
                tappedCount = delegateeListCount() - 1;
                if (tappedCount == 3)
                {
                    fourthNameTextBox.Text = "";
                    hide(fourthNameTextBox, fourthToggle, fourthRowCancelButton);

                }
                firstTimeFlag = 0;
                delegateeList.RemoveAt(3);
                cancelCount--;
                cancelIconClicked();
                reenableUI();
            }

        }

        //toggle action
        private async void firstToggle_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {

                Debug.WriteLine(firstTimeFlag);
                if (firstTimeFlag == 0 && delegateeList[0].access.Equals("n"))
                {
                    delegateeList[0].access = "y";
                    p_ring.IsActive = true;
                    disableUI();
                    NetworkManager manager = NetworkManager.Instance;
                    Task GetToggleResponse = manager.GetToggleRequestAsync(delegateeList[0].access, delegateeList[0].user);
                    await GetToggleResponse;

                    p_ring.IsActive = false;
                    messageStore.Visibility = Visibility.Visible;
                    message.Text = StoreEmpowermentModel.listResponse.storeManagerName + " has enabled \n" + "store manager orders for " + firstNameTextBox.Text + "\n in store " + App.localSettings.Values["stores"];
                    await putDelay();
                    messageStore.Visibility = Visibility.Collapsed;
                    reenableUI();
                }
                else if (firstTimeFlag == 0)
                {
                    delegateeList[0].access = "n";
                    p_ring.IsActive = true;
                    disableUI();
                    NetworkManager manager = NetworkManager.Instance;
                    Task GetToggleResponse = manager.GetToggleRequestAsync(delegateeList[0].access, delegateeList[0].user);
                    await GetToggleResponse;
                    p_ring.IsActive = false;
                    messageStore.Visibility = Visibility.Visible;
                    message.Text = StoreEmpowermentModel.listResponse.storeManagerName + " has disabled \n" + "store manager orders for " + firstNameTextBox.Text + "\n in store " + App.localSettings.Values["stores"];
                    await putDelay();
                    messageStore.Visibility = Visibility.Collapsed;
                    reenableUI();
                }
            }


            catch (Exception ex1)
            {

            }


        }

        private async void secondToggle_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {

                Debug.WriteLine(firstTimeFlag);
                if (firstTimeFlag == 0 && delegateeList[1].access.Equals("n"))
                {
                    delegateeList[1].access = "y";
                    p_ring.IsActive = true;
                    disableUI();
                    NetworkManager manager = NetworkManager.Instance;
                    Task GetToggleResponse = manager.GetToggleRequestAsync(delegateeList[1].access, delegateeList[1].user);
                    await GetToggleResponse;
                    p_ring.IsActive = false;
                    messageStore.Visibility = Visibility.Visible;
                    message.Text = StoreEmpowermentModel.listResponse.storeManagerName + " has enabled \n" + " store manager orders for " + secondNameTextBox.Text + "\n in store " + App.localSettings.Values["stores"];
                    await putDelay();
                    messageStore.Visibility = Visibility.Collapsed;
                    reenableUI();
                }
                else if (firstTimeFlag == 0)
                {
                    delegateeList[1].access = "n";
                    p_ring.IsActive = true;
                    disableUI();
                    NetworkManager manager = NetworkManager.Instance;
                    Task GetToggleResponse = manager.GetToggleRequestAsync(delegateeList[1].access, delegateeList[1].user);
                    await GetToggleResponse;
                    p_ring.IsActive = false;
                    messageStore.Visibility = Visibility.Visible;
                    message.Text = StoreEmpowermentModel.listResponse.storeManagerName + " has disabled \n" + " store manager orders for " + secondNameTextBox.Text + "\n in store " + App.localSettings.Values["stores"];
                    await putDelay();
                    messageStore.Visibility = Visibility.Collapsed;
                    reenableUI();
                }

            }
            catch (Exception ex)
            {

            }


        }
        private async void thirdToggle_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {

                Debug.WriteLine(firstTimeFlag);
                if (firstTimeFlag == 0 && delegateeList[2].access.Equals("n"))
                {
                    delegateeList[2].access = "y";
                    p_ring.IsActive = true;
                    disableUI();
                    NetworkManager manager = NetworkManager.Instance;
                    Task GetToggleResponse = manager.GetToggleRequestAsync(delegateeList[2].access, delegateeList[2].user);
                    await GetToggleResponse;
                    p_ring.IsActive = false;
                    messageStore.Visibility = Visibility.Visible;
                    message.Text = StoreEmpowermentModel.listResponse.storeManagerName + " has enabled \n" + " store manager orders for " + thirdNameTextBox.Text + "\n in store " + App.localSettings.Values["stores"];
                    await putDelay();
                    messageStore.Visibility = Visibility.Collapsed;
                    reenableUI();
                }
                else if (firstTimeFlag == 0)
                {
                    delegateeList[2].access = "n";
                    p_ring.IsActive = true;
                    disableUI();
                    NetworkManager manager = NetworkManager.Instance;
                    Task GetToggleResponse = manager.GetToggleRequestAsync(delegateeList[2].access, delegateeList[2].user);
                    await GetToggleResponse;
                    p_ring.IsActive = false;
                    messageStore.Visibility = Visibility.Visible;
                    message.Text = StoreEmpowermentModel.listResponse.storeManagerName + " has disabled \n" + " store manager orders for " + thirdNameTextBox.Text + "\n in store " + App.localSettings.Values["stores"];
                    await putDelay();
                    messageStore.Visibility = Visibility.Collapsed;
                    reenableUI();
                }

            }
            catch (Exception ex)
            {

            }


        }
        private async void fourthToggle_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {

                Debug.WriteLine(firstTimeFlag);
                if (firstTimeFlag == 0 && delegateeList[3].access.Equals("n"))
                {
                    delegateeList[3].access = "y";
                    p_ring.IsActive = true;
                    disableUI();
                    NetworkManager manager = NetworkManager.Instance;
                    Task GetToggleResponse = manager.GetToggleRequestAsync(delegateeList[3].access, delegateeList[3].user);
                    await GetToggleResponse;
                    p_ring.IsActive = false;
                    messageStore.Visibility = Visibility.Visible;
                    message.Text = StoreEmpowermentModel.listResponse.storeManagerName + " has enabled \n" + "store manager orders for " + fourthNameTextBox.Text + "\n in store " + App.localSettings.Values["stores"];
                    await putDelay();
                    messageStore.Visibility = Visibility.Collapsed;
                    reenableUI();
                }
                else if (firstTimeFlag == 0)
                {
                    delegateeList[3].access = "n";
                    p_ring.IsActive = true;
                    disableUI();
                    NetworkManager manager = NetworkManager.Instance;
                    Task GetToggleResponse = manager.GetToggleRequestAsync(delegateeList[3].access, delegateeList[3].user);
                    await GetToggleResponse;
                    p_ring.IsActive = false;
                    messageStore.Visibility = Visibility.Visible;
                    message.Text = StoreEmpowermentModel.listResponse.storeManagerName + " has disabled \n" + " store manager orders for " + fourthNameTextBox.Text + "\n in store " + App.localSettings.Values["stores"];
                    await putDelay();
                    messageStore.Visibility = Visibility.Collapsed;
                    reenableUI();
                }
            }
            catch (Exception ex)
            {

            }


        }

        //for success handel call
        public void successHandelForTeamAccess(TeamAccessListResponse response)
        {
            ResponseHandler.refreshFlag = 0;
            p_ring.IsActive = false;
            Debug.WriteLine("inside success handler for teamAccess");
            Debug.WriteLine("" + response);
            StoreEmpowermentModel.listResponse = response;
            tappedCount = delegateCount();
            Debug.WriteLine(tappedCount + "****************************");
            delegateeList = new List<Delegatee0>(4);
            for (int i = 0; i < tappedCount; i++)
            {
                delegateeList.Add(new Delegatee0());
            }
            Debug.WriteLine(delegateeList.Count + "****************************");
            initList(tappedCount);
            dataList.Visibility = Visibility.Visible;
            firstTimeFlag = 1;
            failureFlag = 1;
        }


        public void initList(int count)
        {
            if (count >= 1)
            {
                delegateeList[0] = StoreEmpowermentModel.listResponse.delegatees.delegatee0;
            }
            if (count >= 2)
            {
                delegateeList[1] = StoreEmpowermentModel.listResponse.delegatees.delegatee1;
            }
            if (count >= 3)
            {
                delegateeList[2] = StoreEmpowermentModel.listResponse.delegatees.delegatee2;
            }
            if (count >= 4)
            {
                delegateeList[3] = StoreEmpowermentModel.listResponse.delegatees.delegatee3;
            }
        }
        public void successHandleForTeamAccessToggle(string res)
        {
            // TeamAccess.pring.IsActive = false;
            //firstRowCancelButton.Visibility = Visibility.Visible;
            Debug.WriteLine("inside success handle for teamAccessToggle");



        }

        async Task putDelay()
        {
            await Task.Delay(2000);
        }
        //displaying datalist from server
        public void DisplayList()
        {
            try
            {

                if (StoreEmpowermentModel.listResponse.delegatees.delegatee0.user != null)
                {
                    visible(firstNameTextBox, firstToggle);
                    firstNameTextBox.Text = StoreEmpowermentModel.listResponse.delegatees.delegatee0.user;
                    Debug.WriteLine("" + firstNameTextBox.Text);
                    if (StoreEmpowermentModel.listResponse.delegatees.delegatee0.access.Equals("n"))
                    {
                        firstToggle.IsOn = false;

                    }
                    else
                    {
                        firstToggle.IsOn = true;

                    }
                }
                if (StoreEmpowermentModel.listResponse.delegatees.delegatee1.user != null)
                {
                    visible(secondNameTextBox, secondToggle);
                    secondNameTextBox.Text = StoreEmpowermentModel.listResponse.delegatees.delegatee1.user;
                    if (StoreEmpowermentModel.listResponse.delegatees.delegatee1.access.Equals("n"))
                    {
                        secondToggle.IsOn = false;

                    }
                    else
                    {
                        secondToggle.IsOn = true;

                    }
                }
                if (StoreEmpowermentModel.listResponse.delegatees.delegatee2.user != null)
                {
                    thirdNameTextBox.Text = StoreEmpowermentModel.listResponse.delegatees.delegatee2.user;
                    if (StoreEmpowermentModel.listResponse.delegatees.delegatee2.access.Equals("n"))
                    {
                        thirdToggle.IsOn = false;

                    }
                    else
                    {
                        thirdToggle.IsOn = true;
                    }
                    visible(thirdNameTextBox, thirdToggle);
                }
                if (StoreEmpowermentModel.listResponse.delegatees.delegatee3.user != null)
                {
                    fourthNameTextBox.Text = StoreEmpowermentModel.listResponse.delegatees.delegatee3.user;
                    if (StoreEmpowermentModel.listResponse.delegatees.delegatee3.access.Equals("n"))
                    {
                        fourthToggle.IsOn = false;

                    }
                    else
                    {
                        fourthToggle.IsOn = true;
                    }
                    visible(fourthNameTextBox, fourthToggle);
                }
            }
            catch (Exception ex)
            {

            }

            dataList.Visibility = Visibility.Visible;
            popUpGrid.Visibility = Visibility.Collapsed;

        }

        //done button action
        private void doneButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (delegateeListCount() == 0)
            {
                doneButton.Visibility = Visibility.Collapsed;
                firstRowCancelButton.Visibility = Visibility.Collapsed;
                secondRowCancelButton.Visibility = Visibility.Collapsed;
                thirdRowCancelButton.Visibility = Visibility.Collapsed;
                fourthRowCancelButton.Visibility = Visibility.Collapsed;
                addImage.Visibility = Visibility.Visible;
                minusImage.Visibility = Visibility.Collapsed;
            }
            else
            {
                doneButton.Visibility = Visibility.Collapsed;
                firstRowCancelButton.Visibility = Visibility.Collapsed;
                secondRowCancelButton.Visibility = Visibility.Collapsed;
                thirdRowCancelButton.Visibility = Visibility.Collapsed;
                fourthRowCancelButton.Visibility = Visibility.Collapsed;
                addImage.Visibility = Visibility.Visible;
                minusImage.Visibility = Visibility.Visible;

            }
        }

        private void cancelIconClicked()
        {
            addImage.Visibility = Visibility.Collapsed;
            minusImage.Visibility = Visibility.Collapsed;
            doneButton.Visibility = Visibility.Visible;
        }

        private void disableUI()
        {

            addImage.IsTapEnabled = false;

            minusImage.IsTapEnabled = false;
            firstToggle.IsEnabled = false;
            secondToggle.IsEnabled = false;
            thirdToggle.IsEnabled = false;
            fourthToggle.IsEnabled = false;
            doneButton.IsEnabled = false;
            // dataList.Opacity = .1;
            secondaryAccessHeader.IsTapEnabled = false;
        }

        private void reenableUI()
        {



            addImage.IsTapEnabled = true;
            minusImage.IsTapEnabled = true;
            firstToggle.IsEnabled = true;
            secondToggle.IsEnabled = true;
            thirdToggle.IsEnabled = true;
            fourthToggle.IsEnabled = true;
            doneButton.IsEnabled = true;
            dataList.Opacity = 1;
            secondaryAccessHeader.IsTapEnabled = true;

        }
        public int delegateCancelCount()
        {
            if (StoreEmpowermentModel.listResponse.delegatees.delegatee3 == null)
            {
                return 0;
            }
            if (StoreEmpowermentModel.listResponse.delegatees.delegatee2 == null)
            {
                return 1;
            }
            if (StoreEmpowermentModel.listResponse.delegatees.delegatee1 == null)
            {
                return 2;
            }
            if (StoreEmpowermentModel.listResponse.delegatees.delegatee0 == null)
            {
                return 3;
            }
            return 4;

        }
        public void successHandleForTeamAccessUpsert(String employeeName)
        {
            // firstTimeFlag = 2;
            //failureFlag = 2;
            Delegatee0 del = new Delegatee0();
            del.user = employeeName;
            del.access = "y";
            delegateeList.Add(del);
        }


        public int delegateeListCount()
        {
            return delegateeList.Count;
        }

        public int delegateCount()
        {
            if (StoreEmpowermentModel.listResponse.delegatees.delegatee0 == null)
            {
                return 0;
            }
            if (StoreEmpowermentModel.listResponse.delegatees.delegatee1 == null)
            {
                return 1;
            }
            if (StoreEmpowermentModel.listResponse.delegatees.delegatee2 == null)
            {
                return 2;
            }
            if (StoreEmpowermentModel.listResponse.delegatees.delegatee3 == null)
            {
                return 3;
            }
            return 4;
        }

        public void successHandleForTeamAccessCancel()
        {
            firstTimeFlag = 3;
            TeamAccess.pring.IsActive = false;
        }


        public void successHandleForTeamAccessCancel(string res)
        {
            Debug.WriteLine("inside success handelfor delete");
            p_ring.IsActive = false;
            /*if (index == 1)
            {
                StoreEmpowermentModel.listResponse.delegatees.delegatee0 = null;
            }
            else if (index == 2)
            {
                StoreEmpowermentModel.listResponse.delegatees.delegatee1 = null;
            }
            else if (index == 3)
            {
                StoreEmpowermentModel.listResponse.delegatees.delegatee2 = null;
            }
            else if (index == 4)
            {
                StoreEmpowermentModel.listResponse.delegatees.delegatee3 = null;
            }*/
        }

         public static async void getData()
        {
            Debug.WriteLine("inside getData");

            Debug.WriteLine("inside getData1");
            if (App.localSettings.Values["stores"] == null)
            {
                ResponseHandler.teamAccessFlag = 0;

                Debug.WriteLine("Inside Change Store");

                teamAccessptr.Frame.Navigate(typeof(StoreList));
            }
            else
            {
                //ResponseHandler.storeRoutineFlag++;
                NetworkManager manager = NetworkManager.Instance;
                Task GetTeamAccessResponse = manager.TeamAccessAsyncGetList();
                await GetTeamAccessResponse;
                Debug.WriteLine("failure flag is " + failureFlag);
                
                if (failureFlag == 1)
                {
                    teamAccessptr.p_ring.IsActive = false;
                    teamAccessptr.DisplayList();
                    failureFlag = 0;
                    firstTimeFlag = 0;
                }
                try
                {
                    if (delegateeList.Count == 0)
                    {
                        teamAccessptr.dataList.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        teamAccessptr.dataList.Visibility = Visibility.Visible;
                    }

                }
                catch (Exception)
                {

                }



                Debug.WriteLine("failure flag is " + failureFlag);
                if (StoreEmpowermentModel.listResponse.storeManager == false)
                {
                    teamAccessptr.addImage.Visibility = Visibility.Collapsed;
                    teamAccessptr.minusImage.Visibility = Visibility.Collapsed;
                    teamAccessptr.disableUI();

                }
                if (StoreEmpowermentModel.listResponse.storeManager == true)
                {
                    Debug.WriteLine(StoreEmpowermentModel.listResponse.storeManagerName);
                    teamAccessptr.reenableUI();
                }
            }
        }
         public static async void getData(String pageName,String state,String employeeName)
         {
             Debug.WriteLine("inside getData");

             Debug.WriteLine("inside getData1");
             if (App.localSettings.Values["stores"] == null)
             {
                 ResponseHandler.teamAccessFlag = 0;

                 Debug.WriteLine("Inside Change Store");

                 teamAccessptr.Frame.Navigate(typeof(StoreList));
             }
             else
             {
                 //ResponseHandler.storeRoutineFlag++;
                 NetworkManager manager = NetworkManager.Instance;
                 if(pageName.Equals("toggle"))
                 { 
                 Task GetTeamAccessResponse = manager.GetToggleRequestAsync(state,employeeName);
                 await GetTeamAccessResponse;
                 }
                 if(pageName.Equals("cancel"))
                 {
                     Task GetTeamAccessResponse = manager.GetCancelRequestAsync(state, employeeName);
                     await GetTeamAccessResponse;
                 }
                
             }
         }

         public static async void getData(String employeeName)
         {
             Debug.WriteLine("inside getData");

             Debug.WriteLine("inside getData1");
             if (App.localSettings.Values["stores"] == null)
             {
                 ResponseHandler.teamAccessFlag = 0;

                 Debug.WriteLine("Inside Change Store");

                 teamAccessptr.Frame.Navigate(typeof(StoreList));
             }
             else
             {
                 //ResponseHandler.storeRoutineFlag++;
                 NetworkManager manager = NetworkManager.Instance;


                 Task GetTeamAccessResponse = manager.GetNameAsync(employeeName);
                     await GetTeamAccessResponse;
                 

             }
         }
        public void successHandelForTeamAccess()
        {
            pring.IsActive = false;
            addImage.Visibility = Visibility.Collapsed;
            dataList.Visibility = Visibility.Collapsed;
            minusImage.Visibility = Visibility.Collapsed;
            disableUI();
        }
    }
}


