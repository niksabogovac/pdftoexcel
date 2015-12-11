using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            { "OstDevOR" ,BoxTypeEnum.OROCENJA },
            { "PotDevOR",BoxTypeEnum.OROCENJA },
            { "OvlDevAR", BoxTypeEnum.RACUNI },
            { "DKTR", BoxTypeEnum.RACUNI },
            { "LDSDK", BoxTypeEnum.RACUNI },
            { "OstKK", BoxTypeEnum.RACUNI },
            { "OstDTR", BoxTypeEnum.RACUNI },
            { "OvlTR", BoxTypeEnum.RACUNI },
            { "DKDinAR", BoxTypeEnum.RACUNI },
            { "PriSDK" , BoxTypeEnum.RACUNI },
            { "OstDKR", BoxTypeEnum.KREDITI },
            { "PotKR", BoxTypeEnum.KREDITI },
            { "OstDinOR", BoxTypeEnum.OROCENJA},
            { "PotDinOR", BoxTypeEnum.OROCENJA},
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

        public static string ConnectionStringDataGridView = @"Data Source=SERVER\SQLEXPRESS;Initial Catalog=QRCode;Integrated Security=True";
        public static string ConnectionString = @"Server=SERVER\SQLEXPRESS;Database=QRCode;User Id=sa;Password=Niksa@2015;";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Helper"/> class.
        /// </summary>
        public Helper()
        {

        }
        #endregion

        #region Public methods

        /// <summary>
        /// Updates value of doctypes from database when new doctype is added.
        /// </summary>
        public static void UpdateDocTypesFromDatabase()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + Helper.ConnectionString + ";Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [QRCode].[dbo].[DocTypes]", conn);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (doctypeBoxCode.ContainsKey((string)reader[0]))
                    {

                    }
                    else
                    {
                        string value = (string)reader[1];
                        value.ToUpper();
                        if (value.Equals("RACUNI"))
                        {
                            doctypeBoxCode.Add((string)reader[0], BoxTypeEnum.RACUNI);
                        }
                        else if (value.Equals("POZAJMICE"))
                        {
                            doctypeBoxCode.Add((string)reader[0], BoxTypeEnum.POZAJMICE);
                        }
                        else if (value.Equals("KREDITI"))
                        {

                            doctypeBoxCode.Add((string)reader[0], BoxTypeEnum.KREDITI);
                        }
                        else if (value.Equals("OROCENJA"))
                        {
                            doctypeBoxCode.Add((string)reader[0], BoxTypeEnum.OROCENJA);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
