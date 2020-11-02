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

            //BoilerOutageFacilityTypeData.DataSource = facilityTypes;
            TurbineOutageFacilityTypeData.DataSource = facilityTypes;
            MetalsSystemsFacilityTypeData.DataSource = facilityTypes;
            CriticalAssetsFacilityTypeData.DataSource = facilityTypes;
            FireProtectionFacilityTypeData.DataSource = facilityTypes;
            EnvironmentalFacilityTypeData.DataSource = facilityTypes;
            HealthSafetyFacilityTypeData.DataSource = facilityTypes;
            CommentsFacilityTypeData.DataSource = facilityTypes;
            gridMSWInventoryExceptions.DataSource = mswInventoryExceptions;

            if (facilityData.Any(x => x.UserRowCreated != null))
            {
                var boilerIssues = getBoilerIssues(facilityData);

                var statuses = new List<DailyOpsBoilerStatus>();
                var boilerOutageDataList = new List<DailyOpsBoilerData>();
                foreach (var boilerIssue in boilerIssues)
                {
                    string facility = boilerIssue.FacilityID;

                    var sts = Covanta.Common.Enums.Enums.StatusEnum.NotSet;
                    statuses = dailyDataManager.GetDailyOpsBoilerStatusByDateAndFacility(ReportDate, facility, ref sts);

                    foreach(var boilerStatus in statuses)
                    {
                        var dailyOpsBoilerData = new DailyOpsBoilerData()
                        {
                            FacilityType = boilerIssue.FaciltyType,
                            FacilityDescription = boilerIssue.FaciltyDescription,
                            BoilerNumber = boilerStatus.BoilerNumber,
                            Status = boilerStatus.Status,
                            Downtime = boilerStatus.Downtime,
                            UnscheduledOutageExplanation = boilerStatus.UnscheduledOutageExplanation,
                            CumulativeDowntime = boilerStatus.CumulativeDowntime,
                            MonthToDate = boilerStatus.MonthToDate,
                            ExpectedRepairDate = boilerStatus.ExpectedRepairDate
                        };
                        boilerOutageDataList.Add(dailyOpsBoilerData);
                    }
                };
                BoilerOutageData1.DataSource = boilerOutageDataList.OrderByDescending(x => x.FacilityType);
                BoilerOutageData1.DataBind();
            }

            //BoilerOutageFacilityTypeData.DataBind();
            TurbineOutageFacilityTypeData.DataBind();
            MetalsSystemsFacilityTypeData.DataBind();
            CriticalAssetsFacilityTypeData.DataBind();
            FireProtectionFacilityTypeData.DataBind();
            EnvironmentalFacilityTypeData.DataBind();
            HealthSafetyFacilityTypeData.DataBind();
            CommentsFacilityTypeData.DataBind();
            gridMSWInventoryExceptions.DataBind();

            UpdateDowntimeHeaders();
        }

        #endregion

        #region protected methods

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
            TurbineOutageFacilityTypeData.HeaderRow.Cells[7].Text = string.Format("Cumulative Downtime for Month of {0} {1}", ReportDate.ToString("MMMM"), ReportDate.Year);

            //MetalsSystemsFacilityTypeData.HeaderRow.Cells[4].Text = string.Format("Cumulative Downtime for Week Ending {0}/{1}", sundayMonth, sundayDay);
            MetalsSystemsFacilityTypeData.HeaderRow.Cells[7].Text = string.Format("Cumulative Downtime for Month of {0} {1}", ReportDate.ToString("MMMM"), ReportDate.Year);
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
                e.Row.Cells[0].Text = "Facility Type";
                e.Row.Cells[1].Text = "Facility";
                e.Row.Cells[2].Text = "Boiler";
                e.Row.Cells[3].Text = "Status";
                e.Row.Cells[4].Text = "Current Event Downtime";
                e.Row.Cells[5].Text = "Current Event Explanation";
                e.Row.Cells[6].Text = "Current Event Cumulative Downtime";
                e.Row.Cells[7].Text = string.Format("Cumulative Downtime for Month of {0} {1}", ReportDate.ToString("MMMM"), ReportDate.Year);
                e.Row.Cells[8].Text = "Estimated Return to Service Date";

                e.Row.Cells[0].Width = Unit.Pixel(12);
                e.Row.Cells[1].Width = Unit.Pixel(10);
                e.Row.Cells[2].Width = e.Row.Cells[3].Width = e.Row.Cells[4].Width = e.Row.Cells[5].Width = 
                e.Row.Cells[6].Width = e.Row.Cells[7].Width = e.Row.Cells[8].Width = Unit.Pixel(11);

                e.Row.Cells[0].CssClass = e.Row.Cells[1].CssClass = e.Row.Cells[2].CssClass = e.Row.Cells[3].CssClass = e.Row.Cells[4].CssClass = 
                e.Row.Cells[5].CssClass = e.Row.Cells[6].CssClass = e.Row.Cells[7].CssClass = e.Row.Cells[8].CssClass = "SubFacilityRow";
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

                e.Row.Cells[0].Width = Unit.Pixel(12);
                e.Row.Cells[1].Width = Unit.Pixel(10);
                e.Row.Cells[2].Width = e.Row.Cells[3].Width = e.Row.Cells[4].Width = e.Row.Cells[5].Width =
                e.Row.Cells[6].Width = e.Row.Cells[7].Width = e.Row.Cells[8].Width = Unit.Pixel(11);

                e.Row.Cells[0].CssClass = e.Row.Cells[1].CssClass = e.Row.Cells[2].CssClass = e.Row.Cells[3].CssClass = e.Row.Cells[4].CssClass =
                e.Row.Cells[5].CssClass = e.Row.Cells[6].CssClass = e.Row.Cells[7].CssClass = e.Row.Cells[8].CssClass = "SubFacilityRow";
            }
        }

        /// <summary>
        /// Binds actual facility data to each facility type row for the Boiler Outage table.
        /// </summary>
        /// <param name="sender">the Boiler Outage GridView</param>
        /// <param name="e">contains info about the row being created</param>
        protected void BoilerOutageFacilityTypeData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].ColumnSpan = 8;
                e.Row.Cells.RemoveAt(7);
                e.Row.Cells.RemoveAt(6);
                e.Row.Cells.RemoveAt(5);
                e.Row.Cells.RemoveAt(4);
                e.Row.Cells.RemoveAt(3);
                e.Row.Cells.RemoveAt(2);

                List<DailyOpsFacilitiesReportingStats> types = (List<DailyOpsFacilitiesReportingStats>)((GridView)sender).DataSource;
                if (types == null)
                    return;
                string facilityType = types.ElementAt(e.Row.RowIndex).FacilityType;

                List<DailyOpsData> source = getBoilerIssues(dataLists[facilityType]);

                if (source.Count == 0)
                    e.Row.Visible = false;
                else
                {
                    GridView gv = (GridView)e.Row.FindControl("BoilerOutageData");
                    gv.DataSource = source;
                    gv.DataBind();
                }
            }
        }

        /// <summary>
        /// Binds actual boiler status data to each facility row for the Boiler Outage table.
        /// </summary>
        /// <param name="sender">the Boiler Outage sub-GridView (nested within a row of
        /// the main Boiler Outage GridView)</param>
        /// <param name="e">contains info about the row being created</param>
        protected void BoilerOutageData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView BoilerOutageData = (GridView)sender;
                List<DailyOpsData> data = (List<DailyOpsData>)BoilerOutageData.DataSource;
                if (data == null)
                    return;
                string facility = data.ElementAt(e.Row.RowIndex).FacilityID;

                Covanta.Common.Enums.Enums.StatusEnum status = Covanta.Common.Enums.Enums.StatusEnum.NotSet;
                List<DailyOpsBoilerStatus> statuses = dailyDataManager.GetDailyOpsBoilerStatusByDateAndFacility(ReportDate, facility, ref status);

                GridView gv = (GridView)e.Row.FindControl("BoilerStatusData");
                gv.DataSource = statuses;
                gv.DataBind();
            }
        }


        protected void BoilerStatusData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.Cells[3].Text.Trim() == "1/1/1900")
                    e.Row.Cells[3].Text = string.Empty;


            }
        }

        protected void TurbineOutageFacilityTypeData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].ColumnSpan = 8;
                e.Row.Cells.RemoveAt(7);
                e.Row.Cells.RemoveAt(6);
                e.Row.Cells.RemoveAt(5);
                e.Row.Cells.RemoveAt(4);
                e.Row.Cells.RemoveAt(3);
                e.Row.Cells.RemoveAt(2);

                List<DailyOpsFacilitiesReportingStats> types = (List<DailyOpsFacilitiesReportingStats>)((GridView)sender).DataSource;
                if (types == null)
                    return;
                string facilityType = types.ElementAt(e.Row.RowIndex).FacilityType;

                List<DailyOpsData> source = getTurbineIssues(dataLists[facilityType]);

                if (source.Count == 0)
                    e.Row.Visible = false;
                else
                {
                    GridView gv = (GridView)e.Row.FindControl("TurbineOutageData");
                    gv.DataSource = source;
                    gv.DataBind();
                }
            }
        }



        protected void TurbineOutageData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView TurbineOutageData = (GridView)sender;
                List<DailyOpsData> data = (List<DailyOpsData>)TurbineOutageData.DataSource;
                if (data == null)
                    return;
                string facility = data.ElementAt(e.Row.RowIndex).FacilityID;

                Covanta.Common.Enums.Enums.StatusEnum status = Covanta.Common.Enums.Enums.StatusEnum.NotSet;
                List<DailyOpsBoilerStatus> statuses = dailyDataManager.GetDailyOpsTurbGenByDateAndFacility(ReportDate, facility, ref status);

                GridView gv = (GridView)e.Row.FindControl("TurbineStatusData");
                gv.DataSource = statuses;
                gv.DataBind();
            }
        }
        protected void TurbineStatusData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.Cells[3].Text.Trim() == "1/1/1900")
                    e.Row.Cells[3].Text = string.Empty;


            }
        }
        protected void MetalsSystemsFacilityTypeData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].ColumnSpan = 8;
                e.Row.Cells.RemoveAt(8);
                e.Row.Cells.RemoveAt(7);
                e.Row.Cells.RemoveAt(6);
                e.Row.Cells.RemoveAt(5);
                e.Row.Cells.RemoveAt(4);
                e.Row.Cells.RemoveAt(3);
                e.Row.Cells.RemoveAt(2);

                List<DailyOpsFacilitiesReportingStats> types = (List<DailyOpsFacilitiesReportingStats>)((GridView)sender).DataSource;
                if (types == null)
                    return;
                string facilityType = types.ElementAt(e.Row.RowIndex).FacilityType;

                List<DailyOpsData> source = getMetalsSystemsIssues(dataLists[facilityType]);

                if (source.Count == 0)
                    e.Row.Visible = false;
                else
                {
                    GridView gv = (GridView)e.Row.FindControl("MetalsSystemsData");
                    gv.DataSource = source;
                    gv.DataBind();
                }
            }
        }

        protected void MetalsSystemsData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView MetalsSystemsData = (GridView)sender;
                List<DailyOpsData> data = (List<DailyOpsData>)MetalsSystemsData.DataSource;
                if (data == null)
                    return;
                string facility = data.ElementAt(e.Row.RowIndex).FacilityID;

                Covanta.Common.Enums.Enums.StatusEnum status = Covanta.Common.Enums.Enums.StatusEnum.NotSet;
                List<DailyOpsMetalSystemsStatus> statuses = dailyDataManager.GetDailyOpsMetalsStatusByDateAndFacility(ReportDate, facility, ref status);

                GridView gv = (GridView)e.Row.FindControl("MetalsSystemsStatusData");
                gv.DataSource = statuses;
                gv.DataBind();
            }
        }

        protected void CriticalAssetsFacilityTypeData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].ColumnSpan = 2;
                e.Row.Cells.RemoveAt(2);

                List<DailyOpsFacilitiesReportingStats> types = (List<DailyOpsFacilitiesReportingStats>)((GridView)sender).DataSource;
                if (types == null)
                    return;
                string facilityType = types.ElementAt(e.Row.RowIndex).FacilityType;

                List<DailyOpsData> source = getCriticalAssetsIssues(dataLists[facilityType]);

                if (source.Count == 0)
                    e.Row.Visible = false;
                else
                {
                    GridView gv = (GridView)e.Row.FindControl("CriticalAssetsData");
                    gv.DataSource = source;
                    gv.DataBind();
                }
            }
        }

        /// <summary>
        /// Binds actual facility data to each facility type row for the Fire Protection table.
        /// </summary>
        /// <param name="sender">the Fire Protection GridView</param>
        /// <param name="e">contains info about the row being created</param>
        protected void FireProtectionFacilityTypeData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].ColumnSpan = 3;
                e.Row.Cells.RemoveAt(3);
                e.Row.Cells.RemoveAt(2);

                List<DailyOpsFacilitiesReportingStats> types = (List<DailyOpsFacilitiesReportingStats>)((GridView)sender).DataSource;
                if (types == null)
                    return;
                string facilityType = types.ElementAt(e.Row.RowIndex).FacilityType;

                List<DailyOpsData> source = getFireSystemIssues(dataLists[facilityType]);

                if (source.Count == 0)
                    e.Row.Visible = false;
                else
                {
                    GridView gv = (GridView)e.Row.FindControl("FireProtectionData");
                    gv.DataSource = source;
                    gv.DataBind();
                }
            }
        }

        protected void EnvironmentalFacilityTypeData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].ColumnSpan = 3;
                e.Row.Cells.RemoveAt(3);
                e.Row.Cells.RemoveAt(2);

                List<DailyOpsFacilitiesReportingStats> types = (List<DailyOpsFacilitiesReportingStats>)((GridView)sender).DataSource;
                if (types == null)
                    return;
                string facilityType = types.ElementAt(e.Row.RowIndex).FacilityType;

                List<DailyOpsData> source = getEnvironmentalIssues(dataLists[facilityType]);

                if (source.Count == 0)
                    e.Row.Visible = false;
                else
                {
                    GridView gv = (GridView)e.Row.FindControl("EnvironmentalData");
                    gv.DataSource = source;
                    gv.DataBind();
                }
            }
        }

        protected void EnvironmentalData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView HealthSafetyData = (GridView)sender;
                List<DailyOpsData> data = (List<DailyOpsData>)HealthSafetyData.DataSource;
                if (data == null)
                    return;
                string facility = data.ElementAt(e.Row.RowIndex).FacilityID;

                Covanta.Common.Enums.Enums.StatusEnum status = Covanta.Common.Enums.Enums.StatusEnum.NotSet;
                List<DailyOpsReportableEvent> statuses = dailyDataManager.GetDailyOpsEventsEnvironmentalByDateAndFacility(ReportDate, facility, ref status);

                GridView gv = (GridView)e.Row.FindControl("EnvironmentalEventData");
                gv.DataSource = statuses;
                gv.DataBind();
            }
        }

        protected void HealthSafetyFacilityTypeData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].ColumnSpan = 3;
                e.Row.Cells.RemoveAt(3);
                e.Row.Cells.RemoveAt(2);

                List<DailyOpsFacilitiesReportingStats> types = (List<DailyOpsFacilitiesReportingStats>)((GridView)sender).DataSource;
                if (types == null)
                    return;
                string facilityType = types.ElementAt(e.Row.RowIndex).FacilityType;

                List<DailyOpsData> source = getHealthSafetyIssues(dataLists[facilityType]);

                if (source.Count == 0)
                    e.Row.Visible = false;
                else
                {
                    GridView gv = (GridView)e.Row.FindControl("HealthSafetyData");
                    gv.DataSource = source;
                    gv.DataBind();
                }
            }
        }

        protected void HealthSafetyData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView HealthSafetyData = (GridView)sender;
                List<DailyOpsData> data = (List<DailyOpsData>)HealthSafetyData.DataSource;
                if (data == null)
                    return;
                string facility = data.ElementAt(e.Row.RowIndex).FacilityID;

                Covanta.Common.Enums.Enums.StatusEnum status = Covanta.Common.Enums.Enums.StatusEnum.NotSet;
                List<DailyOpsReportableEvent> statuses = dailyDataManager.GetDailyOpsEventsHealthAndSafetyByDateAndFacility(ReportDate, facility, ref status);

                GridView gv = (GridView)e.Row.FindControl("HealthSafetyEventData");
                gv.DataSource = statuses;
                gv.DataBind();
            }
        }

        /// <summary>
        /// Binds actual facility data to each facility type row for the Comments table.
        /// </summary>
        /// <param name="sender">the Comments GridView</param>
        /// <param name="e">contains info about the row being created</param>
        protected void CommentsFacilityTypeData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].ColumnSpan = 3;
                e.Row.Cells.RemoveAt(2);

                List<DailyOpsFacilitiesReportingStats> types = (List<DailyOpsFacilitiesReportingStats>)((GridView)sender).DataSource;
                //var types = types.Where(t => !t.FacilityType.Equals("BioMass"));
                if (types == null)
                    return;
                string facilityType = types.ElementAt(e.Row.RowIndex).FacilityType;

                List<DailyOpsData> source = getCommentsIssues(dataLists[facilityType]);

                if (source.Count == 0)
                    e.Row.Visible = false;
                else
                {
                    GridView gv = (GridView)e.Row.FindControl("CommentsData");
                    gv.DataSource = source;
                    gv.DataBind();
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
            return !(boilerStatus.Equals("") || boilerStatus.Equals("Operational") || boilerStatus.Equals("Decommissioned"));
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