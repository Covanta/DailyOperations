using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Covanta.BusinessLogic;
using Covanta.Entities;

namespace Covanta.UI.DailyOpsInputForm
{
	public partial class ExceptionsReport : System.Web.UI.Page
	{
		#region private static fields
		
		private static string covmetadataConnString = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
		private static DailyOpsBusiUnitManager busUnitManager = new DailyOpsBusiUnitManager(covmetadataConnString);

		#endregion

		#region protected methods

		/// <summary>
		/// Initializes the page when it is first loaded.
		/// Does nothing on subsequent postbacks.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				updateRegionList();
				updateFacilityList();
				// default report date to yesterday
				DateTime yesterday = DateTime.Now.AddDays(-1);
				ReportDate.Text = yesterday.ToShortDateString();
				LastReportDate.Value = ReportDate.Text;
				updateDataTables();
			}
		}

		protected void InputFormButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/DailyOps.aspx");
		}

		protected void SummaryReportButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/SummaryReport.aspx");
		}

		protected void RefreshTimer_Tick(object sender, EventArgs e)
		{
			updateDataTables();
		}

		/// <summary>
		/// Changes which facilities appear in the facility dropdown list depending
		/// on which region the user has selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RegionList_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateFacilityList();
			updateDataTables();
		}

		/// <summary>
		/// Updates the tables based upon which facility the user has selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void FacilityList_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateDataTables();
		}

		/// <summary>
		/// Makes sure the user entered a valid date.
		/// If not, the date is reverted to its previous value.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ReportDate_TextChanged(object sender, EventArgs e)
		{
			if (updateReportDate())
				updateDataTables();
		}

		#endregion

		#region private methods

		/// <summary>
		/// Makes sure the report date is a valid date.
		/// If it is not a date, reverts the report date to its previous value.
		/// </summary>
		/// <returns>whether the report date was truly changed</returns>
		private bool updateReportDate()
		{
			DateTime report;
			if (!DateTime.TryParse(ReportDate.Text, out report))
			{
				ReportDate.Text = LastReportDate.Value;
				return false;
			}

			if (report.Equals(DateTime.Parse(LastReportDate.Value)))
			{
				ReportDate.Text = LastReportDate.Value;
				return false;
			}

			ReportDate.Text = report.ToShortDateString();
			LastReportDate.Value = ReportDate.Text;
			return true;
		}

		/// <summary>
		/// Binds the list of regions to the region dropdown list.
		/// </summary>
		private void updateRegionList()
		{
			List<string> regions = busUnitManager.GetDistinctRegionsList();
			RegionList.DataSource = regions;
			RegionList.DataBind();
		}

		/// <summary>
		/// Binds the list of facilities to the facility dropdown list.
		/// Only facilities in the currently selected region will be added.
		/// </summary>
		private void updateFacilityList()
		{
			List<DailyOpsBusinessUnit> facilities = busUnitManager.GetDailyOpsBusinessUnitsListByRegion(RegionList.SelectedValue);
			facilities.Insert(0, new DailyOpsBusinessUnit("", "All Facilities", 0, false));
			FacilityList.DataSource = facilities;
			FacilityList.DataTextField = "Description";
			FacilityList.DataValueField = "PS_Unit";
			FacilityList.DataBind();
		}

		private void updateDataTables()
		{
			ExceptionsReportData.Region = RegionList.SelectedValue;
			ExceptionsReportData.FacilityID = FacilityList.SelectedValue;
			ExceptionsReportData.ReportDate = DateTime.Parse(ReportDate.Text);
			ExceptionsReportData.Update();
		}

		#endregion
	}
}