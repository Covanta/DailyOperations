using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.BusinessLogic;
using System.Configuration;

namespace Covanta.UI.WorkDayPayAccrualETL
{
    public  class Processor
    {
        string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;

        public void processFiles()
        {
            WorkDayPayManager manager = new WorkDayPayManager(_connStringCovStaging);
            manager.Process();
        }
    }
}
