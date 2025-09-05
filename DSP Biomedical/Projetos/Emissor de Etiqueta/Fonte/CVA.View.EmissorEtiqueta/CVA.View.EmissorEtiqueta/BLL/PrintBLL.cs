using CVA.View.EmissorEtiqueta.MODEL;
using CVA.View.EmissorEtiqueta.Resources;
using System;
using System.IO;
using System.Text;

namespace CVA.View.EmissorEtiqueta.BLL
{
    public partial class PrintBLL
    {
        private OitmBLL _oitmBLL { get; set; }
    }

    public partial class PrintBLL
    {
        public PrintBLL(OitmBLL oitmBLL)
        {
            _oitmBLL = oitmBLL;
        }

        public bool CreateFile(VIEWPARAMETER parameters)
        {
            if (parameters == null)
                return false;

            PRINT etiqueta = null;
            string etiquetaModel = string.Empty;
            StreamWriter file = null;
            
            etiqueta = LoadParameters(parameters.ItemCode);
            etiqueta.Esteril = etiqueta.Esteril == "1" ? "STERIL" : "NO STERIL";
            var hashName = Guid.NewGuid().ToString();
            switch (Convert.ToInt16(parameters.Modelo))
            {
                case 1:
                    var arquivo = new StreamReader(@"C:\PrinterSAP\Modelos\Componente.txt", Encoding.GetEncoding(28591));
                    var leitura = arquivo.ReadToEnd();
                    etiquetaModel = String.Format(leitura,
                                                        etiqueta.Descricao,
                                                        etiqueta.DescricaoEstrangeira,
                                                        etiqueta.Model.ToString(),
                                                        etiqueta.Dimensao,
                                                        etiqueta.Ref,
                                                        parameters.Lote,
                                                        parameters.Fabricacao,
                                                        parameters.Validade,
                                                        etiqueta.Anvisa,
                                                        etiqueta.Material,
                                                        etiqueta.Esteril,
                                                        parameters.Quantidade);
                    etiquetaModel = etiquetaModel.Replace("000000", etiqueta.Ref);

                    file = new StreamWriter(@"C:\PrinterSAP\Etiquetas\Componente" + hashName + ".txt", false, Encoding.GetEncoding(28591));

                    file.WriteLine(etiquetaModel);
                    file.Close();
                    CopyFiles(@"C:\PrinterSAP\Etiquetas\Componente" + hashName + ".txt", hashName);
                    File.Delete(@"C:\PrinterSAP\Etiquetas\Componente" + hashName + ".txt");
                    arquivo.Close();
                    break;
                case 2:
                    var arquivoImplante = new StreamReader(@"C:\PrinterSAP\Modelos\Implante.txt", Encoding.GetEncoding(28591));
                    var implanteLeitura = arquivoImplante.ReadToEnd();
                    etiquetaModel = String.Format(implanteLeitura,
                                                        etiqueta.Descricao,
                                                        etiqueta.DescricaoEstrangeira,
                                                        etiqueta.Model,
                                                        etiqueta.Dimensao,
                                                        etiqueta.Ref,
                                                        parameters.Lote,
                                                        parameters.Fabricacao,
                                                        parameters.Validade,
                                                        etiqueta.Anvisa,
                                                        etiqueta.Material,
                                                        etiqueta.Esteril,
                                                        parameters.Quantidade);
                    etiquetaModel = etiquetaModel.Replace("000000", etiqueta.Ref);

                    file = new StreamWriter(@"C:\PrinterSAP\Etiquetas\Implante" + hashName + ".txt", false, Encoding.GetEncoding(28591));
                    file.WriteLine(etiquetaModel);
                    file.Close();
                    CopyFiles(@"C:\PrinterSAP\Etiquetas\Implante" + hashName + ".txt", hashName);
                    File.Delete(@"C:\PrinterSAP\Etiquetas\Implante" + hashName + ".txt");
                    arquivoImplante.Close();
                    break;
                case 3:
                    var arquivoInterna = new StreamReader(@"C:\PrinterSAP\Modelos\Interna.txt", Encoding.GetEncoding(28591));
                    var internaLeitura = arquivoInterna.ReadToEnd();
                    etiquetaModel = String.Format(internaLeitura,
                                                        etiqueta.Descricao,
                                                        etiqueta.DescricaoEstrangeira,
                                                        etiqueta.Dimensao,
                                                        etiqueta.Ref,
                                                        parameters.Lote,
                                                        parameters.Fabricacao,
                                                        parameters.Validade,
                                                        parameters.Quantidade
                                                        );

                    file = new StreamWriter(@"C:\PrinterSAP\Etiquetas\Interna" + hashName + ".txt", false, Encoding.GetEncoding(28591));
                    file.WriteLine(etiquetaModel);
                    file.Close();
                    CopyFiles(@"C:\PrinterSAP\Etiquetas\Interna" + hashName + ".txt", hashName);
                    File.Delete(@"C:\PrinterSAP\Etiquetas\Interna" + hashName + ".txt");
                    arquivoInterna.Close();
                    break;
                default:
                    return false;
            }
            return false;
        }

        private void CopyFiles(string file, string guid)
        {
            File.Copy(file, Pastas.Destino + "etiqueta" + guid + ".txt");
        }

        private PRINT LoadParameters(string itemCode)
        {
            return _oitmBLL.GetEtiqueta(itemCode);
        }
    }
}