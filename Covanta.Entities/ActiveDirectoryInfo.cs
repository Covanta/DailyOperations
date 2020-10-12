using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the ActiveDirectoryInfo Table
    /// </summary>
    public class ActiveDirectoryInfo
    {

        #region constructors
        /// <summary>
        /// This represents a row on the ActiveDirectoryInfo Table
        /// </summary>
        public ActiveDirectoryInfo() { }
       
        #endregion

        #region public properties
                
        public string SamAccountName { get; set; }
        public string Employeeid { get; set; }
        public string GivenName { get; set; }
        public string Initials { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string MailNickname { get; set; }
        public string PhysicalDeliveryOfficeName { get; set; }
        public string Department { get; set; }
        public string TelephoneNumber { get; set; }
        public string Manager { get; set; }
      
        #endregion
    }
}
