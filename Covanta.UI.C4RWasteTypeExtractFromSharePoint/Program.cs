using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Covanta.Utilities.Helpers;

namespace Covanta.UI.C4RWasteTypeExtractFromSharePoint
{
    class Program
    {
        static void Main(string[] args)
        {
            if (isAppAlreadyRunning())
            {
                return;
            }

            try
            {
                C4RWasteTypeMasterApprovalNumParser parser = new C4RWasteTypeMasterApprovalNumParser();
                parser.ProcessJob();                
            }
            catch (Exception ex)
            {
                //log exception
                EmailHelper.SendEmail("Error while processing C4RWasteTypeExtractFromSharePoint. " + ex.Message);
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
