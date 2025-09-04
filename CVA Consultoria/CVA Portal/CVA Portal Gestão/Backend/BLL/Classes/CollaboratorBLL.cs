using System;
using System.Collections.Generic;
using System.Data;
using AUXILIAR;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class CollaboratorBLL
    {
        #region Atributos
        private StatusBLL _statusBLL { get; set; }
        private CollaboratorDAO _collaboratorDAO { get; set; }
        private AddressBLL _addressBLL { get; set; }
        private SpecialtyBLL _specialtyBLL { get; set; }
        private Helper _helper { get; set; }
        #endregion

        #region Construtor
        public CollaboratorBLL()
        {
            this._collaboratorDAO = new CollaboratorDAO();
            this._addressBLL = new AddressBLL();
            this._specialtyBLL = new SpecialtyBLL();
            this._helper = new Helper();
        }
        #endregion

        public CollaboratorModel Get(int id)
        {
            var model = _collaboratorDAO.Get(id);
            model.Tipo = _collaboratorDAO.Get_Type(id);
            model.Endereco = _collaboratorDAO.Get_Address(id);
            model.Especialidades = _collaboratorDAO.Get_Specialties(id);

            return model;
        }


        public MessageModel Insert(CollaboratorModel model)
        {
            try
            {
                var valid = ValidateModel(model);
                if (valid.Error != null)
                    return valid;

                model.Id = _collaboratorDAO.Insert(model);
                if (model.Id > 0)
                {
                    model.Endereco.User = model.User;
                    model.Endereco.Status = new StatusModel
                    {
                        Id = model.Status.Id
                    };
                    _collaboratorDAO.Insert_Address(model);

                    foreach (var item in model.Especialidades)
                        _collaboratorDAO.Insert_Specialties(model, item);

                    return MessageBLL.Generate("Colaborador inserido.", 0);
                }
                else
                    return MessageBLL.Generate("Falha ao inserir colaborador.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }

        public List<CollaboratorModel> LoadCombo_Collaborator()
        {
            var list = new List<CollaboratorModel>();
            var result = _collaboratorDAO.LoadCombo_Collaborator();

            for(int i = 0; i < result.Rows.Count; i++)
            {
                var model = new CollaboratorModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"]),
                    Nome = result.Rows[i]["Nome"].ToString()
                };
                list.Add(model);
            }
            return list;
        }

        public MessageModel Update(CollaboratorModel model)
        {
            try
            {
                var valid = ValidateModel(model);
                if (valid.Error != null)
                    return valid;

                if (_collaboratorDAO.Update(model) > 0)
                {
                    model.Endereco.User = model.User;
                    model.Endereco.Status = new StatusModel();
                    model.Endereco.Status.Id = model.Status.Id;
                    _collaboratorDAO.Update_Address(model);

                    foreach (var item in model.Especialidades)
                    {
                        var result = _collaboratorDAO.Get_SpecificSpecialty(model, item);
                        if (result.Rows.Count <= 0)
                            _collaboratorDAO.Insert_Specialties(model, item);
                        else
                            _collaboratorDAO.Update_Specialties(model, item);
                    }

                    return MessageBLL.Generate("Colaborador atualizado.", 0);
                }
                else
                    return MessageBLL.Generate("Falha ao atualizar colaborador.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }

        public MessageModel Remove_Specialty(SpecialtyModel model, int idUser)
        {
            try
            {
                var _userBLL = new UserBLL();
                var idCol = _userBLL.GetCollaborator(idUser).Id;
                if (_collaboratorDAO.Remove_Specialty(model, idCol) == 1)
                    return MessageBLL.Generate("Especialidade removida.", 0);
                else
                    return MessageBLL.Generate("Impossível remover uma especialidade que possua apontamentos vínculados.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }


        public CollaboratorModel ImportarDadosColaborador()
        {
            return _helper.ImportarDadosColaborador();
        }

        public List<CollaboratorModel> LoadModel(DataTable result)
        {
            var modelList = new List<CollaboratorModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new CollaboratorModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Insert = Convert.ToDateTime(result.Rows[i]["Insert"].ToString()),
                    Status = new StatusModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Status"].ToString())
                    },
                    Tipo = new CollaboratorTypeModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Tipo.Id"].ToString())
                    },
                    Nome = result.Rows[i]["Nome"].ToString(),
                    Email = result.Rows[i]["Email"].ToString(),
                    Telefone = result.Rows[i]["Telefone"].ToString(),
                    Celular = result.Rows[i]["Celular"].ToString(),
                    CPF = result.Rows[i]["CPF"].ToString(),
                    CNPJ = result.Rows[i]["CNPJ"].ToString(),
                    RG = result.Rows[i]["RG"].ToString(),
                    EmissaoRG = Convert.ToDateTime(result.Rows[i]["EmissaoRG"].ToString()),
                    OrgaoEmissor = result.Rows[i]["OrgaoEmissor"].ToString(),
                    Passaporte = result.Rows[i]["Passaporte"].ToString(),
                    Nacionalidade = result.Rows[i]["Nacionalidade"].ToString(),
                    Naturalidade = result.Rows[i]["Naturalidade"].ToString(),
                    DataNascimento = Convert.ToDateTime(result.Rows[i]["DataNascimento"].ToString()),
                    EstadoCivil = Convert.ToInt32(result.Rows[i]["EstadoCivil"].ToString()),
                    Genero = Convert.ToInt32(result.Rows[i]["Genero"].ToString()),
                    Endereco = new AddressModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Endereco.Id"].ToString()),
                        Street = result.Rows[i]["Endereco.Rua"].ToString(),
                        StreetNo = result.Rows[i]["Endereco.Numero"].ToString(),
                        Block = result.Rows[i]["Endereco.Bairro"].ToString(),
                        City = result.Rows[i]["Endereco.Cidade"].ToString(),
                        ZipCode = result.Rows[i]["Endereco.CEP"].ToString(),
                        //  Country = result.Rows[i]["Endereco.Pais"].ToString(),
                        Uf = new UfModel
                        {
                            Id = Convert.ToInt32(result.Rows[i]["Endereco.UF.Id"].ToString())
                        },
                        Uf_Id = Convert.ToInt32(result.Rows[i]["Endereco.UF.Id"].ToString())
                    },

                    GerenciaProjetos = Convert.ToInt32(result.Rows[i]["GerenciaProjetos"].ToString()),
                    LimiteHoras = Convert.ToDateTime(result.Rows[i]["LimiteHoras"].ToString())
                };
                if (!String.IsNullOrEmpty(result.Rows[i]["ValidadePassaporte"].ToString()))
                    model.ValidadePassaporte = Convert.ToDateTime(result.Rows[i]["ValidadePassaporte"].ToString());

                model.Especialidades = _collaboratorDAO.Get_Specialties(model.Id);

                modelList.Add(model);
            }
            return modelList;
        }

        public void GetAddressById(ref CollaboratorModel collaborator)
        {
            try
            {
                collaborator.Endereco = _collaboratorDAO.GetAddressById(collaborator.Id);
                collaborator.Endereco.Uf = new UfModel() { Id = collaborator.Endereco.Uf_Id };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollaboratorModel GetCollaboratorMaxHoursById(int id)
        {
            try
            {
                return _collaboratorDAO.GetById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollaboratorModel GetByUserID(int userId)
        {
            try
            {
                return _collaboratorDAO.GetForIdUser(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollaboratorModel GetById(int collaboratorId)
        {
            try
            {
                if (collaboratorId == 0)
                    return null;
                var collaborator = _collaboratorDAO.GetById(collaboratorId);
                GetAddressById(ref collaborator);
                collaborator.Especialidades = _collaboratorDAO.Get_Specialties(collaboratorId);
                collaborator.Tipo = _collaboratorDAO.Get_Type(collaboratorId);

                return collaborator;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public List<StatusModel> GetSpecificStatus()
        {
            this._statusBLL = new StatusBLL();
            return _statusBLL.Get(CollaboratorModel.oObjectType);
        }

        public List<CollaboratorModel> Get()
        {
            try
            {
                return _collaboratorDAO.Get();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollaboratorModel> GetPMs()
        {
            try
            {
                var result = _collaboratorDAO.GetProjectManagerPermission();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollaboratorModel> Get_Active()
        {
            try
            {
                return _collaboratorDAO.Get_Active();
            }
            catch (Exception)
            {
                throw;
            }
        }

       

       

        public List<CollaboratorModel> Get_NotUser()
        {
            try
            {
                return _collaboratorDAO.Get_NotUser();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollaboratorModel> CollaboratorsBySpecialty(SpecialtyModel specialty)
        {
            try
            {
                return _collaboratorDAO.GetFromSpecialty(specialty);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollaboratorModel> Get_CollaboratorBySpecialty(int id)
        {
            try
            {
                return _collaboratorDAO.Get_CollaboratorBySpecialty(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollaboratorModel> GetCollaboratorByFilters(string name, string cpf, string cnpj, int sector, int specialty, int status)
        {
            var modeList = new List<CollaboratorModel>();
            var result = _collaboratorDAO.GetCollaboratorByFilters(name, cpf, cnpj, sector, specialty, status);

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new CollaboratorModel()
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Status = new StatusModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Status.Id"].ToString()),
                        Descricao = result.Rows[i]["Status.Descricao"].ToString()
                    },
                    Nome = result.Rows[i]["Nome"].ToString(),
                    CPF = result.Rows[i]["CPF"].ToString(),
                    CNPJ = result.Rows[i]["CNPJ"].ToString(),
                    Email = result.Rows[i]["Email"].ToString()
                };

                modeList.Add(model);
            }
            return modeList;
        }

        public List<CollaboratorTypeModel> GetActiveTypes()
        {
            try
            {
                return _collaboratorDAO.GetActiveTypes();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SpecialtyModel> Get_Specialties(int id)
        {
            var _userBLL = new UserBLL();
            id = _userBLL.GetCollaborator(id).Id;
            return _collaboratorDAO.Get_Specialties(id);
        }

        public List<SpecialtyModel> GetSpecialtyByCollaborator(CollaboratorModel collaborator)
        {
            try
            {
                if (collaborator.Id == 0)
                    collaborator = GetByUserID(collaborator.User.Id);

                return _specialtyBLL.GetByCollaborator(collaborator);
            }
            catch (Exception)
            {
                throw;
            }
        }



        

       

        public MessageModel ValidateModel(CollaboratorModel model)
        {
            try
            {
                if (model.Tipo.Id != 0)
                {
                    if (model.Tipo.CnpjRequired == 1)
                    {
                        if (string.IsNullOrEmpty(model.CNPJ))
                            return MessageBLL.Generate("Obrigatório informar um CNPJ válido", 99, true);
                    }
                    else if (string.IsNullOrEmpty(model.CNPJ))
                        model.CNPJ = "";
                }
                else
                    return MessageBLL.Generate("Obrigatório selecionar o tipo do colaborador", 99, true);
                if (model.Id == 0 && _collaboratorDAO.VerifyExistence(model) != 0)
                    return MessageBLL.Generate("Colaborador já cadastrado com este CNPJ/CPF", 99, true);

                if (model.Endereco == null)
                    return MessageBLL.Generate("Obrigatório informar o endereço", 99, true);

                if (model.DataNascimento == DateTime.MinValue)
                    return MessageBLL.Generate("Obrigatório informar a data de nascimento", 99, true);

                if (string.IsNullOrEmpty(model.Celular))
                    return MessageBLL.Generate("Obrigatório informar o número de celular", 99, true);



                if (string.IsNullOrEmpty(model.CPF))
                    return MessageBLL.Generate("Obrigatório informar um CPF válido", 99, true);

                if (string.IsNullOrEmpty(model.OrgaoEmissor))
                    return MessageBLL.Generate("Obrigatório informar o orgão emissor", 99, true);

                if (string.IsNullOrEmpty(model.Email))
                    return MessageBLL.Generate("Obrigatório informar um e-mail válido", 99, true);

                if (model.Genero == 0)
                    return MessageBLL.Generate("Obrigatório informar o sexo", 99, true);

                if (model.EstadoCivil == 0)
                    return MessageBLL.Generate("Obrigatório informar o estado civíl", 99, true);

                if (string.IsNullOrEmpty(model.Nome))
                    return MessageBLL.Generate("Obrigatório informar o nome completo", 99, true);

                if (string.IsNullOrEmpty(model.Nacionalidade))
                    return MessageBLL.Generate("Obrigatório informar a nacionalidade", 99, true);

                if (string.IsNullOrEmpty(model.Naturalidade))
                    return MessageBLL.Generate("Obrigatório informar a naturalidade", 99, true);

                if (string.IsNullOrEmpty(model.RG))
                    return MessageBLL.Generate("Obrigatório informar o RG", 99, true);

                if (model.EmissaoRG == DateTime.MinValue)
                    return MessageBLL.Generate("Obrigatório informar a emissão do RG", 99, true);

                if (string.IsNullOrEmpty(model.Telefone))
                    model.Telefone = "";

                if (string.IsNullOrEmpty(model.Passaporte))
                    model.Passaporte = "";

                if (model.ValidadePassaporte == DateTime.MinValue || model.ValidadePassaporte == null)
                    model.ValidadePassaporte = DateTime.Today;

                return MessageBLL.Generate("Colaborador validado com sucesso", 0);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollaboratorModel> Get_ProjectResources(int id)
        {
            var list = new List<CollaboratorModel>();

            var result = _collaboratorDAO.Get_ProjectResources(id);
            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new CollaboratorModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"]),
                    Nome = result.Rows[i]["Nome"].ToString()
                };
                list.Add(model);
            }
            return list;
        }

        public List<SpecialtyModel> GetSpecialtiesForCollaborator(int id)
        {
            return _specialtyBLL.GetByCollaborator(id);
        }
    }
}
