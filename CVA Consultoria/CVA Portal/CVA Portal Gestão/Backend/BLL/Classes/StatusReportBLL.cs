using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class StatusReportBLL
    {
        #region Atributos
        private StatusReportDAO _statusReportDAO { get; set; }
        #endregion

        #region Construtor
        public StatusReportBLL()
        {
            this._statusReportDAO = new StatusReportDAO();
        }
        #endregion

        public MessageModel Save(StatusReportModel model)
        {
            if (model.Id == 0)
                return Insert(model);
            else
                return Update(model);
        }

        public MessageModel Insert(StatusReportModel model)
        {
            model = Get_StatusReport_Info(model);

            var isValid = ValidateModel(model);
            if (isValid != null)
                return isValid;
       

            if (_statusReportDAO.Insert(model) > 0)
            {
                foreach (var item in model.Fases)
                    _statusReportDAO.Insert_Steps(model, item);
                return MessageBLL.Generate("Status Report inserido com sucesso.", 0);
            }
            else
                return MessageBLL.Generate("Ocorreu um erro ao inserir o Status Report.", 99, true);
        }
        public MessageModel Update(StatusReportModel model)
        {
            return null;
            //var isValid = ValidateModel(model);
            //if (isValid != null)
            //    return isValid;

            //model = Get_StatusReport_Info(model);

            //if (_statusReportDAO.Insert(model) > 0)
            //    return MessageBLL.Generate("Status Report inserido com sucesso.", 99, true);
            //else
            //    return MessageBLL.Generate("Ocorreu um erro ao inserir o Status Report.", 99, true);
        }


        public StatusReportModel Get_StatusReport_Info(StatusReportModel model)
        {
            var HorasConsumidas = new TimeSpan();

            var result = _statusReportDAO.Get_StatusReport_Info(model.Projeto.Id, model.Data);

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var Inicio = TimeSpan.Parse(result.Rows[i]["Inicio"].ToString());
                var Fim = TimeSpan.Parse(result.Rows[i]["Fim"].ToString());

                var Intervalo = new TimeSpan();
                if (!String.IsNullOrEmpty(result.Rows[i]["Intervalo"].ToString()))
                    Intervalo = TimeSpan.Parse(result.Rows[i]["Intervalo"].ToString());

                HorasConsumidas += (Fim - (Inicio + Intervalo));
            }

            if (result.Rows.Count > 0)
            {
                model.HorasConsumidas = Convert.ToString(Math.Round(HorasConsumidas.TotalHours, 2));
                model.GerenteProjeto = new CollaboratorModel
                {
                    Id = Convert.ToInt32(result.Rows[0]["Gerente.Id"].ToString()),
                    Nome = result.Rows[0]["Gerente.Nome"].ToString(),
                };
            }

            var HorasOrcadas = 0.0;

            foreach(var item in model.Fases)
            {
                HorasOrcadas += Convert.ToDouble(item.HorasOrcadas);
            }

            model.HorasOrcadas = Convert.ToString(Math.Round(HorasOrcadas, 2));

            var concluido = Math.Round((Math.Round(HorasConsumidas.TotalHours, 2) * 100) / Math.Round(HorasOrcadas, 2), 2);
            model.Concluido = concluido.ToString();
            return model;
        }



        public MessageModel ValidateModel(StatusReportModel model)
        {
            if (model.Data == null)
                return MessageBLL.Generate("Preencha o campo 'Data'.", 99, true);
            if (String.IsNullOrEmpty(model.Descricao))
                return MessageBLL.Generate("Preencha o campo 'Descrição'.", 99, true);
            if (String.IsNullOrEmpty(model.PontosAtencao))
                return MessageBLL.Generate("Preencha o campo 'Pontos de Atenção'.", 99, true);
            if (String.IsNullOrEmpty(model.PlanoDeAcao))
                return MessageBLL.Generate("Preencha o campo 'Plano de Ação'.", 99, true);
            if (String.IsNullOrEmpty(model.Conquistas))
                return MessageBLL.Generate("Preencha o campo 'Conquistas'.", 99, true);
            if (String.IsNullOrEmpty(model.ProximosPassos))
                return MessageBLL.Generate("Preencha o campo 'Próximos Passos'.", 99, true);



            if (model.GerenteProjeto == null)
                return MessageBLL.Generate("Obrigatório informar o código", 99, true);
            if (String.IsNullOrEmpty(model.HorasOrcadas))
                return MessageBLL.Generate("Obrigatório informar o código", 99, true);
            if (String.IsNullOrEmpty(model.HorasConsumidas))
                return MessageBLL.Generate("Obrigatório informar o código", 99, true);
            if (String.IsNullOrEmpty(model.Concluido))
                return MessageBLL.Generate("Obrigatório informar o código", 99, true);
            if (model.Fases == null)
                return MessageBLL.Generate("Obrigatório informar o código", 99, true);
            else
                return null;
        }


        public StatusReportModel Get(int id)
        {
            var result = _statusReportDAO.Get(id);
            return LoadModel(result)[0];
        }

        public List<StatusReportModel> Get_All(int id)
        {
            var result = _statusReportDAO.Get_All(id);
            return LoadModel(result);
        }

        public List<StatusReportModel> LoadModel(DataTable result)
        {
            var modelList = new List<StatusReportModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new StatusReportModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Projeto = new ProjectModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Projeto.Id"].ToString())
                    },

                    Data = Convert.ToDateTime(result.Rows[i]["Data"].ToString()),
                    Descricao = result.Rows[i]["Descricao"].ToString(),
                    PontosAtencao = result.Rows[i]["PontosAtencao"].ToString(),
                    PlanoDeAcao = result.Rows[i]["PlanoDeAcao"].ToString(),
                    Conquistas = result.Rows[i]["Conquistas"].ToString(),
                    ProximosPassos = result.Rows[i]["ProximosPassos"].ToString(),
                    GerenteProjeto = new CollaboratorModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["GerenteProjeto.Id"].ToString()),
                        Nome = result.Rows[i]["GerenteProjeto.Nome"].ToString()
                    },
                    HorasOrcadas = result.Rows[i]["HorasOrcadas"].ToString(),
                    HorasConsumidas = result.Rows[i]["HorasConsumidas"].ToString(),
                    Concluido = result.Rows[i]["Concluido"].ToString()
                };
                modelList.Add(model);
            }
            return modelList;
        }

        public string[] Get_ParcialHours(int id, DateTime data)
        {
            var result = new DataTable();

            var PlanejadoFuncional = new Double();
            var PlanejadoGP = new Double();
            var PlanejadoDev = new Double();
            result = _statusReportDAO.Get_ParcialHours_Orc(id, data);

            for (int i = 0; i < result.Rows.Count; i++)
            {
                if (result.Rows[i]["Especialidade"].ToString() == "Funcional")
                    PlanejadoFuncional += Convert.ToDouble(result.Rows[i]["Orçadas"].ToString());

                if (result.Rows[i]["Especialidade"].ToString() == "Gerente")
                    PlanejadoGP += Convert.ToDouble(result.Rows[i]["Orçadas"].ToString());

                if (result.Rows[i]["Especialidade"].ToString() == "Desenvolvedor")
                    PlanejadoDev += Convert.ToDouble(result.Rows[i]["Orçadas"].ToString());
            }

            var ApontadoFuncional = new Double();
            var ApontadoGP = new Double();
            var ApontadoDev = new Double();
            result = _statusReportDAO.Get_ParcialHours_Con(id, data.AddDays(1));

            var HorasConsumidas = new TimeSpan();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var Inicio = TimeSpan.Parse(result.Rows[i]["Inicio"].ToString());
                var Fim = TimeSpan.Parse(result.Rows[i]["Fim"].ToString());

                var Intervalo = new TimeSpan();
                if (!String.IsNullOrEmpty(result.Rows[i]["Intervalo"].ToString()))
                    Intervalo = TimeSpan.Parse(result.Rows[i]["Intervalo"].ToString());

                HorasConsumidas = (Fim - (Inicio + Intervalo));


                if (result.Rows[i]["Especialidade"].ToString() == "Funcional")
                    ApontadoFuncional += HorasConsumidas.TotalHours;

                if (result.Rows[i]["Especialidade"].ToString() == "Gerente")
                    ApontadoGP += HorasConsumidas.TotalHours;

                if (result.Rows[i]["Especialidade"].ToString() == "Desenvolvedor")
                    ApontadoDev += HorasConsumidas.TotalHours;
            }


            var horas = new string[6];
            horas[0] = Math.Round(PlanejadoFuncional, 2).ToString();
            horas[1] = Math.Round(PlanejadoGP, 2).ToString();
            horas[2] = Math.Round(PlanejadoDev, 2).ToString();
            horas[3] = Math.Round(ApontadoFuncional, 2).ToString();
            horas[4] = Math.Round(ApontadoGP, 2).ToString();
            horas[5] = Math.Round(ApontadoDev, 2).ToString();

            return horas;
        }
    }
}