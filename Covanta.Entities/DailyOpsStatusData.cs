using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents the status of a particular boiler in a particular facility
    /// </summary>
    public class DailyOpsStatusData
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

        public string SystemType { get; set; }

        public string WasReprocessed { get; set; }

        public string CriticalAssetsInAlarm { get; set; }

        public DateTime CriticalAssetsExpectedBackOnlineDate { get; set; }

        public string Comments { get; set; }

        public DateTime CriticalEquipmentOOSExpectedBackOnlineDate { get; set; }

        public string FireSystemOutOfService { get; set; }

        public DateTime FireSystemOutOfServiceExpectedBackOnlineDate { get; set; }

        public string EventType { get; set; }

        public string EventDescription { get; set; }

        #endregion
    }
}
