using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    public class SearchProductbyNamePresenter
    {
        ISearchByNameList isearchbynamelist;

        public void Add(ISearchByNameList store)
        {
            isearchbynamelist = store;
        }

        public void populateResult()
        {
            isearchbynamelist.SetSearchByNameListSource();
        }

    }
}
