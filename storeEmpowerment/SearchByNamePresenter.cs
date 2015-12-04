using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    public class SearchByNamePresenter
    {
        ISearchByNameList searchbynamelist;

        public void Add(ISearchByNameList store)
        {
            searchbynamelist = store;
        }
    }
}
