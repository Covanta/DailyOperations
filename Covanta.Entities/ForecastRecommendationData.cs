using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the ForecastRecommendationData Table limited to a few properties
    /// </summary>
    public class ForecastRecommendationData
    {

        #region constructors
        /// <summary>
        /// This represents a row on the ForecastRecommendationData Table limitd to a few properties
        /// </summary>
        public ForecastRecommendationData() { }
       
        #endregion

        #region public properties

        public string Region { get; set; }
        public string Facility { get; set; }
        public string FacilityShortName { get; set; }
        public string AccountID { get; set; }
        public string SequenceNum { get; set; }
        public string AccountDesc { get; set; }
        public string RevShare { get; set; }
        public string Identifier { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int Budget { get; set; }
        public int Forecast { get; set; }
        public int Actual_EST { get; set; }
        public int Actual { get; set; }
  
        #endregion
    }
}
