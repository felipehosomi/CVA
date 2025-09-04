using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;


namespace BLL.Classes
{
    public class UfBLL
    {
        #region Atributos
        private UfDAO _dao { get; set; }
        #endregion

        #region Construtor
        public UfBLL()
        {
            this._dao = new UfDAO();
        }
        #endregion

        public List<UfModel> Get()
        {
            var result = _dao.Get_All();
            return LoadModel(result);
        }

        public List<UfModel> LoadModel(DataTable result)
        {
            var listModel = new List<UfModel>();

            for(int i = 0; i < result.Rows.Count; i++)
            {
                var model = new UfModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Uf = result.Rows[i]["UF"].ToString(),
                    State = result.Rows[i]["State"].ToString()
                };
                listModel.Add(model);
            }
            return listModel;
        }
    }
}