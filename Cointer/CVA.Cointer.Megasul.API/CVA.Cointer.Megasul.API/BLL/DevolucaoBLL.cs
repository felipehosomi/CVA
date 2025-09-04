using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using CVA.Cointer.Megasul.API.Models.ServiceLayer;
using SBO.Hub.SBOHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CVA.Cointer.Megasul.API.BLL
{
    public class DevolucaoBLL
    {
        public string Create(NotaFiscalCancelamentoModel model, DocumentoMarketingModel invoice)
        {
            try
            {
                HanaDAO hanaDAO = new HanaDAO();
                ServiceLayer serviceLayer = new ServiceLayer();

                DateTime date;
                if (!DateTime.TryParseExact(model.identificador.data_hora, "dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                {
                    return $"Campo data_hora deve estar no formato dd/MM/yyyy HH:mm:ss";
                }

                invoice.DocumentStatus = null;
                invoice.DocEntry = null;
                invoice.U_CVA_DocNumCancelado = invoice.DocNum.Value;
                invoice.DocEntry = null;
                invoice.DocNum = null;
                invoice.DocDate = date;


                invoice = serviceLayer.PostAndGetAdded<DocumentoMarketingModel>("CreditNotes", "DocEntry", invoice);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }
    }
}