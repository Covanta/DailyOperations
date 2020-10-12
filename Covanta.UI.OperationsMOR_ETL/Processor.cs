using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.BusinessLogic;
using System.Configuration;

namespace Covanta.UI.OperationsMOR_ETL
{
    public  class Processor
    {
        string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;

        public void processFiles()
        {
            OperationsMORManager manager = new OperationsMORManager(_connStringCovStaging);
            manager.ProcessFiles();


        }
    }
}
