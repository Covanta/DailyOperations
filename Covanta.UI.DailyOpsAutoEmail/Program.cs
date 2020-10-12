using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Covanta.Utilities.Helpers;

namespace Covanta.UI.DailyOpsAutoEmail
{
    class Program
    {
        public const string FACILITY_MANAGER_EMAIL = "FME", CHIEF_ENGINEER_EMAIL = "CEE", MORNING_EXCEPTIONS_REPORT_EMAIL = "MERE";

        static void Main(string[] args)
        {           
           //  protection from running in DEV
           //EmailHelper.SendEmail("Execution has been prevented. Please remove the return statement at the top of Main to restart execution.");
           //return;

            if (isAppAlreadyRunning())
            {
                return;
            }


            ////Delete this after test 11/11/2013
            //if (args.Length == 0)
            //{
            //    DailyOpsEmailer.sendMorningExceptionsReportEmail();
            //    return;
            //}
            ////Delete this after test 11/11/2013



            if (args.Length == 1)
            {
                string emailType = args[0];

				try
				{
					if (emailType.Equals(FACILITY_MANAGER_EMAIL))
						DailyOpsEmailer.sendFacilityManagerEmails();
					else if (emailType.Equals(CHIEF_ENGINEER_EMAIL))
						DailyOpsEmailer.sendChiefEngineerEmails();
					else if (emailType.Equals(MORNING_EXCEPTIONS_REPORT_EMAIL))
						DailyOpsEmailer.sendMorningExceptionsReportEmail();
					else
						EmailHelper.SendEmail(string.Format("No email type match for {0} - no email sent.", emailType));

					//EmailHelper.SendEmail(string.Format("Daily Ops Auto Email Program executed successfully with parameter {0}.", 	emailType));
				}
				catch (Exception ex)
				{
					//log exception
					EmailHelper.SendEmail(string.Format("Error while processing DailyOpsAutoEmail with parameter {0}. {1}",
						emailType, ex.Message));
				}
			}

			else if (args.Length == 2)
			{
				string emailType = args[0];
				string offset = args[1];

				try
				{
					if (emailType.Equals(FACILITY_MANAGER_EMAIL))
						DailyOpsEmailer.sendFacilityManagerEmails(offset);
					else if (emailType.Equals(CHIEF_ENGINEER_EMAIL))
						DailyOpsEmailer.sendChiefEngineerEmails(offset);
					else
						EmailHelper.SendEmail(string.Format("No email type match for {0} {1} - no email sent.", emailType, offset));

					EmailHelper.SendEmail(string.Format("Daily Ops Auto Email Program executed successfully with parameters {0} and {1}.", 
						emailType, offset));
				}
				catch (Exception ex)
				{
					//log exception
					EmailHelper.SendEmail(string.Format("Error while processing DailyOpsAutoEmail with parameters {0} and {1}. {2}",
						emailType, offset, ex.Message));
				}
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
