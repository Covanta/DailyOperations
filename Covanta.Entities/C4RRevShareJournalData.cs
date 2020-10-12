using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the C4R Journal Row (Account 44999)
    /// </summary>
    public class C4RRevShareJournalData
    {

        #region constructors
        /// <summary>
        /// This represents a row on the C4R Journal Row (Account 44999)
        /// </summary>
        public C4RRevShareJournalData() { }

        #endregion

        #region public properties

        public string BusinessUnit { get; set; }       
        public string Year { get; set; }
        public string Month { get; set; }
        public string Account { get; set; }
        public string LineDescription { get; set; }
        public decimal Amount { get; set; }
      
        #endregion
    }
}
