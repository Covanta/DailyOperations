using Covanta.Utilities;
using Covanta.Utilities.Helpers;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Covanta.UI.EnergyMeterFTP
{
    class Program
    {
        static void Main(string[] args)
        {
            if (isAppAlreadyRunning())
            {
                return;
            }
            //int _returnCodeFromSFTP = 0;

            try
            {
                //----- Old code developed by Brian Litwin

                //FileCopierWithWinSCPnew fc = new FileCopierWithWinSCPnew();
                //_returnCodeFromSFTP = fc.CopyProcess();

                //if (_returnCodeFromSFTP == 0)
                //{
                //}
                //else
                //{
                //    EmailHelper.SendEmail("Error while processing .NET job - EnergyMeterFTP. " + "WinSCP returned a code of: " + _returnCodeFromSFTP);
                //}

                //----- New code developed by Arjun on 20-12-2022

                string privateKeyPath = ConfigurationManager.AppSettings["PrivateKeyPath"];
                string sftpUser = ConfigurationManager.AppSettings["SFTPUser"];
                string sftpHost = ConfigurationManager.AppSettings["SFTPHost"];
                string remoteDirectory = ConfigurationManager.AppSettings["SFTPRemoteDirectory"];
                string localDirectory = ConfigurationManager.AppSettings["LocalDirectoryPath"];
                string fileExtension = ConfigurationManager.AppSettings["FileExtension"];

                PrivateKeyFile ObjPrivateKey = null;
                PrivateKeyAuthenticationMethod ObjPrivateKeyAutentication = null;
                using (Stream stream = File.OpenRead(privateKeyPath))
                {
                    ObjPrivateKey = new PrivateKeyFile(stream);
                    ObjPrivateKeyAutentication = new PrivateKeyAuthenticationMethod(sftpUser, ObjPrivateKey);
                    ConnectionInfo objConnectionInfo = new ConnectionInfo(sftpHost, 22, sftpUser, ObjPrivateKeyAutentication);
                    
                    using (var sftp = new SftpClient(objConnectionInfo))
                    {
                        sftp.Connect();
                        var files = sftp.ListDirectory(remoteDirectory);

                        foreach (var file in files)
                        {
                            string remoteFileName = file.Name;
                            if (file.Name.EndsWith(fileExtension))
                            {
                                using (Stream file1 = File.Create(localDirectory + remoteFileName))
                                {
                                    sftp.DownloadFile(remoteDirectory + remoteFileName, file1);
                                    //sftp.DeleteFile(remoteDirectory + remoteFileName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log exception
                EmailHelper.SendEmail("Error while processing .NET job - EnergyMeterFTP. " + ex.Message);
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
