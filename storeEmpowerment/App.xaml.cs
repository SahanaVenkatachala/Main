using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace storeEmpowerment
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        public static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private TransitionCollection transitions;
        public static string appVersion;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            this.Resuming += this.App_Resuming;
            StoreEmpowermentModel model = new StoreEmpowermentModel();
            appVersion = string.Format("{0}.{1}.{2}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build);
            Debug.WriteLine(appVersion);
        }
        
        async void App_Resuming(object sender, object e)
        {
            Frame frame = Window.Current.Content as Frame;

            NetworkManager manager = NetworkManager.Instance;
            Task appVersionTask = manager.AppVersionRequest();
            await appVersionTask;
            if (StoreEmpowermentModel.AppVersionResp.LatestApplicationVersion == null)
            {
                frame.Navigate(typeof(LoginPage));   
            }
            else if (appVersion != StoreEmpowermentModel.AppVersionResp.LatestApplicationVersion)
            {
                popupBox popup = new popupBox("Please update");
                popup.Message();
                frame.Navigate(typeof(LoginPage));   
            }

            else if ((string)localSettings.Values["currentPage"] == "kpi")
            {
                storeRoutine.getData();
               // productSearch.getData();

            }
            //Home.getData();
           
        }

        
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            //CrittercismSDK.Crittercism.Init("556edcb3d4c2452f5c33bd51");
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;
            NetworkManager manager = NetworkManager.Instance;
            Task appVersionTask = manager.AppVersionRequest();
            await appVersionTask;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                localSettings.Values["locationName"] = "friendly";
                localSettings.Values["tab"] = "recentItems";
                localSettings.Values["type"] = "";
               // localSettings.Values["timeStamp"]="";
               // localSettings.Values["calAccess"] = StoreEmpowermentModel.details.calAccess;
               
               // localSettings.Values["newToken"] = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VybmFtZSI6InMyMDAxaW5mb3JtIiwiZG9tYWluIjoiVUtST0kiLCJkZXZpY2VJZCI6ImViNjRmZGMzLThjZDgtNDcxYy1iZmY1LThiYmQxYjI3ZmMiLCJyb2xlIjoiIiwiYWNjZXNzX3Rva2VuIjoiZXlKMGVYQWlPaUpLVjFRaUxDSmhiR2NpT2lKSVV6STFOaUo5LmV5SnliMnhsSWpwYklsVnpaWElpTENKaFpDSmRMQ0oxYzJWeWJtRnRaU0k2SW5NeU1EQXhhVzVtYjNKdElpd2lZMnhwWlc1MElqb2lhVzVtYjNKdExtMXZZbWxzWlM1aGNIQnpJaXdpWkdWMmFXTmxTV1FpT2lKbFlqWTBabVJqTXkwNFkyUTRMVFEzTVdNdFltWm1OUzA0WW1Ka01XSXlOMlpqSWl3aVpHVjJhV05sVG1GdFpTSTZJazVsZUhWeklEVWlMQ0prYjIxaGFXNGlPaUpWUzFKUFNTSXNJbWx6Y3lJNkluTmxiR1lpTENKaGRXUWlPaUpvZEhSd2N6b3ZMMnhoWW5NdWIyTnpaWFF1Ym1WMEwybHVabTl5YlNJc0ltVjRjQ0k2TVRRME1USXlOalU0Tnl3aWJtSm1Jam94TkRReE1UazNOemczZlEuT1hNWHdScjg0bkJHcXhHQUVhaDl5M25tTUJuVVBQUmdiMXM2cUJ2VzJQOCIsImlhdCI6MTQ0MTE5Nzc4NywiZXhwIjoxNDQxMjI2NTg3fQ.d7MQouTP-qUyML_9Kv6Vcu76IhuIQk9F91ur2lPdafc";
             

                

                try
                 {
                     if (localSettings.Values["name1"] == null)
                     {
                         localSettings.Values["name1"] = "";

                     }
                 }
                catch(Exception e0)
                 {

                 }
                 try
                 {
                     if (localSettings.Values["name2"] == null)
                     {
                         localSettings.Values["name2"] = "";

                     }
                 }
                catch(Exception e1)
                 {

                 }
                 try
                 {
                     if (localSettings.Values["name3"] == null)
                     {
                         localSettings.Values["name3"] = "";

                     }
                 }
               catch(Exception e2)
                 {

                 }
                 try
                 {
                     if (localSettings.Values["name4"] == null)
                     {
                         localSettings.Values["name4"] = "";

                     }
                 }
                 catch(Exception e3)
                 {
                 
                 }
               

                //localSettings.Values["storename"] = "";
                Debug.WriteLine("going to check "+ appVersion + "         "+ StoreEmpowermentModel.AppVersionResp.LatestApplicationVersion);
                localSettings.Values["searchCheck"] = "inactive";
                if (localSettings.Values["mode"] == null)
                {
                    localSettings.Values["mode"] = "cs";
                }
                if (StoreEmpowermentModel.AppVersionResp.LatestApplicationVersion == null)
                {
                    if (!rootFrame.Navigate(typeof(LoginPage), e.Arguments))   //LoginPage
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
                else if (appVersion != StoreEmpowermentModel.AppVersionResp.LatestApplicationVersion)
                {
                    popupBox popup = new popupBox("Please update");
                    popup.Message();
                    if (!rootFrame.Navigate(typeof(LoginPage), e.Arguments))   //LoginPage
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
                else
                {
                    if (localSettings.Values["newToken"] == null)
                    {
                        if (!rootFrame.Navigate(typeof(LoginPage), e.Arguments))   //LoginPage
                        {
                            throw new Exception("Failed to create initial page");
                        }

                    }
                    else if (localSettings.Values["stores"] == null)
                    {
                        if (!rootFrame.Navigate(typeof(StoreList), e.Arguments))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }
                    else
                    {

                        if (!rootFrame.Navigate(typeof(productSearch), e.Arguments))  //storeRoutine
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }
                }
                
                
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            Debug.WriteLine("yufgyvguigui");
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
        
    }
}