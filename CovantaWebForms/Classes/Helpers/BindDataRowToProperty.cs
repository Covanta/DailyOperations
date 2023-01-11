using System;

namespace CovantaWebForms.Classes.Helpers
{
    public class BindDataRowToProperty
    {  

        private System.Data.DataRow mData;
        private string mColumn;

        /// <summary>
        /// constructor to initialize earier
        /// </summary>
        /// <param name="dRow"></param>
        public BindDataRowToProperty(System.Data.DataRow dRow)
        {
            mData = dRow;
        }

        /// <summary>
        /// Data property
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Data.DataRow Data
        {
            get { return mData; }
            set { mData = value; }
        }

        /// <summary>
        /// Column property
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Column
        {
            get
            {
                if (string.IsNullOrEmpty(mColumn))
                {
                    return string.Empty;
                }

                return mColumn;
            }
            set { mColumn = value; }
        }

        /// <summary>
        /// ToString 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ToStringValue()
        {
            string ReturnValue = null;
            if ((mData[mColumn] != null) && !(mData[mColumn] == System.DBNull.Value))
            {
               ReturnValue = (string)mData[mColumn];
               return ReturnValue.Trim();
            }
            else
            {
               return null;
            }

         //   return ReturnValue;
        }

        /// <summary>
        /// ToString (taking PropertyName as argument)
		  /// Checks for null before attempting to trim
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ToStringValue(string columnName)
        {

            string ReturnValue = null;

            if ((mData[columnName] != null) && !(mData[columnName] == System.DBNull.Value))
            {
               ReturnValue = (string)mData[columnName];
               return ReturnValue.Trim();
            }
            else
            {
               return null;
            }

        }

        /// <summary>
        /// ToDate
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime ToDate()
        {
            System.DateTime ReturnValue = default(System.DateTime);

            if ((mData[mColumn] != null) && !(mData[mColumn] == System.DBNull.Value))
            {
                ReturnValue = (System.DateTime)mData[mColumn];
            }

            return ReturnValue;
        }

        /// <summary>
        /// ToDate (taking Column as argument)
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime ToDate(string columnName)
        {
            System.DateTime ReturnValue = default(System.DateTime);

            if ((mData[columnName] != null) && !(mData[columnName] == System.DBNull.Value))
            {
                ReturnValue = (System.DateTime)mData[columnName];
            }

            return ReturnValue;
        }

        /// <summary>
        /// ToNullibleDate
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public Nullable<System.DateTime> ToNullibleDate()
        {
            Nullable<System.DateTime> ReturnValue = null;

            if ((mData[mColumn] != null) && !(mData[mColumn] == System.DBNull.Value))
            {
                ReturnValue = (Nullable<System.DateTime>)mData[mColumn];
            }

            return ReturnValue;
        }

        /// <summary>
        /// ToNullibleDate (taking Column Name as argument)
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Nullable<System.DateTime> ToNullibleDate(string columnName)
        {
            Nullable<System.DateTime> ReturnValue = null;

            if ((mData[columnName] != null) && !(mData[columnName] == System.DBNull.Value))
            {
                ReturnValue = (Nullable<System.DateTime>)mData[columnName];
            }

            return ReturnValue;
        }


        /// <summary>
        /// ToInteger
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ToInteger()
        {
            int ReturnValue = 0;

            if ((mData[mColumn] != null) && !(mData[mColumn] == System.DBNull.Value))
            {
                ReturnValue = (int)mData[mColumn];
            }

            return ReturnValue;
        }

        /// <summary>
        /// ToInteger (taking Column Name as argument)
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ToInteger(string columnName)
        {
            int ReturnValue = 0;

            if ((mData[columnName] != null) && !(mData[columnName] == System.DBNull.Value))
            {
					//Convert allows us to 'upgrade' a smallint to an int so that minor db changes do not cause an exception 	
					//ReturnValue = (int)mData[columnName];
					ReturnValue = Convert.ToInt32(mData[columnName]);			
				}

            return ReturnValue;
        }

		  /// <summary>
		  /// ToInterger64 (taking Column Name as argument)
		  /// </summary>
		  /// <param name="columnName"></param>
		  /// <returns></returns>
		  /// <remarks></remarks>
		  public Int64 ToInteger64(string columnName)
		  {
			  Int64 ReturnValue = 0;

			  if ((mData[columnName] != null) && !(mData[columnName] == System.DBNull.Value))
			  {
				  ReturnValue = Convert.ToInt64(mData[columnName]);
			  }

			  return ReturnValue;
		  }


        /// <summary>
        /// ToDecimal taking Column name as argument
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public decimal ToDecimal(string columnName)
        {
            decimal ReturnValue = 0;

            if (!(mData[columnName] == System.DBNull.Value) & (mData[columnName] != null))
            {
                ReturnValue = (decimal)mData[columnName];
            }

            return ReturnValue;
        }

        /// <summary>
        /// ToDecimalNullable taking Column name as argument and retuning nullable decimal
        /// </summary>
        /// <param name="columnName">database column</param>
        /// <returns>decimal or null</returns>
        public decimal? ToDecimalNullable(string columnName)
        {
           decimal? ReturnValue = null;

           if (!(mData[columnName] == System.DBNull.Value) & (mData[columnName] != null))
           {
              ReturnValue = (decimal?)mData[columnName];
           }

           return ReturnValue;
        }


        /// <summary>
        /// ToDecimal  
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public decimal ToDecimal()
        {
            decimal ReturnValue = 0;

            if ((mData[mColumn] != null) && !(mData[mColumn] == System.DBNull.Value))
            {
                ReturnValue = (decimal)mData[mColumn];
            }

            return ReturnValue;
        }

        /// <summary>
        /// ToDouble - taking column name as argument
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ToDouble(string columnName)
        {
            double ReturnValue = 0;

            if ((mData[columnName] != null) && !(mData[columnName] == System.DBNull.Value))
            {
                ReturnValue = (double)mData[columnName];
            }

            return ReturnValue;
        }

        /// <summary>
        /// ToDouble
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ToDouble()
        {
            double ReturnValue = 0;

            if ((mData[mColumn] != null) && !(mData[mColumn] == System.DBNull.Value))
            {
                ReturnValue = (double)mData[mColumn];
            }

            return ReturnValue;
        }

        /// <summary>
        /// ToShort (taking column name as argument)
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public short ToShort(string columnName)
        {
            short ReturnValue = 0;

            if ((mData[columnName] != null) && !(mData[columnName] == System.DBNull.Value))
            {
                ReturnValue = (short)mData[columnName];
            }

            return ReturnValue;
        }

        /// <summary>
        /// ToShort 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public short ToShort()
        {
            short ReturnValue = 0;

            if ((mData[mColumn] != null) && !(mData[mColumn] == System.DBNull.Value))
            {
                ReturnValue = (short)mData[mColumn];
            }

            return ReturnValue;
        }



        /// <summary>
        /// ToBool 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ToBool(string columnName)
        {
            bool ReturnValue = false;

            if ((mData[columnName] != null) && !(mData[columnName] == System.DBNull.Value))
            {
                ReturnValue = (bool)mData[columnName];
            }

            return ReturnValue;
        }

    }

}
