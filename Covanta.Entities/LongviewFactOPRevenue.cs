using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    public class LongviewFactOPRevenue
    {
        
        #region constructors
        /// <summary>
        /// This represents a row on the LongviewFactOPRevenue table
        /// </summary>
        public LongviewFactOPRevenue() { }

        #endregion

        #region public properties

        public string Account { get; set; }       
        public string Type { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Entity { get; set; }
        //public string Detail1 { get; set; }
        //public string Detail2 { get; set; }
        public double Value { get; set; }
        public double PercentOfTotal { get; set; }
        public double NetValue { get; set; }
        public double JournalTotal { get; set; }

     
        #endregion
    }
}
