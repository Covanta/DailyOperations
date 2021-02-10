using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents 1 days worth of Covanta Daily OPs Data
    /// </summary>
    public class DailyOpsData
    {
        #region private variables


        #endregion

        #region constructors

        /// <summary>
        /// Populates the object
        /// </summary>
        /// <param name="facilityID">facilityID</param>
        /// <param name="reportingDate">reportingDate</param>
        /// <param name="tonsDelivered">tonsDelivered</param>
        /// <param name="tonsProcessed">tonsProcessed</param>
        /// <param name="steamProduced">steamProduced</param>
        /// <param name="steamSold">steamSold</param>
        /// <param name="netElectric">netElectric</param>
        /// <param name="outageTypeBoiler1">outageTypeBoiler1</param>
        /// <param name="downTimeBoiler1">downTimeBoiler1</param>
        /// <param name="boiler1ExpectedBackOnlineDate">boiler1ExpectedBackOnlineDate</param>
        /// <param name="explanationBoiler1">explanationBoiler1</param>
        /// <param name="outageTypeBoiler2">outageTypeBoiler2</param>
        /// <param name="downTimeBoiler2">downTimeBoiler2</param>
        /// <param name="explanationBoiler2">explanationBoiler2</param>
        /// <param name="boiler2ExpectedBackOnlineDate">boiler2ExpectedBackOnlineDate</param>
        /// <param name="outageTypeBoiler3">outageTypeBoiler3</param>
        /// <param name="downTimeBoiler3">downTimeBoiler3</param>
        /// <param name="explanationBoiler3">explanationBoiler3</param>
        /// <param name="boiler3ExpectedBackOnlineDate">boiler3ExpectedBackOnlineDate</param>
        /// <param name="outageTypeBoiler4">outageTypeBoiler4</param>
        /// <param name="downTimeBoiler4">downTimeBoiler4</param>
        /// <param name="explanationBoiler4">explanationBoiler4</param>
        /// <param name="boiler4ExpectedBackOnlineDate">boiler4ExpectedBackOnlineDate</param>
        /// <param name="outageTypeBoiler5">outageTypeBoiler5</param>
        /// <param name="downTimeBoiler5">downTimeBoiler5</param>
        /// <param name="explanationBoiler5">explanationBoiler5</param>
        /// <param name="boiler5ExpectedBackOnlineDate">boiler5ExpectedBackOnlineDate</param>
        /// <param name="outageTypeBoiler6">outageTypeBoiler6</param>
        /// <param name="downTimeBoiler6">downTimeBoiler6</param>
        /// <param name="explanationBoiler6">explanationBoiler6</param>
        /// <param name="boiler6ExpectedBackOnlineDate">boiler6ExpectedBackOnlineDate</param>
        /// <param name="outageTypeTurbGen1">outageTypeTurbGen1</param>
        /// <param name="downTimeTurbGen1">downTimeTurbGen1</param>
        /// <param name="explanationTurbGen1">explanationTurbGen1</param>
        /// <param name="turbGen1ExpectedBackOnlineDate">turbGen1ExpectedBackOnlineDate</param>
        /// <param name="outageTypeTurbGen2">outageTypeTurbGen2</param>
        /// <param name="downTimeTurbGen2">downTimeTurbGen2</param>
        /// <param name="explanationTurbGen2">explanationTurbGen2</param>
        /// <param name="turbGen2ExpectedBackOnlineDate">turbGen2ExpectedBackOnlineDate</param>
        /// <param name="ferrousTons">ferrousTons</param>
        /// <param name="nonFerrousTons">nonFerrousTons</param>
        /// <param name="ferrousSystemHoursUnavailable">ferrousSystemHoursUnavailable</param>
        /// <param name="ferrousSystemHoursUnavailableReason">ferrousSystemHoursUnavailableReason</param>
        /// <param name="ferrousSystemExpectedBackOnlineDate">ferrousSystemExpectedBackOnlineDate</param>
        /// <param name="wasAshReprocessedThroughFerrousSystem">wasAshReprocessedThroughFerrousSystem</param>
        /// <param name="wasFerrousSystem100PercentAvailable">wasFerrousSystem100PercentAvailable</param>
        /// <param name="nonFerrousSystemHoursUnavailable">nonFerrousSystemHoursUnavailable</param>
        /// <param name="nonFerrousSystemHoursUnavailableReason">nonFerrousSystemHoursUnavailableReason</param>
        /// <param name="nonFerrousSystemExpectedBackOnlineDate">nonFerrousSystemExpectedBackOnlineDate</param>
        /// <param name="wasAshReprocessedThroughNonFerrousSystem">wasAshReprocessedThroughNonFerrousSystem</param>
        /// <param name="wasNonFerrousSystem100PercentAvailable">wasNonFerrousSystem100PercentAvailable</param>
        /// <param name="nonFerrousSmallsSystemHoursUnavailable">nonFerrousSmallsSystemHoursUnavailable</param>
        /// <param name="nonFerrousSmallsSystemHoursUnavailableReason">nonFerrousSmallsSystemHoursUnavailableReason</param>
        /// <param name="nonFerrousSmallsSystemExpectedBackOnlineDate">nonFerrousSmallsSystemExpectedBackOnlineDate</param>
        /// <param name="wasAshReprocessedThroughNonFerrousSmallsSystem">wasAshReprocessedThroughNonFerrousSmallsSystem</param>
        /// <param name="wasNonFerrousSmallsSystem100PercentAvailable">wasNonFerrousSmallsSystem100PercentAvailable</param>
        /// <param name="frontEndFerrousSystemHoursUnavailable">frontEndFerrousSystemHoursUnavailable</param>
        /// <param name="frontEndFerrousSystemHoursUnavailableReason">frontEndFerrousSystemHoursUnavailableReason</param>
        /// <param name="frontEndFerrousSystemExpectedBackOnlineDate">frontEndFerrousSystemExpectedBackOnlineDate</param>
        /// <param name="wasFrontEndFerrousSystem100PercentAvailable">wasFrontEndFerrousSystem100PercentAvailable</param>
        /// <param name="fireSystemOutOfService">fireSystemOutOfService</param>
        /// <param name="fireSystemOutOfServiceExpectedBackOnlineDate">fireSystemOutOfServiceExpectedBackOnlineDate</param>
        /// <param name="criticalAssetsInAlarm">criticalAssetsInAlarm</param>
        /// <param name="isEnvironmentalEvents">isEnvironmentalEvents</param>
        /// <param name="environmentalEventsType">environmentalEventsType</param>
        /// <param name="environmentalEventsExplanation">environmentalEventsExplanation</param>
        /// <param name="isCEMSEvents">isCEMSEvents</param>
        /// <param name="cemsEventsType">cemsEventsType</param>
        /// <param name="cemsEventsExplanation">cemsEventsExplanation</param>
        /// <param name="healthSafetyFirstAid">healthSafetyFirstAid</param>
        /// <param name="healthSafetyOSHAReportable">healthSafetyOSHAReportable</param>
        /// <param name="healthSafetyNearMiss">healthSafetyNearMiss</param>
        /// <param name="healthSafetyContractor">healthSafetyContractor</param>
        /// <param name="comments">comments</param>
        /// <param name="userRowCreated">userRowCreated</param>
        /// <param name="pitInventory">pitInventory</param>
        public DailyOpsData(string facilityID, DateTime reportingDate, decimal tonsDelivered, decimal tonsProcessed, decimal steamProduced, decimal steamSold, decimal netElectric,
                         string outageTypeBoiler1, decimal downTimeBoiler1, string explanationBoiler1, DateTime boiler1ExpectedBackOnlineDate,
                         string outageTypeBoiler2, decimal downTimeBoiler2, string explanationBoiler2, DateTime boiler2ExpectedBackOnlineDate,
                         string outageTypeBoiler3, decimal downTimeBoiler3, string explanationBoiler3, DateTime boiler3ExpectedBackOnlineDate,
                         string outageTypeBoiler4, decimal downTimeBoiler4, string explanationBoiler4, DateTime boiler4ExpectedBackOnlineDate,
                         string outageTypeBoiler5, decimal downTimeBoiler5, string explanationBoiler5, DateTime boiler5ExpectedBackOnlineDate,
                         string outageTypeBoiler6, decimal downTimeBoiler6, string explanationBoiler6, DateTime boiler6ExpectedBackOnlineDate,
                         string outageTypeTurbGen1, decimal downTimeTurbGen1, string explanationTurbGen1, DateTime turbGen1ExpectedBackOnlineDate,
                         string outageTypeTurbGen2, decimal downTimeTurbGen2, string explanationTurbGen2, DateTime turbGen2ExpectedBackOnlineDate,
                         decimal ferrousTons, decimal nonFerrousTons,
                         decimal ferrousSystemHoursUnavailable, string ferrousSystemHoursUnavailableReason, DateTime ferrousSystemExpectedBackOnlineDate, string wasAshReprocessedThroughFerrousSystem, string wasFerrousSystem100PercentAvailable,
                         decimal nonFerrousSystemHoursUnavailable, string nonFerrousSystemHoursUnavailableReason, DateTime nonFerrousSystemExpectedBackOnlineDate, string wasAshReprocessedThroughNonFerrousSystem, string wasNonFerrousSystem100PercentAvailable,
                         decimal nonFerrousSmallsSystemHoursUnavailable, string nonFerrousSmallsSystemHoursUnavailableReason, DateTime nonFerrousSmallsSystemExpectedBackOnlineDate, string wasAshReprocessedThroughNonFerrousSmallsSystem, string wasNonFerrousSmallsSystem100PercentAvailable,
                         decimal frontEndFerrousSystemHoursUnavailable, string frontEndFerrousSystemHoursUnavailableReason, DateTime frontEndFerrousSystemExpectedBackOnlineDate, string wasAshReprocessedThroughFrontEndFerrousSystem, string wasFrontEndFerrousSystem100PercentAvailable,
                         decimal enhancedFerrousSystemHoursUnavailable, string enhancedFerrousSystemHoursUnavailableReason, DateTime enhancedFerrousSystemExpectedBackOnlineDate, string wasAshReprocessedThroughEnhancedFerrousSystem, string wasEnhancedFerrousSystem100PercentAvailable,
                         decimal enhancedNonFerrousSystemHoursUnavailable, string enhancedNonFerrousSystemHoursUnavailableReason, DateTime enhancedNonFerrousSystemExpectedBackOnlineDate, string wasAshReprocessedThroughEnhancedNonFerrousSystem, string wasEnhancedNonFerrousSystem100PercentAvailable,
                         string fireSystemOutOfService, DateTime fireSystemOutOfServiceExpectedBackOnlineDate, string criticalAssetsInAlarm,
                         bool isEnvironmentalEvents, string environmentalEventsType, string environmentalEventsExplanation,
                         bool isCEMSEvents, string cemsEventsType, string cemsEventsExplanation,
                         string healthSafetyFirstAid, string healthSafetyOSHAReportable, string healthSafetyNearMiss, string healthSafetyContractor,
                         string comments, string userRowCreated, decimal pitInventory, DateTime criticalAssetsExpectedBackOnlineDate, 
                         DateTime criticalEquipmentOOSExpectedBackOnlineDate, decimal preShredInventory, decimal postShredInventory, decimal massBurnInventory)
        {
            FacilityID = facilityID;
            ReportingDate = reportingDate;
            TonsDelivered = tonsDelivered;
            TonsProcessed = tonsProcessed;
            SteamProduced = steamProduced;
            SteamSold = steamSold;
            NetElectric = netElectric;
            DownTimeBoiler1 = downTimeBoiler1;
            OutageTypeBoiler1 = outageTypeBoiler1;
            ExplanationBoiler1 = explanationBoiler1;
            Boiler1ExpectedRepairDate = boiler1ExpectedBackOnlineDate;
            DownTimeBoiler2 = downTimeBoiler2;
            OutageTypeBoiler2 = outageTypeBoiler2;
            ExplanationBoiler2 = explanationBoiler2;
            Boiler2ExpectedRepairDate = boiler2ExpectedBackOnlineDate;
            DownTimeBoiler3 = downTimeBoiler3;
            OutageTypeBoiler3 = outageTypeBoiler3;
            ExplanationBoiler3 = explanationBoiler3;
            Boiler3ExpectedRepairDate = boiler3ExpectedBackOnlineDate;
            DownTimeBoiler4 = downTimeBoiler4;
            OutageTypeBoiler4 = outageTypeBoiler4;
            ExplanationBoiler4 = explanationBoiler4;
            Boiler4ExpectedRepairDate = boiler4ExpectedBackOnlineDate;
            DownTimeBoiler5 = downTimeBoiler5;
            OutageTypeBoiler5 = outageTypeBoiler5;
            ExplanationBoiler5 = explanationBoiler5;
            Boiler5ExpectedRepairDate = boiler5ExpectedBackOnlineDate;
            DownTimeBoiler6 = downTimeBoiler6;
            OutageTypeBoiler6 = outageTypeBoiler6;
            ExplanationBoiler6 = explanationBoiler6;
            Boiler6ExpectedRepairDate = boiler6ExpectedBackOnlineDate;
            OutageTypeTurbGen1 = outageTypeTurbGen1;
            DownTimeTurbGen1 = downTimeTurbGen1;
            ExplanationTurbGen1 = explanationTurbGen1;
            TurbGen1ExpectedRepairDate = turbGen1ExpectedBackOnlineDate;
            OutageTypeTurbGen2 = outageTypeTurbGen2;
            DownTimeTurbGen2 = downTimeTurbGen2;
            ExplanationTurbGen2 = explanationTurbGen2;
            TurbGen2ExpectedRepairDate = turbGen2ExpectedBackOnlineDate;
            FerrousTons = ferrousTons;
            NonFerrousTons = nonFerrousTons;
            FerrousSystemHoursUnavailable = ferrousSystemHoursUnavailable;
            FerrousSystemHoursUnavailableReason = ferrousSystemHoursUnavailableReason;
            FerrousSystemExpectedBackOnlineDate = ferrousSystemExpectedBackOnlineDate;
            WasAshReprocessedThroughFerrousSystem = wasAshReprocessedThroughFerrousSystem;
            WasFerrousSystem100PercentAvailable = wasFerrousSystem100PercentAvailable;

            NonFerrousSystemHoursUnavailable = nonFerrousSystemHoursUnavailable;
            NonFerrousSystemHoursUnavailableReason = nonFerrousSystemHoursUnavailableReason;
            NonFerrousSystemExpectedBackOnlineDate = nonFerrousSystemExpectedBackOnlineDate;
            WasAshReprocessedThroughNonFerrousSystem = wasAshReprocessedThroughNonFerrousSystem;
            WasNonFerrousSystem100PercentAvailable = wasNonFerrousSystem100PercentAvailable;

            NonFerrousSmallsSystemHoursUnavailable = nonFerrousSmallsSystemHoursUnavailable;
            NonFerrousSmallsSystemHoursUnavailableReason = nonFerrousSmallsSystemHoursUnavailableReason;
            NonFerrousSmallsSystemExpectedBackOnlineDate = nonFerrousSmallsSystemExpectedBackOnlineDate;
            WasAshReprocessedThroughNonFerrousSmallsSystem = wasAshReprocessedThroughNonFerrousSmallsSystem;
            WasNonFerrousSmallsSystem100PercentAvailable = wasNonFerrousSmallsSystem100PercentAvailable;

            FrontEndFerrousSystemHoursUnavailable = frontEndFerrousSystemHoursUnavailable;
            FrontEndFerrousSystemHoursUnavailableReason = frontEndFerrousSystemHoursUnavailableReason;
            FrontEndFerrousSystemExpectedBackOnlineDate = frontEndFerrousSystemExpectedBackOnlineDate;
            WasAshReprocessedThroughFrontEndFerrousSystem = wasAshReprocessedThroughFrontEndFerrousSystem;
            WasFrontEndFerrousSystem100PercentAvailable = wasFrontEndFerrousSystem100PercentAvailable;

            EnhancedFerrousSystemHoursUnavailable = enhancedFerrousSystemHoursUnavailable;
            EnhancedFerrousSystemHoursUnavailableReason = enhancedFerrousSystemHoursUnavailableReason;
            EnhancedFerrousSystemExpectedBackOnlineDate = enhancedFerrousSystemExpectedBackOnlineDate;
            WasAshReprocessedThroughEnhancedFerrousSystem = wasAshReprocessedThroughEnhancedFerrousSystem;
            WasEnhancedFerrousSystem100PercentAvailable = wasEnhancedFerrousSystem100PercentAvailable;

            EnhancedNonFerrousSystemHoursUnavailable = enhancedNonFerrousSystemHoursUnavailable;
            EnhancedNonFerrousSystemHoursUnavailableReason = enhancedNonFerrousSystemHoursUnavailableReason;
            EnhancedNonFerrousSystemExpectedBackOnlineDate = enhancedNonFerrousSystemExpectedBackOnlineDate;
            WasAshReprocessedThroughEnhancedNonFerrousSystem = wasAshReprocessedThroughEnhancedNonFerrousSystem;
            WasEnhancedNonFerrousSystem100PercentAvailable = wasEnhancedNonFerrousSystem100PercentAvailable;


            FireSystemOutOfService = fireSystemOutOfService;
            FireSystemOutOfServiceExpectedBackOnlineDate = fireSystemOutOfServiceExpectedBackOnlineDate;
            CriticalAssetsInAlarm = criticalAssetsInAlarm;

            IsEnvironmentalEvents = isEnvironmentalEvents;
            EnvironmentalEventsType = environmentalEventsType;
            EnvironmentalEventsExplanation = environmentalEventsExplanation;

            IsCEMSEvents = isCEMSEvents;
            CEMSEventsType = cemsEventsType;
            CEMSEventsExplanation = cemsEventsExplanation;

            HealthSafetyFirstAid = healthSafetyFirstAid;
            HealthSafetyOSHAReportable = healthSafetyOSHAReportable;
            HealthSafetyNearMiss = healthSafetyNearMiss;
            HealthSafetyContractor = healthSafetyContractor;

            Comments = comments;
            UserRowCreated = userRowCreated;

            PitInventory = pitInventory;
            PreShredInventory = preShredInventory;
            PostShredInventory = postShredInventory;
            MassBurnInventory = massBurnInventory;

            CriticalAssetsExpectedBackOnlineDate = criticalAssetsExpectedBackOnlineDate;
            CriticalEquipmentOOSExpectedBackOnlineDate = criticalEquipmentOOSExpectedBackOnlineDate;
        }

        /// <summary>
        /// This is an empty object
        /// </summary>
        /// <param name="facilityID"></param>
        /// <param name="reportingDate"></param>
        public DailyOpsData(string facilityID, DateTime reportingDate)
        {
            FacilityID = facilityID;
            ReportingDate = reportingDate;
        }

        #endregion

        #region public properties

        public string FacilityID { get; set; }
        public DateTime ReportingDate { get; set; }
        public decimal TonsDelivered { get; set; }
        public decimal TonsProcessed { get; set; }
        public decimal SteamProduced { get; set; }
        public decimal SteamSold { get; set; }
        public decimal NetElectric { get; set; }

        public decimal DownTimeBoiler1 { get; set; }
        public string OutageTypeBoiler1 { get; set; }
        public string ExplanationBoiler1 { get; set; }
        public string ScheduledOutageReasonBoiler1 { get; set; }
        public DateTime Boiler1ExpectedRepairDate { get; set; }

        public decimal DownTimeBoiler2 { get; set; }
        public string OutageTypeBoiler2 { get; set; }
        public string ExplanationBoiler2 { get; set; }
        public string ScheduledOutageReasonBoiler2 { get; set; }
        public DateTime Boiler2ExpectedRepairDate { get; set; }

        public decimal DownTimeBoiler3 { get; set; }
        public string OutageTypeBoiler3 { get; set; }
        public string ExplanationBoiler3 { get; set; }
        public string ScheduledOutageReasonBoiler3 { get; set; }
        public DateTime Boiler3ExpectedRepairDate { get; set; }

        public decimal DownTimeBoiler4 { get; set; }
        public string OutageTypeBoiler4 { get; set; }
        public string ExplanationBoiler4 { get; set; }
        public string ScheduledOutageReasonBoiler4 { get; set; }
        public DateTime Boiler4ExpectedRepairDate { get; set; }

        public decimal DownTimeBoiler5 { get; set; }
        public string OutageTypeBoiler5 { get; set; }
        public string ExplanationBoiler5 { get; set; }
        public string ScheduledOutageReasonBoiler5 { get; set; }
        public DateTime Boiler5ExpectedRepairDate { get; set; }

        public decimal DownTimeBoiler6 { get; set; }
        public string OutageTypeBoiler6 { get; set; }
        public string ExplanationBoiler6 { get; set; }
        public string ScheduledOutageReasonBoiler6 { get; set; }
        public DateTime Boiler6ExpectedRepairDate { get; set; }

        public string OutageTypeTurbGen1 { get; set; }
        public decimal DownTimeTurbGen1 { get; set; }
        public string ExplanationTurbGen1 { get; set; }
        public string ScheduledOutageReasonTurbGen1 { get; set; }
        public DateTime TurbGen1ExpectedRepairDate { get; set; }

        public string OutageTypeTurbGen2 { get; set; }
        public decimal DownTimeTurbGen2 { get; set; }
        public string ExplanationTurbGen2 { get; set; }
        public string ScheduledOutageReasonTurbGen2 { get; set; }
        public DateTime TurbGen2ExpectedRepairDate { get; set; }

        public decimal FerrousTons { get; set; }
        public decimal NonFerrousTons { get; set; }
        public decimal FerrousSystemHoursUnavailable { get; set; }
        public string FerrousSystemHoursUnavailableReason { get; set; }
        public DateTime FerrousSystemExpectedBackOnlineDate { get; set; }
        public string WasAshReprocessedThroughFerrousSystem { get; set; }
        public string WasFerrousSystem100PercentAvailable { get; set; }

        public decimal NonFerrousSystemHoursUnavailable { get; set; }
        public string NonFerrousSystemHoursUnavailableReason { get; set; }
        public DateTime NonFerrousSystemExpectedBackOnlineDate { get; set; }
        public string WasAshReprocessedThroughNonFerrousSystem { get; set; }
        public string WasNonFerrousSystem100PercentAvailable { get; set; }

        public decimal NonFerrousSmallsSystemHoursUnavailable { get; set; }
        public string NonFerrousSmallsSystemHoursUnavailableReason { get; set; }
        public DateTime NonFerrousSmallsSystemExpectedBackOnlineDate { get; set; }
        public string WasAshReprocessedThroughNonFerrousSmallsSystem { get; set; }
        public string WasNonFerrousSmallsSystem100PercentAvailable { get; set; }

        public decimal FrontEndFerrousSystemHoursUnavailable { get; set; }
        public string FrontEndFerrousSystemHoursUnavailableReason { get; set; }
        public DateTime FrontEndFerrousSystemExpectedBackOnlineDate { get; set; }
        public string WasAshReprocessedThroughFrontEndFerrousSystem { get; set; }
        public string WasFrontEndFerrousSystem100PercentAvailable { get; set; }

        public decimal EnhancedFerrousSystemHoursUnavailable { get; set; }
        public string EnhancedFerrousSystemHoursUnavailableReason { get; set; }
        public DateTime EnhancedFerrousSystemExpectedBackOnlineDate { get; set; }
        public string WasAshReprocessedThroughEnhancedFerrousSystem { get; set; }
        public string WasEnhancedFerrousSystem100PercentAvailable { get; set; }

        public decimal EnhancedNonFerrousSystemHoursUnavailable { get; set; }
        public string EnhancedNonFerrousSystemHoursUnavailableReason { get; set; }
        public DateTime EnhancedNonFerrousSystemExpectedBackOnlineDate { get; set; }
        public string WasAshReprocessedThroughEnhancedNonFerrousSystem { get; set; }
        public string WasEnhancedNonFerrousSystem100PercentAvailable { get; set; }

        public string FireSystemOutOfService { get; set; }
        public DateTime FireSystemOutOfServiceExpectedBackOnlineDate { get; set; }
        public string CriticalAssetsInAlarm { get; set; }

        public bool IsEnvironmentalEvents { get; set; }
        public string EnvironmentalEventsType { get; set; }
        public string EnvironmentalEventsExplanation { get; set; }

        public bool IsCEMSEvents { get; set; }
        public string CEMSEventsType { get; set; }
        public string CEMSEventsExplanation { get; set; }

        public string HealthSafetyFirstAid { get; set; }
        public string HealthSafetyOSHAReportable { get; set; }
        public string HealthSafetyNearMiss { get; set; }
        public string HealthSafetyContractor { get; set; }

        public string Comments { get; set; }

        public string UserRowCreated { get; set; }
        public DateTime DateLastModified { get; set; }

        public string FaciltyDescription { get; set; }
        public string FaciltyType { get; set; }

        public decimal PitInventory { get; set; }

        public decimal PreShredInventory { get; set; }

        public decimal PostShredInventory { get; set; }

        public decimal MassBurnInventory { get; set; }

        public DateTime CriticalAssetsExpectedBackOnlineDate { get; set; }

        public DateTime CriticalEquipmentOOSExpectedBackOnlineDate { get; set; }

        #endregion

    }
}