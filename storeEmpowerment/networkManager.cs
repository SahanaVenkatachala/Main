
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
//using Windows.Web.Http;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;
using Windows.Storage;
using storeEmpowerment.calendar;
namespace storeEmpowerment
{
    class NetworkManager
    {

        private static Uri ServerBaseUri
        {

            get { return new Uri("https://labs.ocset.net/smapp/v2"); }
        }

        private static NetworkManager networkManager;

        private System.Net.Http.HttpClient httpClient;


        private NetworkManager()
        {
            httpClient = new System.Net.Http.HttpClient() { MaxResponseContentBufferSize = int.MaxValue };
        }
        public static NetworkManager Instance
        {
            get
            {
                if (networkManager == null)
                {
                    networkManager = new NetworkManager();

                }

                return networkManager;
            }

        }
        public static StringContent jsonSerializer<T>(T dataObject)
        {
            Debug.WriteLine("serialize");
            DataContractJsonSerializer objToJson = new DataContractJsonSerializer(typeof(T));

            MemoryStream memoryStream = new MemoryStream();
            objToJson.WriteObject(memoryStream, dataObject);
            memoryStream.Position = 0;
            var stream = new StreamReader(memoryStream);

            string postData = stream.ReadToEnd();
            Debug.WriteLine("ser" + postData);
            StringContent json = new StringContent(postData);
            json.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            return json;
        }

        public static T jsonDeserialize<T>(string jsonString)
        {
            Debug.WriteLine("inside deserialiser");
            Debug.WriteLine("deser object....." + jsonString);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            Debug.WriteLine("deser" + obj);
            return obj;
        }

        public async Task<RoutineData> RoutineDataSettingsAsync(string userName, string accessToken, string Retail_outlet_no)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
                ResponseHandler handler = new ResponseHandler();
                Debug.WriteLine("inside setting get store kpi");
                GetStoreKPI getstore = new GetStoreKPI(userName, accessToken, Retail_outlet_no);
                RoutineData routinedata = new RoutineData();
                var response = await this.httpClient.GetAsync(new Uri("https://labs.ocset.net/smapp/v2/validate/kpi/getStoreKpiV2/" + App.localSettings.Values["stores"] + "?noCache=" + DateTime.Now.ToString("yyyyMMddHHmmssffff")));
                Debug.WriteLine(response);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    Settings.failureHandleForKpi("No Data to be Displayed");

                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    routinedata = jsonDeserialize<RoutineData>(res);

                    Settings.successHandlesettingsCall(routinedata);
                     
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Debug.WriteLine("inside failure");
                    handler.failureHandleForKpi("Please Try After Some Time");
                    return new RoutineData();
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    handler.handleUnauthorizedForKpi();

                }
                else
                {
                    Debug.WriteLine(response.StatusCode);
                    handler.failureHandleForKpiAbout("Please Check Network Connection");
                    return new RoutineData();
                }
                return routinedata;
            }
            catch (Exception)
            {
                return new RoutineData();
            }


        }


        public async Task<RoutineData> RoutineDataAsync(string userName, string accessToken, string Retail_outlet_no)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
                ResponseHandler handler = new ResponseHandler();
                Debug.WriteLine("inside get store kpi");
                GetStoreKPI getstore = new GetStoreKPI(userName, accessToken, Retail_outlet_no);
                RoutineData routinedata = new RoutineData();
                var response = await this.httpClient.GetAsync(new Uri("https://labs.ocset.net/smapp/v2/validate/kpi/getStoreKpiV2/" + App.localSettings.Values["stores"] + "?noCache=" + DateTime.Now.ToString("yyyyMMddHHmmssffff")));
                Debug.WriteLine("the kpi response is " + response + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    handler.failureHandleForKpi("No Data to be Displayed");
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    Debug.WriteLine("inside okkk");
                    string res = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(res);
                    routinedata = jsonDeserialize<RoutineData>(res);
                    storeRoutine.successHandleforRoutineData(routinedata);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Debug.WriteLine("inside failure");
                    handler.failureHandleForKpi("Please Try After Some Time");
                    return new RoutineData();
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    handler.handleUnauthorizedForKpi();
                }
                else
                {
                    handler.failureHandleForKpi("Please Check Network Connection");
                    return new RoutineData();
                }
                return routinedata;
            }
            catch (Exception)
            {
                return new RoutineData();
            }


        }

        public async Task GetAllStoresAsync(string userName)
        {
            try
            {
                ResponseHandler handler = new ResponseHandler();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
                // string token = App.localSettings.Values["token"].ToString();
                string newToken = App.localSettings.Values["newToken"].ToString();
                string data = "{\"UserName\" : ";
                // access_token
                string temp1 = ",\"access_token\" : ";
                string temp = "\"}";
                data = data + '"' + userName + '"' + temp1 + '"' + newToken + temp;
                Debug.WriteLine("get store list call");
                populateResponse populateResponse = new populateResponse();
                Debug.WriteLine("storeroutine289787   " + data);
                StringContent json = new StringContent(data);
                json.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                Debug.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                var response = await this.httpClient.GetAsync(new Uri("https://labs.ocset.net/smapp/v2/validate/kpi/get_all_stores_v2" + "?noCache=" + DateTime.Now.ToString("yyyyMMddHHmmssffff")));
                Debug.WriteLine("response of store lise" + response);
               // App.localSettings.Values["timeStamp"] = "1";
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    handler.noDataForPopulate();
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    Debug.WriteLine("inside httpstatus code OK");
                    string res = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(res);

                    populateResponse = jsonDeserialize<populateResponse>(res);
                    handler.successHandleForPopulate(populateResponse);
                    StorageFolder cache = Windows.Storage.ApplicationData.Current.LocalFolder;
                    StorageFile storeFile = await cache.CreateFileAsync("stores.txt", CreationCollisionOption.OpenIfExists);
                    await Windows.Storage.FileIO.WriteTextAsync(storeFile, res);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    handler.failureHandleForPopulate("Please Try After Some Time");
                }

                else
                {
                    App.localSettings.Values["refresh"] = null;
                    App.localSettings.Values["newToken"] = null;
                    App.localSettings.Values["expires"] = null;
                    handler.failureHandle("Please Check Your Network Connection");
                }


            }
            catch (Exception)
            {

            }

        }

        public async Task GetProductAsync(string productNumber)
        {
            Debug.WriteLine("inside prod details");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
            Debug.WriteLine("here auth" + httpClient.DefaultRequestHeaders.Authorization);
            var response = await this.httpClient.GetAsync(new Uri("https://labs.ocset.net/smapp/v2/validate/products/details/" + App.localSettings.Values["stores"] + "/" + productNumber + "?noCache=" + DateTime.Now.ToString("yyyyMMddHHmmssffff")));
            Debug.WriteLine("Response : !!!!!! " + response);
            ResponseHandler handler = new ResponseHandler();
            Debug.WriteLine("response code" + response.StatusCode);
            //Debug.WriteLine("Response : !!!!!! " + response);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine("got response");
                string html = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Json is  : " + html);
                if (html.Length < 300)
                {
                    handler.failureHandleForSearch("Item " + productNumber + " could not be found in store " + (string)App.localSettings.Values["stores"] + ".\nThe product may not be ranged in this store or an invalid product number has been entered.");
                }
                else
                {
                    try
                    {

                        StoreEmpowermentModel.details = jsonDeserialize<Details>(html);

                    }
                    catch (Exception)
                    {

                    }

                    handler.successHandleForSearch();
                }

                //Debug.WriteLine(StoreEmpowermentModel.htmlResponse);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                handler.failureHandleForSearch("Item " + productNumber + " could not be found in store " + (string)App.localSettings.Values["stores"] + ".\nThe product may not be ranged in this store or an invalid product number has been entered.");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
               // App.localSettings.Values["refresh"] = null;
               // App.localSettings.Values["newToken"] = null;
               // App.localSettings.Values["expires"] = null;
                App.localSettings.Values["refresh"] = null;
                App.localSettings.Values["newToken"] = null;
                App.localSettings.Values["expires"] = null;
                handler.UnauthSearch();
               // handler.handleUnauthorizedForSearch("product",productNumber);
            }
            else
            {
                handler.failureHandleForSearch("Please Try Again After Some Time");
            }
        }
        public async Task<string> GetCalendarAsync(string productNumber)
        {
            //Debug.WriteLine("inside calendar call");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
            //Debug.WriteLine(httpClient.DefaultRequestHeaders.Authorization);
            Debug.WriteLine("https://labs.ocset.net/inform/store/" + App.localSettings.Values["stores"] + "/products/" + StoreEmpowermentModel.tpnb + "/orders?noCache=" + DateTime.Now.ToString("yyyyMMddHHmmssffff"));
            var response = await this.httpClient.GetAsync(new Uri("https://labs.ocset.net/smapp/v2/validate/storeorder/" + App.localSettings.Values["stores"] + "/" + productNumber + "?noCache=" + DateTime.Now.ToString("yyyyMMddHHmmssffff")));
            ResponseHandler handler = new ResponseHandler();
            //Debug.WriteLine("network :"+response);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string res = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("the calendar response is" + res);
                //Debug.WriteLine(res);
                return res;
                //Debug.WriteLine("got response");
                //string html = await response.Content.ReadAsStringAsync();
                //StoreEmpowermentModel.htmlResponse = handler.processString(html);
                //handler.successHandleForSearch();
                //Debug.WriteLine(StoreEmpowermentModel.htmlResponse);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return "";
                //handler.failureHandleForSearch("Item " + productNumber + " could not be found in store " + (string)App.localSettings.Values["stores"] + ".\nThe product may not be ranged in this store or an invalid product number has been entered.");
            }
            else
            {
                return "";
                //handler.failureHandleForSearch("Please Try Again After Some Time");
            }
        }

        public async Task<string> RevertCalendarAsync(string revertUrl, string req)
        {
            Debug.WriteLine(revertUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);

            Debug.WriteLine(revertUrl);
            StringContent json = new StringContent(req);
            json.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = await this.httpClient.PutAsync(new Uri(revertUrl), json);
            ResponseHandler handler = new ResponseHandler();
            Debug.WriteLine("the revert code is" + response.StatusCode);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return "";
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                await ProductOverview.web.InvokeScriptAsync("SetRev", new string[] { });
                popupBox popup = new popupBox("ERROR You can only amend orders in your store");
                await popup.ShowDialog();
                return "";
            }
            else
            {
                Debug.WriteLine("revert");
                popupBox popup = new popupBox("Uh Oh... Something Went Wrong");
                await popup.ShowDialog();
                return "";
            }
        }
        public async Task<string> SaveCalendarAsync(string saveUrl, string req)
        {
            Debug.WriteLine(saveUrl + "!!!!!!!");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
            // httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            Debug.WriteLine("calendar request is : " + req);

            StringContent json = new StringContent(req);
            Debug.WriteLine(json);
            json.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = await this.httpClient.PostAsync(new Uri(saveUrl), json);
            Debug.WriteLine("response+++++++++++++++++++++++++++ " + response);
            ResponseHandler handler = new ResponseHandler();

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return "";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //await ProductOverview.web.InvokeScriptAsync("SetRev", new string[] { });
                popupBox popup = new popupBox("ERROR You can only amend orders in your store");
                await popup.ShowDialog();
                return "";
            }
            else
            {
                Debug.WriteLine("posttttttttttttttttttttttttttttttttttttttttttttttt");
                popupBox popup = new popupBox("Uh Oh... Something Went Wrong");
                await popup.ShowDialog();
                return "";
            }
        }
        public async Task GetProductByName(string productName)
        {
            Debug.WriteLine("Inside call of product name");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
            List<SearchByNameResponse> searchbynameResponse = new List<SearchByNameResponse>();
            var response = await this.httpClient.GetAsync(new Uri("https://labs.ocset.net/smapp/v2/validate/products/search/" + productName + "?format=grouped & noCache=" + DateTime.Now.ToString("yyyyMMddHHmmssffff")));
            Debug.WriteLine("the response is " + response);
            ResponseHandler handler = new ResponseHandler();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine("got response");
                string res = await response.Content.ReadAsStringAsync();
               
                if (res == "[]")
                {
                    handler.failureHandleForSearch("Could not find any products that match your search criteria.\n\nPlease try a different search term");
                }
                else
                {
                    StoreEmpowermentModel.productsearchResult = jsonDeserialize<List<SearchByNameResponse>>(res);
                    Debug.WriteLine("\nDeserialized");
                    //StoreEmpowermentModel.productNames.Clear();
                    handler.successHandleForSearchByName();
                }

            }
            else if(response.StatusCode==HttpStatusCode.Unauthorized)
            {
               // handler.handleUnauthorizedForSearch("product",productName);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                handler.failureHandleForSearch("Item " + productName + " could not be found in store " + (string)App.localSettings.Values["stores"] + ".\nThe product may not be ranged in this store or an invalid product number has been entered.");
            }

            else
            {
                handler.failureHandleForSearch("Please Try Again After Some Time");
            }


        }
        public async Task StoreRoutineAsync(string userName, string accessToken, int expiryDate)
        {
            try
            {
                ResponseHandler handler = new ResponseHandler();
                Debug.WriteLine("storeroutine");
                PopulateData populateData = new PopulateData(userName, accessToken, expiryDate);
                Debug.WriteLine("storeroutine1");
                populateResponse populateResponse = new populateResponse();
                Debug.WriteLine("storeroutine289787");
                var response = await this.httpClient.PostAsync(new Uri("https://labs.ocset.net/smapp/kpi/populate"), jsonSerializer(populateData));
                Debug.WriteLine(response);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    handler.noDataForPopulate();
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    Debug.WriteLine("inside httpstatus code OK");
                    string res = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(res);
                    populateResponse = jsonDeserialize<populateResponse>(res);
                    handler.successHandleForPopulate(populateResponse);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    handler.failureHandleForPopulate("Please Try After Some Time");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    App.localSettings.Values["refresh"] = null;
                    App.localSettings.Values["newToken"] = null;
                    App.localSettings.Values["expires"] = null;
                    handler.failureHandle("Please Check Your Network Connection");
               
                   // handler.handleUnauthorized();
                }
                else
                {
                    App.localSettings.Values["refresh"] = null;
                    App.localSettings.Values["newToken"] = null;
                    App.localSettings.Values["expires"] = null;
                    handler.failureHandle("Please Check Your Network Connection");
                }


            }
            catch (Exception)
            {

            }

        }

        public async Task AppVersionRequest()
        {
            try
            {
                ResponseHandler handler = new ResponseHandler();
                AppVersionRequest appVersionRequest = new AppVersionRequest();
                //AppVersionResponse appVersionResponse= new AppVersionResponse();
                var response = await this.httpClient.GetAsync(new Uri("https://labs.ocset.net/smapp/v2/getappversion/windows" + "?noCache=" + DateTime.Now.ToString("yyyyMMddHHmmssffff")));
                Debug.WriteLine("the response is " + response + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("inside get api" + res);
                    StoreEmpowermentModel.AppVersionResp = jsonDeserialize<AppVersionResponse>(res);
                    handler.successHandleForAppVersion();
                }

            }
            catch (Exception e)
            {

            }
        }

        public async Task loginRequestAsync(string userName, string passWord, string domainselected)
        {
            try
            {
                ResponseHandler handler = new ResponseHandler();
                LoginData loginData = new LoginData(userName, passWord, domainselected);
                LoginResponse loginResponse = new LoginResponse();
                Debug.WriteLine("login response" + loginResponse);
                var response = await this.httpClient.PostAsync(new Uri("https://labs.ocset.net/smapp/v2/auth/token"), jsonSerializer(loginData));
                Debug.WriteLine("awrgrtshrst " + response);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    loginResponse = jsonDeserialize<LoginResponse>(res);
                    handler.successHandleForLogin(loginResponse);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    handler.failureHandle("Username/Password Not Verified");
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    handler.failureHandle("Too many devices" + " " + "You are using 2 devices already" + " " + " Devices expire 8 hours after last accessing Inform/Availability");
                }
                else
                {
                    handler.failureHandle("Please Check Your Network Connection");
                }
            }
            catch (Exception)
            {

            }

        }
        public async Task refreshRequestAsync(LoginRequest loginRequest)
        {

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
                ResponseHandler handler = new ResponseHandler();
                LoginResponse loginResponse = new LoginResponse();
                Debug.WriteLine(loginRequest.refresh_token + "*************************");
                var response = await this.httpClient.PostAsync(new Uri("https://labs.ocset.net/smapp/v2/auth/refresh"), jsonSerializer(loginRequest));
                Debug.WriteLine(response);
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    string res = await response.Content.ReadAsStringAsync();
                    loginResponse = jsonDeserialize<LoginResponse>(res);
                    handler.successHandleForRefresh(loginResponse);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    handler.failureHandleForRefresh();
                }
                else
                {
                    handler.failureHandleForRefresh();
                }
            }
            catch (Exception)
            {

            }

        }
        
       public async Task refreshRequestAsync(RefreshRequest refreshRequest, string pageName)
        {

              try
              {
                  httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
                  ResponseHandler handler = new ResponseHandler();
                  LoginResponse loginResponse = new LoginResponse();

                  Debug.WriteLine(refreshRequest.refresh_token + "*************************");
                  var response = await this.httpClient.PostAsync(new Uri("https://labs.ocset.net/smapp/v2/auth/refresh"), jsonSerializer(refreshRequest));
                  Debug.WriteLine(response);
                 
                  if (response.StatusCode == HttpStatusCode.OK)
                  {
                      if(pageName.Equals("StoreRoutine"))
                      {
                          //storeRoutine StoreRoutine = new storeRoutine();
                          storeRoutine.getData();
                      }
                      if(pageName.Equals("About"))
                      {
                          
                         // Settings settings = new Settings();
                          Settings.getData();
                      }
                      if(pageName.Equals("teamAccess"))
                      {
                          TeamAccess.getData();
                      }
                      string res = await response.Content.ReadAsStringAsync();
                      loginResponse = jsonDeserialize<LoginResponse>(res);
                      handler.successHandleForRefresh(loginResponse);
                      
                  }
                  else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
                  {
                      handler.failureHandleForRefresh();
                  }
                  else
                  {
                      handler.failureHandleForRefresh();
                  }
              }
              catch (Exception)
              {

              }

          }
          public async Task refreshRequestAsyncForProduct(LoginRequest loginRequest, string productId,string pageName)
          {

              try
              {
                  httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
                  ResponseHandler handler = new ResponseHandler();
                  LoginResponse loginResponse = new LoginResponse();
                  Debug.WriteLine(loginRequest.refresh_token + "*************************");
                  var response = await this.httpClient.PostAsync(new Uri("https://labs.ocset.net/smapp/v2/auth/refresh"), jsonSerializer(loginRequest));
                  Debug.WriteLine(response);
                  
                  if (response.StatusCode == HttpStatusCode.OK)
                  {
                      //productSearch productsearch = new productSearch();
                      if(pageName.Equals("product"))
                      { 
                         productSearch.getData(productId);
                      }
                      if(pageName.Equals("getName"))
                      {
                          TeamAccess.getData(productId);
                      }
                      string res = await response.Content.ReadAsStringAsync();
                      loginResponse = jsonDeserialize<LoginResponse>(res);
                      handler.successHandleForRefresh(loginResponse);
                  }
                  else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
                  {
                      handler.failureHandleForRefresh();
                  }
                  else
                  {
                      handler.failureHandleForRefresh();
                  }
              }
              catch (Exception)
              {

              }

          }

          public async Task refreshRequestAsyncTeamAccessToggle(LoginRequest loginRequest, string pageName, string state, string employeeName)
          {

              try
              {
                  httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
                  ResponseHandler handler = new ResponseHandler();
                  LoginResponse loginResponse = new LoginResponse();
                  Debug.WriteLine(loginRequest.refresh_token + "*************************");
                  var response = await this.httpClient.PostAsync(new Uri("https://labs.ocset.net/smapp/v2/auth/refresh"), jsonSerializer(loginRequest));
                  Debug.WriteLine(response);

                  if (response.StatusCode == HttpStatusCode.OK)
                  {
                      if (pageName.Equals("toggle"))
                      {
                          //productSearch productsearch = new productSearch();
                          TeamAccess.getData("toggle",state,employeeName);

                      }
                      if(pageName.Equals("cancel"))
                      {
                          TeamAccess.getData("cancel",state, employeeName);
                      }
                      string res = await response.Content.ReadAsStringAsync();
                      loginResponse = jsonDeserialize<LoginResponse>(res);
                      handler.successHandleForRefresh(loginResponse);
                  }
                  else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
                  {
                      handler.failureHandleForRefresh();
                  }
                  else
                  {
                      handler.failureHandleForRefresh();
                  }
              }
              catch (Exception)
              {

              }

          }
         
        public async Task GetNameAsync(string employeeName)
        {
            
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
            
            Debug.WriteLine("auth : " + httpClient.DefaultRequestHeaders.Authorization);
            TeamAccess teamAccess = new TeamAccess();
            ResponseHandler handler = new ResponseHandler();
            TeamAccessData teamAccessData = new TeamAccessData(employeeName, (string)App.localSettings.Values["stores"], "y");
            
            Debug.WriteLine("auth 2: " + httpClient.DefaultRequestHeaders.Authorization);
            var response = await this.httpClient.PostAsync(new Uri("https://labs.ocset.net/smapp/v2/validate/delegate/upsert"), jsonSerializer(teamAccessData));
            Debug.WriteLine("response @@@@@ name" + response);
            Debug.WriteLine("response is" + response+response.StatusCode);
            if (response.StatusCode == HttpStatusCode.OK )
            {
                string res = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("response is.............."+res);
                Debug.WriteLine("response aftr deserialization"+" "+ res);
                TeamAccess.pring.IsActive = false;
                teamAccess.successHandleForTeamAccessUpsert(employeeName);

            }
               
            else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                handler.failureHandleForRefresh();
               // handler.handleUnauthorizedForSearch("getName", employeeName);
            }
            else
            {
                handler.failureHandleForTeamAccess("Please check your internet connection");
               
            }

        }

        public async Task GetCancelRequestAsync(string store,string name)
        {
            
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
            TeamAccess teamAccess = new TeamAccess();           
            ResponseHandler handler = new ResponseHandler();
            TeamAccessDeleteData teamAccessDeleteData = new TeamAccessDeleteData(name, store);
            Uri cancel = new Uri("https://labs.ocset.net/smapp/v2/validate/delegate/delete_windows");
            Debug.WriteLine("URI" + cancel);

            var response = await this.httpClient.PostAsync(cancel, jsonSerializer(teamAccessDeleteData));
            Debug.WriteLine("Response" + response);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string res = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(res + "response");
                TeamAccess.pring.IsActive = false;

                 teamAccess.successHandleForTeamAccessCancel(res);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                handler.failureHandleForRefresh();
               // handler.refreshCallTeamAccessToggle("cancel", store, name);
            }
            else
            {
                handler.failureHandleForTeamAccess("Please check your internet connection");

            }
        }

        public async Task GetToggleRequestAsync(string state,string employeeName)
        {
            Debug.WriteLine("inside toggle check state" + state);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
            ResponseHandler handler = new ResponseHandler();
            TeamAccess teamAccess = new TeamAccess();
            TeamAccessData teamAccessData = new TeamAccessData(employeeName, (string)App.localSettings.Values["stores"], state);
            Debug.WriteLine("auth 2: " + httpClient.DefaultRequestHeaders.Authorization);
            var response = await this.httpClient.PostAsync(new Uri("https://labs.ocset.net/smapp/v2/validate/delegate/upsert"), jsonSerializer(teamAccessData));

            Debug.WriteLine("Response @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@" + response+response.StatusCode);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string res = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("@@@@ REsponse" + res);
                teamAccess.successHandleForTeamAccessToggle(res);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                handler.failureHandleForRefresh();
               // handler.refreshCallTeamAccessToggle("toggle",state,employeeName);
            }
            else
            {
                handler.failureHandleForTeamAccess("Please check your internet connection");


            }
        }

        public async Task refreshRequestAsyncTeamAccess(RefreshRequest refreshRequest)
        {

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
                ResponseHandler handler = new ResponseHandler();
                LoginResponse loginResponse = new LoginResponse();

                Debug.WriteLine(refreshRequest.refresh_token + "*************************");
                var response = await this.httpClient.PostAsync(new Uri("https://labs.ocset.net/smapp/v2/auth/refresh"), jsonSerializer(refreshRequest));
                Debug.WriteLine(response);

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    TeamAccess.getData();

                    string res = await response.Content.ReadAsStringAsync();
                    loginResponse = jsonDeserialize<LoginResponse>(res);
                    handler.successHandleForRefresh(loginResponse);

                }
                else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    handler.failureHandleForRefresh();
                }
                else
                {
                    handler.failureHandleForRefresh();
                }
            }
            catch (Exception)
            {

            }

        }
        public async Task TeamAccessAsyncGetList()
        {
            try
            {
                TeamAccess teamAccess = new TeamAccess();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue((string)App.localSettings.Values["newToken"]);
                Debug.WriteLine("inside teamAccess Async");
                ResponseHandler handler = new ResponseHandler();
                TeamAccessListResponse teamAccessListResponse = new TeamAccessListResponse();
                var response = await this.httpClient.GetAsync(new Uri("https://labs.ocset.net/smapp/v2/validate/delegate/list_windows/store" + "/" + App.localSettings.Values["stores"] + "?noCache=" + DateTime.Now.ToString("yyyyMMddHHmmssffff")));
                Debug.WriteLine(response + "response is");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Debug.WriteLine("inside successhandler for teamAccess");
                    string res = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Response iss" + res);
                    teamAccessListResponse = jsonDeserialize<TeamAccessListResponse>(res);
                    Debug.WriteLine("the team access list is "+res);
                    Debug.WriteLine("team list"+teamAccessListResponse.delegatees);
                    teamAccess.successHandelForTeamAccess(teamAccessListResponse);                   
                    
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Debug.WriteLine(response.ReasonPhrase);
                    handler.failureHandleForTeamAccess("Please try after some time ");

                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    handler.handelUnauthorizedForTeamAccess();
                }
                else if(response.ReasonPhrase.Equals("Unprocessable Entity"))
                {

                    string res = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Response********************************" + res);
                    teamAccess.successHandelForTeamAccess();
                    StoreEmpowermentModel.listResponse.storeManager = false;
                }
                else
                {
                    handler.failureHandleForTeamAccess("Please check your internet connection");

                }



            }
            catch (Exception e)
            {

            }
        }
       
    }


    
}
