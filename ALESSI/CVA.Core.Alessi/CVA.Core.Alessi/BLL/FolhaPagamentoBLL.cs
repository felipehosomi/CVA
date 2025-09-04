using CVA.Core.Alessi.DAO.OJDT;
using CVA.Core.Alessi.MODEL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.BLL
{
    public class FolhaPagamentoBLL
    {
        public static int CONTACONTABIL = 0;
        public static int CREDITO = 1;
        public static int DEBITO = 2;
        public static int PARCEIRONEGOCIO = 3;
        public static int CENTROCUSTO01 = 4;
        public static int OBSERVACAO = 5;
        public static int PROJETO = 6;

        public static string Gerar(int bplId, DateTime docDate, DateTime dueDate, string file, ref string lcmId)
        {
            string msg = String.Empty;
            FolhaPagamentoModel model = new FolhaPagamentoModel();
            model.BPlId = bplId;
            model.DocDate = docDate;
            model.DueDate = dueDate;

            model.Lines = new List<FolhaPagamentoLineModel>();

            StreamReader reader = new StreamReader(file);
            try
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(line))
                    {
                        if (!line.Contains(';'))
                        {
                            return "Formato do arquivo inválido. Valores devem ser separados por ponto e vírgula (;)";
                        }

                        FolhaPagamentoLineModel modelLine = new FolhaPagamentoLineModel();

                        string[] splittedLine = line.Split(';');
                        modelLine.ContaContabil = splittedLine[CONTACONTABIL];
                        if (!String.IsNullOrEmpty(splittedLine[CREDITO]))
                        {
                            modelLine.Credito = Convert.ToDouble(splittedLine[CREDITO], CultureInfo.GetCultureInfo("en-US"));
                        }
                        if (!String.IsNullOrEmpty(splittedLine[DEBITO]))
                        {
                            modelLine.Debito = Convert.ToDouble(splittedLine[DEBITO], CultureInfo.GetCultureInfo("en-US"));
                        }
                        modelLine.ParceiroNegocio = splittedLine[PARCEIRONEGOCIO].Trim();
                        modelLine.CentroCusto01 = splittedLine[CENTROCUSTO01].Trim();
                        modelLine.Projeto = splittedLine[PROJETO].Trim();
                        modelLine.Observacao = splittedLine[OBSERVACAO];

                        model.Lines.Add(modelLine);
                    }
                }

                msg = FolhaPagamentoDAO.Gerar(model, ref lcmId);
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
    }
}
