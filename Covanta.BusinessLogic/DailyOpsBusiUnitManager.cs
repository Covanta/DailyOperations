using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using Covanta.DataAccess;
using Covanta.Utilities;
using Covanta.Utilities.Helpers;
using Covanta.Common.Enums;

namespace Covanta.BusinessLogic
{
    public class DailyOpsBusiUnitManager
    {
        #region private variables

        string _dbConnection = null;
        List<DailyOpsBusinessUnit> listOfBusinessUnits = null;
        #endregion

        #region constructors

        public DailyOpsBusiUnitManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the BusiUnitManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }


        #endregion

        #region public methods

        /// <summary>
        /// Returns a list of Business Units objects
        /// </summary>
        /// <returns>A list of Business Units objects</returns>
        public List<DailyOpsBusinessUnit> GetBusinessUnitsList()
        {
            listOfBusinessUnits = null;
            try
            {
                DALBusinessUnits dal = new DALBusinessUnits(_dbConnection);
                listOfBusinessUnits = dal.GetDailyOpsBusinessUnitsList();
            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }
            return listOfBusinessUnits;
        }

        /// <summary>
        /// Returns a list of DailyOps Business Units objects in a particular region
        /// </summary>
        /// <param name="region">Region we want data from</param>
        /// <returns>a list of DailyOps Business Units objects</returns>
        public List<DailyOpsBusinessUnit> GetDailyOpsBusinessUnitsListByRegion(string region)
        {

            List<DailyOpsBusinessUnit> fullListOfBusinessUnits = new List<DailyOpsBusinessUnit>();
            List<DailyOpsBusinessUnit> reducedListOfBusinessUnits = new List<DailyOpsBusinessUnit>();
            try
            {
                DALBusinessUnits dal = new DALBusinessUnits(_dbConnection);
                fullListOfBusinessUnits = dal.GetDailyOpsBusinessUnitsList();
                foreach (DailyOpsBusinessUnit item in fullListOfBusinessUnits)
                {
                    if ((item.Region == region) || (region == string.Empty))
                    {
                        reducedListOfBusinessUnits.Add(item);
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }

            return reducedListOfBusinessUnits;
        }

        /// <summary>
        /// Gets a list of the Five digit PeopleSoft Codes (ex. ALEXA)
        /// </summary>
        /// <returns>List of PeopleSoft Codes</returns>
        public List<String> GetBusinessUnitsPeopleSoft5DigitCodeList()
        {
            if (listOfBusinessUnits == null)
            {
                listOfBusinessUnits = GetBusinessUnitsList();
            }

            List<String> list = new List<string>();
            foreach (DailyOpsBusinessUnit item in listOfBusinessUnits)
            {
                list.Add(item.PS_Unit);
            }
            //list = (from u in list
            //       orderby u.
            //       select u).ToList();


            return list;

        }

        /// <summary>
        /// Gets a list of the Busines Unit Long Names (ex. Covanta Alex/Arl., Inc.)
        /// </summary>
        /// <returns>List of Business Unit Names</returns>
        public List<String> GetBusinessUnitsDescriptionList()
        {
            if (listOfBusinessUnits == null)
            {
                listOfBusinessUnits = GetBusinessUnitsList();
            }

            List<String> list = new List<string>();
            foreach (DailyOpsBusinessUnit item in listOfBusinessUnits)
            {
                list.Add(item.Description);
            }
            //list = (from u in list
            //       orderby u.
            //       select u).ToList();


            return list;

        }

        /// <summary>
        /// Gets the Busines Unit Long Names (ex. Covanta Alex/Arl., Inc.)
        /// </summary>
        /// <param name="facilityID">facilty (PS code)</param>
        /// <returns>Facility long name</returns>
        public string GetBusinessUnitsDescriptionByPSCode(string facilityID)
        {
            List<DailyOpsBusinessUnit> dailyOpsBusinessUnitList = GetBusinessUnitsList();

            string BUDescription = string.Empty;
            foreach (DailyOpsBusinessUnit item in dailyOpsBusinessUnitList)
            {
                if (facilityID == item.PS_Unit)
                {
                    BUDescription = item.Description;
                    break;
                }
            }
            return BUDescription;
        }

        /// <summary>
        /// Gets the Business Unit Type (ex. WTE)
        /// </summary>
        /// <param name="facilityID">facilty (PS code)</param>
        /// <returns>Facility Type</returns>
        public string GetBusinessUnitsTypeByPSCode(string facilityID)
        {
            List<DailyOpsBusinessUnit> dailyOpsBusinessUnitList = GetBusinessUnitsList();

            string FType = string.Empty;
            foreach (DailyOpsBusinessUnit item in dailyOpsBusinessUnitList)
            {
                if (facilityID == item.PS_Unit)
                {
                    FType = item.FacilityType;
                    break;
                }
            }
            return FType;
        }

        /// <summary>
        /// Gets a list of all of the regions in the system
        /// </summary>
        /// <returns>list of all of the regions in the system</returns>
        public List<string> GetDistinctRegionsList()
        {
            if (listOfBusinessUnits == null)
            {
                listOfBusinessUnits = GetBusinessUnitsList();
            }

            List<String> list = new List<string>();
            foreach (DailyOpsBusinessUnit item in listOfBusinessUnits)
            {
                if (list.Exists(element => element == item.Region) == false)
                {
                    list.Add(item.Region);
                }
            }

            return list;
        }

        /// <summary>
        /// Gets a list of facilities in a particular region
        /// </summary>
        /// <param name="region">Region</param>
        /// <returns>list of facilities</returns>
        public List<string> GetFacilityListbyRegion(string region)
        {
            if (listOfBusinessUnits == null)
            {
                listOfBusinessUnits = GetBusinessUnitsList();
            }

            List<String> list = new List<string>();
            foreach (DailyOpsBusinessUnit item in listOfBusinessUnits)
            {
                if (item.Region == region)
                {
                    if (list.Exists(element => element == item.PS_Unit) == false)
                    {
                        list.Add(item.PS_Unit);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Gets a list of distinct facility types
        /// 
        /// </summary>
        /// <returns>a list of DailyOpsBusinessUnits with only the facility type property populated</returns>
        public List<DailyOpsBusinessUnit> GetDistinctFacilityTypesList()
        {
            if (listOfBusinessUnits == null)
            {
                listOfBusinessUnits = GetBusinessUnitsList();
            }

            List<String> listOfFacilityTypes = new List<string>();
            foreach (DailyOpsBusinessUnit item in listOfBusinessUnits)
            {
                if (listOfFacilityTypes.Exists(element => element == item.FacilityType) == false)
                {
                    listOfFacilityTypes.Add(item.FacilityType);
                }
            }

            List<DailyOpsBusinessUnit> listOfDistinctFacilityTypes = new List<DailyOpsBusinessUnit>();

            foreach (string element in listOfFacilityTypes)
            {
                DailyOpsBusinessUnit x = new DailyOpsBusinessUnit(string.Empty, string.Empty, 0, false);
                x.FacilityType = element;
                listOfDistinctFacilityTypes.Add(x);
            }

            return listOfDistinctFacilityTypes;
        }

        /// <summary>
        /// return the number of Facilities in the region and Fac Type
        /// </summary>
        /// <param name="BuList">a list of Business Units</param>
        /// <param name="region">region</param>
        /// <param name="FacType">facility type</param>
        /// <returns>the number of facilities for the parameters passed in</returns>
        public int GetNumberOfFacilitiesInRegionAndFacType(List<DailyOpsBusinessUnit> BuList, string region, string FacType)
        {
            int facilities = 0;
            foreach (DailyOpsBusinessUnit bu in BuList)
            {
                if ((bu.Region == region) && (bu.FacilityType == FacType))
                {
                    facilities++;
                }
            }
            return facilities;
        }

        public List<string> GetDistinctFacilityTypesAsListOfStrings()
        {
            List<DailyOpsBusinessUnit> BUList = GetDistinctFacilityTypesList();
            List<string> list = new List<string>();
            foreach (DailyOpsBusinessUnit bu in BUList)
            {
                list.Add(bu.FacilityType);
            }

            return list;
        }

        /// <summary>
        /// Returns a list of emails by Facility and Position
        /// </summary>
        /// <param name="facilityID">5 char PeopleSoft code</param>
        /// <param name="position">must be CE (chief Engineer) or FM (facility Manager)</param>
        /// <returns>list of emails</returns>
        public List<string> GetDailyOpsBusinessUnitContactEmailList(string facilityID, string position)
        {
            if ((position != "FM") && (position != "CE"))
            {
                return new List<string>();
            }
            DALBusinessUnits dal = new DALBusinessUnits(_dbConnection);
            List<string> list = dal.GetDailyOpsBusinessUnitContactEmailList(facilityID, position);
            return list;
        }

        /// <summary>
        /// Returns a list of Contact names by Facility and Position
        /// </summary>
        /// <param name="facilityID">5 char PeopleSoft code</param>
        /// <param name="position">must be CE (chief Engineer) or FM (facility Manager)</param>
        /// <returns>list of string</returns>
        public List<string> GetDailyOpsBusinessUnitContactNameList(string facilityID, string position)
        {
            if ((position != "FM") && (position != "CE"))
            {
                return new List<string>();
            }
            DALBusinessUnits dal = new DALBusinessUnits(_dbConnection);
            List<string> list = dal.GetDailyOpsBusinessUnitContactNameList(facilityID, position);
            return list;
        }

        /// <summary>
        /// Returns a list of NTIDs by Facility and Position
        /// </summary>
        /// <param name="facilityID">5 char PeopleSoft code</param>
        /// <param name="position">must be CE (chief Engineer) or FM (facility Manager)</param>
        /// <returns>list of string</returns>
        public List<string> GetDailyOpsBusinessUnitNTIDList(string facilityID, string position)
        {
            if ((position != "FM") && (position != "CE"))
            {
                return new List<string>();
            }
            DALBusinessUnits dal = new DALBusinessUnits(_dbConnection);
            List<string> list = dal.GetDailyOpsBusinessUnitNTIDList(facilityID, position);
            return list;
        }

        /// <summary>
        /// Returns the number of hours (could be 0) that the facility is offset from Eastern time
        /// Ex. Long Beach is -3
        /// </summary>
        /// <param name="facilityID">5 char PeopleSoft code</param>      
        /// <returns>string with hours offset</returns>
        public string GetDailyOpsBusinessUnitEasternTimeOffset(string facilityID)
        {
            string timeOffset = "0";
            DALBusinessUnits dal = new DALBusinessUnits(_dbConnection);
            timeOffset = dal.GetDailyOpsBusinessUnitEasternTimeOffset(facilityID);
            return timeOffset;
        }


        #endregion

        #region private methods

        /// <summary>
        /// Formats the exception message
        /// </summary>      
        /// <param name="e">The exception which was thrown</param>
        /// <returns>Formatted text describing the exception</returns>
        private string formatExceptionText(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("An Exception has occurred.");
            sb.Append("\n\n");
            sb.Append("Message: " + e.Message);
            sb.Append("\n");
            sb.Append("Source: " + e.Source);

            return sb.ToString();
        }
        #endregion

    }
}
