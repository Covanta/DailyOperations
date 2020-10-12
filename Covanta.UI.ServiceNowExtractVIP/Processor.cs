using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Covanta.BusinessLogic;
using Covanta.Entities;

namespace Covanta.UI.ServiceNowExtractVIP
{
    public class Processor
    {
        string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;
        string _path = ConfigurationManager.AppSettings["savePath"];
        List<ServiceNowVIP> list = new List<ServiceNowVIP>();

        #region public methods
        public void processData()
        {
            loadDataToEntityList();
            ServiceNowExtractVIPManager manager = new ServiceNowExtractVIPManager(_connStringCovStaging);
            manager.DeleteServiceNowVIPData();
            manager.SaveServiceNowVIPData(list);


        }

        #endregion

        #region private methods
        private void loadDataToEntityList()
        {
            using (StreamReader readFile = new StreamReader(_path))
            {
                string line;
                string[] row;
                int count = 0;
                while ((line = readFile.ReadLine()) != null)
                {
                    //the first row has titles, so skip it
                    if (count != 0)
                    {
                        row = line.Split(',');
                        ServiceNowVIP sn = new ServiceNowVIP();
                        if (row.Length == 6)
                        {
                            sn.VIP = 1;
                            sn.UserName = row[0].Replace("\"", "");
                            sn.Name = row[1].Replace("\"", "");
                            sn.Email = row[2].Replace("\"", "");
                            if (row[3].Replace("\"", "").ToUpper() == "TRUE")     {  sn.Active = 1;  }  else   {sn.Active = 0; }
                            //Created Date
                            if (tryParseDate(row[4].Replace("\"", "")) == true)
                            {
                                sn.SN_CreatedDate = DateTime.Parse(row[4].Replace("\"", ""));
                            }
                            else
                            {
                                sn.SN_CreatedDate = DateTime.Today;
                            }

                            //Updated Date
                            if (tryParseDate(row[5].Replace("\"", "")) == true)
                            {
                                sn.SN_UpdateDate = DateTime.Parse(row[5].Replace("\"", ""));
                            }
                            else
                            {
                                sn.SN_UpdateDate = DateTime.Today;
                            }
                            // if userid is already on the list, then don't add it again
                            if (list.Exists(xx => xx.UserName == sn.UserName) == false)
                            {
                                list.Add(sn);
                            }
                        }
                    }
                    count++;
                }
            }

        }

        private bool tryParseDate(string date)
        {
            bool result = false;
            DateTime outDate = DateTime.Today;
            result = DateTime.TryParse(date, out outDate);
            return result;
        }

        #endregion
    }
}
