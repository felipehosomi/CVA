using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using CVA.Cointer.Megasul.API.Models.ServiceLayer;
using SBO.Hub.SBOHelpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CVA.Cointer.Megasul.API.BLL
{
    public class ContasReceberBLL
    {
        public void AddIncomingPayment(NotaFiscalModel notaFiscalModel, int docEntry, DateTime docDate, int bplId)
        {
            ServiceLayer sl = new ServiceLayer();
            HanaDAO dao = new HanaDAO();

            foreach (var pagamento in notaFiscalModel.pagamentos.Where(m => !m.troco && m.codigo_sap != "VOU"))
            {
                try
                {
                    ContasReceberModel contasReceber = new ContasReceberModel();
                    contasReceber.DocDate = docDate;
                    contasReceber.BPLID = bplId;
                    contasReceber.CardCode = notaFiscalModel.cliente;
                    contasReceber.DueDate = docDate;
                    if (pagamento.codigo_sap.Contains("CR-"))
                    {
                        Paymentcreditcard paymentcreditcard = new Paymentcreditcard();
                        object creditCardId = dao.ExecuteScalar(String.Format(Hana.CreditCard_GetId, ConfigurationManager.AppSettings[$"CartaoCredito_{bplId}"]));
                        if (creditCardId == null)
                        {
                            throw new Exception($"ID do cartão de crédito não encontrado. Verifique o web.config (CartaoCredito_X)");
                        }
                        object creditCardTypeId = dao.ExecuteScalar(String.Format(Hana.CreditCardType_GetId, ConfigurationManager.AppSettings[$"CartaoCreditoFormaPgto_{bplId}"]));
                        if (creditCardTypeId == null)
                        {
                            throw new Exception($"Forma de pgto do cartão de crédito não encontrado. Verifique o web.config (CartaoCreditoFormaPgto_X)");
                        }

                        paymentcreditcard.CreditCardNumber = "9999";
                        paymentcreditcard.AdditionalPaymentSum = 0;
                        paymentcreditcard.CardValidUntil = DateTime.Today;

                        paymentcreditcard.CreditCard = Convert.ToInt32(creditCardId);
                        paymentcreditcard.PaymentMethodCode = Convert.ToInt32(creditCardTypeId);

                        paymentcreditcard.CreditType = "cr_Regular";
                        paymentcreditcard.CreditSum = pagamento.valor;
                        paymentcreditcard.FirstPaymentDue = DateTime.Today;

                        if (pagamento.cartoes != null && pagamento.cartoes.Length > 0)
                        {
                            paymentcreditcard.VoucherNum = pagamento.cartoes[0].autorizacao;
                            paymentcreditcard.NumOfPayments = pagamento.cartoes[0].qtde_parcela;
                        }
                        else
                        {
                            paymentcreditcard.VoucherNum = docEntry.ToString();
                            paymentcreditcard.NumOfPayments = 1;
                        }

                        contasReceber.PaymentCreditCards = new List<Paymentcreditcard>();
                        contasReceber.PaymentCreditCards.Add(paymentcreditcard);
                    }
                    else if (pagamento.codigo_sap.ToUpper() == "DIN")
                    {
                        contasReceber.CashAccount = ConfigurationManager.AppSettings[$"ContaDinheiro_{bplId}"];
                        contasReceber.CashSum = pagamento.valor + notaFiscalModel.pagamentos.Where(m => m.troco && m.codigo_sap == pagamento.codigo_sap).Sum(m => m.valor);
                    }
                    else if (pagamento.codigo_sap.ToUpper() == "CHEQUE")
                    {
                        contasReceber.TransferAccount = ConfigurationManager.AppSettings[$"ContaCheque_{bplId}"];
                        contasReceber.TransferSum = pagamento.valor;
                        contasReceber.TransferDate = contasReceber.DocDate;
                    }
                    else
                    {
                        object boeOrTransf = dao.ExecuteScalar(String.Format(Hana.FormaPagamento_GetByCode, pagamento.codigo_sap));
                        if (boeOrTransf == null)
                        {
                            throw new Exception($"Forma de pagamento {pagamento.codigo_sap} não encontrada");
                        }
                        if (boeOrTransf.ToString() == "B")
                        {
                            contasReceber.BillOfExchange = new BillOfExchange();
                            contasReceber.BillOfExchange.PaymentMethodCode = pagamento.codigo_sap;
                            contasReceber.BillOfExchangeAmount = pagamento.valor;
                        }
                        else
                        {
                            contasReceber.TransferAccount = ConfigurationManager.AppSettings[$"ContaTransferencia_{bplId}"];
                            contasReceber.TransferSum = pagamento.valor;
                            contasReceber.TransferDate = contasReceber.DocDate;
                        }
                    }

                    Paymentinvoice paymentinvoice = new Paymentinvoice();
                    paymentinvoice.DocEntry = docEntry;
                    paymentinvoice.InstallmentId = 0;
                    paymentinvoice.InvoiceType = "it_Invoice";

                    contasReceber.PaymentInvoices = new List<Paymentinvoice>();
                    contasReceber.PaymentInvoices.Add(paymentinvoice);

                    contasReceber = sl.PostAndGetAdded< ContasReceberModel>("IncomingPayments", "DocEntry", contasReceber);
                    List<LcmLineModel> lcmLines = dao.FillListFromCommand<LcmLineModel>(String.Format(Hana.ContasReceber_GetLcmToFix, contasReceber.DocEntry));

                    if (lcmLines.Count > 0)
                    {
                        LcmModel lcmModel = new LcmModel();
                        lcmModel.JournalEntryLines = lcmLines;
                        string error = sl.Patch<LcmModel>("JournalEntries", lcmLines[0].TransId, lcmModel);
                        if (!String.IsNullOrEmpty(error))
                        {
                            throw new Exception("Erro ao atualizar Ref3 no LCM: " + error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!String.IsNullOrEmpty(sl.LastError))
                    {
                        throw new Exception("Erro ao gerar contas a receber: " + sl.LastError);
                    }
                    else
                    {
                        throw new Exception("Erro ao gerar contas a receber: " + ex.Message);
                    }
                }
            }
        }
    }
}