using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.DataAccess
{
    public static class DALSqlStatements
    {

        public static class C4RForecastExtractSQL
        {
            public const string SQLSP_GetC4RForecastExtractSourceFileInfo = "GetC4RForecastExtractSourceFileInfo";
        }

        public static class CeridianAndActiveDirDifferencesSQL
        {
            public const string SQLSP_Get_AD_Ceridian_Merge_VW_Data = "Get_AD_Ceridian_Merge_VW_Data";
            public const string SQLSP_Insert_AD_Ceridian_Differences_Data = "Insert_AD_Ceridian_Differences_Data";
            public const string SQLSP_Truncate_ActiveDirAndCeridianDifferences = "Truncate_ActiveDirAndCeridianDifferences";
            public const string SQLSP_Insert_ProposedSamAccountNames = "Insert_ProposedSamAccountNames";
            public const string SQLSP_Get_AD_Ceridian_Proposed_With_No_Conflict_Detail = "Get_AD_Ceridian_Proposed_With_No_Conflict_Detail";
        }

        public static class C4RRevShareCalcPercentageSQL
        {
            public const string SQLSP_Truncate_C4RRevShareCalcPercentage = "Truncate_C4RRevShareCalcPercentage";
            public const string SQLSP_InsertIntoWildcardC4RRevSharePercentage = "InsertIntoWildcardC4RRevSharePercentages";
        }

        public static class OperationsMORSQL
        {
            public const string SQLSP_Operations_00100_Delete_MORActuals = "Operations_00100_Delete_MORActuals";
            public const string SQLSP_Operations_00300_Insert_MORActuals = "Operations_00300_Insert_MORActuals";
        }

        public static class C4RWasteModel
        {
            public const string SQLSP_C4RWasteModelApp_GetData = "C4RWasteModelApp_GetData";
            public const string SQLSP_C4RWasteModelApp_DeleteAllRows = "C4RWasteModelApp_DeleteAllRows";
            public const string SQLSP_C4RWasteModelApp_InsertAllRows = "C4RWasteModelApp_InsertAllRows";    
        }

        public static class C4RForecastRecommendation
        {           
            public const string SQLSP_C4RForecastRecommendationApp_DeleteAllRows = "C4RForecastRecommendationApp_DeleteAllRows";
            public const string SQLSP_C4RForecastRecommendationApp_InsertAllRows = "C4RForecastRecommendationApp_InsertAllRows";
        }

        public static class DailyOpsSQL
        {
            public const string SQLSP_GetDailyOpsDataByDateAndFacility = "[dailyOps].[GetDailyOpsRecordByFacilityIDAndDate]";
            public const string SQLSP_GetDailyOpsDataByDateAndFacility_V2 = "[dailyOps].[GetDailyOpsRecordByFacilityIDAndDate_V2]";
            public const string SQLSP_GetDailyOpsDataByMonthAndFacility = "[dailyOps].[GetDailyOpsRecordListByFacilityIDAndMonthV2]";
            public const string SQLSP_GetDailyOpsDataByMonthAndFacility_V2 = "[dailyOps].[GetDailyOpsRecordListByFacilityIDAndMonth_V2]";
            public const string SQLSP_GetActiveDailyOpsDataByFacilityID = "[dailyOps].[GetActiveDailyOpsRecordListByFacilityID]";
            public const string SQLSP_GetActiveDailyOpsDataByFacilityID_V2 = "[dailyOps].[GetActiveDailyOpsRecordListByFacilityID_V2]";
            public static string SQLSP_GetDailyOpsDataByID = "[dailyOps].[GetDailyOpsRecordByID]";
            public static string SQLSP_GetDailyOpsDataByID_V2 = "[dailyOps].[GetDailyOpsRecordByID_V2]";
            public static string SQLSP_InsertDailyOpsData = "[dailyOps].[InsertDailyOpsData]";
            public static string SQLSP_InsertDailyOpsData_V2 = "[dailyOps].[InsertDailyOpsData_V2]";
            public static string SQLSP_UpdateDailyOpsData = "[dailyOps].[UpdateDailyOpsData]";
            public static string SQLSP_UpdateDailyOpsData_V2 = "[dailyOps].[UpdateDailyOpsData_V2]";
            public const string SQLSP_GET_GetDailyOpsReportedDatesListByFacilityAndMonth_LIST = "dailyOps.GetDailyOpsReportedDatesListByFacilityAndMonth";
            public const string SQLSP_GET_GetDailyOpsExceptionReportEmail_LIST = "[dbo].[DailyOps_ExceptionReportGetEmailRecipients]";

        }

        public static class CumulativeDowntimeSQL
        {
            public const string SQLSP_CumulativeDowntimeBoiler1 = "dailyOps.CumulativeDowntimeBoiler1";
            public const string SQLSP_CumulativeDowntimeBoiler2 = "dailyOps.CumulativeDowntimeBoiler2";
            public const string SQLSP_CumulativeDowntimeBoiler3 = "dailyOps.CumulativeDowntimeBoiler3";
            public const string SQLSP_CumulativeDowntimeBoiler4 = "dailyOps.CumulativeDowntimeBoiler4";
            public const string SQLSP_CumulativeDowntimeBoiler5 = "dailyOps.CumulativeDowntimeBoiler5";
            public const string SQLSP_CumulativeDowntimeBoiler6 = "dailyOps.CumulativeDowntimeBoiler6";
            public const string SQLSP_CumulativeDownTimeTurbGen1 = "dailyOps.CumulativeDownTimeTurbGen1";
            public const string SQLSP_CumulativeDownTimeTurbGen2 = "dailyOps.CumulativeDownTimeTurbGen2";
            public const string SQLSP_CumulativeFerrousSystemHoursUnavailable = "dailyOps.CumulativeFerrousSystemHoursUnavailable";
            public const string SQLSP_CumulativeNonFerrousSystemHoursUnavailable = "dailyOps.CumulativeNonFerrousSystemHoursUnavailable";
            public const string SQLSP_CumulativeNonFerrousSmallsSystemHoursUnavailable = "dailyOps.CumulativeNonFerrousSmallsSystemHoursUnavailable";
            public const string SQLSP_CumulativeFrontEndFerrousSystemHoursUnavailable = "dailyOps.CumulativeFrontEndFerrousSystemHoursUnavailable";
            public const string SQLSP_CumulativeEnhancedFerrousSystemHoursUnavailable = "dailyOps.CumulativeEnhancedFerrousSystemHoursUnavailable";
            public const string SQLSP_CumulativeEnhancedNonFerrousSystemHoursUnavailable = "dailyOps.CumulativeEnhancedNonFerrousSystemHoursUnavailable";
        }

        public static class DailyOpsBusinessUnitSQL
        {
            public const string SQLSP_GET_DailyOpsBUSINESS_UNIT_LIST = "dailyOps.GetDailyOpsBusinessUnitList";
            public const string SQLSP_GET_GetDailyInputFormContactData_LIST = "dailyOps.GetDailyInputFormContactData";
        }
        public static class AutomatedJobDetailsSQL
        {
            public const string SQLSP_GET_AutomatedJobDetails_LIST = "Get_AutomatedJobDetails_Data";
            public const string SQLSP_GET_AutomatedJobsEmailList_LIST = "Get_AutomatedJobsEmailList_Data";
            public const string SQLSP_GET_AutomatedJobDetails_By_ApplicationName = "Get_AutomatedJobDetails_By_ApplicationName";
            public const string SQLSP_Update_AutomatedJobDetails_By_ApplicationName = "Update_AutomatedJobDetails_By_ApplicationName";
            public const string SQLSP_GET_AutomatedJobDetailsList = "Get_AutomatedJobDetailsList";  
        }
        public static class EmployeeDataSQL
        {
            public const string SQLSP_Get_ActiveDirectoryInfo_By_LastName_LIST = "Get_ActiveDirectoryInfo_By_LastName";
            public const string SQLSP_Get_EmployeeInfo_By_LastName_LIST = "Get_EmployeeInfo_By_LastName";  
        }

        public static class OPRevenue
        {
            public const string SQLSP_NetRevCalc_Get_LongviewOPRevenueReduced_LIST = "NetRevCalc_Get_LongviewOPRevenueReducedList";            
        }

        public static class PS_JRNL
        {
            public const string SQLSP_NetRevCalc_Get_PS_JRNL_NET_Rev44999_LIST = "NetRevCalc_Get_PS_JRNL_NET_Rev44999List";
        }

        //public static class C4RForecastRecExcel
        //{
        //    public const string SQLSP_Get_C4RForecastRecommendationApp_GetAllRows_LIST = "C4RForecastRecommendationApp_GetAllRows";
        //}

        public static class C4RNetRev
        {
            public const string SQLSP_Insert_NetRevValues = "NetRevCalc_Insert_NetRevValues";
            public const string SQLSP_Delete_NetRevValues = "NetRevCalc_Delete_NetRevenueData";
        }

        public static class WorkDayPayAccrualETL
        {
            public const string SQLSP_WorkDayPayAccrualApp_GetMonthlyAccrualDays = "WorkDayPayAccrualApp_GetMonthlyAccrualDays";
        }

        public static class EnvironmentalETL
        {
            public const string SQLSP_Environmental_00100_Delete_EmissionData = "Environmental_00100_Delete_EmissionData";
            public const string SQLSP_Environmental_00200_Get_EmissionData = "Environmental_00200_Get_EmissionData";
            public const string SQLSP_Environmental_00300_Insert_EmissionData = "Environmental_00300_Insert_EmissionData";
        }
        public static class ServiceNowETL
        {
            public const string SQLSP_ServiceNow_00100_Delete_VIPData = "ServiceNow_00100_Delete_VIPData";
            public const string SQLSP_ServiceNow_00200_Insert_VIPData = "ServiceNow_00200_Insert_VIPData";           
        }
        public static class WorkDayOperatingUnitsETL
        {
            public const string SQLSP_Insert_WorkDayOperatingUnits = "Insert_WorkDayOperatingUnits";
            public const string SQLSP_Delete_WorkDayOperatingUnits = "Delete_WorkDayOperatingUnits";
        }
    }
}
