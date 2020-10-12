using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Covanta.DataAccess;


namespace Covanta.BusinessLogic
{
    public class LongviewExtractManager
    {
        #region private variables

        string _dbConnection = null;
        bool _factTableAlreadyCreated = false;
        const char DELIMITER = '_';

        #endregion

        #region constructors
        /// <summary>
        /// Instanciates this class with connection string as parameter
        /// </summary>
        /// <param name="dbConnection">connection string</param>
        public LongviewExtractManager(string dbConnection)
        {
            if (dbConnection == null)
            {
                throw new Exception("The connection string passed into the LongviewExtractManager was null or empty");
            }
            else
            {
                _dbConnection = dbConnection;                
            }
        }
        #endregion

        #region public methods

        /// <summary>
        /// Parses a Dimension File
        /// </summary>
        /// <param name="filepath">the path of the file to be parsed</param>
        public void ParseDimFile(string filepath)
        {
            int lineCount = 0;
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    // Set the target table
                    string targetTableName = Path.GetFileNameWithoutExtension(filepath);
                    Console.WriteLine("Setting up the DB table: " + targetTableName);
                   
                   // DataAccess.SetDimensionTable(_connStringLongview, targetTableName);

                    // Parse each line in the file and put the information into the DB table
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lineCount++;

                    //    DataAccess.InsertIntoDimensionTable(_connStringLongview, targetTableName, line, accountsList, businessUnitList);
                        if (lineCount % 100 == 0)
                        {
                            Console.WriteLine("\t- Processed " + lineCount + " lines");
                        }
                    }
                    Console.WriteLine("Trimming the DB table: " + targetTableName);
                 //   DataAccess.TrimDimensionTable(_connStringLongview, targetTableName);
                }
                Console.WriteLine("FINISHED! Processed " + lineCount + " lines");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while parsing file: " + filepath + " at line " + lineCount + " - ERROR: " + ex.Message);
            }

        }

        /// <summary>
        /// Parses a Fact File
        /// </summary>
        /// <param name="filePath">the path of the file to be parsed</param>
        public void ParseFactFile(string filePath, string schema)
        {
            DALLongviewExtract dal = new DALLongviewExtract(_dbConnection);

            int lineCount = 0;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Set the target table   (the name is FACT_OPREVENUE_F1201_2012-10-04.asc Where ????_??_?? is the last 10 chars of the file name) 
                   
                               
                    string targetTableName = Path.GetFileNameWithoutExtension(filePath).Replace("-", "_");
                    string[] dataParts = targetTableName.Split(DELIMITER);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(dataParts[0]);
                    sb.Append("_");
                    sb.Append(dataParts[1]);
                   

                    targetTableName = sb.ToString();
                 
                    //Only set up the FACT table once (all FACT tables are concatenated into one DB table)                  
                    if (!(_factTableAlreadyCreated))
                    {
                        Console.WriteLine("Setting up the DB table: " + targetTableName);
                        dal.SetFactTable(targetTableName, schema);                       
                        _factTableAlreadyCreated = true;
                    }

                    // Parse each line in the file and put the information into the DB table
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    //while (((line = sr.ReadLine()) != null) && (lineCount < 5000))
                    {
                        lineCount++;
                        dal.InsertIntoFactTable(targetTableName, schema, line, lineCount);                    
                        if (lineCount % 2000 == 0)
                        {
                            Console.WriteLine("\t- Processed " + lineCount + " lines");
                            //break;
                        }
                    }
                }
                Console.WriteLine("FINISHED! Processed " + lineCount + " lines");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while parsing file: " + filePath + " at line " + lineCount + " - ERROR: " + ex.Message);
            }
        }
        #endregion
    }
}
