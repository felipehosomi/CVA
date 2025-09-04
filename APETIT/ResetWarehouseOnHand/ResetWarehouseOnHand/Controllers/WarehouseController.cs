using SAPbobsCOM;

namespace ResetWarehouseOnHand.Controllers
{
    internal class WarehouseController
    {
        public static void SetInventoryGenExit()
        {
            var recordset = (Recordset)SBOController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            recordset.DoQuery(@"select OITW.""ItemCode"", OITW.""WhsCode"", OITW.""OnHand"", OWHS.""BPLid""
                                  from OITW
                                 inner join OITT on OITT.""Code"" = OITW.""ItemCode""
                                 inner join OWHS on OWHS.""WhsCode"" = OITW.""WhsCode""
                                 where OITW.""OnHand"" > 0
                                   and OITT.""U_CVA_ResetWhs"" = 'Y'
                                   and OWHS.""U_CVA_ResetWhs"" = 'Y'
                                 order by OWHS.""BPLid""");

            while (!recordset.EoF)
            {
                Program.Logger.Info("Iniciando o processo de liquidação de estoque.");

                var bplId = recordset.Fields.Item("BPLid").Value.ToString();
                var document = (Documents)SBOController.Company.GetBusinessObject(BoObjectTypes.oInventoryGenExit);
                document.BPL_IDAssignedToInvoice = int.Parse(bplId);
                document.Comments = "Transação criada pelo serviço de liquidação de estoque.";

                while (!recordset.EoF && bplId == recordset.Fields.Item("BPLid").Value.ToString())
                {
                    Program.Logger.Info($"Item: {recordset.Fields.Item("ItemCode").Value} | Depósito: {recordset.Fields.Item("WhsCode").Value} | Quantidade: {recordset.Fields.Item("OnHand").Value}");

                    document.Lines.ItemCode = recordset.Fields.Item("ItemCode").Value.ToString();
                    document.Lines.WarehouseCode = recordset.Fields.Item("WhsCode").Value.ToString();
                    document.Lines.Quantity = double.Parse(recordset.Fields.Item("OnHand").Value.ToString());
                    document.Lines.Add();

                    recordset.MoveNext();
                }

                document.Add();

                if (SBOController.Company.GetLastErrorCode() == 0)
                {
                    Program.Logger.Info("Liquidação de estoque realizada com sucesso.");
                }
                else
                {
                    Program.Logger.Error(SBOController.Company.GetLastErrorCode().ToString(), SBOController.Company.GetLastErrorDescription());
                }
            }
        }
    }
}