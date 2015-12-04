using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    interface ILHMView
    {
        void NavigateToLoginPage();

        void NavigateToAboutPage();

        void NavigateToSelectStorePage();

        void NavigateToSettingsPage();

        void NavigateToComplianceRoutinePage();

        void NavigateToTeamAccessPage();

    }
}
