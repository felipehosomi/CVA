using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVA_Rep_Logging;
using System.IO;
using CVA_Rep_DAL;

namespace CVA_RepConfig.Forms.Conciliador
{
    public partial class Conciliador_DeParaConsultar : UserControl
    {
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;

        public Conciliador_DeParaConsultar()
        {
            InitializeComponent();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Conciliador_DeParaForm>();
        }

        private void Conciliador_DeParaConsultar_Load(object sender, EventArgs e)
        {
            try
            {
                var bll = new ConciliadorDAL();
                var lst = bll.DePara_GetAll();

                foreach (var bases in lst)
                {
                    dgvBases.Rows.Add(bases.ID, bases.BASE_DE, bases.FILIAL_DE, bases.NOME, bases.CNPJ_FILIAL_DE, bases.CNPJ_FILIAL_PARA, bases.FILIAL_PARA);
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
    }
}
