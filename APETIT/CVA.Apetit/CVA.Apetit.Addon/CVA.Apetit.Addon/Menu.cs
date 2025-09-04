using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;

namespace CVA.Apetit.Addon
{
    class Menu
    {
        public void AddMenuItems()
        {
            AddMenuItems("43543", "mnuConsolCardapio", "CVA - Consolidação de Planejamento de Cardápio", 4, SAPbouiCOM.BoMenuType.mt_POPUP);

            //AddMenuItems("mnuConsolCardapio", "CVA_ConfParamConsol", "Configuração de Parâmetros", 1, SAPbouiCOM.BoMenuType.mt_STRING);
            AddMenuItems("mnuConsolCardapio", "CVA_ConfParamConsol", "Configuração de Parâmetros", 1, SAPbouiCOM.BoMenuType.mt_STRING);
            AddMenuItems("mnuConsolCardapio", "CVA_DefInsumos", "Definição de Ítens para Compra Direta", 2, SAPbouiCOM.BoMenuType.mt_STRING);
            AddMenuItems("mnuConsolCardapio", "CVA_CalendReceb", "Calendário de Recebimento de Mercadorias", 3, SAPbouiCOM.BoMenuType.mt_STRING);
            //AddMenuItems("mnuConsolCardapio", "CVA_ConfUnidade", "Configuração de Unidade de Medida Padrão", 4, SAPbouiCOM.BoMenuType.mt_STRING);
            AddMenuItems("mnuConsolCardapio", "CVA_DetPrevisao", "Detalhamento da Geração das Previsões", 4, SAPbouiCOM.BoMenuType.mt_STRING);
            //AddMenuItems("mnuConsolCardapio", "CVA_Consolidacao", "Consolidação", 5, SAPbouiCOM.BoMenuType.mt_STRING);

            

        }

        private void AddMenuItems(string MenuItem, string UniqueID, string TextMenu, int posicao, SAPbouiCOM.BoMenuType tipoMenu)
        {
            try
            {
                SAPbouiCOM.Menus oMenus = null;
                SAPbouiCOM.MenuItem oMenuItem = null;

                oMenus = Application.SBO_Application.Menus;

                SAPbouiCOM.MenuCreationParams oCreationPackage = null;
                oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
                oMenuItem = Application.SBO_Application.Menus.Item(MenuItem); 

                oCreationPackage.Type = tipoMenu;
                oCreationPackage.UniqueID = UniqueID;
                oCreationPackage.String = TextMenu;
                oCreationPackage.Enabled = true;
                oCreationPackage.Position = posicao;

                oMenus = oMenuItem.SubMenus;

                try
                {
                    //  If the manu already exists this code will fail
                    oMenus.AddEx(oCreationPackage);
                }
                catch (Exception e)
                {

                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            var FormTypeCount = -1;
            try
            {
                //if (pVal.BeforeAction)
                //    Class.Conexao.sbo_application.MessageBox(pVal.MenuUID);

                if (pVal.BeforeAction && pVal.MenuUID == "CVA_ConfParamConsol")
                {
                    //FrmConfigParamFilial activeForm = new FrmConfigParamFilial();
                    FrmConfiguracao activeForm = new FrmConfiguracao();
                    activeForm.Show();
                }
                //else if (pVal.BeforeAction && pVal.MenuUID == "CVA_ConfParamConsol1")
                //{
                //    FrmConfiguracao activeForm = new FrmConfiguracao();
                //    activeForm.Show();
                //}
                else if (pVal.BeforeAction && pVal.MenuUID == "CVA_DefInsumos")
                {

                    for (int i = 0; i < Application.SBO_Application.Forms.Count - 1; i++)
                    {
                        if (Application.SBO_Application.Forms.Item(i).TypeEx == "CVA.Apetit.Addon.FrmDefinicaoInsumosComprados")
                            FormTypeCount += 1;
                    }

                    if(FormTypeCount >= 0)
                    {
                        var activeForm = Application.SBO_Application.Forms.GetForm("CVA.Apetit.Addon.FrmDefinicaoInsumosComprados", FormTypeCount);
                        activeForm.Freeze(false);
                        activeForm.Visible = true;
                    }                    
                    else
                    {
                        FrmDefInsumoComprado activeForm = new FrmDefInsumoComprado();
                        activeForm.Show();
                    }
                    
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "CVA_CalendReceb")
                {
                    FormCalendario activeForm = new FormCalendario();
                    activeForm.ShowDialog();
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "CVA_ConfUnidade")
                {
                    //FrmConfigUnidMedida activeForm = new FrmConfigUnidMedida();
                    //activeForm.Show();
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "CVA_DetPrevisao")
                {
                    FrmDetPrevisoes activeForm = new FrmDetPrevisoes();
                    //FrmConsolidacao activeForm = new FrmConsolidacao();
                    activeForm.Show();
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "CVA_Consolidacao")
                {
                    FrmConsolidacao activeForm = new FrmConsolidacao();
                    activeForm.Show();
                }
                else if (!pVal.BeforeAction && pVal.MenuUID == "DelLineFrmDefInsumo")
                {
                    FrmDefInsumoComprado.RemoveMatrixLines();
                }
                else if (!pVal.BeforeAction && pVal.MenuUID == "AddLineFrmDefInsumo")
                {
                    FrmDefInsumoComprado.AddLineFrmDefInsumo();
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

    }
}
