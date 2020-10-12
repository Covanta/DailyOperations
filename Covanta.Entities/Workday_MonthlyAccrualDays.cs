using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the Workday_MonthlyAccrualDays Table 
    /// </summary>
    public class Workday_MonthlyAccrualDays
    {

        #region constructors
       
        public Workday_MonthlyAccrualDays() { }
       
        #endregion

        #region public properties

         public int ID { get; set; }
         public DateTime LastDayOfMonth { get; set; }     
         public string LastDayOfMonthName { get; set; }
         public DateTime MaxPeriodEndDateInMonth { get; set; }
         public int DaysLeftInMonth { get; set; }
        
        
        #endregion
    }
}
 