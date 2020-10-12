using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents the status details of a particular Metal System
    /// </summary>
    public class DailyOpsMetalSystemsStatus
    {
     
        #region constructors

        public DailyOpsMetalSystemsStatus(string systemType, decimal downtime, string downtimeExplanation, DateTime expectedRepairDate, string wasReprocessed)
        {
            SystemType = systemType;
            Downtime = downtime;
            CumulativeDowntime = 0;
            WeekToDate = 0;
            MonthToDate = 0;
            DowntimeExplanation = downtimeExplanation;
            ExpectedRepairDate = expectedRepairDate;
            WasReprocessed = wasReprocessed;
        }

        #endregion

        #region public properties
            
        /// <summary>
        /// Type of Metal we are referring to (ex. Ferrous)
        /// </summary>
        public string SystemType { get; set; }

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

        /// <summary>
        /// Downtime Explanation
        /// </summary>
        public string DowntimeExplanation { get; set; }

        /// <summary>
        /// Expected Repair Date
        /// </summary>
        public DateTime ExpectedRepairDate { get; set; }

        /// <summary>
        /// Was Reprocessed
        /// </summary>
        public string WasReprocessed { get; set; }


        #endregion
    }
}
