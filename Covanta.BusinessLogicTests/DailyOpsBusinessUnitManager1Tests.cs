using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Covanta.Entities;
using Covanta.Common.Enums;
using Covanta.BusinessLogic;

namespace Covanta.BusinessLogicTests
{    
    [TestClass]
    public class DailyOpsBusinessUnitManager1Tests
    {
        public DailyOpsBusinessUnitManager1Tests()
        {
            _covmetadataConnString = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
        }

        private string _covmetadataConnString = null;             

        ///// <summary>
        ///// This test will make sure that we retrive at least 1 BU from the database
        ///// </summary>
        //[TestMethod]
        //public void GetBusinessUnitListTest()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);          
        //    List<DailyOpsBusinessUnit> list =  obj.GetBusinessUnitsList();
        //    Assert.IsTrue(list.Count > 0);
        //    Assert.IsTrue(list[0].NumOfBoilers > 0);
        //    obj = null;
        //    list = null;
        //}

        //[TestMethod]
        //public void GetBusinessUnitsDescriptionByPSCodeTest()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    string facility = "DADEE";  
        //    string BUDescription = "Not Set";
        //    BUDescription = obj.GetBusinessUnitsDescriptionByPSCode(facility);
        //    Assert.IsTrue(BUDescription != "Not Set");        
        //    obj = null;          
        //}

        //[TestMethod]
        //public void GetBusinessUnitsTypeByPSCodeTest()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    string facility = "DADEE";
        //    string BUType = "Not Set";
        //    BUType = obj.GetBusinessUnitsTypeByPSCode(facility);
        //    Assert.IsTrue(BUType != "Not Set");
        //    obj = null;
        //}

        //[TestMethod]
        //public void GetDailyOpsBusinessUnitContactEmailListTest()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    string facility = "DADEE";         
        //    List<string> x = obj.GetDailyOpsBusinessUnitContactEmailList(facility, "FM");
        //    Assert.IsTrue(x.Count > 0);
        //    obj = null;
        //}

        //[TestMethod]
        //public void GetDailyOpsBusinessUnitContactNameListTest()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    string facility = "DADEE";
        //    List<string> x = obj.GetDailyOpsBusinessUnitContactNameList(facility, "FM");
        //    Assert.IsTrue(x.Count > 0);
        //    obj = null;
        //}

        //[TestMethod]
        //public void GetDailyOpsBusinessUnitNTIDListTest()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    string facility = "DADEE";
        //    List<string> x = obj.GetDailyOpsBusinessUnitNTIDList(facility, "FM");
        //    Assert.IsTrue(x.Count > 0);
        //    obj = null;
        //}
        //[TestMethod]
        //public void GetDailyOpsBusinessUnitEasternTimeOffsetTest()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    string facility = "LGBCH";
        //    string timeOffset = string.Empty;
        //    timeOffset = obj.GetDailyOpsBusinessUnitEasternTimeOffset(facility);         
        //    Assert.IsTrue(timeOffset != string.Empty);
        //    obj = null;
        //}


        //[TestMethod]
        //public void GetDistinctFacilityTypesListTest()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    List<DailyOpsBusinessUnit> list = obj.GetDistinctFacilityTypesList();
        //    Assert.IsTrue(0 != 1);
        //    obj = null;
        //}

        //[TestMethod]
        //public void GetDistinctRegionsList()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    List<string> list = obj.GetDistinctRegionsList();
        //    Assert.IsTrue(list.Count > 0);          
        //    obj = null;
        //    list = null;
        //}

        //[TestMethod]
        //public void GetFacilitiesByRegionList()
        //{
        //     string region = "West";
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    List<string> list = obj.GetFacilityListbyRegion(region);
        //    Assert.IsTrue(list.Count > 0);
        //    obj = null;
        //    list = null;
        //}

        //[TestMethod]
        //public void GetDailyOpsBusinessUnitsListByRegion()
        //{
        //    string region = "NY NJ";
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    List<DailyOpsBusinessUnit> list = obj.GetDailyOpsBusinessUnitsListByRegion(region);
        //    Assert.IsTrue(list.Count > 0);
        //    obj = null;
        //    list = null;
        //}

        ///// <summary>
        ///// This test will demonstrate an exception caused by an empty string for the connection string.
        ///// The list will remain null.
        ///// </summary>
        //[TestMethod]
        //public void ExceptionOnGetBusineUnitsListNoConnectionStringPassedtoConstructorTest()
        //{
        //    string badConnString = string.Empty;
        //    List<DailyOpsBusinessUnit> list = null;
        //    int x = 0;
        //    try
        //    {
        //        DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(badConnString);
        //        list = obj.GetBusinessUnitsList();
        //    }
        //    catch (Exception e)
        //    {
        //        string a = e.Message;
        //        x++;
        //    }
        //    Assert.IsTrue(x == 1);
        //    Assert.IsTrue(list == null);
        //}

        ///// <summary>
        ///// This test will demonstrate an exception caused by a bad connection string.
        ///// The list will remain null.
        ///// </summary>
        //[TestMethod]
        //public void ExceptionOnGetBusineUnitsListBadConnectionStringPassedtoConstructorTest()
        //{
        //    string badConnString = @"Data Source=covspdev1v-db\moss_dev;Initial Catalog=covmetadata;User ID=covmetadmin;Password=abc";
        //    List<DailyOpsBusinessUnit> list = null;
        //    int x = 0;
        //    try
        //    {
        //        DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(badConnString);
        //        list = obj.GetBusinessUnitsList();
        //    }
        //    catch (Exception e)
        //    {
        //        string a = e.Message;
        //        x++;
        //    }
        //    Assert.IsTrue(x == 1);
        //    Assert.IsTrue(list == null);
        //}

        ///// <summary>
        ///// This test will make sure that we retrive the 5 digit PS codes list from the database
        ///// </summary>
        //[TestMethod]
        //public void GetBusinessUnitPeopleSoftNamesListTest()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    List<String> list = obj.GetBusinessUnitsPeopleSoft5DigitCodeList();
        //    Assert.IsTrue(list.Count > 0);
        //    obj = null;
        //    list = null;
        //}

        ///// <summary>
        ///// This test will make sure that we retrive the business Unit Descriptions from the database
        ///// </summary>
        //[TestMethod]
        //public void GetBusinessUnitDescriptionsListTest()
        //{
        //    DailyOpsBusiUnitManager obj = new DailyOpsBusiUnitManager(_covmetadataConnString);
        //    List<String> list = obj.GetBusinessUnitsDescriptionList();
        //    Assert.IsTrue(list.Count > 0);
        //    obj = null;
        //    list = null;
        //}
    }
}
