using CVA.AddOn.Common;
using CVA.View.FolhaPagamento.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.FolhaPagamento.DAO
{
    public class FolhaPagamentoDAO
    {
    
        public FolhaPagamentoDAO()
        {
         
        }

        public string Gerar(FolhaPagamentoModel model, ref string lcmId)
        {
            string msg = String.Empty;

            JournalEntries lcm = (JournalEntries)SBOApp.Company.GetBusinessObject(BoObjectTypes.oJournalEntries);
            try
            {
                lcm.ReferenceDate = model.DocDate;
                lcm.TaxDate = model.DocDate;
                lcm.DueDate = model.DueDate;
                lcm.UserFields.Fields.Item("U_CVA_Integracao").Value = "Y";

                int i = 0;
                foreach (var line in model.Lines)
                {
                    i++;
                    lcm.Lines.BPLID = model.BPlId;

                    if (line.ContaContabil != line.ParceiroNegocio)
                    {
                        lcm.Lines.ShortName = line.ParceiroNegocio;
                        lcm.Lines.AccountCode = line.ContaContabil;
                    }
                    else
                    {
                        lcm.Lines.ShortName = line.ContaContabil;
                    }

                    lcm.Lines.AccountCode = line.ContaContabil;
                    lcm.Lines.ShortName = line.ParceiroNegocio;
                    lcm.Lines.Credit = line.Credito;
                    lcm.Lines.Debit = line.Debito;
                    lcm.Lines.CostingCode = line.CentroCusto01;
                    lcm.Lines.CostingCode2 = line.CentroCusto02;
                    lcm.Lines.ProjectCode = line.Projeto;
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
