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
using System.Data.SqlClient;
using System.Data;

namespace Covanta.UI.DailyOpsInputForm
{
    public partial class DailyOpsMassUpdate2 : System.Web.UI.Page
    {
        private static string covmetadataConnString = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
        private static DailyOpsBusiUnitManager busUnitManager = new DailyOpsBusiUnitManager(covmetadataConnString);
        private static DailyOpsManager dailyDataManager = new DailyOpsManager(covmetadataConnString);
        private static DateTime MIN_DATE = DateTime.Parse("1/1/1900");
        private static List<DailyOpsBusinessUnit> facilityList = new List<DailyOpsBusinessUnit>();
        private static decimal toleranceMultiplier; // this is set in web.config 
        public List<DailyOpsDataWithID> dailyopsDataWithIDList = new List<DailyOpsDataWithID>();
        public const string StandBy = "Stand-By";
        public const string StandByBoilerInOutage = "Stand-By (Boilers in Outage)";
        public const string StandBySellingSteam = "Stand-By (Selling Steam)";
        public const string UnscheduledTGOutage = "Unscheduled TG Outage";
        public const string UnscheduledBoilerInOutage = "Unscheduled Outage (Bolier in Outage)";
        public const string UnscheduledSellingSteam = "Unscheduled Outage (Selling Steam)";
        public const string Other = "Other";
        public const string Operational = "Operational";
        public const string UnscheduledOutage = "Unscheduled Outage";

        SqlConnection con;
        bool[] rowChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(covmetadataConnString);
            int totalRows = DailyOpsRecordsGridView.Rows.Count;
            rowChanged = new bool[totalRows];
            if (!Page.IsPostBack)
            {
                LoadFormInitial();
            }


        }

        //private void InitializeForm()
        //{

        //    string facilityCode = Request.QueryString["Facility"];
        //    if (facilityCode != null)
        //        facilityCode = facilityCode.ToUpper();
        //    else facilityCode = "ESSEX"; //set default facility code

        //    //set default reporting date
        //    string reportDate = Request.QueryString["Date"];
        //    DateTime reporting;
        //    if (!DateTime.TryParse(reportDate, out reporting))
        //        reporting = DateTime.Now.AddDays(-1);
        //    ReportingDate.Text = reporting.ToShortDateString();

        //    // bind list of facilities from database to the Facility Drop Down List
        //    List<DailyOpsBusinessUnit> list = busUnitManager.GetBusinessUnitsList();
        //    Facility.DataSource = list;
        //    Facility.DataTextField = "Description";
        //    Facility.DataValueField = "PS_Unit";
        //    Facility.DataBind();
        //    Facility.SelectedValue = facilityCode;

        //    Enums.StatusEnum status = Enums.StatusEnum.NotSet;
        //    //EnvironmentLabel.Text = dailyDataManager.GetDailyOpsDataByID(86, ref status).ID.ToString();
        //    dailyopsDataWithIDList = dailyDataManager.GetActiveDailyOpsDataWithIDByFacility(facilityCode, ref status);

        //    if (dailyopsDataWithIDList.Count > 0)
        //    {
        //        BindGridViewWithList2(dailyopsDataWithIDList);
        //    }
        //}

        private void BindGridViewWithList(List<DailyOpsData> dailyopsDataList)
        {
            //DailyOpsRecordsGridView.DataSource = null;
            DailyOpsRecordsGridView.DataSource = dailyopsDataList;
            DailyOpsRecordsGridView.DataBind();
        }

        private void BindGridViewWithList2(List<DailyOpsDataWithID> dailyopsDataList)
        {
            //DailyOpsRecordsGridView.DataSource = null;
            DailyOpsRecordsGridView.DataSource = dailyopsDataList;
            DailyOpsRecordsGridView.DataBind();
        }
        protected void Facility_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblActiveNumber.Text = "";
            lblMessage.Text = "";
            ReportingDate.Enabled = true;
            LoadForm();
            ReportingDate.Text = "";
        }

        protected void ReportingDate_TextChanged(object sender, EventArgs e)
        {
            lblActiveNumber.Text = "";
            lblMessage.Text = "";
            LoadFormAfterDateChange();
        }

        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            rowChanged[row] = true;
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/DailyOps.aspx");
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblActiveNumber.Text = "";
            lblMessage.Text = "";
            string facilityCode = Facility.SelectedValue;//Request.QueryString["Facility"];
            if (facilityCode != null)
                facilityCode = facilityCode.ToUpper();

            string reportDate = ReportingDate.Text;

            Enums.StatusEnum status = Enums.StatusEnum.NotSet;
            string statusMsg = string.Empty;
            dailyopsDataWithIDList = dailyDataManager.GetActiveDailyOpsDataWithIDByFacility(facilityCode, ref status);

            if (Page.IsPostBack)
            {
                int totalRows = DailyOpsRecordsGridView.Rows.Count;
                for (int r = 0; r < totalRows; r++)
                {
                    if (rowChanged[r])
                    {
                        GridViewRow thisGridViewRow = DailyOpsRecordsGridView.Rows[r];
                        HiddenField hf1 = (HiddenField)thisGridViewRow.FindControl("HiddenField1");
                        string pk = hf1.Value;

                        TextBox tbTonsDelivered = (TextBox)thisGridViewRow.FindControl("tb_TonsDelivered");
                        decimal TonsDelivered = Convert.ToDecimal(tbTonsDelivered.Text);
                        TextBox tbTonsProcessed = (TextBox)thisGridViewRow.FindControl("tb_TonsProcessed");
                        decimal TonsProcessed = Convert.ToDecimal(tbTonsProcessed.Text);

                        TextBox tbSteamProduced = (TextBox)thisGridViewRow.FindControl("tb_SteamProduced");
                        decimal SteamProduced = Convert.ToDecimal(tbSteamProduced.Text);

                        TextBox tbSteamSold = (TextBox)thisGridViewRow.FindControl("tb_SteamSold");
                        decimal SteamSold = Convert.ToDecimal(tbSteamSold.Text);

                        TextBox tbNetElectric = (TextBox)thisGridViewRow.FindControl("tb_NetElectric");
                        decimal NetElectric = Convert.ToDecimal(tbNetElectric.Text);

                        TextBox tbPitInventory = (TextBox)thisGridViewRow.FindControl("tb_PitInventory");
                        decimal PitInventory = Convert.ToDecimal(tbPitInventory.Text);

                        TextBox tbOutageTypeBoiler1 = (TextBox)thisGridViewRow.FindControl("tb_OutageTypeBoiler1");
                        string OutageTypeBoiler1 = tbOutageTypeBoiler1.Text;
                        TextBox tbDownTimeBoiler1 = (TextBox)thisGridViewRow.FindControl("tb_DownTimeBoiler1");
                        decimal DownTimeBoiler1 = Convert.ToDecimal(tbDownTimeBoiler1.Text);

                        TextBox tbOutageTypeBoiler2 = (TextBox)thisGridViewRow.FindControl("tb_OutageTypeBoiler2");
                        string OutageTypeBoiler2 = tbOutageTypeBoiler2.Text;
                        TextBox tbDownTimeBoiler2 = (TextBox)tbOutageTypeBoiler2.FindControl("tb_DownTimeBoiler2");
                        decimal DownTimeBoiler2 = Convert.ToDecimal(tbDownTimeBoiler2.Text);

                        TextBox tbOutageTypeBoiler3 = (TextBox)thisGridViewRow.FindControl("tb_OutageTypeBoiler3");
                        string OutageTypeBoiler3 = tbOutageTypeBoiler3.Text;
                        TextBox tbDownTimeBoiler3 = (TextBox)thisGridViewRow.FindControl("tb_DownTimeBoiler3");
                        decimal DownTimeBoiler3 = Convert.ToDecimal(tbDownTimeBoiler3.Text);

                        TextBox tbOutageTypeBoiler4 = (TextBox)thisGridViewRow.FindControl("tb_OutageTypeBoiler4");
                        string OutageTypeBoiler4 = tbOutageTypeBoiler4.Text;
                        TextBox tbDownTimeBoiler4 = (TextBox)thisGridViewRow.FindControl("tb_DownTimeBoiler4");
                        decimal DownTimeBoiler4 = Convert.ToDecimal(tbDownTimeBoiler4.Text);

                        TextBox tbOutageTypeBoiler5 = (TextBox)thisGridViewRow.FindControl("tb_OutageTypeBoiler5");
                        string OutageTypeBoiler5 = tbOutageTypeBoiler5.Text;
                        TextBox tbDownTimeBoiler5 = (TextBox)thisGridViewRow.FindControl("tb_DownTimeBoiler5");
                        decimal DownTimeBoiler5 = Convert.ToDecimal(tbDownTimeBoiler5.Text);

                        TextBox tbOutageTypeBoiler6 = (TextBox)thisGridViewRow.FindControl("tb_OutageTypeBoiler6");
                        string OutageTypeBoiler6 = tbOutageTypeBoiler6.Text;
                        TextBox tbDownTimeBoiler6 = (TextBox)thisGridViewRow.FindControl("tb_DownTimeBoiler6");
                        decimal DownTimeBoiler6 = Convert.ToDecimal(tbDownTimeBoiler6.Text);

                        TextBox tbOutageTypeTurbGen1 = (TextBox)thisGridViewRow.FindControl("tb_OutageTypeTurbGen1");
                        string OutageTypeTurbGen1 = tbOutageTypeTurbGen1.Text;
                        TextBox tbDownTimeTurbGen1 = (TextBox)thisGridViewRow.FindControl("tb_DownTimeTurbGen1");
                        decimal DownTimeTurbGen1 = Convert.ToDecimal(tbDownTimeTurbGen1.Text);

                        TextBox tbOutageTypeTurbGen2 = (TextBox)thisGridViewRow.FindControl("tb_OutageTypeTurbGen2");
                        string OutageTypeTurbGen2 = tbOutageTypeTurbGen2.Text;
                        TextBox tbDownTimeTurbGen2 = (TextBox)thisGridViewRow.FindControl("tb_DownTimeTurbGen2");
                        decimal DownTimeTurbGen2 = Convert.ToDecimal(tbDownTimeTurbGen2.Text);

                        TextBox tbFerrousTons = (TextBox)thisGridViewRow.FindControl("tb_FerrousTons");
                        decimal FerrousTons = Convert.ToDecimal(tbFerrousTons.Text);

                        TextBox tbNonFerrousTons = (TextBox)thisGridViewRow.FindControl("tb_NonFerrousTons");
                        decimal NonFerrousTons = Convert.ToDecimal(tbNonFerrousTons.Text);

                        DailyOpsDataWithID dailyOpsDataWithID = dailyopsDataWithIDList.Where(el => el.ID == int.Parse(pk)).First();

                        //Edit comment out the below statement temperarily.
                        dailyOpsDataWithID.UserLastModified = Request.ServerVariables["LOGON_USER"].ToString().Substring(4); //this is used in production
                        //dailyOpsDataWithID.UserLastModified = "EChen";
                        dailyOpsDataWithID.TonsDelivered = TonsDelivered;
                        dailyOpsDataWithID.TonsProcessed = TonsProcessed;
                        dailyOpsDataWithID.SteamProduced = SteamProduced;
                        dailyOpsDataWithID.SteamSold = SteamSold;
                        dailyOpsDataWithID.NetElectric = NetElectric;
                        dailyOpsDataWithID.PitInventory = PitInventory;
                        dailyOpsDataWithID.DownTimeBoiler1 = DownTimeBoiler1;
                        dailyOpsDataWithID.DownTimeBoiler2 = DownTimeBoiler2;
                        dailyOpsDataWithID.DownTimeBoiler3 = DownTimeBoiler3;
                        dailyOpsDataWithID.DownTimeBoiler4 = DownTimeBoiler4;
                        dailyOpsDataWithID.DownTimeBoiler5 = DownTimeBoiler5;
                        dailyOpsDataWithID.DownTimeBoiler6 = DownTimeBoiler6;
                        dailyOpsDataWithID.DownTimeTurbGen1 = DownTimeTurbGen1;
                        dailyOpsDataWithID.DownTimeTurbGen2 = DownTimeTurbGen2;
                        dailyOpsDataWithID.FerrousTons = FerrousTons;
                        dailyOpsDataWithID.NonFerrousTons = NonFerrousTons;
                        dailyDataManager.UpdateDailyOpsDataWithID(dailyOpsDataWithID, ref status, ref statusMsg);
                        lblMessage.Text = statusMsg + " ( Update Time: " + System.DateTime.Now.ToString() + " )";
                    }
                }

                if (reportDate.Length != 0)
                    LoadFormForFacilityAtDate(facilityCode, reportDate);
                else
                    LoadForm();
            }
        }

        private void LoadForm()
        {

            string facilityCode = Facility.SelectedValue;
            if (facilityCode != null)
                facilityCode = facilityCode.ToUpper();
            else
                facilityCode = "Essex";


            string reportDate = ReportingDate.Text;

            DateTime reporting;
            if (!DateTime.TryParse(reportDate, out reporting))
                reporting = DateTime.Now.AddDays(-1);
            ReportingDate.Text = reporting.ToShortDateString();

            Enums.StatusEnum status = Enums.StatusEnum.NotSet;
            List<DailyOpsDataWithID> dailyopsDataWithIDList = dailyDataManager.GetActiveDailyOpsDataWithIDByFacility(facilityCode, ref status);

            if (dailyopsDataWithIDList.Count != 0)
            {
                ReportingDateCalendar.StartDate = dailyopsDataWithIDList.Select(e => e.ReportingDate).Min();
                ReportingDateCalendar.EndDate = dailyopsDataWithIDList.Select(e => e.ReportingDate).Max();
            }
            else
            {
                ReportingDate.Text = "";
                ReportingDate.Enabled = false;
            }
            BindGridViewWithList2(dailyopsDataWithIDList);
            lblActiveNumber.Text = "[Number of Active Record(s): " + dailyopsDataWithIDList.Count + "]";

        }

        private void LoadFormInitial()
        {
            string facilityCode = Request.QueryString["Facility"];
            if (facilityCode != null)
                facilityCode = facilityCode.ToUpper();
            else facilityCode = "NA"; //set default facility code

            // bind list of facilities from database to the Facility Drop Down List
            List<DailyOpsBusinessUnit> list = busUnitManager.GetBusinessUnitsList();
            //list.Remove(list.First(i => i.Description == "Wallingford"));
            //list.Remove(list.First(i => i.Description == "Hudson Valley"));
            list.Insert(0, new DailyOpsBusinessUnit("NA", "Select a facility", 0, false));
            Facility.DataSource = list;
            Facility.DataTextField = "Description";
            Facility.DataValueField = "PS_Unit";
            Facility.DataBind();
            Facility.SelectedValue = facilityCode;
            ReportingDate.Enabled = false;
        }

        private void LoadFormAfterDateChange()
        {

            string facilityCode = Facility.SelectedValue;
            if (facilityCode != null)
                facilityCode = facilityCode.ToUpper();


            string reportDate = ReportingDate.Text;
            var user_time = DateTime.Parse(reportDate);

            DateTime reporting;
            if (!DateTime.TryParse(reportDate, out reporting))
                reporting = DateTime.Now.AddDays(-1);

            Enums.StatusEnum status = Enums.StatusEnum.NotSet;


            List<DailyOpsDataWithID> dailyopsDataWithIDList = dailyDataManager.GetActiveDailyOpsDataWithIDByFacility(facilityCode, ref status);

            //ReportingDateCalendar.StartDate = dailyopsDataWithIDList.Select(e => e.ReportingDate).Min();
            //ReportingDateCalendar.EndDate = dailyopsDataWithIDList.Select(e => e.ReportingDate).Max();
            List<DailyOpsDataWithID> dailyopsDataWithIDList2 = dailyopsDataWithIDList.Where(e => e.ReportingDate.ToString("yyyy-MM-dd").Contains(reportDate.ToString())).ToList();

            if (dailyopsDataWithIDList.Count > 0)
            {
                BindGridViewWithList2(dailyopsDataWithIDList2);

            }
        }

        private void LoadFormForFacilityAtDate(string facilityCode, string reportDate)
        {

            var user_time = DateTime.Parse(reportDate);

            DateTime reporting;
            if (!DateTime.TryParse(reportDate, out reporting))
                reporting = DateTime.Now.AddDays(-1);

            Enums.StatusEnum status = Enums.StatusEnum.NotSet;
            List<DailyOpsDataWithID> dailyopsDataWithIDList = dailyDataManager.GetActiveDailyOpsDataWithIDByFacility(facilityCode, ref status);
            ReportingDateCalendar.StartDate = dailyopsDataWithIDList.Select(e => e.ReportingDate).Min();
            ReportingDateCalendar.EndDate = dailyopsDataWithIDList.Select(e => e.ReportingDate).Max();
            List<DailyOpsDataWithID> dailyopsDataWithIDList2 = dailyopsDataWithIDList.Where(e => e.ReportingDate.ToString("yyyy-MM-dd").Contains(reportDate.ToString())).ToList();

            if (dailyopsDataWithIDList2.Count > 0)
            {
                BindGridViewWithList2(dailyopsDataWithIDList2);

            }
        }

        protected void bt_ChooseAll_Click(object sender, EventArgs e)
        {
            LoadForm();
            ReportingDate.Text = "";
            lblMessage.Text = "";
        }

    }
}