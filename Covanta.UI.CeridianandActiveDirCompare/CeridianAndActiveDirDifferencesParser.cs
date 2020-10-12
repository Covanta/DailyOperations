using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Covanta.BusinessLogic;
using Covanta.Entities;
using Covanta.Utilities.Helpers;

namespace Covanta.UI.CeridianandActiveDirCompare
{
    public class CeridianAndActiveDirDifferencesParser
    {
        #region private variables

        string _covStagingConnection;
        List<CeridianAndADSyncInputRow> _fullInputList = null;
        List<CeridianAndADSyncOututRow> _fullOutputList = null;
        int _inRowCount = 0;
        int _rowDifferencesCount = 0;

        #endregion

        #region constructors

        /// <summary>
        /// Main constuctor of the class
        /// </summary>
        public CeridianAndActiveDirDifferencesParser()
        {
            _covStagingConnection = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Main method of the class.  Only public method.
        /// </summary>
        public void ProcessJob()
        {
            getAD_Ceridian_Merge_VW_Data_Rows();
            processCeridianAndADSyncInput();
            writeOutputRowsToDatabase();
            insertIntoProposedSamAccountNamesTable();
            writeADSyncPropossedSAMAcctNoConflictDataToCSV();
            sendCompletedEmail();
        }

        #endregion

        #region private methods

        private void getAD_Ceridian_Merge_VW_Data_Rows()
        {
            CeridianAndActiveDirDifferencesManager manager = new CeridianAndActiveDirDifferencesManager(_covStagingConnection);
            _fullInputList = manager.GetCeridianAndADSyncInputRowList();
            manager = null;
        }

        private void insertIntoProposedSamAccountNamesTable()
        {
            CeridianAndActiveDirDifferencesManager manager = new CeridianAndActiveDirDifferencesManager(_covStagingConnection);
            manager.InsertIntoProposedSamAccountNamesTable();
        }

        private void sendCompletedEmail()
        {
            EmailHelper.SendEmail("Ceridian And ActiveDir Differences parser completed.  "
                + _fullInputList.Count.ToString() + " rows input " + Environment.NewLine
                + _rowDifferencesCount.ToString() + " rows with differences.");
        }

        private void processCeridianAndADSyncInput()
        {
            _fullOutputList = new List<CeridianAndADSyncOututRow>();
            _inRowCount = _fullInputList.Count;

            foreach (CeridianAndADSyncInputRow inRow in _fullInputList)
            {
                CeridianAndADSyncOututRow outRow = new CeridianAndADSyncOututRow();
                processInputRow(inRow, outRow);
                _fullOutputList.Add(outRow);

                if (outRow.Flag_AtLeastOneDifferenceFound == "Y") { _rowDifferencesCount++; }
                outRow = null;
            }
        }

        private void processInputRow(CeridianAndADSyncInputRow inRow, CeridianAndADSyncOututRow outRow)
        {
            //First Name       
            outRow.Flag_Different_FirstName = compareInRowAndOutRowString(inRow.AD_GivenName, inRow.cer_FirstName, outRow);
            //Last Name  
            outRow.Flag_Different_Lastname = compareInRowAndOutRowString(inRow.AD_Surname, inRow.cer_LastName, outRow);
            //Manager Name  
            //outRow.Flag_Different_Manager = compareInRowAndOutRowString(inRow.AD_Manager, inRow.cer_Employee_Supervisor_Name, outRow);
            //Dept  
            outRow.Flag_Different_Title = compareInRowAndOutRowString(inRow.AD_Title, inRow.cer_Title, outRow);

            parseInRowToOutRow(inRow, outRow);

            return;
        }

        private string compareInRowAndOutRowString(string field1, string field2, CeridianAndADSyncOututRow outRow)
        {
            if (field1 != field2)
            {
                outRow.Flag_AtLeastOneDifferenceFound = "Y";
                return "Y";
            }
            else
            {
                return "N";
            }
        }

        private void parseInRowToOutRow(CeridianAndADSyncInputRow inRow, CeridianAndADSyncOututRow outRow)
        {
            outRow.AD_Department = inRow.AD_Department;
            outRow.ad_Employeeid = inRow.ad_Employeeid;
            outRow.AD_GivenName = inRow.AD_GivenName;
            outRow.AD_Initials = inRow.AD_Initials;
            outRow.AD_MailNickname = inRow.AD_MailNickname;
            outRow.AD_Manager = inRow.AD_Manager;
            outRow.AD_Phone = inRow.AD_Phone;
            outRow.AD_PhysicalDeliveryOfficeName = inRow.AD_PhysicalDeliveryOfficeName;
            outRow.AD_SamAccountName = inRow.AD_SamAccountName;
            outRow.AD_Surname = inRow.AD_Surname;
            outRow.AD_Title = inRow.AD_Title;
            outRow.cer_ClockNumber = inRow.cer_ClockNumber;
            DateTime dummyDate = DateTime.Parse("1900/01/01");
            if (inRow.cer_DateEntered < dummyDate) { outRow.cer_DateEntered = dummyDate; } else { outRow.cer_DateEntered = inRow.cer_DateEntered; }
            if (inRow.cer_DateLastHired < dummyDate) { outRow.cer_DateLastHired = dummyDate; } else { outRow.cer_DateLastHired = inRow.cer_DateLastHired; }
            outRow.cer_DayBorn = inRow.cer_DayBorn;
            outRow.Cer_Dept = inRow.Cer_Dept;
            outRow.cer_Division = inRow.cer_Division;
            outRow.cer_EbPSID = inRow.cer_EbPSID;
            outRow.cer_Employee_Supervisor_Name = inRow.cer_Employee_Supervisor_Name;
            outRow.cer_ExemptStatus = inRow.cer_ExemptStatus;
            outRow.cer_FirstName = inRow.cer_FirstName;
            outRow.cer_LastName = inRow.cer_LastName;
            outRow.cer_Location = inRow.cer_Location;
            outRow.cer_Mid_Init = inRow.cer_Mid_Init;
            outRow.cer_MonthBorn = inRow.cer_MonthBorn;
            outRow.cer_OfficePhone = inRow.cer_OfficePhone;
            outRow.cer_PayGroup = inRow.cer_PayGroup;
            outRow.cer_PayType = inRow.cer_PayType;
            outRow.cer_Region = inRow.cer_Region;
            outRow.cer_SSN = inRow.cer_SSN;
            outRow.cer_Status = inRow.cer_Status;
            outRow.cer_Suffix = inRow.cer_Suffix;
            outRow.cer_Supervisor_Flxid = inRow.cer_Supervisor_Flxid;
            if (inRow.cer_TermDate < dummyDate) { outRow.cer_TermDate = dummyDate; } else { outRow.cer_TermDate = inRow.cer_TermDate; }
            outRow.cer_Title = inRow.cer_Title;
            outRow.cer_Union = inRow.cer_Union;
            outRow.cer_UnionIndicator = inRow.cer_UnionIndicator;
        }

        private void writeOutputRowsToDatabase()
        {
            CeridianAndActiveDirDifferencesManager manager = new CeridianAndActiveDirDifferencesManager(_covStagingConnection);
            manager.WriteOutputRowsToDatabase(_fullOutputList);
            manager = null;
        }

        private void writeADSyncPropossedSAMAcctNoConflictDataToCSV()
        {
            //get data fromo database
            CeridianAndActiveDirDifferencesManager manager = new CeridianAndActiveDirDifferencesManager(_covStagingConnection);
            List<CeridianAndADSyncProposedSAMwithNoConflict> list = manager.GetCeridianAndADSyncProposedSAMwithNoConflictList();
           
            //write to CSV file

            string CSVPathAndFile = ConfigurationManager.AppSettings["CSVPathAndFile"];

            manager.WriteCeridianAndADSyncProposedSAMwithNoConflictListToCSV(list, CSVPathAndFile);
        }

        #endregion
    }
}
