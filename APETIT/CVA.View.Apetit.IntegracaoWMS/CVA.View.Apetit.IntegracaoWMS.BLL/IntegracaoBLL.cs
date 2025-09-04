using Newtonsoft.Json;
using System;
using System.Linq;

namespace CVA.View.Apetit.IntegracaoWMS.BLL
{
    public class IntegracaoBLL
    {
        public void ProcessoDocumentoCompra(string table, string tableLine, string objType)
        {
            try
            {
                //Helper.LogInfo("Recuperando registros para sincronização app...");

                var integradorBo = new IntegradorBO();

                var lstCriadas = integradorBo.ObterEntradasIntegrador(table, tableLine, objType);

                foreach (var item in lstCriadas.GroupBy(x=>x.DocEntry))
                {
                    try
                    {
                        //Helper.LogInfo("ProcessoAlteraPrevisaoEntrega Pegasus |DocEntry| = " + item.DocEntry);

                        integradorBo.InsereDocumentoNF(item.ToList());
                    }
                    catch (Exception ex) 
                    {
                        var mess = $"01 - {ex.Message}";
                        integradorBo.AtualizarEntradaIntegrado(item.FirstOrDefault().DocEntry.ToString(), TipoIntegrado.Erro, mess);
                        Helper.LogInfo("Erro... -> " + mess);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogInfo("ProcessoDocumentoCompra - Erro... - " + JsonConvert.SerializeObject(ex));
            }
        }
    }
}
