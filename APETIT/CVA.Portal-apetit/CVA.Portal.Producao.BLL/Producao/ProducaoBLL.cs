using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model.Producao;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Producao
{
    public class ProducaoBLL : BaseBLL
    {
        public string GetPercentualProducao(DateTime? dataDe, DateTime? dataAte, int? nrOP, int? nrPedido)
        {
            string nrOPStr = "NULL";
            string nrPedidoStr = "NULL";

            if (!dataDe.HasValue)
            {
                dataDe = new DateTime(1900, 01, 01);
            }
            if (!dataAte.HasValue)
            {
                dataAte = new DateTime(2100, 01, 01);
            }

            if (nrOP.HasValue)
            {
                nrOPStr = nrOP.ToString();
            }
            if (nrPedido.HasValue)
            {
                nrPedidoStr = nrPedidoStr.ToString();
            }

            string command = String.Format(Commands.Resource.GetString("PercentualProducao_Get"), BaseBLL.Database, dataDe.Value.ToString("yyyyMMdd"), dataAte.Value.ToString("yyyyMMdd"), nrOPStr, nrPedidoStr);
            object retorno = DAO.ExecuteScalar(command);
            double percentual = 100;
            if (retorno != null && retorno != DBNull.Value)
            {
                percentual = Convert.ToDouble(retorno.ToString().Replace(".", ","));
            }
            return percentual.ToString("f2");
        }

        public List<ProducaoModel> GetOPsPendentes(string codUsuario, DateTime? dataDe, DateTime? dataAte, int? nrOP, int? nrPedido, string itemDesc, string etapa)
        {
            string sql = Commands.Resource.GetString("OP_GetPendente");
            if (!dataDe.HasValue)
            {
                dataDe = new DateTime(1900, 01, 01);
            }
            if (!dataAte.HasValue)
            {
                dataAte = new DateTime(2100, 01, 01);
            }

            string op = (nrOP.HasValue) ? nrOP.ToString() : "NULL";
            string pedido = (nrPedido.HasValue) ? nrPedido.ToString() : "NULL";
            if (String.IsNullOrEmpty(etapa))
            {
                etapa = "NULL";
            }
            else
            {
                etapa = $"'{etapa}'";
            }
            sql = String.Format(sql, BaseBLL.Database, codUsuario, dataDe.Value.ToString("yyyyMMdd"), dataAte.Value.ToString("yyyyMMdd"), op, pedido, itemDesc, etapa);

            return DAO.FillListFromCommand<ProducaoModel>(sql);
        }

        public ProducaoModel GetDadosOP(int nrOP, string codEtapa)
        {
            string sql = Commands.Resource.GetString("OP_Get");
            sql = String.Format(sql, BaseBLL.Database, nrOP, codEtapa);
            return DAO.FillModelFromCommand<ProducaoModel>(sql);
        }

        public List<EstruturaProducaoModel> GetEstruturaProducao(int nrOP, int codEtapa)
        {
            string command = String.Format(Commands.Resource.GetString("EstruturaProducao_Get"), BaseBLL.Database, nrOP, codEtapa);
            return DAO.FillListFromCommand<EstruturaProducaoModel>(command);
        }

        public List<RecursoProducaoModel> GetRecursoProducao(int nrOP, int codEtapa)
        {
            string command = string.Format(Commands.Resource.GetString("OP_GetRecurso"), BaseBLL.Database, nrOP, codEtapa);
            return DAO.FillListFromCommand<RecursoProducaoModel>(command);
        }

        public List<HistoricoProducaoModel> GetHistorico(int docEntry)
        {
            return DAO.FillListFromCommand<HistoricoProducaoModel>(String.Format(Commands.Resource.GetString("Etapa_GetHistorico"), BaseBLL.Database, docEntry));
        }

        public async Task<string> FechaOP(ProducaoModel opModel)
        {
            if (Static.Config.ServerType == BoDataServerTypes.dst_HANADB)
            {
                return await this.FechaOPSL(opModel);
            }
            else
            {
                return await this.FechaOPDI(opModel);
            }
        }

        public async Task<string> FechaOPSL(ProducaoModel opModel)
        {
            ServiceLayerUtil sl = new ServiceLayerUtil();
            string retorno = string.Empty;

            // Consulta a OP novamente para validar a quantidade realizada antes do fechamento da OP
            var newOpModel = await sl.GetByIDAsync<OrdemProducaoModel>("ProductionOrders", opModel.DocEntry.ToString());
            if (newOpModel != null && newOpModel.CompletedQuantity == 0)
            {
                retorno = "A Ordem de Produção não pode ser fechada porque a quantidade realizada é zero.";
                return retorno;
            }

            var model = new OrdemProducaoModel();
            model.ProductionOrderStatus = "L";
            retorno = await sl.PatchAsync<OrdemProducaoModel>("ProductionOrders", opModel.DocEntry.ToString(), model);
            if (!String.IsNullOrEmpty(retorno))
            {
                retorno = "Erro ao fechar Ordem de Produção: " + retorno;
            }
            return retorno;
        }

        public async Task<string> FechaOPDI(ProducaoModel opModel)
        {
            ProductionOrders op = SBOConnectionBLL.Company.GetBusinessObject(BoObjectTypes.oProductionOrders) as ProductionOrders;
            try
            {
                op.GetByKey(opModel.DocEntry);
                op.ProductionOrderStatus = BoProductionOrderStatusEnum.boposClosed;
                if (op.Update() != 0)
                {
                    throw new Exception(SBOConnectionBLL.Company.GetLastErrorDescription());
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return "Erro ao fechar Ordem de Produção: " + ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(op);
                op = null;
            }
        }

        public bool IsLastResource(int opDocEntry, int currentResourceLineNum, int stageId = -1)
        {
            string command = string.Format(Commands.Resource.GetString("OP_GetUltimoRecurso"), BaseBLL.Database, opDocEntry, stageId);
            object retorno = DAO.ExecuteScalar(command);
            int lastResource = Convert.ToInt32(retorno);
            return lastResource == currentResourceLineNum;
        }

        public List<ItemOPModel> GetItensEstoque(int opDocEntry, int stageId = -1)
        {
            string command = string.Format(Commands.Resource.GetString("OP_GetItensEstoque"), BaseBLL.Database, opDocEntry, stageId);
            return DAO.FillListFromCommand<ItemOPModel>(command);
        }

        public List<ItemOPModel> GetItensEstoqueRecursos(int opDocEntry, int stageId)
        {
            string command = string.Format(Commands.Resource.GetString("OP_GetItensEstoqueRecursos"), BaseBLL.Database, opDocEntry, stageId);
            return DAO.FillListFromCommand<ItemOPModel>(command);
        }

        public List<ProducaoModel> GetListRecursosByUsuario(string usuario)
        {
            string sql = Commands.Resource.GetString("OP_GetListRecursosByUsuario");
            sql = String.Format(sql, BaseBLL.Database, usuario);
            return DAO.FillListFromCommand<ProducaoModel>(sql);
        }

        public List<ProducaoModel> GetApontamentosQuantidadeInferior(int opDocEntry, double quantidade)
        {
            string sql = Commands.Resource.GetString("Etapa_ValidacaoQuantidadeApontada");
            sql = String.Format(sql, BaseBLL.Database, opDocEntry, Convert.ToString(quantidade, CultureInfo.InvariantCulture));
            return DAO.FillListFromCommand<ProducaoModel>(sql);
        }
    }
}
