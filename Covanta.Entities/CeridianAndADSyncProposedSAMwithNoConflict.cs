using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// CeridianAndADSyncProposedSAMwithNoConflict Detail data
    /// </summary>
    public class CeridianAndADSyncProposedSAMwithNoConflict
    {
        #region constructors
        /// <summary>
        /// This represents a Proposed new SAM account with no conflicts on the proposed name
        /// </summary>
        public CeridianAndADSyncProposedSAMwithNoConflict() { }

        #endregion

        #region public properties

        public string ClockNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string ProposedSamAccountName { get; set; }
        public string Title { get; set; }
        public string Region { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string MgrClockNumber { get; set; }
        public string UnionIndicator { get; set; }
        public string ExemptStatus { get; set; }
      
        #endregion
    } 
}