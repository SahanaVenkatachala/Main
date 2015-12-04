using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace storeEmpowerment
{
    public class StoreListPresenter
    {
        IStoreList iStore;
        
        public void Add(IStoreList store)
        {
            iStore = store;
        }

        public void StoreSearch(List<storeDetails> searchList, string searchKey)
        {
            for (int i = 0; i < StoreEmpowermentModel.stores.Count(); i++)
            {
                //Debug.WriteLine(stores[i].storeName.Contains(searchKey) + "  " + searchKey);
                if (StoreEmpowermentModel.stores[i].storeNumber.Contains(searchKey) || StoreEmpowermentModel.stores[i].storeName.Contains(searchKey.ToUpper()))
                {
                    searchList.Add(StoreEmpowermentModel.stores[i]);
                }
            }
            
        }

        public async void FetchStoresFromFile()
        {
            StorageFolder cache = Windows.Storage.ApplicationData.Current.LocalFolder;

            StorageFile storeFile = await cache.CreateFileAsync("stores.txt", CreationCollisionOption.OpenIfExists);
            string data = await Windows.Storage.FileIO.ReadTextAsync(storeFile);
            if (data != "")
            {
                populateResponse res = new populateResponse();
                res = NetworkManager.jsonDeserialize<populateResponse>(data);
                for (int i = 0; i < res.stores.Count; i++)
                {
                    //Debug.WriteLine(res.stores[i].Store_Name);
                    StoreEmpowermentModel.stores.Add(new storeDetails(res.stores[i].Store_Name, res.stores[i].Retail_Outlet_Number));

                }
                iStore.SetListSource(StoreEmpowermentModel.stores);
               
            }
        }

    }
}
