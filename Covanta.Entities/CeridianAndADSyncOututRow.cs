using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row from the AD_Ceridian_Merge_VW_Data Table wihich has diffenent data not in sync between Ceridian and Active Directory

    /// </summary>
    public class CeridianAndADSyncOututRow
    {

        #region constructors
        /// <summary>
        /// This represents a row from the AD_Ceridian_Merge_VW_Data Table wihich has diffenent data not in sync between Ceridian and Active Directory
        /// </summary>
        public CeridianAndADSyncOututRow()
        {           
            Flag_Different_FirstName = "N";
            Flag_Different_Lastname = "N";
            Flag_Different_Manager = "N";
            Flag_Different_Title = "N";
            Flag_AtLeastOneDifferenceFound = "N";
        }

        #endregion

        #region public properties

        public string cer_EbPSID { get; set; }
        public string cer_Location { get; set; }
        public string AD_PhysicalDeliveryOfficeName { get; set; }
        public string cer_ClockNumber { get; set; }
        public string ad_Employeeid { get; set; }
        public string cer_FirstName { get; set; }
        public string cer_Mid_Init { get; set; }
        public string cer_LastName { get; set; }
        public string AD_GivenName { get; set; }
        public string AD_Initials { get; set; }
        public string AD_Surname { get; set; }
        public string cer_Suffix { get; set; }
        public string cer_Title { get; set; }
        public string AD_Title { get; set; }
        public string cer_Status { get; set; }
        public string cer_Region { get; set; }
        public string Cer_Dept { get; set; }
        public string AD_Department { get; set; }
        public string cer_Division { get; set; }
        public string cer_PayType { get; set; }
        public string cer_PayGroup { get; set; }
        public string cer_OfficePhone { get; set; }
        public string AD_Phone { get; set; }
        public int cer_Supervisor_Flxid { get; set; }
        public string cer_Employee_Supervisor_Name { get; set; }
        public DateTime cer_DateLastHired { get; set; }
        public DateTime cer_DateEntered { get; set; }
        public DateTime cer_TermDate { get; set; }
        public string cer_MonthBorn { get; set; }
        public string cer_DayBorn { get; set; }
        public string cer_Union { get; set; }
        public string cer_UnionIndicator { get; set; }
        public string cer_ExemptStatus { get; set; }
        public string cer_SSN { get; set; }
        public string AD_SamAccountName { get; set; }
        public string AD_MailNickname { get; set; }
        public string AD_Manager { get; set; }      

        public string Flag_AtLeastOneDifferenceFound { get; set; }
        public string Flag_Different_FirstName { get; set; }
        public string Flag_Different_Lastname { get; set; }
        public string Flag_Different_Manager { get; set; }
        public string Flag_Different_Title { get; set; }

      
        #endregion
    } 
}
