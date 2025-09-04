using B1SLayer;
using Fibra.ItemPricing.Models;
using System;
using System.Threading.Tasks;
using System.Net;

namespace Fibra.ItemPricing.Infrastructure
{
    static class UserStructures
    {
        internal static async Task VerifyAsync()
        {
            // Verifica se o campo U_WhsCode na OPLN é existente
            var userField = await ItemPricing.SLConnection.Request("UserFieldsMD(TableName='OPLN')")
                .Filter(@"Name eq 'WhsCode'")
                .AllowHttpStatus(HttpStatusCode.NotFound)
                .GetAsync<UserFieldsMD>();

            if (String.IsNullOrEmpty(userField.Name))
            {
                userField = new UserFieldsMD
                {
                    Name = "WhsCode",
                    Type = "db_Alpha",
                    Size = 8,
                    Description = "Código do depósito",
                    TableName = "OPLN",
                    LinkedSystemObject = "ulWarehouses"
                };

                CreateUserField(userField).Wait();
            }

            var userTable = await ItemPricing.SLConnection.Request("UserTablesMD('CVA_OICJ')").AllowHttpStatus(HttpStatusCode.NotFound).GetAsync<UserTablesMd>();

            if (String.IsNullOrEmpty(userTable.TableName))
            {
                userTable = new UserTablesMd
                {
                    TableName = "CVA_OICJ",
                    TableDescription = "CVA|Item Cost Journal",
                    TableType = "bott_NoObjectAutoIncrement"
                };

                CreateUserTable(userTable).Wait();

                userField = new UserFieldsMD
                {
                    Name = "CreateDate",
                    Type = "db_Date",
                    Size = 8,
                    Description = "Data de criação",
                    TableName = "@CVA_OICJ"
                };

                CreateUserField(userField).Wait();

                userField = new UserFieldsMD
                {
                    Name = "UpdateDate",
                    Type = "db_Date",
                    Size = 8,
                    Description = "Data de atualização",
                    TableName = "@CVA_OICJ"
                };

                CreateUserField(userField).Wait();

                userField = new UserFieldsMD
                {
                    Name = "DocEntry",
                    Type = "db_Numeric",
                    Size = 10,
                    Description = "Nº do documento",
                    TableName = "@CVA_OICJ",
                    EditSize = 10
                };

                CreateUserField(userField).Wait();

                userField = new UserFieldsMD
                {
                    Name = "DocType",
                    Type = "db_Alpha",
                    Size = 10,
                    Description = "Tipo do documento",
                    TableName = "@CVA_OICJ"
                };

                CreateUserField(userField).Wait();

                userField = new UserFieldsMD
                {
                    Name = "ItemCode",
                    Type = "db_Alpha",
                    Size = 50,
                    Description = "Cód. Item",
                    TableName = "@CVA_OICJ"
                };

                CreateUserField(userField).Wait();

                userField = new UserFieldsMD
                {
                    Name = "WhsCode",
                    Type = "db_Alpha",
                    Size = 8,
                    Description = "Cód. Depósito",
                    TableName = "@CVA_OICJ"
                };

                CreateUserField(userField).Wait();

                userField = new UserFieldsMD
                {
                    Name = "PriceList",
                    Type = "db_Numeric",
                    Size = 10,
                    Description = "Lista de preço",
                    TableName = "@CVA_OICJ",
                    EditSize = 10
                };

                CreateUserField(userField).Wait();

                userField = new UserFieldsMD
                {
                    Name = "Cost",
                    Type = "db_Float",
                    SubType = "st_Sum",
                    Description = "Custo",
                    TableName = "@CVA_OICJ",
                };

                CreateUserField(userField).Wait();

                userField = new UserFieldsMD
                {
                    Name = "Status",
                    Type = "db_Alpha",
                    Size = 1,
                    Description = "Status",
                    TableName = "@CVA_OICJ",
                    ValidValuesMd = new System.Collections.Generic.List<ValidValuesMD> { new ValidValuesMD { Value = "O", Description = "Aberto" },
                        new ValidValuesMD { Value = "C", Description = "Fechado" },
                        new ValidValuesMD { Value = "E", Description = "Erro" } }
                };

                CreateUserField(userField).Wait();

                userField = new UserFieldsMD
                {
                    Name = "ErrorMsg",
                    Type = "db_Alpha",
                    Size = 254,
                    Description = "Mensagem de erro",
                    TableName = "@CVA_OICJ"
                };

                CreateUserField(userField).Wait();
            }
        }

        internal static async Task CreateUserField(UserFieldsMD userField)
        {
            await ItemPricing.SLConnection.Request("UserFieldsMD").PostAsync(userField);
        }

        internal static async Task CreateUserTable(UserTablesMd userTable)
        {
            await ItemPricing.SLConnection.Request("UserTablesMD").PostAsync(userTable);
        }
    }
}
