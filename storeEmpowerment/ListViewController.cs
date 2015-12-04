using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace storeEmpowerment
{
    public class ListViewController
    {
        public ListView listView;

        public ListViewController()
        {

        }

        public ListViewController(ListView list)
        {
            listView = list;
        }
        
        public double getPer(string value)
        {
            double doublevalue;
            if (value == "")
                return (0.00);

            else
            {

                doublevalue = Math.Round(Double.Parse(value) * 100, 1);
                return doublevalue;
            }
                
        }

        private void constructData(ThisQuarter qtr)
        {
            try
            {
                
                listView.Items.Add(new ListViewData() { kpiData = qtr.metric_1_description, kpiPer = getPer(qtr.metric_1) + "%", color = getColor(qtr.metric_1_RAG) });
                listView.Items.Add(new ListViewData() { kpiData =qtr.metric_2_description, kpiPer = getPer(qtr.metric_2) + "%", color = getColor(qtr.metric_2_RAG) });
                listView.Items.Add(new ListViewData() { kpiData = qtr.metric_3_description, kpiPer = getPer(qtr.metric_3) + "%", color = getColor(qtr.metric_3_RAG) });
                listView.Items.Add(new ListViewData() { kpiData = qtr.metric_4_description, kpiPer = getPer(qtr.metric_4) + "%", color = getColor(qtr.metric_4_RAG) });
                listView.Items.Add(new ListViewData() { kpiData = qtr.metric_5_description, kpiPer = getPer(qtr.metric_5) + "%", color = getColor(qtr.metric_5_RAG) });
            }
            catch(Exception)
            {

            }
            
        }



        public string getColor(string p)
        {
            if(p=="R")
            {
                return ("#C10B0B");
            }
            else if(p=="G")
            {
                return ("#0B780B");
            }
            else
            {
                return ("#ED9D2B");
            }
        }
        public void clearlist()
        {
            while (true)
            {
                Debug.WriteLine("into clear");
                if (listView.Items[0]!=null)
                {
                    listView.Items.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
            
        }
        public void populateList(int quarterFlag)
        {
            Debug.WriteLine("into it");
            if (listView.Items.Count > 0)
            {
                Debug.WriteLine("into it1");
                clearlist();
                Debug.WriteLine("into it123");
            }
            string green = "#0B780B";
            string red = "#C10B0B";
            if (quarterFlag == 1)
            {
                Debug.WriteLine("into it2");
                constructData(StoreEmpowermentModel.kpiData.this_quarter);
                Debug.WriteLine("into it3");
            }
            else
            {
                constructData(StoreEmpowermentModel.kpiData.last_quarter);
            }



        }

    }
}
