using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;
using System.Linq;

namespace BLL.Classes
{
    public class SpecialtyBLL
    {
        #region Atributos
        private StatusBLL _statusBLL { get; set; }
        private SpecialtyDAO _specialtyDAO { get; set; }
        private CollaboratorDAO _collaboratorDAO { get; set; }
        #endregion

        #region Construtor
        public SpecialtyBLL()
        {
            this._statusBLL = new StatusBLL();
            this._specialtyDAO = new SpecialtyDAO();
            this._collaboratorDAO = new CollaboratorDAO();
        }
        #endregion

        public List<StatusModel> GetSpecificStatus()
        {
            try
            {
                return _statusBLL.Get(SpecialtyModel.oObjectType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MessageModel Save(SpecialtyModel model)
        {
            try
            {
                this._specialtyDAO = new SpecialtyDAO();
                var modelValid = ValidateModel(ref model);
                if (modelValid.Error != null)
                    return modelValid;

                if (model.Id != 0)
                    return Update(ref model);

                var result = _specialtyDAO.Save(model);
                if (result != 0)
                    return MessageBLL.Generate("Especialidade salva com sucesso!", 0);
                return MessageBLL.Generate("Falha ao salvar especialidade", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public List<SpecialtyModel> Get()
        {
            try
            {
                var result = _specialtyDAO.Get();
                return LoadModel(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SpecialtyModel> Get_Specialty(int projectId, int StepId, int user)
        {
            user = _collaboratorDAO.GetForIdUser(user).Id;
            return _specialtyDAO.Get_Specialty(projectId, StepId, user);
        }


        public List<SpecialtyModel> GetByCollaborator(CollaboratorModel collaborator)
        {
            return GetByCollaborator(collaborator.Id);
        }

        public List<SpecialtyTypeModel> Get_TiposEspecialidade()
        {
            return _specialtyDAO.Get_TiposEspecialidade();
        }




        public List<SpecialtyModel> LoadModel(DataTable result)
        {
            var modelList = new List<SpecialtyModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new SpecialtyModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Name = result.Rows[i]["Name"].ToString(),
                    Description = result.Rows[i]["Description"].ToString(),
                    Value = result.Rows[i]["Value"].ToString(),
                    TipoEspecialidade = new SpecialtyTypeModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["TipoEspecialidade.Id"].ToString()),
                        Nome = result.Rows[i]["TipoEspecialidade.Nome"].ToString()
                    },
                };

                modelList.Add(model);
            }
            return modelList;
        }

        public SpecialtyModel Get_ByID(int specialtyID)
        {
            try
            {

                var result = _specialtyDAO.Get_ByID(specialtyID);
                return LoadModel(result)[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SpecialtyModel> Get_All()
        {
            try
            {
                var result = _specialtyDAO.Get_All();
                return LoadModel(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Private Methods
        private MessageModel ValidateModel(ref SpecialtyModel specialty)
        {
            try
            {
                this._specialtyDAO = new SpecialtyDAO();
                if (string.IsNullOrEmpty(specialty.Name) || string.IsNullOrWhiteSpace(specialty.Name))
                    return MessageBLL.Generate("Obrigatório o preenchimento do nome da especialidade", 99, true);
                if (string.IsNullOrWhiteSpace(specialty.Description))
                    specialty.Description = "";

                var statusId = specialty.Status.Id;
                var statusSelected = (from stu in _statusBLL.Get(SpecialtyModel.oObjectType)
                                      where stu.Id == statusId
                                      select stu).FirstOrDefault();

                if (statusSelected == null)
                    return MessageBLL.Generate("Obrigatório a seleção de um status válido", 99, true);

                return MessageBLL.Generate("Campos validados com sucesso!", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private MessageModel Update(ref SpecialtyModel specialty)
        {
            try
            {
                this._specialtyDAO = new SpecialtyDAO();
                if (_specialtyDAO.Update(specialty) > 0)
                    return MessageBLL.Generate("Especialidade atualizada com sucesso", 0);
                return MessageBLL.Generate("Erro ao atualizar especialidade", 99, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SpecialtyModel> GetByCollaborator(int id)
        {
            try
            {

                return _specialtyDAO.GetByCollaborator(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
