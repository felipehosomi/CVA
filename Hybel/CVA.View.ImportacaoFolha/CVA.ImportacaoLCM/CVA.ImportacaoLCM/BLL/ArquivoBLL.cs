using CVA.ImportacaoLCM.Model;
using CVA.ImportacaoLCM.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVA.ImportacaoLCM.DAO;
using System.Reflection;

namespace CVA.ImportacaoLCM.BLL
{
    class ArquivoBLL
    {
        //private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        
        
        public List<DetalheModel> Importar(string caminho)
        {
            var Log = new LogErro();

            List<DetalheModel> listDt = new List<DetalheModel>();
           
            try
            {
               

                //StreamReader texto = new StreamReader(caminho);
                //Stream entrada = File.Open(caminho, FileMode.Open);
                string[] lines = System.IO.File.ReadAllLines(caminho);

                if(lines.First().Length > 0)
                {

                    foreach (var item in lines)
                    {
                        if(item.Length == 871)
                        {
                           
                            DetalheModel dt = new DetalheModel()
                            {
                                NomedoArquivo = item.ExtrairValorDaLinha(1, 60),
                                NomeDaEmpresa = item.ExtrairValorDaLinha(61, 120),
                                CodigoDaEmpresa = item.ExtrairValorDaLinha(121, 127),
                                CNPJdaEmpresa = item.ExtrairValorDaLinha(128, 141),
                                CodigoDaFilial = item.ExtrairValorDaLinha(142, 148),
                                NSequencial = item.ExtrairValorDaLinha(149, 155),
                                NSeqnoDetalhe = item.ExtrairValorDaLinha(156, 162),
                                DatadoLancto = item.ExtrairValorDaLinha(163, 172),
                                ContaDebito = item.ExtrairValorDaLinha(173, 179),
                                ContaCredito = item.ExtrairValorDaLinha(180, 186),
                                ClassifDebito = item.ExtrairValorDaLinha(187, 206),
                                ClassifCredito = item.ExtrairValorDaLinha(207, 226),
                                ValorDoLancto = item.ExtrairValorDaLinha(227, 241),
                                CodigoDoHistorico = item.ExtrairValorDaLinha(242, 248),
                                Historico = item.ExtrairValorDaLinha(249, 760),
                                NomedoUsuario = item.ExtrairValorDaLinha(761, 790),
                                CodigoDoSeparador = item.ExtrairValorDaLinha(791, 797),
                                NomeDoSeparador = item.ExtrairValorDaLinha(798, 817),
                                DescricaoLancto = item.ExtrairValorDaLinha(818, 857),
                                CentroCustoDebit = item.ExtrairValorDaLinha(858, 864),
                                CentroCustoCredit = item.ExtrairValorDaLinha(865, 871)

                            };

                            listDt.Add(dt);

                        }
                        

                    }
                }

                return listDt;
            }
            catch (Exception ex)
            {
               
                Log.Log("Erro Fatal: " + ex.Message + "Data Log: " + DateTime.Now);
                return listDt;
            }

        }

        

    }
}
