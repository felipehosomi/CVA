using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class ProjectTypeBLL
    {
        #region Atributos
        private ProjectTypeDAO _projectTypeDAO { get; set; }
        #endregion

        #region Construtor
        public ProjectTypeBLL()
        {
            this._projectTypeDAO = new ProjectTypeDAO();
        }
        #endregion

        public ProjectTypeModel Get(int id)
        {
            try
            {
                var result = _projectTypeDAO.Get(id);
                return LoadModel(result)[0];
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public List<ProjectTypeModel> Get_All()
        {
            try
            {
                var result = _projectTypeDAO.Get_All();
                return LoadModel(result);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public MessageModel Insert(ProjectTypeModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                if (_projectTypeDAO.Insert(model) > 0)
                    return MessageBLL.Generate("Tipo de projeto inserido.", 0);
                else
                    return MessageBLL.Generate("Falha ao inserir tipo de projeto.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }

        public MessageModel Update(ProjectTypeModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                if (_projectTypeDAO.Update(model) > 0)
                    return MessageBLL.Generate("Tipo de projeto atualizado.", 0);
                else
                    return MessageBLL.Generate("Falha ao atualizar tipo de projeto.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }

        public MessageModel Remove(int id)
        {
            try
            {
                if (_projectTypeDAO.Remove(id) > 0)
                    return MessageBLL.Generate("Tipo de projeto removido.", 0);
                else
                    return MessageBLL.Generate("Não é possível remover um tipo de projeto que esteja em uso.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }

        public MessageModel ValidateModel(ProjectTypeModel model)
        {
            if (String.IsNullOrEmpty(model.Nome))
                return MessageBLL.Generate("Obrigatório informar o nome.", 99, true);
            if (String.IsNullOrEmpty(model.AMS))
                return MessageBLL.Generate("Obrigatório informar se AMS.", 99, true);
            if (String.IsNullOrEmpty(model.Equipe))
                return MessageBLL.Generate("Obrigatório informar a equipe.", 99, true);
            if (String.IsNullOrEmpty(model.Descricao))
                return MessageBLL.Generate("Obrigatório informar a descrição.", 99, true);
            else
                return null;
        }

        public List<ProjectTypeModel> LoadModel(DataTable result)
        {
            var modelList = new List<ProjectTypeModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new ProjectTypeModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Status = new StatusModel()
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Status"].ToString())
                    },
                    Nome = result.Rows[i]["Nome"].ToString(),
                    Equipe = result.Rows[i]["Equipe"].ToString(),
                    Descricao = result.Rows[i]["Descricao"].ToString(),
                    AMS = result.Rows[i]["AMS"].ToString()
                };
                modelList.Add(model);
            }
            return modelList;
        }
    }
}
