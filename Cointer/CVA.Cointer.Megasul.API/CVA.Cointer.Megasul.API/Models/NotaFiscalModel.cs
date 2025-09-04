using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVA.Cointer.Megasul.API.Models
{
    public class NotaFiscalModel
    {
        public string cnpj_filial { get; set; }
        public Identificador identificador { get; set; }
        public Ecf ecf { get; set; }
        public NfProduto[] produtos { get; set; }
        public double valor_total { get; set; }
        public double valor_desconto { get; set; }
        public double valor_acrescimo { get; set; }
        public string vendedor { get; set; }
        public string cliente { get; set; }
        public int pedido_cancelado { get; set; }
        public Pagamento[] pagamentos { get; set; }
    }

    public class Identificador
    {
        public int coo { get; set; }
        public int ccf { get; set; }
        public string data_hora { get; set; }
        public int ef { get; set; }
        public int si { get; set; }
        public int tr { get; set; }
    }

    public class Ecf
    {
        public int numero { get; set; }
        public string serie { get; set; }
    }

    public class NfProduto
    {
        public int sequencial { get; set; }
        public string codigo_sap { get; set; }
        public string un { get; set; }
        public bool cancelado { get; set; }
        public double quantidade { get; set; }
        public double preco { get; set; }
        public double desconto { get; set; }
        public double acrescimo { get; set; }
        public NfImposto imposto { get; set; }
        public NfLote lotes { get; set; }
    }

    public class NfImposto
    {
        public double _base { get; set; }
        public int aliquota { get; set; }
        public double valor { get; set; }
    }

    public class Pagamento
    {
        public string codigo_sap { get; set; }
        public double valor { get; set; }
        public bool troco { get; set; }
        public Cartao[] cartoes { get; set; }
        public Cheque[] cheques { get; set; }
        public Titulo[] titulos { get; set; }
        public Voucher[] vouchers { get; set; }
    }

    public class Cartao
    {
        public string nsu { get; set; }
        public string autorizacao { get; set; }
        public string operacao { get; set; }
        public int qtde_parcela { get; set; }
        public Parcela[] parcelas { get; set; }
    }

    public class NfLote
    {
        public string data_fabricacao { get; set; }
        public string data_validade { get; set; }
        public string nro_serie { get; set; }
        public string identificador { get; set; }
    }

    public class Parcela
    {
        public int parcela { get; set; }
        public double valor { get; set; }
        public double valor_repasse { get; set; }
        public string data_repasse { get; set; }
    }

    public class Cheque
    {
        public int valor { get; set; }
        public string banco { get; set; }
        public string agencia { get; set; }
        public string conta { get; set; }
        public string numero { get; set; }
        public string bom_para { get; set; }
    }

    public class Titulo
    {
        public int valor { get; set; }
        public int parcela { get; set; }
        public string codigo { get; set; }
        public string data_vencimento { get; set; }
    }

    public class Voucher
    {
        public string codigo { get; set; }
        public double valor { get; set; }
    }
}