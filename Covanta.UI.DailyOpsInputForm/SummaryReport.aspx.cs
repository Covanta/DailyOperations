using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Covanta.BusinessLogic;
using Covanta.Entities;
using System.Configuration;

namespace Covanta.UI.DailyOpsInputForm
{
	public partial class SummaryReport : System.Web.UI.Page
	{
		private static string covmetadataConnString = ConfigurationManager.ConnectionStrings["covmetadataConnString"].ConnectionString;
		private static DailyOpsBusiUnitManager busUnitManager = new DailyOpsBusiUnitManager(covmetadataConnString);
		private static DailyOpsManager dailyDataManager = new DailyOpsManager(covmetadataConnString);

		private Dictionary<string, List<DailyOpsData>> dataLists;

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
				updateGridViews();
			}
		}

		protected void InputFormButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/DailyOps.aspx");
		}

		protected void ExceptionsReportButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/ExceptionsReport.aspx");
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
			updateGridViews();
		}

		/// <summary>
		/// Updates the tables based upon which facility the user has selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void FacilityList_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateGridViews();
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
				updateGridViews();
		}

		protected void FacilityReportingFacilityTypeData_RowCreated(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				e.Row.Cells[1].ColumnSpan = 2;
				e.Row.Cells.RemoveAt(2);

				List<DailyOpsFacilitiesReportingStats> types = (List<DailyOpsFacilitiesReportingStats>)((GridView)sender).DataSource;
				if (types == null)
					return;

				string facilityType = types.ElementAt(e.Row.RowIndex).FacilityType;
				List<DailyOpsData> source = dataLists[facilityType];

				if (source.Count == 0)
					e.Row.Visible = false;
				else
				{
					GridView gv = (GridView)e.Row.FindControl("FacilityReportingData");
					gv.DataSource = source;
					gv.DataBind();
				}
			}
		}

		protected void FacilityReportingData_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			DailyOpsData data = (DailyOpsData)e.Row.DataItem;
			if (data == null)
				return;

			// check if data entered
			// if not entered, display email message
			// if entered, display when and by whom

			if (data.UserRowCreated == null)
			{
				/*string fmName = busUnitManager.GetDailyOpsBusinessUnitContactNameList(data.FacilityID, "FM").ElementAt(0);
				string fmEmail = busUnitManager.GetDailyOpsBusinessUnitContactEmailList(data.FacilityID, "FM").ElementAt(0);

				string ceName = busUnitManager.GetDailyOpsBusinessUnitContactNameList(data.FacilityID, "CE").ElementAt(0);
				string ceEmail = busUnitManager.GetDailyOpsBusinessUnitContactEmailList(data.FacilityID, "CE").ElementAt(0);

				string subject = string.Format("Request for {0}\'s Daily Operations Report for {1}", 
					data.FaciltyDescription, data.ReportingDate.ToShortDateString());

				e.Row.Cells[1].Text = string.Format("No data entered. Email <a href=\"mailto:{2}&Subject={0}\">{1}</a> (Facility Manager) or <a href=\"mailto:{4}&Subject={0}\">{3}</a> (Chief Engineer).",
					subject, fmName, fmEmail, ceName, ceEmail);*/

				e.Row.Cells[1].Text = "No data entered.";
			}
			else
			{
				int offset = Int32.Parse(busUnitManager.GetDailyOpsBusinessUnitEasternTimeOffset(data.FacilityID));
				DateTime lastModified = data.DateLastModified;
				
				if (offset != 0)
				{
					DateTime localModified = lastModified.AddHours(offset);
					e.Row.Cells[1].Text = string.Format("Entered on {0} at {1} ({2} local time) by {3}.", lastModified.ToShortDateString(),
						lastModified.ToShortTimeString(), localModified.ToShortTimeString(), data.UserRowCreated);
				}

				else
				{

					e.Row.Cells[1].Text = string.Format("Entered on {0} at {1} by {2}.", lastModified.ToShortDateString(),
						lastModified.ToShortTimeString(), data.UserRowCreated);
				}

				e.Row.Cells[1].CssClass = "FacilityRow Bold";
			}
		}

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

			facilityData = (from u in facilityData
							orderby (u.UserRowCreated != null), u.FaciltyDescription
							select u).ToList();

			foreach (DailyOpsData entry in facilityData)
			{
				string id = entry.FacilityID;
				string key = busUnitManager.GetBusinessUnitsTypeByPSCode(id);
				dataLists[key].Add(entry);
			}
		}

		private void updateGridViews()
		{
			DateTime reportDate = DateTime.Parse(ReportDate.Text);
			Covanta.Common.Enums.Enums.StatusEnum status = Covanta.Common.Enums.Enums.StatusEnum.NotSet;
			List<DailyOpsFacilitiesReportingStats> facilityTypes = dailyDataManager.GetDailyOpsFacilitiesReportingStatsByParameters(reportDate, FacilityList.SelectedValue, RegionList.SelectedValue, ref status);
			List<DailyOpsData> facilityData = dailyDataManager.GetDailyOpsDataByParameters(reportDate, FacilityList.SelectedValue, RegionList.SelectedValue, ref status);

			SummaryData.DataSource = facilityTypes;
			SummaryData.DataBind();

			facilityTypes.RemoveAt(facilityTypes.Count - 1);
			updateDictionary(facilityTypes, facilityData);

			FacilityReportingFacilityTypeData.DataSource = facilityTypes;
			FacilityReportingFacilityTypeData.DataBind();
		}

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
	}
}