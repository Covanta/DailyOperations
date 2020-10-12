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
    public class AutomatedJobManagerTests
    {
        public AutomatedJobManagerTests()
        {            
            _covStagingConnString = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;
        }

        //private string _covmetadataConnString = null;
        private string _covStagingConnString = null;

        [TestMethod]
        public void GetAutomatedJobDetails()
        {
            string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;
            
            AutomatedJobDetails x = new AutomatedJobDetails();    
            Enums.AutomatedJobNameEnum appName = Enums.AutomatedJobNameEnum.LongviewExtract;
            x = AutomatedJobDetailsManager.GetAutomatedJobDetails(appName, _connStringCovStaging);
            //AutomatedJobDetailsManager manager = new AutomatedJobDetailsManager(_covStagingConnString);
            
            //x = manager.GetAutomatedJobDetails(appName);
            Assert.IsTrue(x.Author == "Brian Litwin");           
            //manager = null;
        }

        [TestMethod]
        public void UpdateJobStatus()
        {
            string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;

            //AutomatedJobDetailsManager manager = new AutomatedJobDetailsManager(_covStagingConnString);
            string testDescription = "Test Descriptionxxx";
            AutomatedJobDetailsManager.UpdateJobStatus(Enums.AutomatedJobNameEnum.LongviewExtract, Enums.AutomatedJobStatusEnum.Success, testDescription, _connStringCovStaging);
            Assert.IsTrue(1 == 1);
            //manager = null;
        }          
    }
}
