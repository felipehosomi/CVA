using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CVA_Rep_DAL;
using CVA_Rep_Logging;

namespace CVA_RepConfig.Forms.Base
{
    public partial class ConsultarBase_UserControl : UserControl
    {
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;

        public ConsultarBase_UserControl()
        {
            InitializeComponent();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Base_Form>();
        }

        private void ConsultarBase_UserControl_Load(object sender, EventArgs e)
        {
            try
            {
                var bll = new CVA_BAS_Repository();
                var lst = bll.GetAll().ToList();

                foreach (var bases in lst)
                {
                    var db_typ = string.Empty;
                    switch (bases.DB_TYP)
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

                    dgvBases.Rows.Add(bases.ID, bases.INS, bases.UPD, bases.CVA_STU.STU, bases.SRVR, bases.UNAME,
                        bases.PAS, bases.COMP, bases.USE_TRU.Equals(0) ? "Não" : "Sim", bases.DB_SRVR, bases.DB_UNAME, bases.DB_PAS, db_typ);
                }
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
            if (dgvBases.Columns[e.ColumnIndex].Name == "PAS" && e.Value != null)
            {
                dgvBases.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new String('*', e.Value.ToString().Length);
            }

            if (dgvBases.Columns[e.ColumnIndex].Name == "DB_PAS" && e.Value != null)
            {
                dgvBases.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new String('*', e.Value.ToString().Length);
            }
        }
    }
}