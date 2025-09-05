using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace Electra.Currency.Task
{
    public class HanaService
    {
        public static string ConexaoHana = System.Configuration.ConfigurationManager.ConnectionStrings["Hana"].ConnectionString;
        
        #region [ Get Atualiza ]

        public static bool getAtualiza()
        {
            HanaConnection _conn = new HanaConnection(ConexaoHana);
            _conn.Open();

            bool bRetorno = false;
            string sQuery = String.Empty;            
            sQuery = String.Format($"SELECT \"U_SD_RepCurRate\" FROM  {CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase")}.\"OADM\"");

            HanaCommand cmd = new HanaCommand(sQuery, _conn);
            HanaDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (!String.IsNullOrEmpty(dr[0].ToString()))
                {
                    if (dr[0].ToString().Equals("S"))
                        bRetorno = true;
                    break;
                }
            }

            _conn.Close();

            return bRetorno;
        }

        #endregion

        #region [ Get Max ]

        public static int getMax(string sTabela, string sCampo)
        {
            HanaConnection _conn = new HanaConnection(ConexaoHana);
            _conn.Open();

            int iMax = 1;
            string sQuery = String.Empty;
            sQuery = String.Format($@"SELECT MAX(""{sCampo}"") FROM  {CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase")}.""@{sTabela}"" ");
            
            HanaCommand cmd = new HanaCommand(sQuery, _conn);
            HanaDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (!String.IsNullOrEmpty(dr[0].ToString()))
                {
                    iMax += Convert.ToInt32(dr[0].ToString());
                    break;
                }                    
            }

            _conn.Close();

            return iMax;
        }

        #endregion
        
        #region [ Get Currencies ]

        public static List<string> getCurrencies()
        {
            HanaConnection _conn = new HanaConnection(ConexaoHana);            
            _conn.Open();

            List<string> sLista = new List<string>();
            
            string sQuery = String.Empty;            
            sQuery = String.Format($"SELECT \"ISOCurrCod\" FROM {CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase")}.\"OCRN\" WHERE \"CurrCode\" != 'R$'");

            HanaCommand cmd = new HanaCommand(sQuery, _conn);
            HanaDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                sLista.Add(dr[0].ToString());
            }

            _conn.Close();

            return sLista;
        }

        public static string getCurrencies(string sCurrency)
        {
            HanaConnection _conn = new HanaConnection(ConexaoHana);
            _conn.Open();

            string sRetorno = String.Empty;
            string sQuery = String.Empty;

            sQuery = String.Format($"SELECT TOP 1 \"CurrCode\" FROM {CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase")}.\"OCRN\" WHERE \"ISOCurrCod\" = '{sCurrency}'");

            HanaCommand cmd = new HanaCommand(sQuery, _conn);
            HanaDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                sRetorno = dr[0].ToString();
            }

            _conn.Close();

            return sRetorno;
        }

        #endregion

        #region [ Gravar Log ]

        public static void GravarLog(string sMoeda, string sMensagem)
        {
            var iCode = getMax($"SD_CURRENCYLOG", "U_SD_Id");
            string sData = DateTime.Now.ToShortDateString();
            //sData = sData.Substring(6, 4) + sData.Substring(0, 2) + sData.Substring(3, 2);
            sData = sData.Substring(6, 4) + sData.Substring(3, 2) + sData.Substring(0, 2);
            int iHora = DateTime.Now.Hour;
            int iMinutos = DateTime.Now.Minute;
            string sHora = Convert.ToString(iHora).PadLeft(2, '0') + Convert.ToString(iMinutos).PadLeft(2, '0');
            sMensagem = sMensagem.Replace("'", " ");

            HanaConnection _conn = new HanaConnection(ConexaoHana);
            _conn.Open();

            StringBuilder oBuilder = new StringBuilder();
            oBuilder.AppendLine($"INSERT INTO {CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("ServiceLayerDatabase")}.dbo.\"@SD_CURRENCYLOG\" ");
            oBuilder.AppendLine($"(\"Code\", \"Name\", \"U_SD_Data\", \"U_SD_Hora\", \"U_SD_Moeda\", \"U_SD_Mensage\", \"U_SD_Id\") ");
            oBuilder.AppendLine($"values('{Convert.ToString(iCode)}', '{Convert.ToString(iCode)}', '{sData}', '{sHora}', '{sMoeda}', '{sMensagem}', {Convert.ToInt32(iCode)}) ");
            
            HanaCommand cmd = new HanaCommand(oBuilder.ToString(), _conn);
            HanaDataReader dr = cmd.ExecuteReader();
            _conn.Close();
        }

        #endregion
        
    }
}
