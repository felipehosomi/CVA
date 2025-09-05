using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.Model
{
    public class CorreioModelRemetente
    {
        public string cartao_postagem { get; set; }
        public string numero_contrato { get; set; }
        public string codigo_administrativo { get; set; }
        public string nome_remetente { get; set; }
        public string logradouro_remetente { get; set; }
        public string numero_remetente { get; set; }
        public string complemento_remetente { get; set; }
        public string bairro_remetente { get; set; }
        public string cep_remetente { get; set; }
        public string cidade_remetente { get; set; }
        public string uf_remetente { get; set; }
        public string email_remetente { get; set; }
        public string fax_remetente { get; set; }
        public string URL { get; set; }
        public string User { get; set; }
        public string PassWord { get; set; }

        public int numero_diretoria { get; set; }
        public int Telefone_remetente { get; set; }    
        public int celular_remetente { get; set; }
        public int id_servico { get; set; }
    }

    public class CorreioModelObjeto
    {

        public string Cod_Rastreio { get; set; }
        public string Cod_Servico { get; set; }
        public string Peso_Tarifado { get; set; }

        public string Destinatario { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string NumeroDestinatario { get; set; }
        public string E_mail { get; set; }
        public string fax { get; set; }

        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public int NF { get; set; }
        public string SerieNF { get; set; }
        public decimal VlrNF { get; set; }

        public string TipoObjeto { get; set; }
        public string Altura { get; set; }
        public string Largura { get; set; }
        public string Comprimento { get; set; }
    }

}
