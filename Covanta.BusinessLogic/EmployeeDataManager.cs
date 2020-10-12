using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.Entities;
using Covanta.DataAccess;
using System.Configuration;


namespace Covanta.BusinessLogic
{
    public class EmployeeDataManager
    {
        #region private variables

        string _dbConnection = null;

        #endregion

        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public EmployeeDataManager(string dbConnection)
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the EmployeeDataManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }
       
        #endregion

        #region public methods

        /// <summary>
        /// Returns a list of ActiveDirectoryInfo objects by LastName
        /// </summary>
        /// <param name="lastname">Last Name</param>
        /// <returns>a list of ActiveDirectoryInfo objects</returns>
        public List<ActiveDirectoryInfo> GetActiveDirectoryInfoListByLastName(string lastname)
        {
            List<ActiveDirectoryInfo> list = new List<ActiveDirectoryInfo>();
            try
            {
                DALEmployeeData dal = new DALEmployeeData(_dbConnection);
                list = dal.GetActiveDirectoryInfoListByLastName(lastname);
            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }

            return list;
        }

        /// <summary>
        /// Returns a list of EmployeeInfo objects by LastName
        /// </summary>
        /// <param name="lastname">Last Name</param>
        /// <returns>a list of EmployeeInfo objects</returns>
        public List<EmployeeInfo> GetEmployeeInfoListByLastName(string lastname)
        {
            List<EmployeeInfo> list = new List<EmployeeInfo>();
            try
            {
                DALEmployeeData dal = new DALEmployeeData(_dbConnection);
                list = dal.GetEmployeeInfoListByLastName(lastname);
            }
            catch (Exception e)
            {
                throw new Exception(formatExceptionText(e), e);
            }

            return list;
        }

        /// <summary>
        /// Returns a list of EmployeeInfoForFrontLineVerification objects by LastName
        /// </summary>
        /// <param name="lastname">Last Name</param>
        /// <returns>a list of EmployeeInfoForFrontLineVerification objects</returns>
        public List<EmployeeInfoForFrontLineVerification> GetEmployeeInfoForFrontLineVerificationListByLastName(string lastname)
        {
            List<EmployeeInfo> employeeInfoList = GetEmployeeInfoListByLastName(lastname);          
            List<EmployeeInfoForFrontLineVerification> employeeFrontlineInfoList = new List<EmployeeInfoForFrontLineVerification>();            

            foreach (EmployeeInfo empInfo in employeeInfoList)
            {
                EmployeeInfoForFrontLineVerification employeeFrontlineInfo = new EmployeeInfoForFrontLineVerification();
                employeeFrontlineInfo.EE_ID = empInfo.EE_ID;
                employeeFrontlineInfo.FIRST = empInfo.FIRST;
                employeeFrontlineInfo.LAST = empInfo.LAST;
                employeeFrontlineInfo.SSN = empInfo.SSN;
                employeeFrontlineInfo.Status = empInfo.Status;

                employeeFrontlineInfoList.Add(employeeFrontlineInfo);
            }

            return employeeFrontlineInfoList;
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
