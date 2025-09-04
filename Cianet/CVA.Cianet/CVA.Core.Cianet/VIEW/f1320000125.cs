using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.Core.Cianet.Model;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Cianet.VIEW
{
    public class f1320000125 : BaseForm
    {
        #region Atributos      

        public static List<String> list { get; set; }
        public static List<ListNumSerieModel> list_De_Ate { get; set; }

        #endregion

        #region Constructor
        public f1320000125()
        {
            FormCount++;
        }

        public f1320000125(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.IsSystemForm = true;
            this.ItemEventInfo = itemEvent;
        }

        public f1320000125(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f1320000125(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Eventos no Inicar do Form


        public override bool ItemEvent()
        {
            base.ItemEvent();
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    CarregaNumeroSerie();
                }
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.CharPressed == 9 || ItemEventInfo.CharPressed == 13)
                    {
                        if (ItemEventInfo.ItemUID == "22")
                        {
                            return this.AddSerial(list);
                        }
                    }


                    EditText et_To = Form.Items.Item("et_To").Specific as EditText;
                    EditText et_From = Form.Items.Item("et_From").Specific as EditText;

                    if (ItemEventInfo.ItemUID == "et_To" && ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN && ItemEventInfo.CharPressed == 9)
                    {
                        if (!String.IsNullOrEmpty(et_From.Value) && !String.IsNullOrEmpty(et_To.Value))
                        {
                            this.AddArray();
                        }
                        else
                        {
                            SBOApp.Application.SetStatusBarMessage("Informe o valor 'De' e o valor 'Até'");
                        }

                    }
                }
                else
                {
                    if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_CLICK)
                    {
                        if (ItemEventInfo.ItemUID == "bt_Array")
                        {
                            this.AddArray();
                        }
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.CharPressed == 9 || ItemEventInfo.CharPressed == 13)
                    {
                        if (ItemEventInfo.ItemUID == "22")
                        {
                            return this.AddSerial(list);
                        }
                    }
                }
            }
            return true;
        }

        #endregion

        #region Metodo Carrega os Numeros de Series

        private void CarregaNumeroSerie()
        {
            SBOApp.Application.SetStatusBarMessage("Carregando os Números de Série aguarde...", BoMessageTime.bmt_Short, false);

            Matrix mt_Serie = Form.Items.Item("5").Specific as Matrix;
            EditText et_Serie = Form.Items.Item("22").Specific as EditText;

            list = new List<String>();
            list.Add("");
            for (int i = 1; i <= mt_Serie.VisualRowCount; i++)
            {
                var aux = ((EditText)mt_Serie.GetCellSpecific("1", i)).Value.Trim();
                list.Add(aux);
            }
        }

        #endregion

        #region Atualiza os Numeros de Séries

        private List<String> AtualizaNumeroSerie()
        {
            SBOApp.Application.SetStatusBarMessage("Carregando os Números de Série aguarde...", BoMessageTime.bmt_Short, false);


            Matrix mt_Serie = Form.Items.Item("5").Specific as Matrix;
            EditText et_Serie = Form.Items.Item("22").Specific as EditText;

            list = new List<String>();
            list.Add("");
            for (int i = 1; i <= mt_Serie.VisualRowCount; i++)
            {
                var aux = ((EditText)mt_Serie.GetCellSpecific("1", i)).Value.Trim();
                list.Add(aux);
            }

            return list;
        }

        #endregion

        #region Adicona o Numero de Serie pelo Campo Procurar da tela

        private bool AddSerial(List<String> list)
        {
            try
            {
                Form.Freeze(true);
                EditText et_Serie = Form.Items.Item("22").Specific as EditText;
                Matrix mt_Item = Form.Items.Item("3").Specific as Matrix;
                Matrix mt_Serie = Form.Items.Item("5").Specific as Matrix;

                var primeiroCara = (((EditText)mt_Serie.GetCellSpecific("1", 1))).Value;

                if (primeiroCara != list[1])
                {

                    list = AtualizaNumeroSerie();
                }

                if (!String.IsNullOrEmpty(et_Serie.Value.Trim()))
                {


                    string serie = et_Serie.Value.Trim();


                    for (int i = 0; i <= list.Count; i++)
                    {
                        if (serie == list[i])
                        {
                            mt_Serie.Columns.Item("0").Cells.Item(i).Click();
                            var tst = (((EditText)mt_Serie.GetCellSpecific("1", i))).Value;
                            if (((EditText)mt_Serie.GetCellSpecific("1320000027", i)).Value.Trim().ToLower() == "sim")
                            {
                                SBOApp.Application.SetStatusBarMessage("Número de série selecionado indisponível");
                                return false;
                            }

                            Form.Items.Item("8").Click();
                            et_Serie.Value = String.Empty;
                            Form.Items.Item("1").Click();
                            list.RemoveAt(i);
                            return false;
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
                return false;
            }
            finally
            {
                try
                {
                    Form.Freeze(false);
                }
                catch
                {

                }
            }
        }

        #endregion

        #region Adiciona os Numeros de Serie pelo Intervalo DE - ATE

        private void AddArray()
        {

            Matrix mt_Item = Form.Items.Item("3").Specific as Matrix;
            Matrix mt_Serie = Form.Items.Item("5").Specific as Matrix;
            EditText et_From = Form.Items.Item("et_From").Specific as EditText;
            EditText et_To = Form.Items.Item("et_To").Specific as EditText;

            et_From.TabOrder = 0;
            et_To.TabOrder = 1;
            string de = "";

            try
            {
                Form.Freeze(true);

                list_De_Ate = new List<ListNumSerieModel>();

                var primeiroCara = (((EditText)mt_Serie.GetCellSpecific("1", 1))).Value;

                if (primeiroCara != list[1])
                {

                    list = AtualizaNumeroSerie();
                }

                if (!String.IsNullOrEmpty(et_From.Value) && !String.IsNullOrEmpty(et_To.Value))
                {
                    for (int i = 0; i <= list.Count; i++)
                    {
                        if (list[i] == et_From.Value && list[i] == et_To.Value)
                        {
                            var model = new ListNumSerieModel
                            {
                                index = i,
                                valor = list[i]
                            };
                            // adiciona na lista
                            list_De_Ate.Add(model);
                            break;
                        }

                        // Verifica se  o DE está vazio
                        if (String.IsNullOrEmpty(de))
                        {
                            // Verifica se o na lista é igual ao que está no campo
                            if (list[i] == et_From.Value)
                            {
                                // recebe o valor da lista
                                de = list[i];
                                var model = new ListNumSerieModel
                                {
                                    index = i,
                                    valor = list[i]

                                };
                                // adiciona na lista
                                list_De_Ate.Add(model);
                            }

                        }

                        // Verifica se  o ATE está vazio
                        else
                        {
                            // pega os valores
                            var model = new ListNumSerieModel
                            {
                                index = i,
                                valor = list[i]

                            };
                            // adiciona na lista
                            list_De_Ate.Add(model);
                            // verifica se o cara da lista é o mesmo do campo e sai fora
                            if (list[i] == et_To.Value)
                            {
                                break;
                            }

                        }

                    }

                    SBOApp.Application.SetStatusBarMessage("Processando Operação, Aguardar...", BoMessageTime.bmt_Medium, false);

                    mt_Serie.ClearSelections();
                    for (var i = 0; i < list_De_Ate.Count; i++)
                    {
                        mt_Serie.Columns.Item("0").Cells.Item(list_De_Ate[i].index).Click(BoCellClickType.ct_Regular, (int)BoModifiersEnum.mt_CTRL);
                        if (((EditText)mt_Serie.GetCellSpecific("1320000027", list_De_Ate[i].index)).Value.Trim().ToLower() == "sim")
                        {
                            SBOApp.Application.SetStatusBarMessage("Número de série selecionado indisponível");
                            return;
                        }
                    }

                    et_From.Value = String.Empty;
                    et_To.Value = String.Empty;
                    de = "";

                }
                else
                {
                    SBOApp.Application.SetStatusBarMessage("Informe o valor 'De' e o valor 'Até'");
                }
                Form.Items.Item("8").Click();
                if (Form.Mode == BoFormMode.fm_UPDATE_MODE)
                {
                    Form.Items.Item("1").Click();
                }

                CarregaNumeroSerie();


            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            finally
            {
                Form.Freeze(false);
                et_From.Value = String.Empty;
                et_To.Value = String.Empty;
                et_From.Active = true;
            }
        }


        #endregion

    }
}
