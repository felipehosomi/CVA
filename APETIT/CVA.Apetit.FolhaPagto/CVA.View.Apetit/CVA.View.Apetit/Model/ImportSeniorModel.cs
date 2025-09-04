using CVA.View.Apetit.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Apetit.Model
{
    public class ImportSeniorModel
    {
        public int NumeroLinha { get; set; }

        public string NumLote { get; set; }
        public DateTime DtLancto { get; set; }
        public string CentroCusto { get; set; }
        public string Filial { get; set; }
        public string CentroCusto_2 { get; set; }
        public string Filial_2 { get; set; }
        public string CodRedDebito { get; set; }
        public string CodRedCredito { get; set; }
        public string Historico { get; set; }
        public double Valor { get; set; }

        public ImportSeniorModel(string fileLineContent, int index)
        {
            var curLen = 0;
            NumLote = fileLineContent.SubStrPositioned(ref curLen, 6).Trim();

            var dtLancto = fileLineContent.SubStrPositioned(ref curLen, 8);
            DtLancto = DateTime.ParseExact(dtLancto, "yyyyMMdd", CultureInfo.InvariantCulture);

            CentroCusto = fileLineContent.SubStrPositioned(ref curLen, 4).Trim();
            Filial = fileLineContent.SubStrPositioned(ref curLen, 4).Trim();
            CentroCusto_2 = fileLineContent.SubStrPositioned(ref curLen, 4).Trim();
            Filial_2 = fileLineContent.SubStrPositioned(ref curLen, 4).Trim();

            var useless = fileLineContent.SubStrPositioned(ref curLen, 8);

            CodRedDebito = fileLineContent.SubStrPositioned(ref curLen, 20).Trim();
            CodRedCredito = fileLineContent.SubStrPositioned(ref curLen, 20).Trim();
            Historico = fileLineContent.SubStrPositioned(ref curLen, 204).Trim();
            var vlr = fileLineContent.SubStrPositioned(ref curLen, 16).Trim();
            Valor = double.Parse(vlr.Replace(".", ","));
            NumeroLinha = index + 1;
        }

        public static List<ImportSeniorModel> ToList(string fileContent)
        {
            var retList = new List<ImportSeniorModel>();
            var lines = fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Count(); i++)
            {
                var item = lines[i];
                retList.Add(new ImportSeniorModel(item, i));
            }

            return retList;
        }
    }
}
