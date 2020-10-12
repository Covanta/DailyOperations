using Covanta.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covanta.UI.EnergyMeterETL
{
    public class Processor
    {
        string _connStringCovStaging = ConfigurationManager.ConnectionStrings["covStagingConnString"].ConnectionString;

        public void processFiles()
        {         
            EnergyMeterManager manager = new EnergyMeterManager(_connStringCovStaging);
            manager.Process();
        }
    }
}
