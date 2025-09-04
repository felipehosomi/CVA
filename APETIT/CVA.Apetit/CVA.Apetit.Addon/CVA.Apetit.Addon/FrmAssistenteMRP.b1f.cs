using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace CVA.Apetit.Addon
{
    [FormAttribute("1320000030", "FrmAssistenteMRP.b1f")]
    class FrmAssistenteMRP : SystemFormBase
    {
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.Button Button1;

        public FrmAssistenteMRP()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("btFiltrar").Specific));
            this.Button1.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button1_ClickBefore);
            this.OnCustomInitialize();
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {

        }

        private void OnCustomInitialize()
        {
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("1320000005").Specific));
        }

        private void Button1_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            string sql, s;
            int absID, tot;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                for (int i = this.Matrix0.VisualRowCount; i > 0; i--)
                {
                    s = ((SAPbouiCOM.EditText)this.Matrix0.Columns.Item("1320000019").Cells.Item(i).Specific).Value;
                    Int32.TryParse(s, out absID);

                    if (absID > 0)
                    {
                        sql = string.Format(@"
SELECT COUNT(1) AS ""Cont""
FROM OMSN T0
    INNER JOIN OFCT T1 ON T1.""AbsID"" = T0.""FCTAbs""
WHERE T1.""AbsID"" = {0}
", absID);
                        oRec.DoQuery(sql);
                        if (oRec.RecordCount > 0)
                        {
                            s = oRec.Fields.Item("Cont").Value.ToString();
                            Int32.TryParse(s, out tot);

                            if (tot > 0)
                                this.Matrix0.DeleteRow(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Class.Conexao.sbo_application.MessageBox(ex.Message);
            }
        }
    }
}
