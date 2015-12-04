using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using storeEmpowerment;
using System.Collections.Generic;

namespace StoreEmpowermentUnit
{
    [TestClass]
    public class StoreUnitTests
    {
        [TestMethod]
        public void SearchUnitTest()
        {
            StoreEmpowermentModel model = new StoreEmpowermentModel();
            StoreEmpowermentModel.stores.Add(new storeDetails("BLACKPOOL", "2109"));
            StoreEmpowermentModel.stores.Add( new storeDetails("SANDHRUST", "3149"));
            StoreEmpowermentModel.stores.Add( new storeDetails("LONDON", "2145"));
            StoreEmpowermentModel.stores.Add( new storeDetails("WEMBLEY EXTRA", "2278"));
            StoreEmpowermentModel.stores.Add( new storeDetails("WEMBLEY METRO", "1108"));
            StoreEmpowermentModel.stores.Add( new storeDetails("BLACKPOOL CLIFTON", "3057"));
            StoreEmpowermentModel.stores.Add( new storeDetails("BRISTOL METRO", "3187"));
            StoreEmpowermentModel.stores.Add( new storeDetails("BRAINTREE TOWN CENTRE", "3197"));
            StoreEmpowermentModel.stores.Add( new storeDetails("STAPLE HILL METRO", "3198"));
            StoreEmpowermentModel.stores.Add( new storeDetails("ST HELENS METRO", "1134"));
            StoreEmpowermentModel.stores.Sort();
            List<storeDetails> searchList = new List<storeDetails>();
            StoreListPresenter presenter = new StoreListPresenter();
            presenter.StoreSearch(searchList,"WEM");
            Assert.AreEqual("Wembley Extra", searchList[0].storeName, true, "Search not sorted properly");
            Assert.AreEqual("Wembley Metro", searchList[1].storeName, true, "Matching Elements Not Displayed");
            Assert.AreEqual(2, searchList.Count, "Incorrect Search Results");
        }

        [TestMethod]
        public void GetPerUnitTest()
        {
            ListViewController list = new ListViewController();

            Assert.AreEqual(56.31, list.getPer("0.5631"), 0.02, "Parsed Double Incorrect");
            Assert.AreEqual(0.00, list.getPer(""), 0.00, "Parsed Double Incorrect For Empty String");
        }

        [TestMethod]
        public void GetColorTest()
        {
            ListViewController list = new ListViewController();
            Assert.AreEqual("#C10B0B", list.getColor("R"), true, "Wrong Color Returned");
            Assert.AreEqual("#0B780B", list.getColor("G"), true, "Wrong Color Returned");
            Assert.AreEqual("#ED9D2B", list.getColor("A"), true, "Wrong Color Returned");
        }
    }
}
