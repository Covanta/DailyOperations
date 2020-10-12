using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the EmployeeInfo Table limitd to a few properties
    /// </summary>
    public class EmployeeInfoForFrontLineVerification
    {

        #region constructors
        /// <summary>
        /// This represents a row on the EmployeeInfo Table limitd to a few properties
        /// </summary>
        public EmployeeInfoForFrontLineVerification() { }
       
        #endregion

        #region public properties

        public string Status { get; set; }
        public string LAST { get; set; }
        public string FIRST { get; set; }      
        public string EE_ID { get; set; }        
        public string SSN { get; set; }      
  
        #endregion
    }
}
