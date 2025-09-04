using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class ChangeRequestBLL
    {
        #region Atributos
        public ChangeRequestDAO _changeRequestDAO { get; set; }
        #endregion

        #region Construtor
        public ChangeRequestBLL()
        {
            _changeRequestDAO = new ChangeRequestDAO();

        }
        #endregion

        public MessageModel Save(ChangeRequestModel model)
        {
            if (model.Id == 0)
                return Insert(model);
            else
                return Update(model);
        }

        public MessageModel Insert(ChangeRequestModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                model.Id = _changeRequestDAO.Insert(model);
                foreach (var item in model.RecursosSolicitados)
                {
                    _changeRequestDAO.Insert_Itens(model, item);
                }

                return MessageBLL.Generate("Change Request inserida com sucesso", 0);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate("Ocorreu um erro ao salvar a Change Request: " + ex.Message, 99, true);
            }
        }

        public MessageModel Update(ChangeRequestModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                if (_changeRequestDAO.Update(model) > 0)
                {
                    _changeRequestDAO.Remove_Itens(model);
                    foreach (var item in model.RecursosSolicitados)
                    {
                        _changeRequestDAO.Insert_Itens(model, item);
                        
                        if(model.Situacao == "Aprovada")
                        {
                           _changeRequestDAO.Add_ChangeRequestHours(model, item);
                        }
                    }



                    return MessageBLL.Generate("Change Request atualizada com sucesso", 0);
                }
                else
                    return MessageBLL.Generate("Ocorreu um erro ao atualizar a Change Request", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate("Ocorreu um erro ao atualizar a Change Request: " + ex.Message, 99, true);
            }
        }

        public ChangeRequestModel Get(int id)
        {
            var result = _changeRequestDAO.Get(id);
            return LoadModel(result);
        }

        public List<ChangeRequestModel> Get_for_Project(int id)
        {
            return _changeRequestDAO.Get_for_Project(id);
        }

        public ChangeRequestModel LoadModel(DataTable result)
        {
            var model = new ChangeRequestModel
            {
                Id = Convert.ToInt32(result.Rows[0]["Id"].ToString()),
                Codigo = result.Rows[0]["Codigo"].ToString(),
                Versao = result.Rows[0]["Versao"].ToString(),
                Autor = result.Rows[0]["Autor"].ToString(),
                Situacao = result.Rows[0]["Situacao"].ToString(),
                GPI = result.Rows[0]["GPI"].ToString(),
                GPE = result.Rows[0]["GPE"].ToString(),
                Departamento = result.Rows[0]["Departamento"].ToString(),
                Processo = result.Rows[0]["Processo"].ToString(),
                Descricao = result.Rows[0]["Descricao"].ToString(),
                Motivos = result.Rows[0]["Motivos"].ToString(),
                Recomendacoes = result.Rows[0]["Recomendacoes"].ToString(),
                ImpactosPositivos = result.Rows[0]["ImpactosPositivos"].ToString(),
                ImpactosNegativos = result.Rows[0]["ImpactosNegativos"].ToString(),
                RecursosSolicitados = new List<ChangeRequestRecursoModel>()
            };

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var item = new ChangeRequestRecursoModel
                {
                    RecursoFase = Convert.ToInt32(result.Rows[i]["RecursoFase"].ToString()),
                    RecursoFaseNome = result.Rows[i]["RecursoFaseNome"].ToString(),
                    RecursoEspecialidade = Convert.ToInt32(result.Rows[i]["RecursoEspecialidade"].ToString()),
                    RecursoEspecialidadeNome = result.Rows[i]["RecursoEspecialidadeNome"].ToString(),
                    RecursoHorasSolicitadas = result.Rows[i]["RecursoHorasSolicitadas"].ToString(),
                    RecursoSolicitante = result.Rows[i]["RecursoSolicitante"].ToString(),
                    RecursoNecessidade = result.Rows[i]["RecursoNecessidade"].ToString()
                };
                model.RecursosSolicitados.Add(item);
            }

            return model;
        }

        public MessageModel ValidateModel(ChangeRequestModel model)
        {
            try
            {
                if (model.Codigo == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Código'", 99, true);
                if (model.Versao == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Versão'", 99, true);
                if (model.Autor == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Autor'", 99, true);
                if (model.Situacao == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Status'", 99, true);
                if (model.GPI == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Gerente de Projeto (CVA)'", 99, true);
                if (model.GPE == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Gerente de Projeto (Externo)'", 99, true);
                if (model.Departamento == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Departamento'", 99, true);
                if (model.Processo == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Processo'", 99, true);
                if (model.Descricao == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Descrição'", 99, true);
                if (model.Motivos == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Motivos'", 99, true);
                if (model.Recomendacoes == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Recomendações'", 99, true);
                if (model.ImpactosPositivos == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Impactos Positivos'", 99, true);
                if (model.ImpactosNegativos == null)
                    return MessageBLL.Generate("Obrigatório preencher o campo 'Impactos caso não aprovada'", 99, true);
                if (model.RecursosSolicitados == null)
                    return MessageBLL.Generate("Obrigatório inserir ao menos um 'Recurso Solicitado'", 99, true);

                foreach (var item in model.RecursosSolicitados)
                {
                    if (item.RecursoFase == 0)
                        return MessageBLL.Generate("Obrigatório preencher o campo 'Fase' do Recurso Inserido", 99, true);
                    if (item.RecursoFaseNome == null)
                        return MessageBLL.Generate("Obrigatório preencher o campo 'Fase' do Recurso Inserido", 99, true);
                    if (item.RecursoEspecialidade == 0)
                        return MessageBLL.Generate("Obrigatório preencher o campo 'Especialidade' do Recurso Inserido", 99, true);
                    if (item.RecursoEspecialidadeNome == null)
                        return MessageBLL.Generate("Obrigatório preencher o campo 'Especialidade' do Recurso Inserido", 99, true);
                    if (item.RecursoHorasSolicitadas == null)
                        return MessageBLL.Generate("Obrigatório preencher o campo 'Horas Solicitadas' do Recurso Inserido", 99, true);
                    if (item.RecursoSolicitante == null)
                        return MessageBLL.Generate("Obrigatório preencher o campo 'Solicitante' do Recurso Inserido", 99, true);
                    if (item.RecursoNecessidade == null)
                        return MessageBLL.Generate("Obrigatório preencher o campo 'Necessidade' do Recurso Inserido", 99, true);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}