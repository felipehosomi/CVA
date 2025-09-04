using CVA.AddOn.Common.Controllers;
using CVA.View.EmailAtividade.MODEL;
using CVA.View.EmailAtividade.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAtividade.BLL
{
    public class BranchBLL
    {
        public List<BranchModel> GetBranches()
        {
            CrudController controller = new CrudController();
            return controller.FillModelListAccordingToSql<BranchModel>(Query.Branch_Get);
        }
    }
}
