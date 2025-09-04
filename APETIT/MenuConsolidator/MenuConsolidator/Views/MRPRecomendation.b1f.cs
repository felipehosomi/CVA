using MenuConsolidator.Controllers;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MenuConsolidator.Views
{
    [FormAttribute("65217", "Views/MRPRecomendation.b1f")]
    class MRPRecomendation : SystemFormBase
    {
        public MRPRecomendation()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.btRecalc = ((SAPbouiCOM.Button)(this.GetItem("btRecalc").Specific));
            this.btRecalc.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btRecalc_PressedAfter);
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
        }

        #region [ Item Events ]
        private void btRecalc_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var matrix = ((SAPbouiCOM.Matrix)(this.GetItem("3").Specific));
            var progessBar = SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.CreateProgressBar($"", matrix.RowCount, false);

            try
            {
                UIAPIRawForm.Freeze(true);

                Application.SBO_Application.StatusBar.SetText("Realizando o recalculo das dastas de vencimento das recomendações.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                CommonController.InProcess = true;

                var dtQuery = UIAPIRawForm.DataSources.DataTables.Item("dtQuery");
                var docEntries = new List<int>();

                for (var i = 1; i <= matrix.RowCount; i++)
                {
                    progessBar.Text = $"Recalculando linha {i}/{matrix.RowCount}.";
                    progessBar.Value += 1;

                    var docEntryCol = ((SAPbouiCOM.EditText)matrix.Columns.Item("21").Cells.Item(i).Specific);
                    docEntries.Add(int.Parse(docEntryCol.Value));
                }

                string sql = $@"do begin
	                                        params = select row_number()over(partition by ORCM.""DocEntry"" order by PAM1.""U_ItemCode"" nulls last, PAM1.""U_SFamilia"" nulls last, PAM1.""U_Familia"" nulls last, PAM1.""U_ItmsGrpCod"" nulls last) as ""LineNum"",
                                                            ORCM.""DocEntry"", ORCM.""DueDate"", 
				                                            PAM1.""U_CardCode"", PAM1.""U_Calendar"", PAM1.""U_NonDeliveryDate"", PAM1.""U_LeadTime"",
					                                        IFNULL(ITM9.""Price"", ITM1.""Price"") ""Price""
                                                       from ORCM 
			                                          inner join OWHS on OWHS.""WhsCode"" = ORCM.""Warehouse""
			                                          inner join OITM on OITM.""ItemCode"" = ORCM.""ItemCode""
			                                          inner join ""@CVA_OPAM"" as OPAM on OPAM.""Code"" = OWHS.""BPLid""
			                                          inner join ""@CVA_PAM1"" as PAM1 on PAM1.""Code"" = OPAM.""Code""
			                                            and (PAM1.""U_ItemCode"" = OITM.""ItemCode""
			                                             or PAM1.""U_SFamilia"" = OITM.""U_CVA_Subfamilia""
			                                             or PAM1.""U_Familia"" = OITM.""U_CVA_Familia""
			                                             or PAM1.""U_ItmsGrpCod"" = OITM.""ItmsGrpCod"")
			                                           left join ITM1 on ITM1.""ItemCode"" = OITM.""ItemCode""
			                                            and ITM1.""PriceList"" = PAM1.""U_ListNum""
                                                       left join ITM9  on ITM9.""ItemCode"" = OITM.""ItemCode""
                                                        and ITM9.""PriceList"" = PAM1.""U_ListNum""
                                                      where ORCM.""DocEntry"" in ({String.Join(",", docEntries).Replace(",", "," + Environment.NewLine)});
	
	                                        deliveryDate = select OPRM.""DocEntry"", CLN1.""Code"", row_number()over(partition by OPRM.""DocEntry"" order by CLN1.""U_Date"" desc) as ""LineNum"", CLN1.""U_Date""
				                                             from :params as OPRM
				                                            inner join ""@CVA_CLN1"" as CLN1 on CLN1.""Code"" = OPRM.""U_Calendar""
				                                            where CLN1.""U_Date"" <= add_days(OPRM.""DueDate"", coalesce(OPRM.""U_LeadTime"", 0) * -1)
                                                              and OPRM.""LineNum"" = 1;

                                            nonDeliveryDate = select OPRM.""DocEntry"", NDL1.""Code"", NDL1.""U_Rescheduling""
				                                                from :params as OPRM
				                                               inner join :deliveryDate as OCLN on OCLN.""Code"" = OPRM.""U_Calendar""
                                                                 and OCLN.""DocEntry"" = OPRM.""DocEntry""
                                                                 and OCLN.""LineNum"" = 1
				                                               inner join ""@CVA_NDL1"" as NDL1 on NDL1.""Code"" = OPRM.""U_NonDeliveryDate""
   						                                         and NDL1.""U_Date"" = OCLN.""U_Date""
                                                               where OPRM.""LineNum"" = 1;			    
		                             
	                                        update ORCM 
   	                                           set ORCM.""OrderType"" = case when coalesce(OPRM.""U_CardCode"", ORCM.""CardCode"") is null then 'R' else 'P' end, 
                                                   ORCM.""CardCode"" = coalesce(OPRM.""U_CardCode"", ORCM.""CardCode""),
                                                   ORCM.""Price""	= coalesce(OPRM.""Price"", 0.0),
                                                   ORCM.""PriceBefDi""	= coalesce(OPRM.""Price"", 0.0),
                                                   ORCM.""DueDate"" = coalesce(ONDL.""U_Rescheduling"", OCLN.""U_Date"", ORCM.""DueDate"")		                          	    
                                              from ORCM 
                                             inner join :params as OPRM on OPRM.""DocEntry"" = ORCM.""DocEntry""
                                               and OPRM.""LineNum"" = 1
                                              left join :deliveryDate as OCLN on OCLN.""DocEntry"" = ORCM.""DocEntry""
                                               and OCLN.""LineNum"" = 1
                                              left join :nonDeliveryDate as ONDL on ONDL.""DocEntry"" = ORCM.""DocEntry"";                             
                                        end";

                dtQuery.ExecuteQuery(sql);

                Application.SBO_Application.StatusBar.SetText("Recalculo finalizado. A tela de Recomendações do MRP será fechada para a atualização dos dados em tela.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);

                UIAPIRawForm.Freeze(false);

                GetItem("211").Click();
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                progessBar.Stop();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(progessBar);
                CommonController.InProcess = false;
            }
        }
        #endregion

        private SAPbouiCOM.Button btRecalc;
    }
}
