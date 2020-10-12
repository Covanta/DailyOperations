using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the AutomatedJobDetails Table
    /// </summary>
    public class AutomatedJobDetails
    {

        #region constructors
        /// <summary>
        /// This represents a row on the AutomatedJobDetails Table
        /// </summary>
        public AutomatedJobDetails()  {}
        
        #endregion

        #region public properties

        public string ApplicationName { get; set; }
        public string Job { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string JobRunsOnServerName { get; set; }
        public string JobRunsUnderID { get; set; }
        public string Author { get; set; }
        public int EmailNotificationListID { get; set; }
        public string DatabaseServer { get; set; }
        public string ScheduledRunTime { get; set; }
        public DateTime LastRunTime { get; set; }
        public List<string> EmailsToBeNotifiedList { get; set; }
        public string EmailsToBeNotified { get; set; }

        #endregion
    }
}