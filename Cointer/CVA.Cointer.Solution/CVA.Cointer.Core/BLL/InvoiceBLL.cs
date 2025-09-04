using SAPbobsCOM;
using SAPbouiCOM;
using SBO.Hub;
using System;

namespace CVA.Cointer.Core.BLL
{
    public class InvoiceBLL
    {
        public string GenerateByCreditNote(int docEntry)
        {
            Documents creditNote = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oCreditNotes);
            Documents invoice = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oDrafts);

            creditNote.GetByKey(docEntry);

            invoice.DocObjectCode = BoObjectTypes.oInvoices;
            invoice.CardCode = creditNote.CardCode;
            invoice.DocDate = creditNote.DocDate;
            invoice.DocDueDate = creditNote.DocDueDate;
            invoice.TaxDate = creditNote.TaxDate;
            invoice.TaxExtension.MainUsage = creditNote.TaxExtension.MainUsage;
            invoice.UserFields.Fields.Item("U_CVA_DocEntryDev").Value = creditNote.DocEntry;

            invoice.Comments = "Baseado na devolução " + creditNote.DocNum;

            for (int i = 0; i < creditNote.Lines.Count; i++)
            {
                creditNote.Lines.SetCurrentLine(i);
                if (i > 0)
                {
                    invoice.Lines.Add();
                }

                invoice.Lines.ItemCode = creditNote.Lines.ItemCode;
                invoice.Lines.Quantity = creditNote.Lines.Quantity;
                invoice.Lines.UnitPrice = creditNote.Lines.UnitPrice;
                invoice.Lines.WarehouseCode = creditNote.Lines.WarehouseCode;
                invoice.Lines.Usage = creditNote.Lines.Usage;
                invoice.Lines.TaxCode = creditNote.Lines.TaxCode;

                for (int j = 0; j < creditNote.Lines.BatchNumbers.Count; j++)
                {
                    creditNote.Lines.BatchNumbers.SetCurrentLine(j);
                    if (j > 0)
                    {
                        invoice.Lines.BatchNumbers.Add();
                    }
                    invoice.Lines.BatchNumbers.BatchNumber = creditNote.Lines.BatchNumbers.BatchNumber;
                    invoice.Lines.BatchNumbers.Quantity = creditNote.Lines.BatchNumbers.Quantity;
                }
            }

            if (invoice.Add() != 0)
            {
                return SBOApp.Company.GetLastErrorDescription();
            }
            else
            {
                Form frmDraft = (Form)SBOApp.Application.OpenForm((BoFormObjectEnum)112, "", SBOApp.Company.GetNewObjectKey());
                frmDraft.Select();

                try
                {
                    ComboBox cb_Usage = (ComboBox)frmDraft.Items.Item("1720002171").Specific;
                    cb_Usage.Select(creditNote.TaxExtension.MainUsage.ToString());
                }
                catch (Exception ex)
                {
                    
                }
            }
            return String.Empty;
        }
    }
}
