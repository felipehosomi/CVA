using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Producao;
using System;

namespace CVA.Portal.Producao.Console
{
    class Program
    {
        private static ProducaoBLL producaoBLL = new ProducaoBLL();
        private static EntradaAcabadoBLL entradaBLL = new EntradaAcabadoBLL();
        private static HanaDAO dao = producaoBLL.DAO as HanaDAO;
        private static Logger logger = new Logger();

        static void Main(string[] args)
        {
            try
            {
                logger.Log("Iniciando serviço de verificação de OPs pendentes...");
                logger.Log($"Banco de dados: {HanaDAO.Database}");
                Commands.SetResourceManager();

                System.Console.WriteLine();
                VerificarOPsSemAcabado();

                System.Console.WriteLine();
                VerificarOPsAbertas();
            }
            catch (Exception ex)
            {
                logger.Log(ex.ToString());
            }
            finally
            {
                System.Console.WriteLine();
                logger.Log("Processo finalizado. Fechando em 10 segundos...");
                logger.Save();
                System.Threading.Thread.Sleep(10000);
            }
        }

        private static void VerificarOPsSemAcabado()
        {
            try
            {
                logger.Log("Verificando a existência de OPs sem entrada de acabado...");
                string query = string.Format(Commands.Resource.GetString("OP_GetOPSemAcabado"), HanaDAO.Database);
                var opList = dao.FillListFromCommand<ProducaoModel>(query);

                if (opList != null && opList.Count > 0)
                {
                    logger.Log($"{opList.Count} OPs encontradas.");
                    foreach (var opInfo in opList)
                    {
                        var op = producaoBLL.GetDadosOP(opInfo.NrOP, opInfo.CodEtapa.ToString());

                        if (op != null && op.NrOP > 0)
                        {
                            logger.Log($"Lançando a entrada de acabado da OP {op.NrOP}...");
                            string result = entradaBLL.GeraEntradaAcabadoDI(op).Result;

                            if (string.IsNullOrEmpty(result))
                                logger.Log("OK!");
                            else
                                logger.Log(result);
                        }
                    }
                }
                else
                {
                    logger.Log("Nenhuma OP encontrada.");
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.ToString());
            }
        }

        private static void VerificarOPsAbertas()
        {
            try
            {
                logger.Log("Verificando a existência de OPs com pendência de fechamento...");
                string query = string.Format(Commands.Resource.GetString("OP_GetTodasEtapasFechadasOPAberta"), HanaDAO.Database);
                var opList = dao.FillListFromCommand<ProducaoModel>(query);

                if (opList != null && opList.Count > 0)
                {
                    logger.Log($"{opList.Count} OPs encontradas.");
                    foreach (var opInfo in opList)
                    {
                        if (opInfo != null && opInfo.NrOP > 0)
                        {
                            logger.Log($"Fechando a OP {opInfo.NrOP}...");
                            string result = producaoBLL.FechaOPDI(opInfo).Result;

                            if (string.IsNullOrEmpty(result))
                                logger.Log("OK!");
                            else
                                logger.Log(result);
                        }
                    }
                }
                else
                {
                    logger.Log("Nenhuma OP encontrada.");
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.ToString());
            }
        }
    }
}
