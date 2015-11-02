using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Code
{
    /// <summary>
    /// Represents support structures.
    /// </summary>
    public class Helper
    {
        #region Private members
        /// <summary>
        /// Represents a collection of names of client infos and their historical names.
        /// Not valid so not finished.
        /// </summary>
        private static Dictionary<string, string> clientInfoNamesHistoricalNames = new Dictionary<string, string>()
        {
            { "Potvrde_DinarskiTekuciRacun" ,"PotTR"},
            { "OstalaDokumenta_DinarskiTekuciRacun","OstDevAR"},
            { "OsnovnaDokumenta_DinarskiAvistaRacun","ODDinAR" },
            { "Krediti - ugovaranje","ODKR"},
            { "OsnovnaDokumenta_DevizniOroceniRacun", "ODDevOR" },
            { "OsnovnaDokumenta_DinarskiOroceniRacun" ,"ODDinOR"},
            { "OsnovnaDokumenta_DinarskiTekuciRacun" ,"ODTR" },
            { "OsnovniPodaciODepozitu_DevizniOroceniRacun" , "OPDDevOR"},
            { "OsnovniPodaciODepozitu_DinarskiOroceniRacun" ,"OPDDinOR"},
            { "Pozajmice_DinarskiTekuciRacun" , "PozTR"  },
            { "Kreditne kartice - ugovaranje" , "UruKK"},

        };

        /// <summary>
        /// Represents a dictionary of doctypes and appropriate box types.
        /// </summary>
        private static Dictionary<string, BoxTypeEnum> doctypeBoxCode = new Dictionary<string, BoxTypeEnum>()
        {
            { "PozTR", BoxTypeEnum.POZAJMICE },
            { "UruKK", BoxTypeEnum.KREDITI },
            { "OstDevAR", BoxTypeEnum.RACUNI },
            { "PotTR", BoxTypeEnum.RACUNI },
            { "ODDevAR", BoxTypeEnum.RACUNI },
            { "ODDinAR", BoxTypeEnum.RACUNI },
            { "ODKR", BoxTypeEnum.KREDITI },
            { "ODDevOR", BoxTypeEnum.OROCENJA },
            { "ODDinOR", BoxTypeEnum.OROCENJA },
            { "OPDDevOR", BoxTypeEnum.OROCENJA },
            { "OPDDinOR", BoxTypeEnum.OROCENJA },
            { "DKDevAR", BoxTypeEnum.RACUNI },
            { "ODTR", BoxTypeEnum.RACUNI },

        };


        #endregion

        #region Properties

        /// <summary>
        /// Gets the support historical doctype dictionary.
        /// </summary>
        public static Dictionary<string, string> ClientInfoNamesHistoricalNames
        {
            get
            {
                return clientInfoNamesHistoricalNames;
            }
        }

        /// <summary>
        /// Gets the support <see cref="doctypeBoxCode"/> dictionary.
        /// </summary>
        public static Dictionary<string, BoxTypeEnum> DoctypeBoxCode
        {
            get
            {
                return doctypeBoxCode;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Helper"/> class.
        /// </summary>
        public Helper()
        {

        }
        #endregion


    }
}
