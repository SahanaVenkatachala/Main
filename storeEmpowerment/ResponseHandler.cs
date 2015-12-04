using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    class ResponseHandler
    {
        private popupBox popup;
        public static int populateFlag;
        public static int refreshFlag = 0;
        public static int storeRoutineFlag = 0;
        public static int settingsFlag = 0;
        public static int searchFlag = 0;
        public static int teamAccessFlag = 0;

        private string GetProductNumber()
        {
            int index = StoreEmpowermentModel.htmlResponse.IndexOf("id=\"productImage\"");

            string temp = StoreEmpowermentModel.htmlResponse.Substring(index - 100, 100);

            index = temp.IndexOf("</span>");

            temp = temp.Substring(index - 13, 13);
            Debug.WriteLine("######" + temp);
            return temp;
        }

        private void GetTPNB()
        {
            StoreEmpowermentModel.tpnb = StoreEmpowermentModel.details.Product.TPNB;
        }

        private void addToLocal()
        {
            string productName = StoreEmpowermentModel.details.Product.Description;
            Debug.WriteLine(productName);

            string productNumber = StoreEmpowermentModel.details.Product.EAN;
            Debug.WriteLine(productNumber);
            GetTPNB();
            Debug.WriteLine(productName);
            int searchIndex = StoreEmpowermentModel.recentItems.IndexOf(productName + "\n" + productNumber);
            if (searchIndex >= 0)
            {
                StoreEmpowermentModel.recentItems.RemoveAt(searchIndex);
            }
            StoreEmpowermentModel.recentItems.Insert(0, productName + "\n" + productNumber);
            //StoreEmpowermentModel.recentItems.Add(productName+"\n"+productNumber);
            App.localSettings.Values["recent"] = StoreEmpowermentModel.recentItems.ToArray();
        }

        private void AddJavascript()
        {

            int startIndex = StoreEmpowermentModel.htmlResponse.IndexOf("var auth");
            Debug.WriteLine(startIndex);
            StoreEmpowermentModel.htmlResponse = StoreEmpowermentModel.htmlResponse.Insert(startIndex, "function SetData(data)\n {\njsonData = JSON.parse(data);\n}\n");

            startIndex = StoreEmpowermentModel.htmlResponse.IndexOf("$scope.controls.loading = true;");
            int endIndex = StoreEmpowermentModel.htmlResponse.IndexOf("$scope.showCalendar = true;");
            int span = (endIndex - startIndex);
            Debug.WriteLine(span + "   " + endIndex + "   " + startIndex);
            StoreEmpowermentModel.htmlResponse = StoreEmpowermentModel.htmlResponse.Remove(startIndex, span);
            //Debug.WriteLine("modified : " + StoreEmpowermentModel.htmlResponse);

            string insertString = @"window.external.notify('went in angular');
                $scope.data.response = jsonData;
                
                if(jsonData==null){
                    window.external.notify('400');
                }
                else{
                window.external.notify('data : ' + jsonData.StoreOrder.length);
                var weeks = [];
                for (var i = 0; i < jsonData.StoreOrder.length; i++) {
                    //window.external.notify(i);
                    var dd = $filter('makeDate')(jsonData.StoreOrder[i].OrderDate);
                    d = $filter('tescoWeek')(dd);
                    if (i % 7 == 0) weeks.push([{ WeekNumber: d }]);
                    weeks[weeks.length - 1].push(jsonData.StoreOrder[i]);
                    if (jsonData.StoreOrder[i].OrderDate == jsonData.Today) {
                        jsonData.StoreOrder[i].Today = true;
                    }
                    var index;
                    for (index = 0; index < promo.length; ++index) {
                        var start = new Date(promo[index].StartDate);
                        var end = new Date(promo[index].EndDate);

                        if (dd >= start && dd <= end) {
                            jsonData.StoreOrder[i].OnPromo = true;
                        }
                    };
                }
                window.external.notify('done');
                $scope.data.weeks = weeks;
                $scope.data.so = jsonData;
                $location.hash('calendar');
                $anchorScroll();
                window.external.notify('completed');}";

            StoreEmpowermentModel.htmlResponse = StoreEmpowermentModel.htmlResponse.Insert(startIndex, insertString);
            //Debug.WriteLine("modified : " + StoreEmpowermentModel.htmlResponse);
            startIndex = StoreEmpowermentModel.htmlResponse.IndexOf("$http.delete");
            endIndex = StoreEmpowermentModel.htmlResponse.IndexOf("$scope.selectDate");
            span = (endIndex - startIndex);
            Debug.WriteLine(span + "   " + endIndex + "   " + startIndex);
            StoreEmpowermentModel.htmlResponse = StoreEmpowermentModel.htmlResponse.Remove(startIndex, span);
            insertString = @"window.external.notify('revert ' + 'https://labs.ocset.net/inform/store/' + storeNumber + '/products/' + tpnb + '/orders/' + req.OrderDate);
              
                //$scope.loadOrders();
                $scope.controls.showEditor = false;
                $scope.controls.saving = false;
                $scope.selected.Selected = false;};";
            StoreEmpowermentModel.htmlResponse = StoreEmpowermentModel.htmlResponse.Insert(startIndex, insertString);
            Debug.WriteLine("modified : " + StoreEmpowermentModel.htmlResponse);
        }


        public void successHandleForSearch()
        {
            try
            {
                addToLocal();
                //AddJavascript();
            }
            catch (Exception)
            {
            }
            productSearch.presenter.searchSuccess();

        }



        public async void failureHandleForSearch(string message)
        {
            if ((string)App.localSettings.Values["currentPage"] == "searchPg")
                searchPage.presenter.failSearch();
            else
            {
                productSearch.presenter.failSearch();
            }
            popupBox popup = new popupBox(message);
            await popup.ShowDialog();
        }

        public void ClosePointer()
        {
        }


      /* public async void handleUnauthorizedForSearch(String pageName,String productId)
        {
            Debug.WriteLine("inside about fail 1");
            if (searchFlag != 0)
            {
                Debug.WriteLine("inside about fail 2");
                productSearch.homePointer.Frame.Navigate(typeof(LoginPage));
                failureHandle("Username/Password Not Verified");
            }
            else
            {
                NetworkManager manager = NetworkManager.Instance;

                LoginRequest lrequest = new LoginRequest((string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["refresh"], (string)App.localSettings.Values["username"]);
                Debug.WriteLine(App.localSettings.Values["refresh"] + "*********************");
                if (pageName.Equals("product"))
                {
                    Task refreshResponseTask = manager.refreshRequestAsyncForProduct(lrequest,productId,"product");
                    await refreshResponseTask;
                }
                if(pageName.Equals("getName"))
                {
                    Task refreshResponseTask = manager.refreshRequestAsyncForProduct(lrequest, productId, "getName");
                    await refreshResponseTask;
                }
            }*/
           

           /* productSearch.pring.IsActive = false;
            //productSearch.homePointer.IsEnabled = true;
            popupBox popup = new popupBox("Please Login Again");
            await popup.ShowDialog();
            productSearch.homePointer.Frame.Navigate(typeof(LoginPage));*/
        
     /*   public async void refreshCallTeamAccessToggle(String pageName,String state,String employeeName)
        {
            if (teamAccessFlag != 0)
            {
                Debug.WriteLine("inside about fail 2");
                productSearch.homePointer.Frame.Navigate(typeof(LoginPage));
                failureHandle("Username/Password Not Verified");
            }
            else
            {
                NetworkManager manager = NetworkManager.Instance;

                LoginRequest lrequest = new LoginRequest((string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["refresh"], (string)App.localSettings.Values["username"]);
                Debug.WriteLine(App.localSettings.Values["refresh"] + "*********************");
                Task refreshResponseTask = manager.refreshRequestAsyncTeamAccessToggle(lrequest,pageName,state,employeeName);
                await refreshResponseTask;
            }

        }*/
        public string processString(string html)
        {
            StringBuilder processedHtml = new StringBuilder(html);
            /*processedHtml.Replace(" ", "");*/
            try
            {
                processedHtml.Replace("font-size: 1.2em", "font-size: 1.0em");
                processedHtml.Replace("font-size: 0.9em", "font-size: 1.0em");
                processedHtml.Replace("font-size: 0.8em", "font-size: 1.0em");
                processedHtml.Replace("font-size: 1.25em", "font-size: 1.0em");
                //Debug.WriteLine(StoreEmpowermentModel.htmlResponse + "        " + StoreEmpowermentModel.htmlResponse.Contains("font-size: 1.2em"));
                //Debug.WriteLine(samp);
            }
            catch (Exception)
            {

            }
            return processedHtml.ToString();


        }

        public void failureHandleForRefresh()
        {
            App.localSettings.Values["refresh"] = null;
            App.localSettings.Values["newToken"] = null;
            App.localSettings.Values["expires"] = null;
            storeRoutine.pring.IsActive = false;
            storeRoutine.storePointer.IsEnabled = true;
            storeRoutine.storePointer.Frame.Navigate(typeof(LoginPage));

        }
        //not needed
        /*public async void handleUnauthorized()
        {
            if (storeRoutineFlag != 0)
            {
                App.localSettings.Values["refresh"] = null;
                App.localSettings.Values["newToken"] = null;
                App.localSettings.Values["expires"] = null;
                storeRoutine.storePointer.Frame.Navigate(typeof(LoginPage));
                failureHandle("Username/Password Not Verified");
            }

            else
            {

                NetworkManager manager = NetworkManager.Instance;
                LoginRequest lrequest = new LoginRequest((string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["refresh"], (string)App.localSettings.Values["username"]);
                Task refreshResponseTask = manager.refreshRequestAsync(lrequest);
                await refreshResponseTask;
            }


        }*/
        public async void UnauthSearch()
        {
            productSearch.pring.IsActive = false;
            //productSearch.homePointer.IsEnabled = true;
            popupBox popup = new popupBox("Please Login Again");
            await popup.ShowDialog();
            productSearch.homePointer.Frame.Navigate(typeof(LoginPage));
        }
        public async void handleUnauthorizedForAbout()
        {
            Debug.WriteLine("inside about fail 1");
            if (settingsFlag != 0)
            {
                Debug.WriteLine("inside about fail 2");
                storeRoutine.storePointer.Frame.Navigate(typeof(LoginPage));
                failureHandle("Username/Password Not Verified");
            }
            else
            {
                NetworkManager manager = NetworkManager.Instance;

               // LoginRequest lrequest = new LoginRequest((string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["refresh"], (string)App.localSettings.Values["username"]);
                RefreshRequest Rrequest = new RefreshRequest((string)App.localSettings.Values["refresh"]);
                Debug.WriteLine(App.localSettings.Values["refresh"] + "*********************");
                Task refreshResponseTask = manager.refreshRequestAsync(Rrequest, "About");
                await refreshResponseTask;
            }
           
        }
        public async void handleUnauthorizedForKpi()
        {
            /*Debug.WriteLine("inside kpi fail 1");
            if (storeRoutineFlag != 0)
            {
                Debug.WriteLine("inside kpi fail 2");
                storeRoutine.storePointer.Frame.Navigate(typeof(LoginPage));
                failureHandle("Username/Password Not Verified");
            }
            else { 
            NetworkManager manager = NetworkManager.Instance;

            //LoginRequest lrequest = new LoginRequest((string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["refresh"], (string)App.localSettings.Values["username"]);
            RefreshRequest Rrequest = new RefreshRequest((string)App.localSettings.Values["refresh"]);

            Debug.WriteLine(App.localSettings.Values["refresh"] + "*********************");
            Task refreshResponseTask = manager.refreshRequestAsync(Rrequest, "StoreRoutine");
            await refreshResponseTask;
            }
            */
            Debug.WriteLine("inside kpi fail 1");
            if (refreshFlag == 12)
            {
                Debug.WriteLine("inside kpi fail 2");
                storeRoutine.storePointer.Frame.Navigate(typeof(LoginPage));
                failureHandle("Username/Password Not Verified");
            }

            else
            {
                NetworkManager manager = NetworkManager.Instance;
                
                LoginRequest lrequest = new LoginRequest((string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["refresh"], (string)App.localSettings.Values["username"]);
                Debug.WriteLine(App.localSettings.Values["refresh"]+"*********************");
                Task refreshResponseTask = manager.refreshRequestAsync(lrequest);
                await refreshResponseTask;
            }


        

        }

        public async void handelUnauthorizedForTeamAccess()
        {
             if (refreshFlag == 12)
             {
                 Debug.WriteLine("inside teamAccess failure");
                 storeRoutine.storePointer.Frame.Navigate(typeof(LoginPage));
                 failureHandle("Username/Password Not Verified");
             }

             else
             {
                 NetworkManager manager = NetworkManager.Instance;
                 LoginRequest lrequest = new LoginRequest((string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["refresh"], (string)App.localSettings.Values["username"]);
                 Task refreshResponseTask = manager.refreshRequestAsync(lrequest);
                 await refreshResponseTask;
             }
           /* Debug.WriteLine("inside team acess fail 1");
             if (teamAccessFlag!=0)
             {
                 Debug.WriteLine("inside teamAccess failure");
                 storeRoutine.storePointer.Frame.Navigate(typeof(LoginPage));
                 failureHandle("Username/Password Not Verified");
             }

             else
             {
                 NetworkManager manager = NetworkManager.Instance;
                 //LoginRequest lrequest = new LoginRequest((string)App.localSettings.Values["newToken"], (string)App.localSettings.Values["refresh"], (string)App.localSettings.Values["username"]);
                 RefreshRequest Rrequest = new RefreshRequest((string)App.localSettings.Values["refresh"]);
                 Task refreshResponseTask = manager.refreshRequestAsyncTeamAccess(Rrequest);
                 await refreshResponseTask;
             }*/


        }

        public async void successHandleForLogin(LoginResponse res)
        {

            Debug.WriteLine(App.localSettings.Values["stores"] + "   " + App.localSettings.Values["username"] + "    " + App.localSettings.Values["previous"]);
            Debug.WriteLine("inside login success handle");
            App.localSettings.Values["newToken"] = res.access_token;
            Debug.WriteLine("after token save" + res.access_token+"************************");
            App.localSettings.Values["expires"] = res.expires_in;
            App.localSettings.Values["refresh"] = res.refresh_token;
            Debug.WriteLine(res.refresh_token + "****************************"+App.localSettings.Values["timeStamp"]);
            Debug.WriteLine(res.expires_in);
           /* if (App.localSettings.Values["timeStamp"].Equals(""))
            {
                NetworkManager manager = NetworkManager.Instance;
                populateFlag = 0;
                Task loginResponseTask = manager.GetAllStoresAsync((string)App.localSettings.Values["username"]);
                await loginResponseTask;
            }*/
           
            

                NetworkManager manager = NetworkManager.Instance;
                populateFlag = 0;
                Task loginResponseTask = manager.GetAllStoresAsync((string)App.localSettings.Values["username"]);
                await loginResponseTask;
            


        }

        public void successHandleForAppVersion()
        {
            Debug.WriteLine("\n\nIn successHandleForAppVersion ");
        }

        public void successHandleForRefresh(LoginResponse loginResponse)
        {
            Debug.WriteLine("inside refresh success");
            App.localSettings.Values["refresh"] = loginResponse.refresh_token;
            App.localSettings.Values["newToken"] = loginResponse.access_token;
            App.localSettings.Values["expires"] = loginResponse.expires_in;
            //storeRoutine.pring.IsActive = false;
            //storeRoutine.storePointer.IsEnabled = true;
           // storeRoutine.getData();
            

        }

        public async void failureHandleForPopulate(string message)
        {

            App.localSettings.Values["refresh"] = null;
            App.localSettings.Values["newToken"] = null;
            App.localSettings.Values["expires"] = null;

            LoginPage.pring.IsActive = false;
            LoginPage.pagePoint.IsEnabled = true;
            popupBox popup = new popupBox(message);
            await popup.ShowDialog();

        }

        public async void failureHandle(string message)
        {
            LoginPage.pring.IsActive = false;
            LoginPage.pagePoint.IsEnabled = true;
            popupBox popup = new popupBox(message);
            await popup.ShowDialog();

        }

        public async void failureHandleForKpi(string message)
        {
            storeRoutine.pring.IsActive = false;
            storeRoutine.storePointer.IsEnabled = true;
            popupBox popup = new popupBox(message);
            await popup.ShowDialog();

        }
        public async void failureHandleForKpiAbout(string message)
        {
            Settings.pring.IsActive = false;
            Settings.settingptr.IsEnabled = true;
            popupBox popup = new popupBox(message);
            await popup.ShowDialog();

        }

        public async void noDataForPopulate()
        {
            LoginPage.pring.IsActive = false;
            LoginPage.pagePoint.IsEnabled = true;
            popupBox popup = new popupBox("You Are Not Authorised To Use This App");
            await popup.ShowDialog();
            App.localSettings.Values["refresh"] = null;
            App.localSettings.Values["newToken"] = null;
            App.localSettings.Values["expires"] = null;

        }
        public void successHandleForSearchByName()
        {
            Debug.WriteLine("searchHandleForSearchByName");
            Debug.WriteLine(StoreEmpowermentModel.productsearchResult[0].Products[0].Description);
            StoreEmpowermentModel.productNames.Clear();
            for (int i = 0; i < StoreEmpowermentModel.productsearchResult.Count; ++i)
            {

                StoreEmpowermentModel.productNames.Add(new productDetails(StoreEmpowermentModel.productsearchResult[i].Shelf, "", "", ""));

                for (int j = 0; j < StoreEmpowermentModel.productsearchResult[i].Products.Count; j++)
                {
                    StoreEmpowermentModel.productNames.Add(new productDetails(StoreEmpowermentModel.productsearchResult[i].Products[j].Description, StoreEmpowermentModel.productsearchResult[i].Products[j].ImageURL, StoreEmpowermentModel.productsearchResult[i].Products[j].EAN, ">"));

                }
            }
            
        }

        public void successHandleForPopulate(populateResponse res)
        {
            
            StoreEmpowermentModel.stores = new List<storeDetails>();           
            
            for (int i = 0; i < res.stores.Count; i++)
            {
                StoreEmpowermentModel.stores.Add(new storeDetails(res.stores[i].Store_Name, res.stores[i].Retail_Outlet_Number));
                              
            }
            StoreEmpowermentModel.stores.Sort();

            LoginPage.pring.IsActive = false;
            LoginPage.pagePoint.IsEnabled = true;
            if(App.localSettings.Values["stores"]!=null)
            {
                LoginPage.pagePoint.Frame.Navigate(typeof(productSearch));

            }
            else
            {
                LoginPage.pagePoint.Frame.Navigate(typeof(StoreList));

            }


        }
        public async void failureHandleForTeamAccess(string message)
        {
            Debug.WriteLine("inside failure handle for teamAccess");
            TeamAccess.pring.IsActive = false;

            popupBox popup = new popupBox(message);
            await popup.ShowDialog();

        }
        protected async void messageBox(string msg)
        {
            var msgDlg = new Windows.UI.Popups.MessageDialog(msg);
            await msgDlg.ShowAsync();
        }

        public async void successHandleForTeamAccessToggle(string response)
        {

            TeamAccess.pring.IsActive = false;
            
            Debug.WriteLine("inside success handle for teamAccessToggle");  
          
            popupBox popup = new popupBox(response);
             await popup.ShowDialog();
        }   
    }
}
