using System;
using System.Data;

namespace QR_Code
{
    public static class DataReaderExtenstionMethods
    {
        /// <summary>
        /// Extension method that checks if DataReader contains given column.
        /// </summary>
        /// <param name="r">Data reader to check.</param>
        /// <param name="columnName">Column name to be checked.</param>
        /// <returns>Indicator of success.</returns>
        public static bool HasColumn(this IDataRecord r, string columnName)
        {
            try
            {
                return r.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }

        }
    }
}
