using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;


namespace BLL.Classes
{
    public class SubPeriodBLL
    {
        #region Atributos
        private SubPeriodDAO _subPeriodDAO { get; set; }
        #endregion

        #region Construtor
        public SubPeriodBLL()
        {
            _subPeriodDAO = new SubPeriodDAO();
        }
        #endregion

        private List<SubPeriodModel> LoadModel(DataTable result)
        {
            var modelList = new List<SubPeriodModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new SubPeriodModel()
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    User = new UserModel()
                    {
                        Id = Convert.ToInt32(result.Rows[i]["StatusId"].ToString()),
                        Name = result.Rows[i]["User.Name"].ToString()
                    },
                    Status = new StatusModel()
                    {
                        Descricao = result.Rows[i]["Status"].ToString()
                    },
                    CollaboratorId = Convert.ToInt32(result.Rows[i]["ColId"].ToString()),
                    ClientId = Convert.ToInt32(result.Rows[i]["ClientId"].ToString()),
                    Client = result.Rows[i]["Client"].ToString(),

                    DateFrom = Convert.ToDateTime(result.Rows[i]["DateFrom"].ToString()),
                    DateTo = Convert.ToDateTime(result.Rows[i]["DateTo"].ToString()),

                    Collaborator = result.Rows[i]["Collaborator"].ToString(),


                    ProjectId = Convert.ToInt32(result.Rows[i]["ProjectId"].ToString()),
                    Project = result.Rows[i]["Project"].ToString()
                };

                modelList.Add(model);
            }
            return modelList;
        }

        public MessageModel SetStatus(int periodId, int statusId)
        {
            int success = _subPeriodDAO.SetStatus(periodId, statusId);
            return MessageBLL.Generate("Sub-período alterado com sucesso!", success);
        }

        public MessageModel SetStatusOnList(string periodIdList, int statusId)
        {
            int success = _subPeriodDAO.SetStatusOnList(periodIdList, statusId);
            return MessageBLL.Generate("Lista de sub-períodos alterada com sucesso!", success);
        }
    }
}
