using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class ProjectStepBLL
    {
        #region Atributos
        private ProjectStepDAO _projectStepDAO { get; set; }
        private StatusBLL _statusBLL { get; set; }
        private CollaboratorBLL _collaboratorBLL { get; set; }
        #endregion

        #region Construtor
        public ProjectStepBLL()
        {
            this._projectStepDAO = new ProjectStepDAO();
            this._collaboratorBLL = new CollaboratorBLL();
        }
        #endregion

        public List<StatusModel> GetSpecificStatus()
        {
            this._statusBLL = new StatusBLL();
            return _statusBLL.Get(ProjectStepModel.oObjectType);
        }

        public MessageModel Save(ProjectStepModel model)
        {
            try
            {
                var isValid = ValidateFields(ref model);
                if (isValid.Error != null)
                    return isValid;

                if (model.Id != 0)
                    return Update(model);

                var projectStepId = _projectStepDAO.Save(model);
                if (projectStepId == 0)
                    return MessageBLL.Generate("Ocorreu um erro ao salvar a fase do projeto", 99, true);
                return MessageBLL.Generate("Fase de projeto cadastrada com sucesso", 0);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        private MessageModel Update(ProjectStepModel model)
        {
            try
            {
                var projectStepUpdate = _projectStepDAO.Update(model);
                if (projectStepUpdate > 0)
                    return MessageBLL.Generate("Fase de Projeto atualizada com sucesso", 0);
                return MessageBLL.Generate("Erro ao atualizar fase de projeto", 99, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProjectStepModel> Get(int isProject)
        {
            try
            {
                return _projectStepDAO.Get(isProject);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private MessageModel ValidateFields(ref ProjectStepModel projectStep)
        {
            try
            {
                if (string.IsNullOrEmpty(projectStep.Code))
                    return MessageBLL.Generate("Informe um código para a fase do projeto", 99, true);
                if (string.IsNullOrEmpty(projectStep.Description))
                    projectStep.Description = "";
                if (string.IsNullOrEmpty(projectStep.Name))
                    return MessageBLL.Generate("Informe um nome para a fase do projeto", 99, true);

                return MessageBLL.Generate("Formulário validado com sucesso", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProjectStepModel> Get()
        {
            return _projectStepDAO.Get_All();
        }

        public ProjectStepModel Get_ByID(int id)
        {
            try
            {
                var model = _projectStepDAO.Get_ByID(id);
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StepModel> Get_ProjectSteps(int id, int user)
        {
            if (user != 0)
            {
                user = _collaboratorBLL.GetByUserID(user).Id;
                var result = _projectStepDAO.Get_ProjectSteps(id, user);
                return LoadModel(result);
            }

            else
            {
                var result = _projectStepDAO.Get_ProjectSteps(id, 0);
                return LoadModelFull(result);
            }
        }

        public List<StepModel> LoadModel(DataTable result)
        {
            var modelList = new List<StepModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new StepModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    StepId = Convert.ToInt32(result.Rows[i]["StepId"].ToString()),
                    Nome = result.Rows[i]["Nome"].ToString(),
                    Liberada = result.Rows[i]["Liberada"].ToString(),
                    //DataInicio = Convert.ToDateTime(result.Rows[i]["DataInicio"].ToString()),
                    //DataPrevista = Convert.ToDateTime(result.Rows[i]["DataPrevista"].ToString()),
                    //CustoOrcado = result.Rows[i]["CustoOrcado"].ToString(),
                    //CustoReal = result.Rows[i]["CustoReal"].ToString(),
                    //HorasOrcadas = result.Rows[i]["HorasOrcadas"].ToString(),
                    //HorasConsumidas = result.Rows[i]["HorasConsumidas"].ToString(),
                    //Concluido = result.Rows[i]["Concluido"].ToString()

                };
                modelList.Add(model);
            }
            //for (int i = 0; i < result.Rows.Count; i++)
            //{
            //    var model = new StepModel
            //    {
            //        Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
            //        StepId = Convert.ToInt32(result.Rows[i]["StepId"].ToString()),
            //        Nome = result.Rows[i]["Nome"].ToString(),
            //        DataInicio = Convert.ToDateTime(result.Rows[i]["DataInicio"].ToString()),
            //        DataPrevista = Convert.ToDateTime(result.Rows[i]["DataPrevista"].ToString()),
            //        CustoOrcado = result.Rows[i]["CustoOrcado"].ToString(),
            //        CustoReal = result.Rows[i]["CustoReal"].ToString(),
            //        HorasOrcadas = result.Rows[i]["HorasOrcadas"].ToString(),
            //        HorasConsumidas = result.Rows[i]["HorasConsumidas"].ToString(),
            //        Concluido = result.Rows[i]["Concluido"].ToString()

            //    };
            //    modelList.Add(model);
            //}
            return modelList;
        }

        public List<StepModel> LoadModelFull(DataTable result)
        {
            var modelList = new List<StepModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new StepModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    StepId = Convert.ToInt32(result.Rows[i]["StepId"].ToString()),
                    Nome = result.Rows[i]["Nome"].ToString(),
                    DataInicio = Convert.ToDateTime(result.Rows[i]["DataInicio"].ToString()),
                    DataPrevista = Convert.ToDateTime(result.Rows[i]["DataPrevista"].ToString()),
                    HorasOrcadas = result.Rows[i]["HorasOrcadas"].ToString(),
                    HorasConsumidas = result.Rows[i]["HorasConsumidas"].ToString(),
                };
                modelList.Add(model);
            }
           
            return modelList;
        }
    }
}
