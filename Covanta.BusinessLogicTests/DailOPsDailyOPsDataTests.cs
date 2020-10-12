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
    public class DailOPsDailyOPsDataTests
    {
        public DailOPsDailyOPsDataTests()
        {
            _covmetadataConnString = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
        }

        private string _covmetadataConnString = null;

        //[TestMethod]
        //public void GetFirstMissingReportingDateTest()
        //{            
        //    DateTime date = DateTime.Parse("2012-07-22");
        //    DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
        //    string facility = "WARRE";
        //    DateTime resultDate = manager.GetFirstMissingReportingDate(date, facility);           
        //    Assert.IsTrue(resultDate != null);           
        //    manager = null;
        //}

        //[TestMethod]
        //public void IsFacilityMissingReportingDatesTest()
        //{
        //    DateTime date = DateTime.Parse("2012-08-04");
        //    DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
        //    string facility = "WARRE";
        //    bool result = manager.IsFacilityMissingReportingDates(date, facility);
        //    Assert.IsTrue(result == true);
        //    DateTime resultDate = manager.GetFirstMissingReportingDate(date, facility);
        //    Assert.IsTrue(resultDate != null);     
        //    manager = null;
        //}

        /*
        [TestMethod]
        public void GetDailyOpsDataByDateAndFacilityTestConfirm()
        {
            Enums.StatusEnum testEnumValue = Enums.StatusEnum.NotSet;
            DateTime date = DateTime.Parse("2012-07-23");
            DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
            string facility = "WARRE";
            DailyOpsData data = manager.GetDailyOpsDataByDateAndFacility(date, facility, ref testEnumValue);
            Assert.IsTrue(testEnumValue == Enums.StatusEnum.OK);
            Assert.IsTrue(data != null);         
           
            string facilityID = "WARRE";
            Assert.IsTrue(data.FacilityID == facilityID);     

            DateTime reportingDate = DateTime.Parse("2012-07-22");
            Assert.IsTrue(data.ReportingDate == reportingDate); 

            decimal tonsDelivered = 1.01M;
            Assert.IsTrue(data.TonsDelivered == tonsDelivered); 

            decimal tonsProcessed = 2.01M;
            Assert.IsTrue(data.TonsProcessed == tonsProcessed); 

            decimal steamProduced = 3.01M;
            Assert.IsTrue(data.SteamProduced == steamProduced); 

            decimal steamSold = 4.01M;
            Assert.IsTrue(data.SteamSold == steamSold); 

            decimal netElectric = 5.01M;
            Assert.IsTrue(data.NetElectric == netElectric); 

            decimal downTimeBoiler1 = 6.01M;
            Assert.IsTrue(data.DownTimeBoiler1 == downTimeBoiler1); 

            string outageTypeBoiler1 = "outageTypeBoiler1";
            Assert.IsTrue(data.OutageTypeBoiler1 == outageTypeBoiler1); 

            string explanationBoiler1 = "explanationBoiler1";
            Assert.IsTrue(data.ExplanationBoiler1 == explanationBoiler1); 

            decimal downTimeBoiler2 = 7.01M;
            Assert.IsTrue(data.DownTimeBoiler2 == downTimeBoiler2); 

            string outageTypeBoiler2 = "outageTypeBoiler2";
            Assert.IsTrue(data.OutageTypeBoiler2 == outageTypeBoiler2); 

            string explanationBoiler2 = "explanationBoiler2";
            Assert.IsTrue(data.ExplanationBoiler2 == explanationBoiler2); 

            decimal downTimeBoiler3 = 8.01M;
            Assert.IsTrue(data.DownTimeBoiler3 == downTimeBoiler3); 

            string outageTypeBoiler3 = "outageTypeBoiler3";
            Assert.IsTrue(data.OutageTypeBoiler3 == outageTypeBoiler3); 

            string explanationBoiler3 = "explanationBoiler3";
            Assert.IsTrue(data.ExplanationBoiler3 == explanationBoiler3); 

            decimal downTimeBoiler4 = 9.01M;
            Assert.IsTrue(data.DownTimeBoiler4 == downTimeBoiler4); 

            string outageTypeBoiler4 = "outageTypeBoiler4";
            Assert.IsTrue(data.OutageTypeBoiler4 == outageTypeBoiler4); 

            string explanationBoiler4 = "explanationBoiler4";
            Assert.IsTrue(data.ExplanationBoiler4 == explanationBoiler4); 

            decimal downTimeBoiler5 = 10.01M;
            Assert.IsTrue(data.DownTimeBoiler5 == downTimeBoiler5); 

            string outageTypeBoiler5 = "outageTypeBoiler5";
            Assert.IsTrue(data.OutageTypeBoiler5 == outageTypeBoiler5); 

            string explanationBoiler5 = "explanationBoiler5";
            Assert.IsTrue(data.ExplanationBoiler5 == explanationBoiler5); 

            decimal downTimeBoiler6 = 11.01M;
            Assert.IsTrue(data.DownTimeBoiler6 == downTimeBoiler6); 

            string outageTypeBoiler6 = "outageTypeBoiler6";
            Assert.IsTrue(data.OutageTypeBoiler6 == outageTypeBoiler6); 
            
            string explanationBoiler6 = "explanationBoiler6";
            Assert.IsTrue(data.ExplanationBoiler6 == explanationBoiler6); 

            string outageTypeTurbGen1 = "outageTypeTurbGen1";
            Assert.IsTrue(data.OutageTypeTurbGen1 == outageTypeTurbGen1); 

            decimal downTimeTurbGen1 = 12.01M;
            Assert.IsTrue(data.DownTimeTurbGen1 == downTimeTurbGen1); 

            string explanationTurbGen1 = "explanationTurbGen1";
            Assert.IsTrue(data.ExplanationTurbGen1 == explanationTurbGen1); 


            string outageTypeTurbGen2 = "outageTypeTurbGen2";
            Assert.IsTrue(data.OutageTypeTurbGen2 == outageTypeTurbGen2); 

            decimal downTimeTurbGen2 = 13.01M;
            Assert.IsTrue(data.DownTimeTurbGen2 == downTimeTurbGen2); 

            string explanationTurbGen2 = "explanationTurbGen2";
            Assert.IsTrue(data.ExplanationTurbGen2 == explanationTurbGen2); 

            decimal ferrousTons = 14.01M;
            Assert.IsTrue(data.FerrousTons == ferrousTons); 

            decimal nonFerrousTons = 15.01M;
            Assert.IsTrue(data.NonFerrousTons == nonFerrousTons); 

            decimal ferrousSystemHoursUnavailable = 16.01M;
            Assert.IsTrue(data.FerrousSystemHoursUnavailable == ferrousSystemHoursUnavailable); 

            string ferrousSystemHoursUnavailableReason = "ferrousSystemHoursUnavailableReason";
            Assert.IsTrue(data.FerrousSystemHoursUnavailableReason == ferrousSystemHoursUnavailableReason); 

            DateTime ferrousSystemExpectedBackOnlineDate = DateTime.Parse("2012-07-23");
            Assert.IsTrue(data.FerrousSystemExpectedBackOnlineDate == ferrousSystemExpectedBackOnlineDate); 

            string wasAshReprocessedThroughFerrousSystem = "A";
            Assert.IsTrue(data.WasAshReprocessedThroughFerrousSystem == wasAshReprocessedThroughFerrousSystem); 

            string wasFerrousSystem100PercentAvailable = "B";
            Assert.IsTrue(data.WasFerrousSystem100PercentAvailable == wasFerrousSystem100PercentAvailable); 

            decimal nonFerrousSystemHoursUnavailable = 17.01M;
            Assert.IsTrue(data.NonFerrousSystemHoursUnavailable == nonFerrousSystemHoursUnavailable); 

            string nonFerrousSystemHoursUnavailableReason = "nonFerrousSystemHoursUnavailableReason";
            Assert.IsTrue(data.NonFerrousSystemHoursUnavailableReason == nonFerrousSystemHoursUnavailableReason); 

            DateTime nonFerrousSystemExpectedBackOnlineDate = DateTime.Parse("2012-07-24");
            Assert.IsTrue(data.NonFerrousSystemExpectedBackOnlineDate == nonFerrousSystemExpectedBackOnlineDate); 
            
            string wasAshReprocessedThroughNonFerrousSystem = "C";
            Assert.IsTrue(data.WasAshReprocessedThroughNonFerrousSystem == wasAshReprocessedThroughNonFerrousSystem); 

            string wasNonFerrousSystem100PercentAvailable = "D";
            Assert.IsTrue(data.WasNonFerrousSystem100PercentAvailable == wasNonFerrousSystem100PercentAvailable); 


            decimal nonFerrousSmallsSystemHoursUnavailable = 18.01M;
            Assert.IsTrue(data.NonFerrousSmallsSystemHoursUnavailable == nonFerrousSmallsSystemHoursUnavailable); 

            string nonFerrousSmallsSystemHoursUnavailableReason = "nonFerrousSmallsSystemHoursUnavailableReason";
            Assert.IsTrue(data.NonFerrousSmallsSystemHoursUnavailableReason == nonFerrousSmallsSystemHoursUnavailableReason); 

            DateTime nonFerrousSmallsSystemExpectedBackOnlineDate = DateTime.Parse("2012-07-25");
            Assert.IsTrue(data.NonFerrousSmallsSystemExpectedBackOnlineDate == nonFerrousSmallsSystemExpectedBackOnlineDate); 

            string wasAshReprocessedThroughNonFerrousSmallsSystem = "E";
            Assert.IsTrue(data.WasAshReprocessedThroughNonFerrousSmallsSystem == wasAshReprocessedThroughNonFerrousSmallsSystem); 

            string wasNonFerrousSmallsSystem100PercentAvailable = "F";
            Assert.IsTrue(data.WasNonFerrousSmallsSystem100PercentAvailable == wasNonFerrousSmallsSystem100PercentAvailable); 

            decimal frontEndFerrousSystemHoursUnavailable = 19.01M;
            Assert.IsTrue(data.FrontEndFerrousSystemHoursUnavailable == frontEndFerrousSystemHoursUnavailable); 

            string frontEndFerrousSystemHoursUnavailableReason = "frontEndFerrousSystemHoursUnavailableReason";
            Assert.IsTrue(data.FrontEndFerrousSystemHoursUnavailableReason == frontEndFerrousSystemHoursUnavailableReason); 

            DateTime frontEndFerrousSystemExpectedBackOnlineDate = DateTime.Parse("2012-07-26");
            Assert.IsTrue(data.FrontEndFerrousSystemExpectedBackOnlineDate == frontEndFerrousSystemExpectedBackOnlineDate); 

            string wasFrontEndFerrousSystem100PercentAvailable = "G";
            Assert.IsTrue(data.WasFrontEndFerrousSystem100PercentAvailable == wasFrontEndFerrousSystem100PercentAvailable); 

            string fireSystemOutOfService = "fireSystemOutOfService";
            Assert.IsTrue(data.FireSystemOutOfService == fireSystemOutOfService);

            DateTime fireSystemOutOfServiceExpectedBackOnlineDate = DateTime.Parse("2012-07-27");
            Assert.IsTrue(data.FireSystemOutOfServiceExpectedBackOnlineDate == fireSystemOutOfServiceExpectedBackOnlineDate);

            string criticalAssetsInAlarm = "criticalAssetsInAlarm";
            Assert.IsTrue(data.CriticalAssetsInAlarm == criticalAssetsInAlarm);

            bool isEnvironmentalEvents = false;
            Assert.IsTrue(data.IsEnvironmentalEvents == isEnvironmentalEvents);
          
            string environmentalEventsType = "environmentalEventsType";
            Assert.IsTrue(data.EnvironmentalEventsType == environmentalEventsType);

            string environmentalEventsExplanation = "environmentalEventsExplanation";
            Assert.IsTrue(data.EnvironmentalEventsExplanation == environmentalEventsExplanation);

            bool isCEMSEvents = true;
            Assert.IsTrue(data.IsCEMSEvents == isCEMSEvents);
           
            string cemsEventsType = "cemsEventsType";
            Assert.IsTrue(data.CEMSEventsType == cemsEventsType);

            string cemsEventsExplanation = "cemsEventsExplanation";
            Assert.IsTrue(data.CEMSEventsExplanation == cemsEventsExplanation);

            string healthSafetyFirstAid = "healthSafetyFirstAid";
            Assert.IsTrue(data.HealthSafetyFirstAid == healthSafetyFirstAid);

            string healthSafetyOSHAReportable = "healthSafetyOSHAReportable";
            Assert.IsTrue(data.HealthSafetyOSHAReportable == healthSafetyOSHAReportable);

            string healthSafetyNearMiss =  "healthSafetyNearMiss";
            Assert.IsTrue(data.HealthSafetyNearMiss == healthSafetyNearMiss);

            string healthSafetyContractor =  "healthSafetyContractor";
            Assert.IsTrue(data.HealthSafetyContractor == healthSafetyContractor);

            string comments =  "comments";
            Assert.IsTrue(data.Comments == comments);

            string userRowCreated =  "userRowCre";
            Assert.IsTrue(data.UserRowCreated == userRowCreated);

            
            data = null;
            manager = null;
        }
              

        [TestMethod]
        public void GetDailyOpsDataByDateAndFacilityTest()
        {
            Enums.StatusEnum testEnumValue = Enums.StatusEnum.NotSet;
            DateTime date = DateTime.Parse("2012-07-22");
            DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
            string facility = "ALEXA";
            DailyOpsData data = manager.GetDailyOpsDataByDateAndFacility(date, facility, ref testEnumValue);
            Assert.IsTrue(testEnumValue == Enums.StatusEnum.OK);
            Assert.IsTrue(data != null);
            data = null;
            manager = null;
        }

        [TestMethod]
        public void GetDailyOpsDataByDateAndRegion()
        {
            Enums.StatusEnum testEnumValue = Enums.StatusEnum.NotSet;
            DateTime date = DateTime.Parse("2012-06-29");
            DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
            string region = "NY NJ";
            List<DailyOpsData> data = manager.GetDailyOpsDataByDateAndRegion(date, region, ref testEnumValue);
            Assert.IsTrue(testEnumValue == Enums.StatusEnum.OK);
            Assert.IsTrue(data != null);
            data = null;
            manager = null;
        }

        [TestMethod]
        public void GetDailyOpsDataByDate()
        {
            Enums.StatusEnum testEnumValue = Enums.StatusEnum.NotSet;
            DateTime date = DateTime.Parse("2012-07-22");
            DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
            List<DailyOpsData> data = manager.GetDailyOpsDataByDate(date, ref testEnumValue);
            Assert.IsTrue(testEnumValue == Enums.StatusEnum.OK);
            Assert.IsTrue(data != null);
            data = null;
            manager = null;
        }

        [TestMethod]
        public void GetDailyOpsBoilerStatusByDateAndFacility()
        {
            Enums.StatusEnum testEnumValue = Enums.StatusEnum.NotSet;
            DateTime date = DateTime.Parse("2012-07-22");
            string facility = "DADEE";
            DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
            List<DailyOpsBoilerStatus> data = manager.GetDailyOpsBoilerStatusByDateAndFacility(date, facility, ref testEnumValue);
            Assert.IsTrue(testEnumValue == Enums.StatusEnum.OK);
            Assert.IsTrue(data != null);
            data = null;
            manager = null;
        }

        [TestMethod]
        public void GetNumberOfFacilitiesForEachTypeTest()
        {
            DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
            List<DailyOpsFacilitiesReportingStats> x = new List<DailyOpsFacilitiesReportingStats>();
            //      manager.getNumberOfFacilitiesForEachType(ref x);
            Assert.IsTrue(1 != 2);

            manager = null;
        }

        [TestMethod]
        public void GetDailyOpsFacilitiesReportingStatsByParametersTest1()
        {
            DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
            DateTime date = DateTime.Parse("2012-07-22");
            Enums.StatusEnum testEnumValue = Enums.StatusEnum.NotSet;
            List<DailyOpsFacilitiesReportingStats> x = manager.GetDailyOpsFacilitiesReportingStatsByParameters(date, string.Empty, string.Empty, ref testEnumValue);
            Assert.IsTrue(1 != 2);
            manager = null;
        }

        [TestMethod]
        public void GetDailyOpsFacilitiesReportingStatsByParametersTest2()
        {
            DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
            DateTime date = DateTime.Parse("2012-07-22");
            Enums.StatusEnum testEnumValue = Enums.StatusEnum.NotSet;
            List<DailyOpsFacilitiesReportingStats> x = manager.GetDailyOpsFacilitiesReportingStatsByParameters(date,"DADEE", string.Empty, ref testEnumValue);
            Assert.IsTrue(1 != 2);
            manager = null;
        }

       
        [TestMethod]
        public void GetDailyOpsFacilitiesReportingStatsByParametersTest3()
        {
            DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);
            DateTime date = DateTime.Parse("2012-07-22");
            string region = "Mid Atlantic";
            Enums.StatusEnum testEnumValue = Enums.StatusEnum.NotSet;
            List<DailyOpsFacilitiesReportingStats> x = manager.GetDailyOpsFacilitiesReportingStatsByParameters(date, string.Empty, region, ref testEnumValue);
            Assert.IsTrue(1 != 2);
            manager = null;
        }

        [TestMethod]
        public void InsertDailyOpsDataTest()
        {
            Enums.StatusEnum testEnumValue = Enums.StatusEnum.NotSet;
            string status = string.Empty;
            string facilityID = "WARRE";
            DateTime reportingDate = DateTime.Parse("2012-07-23");
            decimal tonsDelivered = 1.01M;
            decimal tonsProcessed = 2.01M;
            decimal steamProduced = 3.01M;
            decimal steamSold = 4.01M;
            decimal netElectric = 5.01M;

            decimal downTimeBoiler1 = 6.01M;
            string outageTypeBoiler1 = "outageTypeBoiler1";
            string explanationBoiler1 = "explanationBoiler1";

            decimal downTimeBoiler2 = 7.01M;
            string outageTypeBoiler2 = "outageTypeBoiler2";
            string explanationBoiler2 = "explanationBoiler2";

            decimal downTimeBoiler3 = 8.01M;
            string outageTypeBoiler3 = "outageTypeBoiler3";
            string explanationBoiler3 = "explanationBoiler3";

            decimal downTimeBoiler4 = 9.01M;
            string outageTypeBoiler4 = "outageTypeBoiler4";
            string explanationBoiler4 = "explanationBoiler4";

            decimal downTimeBoiler5 = 10.01M;
            string outageTypeBoiler5 = "outageTypeBoiler5";
            string explanationBoiler5 = "explanationBoiler5";

            decimal downTimeBoiler6 = 11.01M;
            string outageTypeBoiler6 = "outageTypeBoiler6";
            string explanationBoiler6 = "explanationBoiler6";

            string outageTypeTurbGen1 = "outageTypeTurbGen1";
            decimal downTimeTurbGen1 = 12.01M;
            string explanationTurbGen1 = "explanationTurbGen1";

            string outageTypeTurbGen2 = "outageTypeTurbGen2";
            decimal downTimeTurbGen2 = 13.01M;
            string explanationTurbGen2 = "explanationTurbGen2";

            decimal ferrousTons = 14.01M;
            decimal nonFerrousTons = 15.01M;

            decimal ferrousSystemHoursUnavailable = 16.01M;
            string ferrousSystemHoursUnavailableReason = "ferrousSystemHoursUnavailableReason";
            DateTime ferrousSystemExpectedBackOnlineDate = DateTime.Parse("2012-07-23");

            string wasAshReprocessedThroughFerrousSystem = "A";
            string wasFerrousSystem100PercentAvailable = "B";

            decimal nonFerrousSystemHoursUnavailable = 17.01M;
            string nonFerrousSystemHoursUnavailableReason = "nonFerrousSystemHoursUnavailableReason";
            DateTime nonFerrousSystemExpectedBackOnlineDate = DateTime.Parse("2012-07-24");
            string wasAshReprocessedThroughNonFerrousSystem = "C";
            string wasNonFerrousSystem100PercentAvailable = "D";

            decimal nonFerrousSmallsSystemHoursUnavailable = 18.01M;
            string nonFerrousSmallsSystemHoursUnavailableReason = "nonFerrousSmallsSystemHoursUnavailableReason";
            DateTime nonFerrousSmallsSystemExpectedBackOnlineDate = DateTime.Parse("2012-07-25");
            string wasAshReprocessedThroughNonFerrousSmallsSystem = "E";
            string wasNonFerrousSmallsSystem100PercentAvailable = "F";

            decimal frontEndFerrousSystemHoursUnavailable = 19.01M;
            string frontEndFerrousSystemHoursUnavailableReason = "frontEndFerrousSystemHoursUnavailableReason";
            DateTime frontEndFerrousSystemExpectedBackOnlineDate = DateTime.Parse("2012-07-26");
            string wasFrontEndFerrousSystem100PercentAvailable = "G";

            string fireSystemOutOfService = "fireSystemOutOfService";
            DateTime fireSystemOutOfServiceExpectedBackOnlineDate = DateTime.Parse("2012-07-27");

            string criticalAssetsInAlarm = "criticalAssetsInAlarm";

            bool isEnvironmentalEvents = false;
           // if (bindObj.ToStringValue("isEnvironmentalEvents") == "Y") { isEnvironmentalEvents = true; }
            string environmentalEventsType = "environmentalEventsType";
            string environmentalEventsExplanation = "environmentalEventsExplanation";

            bool isCEMSEvents = true;
            //if (bindObj.ToStringValue("isCEMSEvents") == "Y") { isCEMSEvents = true; }
            string cemsEventsType = "cemsEventsType";
            string cemsEventsExplanation = "cemsEventsExplanation";

            string healthSafetyFirstAid = "healthSafetyFirstAid";
            string healthSafetyOSHAReportable = "healthSafetyOSHAReportable";
            string healthSafetyNearMiss =  "healthSafetyNearMiss";
            string healthSafetyContractor =  "healthSafetyContractor";

            string comments =  "comments";
            string userRowCreated =  "userRowCre";


            DailyOpsData obj = new DailyOpsData(facilityID, reportingDate, tonsDelivered, tonsProcessed, steamProduced, steamSold, netElectric,
                outageTypeBoiler1, downTimeBoiler1, explanationBoiler1,
                outageTypeBoiler2, downTimeBoiler2, explanationBoiler2,
                outageTypeBoiler3, downTimeBoiler3, explanationBoiler3,
                outageTypeBoiler4, downTimeBoiler4, explanationBoiler4,
                outageTypeBoiler5, downTimeBoiler5, explanationBoiler5,
                outageTypeBoiler6, downTimeBoiler6, explanationBoiler6,
                outageTypeTurbGen1, downTimeTurbGen1, explanationTurbGen1,
                outageTypeTurbGen2, downTimeTurbGen2, explanationTurbGen2,
                ferrousTons, nonFerrousTons,
                ferrousSystemHoursUnavailable, ferrousSystemHoursUnavailableReason, ferrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughFerrousSystem, wasFerrousSystem100PercentAvailable,
                nonFerrousSystemHoursUnavailable, nonFerrousSystemHoursUnavailableReason, nonFerrousSystemExpectedBackOnlineDate, wasAshReprocessedThroughNonFerrousSystem, wasNonFerrousSystem100PercentAvailable,
                nonFerrousSmallsSystemHoursUnavailable, nonFerrousSmallsSystemHoursUnavailableReason, nonFerrousSmallsSystemExpectedBackOnlineDate, wasAshReprocessedThroughNonFerrousSmallsSystem, wasNonFerrousSmallsSystem100PercentAvailable,
                frontEndFerrousSystemHoursUnavailable, frontEndFerrousSystemHoursUnavailableReason, frontEndFerrousSystemExpectedBackOnlineDate, wasFrontEndFerrousSystem100PercentAvailable,
                fireSystemOutOfService, fireSystemOutOfServiceExpectedBackOnlineDate, criticalAssetsInAlarm,
                isEnvironmentalEvents, environmentalEventsType, environmentalEventsExplanation,
                isCEMSEvents, cemsEventsType, cemsEventsExplanation,
                healthSafetyFirstAid, healthSafetyOSHAReportable, healthSafetyNearMiss, healthSafetyContractor,
                comments, userRowCreated);
            DailyOpsManager manager = new DailyOpsManager(_covmetadataConnString);

            manager.InsertDailyOpsData(obj, ref testEnumValue, ref status);
            Assert.IsTrue(testEnumValue == Enums.StatusEnum.OK);
            obj = null;
            manager = null;
        }
         */ 
    }
}
