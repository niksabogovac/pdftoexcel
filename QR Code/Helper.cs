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
        private static Dictionary<string, string> _clientInfoNamesHistoricalNames = new Dictionary<string, string>()
        {
            {"PotTR","Potvrde_DinarskiTekuciRacun"},
            {"OstDevAR","OstalaDokumenta_DinarskiTekuciRacun"},
            {"ODDinAR","OsnovnaDokumenta_DinarskiAvistaRacun"},
            {"ODKR","Krediti - ugovaranje"},
            {"ODDevOR","OsnovnaDokumenta_DevizniOroceniRacun"},
            {"ODDinOR","OsnovnaDokumenta_DinarskiOroceniRacun"},
            {"ODTR","OsnovnaDokumenta_DinarskiTekuciRacun"},
            {"OPDDevOR","OsnovniPodaciODepozitu_DevizniOroceniRacun"},
            {"OPDDinOR","OsnovnaDokumenta_DinarskiOroceniRacun"},
            {"PozTR", "Pozajmice_DinarskiTekuciRacun"},
            {"DKDevAR",null},
            {"DKDinAR",null},
            {"DKTR",null},
            {"UruKK","Kreditne kartice - ugovaranje"},

        };

        /// <summary>
        /// Represents a dictionary of doctypes and appropriate box types.
        /// </summary>
        private static Dictionary<string, BoxTypeEnum> _doctypeBoxCode = new Dictionary<string, BoxTypeEnum>()
        {
            {"PozTR",BoxTypeEnum.POZAJMICE},
            {"UruKK",BoxTypeEnum.KREDITI},
            {"OstDevAR",BoxTypeEnum.RACUNI},
            {"PotTR",BoxTypeEnum.RACUNI},
            {"ODDevAR",BoxTypeEnum.RACUNI},
            {"ODDinAR",BoxTypeEnum.RACUNI},
            {"ODKR",BoxTypeEnum.KREDITI},
            {"ODDevOR",BoxTypeEnum.OROCENJA},
            {"ODDinOR",BoxTypeEnum.OROCENJA},
            {"OPDDevOR",BoxTypeEnum.OROCENJA},
            {"OPDDinOR",BoxTypeEnum.OROCENJA},
            {"DKDevAR",BoxTypeEnum.RACUNI},
            {"ODTR",BoxTypeEnum.RACUNI},

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
                return _clientInfoNamesHistoricalNames;
            }
        }

        public static Dictionary<String,BoxTypeEnum> DoctypeBoxCode
        {
            get
            {
                return _doctypeBoxCode;
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
