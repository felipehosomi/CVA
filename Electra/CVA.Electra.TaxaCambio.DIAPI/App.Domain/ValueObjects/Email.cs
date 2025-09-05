using App.Domain.Helpers;

namespace App.Domain.ValueObject
{
    public class Email
    {
        public const int EnderecoMaxLength = 254;
        public string Endereco { get; private set; }

        //Construtor do EntityFramework
        protected Email()
        {

        }

        public Email(string endereco)
        {
            Validator.ForNullOrEmptyDefaultMessage(endereco, "E-mail");
            Validator.StringLength("E-mail", endereco, EnderecoMaxLength);
            endereco.IsEmailValid();

            Endereco = endereco;
        }




    }
}
