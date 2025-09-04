using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Apetit.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            #region [CVA] Campos Filial

            // Cadastro Filial
            userObjectController.InsertUserField("OBPL", "CVA_IdPayTrack", "ID de Identificação PayTrack", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("OBPL", "CVA_Dim1Custo", "C.Custo da Empresa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8);
            userObjectController.InsertUserField("OBPL", "CVA_Dim2Custo", "C.Custo da Filial ", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8);

            #endregion

        }
    }
}