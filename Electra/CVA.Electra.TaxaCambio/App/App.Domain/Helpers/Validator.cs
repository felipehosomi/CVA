using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Helpers
{
    public class Validator
    {
        public static void ForValidId(string propName, Guid id)
        {
            ForValidId(id, propName + " id inválido!");
        }

        public static void ForValidId(Guid id, string errorMessage)
        {
            if (id == Guid.Empty)
                throw new Exception(errorMessage);
        }

        public static void ForValidId(string propName, int id)
        {
            ForValidId(id, propName + " id inválido!");
        }

        public static void ForValidId(int id, string errorMessage)
        {
            if (!(id > 0))
                throw new Exception(errorMessage);
        }

        public static void ForNegative(int number, string propName)
        {
            if (number < 0)
                throw new Exception(propName + " não pode ser negativo!");
        }

        public static void ForNullOrEmptyDefaultMessage(string value, string propName)
        {
            if (String.IsNullOrEmpty(value))
                throw new Exception(propName + " é obrigatório!");
        }

        public static void ForNullOrEmpty(string value, string errorMessage)
        {
            if (String.IsNullOrEmpty(value))
                throw new Exception(errorMessage);
        }

        public static void StringLength(string propName, string stringValue, int maximum)
        {
            StringLength(stringValue, maximum, propName + " não pode ter mais que " + maximum + " caracteres");
        }

        public static void StringLength(string stringValue, int maximum, string message)
        {
            var length = stringValue.Length;
            if (length > maximum)
            {
                throw new Exception(message);
            }
        }
        public static void StringLength(string propName, string stringValue, int minimum, int maximum)
        {
            StringLength(stringValue, minimum, maximum, propName + " deve ter de " + minimum + " à " + maximum + " caracteres!");
        }

        public static void StringLength(string stringValue, int minimum, int maximum, string message)
        {
            if (String.IsNullOrEmpty(stringValue))
                stringValue = String.Empty;

            var length = stringValue.Length;
            if (length < minimum || length > maximum)
            {
                throw new Exception(message);
            }
        }

        public static bool AreEqual(string a, string b)
        {
            return (a == b);
        }

        public static void AreEqual(string a, string b, string errorMessage)
        {
            if (!AreEqual(a, b))
                throw new Exception(errorMessage);
        }

        public static bool validarCPF(string CPF)
        {
            try
            {
                int[] mt1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] mt2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string TempCPF;
                string Digito;
                int soma;
                int resto;

                CPF = CPF.Trim();
                CPF = CPF.Replace(".", "").Replace("-", "");

                if (CPF.Length != 11)
                    return false;

                TempCPF = CPF.Substring(0, 9);
                soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += int.Parse(TempCPF[i].ToString()) * mt1[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                Digito = resto.ToString();
                TempCPF = TempCPF + Digito;
                soma = 0;

                for (int i = 0; i < 10; i++)
                    soma += int.Parse(TempCPF[i].ToString()) * mt2[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                Digito = Digito + resto.ToString();

                return CPF.EndsWith(Digito);
            }
            catch
            { return false; }
        }
    }
}
