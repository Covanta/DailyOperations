using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the EmployeeInfo Table
    /// </summary>
    public class EmployeeInfo
    {

        #region constructors
        /// <summary>
        /// This represents a row on the ActiveDirectoryInfo Table
        /// </summary>
        public EmployeeInfo() { }
       
        #endregion

        #region public properties

        public DateTime Run_date { get; set; }
        public string EE_ID { get; set; }
        public string Salary_Class { get; set; }
        public string PayRateType { get; set; }
        public string Exempt { get; set; }
        public string SSN { get; set; }
        public string WD_UserName { get; set; }
        public string LAST { get; set; }
        public string FIRST { get; set; }
        public string MIDDLE { get; set; }
        public string Suffix { get; set; }
        public string HOME_ADDR1 { get; set; }
        public string HOME_ADDR2 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string Country { get; set; }
        public string MARITAL { get; set; }
        public string Status { get; set; }
        public DateTime Termination_Date { get; set; }
        public DateTime OrigHireDate { get; set; }
        public string Work_Email { get; set; }
        public string JobTitle { get; set; }
        public string DivisionID { get; set; }
        public string PayGroup { get; set; }
        public string UnionName { get; set; }
        public string RegionID { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public string Work_Addr1 { get; set; }
        public string Work_Addr2 { get; set; }
        public string Work_City { get; set; }
        public string Work_State { get; set; }
        public string Work_Country { get; set; }
        public string Work_Postal { get; set; }
        public string Work_phone { get; set; }
        public string Mgr_FullName { get; set; }
        public string Mgr_EE_ID { get; set; }
        public string Mgr_Email { get; set; }

        #endregion
    }
}
