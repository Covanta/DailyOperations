using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Covanta.BusinessLogic;

namespace Covanta.UI.EnvironmentalEmissionsETL
{
    public class Processor
    {
        string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;

        public void processFiles()
        {
            EnvironmentalEmissionsManager manager = new EnvironmentalEmissionsManager(_connStringCovStaging);
            manager.ProcessFiles();

        }
    }
}
