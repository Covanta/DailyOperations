using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the C4R Forecast Extract File
    /// </summary>
    public class C4RForecastExtractRow
    {

        #region constructors
        /// <summary>
        /// This represents a row on the C4R Forecast Extract File
        /// </summary>
        public C4RForecastExtractRow()  {}

        #endregion

        #region public properties

        public string PeopleSoft5DigitCode { get; set; }       
        public string DIM1 { get; set; }
        public string DIM2 { get; set; }
        public string DIM3 { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
        public string Total { get; set; }
        public string Budget { get; set; }
        public string Variance { get; set; }
      
        #endregion
    }
}
