using Newtonsoft.Json;
using System.Collections.Generic;

namespace CVA.TomTicketDownload.Model
{
    public class Chamado
    {
        [JsonProperty("idchamado")]
        public string IdChamado { get; set; }

        [JsonProperty("protocolo")]
        public int Protocolo { get; set; }

        [JsonProperty("titulo")]
        public string Titulo { get; set; }

        //[JsonProperty("mensagem")]
        //public string Mensagem { get; set; }

        [JsonProperty("mimetype")]
        public string Mimetype { get; set; }

        [JsonProperty("email_cliente")]
        public string EmailCliente { get; set; }

        [JsonProperty("prioridade")]
        public int Prioridade { get; set; }

        [JsonProperty("tempotrabalho")]
        public int TempoTrabalho { get; set; }

        [JsonProperty("tempoabertura")]
        public string TempoAbertura { get; set; }

        [JsonProperty("data_criacao")]
        public string DataCriacao { get; set; }

        [JsonProperty("deadline")]
        public string Deadline { get; set; }

        [JsonProperty("valoritemhora")]
        public int ValorItemHora { get; set; }

        [JsonProperty("valoritemhoraextra")]
        public int ValorItemHoraExtra { get; set; }

        [JsonProperty("valorfinal")]
        public int ValorFinal { get; set; }

        [JsonProperty("valorfinalextra")]
        public int ValorFinalExtra { get; set; }

        [JsonProperty("valorfinalbruto")]
        public int ValorFinalBruto { get; set; }

        [JsonProperty("nomecliente")]
        public string NomeCliente { get; set; }

        [JsonProperty("tipochamado")]
        public string TipoChamado { get; set; }

        [JsonProperty("avaliadoproblemaresolvido")]
        public string AvaliadoProblemaResolvido { get; set; }

        [JsonProperty("avaliadoatendimento")]
        public string AvaliadoAtendimento { get; set; }

        [JsonProperty("avaliacaocomentario")]
        public string AvaliacaoComentario { get; set; }

        [JsonProperty("dataprimeiraresposta")]
        public string DataPrimeiraResposta { get; set; }

        [JsonProperty("dataencerramento")]
        public string DataEncerramento { get; set; }

        [JsonProperty("ultimasituacao")]
        public int UltimaSituacao { get; set; }

        [JsonProperty("dataultimasituacao")]
        public string DataUltimaSituacao { get; set; }

        [JsonProperty("sla_inicio")]
        public string SlaInicio { get; set; }

        [JsonProperty("sla_deadline")]
        public string SlaDeadline { get; set; }

        [JsonProperty("sla_inicializacao_cumprido")]
        public string SlaInicializacaoCumprido { get; set; }

        [JsonProperty("sla_deadline_cumprido")]
        public bool? SlaDeadlineCumprido { get; set; }

        [JsonProperty("descsituacao")]
        public string DescSituacao { get; set; }

        [JsonProperty("categoria")]
        public string Categoria { get; set; }

        [JsonProperty("departamento")]
        public string Departamento { get; set; }

        [JsonProperty("atendente")]
        public string Atendente { get; set; }

        [JsonProperty("id_cliente")]
        public string IdCliente { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("dataultimostatus")]
        public string DataUltimoStatus { get; set; }

        [JsonProperty("nomeorganizacao")]
        public string NomeOrganizacao { get; set; }

        [JsonProperty("historico")]
        public List<Historico> historico { get; set; }

        [JsonProperty("historico_status")]
        public List<HistoricoStatus> HistoricoStatus { get; set; }

        public double TempoAtendimento { get; set; }



    }

    public class ResultRoot
    {
        [JsonProperty("erro")]
        public bool Erro { get; set; }

        [JsonProperty("data")]
        public List<Chamado> Data { get; set; }

        [JsonProperty("mensagem")]
        public string mensagem { get; set; }

        [JsonProperty("total_itens")]
        public int TotalItens { get; set; }
    }

    public class Historico
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("origem")]
        public string Origem { get; set; }

        [JsonProperty("atendente")]
        public string Atendente { get; set; }

        [JsonProperty("data_hora")]
        public string Data_hora { get; set; }
    }


    public class HistoricoStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string ProtolocoChamado { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("atendente_inicio")]
        public string Atendente_inicio { get; set; }

        [JsonProperty("atendente_fim")]
        public string Atendente_fim { get; set; }

        [JsonProperty("inicio")]
        public string Inicio { get; set; }

        [JsonProperty("fim")]
        public string Fim { get; set; }

        public string TempoAtendimento { get; set; }
    }


}