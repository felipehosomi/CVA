using CVA.AddOn.Common.Controllers;
using System;

namespace CVA.View.Hybel.MODEL
{
    public class BoletoModel
    {
        [ModelController(UIFieldName = "Boleto")]
        public int BoeNum { get; set; }
        [ModelController(UIFieldName = "Vencimento")]
        public DateTime Vencimento { get; set; }
        [ModelController(UIFieldName = "Nome")]
        public string Cliente { get; set; }
        //[ModelController(UIFieldName = "Linha Digitável")]
        //public string LinhaDigitavel { get; set; }
        [ModelController(UIFieldName = "Nosso Nr.")]
        public string NossoNumero { get; set; }
        [ModelController(UIFieldName = "Nr. NF")]
        public int NF { get; set; }
        [ModelController(UIFieldName = "Parcela")]
        public string Parcela { get; set; }
        [ModelController(UIFieldName = "E-mail")]
        public string Email { get; set; }
        [ModelController(UIFieldName = "Valor")]
        public double Valor { get; set; }
        [ModelController(UIFieldName = "Banco")]
        public string Banco { get; set; }
    }
}
