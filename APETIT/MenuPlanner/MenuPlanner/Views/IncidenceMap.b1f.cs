using System;
using SAPbouiCOM.Framework;
using MenuPlanner.Controllers;
using System.Globalization;

namespace MenuPlanner.Views
{
    [FormAttribute("MenuPlanner.Views.IncidenceMap", "Views/IncidenceMap.b1f")]
    class IncidenceMap : UserFormBase
    {
        public IncidenceMap()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtIncd = ((SAPbouiCOM.Matrix)(this.GetItem("mtIncd").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("btOk").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.VisibleAfter += new VisibleAfterHandler(this.Form_VisibleAfter);

        }

        private void OnCustomInitialize()
        {
            var fatherForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(CommonController.FormFatherType, CommonController.FormFatherCount);
            var plannerData = fatherForm.DataSources.DBDataSources.Item("@CVA_PLANEJAMENTO");

            if (!String.IsNullOrEmpty(plannerData.GetValue("DocEntry", plannerData.Offset)))
            {
                var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
                var dtIncidence = UIAPIRawForm.DataSources.DataTables.Item("dtIncidence");
                dtQuery.ExecuteQuery($@"select IGP1.""U_ProteinName"" AS ""ProteinName"", IGP1.""U_Incidence"" as ""Incidence"",
	                                           sum((select count(MNP1.""DocEntry"") 
                                                      from ""@CVA_LN_PLANEJAMENTO"" AS MNP1 
                                                     inner join OITM on OITM.""ItemCode"" = MNP1.""U_CVA_INSUMO"" 
	                                                   and OITM.""U_CVA_Familia"" = OPRT.""U_CVA_FAMILIA""
                                                     inner join ""@CVA_TIPOPRATO"" AS ODTP ON ODTP.""Code"" = MNP1.""U_CVA_TIPO_PRATO"" 
                                                       and ODTP.""U_CVA_PROTEINA"" = 'Y'	
                                                     where OMNP.""DocEntry"" = MNP1.""DocEntry"")) AS ""Planned""		
                                          from ""@CVA_PLANEJAMENTO"" as OMNP
                                         inner join ""@CVA_OIGP"" as OIGP on OIGP.""Code"" = OMNP.""U_CVA_ID_CONTRATO""
                                         inner join ""@CVA_IGP1"" as IGP1 on IGP1.""Code"" = OIGP.""Code""
                                         inner join ""@CVA_TIPOPROTEINA"" AS OPRT on OPRT.""Code"" = IGP1.""U_ProteinCode"" 
                                         where OMNP.""DocEntry"" = {plannerData.GetValue("DocEntry", plannerData.Offset)}
                                         group by IGP1.""U_ProteinName"", IGP1.""U_Incidence""");

                if (!dtQuery.IsEmpty)
                {
                    dtIncidence.Rows.Add(dtQuery.Rows.Count);

                    for (var i = 0; i < dtQuery.Rows.Count; i++)
                    {
                        dtIncidence.SetValue("ProteinName", i, dtQuery.GetValue("ProteinName", i));
                        dtIncidence.SetValue("Incidence", i, int.Parse(dtQuery.GetValue("Incidence", i).ToString()));
                        dtIncidence.SetValue("Planned", i, int.Parse(dtQuery.GetValue("Planned", i).ToString()));
                    }
                }

                mtIncd.LoadFromDataSourceEx();
            }

            mtIncd.AutoResizeColumns();

            CommonController.FormFatherType = String.Empty;
            CommonController.FormFatherCount = -1;
        }

        private void Form_VisibleAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            var formWidth = UIAPIRawForm.ClientWidth;
            var formHeight = UIAPIRawForm.ClientHeight;

            // Centralização do form
            UIAPIRawForm.Left = int.Parse(((Application.SBO_Application.Desktop.Width - formWidth) / 2).ToString(CultureInfo.InvariantCulture));
            UIAPIRawForm.Top = int.Parse(((Application.SBO_Application.Desktop.Height - formHeight) / 3).ToString(CultureInfo.InvariantCulture));
        }

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            UIAPIRawForm.Close();
        }

        private SAPbouiCOM.Matrix mtIncd;
        private SAPbouiCOM.Button Button0;
    }
}
