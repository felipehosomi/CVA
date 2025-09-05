using ClosedXML.Excel;
using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Util;
using CVA.View.DIME.DAO.Resources;
using CVA.View.DIME.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CVA.View.DIME.BLL
{
    public class DimeBLL
    {
        public static string GenerateFile(string dimeCode)
        {
            DimeConfigModel configModel = new DimeConfigModel();
            BusinessPlaces branch = SBOApp.Company.GetBusinessObject(BoObjectTypes.oBusinessPlaces);

            try
            {
                configModel = new CrudController("@CVA_DIME").RetrieveModel<DimeConfigModel>($"Code = '{dimeCode}'");

                if (configModel.Filial == 0)
                {
                    branch.GetByKey(1);
                }
                else
                {
                    branch.GetByKey(configModel.Filial);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DIME Configurações: " + ex.Message);
            }
            DimeModel model = new DimeModel();

            try
            {
                model.ContadorModel = new CrudController().FillModelAccordingToSql<DimeContadorModel>(SQL.Contador_Get);
                if (String.IsNullOrEmpty(model.ContadorModel.Nome))
                {
                    throw new Exception("Contador responsável não encontrado, verifique o cadastro de colaboradores");
                }

                model.EmpresaModel = new DimeEmpresaModel();
                model.EmpresaModel.Inscricao = branch.FederalTaxID2.Replace(".", "");
                model.EmpresaModel.Nome = branch.BPLName;
                model.EmpresaModel.Periodo = configModel.DataDe.ToString("MMyyyy");
                model.EmpresaModel.TipoDeclaracao = configModel.TipoDeclaracao;
                model.EmpresaModel.TipoApuracao = configModel.TipoApuracao;
                model.EmpresaModel.TransCredito = configModel.TransCredito;
                model.EmpresaModel.TipoMovimento = configModel.TipoMovimento;
                model.EmpresaModel.SubstitutoTributario = configModel.SubstitutoTributario;
                model.EmpresaModel.EscritaContabil = configModel.EscritaContabil;
                model.EmpresaModel.QtdeEmpregados = configModel.QtdeEmpregados;
            }
            catch (Exception ex)
            {
                throw new Exception("DIME Contador: " + ex.Message);
            }
            CrudController crudController = new CrudController();
            string sql = $" EXEC SP_CVA_DIME_22 '{dimeCode}' ";

            try
            {
                model.Quadro01 = crudController.FillModelListAccordingToSql<DimeDocumentModel>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("DIME 22: " + ex.Message);
            }
            try
            {
                sql = $" EXEC SP_CVA_DIME_23 '{dimeCode}' ";
                model.Quadro02 = crudController.FillModelListAccordingToSql<DimeDocumentModel>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("DIME 23: " + ex.Message);
            }

            // Registro 24 - Quadro 03
            model.Quadro03 = GetQuadro03(model.Quadro01, model.Quadro02);

            // Registro 25 - Quadro 04
            model.Quadro04 = GetQuadro04(model.Quadro03, configModel);

            // Registro 26 - Quadro 05
            model.Quadro05 = GetQuadro05(model.Quadro03, configModel);

            // Registro 30 - Quadro 09
            model.Quadro09 = GetQuadro09(model.Quadro04, model.Quadro05, configModel.Periodo, configModel.Filial);

            try
            {
                // Registro 32 - Quadro 11
                sql = $" EXEC SP_CVA_DIME_32 '{dimeCode}' ";
                model.Quadro11 = crudController.FillModelListAccordingToSql<DimeSomatorioModel>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("DIME 32: " + ex.Message);
            }
            try
            {
                // Registro 33 - Quadro 12
                sql = String.Format(SQL.DimeQuadro12_Get, configModel.Periodo, configModel.Filial);
                model.Quadro12 = crudController.FillModelListAccordingToSql<DimeQuadro12Model>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("DIME 33: " + ex.Message);
            }
            try
            {
                sql = String.Format(SQL.DimeQuadro46_Get, configModel.Periodo, configModel.Filial);
                model.Quadro46 = crudController.FillModelListAccordingToSql<DimeQuadro46Model>(sql);
                int i = 1;
                foreach (var item in model.Quadro46)
                {
                    item.Sequencia = i;
                    i++;
                }

                DimeQuadro46Model quadro46TotalModel = new DimeQuadro46Model();
                quadro46TotalModel.Sequencia = 990;
                quadro46TotalModel.Identificacao = "0";
                quadro46TotalModel.Valor = model.Quadro46.Sum(m => m.Valor);

                model.Quadro46.Add(quadro46TotalModel);

            }
            catch (Exception ex)
            {
                throw new Exception("DIME 46: " + ex.Message);
            }
            try
            {
                sql = $" EXEC SP_CVA_DIME_49 '{dimeCode}' ";
                model.Quadro49 = crudController.FillModelListAccordingToSql<DimeQuadro49Model>(sql);
                DimeQuadro49Model total49 = new DimeQuadro49Model();
                total49.UF = "TT";
                total49.Tipo = "49";
                total49.Quadro = "49";
                total49.BaseCalculo = model.Quadro49.Sum(d => d.BaseCalculo);
                total49.Outras = model.Quadro49.Sum(d => d.Outras);
                total49.OutrosProdutos = model.Quadro49.Sum(d => d.OutrosProdutos);
                total49.Petroleo = model.Quadro49.Sum(d => d.Petroleo);
                total49.Valor = model.Quadro49.Sum(d => d.Valor);
                model.Quadro49.Add(total49);
            }
            catch (Exception ex)
            {
                throw new Exception("DIME 49: " + ex.Message);
            }

            model.Quadro50 = GetQuadro50(dimeCode);
            if (configModel.InfoEstoqueDespesa == 1)
            {
                model.Quadro80 = crudController.FillModelListAccordingToSql<DimeQuadro80Model>(GetSQLProc(configModel, "SP_CVA_DIME_80"));
                model.Quadro84 = crudController.FillModelListAccordingToSql<DimeQuadro84Model>(GetSQLProc(configModel, "SP_CVA_DIME_84"));
            }
            if (configModel.DemonstrativosContabeis == 1)
            {
                model.Quadro81 = crudController.FillModelListAccordingToSql<DimeQuadro81Model>(GetSQLProc(configModel, "SP_CVA_DIME_81"));
                model.Quadro82 = crudController.FillModelListAccordingToSql<DimeQuadro82Model>(GetSQLProc(configModel, "SP_CVA_DIME_82"));
                model.Quadro83 = crudController.FillModelListAccordingToSql<DimeQuadro83Model>(GetSQLProc(configModel, "SP_CVA_DIME_83"));
            }

            string msg = GenerateFile(configModel.Diretorio, model);
            if (String.IsNullOrEmpty(msg))
            {
                msg = GenerateExcel(configModel.Diretorio, model, configModel);
            }

            return msg;
        }

        #region GetQuadro03
        public static List<DimeSomatorioModel> GetQuadro03(List<DimeDocumentModel> quadro01, List<DimeDocumentModel> quadro02)
        {
            List<DimeSomatorioModel> list = new List<DimeSomatorioModel>();
            DimeSomatorioModel model = new DimeSomatorioModel();
            if (quadro01.Sum(d => d.ValorContabil) > 0)
            {
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "010";
                model.Descricao = "Valor contábil";
                model.Valor = quadro01.Sum(d => d.ValorContabil);
                list.Add(model);
            }
            if (quadro01.Sum(d => d.BaseCalculo) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "020";
                model.Descricao = "Base de cálculo";
                model.Valor = quadro01.Sum(d => d.BaseCalculo);
                list.Add(model);
            }
            if (quadro01.Sum(d => d.ImpostoCreditado) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "030";
                model.Descricao = "Imposto creditado";
                model.Valor = quadro01.Sum(d => d.ImpostoCreditado);
                list.Add(model);
            }
            if (quadro01.Sum(d => d.Isentas) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "040";
                model.Descricao = "Operações isentas ou não tributadas";
                model.Valor = quadro01.Sum(d => d.Isentas);
                list.Add(model);
            }
            if (quadro01.Sum(d => d.Outras) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "050";
                model.Descricao = "Outras operações sem crédito do imposto";
                model.Valor = quadro01.Sum(d => d.Outras);
                list.Add(model);
            }
            if (quadro01.Sum(d => d.BaseCalculoImpostoRetido) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "053";
                model.Descricao = "Base de Cálculo Imposto Retido";
                model.Valor = quadro01.Sum(d => d.BaseCalculoImpostoRetido);
                list.Add(model);
            }
            if (quadro01.Sum(d => d.ImpostoRetido) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "054";
                model.Descricao = "Imposto Retido";
                model.Valor = quadro01.Sum(d => d.ImpostoRetido);
                list.Add(model);
            }
            if (quadro01.Sum(d => d.DiferencaAliquota) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "057";
                model.Descricao = "Imposto Diferencial Alíquota";
                model.Valor = quadro01.Sum(d => d.DiferencaAliquota);
                list.Add(model);
            }

            if (quadro02.Sum(d => d.ValorContabil) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "060";
                model.Descricao = "Valor contábil";
                model.Valor = quadro02.Sum(d => d.ValorContabil);
                list.Add(model);
            }
            if (quadro02.Sum(d => d.BaseCalculo) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "070";
                model.Descricao = "Base de cálculo";
                model.Valor = quadro02.Sum(d => d.BaseCalculo);
                list.Add(model);
            }
            if (quadro02.Sum(d => d.ImpostoCreditado) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "080";
                model.Descricao = "Imposto debitado";
                model.Valor = quadro02.Sum(d => d.ImpostoCreditado);
                list.Add(model);
            }
            if (quadro02.Sum(d => d.Isentas) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "090";
                model.Descricao = "Operações isentas ou não tributadas";
                model.Valor = quadro02.Sum(d => d.Isentas);
                list.Add(model);
            }
            if (quadro02.Sum(d => d.Outras) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "100";
                model.Descricao = "Outras operações sem débito do imposto";
                model.Valor = quadro02.Sum(d => d.Outras);
                list.Add(model);
            }
            if (quadro02.Sum(d => d.BaseCalculoImpostoRetido) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "103";
                model.Descricao = "Base de Cálculo Imposto Retido";
                model.Valor = quadro02.Sum(d => d.BaseCalculoImpostoRetido);
                list.Add(model);
            }
            if (quadro02.Sum(d => d.ImpostoRetido) > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "24";
                model.Quadro = "03";
                model.Item = "104";
                model.Descricao = "Imposto Retido";
                model.Valor = quadro02.Sum(d => d.ImpostoRetido);
                list.Add(model);
            }

            return list;
        }
        #endregion

        #region GetQuadro04
        public static List<DimeSomatorioModel> GetQuadro04(List<DimeSomatorioModel> quadro03, DimeConfigModel configModel)
        {
            List<DimeSomatorioModel> list = new List<DimeSomatorioModel>();
            DimeSomatorioModel impostoDebitado = quadro03.FirstOrDefault(d => d.Item == "080");

            DimeSomatorioModel model = new DimeSomatorioModel();
            if (impostoDebitado != null)
            {
                model.Tipo = "25";
                model.Quadro = "04";
                model.Item = "010";
                model.Descricao = "Débito pelas saídas";
                model.Valor = impostoDebitado.Valor;
                list.Add(model);
            }

            if (configModel.DebitoAtivoPermanente > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "25";
                model.Quadro = "04";
                model.Item = "020";
                model.Descricao = "Débito por diferencial de alíquota de ativo permanente";
                model.Valor = configModel.DebitoAtivoPermanente;
                list.Add(model);
            }

            if (configModel.DiferencialAliquotaUsoConsumo > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "25";
                model.Quadro = "04";
                model.Item = "030";
                model.Descricao = "Débito por diferencial de alíquota ref. uso e consumo";
                model.Valor = configModel.DiferencialAliquotaUsoConsumo;
                list.Add(model);
            }

            if (configModel.IcmsCiap > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "25";
                model.Quadro = "04";
                model.Item = "070";
                model.Descricao = "ICMS CIAP";
                model.Valor = configModel.IcmsCiap;
                list.Add(model);
            }

            if (list.Count > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "25";
                model.Quadro = "04";
                model.Item = "990";
                model.Descricao = "Subtotal de Débitos";
                model.Valor = list.Sum(l => l.Valor);
                list.Add(model);
            }

            return list;
        }
        #endregion

        #region GetDime26
        public static List<DimeSomatorioModel> GetQuadro05(List<DimeSomatorioModel> quadro03, DimeConfigModel configModel)
        {
            List<DimeSomatorioModel> list = new List<DimeSomatorioModel>();

            double saldoCredor = 0;
            try
            {
                string sql = $"EXEC SP_CVA_DIME_SALDO_CREDOR {configModel.Filial}, {configModel.DataDe.Month}, {configModel.DataDe.Year}";
                object saldoCredorObj = CrudController.ExecuteScalar(sql);
                if (saldoCredorObj != null)
                {
                    saldoCredor = (double)saldoCredorObj;
                }
            }
            catch { }

            DimeSomatorioModel model = new DimeSomatorioModel();
            if (saldoCredor > 0)
            {
                model.Tipo = "26";
                model.Quadro = "05";
                model.Item = "010";
                model.Descricao = "Saldo credor do mês anterior";
                model.Valor = saldoCredor;
                list.Add(model);
            }

            DimeSomatorioModel impostoCreditado = quadro03.FirstOrDefault(d => d.Item == "030");

            if (impostoCreditado != null && impostoCreditado.Valor > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "26";
                model.Quadro = "05";
                model.Item = "020";
                model.Descricao = "Crédito pelas entradas";
                model.Valor = impostoCreditado.Valor;
                list.Add(model);
            }
            if (configModel.CreditoAtivoPermanente > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "26";
                model.Quadro = "05";
                model.Item = "030";
                model.Descricao = "Crédito de ativo permanente";
                model.Valor = configModel.CreditoAtivoPermanente;
                list.Add(model);
            }

            //DimeSomatorioModel icmsST = quadro03.FirstOrDefault(d => d.Item == "104");

            if (configModel.CreditoIcmsST > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "26";
                model.Quadro = "05";
                model.Item = "050";
                model.Descricao = "Crédito de ICMS retido por substituição tributária";
                model.Valor = configModel.CreditoIcmsST;
                list.Add(model);
            }

            if (configModel.CreditoOutros > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "26";
                model.Quadro = "05";
                model.Item = "160";
                model.Descricao = "Outros Créditos";
                model.Valor = configModel.CreditoOutros;
                list.Add(model);
            }

            if (list.Count > 0)
            {
                model = new DimeSomatorioModel();
                model.Tipo = "26";
                model.Quadro = "05";
                model.Descricao = "Subtotal de créditos";
                model.Item = "990";
                model.Valor = list.Sum(l => l.Valor);
                list.Add(model);
            }
            return list;
        }
        #endregion

        #region GetQuadro09
        public static List<DimeSomatorioModel> GetQuadro09(List<DimeSomatorioModel> dime25, List<DimeSomatorioModel> dime26, string periodo, int filial)
        {
            string sql = String.Format(SQL.DimeQuadro09_Get, periodo, filial);
            List<DimeSomatorioModel> dime30 = new CrudController().FillModelListAccordingToSql<DimeSomatorioModel>(sql);
            foreach (var item in dime30)
            {
                if (item.Item.Length != 3)
                {
                    throw new Exception("Quadro 09 - Item deve possuir 3 caracteres");
                }
                switch (item.Item)
                {
                    case "060":
                        item.Descricao = "Saldos credores recebidos de estabelecimentos consolidados";
                        break;
                    case "075":
                        item.Descricao = "Créditos declarados no DCIP";
                        break;
                }
            }

            //DimeSomatorioModel model = new DimeSomatorioModel();
            //model.Tipo = "30";
            //model.Quadro = "09";
            //model.Item = "010";
            //model.Descricao = "Subtotal de débitos";
            //model.Valor = dime25.FirstOrDefault(d => d.Item == "990").Valor;
            //dime30.Add(model);

            //model = new DimeSomatorioModel();
            //model.Tipo = "30";
            //model.Quadro = "09";
            //model.Item = "050";
            //model.Descricao = "Subtotal de créditos";
            //model.Valor = dime26.FirstOrDefault(d => d.Item == "990").Valor;
            //dime30.Add(model);

            //model = new DimeSomatorioModel();
            //model.Tipo = "30";
            //model.Quadro = "09";
            //model.Item = "040";
            //model.Descricao = "Total de débitos";
            //model.Valor = dime26.Where(d => Convert.ToInt32(d.Item) <= 40).Sum(d => d.Valor);
            //dime30.Add(model);

            //model = new DimeSomatorioModel();
            //model.Tipo = "30";
            //model.Quadro = "09";
            //model.Item = "080";
            //model.Descricao = "Total de créditos";
            //model.Valor = dime26.Where(d => Convert.ToInt32(d.Item) > 40).Sum(d => d.Valor);
            //dime30.Add(model);

            return dime30.OrderBy(d => d.Item).ToList();
        }
        #endregion

        #region GetQuadro50
        public static List<DimeQuadro50Model> GetQuadro50(string dimeCode)
        {
            try
            {
                string sql = $" EXEC SP_CVA_DIME_50 '{dimeCode}' ";
                List<DimeQuadro50Model> dime50 = new CrudController().FillModelListAccordingToSql<DimeQuadro50Model>(sql);

                DimeQuadro50Model total50 = new DimeQuadro50Model();
                total50.UF = "TT";
                total50.Tipo = "50";
                total50.Quadro = "50";
                total50.BaseContribuinte = dime50.Sum(d => d.BaseContribuinte);
                total50.BaseNaoContribuinte = dime50.Sum(d => d.BaseNaoContribuinte);
                total50.IcmsST = dime50.Sum(d => d.IcmsST);
                total50.Outras = dime50.Sum(d => d.Outras);
                total50.ValorContribuinte = dime50.Sum(d => d.ValorContribuinte);
                total50.ValorNaoContribuinte = dime50.Sum(d => d.ValorNaoContribuinte);

                dime50.Add(total50);

                return dime50;
            }
            catch (Exception ex)
            {
                throw new Exception("DIME 50: " + ex.Message);
            }
        }
        #endregion

        #region GenerateFile
        private static string GenerateFile(string diretorio, DimeModel model)
        {
            StreamWriter sw = new StreamWriter(Path.Combine(diretorio, $"DIME_{DateTime.Now.ToString("ddMMyyyyy_HHmmss")}.txt"));
            try
            {
                FileWriterUtil fileWriterUtil = new FileWriterUtil();

                try
                {
                    sw.WriteLine(fileWriterUtil.WriteLine(model.ContadorModel));
                }
                catch (Exception ex)
                {
                    throw new Exception("Contador: " + ex.Message);
                }
                try
                {
                    sw.WriteLine(fileWriterUtil.WriteLine(model.EmpresaModel));
                }
                catch (Exception ex)
                {
                    throw new Exception("Empresa: " + ex.Message);
                }

                try
                {
                    foreach (var item in model.Quadro01)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 01: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro02)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 02: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro03)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 03: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro04)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 04: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro05)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 05: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro09)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 09: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro11)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 11: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro12)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 12: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro46)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 46: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro49)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 49: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro50)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 50: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro80)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 80: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro81)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 81: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro82)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 82: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro83)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 83: " + ex.Message);
                }
                try
                {
                    foreach (var item in model.Quadro84)
                    {
                        sw.WriteLine(fileWriterUtil.WriteLine(item));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 84: " + ex.Message);
                }

                try
                {
                    model.Dime98 = new DimeQuadro98Model();
                    model.Dime98.Quantidade = fileWriterUtil.WrittenLinesQuantity;
                    sw.WriteLine(fileWriterUtil.WriteLine(model.Dime98));
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 98: " + ex.Message);
                }
                try
                {
                    model.Dime99 = new DimeQuadro99Model();
                    model.Dime99.Quantidade = fileWriterUtil.WrittenLinesQuantity + 1;
                    sw.WriteLine(fileWriterUtil.WriteLine(model.Dime99));
                }
                catch (Exception ex)
                {
                    throw new Exception("Quadro 99: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                sw.Close();
            }
            return String.Empty;
        }
        #endregion

        #region GenerateExcel
        private static string GenerateExcel(string diretorio, DimeModel model, DimeConfigModel configModel)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Layout", "DIME.xlsx");
                var wb = new XLWorkbook(path);
                var ws = wb.Worksheet(1);

                int currentRow = 3;

                ws.Cell(currentRow, "B").Value = model.EmpresaModel.Inscricao;
                ws.Cell(currentRow, "D").Value = model.EmpresaModel.Nome;
                ws.Cell(currentRow, "G").Value = model.EmpresaModel.Periodo.Insert(2, "/");

                currentRow++;
                ws.Cell(currentRow, "B").Value = CrudController.ExecuteScalar(String.Format(SQL.UserField_GetDescription, "@CVA_DIME", "Declaracao", model.EmpresaModel.TipoDeclaracao));
                ws.Cell(currentRow, "D").Value = "2 - Normal";
                ws.Cell(currentRow, "G").Value = "1 - Não se Aplica";

                currentRow++;
                ws.Cell(currentRow, "B").Value = CrudController.ExecuteScalar(String.Format(SQL.UserField_GetDescription, "@CVA_DIME", "Apuracao", model.EmpresaModel.TipoApuracao));
                ws.Cell(currentRow, "D").Value = "1 - Não Enquadrada";
                ws.Cell(currentRow, "G").Value = CrudController.ExecuteScalar(String.Format(SQL.UserField_GetDescription, "@CVA_DIME", "Trans_Cred", model.EmpresaModel.TransCredito));

                currentRow++;
                ws.Cell(currentRow, "B").Value = "1 - Não se Aplica";
                ws.Cell(currentRow, "D").Value = "1 - Não se Aplica";
                ws.Cell(currentRow, "G").Value = CrudController.ExecuteScalar(String.Format(SQL.UserField_GetDescription, "@CVA_DIME", "Movimento", model.EmpresaModel.TipoMovimento));

                currentRow++;
                ws.Cell(currentRow, "B").Value = CrudController.ExecuteScalar(String.Format(SQL.UserField_GetDescription, "@CVA_DIME", "ST", model.EmpresaModel.SubstitutoTributario));
                ws.Cell(currentRow, "D").Value = CrudController.ExecuteScalar(String.Format(SQL.UserField_GetDescription, "@CVA_DIME", "Escrita", model.EmpresaModel.EscritaContabil));
                ws.Cell(currentRow, "G").Value = model.EmpresaModel.QtdeEmpregados;

                currentRow += 3;

                DimeDocumentModel dimeDocumentModel = new DimeDocumentModel();
                dimeDocumentModel.CFOP = "Total";
                dimeDocumentModel.ValorContabil = model.Quadro01.Sum(d => d.ValorContabil);
                dimeDocumentModel.BaseCalculo = model.Quadro01.Sum(d => d.BaseCalculo);
                dimeDocumentModel.ImpostoCreditado = model.Quadro01.Sum(d => d.ImpostoCreditado);
                dimeDocumentModel.Isentas = model.Quadro01.Sum(d => d.Isentas);
                dimeDocumentModel.Outras = model.Quadro01.Sum(d => d.Outras);
                dimeDocumentModel.BaseCalculoImpostoRetido = model.Quadro01.Sum(d => d.BaseCalculoImpostoRetido);
                dimeDocumentModel.ImpostoRetido = model.Quadro01.Sum(d => d.ImpostoRetido);
                //dimeDocumentModel.DiferencaAliquota = dimeModel.Dime22.Sum(d => d.DiferencaAliquota);
                model.Quadro01.Insert(model.Quadro01.Count, dimeDocumentModel);

                DataTable dime22 = model.Quadro01.ToDataTable();
                dime22.Columns.Remove("Tipo");
                dime22.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime22);

                currentRow += model.Quadro01.Count + 2;
                ws.Range($"A{currentRow}:G{currentRow}").Merge();
                ws.Cell(currentRow, "A").Style.Font.Bold = true;
                ws.Cell(currentRow, "A").Value = "02 - VALORES FISCAIS SAÍDAS";
                currentRow++;

                dimeDocumentModel = new DimeDocumentModel();
                dimeDocumentModel.CFOP = "Total";
                dimeDocumentModel.ValorContabil = model.Quadro02.Sum(d => d.ValorContabil);
                dimeDocumentModel.BaseCalculo = model.Quadro02.Sum(d => d.BaseCalculo);
                dimeDocumentModel.ImpostoCreditado = model.Quadro02.Sum(d => d.ImpostoCreditado);
                dimeDocumentModel.Isentas = model.Quadro02.Sum(d => d.Isentas);
                dimeDocumentModel.Outras = model.Quadro02.Sum(d => d.Outras);
                dimeDocumentModel.BaseCalculoImpostoRetido = model.Quadro02.Sum(d => d.BaseCalculoImpostoRetido);
                dimeDocumentModel.ImpostoRetido = model.Quadro02.Sum(d => d.ImpostoRetido);
                //dimeDocumentModel.DiferencaAliquota = dimeModel.Dime23.Sum(d => d.DiferencaAliquota);
                model.Quadro02.Insert(model.Quadro02.Count, dimeDocumentModel);

                DataTable dime23 = model.Quadro02.ToDataTable();
                dime23.Columns.Remove("Tipo");
                dime23.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime23);

                currentRow += model.Quadro02.Count + 2;
                ws.Range($"A{currentRow}:G{currentRow}").Merge();
                ws.Cell(currentRow, "A").Style.Font.Bold = true;
                ws.Cell(currentRow, "A").Value = "03 - RESUMO DOS VALORES FISCAIS";
                currentRow++;

                DataTable dime24 = model.Quadro03.ToDataTable();
                dime24.Columns.Remove("Tipo");
                dime24.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime24);

                currentRow += model.Quadro03.Count + 2;
                ws.Range($"A{currentRow}:G{currentRow}").Merge();
                ws.Cell(currentRow, "A").Style.Font.Bold = true;
                ws.Cell(currentRow, "A").Value = "04 - RESUMO DA APURAÇÃO DOS DÉBITOS";
                currentRow++;

                DataTable dime25 = model.Quadro04.ToDataTable();
                dime25.Columns.Remove("Tipo");
                dime25.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime25);

                currentRow += model.Quadro04.Count + 2;
                ws.Range($"A{currentRow}:G{currentRow}").Merge();
                ws.Cell(currentRow, "A").Style.Font.Bold = true;
                ws.Cell(currentRow, "A").Value = "05 - RESUMO DA APURAÇÃO DOS CRÉDITOS";
                currentRow++;

                DataTable dime26 = model.Quadro05.ToDataTable();
                dime26.Columns.Remove("Tipo");
                dime26.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime26);

                currentRow += model.Quadro05.Count + 2;
                ws.Range($"A{currentRow}:G{currentRow}").Merge();
                ws.Cell(currentRow, "A").Style.Font.Bold = true;
                ws.Cell(currentRow, "A").Value = "09 - CÁLCULO DO IMPOSTO A PAGAR OU SALDO CREDOR";
                currentRow++;

                DataTable dime30 = model.Quadro09.ToDataTable();
                dime30.Columns.Remove("Tipo");
                dime30.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime30);

                currentRow += model.Quadro09.Count + 2;
                ws.Range($"A{currentRow}:G{currentRow}").Merge();
                ws.Cell(currentRow, "A").Style.Font.Bold = true;
                ws.Cell(currentRow, "A").Value = "11 - INFORMAÇÕES SOBRE SUBSTITUIÇÃO TRIBUTÁRIA";
                currentRow++;

                DataTable dime32 = model.Quadro11.ToDataTable();
                dime32.Columns.Remove("Tipo");
                dime32.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime32);

                currentRow += model.Quadro11.Count + 2;
                ws.Range($"A{currentRow}:G{currentRow}").Merge();
                ws.Cell(currentRow, "A").Style.Font.Bold = true;
                ws.Cell(currentRow, "A").Value = "12 - DISCRIMINAÇÃO DOS PAGAMENTOS DO IMPOSTO E DOS DÉBITOS ESPECÍFICOS";
                currentRow++;

                DataTable dime33 = model.Quadro12.ToDataTable();
                dime33.Columns.Remove("Tipo");
                dime33.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime33);

                currentRow += model.Quadro12.Count + 2;
                ws.Range($"A{currentRow}:G{currentRow}").Merge();
                ws.Cell(currentRow, "A").Style.Font.Bold = true;
                ws.Cell(currentRow, "A").Value = "46 - CRÉDITOS POR AUTORIZAÇÕES ESPECIAIS";
                currentRow++;

                DataTable dime46 = model.Quadro46.ToDataTable();
                dime46.Columns.Remove("Tipo");
                dime46.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime46);

                currentRow += model.Quadro46.Count + 2;
                ws.Range($"A{currentRow}:G{currentRow}").Merge();
                ws.Cell(currentRow, "A").Style.Font.Bold = true;
                ws.Cell(currentRow, "A").Value = "49 - ENTRADAS POR UNIDADE DA FEDERAÇÃO";
                currentRow++;

                DataTable dime49 = model.Quadro49.ToDataTable();
                dime49.Columns.Remove("Tipo");
                dime49.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime49);

                currentRow += model.Quadro49.Count + 2;
                ws.Range($"A{currentRow}:G{currentRow}").Merge();
                ws.Cell(currentRow, "A").Style.Font.Bold = true;
                ws.Cell(currentRow, "A").Value = "50 - SAÍDAS POR UNIDADE DA FEDERAÇÃO";
                currentRow++;

                ws.Range($"B{currentRow}:C{currentRow}").Merge();
                ws.Cell(currentRow, "B").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(currentRow, "B").Style.Font.Bold = true;
                ws.Cell(currentRow, "B").Value = "Valor Contábil";

                ws.Range($"D{currentRow}:E{currentRow}").Merge();
                ws.Cell(currentRow, "D").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(currentRow, "D").Style.Font.Bold = true;
                ws.Cell(currentRow, "D").Value = "Base de cálculo";
                currentRow++;

                DataTable dime50 = model.Quadro50.ToDataTable();
                dime50.Columns.Remove("Tipo");
                dime50.Columns.Remove("Quadro");
                ws.Cell(currentRow, "A").InsertTable(dime50);
                currentRow += model.Quadro50.Count + 2;

                if (configModel.InfoEstoqueDespesa == 1)
                {
                    ws.Range($"A{currentRow}:G{currentRow}").Merge();
                    ws.Cell(currentRow, "A").Style.Font.Bold = true;
                    ws.Cell(currentRow, "A").Value = "80 - Resumo do Livro Registro de Inventário e Receita Bruta".ToUpper();
                    currentRow++;

                    DataTable dime80 = model.Quadro80.ToDataTable();
                    dime80.Columns.Remove("REG");
                    dime80.Columns.Remove("QUADRO");
                    ws.Cell(currentRow, "A").InsertTable(dime80);
                    currentRow += model.Quadro80.Count + 2;
                }
                if (configModel.DemonstrativosContabeis == 1)
                {
                    ws.Range($"A{currentRow}:G{currentRow}").Merge();
                    ws.Cell(currentRow, "A").Style.Font.Bold = true;
                    ws.Cell(currentRow, "A").Value = "81 - ATIVO";
                    currentRow++;

                    DataTable dime81 = model.Quadro81.ToDataTable();
                    dime81.Columns.Remove("REG");
                    dime81.Columns.Remove("QUADRO");
                    ws.Cell(currentRow, "A").InsertTable(dime81);
                    currentRow += model.Quadro81.Count + 2;

                    ws.Range($"A{currentRow}:G{currentRow}").Merge();
                    ws.Cell(currentRow, "A").Style.Font.Bold = true;
                    ws.Cell(currentRow, "A").Value = "82 - PASSIVO";
                    currentRow++;

                    DataTable dime82 = model.Quadro82.ToDataTable();
                    dime82.Columns.Remove("REG");
                    dime82.Columns.Remove("QUADRO");
                    ws.Cell(currentRow, "A").InsertTable(dime82);
                    currentRow += model.Quadro82.Count + 2;

                    ws.Range($"A{currentRow}:G{currentRow}").Merge();
                    ws.Cell(currentRow, "A").Style.Font.Bold = true;
                    ws.Cell(currentRow, "A").Value = "83 - DEMONSTRAÇÃO DE RESULTADOS";
                    currentRow++;

                    DataTable dime83 = model.Quadro83.ToDataTable();
                    dime83.Columns.Remove("REG");
                    dime83.Columns.Remove("QUADRO");
                    ws.Cell(currentRow, "A").InsertTable(dime83);
                    currentRow += model.Quadro83.Count + 2;
                }
                if (configModel.InfoEstoqueDespesa == 1)
                {
                    ws.Range($"A{currentRow}:G{currentRow}").Merge();
                    ws.Cell(currentRow, "A").Style.Font.Bold = true;
                    ws.Cell(currentRow, "A").Value = "84 - DETALHAMENTO DAS DESPESAS";
                    currentRow++;

                    DataTable dime84 = model.Quadro84.ToDataTable();
                    dime84.Columns.Remove("REG");
                    dime84.Columns.Remove("QUADRO");
                    ws.Cell(currentRow, "A").InsertTable(dime84);
                    currentRow += model.Quadro84.Count + 2;
                }

                string fileName = Path.Combine(diretorio, $"DIME_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx");
                wb.SaveAs(fileName);
                int retorno = SBOApp.Application.MessageBox("Deseja abrir o layout do arquivo gerado?", 1, "Sim", "Não");
                if (retorno == 1)
                {
                    System.Diagnostics.Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return String.Empty;
        }
        #endregion

        private static string GetSQLProc(DimeConfigModel configModel, string proc)
        {
            string sql = $@"DECLARE @DataDe DATETIME
                        DECLARE @DataAte DATETIME

                        SET @DataDe = CAST('{configModel.DataDe.ToString("yyyyMMdd")}' AS DATETIME)
                        SET @DataAte = CAST('{configModel.DataAte.ToString("yyyyMMdd")}' AS DATETIME)

                        EXEC {proc} @DataDe, @DataAte, {configModel.Filial}";
            return sql;
        }
    }
}
