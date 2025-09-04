using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.Core.Alessi.DAO.Resources;
using CVA.Core.Alessi.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.DAO.OJDT
{
    public class FolhaPagamentoDAO
    {
        public static string Gerar(FolhaPagamentoModel model, ref string lcmId)
        {
            string msg = String.Empty;

            JournalEntries lcm = (JournalEntries)SBOApp.Company.GetBusinessObject(BoObjectTypes.oJournalEntries);
            try
            {
                lcm.ReferenceDate = model.DocDate;
                lcm.TaxDate = model.DocDate;
                lcm.DueDate = model.DueDate;

                int i = 0;
                foreach (var line in model.Lines)
                {
                    i++;
                    lcm.Lines.BPLID = model.BPlId;

                    object account = CrudController.ExecuteScalar(String.Format(Query.FolhaPagamento_GetAccount, line.ContaContabil));
                    if (account == null)
                    {
                        return $"Linha {i}: conta {line.ContaContabil} não encontrada!";
                    }
                    object shortName = null;
                    if (line.ContaContabil != line.ParceiroNegocio && !String.IsNullOrEmpty(line.ParceiroNegocio))
                    {
                        shortName = CrudController.ExecuteScalar(String.Format(Query.FolhaPagamento_GetBP, line.ParceiroNegocio));
                        if (shortName == null)
                        {
                            shortName = CrudController.ExecuteScalar(String.Format(Query.FolhaPagamento_GetAccount, line.ParceiroNegocio));
                        }
                        if (shortName == null)
                        {
                            return $"Linha {i}: Parceiro de negócio {line.ParceiroNegocio} não encontrado!";
                        }
                    }
                    else
                    {
                        shortName = account;
                    }
                    if (shortName != account)
                    {
                        lcm.Lines.ShortName = shortName.ToString();
                    }
                    
                    lcm.Lines.AccountCode = account.ToString();
                    lcm.Lines.Credit = line.Credito;
                    lcm.Lines.Debit = line.Debito;
                    if (!String.IsNullOrEmpty(line.CentroCusto01))
                    {
                        lcm.Lines.Reference1 = line.CentroCusto01;
                    }
                    
                    if (!String.IsNullOrEmpty(line.Projeto))
                    {
                        lcm.Lines.ProjectCode = line.Projeto;
                    }
                    if (line.Observacao.Length > 50)
                    {
                        line.Observacao = line.Observacao.Substring(0, 50);
                    }

                    lcm.Lines.LineMemo = line.Observacao;

                    if (i < model.Lines.Count)
                    {
                        lcm.Lines.Add();
                    }
                }

                if (lcm.Add() != 0)
                {
                    msg = SBOApp.Company.GetLastErrorDescription();
                }
                else
                {
                    lcmId = SBOApp.Company.GetNewObjectKey();
                }
            }
            catch (Exception ex)
            {
                msg = "CVA - Erro geral: " + ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(lcm);
                lcm = null;
            }

            return msg;
        }
    }
}
