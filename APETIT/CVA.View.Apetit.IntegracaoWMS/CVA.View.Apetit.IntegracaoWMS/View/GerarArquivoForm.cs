using CVA.View.Apetit.IntegracaoWMS.Helpers;
using CVA.View.Apetit.IntegracaoWMS.Model;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CVA.View.Apetit.IntegracaoWMS.View
{
    public class GerarArquivoForm : BaseForm
    {
        public GerarArquivoForm()
        {
            Type = "INTGERAQ";
            MenuItem = Type;
            FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\{Type}.srf";
        }

        public override void CreateUserFields()
        {
        }

        internal override void LoadDefault(Form oForm)
        {
            Filters.Add(oForm.TypeEx, BoEventTypes.et_CLICK);

            // Armazenagem
            CheckBox ck_Seco = oForm.Items.Item("ck_Seco").Specific;
            CheckBox ck_Resfriado = oForm.Items.Item("ck_Resf").Specific;
            CheckBox ck_Congelado = oForm.Items.Item("ck_Cong").Specific;

            ck_Seco.Checked = true;
            ck_Resfriado.Checked = true;
            ck_Congelado.Checked = true;


            // Status Picking
            CheckBox ck_NS = oForm.Items.Item("ck_NS").Specific;
            CheckBox ck_SP = oForm.Items.Item("ck_SP").Specific;
            CheckBox ck_LP = oForm.Items.Item("ck_LP").Specific;
            CheckBox ck_Sel = oForm.Items.Item("ck_Sel").Specific;
            
            ck_NS.Checked = true;
            ck_SP.Checked = true;
            ck_LP.Checked = true;
            ck_Sel.Checked = true;



            //CarregaComboBox(oForm);
            //oForm.Freeze(true);
            //var f = oForm != null ? oForm : B1Connection.Instance.Application.Forms.ActiveForm;
            //CreateChooseFromList(f);
            //oForm.Freeze(false);
        }

        internal override void MenuEvent(Application Application, ref MenuEvent pVal, out bool bubbleEvent)
        {
            var ret = true;
            var openMenu = OpenMenu(MenuItem, FilePath, pVal);

            if (!string.IsNullOrEmpty(openMenu))
            {
                ret = false;
                Application.SetStatusBarMessage(openMenu);
            }

            bubbleEvent = ret;
        }

        internal override void ItemEvent(Application Application, string FormUID, ref ItemEvent pVal, out bool bubbleEvent)
        {
            var ret = true;

            try
            {
                if (pVal.FormTypeEx.Equals(TYPEEX))
                {
                    var oForm = Application.Forms.Item(pVal.FormUID);
                    try
                    {
                        if (!pVal.BeforeAction)
                        {
                            //var CBestocagem = ((ComboBox)oForm.Items.Item("cb_Estoc").Specific);
                            //if (pVal.ItemUID.Equals("cb_Estoc") && pVal.EventType.Equals(BoEventTypes.et_CLICK))
                            //{
                            //    var t = CBestocagem.ValidValues.Count;

                            //    if (t <= 0)
                            //    {
                            //        var b = CarregaListaComboBox();
                            //        if (t > 0)
                            //        {
                            //            foreach (var item in b)
                            //            {
                            //                CBestocagem.ValidValues.Remove(item.Code, BoSearchKey.psk_Index);
                            //            }

                            //        }
                            //        foreach (var item in b)
                            //        {
                            //            CBestocagem.ValidValues.Add(item.Code, item.Descrption);
                            //        }
                            //    }
                            //}




                            var oGrid = (IGrid)oForm.Items.Item("grdLinhas").Specific;
                            var rota = ((IEditText)oForm.Items.Item("edtDCtrS").Specific).Value;
                            var de = ((IEditText)oForm.Items.Item("edtDDataS").Specific).Value;
                            var ate = ((IEditText)oForm.Items.Item("edtDataSA").Specific).Value;

                            var  ck_Seco      = ((CheckBox)oForm.Items.Item("ck_Seco").Specific).Checked;
                            var  ck_Resfriado = ((CheckBox)oForm.Items.Item("ck_Resf").Specific).Checked;
                            var  ck_Congelado = ((CheckBox)oForm.Items.Item("ck_Cong").Specific).Checked;

                            var ck_NS  = ((CheckBox)oForm.Items.Item("ck_NS").Specific).Checked;
                            var ck_SP  = ((CheckBox)oForm.Items.Item("ck_SP").Specific).Checked;
                            var ck_LP  = ((CheckBox)oForm.Items.Item("ck_LP").Specific).Checked;
                            var ck_Sel = ((CheckBox)oForm.Items.Item("ck_Sel").Specific).Checked;

                            #region Verifica Combos Armazenagem
                            var seco = string.Empty;
                            var resfriado = string.Empty;
                            var congelado = string.Empty;
                            
                            if (ck_Seco)
                            {
                                seco = "1";
                            }
                            if (ck_Resfriado)
                            {
                                resfriado = "2";
                            }
                            if (ck_Congelado)
                            {
                                congelado = "3";
                            }

                            var estocagem = string.Empty;

                            if (!string.IsNullOrEmpty(seco))
                            {
                                estocagem = seco;
                            }
                            if (!string.IsNullOrEmpty(resfriado))
                            {
                                if (!string.IsNullOrEmpty(estocagem))
                                {
                                    estocagem += "," + resfriado;
                                }
                                else
                                {
                                    estocagem = resfriado;
                                }
                                
                            }
                            if (!string.IsNullOrEmpty(congelado))
                            {
                                if (!string.IsNullOrEmpty(estocagem))
                                {
                                    estocagem += "," + congelado;
                                }
                                else
                                {
                                    estocagem = congelado;
                                }
                            }

                            #endregion

                            #region Verifica Campos Status Picking

                            var NaoSelecionado = string.Empty;
                            var SelecionadoParcial = string.Empty;
                            var LiberadoParcial = string.Empty;
                            var Selecionado = string.Empty;

                            if (ck_NS)
                            {
                                NaoSelecionado = "N";
                            }
                            if (ck_SP)
                            {
                                SelecionadoParcial = "P";
                            }
                            if (ck_LP)
                            {
                                LiberadoParcial = "R";
                            }
                            if (ck_Sel)
                            {
                                Selecionado = "Y";
                            }
                                                     

                            #endregion


                            #region Botão Pesquisar
                            if (pVal.ItemUID.Equals("btnSearch") && pVal.EventType.Equals(BoEventTypes.et_CLICK) && oForm.Items.Item("btnSearch").Enabled)
                            {
                                //oForm.Freeze(true);
                                if (!string.IsNullOrEmpty(rota) && !string.IsNullOrEmpty(de) && !string.IsNullOrEmpty(ate))
                                    PesquisarGrid(oForm, rota, DIHelper.Format_StringToDate(de), DIHelper.Format_StringToDate(ate),estocagem, NaoSelecionado,SelecionadoParcial,LiberadoParcial,Selecionado);
                            }
                            #endregion  

                            #region Botão Gerar
                            if (pVal.ItemUID.Equals("btnGerar") && pVal.EventType.Equals(BoEventTypes.et_CLICK) && oForm.Items.Item("btnGerar").Enabled)
                            {
                                //oForm.Freeze(true);
                                GerarArquivoIntegracao(Application, rota, DIHelper.Format_StringToDate(de), DIHelper.Format_StringToDate(ate));
                            }
                            #endregion  

                          
                            oForm.Items.Item("btnSearch").Enabled = !string.IsNullOrEmpty(rota)
                                                                 && !string.IsNullOrEmpty(de)
                                                                 && !string.IsNullOrEmpty(ate);

                            oForm.Items.Item("btnGerar").Enabled = oGrid.Rows.Count > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Application.MessageBox(ex.Message);
                    }
                    finally
                    {
                        //oForm.Freeze(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SetStatusBarMessage(ex.Message);
                ret = false;
            }

            bubbleEvent = ret;
        }

        private void PesquisarGrid(Form oForm, string rota, DateTime de, DateTime ate, string estocagem, string NaoSelecionado, string SelecionadoParcial, string LiberadoParcial, string Selecionado)
        {

            var oGrid = (Grid)oForm.Items.Item("grdLinhas").Specific;
            var sql = ($"CALL {"SPC_CVA_INTEGRAWMS_LISTAR".Aspas()}('{rota}', '{de.ToString("yyyyMMdd")}', '{ate.ToString("yyyyMMdd")}','{estocagem}', '{NaoSelecionado}','{SelecionadoParcial}','{LiberadoParcial}','{Selecionado}');");
            oGrid.DataTable.ExecuteQuery(sql);
            if (oGrid.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(oGrid.DataTable.Columns.Item(0).Cells.Item(0).Value?.ToString()))
                    oGrid.DataTable.Clear();
                else
                    oGrid.Columns.Item(0).Type = BoGridColumnType.gct_CheckBox;
            }
        }

        private void GerarArquivoIntegracao(Application Application, string rota, DateTime de, DateTime ate)
        {
            var oForm = Application.Forms.ActiveForm;
            var oGrid = (Grid)oForm.Items.Item("grdLinhas").Specific;

            SAPbouiCOM.ProgressBar oProgBar;
            //Application.SetStatusBarMessage("Gerando Arquivo de Integração...",BoMessageTime.bmt_Medium,false);
            oProgBar = Application.StatusBar.CreateProgressBar("Gerando Arquivo de Integração...", 27, false);

            oProgBar.Maximum = oGrid.Rows.Count;

         

            #region Obter Lista de ItemCode e DocEntry

            var dicDocEntries = new Dictionary<string, List<string>>();
            for (int i = 0; i < oGrid.Rows.Count; i++)
            {
                if ("Y".Equals(oGrid.DataTable.Columns.Item(0).Cells.Item(i).Value))
                {
                    var itemCode = $"'{oGrid.DataTable.Columns.Item(4).Cells.Item(i).Value?.ToString()}'";
                    var docEntry = $"{oGrid.DataTable.Columns.Item(1).Cells.Item(i).Value?.ToString()}";

                    if (!dicDocEntries.ContainsKey(docEntry))
                        dicDocEntries.Add(docEntry, new List<string> { itemCode });
                    else
                        dicDocEntries[docEntry].Add(itemCode);
                }
                try
                {
                    oProgBar.Value += 1;
                }
                catch { }
            }

            #endregion

            var dir = DIHelper.OpenFolderBrowserDialog();
            if (!string.IsNullOrEmpty(dir))
            {
                if (Directory.Exists(Path.GetDirectoryName(dir)))
                {
                    var fileName = $"{rota}_{DateTime.Now.ToString("yyyyMMdd")}_{DateTime.Now.ToString("HHmmss")}.txt";
                    File.WriteAllText(Path.Combine(dir, fileName), FileHeader.GetItemDataForFile(dicDocEntries, de, ate).AsFileString());

                    Application.MessageBox("Arquivo Gerado com Sucesso...",1);

                }
            }


            System.Runtime.InteropServices.Marshal.ReleaseComObject(oProgBar);
            oProgBar = null;

            oForm.Freeze(true);
            oGrid.DataTable.Clear();

            var Rota = ((IEditText)oForm.Items.Item("edtDCtrS").Specific);
            var De = ((IEditText)oForm.Items.Item("edtDDataS").Specific);
            var Ate = ((IEditText)oForm.Items.Item("edtDataSA").Specific);


            Rota.Value = "";
            De.Value = "";
            Ate.Value = "";

            oForm.Items.Item("edtDCtrS").Click();

            oForm.Freeze(false);

        }

        public override void SetFilters()
        {
            Filters.Add(MenuItem, BoEventTypes.et_MENU_CLICK);
        }

        internal override void FormDataEvent(Application Application, ref BusinessObjectInfo BusinessObjectInfo, out bool bubbleEvent)
        {
            var ret = true;

            bubbleEvent = ret;
        }

        public override void SetMenus()
        {
            Helpers.Menus.Add("INTEGWMS", MenuItem, "Gerar arquivo de Integração", 3, BoMenuType.mt_STRING);
        }


        private List<EstocagemModel> CarregaListaComboBox()
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)B1Connection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);            

            var sql = string.Format($"CALL {"SPC_CVA_VALUES_ESTOCAGEM".Aspas()} () ");
            var listaEsrocagem = new List<EstocagemModel>();

            oRecordSet.DoQuery(sql);
            while (!oRecordSet.EoF)
            {
                var model = new EstocagemModel();

                model.Code = oRecordSet.Fields.Item(0).Value;
                model.Descrption = oRecordSet.Fields.Item(1).Value;

                listaEsrocagem.Add(model);
                

                oRecordSet.MoveNext();
            }

            return listaEsrocagem;
        }
    }
}