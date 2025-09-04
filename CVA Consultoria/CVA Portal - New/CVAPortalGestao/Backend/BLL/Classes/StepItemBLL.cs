using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class StepItemBLL
    {
        #region Atributos
        private StepItemDAO _stepItemDAO { get; set; }
        #endregion

        #region Construtor
        public StepItemBLL()
        {
            this._stepItemDAO = new StepItemDAO();
        }
        #endregion

        public int Insert(StepItemModel item)
        {
            return _stepItemDAO.Insert(item);
        }

        public List<StepItemModel> Get_ForProject(int id)
        {
            var result = _stepItemDAO.Get_ForProject(id);
            return LoadModel(result);
        }

        public void Remove_ForProject(int id)
        {
            _stepItemDAO.Remove_ForProject(id);
        }

        public List<StepItemModel> LoadModel(DataTable result)
        {
            var modelList = new List<StepItemModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new StepItemModel();

                model.Id = Convert.ToInt32(result.Rows[i]["Id"].ToString());
                model.Fase = new StepModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Fase.Id"].ToString()),
                    Nome = result.Rows[i]["Fase.Nome"].ToString(),
                    StepId = Convert.ToInt32(result.Rows[i]["Fase.StepId"].ToString())
                };
                model.Especialidade = new SpecialtyModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Especialidade.Id"].ToString()),
                    Name = result.Rows[i]["Especialidade.Nome"].ToString()
                };
                model.Colaborador = new CollaboratorModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Colaborador.Id"].ToString()),
                    Nome = result.Rows[i]["Colaborador.Nome"].ToString()
                };
                model.CustoOrcado = result.Rows[i]["CustoOrcado"].ToString();
                model.HorasOrcadas = result.Rows[i]["HorasOrcadas"].ToString();

                modelList.Add(model);
            }

            return modelList;
        }
    }
}
