using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    public class MSWInventoryExceptions
    {
        public MSWInventoryExceptions(string facilityType, string facility, string limit, decimal actualInventory, int inventoryMinLimit, int inventoryMaxLimit)
        {
            FacilityType = facilityType;
            Facility = facility;
            Limit = limit;
            ActualInventory = actualInventory;
            InventoryMinLimit = inventoryMinLimit;
            InventoryMaxLimit = inventoryMaxLimit;
        }
        public string FacilityType { get; set; }
        public string Facility { get; set; }
        public string Limit { get; set; }
        public decimal ActualInventory { get; set; }
        public int InventoryMinLimit { get; set; }
        public int InventoryMaxLimit { get; set; }
    }
}
