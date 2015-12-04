using System;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestTools.UITest.Input;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Microsoft.VisualStudio.TestTools.UITesting.WindowsRuntimeControls;
using System.Diagnostics;
using StoreEmpowerment_Automation.NewUIMapClasses;
using StoreEmpowerment_Automation.SearchResultsClasses;



namespace StoreEmpowerment_Automation
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class AutomatedTests
    {
        public static string testResults="{";
        public AutomatedTests()
        {
            
        }
        [TestMethod]
        public void CheckDomainHidingTest()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.UIMap.DomainHidingTest();
           
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }
        [TestMethod]
        public void EmptyDomainBoxMessageTest()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.UIMap.CheckEmptyDomainMessage();
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }
        [TestMethod]
        public void EmptyPasswordTest()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.UIMap.CheckEmptyPasswordMessage();
            
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }
        [TestMethod]
        public void EmptyUsernameTest()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.UIMap.CheckEmptyUsernameMessage();
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }
        [TestMethod]
        public void IncorrectCredentialsTest()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.UIMap.CheckIncorrectCredentials();
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }
        [TestMethod]
        public void CorrectCredentialsFirstTimeTest()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.UIMap.CheckCorrectCredentials();
            myAppWindow.Close();
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }
        [TestMethod]
        public void SelectStoreTest()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.UIMap.SelectStore();
            if (testResults[testResults.Length - 1] != '}')
            {
                AutomatedTests.testResults += "\"PASS\"}";
            }
            
            myAppWindow.Close();
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        }
        [TestMethod]
        public void HomeTest()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.NewUIMap.homeScreenCheck();
            myAppWindow.Close();
        }
        [TestMethod]
        public void RecentItemsCheck()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.NewUIMap.RecentItemsTabClick();
            myAppWindow.Close();

        }
        
        public void SearchResultsCheck()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_8v4sy2eer3gzg!App");
            this.NewUIMap.SearchResultsTabClick();           
            myAppWindow.Close();
        }

   
        [TestMethod]
        public void searchchk()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.SearchResults.searchTest();
            myAppWindow.Close();
            
        }
        [TestMethod]
        public void CancelTest()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.SearchResults.CancelTest();
            myAppWindow.Close();
        }
       /*        [TestMethod]
        public void SearchIconClick()
        {
            XamlWindow myAppWindow = XamlWindow.Launch("ab398442-2b8f-465c-bfa9-6c897ddfffe5_nt9jg1w5dazhj!App");
            this.NewUIMap.SearchClick();
            myAppWindow.Close();
        }*/
        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }
        public NewUIMap NewUIMap
        {
            get
            {
                if ((this.newMap == null))
                {
                    this.newMap = new NewUIMap();
                }

                return this.newMap;
            }
        }

        public SearchResults SearchResults
        {
            get
            {
                if ((this.searchResults == null))
                {
                    this.searchResults = new SearchResults();
                }

                return this.searchResults;
            }
        }
        private UIMap map;
        private NewUIMap newMap;

        private SearchResults searchResults;
    }
}
