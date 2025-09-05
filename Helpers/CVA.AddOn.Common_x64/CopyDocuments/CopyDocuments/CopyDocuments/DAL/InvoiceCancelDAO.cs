using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CopyDocuments.DAL
{
    public class InvoiceCancelDAO
    {
        private B1Connection ConnectionFrom;
        private B1Connection ConnectionTo;

        public InvoiceCancelDAO(B1Connection connectionFrom, B1Connection connectionTo)
        {
            this.ConnectionFrom = connectionFrom;
            this.ConnectionTo = connectionTo;
        }

        public void DoCopy()
        {
            Recordset rstDocFrom = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset rstDocTo = (Recordset)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset rstUpdate = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            Console.WriteLine();
            Console.WriteLine("Buscando dados da tabela OINV");
            rstDocFrom.DoQuery("SELECT DocEntry FROM OINV WHERE ISNULL(U_CVA_Imported, 0) = 1 AND CANCELED = 'C'");

            Console.WriteLine("Registros encontrados: " + rstDocFrom.RecordCount);

            while (!rstDocFrom.EoF)
            {
                rstDocTo.DoQuery("SELECT DocEntry FROM OINV WHERE U_CVA_DocEntryBase = " + rstDocFrom.Fields.Item(0).Value);
                string msg = String.Empty;
                Documents docFrom = (Documents)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                docFrom.GetByKey((int)rstDocTo.Fields.Item(0).Value);

                if (docFrom.Close() != 0)
                {
                    Console.WriteLine($"Fechamento NF:  {docFrom.DocEntry} - {ConnectionFrom.oCompany.GetLastErrorDescription()}");
                }
                else
                {
                    if (docFrom.Cancel() != 0)
                    {
                        Console.WriteLine($"Cancelamento NF:  {docFrom.DocEntry} - {ConnectionFrom.oCompany.GetLastErrorDescription()}");
                    }
                    else
                    {
                        rstUpdate.DoQuery($"UPDATE OINV SET U_CVA_Imported = 2 WHERE DocEntry = " + rstDocFrom.Fields.Item(0).Value);
                    }
                }
                
                rstDocFrom.MoveNext();

                Marshal.ReleaseComObject(docFrom);
                docFrom = null;
            }
        }
    }
}
