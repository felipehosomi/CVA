using CVA.AddOn.Common.Controllers;
using CVA.View.ImportadorFolha.DAO;
using CVA.View.ImportadorFolha.DAO.Resources;
using CVA.View.ImportadorFolha.MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ImportadorFolha.BLL
{
    public class FolhaPagamentoBLL
    {
        public static string Gerar(int bplId, DateTime docDate, DateTime dueDate, string file, ref string lcmId)
        {
            string msg = String.Empty;
            FolhaPagamentoModel model = new FolhaPagamentoModel();
            model.BPlId = bplId;
            model.DocDate = docDate;
            model.DueDate = dueDate;

            model.Lines = new List<FolhaPagamentoLinhaModel>();
            List<FolhaPagamentoItemModel> configList = new CrudController("@CVA_FOLHA_ITEM").RetrieveModelList<FolhaPagamentoItemModel>(String.Empty);

            StreamReader reader = new StreamReader(file);
            try
            {
                string line;
                int lineNum = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(line))
                    {
                        if (!line.Contains(';'))
                        {
                            return "Formato do arquivo inválido. Valores devem ser separados por ponto e vírgula (;)";
                        }

                        FolhaPagamentoLinhaModel lineModel = new FolhaPagamentoLinhaModel();
                        lineModel.Items = new List<FolhaPagamentoItemModel>();

                        string[] splittedLine = line.Split(';');

                        foreach (var item in configList)
                        {
                            FolhaPagamentoItemModel configModel = new FolhaPagamentoItemModel();
                            configModel.CampoLCM = item.CampoLCM;
                            configModel.TipoCampoLCM = item.TipoCampoLCM;

                            if (item.Posicao == 0)
                            {
                                continue;
                            }

                            item.ValorDe = splittedLine[item.Posicao - 1];
                            if (!String.IsNullOrEmpty(item.Consulta.Trim()))
                            {
                                string consulta = String.Empty;
                                try
                                {
                                    consulta = String.Format(item.Consulta, item.ValorDe);
                                    object valorPara = CrudController.ExecuteScalar(consulta);
                                    if (valorPara != null)
                                    {
                                        configModel.ValorPara = valorPara.ToString();
                                    }
                                    else
                                    {
                                        return $"Linha {lineNum}: valor não encontrado. Consulta: {consulta}";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    return $"Linha {lineNum}: Consulta inválida: {consulta}";
                                }
                            }
                            else
                            {
                                configModel.ValorPara = item.ValorDe;
                            }
                            lineModel.Items.Add(configModel);
                        }

                        lineNum++;
                        model.Lines.Add(lineModel);
                    }
                }

                msg = LancamentoContabilDAO.Gerar(model, ref lcmId);
            }
            catch (Exception ex)
            {
                msg = "CVA - Erro geral: " + ex.Message;
            }
            finally
            {
                reader.Close();
            }
            return msg;
        }

        public static bool ExisteConfig()
        {
            return Convert.ToInt32(CrudController.ExecuteScalar(SQL.Folha_GetCount)) > 0;
        }
    }
}
