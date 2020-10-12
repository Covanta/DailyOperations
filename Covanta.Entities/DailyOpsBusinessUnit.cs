using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a Covanta Business Unit
    /// </summary>
    public class DailyOpsBusinessUnit
    {
     
        #region constructors

        /// <summary>
        /// This represents a Covanta DailyOps Business Unit. 
        /// </summary>
        /// <param name="iD">Unique Database ID</param>
        /// <param name="ps_Unit">PeopleSoft 5 char code</param>
        /// <param name="description">Full Name of Business Unit</param>
        /// <param name="numOfBoilers">Number Of Boilers in the Business Unit</param>
        /// <param name="isFacilityFrontEndFerrousSystem">Is this facility a Front End Ferrous System</param>
        public DailyOpsBusinessUnit(string ps_Unit, string description, int numOfBoilers, bool isFacilityFrontEndFerrousSystem)
        {          
            PS_Unit = ps_Unit;  
            Description = description;
            NumOfBoilers = numOfBoilers;
            IsFacilityFrontEndFerrousSystem = isFacilityFrontEndFerrousSystem;
        }

        #endregion

        #region public properties

        /// <summary>
        /// PeopleSoft 5 char code
        /// </summary>
        public string PS_Unit { get; set; }
       
        /// <summary>
        /// Full Name of Business Unit
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Number Of Boilers in the Business Unit
        /// </summary>
        public int NumOfBoilers { get; set; }

        /// <summary>
        /// Is this facility a Front End Ferrous System
        /// </summary>
        public bool IsFacilityFrontEndFerrousSystem { get; set; }

        /// <summary>
        /// Region of Facility
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Type of Facility  example WTE, Wood, BioMass
        /// </summary>
        public string FacilityType { get; set; }

        /// <summary>
        /// Number of turbines at the Facility
        /// </summary>
        public int NumOfTurbines { get; set; }

        /// <summary>
        /// This property represents an upper limit on the number of Tons Delivered which can be entered for 1 day.
        /// Used by the Daily OPs Report input screen to prevent erroneous entries which would skew the reports.
        /// </summary>
        public int MaxTonsDelivered { get; set; }

        /// <summary>
        /// This property represents an upper limit on the number of Tons Processed which can be entered for 1 day.
        /// Used by the Daily OPs Report input screen to prevent erroneous entries which would skew the reports.
        /// </summary>
        public int MaxTonsProcessed { get; set; }

        /// <summary>
        /// This property represents an upper limit on the number of Steam Produced which can be entered for 1 day.
        /// Used by the Daily OPs Report input screen to prevent erroneous entries which would skew the reports.
        /// </summary>
        public int MaxSteamProduced { get; set; }

        /// <summary>
        /// This property represents an upper limit on the number of Steam Sold which can be entered for 1 day.
        /// Used by the Daily OPs Report input screen to prevent erroneous entries which would skew the reports.
        /// </summary>
        public int MaxSteamSold { get; set; }

        /// <summary>
        /// This property represents an upper limit on the number of Net Electric which can be entered for 1 day.
        /// Used by the Daily OPs Report input screen to prevent erroneous entries which would skew the reports.
        /// </summary>
        public int MaxNetElectric { get; set; }
        
        #endregion
    }
}
