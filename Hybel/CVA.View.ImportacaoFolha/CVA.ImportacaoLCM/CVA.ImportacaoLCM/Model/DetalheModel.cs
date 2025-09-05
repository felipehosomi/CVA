using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.Model
{
    class DetalheModel
    {
        public string NomedoArquivo { get; set; }
        public string NomeDaEmpresa { get; set; }
        public string CodigoDaEmpresa { get; set; }
        public string CNPJdaEmpresa { get; set; }
        public string CodigoDaFilial { get; set; }
        public string NSequencial { get; set; }
        public string NSeqnoDetalhe { get; set; }
        public string DatadoLancto { get; set; }
        //(Código) 
        public string ContaDebito { get; set; }
        //(Código) 
        public string ContaCredito { get; set; }
        //(Classif.)
        public string ClassifDebito { get; set; }
        //(Classif.)
        public string ClassifCredito { get; set; }
        public string ValorDoLancto { get; set; }
        public string CodigoDoHistorico { get; set; }
        public string Historico { get; set; }
        public string NomedoUsuario { get; set; }
        public string CodigoDoSeparador { get; set; }
        public string NomeDoSeparador { get; set; }
        public string DescricaoLancto { get; set; }
        public string CentroCustoDebit { get; set; }
        public string CentroCustoCredit { get; set; }

    }
}
