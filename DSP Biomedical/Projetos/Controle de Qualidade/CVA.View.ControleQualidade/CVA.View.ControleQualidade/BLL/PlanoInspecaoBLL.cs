using CVA.View.ControleQualidade.DAO;
using CVA.View.ControleQualidade.MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.BLL
{
    public partial class PlanoInspecaoBLL
    {
        private PlanoInspecaoDAO _planoInspecaoDAO { get; set; }
        private PlanoInspecao _planoInspecao { get; set; }
    }

    public partial class PlanoInspecaoBLL
    {
        public PlanoInspecaoBLL(PlanoInspecaoDAO planoInspecaoDAO)
        {
            _planoInspecaoDAO = planoInspecaoDAO;
        }

        public string GetNextCode()
        {
            try
            {
                return _planoInspecaoDAO.GetMaxCode_Quality1();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetNexCodeLine()
        {
            return _planoInspecaoDAO.GetMaxCode_Quality1();
        }

        public bool VerifyExistence_QualityLine(string code)
        {
            return _planoInspecaoDAO.VerifyExistence_Quantiy1(code);
        }

        public PlanoInspecao GetHeader(string code)
        {
            var planoInspecao = _planoInspecaoDAO.GetPlano(code);
            return planoInspecao;
        }

        public PlanoInspecao Get(string code, TipoInspecaoEnum tipoInspecao)
        {
            try
            {
                var planoInspecao = _planoInspecaoDAO.GetPlano(code);
                if (planoInspecao != null)
                {
                    planoInspecao.Details = _planoInspecaoDAO.GetPlanoInspecaoLinha(code, ((char)tipoInspecao).ToString());
                }

                return planoInspecao;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DuplicarPlanoModel GetPlano(string code)
        {
            try
            {
                var result = _planoInspecaoDAO.RetornaPlano(code);

                if (result != null)
                {
                    result.Linha = _planoInspecaoDAO.RetornaPlanoLinha(code);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}