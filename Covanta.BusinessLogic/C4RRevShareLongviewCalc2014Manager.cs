using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.DataAccess;
using Covanta.Entities;

namespace Covanta.BusinessLogic
{
    public class C4RRevShareLongviewCalc2014Manager
    {
        #region constructors

        public C4RRevShareLongviewCalc2014Manager() { }
        #endregion

        #region classes

        private class LongviewTotalsByEntityByYearByMonth
        {
            public string Entity { get; set; }
            public string Year { get; set; }
            public string Month { get; set; }
            public double Value { get; set; }       
        }

        public class NetRevEntityAccountInfo
        {
            #region constructors
           
            public NetRevEntityAccountInfo() { }
            public NetRevEntityAccountInfo(string entity, string account)
            {
                Entity = entity;
                Account = account;
            }

            #endregion

            #region public properties

            public string Entity { get; set; }
            public string Account { get; set; }           

            #endregion
        }
        #endregion

        #region private variables

        string _connStringCovStaging;
        string _connStringCovIntegration;
        List<LongviewFactOPRevenue> _longviewFactOpRevList = null;
        List<PS_JRNL_NET_REV> _ps_Jrnl_Net_RevList = null;
        List<NetRevEntityAccountInfo> _netRevEntityAccountInfoList = null;
        List<string> _netRevEntityList = null;
        List<LongviewTotalsByEntityByYearByMonth> _longviewTotalsByEntityByYearByMonthList = new List<LongviewTotalsByEntityByYearByMonth>();
        List<string> _months = new List<string>() { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        List<string> _years = new List<string>() { "14", "15", "16", "17" };


        #endregion
        
        #region public methods

        public void Process1(string conStage, string conInt)
        {
            _connStringCovStaging = conStage;
            _connStringCovIntegration = conInt;

            //1  
            getListOfLongViewTotals();
            //_longviewFactOpRevList

            //2
            getListOf_PS_JRNL_NetRevList();
            //_ps_Jrnl_Net_RevList

            //3
            getRevShareEntitesAndAccountsList();
            //_netRevEntityAccountInfoList

            //4
            populateUniqueListOfEntitiesWhichHaveRevShare();
            //_netRevEntityList

            //5
            calcLongviewTotalsByEntityByYearByMonth();
            //_longviewTotalsByEntityByYearByMonthList
           
            //6
            calcLVPercentageByEntityAccountMonthAndMultiply();
            //_longviewFactOpRevList

            //7
            deleteFromDatabase();

            //8
            writeToDatabase();

          
        }

        #endregion

        #region private methods

        private void deleteFromDatabase()
        {
            DALC4RRevShareNet2014 dal = new DALC4RRevShareNet2014(_connStringCovStaging);
            dal.DeleteNetRevToDatabase();
        }
        
        private void writeToDatabase()
        {
            DALC4RRevShareNet2014 dal = new DALC4RRevShareNet2014(_connStringCovStaging);

            // only write to database rows which have a net rev account matching the Entity and account on Chris's spreadsheet.
            List<LongviewFactOPRevenue> listReduced = new List<LongviewFactOPRevenue>();
            foreach (LongviewFactOPRevenue lfv in _longviewFactOpRevList)
            {
                if (_netRevEntityAccountInfoList.Exists(e => e.Account == lfv.Account && e.Entity == lfv.Entity))
                {
                    listReduced.Add(lfv);                    
                }
            }

            // if an entity only has one account - then make the net rev 100 percent of the net value
            foreach (LongviewFactOPRevenue lfv in listReduced)
            {
                int count = _netRevEntityAccountInfoList.Count(e => e.Entity == lfv.Entity);
              
                if (count == 1)
                {
                    lfv.PercentOfTotal = 1;                    
                    lfv.NetValue = lfv.JournalTotal;
                }                
            }
            dal.InsertNetRevToDatabase(listReduced);           
        }     
        
        private void calcLVPercentageByEntityAccountMonthAndMultiply()
        {
            //use _longviewFactOpRevList divided by _longviewTotalsByEntityByMonthList
         
            foreach (LongviewFactOPRevenue l in _longviewFactOpRevList)
            {
                //get percentage
                l.PercentOfTotal = l.Value / _longviewTotalsByEntityByYearByMonthList.First(e => e.Entity == l.Entity && e.Year == l.Year && e.Month == l.Month).Value;
                //use percentage to get net value

                //if we have a value on the journal table, then use it, else Net Rev value is 0
                string aa = "20" + l.Year;
                if (_ps_Jrnl_Net_RevList.Exists(e => e.Entity == l.Entity && e.Year == "20" + l.Year && e.AccountingPeriod == l.Month) == true)
                {
                    l.NetValue = l.PercentOfTotal * _ps_Jrnl_Net_RevList.First(e => e.Entity == l.Entity && e.Year == "20" + l.Year && e.AccountingPeriod == l.Month).Amount;
                    //Write JournalTotal
                    l.JournalTotal = _ps_Jrnl_Net_RevList.First(e => e.Entity == l.Entity && e.Year == "20" + l.Year && e.AccountingPeriod == l.Month).Amount;
                }                
            }
        }

        private void calcLongviewTotalsByEntityByYearByMonth()
        {
            //get totals for longview all accounts per entity

            _longviewTotalsByEntityByYearByMonthList.Clear();
            foreach (string s in _netRevEntityList)
            {
                foreach (string year in _years)
                {
                    foreach (string month in _months)
                    {
                        double total = 0;
                        LongviewTotalsByEntityByYearByMonth x = new LongviewTotalsByEntityByYearByMonth();
                        x.Entity = s;
                        x.Month = month;
                        x.Year = year;

                        foreach (LongviewFactOPRevenue lv in _longviewFactOpRevList)
                        {
                            if ((lv.Entity == s) && (lv.Month == month) && (lv.Year ==year))
                            {
                                total = total + lv.Value;
                            }
                        }
                        x.Value = total;
                        _longviewTotalsByEntityByYearByMonthList.Add(x);
                    }
                }
            }
            _longviewTotalsByEntityByYearByMonthList = _longviewTotalsByEntityByYearByMonthList.OrderBy(x => x.Entity).ThenBy(z => z.Year).ThenBy(y => y.Month).ToList();
         
        }

        private void getRevShareEntitesAndAccountsList()
        {
            _netRevEntityAccountInfoList = new List<NetRevEntityAccountInfo>();
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("DUTCH", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("FAIRF", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("HENNE", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("HONOL", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("HUNTG", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("HUNTS", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("HUNTS", "44400"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("ISLIP", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("LAKEE", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("LGBCH", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("MARIO", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("MARIO", "44400"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("ONOND", "44000"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("ONOND", "44100"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("ONOND", "44150"));
            _netRevEntityAccountInfoList.Add(new NetRevEntityAccountInfo("TRMSE", "44000"));
                   

            //DALC4RRevShareNet2014 dal = new DALC4RRevShareNet2014(_connStringCovIntegration);
           // _c4r_ForecastRecExcelList = dal.Get_C4RForecastRecExcelInfoList();
            //_c4r_ForecastRecExcelList = _c4r_ForecastRecExcelList.FindAll(e => e.RevShare == "X");
            //_c4r_ForecastRecExcelList = _c4r_ForecastRecExcelList.FindAll(e => e.Month1 == "01");
            //_c4r_ForecastRecExcelList = _c4r_ForecastRecExcelList.FindAll(e => e.Identifier == "Net $");
            //_c4r_ForecastRecExcelList = _c4r_ForecastRecExcelList.OrderBy(e => e.Entity).ThenBy(f => f.Account).ThenBy(g => g.Month1).ToList(); 

        }

        private void populateUniqueListOfEntitiesWhichHaveRevShare()
        {
            _netRevEntityList = new List<string>();
            foreach (NetRevEntityAccountInfo s in _netRevEntityAccountInfoList)
            {
                if (_netRevEntityList.Contains(s.Entity) == false)
                {
                    _netRevEntityList.Add(s.Entity);
                }
            }
        }

        private void getListOfLongViewTotals()
        {
            DALC4RRevShareNet2014 dal = new DALC4RRevShareNet2014(_connStringCovStaging);
            _longviewFactOpRevList = dal.Get_OPRevenueList();
            _longviewFactOpRevList = _longviewFactOpRevList.OrderBy(e => e.Entity).ThenBy(f => f.Account).ThenBy(g => g.Month).ToList();
        }

        private void getListOf_PS_JRNL_NetRevList()
        {
            DALC4RRevShareNet2014 dal = new DALC4RRevShareNet2014(_connStringCovStaging);
            _ps_Jrnl_Net_RevList = dal.Get_PS_JournalNetRevList();
            _ps_Jrnl_Net_RevList = _ps_Jrnl_Net_RevList.OrderBy(e => e.Entity).ThenBy(f => f.Account).ThenBy(g => g.AccountingPeriod).ToList();
        }

        #endregion
    }
}
