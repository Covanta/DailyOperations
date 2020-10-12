using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Covanta.BusinessLogic;

namespace Covanta.UI.C4RRevShareLongviewCalc2014
{
    public class Processor
    {
        string _connStringCovIntegration = ConfigurationManager.ConnectionStrings["covIntegrationConnString"].ConnectionString;
        string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;

        public void processFiles()
        {
            C4RRevShareLongviewCalc2014Manager manager = new C4RRevShareLongviewCalc2014Manager();
            manager.Process1(_connStringCovStaging, _connStringCovIntegration);

        }
    }
}
