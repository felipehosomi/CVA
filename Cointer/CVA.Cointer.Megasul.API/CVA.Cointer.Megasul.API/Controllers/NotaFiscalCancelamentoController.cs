using CVA.Cointer.Megasul.API.BLL;
using CVA.Cointer.Megasul.API.Models;
using System;
using System.Web.Http;

namespace CVA.Cointer.Megasul.API.Controllers
{
    public class NotaFiscalCancelamentoController : ApiController
    {

        public NotaFiscalResponseModel Post(NotaFiscalCancelamentoModel model)
        {
            NotaFiscalBLL notaFiscalBLL = new NotaFiscalBLL();
            string error = notaFiscalBLL.Cancel(model);

            NotaFiscalResponseModel notaFiscalResponseModel = new NotaFiscalResponseModel();
            notaFiscalResponseModel.cnpj_filial = model.cnpj_filial;
            notaFiscalResponseModel.identificador = new IdentificadorResponse();
            notaFiscalResponseModel.identificador.ef = model.identificador.ef;
            notaFiscalResponseModel.identificador.si = model.identificador.si;
            notaFiscalResponseModel.identificador.tr = model.identificador.tr;
            notaFiscalResponseModel.resultado = String.IsNullOrEmpty(error);
            notaFiscalResponseModel.mensagem_erro = error;
            return notaFiscalResponseModel;
        }
    }
}
