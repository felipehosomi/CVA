using CVA.Core.TransportLCM.BLL.BaseConciliadora;
using CVA.Core.TransportLCM.HELPER.BaseDestino;
using CVA.Core.TransportLCM.MODEL.BaseConciliadora;
using Dover.Framework.DAO;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.BLL
{
    public class LancamentoContabilBLL
    {
        BusinessOneDAO _businessDAO { get; set; }
        BaseBLL _baseBLL { get; set; }
        BranchBLL _branchBLL { get; set; }

        public LancamentoContabilBLL(BusinessOneDAO businessDAO, BaseBLL baseBLL, BranchBLL branchBLL)
        {
            _businessDAO = businessDAO;
            _baseBLL = baseBLL;
            _branchBLL = branchBLL;
        }

        public string Replicate(int transId, out string idDestino)
        {
            idDestino = String.Empty;
            JournalEntries journalFrom = _businessDAO.GetBusinessObject(BoObjectTypes.oJournalEntries);
            if (!journalFrom.GetByKey(transId))
            {
                return "Registro não encontrado!";
            }

            BaseDePara baseDePara = _baseBLL.GetByCnpj(journalFrom.UserFields.Fields.Item("U_CVA_EmpDestino").Value.ToString());
            if (baseDePara.BaseDe == 0)
            {
                return $"Erro ao buscar empresa destino: CNPJ {journalFrom.UserFields.Fields.Item("U_CVA_EmpDestino").Value.ToString()} não encontrado na base conciliadora!";
            }
            Base baseModel = _baseBLL.Get(baseDePara.BaseDe);
            if (String.IsNullOrEmpty(baseModel.BaseName))
            {
                return $"Erro ao buscar empresa destino: Base {baseDePara.BaseDe} não encontrada na base conciliadora!";
            }
            string connected = Connection.Connect(baseModel);
            if (!String.IsNullOrEmpty(connected))
            {
                return "Erro ao conectar na empresa destino: " + connected;
            }

            JournalEntries journalTo = (JournalEntries)Connection._company.GetBusinessObject(BoObjectTypes.oJournalEntries);

            try
            {
                journalTo.TaxDate = journalFrom.TaxDate;
                journalTo.ReferenceDate = journalFrom.ReferenceDate;
                journalTo.DueDate = journalFrom.DueDate;

                journalTo.Series = journalFrom.Series;
                journalTo.Indicator = journalFrom.Indicator;
                journalTo.ProjectCode = journalFrom.ProjectCode;
                journalTo.UseAutoStorno = journalFrom.UseAutoStorno;

                journalTo.Memo = journalFrom.Memo;
                journalTo.Reference = journalFrom.Reference;
                journalTo.Reference2 = journalFrom.Reference2;
                journalTo.Reference3 = journalFrom.Reference3;

                journalTo.UserFields.Fields.Item("U_CVA_EmpOrigem").Value = journalFrom.Lines.BPLName;
                journalTo.UserFields.Fields.Item("U_CVA_IdOrigem").Value = transId;

                for (int i = 0; i < journalFrom.Lines.Count; i++)
                {
                    journalFrom.Lines.SetCurrentLine(i);

                    journalTo.Lines.BPLID = baseDePara.FilialDe;
                    journalTo.Lines.Reference1 = journalFrom.Lines.Reference1;
                    journalTo.Lines.Reference2 = journalFrom.Lines.Reference2;
                    journalTo.Lines.ReferenceDate1 = journalFrom.Lines.ReferenceDate1;
                    journalTo.Lines.ReferenceDate2 = journalFrom.Lines.ReferenceDate2;
                    journalTo.Lines.LineMemo = journalFrom.Lines.LineMemo;
                    journalTo.Lines.DueDate = journalFrom.Lines.DueDate;
                    journalTo.Lines.TaxDate = journalFrom.Lines.TaxDate;
                    journalTo.Lines.TaxCode = journalFrom.Lines.TaxCode;
                    journalTo.Lines.TaxGroup = journalFrom.Lines.TaxGroup;
                    journalTo.Lines.CostingCode = journalFrom.Lines.CostingCode;
                    journalTo.Lines.CostingCode2 = journalFrom.Lines.CostingCode2;
                    journalTo.Lines.CostingCode3 = journalFrom.Lines.CostingCode3;
                    journalTo.Lines.CostingCode4 = journalFrom.Lines.CostingCode4;
                    journalTo.Lines.CostingCode5 = journalFrom.Lines.CostingCode5;
                    journalTo.Lines.AdditionalReference = journalFrom.Lines.AdditionalReference;

                    var contaDestino = journalFrom.Lines.UserFields.Fields.Item("U_CVA_ContaDestino").Value.ToString();
                    var contaControle = journalFrom.Lines.UserFields.Fields.Item("U_CVA_ContaControle").Value.ToString();

                    if (!contaDestino.Contains("."))
                    {
                        journalTo.Lines.ShortName = contaDestino;
                        journalTo.Lines.AccountCode = contaControle;
                    }
                    else
                    {
                        journalTo.Lines.ShortName = contaDestino;
                        journalTo.Lines.AccountCode = contaDestino;
                    }

                    journalTo.Lines.Credit = journalFrom.Lines.Credit;
                    journalTo.Lines.Debit = journalFrom.Lines.Debit;

                    if (String.IsNullOrEmpty(journalTo.Lines.ShortName))
                    {
                        return String.Format("Linha {0} - Conta destino deve ser preenchida!", i + 1);
                    }

                    if (i + 1 < journalFrom.Lines.Count)
                    {
                        journalTo.Lines.Add();
                    }
                }

                int result = journalTo.Add();
                int errCode;
                string errMsg;

                if (result != 0)
                {
                    Connection._company.GetLastError(out errCode, out errMsg);
                    return String.Format("Falha ao adicionar o LCM ({0} {1}).", errCode, errMsg);
                }
                else
                {
                    idDestino = Connection._company.GetNewObjectKey();
                    journalFrom.UserFields.Fields.Item("U_CVA_IdDestino").Value = Convert.ToInt32(idDestino);
                    _businessDAO.UpdateBusinessObject(journalFrom);
                    return String.Empty;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(journalFrom);
                journalFrom = null;

                Marshal.ReleaseComObject(journalTo);
                journalTo = null;
            }
        }
    }
}
