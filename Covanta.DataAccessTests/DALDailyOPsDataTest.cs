using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Covanta.DataAccess;
using Covanta.Entities;
using Covanta.Common.Enums;


namespace Covanta.DataAccessTests
{
    [TestClass]
    public class DALDailyOPsDataTest
    {
        public DALDailyOPsDataTest()
        {
            _covmetadataConnString = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
        }

        private string _covmetadataConnString = null;

        ///// <summary>
        ///// This test will make sure that we retrive at least 1 Daily OpsRecord from the database
        ///// </summary>
        //[TestMethod]
        //public void GetDailyOpsTest()
        //{
        //    DALDailyOps obj = new DALDailyOps(_covmetadataConnString);
        //    DateTime date = DateTime.Parse("2012-06-26");
        //    Enums.StatusEnum status = Enums.StatusEnum.NotSet;
        //    string x = string.Empty;
        //    DailyOpsData data = obj.LoadDailyOpsDataByDateAndFacility(date, "ALEXA", ref status, ref x);
        //    Assert.IsTrue(data != null);
        //    obj = null;
        //    obj = null;
        //}
    }
}
