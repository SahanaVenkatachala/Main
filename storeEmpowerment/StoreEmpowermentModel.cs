using storeEmpowerment.calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    public class StoreEmpowermentModel
    {
        public StoreEmpowermentModel()
        {
            delegatees = new List<Delegatees>();

            delegatee0 = new Delegatee0();

            listResponse = new TeamAccessListResponse();

            teamAccessData = new TeamAccessData();

           

            details = new Details();

            productNames = new List<productDetails>();

            productsearchResult = new List<SearchByNameResponse>();

            stores = new List<storeDetails>();

            kpiData = new RoutineData();

            AppVersionResp = new AppVersionResponse();

            htmlResponse = "";

            productHtml = "";

            calendarResponse = "";
            tpnb = "";

            recentItems = new List<string>();

            if (App.localSettings.Values["recent"] != null)
            {
                recentItems = ((string[])App.localSettings.Values["recent"]).ToList();
            }
            else
            {
                //recentItems.Insert(0,"");
            }

        }

        /*public StoreEmpowermentModel(DelegateeList delegateeList1)
        {
            this.delegateeList1 = delegateeList1;
        }*/

        public static List<productDetails> productNames;

        public static List<string> recentItems;

        public static List<storeDetails> stores;

        public static List<Delegatees> delegatees;

        public static List<SearchByNameResponse> productsearchResult;

        public static RoutineData kpiData;

        public static Delegatee0 delegatee0;       

        public static AppVersionResponse AppVersionResp;

        public static Details details;

        public static TeamAccessListResponse listResponse;

        public static TeamAccessData teamAccessData;

        public static string response;
       
        public static string htmlResponse;

        public static string productHtml;

        public static string calendarResponse;

        public static string tpnb;
        
    }
}
