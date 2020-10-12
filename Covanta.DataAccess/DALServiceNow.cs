using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Covanta.Entities;

namespace Covanta.DataAccess
{
    public class DALServiceNow : DALBase
    {
        #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALServiceNow(string dbConnection) : base(dbConnection) { }

        #endregion

        #region private variables

        #endregion

        #region public methods

        /// <summary>
        /// Inserts a list of ServiceNowVIP objects into the database
        /// </summary>
        /// <param name="list">List of ServiceNowVIP</param>
        public void InsertServiceNowVIPToDatabase(List<ServiceNowVIP> list)
        {
            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;

            foreach (ServiceNowVIP obj in list)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_dbConnection))
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = DALSqlStatements.ServiceNowETL.SQLSP_ServiceNow_00200_Insert_VIPData;
                            command.Parameters.AddWithValue("@Active", obj.Active);
                            command.Parameters.AddWithValue("@Email", obj.Email);
                            command.Parameters.AddWithValue("@Name", obj.Name);
                            command.Parameters.AddWithValue("@UserName", obj.UserName);
                            command.Parameters.AddWithValue("@VIP", obj.VIP);
                            command.Parameters.AddWithValue("@SN_CreatedDate", obj.SN_CreatedDate);
                            command.Parameters.AddWithValue("@SN_UpdateDate", obj.SN_UpdateDate);

                            commandString = populateExceptionDataBase(command);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
           
        }

        public void DeleteAllServiceNowVIPData()
        {
            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(_dbConnection))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = DALSqlStatements.ServiceNowETL.SQLSP_ServiceNow_00100_Delete_VIPData;

                        commandString = populateExceptionDataBase(command);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(commandString + ex.ToString(), ex);
            }
        }
        #endregion
    }
}
