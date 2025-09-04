using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class AuthorizationBLL
    {
        #region Atributos
        private AuthorizationDAO _authorizationDAO { get; set; }
        #endregion

        #region Construtor
        public AuthorizationBLL()
        {
            this._authorizationDAO = new AuthorizationDAO();
        }
        #endregion

        #region Dias
        public List<AuthorizedDayModel> Get_DiasAutorizados(int idCol)
        {
            var result = _authorizationDAO.Get_DiasAutorizados(idCol);
            return LoadAuthorizedDayModel(result);
        }

        public MessageModel AddDiaAutorizado(AuthorizedDayModel model)
        {
            try
            {
                if (_authorizationDAO.AddDiaAutorizado(model) > 0)
                    return MessageBLL.Generate("Registro inserido com sucesso!", 0);
                else
                    return MessageBLL.Generate("Ocorreu um erro ao gravar o registro.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel RemoveDiaAutorizado(int id)
        {
            try
            {
                if (_authorizationDAO.RemoveDiaAutorizado(id) > 0)
                    return MessageBLL.Generate("Registro removido com sucesso!", 0);
                else
                    return MessageBLL.Generate("Ocorreu um erro ao remover o registro.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public List<AuthorizedDayModel> LoadAuthorizedDayModel(DataTable result)
        {
            var modelList = new List<AuthorizedDayModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new AuthorizedDayModel
                {
                    User = new UserModel
                    {
                        Name = result.Rows[i]["User.Name"].ToString(),
                    },
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Colaborador = Convert.ToInt32(result.Rows[i]["Colaborador"].ToString()),
                    ColaboradorNome = result.Rows[i]["ColaboradorNome"].ToString(),
                    De = Convert.ToDateTime(result.Rows[i]["De"].ToString()),
                    Ate = Convert.ToDateTime(result.Rows[i]["Ate"].ToString())
                };
                modelList.Add(model);
            }
            return modelList;
        }
        #endregion Dias

        #region Despesas
        public List<AuthorizedDayModel> Get_DespesasAutorizados(int idCol)
        {
            var result = _authorizationDAO.Get_DespesasAutorizados(idCol);
            return LoadAuthorizedExpenseModel(result);
        }

        public MessageModel AddDespesaAutorizado(AuthorizedDayModel model)
        {
            try
            {
                if (_authorizationDAO.AddDespesaAutorizado(model) > 0)
                    return MessageBLL.Generate("Registro inserido com sucesso!", 0);
                else
                    return MessageBLL.Generate("Ocorreu um erro ao gravar o registro.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel RemoveDespesaAutorizado(int id)
        {
            try
            {
                if (_authorizationDAO.RemoveDespesaAutorizado(id) > 0)
                    return MessageBLL.Generate("Registro removido com sucesso!", 0);
                else
                    return MessageBLL.Generate("Ocorreu um erro ao remover o registro.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public List<AuthorizedDayModel> LoadAuthorizedExpenseModel(DataTable result)
        {
            var modelList = new List<AuthorizedDayModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new AuthorizedDayModel
                {
                    User = new UserModel
                    {
                        Name = result.Rows[i]["User.Name"].ToString(),
                    },
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Colaborador = Convert.ToInt32(result.Rows[i]["Colaborador"].ToString()),
                    ColaboradorNome = result.Rows[i]["ColaboradorNome"].ToString(),
                    De = Convert.ToDateTime(result.Rows[i]["De"].ToString()),
                    Ate = Convert.ToDateTime(result.Rows[i]["Ate"].ToString())
                };
                modelList.Add(model);
            }
            return modelList;
        }
        #endregion Despesas

        #region Horas
        public List<AuthorizedHoursModel> Get_HorasAutorizadas(int idCol)
        {
            var result = _authorizationDAO.Get_HorasAutorizadas(idCol);
            return LoadAuthorizedHoursModel(result);
        }

        public MessageModel AddHorasAutorizadas(AuthorizedHoursModel model)
        {
            try
            {
                if (_authorizationDAO.AddHorasAutorizadas(model) > 0)
                    return MessageBLL.Generate("Registro inserido com sucesso!", 0);
                else
                    return MessageBLL.Generate("Ocorreu um erro ao gravar o registro.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel RemoveHorasAutorizadas(int id)
        {
            try
            {
                if (_authorizationDAO.RemoveHorasAutorizadas(id) > 0)
                    return MessageBLL.Generate("Registro removido com sucesso!", 0);
                else
                    return MessageBLL.Generate("Ocorreu um erro ao remover o registro.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public List<AuthorizedHoursModel> LoadAuthorizedHoursModel(DataTable result)
        {
            var modelList = new List<AuthorizedHoursModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new AuthorizedHoursModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    User = new UserModel
                    {
                        Name = result.Rows[i]["User"].ToString(),
                    },
                    Colaborador = Convert.ToInt32(result.Rows[i]["Colaborador"].ToString()),
                    ColaboradorNome = result.Rows[i]["ColaboradorNome"].ToString(),
                    Data = Convert.ToDateTime(result.Rows[i]["Data"].ToString()),
                    Horas = Convert.ToDateTime(result.Rows[i]["Horas"].ToString())
                };
                modelList.Add(model);
            }
            return modelList;
        }

        public List<HoursLimitModel> Get_LimiteHoras(int idCol)
        {
            var result = _authorizationDAO.Get_LimiteHoras(idCol);
            return LoadHoursLimitModel(result);
        }

        public MessageModel AddLimiteHoras(HoursLimitModel model)
        {
            try
            {
                if (_authorizationDAO.AddLimiteHoras(model) > 0)
                    return MessageBLL.Generate("Registro inserido com sucesso!", 0);
                else
                    return MessageBLL.Generate("Ocorreu um erro ao gravar o registro.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel RemoveLimiteHoras(int id)
        {
            try
            {
                if (_authorizationDAO.RemoveLimiteHoras(id) > 0)
                    return MessageBLL.Generate("Registro removido com sucesso!", 0);
                else
                    return MessageBLL.Generate("Ocorreu um erro ao remover o registro.", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public List<HoursLimitModel> LoadHoursLimitModel(DataTable result)
        {
            var modelList = new List<HoursLimitModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new HoursLimitModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    User = new UserModel
                    {
                        Name = result.Rows[i]["User"].ToString(),
                    },
                    Colaborador = Convert.ToInt32(result.Rows[i]["Colaborador"].ToString()),
                    ColaboradorNome = result.Rows[i]["ColaboradorNome"].ToString(),
                    Horas = Convert.ToDateTime(result.Rows[i]["Horas"].ToString())
                };
                modelList.Add(model);
            }
            return modelList;
        }
        #endregion Horas
    }
}