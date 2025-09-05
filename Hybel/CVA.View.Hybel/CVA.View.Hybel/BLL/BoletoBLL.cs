using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CVA.View.Hybel.BLL
{
    public class BoletoBLL
    {
        private static List<BoletoModel> BoletoList;
        private static string modeloBoleto;

        public static string GetBoletoSQL(string cardCodeFrom, string cardCodeTo, string dateFrom, string dateTo, string status)
        {
            string sql = SQL.Boleto_Get;
            if (!String.IsNullOrEmpty(cardCodeFrom))
            {
                sql += $" AND OBOE.CardCode >= '{cardCodeFrom}' ";
            }
            if (!String.IsNullOrEmpty(cardCodeTo))
            {
                sql += $" AND OBOE.CardCode <= '{cardCodeTo}' ";
            }
            if (!String.IsNullOrEmpty(dateFrom))
            {
                sql += $" AND OBOE.DueDate >= CAST('{dateFrom}' AS DATETIME) ";
            }
            if (!String.IsNullOrEmpty(dateTo))
            {
                sql += $" AND OBOE.DueDate <= CAST('{dateTo}' AS DATETIME) ";
            }
            if (!String.IsNullOrEmpty(status) && status != "T")
            {
                sql += $" AND OBOE.U_CVA_Email = '{status}' ";
            }
            sql += " ORDER BY OBOE.DueDate ";
            return sql;
        }

        public static string GetLogBoletoSQL(string cardCodeFrom, string cardCodeTo, string dateFrom, string dateTo, string status)
        {
            string sql = SQL.Boleto_GetLog;
            if (!String.IsNullOrEmpty(cardCodeFrom))
            {
                sql += $" AND OBOE.CardCode >= '{cardCodeFrom}' ";
            }
            if (!String.IsNullOrEmpty(cardCodeTo))
            {
                sql += $" AND OBOE.CardCode <= '{cardCodeTo}' ";
            }
            if (!String.IsNullOrEmpty(dateFrom))
            {
                sql += $" AND OBOE.DueDate >= CAST('{dateFrom}' AS DATETIME) ";
            }
            if (!String.IsNullOrEmpty(dateTo))
            {
                sql += $" AND OBOE.DueDate <= CAST('{dateTo}' AS DATETIME) ";
            }
            if (!String.IsNullOrEmpty(status) && status != "T")
            {
                sql += $" AND OBOE.U_CVA_Email = '{status}' ";
            }
            else
            {
                sql += $" AND ISNULL(OBOE.U_CVA_Email, 'N') <> 'N' ";
            }
            sql += " ORDER BY OBOE.DueDate ";
            return sql;
        }

        public static void SendEmails(SAPbouiCOM.DataTable dt_Boleto)
        {
            GridController gridController = new GridController();
            BoletoList = gridController.FillModelFromTableAccordingToValue<BoletoModel>(dt_Boleto, false, "Enviar", "Y");
            string pendenteInSql = string.Join(", ", from item in BoletoList select item.BoeNum);
            CrudController.ExecuteNonQuery(String.Format(SQL.Boleto_UpdatePendente, pendenteInSql));

            Thread thread = new Thread(SendEmailsThread);
            thread.Start();
        }

        private static void SendEmailsThread()
        {
            try
            {
                IEnumerable<IGrouping<string, BoletoModel>> listByBanco = BoletoList.GroupBy(b => b.Banco);
                foreach (var itemByBanco in listByBanco)
                {
                    IEnumerable<IGrouping<string, BoletoModel>> listByPN = itemByBanco.GroupBy(b => b.Cliente);
                    string currentPath = AppDomain.CurrentDomain.BaseDirectory;
                    StreamReader sr = new StreamReader(Path.Combine(currentPath, "ModelosEmail", $"Lembrete_{itemByBanco.Key}.html"));
                    string modeloLembrete = sr.ReadToEnd();
                    sr.Close();

                    sr = new StreamReader(Path.Combine(currentPath, "ModelosEmail", $"Vencido_{itemByBanco.Key}.html"));
                    string modeloVencido = sr.ReadToEnd();
                    sr.Close();

                    sr = new StreamReader(Path.Combine(currentPath, "ModelosEmail", $"AVencer_{itemByBanco.Key}.html"));
                    string modeloAVencer = sr.ReadToEnd();
                    sr.Close();

                    foreach (var itemByPN in listByPN)
                    {
                        List<BoletoModel> lembreteList = itemByPN.Where(i => i.Vencimento > DateTime.Today).ToList();
                        List<BoletoModel> aVencerList = itemByPN.Where(i => i.Vencimento == DateTime.Today).ToList();
                        List<BoletoModel> vencidoList = itemByPN.Where(i => i.Vencimento < DateTime.Today).ToList();

                        string lembreteInSql = string.Join(", ", from item in lembreteList select item.BoeNum);
                        string aVencerInSql = string.Join(", ", from item in aVencerList select item.BoeNum);
                        string vencidoInSql = string.Join(", ", from item in vencidoList select item.BoeNum);
                        string erro;
                        string boletos = String.Empty;
                        if (lembreteList.Count > 0)
                        {
                            foreach (var item in lembreteList)
                            {
                                boletos += String.Format(ModeloBoleto(), item.NossoNumero, item.Vencimento.ToString("dd/MM/yyyy"), item.Valor.ToString("c2"), item.NF, item.Parcela);
                            }
                            string lembreteBody = String.Format(modeloLembrete, boletos);
                            erro = EmailBLL.SendEmail("Boleto(s) - Lembrete", lembreteBody, itemByPN.ElementAt(0).Email);
                            if (!String.IsNullOrEmpty(erro))
                            {
                                CrudController.ExecuteNonQuery(String.Format(SQL.Boleto_UpdateErro, lembreteInSql, erro.Replace("'", "")));
                            }
                            else
                            {
                                CrudController.ExecuteNonQuery(String.Format(SQL.Boleto_UpdateOK, lembreteInSql));
                            }
                        }

                        if (aVencerList.Count > 0)
                        {
                            boletos = String.Empty;
                            foreach (var item in aVencerList)
                            {
                                boletos += String.Format(ModeloBoleto(), item.NossoNumero, item.Vencimento.ToString("dd/MM/yyyy"), item.Valor.ToString("c2"), item.NF, item.Parcela);
                            }
                            string aVencerBody = String.Format(modeloAVencer, boletos);
                            erro = EmailBLL.SendEmail("Boleto(s) - A Vencer", aVencerBody, itemByPN.ElementAt(0).Email);
                            if (!String.IsNullOrEmpty(erro))
                            {
                                CrudController.ExecuteNonQuery(String.Format(SQL.Boleto_UpdateErro, aVencerInSql, erro.Replace("'", "")));
                            }
                            else
                            {
                                CrudController.ExecuteNonQuery(String.Format(SQL.Boleto_UpdateOK, aVencerInSql));
                            }
                        }

                        if (vencidoList.Count > 0)
                        {
                            boletos = String.Empty;
                            foreach (var item in vencidoList)
                            {
                                boletos += String.Format(ModeloBoleto(), item.NossoNumero, item.Vencimento.ToString("dd/MM/yyyy"), item.Valor.ToString("c2"), item.NF, item.Parcela);
                            }
                            string vencidoBody = String.Format(modeloVencido, boletos);
                            erro = EmailBLL.SendEmail("Boleto(s) - Vencidos", vencidoBody, itemByPN.ElementAt(0).Email);
                            if (!String.IsNullOrEmpty(erro))
                            {
                                CrudController.ExecuteNonQuery(String.Format(SQL.Boleto_UpdateErro, vencidoInSql, erro.Replace("'", "")));
                            }
                            else
                            {
                                CrudController.ExecuteNonQuery(String.Format(SQL.Boleto_UpdateOK, vencidoInSql));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.MessageBox("Erro geral ao enviar e-mails: " + ex.Message);
            }
        }

        public static string ModeloBoleto()
        {
            if (String.IsNullOrEmpty(modeloBoleto))
            {
                string currentPath = AppDomain.CurrentDomain.BaseDirectory;
                StreamReader sr = new StreamReader(Path.Combine(currentPath, "ModelosEmail", "Boleto.html"));
                modeloBoleto = sr.ReadToEnd();
                sr.Close();
            }
            return modeloBoleto;
        }
    }
}
