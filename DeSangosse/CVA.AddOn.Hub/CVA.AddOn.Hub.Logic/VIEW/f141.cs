using CVA.AddOn.Common;
using CVA.AddOn.Hub.Logic.BLL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class f141 : DocumentBaseView
    {
        #region Constructor
        public f141()
        {
            FormCount++;
        }

        public f141(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f141(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f141(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool FormDataEvent()
        {
            if (base.FormDataEvent())
            {
                if (BusinessObjectInfo.BeforeAction && BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    //Form = SBOApp.Application.Forms.ActiveForm;
                    DBDataSource dt_OPCH = Form.DataSources.DBDataSources.Item("OPCH");
                    if (dt_OPCH.GetValue("CANCELED", dt_OPCH.Offset)  != "C") // Se não é cancelamento
                    {
                        // Se foi setado o tipo do frete é porque é documento de frete
                        if (!String.IsNullOrEmpty(dt_OPCH.GetValue("U_CVA_Tipo_Frete", dt_OPCH.Offset).Trim()))
                        {
                            DBDataSource dt_PCH1 = Form.DataSources.DBDataSources.Item("PCH1");

                            List<int> baseEntryList = new List<int>();
                            int baseType = 0;

                            for (int i = 0; i < dt_PCH1.Size; i++)
                            {
                                if (!String.IsNullOrEmpty(dt_PCH1.GetValue("BaseType", i).Trim()))
                                {
                                    baseType = Convert.ToInt32(dt_PCH1.GetValue("BaseType", i));

                                    int baseEntry = Convert.ToInt32(dt_PCH1.GetValue("BaseEntry", i));
                                    if (!baseEntryList.Contains(baseEntry))
                                    {
                                        baseEntryList.Add(baseEntry);
                                    }
                                }
                            }

                            if (baseType != 0)
                            {
                                double docTotal = Convert.ToDouble(dt_OPCH.GetValue("DocTotal", dt_OPCH.Offset).Replace(".", ","));

                                NotaFiscalEntradaBLL notaFiscalEntradaBLL = new NotaFiscalEntradaBLL();
                                if (!notaFiscalEntradaBLL.ValidaTotalDocumentoBase((BoObjectTypes)baseType, baseEntryList, docTotal))
                                {
                                    ErrorMessage = "Valor total do documento não confere com valor do documento base!";
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
