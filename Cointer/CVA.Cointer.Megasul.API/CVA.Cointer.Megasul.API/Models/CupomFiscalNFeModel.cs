namespace CVA.Cointer.Megasul.API.Models
{
    public class CupomFiscalNFeModel
    {
        public string cnpj_filial { get; set; }
        public Identificador identificador { get; set; }
        public Nf nf { get; set; }
    }

    public class Nf
    {
        public int numero { get; set; }
        public int serie { get; set; }
        public string xml { get; set; }
        public string chave_acesso { get; set; }
        public int status { get; set; }

        public string cliente { get; set; }
        public string vendedor { get; set; }
    }
}