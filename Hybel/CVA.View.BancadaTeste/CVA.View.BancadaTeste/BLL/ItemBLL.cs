using CVA.View.BancadaTeste.DAO;
using CVA.View.BancadaTeste.Model;
using CVA.View.BancadaTeste.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.BancadaTeste.BLL
{
    public class ItemBLL
    {
        public static string GenerateFile(string path, string op)
        {
            SqlUtil sqlUtil = new SqlUtil(ConfigurationManager.AppSettings["Server"], ConfigurationManager.AppSettings["Database"], ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"]);
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                return $"Erro ao deletar arquivo {path}: {ex.Message}";
            }
            try
            {
                ItemModel itemModel = sqlUtil.FillModelAccordingToSql<ItemModel>(String.Format(SQL.Item_GetBySeries, op));
                if (!String.IsNullOrEmpty(itemModel.ItemCode))
                {
                    StreamWriter sw = new StreamWriter(path);
                    string line = $"{itemModel.Serie};{itemModel.ItemCode};{itemModel.ItemCode.Substring(4, 1)}";
                    double estagio;
                    double deslocamento;

                    double.TryParse(itemModel.Estagio1, out estagio);
                    double.TryParse(itemModel.Deslocamento1, out deslocamento);
                    if (estagio != 0 || deslocamento != 0)
                    {
                        line += $";{itemModel.Estagio1};{itemModel.Deslocamento1}";
                    }

                    double.TryParse(itemModel.Estagio2, out estagio);
                    double.TryParse(itemModel.Deslocamento2, out deslocamento);
                    if (estagio != 0 || deslocamento != 0)
                    {
                        line += $";{itemModel.Estagio2};{itemModel.Deslocamento2}";
                    }

                    double.TryParse(itemModel.Estagio3, out estagio);
                    double.TryParse(itemModel.Deslocamento3, out deslocamento);
                    if (estagio != 0 || deslocamento != 0)
                    {
                        line += $";{itemModel.Estagio3};{itemModel.Deslocamento3}";
                    }

                    double.TryParse(itemModel.Estagio4, out estagio);
                    double.TryParse(itemModel.Deslocamento4, out deslocamento);
                    if (estagio != 0 || deslocamento != 0)
                    {
                        line += $";{itemModel.Estagio4};{itemModel.Deslocamento4}";
                    }

                    double.TryParse(itemModel.Estagio5, out estagio);
                    double.TryParse(itemModel.Deslocamento5, out deslocamento);
                    if (estagio != 0 || deslocamento != 0)
                    {
                        line += $";{itemModel.Estagio5};{itemModel.Deslocamento5}";
                    }

                    double.TryParse(itemModel.Estagio6, out estagio);
                    double.TryParse(itemModel.Deslocamento6, out deslocamento);
                    if (estagio != 0 || deslocamento != 0)
                    {
                        line += $";{itemModel.Estagio6};{itemModel.Deslocamento6}";
                    }

                    sw.WriteLine(line);
                    sw.Close();
                }
                else
                {
                    return $"OP {op} não encontrada!";
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
