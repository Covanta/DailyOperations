using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Common;
using Covanta.Entities;
using Covanta.DataAccess;
using Covanta.Common.Enums;
using Covanta.Utilities.Helpers;

namespace Covanta.BusinessLogic
{
    public class DailyOpsManager
    {
        #region private variables and Constants

        string _dbConnection = null;
        List<DateTime> dateList = null;
        private const string scheduledOutage = "Scheduled Outage";
        private const string scheduledTGOutage = "Scheduled TG Outage";

        #endregion

        #region constructors

        public DailyOpsManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == ""))
            {
                EmailHelper.SendEmail("DailyOpsManager missing Connection String");
                _dbConnection = "";
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Returns a list a Daily Ops data based on the parameters.
        /// If facilityID is passed in, then we ignore the region passed in and, we return all data for the reporting date for that facility.
        /// If region is passed in, then we ignore the facility passed in and, we return all data for the reporting date for that region.
        /// If facilityID is String.Empty and region is String.Empty also, we return all data for the reporting date
        /// Note.  Reporting date must be passed in.
        /// </summary>
        /// <param name="dateDataRepresents">>Date we want data from</param>
        /// <param name="faciltyID">Facility we want data from.  String.Empty is allowed</param>
        /// <param name="region">Region we want data from.  String.Empty is allowed</param>       
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>a list of DailyOpsData objects</returns>
        public List<DailyOpsData> GetDailyOpsDataByParameters(DateTime dateDataRepresents, string faciltyID, string region, ref Enums.StatusEnum status)
        {
            //return all data for reporting date
            if ((faciltyID == string.Empty) && (region == string.Empty))
            {
                return GetDailyOpsDataByDate(dateDataRepresents, ref status);
            }

            //return all data for reporting date for
            //facility
            if (faciltyID != string.Empty)
            {
                List<DailyOpsData> list = new List<DailyOpsData>();
                DailyOpsData item = null;
                item = GetDailyOpsDataByDateAndFacility(dateDataRepresents, faciltyID, ref status);
                if (item != null)
                {
                    list.Add(item);
                }
                return list;
            }

            //return all data for reporting date for
            //region
            if (region != string.Empty)
            {
                return GetDailyOpsDataByDateAndRegion(dateDataRepresents, region, ref status);
            }

            //we shouldn't reach this code
            List<DailyOpsData> returnList = new List<DailyOpsData>();
            return returnList;
        }

        /// <summary>
        ///  Get 1 days worth of Daily Ops Data from the database by 1 specific Date and 1 specific Facility
        /// </summary>
        /// <param name="dateDataRepresents">>Date we want data from</param>
        /// <param name="faciltyID">Facility ID we want data from (Currently we use the PeopleSoft 5 char code)</param>
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>1 DailyOpsData object</returns>
        public DailyOpsData GetDailyOpsDataByDateAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status)
        {
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            status = Enums.StatusEnum.OK;
            string statusMsg = string.Empty;
            DailyOpsData dod = dal.LoadDailyOpsDataByDateAndFacility(dateDataRepresents, faciltyID, ref status, ref statusMsg);
            if (status != Enums.StatusEnum.OK)
            {
                EmailHelper.SendEmail(statusMsg);
            }
            List<DailyOpsData> dodList = new List<DailyOpsData>();
            // if we didn't have any data, then return an empty object (not null)
            if (dod != null)
            {
                dodList.Add(dod);
            }
            else
            {
                dod = new DailyOpsData(faciltyID, dateDataRepresents);
                dodList.Add(dod);
            }

            populateAdditionalDailyOpsDataProperties(ref dodList);
            return dod;
        }

        public CompleteDowntime GetCumulativeDowntime(DateTime dateDataRepresents, string faciltyID, Enums.DowntimeBoilerEnum downtimeBoiler, ref Enums.StatusEnum status)
        {
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            status = Enums.StatusEnum.OK;
            string statusMsg = string.Empty;
            CompleteDowntime dod = dal.CumulativeDowntime(dateDataRepresents, faciltyID, downtimeBoiler, ref status, ref statusMsg);
            if (status != Enums.StatusEnum.OK)
            {
                EmailHelper.SendEmail(statusMsg);
            }
            return dod;
        }

        public DailyOpsDataWithID GetDailyOpsDataByID(int ID, ref Enums.StatusEnum status)
        {
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            status = Enums.StatusEnum.OK;
            string statusMsg = string.Empty;
            DailyOpsDataWithID dod = dal.LoadDailyOpsDataByID(ID, ref status, ref statusMsg);
            if (status != Enums.StatusEnum.OK)
            {
                EmailHelper.SendEmail(statusMsg);
            }
            List<DailyOpsDataWithID> dodList = new List<DailyOpsDataWithID>();
            // if we didn't have any data, then return an empty object (not null)
            if (dod != null)
            {
                dodList.Add(dod);
            }
            else
            {
                dod = new DailyOpsDataWithID(ID);
                dodList.Add(dod);
            }

            populateAdditionalDailyOpsDataWithIDProperties(ref dodList);
            return dod;
        }

        /// <summary>
        /// Gets a list of exception report email Recipients 
        /// </summary>
        /// <returns>list of exception report email Recipients </returns>
        public List<string> GetDailyOpsExceptionEmailRecipientsList()
        {
            List<string> list = new List<string>();
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            list = dal.GetDailyOpsExceptionEmailRecipientsList();
            return list;
        }

        /// <summary>
        /// Get a list of Daily Ops Data from the database by 1 specific Date and 1 specific Region
        /// </summary>
        /// <param name="dateDataRepresents">Date we want data from</param>
        /// <param name="region">Region we want data from</param>
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>a list of DailyOpsData objects</returns>
        public List<DailyOpsData> GetDailyOpsDataByDateAndRegion(DateTime dateDataRepresents, string region, ref Enums.StatusEnum status)
        {
            //get list of facilities in region.
            DailyOpsBusiUnitManager BUManager = new DailyOpsBusiUnitManager(_dbConnection);
            List<string> facilityListInRegion = BUManager.GetFacilityListbyRegion(region);
            BUManager = null;

            //get a list of Daily Ops data which inclused all of the facilities in the region
            List<DailyOpsData> listDailyOpsData = new List<DailyOpsData>();

            DALDailyOps dalDailyOps = new DALDailyOps(_dbConnection);
            status = Enums.StatusEnum.OK;
            string statusMsg = string.Empty;
            foreach (string facility in facilityListInRegion)
            {
                DailyOpsData dod = dalDailyOps.LoadDailyOpsDataByDateAndFacility(dateDataRepresents, facility, ref status, ref statusMsg);

                // if we didn't have any data, then return an empty object (not null)
                if (dod != null)
                {
                    listDailyOpsData.Add(dod);
                }
                else
                {
                    dod = new DailyOpsData(facility, dateDataRepresents);
                    listDailyOpsData.Add(dod);
                }
            }

            if (status != Enums.StatusEnum.OK)
            {
                EmailHelper.SendEmail(statusMsg);
            }
            populateAdditionalDailyOpsDataProperties(ref listDailyOpsData);
            return listDailyOpsData;
        }

        /// <summary>
        /// Get a list of Daily Ops Data from the database by 1 specific Date for all facilities
        /// </summary>
        /// <param name="dateDataRepresents">Date we want data for</param>
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>a list of DailyOpsData objects</returns>
        public List<DailyOpsData> GetDailyOpsDataByDate(DateTime dateDataRepresents, ref Enums.StatusEnum status)
        {
            //get list of facilities
            DailyOpsBusiUnitManager BUManager = new DailyOpsBusiUnitManager(_dbConnection);
            List<string> facilityList = BUManager.GetBusinessUnitsPeopleSoft5DigitCodeList();
            BUManager = null;

            //get a list of Daily Ops data for all of the facilities on a particular date
            List<DailyOpsData> listDailyOpsData = new List<DailyOpsData>();

            DALDailyOps dalDailyOps = new DALDailyOps(_dbConnection);
            status = Enums.StatusEnum.OK;
            string statusMsg = string.Empty;
            foreach (string facility in facilityList)
            {
                DailyOpsData dod = dalDailyOps.LoadDailyOpsDataByDateAndFacility(dateDataRepresents, facility, ref status, ref statusMsg);

                // if we didn't have any data, then return an empty object (not null)
                if (dod != null)
                {
                    listDailyOpsData.Add(dod);
                }
                else
                {
                    dod = new DailyOpsData(facility, dateDataRepresents);
                    listDailyOpsData.Add(dod);
                }
            }

            if (status != Enums.StatusEnum.OK)
            {
                EmailHelper.SendEmail(statusMsg);
            }
            populateAdditionalDailyOpsDataProperties(ref listDailyOpsData);
            return listDailyOpsData;
        }

        /// <summary>
        /// Gets a list of DailyOpsMetalSystemsStatus objects based on parameters
        /// </summary>
        /// <param name="dateDataRepresents">Date we want data for</param>
        /// <param name="faciltyID">Facility ID we want data from (Currently we use the PeopleSoft 5 char code)</param>
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>A list of DailyOpsMetalSystemsStatus objects</returns>
        public List<DailyOpsMetalSystemsStatus> GetDailyOpsMetalsStatusByDateAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status)
        {
            List<DailyOpsMetalSystemsStatus> listDailyOpsMetalsStatus = new List<DailyOpsMetalSystemsStatus>();
            DailyOpsData dailyOpsData = GetDailyOpsDataByDateAndFacility(dateDataRepresents, faciltyID, ref status);
            if (status != Enums.StatusEnum.OK) { return listDailyOpsMetalsStatus; }
            if (dailyOpsData == null) { return listDailyOpsMetalsStatus; }

            //Types
            string ferrous = "Ferrous";
            string nonFerrous = "Non-Ferrous";
            string nonFerrousSmalls = "Non-Ferrous Smalls";
            string frontEndFerrous = "Front End Ferrous";
            string enhancedFerrous = "Enhanced Ferrous";
            string enhancedNonFerrous = "Enhanced Non Ferrous";

            //Ferrous
            if (dailyOpsData.WasFerrousSystem100PercentAvailable == "N")
            {
                DailyOpsMetalSystemsStatus ferrousStatusObject = new DailyOpsMetalSystemsStatus(
                    ferrous,
                    dailyOpsData.FerrousSystemHoursUnavailable,
                    dailyOpsData.FerrousSystemHoursUnavailableReason,
                    dailyOpsData.FerrousSystemExpectedBackOnlineDate,
                    dailyOpsData.WasAshReprocessedThroughFerrousSystem);

                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.FerrousSystemHoursUnavailable, ref status);
                ferrousStatusObject.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                ferrousStatusObject.WeekToDate = completeDowntime.WeekToDate;
                ferrousStatusObject.MonthToDate = completeDowntime.MonthToDate;

                listDailyOpsMetalsStatus.Add(ferrousStatusObject);
            }

            //nonFerrous
            if (dailyOpsData.WasNonFerrousSystem100PercentAvailable == "N")
            {
                DailyOpsMetalSystemsStatus nonFerrousStatusObject = new DailyOpsMetalSystemsStatus(
                    nonFerrous,
                    dailyOpsData.NonFerrousSystemHoursUnavailable,
                    dailyOpsData.NonFerrousSystemHoursUnavailableReason,
                    dailyOpsData.NonFerrousSystemExpectedBackOnlineDate,
                    dailyOpsData.WasAshReprocessedThroughNonFerrousSystem);

                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.NonFerrousSystemHoursUnavailable, ref status);
                nonFerrousStatusObject.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                nonFerrousStatusObject.WeekToDate = completeDowntime.WeekToDate;
                nonFerrousStatusObject.MonthToDate = completeDowntime.MonthToDate;

                listDailyOpsMetalsStatus.Add(nonFerrousStatusObject);
            }

            //nonFerrousSmalls
            if (dailyOpsData.WasNonFerrousSmallsSystem100PercentAvailable == "N")
            {
                DailyOpsMetalSystemsStatus nonFerrousSmallsStatusObject = new DailyOpsMetalSystemsStatus(
                    nonFerrousSmalls,
                    dailyOpsData.NonFerrousSmallsSystemHoursUnavailable,
                    dailyOpsData.NonFerrousSmallsSystemHoursUnavailableReason,
                    dailyOpsData.NonFerrousSmallsSystemExpectedBackOnlineDate,
                    dailyOpsData.WasAshReprocessedThroughNonFerrousSmallsSystem);

                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.NonFerrousSmallsSystemHoursUnavailable, ref status);
                nonFerrousSmallsStatusObject.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                nonFerrousSmallsStatusObject.WeekToDate = completeDowntime.WeekToDate;
                nonFerrousSmallsStatusObject.MonthToDate = completeDowntime.MonthToDate;

                listDailyOpsMetalsStatus.Add(nonFerrousSmallsStatusObject);
            }

            //frontEndFerrous
            if (dailyOpsData.WasFrontEndFerrousSystem100PercentAvailable == "N")
            {
                DailyOpsMetalSystemsStatus frontEndFerrousStatusObject = new DailyOpsMetalSystemsStatus(
                    frontEndFerrous,
                    dailyOpsData.FrontEndFerrousSystemHoursUnavailable,
                    dailyOpsData.FrontEndFerrousSystemHoursUnavailableReason,
                    dailyOpsData.FrontEndFerrousSystemExpectedBackOnlineDate,
                    "N/A");

                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.FrontEndFerrousSystemHoursUnavailable, ref status);
                frontEndFerrousStatusObject.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                frontEndFerrousStatusObject.WeekToDate = completeDowntime.WeekToDate;
                frontEndFerrousStatusObject.MonthToDate = completeDowntime.MonthToDate;

                listDailyOpsMetalsStatus.Add(frontEndFerrousStatusObject);
            }

            //Enhanced Ferrous
            if (dailyOpsData.WasEnhancedFerrousSystem100PercentAvailable == "N")
            {
                DailyOpsMetalSystemsStatus enhancedFerrousStatusObject = new DailyOpsMetalSystemsStatus(
                    enhancedFerrous,
                    dailyOpsData.EnhancedFerrousSystemHoursUnavailable,
                    dailyOpsData.EnhancedFerrousSystemHoursUnavailableReason,
                    dailyOpsData.EnhancedFerrousSystemExpectedBackOnlineDate,
                    dailyOpsData.WasAshReprocessedThroughEnhancedFerrousSystem);

                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.EnhancedFerrousSystemHoursUnavailable, ref status);
                enhancedFerrousStatusObject.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                enhancedFerrousStatusObject.WeekToDate = completeDowntime.WeekToDate;
                enhancedFerrousStatusObject.MonthToDate = completeDowntime.MonthToDate;

                listDailyOpsMetalsStatus.Add(enhancedFerrousStatusObject);
            }

            //Enhanced nonFerrous
            if (dailyOpsData.WasEnhancedNonFerrousSystem100PercentAvailable == "N")
            {
                DailyOpsMetalSystemsStatus enhancedNonFerrousStatusObject = new DailyOpsMetalSystemsStatus(
                    enhancedNonFerrous,
                    dailyOpsData.EnhancedNonFerrousSystemHoursUnavailable,
                    dailyOpsData.EnhancedNonFerrousSystemHoursUnavailableReason,
                    dailyOpsData.EnhancedNonFerrousSystemExpectedBackOnlineDate,
                    dailyOpsData.WasAshReprocessedThroughEnhancedNonFerrousSystem);

                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.EnhancedNonFerrousSystemHoursUnavailable, ref status);
                enhancedNonFerrousStatusObject.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                enhancedNonFerrousStatusObject.WeekToDate = completeDowntime.WeekToDate;
                enhancedNonFerrousStatusObject.MonthToDate = completeDowntime.MonthToDate;

                listDailyOpsMetalsStatus.Add(enhancedNonFerrousStatusObject);
            }

            return listDailyOpsMetalsStatus;
        }

        /// <summary>
        /// Gets a list of DailyOpsReportableEvents by Health And Safety Nature objects based on parameters
        /// </summary>
        /// <param name="dateDataRepresents">Date we want data for</param>
        /// <param name="faciltyID">Facility ID we want data from (Currently we use the PeopleSoft 5 char code)</param>
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>A list of DailyOpsReportableEvent objects</returns>
        public List<DailyOpsReportableEvent> GetDailyOpsEventsHealthAndSafetyByDateAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status)
        {
            List<DailyOpsReportableEvent> listDailyOpsReportableEvents = new List<DailyOpsReportableEvent>();
            DailyOpsData dailyOpsData = GetDailyOpsDataByDateAndFacility(dateDataRepresents, faciltyID, ref status);
            if (status != Enums.StatusEnum.OK) { return listDailyOpsReportableEvents; }
            if (dailyOpsData == null) { return listDailyOpsReportableEvents; }

            string firstAid = "First Aid";
            string oshaRecordable = "OSHA Recordable";
            string nearMiss = "Near Miss";
            string contractor = "Contractor";

            //FirstAid
            if (dailyOpsData.HealthSafetyFirstAid != string.Empty)
            {
                DailyOpsReportableEvent firstAidObject = new DailyOpsReportableEvent(firstAid, dailyOpsData.HealthSafetyFirstAid);
                listDailyOpsReportableEvents.Add(firstAidObject);
            }
            //oshaRecordable
            if (dailyOpsData.HealthSafetyOSHAReportable != string.Empty)
            {
                DailyOpsReportableEvent oshaObject = new DailyOpsReportableEvent(oshaRecordable, dailyOpsData.HealthSafetyOSHAReportable);
                listDailyOpsReportableEvents.Add(oshaObject);
            }
            //nearMiss
            if (dailyOpsData.HealthSafetyNearMiss != string.Empty)
            {
                DailyOpsReportableEvent nearMissObject = new DailyOpsReportableEvent(nearMiss, dailyOpsData.HealthSafetyNearMiss);
                listDailyOpsReportableEvents.Add(nearMissObject);
            }
            //contractor
            if (dailyOpsData.HealthSafetyContractor != string.Empty)
            {
                DailyOpsReportableEvent contractorObject = new DailyOpsReportableEvent(contractor, dailyOpsData.HealthSafetyContractor);
                listDailyOpsReportableEvents.Add(contractorObject);
            }

            return listDailyOpsReportableEvents;
        }

        /// <summary>
        /// Gets a list of DailyOpsReportableEvent of Environments Nature objects based on parameters
        /// </summary>
        /// <param name="dateDataRepresents">Date we want data for</param>
        /// <param name="faciltyID">Facility ID we want data from (Currently we use the PeopleSoft 5 char code)</param>
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>A list of DailyOpsReportableEvent objects</returns>
        public List<DailyOpsReportableEvent> GetDailyOpsEventsEnvironmentalByDateAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status)
        {
            List<DailyOpsReportableEvent> listDailyOpsReportableEvents = new List<DailyOpsReportableEvent>();
            DailyOpsData dailyOpsData = GetDailyOpsDataByDateAndFacility(dateDataRepresents, faciltyID, ref status);
            if (status != Enums.StatusEnum.OK) { return listDailyOpsReportableEvents; }
            if (dailyOpsData == null) { return listDailyOpsReportableEvents; }

            const string constantReportableExempt = "Reportable Exempt";
            //Types
            string environmentalReportable = "Environmental (Reportable)";
            string environmentalReportableExempt = "Environmental (Reportable Exempt)";
            string cemsReportable = "CEMS (Reportable)";
            string cemsReportableExempt = "CEMS (Reportable Exempt)";

            //environmental
            if (dailyOpsData.IsEnvironmentalEvents == true)
            {
                //Not Reportable
                if (dailyOpsData.EnvironmentalEventsType == constantReportableExempt)
                {
                    DailyOpsReportableEvent environmentalReportableObject = new DailyOpsReportableEvent(
                        environmentalReportableExempt, dailyOpsData.EnvironmentalEventsExplanation);
                    listDailyOpsReportableEvents.Add(environmentalReportableObject);
                }
                else
                {
                    //Reportable
                    DailyOpsReportableEvent environmentalReportableObject = new DailyOpsReportableEvent(
                    environmentalReportable, dailyOpsData.EnvironmentalEventsExplanation);
                    listDailyOpsReportableEvents.Add(environmentalReportableObject);
                }
            }

            //CEMS
            if (dailyOpsData.IsCEMSEvents == true)
            {
                //Not Reportable
                if (dailyOpsData.CEMSEventsType == constantReportableExempt)
                {
                    DailyOpsReportableEvent cemsReportableObject = new DailyOpsReportableEvent(
                        cemsReportableExempt, dailyOpsData.CEMSEventsExplanation);
                    listDailyOpsReportableEvents.Add(cemsReportableObject);
                }
                else
                {
                    //Reportable
                    DailyOpsReportableEvent cemsReportableObject = new DailyOpsReportableEvent(
                    cemsReportable, dailyOpsData.CEMSEventsExplanation);
                    listDailyOpsReportableEvents.Add(cemsReportableObject);
                }
            }

            return listDailyOpsReportableEvents;
        }

        /// <summary>
        /// Gets a list of Boiler Status objects based on parameters
        /// </summary>
        /// <param name="dateDataRepresents">Date we want data for</param>
        /// <param name="faciltyID">Facility ID we want data from (Currently we use the PeopleSoft 5 char code)</param>
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>A list of Boiler Status objects</returns>
        public List<DailyOpsBoilerStatus> GetDailyOpsBoilerStatusByDateAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status)
        {
            List<DailyOpsBoilerStatus> listDailyOpsBoilerStatus = new List<DailyOpsBoilerStatus>();
            DailyOpsData dailyOpsData = GetDailyOpsDataByDateAndFacility(dateDataRepresents, faciltyID, ref status);
            if (status != Enums.StatusEnum.OK) { return listDailyOpsBoilerStatus; }
            if (dailyOpsData == null) { return listDailyOpsBoilerStatus; }

            //1
            if ((dailyOpsData.OutageTypeBoiler1 != string.Empty) && (dailyOpsData.OutageTypeBoiler1 != "Operational") && (dailyOpsData.OutageTypeBoiler1 != "Decommissioned"))
            {
                var dailyOpsBoilerStatus = new DailyOpsBoilerStatus(dateDataRepresents, faciltyID, 1, dailyOpsData.OutageTypeBoiler1, dailyOpsData.ExplanationBoiler1, dailyOpsData.DownTimeBoiler1, dailyOpsData.Boiler1ExpectedRepairDate);
                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.DowntimeBoiler1, ref status);
                dailyOpsBoilerStatus.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                dailyOpsBoilerStatus.WeekToDate = completeDowntime.WeekToDate;
                dailyOpsBoilerStatus.MonthToDate = completeDowntime.MonthToDate;
                listDailyOpsBoilerStatus.Add(dailyOpsBoilerStatus);
            }
            //2
            if ((dailyOpsData.OutageTypeBoiler2 != string.Empty) && (dailyOpsData.OutageTypeBoiler2 != "Operational") && (dailyOpsData.OutageTypeBoiler2 != "Decommissioned"))
            {
                var dailyOpsBoilerStatus = new DailyOpsBoilerStatus(dateDataRepresents, faciltyID, 2, dailyOpsData.OutageTypeBoiler2, dailyOpsData.ExplanationBoiler2, dailyOpsData.DownTimeBoiler2, dailyOpsData.Boiler2ExpectedRepairDate);
                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.DowntimeBoiler2, ref status);
                dailyOpsBoilerStatus.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                dailyOpsBoilerStatus.WeekToDate = completeDowntime.WeekToDate;
                dailyOpsBoilerStatus.MonthToDate = completeDowntime.MonthToDate;
                listDailyOpsBoilerStatus.Add(dailyOpsBoilerStatus);
            }
            //3
            if ((dailyOpsData.OutageTypeBoiler3 != string.Empty) && (dailyOpsData.OutageTypeBoiler3 != "Operational") && (dailyOpsData.OutageTypeBoiler3 != "Decommissioned"))
            {
                var dailyOpsBoilerStatus = new DailyOpsBoilerStatus(dateDataRepresents, faciltyID, 3, dailyOpsData.OutageTypeBoiler3, dailyOpsData.ExplanationBoiler3, dailyOpsData.DownTimeBoiler3, dailyOpsData.Boiler3ExpectedRepairDate);
                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.DowntimeBoiler3, ref status);
                dailyOpsBoilerStatus.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                dailyOpsBoilerStatus.WeekToDate = completeDowntime.WeekToDate;
                dailyOpsBoilerStatus.MonthToDate = completeDowntime.MonthToDate;
                listDailyOpsBoilerStatus.Add(dailyOpsBoilerStatus);
            }
            //4
            if ((dailyOpsData.OutageTypeBoiler4 != string.Empty) && (dailyOpsData.OutageTypeBoiler4 != "Operational") && (dailyOpsData.OutageTypeBoiler4 != "Decommissioned"))
            {
                var dailyOpsBoilerStatus = new DailyOpsBoilerStatus(dateDataRepresents, faciltyID, 4, dailyOpsData.OutageTypeBoiler4, dailyOpsData.ExplanationBoiler4, dailyOpsData.DownTimeBoiler4, dailyOpsData.Boiler4ExpectedRepairDate);
                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.DowntimeBoiler4, ref status);
                dailyOpsBoilerStatus.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                dailyOpsBoilerStatus.WeekToDate = completeDowntime.WeekToDate;
                dailyOpsBoilerStatus.MonthToDate = completeDowntime.MonthToDate;
                listDailyOpsBoilerStatus.Add(dailyOpsBoilerStatus);
            }
            //5
            if ((dailyOpsData.OutageTypeBoiler5 != string.Empty) && (dailyOpsData.OutageTypeBoiler5 != "Operational") && (dailyOpsData.OutageTypeBoiler5 != "Decommissioned"))
            {
                var dailyOpsBoilerStatus = new DailyOpsBoilerStatus(dateDataRepresents, faciltyID, 5, dailyOpsData.OutageTypeBoiler5, dailyOpsData.ExplanationBoiler5, dailyOpsData.DownTimeBoiler5, dailyOpsData.Boiler5ExpectedRepairDate);
                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.DowntimeBoiler5, ref status);
                dailyOpsBoilerStatus.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                dailyOpsBoilerStatus.WeekToDate = completeDowntime.WeekToDate;
                dailyOpsBoilerStatus.MonthToDate = completeDowntime.MonthToDate;
                listDailyOpsBoilerStatus.Add(dailyOpsBoilerStatus);
            }
            //6
            if ((dailyOpsData.OutageTypeBoiler6 != string.Empty) && (dailyOpsData.OutageTypeBoiler6 != "Operational") && (dailyOpsData.OutageTypeBoiler6 != "Decommissioned"))
            {
                var dailyOpsBoilerStatus = new DailyOpsBoilerStatus(dateDataRepresents, faciltyID, 6, dailyOpsData.OutageTypeBoiler6, dailyOpsData.ExplanationBoiler6, dailyOpsData.DownTimeBoiler6, dailyOpsData.Boiler6ExpectedRepairDate);
                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.DowntimeBoiler6, ref status);
                dailyOpsBoilerStatus.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                dailyOpsBoilerStatus.WeekToDate = completeDowntime.WeekToDate;
                dailyOpsBoilerStatus.MonthToDate = completeDowntime.MonthToDate;
                listDailyOpsBoilerStatus.Add(dailyOpsBoilerStatus);
            }

            // Added 05/03/2013 - Brian L
            // If we have a Scheduled Boiler Outage, show the value from the Scheldule Reason Dropdown in the Exception Report.
            // It has to go in the Unschedlued Reason part of the page to keep the page look consistant.

            foreach (DailyOpsBoilerStatus stat in listDailyOpsBoilerStatus)
            {
                if (stat.Status == scheduledOutage)
                {
                    if (stat.BoilerNumber == 1)
                    {
                        stat.UnscheduledOutageExplanation = dailyOpsData.ScheduledOutageReasonBoiler1;
                    }
                    else
                        if (stat.BoilerNumber == 2)
                    {
                        stat.UnscheduledOutageExplanation = dailyOpsData.ScheduledOutageReasonBoiler2;
                    }
                    else
                            if (stat.BoilerNumber == 3)
                    {
                        stat.UnscheduledOutageExplanation = dailyOpsData.ScheduledOutageReasonBoiler3;
                    }
                    else
                                if (stat.BoilerNumber == 4)
                    {
                        stat.UnscheduledOutageExplanation = dailyOpsData.ScheduledOutageReasonBoiler4;
                    }
                    else
                                    if (stat.BoilerNumber == 5)
                    {
                        stat.UnscheduledOutageExplanation = dailyOpsData.ScheduledOutageReasonBoiler5;
                    }
                    else
                                        if (stat.BoilerNumber == 6)
                    {
                        stat.UnscheduledOutageExplanation = dailyOpsData.ScheduledOutageReasonBoiler6;
                    }
                }
            }

            return listDailyOpsBoilerStatus;
        }

        /// <summary>
        /// Populates a list of Boiler Status objects based on parameters with Turbine Gen data
        /// </summary>
        /// <param name="dateDataRepresents">Date we want data for</param>
        /// <param name="faciltyID">Facility ID we want data from (Currently we use the PeopleSoft 5 char code)</param>
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>A list of Boiler Status objects with Tubine Gen data populated</returns>
        public List<DailyOpsBoilerStatus> GetDailyOpsTurbGenByDateAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status)
        {
            List<DailyOpsBoilerStatus> listDailyOpsTurbGenStatus = new List<DailyOpsBoilerStatus>();
            DailyOpsData dailyOpsData = GetDailyOpsDataByDateAndFacility(dateDataRepresents, faciltyID, ref status);
            if (status != Enums.StatusEnum.OK) { return listDailyOpsTurbGenStatus; }
            if (dailyOpsData == null) { return listDailyOpsTurbGenStatus; }


            //1
            if ((dailyOpsData.OutageTypeTurbGen1 != string.Empty) && (dailyOpsData.OutageTypeTurbGen1 != "Operational"))
            {
                var dailyOpsTurbGenStatus = new DailyOpsBoilerStatus(dateDataRepresents, faciltyID, 1, dailyOpsData.OutageTypeTurbGen1, dailyOpsData.ExplanationTurbGen1, dailyOpsData.DownTimeTurbGen1, dailyOpsData.TurbGen1ExpectedRepairDate);
                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.DownTimeTurbGen1, ref status);
                dailyOpsTurbGenStatus.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                dailyOpsTurbGenStatus.WeekToDate = completeDowntime.WeekToDate;
                dailyOpsTurbGenStatus.MonthToDate = completeDowntime.MonthToDate;
                listDailyOpsTurbGenStatus.Add(dailyOpsTurbGenStatus);
            }
            //2
            if ((dailyOpsData.OutageTypeTurbGen2 != string.Empty) && (dailyOpsData.OutageTypeTurbGen2 != "Operational"))
            {
                var dailyOpsTurbGenStatus = new DailyOpsBoilerStatus(dateDataRepresents, faciltyID, 2, dailyOpsData.OutageTypeTurbGen2, dailyOpsData.ExplanationTurbGen2, dailyOpsData.DownTimeTurbGen2, dailyOpsData.TurbGen1ExpectedRepairDate);
                var completeDowntime = GetCumulativeDowntime(dateDataRepresents, faciltyID, Enums.DowntimeBoilerEnum.DownTimeTurbGen2, ref status);
                dailyOpsTurbGenStatus.CumulativeDowntime = completeDowntime.CumulativeDowntime;
                dailyOpsTurbGenStatus.WeekToDate = completeDowntime.WeekToDate;
                dailyOpsTurbGenStatus.MonthToDate = completeDowntime.MonthToDate;
                listDailyOpsTurbGenStatus.Add(dailyOpsTurbGenStatus);
            }

            // Added 05/03/2013 - Brian L
            // If we have a Scheduled Turb Gen Outage, show the value from the Scheldule Reason Dropdown in the Exception Report.
            // It has to go in the Unscheduled Reason part of the page to keep the page look consistant.

            foreach (DailyOpsBoilerStatus stat in listDailyOpsTurbGenStatus)
            {
                if (stat.Status == scheduledTGOutage)
                {
                    if (stat.BoilerNumber == 1)
                    {
                        stat.UnscheduledOutageExplanation = dailyOpsData.ScheduledOutageReasonTurbGen1;
                    }
                    else
                        if (stat.BoilerNumber == 2)
                    {
                        stat.UnscheduledOutageExplanation = dailyOpsData.ScheduledOutageReasonTurbGen2;
                    }
                }
            }
            return listDailyOpsTurbGenStatus;
        }

        /// <summary>
        /// Inserts a Daily Ops data object to the database, and return a status of the resuls along with an error message if applicable
        /// </summary>
        /// <param name="dailyOpsdata">1 daily ops data object</param>
        /// <param name="status">a status, usually set to 'not set'</param>
        /// <param name="statusMsg">and empty message</param>
        public void InsertDailyOpsData(DailyOpsData dailyOpsdata, ref Enums.StatusEnum status, ref string statusMsg)
        {
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            dal.InsertDailyOpsData(dailyOpsdata, ref status, ref statusMsg);
            if (status != Enums.StatusEnum.OK)
            {
                EmailHelper.SendEmail(statusMsg);
            }
        }

        public void InsertDailyOpsData_V2(DailyOpsData dailyOpsdata, ref Enums.StatusEnum status, ref string statusMsg)
        {
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            dal.InsertDailyOpsData_V2(dailyOpsdata, ref status, ref statusMsg);
            if (status != Enums.StatusEnum.OK)
            {
                EmailHelper.SendEmail(statusMsg);
            }
        }

        /// <summary>
        /// Update a Daily Ops data object to the database, and return a status of the resuls along with an error message if applicable
        /// </summary>
        /// <param name="dailyOpsdataWithID">1 daily ops data object with ID</param>
        /// <param name="status">a status, usually set to 'not set'</param>
        /// <param name="statusMsg">and empty message</param>
        public void UpdateDailyOpsDataWithID(DailyOpsDataWithID dailyOpsdataWithID, ref Enums.StatusEnum status, ref string statusMsg)
        {
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            dal.UpdateDailyOpsDataWithID(dailyOpsdataWithID, ref status, ref statusMsg);
            if (status != Enums.StatusEnum.OK)
            {
                EmailHelper.SendEmail(statusMsg);
            }
        }

        public List<MSWInventoryExceptions> GetMSWInventoryExceptions(DateTime dateDataRepresents, ref Enums.StatusEnum status)
        {
            string msg = "";
            DALDailyOps dalDailyOps = new DALDailyOps(_dbConnection);
            List<MSWInventoryExceptions> mswInventoryExceptions = dalDailyOps.GetMSWInventoryExceptions(dateDataRepresents, ref status, ref msg);
            return mswInventoryExceptions;
        }

        /// <summary>
        ///  Returns a list a Daily Ops Facilities Reporting Stats based on the parameters.
        ///  If facilityID is passed in, then we ignore the region passed in and, we return all data for the reporting date for that facility.
        ///  If region is passed in, then we ignore the facility passed in and, we return all data for the reporting date for that region.
        ///  If facilityID is String.Empty and region is String.Empty also, we return all data for the reporting date
        ///  Note.  Reporting date must be passed in.
        /// </summary>
        /// <param name="dateDataRepresents">Date we want data from</param>
        /// <param name="faciltyID">Facility we want data from.  String.Empty is allowed</param>
        /// <param name="region">Region we want data from.  String.Empty is allowed</param>
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>a list of DailyOpsFacilitiesReportingStats objects</returns>
        public List<DailyOpsFacilitiesReportingStats> GetDailyOpsFacilitiesReportingStatsByParameters(DateTime dateDataRepresents, string faciltyID, string region, ref Enums.StatusEnum status)
        {
            List<DailyOpsFacilitiesReportingStats> list = new List<DailyOpsFacilitiesReportingStats>();
            //Get Facility Counts Expected by Facility Type (number of facility for each type) and put into the above list
            populateNumberOfFacilitiesForEachType(ref list);

            //return all data for reporting date
            if ((faciltyID == string.Empty) && (region == string.Empty))
            {
                getDailyOpsFacilitiesReportingStatsByDate(dateDataRepresents, ref list, ref status);
                prependListWithAll(ref list);
                return list;
            }

            //return all data for reporting date for
            //facility
            if (faciltyID != string.Empty)
            {
                getDailyOpsFacilitiesReportingStatsByDateAndFacility(dateDataRepresents, ref list, faciltyID, ref status);
                prependListWithAll(ref list);
                return list;
            }

            //return all data for reporting date for
            //region
            if (region != string.Empty)
            {
                getDailyOpsFacilitiesReportingStatsByDateAndRegion(dateDataRepresents, ref list, region, ref status);
                prependListWithAll(ref list);
                return list;
            }

            //we shouldn't reach this code
            List<DailyOpsFacilitiesReportingStats> returnList = new List<DailyOpsFacilitiesReportingStats>();
            prependListWithAll(ref list);
            return returnList;
        }

        /// <summary>
        /// Checks to see if there are reporting dates which have not been entered for the dates earlier in the month of the date and facility passed in.
        /// </summary>
        /// <param name="reportingDate">Date we are thining about reporting on</param>
        /// <param name="faciltyID">Facility we are thining about reporting on</param>
        /// <returns></returns>
        public bool IsFacilityMissingReportingDates(DateTime reportingDate, string faciltyID)
        {
            dateList = null;
            //if it is the 1st of the month then we are ok
            if (reportingDate.Day == 1) { return false; }

            if (GetFirstMissingReportingDate(reportingDate, faciltyID) == DateTime.MinValue)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the fist dateTime which is has not been reported on for the month indicated which should have been reported on.
        /// If all is ok, we return DateTime.Min
        /// </summary>
        /// <param name="reportingDate">The Date we are entering data for</param>
        /// <param name="faciltyID">The facility we are interested in</param>
        /// <returns>DateTime.Min if all is ok, or Returns the 1st missing date if at least one date is missing</returns>
        public DateTime GetFirstMissingReportingDate(DateTime reportingDate, string faciltyID)
        {
            // if reporting date is before 9/1/2012 we ignore this test
            if (reportingDate < new DateTime(2012, 09, 01))
            {
                return DateTime.MinValue;
            }

            // if 1st of month then we ignore this test
            if (reportingDate.Day == 1)
            {
                return DateTime.MinValue;
            }

            // only go to the database to get the list if we havn't gotten it yet
            if (dateList == null)
            {
                dateList = getDailyOpsReportedDatesListByFacilityAndMonth(reportingDate, faciltyID);
            }

            // if still null, then we have a problem in the database, so let the user enter the data, an email should have been sent to support team in other method
            if (dateList == null) { return DateTime.MinValue; }

            DateTime currentTestDate = new DateTime(reportingDate.Year, reportingDate.Month, 1);

            //loop thru the list to see which date if missing
            while (currentTestDate < reportingDate)
            {
                if (dateList.Contains(currentTestDate) == false)
                {
                    return currentTestDate;
                }
                currentTestDate = currentTestDate.AddDays(1);
            }

            return DateTime.MinValue;
        }

        /// <summary>
        ///  Get monthly Daily Ops Data from the database by 1 specific month and 1 specific Facility
        /// </summary>
        /// <param name="dateDataRepresents">>Date we want data from</param>
        /// <param name="faciltyID">Facility ID we want data from (Currently we use the PeopleSoft 5 char code)</param>
        /// <param name="status">An Enum with the status of the call to the database</param>
        /// <returns>List of DailyOpsData object</returns>
        public List<DailyOpsData> GetDailyOpsDataByMonthAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status)
        {
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            status = Enums.StatusEnum.OK;
            string statusMsg = string.Empty;
            List<DailyOpsData> dailyOpsDataList = new List<DailyOpsData>();
            dailyOpsDataList = dal.LoadDailyOpsDataByMonthAndFacility(dateDataRepresents, faciltyID, ref status, ref statusMsg);
            //DailyOpsData dod = dal.LoadDailyOpsDataByMonthAndFacility(dateDataRepresents, faciltyID, ref status, ref statusMsg);
            //if (status != Enums.StatusEnum.OK)
            //{
            //    EmailHelper.SendEmail(statusMsg);
            //}
            //List<DailyOpsData> dodList = new List<DailyOpsData>();
            //// if we didn't have any data, then return an empty object (not null)
            //if (dod != null)
            //{
            //    dodList.Add(dod);
            //}
            //else
            //{
            //    dod = new DailyOpsData(faciltyID, dateDataRepresents);
            //    dodList.Add(dod);
            //}

            populateAdditionalDailyOpsDataProperties(ref dailyOpsDataList);
            //return dod;
            return dailyOpsDataList;
        }

        public List<DailyOpsDataWithID> GetDailyOpsDataWithIDByMonthAndFacility(DateTime dateDataRepresents, string faciltyID, ref Enums.StatusEnum status)
        {
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            status = Enums.StatusEnum.OK;
            string statusMsg = string.Empty;
            List<DailyOpsDataWithID> dailyOpsDataWithIDList = new List<DailyOpsDataWithID>();
            dailyOpsDataWithIDList = dal.LoadDailyOpsDataWithIDByMonthAndFacility(dateDataRepresents, faciltyID, ref status, ref statusMsg);
            //DailyOpsData dod = dal.LoadDailyOpsDataByMonthAndFacility(dateDataRepresents, faciltyID, ref status, ref statusMsg);
            //if (status != Enums.StatusEnum.OK)
            //{
            //    EmailHelper.SendEmail(statusMsg);
            //}
            //List<DailyOpsData> dodList = new List<DailyOpsData>();
            //// if we didn't have any data, then return an empty object (not null)
            //if (dod != null)
            //{
            //    dodList.Add(dod);
            //}
            //else
            //{
            //    dod = new DailyOpsData(faciltyID, dateDataRepresents);
            //    dodList.Add(dod);
            //}

            populateAdditionalDailyOpsDataWithIDProperties(ref dailyOpsDataWithIDList);
            //return dod;
            return dailyOpsDataWithIDList;
        }

        public List<DailyOpsDataWithID> GetActiveDailyOpsDataWithIDByFacility(string faciltyID, ref Enums.StatusEnum status)
        {
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            status = Enums.StatusEnum.OK;
            string statusMsg = string.Empty;
            List<DailyOpsDataWithID> dailyOpsDataWithIDList = new List<DailyOpsDataWithID>();
            dailyOpsDataWithIDList = dal.LoadActiveDailyOpsDataWithIDByFacility(faciltyID, ref status, ref statusMsg);
            //= dal.LoadDailyOpsDataWithIDByMonthAndFacility(dateDataRepresents, faciltyID, ref status, ref statusMsg);
            //DailyOpsData dod = dal.LoadDailyOpsDataByMonthAndFacility(dateDataRepresents, faciltyID, ref status, ref statusMsg);
            //if (status != Enums.StatusEnum.OK)
            //{
            //    EmailHelper.SendEmail(statusMsg);
            //}
            //List<DailyOpsData> dodList = new List<DailyOpsData>();
            //// if we didn't have any data, then return an empty object (not null)
            //if (dod != null)
            //{
            //    dodList.Add(dod);
            //}
            //else
            //{
            //    dod = new DailyOpsData(faciltyID, dateDataRepresents);
            //    dodList.Add(dod);
            //}

            populateAdditionalDailyOpsDataWithIDProperties(ref dailyOpsDataWithIDList);
            //return dod;
            return dailyOpsDataWithIDList;
        }

        #endregion

        #region private methods

        private void prependListWithAll(ref List<DailyOpsFacilitiesReportingStats> list)
        {
            int allExpected = 0;
            int allActual = 0;

            foreach (DailyOpsFacilitiesReportingStats item in list)
            {
                allActual = allActual + item.FacilitiesReportingActual;
                allExpected = allExpected + item.FacilitiesReportingExpected;
            }

            DailyOpsFacilitiesReportingStats x = new DailyOpsFacilitiesReportingStats("All", allExpected, allActual, string.Empty);
            list.Add(x);
        }

        private void populateNumberOfFacilitiesForEachType(ref List<DailyOpsFacilitiesReportingStats> list)
        {
            DailyOpsBusiUnitManager BUManager = new DailyOpsBusiUnitManager(_dbConnection);
            List<string> distinctFacilityTypes = BUManager.GetDistinctFacilityTypesAsListOfStrings();
            List<DailyOpsBusinessUnit> BUList = new List<DailyOpsBusinessUnit>();
            BUList = BUManager.GetBusinessUnitsList();
            BUManager = null;

            for (int i = 0; i < distinctFacilityTypes.Count; i++)
            {
                DailyOpsFacilitiesReportingStats dailyreporingStats = new DailyOpsFacilitiesReportingStats(distinctFacilityTypes[i], 0, 0, string.Empty);
                foreach (DailyOpsBusinessUnit BU in BUList)
                {
                    if (BU.FacilityType == distinctFacilityTypes[i])
                    {
                        dailyreporingStats.FacilitiesReportingExpected++;
                    }
                }
                list.Add(dailyreporingStats);
            }
            return;
        }

        /// <summary>
        /// populates properties FaciltyDescription and FaciltyType for the list passed in
        /// </summary>
        /// <param name="dod">a daily ops data object which will have these two properties populated </param>
        private void populateAdditionalDailyOpsDataProperties(ref List<DailyOpsData> dodList)
        {
            DailyOpsBusiUnitManager BUManager = new DailyOpsBusiUnitManager(_dbConnection);

            List<DailyOpsBusinessUnit> listDailyOpsBusinessUnit = BUManager.GetBusinessUnitsList();
            foreach (DailyOpsData dod in dodList)
            {
                int x = listDailyOpsBusinessUnit.FindIndex(element => element.PS_Unit == dod.FacilityID);
                dod.FaciltyDescription = listDailyOpsBusinessUnit[x].Description;
                dod.FaciltyType = listDailyOpsBusinessUnit[x].FacilityType;
            }
            BUManager = null;
            listDailyOpsBusinessUnit = null;
        }
        private void populateAdditionalDailyOpsDataWithIDProperties(ref List<DailyOpsDataWithID> dodList)
        {
            DailyOpsBusiUnitManager BUManager = new DailyOpsBusiUnitManager(_dbConnection);

            List<DailyOpsBusinessUnit> listDailyOpsBusinessUnit = BUManager.GetBusinessUnitsList();
            foreach (DailyOpsDataWithID dod in dodList)
            {
                int x = listDailyOpsBusinessUnit.FindIndex(element => element.PS_Unit == dod.FacilityID);
                dod.FaciltyDescription = listDailyOpsBusinessUnit[x].Description;
                dod.FaciltyType = listDailyOpsBusinessUnit[x].FacilityType;
            }
            BUManager = null;
            listDailyOpsBusinessUnit = null;
        }

        private void getDailyOpsFacilitiesReportingStatsByDate(DateTime dateDataRepresents, ref List<DailyOpsFacilitiesReportingStats> list, ref Enums.StatusEnum status)
        {
            List<DailyOpsData> listDailyOpsData = GetDailyOpsDataByDate(dateDataRepresents, ref status);

            for (int i = 0; i < list.Count; i++)
            {
                foreach (DailyOpsData x in listDailyOpsData)
                {
                    if ((list[i].FacilityType == x.FaciltyType) && (x.UserRowCreated != null))
                    {
                        list[i].FacilitiesReportingActual++;
                    }
                    else if ((list[i].FacilityType == x.FaciltyType) && (x.UserRowCreated == null))
                    {
                        if (list[i].FacilitiesNotReported != null && list[i].FacilitiesNotReported.Length > 1)
                        {
                            list[i].FacilitiesNotReported += ", ";
                        }
                        list[i].FacilitiesNotReported += x.FaciltyDescription;
                    }
                }
                if (list[i].FacilitiesNotReported.Length == 0)
                {
                    list[i].FacilitiesNotReported = "-";
                }
            }

            return;
        }

        private void getDailyOpsFacilitiesReportingStatsByDateAndFacility(DateTime dateDataRepresents, ref List<DailyOpsFacilitiesReportingStats> list, string facility, ref Enums.StatusEnum status)
        {
            DailyOpsData dailyOpsData = GetDailyOpsDataByDateAndFacility(dateDataRepresents, facility, ref status);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].FacilityType == dailyOpsData.FaciltyType)
                    {
                        if (dailyOpsData.UserRowCreated != null)
                        {
                            list[i].FacilitiesReportingActual++;
                        }
                        //reduce number expected to 1 since this is only one facility
                        list[i].FacilitiesReportingExpected = 1;
                    }
                    else
                    {
                        list[i].FacilitiesReportingExpected = 0;
                    }

                }
            }



            return;
        }

        private void getDailyOpsFacilitiesReportingStatsByDateAndRegion(DateTime dateDataRepresents, ref List<DailyOpsFacilitiesReportingStats> list, string region, ref Enums.StatusEnum status)
        {
            List<DailyOpsData> listDailyOpsData = GetDailyOpsDataByDateAndRegion(dateDataRepresents, region, ref status);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    foreach (DailyOpsData x in listDailyOpsData)
                    {
                        if ((list[i].FacilityType == x.FaciltyType) && (x.UserRowCreated != null))
                        {
                            list[i].FacilitiesReportingActual++;
                        }
                    }
                }
            }

            //reduce expected to report to only this region as max
            DailyOpsBusiUnitManager buManager = new DailyOpsBusiUnitManager(_dbConnection);
            List<DailyOpsBusinessUnit> BUList = buManager.GetBusinessUnitsList();
            foreach (DailyOpsFacilitiesReportingStats x in list)
            {
                x.FacilitiesReportingExpected = buManager.GetNumberOfFacilitiesInRegionAndFacType(BUList, region, x.FacilityType);
            }
            return;
        }

        /// <summary>
        /// Returna list of reporting dataes which have data for the month and facility specified.
        /// </summary>
        /// <param name="dateDataRepresents">Date we want data from (todays's reporting date)</param>
        /// <param name="faciltyID">Facility we want data from.  String.Empty is allowed</param>   
        /// <returns>a list of dates which have data for the month and facility specified</returns>
        private List<DateTime> getDailyOpsReportedDatesListByFacilityAndMonth(DateTime dateDataRepresents, string faciltyID)
        {
            List<DateTime> dateList = new List<DateTime>();
            DALDailyOps dal = new DALDailyOps(_dbConnection);
            Enums.StatusEnum status = Enums.StatusEnum.OK;
            string statusMsg = string.Empty;
            dateList = dal.GetDailyOpsReportedDatesListByFacilityAndMonth(dateDataRepresents, faciltyID, ref status, ref statusMsg);
            if (status != Enums.StatusEnum.OK)
            {
                EmailHelper.SendEmail(statusMsg);
            }

            return dateList;
        }


        #endregion

    }
}
