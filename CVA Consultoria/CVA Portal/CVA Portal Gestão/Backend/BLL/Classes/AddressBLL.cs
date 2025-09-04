using System;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class AddressBLL
    {
        #region Atributos
        private AddressDAO _addressDAO { get; set; }
        #endregion

        #region Construtor
        public AddressBLL()
        {
            this._addressDAO = new AddressDAO();            
        }
        #endregion

        public MessageModel Save(AddressModel address)
        {
            try
            {
                var valid = IsValid(address);
                if (valid.Error != null)
                    return valid;

                var insert = _addressDAO.Insert(address);
                if (insert != 0)
                    return MessageBLL.Generate("Endereço registrado com sucesso", insert);
                return MessageBLL.Generate("Erro ao salvar endereço", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel IsValid(AddressModel address)
        {
            try
            {
                if (string.IsNullOrEmpty(address.Block))
                    return MessageBLL.Generate("Obrigatório o preenchimento do bairro", 99, true);
                
                if(string.IsNullOrEmpty(address.City))
                    return MessageBLL.Generate("Obrigatório o preenchimento da cidade", 99, true);
                
                if (string.IsNullOrEmpty(address.Street))
                    return MessageBLL.Generate("Obrigatório o preenchimento da rua", 99, true);

                if (string.IsNullOrEmpty(address.StreetNo))
                    return MessageBLL.Generate("Obrigatório o preenchimento o número da rua", 99, true);

                if (address.Id == 0)
                {
                    if (address.Uf == null || address.Uf.Id == 0)
                        return MessageBLL.Generate("Obrigatório a seleção do estado", 99, true);
                }

                if (string.IsNullOrEmpty(address.ZipCode))
                    return MessageBLL.Generate("Obrigatório o preenchimento do cep", 99, true);

                return MessageBLL.Generate("Endereço validado com sucesso", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private MessageModel Update(AddressModel address)
        {
            try
            {
                var update = _addressDAO.Update(address);
                if (update > 0)
                    return MessageBLL.Generate("Endereço atualizado com sucesso", 0);
                else
                    return MessageBLL.Generate("Falha ao atualizar endereço", 99, true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
