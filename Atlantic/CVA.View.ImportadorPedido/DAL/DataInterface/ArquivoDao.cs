using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using MODEL;
using DAL.Connection;
using DAL.Data;
using System.Runtime.InteropServices;
using System.IO;

namespace DAL.DataInterface
{
    public class ArquivoDao
    {
        #region Insert
        public void Insert(Arquivo model)
        {
            var oCompanyService = ConnectionDao.Instance.Company.GetCompanyService();
            var oGeneralService = oCompanyService.GetGeneralService("CVA_IMP_PED");
            var oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

            oGeneralData.SetProperty("Code", HelperDao.GetNextCode("CVA_IMP_PED").ToString().PadLeft(8, '0'));
            oGeneralData.SetProperty("U_ARQUIVO", Path.GetFileName(model.NOMEARQUIVO));
            oGeneralData.SetProperty("U_DATA", model.DATAIMPORTACAO);
            oGeneralData.SetProperty("U_STATUS", model.STATUSARQUIVO);
            //oGeneralData.SetProperty("U_LOG", model.MENSAGEMSTATUS);
            oGeneralData.SetProperty("U_LINHAS", model.QTDLINHAS);

            if (model.LINHAS.Count > 0)
            {
                var oChildren = oGeneralData.Child("CVA_IMP_PED1");
                foreach (var linha in model.LINHAS)
                {
                    var oChild = oChildren.Add();
                    oChild.SetProperty("U_CNPJ", linha.CNPJATLANTIC);
                    oChild.SetProperty("U_EMPRESA", linha.EMPRESA);
                    oChild.SetProperty("U_BASE", linha.BASE);
                    oChild.SetProperty("U_STATUS", linha.STATUSLINHA);
                    oChild.SetProperty("U_LINHA", linha.LINHA);
                    oChild.SetProperty("U_LOG", linha.MENSAGEMSTATUS);
                    oChild.SetProperty("U_PN", linha.PN);
                    oChild.SetProperty("U_LCTO", linha.DATALANCAMENTO.Value);
                    oChild.SetProperty("U_ITEM", linha.ITEM);
                    oChild.SetProperty("U_QTD", linha.QUANTIDADE);
                    oChild.SetProperty("U_VALOR", linha.VALOR);
                    if (!String.IsNullOrEmpty(linha.NUMERONOTA))
                    {
                        oChild.SetProperty("U_NUM", linha.NUMERONOTA);
                    }
                    if (linha.NUMEROPEDIDOSAP.HasValue)
                    {
                        oChild.SetProperty("U_NUMSAP", linha.NUMEROPEDIDOSAP.ToString());
                    }

                    //oChild.SetProperty("U_COND", linha.CONDICAOPAGAMENTO);
                    //oChild.SetProperty("U_DTST", linha.DATASTATUS);
                    //oChild.SetProperty("U_VCTO", linha.DATAVENCIMENTO);
                    //oChild.SetProperty("U_DIM1", linha.DIMENSAO01);
                    //oChild.SetProperty("U_DIM2", linha.DIMENSAO02);
                    //oChild.SetProperty("U_DIM3", linha.DIMENSAO03);
                    //oChild.SetProperty("U_DIM4", linha.DIMENSAO04);
                    //oChild.SetProperty("U_DIM5", linha.DIMENSAO05);
                    //oChild.SetProperty("U_FORMA", linha.FORMAPAGAMENTO);
                    //oChild.SetProperty("U_MODEL", linha.MODELO);
                    //oChild.SetProperty("U_OBS", linha.OBSERVACAO);
                    //oChild.SetProperty("U_PRJ", linha.PROJETO);
                    //oChild.SetProperty("U_SERIE", linha.SERIENOTA);
                    //oChild.SetProperty("U_UTIL", linha.UTILIZACAO);
                }

                Marshal.ReleaseComObject(oChildren);
                oChildren = null;
            }

            oGeneralService.Add(oGeneralData);

            Marshal.ReleaseComObject(oCompanyService);
            Marshal.ReleaseComObject(oGeneralService);
            Marshal.ReleaseComObject(oGeneralData);

            oCompanyService = null;
            oGeneralService = null;
            oGeneralData = null;
        }
        #endregion

        #region InsertHeader
        public void InsertHeader(Arquivo model)
        {
            var oCompanyService = ConnectionDao.Instance.Company.GetCompanyService();
            var oGeneralService = oCompanyService.GetGeneralService("CVA_IMP_PED");
            var oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

            oGeneralData.SetProperty("Code", HelperDao.GetNextCode("CVA_IMP_PED").ToString().PadLeft(8, '0'));
            oGeneralData.SetProperty("U_ARQUIVO", Path.GetFileName(model.NOMEARQUIVO));
            oGeneralData.SetProperty("U_DATA", model.DATAIMPORTACAO);
            oGeneralData.SetProperty("U_STATUS", model.STATUSARQUIVO);
            //oGeneralData.SetProperty("U_LOG", model.MENSAGEMSTATUS);
            oGeneralData.SetProperty("U_LINHAS", model.QTDLINHAS);

            oGeneralService.Add(oGeneralData);

            Marshal.ReleaseComObject(oCompanyService);
            Marshal.ReleaseComObject(oGeneralService);
            Marshal.ReleaseComObject(oGeneralData);

            oCompanyService = null;
            oGeneralService = null;
            oGeneralData = null;
        }
        #endregion


        #region InsertLine
        public void InsertLine(string code, ArquivoLinha linha)
        {
            var oCompanyService = ConnectionDao.Instance.Company.GetCompanyService();
            var oGeneralService = oCompanyService.GetGeneralService("CVA_IMP_PED");

            var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
            oGeneralParams.SetProperty("Code", code);

            var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
            var oChildren = oGeneralData.Child("CVA_IMPPED1");

            var oChild = oChildren.Add();
            if (!String.IsNullOrEmpty(linha.CNPJATLANTIC))
            {
                oChild.SetProperty("U_CNPJ", linha.CNPJATLANTIC);
            }
            if (!String.IsNullOrEmpty(linha.EMPRESA))
            {
                oChild.SetProperty("U_EMPRESA", linha.EMPRESA);
            }
            if (!String.IsNullOrEmpty(linha.BASE))
            {
                oChild.SetProperty("U_BASE", linha.BASE);
            }
            oChild.SetProperty("U_STATUS", linha.STATUSLINHA);
            oChild.SetProperty("U_LINHA", linha.LINHA);
            if (!String.IsNullOrEmpty(linha.MENSAGEMSTATUS))
            {
                oChild.SetProperty("U_LOG", linha.MENSAGEMSTATUS);
            }
            if (!String.IsNullOrEmpty(linha.PN))
            {
                oChild.SetProperty("U_PN", linha.PN);
            }
            oChild.SetProperty("U_LCTO", linha.DATALANCAMENTO.Value);
            if (!String.IsNullOrEmpty(linha.ITEM))
            {
                oChild.SetProperty("U_ITEM", linha.ITEM);
            }
            oChild.SetProperty("U_QTD", linha.QUANTIDADE);
            oChild.SetProperty("U_VALOR", linha.VALOR);
            if (!String.IsNullOrEmpty(linha.NUMERONOTA))
            {
                oChild.SetProperty("U_NUM", linha.NUMERONOTA);
            }
            if (linha.NUMEROPEDIDOSAP.HasValue)
            {
                oChild.SetProperty("U_NUMSAP", linha.NUMEROPEDIDOSAP.ToString());
            }

            Marshal.ReleaseComObject(oChildren);
            oChildren = null;

            oGeneralService.Update(oGeneralData);

            Marshal.ReleaseComObject(oCompanyService);
            Marshal.ReleaseComObject(oGeneralService);
            Marshal.ReleaseComObject(oGeneralData);

            oCompanyService = null;
            oGeneralService = null;
            oGeneralData = null;
        }
        #endregion

        #region Update
        public void Update(Arquivo model)
        {
            Recordset rst = ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            foreach (var linha in model.LINHAS)
            {
                string sql = String.Format(Query.ImpPedido_Update, linha.STATUSLINHA, linha.MENSAGEMSTATUS.Replace("'", ""), linha.CODE, linha.LINHA);
                rst.DoQuery(sql);
            }
            Marshal.ReleaseComObject(rst);
            rst = null;
        }
        #endregion

        #region Get
        public List<Arquivo> Get(string filename, int status)
        {
            var lst = new List<Arquivo>();
            var oRecordset = (Recordset)ConnectionDao.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery($"SELECT DISTINCT Code FROM [@CVA_IMPPED] WHERE U_ARQUIVO = '{filename}' AND U_STATUS = {status}");

            while (!oRecordset.EoF)
            {
                var arquivo = new Arquivo();
                var oCompanyService = ConnectionDao.Instance.Company.GetCompanyService();
                var oGeneralService = oCompanyService.GetGeneralService("CVA_IMPPED");
                var oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                oGeneralParams.SetProperty("Code", oRecordset.Fields.Item("Code").Value.ToString());

                var oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                var oChildren = oGeneralData.Child("CVA_IMPPED1");

                //arquivo.DATAIMPORTACAO = oGeneralData.GetProperty("U_DATA");
                //arquivo.MENSAGEMSTATUS = oGeneralData.GetProperty("U_LOG");
                //arquivo.STATUSARQUIVO = oGeneralData.GetProperty("U_STATUS");
                //arquivo.NOMEARQUIVO = oGeneralData.GetProperty("U_ARQUIVO");
                //arquivo.QTDLINHAS = oGeneralData.GetProperty("U_LINHAS");

                for (var i = 0; i < oChildren.Count; i++)
                {
                    var oChild = oChildren.Item(i);
                    var linha = new ArquivoLinha();

                    //linha.CNPJATLANTIC = oChild.GetProperty("U_CNPJ");
                    //linha.CONDICAOPAGAMENTO = oChild.GetProperty("U_COND");
                    //linha.DATALANCAMENTO = oChild.GetProperty("U_LCTO");
                    //linha.DATASTATUS = oChild.GetProperty("U_DTST");
                    //linha.DATAVENCIMENTO = oChild.GetProperty("U_VCTO");
                    //linha.DIMENSAO01 = oChild.GetProperty("U_DIM1");
                    //linha.DIMENSAO02 = oChild.GetProperty("U_DIM2");
                    //linha.DIMENSAO03 = oChild.GetProperty("U_DIM3");
                    //linha.DIMENSAO04 = oChild.GetProperty("U_DIM4");
                    //linha.DIMENSAO05 = oChild.GetProperty("U_DIM5");
                    //linha.FORMAPAGAMENTO = oChild.GetProperty("U_FORMA");
                    //linha.ITEM = oChild.GetProperty("U_ITEM");
                    //linha.LINHA = oChild.GetProperty("U_LINHA");
                    //linha.MENSAGEMSTATUS = oChild.GetProperty("U_LOG");
                    //linha.MODELO = oChild.GetProperty("U_MODEL");
                    //linha.NUMERONOTA = oChild.GetProperty("U_NUM");
                    //linha.NUMEROPEDIDOSAP = oChild.GetProperty("U_NUMSAP");
                    //linha.OBSERVACAO = oChild.GetProperty("U_OBS");
                    //linha.PN = oChild.GetProperty("U_PN");
                    //linha.PROJETO = oChild.GetProperty("U_PRJ");
                    //linha.QUANTIDADE = oChild.GetProperty("U_QTD");
                    //linha.SERIENOTA = oChild.GetProperty("U_SERIE");
                    //linha.STATUSLINHA = oChild.GetProperty("U_STATUS");
                    //linha.UTILIZACAO = oChild.GetProperty("U_UTIL");
                    //linha.VALOR = oChild.GetProperty("U_VALOR");

                    arquivo.LINHAS.Add(linha);
                }

                lst.Add(arquivo);
                oRecordset.MoveNext();
            }

            return lst;
        }
        #endregion
    }
}
