using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the C4RWasteModel Table 
    /// </summary>
    public class C4RWasteModel
    {

        #region constructors
        /// <summary>
        /// This represents a row on the C4RWasteModel Table 
        /// </summary>
        public C4RWasteModel() { }
       
        #endregion

        #region public properties

        public string Location { get; set; }
        public DateTime Date1 { get; set; }
        public int Base { get; set; }      
        public int Spot { get; set; }        
        public int Premium { get; set; }      
        public int RaysPremium { get; set; }
        public int SpecialSolid { get; set; }
        public int SpecialLDI { get; set; }
        public int MSWReceived { get; set; }
        public int TotalSolidReceived { get; set; }
        public int SteamGenerated { get; set; }
        public int SteamFactor { get; set; }
        public int SteamRate { get; set; }
        public int BoilersOnline { get; set; }
        public int TotalSolidProcessed { get; set; }
        public int ModelCalcPitInventory { get; set; }
        public int PlantEstInventory { get; set; }
        public int ModelVsPlantVariance { get; set; }
        public DateTime PriorMonday { get; set; }      
        public bool isWithin_Next_3_Weeks { get; set; }    
        public bool isWithin_Next_6_Weeks { get; set; }
        public bool isBelowMinTons { get; set; }
        public int MinTons { get; set; }
        public bool isDateInPast { get; set; }
        public bool isDateToday { get; set; }
        public DateTime NextDateBelowMinTons { get; set; }
        public bool Next_3_WeeksHasValueBelowMin { get; set; }
        public int NextTonsBelowMinTons { get; set; }
        public DateTime DocumentLastModifiedDate { get; set; }
      
        #endregion
    }
}
 