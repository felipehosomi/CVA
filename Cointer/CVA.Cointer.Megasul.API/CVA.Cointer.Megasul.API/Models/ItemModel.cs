using System.Collections.Generic;

namespace CVA.Cointer.Megasul.API.Models
{
    public class ItemModel : PagingModel
    {
       public List<Produto> produtos { get; set; }
    }

    public class Produto
    {
        [Newtonsoft.Json.JsonIgnore]
        public System.Int64 TotalRecords { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string codigo_barras { get; set; }
        public string abreviatura { get; set; }
        public List<Barra> barras { get; set; }
        public string descricao { get; set; }
        public string cest { get; set; }
        public string codigo_origem { get; set; }
        public string codigo_sap { get; set; }
        public Imposto imposto { get; set; }
        public Lote[] lotes { get; set; }
        public string ncm { get; set; }
        public string pesavel { get; set; }
        public double preco { get; set; }
        public string um { get; set; }
        public string tipo_lote { get; set; }
        public string composicao { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string TipoIcms { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public double AliquotaIcms { get; set; }
    }

    public class Imposto
    {
        public Icms icms { get; set; }
        public Ipi ipi { get; set; }
        public Pis_Cofins pis_cofins { get; set; }
    }

    public class Icms
    {
        public double aliquota { get; set; }
        public double perc_reducao { get; set; }
        public string tipo { get; set; }
    }

    public class Ipi
    {
        public double aliquota { get; set; }
        public int enquadramento { get; set; }
        public string selo { get; set; }
        public string st_entrada { get; set; }
        public string st_saida { get; set; }
    }

    public class Pis_Cofins
    {
        public int identificador_cofins { get; set; }
        public int identificador_pis { get; set; }
        public double perc_cofins_entrada { get; set; }
        public double perc_cofins_saida { get; set; }
        public double perc_pis_entrada { get; set; }
        public double perc_pis_saida { get; set; }
        public string st_entrada { get; set; }
        public string st_saida { get; set; }
    }

    public class Barra
    {
        [Newtonsoft.Json.JsonIgnore]
        public string ItemCode { get; set; }
        public string codigo { get; set; }
        public string codigo_fiscal { get; set; }
        public string identificador_lote { get; set; }
        public string unidade_medida { get; set; }
    }

    public class Lote
    {
        public string data_fabricacao { get; set; }
        public string data_validade { get; set; }
        public string identificador { get; set; }
        public string nro_serie { get; set; }
    }
}