using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a Daily Ops reportable event for a particular facility for a particular day
    /// </summary>
    public class DailyOpsReportableEvent
    {

        #region constructors

        public DailyOpsReportableEvent(string eventType, string description)
        {
            EventType = eventType;
            Description = description;
        }

        #endregion

        #region public properties

        /// <summary>
        /// Type of Event we are referring to (ex. OSHA, CEMS - Reportable)
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Description of Event
        /// </summary>
        public string Description { get; set; }
              
        #endregion
    }
}
