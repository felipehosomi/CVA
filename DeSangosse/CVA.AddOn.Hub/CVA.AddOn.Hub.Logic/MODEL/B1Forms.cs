using CVA.Hub.MODEL;
using System.Collections.Generic;

namespace CVA.Hub.HELPER
{
    public class B1Forms
    {
        public const string Item = "150";
        public const string CodigoImposto = "82105";
        public const string MenuCopiarPara = "-9876";

        public const string CotacaoVenda = "149";
        public const string PedidoVenda = "139";
        public const string NotaFiscalSaida = "133";
        public const string DevolucaoNotaFiscalSaida = "179";
        public const string Entrega = "140";
        public const string DevolucaoSaida = "180";
        public const string AdiantamentoCliente = "65300";
        public const string EntregaFutura = "60091";

        public const string SolicitacaoCompra = "14700000200";
        public const string OfertaCompra = "540000988";
        public const string PedidoCompra = "142";
        public const string NotaFiscalEntrada = "141";
        public const string DevolucaoNotaFiscalEntrada = "181";
        public const string RecebimentoMercadoria = "143";
        public const string DevolucaoMercadoria = "182";
        public const string RecebimentoFuturo = "60092";
        public const string AdiantamentoFornecedor = "65301";

        public static List<string> FormsSaida = new List<string>()
        {
            B1Forms.CotacaoVenda,
            B1Forms.PedidoVenda,
            B1Forms.NotaFiscalSaida,
            B1Forms.DevolucaoNotaFiscalSaida,
            B1Forms.Entrega,
            B1Forms.DevolucaoSaida,
            B1Forms.AdiantamentoCliente,
            B1Forms.EntregaFutura
        };

        public static List<string> FormsEntrada = new List<string>()
        {
            B1Forms.SolicitacaoCompra,
            B1Forms.OfertaCompra,
            B1Forms.PedidoCompra,
            B1Forms.NotaFiscalEntrada,
            B1Forms.DevolucaoNotaFiscalEntrada,
            B1Forms.RecebimentoMercadoria,
            B1Forms.DevolucaoMercadoria,
            B1Forms.RecebimentoFuturo,
            B1Forms.AdiantamentoFornecedor,
        };

        public static bool IsMarketingDocument(string form)
        {
            if (B1Forms.FormsSaida.Contains(form) || B1Forms.FormsEntrada.Contains(form))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static TipoDoc GetTipoDoc(string form)
        {
            if (B1Forms.FormsSaida.Contains(form))
            {
                return TipoDoc.Saida;
            }
            else
            {
                return TipoDoc.Entrada;
            }
        }
    }
}
