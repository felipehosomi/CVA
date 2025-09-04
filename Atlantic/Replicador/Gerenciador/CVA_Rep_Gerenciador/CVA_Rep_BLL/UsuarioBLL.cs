using CVA_Rep_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace CVA_Rep_BLL
{
    public class UsuarioBLL
    {
        UsuarioDAL _usuarioDAL = new UsuarioDAL();
        ConciliadorDAL _conciliadorDAL = new ConciliadorDAL();

        public bool UpdatePassword(int USERID, string password)//Atualiza a senha do usuário selecionado em todas as bases
        {
            CVA_REG_BLL regBLL = new CVA_REG_BLL();
            CVA_REG reg = new CVA_REG();
            {
                reg.INS = DateTime.Now;
                reg.UPD = null;
                reg.STU = 3;
                reg.BAS = 1;
                reg.CODE = USERID + "|" + password;
                reg.OBJ = 3;
                reg.FUNC = 2;
                reg.BAS_ERR = null;
            }

            return regBLL.UpdateUserPassword(reg);
        }

        public List<USER> GetUsers()//Retorna todos os usuários da base consolidadora
        {
            CVA_BASES lastBase = GetLastBase();
            return _usuarioDAL.GetUsers(lastBase);
        }

        public CVA_BASES GetLastBase()//Retorna a base consolidadora
        {
            return _conciliadorDAL.Bases_GetAll().Where(m => m.TIPO == 1).FirstOrDefault();
        }

        public string GeneratePassword()//Gera uma nova senha aleatória
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return password;
        }
    }
}
