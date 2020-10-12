using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Common.Enums
{
    public static class Enums
    {

        //enums
        public enum StatusEnum
        {
            NotSet,
            OK,
            Error,
            InvalidDate,
            InvalidFacility
        }

        public enum AutomatedJobNameEnum
        {
            LongviewExtract,
            C4RForecastApprovals,
            WorkDaySFTP,
            SSIS_WorkDayExtract,
            AutomatedJobStatusEmailer,
            SSIS_WorkDayPushToCSV,
            SSIS_LoadDBFromADirCSV,
            None
        }

        /// <summary>
        /// 0 is success, 1 is Fail, 2 is ResetFromEmailerJob
        /// </summary>
        public enum AutomatedJobStatusEnum
        {
            Success,
            Fail,
            Reset
          //  ResetFromEmailerJob          
        }

        public enum DowntimeBoilerEnum
        {
            DowntimeBoiler1,
            DowntimeBoiler2,
            DowntimeBoiler3,
            DowntimeBoiler4,
            DowntimeBoiler5,
            DowntimeBoiler6,
            DownTimeTurbGen1,
            DownTimeTurbGen2,
            FerrousSystemHoursUnavailable,
            NonFerrousSystemHoursUnavailable,
            NonFerrousSmallsSystemHoursUnavailable,
            FrontEndFerrousSystemHoursUnavailable,
            EnhancedFerrousSystemHoursUnavailable,
            EnhancedNonFerrousSystemHoursUnavailable
        }
    }
}
