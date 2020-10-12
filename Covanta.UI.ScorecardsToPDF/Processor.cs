using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Covanta.BusinessLogic;

namespace Covanta.UI.ScorecardsToPDF
{
    public class Processor
    {
        public void processFiles()
        {
            ScorecardsToPDFManager manager = new ScorecardsToPDFManager();           
            manager.ProcessFiles();
        }
    }
}
