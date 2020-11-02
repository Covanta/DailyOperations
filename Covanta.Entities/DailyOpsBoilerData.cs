using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents the status of a particular boiler in a particular facility
    /// </summary>
    public class DailyOpsBoilerData
    {
        #region public properties

        public string FacilityType { get; set; }

        public string FacilityDescription { get; set; }
            
        public int BoilerNumber { get; set; }

        public string Status { get; set; }

        public string UnscheduledOutageExplanation { get; set; }

        public decimal Downtime { get; set; }

        public decimal CumulativeDowntime { get; set; }

        public decimal MonthToDate { get; set; }

        public DateTime ExpectedRepairDate { get; set; }

        #endregion
    }
}
