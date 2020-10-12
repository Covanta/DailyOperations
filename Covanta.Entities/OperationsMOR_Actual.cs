using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covanta.Entities
{
    /// <summary>
    /// This represents a row on the Operations MOR Actual Table
    /// </summary>
    public class OperationsMOR_Actual
    {

        #region constructors
        /// <summary>
        /// This represents a row on the Operations MOR Actual Table
        /// </summary>
        public OperationsMOR_Actual() { }
       
        #endregion

        #region public properties

         public string Yr { get; set; }
         public string Mth { get; set; }
         public string PLT { get; set; }
         public decimal STE { get; set; }
         public decimal REFREC { get; set; }
         public decimal SUPWAS { get; set; }
         public decimal REFPRO { get; set; }
         public decimal HHVCALC { get; set; }
         public decimal GROELE { get; set; }
         public decimal NETELE { get; set; }
         public decimal PURPOW { get; set; }
         public decimal STEEXP { get; set; }
         public decimal FER { get; set; }
         public decimal NONFER { get; set; }
         public decimal ASH { get; set; }
         public decimal PEBLIM { get; set; }
         public decimal HYDLIM { get; set; }
         public decimal DOLLIM { get; set; }
         public decimal AMM { get; set; }
         public decimal URE { get; set; }
         public decimal CAR { get; set; }
         public decimal TUBLEA { get; set; }
         public decimal AUXFUEOILPPS { get; set; }
         public decimal AUXNATGAS { get; set; }
         public decimal AUXPROKCF { get; set; }
         public decimal AUXPROKLB { get; set; }
         public decimal AUXLANGAS { get; set; }
         public decimal AUXWOOPAL { get; set; }
         public decimal B1FEETEM { get; set; }
         public decimal B1STEPRO { get; set; }
         public decimal B2STEPRO { get; set; }
         public decimal B3STEPRO { get; set; }
         public decimal B4STEPRO { get; set; }
         public decimal B5STEPRO { get; set; }
         public decimal B6STEPRO { get; set; }
         public decimal B1STETEM { get; set; }
         public decimal B2STETEM { get; set; }
         public decimal B3STETEM { get; set; }
         public decimal B4STETEM { get; set; }
         public decimal B5STETEM { get; set; }
         public decimal B6STETEM { get; set; }
         public decimal B1HCAT { get; set; }
         public decimal B2HCAT { get; set; }
         public decimal B3HCAT { get; set; }
         public decimal B4HCAT { get; set; }
         public decimal B5HCAT { get; set; }
         public decimal B6HCAT { get; set; }
         public decimal B1EEGT { get; set; }
         public decimal B2EEGT { get; set; }
         public decimal B3EEGT { get; set; }
         public decimal B4EEGT { get; set; }
         public decimal B5EEGT { get; set; }
         public decimal B6EEGT { get; set; }
         public decimal B1EEO2 { get; set; }
         public decimal B2EEO2 { get; set; }
         public decimal B3EEO2 { get; set; }
         public decimal B4EEO2 { get; set; }
         public decimal B5EEO2 { get; set; }
         public decimal B6EEO2 { get; set; }
         public decimal B1AHEGT { get; set; }
         public decimal B2AHEGT { get; set; }
         public decimal B3AHEGT { get; set; }
         public decimal B4AHEGT { get; set; }
         public decimal B5AHEGT { get; set; }
         public decimal B6AHEGT { get; set; }
         public decimal B1STEPRE { get; set; }
         public decimal B2STEPRE { get; set; }
         public decimal B3STEPRE { get; set; }
         public decimal B4STEPRE { get; set; }
         public decimal B5STEPRE { get; set; }
         public decimal B6STEPRE { get; set; }
         public decimal B1BHDT { get; set; }
         public decimal B2BHDT { get; set; }
         public decimal B3BHDT { get; set; }
         public decimal B4BHDT { get; set; }
         public decimal B5BHDT { get; set; }
         public decimal B6BHDT { get; set; }
         public decimal BOP { get; set; }
         public decimal BST { get; set; }
         public decimal BOutSch { get; set; }
         public decimal BOutUNS { get; set; }
         public decimal B1OP { get; set; }
         public decimal B2OP { get; set; }
         public decimal B3OP { get; set; }
         public decimal B4OP { get; set; }
         public decimal B5OP { get; set; }
         public decimal B6OP { get; set; }
         public decimal B1ST { get; set; }
         public decimal B2ST { get; set; }
         public decimal B3ST { get; set; }
         public decimal B4ST { get; set; }
         public decimal B5ST { get; set; }
         public decimal B6ST { get; set; }
         public decimal B1UN { get; set; }
         public decimal B2UN { get; set; }
         public decimal B3UN { get; set; }
         public decimal B4UN { get; set; }
         public decimal B5UN { get; set; }
         public decimal B6UN { get; set; }
         public decimal TOP { get; set; }
         public decimal TST { get; set; }
         public decimal TUN { get; set; }
         public decimal T1OP { get; set; }
         public decimal T2OP { get; set; }
         public decimal T1ST { get; set; }
         public decimal T2ST { get; set; }
         public decimal T1UN { get; set; }
         public decimal T2UN { get; set; }
         public decimal DaysInMonth { get; set; }
         public decimal NOOFBLRS { get; set; }
         public decimal DESNOOFOPEUNI { get; set; }
         public decimal DESNOOFTRBS { get; set; }
         public decimal DESREFTPD { get; set; }
         public decimal DESHHV { get; set; }
         public decimal DESSTEPPH { get; set; }
         public decimal DESTURMW { get; set; }
         public decimal DESSTEPRE { get; set; }
         public decimal DESSTETEM { get; set; }
         public decimal DESGROER { get; set; }
         public decimal DESNETER { get; set; }
         public decimal DESPEBLIM { get; set; }
         public decimal DESAMM { get; set; }
         public decimal DESCAR { get; set; }
         public decimal DESFEETEM { get; set; }
         public decimal BASEEGT { get; set; }
         public decimal BASHCAT { get; set; }
         public decimal BASAHEGT { get; set; }
         public decimal BASXAIR { get; set; }
         public string O2TYPE { get; set; }
         public string RefProExpMon { get; set; }
         public string RefProExpYTD { get; set; }
         public string NetEleExpMon { get; set; }
         public string NetEleExpYTD { get; set; }
         public string FerNonFerExpMon { get; set; }
         public string FerNonFerExpYTD { get; set; }
         public string ExpSteExpMon { get; set; }
         public string ExpSteExpYTD { get; set; }

        #endregion
    }   
}
