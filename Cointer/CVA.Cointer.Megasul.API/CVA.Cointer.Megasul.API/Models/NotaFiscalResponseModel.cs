namespace CVA.Cointer.Megasul.API.Models
{
    public class NotaFiscalResponseModel
    {
        public string cnpj_filial { get; set; }
        public IdentificadorResponse identificador { get; set; }
        public bool resultado { get; set; }
        public string mensagem_erro { get; set; }
    }

    public class IdentificadorResponse
    { 
        public int ef { get; set; }
        public int si { get; set; }
        public int tr { get; set; }
    }
}