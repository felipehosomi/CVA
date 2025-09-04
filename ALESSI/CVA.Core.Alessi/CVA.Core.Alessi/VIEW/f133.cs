using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.VIEW
{
    public class f133 : BaseForm
    {
        private Form form;
        
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

        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction)
            { 
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    DBDataSource ds_OINV = Form.DataSources.DBDataSources.Item("OINV");
                    string docentry = ds_OINV.GetValue("DocEntry", ds_OINV.Offset);

                    Documents oDocOrigem = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oInvoices);
                    
                    if (!string.IsNullOrEmpty(docentry))
                    {
                        if (oDocOrigem.GetByKey(Convert.ToInt32(docentry)))
                        {
                            try
                            {
                                int qtde = TotalEmbalgem();

                                var pesoBruto = oDocOrigem.TaxExtension.NetWeight;

                                oDocOrigem.TaxExtension.PackQuantity = qtde;
                                oDocOrigem.TaxExtension.PackDescription = "VOL";
                                oDocOrigem.TaxExtension.GrossWeight = pesoBruto;

                                oDocOrigem.Update();
                                                                
                            }
                            catch (Exception ex)
                            {
                                SBOApp.Application.MessageBox(ex.ToString());
                            }
                        }
                    }                    
                }
                
            }            
            return true;
        }
        
        public int TotalEmbalgem()
        {
            var oMatrix = (Matrix)Form.Items.Item("38").Specific;
            var aux = 0;

            for (int i = 1; i <= oMatrix.VisualRowCount; i++)
            {
                var coluna1 = ((EditText)oMatrix.GetCellSpecific("13", i)).Value.ToString().Replace(".", ",");

                var Quantidade = Math.Truncate(Convert.ToDouble(coluna1));
                aux += Convert.ToInt32(Quantidade);
            }
            return aux;
        }       
    }
}
