using CVA.AddOn.Common.Controllers;
using System;

namespace CVA.View.DIME.MODEL
{
    public class DimeConfigModel
    {
        [ModelController]
        public string Code { get; set; }

        [ModelController(ColumnName = "U_Filial")]
        public int Filial { get; set; }

        [ModelController(ColumnName = "U_Periodo")]
        public string Periodo { get; set; }

        [ModelController(ColumnName = "U_DtDe")]
        public DateTime DataDe { get; set; }

        [ModelController(ColumnName = "U_DtAte")]
        public DateTime DataAte { get; set; }

        [ModelController(ColumnName = "U_Dir")]
        public string Diretorio { get; set; }

        [ModelController(ColumnName = "U_Declaracao")]
        public int TipoDeclaracao { get; set; }

        [ModelController(ColumnName = "U_Apuracao")]
        public int TipoApuracao { get; set; }

        [ModelController(ColumnName = "U_Trans_Cred")]
        public int TransCredito { get; set; }

        [ModelController(ColumnName = "U_Movimento")]
        public int TipoMovimento { get; set; }

        [ModelController(ColumnName = "U_ST")]
        public int SubstitutoTributario { get; set; }

        [ModelController(ColumnName = "U_Escrita")]
        public int EscritaContabil { get; set; }

        [ModelController(ColumnName = "U_Empregados")]
        public int QtdeEmpregados { get; set; }


        [ModelController(ColumnName = "U_Deb_Ativo")]
        public double DebitoAtivoPermanente { get; set; }

        [ModelController(ColumnName = "U_CIAP")]
        public double IcmsCiap { get; set; }

        [ModelController(ColumnName = "U_Dif_Uso_Cons")]
        public double DiferencialAliquotaUsoConsumo { get; set; }

        [ModelController(ColumnName = "U_Cred_Ativo")]
        public double CreditoAtivoPermanente { get; set; }

        [ModelController(ColumnName = "U_Cred_ST")]
        public double CreditoIcmsST { get; set; }

        [ModelController(ColumnName = "U_Cred_Outros")]
        public double CreditoOutros { get; set; }

        [ModelController(ColumnName = "U_Info_ED")]
        public int InfoEstoqueDespesa { get; set; }

        [ModelController(ColumnName = "U_Demo_Cont")]
        public int DemonstrativosContabeis { get; set; }
    }
}
