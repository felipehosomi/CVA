using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Common.Util;
using CVA.Core.ObrigacoesFiscais.BLL;
using CVA.Core.ObrigacoesFiscais.DAO.Resources;
using CVA.Core.ObrigacoesFiscais.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ObrigacoesFiscais.VIEW
{
    /// <summary>
    /// Link campos Pai X Filho
    /// </summary>
    public class f2000004006 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000004006()
        {
            FormCount++;
        }

        public f2000004006(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000004006(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000004006(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public object Show(string parentCode, string parentObjType, string parentObjName, string childCode)
        {
            Form = (Form)base.Show();
            Form.Freeze(true);
            try
            {
                Form.EnableMenu("1292", true);
                Form.EnableMenu("1293", true);

                FileMappingModel childModel = new CrudController("@CVA_FILE_MAP").RetrieveModel<FileMappingModel>($"Code = '{childCode}'");

                Matrix mt_Fields = (Matrix)Form.Items.Item("mt_Fields").Specific;

                string sql = FileBLL.GetSQL(parentObjType, parentObjName);
                DataTable dt_Parent = Form.DataSources.DataTables.Item("dt_Parent");
                dt_Parent.ExecuteQuery(sql);

                sql = FileBLL.GetSQL(childModel.ObjectType, childModel.ObjectName);
                DataTable dt_Child = Form.DataSources.DataTables.Item("dt_Child");
                dt_Child.ExecuteQuery(sql);

                ComboBox cl_Parent = (ComboBox)mt_Fields.GetCellSpecific("cl_Parent", 0);
                for (int i = 0; i < dt_Parent.Columns.Count; i++)
                {
                    cl_Parent.ValidValues.Add(dt_Parent.Columns.Item(i).Name, dt_Parent.Columns.Item(i).Name);
                }

                ComboBox cl_Child = (ComboBox)mt_Fields.GetCellSpecific("cl_Child", 0);
                for (int i = 0; i < dt_Child.Columns.Count; i++)
                {
                    cl_Child.ValidValues.Add(dt_Child.Columns.Item(i).Name, dt_Child.Columns.Item(i).Name);
                }

                Form.Items.Item("et_Code").Visible = true;
                ((EditText)Form.Items.Item("et_Code").Specific).Value = parentCode;
                Form.Items.Item("1").Click();
                Form.Items.Item("et_Code").Visible = false;

                DBDataSource dt_Fields = Form.DataSources.DBDataSources.Item("@CVA_FILE_MAP_LINK");
                dt_Fields.Clear();

                mt_Fields.AddRow();

                Form.EnableMenu("1293", true);
                Form.EnableMenu("1292", true);
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
    }
}
