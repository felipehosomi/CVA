using System;
using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class ProjectParametersBLL
    {
        #region Atributos
        public ProjectParametersDAO _projectParametersDAO { get; set; }
        #endregion

        #region Construtor
        public ProjectParametersBLL()
        {
            this._projectParametersDAO = new ProjectParametersDAO();
        }
        #endregion

        public List<ProjectParametersModel> Get_All()
        {
            return _projectParametersDAO.Get_All();
        }

        public MessageModel Save(ProjectParametersModel model)
        {
            if (model.Id == 0)
                return Insert(model);
            else
                return Update(model);
        }

        public MessageModel Insert(ProjectParametersModel model)
        {
            try
            {
                if (_projectParametersDAO.Insert(model) > 0)
                    return MessageBLL.Generate("Parâmetro inserido.", 0);
                else
                    return MessageBLL.Generate("Falha ao inserir parâmetro.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }

        public MessageModel Update(ProjectParametersModel model)
        {
            try
            {
                if (_projectParametersDAO.Update(model) > 0)
                    return MessageBLL.Generate("Parâmetro atualizado.", 0);
                else
                    return MessageBLL.Generate("Falha ao atualizar parâmetro.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }
    }
}
