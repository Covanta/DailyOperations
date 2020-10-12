using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents the statistics of the reporting facilities.
    /// It shows the total number of facilities reporting out of the full number of facilities which should report, by Facility Type. 
    /// </summary>
    public class DailyOpsFacilitiesReportingStats
    {
     
        #region constructors

        public DailyOpsFacilitiesReportingStats(string facilityType, int facilitiesReportingExpected, int facilitiesReportingActual)
        {
            FacilityType = facilityType;
            FacilitiesReportingExpected = facilitiesReportingExpected;
            FacilitiesReportingActual = facilitiesReportingActual;         
        }

        #endregion

        #region public properties

        /// <summary>
        /// Facility Type
        /// </summary>
        public string FacilityType { get; set; }

        /// <summary>
        /// Number of Facilities we are expecting to report
        /// </summary>
        public int FacilitiesReportingExpected { get; set; }

        // <summary>
        /// Number of Facilities who have reported
        /// </summary>
        public int FacilitiesReportingActual { get; set; }
       
        #endregion
    }
}
