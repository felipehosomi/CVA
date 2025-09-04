using Flurl.Http;
using PackIndicator.Models;
using SAPbobsCOM;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SAPbouiCOM;
using System.Runtime.InteropServices;
using System.Linq;
using PackIndicator.DAO;

namespace PackIndicator.Controllers
{
    class PackageController
    {
        private string _token = "910369D37BF39A5F88A4D96E9FD0A8D8D642BE5BB97335E80471F6E84C7C72337FA96F5B9527610E43FAA584796DC0816F6E864F747EE73015530B7CDAF484C";

        public async Task<List<Packages>> GetPackages(string itemCode)
        {
            var recordset = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                var query = @"SELECT * FROM ""@CVA_PACK_CONFIG""";
                recordset.DoQuery(query);

                if (recordset.RecordCount == 0)
                {
                    throw new System.Exception($"Configurações da API não encontradas (@CVA_PACK_CONFIG)");
                }
                else
                {
                    string url = recordset.Fields.Item("U_CVA_URL").Value.ToString();
                    string token = recordset.Fields.Item("U_CVA_TOKEN").Value.ToString();
                    if (!url.EndsWith("/"))
                    {
                        url += "/";
                    }

                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText($"{url}{token},{itemCode}.", BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_None);
                    List<Packages> list = await $"{url}{token},{itemCode}.".GetJsonAsync<List<Packages>>();
                    // Buscando fator de conversão do SAP, pois houve casos em que o preço unitário foi calculado errado, talvez seja o motivo
                    if (list != null)
                    {
                        foreach (var itemByUom in list.GroupBy(m => m.Codigocliente.Split('.').Last()))
                        {
                            recordset.DoQuery(String.Format(Hana.UomFactor_Get, itemByUom.Key));
                            double factor = (double)recordset.Fields.Item(0).Value;
                            if (factor == 0)
                            {
                                throw new Exception($"Fator de conversão não encontrado para a UM {itemByUom.Key}");
                            }

                            foreach (var item in itemByUom)
                            {
                                item.Fatorconversao = factor;
                            }
                        }
                    }
                    return list;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                Marshal.ReleaseComObject(recordset);
                recordset = null;
            }
        }

        public static int GetUomEntry(string itemCode, string uomCode)
        {
            var recordset = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                var query = $@"select OUOM.""UomEntry"" 
                             from OUOM 
                            where OUOM.""UomEntry"" = substring('{itemCode}', locate('{itemCode}', '.', -1) + 1, length('{itemCode}'))";
                recordset.DoQuery(query);

                if (recordset.RecordCount == 0)
                {
                    throw new System.Exception($"Unidade de medida {uomCode} não cadastrada no SAP Business One.");
                }
                else
                {
                    return int.Parse(recordset.Fields.Item("UomEntry").Value.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.ReleaseComObject(recordset);
            }
        }

        public static int GetUomEntry(string uomCode)
        {
            var recordset = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                var query = $@"select OUOM.""UomEntry"" 
                             from OUOM 
                            where OUOM.""UomCode"" = '{uomCode}'";
                recordset.DoQuery(query);

                if (recordset.RecordCount == 0)
                {
                    throw new System.Exception($"Unidade de medida {uomCode} não cadastrada no SAP Business One.");
                }
                else
                {
                    return int.Parse(recordset.Fields.Item("UomEntry").Value.ToString());
                }
            }
            finally
            {
                Marshal.ReleaseComObject(recordset);
                recordset = null;
            }
        }

        public static double SetReservedQuantity(string itemCode, DateTime dueDate)
        {
            var recordset = (Recordset)CommonController.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                var query = $@"select sum(RDR1.""Quantity"") as ""ReserveQty""
                             from RDR1
                            where RDR1.""PickStatus"" = 'R'
                              and RDR1.""ItemCode"" = left('{itemCode}', locate('{itemCode}', '.', -1) - 1)
                              and RDR1.""UomEntry"" = substring('{itemCode}', locate('{itemCode}', '.', -1) + 1, length('{itemCode}'))
                              and RDR1.""U_CVA_DueDate"" = '{dueDate.ToString("yyyyMMdd")}'";
                recordset.DoQuery(query);


                if (recordset.RecordCount == 0)
                {
                    return 0.0;
                }
                else
                {
                    return double.Parse(recordset.Fields.Item("ReserveQty").Value.ToString());
                }
            }
            finally
            {
                Marshal.ReleaseComObject(recordset);
                recordset = null;
            }
        }
    }
}
