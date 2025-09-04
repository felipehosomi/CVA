using CVA.View.FolhaPagamento.MODEL;
using Dover.Framework.DAO;
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
        private SAPbobsCOM.Company _company { get; set; }

        public FolhaPagamentoDAO(BusinessOneDAO businessOneDAO, SAPbobsCOM.Company company)
        {
            _company = company;
        }

        public string Gerar(FolhaPagamentoModel model, ref string lcmId)
        {
            string msg = String.Empty;

            JournalEntries lcm = (JournalEntries)_company.GetBusinessObject(BoObjectTypes.oJournalEntries);
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
                    lcm.Lines.Credit = line.Credito;
                    lcm.Lines.Debit = line.Debito;
                    if (!String.IsNullOrEmpty(line.CentroCusto01))
                    {
                        lcm.Lines.CostingCode = line.CentroCusto01;
                    }
                    if (!String.IsNullOrEmpty(line.CentroCusto02))
                    {
                        lcm.Lines.CostingCode2 = line.CentroCusto02;
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
                    msg = _company.GetLastErrorDescription();
                }
                else
                {
                    lcmId = _company.GetNewObjectKey();
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
