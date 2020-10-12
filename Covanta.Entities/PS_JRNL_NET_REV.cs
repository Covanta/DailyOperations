using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    public class PS_JRNL_NET_REV
    {
        #region constructors
        /// <summary>
        /// This represents a row on the PS_JRNL_NET_REV table
        /// </summary>
        public PS_JRNL_NET_REV() { }

        #endregion

        #region public properties

        public string Entity { get; set; }       
        //public DateTime JournalDate { get; set; }
        public string Account { get; set; }
        public double Amount { get; set; }
        public string Year { get; set; }
        public string AccountingPeriod { get; set; }
      
        #endregion
    }
}
