using CVA.AddOn.Common;
using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Escoteiros.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            #region [CVA] Campos Doc Marketing

            //// Cadastro PN
            //userObjectController.InsertUserField("OCRD", "CVA_TipoPag", "Pagamento Antecipado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("OCRD", "CVA_TipoPag", "A", "Antecipado", true);
            //userObjectController.AddValidValueToUserField("OCRD", "CVA_TipoPag", "F", "Faturado");

            //userObjectController.InsertUserField("OCRD", "CVA_Distribuidor", "Distribuidor", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("OCRD", "CVA_Distribuidor", "Y", "Sim");
            //userObjectController.AddValidValueToUserField("OCRD", "CVA_Distribuidor", "N", "Não",true);

            //// Endereço PN
            //userObjectController.InsertUserField("CRD1", "CVA_AddresRef", "Referência Entrega", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);

            //// Cadastro de Item
            //userObjectController.InsertUserField("OITM", "CVA_ImprCodBar", "Iprimir Código de Barras", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("OITM", "CVA_ImprCodBar", "Y", "Sim", true);
            //userObjectController.AddValidValueToUserField("OITM", "CVA_ImprCodBar", "N", "Não");


            //// Pedido de Venda
            //userObjectController.InsertUserField("ORDR", "CVA_Picking", "Picking Confirmado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("ORDR", "CVA_Picking", "Y", "Sim");
            //userObjectController.AddValidValueToUserField("ORDR", "CVA_Picking", "N", "Não",true);

            //userObjectController.InsertUserField("ORDR", "CVA_User_1Conf", "Usuário Abertura Picking 1", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            //userObjectController.InsertUserField("ORDR", "CVA_Aber_1Conf", "Abertura Picking 1", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);
            //userObjectController.InsertUserField("ORDR", "CVA_Fin_1Conf",  "Finalização Picking 1", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);

            //userObjectController.InsertUserField("ORDR", "CVA_User_2Conf", "Usuário Abertura Picking 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            //userObjectController.InsertUserField("ORDR", "CVA_Aber_2Conf", "Abertura Picking 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);
            //userObjectController.InsertUserField("ORDR", "CVA_Fin_2Conf",  "Finalização Picking 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);

            //userObjectController.InsertUserField("ORDR", "CVA_Status_Picking", "Status Picking", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("ORDR", "CVA_Status_Picking", "1", "Inciado");
            //userObjectController.AddValidValueToUserField("ORDR", "CVA_Status_Picking", "2", "Pendente", true);
            //userObjectController.AddValidValueToUserField("ORDR", "CVA_Status_Picking", "3", "Finalizado");

            //userObjectController.InsertUserField("ORDR", "CVA_Picking_Impresso", "Picking Impresso", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("ORDR", "CVA_Picking_Impresso", "1", "Impresso");
            //userObjectController.AddValidValueToUserField("ORDR", "CVA_Picking_Impresso", "2", "Não Impresso", true);
            ////userObjectController.AddValidValueToUserField("ORDR", "CVA_Picking_Impresso", "3", "Finalizado");

            userObjectController.InsertUserField("ORDR", "CVA_DT_Envio_PLP", "Data Envio PLP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);

            //// Nota de Saída
            //userObjectController.InsertUserField("OINV", "CVA_CodRastreio", "Código do Rastreio", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 36);
            //userObjectController.InsertUserField("OINV", "CVA_NumIdlista", "Número da Lista de Postagem", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("OINV", "CVA_OrderNum", "Número Pedido InteliPost", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            //userObjectController.InsertUserField("OINV", "CVA_Intelipost", "Integração com InteliPost", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("OINV", "CVA_Intelipost", "S", "Sim", true);
            //userObjectController.AddValidValueToUserField("OINV", "CVA_Intelipost", "N", "Não");
            //userObjectController.InsertUserField("OINV", "CVA_Integrado", "Integrado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("OINV", "CVA_Integrado", "S", "Sim");
            //userObjectController.AddValidValueToUserField("OINV", "CVA_Integrado", "N", "Não", true);

            ////Depósito
            //userObjectController.InsertUserField("OWHS", "CVA_Piso", "Piso", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("OWHS", "CVA_Rua", "Rua", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            //userObjectController.InsertUserField("OWHS", "CVA_Estante", "Estante", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            //userObjectController.InsertUserField("OWHS", "CVA_Prateleira", "Prateleira", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            //userObjectController.InsertUserField("OWHS", "CVA_Escaninho", "Escaninho", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            //// Utilização
            //userObjectController.InsertUserField("OUSG", "CVA_TipoDocumento", "Tipo Documento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("OUSG", "CVA_TipoDocumento", "1", "NF Saída");
            //userObjectController.AddValidValueToUserField("OUSG", "CVA_TipoDocumento", "2", "Entrega");



            #endregion

            #region [CVA] Parametros Gerais            

            //userObjectController.CreateUserTable("CVA_ParGerais", "[CVA] Parametros Gerais ", BoUTBTableType.bott_MasterData);
            //userObjectController.InsertUserField("@CVA_ParGerais", "CVA_ParcAntecipado", "Pagamento Antecipado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_ParGerais", "CVA_ParcFaturado", "Pagamento Faturado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_ParGerais", "CVA_CaminhoXML", "Caminho XML", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            //userObjectController.CreateUserObject("CVA_ParGerais", "[CVA] Parametros Gerais", "@CVA_ParGerais", BoUDOObjType.boud_MasterData);
            //userObjectController.MakeFieldsSearchable("@CVA_ParGerais");
            #endregion

            #region [CVA] Serviço Postagem

            //userObjectController.CreateUserTable("CVA_ServPostagem", "[CVA] Tipo de Serviço Postagem", BoUTBTableType.bott_NoObject);
            //userObjectController.MakeFieldsSearchable("@CVA_ServPostagem");

            #endregion            

            #region [CVA] Cadastro de Configurações de Despacho     

            //// Cabeçalho            
            //userObjectController.CreateUserTable("CVA_CfgDespacho", "[CVA] Cadastro de Despacho", BoUTBTableType.bott_MasterData);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_TipoDespacho", "Tipo de Despacho", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_CardCode", "Transportador", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_CardName", "Razão Social", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_Status", "Status", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);

            //// Aba Configuração
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_CodADM", "Código Administrativo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_DeclareVlr", "Declarar Valor Maior que", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Price, 10);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_UnidPostagem", "Unidade de Postagem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_CepPostagem", "CEP Postagem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_NumCartao", "N. Cartão Postagem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_NumDiretoria", "Número Diretoria", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 6);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_NumContrato", "Número Contrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_DataContrato", "Data do Contato", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_UfContrato", "UF Contrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_TipoServico", "Tipo Serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_SerieIni", "Serie Inicial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_SerieFin", "Serie Final", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_SerieAtu", "Serie Atual", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_AlertaMinimo", "Alerta Minimo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 3);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_DigTrackNumber", "Quantidade de Digitos", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_SufixoCodRastreio", "Sufixo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_NumIdLista", "Ultimo Numero de Lista", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_CalDigto", "Calcula Digito Correios", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);


            //// Aba Web Service
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_WSUser", "Usuario WS", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_WSPass", "senha WS", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_WS_NumCartao", "N.Cartão Postagem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_WS_NumContrato", "N.Contrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_WS_IDContrato", "CNPJ do Contrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_WS_URL", "URL", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_WS_TpSrvPost", "Tipo serviço Postagem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_WS_IDServico", "Id Serviço Postagem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_WS_CodServico", " Cod. Serviço Postagem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);

            //// Aba Rastrear Objetos
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_ROUser", "Usuario RO", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);
            //userObjectController.InsertUserField("@CVA_CfgDespacho", "CVA_ROPass", "Senha RO", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);

            //userObjectController.CreateUserObject("CVA_CfgDespacho", "[CVA] Cadastro de Despacho", "@CVA_CfgDespacho", BoUDOObjType.boud_MasterData);
            //userObjectController.MakeFieldsSearchable("@CVA_CfgDespacho");
            #endregion

            #region [CVA] Conferencia

            //// Cabeçãlho
            //userObjectController.CreateUserTable("CVA_Conferencia", "[CVA] Conferencia", BoUTBTableType.bott_Document);

            //userObjectController.InsertUserField("@CVA_Conferencia", "CVA_DocEntry", "DocEntry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("@CVA_Conferencia", "CVA_DocStatus", "Doc Status", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.InsertUserField("@CVA_Conferencia", "CVA_Status1", "Status 1", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.InsertUserField("@CVA_Conferencia", "CVA_Status2", "Status 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.InsertUserField("@CVA_Conferencia", "CVA_DtInicial_1", "Data Inicial 1", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_Conferencia", "CVA_DtFinal_1", "Data Final 1", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_Conferencia", "CVA_DtInicial_2", "Data Inicial 2", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_Conferencia", "CVA_DtFinal_2", "Data Final 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);

            //// Linhas do Cadastro
            //userObjectController.CreateUserTable("CVA_Conferencia_Linha", "[CVA] Conferencia Linha", BoUTBTableType.bott_DocumentLines);

            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_DocEntry", "DocEntry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_LineNum", "Linha", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_ItemCode", "ItemCode", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_Descricao", "Descricao", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_Quant_1", "Quantidade 1", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_Quant_2", "Quantidade 2", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_User_1", "Usuario 1", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_User_2", "Usuario 2", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_Data_1", "Data 1", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_Hora_1", "Hora 1", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_Data_2", "Data 2", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_Conferencia_Linha", "CVA_Hora_2", "Hora 2", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10);

            //userObjectController.CreateUserObject("CVA_Conferencia", "[CVA] Conferencia ", "@CVA_Conferencia", BoUDOObjType.boud_Document,false,false,);

            #endregion

            #region Tipos de Volumes

            //userObjectController.CreateUserTable("CVA_TpVol", "[CVA] Tipo de Volumes", BoUTBTableType.bott_NoObject);

            //userObjectController.InsertUserField("@CVA_TpVol", "CVA_TpVol", "Tipo Volume", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            //userObjectController.InsertUserField("@CVA_TpVol", "CVA_Peso", "Peso", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11);
            //userObjectController.InsertUserField("@CVA_TpVol", "CVA_Largura", "Largura", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11);
            //userObjectController.InsertUserField("@CVA_TpVol", "CVA_Altura", "Altura", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11);
            //userObjectController.InsertUserField("@CVA_TpVol", "CVA_Comprimento", "Comprimento", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 11);

            //userObjectController.MakeFieldsSearchable("@CVA_TpVol");


            #endregion

            #region Volume de Despacho

            //// Cabeçalho

            //userObjectController.CreateUserTable("CVA_VolEmb", "[CVA] Volumes de Despacho", BoUTBTableType.bott_MasterData);
            //userObjectController.InsertUserField("@CVA_VolEmb", "DocDate", "Data Lancamento", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 20,true);

            //userObjectController.CreateUserObject("CVA_VolEmb", "[CVA] Volumes de Despacho", "@CVA_VolEmb", BoUDOObjType.boud_MasterData);
            //userObjectController.MakeFieldsSearchable("@CVA_VolEmb");


            //// Linha

            //userObjectController.CreateUserTable("CVA_VolLinha", "[CVA] Volumes Linha ", BoUTBTableType.bott_MasterDataLines);
            //userObjectController.AddChildTableToUserObject("CVA_VolEmb", "CVA_VolLinha");

            //userObjectController.InsertUserField("@CVA_VolLinha", "CVA_IdVolume", "Tipo de Volume", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);
            //userObjectController.InsertUserField("@CVA_VolLinha", "CVA_Quantity", "Quantidade", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Quantity, 11);
            //userObjectController.InsertUserField("@CVA_VolLinha", "CVA_Peso", "Peso Bruto", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 11);

            #endregion


            #region Login Conferencia

            //userObjectController.CreateUserTable("CVA_Login", "[CVA] Login Conferência", BoUTBTableType.bott_NoObject);

            //userObjectController.InsertUserField("@CVA_Login", "CVA_Auto", "Autorização Total", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            //userObjectController.AddValidValueToUserField("@CVA_Login", "CVA_Auto", "Y", "Sim");
            //userObjectController.AddValidValueToUserField("@CVA_Login", "CVA_Auto", "N", "Não",true);

            //userObjectController.MakeFieldsSearchable("@CVA_Login");


            #endregion

        }
        public static void CreateLogin()
        {
            try
            {
                SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordSet.DoQuery("\tif not Exists(SELECT loginname FROM master.dbo.syslogins where loginname = 'cva' and dbname = '" + SBOApp.Company.CompanyDB + "' ) \r\n                    \tBEGIN\r\n                            CREATE LOGIN [cva] WITH PASSWORD=N'Cva@sa01', DEFAULT_DATABASE=[" + SBOApp.Company.CompanyDB + "], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF\r\n                            EXEC sys.sp_addsrvrolemember @loginame = N'cva', @rolename = N'sysadmin'\r\n                            ALTER LOGIN [cva] ENABLE\r\n                            RECONFIGURE WITH OVERRIDE\r\n                        end");
            }
            catch (Exception)
            {
            }
        }
    }
}
