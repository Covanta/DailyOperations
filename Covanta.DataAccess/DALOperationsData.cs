using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Covanta.Entities;

namespace Covanta.DataAccess
{
    public class DALOperationsData : DALBase
    {
        #region constructors

        /// <summary>
        /// Initializes the new object with the connection string which is passed in.
        /// </summary>
        /// <param name="dbConnection">connection string data</param>
        public DALOperationsData(string dbConnection) : base(dbConnection) { }

        #endregion

        #region public methods

        /// <summary>
        /// Calls Stored Procedure to delete all rows from the MOR actuals table
        /// </summary>
        public void DeleteAllFromOperations_MORActuals()
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
                        command.CommandText = DALSqlStatements.OperationsMORSQL.SQLSP_Operations_00100_Delete_MORActuals;

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

        /// <summary>
        /// Parses thru a data table of MOR Actuals data and parses it into a list of objects
        /// </summary>
        /// <param name="dt">A data table populated with MOR actuals data</param>
        /// <param name="list">A list of MOR actuals objects</param>
        public void ParseOperationsMOR_ActualDataTable(DataTable dt, List<OperationsMOR_Actual> list)
        {
            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);              

                OperationsMOR_Actual obj = new OperationsMOR_Actual();
                if (bindObj.ToStringValue("Yr") != "Year")
                {
                    obj.Yr =  populateString(bindObj, "Yr");
                    obj.Mth = populateString(bindObj, "Mth");
                    obj.PLT = populateString(bindObj, "PLT");                
                    obj.STE = populateDecimal(bindObj, "STE");
                    obj.REFREC = populateDecimal(bindObj, "REFREC");
                    obj.SUPWAS = populateDecimal(bindObj, "SUPWAS");
                    obj.REFPRO = populateDecimal(bindObj, "REFPRO");
                    obj.HHVCALC = populateDecimal(bindObj, "HHVCALC");
                    obj.GROELE = populateDecimal(bindObj, "GROELE");
                    obj.NETELE = populateDecimal(bindObj, "NETELE");
                    obj.PURPOW = populateDecimal(bindObj, "PURPOW");
                    obj.STEEXP = populateDecimal(bindObj, "STEEXP");
                    obj.FER = populateDecimal(bindObj, "FER");
                    obj.NONFER = populateDecimal(bindObj, "NONFER");
                    obj.ASH = populateDecimal(bindObj, "ASH");
                    obj.PEBLIM = populateDecimal(bindObj, "PEBLIM");
                    obj.HYDLIM = populateDecimal(bindObj, "HYDLIM");
                    obj.DOLLIM = populateDecimal(bindObj, "DOLLIM");
                    obj.AMM = populateDecimal(bindObj, "AMM");
                    obj.URE = populateDecimal(bindObj, "URE");
                    obj.CAR = populateDecimal(bindObj, "CAR");
                    obj.TUBLEA = populateDecimal(bindObj, "TUBLEA");
                    obj.AUXFUEOILPPS = populateDecimal(bindObj, "AUXFUEOILPPS");
                    obj.AUXNATGAS = populateDecimal(bindObj, "AUXNATGAS");
                    obj.AUXPROKCF = populateDecimal(bindObj, "AUXPROKCF");
                    obj.AUXPROKLB = populateDecimal(bindObj, "AUXPROKLB");
                    obj.AUXLANGAS = populateDecimal(bindObj, "AUXLANGAS");
                    obj.AUXWOOPAL = populateDecimal(bindObj, "AUXWOOPAL");
                    obj.B1FEETEM = populateDecimal(bindObj, "B1FEETEM");
                    obj.B1STEPRO = populateDecimal(bindObj, "B1STEPRO");
                    obj.B2STEPRO = populateDecimal(bindObj, "B2STEPRO");
                    obj.B3STEPRO = populateDecimal(bindObj, "B3STEPRO");
                    obj.B4STEPRO = populateDecimal(bindObj, "B4STEPRO");
                    obj.B5STEPRO = populateDecimal(bindObj, "B5STEPRO");
                    obj.B6STEPRO = populateDecimal(bindObj, "B6STEPRO");
                    obj.B1STETEM = populateDecimal(bindObj, "B1STETEM");
                    obj.B2STETEM = populateDecimal(bindObj, "B2STETEM");
                    obj.B3STETEM = populateDecimal(bindObj, "B3STETEM");
                    obj.B4STETEM = populateDecimal(bindObj, "B4STETEM");
                    obj.B5STETEM = populateDecimal(bindObj, "B5STETEM");
                    obj.B6STETEM = populateDecimal(bindObj, "B6STETEM");
                    obj.B1HCAT = populateDecimal(bindObj, "B1HCAT");
                    obj.B2HCAT = populateDecimal(bindObj, "B2HCAT");
                    obj.B3HCAT = populateDecimal(bindObj, "B3HCAT");
                    obj.B4HCAT = populateDecimal(bindObj, "B4HCAT");
                    obj.B5HCAT = populateDecimal(bindObj, "B5HCAT");
                    obj.B6HCAT = populateDecimal(bindObj, "B6HCAT");
                    obj.B1EEGT = populateDecimal(bindObj, "B1EEGT");
                    obj.B2EEGT = populateDecimal(bindObj, "B2EEGT");
                    obj.B3EEGT = populateDecimal(bindObj, "B3EEGT");
                    obj.B4EEGT = populateDecimal(bindObj, "B4EEGT");
                    obj.B5EEGT = populateDecimal(bindObj, "B5EEGT");
                    obj.B6EEGT = populateDecimal(bindObj, "B6EEGT");
                    obj.B1EEO2 = populateDecimal(bindObj, "B1EEO2");
                    obj.B2EEO2 = populateDecimal(bindObj, "B2EEO2");
                    obj.B3EEO2 = populateDecimal(bindObj, "B3EEO2");
                    obj.B4EEO2 = populateDecimal(bindObj, "B4EEO2");
                    obj.B5EEO2 = populateDecimal(bindObj, "B5EEO2");
                    obj.B6EEO2 = populateDecimal(bindObj, "B6EEO2");
                    obj.B1AHEGT = populateDecimal(bindObj, "B1AHEGT");
                    obj.B2AHEGT = populateDecimal(bindObj, "B2AHEGT");
                    obj.B3AHEGT = populateDecimal(bindObj, "B3AHEGT");
                    obj.B4AHEGT = populateDecimal(bindObj, "B4AHEGT");
                    obj.B5AHEGT = populateDecimal(bindObj, "B5AHEGT");
                    obj.B6AHEGT = populateDecimal(bindObj, "B6AHEGT");
                    obj.B1STEPRE = populateDecimal(bindObj, "B1STEPRE");
                    obj.B2STEPRE = populateDecimal(bindObj, "B2STEPRE");
                    obj.B3STEPRE = populateDecimal(bindObj, "B3STEPRE");
                    obj.B4STEPRE = populateDecimal(bindObj, "B4STEPRE");
                    obj.B5STEPRE = populateDecimal(bindObj, "B5STEPRE");
                    obj.B6STEPRE = populateDecimal(bindObj, "B6STEPRE");
                    obj.B1BHDT = populateDecimal(bindObj, "B1BHDT");
                    obj.B2BHDT = populateDecimal(bindObj, "B2BHDT");
                    obj.B3BHDT = populateDecimal(bindObj, "B3BHDT");
                    obj.B4BHDT = populateDecimal(bindObj, "B4BHDT");
                    obj.B5BHDT = populateDecimal(bindObj, "B5BHDT");
                    obj.B6BHDT = populateDecimal(bindObj, "B6BHDT");
                    obj.BOP = populateDecimal(bindObj, "BOP");
                    obj.BST = populateDecimal(bindObj, "BST");
                    obj.BOutSch = populateDecimal(bindObj, "BOutSch");
                    obj.BOutUNS = populateDecimal(bindObj, "BOutUNS");
                    obj.B1OP = populateDecimal(bindObj, "B1OP");
                    obj.B2OP = populateDecimal(bindObj, "B2OP");
                    obj.B3OP = populateDecimal(bindObj, "B3OP");
                    obj.B4OP = populateDecimal(bindObj, "B4OP");
                    obj.B5OP = populateDecimal(bindObj, "B5OP");
                    obj.B6OP = populateDecimal(bindObj, "B6OP");
                    obj.B1ST = populateDecimal(bindObj, "B1ST");
                    obj.B2ST = populateDecimal(bindObj, "B2ST");
                    obj.B3ST = populateDecimal(bindObj, "B3ST");
                    obj.B4ST = populateDecimal(bindObj, "B4ST");
                    obj.B5ST = populateDecimal(bindObj, "B5ST");
                    obj.B6ST = populateDecimal(bindObj, "B6ST");
                    obj.B1UN = populateDecimal(bindObj, "B1UN");
                    obj.B2UN = populateDecimal(bindObj, "B2UN");
                    obj.B3UN = populateDecimal(bindObj, "B3UN");
                    obj.B4UN = populateDecimal(bindObj, "B4UN");
                    obj.B5UN = populateDecimal(bindObj, "B5UN");
                    obj.B6UN = populateDecimal(bindObj, "B6UN");
                    obj.TOP = populateDecimal(bindObj, "TOP");
                    obj.TST = populateDecimal(bindObj, "TST");
                    obj.T1OP = populateDecimal(bindObj, "T1OP");
                    obj.T2OP = populateDecimal(bindObj, "T2OP");
                    obj.T1ST = populateDecimal(bindObj, "T1ST");
                    obj.T2ST = populateDecimal(bindObj, "T2ST");
                    obj.T1UN = populateDecimal(bindObj, "T1UN");
                    obj.T2UN = populateDecimal(bindObj, "T2UN");
                    obj.DaysInMonth = populateDecimal(bindObj, "DaysInMonth");
                    obj.NOOFBLRS = populateDecimal(bindObj, "NOOFBLRS");
                    obj.DESNOOFOPEUNI = populateDecimal(bindObj, "DESNOOFOPEUNI");
                    obj.DESNOOFTRBS = populateDecimal(bindObj, "DESNOOFTRBS");
                    obj.DESREFTPD = populateDecimal(bindObj, "DESREFTPD");
                    obj.DESHHV = populateDecimal(bindObj, "DESHHV");
                    obj.DESSTEPPH = populateDecimal(bindObj, "DESSTEPPH");
                    obj.DESTURMW = populateDecimal(bindObj, "DESTURMW");
                    obj.DESSTEPRE = populateDecimal(bindObj, "DESSTEPRE");
                    obj.DESSTETEM = populateDecimal(bindObj, "DESSTETEM");
                    obj.DESGROER = populateDecimal(bindObj, "DESGROER");
                    obj.DESNETER = populateDecimal(bindObj, "DESNETER");
                    obj.DESPEBLIM = populateDecimal(bindObj, "DESPEBLIM");
                    obj.DESAMM = populateDecimal(bindObj, "DESAMM");
                    obj.DESCAR = populateDecimal(bindObj, "DESCAR");
                    obj.DESFEETEM = populateDecimal(bindObj, "DESFEETEM");
                    obj.BASEEGT = populateDecimal(bindObj, "BASEEGT");
                    obj.BASHCAT = populateDecimal(bindObj, "BASHCAT");
                    obj.BASAHEGT = populateDecimal(bindObj, "BASAHEGT");
                    obj.BASXAIR = populateDecimal(bindObj, "BASXAIR");
                    obj.O2TYPE = populateString(bindObj, "O2TYPE");
                    obj.RefProExpMon = populateString(bindObj, "RefProExpMon");
                    obj.RefProExpYTD = populateString(bindObj, "RefProExpYTD");
                    obj.NetEleExpMon = populateString(bindObj, "NetEleExpMon");
                    obj.NetEleExpYTD = populateString(bindObj, "NetEleExpYTD");
                    obj.FerNonFerExpMon = populateString(bindObj, "FerNonFerExpMon");
                    obj.FerNonFerExpYTD = populateString(bindObj, "FerNonFerExpYTD");
                    obj.ExpSteExpMon = populateString(bindObj, "ExpSteExpMon");
                    obj.ExpSteExpYTD = populateString(bindObj, "ExpSteExpYTD");                 
                   
                    // we don't want the description row to be parsed, so if we are on that row, dont add it
                    if (obj.Yr != "Year")
                    {
                        list.Add(obj);
                    }
                }
            }
        }
                       
        /// <summary>
        /// Inserts a list of MOR actuals into the database
        /// </summary>
        /// <param name="list">List of MOR actuals</param>
        public void InsertOperationsMOR_Actuals(List<OperationsMOR_Actual> list)
        {
            //populate with the commandTostring for use with exception message
            string commandString = string.Empty;
            //int counter = 0;
            foreach (OperationsMOR_Actual MOR in list)
            {
                //counter++;
                //if (counter < 3000)
                //{
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(_dbConnection))
                        {
                            using (SqlCommand command = new SqlCommand())
                            {
                                command.Connection = connection;
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandText = DALSqlStatements.OperationsMORSQL.SQLSP_Operations_00300_Insert_MORActuals;
                                command.Parameters.AddWithValue("@Yr", MOR.Yr);
                                command.Parameters.AddWithValue("@Mth", MOR.Mth);
                                command.Parameters.AddWithValue("@PLT", MOR.PLT);
                                command.Parameters.AddWithValue("@STE", MOR.STE);
                                command.Parameters.AddWithValue("@REFREC", MOR.REFREC);
                                command.Parameters.AddWithValue("@SUPWAS", MOR.SUPWAS);
                                command.Parameters.AddWithValue("@REFPRO", MOR.REFPRO);
                                command.Parameters.AddWithValue("@HHVCALC", MOR.HHVCALC);
                                command.Parameters.AddWithValue("@GROELE", MOR.GROELE);
                                command.Parameters.AddWithValue("@NETELE", MOR.NETELE);
                                command.Parameters.AddWithValue("@PURPOW", MOR.PURPOW);
                                command.Parameters.AddWithValue("@STEEXP", MOR.STEEXP);
                                command.Parameters.AddWithValue("@FER", MOR.FER);
                                command.Parameters.AddWithValue("@NONFER", MOR.NONFER);
                                command.Parameters.AddWithValue("@ASH", MOR.ASH);
                                command.Parameters.AddWithValue("@PEBLIM", MOR.PEBLIM);
                                command.Parameters.AddWithValue("@HYDLIM", MOR.HYDLIM);
                                command.Parameters.AddWithValue("@DOLLIM", MOR.DOLLIM);
                                command.Parameters.AddWithValue("@AMM", MOR.AMM);
                                command.Parameters.AddWithValue("@URE", MOR.URE);
                                command.Parameters.AddWithValue("@CAR", MOR.CAR);
                                command.Parameters.AddWithValue("@TUBLEA", MOR.TUBLEA);
                                command.Parameters.AddWithValue("@AUXFUEOILPPS", MOR.AUXFUEOILPPS);
                                command.Parameters.AddWithValue("@AUXNATGAS", MOR.AUXNATGAS);
                                command.Parameters.AddWithValue("@AUXPROKCF", MOR.AUXPROKCF);
                                command.Parameters.AddWithValue("@AUXPROKLB", MOR.AUXPROKLB);
                                command.Parameters.AddWithValue("@AUXLANGAS", MOR.AUXLANGAS);
                                command.Parameters.AddWithValue("@AUXWOOPAL", MOR.AUXWOOPAL);
                                command.Parameters.AddWithValue("@B1FEETEM", MOR.B1FEETEM);
                                command.Parameters.AddWithValue("@B1STEPRO", MOR.B1STEPRO);
                                command.Parameters.AddWithValue("@B2STEPRO", MOR.B2STEPRO);
                                command.Parameters.AddWithValue("@B3STEPRO", MOR.B3STEPRO);
                                command.Parameters.AddWithValue("@B4STEPRO", MOR.B4STEPRO);
                                command.Parameters.AddWithValue("@B5STEPRO", MOR.B5STEPRO);
                                command.Parameters.AddWithValue("@B6STEPRO", MOR.B6STEPRO);
                                command.Parameters.AddWithValue("@B1STETEM", MOR.B1STETEM);
                                command.Parameters.AddWithValue("@B2STETEM", MOR.B2STETEM);
                                command.Parameters.AddWithValue("@B3STETEM", MOR.B3STETEM);
                                command.Parameters.AddWithValue("@B4STETEM", MOR.B4STETEM);
                                command.Parameters.AddWithValue("@B5STETEM", MOR.B5STETEM);
                                command.Parameters.AddWithValue("@B6STETEM", MOR.B6STETEM);
                                command.Parameters.AddWithValue("@B1HCAT", MOR.B1HCAT);
                                command.Parameters.AddWithValue("@B2HCAT", MOR.B2HCAT);
                                command.Parameters.AddWithValue("@B3HCAT", MOR.B3HCAT);
                                command.Parameters.AddWithValue("@B4HCAT", MOR.B4HCAT);
                                command.Parameters.AddWithValue("@B5HCAT", MOR.B5HCAT);
                                command.Parameters.AddWithValue("@B6HCAT", MOR.B6HCAT);
                                command.Parameters.AddWithValue("@B1EEGT", MOR.B1EEGT);
                                command.Parameters.AddWithValue("@B2EEGT", MOR.B2EEGT);
                                command.Parameters.AddWithValue("@B3EEGT", MOR.B3EEGT);
                                command.Parameters.AddWithValue("@B4EEGT", MOR.B4EEGT);
                                command.Parameters.AddWithValue("@B5EEGT", MOR.B5EEGT);
                                command.Parameters.AddWithValue("@B6EEGT", MOR.B6EEGT);
                                command.Parameters.AddWithValue("@B1EEO2", MOR.B1EEO2);
                                command.Parameters.AddWithValue("@B2EEO2", MOR.B2EEO2);
                                command.Parameters.AddWithValue("@B3EEO2", MOR.B3EEO2);
                                command.Parameters.AddWithValue("@B4EEO2", MOR.B4EEO2);
                                command.Parameters.AddWithValue("@B5EEO2", MOR.B5EEO2);
                                command.Parameters.AddWithValue("@B6EEO2", MOR.B6EEO2);
                                command.Parameters.AddWithValue("@B1AHEGT", MOR.B1AHEGT);
                                command.Parameters.AddWithValue("@B2AHEGT", MOR.B2AHEGT);
                                command.Parameters.AddWithValue("@B3AHEGT", MOR.B3AHEGT);
                                command.Parameters.AddWithValue("@B4AHEGT", MOR.B4AHEGT);
                                command.Parameters.AddWithValue("@B5AHEGT", MOR.B5AHEGT);
                                command.Parameters.AddWithValue("@B6AHEGT", MOR.B6AHEGT);
                                command.Parameters.AddWithValue("@B1STEPRE", MOR.B1STEPRE);
                                command.Parameters.AddWithValue("@B2STEPRE", MOR.B2STEPRE);
                                command.Parameters.AddWithValue("@B3STEPRE", MOR.B3STEPRE);
                                command.Parameters.AddWithValue("@B4STEPRE", MOR.B4STEPRE);
                                command.Parameters.AddWithValue("@B5STEPRE", MOR.B5STEPRE);
                                command.Parameters.AddWithValue("@B6STEPRE", MOR.B6STEPRE);
                                command.Parameters.AddWithValue("@B1BHDT", MOR.B1BHDT);
                                command.Parameters.AddWithValue("@B2BHDT", MOR.B2BHDT);
                                command.Parameters.AddWithValue("@B3BHDT", MOR.B3BHDT);
                                command.Parameters.AddWithValue("@B4BHDT", MOR.B4BHDT);
                                command.Parameters.AddWithValue("@B5BHDT", MOR.B5BHDT);
                                command.Parameters.AddWithValue("@B6BHDT", MOR.B6BHDT);
                                command.Parameters.AddWithValue("@BOP", MOR.BOP);
                                command.Parameters.AddWithValue("@BST", MOR.BST);
                                command.Parameters.AddWithValue("@BOutSch", MOR.BOutSch);
                                command.Parameters.AddWithValue("@BOutUNS", MOR.BOutUNS);
                                command.Parameters.AddWithValue("@B1OP", MOR.B1OP);
                                command.Parameters.AddWithValue("@B2OP", MOR.B2OP);
                                command.Parameters.AddWithValue("@B3OP", MOR.B3OP);
                                command.Parameters.AddWithValue("@B4OP", MOR.B4OP);
                                command.Parameters.AddWithValue("@B5OP", MOR.B5OP);
                                command.Parameters.AddWithValue("@B6OP", MOR.B6OP);
                                command.Parameters.AddWithValue("@B1ST", MOR.B1ST);
                                command.Parameters.AddWithValue("@B2ST", MOR.B2ST);
                                command.Parameters.AddWithValue("@B3ST", MOR.B3ST);
                                command.Parameters.AddWithValue("@B4ST", MOR.B4ST);
                                command.Parameters.AddWithValue("@B5ST", MOR.B5ST);
                                command.Parameters.AddWithValue("@B6ST", MOR.B6ST);
                                command.Parameters.AddWithValue("@B1UN", MOR.B1UN);
                                command.Parameters.AddWithValue("@B2UN", MOR.B2UN);
                                command.Parameters.AddWithValue("@B3UN", MOR.B3UN);
                                command.Parameters.AddWithValue("@B4UN", MOR.B4UN);
                                command.Parameters.AddWithValue("@B5UN", MOR.B5UN);
                                command.Parameters.AddWithValue("@B6UN", MOR.B6UN);
                                command.Parameters.AddWithValue("@TOP", MOR.TOP);
                                command.Parameters.AddWithValue("@TST", MOR.TST);
                                command.Parameters.AddWithValue("@TUN", MOR.TUN);
                                command.Parameters.AddWithValue("@T1OP", MOR.T1OP);
                                command.Parameters.AddWithValue("@T2OP", MOR.T2OP);
                                command.Parameters.AddWithValue("@T1ST", MOR.T1ST);
                                command.Parameters.AddWithValue("@T2ST", MOR.T2ST);
                                command.Parameters.AddWithValue("@T1UN", MOR.T1UN);
                                command.Parameters.AddWithValue("@T2UN", MOR.T2UN);
                                command.Parameters.AddWithValue("@DaysInMonth", MOR.DaysInMonth);
                                command.Parameters.AddWithValue("@NOOFBLRS", MOR.NOOFBLRS);
                                command.Parameters.AddWithValue("@DESNOOFOPEUNI", MOR.DESNOOFOPEUNI);
                                command.Parameters.AddWithValue("@DESNOOFTRBS", MOR.DESNOOFTRBS);
                                command.Parameters.AddWithValue("@DESREFTPD", MOR.DESREFTPD);
                                command.Parameters.AddWithValue("@DESHHV", MOR.DESHHV);
                                command.Parameters.AddWithValue("@DESSTEPPH", MOR.DESSTEPPH);
                                command.Parameters.AddWithValue("@DESTURMW", MOR.DESTURMW);
                                command.Parameters.AddWithValue("@DESSTEPRE", MOR.DESSTEPRE);
                                command.Parameters.AddWithValue("@DESSTETEM", MOR.DESSTETEM);
                                command.Parameters.AddWithValue("@DESGROER", MOR.DESGROER);
                                command.Parameters.AddWithValue("@DESNETER", MOR.DESNETER);
                                command.Parameters.AddWithValue("@DESPEBLIM", MOR.DESPEBLIM);
                                command.Parameters.AddWithValue("@DESAMM", MOR.DESAMM);
                                command.Parameters.AddWithValue("@DESCAR", MOR.DESCAR);
                                command.Parameters.AddWithValue("@DESFEETEM", MOR.DESFEETEM);
                                command.Parameters.AddWithValue("@BASEEGT", MOR.BASEEGT);
                                command.Parameters.AddWithValue("@BASHCAT", MOR.BASHCAT);
                                command.Parameters.AddWithValue("@BASAHEGT", MOR.BASAHEGT);
                                command.Parameters.AddWithValue("@BASXAIR", MOR.BASXAIR);
                                command.Parameters.AddWithValue("@O2TYPE", MOR.O2TYPE);                               
                                command.Parameters.AddWithValue("@RefProExpMon", MOR.RefProExpMon);
                                command.Parameters.AddWithValue("@RefProExpYTD", MOR.RefProExpYTD);
                                command.Parameters.AddWithValue("@NetEleExpMon", MOR.NetEleExpMon);
                                command.Parameters.AddWithValue("@NetEleExpYTD", MOR.NetEleExpYTD);
                                command.Parameters.AddWithValue("@FerNonFerExpMon", MOR.FerNonFerExpMon);
                                command.Parameters.AddWithValue("@FerNonFerExpYTD", MOR.FerNonFerExpYTD);
                                command.Parameters.AddWithValue("@ExpSteExpMon", MOR.ExpSteExpMon);
                                command.Parameters.AddWithValue("@ExpSteExpYTD", MOR.ExpSteExpYTD);                           

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
              //  }
            }
        }


        #endregion

        #region private methods

        private void loadEmployeeInfoList(DataTable dt, List<EmployeeInfo> list)
        {
            list.Clear();

            foreach (DataRow dr in dt.Rows)
            {
                BindDataRowToProperty bindObj = new BindDataRowToProperty(dr);

                EmployeeInfo obj = new EmployeeInfo();
                obj.Run_date = bindObj.ToDate("Run_date");
                obj.EE_ID = bindObj.ToStringValue("EE_ID");
                obj.Salary_Class = bindObj.ToStringValue("Salary_Class");
                obj.PayRateType = bindObj.ToStringValue("PayRateType");
                obj.Exempt = bindObj.ToStringValue("Exempt");
                obj.SSN = bindObj.ToStringValue("SSN");
                obj.WD_UserName = bindObj.ToStringValue("WD_UserName");
                obj.LAST = bindObj.ToStringValue("LAST");
                obj.FIRST = bindObj.ToStringValue("FIRST");
                obj.MIDDLE = bindObj.ToStringValue("MIDDLE");
                obj.Suffix = bindObj.ToStringValue("Suffix");
                obj.HOME_ADDR1 = bindObj.ToStringValue("HOME_ADDR1");
                obj.HOME_ADDR2 = bindObj.ToStringValue("HOME_ADDR2");
                obj.CITY = bindObj.ToStringValue("CITY");
                obj.STATE = bindObj.ToStringValue("STATE");
                obj.ZIP = bindObj.ToStringValue("ZIP");
                obj.Country = bindObj.ToStringValue("Country");
                obj.MARITAL = bindObj.ToStringValue("MARITAL");
                obj.Status = bindObj.ToStringValue("Status");
                obj.Termination_Date = bindObj.ToDate("Termination_Date");
                obj.OrigHireDate = bindObj.ToDate("OrigHireDate");
                obj.Work_Email = bindObj.ToStringValue("Work_Email");
                obj.JobTitle = bindObj.ToStringValue("JobTitle");
                obj.DivisionID = bindObj.ToStringValue("DivisionID");
                obj.PayGroup = bindObj.ToStringValue("PayGroup");
                obj.UnionName = bindObj.ToStringValue("UnionName");
                obj.RegionID = bindObj.ToStringValue("RegionID");
                obj.CompanyName = bindObj.ToStringValue("CompanyName");
                obj.Location = bindObj.ToStringValue("Location");
                obj.Department = bindObj.ToStringValue("Department");
                obj.Work_Addr1 = bindObj.ToStringValue("Work_Addr1");
                obj.Work_Addr2 = bindObj.ToStringValue("Work_Addr2");
                obj.Work_City = bindObj.ToStringValue("Work_City");
                obj.Work_State = bindObj.ToStringValue("Work_State");
                obj.Work_Country = bindObj.ToStringValue("Work_Country");
                obj.Work_Postal = bindObj.ToStringValue("Work_Postal");
                obj.Work_phone = bindObj.ToStringValue("Work_phone");
                obj.Mgr_FullName = bindObj.ToStringValue("Mgr_FullName");
                obj.Mgr_EE_ID = bindObj.ToStringValue("Mgr_EE_ID");
                obj.Mgr_Email = bindObj.ToStringValue("Mgr_Email");
                list.Add(obj);
            }
        }

        private string populateString(BindDataRowToProperty bindObj, string p)
        {
            if (bindObj.ToStringValue(p) == null)
            {
                return String.Empty;
            }
            else
            {
                return bindObj.ToStringValue(p);
            }
        }

        private decimal populateDecimal(BindDataRowToProperty bindObj, string p)
        {
            decimal returnValue = 0;
            if (bindObj.ToStringValue(p) == null)
            {
                return 0;
            }
            else
            {
                if (decimal.TryParse(bindObj.ToStringValue(p), out returnValue))
                {
                    return returnValue;
                }
                else
                {
                    return 0;
                }
            }
        }
        

        #endregion
    }
}
