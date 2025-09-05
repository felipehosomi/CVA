using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CVA.Electra.ImportaFolha
{
    public class LinhaCSV
    {


        public LinhaCSV(string[] line)
        {
            try
            {
                /*Comentado para utilização na KESTAL*/

                //NumeroLote = Convert.ToInt32(line[0]);
                //ContadorLinhas = Convert.ToInt32(line[1]);

                /*Comentado para utilização na KESTAL*/


                ContaContabil = line[0];
                ValorDebito = Double.Parse(line[2].ToString().Replace(",", "."), CultureInfo.InvariantCulture);
                ValorCredito = Double.Parse(line[1].ToString().Replace(",", "."), CultureInfo.InvariantCulture);
                ContaContabilExc = line[3];
                CentroCusto = line[4];
                Observacao = line[5];
                Projeto = line[6];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*Comentado para utilização na KESTAL*/

        //public int NumeroLote;

        //public int ContadorLinhas;

        /*Comentado para utilização na KESTAL*/

        public string ContaContabil;

        public double ValorDebito;

        public double ValorCredito;

        public string ContaContabilExc;

        public string CentroCusto;

        public string Observacao;

        public string Projeto;


    }
}
