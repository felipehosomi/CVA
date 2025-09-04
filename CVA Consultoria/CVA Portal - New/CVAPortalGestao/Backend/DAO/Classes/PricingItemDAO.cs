using AUXILIAR;

using DAO.Resources;
using MODEL.Classes;
using System;
using System.Data;

namespace DAO.Classes
{
    public class PricingItemDAO
    {
        public DataTable Get(int id)
        {
            throw new NotImplementedException();
        }

        public DataTable Get_All()
        {
            throw new NotImplementedException();
        }

        public DataTable Get_PricingItens(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.PricingItem_Get_PricingItens);
                    conn.InsertParameter("ID_PRI", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public DataTable Get_PricingItens_oprt(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.PricingItem_Get_PricingItens_oprt);
                    conn.InsertParameter("ID_PRI", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Insert(PricingItemModel model)
        {
            throw new NotImplementedException();
        }

        public int Insert_PricingItem(PricingModel model, PricingItemModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.PricingItem_Insert_PricingItem);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_PRI", model.Id);
                    conn.InsertParameter("ID_ESP", item.Especialidade.Id);
                    conn.InsertParameter("COL", item.Colaborador);

                    conn.InsertParameter("HRS", item.EspecialidadeHoras);
                    conn.InsertParameter("VAL", item.EspecialidadeValor);
                 
                    conn.InsertParameter("VAL_BKO", item.ValorBackoffice);
                    conn.InsertParameter("VAL_RSC", item.ValorRisco);
                    conn.InsertParameter("VAL_MRG", item.ValorMargem);
                    conn.InsertParameter("VAL_COM", item.ValorComissao);
                    conn.InsertParameter("VAL_IMP", item.ValorImposto);

                    conn.InsertParameter("HOT_DIA", item.HotelDiarias);
                    conn.InsertParameter("HOT_VAL", item.HotelValor);
                    conn.InsertParameter("HOT_TOT", item.HotelTotal);

                    conn.InsertParameter("KLM_TRE", item.KmTrechos);
                    conn.InsertParameter("KLM_DIS", item.KmDistancia);
                    conn.InsertParameter("KLM_VAL", item.KmValor);
                    conn.InsertParameter("KLM_TOT", item.KmTotal);

                    conn.InsertParameter("ALI_DIA", item.AlimentacaoDias);
                    conn.InsertParameter("ALI_VAL", item.AlimentacaoValor);
                    conn.InsertParameter("ALI_TOT", item.AlimentacaoTotal);

                    conn.InsertParameter("DSL_HRS", item.DeslocamentoHoras);
                    conn.InsertParameter("DSL_VAL", item.DeslocamentoValor);
                    conn.InsertParameter("DSL_TOT", item.DeslocamentoTotal);

                    conn.InsertParameter("AER_TRE", item.AereoTrechos);
                    conn.InsertParameter("AER_VAL", item.AereoValor);
                    conn.InsertParameter("AER_TOT", item.AereoTotal);

                    conn.InsertParameter("SUB_TOT", item.RecursoSubTotal);
                    conn.InsertParameter("DSP_TOT", item.RecursoDespesas);
                    conn.InsertParameter("SUB_DSP", item.RecursoValorComDespesas);
                    conn.InsertParameter("DSP_IMP", item.RecursoValorComImpostos);
                    conn.InsertParameter("VAL_HRS", item.RecursoValorHorasSemDespesa);
                    conn.InsertParameter("VAL_HRS_DSP", item.RecursoValorHorasComDespesa);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Remove(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.PricingItem_Remove);
                    conn.InsertParameter("ID_PRI", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(PricingItemModel model)
        {
            throw new NotImplementedException();
        }

        public int Opportunitty_Insert_PricingItem(PricingModel model, PricingItemModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.PricingItem_Opportunitty_Insert_PricingItem);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_PRI", model.Id);
                    conn.InsertParameter("ID_ESP", item.Especialidade.Id);
                    
                    conn.InsertParameter("HRS", item.EspecialidadeHoras);
                    conn.InsertParameter("VAL", item.EspecialidadeValor);
                    conn.InsertParameter("CST", item.EspecialidadeCusto);

                    conn.InsertParameter("VAL_BKO", item.ValorBackoffice);
                    conn.InsertParameter("VAL_RSC", item.ValorRisco);
                    conn.InsertParameter("VAL_MRG", item.ValorMargem);
                    conn.InsertParameter("VAL_COM", item.ValorComissao);
                    conn.InsertParameter("VAL_IMP", item.ValorImposto);

                    conn.InsertParameter("HOT_DIA", item.HotelDiarias);
                    conn.InsertParameter("HOT_VAL", item.HotelValor);
                    conn.InsertParameter("HOT_TOT", item.HotelTotal);

                    conn.InsertParameter("KLM_TRE", item.KmTrechos);
                    conn.InsertParameter("KLM_DIS", item.KmDistancia);
                    conn.InsertParameter("KLM_VAL", item.KmValor);
                    conn.InsertParameter("KLM_TOT", item.KmTotal);

                    conn.InsertParameter("ALI_DIA", item.AlimentacaoDias);
                    conn.InsertParameter("ALI_VAL", item.AlimentacaoValor);
                    conn.InsertParameter("ALI_TOT", item.AlimentacaoTotal);

                    conn.InsertParameter("DSL_HRS", item.DeslocamentoHoras);
                    conn.InsertParameter("DSL_VAL", item.DeslocamentoValor);
                    conn.InsertParameter("DSL_TOT", item.DeslocamentoTotal);

                    conn.InsertParameter("AER_TRE", item.AereoTrechos);
                    conn.InsertParameter("AER_VAL", item.AereoValor);
                    conn.InsertParameter("AER_TOT", item.AereoTotal);

                    conn.InsertParameter("SUB_TOT", item.RecursoSubTotal);
                    conn.InsertParameter("DSP_TOT", item.RecursoDespesas);
                    conn.InsertParameter("SUB_DSP", item.RecursoValorComDespesas);
                    conn.InsertParameter("DSP_IMP", item.RecursoValorComImpostos);
                    conn.InsertParameter("VAL_HRS", item.RecursoValorHorasSemDespesa);
                    conn.InsertParameter("VAL_HRS_DSP", item.RecursoValorHorasComDespesa);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Remove_Oprt(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.PricingItem_Remove_oprt);
                    conn.InsertParameter("ID_PRI", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
