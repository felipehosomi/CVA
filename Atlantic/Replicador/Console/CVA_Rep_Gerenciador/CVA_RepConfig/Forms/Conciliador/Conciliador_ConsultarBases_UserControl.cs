using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CVA_Rep_Logging;
using CVA_Rep_DAL;

namespace CVA_RepConfig.Forms.Conciliador
{
    public partial class Conciliador_ConsultarBases_UserControl : UserControl
    {
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;

        public Conciliador_ConsultarBases_UserControl()
        {
            InitializeComponent();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Conciliador_BasesForm>();
        }

        private void Conciliador_ConsultarBases_UserControl_Load(object sender, EventArgs e)
        {
            try
            {
                var bll = new ConciliadorDAL();
                var lst = bll.Bases_GetAll();

                foreach (var bases in lst)
                {
                    var tipo = string.Empty;
                    var db_typ = string.Empty;
                    switch (bases.DB_TYPE)
                    {
                        case 1:
                            db_typ = "MSSQL";
                            break;
                        case 2:
                            db_typ = "DB2";
                            break;
                        case 3:
                            db_typ = "SYBASE";
                            break;
                        case 4:
                            db_typ = "MSSQL 2005";
                            break;
                        case 5:
                            db_typ = "MAXDB";
                            break;
                        case 6:
                            db_typ = "MSSQL 2008";
                            break;
                        case 7:
                            db_typ = "MSSQL 2012";
                            break;
                        case 8:
                            db_typ = "MSSQL 2014";
                            break;
                        case 9:
                            db_typ = "HANA DB";
                            break;
                    }

                    switch (bases.TIPO)
                    {
                        case 1:
                            tipo = "Base consolidadora";
                            break;
                        case 2:
                            tipo = "Base origem";
                            break;
                    }

                    dgvBases.Rows.Add(bases.ID, bases.LICENSE_SERVER, bases.USERNAME, bases.PASSWD, bases.BASE, bases.USE_TRUSTED.Equals(0) ? "Não" : "Sim", bases.DB_SERVER, bases.DB_USERNAME, bases.DB_PASSWD, db_typ, tipo);
                }

                bll.CloseConnection();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
                MessageBox.Show(
                    "Ocorreu um erro interno." + Environment.NewLine + "Por favor, verifique o arquivo de log.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvBases_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvBases.CurrentRow.Tag != null)
                e.Control.Text = dgvBases.CurrentRow.Tag.ToString();
        }

        private void dgvBases_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvBases.Columns[e.ColumnIndex].Name == "PASSWD" && e.Value != null)
            {
                dgvBases.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new String('*', e.Value.ToString().Length);
            }

            if (dgvBases.Columns[e.ColumnIndex].Name == "DB_PASSWD" && e.Value != null)
            {
                dgvBases.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new String('*', e.Value.ToString().Length);
            }
        }
    }
}
