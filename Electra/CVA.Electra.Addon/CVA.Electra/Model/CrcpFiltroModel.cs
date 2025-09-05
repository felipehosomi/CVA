using System;

namespace CVA.View.CRCP.Model
{
    public class CrcpFiltroModel
    {
        public string TipoRelatorio { get; set; }
        public string TipoData { get; set; }
        public DateTime DataDe { get; set; }
        public DateTime DataAte { get; set; }
        public string CardCode { get; set; }
        public string GrupoPN { get; set; }
        public string NrRefPN { get; set; }
        public string StatusCobranca { get; set; }
        public string Observacoes { get; set; }
    }
}
