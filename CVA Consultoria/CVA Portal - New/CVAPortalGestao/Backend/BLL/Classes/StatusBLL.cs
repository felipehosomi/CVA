using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class StatusBLL
    {
        #region Atributos
        private StatusDAO _statusDAO { get; set; }
        #endregion

        #region Construtor
        public StatusBLL()
        {
            this._statusDAO = new StatusDAO();
        }
        #endregion

        public List<StatusModel> Get(int objectId)
        {
            try
            {
               var result = _statusDAO.Get(objectId);
                return LoadModel(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<StatusModel> LoadModel(DataTable result)
        {
            var modelList = new List<StatusModel>();

            for (int i = 0; i < result.Rows.Count;i++)
            {
                var model = new StatusModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Descricao = result.Rows[i]["Descricao"].ToString()
                };

                modelList.Add(model);
            }
            return modelList;
        }
    }
}
