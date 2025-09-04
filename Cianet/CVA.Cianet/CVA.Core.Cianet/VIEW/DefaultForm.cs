using CVA.AddOn.Common.Forms;
using CVA.Core.Cianet.BLL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Cianet.VIEW
{
    public class DefaultForm : BaseForm
    {
        internal Form form;
        internal RegraFreteBLL _bll;

        // Media Simples //

        #region Calcula_Frete
        internal double Calcula_Frete(Matrix matriz, string cliente, out string transportadora)
        {
            transportadora = "";

            var lstLinhas = new List<CalculoFreteModel>();

            for (int i = 1; i < matriz.VisualRowCount; i++)
            {
                var coluna1 = ((EditText)matriz.GetCellSpecific("1", i)).Value.ToString();
                var coluna2 = ((EditText)matriz.GetCellSpecific("11", i)).Value.ToString();
                var coluna3 = ((EditText)matriz.GetCellSpecific("21", i)).Value.ToString();

                var produto = coluna1;
                var quantidade = coluna2;
                var valorProduto = Convert.ToDouble(coluna3.Replace(".", "").Substring(2, coluna3.Length - 3));
                double porcentagem;

                var valorLinha = _bll.CalculaFrete(cliente, produto, quantidade, valorProduto, out transportadora, out porcentagem);
                lstLinhas.Add(new CalculoFreteModel(produto, quantidade, valorProduto, transportadora, valorLinha, porcentagem));
            }

            var mxPercent = lstLinhas.Max(x => x.Porcentagem);
            transportadora = lstLinhas.FirstOrDefault(x => x.Porcentagem == mxPercent).Transportadora;
            var valorFrete = lstLinhas.Sum(x => x.ValorProduto) * (mxPercent / 100);

            return valorFrete;
        }
        #endregion

        /// Media Ponderada //
        //#region Claculo do Frete
        //private decimal Calcula_Frete(Matrix matriz, string cliente)
        //{
        //    var valorFrete = 0.0;
        //    var aux = 0.0;
        //    var aux1 = 0.0;
        //    for (int i = 1; i < matriz.VisualRowCount; i++)
        //    {

        //        var coluna1 = ((EditText)matriz.GetCellSpecific("1", i)).Value.ToString();
        //        var coluna2 = ((EditText)matriz.GetCellSpecific("11", i)).Value.ToString();
        //        var coluna3 = ((EditText)matriz.GetCellSpecific("21", i)).Value.ToString();


        //        var produto = coluna1;
        //        var quantidade = coluna2;
        //        var valorProduto = coluna3;


        //        valorFrete = _bll.CalculaFrete(cliente, produto, quantidade, valorProduto);

        //        var aux2 = valorProduto.Replace(".", "");

        //        valorProduto = aux2.Substring(2, valorProduto.Length - 5);


        //        aux += valorFrete * Convert.ToDouble(valorProduto);
        //        aux1 += Convert.ToDouble(valorProduto);
        //    }

        //    var MediaP = Math.Round(Convert.ToDecimal((aux / aux1)), 2);
        //    return MediaP;
        //}

        //#endregion
    }

    class CalculoFreteModel
    {
        public string Produto { get; set; }
        public string Quantidade { get; set; }
        public double ValorProduto { get; set; }
        public string Transportadora { get; set; }
        public double ValorLinha { get; set; }
        public double Porcentagem { get; set; }

        public CalculoFreteModel(string produto, string quantidade, double valorProduto, string transportadora, double valorLinha, double porcentagem)
        {
            Produto = produto;
            Quantidade = quantidade;
            ValorProduto = valorProduto;
            Transportadora = transportadora;
            ValorLinha = valorLinha;
            Porcentagem = porcentagem;
        }
    }
}
