using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Covanta.Common.Enums;
using Covanta.BusinessLogic;
using Covanta.Entities;

namespace Covanta.BusinessLogicTests
{
    [TestClass]
    public class CeridianAndADTest
    {
        public CeridianAndADTest()
        {

            _covStagingConnString = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;
        }


        private string _covStagingConnString = null;

        //[TestMethod]
        //public void GetADSynProposedNoConflictTest()
        //{
        //    CeridianAndActiveDirDifferencesManager manager = new CeridianAndActiveDirDifferencesManager(_covStagingConnString);
        //    List<CeridianAndADSyncProposedSAMwithNoConflict> list = manager.GetCeridianAndADSyncProposedSAMwithNoConflictList();

        //    Assert.IsTrue(1 == 1);
        //    manager = null;
        //}

        //[TestMethod]
        //public void WriteToCSVTest()
        //{
        //    string CSVPathAndFile = ConfigurationManager.AppSettings["CSVPathAndFile"];
        //    CeridianAndActiveDirDifferencesManager manager = new CeridianAndActiveDirDifferencesManager(_covStagingConnString);
        //    List<CeridianAndADSyncProposedSAMwithNoConflict> list = manager.GetCeridianAndADSyncProposedSAMwithNoConflictList();

        //    manager.WriteCeridianAndADSyncProposedSAMwithNoConflictListToCSV(list, CSVPathAndFile);
        //    Assert.IsTrue(1 == 1);
        //    manager = null;
        //}

    }
}
