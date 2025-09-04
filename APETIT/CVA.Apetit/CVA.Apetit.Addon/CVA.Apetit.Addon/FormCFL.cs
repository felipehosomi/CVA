using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.Apetit.Addon
{
    public partial class FormCFL : Form
    {
        private static bool carregado;

        public FormCFL()
        {
            InitializeComponent();
        }

        //==================================================================================================================================//
        private void FormCFL_Load(object sender, EventArgs e)
        //==================================================================================================================================//
        {
            MontaTela();
            CarregarDadosTela();
        }

        //==================================================================================================================================//
        private void MontaTela()
        //==================================================================================================================================//
        {
            try
            {
                this.Text = Class.CFL.titulo;

                grid.Columns.Clear();

                // var col =                           Título               ,  Campo          , width, readOnly, visible, alinhamento,           format, fonte.size, Sortable
                var col01 = Class.Geral.retColTextBox("#"                   , "Linha"         ,    50,     true,   false,   "direita",               "",          0,     true);
                var col02 = Class.Geral.retColCheckBox("Check"              , "Check"         ,    38,    false,    true,    "centro",               "",          0,     true);
                var col03 = Class.Geral.retColTextBox(Class.CFL.tituloChave , Class.CFL.chave ,   100,     true,    true,   "direita",               "",          0,     true);
                var col04 = Class.Geral.retColTextBox(Class.CFL.tituloCampo1, Class.CFL.campo1,   250,     true,    true,  "esquerda",               "",          0,     true);
                var col05 = Class.Geral.retColTextBox(Class.CFL.tituloCampo2, Class.CFL.campo2,   250,     true,    true,  "esquerda",               "",          0,     true);

                grid.Columns.AddRange(new DataGridViewColumn[] { col01, col02, col03, col04, col05 });
                grid.AllowUserToAddRows = false;
                grid.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
                grid.RowHeadersVisible = false;
                grid.EditMode = DataGridViewEditMode.EditOnEnter;
                grid.ReadOnly = false;
                //this.grid.CellValidating += new DataGridViewCellValidatingEventHandler(grid_CellValidating);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //==================================================================================================================================//
        private void CarregarDadosTela()
        //==================================================================================================================================//
        {
            string sql;
            int num = 0, numHoras = 72;

            try
            {
                carregado = false;

                if (Class.CFL.tabela != "@CVA_CAR_LOTE")
                {
                    if (Class.CFL.campo2 == "")
                        sql = string.Format(@"
SELECT ""{0}"" AS ""BPLId"", ""{1}"" AS ""BPLName"", '' AS ""BPLFrName"" FROM ""{2}"" ORDER BY ""{0}"" ",
    Class.CFL.chave, Class.CFL.campo1, Class.CFL.tabela);
                    else
                        sql = string.Format(@"
SELECT ""{0}"" AS ""BPLId"", ""{1}"" AS ""BPLName"", ""{2}"" AS ""BPLFrName"" FROM ""{3}"" ORDER BY ""{0}"" ",
    Class.CFL.chave, Class.CFL.campo1, Class.CFL.campo2, Class.CFL.tabela);
                }
                else
                {
                    sql = string.Format(@"
SELECT 
	T0.""U_Lote"" AS ""BPLId""
	,TO_VARCHAR(T0.""U_CreateDate"", 'DD/MM/YYYY HH:MM:SS') || ' - ' || T0.""U_ID_Filial"" || ' - ' || T1.""BPLName"" AS ""BPLName""
	,'De: ' || TO_VARCHAR(T0.""U_DataDe"", 'DD/MM/YYYY') || ' Até ' || TO_VARCHAR(T0.""U_DataAte"", 'DD/MM/YYYY') AS ""BPLFrName"" 
FROM ""@CVA_CAR_LOTE"" T0
	INNER JOIN OBPL T1 ON T1.""BPLId"" = T0.""U_ID_Filial""
WHERE SECONDS_BETWEEN(""U_CreateDate"", CURRENT_TIMESTAMP) / 3600 < {0} 
    AND IFNULL(T0.""U_Cancelado"", '') <> 'Y'
", numHoras);
                }

                System.Data.DataTable dt = Class.Conexao.ExecuteSqlDataTable(sql);

                foreach (DataRow linha in dt.Rows)
                {
                    grid.Rows.Add();
                    DataGridViewRow row = grid.Rows[num];

                    row.Cells["Linha"].Value = (num + 1);
                    row.Cells["Check"].Value = ("S" != "S");
                    row.Cells[Class.CFL.chave].Value = Convert.ToInt32(linha["BPLId"].ToString().Trim());
                    row.Cells[Class.CFL.campo1].Value = linha["BPLName"].ToString().Trim();
                    row.Cells[Class.CFL.campo2].Value = linha["BPLFrName"].ToString().Trim();
                    num++;
                    carregado = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //==================================================================================================================================//
        {
            if (carregado)
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        //==================================================================================================================================//
        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        //==================================================================================================================================//
        {
            int linha;

            try
            {
                linha = e.RowIndex;
                
                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    if ((grid.Rows[i].Cells["Check"].Value.ToString() == "True") && (i != linha))
                        grid.Rows[i].Cells["Check"].Value = "False";
                }
            }
            catch { }
        }

        //==================================================================================================================================//
        private void btnSelecionar_Click(object sender, EventArgs e)
        //==================================================================================================================================//
        {
            foreach (DataGridViewRow linha in grid.Rows)
            {
                if (linha.Cells["Check"].Value.ToString() == "True")
                {
                    if (Class.CFL.tabela != "@CVA_CAR_LOTE")
                        Class.CFL.retorno = linha.Cells[Class.CFL.chave].Value.ToString();
                    else
                        Class.CFL.retorno = linha.Cells[Class.CFL.chave].Value.ToString() + " - " + linha.Cells[Class.CFL.campo1].Value.ToString();
                }
            }
            this.Close();
        }

        //==================================================================================================================================//
        private void txtProcurar_TextChanged(object sender, EventArgs e)
        //==================================================================================================================================//
        {
            int rowIndex = -1;
            string coluna = "";

            grid.ClearSelection();
            try { coluna = grid.SortedColumn.Name; } catch { }


            if ( (!string.IsNullOrEmpty(txtProcurar.Text)) && (coluna != "") )
            {
                foreach (DataGridViewRow linha in grid.Rows)
                {
                    if (linha.Cells[coluna].Value.ToString().ToUpper().StartsWith(txtProcurar.Text.ToUpper()))
                    {
                        rowIndex = linha.Index;
                        break;
                    }
                }

                if (rowIndex > -1)
                {
                    grid.Rows[rowIndex].Selected = true;
                    grid.FirstDisplayedScrollingRowIndex = rowIndex;
                }
            }
        }

        //==================================================================================================================================//
        private void btnCancelar_Click(object sender, EventArgs e)
        //==================================================================================================================================//
        {
            this.Close();
        }
    }
}
