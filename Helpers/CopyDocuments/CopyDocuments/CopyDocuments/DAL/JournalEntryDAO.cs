using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CopyDocuments.DAL
{
    public class JournalEntryDAO
    {
        private B1Connection ConnectionFrom;
        private B1Connection ConnectionTo;

        public JournalEntryDAO(B1Connection connectionFrom, B1Connection connectionTo)
        {
            this.ConnectionFrom = connectionFrom;
            this.ConnectionTo = connectionTo;
        }

        public void DoCopy()
        {
            Program.Logger.Info("Buscando LCM's");
            Recordset rstDocFrom = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset rstUpdate = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            rstDocFrom.DoQuery("SELECT TransId FROM OJDT WHERE ISNULL(U_CVA_Imported, 0) = 0 AND RefDate > CAST('20170101' AS DATETIME) AND RefDate < CAST('20170131' AS DATETIME) AND TransType = 30");

            Program.Logger.Info("Registros encontrados: " + rstDocFrom.RecordCount);

            while (!rstDocFrom.EoF)
            {
                JournalEntries docFrom = (JournalEntries)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.oJournalEntries);
                docFrom.GetByKey((int)rstDocFrom.Fields.Item(0).Value);

                JournalEntries docTo = (JournalEntries)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.oJournalEntries);

                docTo.ReferenceDate = docFrom.ReferenceDate;
                docTo.DueDate = docFrom.DueDate;
                docTo.TaxDate = docFrom.TaxDate;
                docTo.VatDate = docFrom.VatDate;
                docTo.Reference = docFrom.Reference;
                docTo.Reference2 = docFrom.Reference2;
                docTo.Reference3 = docFrom.Reference3;

                for (int i = 0; i < docFrom.Lines.Count; i++)
                {
                    if (!String.IsNullOrEmpty(docTo.Lines.AccountCode))
                    {
                        docTo.Lines.Add();
                    }
                    docFrom.Lines.SetCurrentLine(i);

                    docTo.Lines.BPLID = 1;

                    if (!String.IsNullOrEmpty(docFrom.Lines.AccountCode))
                    {
                        docTo.Lines.AccountCode = this.GetAccount(docFrom.Lines.AccountCode);
                    }
                    if (!String.IsNullOrEmpty(docFrom.Lines.ShortName))
                    {
                        docTo.Lines.ShortName = this.GetAccount(docFrom.Lines.ShortName);
                    }
                    docTo.Lines.Credit = docFrom.Lines.Credit;
                    docTo.Lines.Debit = docFrom.Lines.Debit;
                    docTo.Lines.LineMemo = docFrom.Lines.LineMemo;
                    docTo.Lines.ControlAccount = docFrom.Lines.ControlAccount;
                    docTo.Lines.CostingCode = docFrom.Lines.CostingCode;
                    docTo.Lines.DueDate = docFrom.Lines.DueDate;
                    docTo.Lines.Reference1 = docFrom.Lines.Reference1;
                    docTo.Lines.Reference2 = docFrom.Lines.Reference2;
                    docTo.Lines.TaxCode = docFrom.Lines.TaxCode;
                    docTo.Lines.TaxDate = docFrom.Lines.TaxDate;
                    docTo.Lines.ProjectCode = docFrom.Lines.ProjectCode;
                }

                if (docTo.Add() != 0)
                {
                    Program.Logger.Info($"LCM: {docFrom.JdtNum} - {ConnectionTo.oCompany.GetLastErrorDescription()}");
                }
                else
                {
                    rstUpdate.DoQuery("UPDATE OJDT SET U_CVA_Imported = 1 WHERE TransId = " + docFrom.JdtNum);
                }

                Marshal.ReleaseComObject(docFrom);
                docFrom = null;

                Marshal.ReleaseComObject(docTo);
                docTo = null;

                rstDocFrom.MoveNext();
            }
        }

        private string GetAccount(string account)
        {
            account = account.Replace("1.1.1", "1.1.01");
            account = account.Replace("1.1.2", "1.1.02");

            switch (account)
            {
                case "1.1.01.02.007":
                    account = "1.1.01.02.049";
                    break;
                case "1.1.01.02.001":
                case "1.1.01.02.002":
                    account = "1.1.01.02.011";
                    break;
                case "1.1.01.02.009":
                    account = "1.1.01.02.025";
                    break;
                case "1.1.01.02.032":
                    account = "1.1.01.02.037";
                    break;
                case "1.1.01.02.033":
                    account = "1.1.01.02.079";
                    break;
                case "1.1.01.02.040":
                    account = "1.1.01.02.036";
                    break;
                case "1.1.02.01.004":
                    account = "1.1.01.01.004";
                    break;
            }
            return account;
        }
    }
}
