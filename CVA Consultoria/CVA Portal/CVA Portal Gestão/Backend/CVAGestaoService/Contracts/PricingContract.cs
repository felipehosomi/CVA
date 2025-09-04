using BLL.Classes;
using MODEL.Classes;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class PricingContract
    {
        #region Atributos
        private PricingBLL _pricingBLL { get; set; }
        #endregion

        #region Construtor
        public PricingContract()
        {
            this._pricingBLL = new PricingBLL();
        }
        #endregion

        public PricingModel Get(int id)
        {
            return _pricingBLL.Get(id);
        }

        public PricingModel Get_Info(int id)
        {
            return _pricingBLL.Get_Info(id);
        }

        public PricingModel Get_By_Project(int id)
        {
            return _pricingBLL.Get_By_Project(id);
        }

        public PricingModel Get_By_Opportunitty(int id)
        {
            return _pricingBLL.Get_By_Opportunitty(id);
        }

        public MessageModel Insert(PricingModel model, int id)
        {
            return _pricingBLL.Insert_ProjectPricing(model, id);
        }

        public MessageModel Update(PricingModel model, int id)
        {
            return _pricingBLL.Update_ProjectPricing(model, id);
        }

        public MessageModel Opportunitty_Insert(PricingModel model, int id)
        {
            return _pricingBLL.Opportunitty_Insert(model, id);
        }

        public MessageModel Opportunitty_Update(PricingModel model, int id)
        {
            return _pricingBLL.Update_OpportunityPricing(model, id);
        }
    }
}