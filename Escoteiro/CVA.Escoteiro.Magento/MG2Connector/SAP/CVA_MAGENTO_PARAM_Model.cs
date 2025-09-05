namespace Escoteiro.Magento.Models
{
    public class CVA_MAGENTO_PARAM_Model
    {
        public string Code { get; set; }

        public string Name { get { return Code; } }

        public int U_BplID { get; set; }

        public string U_Deposito { get; set; }

        public string U_Sequencia { get; set; }

        public string U_Utilizacao { get; set; }

        public int? U_Series { get; set; }

        public string U_TaxExpsCode { get; set; }

        public string U_Metodo { get; set; }

        public string U_State { get; set; }

        public string U_Status { get; set; }

        public string U_CrTypeName { get; set; }

        public string U_CashAccount { get; set; }

        public string U_CreditCardOp { get; set; }
    }
}
