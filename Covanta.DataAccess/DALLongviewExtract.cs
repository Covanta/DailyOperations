using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Covanta.DataAccess
{
    /// <summary>
    /// Performs Insert functions to the Longview Extracted data
    /// </summary>
    public class DALLongviewExtract : DALBase
    {
         #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALLongviewExtract(string dbConnection) : base(dbConnection) { }

        #endregion

        #region Public Constants

        public const char DELIMITER = '{';
        public const string SQL_INSERT_STATEMENT = "INSERT INTO {0} ({1}) VALUES ({2})";
        public const string SQL_DATA_EXISTS_STATEMENT = "SELECT TOP 1 {0} from {1} WHERE {0} IS NOT NULL";
        public const string SQL_COLUMN_DROP_STATEMENT = "ALTER TABLE {0} DROP COLUMN {1}";

        public const int DIMENSION_COLUMN_COUNT = 20;
        public const string DIMENSION_TABLE_CREATE_SQL_STATEMENT = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'{0}') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) BEGIN DROP TABLE {0} END CREATE TABLE {0}( [id] [int] NULL, [rowNum] [int] NULL, [numOfLevels] [int] NULL, [dimention] [int] NULL, [field1] [nvarchar](100) NULL, {1}) ON [PRIMARY]";
        public const string DIMENSION_CODE_COLUMN_NAME = "dim{0}code";
        public const string DIMENSION_CODE_COLUMN_FOR_TABLE_CREATION = "[" + DIMENSION_CODE_COLUMN_NAME + "] [nvarchar](100) NULL";
        public const string DIMENSION_DESCRIPTION_COLUMN_NAME = "dim{0}description";
        public const string DIMENSION_DESCRIPTION_FOR_TABLE_CREATION = "[" + DIMENSION_DESCRIPTION_COLUMN_NAME + "] [nvarchar](100) NULL";

        // NOTE: Make sure all of the column names here are consistent across the constants below
        public const string FACT_TABLE_CREATE_SQL_STATEMENT = "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'{0}') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) BEGIN DROP TABLE {0} END CREATE TABLE {0}( [Account] [nvarchar](15) NULL, [Type] [nvarchar](15) NULL, [Year] [nvarchar](4) NULL, [Month] [nvarchar](2) NULL, [Entity] [nvarchar](15) NULL, [Detail1] [nvarchar](15) NULL, [Detail2] [nvarchar](15) NULL, [Value] [float] NULL ) ON [PRIMARY]";
        public const string FACT_TABLE_INSERT_STATEMENT = "INSERT INTO {0} ( {1} ) VALUES ({2})";

        #endregion

        #region Public Static fields
      
        public static string[] FACT_TABLE_COLUMNS = { "Account", "Type", "Year", "Month", "Entity", "Detail1", "Detail2", "Value" };
        public static StringBuilder sbColumns;
        public static StringBuilder sbValues;
        public static List<string> names;
        public static List<string> values;

        #endregion

        #region public methods

        /// <summary>
        /// Prepares the command and connection for creating a new FACT table
        /// </summary>
        /// <param name="tableName">The name of the new database FACT table</param>
        public void SetFactTable(string tableName, string schema)
        {
            tableName = schema + "." + tableName;
            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand cmd = new SqlCommand();

            //populate the command object
            cmd.Connection = connection;
            cmd.CommandText = String.Format(FACT_TABLE_CREATE_SQL_STATEMENT, tableName);
            cmd.CommandType = CommandType.Text;
            executeSQLCommandExecuteNonQuery(cmd, connection);          
        }
           
        public void InsertIntoFactTable(string tableName, string schema, string data, int lineCount)
        {
            tableName = schema + "." + tableName;
            // Split the data based on the default delimiter
            string[] dataParts = data.Split(DELIMITER);

            if (dataParts.Count() <= 0)
            {
                throw new FormatException("Data not in the expected format - splitting on the \"" + DELIMITER + "\" symbol did not yield any results.");
            }
            if (dataParts.Count() != FACT_TABLE_COLUMNS.Count())
            {
                throw new FormatException("This should contain " + FACT_TABLE_COLUMNS.Count() + " columns but it had " + dataParts.Count());
            }

            //We only want rows which are valid and in the format A44000, so we do not insert any row which doesn't start with an A or S in dataParts[0]
            if ( (dataParts[0].ToUpper().Substring(0, 1) != "A") && (dataParts[0].ToUpper().Substring(0, 1) != "S"))
            {
                return;
            }
           
            //We do not want to capture the 'A' or 'S' in the Account number, so we remove the leading 'A' or 'S' in the account number          
            if ( (dataParts[0].ToUpper().StartsWith("A")) || (dataParts[0].ToUpper().StartsWith("S")))
            {
                dataParts[0] = dataParts[0].Remove(0, 1);
            }
            // Create the insert statement for the data
            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();
            List<object> values = new List<object>();

            //// Get column and set it for the insert
            for (int i = 0; i < FACT_TABLE_COLUMNS.Count(); i++)
            {
                sbColumns.Append("[" + FACT_TABLE_COLUMNS[i] + "]");
                sbValues.Append("@" + FACT_TABLE_COLUMNS[i]);
                if (i != FACT_TABLE_COLUMNS.Count() - 1)
                {
                    sbColumns.Append(", ");
                    sbValues.Append(", ");
                }
            }

            // Build the SqlCommand to perform the insert and populate the parameters          
            //create the connection and command objects
            SqlConnection connection = new SqlConnection(_dbConnection);
            SqlCommand cmd = new SqlCommand();

            //populate the command object
            cmd.Connection = connection;
            cmd.CommandText = string.Format(FACT_TABLE_INSERT_STATEMENT, tableName, sbColumns.ToString(), sbValues.ToString());
            cmd.CommandType = CommandType.Text;
              

            // Iterate through the data part list
            for (int i = 0; i < dataParts.Count(); i++)
            {
                // if the value is not numberic, then replace it with 0
                if ((i == 7) &&  (IsNumeric(dataParts[i]) == false))
                {
                    dataParts[i] = "0"; 
                }
                cmd.Parameters.AddWithValue("@" + FACT_TABLE_COLUMNS[i], dataParts[i]);
                values.Add(dataParts[i]);
            }

            // Insert the information 
            executeSQLCommandExecuteNonQuery(cmd, connection);     
           
        }

        public bool IsNumeric(string x)
        {
            decimal result;
            bool returnValue = false;
            if (decimal.TryParse(x, out result))
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            return returnValue;
        }
        #endregion

    }
}
