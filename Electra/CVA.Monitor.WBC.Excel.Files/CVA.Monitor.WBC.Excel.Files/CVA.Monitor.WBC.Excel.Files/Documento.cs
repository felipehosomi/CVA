using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Monitor.WBC.Excel.Files
{
    public class Documento
    {
        public string Movimentacao { get; set; }
        public string Contraparte_CNPJ { get; set; }
        public string Interveniente_Comissionado { get; set; }
        public string Parte_CNPJ { get; set; }
        public string Submercado { get; set; }
        public string Codigo_CCEE { get; set; }
        public string Codigo_WBC { get; set; }
        public string Observacao { get; set; }
        public string Ano { get; set; }
        public string Mes { get; set; }
        public string Valor_parcela_1 { get; set; }
        public DateTime Data_parcela_1 { get; set; }
        public DateTime? Data_emissao_fatura { get; set; }
        public string Form_AGIO { get; set; }
        public string Valor_TRU { get; set; }
        public string Perfil_CCEE_vendedor { get; set; }
        public string QuantAtualizada { get; set; }
        public string Situacao_faturamento_backoffice { get; set; }
        public string Chave { get; set; }
        public string CodigoDoItem { get; set; }
        public string CodigoDoVededor { get; set; }
        public string Indicator { get; set; }
        public Int64 LinhaPlanilha { get; set; }

        //Novos Campos
        public string Condicao_pagto { get; set; }
        public string Valor_parcela_2 { get; set; }
        public string Valor_parcela_3 { get; set; }
        public DateTime Data_emissao_prevista { get; set; }
        public string Numero_referencia_contrato { get; set; }
        public bool EhTresPagamentos { get; set; }

        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string CentroDeCusto { get; set; }
        public string ItemCode { get; set; }
        public string SlpCode { get; set; }
        public string BPLId { get; set; }
        public int UsoPrincipal { get; set; }
        public string Observacao_Pagamentos { get; set; }
        public string Situacao_Contrato { get; set; }
        public string Nome_Contrato { get; set; }
        public string ValorReajustado { get; set; }
        public string Portfolio_Vendedor { get; set; }
        public DateTime Suprimento_inicio { get; set; }
        public DateTime Suprimento_termino { get; set; }
        public string MsgErro { get; set; }
    }
}
