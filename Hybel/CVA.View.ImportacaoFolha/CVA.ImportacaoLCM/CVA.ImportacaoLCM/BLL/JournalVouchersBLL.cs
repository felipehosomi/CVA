using CVA.ImportacaoLCM.Controller;
using CVA.ImportacaoLCM.DAO;
using CVA.ImportacaoLCM.Model;
using CVA.ImportacaoLCM.View;

using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.BLL
{
    class JournalVouchersBLL
    {
        //private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public int Criar(List<DetalheModel> LCM, string dtLancto, string dtVencto, int BplId, string comboBox)
        {
            var Log = new LogErro();

            SAPbobsCOM.JournalEntries PreLCM;

            var edDataLcto = FormatterHelper.StringToDate(dtLancto.ToString());
            var edDataVcto = FormatterHelper.StringToDate(dtVencto.ToString());
            int linha = 0;
            int err = 0;
            string errdsc = string.Empty;
            int cont = 0;
            double valor = 0;
            try
            {

                PreLCM = (SAPbobsCOM.JournalEntries)SBOApp.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);


                PreLCM.DueDate = edDataVcto.Date;
                PreLCM.ReferenceDate = edDataLcto.Date;
                PreLCM.TaxDate = edDataLcto.Date;
                PreLCM.Memo = "Apropriação da Folha " + edDataLcto.Month + "/" + edDataLcto.Year;

                foreach (var model in LCM)
                {


                    if (!string.IsNullOrWhiteSpace(model.ClassifDebito.Trim()))
                    {
                        var centroCusto = CentroCustoDAO.BuscaCentroCusto(model.CentroCustoDebit.TrimStart('0'));
                        var contaControle = ContaControleDAO.BuscaContaControle(model.ClassifDebito.Trim());
                        var cbCusto = comboBox;

                        if (contaControle.Count > 0)
                        {
                            //alteração Centro de custo
                            if (cbCusto == "0")
                            {
                                if (!string.IsNullOrWhiteSpace(centroCusto))
                                {
                                    if (!string.IsNullOrEmpty(contaControle.ToString()))
                                    {
                                        valor = Convert.ToDouble(model.ValorDoLancto) / 100;

                                        if (cont > 0)
                                            PreLCM.Lines.Add();

                                        PreLCM.Lines.DueDate = edDataLcto.Date;
                                        PreLCM.Lines.ReferenceDate1 = edDataLcto.Date;
                                        PreLCM.Lines.TaxDate = edDataLcto.Date;
                                        PreLCM.Lines.LineMemo = model.Historico.Substring(0, model.Historico.Length > 50 ? 50 : model.Historico.Length);
                                        if (contaControle.Count > 1)
                                        {
                                            PreLCM.Lines.ShortName = contaControle.First();
                                            PreLCM.Lines.ControlAccount = contaControle.Last();
                                        }
                                        else
                                        {
                                            PreLCM.Lines.ShortName = contaControle.First();
                                        }
                                        PreLCM.Lines.Debit = valor;
                                        PreLCM.Lines.CostingCode = centroCusto;
                                        PreLCM.Lines.BPLID = BplId;


                                        cont++;

                                        if (!string.IsNullOrWhiteSpace(model.ClassifCredito.Trim()))
                                        {
                                            contaControle = ContaControleDAO.BuscaContaControle(model.ClassifCredito.Trim());

                                            if (cont > 0)
                                                PreLCM.Lines.Add();

                                            PreLCM.Lines.DueDate = edDataLcto.Date;
                                            PreLCM.Lines.ReferenceDate1 = edDataLcto.Date;
                                            PreLCM.Lines.TaxDate = edDataLcto.Date;
                                            PreLCM.Lines.LineMemo = model.Historico.Substring(0, model.Historico.Length > 50 ? 50 : model.Historico.Length);
                                            if (contaControle.Count > 0)
                                            {
                                                if (contaControle.Count > 1)
                                                {
                                                    PreLCM.Lines.ShortName = contaControle.First();
                                                    PreLCM.Lines.ControlAccount = contaControle.Last();
                                                }
                                                else
                                                {
                                                    PreLCM.Lines.ShortName = contaControle.First();
                                                }
                                            }
                                            else
                                            {
                                                Log.Log("Linha: " + linha + "  sem Conta contábil SAP, Conta contábil domínio:" + model.ClassifCredito.Trim());
                                                linha++;
                                            }

                                            PreLCM.Lines.Credit = valor;
                                            PreLCM.Lines.CostingCode = centroCusto;
                                            PreLCM.Lines.BPLID = BplId;


                                            cont++;

                                        }

                                    }
                                    else
                                    {
                                        Log.Log("Linha: " + linha + "  Conta contábil SAP sem Parceiro: " + contaControle.First());
                                        linha++;
                                    }
                                }
                                else
                                {
                                    Log.Log("Linha: " + linha + "  sem centro de custo SAP, Centro de Custo Dominio: " + model.CentroCustoDebit.Trim());
                                    linha++;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrWhiteSpace(cbCusto))
                                {
                                    if (!string.IsNullOrEmpty(contaControle.ToString()))
                                    {
                                        valor = Convert.ToDouble(model.ValorDoLancto) / 100;

                                        if (cont > 0)
                                            PreLCM.Lines.Add();

                                        PreLCM.Lines.DueDate = edDataLcto.Date;
                                        PreLCM.Lines.ReferenceDate1 = edDataLcto.Date;
                                        PreLCM.Lines.TaxDate = edDataLcto.Date;
                                        PreLCM.Lines.LineMemo = model.Historico.Substring(0, model.Historico.Length > 50 ? 50 : model.Historico.Length);
                                        if (contaControle.Count > 1)
                                        {
                                            PreLCM.Lines.ShortName = contaControle.First();
                                            PreLCM.Lines.ControlAccount = contaControle.Last();
                                        }
                                        else
                                        {
                                            PreLCM.Lines.ShortName = contaControle.First();
                                        }
                                        PreLCM.Lines.Debit = valor;
                                        PreLCM.Lines.CostingCode = cbCusto;
                                        PreLCM.Lines.BPLID = BplId;


                                        cont++;

                                        if (!string.IsNullOrWhiteSpace(model.ClassifCredito.Trim()))
                                        {
                                            contaControle = ContaControleDAO.BuscaContaControle(model.ClassifCredito.Trim());

                                            if (cont > 0)
                                                PreLCM.Lines.Add();

                                            PreLCM.Lines.DueDate = edDataLcto.Date;
                                            PreLCM.Lines.ReferenceDate1 = edDataLcto.Date;
                                            PreLCM.Lines.TaxDate = edDataLcto.Date;
                                            PreLCM.Lines.LineMemo = model.Historico.Substring(0, model.Historico.Length > 50 ? 50 : model.Historico.Length);
                                            if (contaControle.Count > 0)
                                            {
                                                if (contaControle.Count > 1)
                                                {
                                                    PreLCM.Lines.ShortName = contaControle.First();
                                                    PreLCM.Lines.ControlAccount = contaControle.Last();
                                                }
                                                else
                                                {
                                                    PreLCM.Lines.ShortName = contaControle.First();
                                                }
                                            }
                                            else
                                            {
                                                Log.Log("Linha: " + linha + "  sem Conta contábil SAP, Conta contábil domínio: " + model.ClassifCredito.Trim());
                                                linha++;
                                            }

                                            PreLCM.Lines.Credit = valor;
                                            PreLCM.Lines.CostingCode = cbCusto;
                                            PreLCM.Lines.BPLID = BplId;


                                            cont++;

                                        }
                                    }
                                    else
                                    {
                                        Log.Log("Linha: " + linha + "  Conta contábil SAP sem Parceiro: " + contaControle.First());
                                        linha++;
                                    }

                                }
                                else
                                {
                                    Log.Log("Linha: " + linha + "  sem centro de custo SAP, Centro de Custo Dominio: " + model.CentroCustoDebit.Trim());
                                    linha++;
                                }

                            }

                        }
                        else
                        {
                            Log.Log("Linha: " + linha + "  sem Conta contábil SAP, Conta contábil domínio: " + model.ClassifDebito.Trim());
                            linha++;
                        }
                    }
                    else
                    {

                        if (!string.IsNullOrWhiteSpace(model.ClassifCredito.Trim()))
                        {
                            var contaControle = ContaControleDAO.BuscaContaControle(model.ClassifCredito.Trim());
                            string centroCusto = CentroCustoDAO.BuscaCentroCusto(model.CentroCustoDebit.TrimStart('0'));

                            var cbCusto = "";
                            cbCusto = comboBox;

                            if (contaControle.Count > 0)
                            {
                                var cc = contaControle.First();

                                if (!string.IsNullOrEmpty(cc))
                                {
                                    if (cbCusto == "0")
                                    {
                                        if (!string.IsNullOrWhiteSpace(centroCusto))
                                        {
                                            if (cont > 0)
                                                PreLCM.Lines.Add();

                                            valor = Convert.ToDouble(model.ValorDoLancto) / 100;

                                            PreLCM.Lines.DueDate = edDataLcto.Date;
                                            PreLCM.Lines.ReferenceDate1 = edDataLcto.Date;
                                            PreLCM.Lines.TaxDate = edDataLcto.Date;
                                            PreLCM.Lines.LineMemo = model.Historico.Substring(0, model.Historico.Length > 50 ? 50 : model.Historico.Length);
                                            if (contaControle.Count > 1)
                                            {
                                                PreLCM.Lines.ShortName = contaControle.First();
                                                PreLCM.Lines.ControlAccount = contaControle.Last();
                                            }
                                            else
                                            {
                                                PreLCM.Lines.ShortName = contaControle.First();
                                            }
                                            PreLCM.Lines.Credit = valor;
                                            PreLCM.Lines.CostingCode = centroCusto;
                                            PreLCM.Lines.BPLID = BplId;


                                            cont++;
                                        }
                                        else
                                        {
                                            Log.Log("Linha: " + linha + "  sem centro de custo SAP, Centro de Custo Dominio: " + model.CentroCustoCredit.Trim());
                                            linha++;
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(cbCusto))
                                        {
                                            if (cont > 0)
                                                PreLCM.Lines.Add();

                                            valor = Convert.ToDouble(model.ValorDoLancto) / 100;

                                            PreLCM.Lines.DueDate = edDataLcto.Date;
                                            PreLCM.Lines.ReferenceDate1 = edDataLcto.Date;
                                            PreLCM.Lines.TaxDate = edDataLcto.Date;
                                            PreLCM.Lines.LineMemo = model.Historico.Substring(0, model.Historico.Length > 50 ? 50 : model.Historico.Length);
                                            if (contaControle.Count > 1)
                                            {
                                                PreLCM.Lines.ShortName = contaControle.First();
                                                PreLCM.Lines.ControlAccount = contaControle.Last();
                                            }
                                            else
                                            {
                                                PreLCM.Lines.ShortName = contaControle.First();
                                            }
                                            PreLCM.Lines.Credit = valor;
                                            PreLCM.Lines.CostingCode = cbCusto;
                                            PreLCM.Lines.BPLID = BplId;


                                            cont++;
                                        }
                                        else
                                        {
                                            Log.Log("Linha: " + linha + "  sem centro de custo SAP, Centro de Custo Dominio: " + model.CentroCustoCredit.Trim());
                                            linha++;
                                        }
                                    }
                                }
                                else
                                {
                                    Log.Log("Linha: " + linha + "  Conta contábil SAP sem Parceiro: " + contaControle.Last());
                                    linha++;                                   
                                }
                            }
                            else
                            {
                                Log.Log("Linha: " + linha + "  sem Conta contábil SAP, Conta contábil domínio: " + model.ClassifCredito.Trim());
                                linha++;
                            }
                        }
                    }
                }

                if (linha == 0)
                {
                    PreLCM.Add();

                    SBOApp.oCompany.GetLastError(out err, out errdsc);

                    if (err != 0)
                    {
                        Log.Log("Erro ao adicionar LCM: " + err.ToString() + "Data Log: " + DateTime.Now + " Status: " + errdsc);
                        return err;
                    }
                }
                else
                {
                    err = 1;
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(PreLCM);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                return err;
            }
            catch (Exception e)
            {
                //Log.Log("Erro Fatal: " + e.Message + "Data Log: " + DateTime.Now);
                return 1;
            }
        }
    }
}
