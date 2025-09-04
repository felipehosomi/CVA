using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class MemberBLL
    {
        #region Atributos
        private MemberDAO _memberDAO { get; set; }
        #endregion

        #region Construtor
        public MemberBLL()
        {
            this._memberDAO = new MemberDAO();
        }
        #endregion


        public MemberModel Get(int id)
        {
            try
            {
                var result = _memberDAO.Get(id);
                return LoadModel(result)[0];
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public List<MemberModel> Get_All()
        {
            try
            {
                var result = _memberDAO.Get_All();
                return LoadModel(result);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public List<MemberModel> Get_ProjectMembers(int id)
        {
            var result = _memberDAO.Get_ProjectMembers(id);
            return LoadModel(result);
        }

        public MessageModel Insert(MemberModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid.Error != null)
                    return isValid;

                if (_memberDAO.Insert(model) > 0)
                    return MessageBLL.Generate("Membro inserido com sucesso", 0);

                else
                    return MessageBLL.Generate("Erro ao inserir membro", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel Update(MemberModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid.Error != null)
                    return isValid;

                if (_memberDAO.Update(model) > 0)
                    return MessageBLL.Generate("Membro atualizado com sucesso", 0);
                else
                    return MessageBLL.Generate("Erro ao atualizar membro", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel Remove(int id)
        {
            try
            {
                if (_memberDAO.Remove(id) > 0)
                    return MessageBLL.Generate("Membro removido com sucesso", 0);
                else
                    return MessageBLL.Generate("Erro ao remover membro", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel ValidateModel(MemberModel model)
        {
            if (model.Nome == null)
                return MessageBLL.Generate("Obrigatório informar o nome", 99, true);
            if (model.Telefone == null)
                return MessageBLL.Generate("Obrigatório informar o telefone", 99, true);
            if (model.Email == null)
                return MessageBLL.Generate("Obrigatório informar o email", 99, true);
            if (model.Departamento == null)
                return MessageBLL.Generate("Obrigatório informar o departamento", 99, true);

            else
                return null;
        }

        public List<MemberModel> LoadModel(DataTable result)
        {
            var modelList = new List<MemberModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new MemberModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Status = new StatusModel()
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Status"].ToString())
                    },
                    Nome = result.Rows[i]["Nome"].ToString(),
                    Telefone = result.Rows[i]["Telefone"].ToString(),
                    Email = result.Rows[i]["Email"].ToString(),
                    Departamento = result.Rows[i]["Departamento"].ToString()
                };
                modelList.Add(model);
            }
            return modelList;
        }


    }
}
