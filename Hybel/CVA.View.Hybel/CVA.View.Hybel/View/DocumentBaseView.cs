using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.View.Hybel.BLL;
using SAPbouiCOM;
using System;

namespace CVA.View.Hybel.View
{
    public class DocumentBaseView : BaseForm
    {
        private static string CardCode = String.Empty;
        public static int Line;
        public static string ItemHybel;

        public override bool ItemEvent()
        {
            base.ItemEvent();
            if (!ItemEventInfo.Before_Action)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    CardCode = String.Empty;
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_VALIDATE)
                {
                    if (ItemEventInfo.ItemUID == "4" || ItemEventInfo.ItemUID == "54")
                    {
                        //var cardCode = ((EditText)Form.Items.Item("4").Specific).Value;
                        //try
                        //{
                        //    if (!String.IsNullOrEmpty(cardCode) && cardCode != CardCode)
                        //    {
                        //        if (ParceiroBLL.Check_BusinessPartnerStatus(cardCode))
                        //        {
                        //            EventFilterBLL.DisableEvents();

                        //            var resposta = SBOApp.Application.MessageBox(@"Este cliente possui pendências e/ou está inativo a mais de 31 dias. " +
                        //                                             "Deseja continuar?", 1, "Sim", "Não");
                        //            if (resposta == 2)
                        //            {
                        //                SBOApp.Application.Menus.Item("1281").Activate();
                        //            }
                        //        }
                        //    }
                        //    CardCode = cardCode;
                        //}
                        //catch (Exception ex)
                        //{
                        //    SBOApp.Application.SetStatusBarMessage(ex.Message);
                        //}
                        //finally
                        //{
                        //    EventFilterBLL.EnableEvents();
                        //}
                    }
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.ItemUID == "4" || ItemEventInfo.ItemUID == "54")
                    {
                        if (ItemEventInfo.CharPressed != 9)
                        {
                            CardCode = String.Empty;
                        }
                    }
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_LOST_FOCUS)
                {
                    if (ItemEventInfo.ItemUID == "38" && ItemEventInfo.ColUID == "U_CVA_Hybel")
                    {
                        //EventFilterBLL.DisableEvents();
                        Form.Freeze(true);
                        try
                        {
                            Matrix mtItem = Form.Items.Item("38").Specific as Matrix;
                            string hybel = ((EditText)mtItem.GetCellSpecific("U_CVA_Hybel", ItemEventInfo.Row)).Value;
                            Form form = this.Form;
                            ComboBox cb_Filial = Form.Items.Item("2001").Specific as ComboBox;

                            if (Line != ItemEventInfo.Row || ItemHybel != hybel)
                            {
                                new f2000003037().Show(Convert.ToInt32(cb_Filial.Value), hybel, ref form, ItemEventInfo.Row);
                            }
                        }
                        catch (Exception ex)
                        {
                            SBOApp.Application.SetStatusBarMessage(ex.Message);
                        }
                        finally
                        {
                            //EventFilterBLL.SetDefaultEvents();
                            Form.Freeze(false);
                        }
                    }
                    if (ItemEventInfo.ItemUID == "38" && ItemEventInfo.ColUID == "U_CVA_Concorrente")
                    {
                        //EventFilterBLL.DisableEvents();
                        Form.Freeze(true);
                        try
                        {
                            Matrix mtItem = Form.Items.Item("38").Specific as Matrix;
                            string concorrente = ((EditText)mtItem.GetCellSpecific("U_CVA_Concorrente", ItemEventInfo.Row)).Value;
                            //DBDataSource dt_Item = Form.DataSources.DBDataSources.Item("RDR1");
                            //dt_Item.GetValue("U_CVA_Concorrente", ItemEventInfo.Row - 1);
                            if (!String.IsNullOrEmpty(concorrente.Trim()))
                            {
                                int itemQty = ConcorrenteBLL.GetItemQuantity(concorrente);
                                if (itemQty == 0)
                                {
                                    SBOApp.Application.SetStatusBarMessage("Código não encontrado!");
                                }
                                else if (itemQty == 1)
                                {
                                    ((EditText)mtItem.GetCellSpecific("1", ItemEventInfo.Row)).Value = ConcorrenteBLL.GetItemCode(concorrente);
                                    // Setando novamente o campo, às vezes ele estava sendo apagado
                                    ((EditText)mtItem.GetCellSpecific("U_CVA_Concorrente", ItemEventInfo.Row)).Value = concorrente;
                                }
                                else
                                {
                                    Form form = this.Form;
                                    new f2000003032().Show(concorrente, ref form, ItemEventInfo.Row);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            SBOApp.Application.SetStatusBarMessage(ex.Message);
                        }
                        finally
                        {
                            //EventFilterBLL.SetDefaultEvents();
                            Form.Freeze(false);
                        }
                    }
                    //if (ItemEventInfo.ItemUID == "38" && ItemEventInfo.ColUID == "U_CVA_Engenharia")
                    //{
                    //    //EventFilterBLL.DisableEvents();
                    //    Form.Freeze(true);
                    //    try
                    //    {
                    //        Matrix mtItem = Form.Items.Item("38").Specific as Matrix;

                    //        string engenharia = ((EditText)mtItem.GetCellSpecific("U_CVA_Engenharia", ItemEventInfo.Row)).Value;
                    //        if (!String.IsNullOrEmpty(engenharia.Trim()))
                    //        {
                    //            string itemCode = ItemBLL.GetItemCodeByEngenharia(engenharia);
                    //            if (!String.IsNullOrEmpty(itemCode))
                    //            {
                    //                ((EditText)mtItem.GetCellSpecific("1", ItemEventInfo.Row)).Value = itemCode;
                    //            }
                    //            else
                    //            {
                    //                SBOApp.Application.SetStatusBarMessage("Código do item não encontrado!");
                    //            }
                    //            // Setando novamente o campo, às vezes ele estava sendo apagado
                    //            ((EditText)mtItem.GetCellSpecific("U_CVA_Engenharia", ItemEventInfo.Row)).Value = engenharia;
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        SBOApp.Application.SetStatusBarMessage(ex.Message);
                    //    }
                    //    finally
                    //    {
                    //        //EventFilterBLL.SetDefaultEvents();
                    //        Form.Freeze(false);
                    //    }
                    //}
                }
            }
          
            return true;
        }
    }
}
