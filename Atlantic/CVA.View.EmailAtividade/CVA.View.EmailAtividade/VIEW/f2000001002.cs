using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAtividade.VIEW
{
    public class f2000001002 : BaseForm
    {
        private Form Form;

        #region Constructor
        public f2000001002()
        {
            FormCount++;
        }

        public f2000001002(SAPbouiCOM.ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f2000001002(SAPbouiCOM.BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f2000001002(SAPbouiCOM.ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override object Show()
        {
            Form = (Form)base.Show();
            try
            {
                Form.Freeze(true);
                DataTable dt_Items = Form.DataSources.DataTables.Item("dt_Items");
                dt_Items.Rows.Add(14);
                dt_Items.Columns.Item(0).Cells.Item(0).Value = "Nome da Empresa";
                dt_Items.Columns.Item(1).Cells.Item(0).Value = "{CompanyName}";
                dt_Items.Columns.Item(0).Cells.Item(1).Value = "Nome do Banco de Dados";
                dt_Items.Columns.Item(1).Cells.Item(1).Value = "{CompanyDB}";
                dt_Items.Columns.Item(0).Cells.Item(2).Value  = "Atividade";
                dt_Items.Columns.Item(1).Cells.Item(2).Value  = "{Action}";
                dt_Items.Columns.Item(0).Cells.Item(3).Value  = "Tipo de Atividade";
                dt_Items.Columns.Item(1).Cells.Item(3).Value  = "{CntctType}";
                dt_Items.Columns.Item(0).Cells.Item(4).Value  = "Assunto da Atividade";
                dt_Items.Columns.Item(1).Cells.Item(4).Value  = "{CntctSbjct}";
                dt_Items.Columns.Item(0).Cells.Item(5).Value  = "Usuário";
                dt_Items.Columns.Item(1).Cells.Item(5).Value  = "{User}";
                dt_Items.Columns.Item(0).Cells.Item(6).Value  = "Número da Atividade";
                dt_Items.Columns.Item(1).Cells.Item(6).Value  = "{ClgCode}";
                dt_Items.Columns.Item(0).Cells.Item(7).Value  = "Código do Parceiro de Negócio";
                dt_Items.Columns.Item(1).Cells.Item(7).Value  = "{CardCode}";
                dt_Items.Columns.Item(0).Cells.Item(8).Value  = "Nome do Parceiro de Negócio";
                dt_Items.Columns.Item(1).Cells.Item(8).Value  = "{CardName}";
                dt_Items.Columns.Item(0).Cells.Item(9).Value  = "Observações";
                dt_Items.Columns.Item(1).Cells.Item(9).Value  = "{Details}";
                dt_Items.Columns.Item(0).Cells.Item(10).Value = "Data Inicial da Atividade";
                dt_Items.Columns.Item(1).Cells.Item(10).Value = "{Recontact}";
                dt_Items.Columns.Item(0).Cells.Item(11).Value = "Data Final da Atividade";
                dt_Items.Columns.Item(1).Cells.Item(11).Value = "{EndDate}";
                dt_Items.Columns.Item(0).Cells.Item(12).Value = "Tipo do documento";
                dt_Items.Columns.Item(1).Cells.Item(12).Value = "{DocType}";
                dt_Items.Columns.Item(0).Cells.Item(13).Value = "Número do Documento Relacionado";
                dt_Items.Columns.Item(1).Cells.Item(13).Value = "{DocNum}";
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
