using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Covanta.BusinessLogic;

namespace CitectExcelConsolidate
{
    public class Processor
    {
        public void processFiles()
        {
            CitectManager manager = new CitectManager();
            manager.ProcessFiles();
        }
       
    }
}
