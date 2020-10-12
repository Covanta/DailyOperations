using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.BusinessLogic;
using System.Configuration;

namespace Covanta.UI.C4RWasteModel_ETL
{
    public  class Processor
    {
        string _connStringCovIntegration = ConfigurationManager.ConnectionStrings["covIntegrationConnString"].ConnectionString;

        public void processFiles()
        {
            C4RWasteModelManager manager = new C4RWasteModelManager(_connStringCovIntegration);
            manager.ProcessFiles();
        }
    }
}
