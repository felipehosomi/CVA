using CopyDocuments.DAL;
using CopyDocuments.Model;
using log4net;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyDocuments
{
    class Program
    {
        public static ILog Logger;

        static void Main(string[] args)
        {
            Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();

            //B1Connection connectionFrom = new B1Connection("manager", "1234", "SBO_Cianet", "192.168.0.203:30000", false, "sa", "@cia936net10", BoDataServerTypes.dst_MSSQL2012, "SAP");
            //B1Connection connectionTo = new B1Connection("manager", "cva$sap16", "SBO_Cianet_Nova", "192.168.0.203:30000", false, "sa", "@cia936net10", BoDataServerTypes.dst_MSSQL2012, "SAP");

            B1Connection connectionFrom = new B1Connection("manager", "manager", "SBODemoBR", "localhost:30000", false, "sa", "sa@123", BoDataServerTypes.dst_MSSQL2014, "FELIPE-PC");
            B1Connection connectionTo = new B1Connection("manager", "manager", "SBODemoBR_02", "localhost:30000", false, "sa", "sa@123", BoDataServerTypes.dst_MSSQL2014, "FELIPE-PC");

            Logger.Info("Conectando empresa origem");
            connectionFrom.Connect();
            Logger.Info("Conectando empresa destino");
            connectionTo.Connect();

            DatabaseDAO databaseDAO = new DatabaseDAO(connectionFrom, connectionTo);
            databaseDAO.Verify();

            //JournalEntryDAO journalEntryDAO = new JournalEntryDAO(connectionFrom, connectionTo);
            //journalEntryDAO.DoCopy();

            //ProductionOrderDAO productionOrderDAO = new ProductionOrderDAO(connectionFrom, connectionTo);
            //productionOrderDAO.DoCopy();

            //StockTransferDAO stockTransferDAO = new StockTransferDAO(connectionFrom, connectionTo);
            //stockTransferDAO.DoCopy();

            MarketingDocumentDAO marketingDocumentDAO = new MarketingDocumentDAO(connectionFrom, connectionTo);

            //marketingDocumentDAO.DoCopy(BoObjectTypes.oPurchaseDeliveryNotes, "OPDN", DocTypeEnum.In);
            //marketingDocumentDAO.DoCopy(BoObjectTypes.oPurchaseReturns, "ORPD", DocTypeEnum.In);
            //marketingDocumentDAO.DoCopy(BoObjectTypes.oPurchaseInvoices, "OPCH", DocTypeEnum.In);
            //marketingDocumentDAO.DoCopy(BoObjectTypes.oPurchaseInvoices, "ORPC", DocTypeEnum.In);

            marketingDocumentDAO.DoCopy(BoObjectTypes.oOrders, "ORDR", DocTypeEnum.Out);
            //marketingDocumentDAO.DoCopy(BoObjectTypes.oReturns, "ORDN", DocTypeEnum.Out);
            //marketingDocumentDAO.DoCopy(BoObjectTypes.oCreditNotes, "ORIN", DocTypeEnum.Out);
            //marketingDocumentDAO.DoCopy(BoObjectTypes.oDeliveryNotes, "ODLN", DocTypeEnum.Out);
            //marketingDocumentDAO.DoCopy(BoObjectTypes.oInvoices, "OINV", DocTypeEnum.Out);

            Logger.Info("Execução Finalizada!");            
            Console.ReadKey();
        }
    }
}
