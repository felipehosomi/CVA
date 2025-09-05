using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.TransferenciaFiliais.BLL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.TransferenciaFiliais.View
{
    public class f133 : BaseForm
    {
        #region Constructor
        public f133()
        {
            FormCount++;
        }

        public f133(ItemEvent itemEvent)
        {
            this.IsSystemForm = true;
            this.ItemEventInfo = itemEvent;
        }

        public f133(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f133(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    Item oItem = Form.Items.Item("10000330");
                    Item btItem = Form.Items.Add("btGeraDev", BoFormItemTypes.it_BUTTON);
                    btItem.Top = oItem.Top;
                    btItem.Left = oItem.Left - 100;
                    btItem.Width = 100;
                    Button oButton = (Button)btItem.Specific;
                    oButton.Caption = "Gera Devolução";
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "btGeraDev")
                    {
                        Item btItem = Form.Items.Item("btGeraDev");
                        if (btItem.Enabled)
                        {
                            int DocNum = Convert.ToInt32(((EditText)Form.Items.Item("8").Specific).Value);
                            TransferenciaBLL.GeraEsbocoDevolucaoNFSaida(DocNum);
                        }
                    }
                }
            }

            return true;
        }

        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    Recordset oRs = SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;

                    string chaveAcesso = string.Empty;
                    oRs.DoQuery($"select U_ChaveAcesso from [@SKL25NFE] where U_DocEntry = '{Form.DataSources.DBDataSources.Item("OINV").GetValue("DocEntry",0)}' and U_inStatus = 3 and U_cdErro = 100 and U_tipoDocumento = 'NS' ");
                    chaveAcesso = (string)oRs.Fields.Item("U_chaveacesso").Value;

                    oRs.DoQuery(string.Format("select top 1 * from (\r\nselect 'Tipo' = 'E', DocEntry from ODRF where u_cva_nf_custo = '{0}' and U_chaveacesso='{1}' \r\nunion all\r\nselect 'Tipo' = 'D', DocEntry from ORIN where u_cva_nf_custo = '{0}' and U_chaveacesso='{1}'\r\n) as dados order by Tipo, DocEntry desc", ((EditText)Form.Items.Item("2036").Specific).Value.ToString(), chaveAcesso));
                    if (oRs.RecordCount > 0 && (((dynamic)oRs.Fields.Item("Tipo").Value == "D") ? true : false))
                    {
                        Item btItem = Form.Items.Item("btGeraDev");
                        btItem.Enabled = false;
                    }
                }
            }
            return true;
        }
    }
}
