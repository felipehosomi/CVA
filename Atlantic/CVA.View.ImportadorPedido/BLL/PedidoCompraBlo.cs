using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using DAL.Excel;
using DAL.DataInterface;
using MODEL;
using SAPbobsCOM;
using DAL.Connection;
using System.Collections;
using DAL.Data;
using System.Windows.Forms;

namespace BLL
{
    public class PedidoCompraBlo
    {
        private readonly ExcelFileDao _ExcelFileDao;

        #region ExcelFields
        private static int CNPJATLANTIC = 0;
        private static int PN = 1;
        private static int ITEM = 2;
        private static int QUANTIDADE = 3;
        private static int VALOR = 4;
        private static int DATAVENCIMENTO = 5;
        private static int DATALANCAMENTO = 6;
        private static int UTILIZACAO = 7;
        private static int CONDICAOPAGAMENTO = 8;
        private static int FORMAPAGAMENTO = 9;
        private static int NUMERONOTA = 10;
        private static int SERIENOTA = 11;
        private static int MODELO = 12;
        private static int DIMENSAO01 = 13;
        private static int DIMENSAO02 = 14;
        private static int DIMENSAO03 = 15;
        private static int DIMENSAO04 = 16;
        private static int DIMENSAO05 = 17;
        private static int PROJETO = 18;
        private static int OBSERVACAO = 19;
        private static int IMPOSTORETIDO1 = 20;
        private static int IMPOSTORETIDO2 = 21;
        private static int ANEXO1 = 22;
        private static int ANEXO2 = 23;
        private static int ANEXO3 = 24;
        private static int ANEXO4 = 25;
        private static int ANEXO5 = 26;
        private static int ALIQIPI = 27;
        private static int APLICACAO = 28;
        private static int ALIQST = 29;
        #endregion

        public PedidoCompraBlo()
        {
            _ExcelFileDao = new ExcelFileDao();
        }

        #region Import
        public Arquivo Import(string filePath, SAPbouiCOM.DataTable dt_Imp, SAPbouiCOM.Form form)
        {
            var msg = string.Empty;
            var arquivo = new Arquivo();

            try
            {
                if (!File.Exists(filePath))
                {
                    arquivo.MENSAGEMSTATUS = "Arquivo não encontrado!";
                }

                var dtbExcel = _ExcelFileDao.ReadFile(filePath, ConfigurationManager.AppSettings["ExcelSheet"]);
                arquivo.DATAIMPORTACAO = DateTime.Now;
                arquivo.NOMEARQUIVO = filePath;
                arquivo.QTDLINHAS = dtbExcel.Rows.Count;
                arquivo.STATUSARQUIVO = 0;

                var i = 1;
                foreach (DataRow item in dtbExcel.Rows)
                {
                    if (String.IsNullOrEmpty(item[CNPJATLANTIC].ToString()))
                    {
                        arquivo.QTDLINHAS--;
                        continue;
                    }

                    var arquivoLinha = new ArquivoLinha();
                    arquivoLinha.CNPJATLANTIC = item[CNPJATLANTIC].ToString();
                    arquivoLinha.PN = item[PN].ToString();
                    arquivoLinha.ITEM = item[ITEM].ToString();
                    arquivoLinha.QUANTIDADE = double.Parse(item[QUANTIDADE].ToString());
                    arquivoLinha.VALOR = double.Parse(item[VALOR].ToString());
                    arquivoLinha.DATAVENCIMENTO = DateTime.Parse(item[DATAVENCIMENTO].ToString());
                    arquivoLinha.DATALANCAMENTO = DateTime.Parse(item[DATALANCAMENTO].ToString());
                    arquivoLinha.UTILIZACAO = item[UTILIZACAO].ToString();
                    arquivoLinha.CONDICAOPAGAMENTO = item[CONDICAOPAGAMENTO].ToString();
                    arquivoLinha.FORMAPAGAMENTO = item[FORMAPAGAMENTO].ToString();
                    arquivoLinha.NUMERONOTA = item[NUMERONOTA].ToString();
                    arquivoLinha.SERIENOTA = item[SERIENOTA].ToString();
                    arquivoLinha.MODELO = item[MODELO].ToString();
                    arquivoLinha.DIMENSAO01 = item[DIMENSAO01].ToString();
                    arquivoLinha.DIMENSAO02 = item[DIMENSAO02].ToString();
                    arquivoLinha.DIMENSAO03 = item[DIMENSAO03].ToString();
                    arquivoLinha.DIMENSAO04 = item[DIMENSAO04].ToString();
                    arquivoLinha.DIMENSAO05 = item[DIMENSAO05].ToString();
                    arquivoLinha.PROJETO = item[PROJETO].ToString();
                    arquivoLinha.OBSERVACAO = item[OBSERVACAO].ToString();
                    arquivoLinha.IMPOSTORETIDO1 = item[IMPOSTORETIDO1].ToString();
                    arquivoLinha.IMPOSTORETIDO2 = item[IMPOSTORETIDO2].ToString();
                    arquivoLinha.ANEXO1 = item[ANEXO1].ToString();
                    arquivoLinha.ANEXO2 = item[ANEXO2].ToString();
                    arquivoLinha.ANEXO3 = item[ANEXO3].ToString();
                    arquivoLinha.ANEXO4 = item[ANEXO4].ToString();
                    arquivoLinha.ANEXO5 = item[ANEXO5].ToString();
                    arquivoLinha.u_cva_integracao = "Y";
                    arquivoLinha.u_sx_aliqIPI = item[ALIQIPI].ToString();
                    arquivoLinha.u_sx_aplicacao = item[APLICACAO].ToString();
                    arquivoLinha.u_sx_aliqST = item[ALIQST].ToString();


                    arquivoLinha.LINHA = i;
                    arquivo.LINHAS.Add(arquivoLinha);
                    i++;
                }

                this.GenerateOrders(arquivo, dt_Imp, form);
            }
            catch (Exception ex)
            {
                arquivo.MENSAGEMSTATUS = ex.Message;
            }

            return arquivo;
        }
        #endregion

        #region GenerateOrders
        public void GenerateOrders(Arquivo arquivo, SAPbouiCOM.DataTable dt_Imp, SAPbouiCOM.Form form)
        {
            /*trello 656 - João claudino INI*/
            //VERIFICA SE TODAS AS LINHAS POSSUEM O ANEXO 1
            if (arquivo.LINHAS.Where(a => a.ANEXO1.Length == 0).Count() > 0)
            {
                throw new Exception($"Todas as Linhas do Arquivo precisam possuir o Anexo I preenchido!");
            }
            /*trello 656 - João claudino FIM*/

            /*João claudino INI*/
            //verifica se todos os arquivo de origem existem
            foreach (ArquivoLinha item in arquivo.LINHAS.Where(a => a.ANEXO1.Length > 0))
            {
                if (!File.Exists(item.ANEXO1))
                {
                    throw new Exception($"Anexo 1 " + item.ANEXO1 + " não existe!");
                }
            }
            foreach (ArquivoLinha item in arquivo.LINHAS.Where(a => a.ANEXO2.Length > 0))
            {
                if (!File.Exists(item.ANEXO2))
                {
                    throw new Exception($"Anexo 2 " + item.ANEXO2 + " não existe!");
                }
            }
            foreach (ArquivoLinha item in arquivo.LINHAS.Where(a => a.ANEXO3.Length > 0))
            {
                if (!File.Exists(item.ANEXO3))
                {
                    throw new Exception($"Anexo 3 " + item.ANEXO3 + " não existe!");
                }
            }
            foreach (ArquivoLinha item in arquivo.LINHAS.Where(a => a.ANEXO4.Length > 0))
            {
                if (!File.Exists(item.ANEXO4))
                {
                    throw new Exception($"Anexo 4 " + item.ANEXO4 + " não existe!");
                }
            }
            foreach (ArquivoLinha item in arquivo.LINHAS.Where(a => a.ANEXO5.Length > 0))
            {
                if (!File.Exists(item.ANEXO5))
                {
                    throw new Exception($"Anexo 5 " + item.ANEXO5 + " não existe!");
                }
            }
            /*João claudino FIM*/
            IEnumerable<IGrouping<string, ArquivoLinha>> groupedByCnpj = arquivo.LINHAS.GroupBy(l => l.CNPJATLANTIC);

            foreach (var itemByCnpj in groupedByCnpj)
            {
                BaseModel baseModel = DatabaseConfigDao.GetBaseByCnpj(itemByCnpj.Key);
                string connectionError = ConnectionDao.ConnectExternal(baseModel);

                if (!String.IsNullOrEmpty(connectionError))
                {
                    foreach (var linha in itemByCnpj)
                    {
                        linha.STATUSLINHA = 2;
                        linha.MENSAGEMSTATUS = $"Erro ao conectar na empresa de CNPJ {itemByCnpj.Key}: {connectionError}";
                    }
                    continue;
                }

                foreach (var linha in itemByCnpj)
                {
                    Attachments2 att = ConnectionDao.ExternalCompany.GetBusinessObject(BoObjectTypes.oAttachments2) as Attachments2;
                    Documents doc = ConnectionDao.ExternalCompany.GetBusinessObject(BoObjectTypes.oPurchaseOrders) as Documents;
                    Recordset rst = ConnectionDao.ExternalCompany.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                    CompanyService com_service = ConnectionDao.ExternalCompany.GetCompanyService();
                    PathAdmin pattach = com_service.GetPathAdmin();

                    try
                    {
                        linha.BASE = ConnectionDao.ExternalCompany.CompanyDB;
                        linha.EMPRESA = ConnectionDao.ExternalCompany.CompanyName;

                        linha.DATALANCAMENTO = linha.DATALANCAMENTO.HasValue ? linha.DATALANCAMENTO.Value : DateTime.Today;
                        linha.DATAVENCIMENTO = linha.DATAVENCIMENTO.HasValue ? linha.DATAVENCIMENTO.Value : DateTime.Today;

                        doc.BPL_IDAssignedToInvoice = FilialDao.GetId(linha.CNPJATLANTIC);
                        doc.CardCode = linha.PN;
                        doc.DocDate = linha.DATALANCAMENTO.Value;
                        doc.DocDueDate = linha.DATAVENCIMENTO.Value;

                        int numeroNota;
                        Int32.TryParse(linha.NUMERONOTA, out numeroNota);
                        if (numeroNota != 0)
                        {
                            doc.SequenceCode = -2;
                            doc.SequenceSerial = numeroNota;
                            doc.SeriesString = linha.SERIENOTA;
                        }

                        if (!String.IsNullOrEmpty(linha.MODELO.Trim()))
                        {
                            doc.SequenceModel = linha.MODELO;
                        }

                        if (!String.IsNullOrEmpty(linha.CONDICAOPAGAMENTO.Trim()))
                        {
                            string condicaoPagamento = HelperDao.GetIdByDesc(linha.CONDICAOPAGAMENTO, "OCTG", "GroupNum", "PymntGroup");
                            int condicaoId;
                            Int32.TryParse(condicaoPagamento, out condicaoId);
                            if (condicaoId != 0)
                            {
                                doc.PaymentGroupCode = condicaoId;
                            }
                            else
                            {
                                linha.STATUSLINHA = 2;
                                linha.MENSAGEMSTATUS = $"Condição de pagamento '{linha.CONDICAOPAGAMENTO}' não encontrada na base {ConnectionDao.ExternalCompany.CompanyName}";
                                continue;
                            }
                        }

                        if (!String.IsNullOrEmpty(linha.FORMAPAGAMENTO.Trim()))
                        {
                            string formaPagamento = HelperDao.GetIdByDesc(linha.FORMAPAGAMENTO, "OPYM", "PayMethCod", "Descript");
                            if (!String.IsNullOrEmpty(formaPagamento))
                            {
                                doc.PaymentMethod = formaPagamento;
                            }
                            else
                            {
                                linha.STATUSLINHA = 2;
                                linha.MENSAGEMSTATUS = $"Forma de pagamento '{linha.FORMAPAGAMENTO}' não encontrada na base {ConnectionDao.ExternalCompany.CompanyName}";
                                continue;
                            }
                        }

                        doc.Comments = linha.OBSERVACAO;

                        if (!String.IsNullOrEmpty(linha.IMPOSTORETIDO1))
                        {
                            doc.Lines.WithholdingTaxLines.WTCode = linha.IMPOSTORETIDO1;
                        }
                        if (!String.IsNullOrEmpty(linha.IMPOSTORETIDO2))
                        {
                            doc.Lines.WithholdingTaxLines.Add();
                            doc.Lines.WithholdingTaxLines.WTCode = linha.IMPOSTORETIDO2;
                        }

                        doc.Lines.ItemCode = linha.ITEM;
                        doc.Lines.Quantity = linha.QUANTIDADE;
                        doc.Lines.UnitPrice = linha.VALOR;

                        string utilizacao = HelperDao.GetIdByDesc(linha.UTILIZACAO, "OUSG", "ID", "Usage");
                        if (!String.IsNullOrEmpty(utilizacao))
                        {
                            doc.Lines.Usage = utilizacao;
                        }
                        else
                        {
                            linha.STATUSLINHA = 2;
                            linha.MENSAGEMSTATUS = $"Utilização '{linha.UTILIZACAO}' não encontrada na base {ConnectionDao.ExternalCompany.CompanyName}";
                            continue;
                        }

                        if (!String.IsNullOrEmpty(linha.PROJETO.Trim()))
                        {
                            doc.Lines.ProjectCode = linha.PROJETO;
                        }
                        if (!String.IsNullOrEmpty(linha.DIMENSAO01.Trim()))
                        {
                            doc.Lines.CostingCode = linha.DIMENSAO01;
                        }
                        if (!String.IsNullOrEmpty(linha.DIMENSAO02.Trim()))
                        {
                            doc.Lines.CostingCode2 = linha.DIMENSAO02;
                        }
                        if (!String.IsNullOrEmpty(linha.DIMENSAO03.Trim()))
                        {
                            doc.Lines.CostingCode3 = linha.DIMENSAO03;
                        }
                        if (!String.IsNullOrEmpty(linha.DIMENSAO04.Trim()))
                        {
                            doc.Lines.CostingCode4 = linha.DIMENSAO04;
                        }
                        if (!String.IsNullOrEmpty(linha.DIMENSAO05.Trim()))
                        {
                            doc.Lines.CostingCode5 = linha.DIMENSAO05;
                        }


                        string sourcePath = System.IO.Path.GetDirectoryName(linha.ANEXO1);
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(linha.ANEXO1);
                        string fileExtension = System.IO.Path.GetExtension(linha.ANEXO1).Substring(1);

                        string fileSql = String.Format(Query.Anexo_GetId, pattach.AttachmentsFolderPath, sourcePath, fileName, fileExtension);
                        rst.DoQuery(fileSql);

                        int attAbsEntry = (int)rst.Fields.Item(0).Value;

                        if (attAbsEntry == 0)
                        {
                            att.Lines.SourcePath = sourcePath;
                            att.Lines.FileName = fileName;
                            att.Lines.FileExtension = fileExtension;

                            if (!String.IsNullOrEmpty(linha.ANEXO2.Trim()))
                            {
                                sourcePath = System.IO.Path.GetDirectoryName(linha.ANEXO2);
                                fileName = System.IO.Path.GetFileNameWithoutExtension(linha.ANEXO2);
                                fileExtension = System.IO.Path.GetExtension(linha.ANEXO2).Substring(1);

                                att.Lines.Add();
                                att.Lines.SourcePath = sourcePath;
                                att.Lines.FileName = fileName;
                                att.Lines.FileExtension = fileExtension;
                            }
                            if (!String.IsNullOrEmpty(linha.ANEXO3.Trim()))
                            {
                                sourcePath = System.IO.Path.GetDirectoryName(linha.ANEXO3);
                                fileName = System.IO.Path.GetFileNameWithoutExtension(linha.ANEXO3);
                                fileExtension = System.IO.Path.GetExtension(linha.ANEXO3).Substring(1);

                                att.Lines.SourcePath = sourcePath;
                                att.Lines.FileName = fileName;
                                att.Lines.FileExtension = fileExtension;
                                att.Lines.Add();
                            }
                            if (!String.IsNullOrEmpty(linha.ANEXO4.Trim()))
                            {
                                sourcePath = System.IO.Path.GetDirectoryName(linha.ANEXO4);
                                fileName = System.IO.Path.GetFileNameWithoutExtension(linha.ANEXO4);
                                fileExtension = System.IO.Path.GetExtension(linha.ANEXO4).Substring(1);

                                att.Lines.Add();
                                att.Lines.SourcePath = sourcePath;
                                att.Lines.FileName = fileName;
                                att.Lines.FileExtension = fileExtension;
                            }
                            if (!String.IsNullOrEmpty(linha.ANEXO5.Trim()))
                            {
                                sourcePath = System.IO.Path.GetDirectoryName(linha.ANEXO5);
                                fileName = System.IO.Path.GetFileNameWithoutExtension(linha.ANEXO5);
                                fileExtension = System.IO.Path.GetExtension(linha.ANEXO5).Substring(1);

                                att.Lines.Add();
                                att.Lines.SourcePath = sourcePath;
                                att.Lines.FileName = fileName;
                                att.Lines.FileExtension = fileExtension;
                            }

                            /*João claudino somente adidicona o pedido de compra se conseguiu salvar o anexo*/
                            if (att.Add() != 0)
                            {
                                linha.STATUSLINHA = 2;
                                linha.MENSAGEMSTATUS = $"{ConnectionDao.ExternalCompany.GetLastErrorDescription()} - Base : " + ConnectionDao.ExternalCompany.CompanyDB + " Attachments Folder Path: " + pattach.AttachmentsFolderPath;
                            }
                            else
                            {
                                string ret = ConnectionDao.ExternalCompany.GetNewObjectKey();
                                attAbsEntry = Convert.ToInt32(attAbsEntry);
                            }
                        }
                        else
                        {
                            doc.AttachmentEntry = attAbsEntry;

                            if (doc.Add() != 0)
                            {
                                linha.STATUSLINHA = 2;
                                linha.MENSAGEMSTATUS = $"{ConnectionDao.ExternalCompany.GetLastErrorDescription()}";
                            }
                            else
                            {
                                linha.NUMEROPEDIDOSAP = Convert.ToInt32(ConnectionDao.ExternalCompany.GetNewObjectKey());
                                linha.STATUSLINHA = 1;

                                linha.MENSAGEMSTATUS = $"Pedido gerado com sucesso! Base : " + ConnectionDao.ExternalCompany.CompanyDB + " - NR SAP: " + linha.NUMEROPEDIDOSAP.ToString() + " Server: " + ConnectionDao.ExternalCompany.Server;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        linha.STATUSLINHA = 2;
                        linha.MENSAGEMSTATUS = $"Erro geral: {ex.Message}";
                    }
                    finally
                    {
                        form.Freeze(true);
                        dt_Imp.Rows.Add();
                        dt_Imp.SetValue("Linha", dt_Imp.Rows.Count - 1, linha.LINHA);
                        dt_Imp.SetValue("Empresa", dt_Imp.Rows.Count - 1, linha.EMPRESA);
                        dt_Imp.SetValue("Status", dt_Imp.Rows.Count - 1, linha.STATUSDESC);
                        dt_Imp.SetValue("Erro", dt_Imp.Rows.Count - 1, linha.MENSAGEMSTATUS);
                        form.Freeze(false);

                        Marshal.ReleaseComObject(doc);
                        doc = null;

                        Marshal.ReleaseComObject(att);
                        att = null;

                        Marshal.ReleaseComObject(rst);
                        rst = null;

                        Marshal.ReleaseComObject(com_service);
                        com_service = null;

                        Marshal.ReleaseComObject(pattach);
                        pattach = null;
                    }
                }
            }

            ArquivoDao dao = new ArquivoDao();
            dao.Insert(arquivo);
        }
        #endregion

        public Arquivo Cancel(Arquivo arquivo, SAPbouiCOM.DataTable dt_Doc, SAPbouiCOM.Form form)
        {
            try
            {
                IEnumerable<IGrouping<string, ArquivoLinha>> groupedByBase = arquivo.LINHAS.GroupBy(l => l.BASE);

                foreach (var itemByBase in groupedByBase)
                {
                    BaseModel baseModel = DatabaseConfigDao.GetBaseByName(itemByBase.Key);
                    ConnectionDao.ConnectExternal(baseModel);

                    foreach (var linha in itemByBase)
                    {
                        Documents po = ConnectionDao.ExternalCompany.GetBusinessObject(BoObjectTypes.oPurchaseOrders) as Documents;

                        try
                        {
                            po.GetByKey(linha.NUMEROPEDIDOSAP.Value);

                            if (po.Cancel() != 0)
                            {
                                linha.STATUSLINHA = 6;
                                linha.MENSAGEMSTATUS = $"{ConnectionDao.ExternalCompany.GetLastErrorDescription()}";
                            }
                            else
                            {
                                linha.STATUSLINHA = 5;
                                linha.MENSAGEMSTATUS = $"Pedido cancelado";
                            }
                        }
                        catch (Exception ex)
                        {
                            linha.STATUSLINHA = 6;
                            linha.MENSAGEMSTATUS = $"Erro geral: {ex.Message}";
                        }
                        finally
                        {
                            form.Freeze(true);

                            dt_Doc.SetValue("Status", linha.LINHAGRID - 1, linha.STATUSDESC);
                            dt_Doc.SetValue("Log", linha.LINHAGRID - 1, linha.MENSAGEMSTATUS);

                            form.Freeze(false);

                            Marshal.ReleaseComObject(po);
                            po = null;
                        }
                    }
                }

                ArquivoDao dao = new ArquivoDao();
                dao.Update(arquivo);
            }
            catch (Exception ex)
            {
                arquivo.MENSAGEMSTATUS = ex.Message;
            }
            return arquivo;
        }
    }
}
