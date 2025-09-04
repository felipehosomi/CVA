using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model.Estoque;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Estoque
{
    public class TransferenciaEstoqueBLL : BaseBLL
    {
        public TransferenciaEstoqueBLL()
        {
        }

        public async Task<string> ExecutarTransferencia(List<ProducaoModel> model, string usuario)
        {
            string logValidation = string.Empty;
            var logBLL = new TransferenciaLogBLL();
            var sl = new ServiceLayerUtil();
            await sl.Login();

            var ops = model.GroupBy(x => new { x.NrOP, x.Etapa, x.CodEtapa }).ToList();
            var document = new TransferenciaEstoqueModel();
            document.DocObjectCode = "1250000001";
            document.StockTransferLines = new List<LinhaTransferenciaEstoqueModel>();
            string opDetails = string.Empty;

            foreach (var item in ops)
            {
                var transfer = logBLL.GetValidTransfer(item.Key.NrOP, item.Key.CodEtapa);
                if (transfer != null && !string.IsNullOrEmpty(transfer.Code))
                    logValidation += $"Já existe uma transferência realizada para a OP {item.Key.NrOP} e etapa {item.Key.Etapa}.\n";

                opDetails += $"OP {item.Key.NrOP}, etapa {item.Key.Etapa}\n";
            }

            if (!string.IsNullOrEmpty(logValidation))
                throw new Exception(logValidation);

            document.U_CVA_ObsPortal = $"Solicitação de transferência gerada no portal pelo usuário {usuario}.\n\nDetalhes:\n\n{opDetails}";
            var listaItens = new List<ItemOPModel>();

            foreach (var op in model)
            {
                foreach (var item in op.Itens)
                {
                    item.Etapa = op.Etapa;
                    listaItens.Add(item);
                }
            }

            var itensAgrupados = listaItens
                .GroupBy(x => new { x.ItemCode, x.DefaultWareHouse, x.wareHouse, x.Etapa })
                .Select(x => new { ItemCode = x.Key.ItemCode, DefaultWareHouse = x.Key.DefaultWareHouse, wareHouse = x.Key.wareHouse, Etapa = x.Key.Etapa, PlannedQty = x.Sum(s => s.PlannedQty) });

            foreach (var item in itensAgrupados)
            {
                var line = new LinhaTransferenciaEstoqueModel();
                line.ItemCode = item.ItemCode;
                line.FromWarehouseCode = item.DefaultWareHouse;
                line.WarehouseCode = item.wareHouse;
                line.Quantity = item.PlannedQty;
                line.U_CVA_Etapa = item.Etapa;
                document.StockTransferLines.Add(line);
            }

            var retorno = await sl.PostAndReturnEntryAsync("InventoryTransferRequests", document);

            if (!string.IsNullOrEmpty(retorno.Item2))
            {
                throw new Exception(retorno.Item2);
            }

            if (retorno.Item1 > 0)
            {
                foreach (var item in ops)
                {
                    var transfer = new TransferenciaLogModel();
                    transfer.OPDocNum = item.Key.NrOP;
                    transfer.StageId = item.Key.CodEtapa;
                    transfer.TQDocEntry = retorno.Item1;
                    logBLL.Create(transfer);
                }
            }

            return retorno.Item2;
        }

        public TransferenciaEstoqueModel GetTransfByOPDocNumStageId(int opDocNum, int stageId)
        {
            var model = DAO.FillModelFromCommand<TransferenciaEstoqueModel>(string.Format(Commands.Resource.GetString("PedidoTransferencia_GetByOPAndStageId"), BaseBLL.Database, opDocNum, stageId));

            if (model != null)
            {
                model.StockTransferLines = DAO.FillListFromCommand<LinhaTransferenciaEstoqueModel>(string.Format(Commands.Resource.GetString("PedidoTransferencia_GetDetailsByOPAndStageId"), BaseBLL.Database, opDocNum, stageId));
            }

            return model;
        }
    }
}
