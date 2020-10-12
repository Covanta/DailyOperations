using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// This represents a row on the EnvEmissionData Table 
    /// </summary>
    public class EnvEmissionData
    {

        #region constructors
        /// <summary>
        /// This represents a row on the EnvEmissionData Table 
        /// </summary>
        public EnvEmissionData() { }
       
        #endregion

        #region public properties

        public string Type { get; set; }
        public string Entity { get; set; }
        public string Year { get; set; }
        public Decimal  Value { get; set; }    
        public DateTime DocumentLastModifiedDate { get; set; }
      
        #endregion
    }
}
 