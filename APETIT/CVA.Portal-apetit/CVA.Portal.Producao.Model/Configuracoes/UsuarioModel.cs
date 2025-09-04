using CVA.AddOn.Common.Controllers;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.Model.Configuracoes
{
    public class UsuarioModel : JsonModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_Usuario")]
        public string Usuario { get; set; }

        [ModelController(ColumnName = "U_Senha")]
        public string Senha { get; set; }

        public string NovaSenha { get; set; }

        public string ConfirmarSenha { get; set; }

        [ModelController(ColumnName = "U_Ativo")]
        public int AtivoInt { get; set; }

        [ModelController(ColumnName = "U_CodPerfil")]
        public string CodPerfil { get; set; }

        public List<ViewModel> ViewList { get; set; }

        public List<UsuarioEtapaModel> EtapaList { get; set; }

        public bool Ativo { get; set; }

        [ModelController(ColumnName = "U_NumeroCartao")]
        public string NumeroCartao { get; set; }
        public List<BPLModel> Filiais { get; set; }
    }

    public class UsuarioIDModel
    {
        public int USERID { get; set; }
    }
}
