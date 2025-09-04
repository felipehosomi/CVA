using Addon.CVA.View.Apetit.Cardapio.Helpers;
using CVA.View.Apetit.Cardapio.Helpers;
using SAPbouiCOM;
using System;

namespace CVA.View.Apetit.Cardapio.View
{
    public class ContratadoXPlanejado : BaseForm
    {
        public string IdPlan { get; set; }

        EditText edtC;
        Grid grdRes;
        DataTable dtRes;

        public ContratadoXPlanejado(string idPlan)
        {
            Type = "CARDCPLN";
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
            IdPlan = idPlan;
        }

        public override void Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        public override void CreateUserFields() { }

        internal override void LoadDefault(Form oForm)
        {
            //oForm.Freeze(true);

            edtC = (EditText)oForm.Items.Item("edtC").Specific;
            grdRes = (Grid)oForm.Items.Item("grdRes").Specific;
            dtRes = (DataTable)oForm.DataSources.DataTables.Item("dtR");

            edtC.Value = IdPlan;

            dtRes.ExecuteQuery($@"
                DO BEGIN
	                declare PlanId varchar(250) = '{IdPlan}';
	                declare Contrato varchar(250);
	                declare Qtd INT = 0;
	
	                SELECT top 1 {"U_CVA_ID_CONTRATO".Aspas()} INTO Contrato FROM {"@CVA_PLANEJAMENTO".Aspas()} WHERE {"Code".Aspas()} = PlanId;
	
	                SELECT distinct
                         TBG.{ "Code".Aspas()}
		                ,TBG.{ "U_CVA_DES_TP_PROT".Aspas()} AS { "Contratado".Aspas()}
                        , TBG.{ "U_CVA_INCIDENCIA".Aspas()}
		                ,(
			                SELECT 
				                COUNT({ "Code".Aspas()}) 
                            FROM { "@CVA_LN_PLANEJAMENTO".Aspas()} AS PL 
                                INNER JOIN { "OITM".Aspas()} AS OITM ON
                                    PL.{ "U_CVA_INSUMO".Aspas()} = OITM.{ "ItemCode".Aspas()}
					                AND OITM.{ "U_CVA_Familia".Aspas()} = TPP.{ "U_CVA_FAMILIA".Aspas()}
			                AND OITM.{ "U_CVA_Subfamilia".Aspas()} = TPP.{ "U_CVA_SUB_FAMILIA".Aspas()}
			                WHERE PL.{ "U_CVA_TIPO_PRATO".Aspas()} = TPRATO.{ "Code".Aspas()}
		                ) AS { "Planejado".Aspas()}		
	                FROM  { "@CVA_TABGRAMATURA".Aspas()} AS TBG
                        INNER JOIN { "@CVA_TIPOPROTEINA".Aspas()} AS TPP ON
                                TBG.{ "U_CVA_TIPO_PROTEINA".Aspas()} = TPP.{ "Code".Aspas()}
		                INNER JOIN { "@CVA_TIPOPRATO".Aspas()} AS TPRATO ON
                                TPRATO.{ "U_CVA_FAMILIA".Aspas()} = TPP.{ "U_CVA_FAMILIA".Aspas()}
                			AND	TPRATO.{ "U_CVA_SUB_FAMILIA".Aspas()} = TPP.{ "U_CVA_SUB_FAMILIA".Aspas()}

	                WHERE 	TBG.{ "U_CVA_ID_CONTRATO".Aspas()} = Contrato
                        AND TPRATO.{ "U_CVA_PROTEINA".Aspas()} = 'Y'	
                    GROUP BY
                        TBG.{ "Code".Aspas()}
		                ,TBG.{ "U_CVA_DES_TP_PROT".Aspas()}
		                ,TBG.{ "U_CVA_INCIDENCIA".Aspas()}
		                ,TPRATO.{ "Code".Aspas()}
	                ;
                END;
            ");

            //oForm.Freeze(false);
        }

        internal override void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            bubbleEvent = ret;
        }

        public override void SetFilters()
        {
            Filters.Add(MenuItem, BoEventTypes.et_MENU_CLICK);

            Filters.Add(Type, BoEventTypes.et_COMBO_SELECT);
            Filters.Add(Type, BoEventTypes.et_CHOOSE_FROM_LIST);
            Filters.Add(Type, BoEventTypes.et_PICKER_CLICKED);
            Filters.Add(Type, BoEventTypes.et_VALIDATE);
            Filters.Add(Type, BoEventTypes.et_LOST_FOCUS);
            Filters.Add(Type, BoEventTypes.et_FORM_DATA_ADD);
            Filters.Add(Type, BoEventTypes.et_FORM_DATA_UPDATE);
            Filters.Add(Type, BoEventTypes.et_FORM_DATA_LOAD);
            Filters.Add(Type, BoEventTypes.et_ITEM_PRESSED);
            Filters.Add(Type, BoEventTypes.et_MATRIX_LINK_PRESSED);
        }

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            bubbleEvent = ret;
        }

        public override void SetMenus() { }
    }
}