using CVA.AddOn.Common.Controllers;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Model
{
    public class ApontamentoModel
    {
        public ApontamentoModel()
        {
            Itens = new List<ApontamentoGetInfoItensContratoModel>();
            BPs = new List<ApontamentoGetInfoBPContratoModel>();
        }

        [Display(Name = "Ordem de produção")]
        public string ContratoCode { get; set; }
        public SelectList ContratoList { get; set; }

        [Display(Name = "Filial")]
        public string BPLIdCode { get; set; }
        public SelectList BPLIdList { get; set; }

        [Display(Name = "Cliente")]
        public string ClienteCode { get; set; }
        public string ClienteName{ get; set; }

        [Display(Name = "Data")]
        public DateTime apontamentoData { get; set; }

        [Display(Name = "Qtde de Planejada")]
        public double QtyPlan { get; set; }

        [Display(Name = "Qtde de Refeições")]
        public double QtyRef { get; set; }

        [Display(Name = "Resto/Ingesta (KG)")]
        public double QtyResto { get; set; }

        [Display(Name = "Sobra Limpa (KG)")]
        public double QtySobra { get; set; }

        public double ComensaisDia { get; set; }
        
        [Display(Name = "Serviço")]
        public string ServicoCode { get; set; }
        public SelectList ServicoList { get; set; }


        //ModalInsumo
        public SelectList InsumoList { get; set; }
        [Display(Name = "Código Insumo")]
        public string InsumoCode { get; set; }
        public string InsumoName { get; set; }

        [Display(Name = "Qtde. Planejada")]
        public double InsumoQty { get; set; }


        public List<ApontamentoGetInfoItensContratoModel> Itens { get; set; }
        public List<ApontamentoGetInfoBPContratoModel> BPs { get; set; }
        public string UserCode { get; set; }
        public string OrderCode { get; set; }

        public string IdApontamento { get; set; }

        public string startDate { get; set; }
    }

    public class ApontamentoDataInicio
    {
        public string startDate { get; set; }
    }

    public class ApontamentoGetClienteModel
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }

        public string U_CVA_CNPJ { get; set; }
    }

    public class ApontamentoGetGrupoServicoModel
    {
        [ModelController(ColumnName = "U_CVA_ID_SERVICO")]
        public string U_CVA_ID_SERVICO { get; set; }

        [ModelController(ColumnName = "U_CVA_D_SERVICO")]
        public string U_CVA_D_SERVICO { get; set; }
    }

    public class ApontamentoGetInfoContratoModel
    {
        public ApontamentoGetInfoContratoModel()
        {
            Itens = new List<ApontamentoGetInfoItensContratoModel>();
            Services = new List<ApontamentoGetServicoModel>();
        }

        public string U_CVA_ID_CLIENTE { get; set; }
        public string U_CVA_DES_CLIENTE { get; set; }
        public double ComensaisDia { get; set; }
        public List<ApontamentoGetInfoItensContratoModel> Itens { get; set; }
        public List<ApontamentoGetServicoModel> Services { get; set; }

    }

    public class ApontamentoGetInfoItensContratoModel
    {
        public int DocNum { get; set; }
        public int DocEntry { get; set; }
        public int LineNum { get; set; }

        public string ProdItem { get; set; }
        public string ProdName { get; set; }

        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Display(Name = "Código Insumo")]
        public string ItemCode { get; set; }
        [Display(Name = "Descrição")]
        public string ItemName { get; set; }

        [Display(Name = "Qtde. Planejada")]
        public double QtyPlanejado { get; set; }
        [Display(Name = "Und")]
        public string Und { get; set; }

        [Display(Name = "Qtde. Efetiva")]
        public double QtyEfetiva { get; set; }

        [Display(Name = "Qtde. Utilizada")]
        public double QtyUtilizada { get; set; }

        [Display(Name = "Diferença")]
        public double QtyDif { get; set; }

        public double QtySaldoHidden { get; set; }
        [Display(Name = "Saldo")]        
        public double QtySaldo { get; set; }

        [Display(Name = "Motivo")]
        public string Motivo { get; set; }
        public string MotivoList { get; set; }

        [Display(Name = "Justificativa")]
        public string Justificativa { get; set; }

        public string ItemCodeChange { get; set; }
        public string ItemNameChange { get; set; }
        public bool Delete { get; set; }

        public double QtyPlanejadoOrdemProducao { get; set; }
    }

    public class ApontamentoGetInfoBPContratoModel
    {
        public string BPType { get; set; }

        [Display(Name = "Código do Cliente")]
        public string BPCardCode { get; set; }

        [Display(Name = "Nome do Cliente")]
        public string BPCardName { get; set; }

        [Display(Name = "Quantidade de Refeições")]
        public double BPQtyRefeicao { get; set; }
        public bool Remove { get; set; }

    }

    public class ApontamentoGetServicoModel
    {
        public string U_CVA_ID_SERVICO { get; set; }
        public string U_CVA_D_SERVICO { get; set; }
    }

    public class ApontamentoGetQtyModel
    {
        public double Qty { get; set; }
    }

    public class ApontamentoCheckDay
    { 
        public DateTime? LASTDAY { get; set; }
        public double YESTERDAY { get; set; }
    }

    public class ApontamentoGetNextValueTerceiros
    {
        public double Key { get; set; }
        public Int64 DocEntry { get; set; }
    }

    public class ApontamentoGetNextKeyModel
    {
        public double Key { get; set; }
    }

    public class ApontamentoStatusUser
    {
        public string U_CVA_BLOQUEIO_APTO { get; set; }
        public string U_CVA_VALDTMP_APTO { get; set; }
    }


    public class ApontamentoItemList
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
    }

    public class ApontamentoItemInfo : ApontamentoItemList
    {
        public string Und { get; set; }
        public double QtySaldo { get; set; }
        public string MotivoList { get; set; }
    }

    public class ApontamentoGetItemOnHand
    {
        public string ItemName { get; set; }
        public string Und { get; set; }
        public double? OnHand { get; set; }
        public double? AvgPrice { get; set; }
    }


}
