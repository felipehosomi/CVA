using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;
using System.Linq;

namespace BLL.Classes
{
    public class ProfileBLL
    {
        #region Atributos
        private ProfileDAO _profileDAO { get; set; }
        private StatusBLL _statusBLL { get; set; }
        private UserViewBLL _userViewBLL { get; set; }
        #endregion

        #region Construtor
        public ProfileBLL()
        {
            this._profileDAO = new ProfileDAO();
            this._userViewBLL = new UserViewBLL();
        }
        #endregion

        public List<ProfileModel> Get()
        {
            try
            {
                this._userViewBLL = new UserViewBLL();
                var profiles = _profileDAO.Get();
                foreach (var profile in profiles)
                    profile.UserView = _userViewBLL.Get_ByProfileID(profile.Id);
                return profiles;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StatusModel> GetSpecificStatus()
        {

            this._statusBLL = new StatusBLL();
            return _statusBLL.Get(ProfileModel.oObjectType);
        }

        public MessageModel Save(ProfileModel model)
        {
            try
            {
                var isValid = ValidateFields(ref model);
                if (isValid.Error != null) return isValid;

                if (model.Id != 0)
                    return Update(model);

                var profileId = _profileDAO.Save(model);
                if (profileId == 0)
                    return MessageBLL.Generate("Ocorreu um erro ao salvar o perfil", 99, true);

                model.Id = profileId;
                var profile1 = Profile1_Save(model);
                if (profile1.Where(x => x.Error != null).Count() > 0)
                    return MessageBLL.Generate("Ocorreu um erro ao salvar a ligação do perfil", 99, true);

                return MessageBLL.Generate("Perfil savo com sucesso", 0);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public ProfileModel GetUser(int userID)
        {
            try
            {
                var profile = _profileDAO.GetUser(userID);
                if (profile != null)
                {
                    _userViewBLL = new UserViewBLL();
                    profile.UserView = _userViewBLL.Get_ByProfileID(profile.Id);
                }
                    
                return profile;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProfileModel Get_ByID(int profileID)
        {
            try
            {
                var profile = _profileDAO.Get_ByID(profileID);
                profile.UserView = _userViewBLL.Get_ByProfileID(profile.Id);

                return profile;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MessageModel Update(ProfileModel profile)
        {
            try
            {
                var update = _profileDAO.Update(profile);
                if (update != 0)
                {
                    var inactiveAccess = _profileDAO.Profile1_Inactive(profile.Id);
                    foreach (var view in profile.UserView)
                    {
                        var profile1_update = Profile1_Update(profile, view.Id);
                        if (profile1_update.Error != null)
                            return profile1_update;
                    }
                    return MessageBLL.Generate("Perfil atualizado com sucesso!", 0);
                }
                return MessageBLL.Generate("Erro ao atualizar perfil", 99, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private MessageModel Profile1_Update(ProfileModel profile, int userViewID)
        {
            try
            {
                if (_profileDAO.Profile1_Update(profile, userViewID) > 0)
                    return MessageBLL.Generate("Acesso atualizado com sucesso!", 0);
                return MessageBLL.Generate("Erro ao atualizar acesso", 99, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private MessageModel ValidateFields(ref ProfileModel profile)
        {
            try
            {
                if (string.IsNullOrEmpty(profile.Name))
                    return MessageBLL.Generate("Informe o nome do perfil", 99, true);
                if (profile.UserView == null || profile.UserView.Count == 0)
                    return MessageBLL.Generate("Informe o(s) acesso(s)", 99, true);
                if (string.IsNullOrEmpty(profile.Description))
                    profile.Description = "";
                if (string.IsNullOrEmpty(profile.Initials))
                    profile.Initials = profile.Name.Substring(0, 3);
                return MessageBLL.Generate("Formulário validado com sucesso", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<MessageModel> Profile1_Save(ProfileModel profile)
        {
            try
            {
                var list = new List<MessageModel>();
                foreach (var item in profile.UserView)
                {
                    if (_profileDAO.Profile1_Save(profile, item.Id) == 0)
                        list.Add(MessageBLL.Generate("Erro ao adicionar a ligação do perfil com as telas", 99, true));
                    else
                        list.Add(MessageBLL.Generate("Telas adicionadas com sucesso", 0));
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
