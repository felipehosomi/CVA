using CVA.AddOn.Common.DAO;
using CVA.Hybel.Fesmo.Model;
using CVA.Hybel.Fesmo.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Hybel.Fesmo.BLL
{
    public class OrdemProducaoBLL
    {
        public string ExecutaIntegracao(string diretorio)
        {
            try
            {
                SqlDAO dao = new SqlDAO();

                string sql = " EXEC SP_CVA_FESMO_OP ";
                List<OrdemProducaoModel> list = dao.FillListFromCommand<OrdemProducaoModel>(sql);

                IEnumerable<IGrouping<int, OrdemProducaoModel>> groupedByOP = list.GroupBy(m => m.NrOP);

                foreach (var itemByOP in groupedByOP)
                {
                    string file = Path.Combine(diretorio, itemByOP.Key.ToString().PadLeft(11, '0') + ".txt");

                    StreamWriter sw = new StreamWriter(file);
                    sw.WriteLine(itemByOP.ElementAt(0).ItemCode);
                    foreach (var item in itemByOP)
                    {
                        sw.WriteLine(item.Serie);
                    }
                    sw.Close();
                    dao.ExecuteNonQuery(String.Format(SQL.OrdemProducao_UpdateStatus, itemByOP.Key));
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }            
        }
    }
}
