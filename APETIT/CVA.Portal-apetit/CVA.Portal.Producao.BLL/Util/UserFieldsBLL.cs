using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.DataBase;
using CVA.Portal.Producao.Model.Configuracoes;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Util
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields(string server, string companyDB, int dbServerType, string username, string password)
        {
            var company = new Company
            {
                Server = server,
                CompanyDB = companyDB,
                DbServerType = (BoDataServerTypes)dbServerType,
                UserName = username,
                Password = password,
                language = BoSuppLangs.ln_Portuguese_Br
            };

            int ret = SBOApp.ConnectToCompany(company);

            if (ret != 0)
            {
                throw new Exception($"{SBOApp.Company.GetLastErrorCode()} - {SBOApp.Company.GetLastErrorDescription()}");
            }

            UserObjectController userObjectController = new UserObjectController();

            //userObjectController.InsertUserField("OHEM", "CVA_Senha", "Senha Portal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("OITM", "modelo", "Modelo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 5);
            userObjectController.InsertUserField("OITM", "datavctolote", "Data de vcto do lote", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("OITM", "CVA_MEDIDAS", "Medidas", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("WTQ1", "CVA_Etapa", "Etapa OP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("IGE1", "CVA_TipoApont", "Tipo apontamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("IGE1", "CVA_MotiPerda", "Motivo de perda", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("IGE1", "CVA_QtdNOK", "Quantidade NOK", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);

            userObjectController.InsertUserField("WOR4", "CVA_Status", "Status Etapa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("WOR4", "U_CVA_Status", "O", "Aberto", true);
            userObjectController.AddValidValueToUserField("WOR4", "U_CVA_Status", "P", "Parcial");
            userObjectController.AddValidValueToUserField("WOR4", "U_CVA_Status", "C", "Fechado");

            userObjectController.InsertUserField("WOR1", "CVA_Status", "Status recurso", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("WOR1", "U_CVA_Status", "P", "Parcial");
            userObjectController.AddValidValueToUserField("WOR1", "U_CVA_Status", "T", "Total");

            userObjectController.InsertUserField("WOR4", "CVA_DataHora", "Data Encerramento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);

            userObjectController.InsertUserField("WOR1", "impetiqueta", "Impressão de Etiqueta?", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("WOR1", "U_impetiqueta", "S", "Sim");
            userObjectController.AddValidValueToUserField("WOR1", "U_impetiqueta", "N", "Não");

            userObjectController.InsertUserField("OWTQ", "CVA_ObsPortal", "Obs. do Portal", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);

            userObjectController.CreateUserTable("CVA_PORTAL_PARAM", "CVA Portal: Parâmetrizações", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_PORTAL_PARAM", "Parcial", "Permite apontamento parcial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
            userObjectController.InsertUserField("@CVA_PORTAL_PARAM", "InspecaoMP", "Tabela Inspeção MP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);

            userObjectController.CreateUserTable("CVA_APONTAMENTO", "CVA Portal: Apontamentos", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_APONTAMENTO", "NrOP", "Nr. OP", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_APONTAMENTO", "CodEtapa", "CodEtapa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_APONTAMENTO", "QtdeApontada", "Qtde. Apontada", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 254);
            userObjectController.InsertUserField("@CVA_APONTAMENTO", "QtdeCQ", "Qtde. Qualidade", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 254);

            userObjectController.CreateUserTable("CVA_VIEW", "CVA Portal: Telas", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_VIEW", "View", "Tela", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_VIEW", "Descricao", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_VIEW", "Posicao", "Posição", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_VIEW", "CodPai", "Cód. Pai", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_VIEW", "Controller", "Controller", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_VIEW", "Action", "Action", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_VIEW", "Icone", "Ícone", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.CreateUserTable("CVA_PERFIL", "CVA Portal: Perfil", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_PERFIL", "Descricao", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_PERFIL", "Ativo", "Ativo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("@CVA_PERFIL", "U_Ativo", "1", "Sim", true);
            //userObjectController.AddValidValueToUserField("@CVA_PERFIL", "U_Ativo", "0", "Não");

            userObjectController.CreateUserTable("CVA_PERFIL_VIEW", "CVA Portal: Perfil", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_PERFIL_VIEW", "CodPerfil", "Cód. Perfil", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_PERFIL_VIEW", "CodView", "Cód. Tela", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);

            userObjectController.CreateUserTable("CVA_USUARIO", "CVA Portal: Usuário", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_USUARIO", "Usuario", "Usuário", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_USUARIO", "Senha", "Senha", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 2000);
            userObjectController.InsertUserField("@CVA_USUARIO", "CodPerfil", "Cód. Perfil", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_USUARIO", "Ativo", "Ativo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
            userObjectController.InsertUserField("@CVA_USUARIO", "NumeroCartao", "Número do cartão", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            //userObjectController.AddValidValueToUserField("@CVA_USUARIO", "U_Ativo", "1", "Sim", true);
            //userObjectController.AddValidValueToUserField("@CVA_USUARIO", "U_Ativo", "0", "Não");

            userObjectController.CreateUserTable("CVA_USUARIO_ETAPA", "CVA Portal: Etapa Prod.", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_USUARIO_ETAPA", "CodUsuario", "Cód. Usuário", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_USUARIO_ETAPA", "CodEtapa", "Cód. Etapa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.CreateUserTable("CVA_USUARIO_RECURSO", "CVA Portal: Rec Prod.", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_USUARIO_RECURSO", "CodUsuario", "Cód. Usuário", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_USUARIO_RECURSO", "ResCode", "Cód. Recurso", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_USUARIO_RECURSO", "ResName", "Nome Recurso", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.CreateUserTable("CVA_TIPO_ESPEC", "CVA Portal: Tipo Espec.", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_TIPO_ESPEC", "Descricao", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_TIPO_ESPEC", "Tipo", "Tipo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);

            userObjectController.CreateUserTable("CVA_MODELO_FICHA", "CVA Portal: Modelo Ficha", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA", "QtdeAmostra", "Qtde. Amostra", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA", "Descricao", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA", "Observacao", "Observação", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA", "ObsPlano", "Obs. Plano", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA", "NrRevisao", "Nr. Revisão", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA", "Status", "Status", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA", "CodTAmostra", "Cód. Tab. Amostra", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA", "LoteTodo", "Insp. 100% do lote", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);

            userObjectController.CreateUserTable("CVA_MODELO_FICHA1", "CVA Portal: Itens Modelo", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "ID", "ID Espec.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "CodModelo", "Cód. Ficha", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "CodEspec", "Cód. Espec.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "VlrNominal", "Vlr. Nominal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "PadraoDe", "Padrão De", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "PadraoAte", "Padrão Até", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "Analise", "Análise", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "Observacao", "Observação", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "Metodo", "Método", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "Amostragem", "Amostragem", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);
            userObjectController.InsertUserField("@CVA_MODELO_FICHA1", "Link", "Link", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.CreateUserTable("CVA_FICHA_PRODUTO", "CVA Portal: Ficha X Prod.", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_FICHA_PRODUTO", "Tipo", "Tipo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.InsertUserField("@CVA_FICHA_PRODUTO", "CodGrupo", "Cód. Grupo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_PRODUTO", "CodItem", "Cód. Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("@CVA_FICHA_PRODUTO", "CodModelo", "Cód. Ficha", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_FICHA_PRODUTO", "CodEtapa", "Cód. Etapa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_FICHA_PRODUTO", "Ativo", "Ativo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
            userObjectController.InsertUserField("@CVA_FICHA_PRODUTO", "Obrigatorio", "Obrigatório", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);

            userObjectController.CreateUserTable("CVA_FICHA_INSPECAO", "CVA Portal: Ficha Inspeção", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "Ano", "Ano", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 4);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "ID", "ID", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "Sequencia", "Sequencia", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "QtdeAmostra", "Qtde. Amostra", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "SeqParcial", "Sequencia Parcial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "SeqFinalizada", "Sequencia Finalizada", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
            //userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "QtdeSeq", "QtdeSeq", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "DataInsp", "Data Inspeção", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "DataDoc", "Data Doc.", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "CodItem", "Cód. Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "DescItem", "Cód. Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "DocEntry", "ID Doc.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "DocNum", "Nr Doc.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "LineNum", "Linha Doc.", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "TipoDoc", "Tipo Doc.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "Quantidade", "Quantidade", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "LoteSerie", "Lote/Série", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "CodModelo", "Cód. Modelo Ficha", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "QtdeAnalisada", "Qtde. Analisada", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "Status", "Status", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "CodUsuario", "Cód. Usuário", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            //userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "NomeUsuario", "Nome Usuário", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "CodPN", "Cód. PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "NomePN", "Nome PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "NrNF", "Nr. NF", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "CodEtapa", "Cód. Etapa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO", "StatusLote", "Status Lote", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);

            userObjectController.CreateUserTable("CVA_FICHA_INSPECAO1", "CVA Portal: Item Inspeção", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "ID", "ID", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "Sequencia", "Sequencia", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "IdAmostra", "ID Amostra", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "CodFicha", "Cód. Ficha Insp.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "CodEspec", "Cód. Ficha Insp.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "VlrNominal", "Vlr. Nominal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "PadraoDe", "Padrão De", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "PadraoAte", "Padrão Até", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "Analise", "Análise", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "Observacao", "Observação", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "Metodo", "Método", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "Amostragem", "Amostragem", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "CodRecurso", "Cód. Recurso", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "Resultado", "Resultado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_FICHA_INSPECAO1", "Link", "Link", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.CreateUserTable("CVA_PESQUISA", "CVA: Pesquisas Satisfação", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_PESQUISA", "Desc", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_PESQUISA", "Ativa", "Ativa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);

            userObjectController.CreateUserTable("CVA_APONTAMENTO_HR", "CVA Portal: Apontamento Horas", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "OPDocEntry", "Entry OP", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "OPDocNum", "OP", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "CodEtapa", "Etapa OP", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "NomeEtapa", "Nome da Etapa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "RecursoLineNum", "Linha do Recurso", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "RecursoCod", "Cód. do Recurso", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "RecursoNome", "Nome da Etapa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "CodUsuario", "Cód. Usuário", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "StartDateTime", "Data/hora início", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 25);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "EndDateTime", "Data/hora fim", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 25);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "Duration", "Duração em minutos", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "OkQuantity", "Quantidade OK", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "NOkQuantity", "Quantidade NOK", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "OcrCode", "Centro de custo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "Obs", "Observações", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "Tipo", "Tipo apontamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "Status", "Status", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "MotivoPerda", "Motivo de Perda", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@CVA_APONTAMENTO_HR", "InspecaoTotal", "Inspeção no Lote Todo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);

            userObjectController.CreateUserTable("CVA_PED_TRANSF_OP", "CVA Portal: Ped. Transferência", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_PED_TRANSF_OP", "TQDocEntry", "OWTQ DocEntry", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_PED_TRANSF_OP", "OPDocNum", "OWOR DocNum", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_PED_TRANSF_OP", "StageId", "Cód. Etapa", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            userObjectController.CreateUserTable("CVA_TIPO_APONT", "CVA Portal: Tipo Apontamento", BoUTBTableType.bott_MasterData);
            userObjectController.CreateUserTable("CVA_MOT_PERDA", "CVA Portal: Motivos de Perda", BoUTBTableType.bott_MasterData);

            userObjectController.CreateUserTable("CVA_TAB_AMOSTRA", "CVA Portal: Tabelas de Amostra", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_TAB_AMOSTRA", "Descricao", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.CreateUserTable("CVA_TAB_AMOSTRA1", "CVA Portal: Item Tab. Amostra", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_TAB_AMOSTRA1", "CodTAmostra", "Cód. Tab. Amostra", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@CVA_TAB_AMOSTRA1", "QtdBase", "Quantidade base", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_TAB_AMOSTRA1", "QtdAmostra", "Quantidade amostra", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_TAB_AMOSTRA1", "QtdMaxRejeito", "Qtde. máx. rejeito", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);


            ///Portal CVA -> Apetit
            userObjectController.InsertUserField("OITB", "LISTAR_PORTAL", "Listar no Portal de Apontamento?", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, true, "N");
            userObjectController.AddValidValueToUserField("OITB", "LISTAR_PORTAL", new Dictionary<string, string> { { "Y", "Sim" }, { "N", "Não" } }, "N");


            userObjectController.CreateUserTable("CVA_MOTIVO_APO", "CVA: Motivo para Apontamento", BoUTBTableType.bott_NoObjectAutoIncrement);
            userObjectController.CreateUserTable("CVA_MOTIVO_REPO", "CVA: Motivo para Reposição", BoUTBTableType.bott_NoObjectAutoIncrement);

            userObjectController.CreateUserTable("CVA_DEP_APROV", "CVA: Depart. de Aprovação", BoUTBTableType.bott_MasterData);

            userObjectController.CreateUserObject("CVA_Depart_Aprov", "Departamento de Aprovação", "@CVA_DEP_APROV", BoUDOObjType.boud_MasterData, false, false, true, true, true, true, true, 3328, 14);

            userObjectController.InsertUserField("@CVA_MOTIVO_REPO", "CVA_Depart", "Departamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, false, string.Empty, "", "CVA_Depart_Aprov");

            //userObjectController.InsertUserField("OWOR", "CVA_CONTRATO", "Nº Contrato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("OWOR", "CVA_RESTO", "Resto/Ingesta (KG)", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("OWOR", "CVA_SOBRA", "Sobra Limpa (KG)", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("OWOR", "CVA_SERVICO", "Serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("OWOR", "CVA_Turno", "Turno", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, false, string.Empty, "CVA_TURNO");
            userObjectController.InsertUserField("OWOR", "CVA_APO", "Apontamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, false, string.Empty, "CVA_APTO_TERCEIROS");
            userObjectController.InsertUserField("OWOR", "CVA_APO_ZERO", "Apontamento Zerado?", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, true, "N");
            userObjectController.AddValidValueToUserField("OWOR", "CVA_APO_ZERO", new Dictionary<string, string> { { "Y", "Sim" }, { "N", "Não" } }, "N");

            userObjectController.InsertUserField("WOR1", "CVA_SubMotivo", "Substituto", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("WOR1", "CVA_Substituto", "Substituto - Motivo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("WOR1", "CVA_SubJust", "Substituto - Justificativa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            
            userObjectController.InsertUserField("OBPL", "CVA_Codigo2PN", "Selecionar PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);

            userObjectController.InsertUserField("OITM", "CVA_Planejar", "Planejar", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OITM", "U_CVA_Planejar", "Y", "Sim");
            userObjectController.AddValidValueToUserField("OITM", "U_CVA_Planejar", "N", "Não");

            userObjectController.CreateUserTable("CVA_APTO_TERCEIROS", "CVA: Apontamento Terceiros", BoUTBTableType.bott_MasterData);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS", "FILIAL", "Filial", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS", "DATA", "Data", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS", "TURNO", "Turno", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS", "SERVICO", "Serviço", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS", "QTYPLAN", "Qtd Planejado", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS", "QTYREF", "Qtd Refeições", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS", "USERPORTAL", "User Portal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);


            userObjectController.CreateUserTable("CVA_APTO_TERCEIROS1", "CVA: Apontamento Terceiros1", BoUTBTableType.bott_MasterDataLines);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS1", "CARDCODE", "CardCode", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS1", "CARDNAME", "CardName", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS1", "QTYAPT", "Qtd Apontada", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_APTO_TERCEIROS1", "QTYREF", "Qtd Refeições", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);

            userObjectController.CreateUserObject("CVA_APTO_TERCEIROS", "CVA: Apontamento Terceiros", "CVA_APTO_TERCEIROS", BoUDOObjType.boud_MasterData);
            userObjectController.AddChildTableToUserObject("CVA_APTO_TERCEIROS", "CVA_APTO_TERCEIROS1");

            userObjectController.InsertUserField("OCRD", "CVA_TERCEIROS", "Terceiros ?", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, true, "N");
            userObjectController.AddValidValueToUserField("OCRD", "CVA_TERCEIROS", new Dictionary<string, string> { { "Y", "Sim" }, { "N", "Não" } }, "N");

            userObjectController.InsertUserField("OHEM", "CVA_BLOQUEIO_APTO", "[Apontamento] Bloqueado?", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, true, "N");
            userObjectController.AddValidValueToUserField("OHEM", "CVA_BLOQUEIO_APTO", new Dictionary<string, string> { { "Y", "Sim" }, { "N", "Não" } }, "N");

            userObjectController.InsertUserField("OHEM", "CVA_VALDTMP_APTO", "[Apontamento] Validação do prazo?", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, true, "Y");
            userObjectController.AddValidValueToUserField("OHEM", "CVA_VALDTMP_APTO", new Dictionary<string, string> { { "Y", "Sim" }, { "N", "Não" } }, "Y");

            userObjectController.CreateUserTable("CVA_TIPO_SAIDA", "CVA Portal: Tipo Saída", BoUTBTableType.bott_MasterData);
            userObjectController.InsertUserField("IGE1", "CVA_TipoSaida", "Tipo de Saída - Portal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, false, "", "CVA_TIPO_SAIDA");
            //InsertDefaultData();
        }

        public static void InsertDefaultDataApetit()
        {
            CrudController crudController = new CrudController();
            crudController.TableName = "@CVA_VIEW";

            ViewModel viewModel = new ViewModel();
            List<ViewModel> viewsList = new List<ViewModel>();


            if (string.IsNullOrEmpty(crudController.Exists("\"U_View\" = 'Portal'")))
            {
                viewModel = new ViewModel();
                viewModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                viewModel.View = "Portal";
                viewModel.Descricao = "Acesso ao portal";
                viewModel.Posicao = 1;
                viewModel.Controller = "Apetit";

                crudController.Model = viewModel;
                crudController.CreateModel();
                viewsList.Add(viewModel);
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_View\" = 'Usuários'")))
            {
                viewModel = new ViewModel();
                viewModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                viewModel.View = "Usuários";
                viewModel.Descricao = "Usuários";
                viewModel.Posicao = 2;
                viewModel.Controller = "Usuario";

                crudController.Model = viewModel;
                crudController.CreateModel();
                viewsList.Add(viewModel);
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_View\" = 'Saída de Materiais'")))
            {
                viewModel = new ViewModel();
                viewModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                viewModel.View = "Saída de Materiais";
                viewModel.Descricao = "Saída de Materiais";
                viewModel.Posicao = 3;
                viewModel.Controller = "SaidaMateriais";

                crudController.Model = viewModel;
                crudController.CreateModel();
                viewsList.Add(viewModel);
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_View\" = 'Reposição de Insumos'")))
            {
                viewModel = new ViewModel();
                viewModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                viewModel.View = "Reposição de Insumos";
                viewModel.Descricao = "Reposição de Insumos";
                viewModel.Posicao = 4;
                viewModel.Controller = "ReposicaoInsumos";

                crudController.Model = viewModel;
                crudController.CreateModel();
                viewsList.Add(viewModel);
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_View\" = 'Apontamento Acompanhamento'")))
            {
                viewModel = new ViewModel();
                viewModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                viewModel.View = "Apontamento Acompanhamento";
                viewModel.Descricao = "Apontamento Acompanhamento";
                viewModel.Posicao = 5;
                viewModel.Controller = "ApontamentoPainel";

                crudController.Model = viewModel;
                crudController.CreateModel();
                viewsList.Add(viewModel);
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_View\" = 'Apontamento Refeições'")))
            {
                viewModel = new ViewModel();
                viewModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                viewModel.View = "Apontamento Refeições";
                viewModel.Descricao = "Apontamento Refeições";
                viewModel.Posicao = 6;
                viewModel.Controller = "Apontamento";

                crudController.Model = viewModel;
                crudController.CreateModel();
                viewsList.Add(viewModel);
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_View\" = 'Apontamento Serviço Extra'")))
            {
                viewModel = new ViewModel();
                viewModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                viewModel.View = "Apontamento Serviço Extra";
                viewModel.Descricao = "Apontamento Serviço Extra";
                viewModel.Posicao = 7;
                viewModel.Controller = "ServicoExtra";

                crudController.Model = viewModel;
                crudController.CreateModel();
                viewsList.Add(viewModel);
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_View\" = 'Consulta Estoque'")))
            {
                viewModel = new ViewModel();
                viewModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                viewModel.View = "Consulta Estoque";
                viewModel.Descricao = "Consulta Estoque";
                viewModel.Posicao = 8;
                viewModel.Controller = "ReportPosicaoEstoque";

                crudController.Model = viewModel;
                crudController.CreateModel();
                viewsList.Add(viewModel);
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_View\" = 'Cotação de Compra'")))
            {
                viewModel = new ViewModel();
                viewModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                viewModel.View = "Cotação de Compra";
                viewModel.Descricao = "Cotação de Compra";
                viewModel.Posicao = 9;
                viewModel.Controller = "OfertaCompra";

                crudController.Model = viewModel;
                crudController.CreateModel();
                viewsList.Add(viewModel);
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_View\" = 'Perfil'")))
            {
                viewModel = new ViewModel();
                viewModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                viewModel.View = "Perfil";
                viewModel.Descricao = "Perfil";
                viewModel.Posicao = 10;
                viewModel.Controller = "Perfil";

                crudController.Model = viewModel;
                crudController.CreateModel();
                viewsList.Add(viewModel);
            }


            crudController.TableName = "@CVA_USUARIO";
            if (string.IsNullOrEmpty(crudController.Exists("Code = '0000000001'")))
            {
                UsuarioModel usuarioModel = new UsuarioModel();
                usuarioModel.Code = "0000000001";
                usuarioModel.CodPerfil = "0000000001";
                usuarioModel.AtivoInt = 1;
                usuarioModel.Usuario = "manager";
                usuarioModel.Senha = EncryptController.Encrypt("1234", "manager");

                crudController.Model = usuarioModel;
                crudController.CreateModel();
            }

            crudController.TableName = "@CVA_PERFIL";
            if (string.IsNullOrEmpty(crudController.Exists("\"U_Descricao\" = 'Administrador'")))
            {
                PerfilModel perfilModel = new PerfilModel();
                perfilModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                perfilModel.Descricao = "Administrador";
                perfilModel.AtivoInt = 1;

                crudController.Model = perfilModel;
                crudController.CreateModel();
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_Descricao\" = 'Comprador'")))
            {
                PerfilModel perfilModel = new PerfilModel();
                perfilModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                perfilModel.Descricao = "Comprador";
                perfilModel.AtivoInt = 1;

                crudController.Model = perfilModel;
                crudController.CreateModel();
            }

            if (string.IsNullOrEmpty(crudController.Exists("\"U_Descricao\" = 'Fornecedor'")))
            {
                PerfilModel perfilModel = new PerfilModel();
                perfilModel.Code = CrudController.GetNextCode(crudController.TableName).PadLeft(10, '0');
                perfilModel.Descricao = "Fornecedor";
                perfilModel.AtivoInt = 1;

                crudController.Model = perfilModel;
                crudController.CreateModel();
            }

            crudController.TableName = "@CVA_PERFIL_VIEW";
            foreach (var item in viewsList)
            {
                PerfilViewModel perfilViewModel = new PerfilViewModel();
                if (String.IsNullOrEmpty(crudController.Exists($"U_CodView = '{item.Code}'")))
                {
                    perfilViewModel.CodPerfil = "0000000001";
                    perfilViewModel.CodView = item.Code;

                    crudController.Model = perfilViewModel;
                    crudController.CreateModel();
                }
            }



        }
    }
}
