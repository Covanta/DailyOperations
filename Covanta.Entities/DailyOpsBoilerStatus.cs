using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents the status of a particular boiler in a particular facility
    /// </summary>
    public class DailyOpsBoilerStatus
    {
     
        #region constructors

        public DailyOpsBoilerStatus(DateTime reportingDate, string ps_Unit, int boilerNumber, string status, string unscheduledOutageExplanation, decimal downtime, DateTime expectedRepairDate)
        {
            ReportingDate = reportingDate;
            PS_Unit = ps_Unit;
            BoilerNumber = boilerNumber;
            Status = status;
            UnscheduledOutageExplanation = unscheduledOutageExplanation;
            Downtime = downtime;
            CumulativeDowntime = 0;
            WeekToDate = 0;
            MonthToDate = 0;
            ExpectedRepairDate = expectedRepairDate;
        }

        #endregion

        #region public properties

        /// <summary>
        /// Reporting Date
        /// </summary>
        public DateTime ReportingDate { get; set; }

        /// <summary>
        /// PeopleSoft 5 char code
        /// </summary>
        public string PS_Unit { get; set; }
            
        /// <summary>
        /// Number Of Boiler we are referring to (which boiler)
        /// </summary>
        public int BoilerNumber { get; set; }

        /// <summary>
        /// Status of Boiler (example: Operational)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Unscheduled Outage Explanation
        /// </summary>
        public string UnscheduledOutageExplanation { get; set; }

        /// <summary>
        /// Downtime
        /// </summary>
        public decimal Downtime { get; set; }

        /// <summary>
        /// Cumulative Downtime
        /// </summary>
        public decimal CumulativeDowntime { get; set; }

        /// <summary>
        /// Cumulative Week to  date
        /// </summary>
        public decimal WeekToDate { get; set; }

        /// <summary>
        /// Cumulative month to date
        /// </summary>
        public decimal MonthToDate { get; set; }

        public DateTime ExpectedRepairDate { get; set; }

        #endregion
    }
}
