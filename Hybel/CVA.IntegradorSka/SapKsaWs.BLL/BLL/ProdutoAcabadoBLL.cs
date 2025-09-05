using DelimitedDataHelper.Tab;
using log4net;
using SapKsaWs.BLL.HELPER;
using SapKsaWs.BLL.MODEL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapKsaWs.BLL
{
    public class ProdutoAcabadoBLL
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProdutoAcabadoBLL()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void GeraArquivoAcabado()
        {
            string filePath = ConfigurationManager.AppSettings["ArquivoAcabado"];
            string update = DAO.MES.Log_Update;
            var helperSap = new SqlHelper("sap");
            var helperSka = new SqlHelper("ska");
            Logger.Debug("Buscando dados consumo OK");
            // Busca os que o consumo estão OK
            List<SSPExportProdLogModel> listConsumoOK = helperSka.FillModelList<SSPExportProdLogModel>(DAO.MES.Log_GetConsumoOK);

            Logger.Debug("Dados encontrados: " + listConsumoOK.Count);
            foreach (var itemConsumoOK in listConsumoOK)
            {
                List<ConsumoOPModel> listConsumo = helperSap.FillModelList<ConsumoOPModel>(String.Format(DAO.BEAS.ProdutoAcabado_GetByMesId, itemConsumoOK.IdMes));
                StatusLogEnum status =  StatusLogEnum.EmProcessamento;

                if (status == StatusLogEnum.EmProcessamento)
                {
                    Logger.Debug($"Gerando arquivo Acabado - OP: {itemConsumoOK.OP}, BELPOS_ID : {itemConsumoOK.BelPosId}, POS_ID: {itemConsumoOK.BelPosId}");
                    string fileName = $"Acabado_OP_{itemConsumoOK.OP.Trim()}_{itemConsumoOK.BelPosId.Trim()}_{itemConsumoOK.PosId.Trim()}_{itemConsumoOK.IdMes}_{DateTime.Now.ToString("ddMMyyyy_HHmmssfff")}.txt";
                    TabDelimitedDataWriter.WriteToTabDelimitedFile(listConsumo, Path.Combine(filePath, fileName));
                    helperSka.ExecuteNonQuery(String.Format(update, (int)status, "", itemConsumoOK.IdMes, (int)TipoLogEnum.ProdutoAcabado));
                }
            }
            helperSap.Dispose();
            helperSka.Dispose();
        }

        public void VerificaEntradaAcabado()
        {
            RefugoBLL refugoBLL = new RefugoBLL();
            Logger.Debug("Verificando entrada de acabado");
            var helperSap = new SqlHelper("sap");
            var helperSka = new SqlHelper("ska");

            string update = DAO.MES.Log_Update;

            List<SSPExportProdLogModel> modelList = helperSap.FillModelList<SSPExportProdLogModel>(DAO.BEAS.ProdutoAcabado_GetEntradas);
            Logger.Debug("Dados encontrados: " + modelList.Count);

            foreach (var item in modelList)
            {
                StatusLogEnum status;
                if (!String.IsNullOrEmpty(item.Erro))
                {
                    status = StatusLogEnum.VerificarObservacao;
                }
                else
                {
                    if (item.Erro == null)
                    {
                        item.Erro = "";
                    }

                    if (item.Refugo > 0)
                    {
                        item.Erro = refugoBLL.GeraTransferencia(item);
                    }
                    if (String.IsNullOrEmpty(item.Erro))
                    {
                        status = StatusLogEnum.ImportadoComSucesso;
                    }
                    else
                    {
                        status = StatusLogEnum.VerificarObservacao;
                    }
                }

                string updateSql = String.Format(update, (int)status, item.Erro.Replace("'", ""), item.IdMes, (int)TipoLogEnum.ProdutoAcabado);
                helperSka.ExecuteNonQuery(updateSql);
            }

            helperSap.Dispose();
            helperSka.Dispose();
        }
    }
}
