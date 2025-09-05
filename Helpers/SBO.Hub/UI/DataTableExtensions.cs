using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM;
using System.Reflection;
using System.Runtime.InteropServices;
using SBO.Hub.Attributes;

namespace SBO.Hub.UI
{
    public static class DataTableExtensions
    {
        public static DataTable FillTableFromModel<T>(this DataTable table, List<T> modelList)
        {
            table.Rows.Add(modelList.Count);

            Type modelType = typeof(T);
            // Seta os valores no model
            foreach (PropertyInfo property in modelType.GetProperties())
            {
                // Busca os Custom Attributes
                foreach (Attribute attribute in property.GetCustomAttributes(true))
                {
                    int i = 0;
                    foreach (var item in modelList)
                    {
                        HubModelAttribute hubModel = attribute as HubModelAttribute;
                        if (hubModel != null)
                        {
                            if (!String.IsNullOrEmpty(hubModel.UIFieldName))
                            {
                                table.SetValue(hubModel.UIFieldName, i, property.GetValue(item, null));
                            }
                        }
                        i++;
                    }
                }
            }
            return table;
        }

        public static List<T> FillModelFromTable<T>(this DataTable table)
        {
            return FillModelFromTable<T>(table, false);
        }

        public static List<T> FillModelFromProperties<T>(this DataTable table)
        {
            List<T> modelList = new List<T>();
            // Cria nova instância do model
            T model;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                model = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    try
                    {
                        property.SetValue(model, table.GetValue(property.Name, i), null);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Erro ao setar campo {property.Name}: {ex.Message}");
                    }
                }
                modelList.Add(model);
            }

            return modelList;
        }

        public static List<T> FillModelFromTableAccordingToValue<T>(this DataTable table, bool showProgressBar, string columnName, object okValue, bool showColumnError = true)
        {
            List<T> modelList = new List<T>();
            // Cria nova instância do model
            T model;

            ProgressBar pgb = null;
            if (showProgressBar)
            {
                pgb = SBOApp.Application.StatusBar.CreateProgressBar("Carregando dados da tela", table.Rows.Count, false);
            }
            HubModelAttribute hubModel;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (showProgressBar)
                {
                    pgb.Value++;
                }

                if (table.GetValue(columnName, i).ToString() != okValue.ToString())
                {
                    continue;
                }

                model = Activator.CreateInstance<T>();
                // Seta os valores no model
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    // Busca os Custom Attributes
                    foreach (Attribute attribute in property.GetCustomAttributes(true))
                    {
                        hubModel = attribute as HubModelAttribute;
                        if (hubModel != null)
                        {
                            if (String.IsNullOrEmpty(hubModel.UIFieldName))
                            {
                                hubModel.UIFieldName = hubModel.Description;
                            }

                            if (String.IsNullOrEmpty(hubModel.UIFieldName))
                            {
                                break;
                            }
                            else
                            {
                                try
                                {
                                    property.SetValue(model, table.GetValue(hubModel.UIFieldName, i), null);
                                }
                                catch
                                {
                                    try
                                    {
                                        if (property.PropertyType == typeof(string))
                                        {
                                            property.SetValue(model, table.GetValue(hubModel.UIFieldName, i).ToString(), null);
                                        }
                                        else if (property.PropertyType == typeof(int))
                                        {
                                            property.SetValue(model, Convert.ToInt32(table.GetValue(hubModel.UIFieldName, i).ToString()), null);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (showColumnError)
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                modelList.Add(model);
            }
            if (pgb != null)
            {
                pgb.Stop();
                Marshal.ReleaseComObject(pgb);
                pgb = null;
            }

            return modelList;
        }

        public static List<T> FillModelFromTable<T>(this DataTable table, bool showProgressBar)
        {
            List<T> modelList = new List<T>();
            // Cria nova instância do model
            T model;

            ProgressBar pgb = null;
            if (showProgressBar)
            {
                pgb = SBOApp.Application.StatusBar.CreateProgressBar("Carregando dados da tela", table.Rows.Count, false);
            }
            HubModelAttribute hubModel;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (showProgressBar)
                {
                    pgb.Value++;
                }
                model = Activator.CreateInstance<T>();
                // Seta os valores no model
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    // Busca os Custom Attributes
                    foreach (Attribute attribute in property.GetCustomAttributes(true))
                    {
                        hubModel = attribute as HubModelAttribute;
                        if (hubModel != null)
                        {
                            if (String.IsNullOrEmpty(hubModel.UIFieldName))
                            {
                                hubModel.UIFieldName = hubModel.Description;
                            }

                            if (String.IsNullOrEmpty(hubModel.UIFieldName))
                            {
                                break;
                            }
                            else
                            {
                                property.SetValue(model, table.Columns.Item(hubModel.UIFieldName).Cells.Item(i).Value, null);
                            }
                        }
                    }
                }
                modelList.Add(model);
            }
            if (pgb != null)
            {
                pgb.Stop();
                Marshal.ReleaseComObject(pgb);
                pgb = null;
            }

            return modelList;
        }

        /// <summary>
        /// Preenche lista de acordo com valor em determinada coluna
        /// </summary>
        /// <param name="columnName">Nome da coluna que irá retornar na lista</param>
        /// <param name="clmToCheck">Nome da coluna para verificar o valor</param>
        /// <param name="valueToCheck">Valor a ser verificado</param>
        /// <param name="table">Tabela</param>
        /// <returns></returns>
        public static List<string> FillListAccordingToValue(this DataTable table, string columnName, string clmToCheck, string valueToCheck)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.GetValue(clmToCheck, i).ToString() == valueToCheck)
                {
                    list.Add(table.GetValue(columnName, i).ToString());
                }
            }

            return list;
        }
    }
}
