using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Covanta.Entities;
using System.Configuration;
using Covanta.Utilities.Helpers;

namespace Covanta.UI.AutomatedJobsStatusEmailer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (isAppAlreadyRunning())
            {
                return;
            }
           
            string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;
            AutomatedJobDetails _jobDetails = new AutomatedJobDetails();

            try
            {

                StatusProcessor sp = new StatusProcessor();
                sp.Process();

                //// test exception
                //int xx = 0;
                //int c = 3;
                //int d = c / xx;
            }
            catch (Exception ex)
            {
                //log exception
                EmailHelper.SendEmail("Error while processing .NET job - AutomatedJobsStatusEmailer. " + ex.Message);
            }
        }

        private static bool isAppAlreadyRunning()
        {
            Process[] appProc;
            string strModName;
            string strProcName;
            strModName = Process.GetCurrentProcess().MainModule.ModuleName;
            strProcName = System.IO.Path.GetFileNameWithoutExtension(strModName);
            appProc = Process.GetProcessesByName(strProcName);
            if (appProc.Length > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
