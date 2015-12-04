using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    public class SearchPresenter
    {
        private ISearchPage iView;
        private ISearchByNameList isearch;

        public void add(ISearchPage iObject){
            Debug.WriteLine("inside 12 12"+ iObject);
            iView = iObject;
            if (iView == null)
            {
                Debug.WriteLine("inside 12 13");
            }
        }

        public void searchSuccess(){
            iView.HideRing();
            iView.NavigateToOverview();
        }

        public void searchByNameSuccess()
        {
           
            isearch.SetSearchByNameListSource();
            iView.HideRing();
            isearch.NavigateToHomePage();
        }
        public void failSearch()
        {
            iView.HideRing();
        }
    }
}
