using MODEL.Enumerators;
using MODEL.Interface;
using System;
using System.Collections.Generic;

namespace MODEL.Classes
{
    public class CollaboratorModel : IModel
    {
        public static int oObjectType { get { return (int)ObjectType.Collaborator; } }
        public CollaboratorTypeModel Tipo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string RG { get; set; }
        public DateTime EmissaoRG { get; set; }
        public string OrgaoEmissor { get; set; }
        public string Passaporte { get; set; }
        public string Nacionalidade { get; set; }
        public DateTime ValidadePassaporte { get; set; }
        public string Naturalidade { get; set; }
        public DateTime DataNascimento { get; set; }
        public int EstadoCivil { get; set; }
        public int Genero { get; set; }
        public AddressModel Endereco { get; set; }
        public List<SpecialtyModel> Especialidades { get; set; }
        public int GerenciaProjetos { get; set; }
        public DateTime? LimiteHoras { get; set; }
    }
}
