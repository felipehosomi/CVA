using CVA.AddOn.Common.Attributes;

namespace CVA.View.DIME.MODEL
{
    public class DimeEmpresaModel
    {
        [FileWriter(Position = 1, Size = 2)]
        public string Tipo { get; set; } = "21";

        [FileWriter(Position = 3, Size = 2)]
        public string Quadro { get; set; } = "00";

        [FileWriter(Position = 5, Size = 9)]
        public string Inscricao { get; set; }

        [FileWriter(Position = 14, Size = 50)]
        public string Nome { get; set; }

        [FileWriter(Position = 54, Size = 6)]
        public string Periodo { get; set; }

        [FileWriter(Position = 70, Size = 1)]
        public int TipoDeclaracao { get; set; }

        [FileWriter(Position = 71, Size = 1)]
        public int RegimeApuracao { get; set; } = 2;

        [FileWriter(Position = 72, Size = 1)]
        public int PorteEmpresa { get; set; } = 1;

        [FileWriter(Position = 73, Size = 1)]
        public int TipoApuracao { get; set; }

        [FileWriter(Position = 74, Size = 1)]
        public int ApuracaoCentralizada { get; set; } = 1;

        [FileWriter(Position = 75, Size = 1)]
        public int TransCredito { get; set; }

        [FileWriter(Position = 76, Size = 1)]
        public int CreditoPresumido { get; set; } = 1;

        [FileWriter(Position = 77, Size = 1)]
        public int CreditoIncentivosFiscais { get; set; } = 1;

        [FileWriter(Position = 78, Size = 1)]
        public int TipoMovimento { get; set; }

        [FileWriter(Position = 79, Size = 1)]
        public int SubstitutoTributario { get; set; }

        [FileWriter(Position = 80, Size = 1)]
        public int EscritaContabil { get; set; }

        [FileWriter(Position = 81, Size = 5)]
        public int QtdeEmpregados { get; set; }
    }
}
