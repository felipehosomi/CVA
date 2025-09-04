using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.Apetit.Addon.Class
{
    public class Geral
    {
       
        //==================================================================================================================================//
        static public DataGridViewCheckBoxColumn retColCheckBox(string headerText, string name, int width, bool readOnly, bool visible, string alignment, string format, int fontSize, bool sortable)
        //==================================================================================================================================//
        {
            var col = new DataGridViewCheckBoxColumn();

            col.HeaderText = headerText;
            col.Name = name;
            col.Width = width;
            col.ReadOnly = readOnly;
            col.Visible = visible;
            col.DefaultCellStyle.Alignment = retornaAlinhamnento(alignment);
            if (format != "")
                col.DefaultCellStyle.Format = format;
            if (fontSize > 0)
            {
                col.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", fontSize);
                col.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", fontSize - 2, FontStyle.Regular, GraphicsUnit.Point);
            }
            if (sortable)
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            else
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            return col;
        }

        //==================================================================================================================================//
        static public DataGridViewTextBoxColumn retColTextBox(string headerText, string name, int width, bool readOnly, bool visible, string alignment, string format, int fontSize, bool sortable)
        //==================================================================================================================================//
        {
            var col = new DataGridViewTextBoxColumn();

            col.HeaderText = headerText;

            col.Name = name;
            col.Width = width;
            col.ReadOnly = readOnly;
            col.Visible = visible;
            col.DefaultCellStyle.Alignment = retornaAlinhamnento(alignment);
            if (format != "")
            {
                //DataGridViewCellStyle style = new DataGridViewCellStyle();
                //style.Format = "N3";
                //col.DefaultCellStyle = style;                     //.Format = format;
                col.DefaultCellStyle.Format = format;
            }
            if (fontSize > 0)
            {
                col.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular, GraphicsUnit.Point);
                col.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", fontSize - 2, FontStyle.Regular, GraphicsUnit.Point);
            }
            if (sortable)
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            else
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            return col;
        }

        //==================================================================================================================================//
        static public DataGridViewComboBoxColumn retColComboBox(string headerText, string name, int width, bool readOnly, bool visible, string alignment, string format, int fontSize, bool sortable)
        //==================================================================================================================================//
        {
            var col = new DataGridViewComboBoxColumn();

            col.HeaderText = headerText;
            col.Name = name;
            col.Width = width;
            col.ReadOnly = readOnly;
            col.Visible = visible;
            col.DefaultCellStyle.Alignment = retornaAlinhamnento(alignment);
            if (format != "")
                col.DefaultCellStyle.Format = format;
            if (fontSize > 0)
            {
                col.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", fontSize);
                col.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", fontSize - 2, FontStyle.Regular, GraphicsUnit.Point);
            }
            if (sortable)
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            else
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            return col;
        }

        //==================================================================================================================================//
        static public DataGridViewContentAlignment retornaAlinhamnento(string alignment)
        //==================================================================================================================================//
        {
            DataGridViewContentAlignment retorno = DataGridViewContentAlignment.MiddleLeft;

            if (alignment == "direita")
                retorno = DataGridViewContentAlignment.MiddleRight;
            if (alignment == "centro")
                retorno = DataGridViewContentAlignment.MiddleCenter;

            return retorno;
        }

        //==================================================================================================================================//
        static public System.Data.DataTable TabelaPlanejamento(int filial, DateTime dtIni, DateTime dtFim)
        //==================================================================================================================================//
        {
            System.Data.DataTable table = null;
            string sql, dataDe, dataAte;

            try
            {
                dataDe = dtIni.ToString("yyyyMMdd");
                dataAte = dtFim.ToString("yyyyMMdd");

                sql = string.Format(@"
SELECT DISTINCT T0.*
FROM ""@CVA_PLANEJAMENTO"" T0
    INNER JOIN ""@CVA_LN_PLANEJAMENTO"" T1 ON T1.""U_CVA_PLAN_ID"" = T0.""Code""
    LEFT JOIN OOAT T12 ON T12.""Number"" = T0.""U_CVA_ID_CONTRATO""
    LEFT JOIN OBPL T8 ON T8.""BPLId"" = T12.""U_CVA_FILIAL""
WHERE
    T8.""BPLId"" = {0}
    AND IFNULL(T1.""U_CVA_INSUMO"", '') <> ''
    AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) >= '{1}'
    AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) <= '{2}'
", filial, dataDe, dataAte);

                table = Class.Conexao.ExecuteSqlDataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return table;
        }

        //==================================================================================================================================//
        static public System.Data.DataTable TabelaLnPlanejamento(int filial, DateTime dtIni, DateTime dtFim)
        //==================================================================================================================================//
        {
            System.Data.DataTable table = null;
            string sql, dataDe, dataAte;

            try
            {
                dataDe = dtIni.ToString("yyyyMMdd");
                dataAte = dtFim.ToString("yyyyMMdd");

                sql = string.Format(@"
SELECT
    T1.*
    ,ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) AS ""DataPlanej""
    ,   CASE O.""InvntryUom""
        WHEN 'UN'
            THEN
                ROUND((SELECT  SUM(T20.""U_CVA_QTD"")  FROM ""@CVA_TURNO_QTD"" T20 WHERE T20.""U_CVA_ID_LN_PLAN"" = T1.""Code"") * (T1.""U_CVA_PERCENT"" / 100) , 0, ROUND_HALF_UP)
		ELSE
            (SELECT  SUM(T20.""U_CVA_QTD"")  FROM ""@CVA_TURNO_QTD"" T20 WHERE T20.""U_CVA_ID_LN_PLAN"" = T1.""Code"") * (T1.""U_CVA_PERCENT"" / 100)		
	END AS ""QTD""
FROM ""@CVA_PLANEJAMENTO"" T0
    INNER JOIN ""@CVA_LN_PLANEJAMENTO"" T1 ON T1.""U_CVA_PLAN_ID"" = T0.""Code""
    INNER JOIN OITM AS O ON O.""ItemCode"" = T1.""U_CVA_INSUMO""
    LEFT JOIN OOAT T12 ON T12.""Number"" = T0.""U_CVA_ID_CONTRATO""
    LEFT JOIN OBPL T8 ON T8.""BPLId"" = T12.""U_CVA_FILIAL""
WHERE
    T8.""BPLId"" = {0}
    AND IFNULL(T1.""U_CVA_INSUMO"", '') <> ''
    AND IFNULL(T1.""U_CVA_LOTE_CONSOL"", 0) = 0
    AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) >= '{1}'
    AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) <= '{2}'
", filial, dataDe, dataAte);

                table = Class.Conexao.ExecuteSqlDataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return table;
        }

        //==================================================================================================================================//
        static public System.Data.DataTable TabelaOPs(int filial, DateTime dtIni, DateTime dtFim)
        //==================================================================================================================================//
        {
            System.Data.DataTable table = null;
            string sql, dataDe, dataAte;

            try
            {
                dataDe = dtIni.ToString("yyyyMMdd");
                dataAte = dtFim.ToString("yyyyMMdd");

                sql = string.Format(@"
SELECT
    T1.*
    ,ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) AS ""DataPlanej""
	,CASE O.""InvntryUom""
        WHEN 'UN'
            THEN
                ROUND( (T14.""U_CVA_QTD"" * (T1.""U_CVA_PERCENT"" / 100)) , 0, ROUND_HALF_UP)
        ELSE
            ((T14.""U_CVA_QTD"" * (T1.""U_CVA_PERCENT"" / 100)))		
	 END AS ""QTD""
    ,T14.""U_CVA_ID_TURNO"" AS ""U_CVA_ID_TURNO""
    ,T0.""U_CVA_ID_SERVICO"" AS ""U_CVA_ID_SERVICO""
    ,T8.""BPLId"" AS ""BPLId""
FROM ""@CVA_PLANEJAMENTO"" AS T0
    INNER JOIN ""@CVA_LN_PLANEJAMENTO"" AS T1 ON T1.""U_CVA_PLAN_ID"" = T0.""Code""
    INNER JOIN OITM AS O ON O.""ItemCode"" = T1.""U_CVA_INSUMO""
    LEFT JOIN OOAT T12 ON T12.""Number"" = T0.""U_CVA_ID_CONTRATO""
    LEFT JOIN OBPL T8 ON T8.""BPLId"" = T12.""U_CVA_FILIAL""
    LEFT JOIN ""@CVA_TURNO_QTD"" T14 ON T14.""U_CVA_ID_LN_PLAN"" = T1.""Code""
	LEFT JOIN ""@CVA_SERVICO_PLAN"" T16 ON T16.""Code"" = T0.""U_CVA_ID_SERVICO"" 
WHERE
    T8.""BPLId"" = {0}
    AND IFNULL(T1.""U_CVA_INSUMO"", '') <> ''
    AND IFNULL(T1.""U_CVA_LOTE_CONSOL"", 0) = 0
    AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) >= '{1}'
    AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) <= '{2}'
", filial, dataDe, dataAte);

                table = Class.Conexao.ExecuteSqlDataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return table;
        }

        //==================================================================================================================================//
        static public string TabelaGridTela(int filial, DateTime dtIni, DateTime dtFim)
        //==================================================================================================================================//
        {
            string sql = "", dataDe, dataAte;

            try
            {
                dataDe = dtIni.ToString("yyyyMMdd");
                dataAte = dtFim.ToString("yyyyMMdd");

                sql = string.Format(@"
SELECT 
	""Data"" AS ""Data Entrega""
	,MAX(""DiasPlanej"") AS ""Dias Segurança""
	,MIN(""DataPlanej"") AS ""Data Prim Consumo""
    ,MAX(""Categoria"") AS ""Categoria""
    ,MAX(""Familia"") AS ""Família""
    ,MAX(""SubFamilia"") AS ""SubFamília""
    ,""ItemCode"" AS ""Código Item""
	,MAX(""ItemName"") AS ""Descrição""
	,SUM(""Quant"") AS ""Quantidade""
	,MAX(""Origem"") AS ""Origem""
FROM (
	SELECT
		T8.""BPLId"" AS ""Filial""
		,T1.""U_CVA_INSUMO""
		,T1.""U_CVA_INSUMO_DES""
		,ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) AS ""DataPlanej""
		,IFNULL(T10.""U_DiasPlanej"", 0) AS ""DiasPlanej""
		,(	SELECT MAX(T21.""U_CVA_Data"")
			FROM ""@CVA_CAR_CAL"" T20 
				LEFT JOIN ""@CVA_CAR_CAL1"" T21 ON T21.""DocEntry"" = T20.""DocEntry""
			WHERE T20.""U_BPLId"" = T8.""BPLId""
				AND T20.""U_Categoria"" = T6.""U_CVA_Categoria""
                AND T21.""U_CVA_Data"" <= ADD_DAYS(ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1), IFNULL(T10.""U_DiasPlanej"", 0) * -1)
		) AS ""Data""
		,T1.""U_CVA_INSUMO""
		,T6.""ItemCode"" AS ""ItemCode""
		,T6.""ItemName"" AS ""ItemName""
		,T9.""DocEntry""
		,CASE
			WHEN IFNULL(T9.""DocEntry"", 0) > 0 THEN 'FI'
			WHEN IFNULL(T9.""DocEntry"", 0) = 0 THEN 'CD'
			ELSE ''
		END AS ""Origem""
		,T16.""Code"" AS ""IdServico""
		,T16.""Name"" AS ""Servico""
		,T14.""U_CVA_ID_TURNO"" AS ""IdTurno""
		,T14.""U_CVA_DES_TURNO"" AS ""Turno""
        ,T6.""U_CVA_Familia"" AS ""Familia""
        ,T6.""U_CVA_Subfamilia"" AS ""SubFamilia""
		,T18.""Descr"" AS ""Categoria""
		,(T5.""Quantity"" / T4.""PlAvgSize"") * 
        (CASE T6.""InvntryUom""

            WHEN 'UN'

                THEN

                    ROUND((TO_DECIMAL(T14.""U_CVA_QTD"") * (T1.""U_CVA_PERCENT"" / 100)), 0, ROUND_HALF_UP)

            ELSE
                ((T14.""U_CVA_QTD"" * (T1.""U_CVA_PERCENT"" / 100)))		
		 END) AS ""Quant""
    FROM ""@CVA_PLANEJAMENTO"" T0
		INNER JOIN ""@CVA_LN_PLANEJAMENTO"" T1 ON T1.""U_CVA_PLAN_ID"" = T0.""Code""
		INNER JOIN ""OITT"" T4 ON T4.""Code"" = T1.""U_CVA_INSUMO"" 
		INNER JOIN ""ITT1"" T5 ON T5.""Father"" = T4.""Code""
		INNER JOIN ""OITM"" T6 ON T6.""ItemCode"" = T5.""Code""
		INNER JOIN ""OITM"" T3 ON T3.""ItemCode"" = T1.""U_CVA_INSUMO""
		LEFT JOIN ""OCRD"" T2 ON T2.""CardCode"" = T0.""U_CVA_ID_CLIENTE""
		LEFT JOIN ""OOAT"" T12 ON T12.""Number"" = T0.""U_CVA_ID_CONTRATO""
		LEFT JOIN ""OBPL"" T8 ON T8.""BPLId"" = T12.""U_CVA_FILIAL""
		LEFT JOIN ""@CVA_CAR_INS"" T7 ON T7.""U_BPLId"" = T8.""BPLId""
		LEFT JOIN ""@CVA_CAR_INS1"" T9 ON T9.""DocEntry"" = T7.""DocEntry"" 
			AND (
                    ((T9.""U_CVA_ItmsGrpCod"" = T6.""ItmsGrpCod"") AND (IFNULL(T9.""U_CVA_ItemCode"", '') = ''))
                    OR (IFNULL(T9.""U_CVA_ItemCode"", '') = T6.""ItemCode"")
                    OR (IFNULL(T9.""U_CVA_Familia"", '') = T6.""U_CVA_Familia"")
                    OR (IFNULL(T9.""U_CVA_SFamilia"", '') = T6.""U_CVA_Subfamilia"")
                )
		LEFT JOIN ""@CVA_TURNO_QTD"" T14 ON T14.""U_CVA_ID_LN_PLAN"" = T1.""Code""
		LEFT JOIN ""@CVA_SERVICO_PLAN"" T16 ON T16.""Code"" = T0.""U_CVA_ID_SERVICO"" 
		LEFT JOIN ""CUFD"" T17 ON T17.""TableID"" = 'OITM' AND T17.""AliasID"" = 'CVA_Categoria' 
        LEFT JOIN ""UFD1"" T18 ON T18.""TableID"" = T17.""TableID"" AND T18.""FieldID"" = T17.""FieldID"" AND T18.""FldValue"" = T6.""U_CVA_Categoria""
        LEFT JOIN ""@CVA_CAR_CONFIG"" T10 ON T10.""U_BPLId"" = T8.""BPLId""
    WHERE
        T8.""BPLId"" = {0}
		AND IFNULL(T1.""U_CVA_INSUMO"", '') <> ''
        AND IFNULL(T1.""U_CVA_LOTE_CONSOL"", 0) = 0
		AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) >= '{1}'  
		AND ADD_DAYS(T0.""U_CVA_DATA_REF"", T1.""Name"" - 1) <= '{2}'
	ORDER BY T1.""U_CVA_INSUMO""
	)
GROUP BY ""Filial"", ""Data"" ,""ItemCode""
ORDER BY ""ItemCode""		
", filial, dataDe, dataAte);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sql;
        }

        //==================================================================================================================================//
        static public bool PlanejamentoCarregado(string tabela, string code)
        //==================================================================================================================================//
        {
            bool achou = false;
            string sql;
            int cont;

            try
            {
                if (tabela == "@CVA_CAR_OP")
                    sql = string.Format(@"SELECT COUNT(1) FROM ""{0}"" WHERE ""U_ID_PLAN1"" = {1} ", tabela, code);
                else if (tabela == "@CVA_CAR_PLAN")
                    sql = string.Format(@"SELECT COUNT(1) FROM ""{0}"" WHERE ""U_Code"" = {1} ", tabela, code);
                else
                    sql = string.Format(@"SELECT COUNT(1) FROM ""{0}"" WHERE ""Code"" = {1} ", tabela, code);
                cont = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());

                if (cont > 0)
                    achou = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return achou;
        }

        //==================================================================================================================================//
        static public void SubirPlanejamento(int lote, System.Data.DataTable table)
        //==================================================================================================================================//
        {
            string sql, bplName = "", codigo2PN = "", msg = "", sVigencia, sDataRef, code;
            int diasPlanej = -1, bplId, bplIdCD, precoUnit = 0, usgTransf = 0, chave;
            DateTime vigencia, dataRef;
            System.Data.DataTable table1;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"SELECT ""U_ID_Filial"" FROM ""@CVA_CAR_LOTE"" WHERE ""U_Lote"" = {0}", lote);
                Int32.TryParse(Class.Conexao.ExecuteSqlScalar(sql).ToString(), out bplId);
                if (bplId <= 0)
                    throw new Exception("Não conseguiu identificar o código da filial");

                sql = string.Format(@"
SELECT TOP 1 
    T0.""BPLName""
    ,IFNULL(T1.""U_DiasPlanej"", -1) AS ""U_DiasPlanej""
    ,IFNULL(T1.""U_CardCodePN"", '') AS ""U_CardCodePN""
    ,IFNULL(T1.""U_PrecoUnit"", 0) AS ""U_PrecoUnit""
    ,IFNULL(T1.""U_UsgTransf"", 0) AS ""U_UsgTransf""
    ,IFNULL(T1.""U_BPLId_CD"", 0) AS ""U_BPLId_CD""
FROM OBPL T0
	LEFT JOIN ""@CVA_CAR_CONFIG"" T1 ON T1.""U_BPLId"" = T0.""BPLId""
WHERE T0.""BPLId"" = {0}
", bplId);
                table1 = Class.Conexao.ExecuteSqlDataTable(sql);

                diasPlanej = Convert.ToInt32(table1.Rows[0]["U_DiasPlanej"].ToString());
                bplName = table1.Rows[0]["BPLName"].ToString();
                codigo2PN = table1.Rows[0]["U_CardCodePN"].ToString();
                precoUnit = Convert.ToInt32(table1.Rows[0]["U_PrecoUnit"].ToString());
                usgTransf = Convert.ToInt32(table1.Rows[0]["U_UsgTransf"].ToString());
                bplIdCD = Convert.ToInt32(table1.Rows[0]["U_BPLId_CD"].ToString());

                if (diasPlanej == -1)
                    msg += "Dias de segurança não cadastrado nas configurações de filial" + Environment.NewLine;
                if (codigo2PN == "")
                    msg += "Código do PN da filial não cadastrado nas configurações de filial" + Environment.NewLine;
                if (precoUnit == 0)
                    msg += "Seleção de preço unitário não cadastrado nas configurações da consolidação" + Environment.NewLine;
                if (usgTransf == 0)
                    msg += "Usage de transferência não cadastrado nas configurações da consolidação" + Environment.NewLine;
                if (bplIdCD == 0)
                    msg += "CD não cadastrado nas configurações da consolidação" + Environment.NewLine;

                if (!string.IsNullOrEmpty(msg))
                    throw new Exception(msg);

                foreach (System.Data.DataRow linha in table.Rows)
                {
                    code = linha["Code"].ToString();

                    if (!PlanejamentoCarregado("@CVA_CAR_PLAN", code))
                    {
                        chave = RetornaNextCode("@CVA_CAR_PLAN");

                        vigencia = Convert.ToDateTime(linha["U_CVA_VIGENCIA_CONTR"].ToString());
                        sVigencia = vigencia.ToString("yyyyMMdd");

                        dataRef = Convert.ToDateTime(linha["U_CVA_DATA_REF"].ToString());
                        sDataRef = dataRef.ToString("yyyyMMdd");

                        sql = string.Format(@"
INSERT INTO ""@CVA_CAR_PLAN"" (
""Code"", ""Name"", ""U_Code"", ""U_CVA_ID_CLIENTE"", ""U_CVA_ID_CONTRATO"", ""U_CVA_ID_MODEL_CARD"", ""U_CVA_VIGENCIA_CONTR"", ""U_CVA_DES_CLIENTE"", ""U_CVA_DES_MODELO_CARD"", ""U_CVA_DATA_REF"", ""U_CVA_ID_SERVICO"", ""U_CVA_DES_SERVICO"", ""U_CVA_ID_G_SERVICO"", ""U_CVA_DES_G_SERVICO"", ""U_Dias_Planej"", ""U_BPLId"", ""U_BPLIdCD"", ""U_Codigo2PN"", ""U_PrecoUnit"", ""U_UsgTransf"") VALUES (
{0}     , '{1}'   , {2}       , '{3}'               , '{4}'                , '{5}'                  , '{6}'                   , '{7}'                , '{8}'                    , '{9}'             , '{10}'              , '{11}'               , '{12}'                , '{13}'                 , {14}             , {15}       , {16}         ,  '{17}'         , {18}           , {19}          )",
        chave
        , chave.ToString()
        , code
        , linha["U_CVA_ID_CLIENTE"].ToString()
        , linha["U_CVA_ID_CONTRATO"].ToString()
        , linha["U_CVA_ID_MODEL_CARD"].ToString()
        , sVigencia
        , linha["U_CVA_DES_CLIENTE"].ToString()
        , linha["U_CVA_DES_MODELO_CARD"].ToString()
        , sDataRef
        , linha["U_CVA_ID_SERVICO"].ToString()
        , linha["U_CVA_DES_SERVICO"].ToString()
        , linha["U_CVA_ID_G_SERVICO"].ToString()
        , linha["U_CVA_DES_G_SERVICO"].ToString()
        , diasPlanej
        , bplId
        , bplIdCD
        , codigo2PN
        , precoUnit
        , usgTransf
        );
                        oRec.DoQuery(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        static public void SubirLnPlanejamento(int lote, System.Data.DataTable table)
        //==================================================================================================================================//
        {
            string sql = "", msg = "", sDataConsumo;
            int chave;
            DateTime dataConsumo;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (System.Data.DataRow linha in table.Rows)
                {
                    chave = RetornaNextCode("@CVA_CAR_PLAN1");

                    dataConsumo = Convert.ToDateTime(linha["DataPlanej"].ToString());
                    sDataConsumo = dataConsumo.ToString("yyyyMMdd");

                    sql = string.Format(@"
INSERT INTO ""@CVA_CAR_PLAN1"" (
""Code"", ""Name"", ""U_Code"", ""U_CVA_PLAN_ID"", ""U_CVA_TIPO_PRATO"", ""U_CVA_TIPO_PRATO_DES"", ""U_CVA_INSUMO"", ""U_CVA_INSUMO_DES"", ""U_CVA_PERCENT"", ""U_CVA_QTD_ORI"", ""U_CVA_CUSTO_ITEM"", ""U_CVA_TOTAL"", ""U_CVA_MODELO_LIN_ID"", ""U_CVA_DIA_SEMANA"", ""U_QTD"", ""U_Data_Consumo"", ""U_Lote"") VALUES (
{0}     , '{1}'   , {2}       , '{3}'            , '{4}'               , '{5}'                   , '{6}'           , '{7}'               , {8}              , {9}              , {10}                 , {11}           , '{12}'                 , '{13}'              , {14}     , '{15}'            , {16} )",
    chave
    , chave.ToString()
    , linha["Code"].ToString()
    , linha["U_CVA_PLAN_ID"].ToString()
    , linha["U_CVA_TIPO_PRATO"].ToString()
    , linha["U_CVA_TIPO_PRATO_DES"].ToString()
    , linha["U_CVA_INSUMO"].ToString()
    , linha["U_CVA_INSUMO_DES"].ToString()
    , linha["U_CVA_PERCENT"].ToString().Replace(',', '.')
    , linha["U_CVA_QTD_ORI"].ToString()
    , linha["U_CVA_CUSTO_ITEM"].ToString().Replace(',', '.')
    , linha["U_CVA_TOTAL"].ToString().Replace(',', '.')
    , linha["U_CVA_MODELO_LIN_ID"].ToString()
    , linha["U_CVA_DIA_SEMANA"].ToString()
    , linha["QTD"].ToString()
    , sDataConsumo
    , lote    
    );
                    oRec.DoQuery(sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        static public void SubirOPPlanejamento(int lote, System.Data.DataTable table)
        //==================================================================================================================================//
        {
            string sql = "", msg = "", sDataConsumo, code, depositoPadrao;
            int chave, codePlan1;
            DateTime dataConsumo;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                foreach (System.Data.DataRow linha in table.Rows)
                {
                    code = linha["Code"].ToString();
                    sql = string.Format(@"SELECT ""Code"" FROM ""@CVA_CAR_PLAN1"" WHERE ""U_Code"" = {0} AND ""U_Lote"" = {1} ", code, lote);
                    oRec.DoQuery(sql);
                    Int32.TryParse(oRec.Fields.Item("Code").Value.ToString(), out codePlan1);

                    depositoPadrao = Class.Geral.RetornaCodDepPadrao(linha["BPLId"].ToString());

                    chave = RetornaNextCode("@CVA_CAR_OP");

                    dataConsumo = Convert.ToDateTime(linha["DataPlanej"].ToString());
                    sDataConsumo = dataConsumo.ToString("yyyyMMdd");

                    sql = string.Format(@"
INSERT INTO ""@CVA_CAR_OP"" (
""Code"", ""Name"", ""U_Code"", ""U_ID_PLAN1"", ""U_ItemCode"", ""U_Data_Consumo"", ""U_Quant"", ""U_DflWhs"", ""U_Servico"", ""U_Turno"") VALUES (
{0}     , '{1}'   , {2}       , {3}           , '{4}'         , '{5}'             , {6}        , '{7}'       , {8}          , {9})",
    chave
    , chave.ToString()
    , code
    , codePlan1
    , linha["U_CVA_INSUMO"].ToString()
    , sDataConsumo
    , linha["QTD"].ToString()
    , depositoPadrao
    , linha["U_CVA_ID_SERVICO"].ToString()
    , linha["U_CVA_ID_TURNO"].ToString()
    );
                    oRec.DoQuery(sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        static public void Subir_LN_OPPlanejamento(int lote)
        //==================================================================================================================================//
        {
            string sql = "", msg = "", sDataConsumo, sDataEntrega, code, depositoPadrao, itemPaiEstrutura, itemCode, origem, s, familia, subFamilia, categoria;
            int chave, codePlan1, bplId, bplIdCD;
            DateTime dataConsumo, dataEntrega;
            double quant, quantPai, aux;
            System.Data.DataTable table;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                SAPbobsCOM.Recordset oRec1 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"SELECT ""U_ID_Filial"" FROM ""@CVA_CAR_LOTE"" WHERE ""U_Lote"" = {0}", lote);
                Int32.TryParse(Class.Conexao.ExecuteSqlScalar(sql).ToString(), out bplId);
                if (bplId <= 0)
                    throw new Exception("Não conseguiu identificar o código da filial");

                bplIdCD = Convert.ToInt32(Class.Geral.RetornaCodFilialCD(bplId.ToString()));

                sql = string.Format(@"
SELECT T0.*
FROM ""@CVA_CAR_OP"" T0
    INNER JOIN ""@CVA_CAR_PLAN1"" T1 ON T1.""U_Code"" = T0.""U_Code""
WHERE T1.""U_Lote"" = {0}
", lote);
                oRec.DoQuery(sql);
                oRec.MoveFirst();
                for (int i = 0; i < oRec.RecordCount; i++)
                {
                    itemPaiEstrutura = oRec.Fields.Item("U_ItemCode").Value.ToString();
                    double.TryParse(oRec.Fields.Item("U_Quant").Value.ToString(), out quantPai);
                    code = oRec.Fields.Item("Code").Value.ToString();
                    dataConsumo = Convert.ToDateTime(oRec.Fields.Item("U_Data_Consumo").Value.ToString());
                    sDataConsumo = dataConsumo.ToString("yyyyMMdd");

                    sql = string.Format(@"
SELECT 
	T5.""Code""
    , (T5.""Quantity"" / T4.""PlAvgSize"") AS ""Quant""
    ,T1.""U_CVA_Familia"" AS ""U_Familia""
    ,T1.""U_CVA_Subfamilia"" AS ""U_SubFamilia""
    ,T1.""U_CVA_Categoria"" AS ""U_Categoria""
    ,(  SELECT MAX(T21.""U_CVA_Data"")
        FROM ""@CVA_CAR_CAL"" T20 
            INNER JOIN ""@CVA_CAR_CAL1"" T21 ON T21.""DocEntry"" = T20.""DocEntry""
            INNER JOIN ""@CVA_CAR_CONFIG"" T22 ON T22.""U_BPLId"" = T20.""U_BPLId""
		WHERE T20.""U_BPLId"" = {0}
			AND T20.""U_Categoria"" = T1.""U_CVA_Categoria""
            AND T21.""U_CVA_Data"" <= ADD_DAYS('{2}', IFNULL(T22.""U_DiasPlanej"", 0) * -1)
	) AS ""Data""
FROM OITT T4
    INNER JOIN ITT1 T5 ON T5.""Father"" = T4.""Code""
    INNER JOIN OITM T1 ON T1.""ItemCode"" = T5.""Code""
WHERE T4.""Code"" = '{1}'
", bplId, itemPaiEstrutura, sDataConsumo);
                    table = Class.Conexao.ExecuteSqlDataTable(sql);

                    foreach (System.Data.DataRow linha in table.Rows)
                    {
                        chave = RetornaNextCode("@CVA_CAR_OP1");
                        itemCode = linha["Code"].ToString();
                        familia = linha["U_Familia"].ToString();
                        subFamilia = linha["U_SubFamilia"].ToString();
                        categoria = linha["U_Categoria"].ToString();
                        double.TryParse(linha["Quant"].ToString(), out aux);
                        quant = quantPai * aux;
                        dataEntrega = Convert.ToDateTime(linha["Data"].ToString());
                        sDataEntrega = dataEntrega.ToString("yyyyMMdd");

                        sql = string.Format(@"
SELECT COUNT(1) AS ""Contador""
FROM OITM T6
    INNER JOIN ""@CVA_CAR_INS1"" T9 ON T9.""U_CVA_ItemCode"" = T6.""ItemCode""
    INNER JOIN ""@CVA_CAR_INS"" T7 ON T7.""DocEntry"" = T9.""DocEntry""
WHERE T7.""U_BPLId"" = {0}
    AND T6.""ItemCode"" = '{1}'
    AND(
            ((T9.""U_CVA_ItmsGrpCod"" = T6.""ItmsGrpCod"") AND(IFNULL(T9.""U_CVA_ItemCode"", '') = ''))
            OR(IFNULL(T9.""U_CVA_ItemCode"", '') = T6.""ItemCode"")
            OR(IFNULL(T9.""U_CVA_Familia"", '') = T6.""U_CVA_Familia"")
            OR(IFNULL(T9.""U_CVA_SFamilia"", '') = T6.""U_CVA_Subfamilia"")
        )
", bplId, itemCode);

                        s = Class.Conexao.ExecuteSqlScalar(sql).ToString();
                        if (s == "0")
                        {
                            origem = "CD";
                            depositoPadrao = Class.Geral.RetornaCodDepPadrao(bplIdCD.ToString());
                        }
                        else
                        {
                            origem = "FI";
                            depositoPadrao = Class.Geral.RetornaCodDepPadrao(bplId.ToString());
                        }

                        sql = string.Format(@"
INSERT INTO ""@CVA_CAR_OP1"" (
""Code"", ""Name"", ""U_ID_OP"", ""U_ItemCode"", ""U_Quant"", ""U_Origem"", ""U_Data_Consumo"", ""U_Data_Entrega"", ""U_DflWhs"", ""U_Lote"",  ""U_Familia"", ""U_SubFamilia"", ""U_Categoria"") VALUES (
{0}     , '{1}'   , {2}        , '{3}'         , {4}        , '{5}'       , '{6}'             , '{7}'             , '{8}'       , {9}       , '{10}'        , '{11}'          , '{12}' )",
        chave
        , chave.ToString()
        , code
        , itemCode
        , quant.ToString().Replace(',', '.')
        , origem
        , sDataConsumo
        , sDataEntrega
        , depositoPadrao
        , lote
        , familia
        , subFamilia
        , categoria
        );
                        oRec1.DoQuery(sql);
                    }
                    oRec.MoveNext();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //==================================================================================================================================//
        static public int RetornaLote(int filial, DateTime dtIni, DateTime dtFim, string produtoIni, string produtoFim)
        //==================================================================================================================================//
        {
            int retorno = 0;
            string sql, dataDe, dataAte, now;

            try
            {
                dataDe = dtIni.ToString("yyyyMMdd");
                dataAte = dtFim.ToString("yyyyMMdd");
                now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = $@"select ifnull(max(""Code""), 0) + 1 AS ""Lote"" FROM ""@CVA_CAR_LOTE""";
                oRec.DoQuery(sql);
                retorno = Convert.ToInt32(oRec.Fields.Item("Lote").Value);

                sql = string.Format(@"
INSERT INTO ""@CVA_CAR_LOTE"" (
""Code"", ""Name"", ""U_Lote"", ""U_ID_Filial"", ""U_CreateDate"", ""U_DataDe"", ""U_DataAte"", ""U_ProdutoIni"", ""U_ProdutoFim"") VALUES (
'{0}'   , '{1}'   , {2}       , '{3}'          , '{4}'           , '{5}'       , '{6}'        , '{7}'           , '{8}')",
retorno, retorno, retorno, filial, now, dataDe, dataAte, produtoIni, produtoFim);

                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retorno;
        }

        //==================================================================================================================================//
        static public int RetornaNextCode(string tabela)
        //==================================================================================================================================//
        {
            int retorno = 0;
            string sql;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"select ifnull(max(""Code""), 0) + 1 AS ""Code"" FROM ""{0}"" ", tabela);
                oRec.DoQuery(sql);
                retorno = Convert.ToInt32(oRec.Fields.Item("Code").Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retorno;
        }

        //==================================================================================================================================//
        static public string RetornaCodDepPadrao(string filial)
        //==================================================================================================================================//
        {
            string sql, cod = "";

            try
            {
                SAPbobsCOM.Recordset oRec2 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"SELECT IFNULL(""DflWhs"", '') AS ""DflWhs"" FROM ""OBPL"" WHERE ""BPLId"" = {0} ", filial);
                oRec2.DoQuery(sql);

                if (oRec2.RecordCount > 0)
                {
                    cod = oRec2.Fields.Item("DflWhs").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cod;
        }

        //==================================================================================================================================//
        static public string RetornaCodFilialCD(string filial)
        //==================================================================================================================================//
        {
            string sql, cod = "";

            try
            {
                SAPbobsCOM.Recordset oRec2 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"
SELECT IFNULL(""U_BPLId_CD"", '') AS ""U_BPLId_CD""
FROM ""@CVA_CAR_CONFIG""
WHERE ""U_BPLId"" = {0}
", filial);
                oRec2.DoQuery(sql);

                if (oRec2.RecordCount > 0)
                {
                    cod = oRec2.Fields.Item("U_BPLId_CD").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cod;
        }























        //==================================================================================================================================//
        static public void SubirPlanejamentoX(System.Data.DataRow linha)
        //==================================================================================================================================//
        {
            string sql, bplName = "", codigo2PN = "", msg = "", sVigencia, sDataRef;
            System.Data.DataTable table = null;
            int maxCode, diasPlanej = -1, bplId, precoUnit = 0, usgTransf = 0;
            DateTime vigencia, dataRef;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"
SELECT T8.""BPLId""
FROM ""@CVA_PLANEJAMENTO"" T0
    LEFT JOIN ""OOAT"" T12 ON T12.""Number"" = T0.""U_CVA_ID_CONTRATO""
    LEFT JOIN ""OBPL"" T8 ON T8.""BPLId"" = T12.""U_CVA_FILIAL""
WHERE
    T0.""Code"" = {0}
", linha["Code"].ToString());
                bplId = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());

                if (bplId > 0)
                {
                    // ************************** TROCAR @CVA_CAR_CONF PARA @CVA_CAR_CONFIG
                    sql = string.Format(@"
SELECT TOP 1 
    T0.""BPLName""
    ,IFNULL(T0.""U_CVA_DiasPlanej"", -1) AS ""U_CVA_DiasPlanej""
    ,IFNULL(T0.""U_CVA_Codigo2PN"", '') AS ""U_CVA_Codigo2PN""
    ,IFNULL(T2.""U_PrecoUnit"", 0) AS ""U_PrecoUnit""
    ,IFNULL(T2.""U_UsgTransf"", 0) AS ""U_UsgTransf""
FROM OBPL T0
	LEFT JOIN ""@CVA_CAR_CONF"" T1 ON T1.""U_BPLId"" = T0.""BPLId""
	LEFT JOIN ""@CVA_CAR_CONF1"" T2 ON T2.""DocEntry"" = T1.""DocEntry""
WHERE T0.""BPLId"" = {0}
", bplId);
                    table = Class.Conexao.ExecuteSqlDataTable(sql);

                    diasPlanej = Convert.ToInt32(table.Rows[0]["U_CVA_DiasPlanej"].ToString());
                    bplName = table.Rows[0]["BPLName"].ToString();
                    codigo2PN = table.Rows[0]["U_CVA_Codigo2PN"].ToString();
                    precoUnit = Convert.ToInt32(table.Rows[0]["U_PrecoUnit"].ToString());
                    usgTransf = Convert.ToInt32(table.Rows[0]["U_UsgTransf"].ToString());

                    if (diasPlanej == -1)
                        msg += "Dias de segurança não cadastrado nas configurações de filial" + Environment.NewLine;
                    if (codigo2PN == "")
                        msg += "Código do PN da filial não cadastrado nas configurações de filial" + Environment.NewLine;
                    if (precoUnit == 0)
                        msg += "Seleção de preço unitário não cadastrado nas configurações da consolidação" + Environment.NewLine;
                    if (usgTransf == 0)
                        msg += "Usage de transferência não cadastrado nas configurações da consolidação" + Environment.NewLine;
                }
                else
                    msg += "Filial não identificada no planejamento" + Environment.NewLine;

                if (!string.IsNullOrEmpty(msg))
                    throw new Exception(msg);

                //sql = string.Format(@"select ifnull(max(""Code""), 0) + 1 AS ""Max"" FROM ""@CVA_CAR_CONSOL""");
                //oRec.DoQuery(sql);
                //maxCode = Convert.ToInt32(oRec.Fields.Item("Max").Value);

                vigencia = Convert.ToDateTime(linha["U_CVA_VIGENCIA_CONTR"].ToString());
                sVigencia = vigencia.ToString("yyyy-MM-dd");

                dataRef = Convert.ToDateTime(linha["U_CVA_DATA_REF"].ToString());
                sDataRef = dataRef.ToString("yyyy-MM-dd");

                sql = string.Format(@"
INSERT INTO ""@CVA_CAR_PLAN"" (
""Code"", ""Name"", ""U_CVA_ID_CLIENTE"", ""U_CVA_ID_CONTRATO"", ""U_CVA_ID_MODEL_CARD"", ""U_CVA_VIGENCIA_CONTR"", ""U_CVA_DES_CLIENTE"", ""U_CVA_DES_MODELO_CARD"", ""U_CVA_DATA_REF"", ""U_CVA_ID_SERVICO"", ""U_CVA_DES_SERVICO"", ""U_CVA_ID_G_SERVICO"", ""U_CVA_DES_G_SERVICO"", ""U_CVA_Dias_Planej"", ""U_CVA_BPLId"", ""U_CVA_BPLName"", ""U_CVA_Codigo2PN"", ""U_CVA_PrecoUnit"", ""U_CVA_UsgTransf"") VALUES (
{0}     , '{1}'   , '{2}'               , '{3}'                , '{4}'                  , '{5}'                   , '{6}'                , '{7}'                    , '{8}'             , '{9}'               , '{10}'               , '{11}'                , '{12}'                 , {13}                 , {14}           , '{15}'           , '{16}'             , {17}               , {18} )",
linha["Code"].ToString()
, linha["Name"].ToString()
, linha["U_CVA_ID_CLIENTE"].ToString()
, linha["U_CVA_ID_CONTRATO"].ToString()
, linha["U_CVA_ID_MODEL_CARD"].ToString()
, sVigencia
, linha["U_CVA_DES_CLIENTE"].ToString()
, linha["U_CVA_DES_MODELO_CARD"].ToString()
, sDataRef
, linha["U_CVA_ID_SERVICO"].ToString()
, linha["U_CVA_DES_SERVICO"].ToString()
, linha["U_CVA_ID_G_SERVICO"].ToString()
, linha["U_CVA_DES_G_SERVICO"].ToString()
, diasPlanej
, bplId
, bplName
, codigo2PN
, precoUnit
, usgTransf
);

                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        static public void SubirLnPlanejamentoX(int bplId, System.Data.DataRow linha)
        //==================================================================================================================================//
        {
            string sql = "", msg = "", sDataConsumo;
            System.Data.DataTable table = null;
            DateTime dataConsumo;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                dataConsumo = Convert.ToDateTime(linha["DataPlanej"].ToString());
                sDataConsumo = dataConsumo.ToString("yyyy-MM-dd");

                sql = string.Format(@"
INSERT INTO ""@CVA_CAR_PLAN1"" (
""Code"", ""Name"", ""U_CVA_PLAN_ID"", ""U_CVA_TIPO_PRATO"", ""U_CVA_TIPO_PRATO_DES"", ""U_CVA_INSUMO"", ""U_CVA_INSUMO_DES"", ""U_CVA_PERCENT"", ""U_CVA_QTD"", ""U_CVA_QTD_ORI"", ""U_CVA_CUSTO_ITEM"", ""U_CVA_TOTAL"", ""U_CVA_MODELO_LIN_ID"", ""U_CVA_DIA_SEMANA"", ""U_CVA_Data_Consumo"") VALUES (
{0}     , '{1}'   , '{2}'            , '{3}'               , '{4}'                   , '{5}'           , '{6}'               , {7}              , {8}          , {9}              , {10}                , {11}           , '{12}'                 , '{13}'              , '{14}' )",
linha["Code"].ToString()
, linha["Name"].ToString()
, linha["U_CVA_PLAN_ID"].ToString()
, linha["U_CVA_TIPO_PRATO"].ToString()
, linha["U_CVA_TIPO_PRATO_DES"].ToString()
, linha["U_CVA_INSUMO"].ToString()
, linha["U_CVA_INSUMO_DES"].ToString()
, linha["U_CVA_PERCENT"].ToString().Replace(',', '.')
, linha["U_CVA_QTD"].ToString()
, linha["U_CVA_QTD_ORI"].ToString()
, linha["U_CVA_CUSTO_ITEM"].ToString().Replace(',', '.')
, linha["U_CVA_TOTAL"].ToString().Replace(',', '.')
, linha["U_CVA_MODELO_LIN_ID"].ToString()
, linha["U_CVA_DIA_SEMANA"].ToString()
, sDataConsumo
);
                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                msg = sql;
                throw ex;
            }
        }

        //==================================================================================================================================//
        static public void SubirOPPlanejamentoX(int bplId, string code)
        //==================================================================================================================================//
        {
            string sql = "", msg = "", sDataConsumo;
            System.Data.DataTable table = null;
            DateTime dataConsumo;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);


                /*
                dataConsumo = Convert.ToDateTime(linha["DataPlanej"].ToString());
                sDataConsumo = dataConsumo.ToString("yyyy-MM-dd");

                sql = string.Format(@"
INSERT INTO ""@CVA_CAR_PLAN1"" (
""Code"", ""Name"", ""U_CVA_PLAN_ID"", ""U_CVA_TIPO_PRATO"", ""U_CVA_TIPO_PRATO_DES"", ""U_CVA_INSUMO"", ""U_CVA_INSUMO_DES"", ""U_CVA_PERCENT"", ""U_CVA_QTD"", ""U_CVA_QTD_ORI"", ""U_CVA_CUSTO_ITEM"", ""U_CVA_TOTAL"", ""U_CVA_MODELO_LIN_ID"", ""U_CVA_DIA_SEMANA"", ""U_CVA_Data_Consumo"") VALUES (
{0}     , '{1}'   , '{2}'            , '{3}'               , '{4}'                   , '{5}'           , '{6}'               , {7}              , {8}          , {9}              , {10}                , {11}           , '{12}'                 , '{13}'              , '{14}' )",
linha["Code"].ToString()
, linha["Name"].ToString()
, linha["U_CVA_PLAN_ID"].ToString()
, linha["U_CVA_TIPO_PRATO"].ToString()
, linha["U_CVA_TIPO_PRATO_DES"].ToString()
, linha["U_CVA_INSUMO"].ToString()
, linha["U_CVA_INSUMO_DES"].ToString()
, linha["U_CVA_PERCENT"].ToString().Replace(',', '.')
, linha["U_CVA_QTD"].ToString()
, linha["U_CVA_QTD_ORI"].ToString()
, linha["U_CVA_CUSTO_ITEM"].ToString().Replace(',', '.')
, linha["U_CVA_TOTAL"].ToString().Replace(',', '.')
, linha["U_CVA_MODELO_LIN_ID"].ToString()
, linha["U_CVA_DIA_SEMANA"].ToString()
, sDataConsumo
);
                oRec.DoQuery(sql);
                */
            }
            catch (Exception ex)
            {
                msg = sql;
                throw ex;
            }
        }

        //==================================================================================================================================//
        static public void CFL()    // Não é mais usado
        //==================================================================================================================================//
        {
            try
            {
                if ((Class.CFL.tabela != "") && (Class.CFL.chave != "") && (Class.CFL.campo1 != ""))
                {
                    FormCFL cfl = new FormCFL();
                    cfl.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }







    }
}
