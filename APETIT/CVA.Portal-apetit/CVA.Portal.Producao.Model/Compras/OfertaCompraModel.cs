using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Model.Compras
{
    public class OfertaCompraModel
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string ReqDate { get; set; }
        public string Comments { get; set; }
        public string CardCode { get; set; }
        public string Status { get; set; }
        public int GroupNum { get; set; }
        public string ObsRevisao { get; set; }
        public string CompradorNome { get; set; }
        public string CompradorEmail { get; set; }
        public string FornecedorNome { get; set; }
        public string FornecedorEmail { get; set; }
        public List<OfertaCompraItemModel> Itens { get; set; }
        public string Filial { get; set; }
        public string TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CardName { get; set; }
    }

    public class OfertaCompraItemModel
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public double Quantity { get; set; }
        public double PQTReqQty { get; set; }
        public string ShipDate { get; set; } 
        public string Price { get; set; }
        public string ValorTotal { get; set; }
        public string UnidadeMedida { get; set; }
        public string StatusItem { get; set; }
        public string UomEntry { get; set; }
        public double InvQty { get; set; }
        public string unitMsr { get; set; }
        public double NumPerMsr { get; set; }
        public string FreeTxt { get; set; }
        public int LeadTime { get; set; }
        public SelectList CodigosUM { get; set; }
        public bool newRegister { get; set; }
    }

    public class OfertaCompraItemUMModel
    {
        public string Code { get; set; }
        public string Desc { get; set; }
    }

    public class OfertaCompraListModel
    {
        public int DocNum { get; set; }
        public string DocDate { get; set; }
        public string Status { get; set; }
        public string CardCode { get; set; }
        public string Filial { get; set; }
    }

    public enum TiposEmail
    {
        EnvioCotacao = 1,
        AprovacaoCotacao = 2,
        ReprovacaoCotacao = 3,
        RevisaoCotacao = 4,
        RecusaSolicitacao = 5
    }

    public class OfertaCompraPatchModel
    {
        public List<OfertaCompraDocumentLines> DocumentLines { get; set; }
    }

    public class OfertaCompraDocumentLines
    {
        public string LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ShipDate { get; set; }
        public string FreeText { get; set; }
        public string RequiredQuantity { get; set; }
        public string UoMEntry { get; set; }
        public string UoMCode { get; set; }
    }
}
