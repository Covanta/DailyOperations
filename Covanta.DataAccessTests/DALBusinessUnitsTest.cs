using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Covanta.Entities;
using Covanta.DataAccess;
using Covanta.Common.Enums;

namespace Covanta.DataAccessTests
{    
    [TestClass]
    public class DALBusinessUnitsTest
    {
        public DALBusinessUnitsTest()
        {
            _covmetadataConnString = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
        }

        private string _covmetadataConnString = null;             

        ///// <summary>
        ///// This test will make sure that we retrive at least 1 BU from the database
        ///// </summary>
        //[TestMethod]
        //public void GetBusineUnitsListTest()
        //{
        //    DALBusinessUnits obj = new DALBusinessUnits(_covmetadataConnString);          
        //    List<DailyOpsBusinessUnit> list =  obj.GetDailyOpsBusinessUnitsList();
        //    Assert.IsTrue(list.Count > 0);
        //    obj = null;
        //    list = null;
        //}

        ///// <summary>
        ///// This test will demonstrate an exception.
        ///// The list will remain null.
        ///// </summary>
        //[TestMethod]
        //public void ExceptionOnGetBusineUnitsListTest()
        //{
        //    string badConnString = @"Data Source=covspdev1v-db\moss_dev;Initial Catalog=covmetadata;User ID=covmetadmin;Password=abc";
        //    DALBusinessUnits obj = new DALBusinessUnits(badConnString);

        //    List<DailyOpsBusinessUnit> list = null;
        //    int x = 0;
        //    try
        //    {
        //       list = obj.GetDailyOpsBusinessUnitsList();
        //    }
        //    catch (Exception)
        //    {           
        //        x++;
        //    }
        //    Assert.IsTrue(list == null);
        //    Assert.IsTrue(x == 1);
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
        //    int x = 0;
        //    try
        //    {
        //        DALBusinessUnits obj = new DALBusinessUnits(badConnString);
        //    }
        //    catch (Exception)
        //    {
        //        x++;
        //    }           
        //    Assert.IsTrue(x == 1);
        //}
    }
}
