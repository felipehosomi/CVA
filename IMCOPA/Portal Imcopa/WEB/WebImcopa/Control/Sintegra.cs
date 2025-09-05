using System;
using System.Collections.Generic;
using System.Xml;
using System.ServiceModel;
using System.Text.RegularExpressions;
using WebImcopa.WebServiceRobos;

namespace WebImcopa
{

    public class Sintegra
    {
        #region Atributos

        /// <summary>
        /// Lista de webservices cadastrados no audicon
        /// </summary>
        private Dictionary<string, string> listWebServices = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
             {"DELTA" ,"http://delta.audiconsearch.com.br/WebServiceRobos.asmx" },
        };

        /// <summary>
        /// Usuario para logar no webservice
        /// </summary>
        private string usuario { get { return @"imcopa\webservice"; } }

        /// <summary>
        /// Senha para logar no webservice
        /// </summary>
        private string senha { get { return @"$V7r0II_hS05csc@Y@Z_"; } }

        /// <summary>
        /// Grupo de serviço a ser consultado
        /// </summary>
        private string grupo { get { return @"Sintegra"; } }

        #endregion Atributos

        #region DELTA

        /// <summary>
        /// Método responsável por realizar uma consulta ao webservice WS por CNPJ
        /// </summary>
        public XmlNode TesteUnitario_DELTA_CNPJ(string cnpj, string UF)
        {
            #region Declaração

            string servico;
            string tipo_pesquisa;
            List<Chave> chaves;
            Colecao colecao;

            WebServiceRobosSoapClient WebService;

            #endregion Declaração

            #region Implementação

            BasicHttpBinding binding = new BasicHttpBinding();

            EndpointAddress address = new EndpointAddress(listWebServices["DELTA"].ToString());

            //Instancia o web service
            WebService = new WebServiceRobosSoapClient(binding, address);
            //WebService = new WebServiceRobosSoapClient();

            //Seta as informações de pesquisa
            servico = @"Sintegra" + UF;
            tipo_pesquisa = @"J";

            //Seta as informações das chaves que serão utilizadas
            chaves = new List<Chave>();
            chaves.Add(new Chave() { nome = "CNPJ", valor = cnpj });

            //Converte a lista em array
                colecao = new Colecao();
            colecao.Chaves = chaves.ToArray();

            //Realiza a autenticação no webservice
            Guid guid = WebService.Autenticar(usuario, senha);

            //Verifica se a autenticacao ocorreu corretamente
            if (guid == Guid.Empty)
            {
                throw new Exception("Não autenticado");
            }

            //Preenche os dados de autenticação
            TokendeAutenticacao token = new TokendeAutenticacao();
            token.IdUsuario = 0;
            token.InnerToken = guid;

            //Realiza a pesquisa
            XmlNode xmlRetorno = WebService.Pesquisar(token, grupo, servico, tipo_pesquisa, colecao); //PesquisarComHTML(token, grupo, servico, tipo_pesquisa, colecao);
            if (xmlRetorno == null)
            {
                throw new Exception("XML retorno igual a NULL");
            }

            if (Regex.IsMatch(xmlRetorno.InnerXml, @"Problemas na Busca|Timeout|Site Fora"))
            {
                throw new Exception("Erro na consulta", new Exception(xmlRetorno.InnerXml.ToString()));
            }

            //string[] retorno = new string[2];
            //retorno[0] = xmlRetorno.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/InscricaoEstadual").InnerText;
            //retorno[1] = xmlRetorno.SelectSingleNode($@"Arquivo/Registro/Retorno/Dados/SituacaoAtual").InnerText;
            return xmlRetorno;
            #endregion Implementação        
        }

        /// <summary>
        /// Método responsável por realizar uma consulta ao webservice DELTA por IE
        /// </summary>
        public void TesteUnitario_DELTA_IE()
        {
            #region Declaração

            string servico;
            string tipo_pesquisa;
            List<Chave> chaves;
            Colecao colecao;

            WebServiceRobosSoapClient WebService;

            #endregion Declaração

            #region Implementação

            BasicHttpBinding binding = new BasicHttpBinding();

            EndpointAddress address = new EndpointAddress(listWebServices["DELTA"].ToString());

            //Instancia o web service
            WebService = new WebServiceRobosSoapClient(binding, address);
            //WebService = new WebServiceRobosSoapClient();

            //Seta as informações de pesquisa
            servico = @"SintegraRS";
            tipo_pesquisa = @"I";

            //Seta as informações das chaves que serão utilizadas
            chaves = new List<Chave>();
            chaves.Add(new Chave() { nome = "IE", valor = "1310089725" });

            //Converte a lista em array
            colecao = new Colecao();
            colecao.Chaves = chaves.ToArray();

            //Realiza a autenticação no webservice
            Guid guid = WebService.Autenticar(usuario, senha);

            //Verifica se a autenticacao ocorreu corretamente
            if (guid == Guid.Empty)
            {
                throw new Exception("Não autenticado");
            }

            //Preenche os dados de autenticação
            TokendeAutenticacao token = new TokendeAutenticacao();
            token.IdUsuario = 0;
            token.InnerToken = guid;

            //Realiza a pesquisa
            XmlNode xmlRetorno = WebService.PesquisarComHTML(token, grupo, servico, tipo_pesquisa, colecao);
            if (xmlRetorno == null)
            {
                throw new Exception("XML retorno igual a NULL");
            }

            if (Regex.IsMatch(xmlRetorno.InnerXml, @"Problemas na Busca|Timeout|Site Fora"))
            {
                throw new Exception("Erro na consulta", new Exception(xmlRetorno.InnerXml.ToString()));
            }

            System.Diagnostics.Trace.WriteLine(xmlRetorno.InnerXml.ToString());

            #endregion Implementação        
        }

        /// <summary>
        /// Método responsável por realizar uma consulta ao webservice DELTA por IPR
        /// </summary>
        public void TesteUnitario_DELTA_IPR()
        {
            #region Declaração

            string servico;
            string tipo_pesquisa;
            List<Chave> chaves;
            Colecao colecao;

            WebServiceRobosSoapClient WebService;

            #endregion Declaração

            #region Implementação

            BasicHttpBinding binding = new BasicHttpBinding();

            EndpointAddress address = new EndpointAddress(listWebServices["DELTA"].ToString());

            //Instancia o web service
            WebService = new WebServiceRobosSoapClient(binding, address);
            //WebService = new WebServiceRobosSoapClient();

            //Seta as informações de pesquisa
            servico = @"SintegraPR";
            tipo_pesquisa = @"P";

            //Seta as informações das chaves que serão utilizadas
            chaves = new List<Chave>();
            chaves.Add(new Chave() { nome = "IPR", valor = "9500516396" });

            //Converte a lista em array
            colecao = new Colecao();
            colecao.Chaves = chaves.ToArray();

            //Realiza a autenticação no webservice
            Guid guid = WebService.Autenticar(usuario, senha);

            //Verifica se a autenticacao ocorreu corretamente
            if (guid == Guid.Empty)
            {
                throw new Exception("Não autenticado");
            }

            //Preenche os dados de autenticação
            TokendeAutenticacao token = new TokendeAutenticacao();
            token.IdUsuario = 0;
            token.InnerToken = guid;

            //Realiza a pesquisa
            XmlNode xmlRetorno = WebService.PesquisarComHTML(token, grupo, servico, tipo_pesquisa, colecao);
            if (xmlRetorno == null)
            {
                throw new Exception("XML retorno igual a NULL");
            }

            if (Regex.IsMatch(xmlRetorno.InnerXml, @"Problemas na Busca|Timeout|Site Fora"))
            {
                throw new Exception("Erro na consulta", new Exception(xmlRetorno.InnerXml.ToString()));
            }

            System.Diagnostics.Trace.WriteLine(xmlRetorno.InnerXml.ToString());

            #endregion Implementação        
        }

        /// <summary>
        /// Método responsável por realizar uma consulta ao webservice DELTA por CPF
        /// </summary>
        public void TesteUnitario_DELTA_CPF()
        {
            #region Declaração

            string servico;
            string tipo_pesquisa;
            List<Chave> chaves;
            Colecao colecao;

            WebServiceRobosSoapClient WebService;

            #endregion Declaração

            #region Implementação

            BasicHttpBinding binding = new BasicHttpBinding();

            EndpointAddress address = new EndpointAddress(listWebServices["DELTA"].ToString());

            //Instancia o web service
            WebService = new WebServiceRobosSoapClient(binding, address);
            //WebService = new WebServiceRobosSoapClient();

            //Seta as informações de pesquisa
            servico = @"SintegraBA";
            tipo_pesquisa = @"F";

            //Seta as informações das chaves que serão utilizadas
            chaves = new List<Chave>();
            chaves.Add(new Chave() { nome = "CPF", valor = "14513986891" });

            //Converte a lista em array
            colecao = new Colecao();
            colecao.Chaves = chaves.ToArray();

            //Realiza a autenticação no webservice
            Guid guid = WebService.Autenticar(usuario, senha);

            //Verifica se a autenticacao ocorreu corretamente
            if (guid == Guid.Empty)
            {
                throw new Exception("Não autenticado");
            }

            //Preenche os dados de autenticação
            TokendeAutenticacao token = new TokendeAutenticacao();
            token.IdUsuario = 0;
            token.InnerToken = guid;

            //Realiza a pesquisa
            XmlNode xmlRetorno = WebService.PesquisarComHTML(token, grupo, servico, tipo_pesquisa, colecao);
            if (xmlRetorno == null)
            {
                throw new Exception("XML retorno igual a NULL");
            }

            if (Regex.IsMatch(xmlRetorno.InnerXml, @"Problemas na Busca|Timeout|Site Fora"))
            {
                throw new Exception("Erro na consulta", new Exception(xmlRetorno.InnerXml.ToString()));
            }

            System.Diagnostics.Trace.WriteLine(xmlRetorno.InnerXml.ToString());

            #endregion Implementação        
        }

        #endregion DELTA
    }
}