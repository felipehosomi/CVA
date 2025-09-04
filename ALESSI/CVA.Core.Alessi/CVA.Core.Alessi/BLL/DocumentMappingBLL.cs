using CVA.Core.Alessi.DAO.UserTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.BLL
{
    public class DocumentMappingBLL
    {
        public List<string> GetLayouts(int objType)
        {
            return new DocumentMappingDAO().GetLayouts(objType);
        }
    }
}
