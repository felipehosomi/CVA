using AUXILIAR;
using DAO;
using MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public Etiqueta Get_By_Order(string order)
        {
            var result = _impressorDAO.Get_By_Order(order);
            return LoadModel(result);
        }

        public Etiqueta LoadModel(DataTable result)
        {
            var model = new Etiqueta();
            model.Quantidade = result.Rows[0]["Quantidade"].ToString();

            if (!String.IsNullOrEmpty(result.Rows[0]["Status"].ToString()))
                model.Status = result.Rows[0]["Status"].ToString();
            else
                model.Status = "ORDEM NÃO IMPRESSA";

            model.NomeProduto = result.Rows[0]["NomeProduto"].ToString();

            return model;
        }

    public bool Print_Padrao(Etiqueta etiqueta)
    {
        try
        {
            string tipoEtiqueta = (@"C:\CVA Consultoria\Impressor de Etiqueta\Modelos\Padrao.rpt");
            _printer.Print(etiqueta, tipoEtiqueta);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool Print_Lote(Etiqueta etiqueta)
    {
        try
        {
            string tipoEtiqueta = (@"C:\CVA Consultoria\Impressor de Etiqueta\Modelos\Lote.rpt");
            _printer.Print(etiqueta, tipoEtiqueta);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Print_PadraoIngles(Etiqueta etiqueta)
    {
        try
        {
            string tipoEtiqueta = (@"C:\CVA Consultoria\Impressor de Etiqueta\Modelos\PadraoIngles.rpt");
            _printer.Print(etiqueta, tipoEtiqueta);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Print_Pacote(Etiqueta etiqueta)
    {
        try
        {
            string tipoEtiqueta = (@"C:\CVA Consultoria\Impressor de Etiqueta\Modelos\Pacote.rpt");
            _printer.Print(etiqueta, tipoEtiqueta);

            return true;
        }
        catch (Exception)
        {
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
