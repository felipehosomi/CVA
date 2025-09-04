using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.View.ImportadorFolha.BLL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ImportadorFolha.VIEW
{
    /// <summary>
    /// Configurações Importação
    /// </summary>
    public class f2000006062 : BaseForm
    {
        private Form Form;

        #region Show
        public override object Show()
        {
            Form = (Form)base.Show();
            EditText et_Code = Form.Items.Item("et_Code").Specific as EditText;

            if (FolhaPagamentoBLL.ExisteConfig())
            {
                Form.Mode = BoFormMode.fm_FIND_MODE;
                Form.Freeze(true);
                et_Code.Item.Visible = true;
                et_Code.Value = "0001";
                Form.Items.Item("1").Click();
                et_Code.Item.Visible = false;
                Form.Freeze(false);
            }
            else
            {
                Form.Mode = BoFormMode.fm_ADD_MODE;
                et_Code.Value = "0001";
                this.AddRow();
            }

            return Form;
        }
        #endregion

        #region Constructor
        public f2000006062()
        {
            FormCount++;
        }

        public f2000006062(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000006062(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000006062(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region ItemEvent
        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType != BoEventTypes.et_FORM_UNLOAD)
                {
                    Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                }
                if (ItemEventInfo.EventType == SAPbouiCOM.BoEventTypes.et_LOST_FOCUS)
                {
                    if (ItemEventInfo.ItemUID == "mt_Fields")
                    {
                        Matrix mt_Fields = Form.Items.Item("mt_Fields").Specific as Matrix;
                        if (ItemEventInfo.Row == mt_Fields.RowCount)
                        {
                            if (!String.IsNullOrEmpty(((EditText)mt_Fields.GetCellSpecific("cl_Pos", ItemEventInfo.Row)).Value))
                            {
                                this.AddRow();
                            }
                        }
                    }
                }
            }
           
            return true;
        }
        #endregion

        public override bool FormDataEvent()
        {
            if (BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    if (FolhaPagamentoBLL.ExisteConfig())
                    {
                        SBOApp.Application.MessageBox("Impossível adicionar nova configuração. Já existe uma configuração adicionada");
                        return false;
                    }
                }
            }
            return true;
        }

        #region RightClickEvent
        public override bool RightClickEvent()
        {
            Form = SBOApp.Application.Forms.ActiveForm;
            if (Form.Mode == BoFormMode.fm_ADD_MODE || Form.Mode == BoFormMode.fm_UPDATE_MODE || Form.Mode == BoFormMode.fm_OK_MODE)
            {
                if (ContextMenuInfo.BeforeAction && ContextMenuInfo.EventType == BoEventTypes.et_RIGHT_CLICK)
                {
                    if (ContextMenuInfo.ItemUID == "mt_Fields")
                    {
                        if (ContextMenuInfo.Row >= 0)
                        {
                            this.CreateRightClickMenuItem();
                            Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
                            mt_Fields.SelectRow(ContextMenuInfo.Row, true, false);
                        }
                    }
                    else
                    {
                        if (Form.Menu.Exists("M6162"))
                        {
                            Form.Menu.RemoveEx("M6162");
                        }
                    }
                }
            }
            return true;
        }

        private void CreateRightClickMenuItem()
        {
            try
            {
                if (!Form.Menu.Exists("M6162"))
                {
                    MenuCreationParams oCreationPackage = (MenuCreationParams)(SBOApp.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams));
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "M6162";
                    oCreationPackage.String = "Remover Linha";
                    oCreationPackage.Enabled = true;
                    Form.Menu.AddEx(oCreationPackage);
                }
            }
            catch { }
        }
        #endregion

        private void AddRow()
        {
            DBDataSource dt_Fields = Form.DataSources.DBDataSources.Item("@CVA_FOLHA_ITEM");
            dt_Fields.Clear();

            Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;
            mt_Fields.AddRow();
        }
    }
}
