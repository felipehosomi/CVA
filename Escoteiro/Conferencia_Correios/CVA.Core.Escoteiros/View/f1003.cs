using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.Core.Escoteiros.Arquivo;
using SAPbouiCOM;
using System;
using System.Collections;

namespace CVA.Core.Escoteiros.View
{
    //[CVA.AddOn.Common.Attributes.Form(3048)]
    public class f1003 : BaseForm
    {
        Form Form;
        Form _Form;
        public static string Path;

        #region Constructor
        public f1003()
        {
            FormCount++;
        }

        public f1003(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f1003(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f1003(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion


        public override object Show()
        {
            Form = (Form)base.Show();
            //((ComboBox)Form.Items.Item("tx_cli").Specific).AddValuesFromQuery(DAO.Query.ValuesRpt);
            CarregaFormulario();


            return Form;
        }


        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST && ItemEventInfo.ItemUID == "tx_Cliente")
                {
                    this.ChooseFromListBPName();
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "btn_ok")
                {
                    GerarRelatorio();

                }



            }
            return true;
        }

        public static void CarregaFormulario()
        {
            SAPbouiCOM.Form Form  = SBOApp.Application.Forms.ActiveForm; 
            //Form = SBOApp.Application.Forms.GetForm(ItemEventInfo.FormTypeEx, ItemEventInfo.FormTypeCount);
            Form.DataSources.UserDataSources.Add("U_De", SAPbouiCOM.BoDataType.dt_SHORT_NUMBER, 10);

            SAPbouiCOM.EditText oEdit = (SAPbouiCOM.EditText)Form.Items.Item("tx_PedIni").Specific;
            oEdit.DataBind.SetBound(true, "", "U_De");
            

            Form.DataSources.UserDataSources.Add("U_Ate", SAPbouiCOM.BoDataType.dt_SHORT_NUMBER, 10);
            oEdit = (SAPbouiCOM.EditText)Form.Items.Item("tx_PedFin").Specific;
            oEdit.DataBind.SetBound(true, "", "U_Ate");


            Form.DataSources.UserDataSources.Add("U_DtIni", SAPbouiCOM.BoDataType.dt_DATE, 10);
            oEdit = (SAPbouiCOM.EditText)Form.Items.Item("tx_dtIni").Specific;
            oEdit.DataBind.SetBound(true, "", "U_DtIni");
            oEdit.Value = DateTime.Now.ToString("yyyyMMdd");

            Form.DataSources.UserDataSources.Add("U_DtFin", SAPbouiCOM.BoDataType.dt_DATE, 10);
            oEdit = (SAPbouiCOM.EditText)Form.Items.Item("tx_dtFin").Specific;
            oEdit.DataBind.SetBound(true, "", "U_DtFin");
            oEdit.Value = DateTime.Now.ToString("yyyyMMdd");

            //Form.DataSources.UserDataSources.Add("ud_PN", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 30);
            SAPbouiCOM.EditText edit = (SAPbouiCOM.EditText)Form.Items.Item("tx_Cliente").Specific;
            edit.DataBind.SetBound(true, "", "ud_PN");

            SAPbouiCOM.CheckBox _edit = (SAPbouiCOM.CheckBox)Form.Items.Item("ck_Impre").Specific;
            _edit.DataBind.SetBound(true, "", "ud_Ck");





            //ck_Impresso

            //SAPbouiCOM.EditText _edit = (SAPbouiCOM.EditText)Form.Items.Item("tx_Cliente").Specific;
            //edit.DataBind.SetBound(true, "", "ud_PN");
        }


        public void GerarRelatorio()
        {
            var xml = new XMLReader();
            SAPbouiCOM.Form oForm = SBOApp.Application.Forms.ActiveForm;

            int PedidoIni = 0;
            int pedidoFin = 0;
            DateTime DtInicial = DateTime.Parse("1900/01/01");
            DateTime DtFinal = DateTime.Parse("1900/01/01");
            string cliente = string.Empty;
            string chekBox = string.Empty;
            string schema = string.Empty;

            PedidoIni = Convert.ToInt32(oForm.DataSources.UserDataSources.Item("U_De").Value);
            pedidoFin = Convert.ToInt32(oForm.DataSources.UserDataSources.Item("U_Ate").Value);

            DtInicial = Convert.ToDateTime(oForm.DataSources.UserDataSources.Item("U_DtIni").Value);
            DtFinal = Convert.ToDateTime(oForm.DataSources.UserDataSources.Item("U_DtFin").Value);

            //ComboBox cb = (ComboBox)oForm.Items.Item("tx_cli").Specific;
            //cliente = cb.Selected.Value;
            cliente = oForm.DataSources.UserDataSources.Item("ud_PN").Value;

            chekBox = oForm.DataSources.UserDataSources.Item("ud_Ck").Value;
            if (string.IsNullOrEmpty(chekBox))
            {
                chekBox = "N";
            }

            SAPbobsCOM.Company company = SBOApp.Company;
            schema = company.CompanyDB;


            Hashtable reportParams = new Hashtable();
            reportParams.Add("PedidoIni", PedidoIni);
            reportParams.Add("PedidoFim", pedidoFin);

            reportParams.Add("DataInicio", DtInicial);
            reportParams.Add("DataFinal", DtFinal);
            reportParams.Add("Impresso", chekBox);

            reportParams.Add("Schema@", schema);

            if (string.IsNullOrEmpty(cliente))
            {
                cliente = "*";
                
            }
            reportParams.Add("Cliente", cliente);

            CrystalReport crRelatorio = new CrystalReport();
            crRelatorio.ExecuteCrystalReport(@"C:\CVA Consultoria\Relatórios\Separação de Mercadoria.rpt", reportParams);




        }

        private void ChooseFromListBPName()
        {

            SAPbouiCOM.Form oForm = SBOApp.Application.Forms.Item(ItemEventInfo.FormUID);
            IChooseFromListEvent oCFLEvento = ((IChooseFromListEvent)(ItemEventInfo));
            ChooseFromList oCFL = oForm.ChooseFromLists.Item(oCFLEvento.ChooseFromListUID);
            DataTable oDataTable = oCFLEvento.SelectedObjects;

            if (oDataTable != null && oDataTable.Rows.Count > 0)
            {
                // Razão Social
                string cardCardName = oDataTable.GetValue("CardName", 0).ToString();
                string CardCode = oDataTable.GetValue("CardCode", 0).ToString();
                oForm.DataSources.UserDataSources.Item("ud_PN").Value = CardCode;

                //((EditText)oForm.Items.Item("tx_Cliente").Specific).Value = cardCardName;
                

            }

        }
    }
    
}
