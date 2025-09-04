using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.ImportadorFolha.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ImportadorFolha.DAO
{
    public class LancamentoContabilDAO
    {
        public static string Gerar(FolhaPagamentoModel model, ref string lcmId)
        {
            string msg = String.Empty;

            JournalEntries lcm = (JournalEntries)SBOApp.Company.GetBusinessObject(BoObjectTypes.oJournalEntries);
            int i = 0;

            try
            {
                lcm.ReferenceDate = model.DocDate;
                lcm.TaxDate = model.DocDate;
                lcm.DueDate = model.DueDate;

                foreach (var line in model.Lines)
                {
                    i++;
                    lcm.Lines.BPLID = model.BPlId;
                    object[] param = new object[1];
                    
                    foreach (var field in line.Items)
                    {
                        if (String.IsNullOrEmpty(field.ValorPara))
                        {
                            continue;
                        }

                        switch ((FieldTypeEnum)field.TipoCampoLCM)
                        {
                            case FieldTypeEnum.AlphaNumeric:
                                param[0] = field.ValorPara;
                                break;
                            case FieldTypeEnum.Integer:
                                param[0] = Convert.ToInt32(field.ValorPara);
                                break;
                            case FieldTypeEnum.Decimal:
                                param[0] = Convert.ToDouble(field.ValorPara.Replace(".", ","));
                                break;
                            default:
                                break;
                        }

                        typeof(JournalEntries_Lines).InvokeMember(field.CampoLCM, System.Reflection.BindingFlags.SetProperty, null, lcm.Lines, param);
                    }

                    if (i < model.Lines.Count)
                    {
                        lcm.Lines.Add();
                    }
                }

                if (lcm.Add() != 0)
                {
                    msg = SBOApp.Company.GetLastErrorDescription();
                }
                else
                {
                    lcmId = SBOApp.Company.GetNewObjectKey();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(lcm);
                lcm = null;
            }
            return msg;
        }
    }
}
