using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using CVA.View.Hybel.MODEL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.IO;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Consulta de Produto
    /// </summary>
    public class f2000003041 : BaseForm
    {
        Form Form;
        public static bool Changed = false;
        static _IApplicationEvents_LayoutKeyEventEventHandler LayoutKeyEventHandler;

        #region Constructor
        public f2000003041()
        {
            FormCount++;
        }

        public f2000003041(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000003041(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000003041(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Show
        public object Show(ItemModel itemModel)
        {
            Form = (Form)base.Show();
            Form.Freeze(true);
            try
            {
                this.CreateReport();

                Form.DataSources.UserDataSources.Item("ud_Item").Value = itemModel.ItemCode;
                Form.DataSources.UserDataSources.Item("ud_Desc").Value = itemModel.ItemName;
                Form.DataSources.UserDataSources.Item("ud_Fis").Value = itemModel.EstoqueFisico.ToString();
                Form.DataSources.UserDataSources.Item("ud_Enc").Value = itemModel.EstoqueEncomendado.ToString();
                Form.DataSources.UserDataSources.Item("ud_Res").Value = itemModel.EstoqueReservado.ToString();
                Form.DataSources.UserDataSources.Item("ud_Dis").Value = itemModel.EstoqueDisponivel.ToString();
                Form.DataSources.UserDataSources.Item("ud_Min").Value = itemModel.EstoqueMinimo.ToString();

                Form.DataSources.DataTables.Item("dt_OP").ExecuteQuery(SimuladorVendaBLL.GetOPsSQL(itemModel.ItemCode));

                Grid gr_OP = Form.Items.Item("gr_OP").Specific as Grid;
                gr_OP.RowHeaders.Width = 10;

                EditTextColumn cl_QtdeOP = (EditTextColumn)gr_OP.Columns.Item("Qtde");
                cl_QtdeOP.ColumnSetting.SumType = BoColumnSumType.bst_Auto;

                if (gr_OP.Rows.Count > 0)
                {
                    gr_OP.Rows.SelectedRows.Add(0);
                    int op = Convert.ToInt32(Form.DataSources.DataTables.Item("dt_OP").GetValue("Nr. OP", 0));
                    Form.DataSources.DataTables.Item("dt_Roteiro").ExecuteQuery(SimuladorVendaBLL.GetRoteiroSQL(op, itemModel.ItemCode));

                    Grid gr_Roteiro = (Grid)Form.Items.Item("gr_Roteiro").Specific;
                    gr_Roteiro.RowHeaders.Width = 0;
                }
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                Form.Freeze(false);
            }
            return Form;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
            {
                Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
            }
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    LayoutKeyEventHandler = new _IApplicationEvents_LayoutKeyEventEventHandler(LayoutKeyEvent);
                    SBOApp.Application.LayoutKeyEvent += LayoutKeyEventHandler;
                }
                else if (ItemEventInfo.EventType == BoEventTypes.et_FORM_CLOSE)
                {
                    SBOApp.Application.LayoutKeyEvent -= LayoutKeyEventHandler;
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_RESIZE)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                    try
                    {

                        Form.Freeze(true);
                        Item gr_OP = Form.Items.Item("gr_OP");
                        gr_OP.Height = (int)(Form.Height * 0.26);

                        Item gr_Roteiro = Form.Items.Item("gr_Roteiro");
                        gr_Roteiro.Top = (Form.Height / 2) - 40;
                        gr_Roteiro.Height = (int)(Form.Height * 45);
                    }
                    catch { }
                    finally
                    {
                        Form.Freeze(false);
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "gr_OP")
                    {
                        if (ItemEventInfo.Row >= 0)
                        {
                            try
                            {
                                Form.Freeze(true);
                                Grid gr_OP = (Grid)Form.Items.Item("gr_OP").Specific;
                                gr_OP.Rows.SelectedRows.Add(ItemEventInfo.Row);

                                int op = Convert.ToInt32( Form.DataSources.DataTables.Item("dt_OP").GetValue("Nr. OP", ItemEventInfo.Row));
                                Form.DataSources.DataTables.Item("dt_Roteiro").ExecuteQuery(SimuladorVendaBLL.GetRoteiroSQL(op, Form.DataSources.UserDataSources.Item("ud_Item").Value));

                                Grid gr_Roteiro = (Grid)Form.Items.Item("gr_Roteiro").Specific;
                                gr_Roteiro.RowHeaders.Width = 0;
                            }
                            catch (Exception ex)
                            {
                                SBOApp.Application.SetStatusBarMessage(ex.Message);
                            }
                            finally
                            {
                                Form.Freeze(false);
                            }
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region LayoutKeyEvent
        void LayoutKeyEvent(ref LayoutKeyInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            
            if (eventInfo.FormUID.StartsWith("f2000003041"))
            {
                Grid gr_OP = (Grid)Form.Items.Item("gr_OP").Specific;

                ItemModel itemModel = new ItemModel();

                itemModel.ItemCode = Form.DataSources.UserDataSources.Item("ud_Item").Value;
                itemModel.EstoqueFisico = Convert.ToDouble(Form.DataSources.UserDataSources.Item("ud_Fis").Value);
                itemModel.EstoqueReservado = Convert.ToDouble(Form.DataSources.UserDataSources.Item("ud_Res").Value);
                itemModel.EstoqueEncomendado = Convert.ToDouble(Form.DataSources.UserDataSources.Item("ud_Enc").Value);
                itemModel.EstoqueDisponivel = Convert.ToDouble(Form.DataSources.UserDataSources.Item("ud_Dis").Value);

                DataTable dt_OP = Form.DataSources.DataTables.Item("dt_OP");
                int op = Convert.ToInt32(dt_OP.GetValue("Nr. OP", gr_OP.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder)));

                RoteiroBLL.SetRoteiroLayout(itemModel, op);

                eventInfo.LayoutKey = UserBLL.GetUserId().ToString(); //Set the key of the layout 
            }
        }
        #endregion

        #region CreateReport
        private void CreateReport()
        {
            CompanyService service = SBOApp.Company.GetCompanyService();

            ReportTypesService rptTypeService = (ReportTypesService)service.GetBusinessService(ServiceTypes.ReportTypesService);
            ReportType newType = (ReportType)rptTypeService.GetDataInterface(ReportTypesServiceDataInterfaces.rtsReportType);

            newType.TypeName = "RoteiroOP";
            newType.AddonName = "RoteiroOP";
            newType.AddonFormType = "2000003041";
            newType.MenuID = "3041";

            bool createReport = ReportController.GetMenuReport(newType.AddonName);
            if (createReport)
            {
                ReportTypeParams newTypeParam = rptTypeService.AddReportType(newType);

                ReportLayoutsService rptService = (ReportLayoutsService)
                service.GetBusinessService(ServiceTypes.ReportLayoutsService);
                ReportLayout newReport = (ReportLayout)rptService.GetDataInterface(ReportLayoutsServiceDataInterfaces.rlsdiReportLayout);

                newReport.Author = SBOApp.Company.UserName;
                newReport.Category = ReportLayoutCategoryEnum.rlcCrystal;
                newReport.Name = "RoteiroOP";
                newReport.TypeCode = newTypeParam.TypeCode;
                ReportLayoutParams newReportParam = rptService.AddReportLayout(newReport);

                newType = rptTypeService.GetReportType(newTypeParam);
                newType.DefaultReportLayout = newReportParam.LayoutCode;
                rptTypeService.UpdateReportType(newType);

                BlobParams oBlobParams = (BlobParams)
                service.GetDataInterface(CompanyServiceDataInterfaces.csdiBlobParams);
                oBlobParams.Table = "RDOC";
                oBlobParams.Field = "Template";
                BlobTableKeySegment oKeySegment = oBlobParams.BlobTableKeySegments.Add();
                oKeySegment.Name = "DocCode";
                oKeySegment.Value = newReportParam.LayoutCode;

                string appPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

                if (!appPath.EndsWith(@"\"))
                {
                    appPath += @"\";
                }

                FileStream oFile = new FileStream(Path.Combine(appPath, "Reports", "RoteiroOP.rpt"), FileMode.Open);
                int fileSize = (int)oFile.Length;
                byte[] buf = new byte[fileSize];
                oFile.Read(buf, 0, fileSize);
                //oFile.Dispose();
                oFile.Close();
                Blob oBlob = (Blob)service.GetDataInterface(CompanyServiceDataInterfaces.csdiBlob);
                oBlob.Content = Convert.ToBase64String(buf, 0, fileSize);
                service.SetBlob(oBlobParams, oBlob);
                Form.ReportType = newType.TypeCode;
            }
            else
            {
                string docCode = ReportController.GetLayoutReport("RoteiroOP");
                Form.ReportType = docCode;
            }
        }
        #endregion
    }
}
