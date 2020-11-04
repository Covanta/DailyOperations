using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Covanta.BusinessLogic;
using System.Configuration;
using Covanta.Entities;

namespace Covanta.UI.DailyOpsInputForm
{
    public partial class ExceptionsReportData : System.Web.UI.UserControl
    {
        #region private static fields

        private static string covmetadataConnString = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
        private static DailyOpsBusiUnitManager busUnitManager = new DailyOpsBusiUnitManager(covmetadataConnString);
        private static DailyOpsManager dailyDataManager = new DailyOpsManager(covmetadataConnString);

        #endregion

        #region private fields

        private Dictionary<string, List<DailyOpsData>> dataLists;

        #endregion

        #region public properties

        public string FacilityID { get; set; }

        public string Region { get; set; }

        public DateTime ReportDate { get; set; }

        #endregion

        #region constructors

        public ExceptionsReportData()
        {
            FacilityID = "";
            Region = "";
            ReportDate = DateTime.Now.AddDays(-1).Date;            
        }

        #endregion

        #region public methods

        /// <summary>
        /// Ultimately updates every GridView on the page to display the proper data.
        /// This method MUST be called in order for any changes to the region, facility, or date to take effect.
        /// </summary>
        public void Update()
        {
            Covanta.Common.Enums.Enums.StatusEnum status = Covanta.Common.Enums.Enums.StatusEnum.NotSet;
            List<DailyOpsFacilitiesReportingStats> facilityTypes = dailyDataManager.GetDailyOpsFacilitiesReportingStatsByParameters(ReportDate, FacilityID, Region, ref status);
            List<DailyOpsData> facilityData = dailyDataManager.GetDailyOpsDataByParameters(ReportDate, FacilityID, Region, ref status);
            List<MSWInventoryExceptions> mswInventoryExceptions = dailyDataManager.GetMSWInventoryExceptions(ReportDate, ref status);

            SummaryData.DataSource = facilityTypes;
            SummaryData.DataBind();

            facilityTypes.RemoveAt(facilityTypes.Count - 1);
            updateDictionary(facilityTypes, facilityData);
            gridMSWInventoryExceptions.DataSource = mswInventoryExceptions;

            if (facilityData.Any(x => x.UserRowCreated != null))
            {
                facilityData = facilityData.Where(x => x.UserRowCreated != null).ToList();
                var boilerIssues = getBoilerIssues(facilityData);
                BoilerOutageData1.DataSource = GetStatuses(boilerIssues, "boiler");
                BoilerOutageData1.DataBind();

                var turbineIssues = getTurbineIssues(facilityData);
                TurbineGeneratorOutage.DataSource = GetStatuses(turbineIssues, "turbine");
                TurbineGeneratorOutage.DataBind();

                var metalSystemsIssues = getMetalsSystemsIssues(facilityData);
                MetalSystemsOutageData.DataSource = GetStatuses(metalSystemsIssues, "metalSystems");
                MetalSystemsOutageData.DataBind();
                
                var criticalAssetsIssues = getCriticalAssetsIssues(facilityData);
                CriticalAssestsInAlarm.DataSource = GetStatuses(criticalAssetsIssues, "criticalAssets");
                CriticalAssestsInAlarm.DataBind();

                var criticalEquipmentIssues = getCommentsIssues(facilityData);
                CriticalEquipment.DataSource = GetStatuses(criticalEquipmentIssues, "criticalEquipment");
                CriticalEquipment.DataBind();

                var fireProtectionIssues = getFireSystemIssues(facilityData);
                FireProtection.DataSource = GetStatuses(fireProtectionIssues, "fireProtection");
                FireProtection.DataBind();

                var environmentalEventsIssues = getEnvironmentalIssues(facilityData);
                EnvironmentalEvents.DataSource = GetStatuses(environmentalEventsIssues, "environmentalEvents");
                EnvironmentalEvents.DataBind();

                var healthSafetyIssues = getHealthSafetyIssues(facilityData);
                HealthSafety.DataSource = GetStatuses(healthSafetyIssues, "healthSafety");
                HealthSafety.DataBind();
            }

            gridMSWInventoryExceptions.DataSource = mswInventoryExceptions;
            gridMSWInventoryExceptions.DataBind();

            UpdateDowntimeHeaders();
        }

        #endregion

        #region protected methods

        public List<DailyOpsStatusData> GetStatuses(List<DailyOpsData> issues, string statusFor)
        {
            var statuses = new List<DailyOpsBoilerStatus>();
            var outageDataList = new List<DailyOpsStatusData>();
            var sts = Covanta.Common.Enums.Enums.StatusEnum.NotSet;

            foreach (var boilerIssue in issues)
            {
                string facility = boilerIssue.FacilityID;

                if (statusFor == "boiler")
                {
                    statuses = dailyDataManager.GetDailyOpsBoilerStatusByDateAndFacility(ReportDate, facility, ref sts);
                }
                else if (statusFor == "turbine")
                {
                    statuses = dailyDataManager.GetDailyOpsTurbGenByDateAndFacility(ReportDate, facility, ref sts);
                }
                else if(statusFor == "metalSystems")
                {
                    var metalSysStatuses = dailyDataManager.GetDailyOpsMetalsStatusByDateAndFacility(ReportDate, facility, ref sts);
                    foreach (var status in metalSysStatuses)
                    {
                        var dailyOpsBoilerData = new DailyOpsStatusData()
                        {
                            FacilityType = boilerIssue.FaciltyType,
                            FacilityDescription = boilerIssue.FaciltyDescription,
                            BoilerNumber = 0,
                            Status = string.Empty,
                            Downtime = status.Downtime,
                            UnscheduledOutageExplanation = status.DowntimeExplanation,
                            CumulativeDowntime = status.CumulativeDowntime,
                            MonthToDate = status.MonthToDate,
                            ExpectedRepairDate = status.ExpectedRepairDate,
                            SystemType = status.SystemType,
                            WasReprocessed = status.WasReprocessed
                        };
                        outageDataList.Add(dailyOpsBoilerData);
                    }
                }
                else if(statusFor == "criticalAssets")
                {
                    var dailyOpsBoilerData = new DailyOpsStatusData()
                    {
                        FacilityType = boilerIssue.FaciltyType,
                        FacilityDescription = boilerIssue.FaciltyDescription,
                        CriticalAssetsInAlarm = boilerIssue.CriticalAssetsInAlarm,
                        CriticalAssetsExpectedBackOnlineDate = boilerIssue.CriticalAssetsExpectedBackOnlineDate
                    };
                    outageDataList.Add(dailyOpsBoilerData);
                }
                else if(statusFor == "criticalEquipment")
                {
                    var dailyOpsBoilerData = new DailyOpsStatusData()
                    {
                        FacilityType = boilerIssue.FaciltyType,
                        FacilityDescription = boilerIssue.FaciltyDescription,
                        Comments = boilerIssue.Comments,
                        CriticalEquipmentOOSExpectedBackOnlineDate = boilerIssue.CriticalEquipmentOOSExpectedBackOnlineDate
                    };
                    outageDataList.Add(dailyOpsBoilerData);
                }
                else if(statusFor == "fireProtection")
                {
                    var dailyOpsBoilerData = new DailyOpsStatusData()
                    {
                        FacilityType = boilerIssue.FaciltyType,
                        FacilityDescription = boilerIssue.FaciltyDescription,
                        FireSystemOutOfService = boilerIssue.FireSystemOutOfService,
                        FireSystemOutOfServiceExpectedBackOnlineDate = boilerIssue.FireSystemOutOfServiceExpectedBackOnlineDate
                    };
                    outageDataList.Add(dailyOpsBoilerData);
                }
                else if(statusFor == "environmentalEvents")
                {
                    List<DailyOpsReportableEvent> environmentalEventsStatuses = dailyDataManager.GetDailyOpsEventsEnvironmentalByDateAndFacility(ReportDate, facility, ref sts);
                    foreach (var status in environmentalEventsStatuses)
                    {
                        var dailyOpsBoilerData = new DailyOpsStatusData()
                        {
                            FacilityType = boilerIssue.FaciltyType,
                            FacilityDescription = boilerIssue.FaciltyDescription,
                            EventType = status.EventType,
                            EventDescription = status.Description
                        };
                        outageDataList.Add(dailyOpsBoilerData);
                    }
                }
                else if(statusFor == "healthSafety")
                {
                    List<DailyOpsReportableEvent> healthSafetyStatuses = dailyDataManager.GetDailyOpsEventsHealthAndSafetyByDateAndFacility(ReportDate, facility, ref sts);
                    foreach (var status in healthSafetyStatuses)
                    {
                        var dailyOpsBoilerData = new DailyOpsStatusData()
                        {
                            FacilityType = boilerIssue.FaciltyType,
                            FacilityDescription = boilerIssue.FaciltyDescription,
                            EventType = status.EventType,
                            EventDescription = status.Description
                        };
                        outageDataList.Add(dailyOpsBoilerData);
                    }
                }
                if (statusFor != "metalSystems")
                {
                    foreach (var boilerStatus in statuses)
                    {
                        var dailyOpsBoilerData = new DailyOpsStatusData()
                        {
                            FacilityType = boilerIssue.FaciltyType,
                            FacilityDescription = boilerIssue.FaciltyDescription,
                            BoilerNumber = boilerStatus.BoilerNumber,
                            Status = boilerStatus.Status,
                            Downtime = boilerStatus.Downtime,
                            UnscheduledOutageExplanation = boilerStatus.UnscheduledOutageExplanation,
                            CumulativeDowntime = boilerStatus.CumulativeDowntime,
                            MonthToDate = boilerStatus.MonthToDate,
                            ExpectedRepairDate = boilerStatus.ExpectedRepairDate,
                        };
                        outageDataList.Add(dailyOpsBoilerData);
                    }
                }
            }
            return outageDataList.OrderByDescending(x => x.FacilityType).ToList();
        }

        /// <summary>
        /// Calculates and sets date for downtime headers.
        /// </summary>
        protected void UpdateDowntimeHeaders()
        {
            DateTime sundayDate = new DateTime();
            DateTime initialDate = ReportDate;
            string sundayMonth = sundayDate.Month.ToString();
            string sundayDay = sundayDate.Day.ToString();
            bool foundDate = false;
            while (!foundDate)
            {
                if (initialDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    sundayDate = initialDate;
                    if (sundayDate.Month <= 9)
                    {
                        sundayMonth = string.Format("0{0}", sundayDate.Month);
                    }
                    else
                    {
                        sundayMonth = sundayDate.Month.ToString();
                    }

                    if (sundayDate.Day <= 9)
                    {
                        sundayDay = string.Format("0{0}", sundayDate.Day);
                    }
                    else
                    {
                        sundayDay = sundayDate.Day.ToString();
                    }
                    foundDate = true;
                }
                else
                {
                    initialDate = initialDate.AddDays(1);
                }
            }

            //BoilerOutageFacilityTypeData.HeaderRow.Cells[5].Text = string.Format("Cumulative Downtime for Week Ending {0}/{1}", sundayMonth, sundayDay);
            //BoilerOutageFacilityTypeData.HeaderRow.Cells[7].Text = string.Format("Cumulative Downtime for Month of {0} {1}", ReportDate.ToString("MMMM"), ReportDate.Year);

            //TurbineOutageFacilityTypeData.HeaderRow.Cells[5].Text = string.Format("Cumulative Downtime for Week Ending {0}/{1}", sundayMonth, sundayDay);
            //TurbineOutageFacilityTypeData.HeaderRow.Cells[7].Text = string.Format("Cumulative Downtime for Month of {0} {1}", ReportDate.ToString("MMMM"), ReportDate.Year);

            //MetalsSystemsFacilityTypeData.HeaderRow.Cells[4].Text = string.Format("Cumulative Downtime for Week Ending {0}/{1}", sundayMonth, sundayDay);
            //MetalsSystemsFacilityTypeData.HeaderRow.Cells[7].Text = string.Format("Cumulative Downtime for Month of {0} {1}", ReportDate.ToString("MMMM"), ReportDate.Year);
        }

        #endregion

        #region protected methods
        protected void gridMSWInventoryExceptions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Facility Type";
                e.Row.Cells[3].Text = "Actual Inventory";
                e.Row.Cells[4].Text = "Inventory Min Limit";
                e.Row.Cells[5].Text = "Inventory Max Limit";
            }
            else if(e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > 0)
            {
                GridViewRow prevrow = gridMSWInventoryExceptions.Rows[e.Row.RowIndex - 1];
                if(e.Row.Cells[0].Text == prevrow.Cells[0].Text)
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
            }
        }
        
        protected void BoilerOutageData1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[7].Text = string.Format("Cumulative Downtime for Month of {0} {1}", ReportDate.ToString("MMMM"), ReportDate.Year);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > 0)
            {
                GridViewRow prevrow = BoilerOutageData1.Rows[e.Row.RowIndex - 1];
                if (e.Row.Cells[0].Text == prevrow.Cells[0].Text)
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
                if (e.Row.Cells[1].Text == prevrow.Cells[1].Text)
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                }
            }
        }
        
        protected void TurbineGeneratorOutage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[7].Text = string.Format("Cumulative Downtime for Month of {0} {1}", ReportDate.ToString("MMMM"), ReportDate.Year);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > 0)
            {
                GridViewRow prevrow = TurbineGeneratorOutage.Rows[e.Row.RowIndex - 1];
                if (e.Row.Cells[0].Text == prevrow.Cells[0].Text)
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
                if (e.Row.Cells[1].Text == prevrow.Cells[1].Text)
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                }
            }
        }

        protected void MetalSystemsOutageData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[7].Text = string.Format("Cumulative Downtime for Month of {0} {1}", ReportDate.ToString("MMMM"), ReportDate.Year);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > 0)
            {
                GridViewRow prevrow = MetalSystemsOutageData.Rows[e.Row.RowIndex - 1];
                if (e.Row.Cells[0].Text == prevrow.Cells[0].Text)
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
                if (e.Row.Cells[1].Text == prevrow.Cells[1].Text)
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                }
            }
        }
        
        protected void CriticalAssestsInAlarm_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > 0)
            {
                GridViewRow prevrow = CriticalAssestsInAlarm.Rows[e.Row.RowIndex - 1];
                if (e.Row.Cells[0].Text == prevrow.Cells[0].Text)
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
                if (e.Row.Cells[1].Text == prevrow.Cells[1].Text)
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                }
            }
        }

        protected void CriticalEquipment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > 0)
            {
                GridViewRow prevrow = CriticalEquipment.Rows[e.Row.RowIndex - 1];
                if (e.Row.Cells[0].Text == prevrow.Cells[0].Text)
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
                if (e.Row.Cells[1].Text == prevrow.Cells[1].Text)
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                }
            }
        }

        protected void FireProtection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > 0)
            {
                GridViewRow prevrow = FireProtection.Rows[e.Row.RowIndex - 1];
                if (e.Row.Cells[0].Text == prevrow.Cells[0].Text)
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
                if (e.Row.Cells[1].Text == prevrow.Cells[1].Text)
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                }
            }
        }
        
        protected void EnvironmentalEvents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > 0)
            {
                GridViewRow prevrow = EnvironmentalEvents.Rows[e.Row.RowIndex - 1];
                if (e.Row.Cells[0].Text == prevrow.Cells[0].Text)
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
                if (e.Row.Cells[1].Text == prevrow.Cells[1].Text)
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                }
            }
        }

        protected void HealthSafety_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex > 0)
            {
                GridViewRow prevrow = HealthSafety.Rows[e.Row.RowIndex - 1];
                if (e.Row.Cells[0].Text == prevrow.Cells[0].Text)
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
                if (e.Row.Cells[1].Text == prevrow.Cells[1].Text)
                {
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                }
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Creates a list of DailyOpsData for each facility type.
        /// This is used to organize the facilities by type in the GridViews.
        /// </summary>
        private void updateDictionary(List<DailyOpsFacilitiesReportingStats> facilityTypes, List<DailyOpsData> facilityData)
        {
            // eventually, the parameter will be List<FacilityType>
            dataLists = new Dictionary<string, List<DailyOpsData>>();
            foreach (DailyOpsFacilitiesReportingStats entry in facilityTypes)
                dataLists.Add(entry.FacilityType, new List<DailyOpsData>());

            foreach (DailyOpsData entry in facilityData)
                if (entry.UserRowCreated != null)
                {
                    string id = entry.FacilityID;
                    string key = busUnitManager.GetBusinessUnitsTypeByPSCode(id);
                    dataLists[key].Add(entry);
                }
        }

        #endregion

        #region private static methods

        /// <summary>
        /// Returns true if the specified boiler status is considered
        /// "non-operational," false otherwise.
        /// </summary>
        /// <param name="boilerStatus">the status to evaluate</param>
        /// <returns>whether the status means "not operational"</returns>
        private static bool isNotOperational(string boilerStatus)
        {
            //return !(boilerStatus.Equals("") || boilerStatus.Equals("Operational"));
            //Changed by Eric Chen Sept 10 2017 to remove decomissioned
            if (boilerStatus == null)
            {
                return false;
            }
            else
            {
                return !(boilerStatus.Equals("") || boilerStatus.Equals("Operational") || boilerStatus.Equals("Decommissioned"));
            }
        }

        /// <summary>
        /// Returns a list of DailyOpsData that (a) are on the input list, and
        /// (b) have at least one non-operational boiler.
        /// In essence, returns a list of DailyOpsData that will be bound to
        /// the Boiler Outage GridView.
        /// </summary>
        /// <param name="dataList">the list of DailyOpsData to look at</param>
        /// <returns>only those DailyOpsData objects with non-operational boilers</returns>
        private static List<DailyOpsData> getBoilerIssues(List<DailyOpsData> dataList)
        {
            List<DailyOpsData> boilerList = new List<DailyOpsData>();

            foreach (DailyOpsData entry in dataList)
            {
                if (isNotOperational(entry.OutageTypeBoiler1))
                    boilerList.Add(entry);
                else if (isNotOperational(entry.OutageTypeBoiler2))
                    boilerList.Add(entry);
                else if (isNotOperational(entry.OutageTypeBoiler3))
                    boilerList.Add(entry);
                else if (isNotOperational(entry.OutageTypeBoiler4))
                    boilerList.Add(entry);
                else if (isNotOperational(entry.OutageTypeBoiler5))
                    boilerList.Add(entry);
                else if (isNotOperational(entry.OutageTypeBoiler6))
                    boilerList.Add(entry);
            }

            return boilerList;
        }

        /// <summary>
        /// Returns a list of DailyOpsData that (a) are on the input list, and
        /// (b) have at least one non-operational turbine/generator.
        /// In essence, returns a list of DailyOpsData that will be bound to
        /// the Turbine Outage GridView.
        /// </summary>
        /// <param name="dataList">the list of DailyOpsData to look at</param>
        /// <returns>only those DailyOpsData objects with non-operational turbines/generators</returns>
        private static List<DailyOpsData> getTurbineIssues(List<DailyOpsData> dataList)
        {
            List<DailyOpsData> turbineList = new List<DailyOpsData>();

            foreach (DailyOpsData entry in dataList)
            {
                if (isNotOperational(entry.OutageTypeTurbGen1))
                    turbineList.Add(entry);
                else if (isNotOperational(entry.OutageTypeTurbGen2))
                    turbineList.Add(entry);
            }

            return turbineList;
        }

        private static List<DailyOpsData> getMetalsSystemsIssues(List<DailyOpsData> dataList)
        {
            List<DailyOpsData> metalsSystemList = new List<DailyOpsData>();
            foreach (DailyOpsData entry in dataList)
            {
                if (entry.WasFerrousSystem100PercentAvailable.Equals("N"))
                    metalsSystemList.Add(entry);
                else if (entry.WasNonFerrousSystem100PercentAvailable.Equals("N"))
                    metalsSystemList.Add(entry);
                else if (entry.WasNonFerrousSmallsSystem100PercentAvailable.Equals("N"))
                    metalsSystemList.Add(entry);
                else if (entry.WasFrontEndFerrousSystem100PercentAvailable.Equals("N"))
                    metalsSystemList.Add(entry);
                else if (entry.WasEnhancedFerrousSystem100PercentAvailable.Equals("N"))
                    metalsSystemList.Add(entry);
                else if (entry.WasEnhancedNonFerrousSystem100PercentAvailable.Equals("N"))
                    metalsSystemList.Add(entry);
            }
            return metalsSystemList;
        }

        private static List<DailyOpsData> getCriticalAssetsIssues(List<DailyOpsData> dataList)
        {
            List<DailyOpsData> critAssetsList = new List<DailyOpsData>();

            foreach (DailyOpsData entry in dataList)
                if (!string.IsNullOrEmpty(entry.CriticalAssetsInAlarm.Trim()))
                    critAssetsList.Add(entry);

            return critAssetsList;
        }

        /// <summary>
        /// Returns a list of DailyOpsData that (a) are on the input list, and
        /// (b) have a fire system isse.
        /// In essence, returns a list of DailyOpsData that will be bound to
        /// the Fire Protection GridView.
        /// </summary>
        /// <param name="dataList">the list of DailyOpsData to look at</param>
        /// <returns>only those DailyOpsData objects with fire system issues</returns>
        private static List<DailyOpsData> getFireSystemIssues(List<DailyOpsData> dataList)
        {
            List<DailyOpsData> fireList = new List<DailyOpsData>();

            foreach (DailyOpsData entry in dataList)
                if (!string.IsNullOrEmpty(entry.FireSystemOutOfService.Trim()))
                    fireList.Add(entry);

            return fireList;
        }

        private static List<DailyOpsData> getEnvironmentalIssues(List<DailyOpsData> dataList)
        {
            List<DailyOpsData> environList = new List<DailyOpsData>();

            foreach (DailyOpsData entry in dataList)
            {
                if (entry.IsEnvironmentalEvents)
                    environList.Add(entry);
                else if (entry.IsCEMSEvents)
                    environList.Add(entry);
            }

            return environList;
        }

        private static List<DailyOpsData> getHealthSafetyIssues(List<DailyOpsData> dataList)
        {
            List<DailyOpsData> hsList = new List<DailyOpsData>();

            foreach (DailyOpsData entry in dataList)
            {
                if (!string.IsNullOrEmpty(entry.HealthSafetyFirstAid.Trim()))
                    hsList.Add(entry);
                else if (!string.IsNullOrEmpty(entry.HealthSafetyOSHAReportable.Trim()))
                    hsList.Add(entry);
                else if (!string.IsNullOrEmpty(entry.HealthSafetyNearMiss.Trim()))
                    hsList.Add(entry);
                else if (!string.IsNullOrEmpty(entry.HealthSafetyContractor.Trim()))
                    hsList.Add(entry);
            }

            return hsList;
        }

        /// <summary>
        /// Returns a list of DailyOpsData that (a) are on the input list, and
        /// (b) have a comment.
        /// In essence, returns a list of DailyOpsData that will be bound to
        /// the Comments GridView.
        /// </summary>
        /// <param name="dataList">the list of DailyOpsData to look at</param>
        /// <returns>only those DailyOpsData objects with comments</returns>
        private static List<DailyOpsData> getCommentsIssues(List<DailyOpsData> dataList)
        {
            List<DailyOpsData> commentsList = new List<DailyOpsData>();

            foreach (DailyOpsData entry in dataList)
                if (!string.IsNullOrEmpty(entry.Comments.Trim()))
                    commentsList.Add(entry);

            return commentsList;
        }

        #endregion
    }
}