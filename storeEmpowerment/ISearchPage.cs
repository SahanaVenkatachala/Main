using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    public interface ISearchPage
    {
        void NavigateToOverview();

        void ShowRing();

        void HideRing();

        //void NavigateToHomePage();

       // void PopulateResult();
    }
}
