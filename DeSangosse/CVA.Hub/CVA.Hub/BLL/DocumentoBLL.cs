using Dover.Framework.DAO;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hub.BLL
{
    public class DocumentoBLL
    {
        private BusinessOneDAO _BusinessOneDAO { get; set; }
        private CodigoImpostoBLL _CodigoImpostoBLL { get; set; }

        public DocumentoBLL(BusinessOneDAO businessOneDAO, CodigoImpostoBLL codigoImpostoBLL)
        {
            _BusinessOneDAO = businessOneDAO;
            _CodigoImpostoBLL = codigoImpostoBLL;
        }


        public void UpdateObs(int docEntry, int objType)
        {
            List<string> codImpostoList = new List<string>();

            Documents doc = _BusinessOneDAO.GetBusinessObject((BoObjectTypes)objType);
            doc.GetByKey(docEntry);

            for (int i = 0; i < doc.Lines.Count; i++)
            {
                doc.Lines.SetCurrentLine(i);
                if (!String.IsNullOrEmpty(doc.Lines.TaxCode) && !codImpostoList.Contains(doc.Lines.TaxCode))
                {
                    codImpostoList.Add(doc.Lines.TaxCode);
                }
            }

            List<string> obsNFList = _CodigoImpostoBLL.GetObsNF(codImpostoList);
            foreach (var item in obsNFList)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    if (!doc.OpeningRemarks.ToLower().Contains(item.ToLower()))
                    {
                        if (!String.IsNullOrEmpty(doc.OpeningRemarks))
                        {
                            doc.OpeningRemarks += "; ";
                        }
                        doc.OpeningRemarks += item;
                    }
                }
            }

            _BusinessOneDAO.UpdateBusinessObject(doc);
            //if (doc.Update() != 0)
            //{
            //    throw new Exception(_BusinessOneDAO.)
            //}

            Marshal.ReleaseComObject(doc);
            doc = null;
        }
    }
}
