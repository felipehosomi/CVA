using System;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class ContactBLL
    {
        #region Atributos
        private ContactDAO _contactDAO { get; set; }
        #endregion

        #region Construtor
        public ContactBLL()
        {
            this._contactDAO = new ContactDAO();
        }
        #endregion

        public MessageModel Insert(ContactModel contact)
        {
            try
            {
                var isValid = ValidateFields(ref contact);
                if (isValid.Error != null)
                    return isValid;

                //if (contact.Id != 0)
                //    return Update(contact);
                
                var contactId = _contactDAO.Save(contact);
                if(contactId != 0)
                    return MessageBLL.Generate("Contato gerado com sucesso!", contactId);
                return MessageBLL.Generate("Erro ao salvar contato!", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }
        
        private MessageModel ValidateFields(ref ContactModel contact)
        {
            try
            {
                if (string.IsNullOrEmpty(contact.CellPhone))
                    contact.CellPhone = "";
                if (string.IsNullOrEmpty(contact.Email))
                    contact.Email = "";
                if (string.IsNullOrEmpty(contact.Name))
                    contact.Name = "";
                if (string.IsNullOrEmpty(contact.Phone))
                    contact.Phone = "";
                if (string.IsNullOrEmpty(contact.Site))
                    contact.Site = "";

                return MessageBLL.Generate("Contato validado com sucesso", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private MessageModel Update(ContactModel contact)
        {
            try
            {
                var update = _contactDAO.Update(contact);
                if (update > 0)
                    return MessageBLL.Generate("Contato atualizado com sucesso", 0);
                return MessageBLL.Generate("Erro ao atualizar contato", 99, true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
