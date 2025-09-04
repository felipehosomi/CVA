using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace CVA.Apetit.Addon.Class
{
    class DB
    {
        //==================================================================================================================================//
        static public string UserFieldsBLL()
        //==================================================================================================================================//
        {
            string msg = "";

            try
            {
                #region [CVA] Campos Filial - OBPL
                AddUserField("OBPL", "CVA_IdPayTrack", "ID de Identificação PayTrack", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("OBPL", "CVA_Dim1Custo", "C.Custo da Empresa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8, null, null, null);
                AddUserField("OBPL", "CVA_Dim2Custo", "C.Custo da Filial ", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8, null, null, null);
                //AddUserField("OBPL", "CVA_DtConsFin", "Dt Término Consolidação ", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                //AddUserField("OBPL", "CVA_DiasPlanej", "Dt Planejamento X dias", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                #endregion

                #region [CVA] Campos Previsão - OFCT
                AddUserField("OFCT", "BPLId", "Filial", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                AddUserField("OFCT", "Lote", "Lote Consolidação", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);

                AddUserField("ORDR", "Lote", "Lote Consolidação", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("OWOR", "Lote", "Lote Consolidação", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                #endregion

                #region [CVA] Campos Previsão - OPRQ
                AddUserField("OPRQ", "IdPrevisao", "Id Previsao", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                #endregion

                #region [CVA] Campos Consolidação de Planejamento de Cardápio

                AddUserField("@CVA_LN_PLANEJAMENTO", "CVA_LOTE_CONSOL", "Lote Consolidado", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);

                //CreateUserTable("@CVA_CAR_CONF", "CVA-CARD-Conf Parâmetros", SAPbobsCOM.BoUTBTableType.bott_Document);
                //AddUserField("@CVA_CAR_CONF", "BPLId", "Filial ID", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                //AddUserField("@CVA_CAR_CONF", "BPLName", "Filial Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, null, null, null);
                
                //string[,] vvVale = new string[2, 2];
                //vvVale[0, 0] = "1";
                //vvVale[0, 1] = "Última Entrada";
                //vvVale[1, 0] = "2";
                //vvVale[1, 1] = "Custo Médio atual";

                //CreateUserTable("@CVA_CAR_CONF1", "CVA-CARD-Linha Parâmetros", SAPbobsCOM.BoUTBTableType.bott_DocumentLines);
                //AddUserField("@CVA_CAR_CONF1", "CardCodePN", "CardCode PN Filial", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                ////AddUserField("@CVA_CAR_CONF1", "CardCodePA", "CardCode Armazenagem" , SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                //AddUserField("@CVA_CAR_CONF1", "WhsCode", "Depósito Padrão Envio Direto", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 8, null, null, null);
                //AddUserField("@CVA_CAR_CONF1", "BPLId", "Código da Filial", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                //AddUserField("@CVA_CAR_CONF1", "UsgTransf", "Utilização para transferência", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                //AddUserField("@CVA_CAR_CONF1", "UsgRetorno", "Utilização Retorno de Armazenagem", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                //AddUserField("@CVA_CAR_CONF1", "UsgRemessa", "Utilização Remessa de Armazenagem", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                //AddUserField("@CVA_CAR_CONF1", "PrecoUnit", "Preço Unitário", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 1, vvVale, "2", null);
                //AddUserField("@CVA_CAR_CONF1", "DiasPlanej", "Dt Planejamento X dias", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                //AddUserField("@CVA_CAR_CONF1", "BPLId1", "Filial ID", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);

                CreateUserTable("@CVA_CAR_CONFIG", "CVA-CARD-Configuração", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                AddUserField("@CVA_CAR_CONFIG", "BPLId", "Filial ID", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONFIG", "CardCodePN", "CardCode PN Filial", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_CONFIG", "WhsCode", "Depósito Padrão Envio Direto", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 8, null, null, null);
                AddUserField("@CVA_CAR_CONFIG", "BPLId_CD", "Código da Filial CD", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                AddUserField("@CVA_CAR_CONFIG", "UsgTransf", "Utilização para transferência", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONFIG", "UsgRetorno", "Utilização Retorno de Armazenagem", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONFIG", "UsgRemessa", "Utilização Remessa de Armazenagem", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONFIG", "PrecoUnit", "Lista de Preços", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONFIG", "DiasPlanej", "Dt Planejamento X dias", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);

                CreateUserTable("@CVA_CAR_INS", "CVA-CARD-Definição Insumos", SAPbobsCOM.BoUTBTableType.bott_Document);
                AddUserField("@CVA_CAR_INS", "BPLId", "Filial", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                AddUserField("@CVA_CAR_INS", "BPLName", "Filial Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, null, null, null);

                CreateUserTable("@CVA_CAR_INS1", "CVA-CARD-Linha Insumos", SAPbobsCOM.BoUTBTableType.bott_DocumentLines);
                AddUserField("@CVA_CAR_INS1", "CVA_ItmsGrpCod", "Grupo de Item", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_INS1", "CVA_Familia", "Família", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                AddUserField("@CVA_CAR_INS1", "CVA_SFamilia", "Sub-Família", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                AddUserField("@CVA_CAR_INS1", "CVA_ItemCode", "Número do Item", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);

                CreateUserTable("@CVA_CAR_CAL", "CVA-CARD-Calendário Receb", SAPbobsCOM.BoUTBTableType.bott_Document);
                AddUserField("@CVA_CAR_CAL", "BPLId", "Filial", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                AddUserField("@CVA_CAR_CAL", "Period", "Periodicidade", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CAL", "Categoria", "Categoria", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);

                CreateUserTable("@CVA_CAR_CAL1", "CVA-CARD-Linha Calendário", SAPbobsCOM.BoUTBTableType.bott_DocumentLines);
                AddUserField("@CVA_CAR_CAL1", "CVA_Data", "Data", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);

                //CreateUserTable("@CVA_CAR_MEDIDA", "CVA-CARD-Medida Padrão", SAPbobsCOM.BoUTBTableType.bott_Document);
                //AddUserField("@CVA_CAR_MEDIDA", "ItemCode", "Produto", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                //AddUserField("@CVA_CAR_MEDIDA", "UomCode", "Produto", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                //AddUserField("@CVA_CAR_MEDIDA", "BaseQty", "Quant Base", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 10, null, null, null);

                CreateUserTable("@CVA_CAR_LOTE", "CVA-CARD-LOTE", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                AddUserField("@CVA_CAR_LOTE", "Lote", "Lote", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_LOTE", "ID_Filial", "ID Filial", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                AddUserField("@CVA_CAR_LOTE", "CreateDate", "CreateDate", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_LOTE", "DataDe", "Data De", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_LOTE", "DataAte", "Data Até", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_LOTE", "ProdutoIni", "Produto Ini", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                AddUserField("@CVA_CAR_LOTE", "ProdutoFim", "Produto Fim", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                AddUserField("@CVA_CAR_LOTE", "DtCancela", "Data Cancelado", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_LOTE", "UserCancela", "User Cancela", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_LOTE", "Cancelado", "Cancelado", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, null, null, null);
                AddUserField("@CVA_CAR_LOTE", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 1, null, null, null);

                string[,] vVale = new string[3, 2];
                vVale[0, 0] = "0";
                vVale[0, 1] = "Não processado";
                vVale[1, 0] = "1";
                vVale[1, 1] = "Processado";
                vVale[2, 0] = "2";
                vVale[2, 1] = "Erro";

                CreateUserTable("@CVA_CAR_CONSOL", "CVA-CARD-Consolidação", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                AddUserField("@CVA_CAR_CONSOL", "Lote", "Lote", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "Tipo", "Tipo (OP/PC/PV)", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "ID_Filial", "ID Filial", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "CreateDate", "CreateDate", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "Data", "Data Entrega", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "DataOrig", "Data Original", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "ItemCode", "Produto", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "Quant", "Quantidade", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 10, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "Origem", "Origem (CD/FI)", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "IdTurno", "Id Turno", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "IdServico", "IdServico", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "DocEntryPC", "Chave Previsao", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "DocEntryOP", "Chave OP", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "DocEntryPV", "Chave Pedido Venda", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "Cancelado", "Cancelado", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, null, null, null);
                AddUserField("@CVA_CAR_CONSOL", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 1, vVale, "0", null);
                AddUserField("@CVA_CAR_CONSOL", "Msg", "Erro", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 254, null, null, null);

                //string[] childTableName = new string[1];
                //string[] childObjectName = new string[1];
                //childTableName[0] = "CVA_CAR_CONF1";
                //childObjectName[0] = "O_CVA_CAR_CONF1";
                //AddUDO("O_CVA_CAR_CONF", "CVA_CAR_CONF", "Cardápio - Config", SAPbobsCOM.BoUDOObjType.boud_Document, childTableName, childObjectName);

                string[] childTableName = new string[1];
                string[] childObjectName = new string[1];
                childTableName[0] = "CVA_CAR_INS1";
                childObjectName[0] = "O_CVA_CAR_INS1";
                AddUDO("O_CVA_CAR_INS", "CVA_CAR_INS", "Cardápio - Insumos", SAPbobsCOM.BoUDOObjType.boud_Document, childTableName, childObjectName);

                //childTableName = new string[1];
                //childObjectName = new string[1];
                //childTableName[0] = "CVA_CAR_CAL1";
                //childObjectName[0] = "O_CVA_CAR_CAL1";
                //AddUDO("O_CVA_CAR_CAL", "CVA_CAR_CAL", "Cardápio - Calendário", SAPbobsCOM.BoUDOObjType.boud_Document, childTableName, childObjectName);

                //CreateUserTable("@CVA_CAR_TESTE", "CVA-CARD-TESTE", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                //AddUserField("@CVA_CAR_TESTE", "CardCode", "CardCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                //AddUserField("@CVA_CAR_TESTE", "BPLId", "Filial", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                //AddUserField("@CVA_CAR_TESTE", "Data", "Data Consumo", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                //AddUserField("@CVA_CAR_TESTE", "ItemCode", "Produto", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                //AddUserField("@CVA_CAR_TESTE", "Quant", "Quantidade", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 10, null, null, null);
                //AddUserField("@CVA_CAR_TESTE", "IdTurno", "Id Turno", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                //AddUserField("@CVA_CAR_TESTE", "IdServico", "IdServico", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                //AddUserField("@CVA_CAR_TESTE", "Flag", "Flag Status", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, null, null, null);

                //string[,] vvVale = new string[2, 2];
                //vvVale[0, 0] = "1";
                //vvVale[0, 1] = "Última Entrada";
                //vvVale[1, 0] = "2";
                //vvVale[1, 1] = "Custo Médio atual";

                CreateUserTable("@CVA_CAR_PLAN", "CVA_CAR_Planejamento", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                AddUserField("@CVA_CAR_PLAN", "Code", "Code do Planejamento", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_ID_CLIENTE", "ID Cliente", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_ID_CONTRATO", "ID Contrato", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_ID_MODEL_CARD", "ID Modelo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_VIGENCIA_CONTR", "Vigência", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_DES_CLIENTE", "Descr. Cliente", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 254, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_DES_MODELO_CARD", "Descr. Modelo Card", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 254, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_DATA_REF", "Data Referência", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_ID_SERVICO", "ID Servico", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_DES_SERVICO", "Descr. Serviço", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 254, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_ID_G_SERVICO", "ID Grupo Serviço", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "CVA_DES_G_SERVICO", "Descr. Grupo Serviço", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 254, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "Dias_Planej", "Dias Segurança", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "BPLId", "ID Filial", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "BPLIdCD", "ID CD", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "Codigo2PN", "ID PN Filial", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "PrecoUnit", "Preço Unitário", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN", "UsgTransf", "Utilização p/ transferência", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);

                CreateUserTable("@CVA_CAR_PLAN1", "CVA_CAR_LN_Planejamento", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                AddUserField("@CVA_CAR_PLAN1", "Code", "Code do LN_Planejamento", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_PLAN_ID", "ID Planejamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_TIPO_PRATO", "Tipo de Prato", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 254, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_TIPO_PRATO_DES", "Descr. Tipo Prato", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 254, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_INSUMO", "ID Insumo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_INSUMO_DES", "Descr. Insumo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 254, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_PERCENT", "%", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_QTD_ORI", "Qtd Original", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_CUSTO_ITEM", "Custo Médio", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_TOTAL", "Total", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_MODELO_LIN_ID", "ID Lin Model", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "CVA_DIA_SEMANA", "Dia Semana", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "QTD", "Quantidade", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "Data_Consumo", "Data Consumo", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_PLAN1", "Lote", "Lote", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);

                CreateUserTable("@CVA_CAR_OP", "CVA_CAR_OP", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                AddUserField("@CVA_CAR_OP", "Code", "Code do LN_Planejamento", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP", "ID_PLAN1", "ID Plan1", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_OP", "Data_Consumo", "Data Consumo", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP", "Quant", "Quantidade", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 10, null, null, null);
                AddUserField("@CVA_CAR_OP", "DflWhs", "Cod Depósito Padrão", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 8, null, null, null);
                AddUserField("@CVA_CAR_OP", "Servico", "Servico", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP", "Turno", "Turno", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP", "DocEntryOP", "DocEntry OP", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);

                CreateUserTable("@CVA_CAR_OP1", "CVA_CAR_OP1", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                AddUserField("@CVA_CAR_OP1", "ID_OP", "ID OP", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP1", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 15, null, null, null);
                AddUserField("@CVA_CAR_OP1", "Quant", "Quantidade", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 10, null, null, null);
                AddUserField("@CVA_CAR_OP1", "Origem", "Origem", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, null, null, null);
                AddUserField("@CVA_CAR_OP1", "Data_Consumo", "Data Consumo", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP1", "Data_Entrega", "Data Entrega", SAPbobsCOM.BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP1", "DflWhs", "Cod Depósito Padrão", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 8, null, null, null);
                AddUserField("@CVA_CAR_OP1", "Lote", "Lote Consolidado", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP1", "Familia", "Familia", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                AddUserField("@CVA_CAR_OP1", "SubFamilia", "Sub-Familia", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, null, null, null);
                AddUserField("@CVA_CAR_OP1", "Categoria", "Categoria", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP1", "DocEntryPC", "DocEntry PC", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);
                AddUserField("@CVA_CAR_OP1", "DocEntryPV", "DocEntry PV", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, null, null, null);


                #endregion


            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            finally
            {
                //Class.Conexao.FecharConexao("Stage");
            }

            return msg;

        }

        private static void CreateUserTable(string UserTableName, string UserTableDesc, BoUTBTableType UserTableType)
        {
            int iCodErro;
            string sErrMsg = "";

            UserTableName = UserTableName.Replace("@", "").Replace("[", "").Replace("]", "").Trim();
            UserTablesMD oUserTableMD = (UserTablesMD)Conexao.oCompany.GetBusinessObject(BoObjectTypes.oUserTables);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTableMD);
            oUserTableMD = null;
            GC.Collect();
            oUserTableMD = (UserTablesMD)Conexao.oCompany.GetBusinessObject(BoObjectTypes.oUserTables);

            try
            {
                if (!oUserTableMD.GetByKey(UserTableName))
                {
                    oUserTableMD.TableName = UserTableName;
                    oUserTableMD.TableDescription = UserTableDesc;
                    oUserTableMD.TableType = UserTableType;

                    if (oUserTableMD.Add() != 0)
                    {
                        Conexao.oCompany.GetLastError(out iCodErro, out sErrMsg);
                        Conexao.sbo_application.StatusBar.SetText($@"Erro ao criar tabela - {UserTableName} - {sErrMsg}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Marshal.ReleaseComObject(oUserTableMD);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTableMD);
                oUserTableMD = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private static void AddUserField(string NomeTabela, string NomeCampo, string DescCampo, SAPbobsCOM.BoFieldTypes Tipo, SAPbobsCOM.BoFldSubTypes SubTipo, Int16 Tamanho, string[,] valoresValidos, string valorDefault, string linkedTable)
        {
            int lErrCode;
            string sErrMsg = "";

            string strSql = string.Format(@"select COUNT(*)  
                                                from CUFD 
                                                where ""TableID"" = '{0}' 
                                                    and ""AliasID"" = '{1}'", NomeTabela, NomeCampo);
            //0 - Campo Não exite
            //1 - Campos Existe
            int resultado = (int)Conexao.ExecuteSqlScalar(strSql);
            if (resultado == 0)
            {
                try
                {
                    Conexao.sbo_application.StatusBar.SetText($@"Criando Campo {NomeTabela}.{DescCampo}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    //string sSquery = "SELECT ""[name]"" FROM syscolumns WHERE ""[name]"" = 'U_" + NomeCampo + " ' and id = (SELECT id FROM sysobjects WHERE type = 'U'AND [NAME] = '" + NomeTabela.Replace("[", "").Replace("]", "") + "')";
                    //object oResult = Conexao.ExecuteSqlScalar(sSquery);
                    //if (oResult != null) return;

                    SAPbobsCOM.UserFieldsMD oUserField;
                    oUserField = (SAPbobsCOM.UserFieldsMD)Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                    oUserField.TableName = NomeTabela.Replace("@", "").Replace("[", "").Replace("]", "").Trim();
                    oUserField.Name = NomeCampo;
                    oUserField.Description = DescCampo;
                    oUserField.Type = Tipo;
                    oUserField.SubType = SubTipo;
                    oUserField.DefaultValue = valorDefault;
                    if (!string.IsNullOrEmpty(linkedTable)) oUserField.LinkedTable = linkedTable;

                    //adicionar valores válidos
                    if (valoresValidos != null)
                    {
                        Int32 qtd = valoresValidos.GetLength(0);
                        if (qtd > 0)
                        {
                            for (int i = 0; i < qtd; i++)
                            {
                                oUserField.ValidValues.Value = valoresValidos[i, 0];
                                oUserField.ValidValues.Description = valoresValidos[i, 1];
                                oUserField.ValidValues.Add();
                            }
                        }
                    }

                    if (Tamanho != 0)
                        oUserField.EditSize = Tamanho;

                    try
                    {
                        oUserField.Add();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserField);
                        oUserField = null;
                        Conexao.oCompany.GetLastError(out lErrCode, out sErrMsg);
                        if (lErrCode != 0)
                        {
                            Conexao.sbo_application.StatusBar.SetText($@"Erro ao criar campo - {NomeCampo} - {sErrMsg}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                            //throw new Exception(sErrMsg);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    oUserField = null;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }



        private static void InsertUserField(string TableName, string FieldName, string FieldDescription, BoFieldTypes oType, BoFldSubTypes oSubType, int FieldSize, bool MandatoryYN = false, string DefaultValue = "", string linkedTable = "")
        {
            int iCodErro;
            string sErrMsg = "";

            UserFieldsMD oUserFieldsMD = ((UserFieldsMD)(Conexao.oCompany.GetBusinessObject(BoObjectTypes.oUserFields)));

            try
            {
                if (FieldDescription.Length > 30)
                {
                    FieldDescription = FieldDescription.Substring(0, 30);
                }

                string Sql = @"SELECT ""FieldID"" FROM CUFD WHERE ""TableID"" = '{0}' AND ""AliasID"" = '{1}' ";
                Sql = String.Format(Sql, TableName.ToUpper(), FieldName);
                string FieldId = QueryForValue(Sql);

                if (FieldId != null)
                {
                    return;
                }

                oUserFieldsMD.TableName = TableName.Replace("@", "").Replace("[", "").Replace("]", "").Trim();
                oUserFieldsMD.Name = FieldName;
                oUserFieldsMD.Description = FieldDescription;
                oUserFieldsMD.Type = oType;
                oUserFieldsMD.SubType = oSubType;
                oUserFieldsMD.Mandatory = GetSapBoolean(MandatoryYN);

                if (!String.IsNullOrEmpty(DefaultValue))
                {
                    oUserFieldsMD.DefaultValue = DefaultValue;
                }
                if (!String.IsNullOrEmpty(linkedTable))
                {
                    oUserFieldsMD.LinkedTable = linkedTable;
                }

                if (FieldSize > 0)
                    oUserFieldsMD.EditSize = FieldSize;

                if (oUserFieldsMD.Add() != 0)
                {
                    Conexao.oCompany.GetLastError(out iCodErro, out sErrMsg);
                    Conexao.sbo_application.StatusBar.SetText($@"Erro ao criar tabela - {FieldName} - {sErrMsg}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private static void AddValidValueToUserField(string TableName, string FieldName, string Value, string Description)
        {
            AddValidValueToUserField(TableName.ToUpper(), FieldName, Value, Description, false);
        }

        private static void AddValidValueToUserField(string TableName, string FieldName, string Value, string Description, bool IsDefault)
        {
            int iCodErro;
            string sErrMsg = "";

            UserFieldsMD oUserFieldsMD = ((UserFieldsMD)(Conexao.oCompany.GetBusinessObject(BoObjectTypes.oUserFields)));
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            oUserFieldsMD = null;
            oUserFieldsMD = ((UserFieldsMD)(Conexao.oCompany.GetBusinessObject(BoObjectTypes.oUserFields)));

            try
            {
                string sql = @" SELECT UFD1.""IndexID"" FROM CUFD
                            INNER JOIN UFD1 
                                ON CUFD.""TableID"" = UFD1.""TableID"" 
                               AND CUFD.""FieldID"" = UFD1.""FieldID""
                         WHERE CUFD.""TableID"" = '{0}' 
                         AND CUFD.""AliasID""= '{1}' 
                         AND UFD1.""FldValue"" = '{2}'";
                sql = String.Format(sql, TableName, FieldName.Replace("U_", ""), Value);

                string IndexId = QueryForValue(sql);

                if (IndexId != null)
                {
                    return;
                }

                sql = @" SELECT ""FieldID"" FROM CUFD WHERE ""TableID"" = '{0}' AND ""AliasID"" = '{1}' ";
                sql = String.Format(sql, TableName, FieldName.Replace("U_", ""));
                string FieldId = QueryForValue(sql);

                if (!oUserFieldsMD.GetByKey(TableName, Convert.ToInt32(FieldId)))
                {
                    throw new Exception("Campo não encontrado!");
                }

                if (!String.IsNullOrEmpty(oUserFieldsMD.ValidValues.Value))
                {
                    oUserFieldsMD.ValidValues.Add();
                }

                oUserFieldsMD.ValidValues.Value = Value;
                oUserFieldsMD.ValidValues.Description = Description;

                if (IsDefault)
                    oUserFieldsMD.DefaultValue = Value;

                if (oUserFieldsMD.Update() != 0)
                {
                    Conexao.oCompany.GetLastError(out iCodErro, out sErrMsg);
                    Conexao.sbo_application.StatusBar.SetText($@"Erro ao criar tabela - {FieldName} - {sErrMsg}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }

            }
            catch (Exception ex)
            {
                //Log.AppendFormat("Erro geral ao inserir valor válido: {0}", ex.Message);
                throw ex;
            }
            finally
            {
                //Marshal.ReleaseComObject(oUserFieldsMD);
                //oUserFieldsMD = null;
                //GC.Collect();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                Conexao.oCompany.GetLastError(out iCodErro, out sErrMsg);
                if (iCodErro != 0)
                {
                    Conexao.sbo_application.StatusBar.SetText($@"Erro ao criar campo - {FieldName} - {sErrMsg}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    //throw new Exception(sErrMsg);
                }
            }
        }

        private static void AddUDO(string sUDO, string sTable, string sDescricaoUDO, SAPbobsCOM.BoUDOObjType oBoUDOObjType, string[] childTableName, string[] childObjectName)
        {
            int lRetCode = 0;
            int iTabelasFilhas = 0;
            string sErrMsg = "";
            string sQuery = "";
            bool bUpdate = false;
            bool bExisteColuna = false;
            bool bExisteTabelaFilha = false;

            SAPbobsCOM.UserObjectsMD oUserObjectMD = null;

            oUserObjectMD = (SAPbobsCOM.UserObjectsMD)Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);

            System.Data.DataTable tb = new System.Data.DataTable();

            try
            {
                if (oUserObjectMD.GetByKey(sUDO))
                {
                    return;
                }

                oUserObjectMD.CanCancel = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanClose = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanDelete = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanFind = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanYearTransfer = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.ManageSeries = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.Code = sUDO;
                oUserObjectMD.Name = sDescricaoUDO;
                oUserObjectMD.ObjectType = oBoUDOObjType;
                oUserObjectMD.TableName = sTable;

                oUserObjectMD.FindColumns.ColumnAlias = "DocEntry";
                oUserObjectMD.FindColumns.ColumnDescription = "DocEntry";
                oUserObjectMD.FindColumns.Add();


                //sQuery = String.Format("SELECT  COLUNAS.NAME AS COLUNA " + Environment.NewLine +
                //                       " FROM SYSOBJECTS AS TABELAS," + Environment.NewLine +
                //                       "      SYSCOLUMNS AS COLUNAS" + Environment.NewLine +
                //                       " WHERE " + Environment.NewLine +
                //                       "     TABELAS.ID = COLUNAS.ID" + Environment.NewLine +
                //                       "     AND TABELAS.NAME = '@{0}' and (left(COLUNAS.NAME,2)='U_' or COLUNAS.NAME IN ('DocEntry'))", sTable);

                //tb = B1Connections.ExecuteSqlDataTable(sQuery);

                //int count = 0;
                //foreach (System.Data.DataRow oRow in tb.Rows)
                //{
                //    bExisteColuna = false;
                //    //verificar se existe coluna
                //    for (int g = 0; g < oUserObjectMD.FindColumns.Count; g++)
                //    {
                //        oUserObjectMD.FindColumns.SetCurrentLine(g);
                //        if (oUserObjectMD.FindColumns.ColumnAlias == oRow["COLUNA"].ToString())
                //        {
                //            bExisteColuna = true;
                //            break;
                //        }
                //    }

                //    if (bExisteColuna == true)
                //    {
                //        oUserObjectMD.FindColumns.ColumnDescription = oRow["COLUNA"].ToString();
                //    }
                //    else
                //    {
                //        if (count > 0) oUserObjectMD.FindColumns.Add();
                //        oUserObjectMD.FindColumns.ColumnAlias = oRow["COLUNA"].ToString();
                //        oUserObjectMD.FindColumns.ColumnDescription = oRow["COLUNA"].ToString();
                //    }

                //    count++;
                //}

                //Adicionar tabelas filhas
                if (childObjectName != null)
                {
                    for (int x = 0; x < childObjectName.Length; x++)
                    {

                        iTabelasFilhas = oUserObjectMD.ChildTables.Count;
                        bExisteTabelaFilha = false;
                        for (int y = 0; y < iTabelasFilhas; y++)
                        {
                            oUserObjectMD.ChildTables.SetCurrentLine(y);
                            if (oUserObjectMD.ChildTables.TableName == childTableName[x])
                            {
                                bExisteTabelaFilha = true;
                                break;
                            }
                        }

                        if (bExisteTabelaFilha == false)
                        {
                            if (x > 0) oUserObjectMD.ChildTables.Add();
                            if (childObjectName[x] != "" && childTableName[x] != "")
                            {
                                oUserObjectMD.ChildTables.TableName = childTableName[x];
                                oUserObjectMD.ChildTables.ObjectName = childObjectName[x];
                            }
                        }

                    }
                }

                if (bUpdate)
                    lRetCode = oUserObjectMD.Update();
                else
                    lRetCode = oUserObjectMD.Add();

                // check for errors in the process
                if (lRetCode != 0)
                {
                    Conexao.oCompany.GetLastError(out lRetCode, out sErrMsg);
                }

            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show(e.ToString()); }


            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD);
            oUserObjectMD = null;
            tb.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        public static string QueryForValue(string Sql)
        {
            Recordset oRecordset = (Recordset)(Conexao.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset));
            string Retorno = null;
            try
            {
                // Sql = SBOApp.TranslateToHana(Sql);
                oRecordset.DoQuery(Sql);

                // Executa e, caso exista ao menos um registro, devolve o mesmo.
                // retorna sempre o primeiro campo da consulta (SEMPRE)
                if (!oRecordset.EoF)
                {
                    Retorno = oRecordset.Fields.Item(0).Value.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Marshal.ReleaseComObject(oRecordset);
                oRecordset = null;
                GC.Collect();

            }

            return Retorno;
        }

        public static BoYesNoEnum GetSapBoolean(bool Variavel)
        {
            if (Variavel)
                return BoYesNoEnum.tYES;
            else
                return BoYesNoEnum.tNO;

        }

        public static void CreateUserObject(string ObjectName, string ObjectDesc, string TableName, BoUDOObjType ObjectType, bool CanLog = false, bool CanYearTransfer = false)
        {
            CreateUserObject(ObjectName, ObjectDesc, TableName, ObjectType, CanLog, CanYearTransfer, false, false, false, true, true, 0, 0, null);
        }

        public static void CreateUserObject(string ObjectName, string ObjectDesc, string TableName, BoUDOObjType ObjectType, bool CanLog, bool CanYearTransfer, bool CanCancel, bool CanClose, bool CanCreateDefaultForm, bool CanDelete, bool CanFind, int FatherMenuId, int menuPosition, string srfFile = "", GenericModel findColumns = null)
        {
            // se não preenchido um table name separado, usa o mesmo do objeto
            if (String.IsNullOrEmpty(TableName))
                TableName = ObjectName;

            UserObjectsMD UserObjectsMD = (UserObjectsMD)Conexao.oCompany.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            // Remove a arroba do usertable Name
            TableName = TableName.Replace("@", "");

            bool bUpdate = UserObjectsMD.GetByKey(ObjectName);
            if (bUpdate) return;

            UserObjectsMD.Code = ObjectName;
            UserObjectsMD.Name = ObjectDesc;
            UserObjectsMD.ObjectType = ObjectType;
            UserObjectsMD.TableName = TableName;

            //UserObjectsMD.CanArchive = GetSapBoolean(CanArchive);
            UserObjectsMD.CanCancel = GetSapBoolean(CanCancel);
            UserObjectsMD.CanClose = GetSapBoolean(CanClose);
            UserObjectsMD.CanCreateDefaultForm = GetSapBoolean(CanCreateDefaultForm);
            UserObjectsMD.CanDelete = GetSapBoolean(CanDelete);
            UserObjectsMD.CanFind = GetSapBoolean(CanFind);
            UserObjectsMD.CanLog = GetSapBoolean(CanLog);
            UserObjectsMD.CanYearTransfer = GetSapBoolean(CanYearTransfer);

            if (CanFind)
            {
                UserObjectsMD.FindColumns.ColumnAlias = "Code";
                UserObjectsMD.FindColumns.ColumnDescription = "Código";
                UserObjectsMD.FindColumns.Add();
            }

            if (CanCreateDefaultForm)
            {
                UserObjectsMD.CanCreateDefaultForm = BoYesNoEnum.tYES;
                UserObjectsMD.CanCancel = GetSapBoolean(CanCancel);
                UserObjectsMD.CanClose = GetSapBoolean(CanClose);
                UserObjectsMD.CanDelete = GetSapBoolean(CanDelete);
                UserObjectsMD.CanFind = GetSapBoolean(CanFind);
                UserObjectsMD.ExtensionName = "";
                UserObjectsMD.OverwriteDllfile = BoYesNoEnum.tYES;
                UserObjectsMD.ManageSeries = BoYesNoEnum.tYES;
                UserObjectsMD.UseUniqueFormType = BoYesNoEnum.tYES;
                UserObjectsMD.EnableEnhancedForm = BoYesNoEnum.tNO;
                UserObjectsMD.RebuildEnhancedForm = BoYesNoEnum.tNO;
                UserObjectsMD.FormSRF = srfFile;

                UserObjectsMD.FormColumns.FormColumnAlias = "Code";
                UserObjectsMD.FormColumns.FormColumnDescription = "Código";
                UserObjectsMD.FormColumns.Add();

                if (findColumns != null && findColumns.Fields != null)
                {
                    foreach (KeyValuePair<string, object> pair in findColumns.Fields)
                    {
                        UserObjectsMD.FormColumns.FormColumnAlias = pair.Key;
                        UserObjectsMD.FormColumns.FormColumnDescription = pair.Value.ToString();
                        UserObjectsMD.FormColumns.Add();
                    }
                }

                if (findColumns != null && findColumns.Fields != null)
                {
                    foreach (KeyValuePair<string, object> pair in findColumns.Fields)
                    {
                        UserObjectsMD.FindColumns.ColumnAlias = pair.Key;
                        UserObjectsMD.FindColumns.ColumnDescription = pair.Value.ToString();
                        UserObjectsMD.FindColumns.Add();
                    }
                }

                UserObjectsMD.FatherMenuID = FatherMenuId;
                UserObjectsMD.Position = menuPosition;
                UserObjectsMD.MenuItem = BoYesNoEnum.tYES;
                UserObjectsMD.MenuUID = ObjectName;
                UserObjectsMD.MenuCaption = ObjectDesc;
            }

            //if (bUpdate)
            //{
            //    CodErro = UserObjectsMD.Update();
            //}
            //else
            //    CodErro = UserObjectsMD.Add();

            //ValidateAction();

            //Marshal.ReleaseComObject(UserObjectsMD);
            UserObjectsMD = null;
            //GC.Collect();
        }



    }
}
