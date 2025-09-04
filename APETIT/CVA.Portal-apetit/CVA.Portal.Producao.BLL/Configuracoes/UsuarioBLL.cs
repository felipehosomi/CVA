using CVA.Portal.Producao.BLL.Util;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Configuracoes;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Configuracoes
{
    public class UsuarioBLL : BaseBLL
    {
        UsuarioEtapaBLL UsuarioEtapaBLL = new UsuarioEtapaBLL();

        public UsuarioBLL()
        {
            DAO.TableName = "@CVA_USUARIO";
        }

        public List<UsuarioModel> Get()
        {
            return DAO.RetrieveModelList<UsuarioModel>();
        }

        public UsuarioModel Get(string code)
        {
            UsuarioModel model = new UsuarioModel();
            if (code != "-1")
            {
                model = DAO.RetrieveModel<UsuarioModel>($"\"U_Usuario\" = '{code.Replace("__", ".")}'");
                if (!String.IsNullOrEmpty(model.Code))
                {
                    model.Senha = EncryptBLL.Decrypt(model.Senha, model.Usuario);
                    model.Ativo = model.AtivoInt == 1;

                    ViewBLL viewBLL = new ViewBLL();
                    model.ViewList = viewBLL.GetPorUsuario(code);
                    model.Filiais = GetFiliaisByUserCode(code);
                }
                else
                {
                    throw new Exception("Usuário não encontrado!");
                }
            }
            model.EtapaList = UsuarioEtapaBLL.GetByUsuario(model.Code);
            foreach (var item in model.EtapaList)
            {
                item.Selected = item.SelectedInt == 1;
            }

            return model;
        }

        private List<BPLModel> GetFiliaisByUserCode(string userCode)
        {
            var bplList = DAO.FillListFromCommand<BPLModel>(string.Format(Commands.Resource.GetString("Usuario_GetFiliaisByUserCode"), BaseBLL.Database, userCode));
            return bplList;
        }

        public UsuarioModel GetByFornecedor(string usuario, string senha)
        {
            UsuarioModel model = new UsuarioModel();

            model = DAO.FillModelFromCommand<UsuarioModel>(string.Format(Commands.Resource.GetString("Usuario_Fornecedor"), BaseBLL.Database, usuario, senha));

            model.Ativo = true;
            if (!String.IsNullOrEmpty(model.Code))
            {
                ViewBLL viewBLL = new ViewBLL();
                model.ViewList = viewBLL.GetPorFornecedor();
            }
            else
            {
                throw new Exception("Usuário não encontrado!");
            }
            
            model.EtapaList = UsuarioEtapaBLL.GetByUsuario(model.Code);
            foreach (var item in model.EtapaList)
            {
                item.Selected = item.SelectedInt == 1;
            }

            return model;
        }

        public UsuarioModel GetByCartao(string numeroCartao)
        {
            UsuarioModel model = new UsuarioModel();
            if (numeroCartao != "-1")
            {
                model = DAO.RetrieveModel<UsuarioModel>($"\"U_NumeroCartao\" = '{numeroCartao.Trim()}'");
                if (!String.IsNullOrEmpty(model.Code))
                {
                    model.Senha = EncryptBLL.Decrypt(model.Senha, model.Usuario);
                    model.Ativo = model.AtivoInt == 1;

                    ViewBLL viewBLL = new ViewBLL();
                    model.ViewList = viewBLL.GetPorUsuario(model.Usuario);
                }
                else
                {
                    throw new Exception("Usuário não encontrado!");
                }
            }
            model.EtapaList = UsuarioEtapaBLL.GetByUsuario(model.Code);
            foreach (var item in model.EtapaList)
            {
                item.Selected = item.SelectedInt == 1;
            }

            return model;
        }


        public string Login(string codUsuario, string senha)
        {
            Commands.SetResourceManager();

            UsuarioModel model = this.Get(codUsuario);
            if (String.IsNullOrEmpty(model.Code))
            {
                throw new Exception("Usuário não encontrado. Verifique os caracteres maiúsculos/minúsculos.");
            }
            if (model.Senha != senha)
            {
                throw new Exception("Senha inválida");
            }
            if (!model.Ativo)
            {
                throw new Exception("Usuário desativado");
            }
            return "";
        }

        public string LoginFornecedor(string usuario, string senha)
        {
            Commands.SetResourceManager();

            UsuarioModel model = this.GetByFornecedor(usuario, senha);
            if (String.IsNullOrEmpty(model.Code))
            {
                throw new Exception("Usuário não encontrado. Verifique o número do cartão.");
            }
            if (!model.Ativo)
            {
                throw new Exception("Usuário desativado");
            }
            return "";
        }

        public string LoginCartao(string numeroCartao)
        {
            Commands.SetResourceManager();

            UsuarioModel model = this.GetByCartao(numeroCartao);
            if (String.IsNullOrEmpty(model.Code))
            {
                throw new Exception("Usuário não encontrado. Verifique o número do cartão.");
            }
            if (!model.Ativo)
            {
                throw new Exception("Usuário desativado");
            }
            return "";
        }

        public void Create(UsuarioModel model)
        {
            DAO.Model = model;
            model.AtivoInt = Convert.ToInt32(model.Ativo);
            model.Senha = EncryptBLL.Encrypt(model.Senha, model.Usuario);
            DAO.CreateModel();
            UsuarioEtapaBLL.Create(model);
        }

        public void Update(UsuarioModel model)
        {
            DAO.Model = model;
            model.AtivoInt = Convert.ToInt32(model.Ativo);
            model.Senha = EncryptBLL.Encrypt(model.Senha, model.Usuario);
            DAO.UpdateModel();
            UsuarioEtapaBLL.Update(model);
        }

        public UsuarioIDModel GetUserID(string userCode)
        {
            try
            {
                return DAO.FillModelFromCommand<UsuarioIDModel>(String.Format(Commands.Resource.GetString("Usuario_GetUserID"), BaseBLL.Database, userCode));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
