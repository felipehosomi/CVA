using CVA.Core.TransportLCM.SERVICE.OCPR;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.BLL
{
    public class InvoiceBLL
    {
        private Company _Company { get; set; }
        private BusinessPartnerContactDAO _BPContactDAO { get; set; }

        public InvoiceBLL(Company company, BusinessPartnerContactDAO BPContactDAO)
        {
            _Company = company;
            _BPContactDAO = BPContactDAO;
        }

        public string UpdateEmailField(int docEntry)
        {
            string msg = String.Empty;

            Documents doc = (Documents)_Company.GetBusinessObject(BoObjectTypes.oInvoices);
            try
            {
                doc.GetByKey(docEntry);

                List<string> emailList = _BPContactDAO.GetEmails(doc.CardCode);
                string emails = String.Empty;
                foreach (var item in emailList)
                {
                    emails += $";{item}";
                }

                if (!String.IsNullOrEmpty(emails))
                {
                    emails = emails.Substring(1);
                    doc.UserFields.Fields.Item("U_EmailEnvDanfe").Value = emails;
                    if (doc.Update() != 0)
                    {
                        msg = _Company.GetLastErrorDescription();
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(doc);
                doc = null;
            }

            return msg;
        }
    }
}
