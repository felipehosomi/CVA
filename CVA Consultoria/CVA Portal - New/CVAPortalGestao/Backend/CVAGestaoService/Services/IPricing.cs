using MODEL.Classes;

using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IPricing
    {
        [OperationContract]
        PricingModel Pricing_Get(int id);

        [OperationContract]
        PricingModel Pricing_Get_Info(int id);

        [OperationContract]
        PricingModel Pricing_Get_By_Project(int id);

        [OperationContract]
        PricingModel Pricing_Get_By_Opportunitty(int id);

        [OperationContract]
        MessageModel Pricing_Insert(PricingModel model, int id);

        [OperationContract]
        MessageModel Pricing_Update(PricingModel model, int id);

        [OperationContract]
        MessageModel Pricing_Opportunitty_Insert(PricingModel model, int id);

        [OperationContract]
        MessageModel Pricing_Opportunitty_Update(PricingModel model, int id);
    }
}