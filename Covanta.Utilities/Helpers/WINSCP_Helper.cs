using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using Covanta.Utilities.Helpers;
using System.Net.Mail;

namespace Covanta.Utilities.Helpers
{
    public class WINSCP_Helper
    {
        #region private variables

        string _WinSCPPath;
        string _WinSCPParamaters;
        string _logPath;
        int _returnCode;
        
        #endregion

        #region constructors

        /// <summary>
        /// Main constuctor of the class
        /// </summary>
        public WINSCP_Helper()
        {
            _WinSCPPath = ConfigurationManager.AppSettings["WinSCPPath"];
            _WinSCPParamaters = ConfigurationManager.AppSettings["WinSCPParamaters"];
            _logPath = ConfigurationManager.AppSettings["logPath"];        
        }

        #endregion

        #region public methods

        public int CopyProcess()
        {
              _returnCode = 0;
              _returnCode = callCommandLine();             
              return _returnCode;             
        }

        #endregion

        #region private methods

        private int callCommandLine()
        {            
            int exitCode;
            int Timeout = 50000;            
            ProcessStartInfo startInfo;
            Process Process;
            string command = _WinSCPPath + _WinSCPParamaters;       
            startInfo = new ProcessStartInfo("cmd.exe", "/C " + command);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false; //required to redirect
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;            
            Process = Process.Start(startInfo);
            Process.WaitForExit(Timeout);
            exitCode = Process.ExitCode;
            Process.Close();
            return exitCode;
        }

      
        #endregion
    }
}