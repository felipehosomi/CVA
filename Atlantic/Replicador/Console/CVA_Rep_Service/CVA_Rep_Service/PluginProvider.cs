using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CVA_Obj_Shared.Interfaces;
using SAPbobsCOM;
namespace CVA_Rep_Service
{
    public class PluginProvider
    {
        private List<IPlugin> pluginInstances;

        public void Initialize()
        {
            pluginInstances = (from file in
                Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Plugins\"), "*.dll")
                from type in Assembly.LoadFrom(file).GetTypes()
                where type.BaseType == typeof (IPlugin)
                select (IPlugin) Activator.CreateInstance(type)).ToList();
        }

        public void Create(string pluginName, Dictionary<int, string> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Create(listaRegistros, oCompany);
        }

        public void Create(string pluginName, Dictionary<int, CentroCustoService> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Create(listaRegistros, oCompany);
        }

        public void Create(string pluginName, Dictionary<int, string> listaCodigosImposto,
            Dictionary<int, string> listaTiposImposto, Dictionary<string, string> listaAliquotasImposto,
            Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Create(listaCodigosImposto, listaTiposImposto, listaAliquotasImposto, oCompany);
        }

        public void Update(string pluginName, Dictionary<int, string> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Update(listaRegistros, oCompany);
        }

        public void Update(string pluginName, Dictionary<int, CentroCustoService> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Update(listaRegistros, oCompany);
        }

        public void Update(string pluginName, Dictionary<int, string> listaCodigosImposto,
            Dictionary<int, string> listaTiposImposto, Dictionary<string, string> listaAliquotasImposto,
            Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Update(listaCodigosImposto, listaTiposImposto, listaAliquotasImposto, oCompany);
        }

        public void Delete(string pluginName, Dictionary<int, string> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Delete(listaRegistros, oCompany);
        }

        public void Delete(string pluginName, Dictionary<int, CentroCustoService> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Delete(listaRegistros, oCompany);
        }

        public void Delete(string pluginName, Dictionary<int, string> listaCodigosImposto,
            Dictionary<int, string> listaTiposImposto, Dictionary<string, string> listaAliquotasImposto,
            Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Delete(listaCodigosImposto, listaTiposImposto, listaAliquotasImposto, oCompany);
        }

        public void Create(string pluginName, Dictionary<int, UsuarioService> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Create(listaRegistros, oCompany);
        }

        public void Update(string pluginName, Dictionary<int, UsuarioService> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Update(listaRegistros, oCompany);
        }

        public void Delete(string pluginName, Dictionary<int, UsuarioService> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Delete(listaRegistros, oCompany);
        }

        public void Create(string pluginName, Dictionary<int, UtilizacaoService> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Create(listaRegistros, oCompany);
        }

        public void Update(string pluginName, Dictionary<int, UtilizacaoService> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Update(listaRegistros, oCompany);
        }

        public void Delete(string pluginName, Dictionary<int, UtilizacaoService> listaRegistros, Company oCompany)
        {
            if (pluginInstances != null)
                foreach (
                    var instance in pluginInstances.ToArray().Where(instance => instance.Name == pluginName).ToArray())
                    instance.Delete(listaRegistros, oCompany);
        }
    }
}