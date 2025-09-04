using SAPbobsCOM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CVA.View.Apetit.IntegracaoWMS.BLL
{
    public class IntegradorBO : BaseBO
    {
        private readonly HannaConnector _conn;
        private SAPbobsCOM.Company oCompany;

        /// <summary>
        /// SAP Constructor
        /// </summary>
        public IntegradorBO()
        {
            this._conn = new HannaConnector(ParametrosConexao.param.connectionString, ParametrosConexao.param.database);
        }

        public List<dynamic> ObterEntradasIntegrador(string table, string tableLine, string objType)
        {
            var query = $@"
                            SELECT 
	                             O.{"DocEntry".Aspas()}
	                            ,O.{ "CardCode".Aspas()}
	                            ,O.{ "DocDate".Aspas()}
	                            ,O.{ "Model".Aspas()}
	                            ,O.{ "DocDate".Aspas()}
	                            ,O.{"DocDueDate".Aspas()}
	                            ,O.{"TaxDate".Aspas()}
	                            ,O.{"BPLId".Aspas()}
	                            ,O1.{ "ItemCode".Aspas()}
	                            ,O1.{ "UomCode".Aspas()}
	                            ,O1.{ "Quantity".Aspas()}
	                            ,O1.{ "Price".Aspas()}
	                            ,O1.{ "Usage".Aspas()}
	                            ,O1.{ "TaxCode".Aspas()}
	                            ,O1.{ "WhsCode".Aspas()}
                            FROM {{0}}.OPDN AS O
	                            INNER JOIN {{0}}.PDN1 AS O1 ON
		                            O.{ "DocEntry".Aspas()} = O1.{ "DocEntry".Aspas()}
	                            INNER JOIN {{0}}.{ "@CVA_WMS_INTEGRACAO".Aspas()} AS INTEGRA ON
		                            INTEGRA.{ "U_CVA_Key".Aspas()} = O.{ "DocEntry".Aspas()}
                            WHERE 	O.{ "ObjType".Aspas()} = INTEGRA.{"U_CVA_ObjType".Aspas()}
	                            AND INTEGRA.{ "U_CVA_ObjType".Aspas()} = 20
	                            AND INTEGRA.{"U_CVA_Integrado".Aspas()} = {(int)TipoIntegrado.Criado}
            ;";
            return _conn.QueryListWithCompany(query);
        }

        public void AtualizarEntradaIntegrado(string docEntry, TipoIntegrado status = TipoIntegrado.Enviado, string Erro = "")
        {
            _conn.ExecuteNonQueryWithCompany($@"
                                                UPDATE {{0}}.{"@CVA_WMS_INTEGRACAO".Aspas()}
                                                SET {"U_CVA_Integrado".Aspas()} = {(int)status}
                                                {$",{"U_CVA_IntegradoEm".Aspas()} = CURRENT_DATE"}
                                                {(status == TipoIntegrado.Erro ? ($",{"U_CVA_IntegraErro".Aspas()} = '{Erro.Replace("'", "").Replace("'", "")}'") : "")} 
                                                WHERE {"U_CVA_Key".Aspas()} = {docEntry}
                                              ");
        }






        internal void InsereDocumentoNF(List<dynamic> itens)
        {
            if (!itens.Any())
                throw new Exception("itens não encontrado, não pode ser vazio");

            if (itens.Any(x => string.IsNullOrEmpty(x.ItemCode.ToString())))
                throw new Exception("ItemCode Não encontrado");

            if (SAPConnector.ConectarSap(out oCompany))
            {
                #region para consulta posterior
                /*
                var cardCode = "";
                Recordset rec = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string cnpj = item.U_CVA_TaxId0.ToString();

                rec.DoQuery($@"
                            SELECT "U_CardCodePA" FROM "@CVA_CAR_CONF1" WHERE "U_WhsCode" = 
                    ");

                //se não encontrar o registro
                if (rec.RecordCount == 0)
                    throw new Exception("Parceiro não encontrado");

                cardCode = rec.Fields.Item("CardCode").Value.ToString();
                */

                //var atende = false;
                //List<dynamic> lstEstoque = null;// ContultaEstoque(item.U_CVA_ItemCode.ToString(), int.Parse(item.U_CVA_Quantity.ToString()), out atende);

                //if (!atende)
                //    throw new Exception("Erro na geração do pedido, não atendido");
                #endregion

                oCompany.StartTransaction();
                try
                {
                    var cardCode = "";
                    var defaultWarehouse = "";
                    var usage = "";
                    var header = itens.FirstOrDefault();

                    #region buscar CardCode
                    Recordset rec = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                    rec.DoQuery($@"
                            SELECT 
                                 {"U_CardCodePA".Aspas()} 
                                ,{"U_UsgTransf".Aspas()}
                            FROM {"@CVA_CAR_CONF1".Aspas()} 
                            WHERE   {"U_WhsCode".Aspas()} = '{header.WhsCode}' 
                                AND {"U_BPLId".Aspas()} = '{header.BPLId}';    
                    ");

                    //se não encontrar o registro
                    if (rec.RecordCount == 0)
                        throw new Exception("Parceiro não encontrado");

                    cardCode = rec.Fields.Item("U_CardCodePA").Value.ToString();
                    usage = rec.Fields.Item("U_UsgTransf").Value.ToString();
                    #endregion

                    #region Buscar Depósito Padrão
                    rec.DoQuery($@"
                            SELECT 
                                 {"U_CVA_DflWhs".Aspas()} 
                            FROM {"OCRD".Aspas()} 
                            WHERE   {"CardCode".Aspas()} = '{cardCode}';    
                    ");

                    //se não encontrar o registro
                    if (rec.RecordCount == 0)
                        throw new Exception("Depósito Padrão não encontrado");

                    defaultWarehouse = rec.Fields.Item("U_CVA_DflWhs").Value.ToString();

                    if (string.IsNullOrEmpty(defaultWarehouse))
                        throw new Exception("Depósito Padrão não configurado para Parceiro");

                    #endregion

                    #region GERAR OINV
                    var oinv = oCompany.GetBusinessObject(BoObjectTypes.oInvoices) as Documents;

                    oinv.CardCode = cardCode;
                    oinv.DocDate = header.DocDate;
                    oinv.DocDueDate = header.DocDueDate;
                    oinv.TaxDate = header.TaxDate;

                    foreach (var item in itens)
                    {
                        oinv.Lines.ItemCode = item.ItemCode;
                        //oinv.Lines.UoMCode = item.UomCode; -- readonly
                        oinv.Lines.Quantity = item.Quantity;
                        oinv.Lines.Price = item.Price;
                        oinv.Lines.Usage = usage;
                        oinv.Lines.TaxCode = item.TaxCode;
                        oinv.Lines.WarehouseCode = item.WhsCode;

                        var erro = oinv.Add();
                        var msg = "";
                        if (erro != 0)
                        {
                            oCompany.GetLastError(out erro, out msg);
                            throw new Exception("Erro na geração de OINV " + msg);
                        }
                    }
                    #endregion

                    #region GERAR TRANSFERENCIA OWTR
                    var owtr = oCompany.GetBusinessObject(BoObjectTypes.oStockTransfer) as StockTransfer;

                    owtr.CardCode = cardCode;
                    owtr.FromWarehouse = defaultWarehouse;
                    owtr.ToWarehouse = header.BPLId;

                    foreach (var item in itens)
                    {
                        owtr.Lines.ItemCode = item.ItemCode;
                        owtr.Lines.Quantity = item.Quantity;

                        var erro = owtr.Add();
                        var msg = "";
                        if (erro != 0)
                        {
                            oCompany.GetLastError(out erro, out msg);
                            throw new Exception("Erro na geração de OWTR " + msg);
                        }
                    }
                    #endregion

                    oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                }
                catch (Exception ex)
                {
                    if (oCompany.InTransaction)
                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                    throw ex;
                }
            }
        }
    }
}
