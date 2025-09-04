using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Apetit
{
    public class AttachmentBLL : BaseBLL
    {
        ServiceLayerUtil sl = new ServiceLayerUtil();

        
        public AttachmentsPath GetAttachmentsPath()
        {
            try
            {
                //AttachPath
                return DAO.FillModelFromCommand<AttachmentsPath>(string.Format(Commands.Resource.GetString("ServicoExtra_AttachmentsPath"), Database));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void SaveFile(byte[] item, string path, string file)
        {
            try
            {
                var pathFull = path + file;
                File.WriteAllBytes(pathFull, item);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salver o anexo na pasta: " + ex.Message);
            }
        }

        public async Task<int?> AttachmentSL(string path, List<AttachmentsAPI> file)
        {
            try
            {
                var retorno = string.Empty;

                var objSL = new AttachmentModel();

                objSL.Attachments2_Lines = new List<Attachments2_Lines>();
                var count = 0;
                foreach (var itemFiles in file)
                {
                    objSL.Attachments2_Lines.Add(new Attachments2_Lines
                    {
                        AttachmentDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        FileExtension = FormatFile(itemFiles.type),
                        FileName = itemFiles.fileName.Replace($".{FormatFile(itemFiles.type)}", string.Empty),
                        FreeText = $"[{count}] | " + (string.IsNullOrEmpty(itemFiles.comments) ? "": itemFiles.comments),
                        Override = "tYES",
                        SourcePath = path
                    });
                    count++;
                }

                var retSL = await sl.PostAndReturnAbsoluteEntryAsync("Attachments2", objSL);

                if (retSL.Item1 == 0)
                    throw new Exception($"Erro ao salvar o anexo no SL: " + retSL.Item2);

                return retSL.Item1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FormatFile(string type)
        {
            switch (type.ToLower())
            {
                case "application/pdf":
                    return "pdf";
                case "image/jpeg":
                    return "jpeg";
                case "image/jpg":
                    return "jpg";
                case "application/vnd.ms-excel":
                    return "xls";
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return "xlsx";
                default:
                    return "pdf";
            }
        }
    }
}
