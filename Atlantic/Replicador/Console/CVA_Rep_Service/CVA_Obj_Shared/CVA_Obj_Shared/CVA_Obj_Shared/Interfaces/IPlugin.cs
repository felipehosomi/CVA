using System;
using System.Collections.Generic;
using SAPbobsCOM;

namespace CVA_Obj_Shared.Interfaces
{
    public abstract class IPlugin : IDisposable
    {
        public virtual string Name { get; set; }
        public abstract void Dispose();

        public abstract void Close();
        protected abstract void Dispose(bool disposing);

        public abstract int Create(Dictionary<int, string> listaRegistros, Company oCompany);
        public abstract int Update(Dictionary<int, string> listaRegistros, Company oCompany);
        public abstract int Delete(Dictionary<int, string> listaRegistros, Company oCompany);

        public virtual int Create(Dictionary<int, CentroCustoService> listaRegistros, Company oCompany)
        {
            return 1;
        }

        public virtual int Update(Dictionary<int, CentroCustoService> listaRegistros, Company oCompany)
        {
            return 1;
        }

        public virtual int Delete(Dictionary<int, CentroCustoService> listaRegistros, Company oCompany)
        {
            return 1;
        }

        public virtual int Create(Dictionary<int, string> listaCodigosImposto, Dictionary<int, string> listaTiposImposto,
            Dictionary<string, string> listaAliquotasImposto, Company oCompany)
        {
            return 1;
        }

        public virtual int Update(Dictionary<int, string> listaCodigosImposto, Dictionary<int, string> listaTiposImposto,
            Dictionary<string, string> listaAliquotasImposto, Company oCompany)
        {
            return 1;
        }

        public virtual int Delete(Dictionary<int, string> listaCodigosImposto, Dictionary<int, string> listaTiposImposto,
            Dictionary<string, string> listaAliquotasImposto, Company oCompany)
        {
            return 1;
        }
        
        public virtual int Create(Dictionary<int, UsuarioService> listaRegistros, Company oCompany)
        {
            return 1;
        }
        
        public virtual int Update(Dictionary<int, UsuarioService> listaRegistros, Company oCompany)
        {
            return 1;
        }
        
        public virtual int Delete(Dictionary<int, UsuarioService> listaRegistros, Company oCompany)
        {
            return 1;
        }

        public virtual int Create(Dictionary<int, UtilizacaoService> listaRegistros, Company oCompany)
        {
            return 1;
        }

        public virtual int Update(Dictionary<int, UtilizacaoService> listaRegistros, Company oCompany)
        {
            return 1;
        }

        public virtual int Delete(Dictionary<int, UtilizacaoService> listaRegistros, Company oCompany)
        {
            return 1;
        }
    }

    public class CentroCustoService
    {
        public BoYesNoEnum Active;
        public string CostCenterType;
        public DateTime EffectiveFrom;
        public DateTime EffectiveTo;
        public string FactorCode;
        public string FactorDescription;
        public string GroupCode;
        public int InWhichDimension;
    }

    public class UsuarioService
    {
        public int Branch { get; set; }
        public int CashLimit { get; set; }
        public string Defaults { get; set; }
        public int Department { get; set; }
        public string eMail { get; set; }
        public string FaxNumber { get; set; }
        public int Locked { get; set; }
        public double MaxCashAmtForIncmngPayts { get; set; }
        public double MaxDiscountGeneral { get; set; }
        public double MaxDiscountPurchase { get; set; }
        public double MaxDiscountSales { get; set; }
        public string MobilePhoneNumber { get; set; }
        public int Superuser { get; set; }
        public List<FiliaisUsuarioService> UserBranchAssignment { get; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public List<PermissoesUsuarioService> UserPermission { get; }

        public UsuarioService()
        {
            UserPermission = new List<PermissoesUsuarioService>();
            UserBranchAssignment = new List<FiliaisUsuarioService>();
        }
    }

    public class PermissoesUsuarioService
    {
        public int Permission { get; set; }
        public string PermissionID { get; set; }
    }

    public class FiliaisUsuarioService
    {
        public int BPLID { get; set; }
    }

    public class UtilizacaoService
    {
        public string Description { get; set; }
        public string IncomingImportCFOPCode { get; set; }
        public string IncomingInStateCFOPCode { get; set; }
        public string IncomingOutStateCFOPCode { get; set; }
        public string OutgoingExportCFOPCode { get; set; }
        public string OutgoingInStateCFOPCode { get; set; }
        public string OutgoingOutStateCFOPCode { get; set; }
        public int ThirdParty { get; set; }
        public string Usage { get; set; }
        public List<CamposUsuarioService> CamposUsuario { get; set; }

        public UtilizacaoService()
        {
            CamposUsuario = new List<CamposUsuarioService>();
        }
    }

    public class CamposUsuarioService
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}