using System;
using System.Collections.Generic;
using System.Xml;
using System.ServiceModel;
using WebImcopa.WebServiceRobos;

namespace WebImcopa
{
    /// <summary>
    /// Classe responsável por realizar os testes ao grupo de serviço federal
    /// </summary>

    public class Federal
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
        private string grupo { get { return @"Federal"; } }

        #endregion Atributos

        #region DELTA

        /// <summary>
        /// Método responsável por realizar uma consulta ao webservice WS
        /// </summary>
     
        public XmlNode TesteUnitario_DELTA(string cnpj)
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
            servico = @"SituacaoCadastralPessoaJuridica";
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
                System.Diagnostics.Trace.WriteLine(@"Não autenticado");
                return null;
            }

            //Preenche os dados de autenticação
            TokendeAutenticacao token = new TokendeAutenticacao();
            token.IdUsuario = 0;
            token.InnerToken = guid;

            //Realiza a pesquisa
            XmlNode xmlRetorno = WebService.PesquisarComHTML(token, grupo, servico, tipo_pesquisa, colecao);
            if (xmlRetorno == null)
            {
                System.Diagnostics.Trace.WriteLine(@"Problema no retorno");
                return null;
            }
            return xmlRetorno;
            
            #endregion Implementação        
        }

        #endregion DELTA
    }
}
