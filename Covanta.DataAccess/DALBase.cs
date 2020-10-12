using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Covanta.Common.Enums;
using System.Data.SqlClient;
using Covanta.Utilities.Helpers;

namespace Covanta.DataAccess
{
    /// <summary>
    /// Provides a base class for the DAL classes.  These methonds provide a 
    /// </summary>
    public class DALBase
    {
        #region constructors
       
        public DALBase(string dbConnection) 
        {
            if ((dbConnection == null) || (dbConnection == string.Empty))
            {
                throw new Exception("The connection string passed into the Data layer was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;
            }
        }
        #endregion

        #region protected variables

        protected string _dbConnection = null;

        #endregion

        #region protected methods

        /// <summary>
        /// Call the Stored Proc Names in the input parameters and executes it.  Throws an exception if on is encountered.
        /// This is for READ ONLY Stored Procs which return only 1 data table.      
        /// </summary>
        /// <param name="command">SQL command object</param>
        /// <param name="connection">SQL Connection object</param>       
        /// <returns>A populated DataTable</returns>
        protected DataTable executeSQLCommandGetDataTable(SqlCommand command, SqlConnection connection)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                using (connection)
                {
                    using (command)
                    {                        
                        connection.Open();
                        ds.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, new string[] { "dt" });
                        dt = ds.Tables["dt"];
                    }
                }
            }
            catch (SqlException e)
            {               
                StringBuilder sb = new StringBuilder();
                sb.Append("A SqlException has occurred.");
                sb.Append("\n\n");
                sb.Append(parseCommandString(command));
                sb.Append("\n\n");
                sb.Append(parseSqlExceptionDetails(e));

                string errorString = sb.ToString();
                throw new Exception(errorString, e);
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("An Exception has occurred.");
                sb.Append("\n\n");
                sb.Append(parseCommandString(command));
                sb.Append("\n\n");
                sb.Append(parseExceptionDetails(e));

                string errorString = sb.ToString();

                throw new Exception(errorString, e);
            }

            return dt;
        }               

        /// <summary>
        /// Call the Stored Proc Names in the input parameters and executes it.
        /// This is for ExecuteNonQuery Stored Procs(Inserts, Updates, Deletes).
        /// </summary>
        /// <param name="command">SQL command object</param>
        /// <param name="connection">SQL Connection object</param>       
        /// <returns>void</returns>
        protected void executeSQLCommandExecuteNonQuery(SqlCommand command, SqlConnection connection)
        {            
            try
            {
                using (connection)
                {
                    using (command)
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("A SqlException has occurred.");
                sb.Append("\n\n");
                sb.Append(parseCommandString(command));
                sb.Append("\n\n");
                sb.Append(parseSqlExceptionDetails(e));

                string errorString = sb.ToString();
                throw new Exception(errorString, e);
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("An Exception has occurred.");
                sb.Append("\n\n");
                sb.Append(parseCommandString(command));
                sb.Append("\n\n");
                sb.Append(parseExceptionDetails(e));

                string errorString = sb.ToString();

                throw new Exception(errorString, e);
            }

            return;
        }

        public string populateExceptionDataBase(SqlCommand command)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(command.Connection.ConnectionString);
            sb.AppendLine(command.CommandText);
            sb.AppendLine(command.CommandType.ToString());
            foreach (SqlParameter param in command.Parameters)
            {
                if (param.Value != null)
                {
                    sb.AppendLine(param.ToString() + "  " + param.Value.ToString());
                }                
            }

            return sb.ToString();
        }

     

        #endregion

        #region private methods

        /// <summary>
        /// Populates a string with info from the SQLCommand.  This string can be used to inquire abount and error if one occurs.        
        /// </summary>
        /// <param name="command">Command which is used to make the database call.</param>
        /// <returns>Returns a string containing info from the SQLCommand. </returns>
        protected string parseCommandString(SqlCommand command)
        {
            string message = string.Empty;
            message += "Parsed Command String is as follows:" + "\n";
            message += "CommandType: " + command.CommandType.ToString() + "\n" +
                        "CommandText: " + command.CommandText + "\n" +
                        "ConnectionString: " + command.Connection.ConnectionString;                              
            
            return message;
        }

        /// <summary>
        /// Parses thru the details of a SqlException and returns a formated string of the details.
        /// </summary>
        /// <param name="e">SqlException</param>
        /// <returns>Formated string of the details of the SqlException</returns>
        private string parseSqlExceptionDetails(SqlException e)
        {            
            string message = string.Empty;
            message += "Parsed SqlException String is as follows:" + "\n";
            for (int i = 0; i < e.Errors.Count; i++)
            {
                message += "Index #" + i + "\n" +
                                 "Message: "    + e.Errors[i].Message    + "\n" +
                                 "LineNumber: " + e.Errors[i].LineNumber + "\n" +
                                 "Source: "     + e.Errors[i].Source     + "\n" +
                                 "Server: "     + e.Errors[i].Server     + "\n" +
                                 "Procedure: "  + e.Errors[i].Procedure  + "\n";
            }
            return message;
        }

        /// <summary>
        /// Parses thru the details of an Exception and returns a formated string of the details.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <returns>Formated string of the details of the Exception</returns>
        private string parseExceptionDetails(Exception e)
        {
            string message = string.Empty;
            message += "Parsed Exception String is as follows:" + "\n";
            message =  "Message: " + e.Message + "\n" +
                       "Source: "  + e.Source  + "\n";
            
            return message;
        }

 

        #endregion

    }
}
