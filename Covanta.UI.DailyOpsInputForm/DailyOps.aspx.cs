using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Covanta.BusinessLogic;
using Covanta.Entities;
using Covanta.Common.Enums;
using System.Drawing;

// to get user ID (e.g., cov\jrittner) as a string: Request.ServerVariables["LOGON_USER"].ToString();

namespace Covanta.UI.DailyOpsInputForm
{

    public partial class DailyOps : System.Web.UI.Page
    {

        #region private static fields

        private static string covmetadataConnString = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
        private static DailyOpsBusiUnitManager busUnitManager = new DailyOpsBusiUnitManager(covmetadataConnString);
        private static DailyOpsManager dailyDataManager = new DailyOpsManager(covmetadataConnString);
        private static DateTime MIN_DATE = DateTime.Parse("1/1/1900");
        private static List<DailyOpsBusinessUnit> facilityList = new List<DailyOpsBusinessUnit>();
        private static decimal toleranceMultiplier; // this is set in web.config 
        public const string StandBy = "Stand-By";
        public const string StandByBoilerInOutage = "Stand-By (Boilers in Outage)";
        public const string StandBySellingSteam = "Stand-By (Selling Steam)";
        public const string UnscheduledTGOutage = "Unscheduled TG Outage";
        public const string UnscheduledBoilerInOutage = "Unscheduled Outage (Bolier in Outage)";
        public const string UnscheduledSellingSteam = "Unscheduled Outage (Selling Steam)";
        public const string Other = "Other";

        public const string Operational = "Operational";
        public const string UnscheduledOutage = "Unscheduled Outage";

        #endregion

        #region protected methods


        protected void Page_Init(object sender, EventArgs e)
        {
         
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // first time loading page
            if (!Page.IsPostBack)
            {
                InitializeForm();
                // updateDynamicFields();
                if (Request.Cookies["DailyOpsInputFormFacility"] != null)
                {
                    Facility.SelectedValue = Server.HtmlEncode(Request.Cookies["DailyOpsInputFormFacility"].Value);
                }
                updateDynamicFields();
                LoadForm();
            }
        }

        protected void ExceptionsReportButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ExceptionsReport.aspx");
        }

        protected void SummaryReportButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SummaryReport.aspx");
        }

        protected void MassUpdateButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DailyOpsMassUpdate2.aspx");
        }

        protected void Facility_SelectedIndexChanged(object sender, EventArgs e)
        {
            setUpCookieForPSCode();
            updateDynamicFields();
            LoadForm();
            //Added by ERic Chen
            DisablePreviousDecommissionedFields();
            //hide Boiler 1 & 2 for Palm2
            HidePalmsBoiler12fields();
        }

        protected void ReportingDate_TextChanged(object sender, EventArgs e)
        {
            if (updateReportingDate())
                LoadForm();
            DisablePreviousDecommissionedFields();
            HidePalmsBoiler12fields();

        }

        /// <summary>
        /// First checks if the form fields are all valid. If they are, submits data to the database.
        /// If they aren't, displays an error message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string errors, warnings;
            if (ValidateFields(out errors, out warnings))
            {
                submitData();
                InputRegion.Visible = false;
                ConfirmationPrompt.Visible = true;
                doWhereControl(clearContents, isClearableField);
                DisablePreviousDecommissionedFields();
                HidePalmsBoiler12fields();
            }

            else
            {
                PopulatePeopleSoftCodeHint();
                DisablePreviousDecommissionedFields();
                HidePalmsBoiler12fields();
                if (!errors.Equals(""))
                {
                    Errors.Visible = true;
                    Errors.InnerHtml += errors;
                }
                if (!warnings.Equals(""))
                {
                    Warnings.Visible = true;
                    Warnings.InnerHtml += warnings;
                }
            }
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            doWhereControl(clearContents, isClearableField);
            DisablePreviousDecommissionedFields();
            HidePalmsBoiler12fields();
        }

        /// <summary>
        /// Prepoluates all input fields with data from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            UpdatePrompt.Visible = false;
            InputRegion.Visible = true;
            prepolulateFields();
            //DisablePreviousDecommissionedFields();

            doWhereControl(updateDateField, isInputCalendar);
            DisablePreviousDecommissionedFields();
            PopulatePeopleSoftCodeHint();
            HidePalmsBoiler12fields();
        }

        protected void ReplaceButton_Click(object sender, EventArgs e)
        {
            UpdatePrompt.Visible = false;
            InputRegion.Visible = true;
            doWhereControl(updateDateField, isInputCalendar);
            DisablePreviousDecommissionedFields();
            PopulatePeopleSoftCodeHint();
            HidePalmsBoiler12fields();
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            InputRegion.Visible = true;
            ConfirmationPrompt.Visible = false;
            prepolulateFields();
            DisablePreviousDecommissionedFields();
            PopulatePeopleSoftCodeHint();
            HidePalmsBoiler12fields();
        }

        #endregion

        #region private methods

        /// <summary>
        /// Sets up the form when the page first loads (binds data to facility list, initializes reporting date to yesterday).
        /// </summary>
        private void InitializeForm()
        {
            string facilityCode = Request.QueryString["Facility"];
            if (facilityCode != null)
                facilityCode = facilityCode.ToUpper();
            string reportDate = Request.QueryString["Date"];

            // bind list of facilities from database to the Facility Drop Down List
            List<DailyOpsBusinessUnit> list = busUnitManager.GetBusinessUnitsList();
            Facility.DataSource = list;
            Facility.DataTextField = "Description";
            Facility.DataValueField = "PS_Unit";
            Facility.DataBind();
            Facility.SelectedValue = facilityCode;

            DateTime reporting;
            if (!DateTime.TryParse(reportDate, out reporting))
                reporting = DateTime.Now.AddHours(+5).AddDays(-1);
            ReportingDate.Text = reporting.ToShortDateString();
            LastReportingDate.Value = ReportingDate.Text;
            if (Request.ServerVariables["LOGON_USER"].ToString().Length > 4)
            {
                UserName.Text = Request.ServerVariables["LOGON_USER"].ToString().Substring(4);
            }
            else
            {
                UserName.Text = "TestAcct";
            }
            //UserName.Text = "EChen";
            EnvironmentLabel.Text = ConfigurationManager.AppSettings["systemEnvironment"].ToString();
        }

        /// <summary>
        /// Sets up the form when the reporting date or facility is changed.
        /// This should handle locking out the entry, retrieving pre-existing entry from database, etc.
        /// </summary>
        private void LoadForm()
        {
            // check if database already contains entry
            // if so, prompt UPDATE, REPLACE
            //// UPDATE: reset all fields and prepopulate with database data
            //// REPLACE: retain data already entered

            InputRegion.Visible = false;
            UpdatePrompt.Visible = false;
            ConfirmationPrompt.Visible = false;

            PopulatePeopleSoftCodeHint();

            //if we have missing reporting dates, then return here so the message can be displayed
            checkForMissingEntriesEarlierInTheMonth();
            if (ReportingDatesMissingPrompt.Visible == true)
            {
                return;
            }

            Enums.StatusEnum status = Enums.StatusEnum.NotSet;
            DailyOpsData data = dailyDataManager.GetDailyOpsDataByDateAndFacility(DateTime.Parse(ReportingDate.Text), Facility.SelectedValue, ref status);
            if (data.UserRowCreated != null)
            {
                int offset = Int32.Parse(busUnitManager.GetDailyOpsBusinessUnitEasternTimeOffset(data.FacilityID));
                string user = data.UserRowCreated;
                DateTime modified = data.DateLastModified.AddHours(offset);
                UpdatePromptText.InnerText = string.Format("A report for this date was previously submitted by {0} on {1} at {2}.", user, modified.ToShortDateString(), modified.ToShortTimeString());
                UpdatePrompt.Visible = true;
            }
            else
            {
                doWhereControl(clearContents, isClearableField);
                InputRegion.Visible = true;
                doWhereControl(updateDateField, isInputCalendar);
                //added by eric chen if no record found lear the records
                doWhereControl(clearContents, isClearableField);
                DisablePreviousDecommissionedFields();
                HidePalmsBoiler12fields();
            }

            if (data.FaciltyType == "RDF")
            {
                PreShredInventoryRow.Visible = true;
                PostShredInventoryRow.Visible = true;
                PitInventoryRow.Visible = false;
                if (data.FacilityID == "HONOL")
                {
                    MassBurnInventoryRow.Visible = true;
                }
                else
                {
                    MassBurnInventoryRow.Visible = false;
                }
            }
            else
            {
                PreShredInventoryRow.Visible = false;
                PostShredInventoryRow.Visible = false;
                MassBurnInventoryRow.Visible = false;
                PitInventoryRow.Visible = true;
            }
        }

        /// <summary>
        /// Dynamically hides/shows boiler fields and front end fields to show only fields for the assets the selected facility actually has.
        /// </summary>
        private void updateDynamicFields()
        {
            int numBoilers = 6;
            int numTurbines = 2;
            bool hasFrontEnd = true;

            DailyOpsBusinessUnit selected = getSelectedFacility(Facility.SelectedItem.Text);
            if (selected != null)
            {
                numBoilers = selected.NumOfBoilers;
                numTurbines = selected.NumOfTurbines;
                hasFrontEnd = selected.IsFacilityFrontEndFerrousSystem;
            }

            for (int i = 0; i < numBoilers; i++)
                Page.FindControl("Boiler" + (i + 1) + "Input").Visible = true;
            for (int i = numBoilers; i < 6; i++)
                Page.FindControl("Boiler" + (i + 1) + "Input").Visible = false;

            for (int i = 0; i < numTurbines; i++)
                Page.FindControl("Turbine" + (i + 1) + "Input").Visible = true;
            for (int i = numTurbines; i < 2; i++)
                Page.FindControl("Turbine" + (i + 1) + "Input").Visible = false;

            FrontEndInput.Visible = hasFrontEnd;

            List<string> FMIds = busUnitManager.GetDailyOpsBusinessUnitNTIDList(Facility.SelectedValue, "FM");
            List<string> FMEmails = busUnitManager.GetDailyOpsBusinessUnitContactEmailList(Facility.SelectedValue, "FM");
            List<string> FMNames = busUnitManager.GetDailyOpsBusinessUnitContactNameList(Facility.SelectedValue, "FM");

            List<string> CEIds = busUnitManager.GetDailyOpsBusinessUnitNTIDList(Facility.SelectedValue, "CE");
            List<string> CEEmails = busUnitManager.GetDailyOpsBusinessUnitContactEmailList(Facility.SelectedValue, "CE");
            List<string> CENames = busUnitManager.GetDailyOpsBusinessUnitContactNameList(Facility.SelectedValue, "CE");

            if (FMNames.Count > 0)
                FacilityManagerName.Text = FMNames[0];
            else
                FacilityManagerName.Text = "";
            if (FMEmails.Count > 0)
                FacilityManagerEmail.Text = FMEmails[0];
            else
                FacilityManagerEmail.Text = "";
            if (FMIds.Count > 0)
                FacilityManagerUserId.Text = FMIds[0];
            else
                FacilityManagerUserId.Text = "";

            if (CENames.Count > 0)
                ChiefEngineerName.Text = CENames[0];
            else
                ChiefEngineerName.Text = "";
            if (CEEmails.Count > 0)
                ChiefEngineerEmail.Text = CEEmails[0];
            else
                ChiefEngineerEmail.Text = "";
            if (CEIds.Count > 0)
                ChiefEngineerUserId.Text = CEIds[0];
            else
                ChiefEngineerUserId.Text = "";
        }

        private void setUpCookieForPSCode()
        {
            // Set up cookie
            HttpCookie aCookie;
            if (Request.Cookies["DailyOpsInputFormFacility"] == null)
            {
                aCookie = new HttpCookie("DailyOpsInputFormFacility");
            }
            else
            {
                aCookie = Request.Cookies["DailyOpsInputFormFacility"];
            }

            if (Facility.SelectedValue != null)
            {
                aCookie.Value = Facility.SelectedValue;
                aCookie.Expires = DateTime.Now.AddYears(10);
                Response.Cookies.Add(aCookie);
            }
            else
            {
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Remove("DailyOpsInputFormFacility");
            }
        }


        /// <summary>
        /// Check to see if there are missing entries for dates earlier in the month.
        /// If there are, then display a message and force the user to enter the missing dates data.
        /// </summary>
        private void checkForMissingEntriesEarlierInTheMonth()
        {
            ReportingDatesMissingPrompt.Visible = false;
            if (dailyDataManager.IsFacilityMissingReportingDates(DateTime.Parse(ReportingDate.Text), Facility.SelectedValue))
            {
                DateTime missingReportingDate = dailyDataManager.GetFirstMissingReportingDate(DateTime.Parse(ReportingDate.Text), Facility.SelectedValue);
                string missingDateString = missingReportingDate.ToShortDateString();
                ReportingDatesMissingPromptText.InnerText = string.Format("We are showing that {0} has not entered data for reporting date {1}.  You will not be able to enter data for {2} until you enter data for {3}.  Please switch to reporting date {4} and enter the Daily Ops data for that day.", Facility.SelectedValue, missingDateString, ReportingDate.Text, missingDateString, missingDateString);
                ReportingDatesMissingPrompt.Visible = true;
            }
        }

        private void PopulatePeopleSoftCodeHint()
        {
            //Populate Hint
            //PeopleSoftCodeHint.Text = "Hint:  " + Facility.SelectedValue;
            PeopleSoftCodeHint.Text = "Hint:  If this is " + Facility.SelectedItem + ".  PeolpleSoft Code is " + Facility.SelectedValue;
            PeopleSoftCodeHint.ForeColor = Color.Red;

            PeopleSoftCode.Text = Facility.SelectedValue;
        }

        /// <summary>
        /// Makes sure the reporting date is a valid date.
        /// If it is not a date, reverts the reporting date to its previous value.
        /// </summary>
        /// <returns>whether the reporting date has changed</returns>
        private bool updateReportingDate()
        {
            DateTime reporting;
            if (!DateTime.TryParse(ReportingDate.Text, out reporting))
            {
                ReportingDate.Text = LastReportingDate.Value;
                return false;
            }

            // Do not allow the user to enter a date in the future.
            // Today is fine as long as it is toward the end of the day
            DateErrorMessage.Text = string.Empty;
            DateErrorMessage.ForeColor = Color.Red;
            //future
            if (reporting > DateTime.Today)
            {
                ReportingDate.Text = LastReportingDate.Value;
                DateErrorMessage.Text = "You are not permitted to enter data for a future date.  Reverting back to yesterdays date.";
                return false;
            }
            if (reporting == DateTime.Today)
            {
                ReportingDate.Text = LastReportingDate.Value;
                DateErrorMessage.Text = "You are not permitted to enter data for today yet.  Reverting back to yesterdays date.";
                return false;
            }

            if (reporting.Equals(DateTime.Parse(LastReportingDate.Value)))
            {
                ReportingDate.Text = LastReportingDate.Value;
                return false;
            }

            ReportingDate.Text = reporting.ToShortDateString();
            LastReportingDate.Value = ReportingDate.Text;
            return true;
        }

        private void prepolulateFields()
        {
            // retrieve data from database
            Enums.StatusEnum status = Enums.StatusEnum.OK;
            DailyOpsData data = dailyDataManager.GetDailyOpsDataByDateAndFacility(DateTime.Parse(ReportingDate.Text), Facility.SelectedValue, ref status);

            if(data.FaciltyType == "RDF")
            {
                PreShredInventoryRow.Visible = true;
                PostShredInventoryRow.Visible = true;
                PitInventoryRow.Visible = false;
                PreShredInventory.Text = "" + data.PreShredInventory;
                PostShredInventory.Text = "" + data.PostShredInventory;
                if (data.FacilityID == "HONOL")
                {
                    MassBurnInventoryRow.Visible = true;
                    MassBurnInventory.Text = "" + data.MassBurnInventory;
                }
            }
            else
            {
                PreShredInventoryRow.Visible = false;
                PostShredInventoryRow.Visible = false;
                MassBurnInventoryRow.Visible = false;
                PitInventoryRow.Visible = true;
            }

            // prepolulate fields
            TonsDelivered.Text = "" + data.TonsDelivered;
            TonsProcessed.Text = "" + data.TonsProcessed;
            SteamProduced.Text = "" + data.SteamProduced;
            SteamSold.Text = "" + data.SteamSold;
            NetElectric.Text = "" + data.NetElectric;
            PitInventory.Text = "" + data.PitInventory;

            Boiler1Status.SelectedValue = data.OutageTypeBoiler1;
            Boiler1Downtime.Text = "";

            Boiler1ExpectedRepairDate.Text = "";
            Boiler1Unscheduled.Text = "";

            //Added by Kranthi Astakala on 7/12/2017 to disable the Boiler 1 and Boiler 2 for Niagara favility if their values are decomissioned
            if (data.OutageTypeBoiler1.Equals("Decommissioned"))
            {
                Boiler1Decommissioned.Enabled = true;
                Boiler1Status.Enabled = false;
                Boiler1DowntimeLabel.Visible = false;
                Boiler1Downtime.Visible = false;
                Boiler1ExpectedRepairDateLabel.Visible = false;
                Boiler1ExpectedRepairDate.Visible = false;
            }
            else             //Modified by Eric to fix issues with inactivated non-decommissioned dropdown list
            {
                Boiler1Decommissioned.Enabled = false;
                Boiler1Status.Enabled = true;
                Boiler1DowntimeLabel.Visible = true;
                Boiler1Downtime.Visible = true;
                Boiler1ExpectedRepairDateLabel.Visible = true;
                Boiler1ExpectedRepairDate.Visible = true;
            }


            if (data.OutageTypeBoiler2.Equals("Decommissioned"))
            {
                Boiler2Decommissioned.Enabled = true;
                Boiler2Status.Enabled = false;
                Boiler2DowntimeLabel.Visible = false;
                Boiler2Downtime.Visible = false;
                Boiler2ExpectedRepairDateLabel.Visible = false;
                Boiler2ExpectedRepairDate.Visible = false;
            }
            else             //Modified by Eric to fix issues with inactivated non-decommissioned dropdown list
            {
                Boiler2Decommissioned.Enabled = false;
                Boiler2Status.Enabled = true;
                Boiler2DowntimeLabel.Visible = true;
                Boiler2Downtime.Visible = true;
                Boiler2ExpectedRepairDateLabel.Visible = true;
                Boiler2ExpectedRepairDate.Visible = true;
            }

            DisablePreviousDecommissionedFields();
            HidePalmsBoiler12fields();

            //added decomissioned senario in the if condition
            if (!data.OutageTypeBoiler1.Equals("") && !data.OutageTypeBoiler1.Equals("Operational") && !data.OutageTypeBoiler1.Equals("Decommisioned"))
            {

                Boiler1Downtime.Text = "" + data.DownTimeBoiler1;
                Boiler1ExpectedRepairDate.Text = (data.Boiler1ExpectedRepairDate.Equals(MIN_DATE)) ? "" : data.Boiler1ExpectedRepairDate.ToShortDateString();
                if ((data.OutageTypeBoiler1.Equals("Unscheduled Outage")) || (data.OutageTypeBoiler1.Equals(StandBy)))
                {
                    Boiler1Unscheduled.Text = data.ExplanationBoiler1;

                }
            }

            Boiler2Status.SelectedValue = data.OutageTypeBoiler2;
            Boiler2Downtime.Text = "";
            Boiler2Unscheduled.Text = "";
            Boiler2ExpectedRepairDate.Text = "";
            if (!data.OutageTypeBoiler2.Equals("") && !data.OutageTypeBoiler2.Equals("Operational") && !data.OutageTypeBoiler2.Equals("Decommisioned"))
            {
                Boiler2Downtime.Text = "" + data.DownTimeBoiler2;
                Boiler2ExpectedRepairDate.Text = (data.Boiler2ExpectedRepairDate.Equals(MIN_DATE)) ? "" : data.Boiler2ExpectedRepairDate.ToShortDateString();
                if ((data.OutageTypeBoiler2.Equals("Unscheduled Outage")) || (data.OutageTypeBoiler2.Equals(StandBy)))
                    Boiler2Unscheduled.Text = data.ExplanationBoiler2;
            }

            Boiler3Status.SelectedValue = data.OutageTypeBoiler3;
            Boiler3Downtime.Text = "";
            Boiler3ExpectedRepairDate.Text = "";
            Boiler3Unscheduled.Text = "";
            if (!data.OutageTypeBoiler3.Equals("") && !data.OutageTypeBoiler3.Equals("Operational") && !data.OutageTypeBoiler3.Equals("Decommisioned"))
            {
                Boiler3Downtime.Text = "" + data.DownTimeBoiler3;
                Boiler3ExpectedRepairDate.Text = (data.Boiler3ExpectedRepairDate.Equals(MIN_DATE)) ? "" : data.Boiler3ExpectedRepairDate.ToShortDateString();

                if ((data.OutageTypeBoiler3.Equals("Unscheduled Outage")) || (data.OutageTypeBoiler3.Equals(StandBy)))
                    Boiler3Unscheduled.Text = data.ExplanationBoiler3;
            }

            Boiler4Status.SelectedValue = data.OutageTypeBoiler4;
            Boiler4Downtime.Text = "";
            Boiler4ExpectedRepairDate.Text = "";
            Boiler4Unscheduled.Text = "";
            if (!data.OutageTypeBoiler4.Equals("") && !data.OutageTypeBoiler4.Equals("Operational") && !data.OutageTypeBoiler4.Equals("Decommisioned"))
            {
                Boiler4Downtime.Text = "" + data.DownTimeBoiler4;
                Boiler4ExpectedRepairDate.Text = (data.Boiler4ExpectedRepairDate.Equals(MIN_DATE)) ? "" : data.Boiler4ExpectedRepairDate.ToShortDateString();

                if ((data.OutageTypeBoiler4.Equals("Unscheduled Outage")) || (data.OutageTypeBoiler4.Equals(StandBy)))
                    Boiler4Unscheduled.Text = data.ExplanationBoiler4;
            }

            Boiler5Status.SelectedValue = data.OutageTypeBoiler5;
            Boiler5Downtime.Text = "";
            Boiler5ExpectedRepairDate.Text = "";
            Boiler5Unscheduled.Text = "";
            if (!data.OutageTypeBoiler5.Equals("") && !data.OutageTypeBoiler5.Equals("Operational") && !data.OutageTypeBoiler5.Equals("Decommisioned"))
            {
                Boiler5Downtime.Text = "" + data.DownTimeBoiler5;
                Boiler5ExpectedRepairDate.Text = (data.Boiler5ExpectedRepairDate.Equals(MIN_DATE)) ? "" : data.Boiler5ExpectedRepairDate.ToShortDateString();
                if ((data.OutageTypeBoiler5.Equals("Unscheduled Outage")) || (data.OutageTypeBoiler5.Equals(StandBy)))
                    Boiler5Unscheduled.Text = data.ExplanationBoiler5;
            }

            Boiler6Status.SelectedValue = data.OutageTypeBoiler6;
            Boiler6Downtime.Text = "";
            Boiler6ExpectedRepairDate.Text = "";
            Boiler6Unscheduled.Text = (data.Boiler6ExpectedRepairDate.Equals(MIN_DATE)) ? "" : data.Boiler6ExpectedRepairDate.ToShortDateString();
            if (!data.OutageTypeBoiler6.Equals("") && !data.OutageTypeBoiler6.Equals("Operational") && !data.OutageTypeBoiler6.Equals("Decommisioned"))
            {
                Boiler6Downtime.Text = "" + data.DownTimeBoiler6;
                Boiler6ExpectedRepairDate.Text = (data.Boiler6ExpectedRepairDate.Equals(MIN_DATE)) ? "" : data.Boiler6ExpectedRepairDate.ToShortDateString();
                if ((data.OutageTypeBoiler6.Equals("Unscheduled Outage")) || (data.OutageTypeBoiler6.Equals(StandBy)))
                    Boiler6Unscheduled.Text = data.ExplanationBoiler6;
            }

            Turbine1Status.SelectedValue = data.OutageTypeTurbGen1;
            Turbine1Downtime.Text = "";
            Turbine1ExpectedRepairDate.Text = "";
            Turbine1Unscheduled.Text = (data.TurbGen1ExpectedRepairDate.Equals(MIN_DATE)) ? "" : data.TurbGen1ExpectedRepairDate.ToShortDateString();
            if (!data.OutageTypeTurbGen1.Equals("") && !data.OutageTypeTurbGen1.Equals("Operational"))
            {
                Turbine1Downtime.Text = "" + data.DownTimeTurbGen1;
                Turbine1ExpectedRepairDate.Text = (data.TurbGen1ExpectedRepairDate.Equals(MIN_DATE)) ? "" : data.TurbGen1ExpectedRepairDate.ToShortDateString();
                if ((data.OutageTypeTurbGen1.Equals(UnscheduledTGOutage)) || (data.OutageTypeTurbGen1.Equals(Other)))
                    Turbine1Unscheduled.Text = data.ExplanationTurbGen1;
            }

            Turbine2Status.SelectedValue = data.OutageTypeTurbGen2;
            Turbine2Downtime.Text = "";
            Turbine2ExpectedRepairDate.Text = "";
            Turbine2Unscheduled.Text = "";
            if (!data.OutageTypeTurbGen2.Equals("") && !data.OutageTypeTurbGen2.Equals("Operational"))
            {
                Turbine2Downtime.Text = "" + data.DownTimeTurbGen2;
                Turbine2ExpectedRepairDate.Text = (data.TurbGen2ExpectedRepairDate.Equals(MIN_DATE)) ? "" : data.TurbGen2ExpectedRepairDate.ToShortDateString();
                if ((data.OutageTypeTurbGen2.Equals(UnscheduledTGOutage)) || (data.OutageTypeTurbGen2.Equals(Other)))
                    Turbine2Unscheduled.Text = data.ExplanationTurbGen2;
            }

            //Boiler1ScheduledReason.SelectedValue = data.ScheduledOutageReasonBoiler1;
            Boiler1Scheduled.Text = data.ScheduledOutageReasonBoiler1;
            Boiler2Scheduled.Text = data.ScheduledOutageReasonBoiler2;
            Boiler3Scheduled.Text = data.ScheduledOutageReasonBoiler3;
            Boiler4Scheduled.Text = data.ScheduledOutageReasonBoiler4;
            Boiler5Scheduled.Text = data.ScheduledOutageReasonBoiler5;
            Boiler6Scheduled.Text = data.ScheduledOutageReasonBoiler6;

            
            Turbine1Scheduled.Text = data.ScheduledOutageReasonTurbGen1;
            Turbine2Scheduled.Text = data.ScheduledOutageReasonTurbGen2;

            FerrousTons.Text = "" + data.FerrousTons;
            NonFerrousTons.Text = "" + data.NonFerrousTons;

            FerrousAvailable.SelectedValue = data.WasFerrousSystem100PercentAvailable;
            FerrousHoursUnavailable.Text = "";
            FerrousReason.Text = "";
            FerrousDate.Text = "";
            FerrousReprocessed.ClearSelection();
            if (data.WasFerrousSystem100PercentAvailable.Equals("N"))
            {
                FerrousHoursUnavailable.Text = "" + data.FerrousSystemHoursUnavailable;
                FerrousReason.Text = data.FerrousSystemHoursUnavailableReason;
                FerrousDate.Text = (data.FerrousSystemExpectedBackOnlineDate.Equals(MIN_DATE)) ? "" : data.FerrousSystemExpectedBackOnlineDate.ToShortDateString();
                FerrousReprocessed.SelectedValue = data.WasAshReprocessedThroughFerrousSystem;
            }

            NonFerrousAvailable.SelectedValue = data.WasNonFerrousSystem100PercentAvailable;
            NonFerrousHoursUnavailable.Text = "";
            NonFerrousReason.Text = "";
            NonFerrousDate.Text = "";
            NonFerrousReprocessed.ClearSelection();
            if (data.WasNonFerrousSystem100PercentAvailable.Equals("N"))
            {
                NonFerrousHoursUnavailable.Text = "" + data.NonFerrousSystemHoursUnavailable;
                NonFerrousReason.Text = data.NonFerrousSystemHoursUnavailableReason;
                NonFerrousDate.Text = data.NonFerrousSystemExpectedBackOnlineDate.ToShortDateString();
                NonFerrousReprocessed.SelectedValue = data.WasAshReprocessedThroughNonFerrousSystem;
            }

            NonFerrousSmallsAvailable.SelectedValue = data.WasNonFerrousSmallsSystem100PercentAvailable;
            NonFerrousSmallsHoursUnavailable.Text = "";
            NonFerrousSmallsReason.Text = "";
            NonFerrousSmallsDate.Text = "";
            NonFerrousSmallsReprocessed.ClearSelection();
            if (data.WasNonFerrousSmallsSystem100PercentAvailable.Equals("N"))
            {
                NonFerrousSmallsHoursUnavailable.Text = "" + data.NonFerrousSmallsSystemHoursUnavailable;
                NonFerrousSmallsReason.Text = data.NonFerrousSmallsSystemHoursUnavailableReason;
                NonFerrousSmallsDate.Text = (data.NonFerrousSmallsSystemExpectedBackOnlineDate.Equals(MIN_DATE)) ? "" : data.NonFerrousSmallsSystemExpectedBackOnlineDate.ToShortDateString();
                NonFerrousSmallsReprocessed.SelectedValue = data.WasAshReprocessedThroughNonFerrousSmallsSystem;
            }

            FrontEndFerrousAvailable.SelectedValue = data.WasFrontEndFerrousSystem100PercentAvailable;
            FrontEndFerrousHoursUnavailable.Text = "";
            FrontEndFerrousReason.Text = "";
            FrontEndFerrousDate.Text = "";
            FrontEndFerrousReprocessed.ClearSelection();
            if (data.WasFrontEndFerrousSystem100PercentAvailable.Equals("N"))
            {
                FrontEndFerrousHoursUnavailable.Text = "" + data.FrontEndFerrousSystemHoursUnavailable;
                FrontEndFerrousReason.Text = data.FrontEndFerrousSystemHoursUnavailableReason;
                FrontEndFerrousDate.Text = (data.FrontEndFerrousSystemExpectedBackOnlineDate.Equals(MIN_DATE)) ? "" : data.FrontEndFerrousSystemExpectedBackOnlineDate.ToShortDateString();
                FrontEndFerrousReprocessed.SelectedValue = data.WasAshReprocessedThroughFrontEndFerrousSystem;
            }



            EnhancedFerrousAvailable.SelectedValue = data.WasEnhancedFerrousSystem100PercentAvailable;
            EnhancedFerrousHoursUnavailable.Text = "";
            EnhancedFerrousReason.Text = "";
            EnhancedFerrousDate.Text = "";
            EnhancedFerrousReprocessed.ClearSelection();
            EnhancedFerrousAvailable.SelectedValue = data.WasEnhancedFerrousSystem100PercentAvailable;
            if (data.WasEnhancedFerrousSystem100PercentAvailable.Equals("N"))
            {
                EnhancedFerrousHoursUnavailable.Text = "" + data.EnhancedFerrousSystemHoursUnavailable;
                EnhancedFerrousReason.Text = data.EnhancedFerrousSystemHoursUnavailableReason;
                EnhancedFerrousDate.Text = (data.EnhancedFerrousSystemExpectedBackOnlineDate.Equals(MIN_DATE)) ? "" : data.EnhancedFerrousSystemExpectedBackOnlineDate.ToShortDateString();
                EnhancedFerrousReprocessed.SelectedValue = data.WasAshReprocessedThroughEnhancedFerrousSystem;
            }

            EnhancedNonFerrousAvailable.SelectedValue = data.WasEnhancedNonFerrousSystem100PercentAvailable;
            EnhancedNonFerrousHoursUnavailable.Text = "";
            EnhancedNonFerrousReason.Text = "";
            EnhancedNonFerrousDate.Text = "";
            EnhancedNonFerrousReprocessed.ClearSelection();
            EnhancedNonFerrousAvailable.SelectedValue = data.WasEnhancedNonFerrousSystem100PercentAvailable;
            if (data.WasEnhancedNonFerrousSystem100PercentAvailable.Equals("N"))
            {
                EnhancedNonFerrousHoursUnavailable.Text = "" + data.EnhancedNonFerrousSystemHoursUnavailable;
                EnhancedNonFerrousReason.Text = data.EnhancedNonFerrousSystemHoursUnavailableReason;
                EnhancedNonFerrousDate.Text = (data.EnhancedNonFerrousSystemExpectedBackOnlineDate.Equals(MIN_DATE)) ? "" : data.EnhancedNonFerrousSystemExpectedBackOnlineDate.ToShortDateString();
                EnhancedNonFerrousReprocessed.SelectedValue = data.WasAshReprocessedThroughEnhancedNonFerrousSystem;
            }

            FireSystem.Text = data.FireSystemOutOfService;
            FireSystemDate.Text = (data.FireSystemOutOfServiceExpectedBackOnlineDate.Equals(MIN_DATE)) ? "" : data.FireSystemOutOfServiceExpectedBackOnlineDate.ToShortDateString();
            FireSystemCheckBox.Checked = string.IsNullOrEmpty(FireSystem.Text.Trim());

            CriticalAssets.Text = data.CriticalAssetsInAlarm;
            CriticalAssetsCheckBox.Checked = string.IsNullOrEmpty(CriticalAssets.Text.Trim());

            EnvironmentalEvents.SelectedValue = "" + data.IsEnvironmentalEvents;
            if (data.EnvironmentalEventsType.Equals(""))
                EnvironmentalEventType.ClearSelection();
            else
                EnvironmentalEventType.SelectedValue = data.EnvironmentalEventsType;
            EnvironmentalExplanation.Text = data.EnvironmentalEventsExplanation;

            CemsEvents.SelectedValue = "" + data.IsCEMSEvents;
            if (data.CEMSEventsType.Equals(""))
                CemsEventType.ClearSelection();
            else
                CemsEventType.SelectedValue = data.CEMSEventsType;
            CemsExplanation.Text = data.CEMSEventsExplanation;

            FirstAid.Text = data.HealthSafetyFirstAid;
            FirstAidCheckBox.Checked = string.IsNullOrEmpty(FirstAid.Text.Trim());

            OshaRecordable.Text = data.HealthSafetyOSHAReportable;
            OshaRecordableCheckBox.Checked = string.IsNullOrEmpty(OshaRecordable.Text.Trim());

            EmployeeSafetyIncidents.Text = data.HealthSafetyEmployeeSafetyIncidents;
            EmployeeSafetyIncidentsCheckBox.Checked = string.IsNullOrEmpty(EmployeeSafetyIncidents.Text.Trim());            

            NearMiss.Text = data.HealthSafetyNearMiss;
            NearMissCheckBox.Checked = string.IsNullOrEmpty(NearMiss.Text.Trim());

            Contractor.Text = data.HealthSafetyContractor;
            ContractorCheckBox.Checked = string.IsNullOrEmpty(Contractor.Text.Trim());

            EmployeeSafetyIncidents.Text = data.HealthSafetyEmployeeSafetyIncidents;
            EmployeeSafetyIncidentsCheckBox.Checked = string.IsNullOrEmpty(EmployeeSafetyIncidents.Text.Trim());

            Comments.Text = data.Comments;
            CommentsCheckBox.Checked = string.IsNullOrEmpty(Comments.Text.Trim());

            CriticalAssetsDate.Text = (data.CriticalAssetsExpectedBackOnlineDate.Equals(MIN_DATE)) ? "" : data.CriticalAssetsExpectedBackOnlineDate.ToShortDateString();
            CommentsDate.Text = (data.CriticalEquipmentOOSExpectedBackOnlineDate.Equals(MIN_DATE)) ? "" : data.CriticalEquipmentOOSExpectedBackOnlineDate.ToShortDateString();
        }


        private void DisablePreviousDecommissionedFields()
        {
            // retrieve data from database from lastest records
            Enums.StatusEnum status = Enums.StatusEnum.OK;
            DailyOpsData data = dailyDataManager.GetDailyOpsDataByDateAndFacility(DateTime.Parse(ReportingDate.Text).AddDays(-1), Facility.SelectedValue, ref status);
            //Added by Kranthi Astakala on 7/12/2017 to disable the Boiler 1 and Boiler 2 for Niagara favility if their values are decomissioned
            if (data.OutageTypeBoiler1 is null) {
                Boiler1Decommissioned.Enabled = false;
                Boiler1Status.Enabled = true;
                Boiler1DowntimeLabel.Visible = true;
                Boiler1Downtime.Visible = true;
                Boiler1ExpectedRepairDateLabel.Visible = true;
                Boiler1ExpectedRepairDate.Visible = true;
            }
            else if (data.OutageTypeBoiler1.Equals("Decommissioned"))
            {
                
                Boiler1Decommissioned.Enabled = true;
                Boiler1Status.SelectedValue = "Decommissioned";
                Boiler1Status.Enabled = false;
                Boiler1DowntimeLabel.Visible = false;
                Boiler1Downtime.Visible = false;
                Boiler1ExpectedRepairDateLabel.Visible = false;
                Boiler1ExpectedRepairDate.Visible = false;
            }
            else             //Modified by Eric to fix issues with inactivated non-decommissioned dropdown list
            {
                Boiler1Decommissioned.Enabled = false;
                Boiler1Status.Enabled = true;
                Boiler1DowntimeLabel.Visible = true;
                Boiler1Downtime.Visible = true;
                Boiler1ExpectedRepairDateLabel.Visible = true;
                Boiler1ExpectedRepairDate.Visible = true;
            }

            if (data.OutageTypeBoiler2 is null)
            {
                Boiler2Decommissioned.Enabled = false;
                Boiler2Status.Enabled = true;
                Boiler2DowntimeLabel.Visible = true;
                Boiler2Downtime.Visible = true;
                Boiler2ExpectedRepairDateLabel.Visible = true;
                Boiler2ExpectedRepairDate.Visible = true;
            }
            else if (data.OutageTypeBoiler2.Equals("Decommissioned"))
            {
                Boiler2Decommissioned.Enabled = true;
                Boiler2Status.SelectedValue = "Decommissioned";
                Boiler2Status.Enabled = false;
                Boiler2DowntimeLabel.Visible = false;
                Boiler2Downtime.Visible = false;
                Boiler2ExpectedRepairDateLabel.Visible = false;
                Boiler2ExpectedRepairDate.Visible = false;
            }
            else             //Modified by Eric to fix issues with inactivated non-decommissioned dropdown list
            {
                Boiler2Decommissioned.Enabled = false;
                Boiler2Status.Enabled = true;
                Boiler2DowntimeLabel.Visible = true;
                Boiler2Downtime.Visible = true;
                Boiler2ExpectedRepairDateLabel.Visible = true;
                Boiler2ExpectedRepairDate.Visible = true;
            }
        }

        private void HidePalmsBoiler12fields()
        {
            if (Facility.SelectedValue == "PALM2")
            {
                //Page.FindControl("Boiler1Input").Visible = false;
                //Page.FindControl("Boiler2Input").Visible = false;
            }
        }
        /// <summary>
        /// Updates all date fields *except* the reporting date.
        /// Updates the calendars to not allow a date selection preceding the reporting date
        /// </summary>
        /// <param name="c">either a date field or calendar extender to be updated</param>
        private void updateDateField(Control c)
        {
            DateTime reporting = DateTime.Parse(ReportingDate.Text);
            // prevent user from selecting illegal date via calendar
            ((AjaxControlToolkit.CalendarExtender)c).StartDate = reporting;
        }



        /// <summary>
        /// Validates all input fields for legal values.
        /// For any validation check that fails, the errorMessage string will be updated appropriately.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns>true if all fields are valid, false if not</returns>
        private bool ValidateFields(out string errorMessage, out string warningMessage)
        {
            errorMessage = "";
            warningMessage = "";

            DateTime reporting;
            if (!DateTime.TryParse(ReportingDate.Text, out reporting))
                errorMessage += "Please enter a valid Reporting Date.<br />";

            List<Control> controls = getAllControls();
            foreach (Control c in controls)
            {
                if (c.Visible)
                {
                    if (c is TextBox)
                    {
                        if (isNumericField(c))
                            //check for numeric,  check if entry is not too large (above budget tolerance)
                            validateNumericField((TextBox)c, ref errorMessage);
                        else if (isDowntimeField(c))
                            validateDowntimeField((TextBox)c, ref errorMessage);
                        else if (isUnscheduledField(c))
                            validateUnscheduledField((TextBox)c, ref errorMessage);
                        else if (isHoursUnavailableField(c))
                            validateHoursUnavailableField((TextBox)c, ref errorMessage);
                        else if (isReasonField(c))
                            validateReasonField((TextBox)c, ref errorMessage);
                        else if (isDateField(c))
                            validateDateField((TextBox)c, ref errorMessage, ref warningMessage);
                        else if (isReprocessedField(c))
                            validateReprocessedField((RadioButtonList)c, ref errorMessage);
                        else if (isEventExplanationField(c))
                            validateEventExplanationField((TextBox)c, ref errorMessage);
                        else if (isOptionalField(c))
                            validateOptionalField((TextBox)c, ref errorMessage);
                        else if (isPSCodeTextBoxValidationField(c))
                            validatePSCodeTextBoxValidationField((TextBox)c, ref errorMessage);
                    }
                    else if (c is RadioButtonList)
                    {
                        if (isYesNoField(c))
                            validateYesNoField((RadioButtonList)c, ref errorMessage);
                        else if (isReprocessedField(c))
                            validateReprocessedField((RadioButtonList)c, ref errorMessage);
                        else if (isEventTypeField(c))
                            validateEventTypeField((RadioButtonList)c, ref errorMessage);
                    }
                    else if (c is DropDownList)
                    {
                        if (isOutageStatusField(c))
                            validateOutageStatusField((DropDownList)c, ref errorMessage);
                    }

                }
            }

            return errorMessage.Equals("") && warningMessage.Equals("");
        }

        private void submitData()
        {
            string facility = Facility.SelectedValue;
            DateTime reportingDate = DateTime.Parse(ReportingDate.Text);

            decimal tonsDelivered = getNumericFieldValue(TonsDelivered);
            decimal tonsProcessed = getNumericFieldValue(TonsProcessed);
            decimal steamProduced = getNumericFieldValue(SteamProduced);
            decimal steamSold = getNumericFieldValue(SteamSold);
            decimal netElectric = getNumericFieldValue(NetElectric);
            decimal pitInventory = getNumericFieldValue(PitInventory);
            decimal preShredInventory = getNumericFieldValue(PreShredInventory);
            decimal postShredInventory = getNumericFieldValue(PostShredInventory);
            decimal massBurnInventory = getNumericFieldValue(MassBurnInventory);

            string boiler1Status = (Boiler1Status.Visible) ? Boiler1Status.SelectedValue : "";
            decimal boiler1Downtime = (!boiler1Status.Equals("") && !boiler1Status.Equals("Operational")) ? getNumericFieldValue(Boiler1Downtime) : 0;
            string boiler1Unscheduled = (boiler1Status.Equals("Unscheduled Outage")) || (boiler1Status.Equals(StandBy)) ? Boiler1Unscheduled.Text : "";
            //string boiler1ScheduledReason = (boiler1Status.Equals("Scheduled Outage")) ? Boiler1ScheduledReason.SelectedValue : "";
            string boiler1Scheduled= (boiler1Status.Equals("Scheduled Outage")) ? Boiler1Scheduled.Text : "";
            DateTime boiler1ExpectedBackOnlineDate = getDateFieldValue(Boiler1ExpectedRepairDate);

            string boiler2Status = (Boiler2Status.Visible) ? Boiler2Status.SelectedValue : "";
            decimal boiler2Downtime = (!boiler2Status.Equals("") && !boiler2Status.Equals("Operational")) ? getNumericFieldValue(Boiler2Downtime) : 0;
            string boiler2Unscheduled = (boiler2Status.Equals("Unscheduled Outage")) || (boiler2Status.Equals(StandBy)) ? Boiler2Unscheduled.Text : "";
            string boiler2Scheduled = (boiler2Status.Equals("Scheduled Outage")) ? Boiler2Scheduled.Text : "";
            DateTime boiler2ExpectedBackOnlineDate = getDateFieldValue(Boiler2ExpectedRepairDate);

            string boiler3Status = (Boiler3Status.Visible) ? Boiler3Status.SelectedValue : "";
            decimal boiler3Downtime = (!boiler3Status.Equals("") && !boiler3Status.Equals("Operational")) ? getNumericFieldValue(Boiler3Downtime) : 0;
            string boiler3Unscheduled = (boiler3Status.Equals("Unscheduled Outage")) || (boiler3Status.Equals(StandBy)) ? Boiler3Unscheduled.Text : "";
            string boiler3Scheduled = (boiler3Status.Equals("Scheduled Outage")) ? Boiler3Scheduled.Text : "";
            DateTime boiler3ExpectedBackOnlineDate = getDateFieldValue(Boiler3ExpectedRepairDate);

            string boiler4Status = (Boiler4Status.Visible) ? Boiler4Status.SelectedValue : "";
            decimal boiler4Downtime = (!boiler4Status.Equals("") && !boiler4Status.Equals("Operational")) ? getNumericFieldValue(Boiler4Downtime) : 0;
            string boiler4Unscheduled = (boiler4Status.Equals("Unscheduled Outage")) || (boiler4Status.Equals(StandBy)) ? Boiler4Unscheduled.Text : "";
            string boiler4Scheduled = (boiler4Status.Equals("Scheduled Outage")) ? Boiler4Scheduled.Text : "";
            DateTime boiler4ExpectedBackOnlineDate = getDateFieldValue(Boiler4ExpectedRepairDate);

            string boiler5Status = (Boiler5Status.Visible) ? Boiler5Status.SelectedValue : "";
            decimal boiler5Downtime = (!boiler5Status.Equals("") && !boiler5Status.Equals("Operational")) ? getNumericFieldValue(Boiler5Downtime) : 0;
            string boiler5Unscheduled = (boiler5Status.Equals("Unscheduled Outage")) || (boiler5Status.Equals(StandBy)) ? Boiler5Unscheduled.Text : "";
            string boiler5Scheduled = (boiler5Status.Equals("Scheduled Outage")) ? Boiler5Scheduled.Text : "";
            DateTime boiler5ExpectedBackOnlineDate = getDateFieldValue(Boiler5ExpectedRepairDate);

            string boiler6Status = (Boiler6Status.Visible) ? Boiler6Status.SelectedValue : "";
            decimal boiler6Downtime = (!boiler6Status.Equals("") && !boiler6Status.Equals("Operational")) ? getNumericFieldValue(Boiler6Downtime) : 0;
            string boiler6Unscheduled = (boiler6Status.Equals("Unscheduled Outage")) || (boiler6Status.Equals(StandBy)) ? Boiler6Unscheduled.Text : "";
            string boiler6Scheduled = (boiler6Status.Equals("Scheduled Outage")) ? Boiler6Scheduled.Text : "";
            DateTime boiler6ExpectedBackOnlineDate = getDateFieldValue(Boiler6ExpectedRepairDate);

            string turbine1Status = (Turbine1Status.Visible) ? Turbine1Status.SelectedValue : "";
            decimal turbine1Downtime = (!turbine1Status.Equals("") && !turbine1Status.Equals("Operational")) ? getNumericFieldValue(Turbine1Downtime) : 0;
            string turbine1Unscheduled = (turbine1Status.Equals(UnscheduledTGOutage)) || (turbine1Status.Equals(Other)) ? Turbine1Unscheduled.Text : "";
            string turbine1Scheduled = (turbine1Status.Equals("Scheduled TG Outage")) ? Turbine1Scheduled.Text : "";
            DateTime turbGen1ExpectedBackOnlineDate = getDateFieldValue(Turbine1ExpectedRepairDate);

            string turbine2Status = (Turbine2Status.Visible) ? Turbine2Status.SelectedValue : "";
            decimal turbine2Downtime = (!turbine2Status.Equals("") && !turbine2Status.Equals("Operational")) ? getNumericFieldValue(Turbine2Downtime) : 0;
            string turbine2Unscheduled = (turbine2Status.Equals(UnscheduledTGOutage)) || (turbine2Status.Equals(Other)) ? Turbine2Unscheduled.Text : "";
            string turbine2Scheduled = (turbine2Status.Equals("Scheduled TG Outage")) ? Turbine2Scheduled.Text : "";
            DateTime turbGen2ExpectedBackOnlineDate = getDateFieldValue(Turbine2ExpectedRepairDate);

            decimal ferrousTons = getNumericFieldValue(FerrousTons);
            decimal nonFerrousTons = getNumericFieldValue(NonFerrousTons);

            string ferrousAvailable = FerrousAvailable.SelectedValue;
            decimal ferrousHoursUnavailable = 0;
            string ferrousUnavailableReason = "";
            string ferrousReprocessed = "";
            DateTime ferrousDate = MIN_DATE;
            if (ferrousAvailable.Equals("N"))
            {
                ferrousHoursUnavailable = getNumericFieldValue(FerrousHoursUnavailable);
                ferrousUnavailableReason = FerrousReason.Text;
                ferrousDate = getDateFieldValue(FerrousDate);
                ferrousReprocessed = FerrousReprocessed.SelectedValue;
            }

            string nonFerrousAvailable = NonFerrousAvailable.SelectedValue;
            decimal nonFerrousHoursUnavailable = 0;
            string nonFerrousUnavailableReason = "";
            string nonFerrousReprocessed = "";
            DateTime nonFerrousDate = MIN_DATE;
            if (nonFerrousAvailable.Equals("N"))
            {
                nonFerrousHoursUnavailable = getNumericFieldValue(NonFerrousHoursUnavailable);
                nonFerrousUnavailableReason = NonFerrousReason.Text;
                nonFerrousDate = getDateFieldValue(NonFerrousDate);
                nonFerrousReprocessed = NonFerrousReprocessed.SelectedValue;
            }

            string nonFerrousSmallsAvailable = NonFerrousSmallsAvailable.SelectedValue;
            decimal nonFerrousSmallsHoursUnavailable = 0;
            string nonFerrousSmallsUnavailableReason = "";
            string nonFerrousSmallsReprocessed = "";
            DateTime nonFerrousSmallsDate = MIN_DATE;
            if (nonFerrousSmallsAvailable.Equals("N"))
            {
                nonFerrousSmallsHoursUnavailable = getNumericFieldValue(NonFerrousSmallsHoursUnavailable);
                nonFerrousSmallsUnavailableReason = NonFerrousSmallsReason.Text;
                nonFerrousSmallsDate = getDateFieldValue(NonFerrousSmallsDate);
                nonFerrousSmallsReprocessed = NonFerrousSmallsReprocessed.SelectedValue;
            }

            //frontendferrous system is null by default
            string frontEndFerrousAvailable = (FrontEndFerrousAvailable.Visible) ? FrontEndFerrousAvailable.SelectedValue : "";
            decimal frontEndFerrousHoursUnavailable = 0;
            string frontEndFerrousUnavailableReason = "";
            string frontEndFerrousReprocessed = "";
            DateTime frontEndFerrousDate = MIN_DATE;
            if (frontEndFerrousAvailable.Equals("N"))
            {
                frontEndFerrousHoursUnavailable = getNumericFieldValue(FrontEndFerrousHoursUnavailable);
                frontEndFerrousUnavailableReason = (frontEndFerrousHoursUnavailable > 0) ? FrontEndFerrousReason.Text : "";
                frontEndFerrousDate = getDateFieldValue(FrontEndFerrousDate);
                frontEndFerrousReprocessed = FrontEndFerrousReprocessed.SelectedValue;

            }

            string enhancedFerrousAvailable = (EnhancedFerrousAvailable.Visible) ? EnhancedFerrousAvailable.SelectedValue : "";
            decimal enhancedFerrousHoursUnavailable = 0;
            string enhancedFerrousUnavailableReason = "";
            string enhancedFerrousReprocessed = "";
            DateTime enhancedFerrousDate = MIN_DATE;
            if (enhancedFerrousAvailable.Equals("N"))
            {
                enhancedFerrousHoursUnavailable = getNumericFieldValue(EnhancedFerrousHoursUnavailable);
                enhancedFerrousUnavailableReason = EnhancedFerrousReason.Text;
                enhancedFerrousDate = getDateFieldValue(EnhancedFerrousDate);
                enhancedFerrousReprocessed = EnhancedFerrousReprocessed.SelectedValue;
            }

            string enhancedNonFerrousAvailable = (EnhancedNonFerrousAvailable.Visible) ? EnhancedNonFerrousAvailable.SelectedValue : "";
            decimal enhancedNonFerrousHoursUnavailable = 0;
            string enhancedNonFerrousUnavailableReason = "";
            string enhancedNonFerrousReprocessed = "";
            DateTime enhancedNonFerrousDate = MIN_DATE;
            if (enhancedNonFerrousAvailable.Equals("N"))
            {
                enhancedNonFerrousHoursUnavailable = getNumericFieldValue(EnhancedNonFerrousHoursUnavailable);
                enhancedNonFerrousUnavailableReason = EnhancedNonFerrousReason.Text;
                enhancedNonFerrousDate = getDateFieldValue(EnhancedNonFerrousDate);
                enhancedNonFerrousReprocessed = EnhancedNonFerrousReprocessed.SelectedValue;
            }

            string fireSystem = getOptionalFieldValue(FireSystem, FireSystemCheckBox);
            DateTime fireSystemDate = getDateFieldValue(FireSystemDate);

            string criticalAssets = getOptionalFieldValue(CriticalAssets, CriticalAssetsCheckBox);

            bool environmentalEvents = Boolean.Parse(EnvironmentalEvents.SelectedValue);
            string environmentalType = environmentalEvents ? EnvironmentalEventType.SelectedValue : "";
            string environmentalExplanation = environmentalEvents ? EnvironmentalExplanation.Text : "";

            bool cemsEvents = Boolean.Parse(CemsEvents.SelectedValue);
            string cemsType = cemsEvents ? CemsEventType.SelectedValue : "";
            string cemsExplanation = cemsEvents ? CemsExplanation.Text : "";

            string firstAid = getOptionalFieldValue(FirstAid, FirstAidCheckBox);
            string osha = getOptionalFieldValue(OshaRecordable, OshaRecordableCheckBox);
            string nearMiss = getOptionalFieldValue(NearMiss, NearMissCheckBox);
            string contractor = getOptionalFieldValue(Contractor, ContractorCheckBox);
            string employeeSafetyIncidents = getOptionalFieldValue(EmployeeSafetyIncidents, EmployeeSafetyIncidentsCheckBox);

            string comments = getOptionalFieldValue(Comments, CommentsCheckBox);


            DateTime criticalAssetsDate = getDateFieldValue(CriticalAssetsDate);

            DateTime commentsDate = getDateFieldValue(CommentsDate);

            //Changed by Eric Chen to make the test work on local machine
            //string userID = Request.ServerVariables["LOGON_USER"].ToString().Substring(4); 
            //string userID = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Substring(4); //USDER ID Added back comment out for testing.
            string userID = "System";

            Enums.StatusEnum status = Enums.StatusEnum.NotSet;
            string statusMessage = "";

            DailyOpsData dod = new DailyOpsData(
                    facility,
                    reportingDate,
                    tonsDelivered,
                    tonsProcessed,
                    steamProduced,
                    steamSold,
                    netElectric,
                    boiler1Status,
                    boiler1Downtime,
                    boiler1Unscheduled,
                    boiler1ExpectedBackOnlineDate,
                    boiler2Status,
                    boiler2Downtime,
                    boiler2Unscheduled,
                    boiler2ExpectedBackOnlineDate,
                    boiler3Status,
                    boiler3Downtime,
                    boiler3Unscheduled,
                    boiler3ExpectedBackOnlineDate,
                    boiler4Status,
                    boiler4Downtime,
                    boiler4Unscheduled,
                    boiler4ExpectedBackOnlineDate,
                    boiler5Status,
                    boiler5Downtime,
                    boiler5Unscheduled,
                    boiler5ExpectedBackOnlineDate,
                    boiler6Status,
                    boiler6Downtime,
                    boiler6Unscheduled,
                    boiler6ExpectedBackOnlineDate,
                    turbine1Status,
                    turbine1Downtime,
                    turbine1Unscheduled,
                    turbGen1ExpectedBackOnlineDate,
                    turbine2Status,
                    turbine2Downtime,
                    turbine2Unscheduled,
                    turbGen2ExpectedBackOnlineDate,
                    ferrousTons,
                    nonFerrousTons,
                    ferrousHoursUnavailable,
                    ferrousUnavailableReason,
                    ferrousDate,
                    ferrousReprocessed,
                    ferrousAvailable,
                    nonFerrousHoursUnavailable,
                    nonFerrousUnavailableReason,
                    nonFerrousDate,
                    nonFerrousReprocessed,
                    nonFerrousAvailable,
                    nonFerrousSmallsHoursUnavailable,
                    nonFerrousSmallsUnavailableReason,
                    nonFerrousSmallsDate,
                    nonFerrousSmallsReprocessed,
                    nonFerrousSmallsAvailable,
                    frontEndFerrousHoursUnavailable,
                    frontEndFerrousUnavailableReason,
                    frontEndFerrousDate,
                    frontEndFerrousReprocessed,
                    frontEndFerrousAvailable,
                    enhancedFerrousHoursUnavailable,
                    enhancedFerrousUnavailableReason,
                    enhancedFerrousDate,
                    enhancedFerrousReprocessed,
                    enhancedFerrousAvailable,
                    enhancedNonFerrousHoursUnavailable,
                    enhancedNonFerrousUnavailableReason,
                    enhancedNonFerrousDate,
                    enhancedNonFerrousReprocessed,
                    enhancedNonFerrousAvailable,
                    fireSystem,
                    fireSystemDate,
                    criticalAssets,
                    environmentalEvents,
                    environmentalType,
                    environmentalExplanation,
                    cemsEvents,
                    cemsType,
                    cemsExplanation,
                    firstAid,
                    osha,
                    nearMiss,
                    contractor,
                    employeeSafetyIncidents,
                    comments,
                    userID,
                    pitInventory,
                    criticalAssetsDate,
                    commentsDate,
                    preShredInventory,
                    postShredInventory,
                    massBurnInventory
                    );
            dod.ScheduledOutageReasonBoiler1 = boiler1Scheduled;
            dod.ScheduledOutageReasonBoiler2 = boiler2Scheduled;
            dod.ScheduledOutageReasonBoiler3 = boiler3Scheduled;
            dod.ScheduledOutageReasonBoiler4 = boiler4Scheduled;
            dod.ScheduledOutageReasonBoiler5 = boiler5Scheduled;
            dod.ScheduledOutageReasonBoiler6 = boiler6Scheduled;
            dod.ScheduledOutageReasonTurbGen1 = turbine1Scheduled;
            dod.ScheduledOutageReasonTurbGen2 = turbine2Scheduled;


            dailyDataManager.InsertDailyOpsData_V2(dod, ref status, ref statusMessage);

            //dailyDataManager.InsertDailyOpsData(
            //    new DailyOpsData(
            //        facility,
            //        reportingDate,
            //        tonsDelivered,
            //        tonsProcessed,
            //        steamProduced,
            //        steamSold,
            //        netElectric,
            //        boiler1Status,
            //        boiler1Downtime,
            //        boiler1Unscheduled,
            //        boiler2Status,
            //        boiler2Downtime,
            //        boiler2Unscheduled,
            //        boiler3Status,
            //        boiler3Downtime,
            //        boiler3Unscheduled,
            //        boiler4Status,
            //        boiler4Downtime,
            //        boiler4Unscheduled,
            //        boiler5Status,
            //        boiler5Downtime,
            //        boiler5Unscheduled,
            //        boiler6Status,
            //        boiler6Downtime,
            //        boiler6Unscheduled,
            //        turbine1Status,
            //        turbine1Downtime,
            //        turbine1Unscheduled,
            //        turbine2Status,
            //        turbine2Downtime,
            //        turbine2Unscheduled,
            //        ferrousTons,
            //        nonFerrousTons,
            //        ferrousHoursUnavailable,
            //        ferrousUnavailableReason,
            //        ferrousDate,
            //        ferrousReprocessed,
            //        ferrousAvailable,
            //        nonFerrousHoursUnavailable,
            //        nonFerrousUnavailableReason,
            //        nonFerrousDate,
            //        nonFerrousReprocessed,
            //        nonFerrousAvailable,
            //        nonFerrousSmallsHoursUnavailable,
            //        nonFerrousSmallsUnavailableReason,
            //        nonFerrousSmallsDate,
            //        nonFerrousSmallsReprocessed,
            //        nonFerrousSmallsAvailable,
            //        frontEndFerrousHoursUnavailable,
            //        frontEndFerrousUnavailableReason,
            //        frontEndFerrousDate,
            //        frontEndFerrousAvailable,
            //        fireSystem,
            //        fireSystemDate,
            //        criticalAssets,
            //        environmentalEvents,
            //        environmentalType,
            //        environmentalExplanation,
            //        cemsEvents,
            //        cemsType,
            //        cemsExplanation,
            //        firstAid,
            //        osha,
            //        nearMiss,
            //        contractor,
            //        comments,
            //        userID,
            //        pitInventory
            //        ), ref status, ref statusMessage);
        }

        /// <summary>
        /// Returns every control on this page in a list.
        /// </summary>
        /// <returns>a list of all controls on this page</returns>
        private List<Control> getAllControls()
        {
            List<Control> controls = new List<Control>();
            getAllControlsHelper(controls, InputRegion);
            return controls;
        }

        /// <summary>
        /// Recursive helper function for getAllControls.
        /// </summary>
        /// <param name="controlList">the list to add controls to</param>
        /// <param name="parent">the parent control to use</param>
        private void getAllControlsHelper(List<Control> controlList, Control parent)
        {
            controlList.Add(parent);

            foreach (Control c in parent.Controls)
                getAllControlsHelper(controlList, c);
        }

        /// <summary>
        /// Repeatedly calls the specified process, with the parameter being each control in sequence for which
        /// at least one of the specified conditions returns true with that control as a parameter.
        /// </summary>
        /// <param name="process">the process (i.e., method/function) to call</param>
        /// <param name="conditions">the boolean methods/functions to use in the check</param>
        private void doWhereControl(Action<Control> process, params Func<Control, bool>[] conditions)
        {
            doWhereControlHelper(InputRegion, process, conditions);
        }

        /// <summary>
        /// Recursive helper function for doWhereControl.
        /// </summary>
        /// <param name="parent">the parent control to use</param>
        /// <param name="process">the process (method/function) to call</param>
        /// <param name="conditions">the boolean methods/functions to use in the check</param>
        private void doWhereControlHelper(Control parent, Action<Control> process, Func<Control, bool>[] conditions)
        {
            foreach (Func<Control, bool> condition in conditions)
                if (condition(parent))
                {
                    process(parent);
                    break;
                }

            foreach (Control c in parent.Controls)
                doWhereControlHelper(c, process, conditions);
        }

        /// <summary>
        /// Validates the specified numeric field. The field must contain a non-negative number that may be a decimal.
        /// If the field is valid, the error string is unchanged.
        /// If it is invalid, an appropriate message is appended.
        /// 
        /// Validate if the entered number is beow the max tolerance for the field for that facility
        /// 
        /// </summary>
        /// <param name="field">the numeric field to check</param>
        /// <param name="error">the string to append error messages to</param>
        private void validateNumericField(TextBox field, ref string error)
        {
            decimal value;
            bool errorAlreadyFoundForThisField = false;
            // if the numeric field does not contain a number
            if (!Decimal.TryParse(field.Text, out value))
            {
                Label fieldLabel = getLabelFor(field);
                error += string.Format("Please enter a valid value for \"{0}.\"<br />", fieldLabel.Text.Replace(":", ""));
                fieldLabel.CssClass = "Error";
                errorAlreadyFoundForThisField = true;
            }

            // if the numeric field contains a negative number
            else if (value < 0)
            {
                Label fieldLabel = getLabelFor(field);
                error += string.Format("Please enter a non-negative value for \"{0}.\"<br />", fieldLabel.Text.Replace(":", ""));
                fieldLabel.CssClass = "Error";
                errorAlreadyFoundForThisField = true;
            }

            // Validate if the entered number is beow the max tolerance for the field for that facility
            if (!errorAlreadyFoundForThisField)
            {
                validateForMaxTolerance(field, ref error);
            }
        }

        private void validateOutageStatusField(DropDownList field, ref string error)
        {
            if (field.SelectedValue.Equals(""))
            {
                Label outageLabel = getLabelFor(field);
                error += string.Format("Please select \"{0}.\"<br />", outageLabel.Text);
                outageLabel.CssClass = "Error";
            }
        }

        private void validateDowntimeField(TextBox field, ref string error)
        {
            string status = ((DropDownList)Page.FindControl(field.ID.Replace("Downtime", "Status"))).SelectedValue;
            if (status.Equals("") || status.Equals("Operational") || status.Equals("N/A"))
                return;

            decimal value;
            if (!Decimal.TryParse(field.Text, out value))
            {
                Label downtimeLabel = getLabelFor(field);
                error += string.Format("Please enter a valid value for \"{0}.\"<br />", downtimeLabel.Text);
                downtimeLabel.CssClass = "Error";
            }
            else if (value <= 0)
            {
                Label downtimeLabel = getLabelFor(field);
                error += string.Format("Please enter a positive value for \"{0}.\"<br />", downtimeLabel.Text);
                downtimeLabel.CssClass = "Error";
            }
            else if (value > 24)
            {
                Label downtimeLabel = getLabelFor(field);
                error += string.Format("Please enter a value less than or equal to 24 for \"{0}.\"<br />", downtimeLabel.Text);
                downtimeLabel.CssClass = "Error";
            }
        }

        private void validateUnscheduledField(TextBox field, ref string error)
        {
            string status = ((DropDownList)Page.FindControl(field.ID.Replace("Unscheduled", "Status"))).SelectedValue;
            if (!status.Equals("Unscheduled Outage"))
            {
                if (!status.Equals(StandBy))
                    return;
            }

            if (!string.IsNullOrEmpty(field.Text.Trim()))
                return;

            Label unscheduledLabel = getLabelFor(field);
            error += string.Format("Please enter \"{0}.\"<br />", unscheduledLabel.Text);
            unscheduledLabel.CssClass = "Error";
        }

        private void validateHoursUnavailableField(TextBox hoursField, ref string error)
        {
            string available = ((RadioButtonList)Page.FindControl(hoursField.ID.Replace("HoursUnavailable", "Available"))).SelectedValue;
            if (!available.Equals("N"))
                return;

            decimal value;
            if (!Decimal.TryParse(hoursField.Text, out value))
            {
                Label hoursLabel = getLabelFor(hoursField);
                error += string.Format("Please enter a valid value for \"{0}.\"<br />", hoursLabel.Text);
                hoursLabel.CssClass = "Error";
            }
            else if (value <= 0)
            {
                Label hoursLabel = getLabelFor(hoursField);
                error += string.Format("Please enter a positive value for \"{0}.\"<br />", hoursLabel.Text);
                hoursLabel.CssClass = "Error";
            }
        }

        /// <summary>
        /// Validates the specified reason field. The field must be filled in if the corresponding "100% Available" selection is "No".
        /// If the field is valid, the error string is unchanged.
        /// If it is invalid, an appropriate message is appended.
        /// </summary>
        /// <param name="field">the reason field to check</param>
        /// <param name="error">the string to append error messages to</param>
        private void validateReasonField(TextBox reason, ref string error)
        {
            //if the reason field was filled in, it is valid
            if (!string.IsNullOrEmpty(reason.Text.Trim())) //if (!string.IsNullOrWhiteSpace(reason.Text))
                return;

            //if the "100% available" no option selected, corresponding reason field is required
            string available = ((RadioButtonList)Page.FindControl(reason.ID.Replace("Reason", "Available"))).SelectedValue;
            if (!available.Equals("N"))
                return;

            Label reasonLabel = getLabelFor(reason);
            error += string.Format("Please enter \"{0}.\"<br />", reasonLabel.Text);
            reasonLabel.CssClass = "Error";
        }

        /// <summary>
        /// Validates the specified date field. The field must contain a valid date that does not precede the reporting date.
        /// If the field is valid, the error string is unchanged.
        /// If it is invalid, an appropriate message is appended.
        /// </summary>
        /// <param name="field">the date field to check (not the reporting date)</param>
        /// <param name="error">the string to append error messages to</param>
        private void validateDateField(TextBox dateField, ref string error, ref string warning)
        {
            if (dateField.Equals(FireSystemDate))
            {
                if (FireSystemCheckBox.Checked)
                    return;
            }
            else if (dateField.Equals(Boiler1ExpectedRepairDate) || dateField.Equals(Boiler2ExpectedRepairDate) || dateField.Equals(Boiler3ExpectedRepairDate)
                || dateField.Equals(Boiler4ExpectedRepairDate) || dateField.Equals(Boiler5ExpectedRepairDate) || dateField.Equals(Boiler6ExpectedRepairDate)
                || dateField.Equals(Turbine1ExpectedRepairDate) || dateField.Equals(Turbine2ExpectedRepairDate) || dateField.Equals(CriticalAssetsDate) || dateField.Equals(CommentsDate))
            {
                // if (FireSystemCheckBox.Checked)
                return;
            }
            else
            {
                string available = ((RadioButtonList)Page.FindControl(dateField.ID.Replace("Date", "Available"))).SelectedValue;
                if (!available.Equals("N"))
                    return;
            }

            if (string.IsNullOrEmpty(dateField.Text.Trim())) //if (string.IsNullOrWhiteSpace(dateField.Text))
            {
                // output error
                Label dateLabel = getLabelFor(dateField);
                error += string.Format("Please enter a \"{0}.\"<br />", dateLabel.Text);
                dateLabel.CssClass = "Error";
                return;
            }

            // make sure that the date field contains (a) a legal date, and (b) a date that does not precede the reporting date
            DateTime input;
            if (!DateTime.TryParse(dateField.Text, out input))
            {
                Label dateLabel = getLabelFor(dateField);
                error += string.Format("Please enter a valid \"{0}.\"<br />", dateLabel.Text);
                dateLabel.CssClass = "Error";
            }

            // e.g., user entered 2/2, which is treated as February 2 of the current year (or next year, depending on reporting date) 
            // thus a warning is raised to have user check the date for accuracy
            else if (!input.ToShortDateString().Equals(dateField.Text))
            {
                DateTime reporting = DateTime.Parse(ReportingDate.Text);
                if (input < reporting)
                    input = input.AddYears(reporting.Year - input.Year + 1);
                dateField.Text = input.ToShortDateString();

                Label dateLabel = getLabelFor(dateField);
                dateLabel.CssClass = "Warning";

                warning += string.Format("Please confirm the \"{0}\" of {1}.<br />", dateLabel.Text, dateField.Text);
            }

            else if (input < DateTime.Parse(ReportingDate.Text))
            {
                Label dateLabel = getLabelFor(dateField);
                error += string.Format("Please enter a  \"{0}\" that does not precede the reporting date of {1}.<br />",
                    dateLabel.Text, ReportingDate.Text);
                dateLabel.CssClass = "Error";
            }
        }

        private void validateReprocessedField(RadioButtonList reprocessedField, ref string error)
        {
            string available = ((RadioButtonList)Page.FindControl(reprocessedField.ID.Replace("Reprocessed", "Available"))).SelectedValue;
            if (!available.Equals("N"))
                return;

            if (reprocessedField.SelectedValue.Equals(""))
            {
                Label reprocessedLabel = getLabelFor(reprocessedField);
                error += string.Format("Please make a selection for \"{0}\"<br />", reprocessedLabel.Text);
                reprocessedLabel.CssClass = "Error";
            }
        }

        /// <summary>
        /// Validates the specified yes/no field. One of the options must have been selected.
        /// If the field is valid, the error string is unchanged.
        /// If it is invalid, an appropriate message is appended.
        /// </summary>
        /// <param name="field">the yes/no field to check</param>
        /// <param name="error">the string to append error messages to</param>
        private void validateYesNoField(RadioButtonList yesNoField, ref string error)
        {
            if (yesNoField.SelectedIndex < 0)
            {
                Label yesNoLabel = getLabelFor(yesNoField);
                error += string.Format("Please make a selection for \"{0}\"<br />", yesNoLabel.Text);
                yesNoLabel.CssClass = "Error";
            }
        }

        private void validateEventTypeField(RadioButtonList eventTypeField, ref string error)
        {
            string wereEvents = ((RadioButtonList)Page.FindControl(eventTypeField.ID.Replace("EventType", "Events"))).SelectedValue;
            if (!wereEvents.Equals("True"))
                return;

            if (eventTypeField.SelectedValue.Equals(""))
            {
                Label eventTypeLabel = getLabelFor(eventTypeField);
                error += string.Format("Please make a selection for \"{0}\"<br />", eventTypeLabel.Text);
                eventTypeLabel.CssClass = "Error";
            }
        }

        private void validateEventExplanationField(TextBox eventExplanationField, ref string error)
        {
            string wereEvents = ((RadioButtonList)Page.FindControl(eventExplanationField.ID.Replace("Explanation", "Events"))).SelectedValue;
            if (!wereEvents.Equals("True"))
                return;

            if (string.IsNullOrEmpty(eventExplanationField.Text.Trim()))
            {
                Label eventExplanationLabel = getLabelFor(eventExplanationField);
                error += string.Format("Please enter \"{0}.\"<br />", eventExplanationLabel.Text);
                eventExplanationLabel.CssClass = "Error";
            }
        }

        /// <summary>
        /// Validates the specified optional field. It must either contain text or its "None" CheckBox must be checked.
        /// If the field is valid, the error string is unchanged.
        /// If it is invalid, an appropriate message is appended.
        /// </summary>
        /// <param name="field">the optional field to check</param>
        /// <param name="error">the string to append error messages to</param>
        private void validateOptionalField(TextBox optionalField, ref string error)
        {
            if (string.IsNullOrEmpty(optionalField.Text.Trim())) //if (string.IsNullOrWhiteSpace(optionalField.Text))
                if (!((CheckBox)Page.FindControl(optionalField.ID + "CheckBox")).Checked)
                {
                    Label optionalLabel = getLabelFor(optionalField);
                    error += string.Format("Please enter a value for \"{0}.\"<br />", optionalLabel.Text);
                    optionalLabel.CssClass = "Error";
                }

        }

        private void validatePSCodeTextBoxValidationField(TextBox psField, ref string error)
        {
            if (psField.Text.Trim().ToUpper() != Facility.SelectedValue.ToUpper())
            {
                Label psValidationLabel = getLabelFor(psField);
                error += string.Format("Please enter correct value for \"{0}.\"<br />", psValidationLabel.Text);
                psValidationLabel.CssClass = "Error";
            }

        }

        /// <summary>
        /// Returns the corresponding label for the specified control.
        /// </summary>
        /// <param name="c">the control whose label should be retrieved</param>
        private Label getLabelFor(Control c)
        {
            return (Label)Page.FindControl(c.ID + "Label");
        }

        /// <summary>
        /// Clears the contents of the specified control, if it is a TextBox, RadioButtonList, or CheckBox.
        /// </summary>
        /// <param name="c">the control to clear</param>
        private void clearContents(Control c)
        {
            if (c is TextBox)
                ((TextBox)c).Text = "";

            else if (c is DropDownList)
                ((DropDownList)c).ClearSelection();

            else if (c is RadioButtonList)
                ((RadioButtonList)c).ClearSelection();

            else if (c is CheckBox)
                ((CheckBox)c).Checked = false;
        }

        private void validateForMaxTolerance(TextBox field, ref string error)
        {
            //Tolerance Multiplier
            toleranceMultiplier = 2.0M;  //default
            string tm = ConfigurationManager.AppSettings["maxToleranceMultiplier"];
            if (isDecimal(tm)) { toleranceMultiplier = decimal.Parse(tm); }

            //TonsDelivered
            if (field.ID == "TonsDelivered")
            {
                TonsDeliveredMessage.Text = string.Empty;
                if (!isValueBelowMaxTolerance(field))
                {
                    DailyOpsBusinessUnit facility = getBusinessUnitForToleranceCheck(this.Facility.SelectedValue);
                    decimal maxValue = facility.MaxTonsDelivered * toleranceMultiplier;
                    int maxValueInt = (int)(Math.Floor(maxValue));
                    TonsDeliveredMessage.Text = string.Format("The value entered ({0}) is too high or invalid.  Please enter a value under {1} ", field.Text, maxValueInt.ToString());
                    Label fieldLabel = getLabelFor(field);
                    error += string.Format("Please change the value for \"{0}.\"<br />", fieldLabel.Text.Replace(":", ""));
                    fieldLabel.CssClass = "Error";
                }
            }

            //TonsProcessed
            if (field.ID == "TonsProcessed")
            {
                TonsProcessedMessage.Text = string.Empty;
                if (!isValueBelowMaxTolerance(field))
                {
                    DailyOpsBusinessUnit facility = getBusinessUnitForToleranceCheck(this.Facility.SelectedValue);
                    decimal maxValue = facility.MaxTonsProcessed * toleranceMultiplier;
                    int maxValueInt = (int)(Math.Floor(maxValue));
                    TonsProcessedMessage.Text = string.Format("The value entered ({0}) is too high or invalid.  Please enter a value under {1} ", field.Text, maxValueInt.ToString());
                    Label fieldLabel = getLabelFor(field);
                    error += string.Format("Please change the value for \"{0}.\"<br />", fieldLabel.Text.Replace(":", ""));
                    fieldLabel.CssClass = "Error";
                }
            }

            //SteamProduced
            if (field.ID == "SteamProduced")
            {
                SteamProducedMessage.Text = string.Empty;
                if (!isValueBelowMaxTolerance(field))
                {
                    DailyOpsBusinessUnit facility = getBusinessUnitForToleranceCheck(this.Facility.SelectedValue);
                    decimal maxValue = facility.MaxSteamProduced * toleranceMultiplier;
                    int maxValueInt = (int)(Math.Floor(maxValue));
                    SteamProducedMessage.Text = string.Format("The value entered ({0}) is too high or invalid.  Please enter a value under {1} ", field.Text, maxValueInt.ToString());
                    Label fieldLabel = getLabelFor(field);
                    error += string.Format("Please change the value for \"{0}.\"<br />", fieldLabel.Text.Replace(":", ""));
                    fieldLabel.CssClass = "Error";
                }
            }

            //SteamSold
            if (field.ID == "SteamSold")
            {
                SteamSoldMessage.Text = string.Empty;
                if (!isValueBelowMaxTolerance(field))
                {
                    DailyOpsBusinessUnit facility = getBusinessUnitForToleranceCheck(this.Facility.SelectedValue);
                    decimal maxValue = facility.MaxSteamSold * toleranceMultiplier;
                    int maxValueInt = (int)(Math.Floor(maxValue));
                    SteamSoldMessage.Text = string.Format("The value entered ({0}) is too high or invalid.  Please enter a value under {1} ", field.Text, maxValueInt.ToString());
                    Label fieldLabel = getLabelFor(field);
                    error += string.Format("Please change the value for \"{0}.\"<br />", fieldLabel.Text.Replace(":", ""));
                    fieldLabel.CssClass = "Error";
                }
            }

            //NetElectric
            if (field.ID == "NetElectric")
            {
                NetElectricMessage.Text = string.Empty;
                if (!isValueBelowMaxTolerance(field))
                {
                    DailyOpsBusinessUnit facility = getBusinessUnitForToleranceCheck(this.Facility.SelectedValue);
                    decimal maxValue = facility.MaxNetElectric * toleranceMultiplier;
                    int maxValueInt = (int)(Math.Floor(maxValue));
                    NetElectricMessage.Text = string.Format("The value entered ({0}) is too high or invalid.  Please enter a value under {1} ", field.Text, maxValueInt.ToString());
                    Label fieldLabel = getLabelFor(field);
                    error += string.Format("Please change the value for \"{0}.\"<br />", fieldLabel.Text.Replace(":", ""));
                    fieldLabel.CssClass = "Error";
                }
            }
        }



        /// <summary>
        /// Checks to see if the value entered for a field is within tolerances described in the business unit master
        /// </summary>
        /// <param name="fieldBeingChecked">Field being checked for tolerances</param>
        /// <returns>True if number is within tolerances, false if number is too high.</returns>
        private bool isValueBelowMaxTolerance(TextBox fieldBeingChecked)
        {
            DailyOpsBusinessUnit facility = getBusinessUnitForToleranceCheck(this.Facility.SelectedValue);

            bool valueToReturn = false;
            switch (fieldBeingChecked.ID)
            {
                //return true if all is ok
                case "TonsDelivered":
                    if ((isDecimal(fieldBeingChecked.Text)) && (facility.MaxTonsDelivered * toleranceMultiplier > decimal.Parse(fieldBeingChecked.Text)))
                    {
                        valueToReturn = true;
                    }
                    break;
                case "TonsProcessed":
                    if ((isDecimal(TonsProcessed.Text)) && (facility.MaxTonsProcessed * toleranceMultiplier > decimal.Parse(TonsProcessed.Text)))
                    {
                        valueToReturn = true;
                    }
                    break;
                case "SteamProduced":
                    if ((isDecimal(SteamProduced.Text)) && (facility.MaxSteamProduced * toleranceMultiplier > decimal.Parse(SteamProduced.Text)))
                    {
                        valueToReturn = true;
                    }
                    break;
                case "SteamSold":
                    if ((isDecimal(SteamSold.Text)) && (facility.MaxSteamSold * toleranceMultiplier > decimal.Parse(SteamSold.Text)))
                    {
                        valueToReturn = true;
                    }
                    break;
                case "NetElectric":
                    if ((isDecimal(NetElectric.Text)) && (facility.MaxNetElectric * toleranceMultiplier > decimal.Parse(NetElectric.Text)))
                    {
                        valueToReturn = true;
                    }
                    break;
                default:
                    valueToReturn = false;
                    break;

            }
            return valueToReturn;
        }

        /// <summary>
        /// returns true if the passed in value is an int
        /// </summary>
        /// <param name="p">value to be tested</param>
        /// <returns>true of false</returns>
        private bool isInt(string p)
        {
            bool returnValue = true;
            int value = 0;
            if (!int.TryParse(p, out value))
            {
                returnValue = false;
            }
            return returnValue;
        }

        /// <summary>
        /// returns true if the passed in value is a decimal
        /// </summary>
        /// <param name="p">value to be tested</param>
        /// <returns>true of false</returns>
        private bool isDecimal(string p)
        {
            bool returnValue = true;
            decimal value = 0;
            if (!decimal.TryParse(p, out value))
            {
                returnValue = false;
            }
            return returnValue;
        }

        /// <summary>
        /// Returns a business unit object based on the parameters.
        /// Will be used in the tolerance check.
        /// If we already went to the database for this value, then we do not return to the database
        /// </summary>
        /// <param name="facility">facilty to return an object for</param>
        /// <returns>a daily ops busines unit object</returns>
        private DailyOpsBusinessUnit getBusinessUnitForToleranceCheck(string facility)
        {
            if (facilityList.Count == 0)
            {
                facilityList = busUnitManager.GetBusinessUnitsList();
            }
            DailyOpsBusinessUnit businessUnit = facilityList.Find(xx => xx.PS_Unit == facility);
            return businessUnit;
        }

        #endregion

        #region private static methods

        /// <summary>
        /// Returns the DailyOpsBusinessUnit with the specified description using a linear search.
        /// </summary>
        /// <param name="Description">the name of the facility</param>
        /// <returns>the DailyOpsBusinessUnit with that description</returns>
        private static DailyOpsBusinessUnit getSelectedFacility(string Description)
        {
            List<DailyOpsBusinessUnit> units = busUnitManager.GetBusinessUnitsList();
            DailyOpsBusinessUnit selected = null;

            foreach (DailyOpsBusinessUnit BU in units)
                if (BU.Description.Equals(Description))
                {
                    selected = BU;
                    break;
                }

            return selected;
        }


        /// <summary>
        /// Returns true if c is a date field, false if not.
        /// A control is a date field if (a) it is a TextBox, and (b) it's CSS class is DateField.
        /// </summary>
        /// <param name="c">the control to check</param>
        /// <returns>whether the control is a date field</returns>
        private static bool isDateField(Control c)
        {
            return c is TextBox && ((TextBox)c).CssClass.Equals("DateField");
        }

        /// <summary>
        /// Returns true if c is a hidden field, false if not.
        /// A control is a hidden field if it is an object of type HiddenField, but is not the LastReportingDate.
        /// </summary>
        /// <param name="c">the control to check</param>
        /// <returns>whether the control is a hidden field</returns>
        private static bool isHiddenField(Control c)
        {
            return c is HiddenField && !c.ID.Equals("LastReportingDate");
        }

        /// <summary>
        /// Returns true if c is an input calendar, false if not.
        /// A control is an input calendar if (a) it is a CalendarExtender, and (b) it is not the ReportingDateCalendar.
        /// </summary>
        /// <param name="c">the control to check</param>
        /// <returns>whether the control is an input calendar</returns>
        private static bool isInputCalendar(Control c)
        {
            return c is AjaxControlToolkit.CalendarExtender && !((AjaxControlToolkit.CalendarExtender)c).ID.Equals("ReportingDateCalendar");
        }

        /// <summary>
        /// Returns true if the control is a numeric field, false if not.
        /// A control is a numeric field if (a) it is a TextBox, and (b) it's CSS class is NumericField.
        /// </summary>
        /// <param name="c">the control to check</param>
        /// <returns>whether the control is a number field</returns>
        private static bool isNumericField(Control c)
        {
            return c is TextBox && ((TextBox)c).CssClass.Equals("NumericField");
        }

        private static bool isOutageStatusField(Control c)
        {
            return c is DropDownList && ((DropDownList)c).CssClass.Equals("OutageStatusField");
        }

        private static bool isDowntimeField(Control c)
        {
            return c is TextBox && ((TextBox)c).CssClass.Equals("DowntimeField");
        }

        private static bool isUnscheduledField(Control c)
        {
            return c is TextBox && ((TextBox)c).CssClass.Equals("UnscheduledField");
        }

        private static bool isHoursUnavailableField(Control c)
        {
            return c is TextBox && ((TextBox)c).CssClass.Equals("HoursUnavailableField");
        }

        /// <summary>
        /// Returns true if the control is a reason field, false if not.
        /// A control is a reason field if (a) it is a TextBox, and (b) it's CSS class is ReasonField.
        /// </summary>
        /// <param name="c">the control to check</param>
        /// <returns>whether the control is a number field</returns>
        private static bool isReasonField(Control c)
        {
            return c is TextBox && ((TextBox)c).CssClass.Equals("ReasonField");
        }

        private static bool isReprocessedField(Control c)
        {
            return c is RadioButtonList && ((RadioButtonList)c).CssClass.Equals("ReprocessedField");
        }

        /// <summary>
        /// Returns true if the control is a yes/no field, false if not.
        /// A control is a yes/no field if (a) it is a RadioButtonList, and (b) it's CSS class is YesNoField.
        /// </summary>
        /// <param name="c">the control to check</param>
        /// <returns>whether the control is a number field</returns>
        private static bool isYesNoField(Control c)
        {
            return c is RadioButtonList && ((RadioButtonList)c).CssClass.Equals("YesNoField");
        }

        private static bool isEventTypeField(Control c)
        {
            return c is RadioButtonList && ((RadioButtonList)c).CssClass.Equals("EventTypeField");
        }

        private static bool isEventExplanationField(Control c)
        {
            return c is TextBox && ((TextBox)c).CssClass.Equals("EventExplanationField");
        }

        /// <summary>
        /// Returns true if the control is an optional field (meaning there is a "None" checkbox).
        /// A control is an optional field if (a) it is a TextBox, and (b) it's CSS class is OptionalField.
        /// </summary>
        /// <param name="c">the control to check</param>
        /// <returns>whether the control is an optional field</returns>
        private static bool isOptionalField(Control c)
        {
            return c is TextBox && ((TextBox)c).CssClass.Equals("OptionalField");
        }

        private static bool isPSCodeTextBoxValidationField(Control c)
        {
            return c is TextBox && ((TextBox)c).CssClass.Equals("PSCodeTextBoxValidationField");
        }
        /// <summary>
        /// Returns true if the control can be cleared without issue, false if not.
        /// </summary>
        /// <param name="c">the control to check</param>
        /// <returns>whether the control is clearable</returns>
        private static bool isClearableField(Control c)
        {
            return (c is DropDownList && !c.ID.Equals("Facility")) || (c is TextBox && !c.ID.Equals("ReportingDate") & !c.ID.Equals("UserName") & !c.ID.Equals("PeopleSoftCode")) ||
                (c is RadioButtonList) || (c is CheckBox);
        }

        /// <summary>
        /// Returns the decimal value of the specified TextBox.
        /// If the field does not contain a decimal number, or is not visible, returns 0.
        /// </summary>
        /// <param name="field">the numeric field whose value is to be retrieved</param>
        /// <returns>the decimal value of the specified TextBox</returns>
        private static decimal getNumericFieldValue(TextBox field)
        {
            if (!field.Visible)
                return 0;

            decimal value;
            return Decimal.TryParse(field.Text, out value) ? value : 0;
        }

        /// <summary>
        /// Returns the date value of the specified TextBox.
        /// If the field does not contain a date number, or is not visible, returns 1/1/1900.
        /// </summary>
        /// <param name="field">the date field whose value is to be retrieved</param>
        /// <returns>the date value of the specified TextBox</returns>
        private static DateTime getDateFieldValue(TextBox field)
        {
            DateTime input;
            return (DateTime.TryParse(field.Text, out input)) ? input : MIN_DATE;
        }

        /// <summary>
        /// Returns the value of the specified optional field.
        /// If the "None" checkbox is checked, return the empty string.
        /// </summary>
        /// <param name="field">the optional field whose value is to be retrieved</param>
        /// <returns>the value of the specified optional field</returns>
        private static string getOptionalFieldValue(TextBox field, CheckBox checkbox)
        {
            return (checkbox.Checked) ? "" : field.Text;
        }



        #endregion

        protected void Turbine1Unscheduled_TextChanged(object sender, EventArgs e)
        {

        }




    }
}