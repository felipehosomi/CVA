using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class ArquivoLinha
    {
        public string CODE { get; set; }
        public int LINHAGRID { get; set; }
        public int LINHA { get; set; }
        public string CNPJATLANTIC { get; set; }
        public string BASE { get; set; }
        public string EMPRESA { get; set; }
        public string PN { get; set; }
        public string ITEM { get; set; }
        public double QUANTIDADE { get; set; }
        public double VALOR { get; set; }
        public DateTime? DATAVENCIMENTO { get; set; }
        public DateTime? DATALANCAMENTO { get; set; }
        public string UTILIZACAO { get; set; }
        public string CONDICAOPAGAMENTO { get; set; }
        public string FORMAPAGAMENTO { get; set; }
        public string NUMERONOTA { get; set; }
        public string SERIENOTA { get; set; }
        public string MODELO { get; set; }
        public int SEQUENCIANF { get; set; }
        public string DIMENSAO01 { get; set; }
        public string DIMENSAO02 { get; set; }
        public string DIMENSAO03 { get; set; }
        public string DIMENSAO04 { get; set; }
        public string DIMENSAO05 { get; set; }
        public string PROJETO { get; set; }
        public string OBSERVACAO { get; set; }
        public int STATUSLINHA { get; set; }
        public string MENSAGEMSTATUS { get; set; }
        public DateTime? DATASTATUS { get; set; }
        public int? NUMEROPEDIDOSAP { get; set; }

        public string IMPOSTORETIDO1 { get; set; }
        public string IMPOSTORETIDO2 { get; set; }
        public string ANEXO1 { get; set; }
        public string ANEXO2 { get; set; }
        public string ANEXO3 { get; set; }
        public string ANEXO4 { get; set; }
        public string ANEXO5 { get; set; }

        public string u_cva_integracao { get; set; }
        public string u_sx_aliqIPI { get; set; }
        public string u_sx_aplicacao { get; set; }
        public string u_sx_aliqST { get; set; }


        public string STATUSDESC
        {
            get
            {
                switch (STATUSLINHA)
                {
                    case 1:
                        return "Pedido gerado";
                    case 2:
                        return "Erro ao gerar pedido";
                    case 3:
                        return "NF gerada";
                    case 4:
                        return "Erro ao gerar NF";
                }
                return String.Empty;
            }
        }
    }
}
