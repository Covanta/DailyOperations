using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a  C4R Forecast Extract Source File Info object
    /// </summary>
    public class C4RForecastExtractSourceFileInfo
    {
        #region constructors

        /// <summary>
        /// This represents a C4R Forecast Extract File Location object.
        /// </summary>
        /// <param name="path">HTTP path</param>
        /// <param name="fileName">FileName</param>
        public C4RForecastExtractSourceFileInfo(string path, string fileName, int rowStartDIM1, int rowEndDIM1, int rowStartDIM2_part1, int rowEndDIM2_part1, int rowStartDIM2_part2, int rowEndDIM2_part2, int rowStartDIM2_part3, int rowEndDIM2_part3, int rowStartDIM2_part4, int rowEndDIM2_part4)
        {
            Path = path;
            FileName = fileName;
            DIM1_RowStart = rowStartDIM1;
            DIM1_RowEnd = rowEndDIM1;
            DIM2_Part1_RowStart = rowStartDIM2_part1;
            DIM2_Part1_RowEnd = rowEndDIM2_part1;
            DIM2_Part2_RowStart = rowStartDIM2_part2;
            DIM2_Part2_RowEnd = rowEndDIM2_part2;
            DIM2_Part3_RowStart = rowStartDIM2_part3;
            DIM2_Part3_RowEnd = rowEndDIM2_part3;
            DIM2_Part4_RowStart = rowStartDIM2_part4;
            DIM2_Part4_RowEnd = rowEndDIM2_part4;         
        }
              

        #endregion

        #region public properties

        /// <summary>
        /// Path where file is located
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName { get; set; }
      
        public int DIM1_RowStart { get; set; }
        public int DIM1_RowEnd { get; set; }
        public int DIM2_Part1_RowStart { get; set; }
        public int DIM2_Part1_RowEnd { get; set; }
        public int DIM2_Part2_RowStart { get; set; }
        public int DIM2_Part2_RowEnd { get; set; }
        public int DIM2_Part3_RowStart { get; set; }
        public int DIM2_Part3_RowEnd { get; set; }
        public int DIM2_Part4_RowStart { get; set; }
        public int DIM2_Part4_RowEnd { get; set; }

                    
        #endregion
    }
}
