using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IProfile
    {
        [OperationContract]
        MessageModel SaveProfile(ProfileModel profile);

        [OperationContract]
        List<StatusModel> ProfileGetStatus();

        [OperationContract]
        List<ProfileModel> GetProfiles();

        [OperationContract]
        ProfileModel GetProfile_ByID(int profileID);
    }
}