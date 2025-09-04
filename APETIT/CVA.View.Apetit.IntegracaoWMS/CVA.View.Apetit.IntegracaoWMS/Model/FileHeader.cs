using CVA.View.Apetit.IntegracaoWMS.Helpers;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Apetit.IntegracaoWMS.Model
{
    public class FileHeader
    {
        public string IDENTIFICADOR { get; set; } = "1";
        public string CNPJ_CPF_CLI { get; set; }
        public string CLI_NOME { get; set; }
        public DateTime DATA_ARQ { get; set; }
        public List<FileRowHeader> FileRowHeaders { get; set; } = new List<FileRowHeader>();

        public FileHeader(Recordset rec)
        {
            var frh = new List<FileRowHeader>();
            while (!rec.EoF)
            {
                CNPJ_CPF_CLI = rec.Fields.Item("CNPJ_CPF_CLI")?.Value;
                CLI_NOME = rec.Fields.Item("CLI_NOME")?.Value;
                DATA_ARQ = rec.Fields.Item("DATA_ARQ")?.Value;
                FileRowHeaders.Add(new FileRowHeader(rec));
            }
        }

        public string AsFileString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append($"{IDENTIFICADOR}");
            strBuilder.Append($"{CNPJ_CPF_CLI.Replace(".", "").Replace("/", "").Replace("\\", "").Replace(",", "").Trim().FillSpacesAtRight(14)}");
            strBuilder.Append($"{CLI_NOME.FillSpacesAtRight(50)}");
            strBuilder.Append($"{DATA_ARQ.ToString("ddMMyyyy").FillSpacesAtRight(6)}");
            strBuilder.AppendLine();

            foreach (var item in FileRowHeaders)            
                strBuilder.Append(item.AsFileString());

            return strBuilder.ToString();
        }

        public static FileHeader GetItemDataForFile(Dictionary<string, List<string>> docEntryList, DateTime de, DateTime ate)
        {
            Recordset rec = B1Connection.Instance.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var queryBuilder = new StringBuilder();

            queryBuilder.Append($@"
                SELECT 
	                 O.{"DocEntry".Aspas()}							                    AS {"DocEntry".Aspas()}
	                ,O1.{ "ItemCode".Aspas()} 							                AS { "ItemCode".Aspas()}
	                ,OB.{ "TaxIdNum".Aspas()}							                AS { "CNPJ_CPF_CLI".Aspas()}
	                ,OB.{ "BPLName".Aspas()}							                AS { "CLI_NOME".Aspas()}
	                ,CURRENT_DATE							                            AS { "DATA_ARQ".Aspas()}
	                ,'  '									                            AS { "COD_DESTINATARIO".Aspas()}
	                ,OB.{ "BPLName".Aspas()}							                AS { "RAZ_SOC_DESTINATARIO".Aspas()}
	                ,CONCAT(CONCAT(CONCAT(OB.{ "AddrType".Aspas()}, ' '), CONCAT(OB.{ "Street".Aspas()}, ',')),OB.{ "StreetNo".Aspas()})	
											                                            AS { "ENDE_ENTREGA".Aspas()}
	                ,OB.{ "Block".Aspas()}								                AS { "BAIRRO_ENTREGA".Aspas()}
	                ,OB.{ "ZipCode".Aspas()}							                AS { "CEP_ENTREGA".Aspas()}
	                ,OB.{ "City".Aspas()}								                AS { "CIDADE_ENTREGA".Aspas()}
	                ,OB.{ "State".Aspas()}								                AS { "ESTADO_ENTREGA".Aspas()}
	                ,O.{ "DocNum".Aspas()}								                AS { "NUM_PEDIDO".Aspas()}
	                ,O1.{ "ShipDate".Aspas()}							                AS { "DATA_PEDIDO".Aspas()}
	                --,SUM(O1.{"InvQty".Aspas()})						                AS { "QTDE_ITEMS".Aspas()}
	                --,SUM(O1.{"LineTotal".Aspas()})					                AS { "VLR_TOT_PEDIDO".Aspas()}
	                --,SUM(O1.{ "InvQty".Aspas()} * IT.{ "SWeight1".Aspas()})			AS { "PESO_LIQ_PEDIDO".Aspas()}
	                ,'          '							                            AS { "NUM_NF".Aspas()}
	                ,'          '							                            AS { "SIF".Aspas()}
	                ,'          '							                            AS { "NUM_CARGA".Aspas()}
	                --,SUM(O1.{"InvQty".Aspas()} * IT.{"SWeight1".Aspas()})			    AS { "PESO_BRT_PEDIDO".Aspas()}
	                ,OB.{ "TaxIdNum".Aspas()}							                AS { "CNPJ_CPF_DEST".Aspas()}
	                ,'          '							                            AS { "COD_PRODUTO_ANT".Aspas()}
	                ,O1.{ "Dscription".Aspas()}						                    AS { "DESCR_PRODUTO".Aspas()}
	                ,O1.{ "InvQty".Aspas()}							                    AS { "QTDE_PRODUTO".Aspas()}
	                ,O1.{ "LineTotal".Aspas()}	/ O1.{ "InvQty".Aspas()}			    AS { "VLR_UNITARIO".Aspas()}
	                ,O1.{ "InvQty".Aspas()} * IT.{ "SWeight1".Aspas()}			        AS { "PESO_LIQ_ITEM".Aspas()}
	                ,O.{ "DocNum".Aspas()}								                AS { "NUM_PEDIDO".Aspas()}
	                ,'           '							                            AS { "PLACA_VEICULO".Aspas()}
	                ,O1.{ "ItemCode".Aspas()}							                AS { "COD_PRODUTO".Aspas()}
	                ,O1.{ "PackQty".Aspas()}							                AS { "QTDE_VOLUMES".Aspas()}
	                ,'          '							                            AS { "NUM_LOTE_FABR".Aspas()}
	                ,O1.{ "LineTotal".Aspas()}							                AS { "LineTotal".Aspas()}
	                ,IT.{ "SWeight1".Aspas()}							                AS { "SWeight1".Aspas()}
	
                FROM ORDR AS O 
	                INNER JOIN RDR1 AS O1 ON
		                O.{ "DocEntry".Aspas()} = O1.{ "DocEntry".Aspas()}
	                INNER JOIN OBPL AS OB ON
		                O.{ "BPLId".Aspas()} = OB.{ "BPLId".Aspas()}
	                INNER JOIN OITM AS IT ON
		                O1.{ "ItemCode".Aspas()} = IT.{ "ItemCode".Aspas()}
                WHERE 
		                O.{ "DocStatus".Aspas()} = 'O'
	                AND	O1.{ "LineStatus".Aspas()} = 'O'
	                AND	O1.{ "U_CVA_IntegradoOK".Aspas()} <> 'Y'
	                AND O1.{ "ShipDate".Aspas()} BETWEEN '{ de.ToString("yyyyMMdd")}' AND '{ ate.ToString("yyyyMMdd")}'		
                    AND (		
            ");

            var orStr = "";
            foreach (var item in docEntryList)
            {
                if (!item.Value.Any()) continue;

                queryBuilder.AppendLine($@"
                    {orStr}
                    (
                            O1.{ "ItemCode".Aspas()} IN ( {string.Join(",", item.Value)})
                        AND O.{ "DocEntry".Aspas()} = {item.Key}
                    )
                ");
                orStr = "OR";
            }
             
            queryBuilder.Append(") ;");
            rec.DoQuery(queryBuilder.ToString());

            return new FileHeader(rec);
        }
    }

    public class FileRowHeader
    {
        public string IDENTIFICADOR { get; set; } = "2";
        public int DocEntry { get; set; }
        public string COD_DESTINATARIO { get; set; }
        public string RAZ_SOC_DESTINATARIO { get; set; }
        public string ENDE_ENTREGA { get; set; }
        public string BAIRRO_ENTREGA { get; set; }
        public string CEP_ENTREGA { get; set; }
        public string CIDADE_ENTREGA { get; set; }
        public string ESTADO_ENTREGA { get; set; }
        public int NUM_PEDIDO { get; set; }
        public DateTime DATA_PEDIDO { get; set; }
        public double QTDE_ITEMS { get; set; }
        public double VLR_TOT_PEDIDO { get; set; }
        public double PESO_LIQ_PEDIDO { get; set; }
        public double PESO_BRT_PEDIDO { get; set; }
        public string NUM_NF { get; set; }
        public string SIF { get; set; }
        public string NUM_CARGA { get; set; }
        public string CNPJ_CPF_DEST { get; set; }
        public List<FileRowDetail> FileRowDetails { get; set; } = new List<FileRowDetail>();

        public FileRowHeader(Recordset rec)
        {
            var frh = new List<FileRowDetail>();
            DocEntry = rec.Fields.Item("DocEntry")?.Value;

            while (!rec.EoF && DocEntry == rec.Fields.Item("DocEntry")?.Value)
            {
                COD_DESTINATARIO = rec.Fields.Item("COD_DESTINATARIO")?.Value;
                RAZ_SOC_DESTINATARIO = rec.Fields.Item("RAZ_SOC_DESTINATARIO")?.Value;
                ENDE_ENTREGA = rec.Fields.Item("ENDE_ENTREGA")?.Value;
                BAIRRO_ENTREGA = rec.Fields.Item("BAIRRO_ENTREGA")?.Value;
                CEP_ENTREGA = rec.Fields.Item("CEP_ENTREGA")?.Value;
                CIDADE_ENTREGA = rec.Fields.Item("CIDADE_ENTREGA")?.Value;
                ESTADO_ENTREGA = rec.Fields.Item("ESTADO_ENTREGA")?.Value;
                NUM_PEDIDO = rec.Fields.Item("NUM_PEDIDO")?.Value;
                DATA_PEDIDO = rec.Fields.Item("DATA_PEDIDO")?.Value;
                //QTDE_ITEMS = rec.Fields.Item("QTDE_ITEMS")?.Value;
                //VLR_TOT_PEDIDO = rec.Fields.Item("VLR_TOT_PEDIDO")?.Value;
                //PESO_LIQ_PEDIDO = rec.Fields.Item("PESO_LIQ_PEDIDO")?.Value;
                //PESO_BRT_PEDIDO = rec.Fields.Item("PESO_BRT_PEDIDO")?.Value;
                NUM_NF = rec.Fields.Item("NUM_NF")?.Value;
                SIF = rec.Fields.Item("SIF")?.Value;
                NUM_CARGA = rec.Fields.Item("NUM_CARGA")?.Value;
                CNPJ_CPF_DEST = rec.Fields.Item("CNPJ_CPF_DEST")?.Value;
                FileRowDetails.Add(new FileRowDetail(rec));
                rec.MoveNext();
            }

            QTDE_ITEMS = FileRowDetails.Sum(x => Convert.ToDouble(x.QTDE_PRODUTO));
            VLR_TOT_PEDIDO = FileRowDetails.Sum(x => x.LineTotal);
            PESO_LIQ_PEDIDO = FileRowDetails.Sum(x => Convert.ToDouble(x.QTDE_PRODUTO) * x.SWeight1);
            PESO_BRT_PEDIDO = FileRowDetails.Sum(x => Convert.ToDouble(x.QTDE_PRODUTO) * x.SWeight1);
        }


        public string AsFileString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append($"{IDENTIFICADOR}");
            strBuilder.Append($"{COD_DESTINATARIO.FillSpacesAtLeft(10)}");
            strBuilder.Append($"{RemoverAcentos(RAZ_SOC_DESTINATARIO).FillSpacesAtRight(50)}");
            strBuilder.Append($"{RemoverAcentos(ENDE_ENTREGA).FillSpacesAtRight(50)}");
            strBuilder.Append($"{RemoverAcentos(BAIRRO_ENTREGA).FillSpacesAtRight(20)}");
            strBuilder.Append($"{CEP_ENTREGA.FillSpacesAtRight(9)}");
            strBuilder.Append($"{RemoverAcentos(CIDADE_ENTREGA).FillSpacesAtRight(25)}");
            strBuilder.Append($"{ESTADO_ENTREGA.FillSpacesAtRight(2)}");
            strBuilder.Append($"{NUM_PEDIDO.FillZeroes(10)}");
            strBuilder.Append($"{DATA_PEDIDO.ToString("ddMMyyyy").FillSpacesAtRight(6)}");
            strBuilder.Append($"{QTDE_ITEMS.ToString().Replace(",", "").FillZeroes(4)}");
            strBuilder.Append($"{VLR_TOT_PEDIDO.ToString("N2").FillZeroes(13)}");
            strBuilder.Append($"{PESO_LIQ_PEDIDO.ToString("N2").Replace(",","").FillZeroes(10)}");
            strBuilder.Append($"{NUM_NF.FillSpacesAtRight(10)}");
            strBuilder.Append($"{SIF.FillSpacesAtRight(10)}");
            strBuilder.Append($"{NUM_CARGA.FillSpacesAtRight(10)}");
            strBuilder.Append($"{PESO_BRT_PEDIDO.ToString("N3").Replace(",", "").FillZeroes(10)}");
            strBuilder.Append($"{CNPJ_CPF_DEST.FillSpacesAtRight(14).Replace(".", "").Replace("/", "").Replace("\\", "").Replace(",", "").Trim()}");
            strBuilder.Append($"{NUM_NF.FillSpacesAtRight(10)}");
            strBuilder.AppendLine();

            foreach (var item in FileRowDetails)
                strBuilder.Append(item.AsFileString());

            return strBuilder.ToString();
        }


        public string RemoverAcentos(string texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[k]);
                }
            }
            return sb.ToString();
        }
    }

    public class FileRowDetail
    {
        public string IDENTIFICADOR { get; set; } = "3";
        public string COD_PRODUTO_ANT { get; set; }
        public string DESCR_PRODUTO { get; set; }
        public double QTDE_PRODUTO { get; set; }
        public double VLR_UNITARIO { get; set; }
        public double PESO_LIQ_ITEM { get; set; }
        public string NUM_PEDIDO { get; set; }
        public string PLACA_VEICULO { get; set; }
        public string COD_PRODUTO { get; set; }
        public double QTDE_VOLUMES { get; set; }
        public string NUM_LOTE_FABR { get; set; }
        public double LineTotal { get; set; }
        public double SWeight1 { get; set; }

        public FileRowDetail(Recordset rec)
        {
            COD_PRODUTO_ANT = rec.Fields.Item("COD_PRODUTO_ANT")?.Value;
            DESCR_PRODUTO = rec.Fields.Item("DESCR_PRODUTO")?.Value;
            QTDE_PRODUTO = rec.Fields.Item("QTDE_PRODUTO")?.Value;
            VLR_UNITARIO = rec.Fields.Item("VLR_UNITARIO")?.Value;
            PESO_LIQ_ITEM = rec.Fields.Item("PESO_LIQ_ITEM")?.Value;
            NUM_PEDIDO =  Convert.ToString(rec.Fields.Item("NUM_PEDIDO")?.Value);
            PLACA_VEICULO = rec.Fields.Item("PLACA_VEICULO")?.Value;
            COD_PRODUTO = rec.Fields.Item("COD_PRODUTO")?.Value;
            QTDE_VOLUMES = rec.Fields.Item("QTDE_VOLUMES")?.Value;
            NUM_LOTE_FABR = rec.Fields.Item("NUM_LOTE_FABR")?.Value;
            LineTotal = rec.Fields.Item("LineTotal")?.Value;
            SWeight1 = rec.Fields.Item("SWeight1")?.Value;
        }

        public string AsFileString()
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append($"{IDENTIFICADOR}");
            strBuilder.Append($"{COD_PRODUTO_ANT.FillSpacesAtRight(10)}");
            strBuilder.Append($"{DESCR_PRODUTO.FillSpacesAtRight(50)}");
            strBuilder.Append($"0".FillZeroes(4));
            //strBuilder.Append($"{Math.Round(QTDE_PRODUTO,3).ToString().Replace(",", "").PadLeft(4,'0')}");// FillZeroes(4)}");
            strBuilder.Append($"{VLR_UNITARIO.ToString("N5").FillZeroes(13)}");
            strBuilder.Append($"{QTDE_PRODUTO.ToString("N3").Replace(",", "").FillZeroes(10)}");
            //strBuilder.Append($"{SWeight1.ToString("N3").Replace(",", "").FillZeroes(10)}");//ToString("N3").FillZeroes(10)}");
            strBuilder.Append($"{NUM_PEDIDO.FillZeroes(10)}");
            strBuilder.Append($"{PLACA_VEICULO.FillSpacesAtRight(11)}");
            strBuilder.Append($"{COD_PRODUTO.Replace(".", "").FillSpacesAtRight(20)}");
            strBuilder.Append($"{Math.Round(QTDE_VOLUMES,3).ToString().Replace(",", "").FillZeroes(10)}");
            strBuilder.Append($"{NUM_LOTE_FABR.FillSpacesAtRight(10)}");
            strBuilder.AppendLine();

            return strBuilder.ToString();
        }

        
    }

    
}
