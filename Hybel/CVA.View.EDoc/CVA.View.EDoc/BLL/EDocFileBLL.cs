using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Util;
using CVA.View.EDoc.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CVA.View.EDoc.BLL
{
    public class EDocFileBLL
    {
        public static string GenerateFile(EDocFilterModel filterModel)
        {
            string msg = String.Empty;
            if (!Directory.Exists(filterModel.Diretorio))
            {
                return "Diretório não encontrado!";
            }

            FileWriterUtil fileWriterUtil = new FileWriterUtil(Path.Combine(filterModel.Diretorio, $"Edoc{DateTime.Today.ToString("ddMMyyyy")}.txt"));
            try
            {
                EDocModel eDocModel = new EDocModel();
                fileWriterUtil.Separator = "|";
                fileWriterUtil.StartsWithSeparator = true;
                fileWriterUtil.DecimalSeparator = ",";
                fileWriterUtil.DateFormat = "ddMMyyyy";
                fileWriterUtil.AlphaNumericPaddingType = AddOn.Common.Enums.PaddingTypeEnum.None;
                fileWriterUtil.NumericPaddingType = AddOn.Common.Enums.PaddingTypeEnum.None;
                fileWriterUtil.DecimalEmptyIfZero = true;

                EDocSqlBLL sqlBLL = new EDocSqlBLL(filterModel);
                string sql = sqlBLL.Get0000Sql();

                eDocModel.EDoc0000Model = new CrudController().FillModelAccordingToSql<EDoc0000Model>(sql);
                eDocModel.EDoc0000Model.Layout = filterModel.VersaoLayout;
                eDocModel.EDoc0000Model.Finalidade = filterModel.FinalidadeArquivo;
                fileWriterUtil.WriteLine(eDocModel.EDoc0000Model);

                eDocModel.EDoc0001Model = new EDoc0001Model();
                fileWriterUtil.WriteLine(eDocModel.EDoc0001Model);

                sql = sqlBLL.Get0005Sql();
                eDocModel.EDoc0005Model = new CrudController().FillModelAccordingToSql<EDoc0005Model>(sql);
                if (String.IsNullOrEmpty(eDocModel.EDoc0005Model.Nome))
                {
                    throw new Exception("Dados complementares do contribuinte não encontrados, verifique o cadastro no menu CVA - EDoc -> Contribuinte");
                }
                fileWriterUtil.WriteLine(eDocModel.EDoc0005Model);

                eDocModel.EDoc0030Model = new EDoc0030Model();
                fileWriterUtil.WriteLine(eDocModel.EDoc0030Model);
                
                sql = sqlBLL.Get0100Sql();
                eDocModel.EDoc0100Model = new CrudController().FillModelAccordingToSql<EDoc0100Model>(sql);
                if (String.IsNullOrEmpty(eDocModel.EDoc0100Model.Nome))
                {
                    throw new Exception("Dados do contador responsável não encontrados, verifique o cadastro de Colaboradores");
                }
                fileWriterUtil.WriteLine(eDocModel.EDoc0100Model);

                sql = sqlBLL.Get0150Sql();
                eDocModel.EDoc0150Model = new CrudController().FillModelListAccordingToSql<EDoc0150Model>(sql);
                fileWriterUtil.WriteLine<EDoc0150Model>(eDocModel.EDoc0150Model);

                sql = sqlBLL.Get0200Sql();
                eDocModel.EDoc0200Model = new CrudController().FillModelListAccordingToSql<EDoc0200Model>(sql);
                fileWriterUtil.WriteLine<EDoc0200Model>(eDocModel.EDoc0200Model);

                sql = sqlBLL.Get0400Sql();
                eDocModel.EDoc0400Model = new CrudController().FillModelListAccordingToSql<EDoc0400Model>(sql);
                fileWriterUtil.WriteLine<EDoc0400Model>(eDocModel.EDoc0400Model);

                EDoc0990Model model0990 = new EDoc0990Model();
                model0990.QtdeLinhas = 1;  // 0000
                model0990.QtdeLinhas += 1; // 0001
                model0990.QtdeLinhas += 1; // 0005
                model0990.QtdeLinhas += 1; // 0030
                model0990.QtdeLinhas += 1; // 0100
                model0990.QtdeLinhas += eDocModel.EDoc0150Model.Count;
                model0990.QtdeLinhas += eDocModel.EDoc0200Model.Count;
                model0990.QtdeLinhas += eDocModel.EDoc0400Model.Count;
                model0990.QtdeLinhas += 1; // 0990
                fileWriterUtil.WriteLine(model0990);

                eDocModel.EDocC001Model= new EDocC001Model();
                fileWriterUtil.WriteLine(eDocModel.EDocC001Model);

                sql = sqlBLL.GetC020Sql();
                eDocModel.EDocC020Model = new CrudController().FillModelListAccordingToSql<EDocC020Model>(sql);
                //fileWriterUtil.WriteLine(modelC020);

                sql = sqlBLL.GetC300Sql();
                eDocModel.EDocC300Model = new CrudController().FillModelListAccordingToSql<EDocC300Model>(sql);
                //fileWriterUtil.WriteLine(modelC300);

                foreach (var itemC020 in eDocModel.EDocC020Model)
                {
                    fileWriterUtil.WriteLine(itemC020);
                    List<EDocC300Model> itens = eDocModel.EDocC300Model.Where(m => m.DocEntry == itemC020.DocEntry && m.ObjType == itemC020.ObjType && m.UsageId == itemC020.UsageId).ToList();
                    foreach (var itemC3000 in itens)
                    {
                        fileWriterUtil.WriteLine(itemC3000);
                    }
                }

                EDocC990Model modelC990 = new EDocC990Model();
                modelC990.QtdeLinhas = 1; // C001
                modelC990.QtdeLinhas += eDocModel.EDocC020Model.Count;
                modelC990.QtdeLinhas += eDocModel.EDocC300Model.Count;
                modelC990.QtdeLinhas += 1; // C990
                fileWriterUtil.WriteLine(modelC990);

                eDocModel.EDoc9001Model = new EDoc9001Model();
                fileWriterUtil.WriteLine(eDocModel.EDoc9001Model);

                List<EDoc9900Model> model9900 = new List<EDoc9900Model>();
                model9900.Add(new EDoc9900Model() { SiglaLinha = "0000", QtdeLinhas = 1 });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "0001", QtdeLinhas = 1 });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "0005", QtdeLinhas = 1 });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "0030", QtdeLinhas = 1 });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "0100", QtdeLinhas = 1 });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "0150", QtdeLinhas = eDocModel.EDoc0150Model.Count });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "0200", QtdeLinhas = eDocModel.EDoc0200Model.Count });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "0400", QtdeLinhas = eDocModel.EDoc0400Model.Count });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "0990", QtdeLinhas = 1});
                model9900.Add(new EDoc9900Model() { SiglaLinha = "C001", QtdeLinhas = 1 });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "C020", QtdeLinhas = eDocModel.EDocC020Model.Count });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "C300", QtdeLinhas = eDocModel.EDocC300Model.Count });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "C990", QtdeLinhas = 1 });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "9001", QtdeLinhas = 1 });
                model9900.Add(new EDoc9900Model() { SiglaLinha = "9990", QtdeLinhas = model9900.Count + 1});
                fileWriterUtil.WriteLine<EDoc9900Model>(model9900);

                eDocModel.EDoc9990Model = new EDoc9990Model();
                eDocModel.EDoc9990Model.QtdeLinhas = model9900.Count + 1;
                fileWriterUtil.WriteLine(eDocModel.EDoc9990Model);

                eDocModel.EDoc9999Model= new EDoc9999Model();
                eDocModel.EDoc9999Model.QtdeLinhas = fileWriterUtil.WrittenLinesQuantity + 1;
                fileWriterUtil.WriteLine(eDocModel.EDoc9999Model);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                fileWriterUtil.CloseFile();
            }
            return msg;
        }
    }
}
