using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the ServiceNowVIP Table 
    /// </summary>
    public class ServiceNowVIP
    {
        #region constructors
        /// <summary>
        /// This represents a row on the ServiceNowVIP Table 
        /// </summary>
        public ServiceNowVIP() { }
       
        #endregion

        #region public properties

        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Active { get; set; }
        public int VIP { get; set; }
        public DateTime SN_CreatedDate { get; set; }
        public DateTime SN_UpdateDate { get; set; }        
      
        #endregion
    }
}
 
