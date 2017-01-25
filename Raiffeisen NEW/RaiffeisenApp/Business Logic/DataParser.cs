using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusinessLogic
{
    public static class DataParser
    {

        #region Public methods

        /// <summary>
        /// Parses input string to find id of code.
        /// </summary>
        /// <param name="inputCode">Input qr code.</param>
        /// <returns>Found ID. <b>NULL</b> if not found.</returns>
        public static string GetIdFromQrCode(string inputCode)
        {
            JObject qrCode = JObject.Parse(inputCode);
            return qrCode.First.Next.First.ToString();
        }

        /// <summary>
        /// Calculates number of needed fileNumbers.
        /// </summary>
        /// <param name="numberOfFiles">Number of files.</param>
        /// <returns>Calculated number of fileNumbers.</returns>
        public static int CalculateNumberOfCodes(int numberOfFiles)
        {
            int ret = 0;
            ret = numberOfFiles / 20;
            if (numberOfFiles % 20 != 0)
            {
                ret++;
            }
            return ret;
        }

        #endregion
    }
}
