using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    class LHMPresenter
    {
        ILHMView iView;
        public void Add(ILHMView view)
        {
            iView = view;
            
        }

        public void Logout()
        {
            App.localSettings.Values["refresh"] = null;
             System.Diagnostics.Debug.WriteLine(App.localSettings.Values["refresh"]);
            App.localSettings.Values["newToken"] = null;
            System.Diagnostics.Debug.WriteLine(App.localSettings.Values["newToken"]);
            App.localSettings.Values["expires"] = null;
           // App.localSettings.Values["timeStamp"] = "1";
            System.Diagnostics.Debug.WriteLine(App.localSettings.Values["expires"]);
            iView.NavigateToLoginPage();
        }
        public void ChangeStore()
        {
            iView.NavigateToSelectStorePage();
        }

        public void settings()
        {
            iView.NavigateToSettingsPage();
        }

        public void about()
        {
            iView.NavigateToAboutPage();
        }
       public void teamAccess()
        {
            iView.NavigateToTeamAccessPage();
        }


        public void complianceRoutine()
        {
            iView.NavigateToComplianceRoutinePage();
        }

        
        
    }
}
