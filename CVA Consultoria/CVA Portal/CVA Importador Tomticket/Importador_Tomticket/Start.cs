using AUXILIAR;
using BLL;
using MODEL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;


namespace Importador_Tomticket
{
    public class Start
    {
        public static void Main(string[] args)
        {
            int count, pageNum;
            string token;
            ChamadoBLL _chamadoBLL = new ChamadoBLL();
            XmlReader _xmlReader = new XmlReader();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;

            #region Importação chamados A1
            Console.WriteLine("Verificando chamados A1...");
            count = 1;
            pageNum = 1;
            token = _xmlReader.readTokenA1();
            while (count != 0)
            {

                WebRequest requisicao = WebRequest.Create($@"https://api.tomticket.com/chamados/{token}/{pageNum}");
                WebResponse resposta = requisicao.GetResponse();
                Stream dataStream = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string json = reader.ReadToEnd();

                // Deserialize the Json object
                Data data = JsonConvert.DeserializeObject<Data>(json);

                var chamados = new List<ChamadoModel>();

                foreach (var c in data.data)
                {
                    c.prd = "A1";
                    chamados.Add(c);
                }

                _chamadoBLL.Save(chamados);

                count = data.data.Count;
                reader.Close();
                resposta.Close();
                pageNum++;
            }
            Console.WriteLine("Chamados A1 importados");
            #endregion

            #region Importação chamados B1
            Console.WriteLine("Verificando chamados B1...");
            count = 1;
            pageNum = 1;
            token = _xmlReader.readTokenB1();
            while (count != 0)
            {
                WebRequest requisicao = WebRequest.Create($@"https://api.tomticket.com/chamados/{token}/{pageNum}");
                WebResponse resposta = requisicao.GetResponse();
                Stream dataStream = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string json = reader.ReadToEnd();

                // Deserialize the Json object
                Data data = JsonConvert.DeserializeObject<Data>(json);

                var chamados = new List<ChamadoModel>();

                foreach (var c in data.data)
                {
                    c.prd = "B1";
                    chamados.Add(c);
                }

                _chamadoBLL.Save(chamados);

                count = data.data.Count;
                reader.Close();
                resposta.Close();
                pageNum++;
            }
            Console.WriteLine("Chamados B1 importados");
            #endregion

            Console.WriteLine("Processo de importação finalizado. Verifique o arquivo de Log");
        }
    }
}
