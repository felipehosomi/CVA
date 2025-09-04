using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class ProfileContract
    {
        private ProfileBLL _profileBLL { get; set; }

        public ProfileContract()
        {
            this._profileBLL = new ProfileBLL();
        }

        public List<ProfileModel> Get()
        {
            return _profileBLL.Get();
        }

        public MessageModel Save(ProfileModel profile)
        {
            return _profileBLL.Save(profile);
        }

        public List<StatusModel> GetSpecificStatus()
        {
            return _profileBLL.GetSpecificStatus();
        }

        public ProfileModel Get_ByID(int profileID)
        {
            return _profileBLL.Get_ByID(profileID);
        }
        
    }
}