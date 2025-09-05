using AUXILIAR;
using DAO;
using MODEL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLL
{
    public class ImpressorBLL
    {
        #region Atributos
        public ImpressorDAO _impressorDAO { get; set; }
        public Printer _printer { get; set; }
        #endregion

        #region Construtor
        public ImpressorBLL()
        {
            this._impressorDAO = new ImpressorDAO();
            this._printer = new Printer();
        }
        #endregion


        public EtiquetaModel Get_By_Order(string order)
        {
            var result = _impressorDAO.Get_By_Order(order);
            return LoadModel(result);
        }

        public EtiquetaModel LoadModel(DataTable result)
        {
            var model = new EtiquetaModel();
            model.Quantidade = Convert.ToInt32(result.Rows[0]["Quantidade"].ToString());

            if (!String.IsNullOrEmpty(result.Rows[0]["Status"].ToString()))
                model.Status = result.Rows[0]["Status"].ToString();
            else
                model.Status = "ETIQUETA NÃO IMPRESSA";

            model.NomeProduto = result.Rows[0]["NomeProduto"].ToString();

            return model;
        }

        public bool Print_Padrao(EtiquetaModel etiquetaModel, string impressora, int? qtdeImpressao)
        {
            try
            {
                List<EtiquetaModel> list = new List<EtiquetaModel>();
                list.Add(etiquetaModel);
                string tipoEtiqueta = (Path.Combine(Application.StartupPath, ConfigurationManager.AppSettings["Layout"]));
                //_printer.Print(list, impressora, tipoEtiqueta, qtdeImpressao);
                OrdemBLL.GenerateFileToPrintPRN(list, tipoEtiqueta, impressora, qtdeImpressao);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool Print_Padrao(int ordem, string impressora, int? qtdeImpressao)
        {
            try
            {
                List<EtiquetaModel> listToPrint = new OrdemBLL().GetEtiqueta(ordem);
                string tipoEtiqueta = (Path.Combine(Application.StartupPath, ConfigurationManager.AppSettings["Layout"]));
                //_printer.Print(listToPrint, impressora, tipoEtiqueta, qtdeImpressao);
                OrdemBLL.GenerateFileToPrintPRN(listToPrint, tipoEtiqueta, impressora, qtdeImpressao);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void UpdateStatus(string order)
        {
            _impressorDAO.UpdateStatus(order);
        }

        public List<string> GetPrinters()
        {
            var modelList = new List<string>();

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                modelList.Add(printer);
            }
            return modelList;
        }
    }
}
