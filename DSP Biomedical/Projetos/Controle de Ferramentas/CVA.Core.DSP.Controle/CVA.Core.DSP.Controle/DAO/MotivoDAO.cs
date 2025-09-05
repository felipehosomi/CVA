using CVA.Core.DSP.Controle.BLL;
using CVA.Core.DSP.Controle.HELPER;
using CVA.Core.DSP.Controle.VIEW;
using Dover.Framework.DAO;
using SAPbobsCOM;
using System;


namespace CVA.Core.DSP.Controle.DAO
{
    
    public  class MotivoDAO
    {
        public MotivoBLL _motivoBLL { get; set; }
        public MotivoDAO _motivoDAO { get; set; }
        public MotivoView _motivoView { get; set; }
        public SAPbouiCOM.Application _application { get; set; }
        public SAPbobsCOM.Company _company { get; set; }


        public MotivoDAO()
        {


        }

        //public bool Insert(string Code, string Name)
        //{
        //    try
        //    {               

        //        var table = _company.UserTables.Item("CVA_MOTIVO");

        //        table.Code = Code;
        //        table.Name = Name;
        //        table.UserFields.Fields.Item("U_NOME").Value = Code;
        //        table.UserFields.Fields.Item("U_DESCRICAO").Value = Name;

        //        if (table.Add() != 0)
        //        {
        //            int ErrorCode;
        //            string errorMsg;

        //            _company.GetLastError(out ErrorCode, out errorMsg);

        //            _application.SetStatusBarMessage("Cadastro não efetuado " + ErrorCode + " " + errorMsg, SAPbouiCOM.BoMessageTime.bmt_Short);
        //            BusinessOneLog.Add("Erro ao adicionar motivo : " + errorMsg, true);
        //            return false;
        //        }

        //        _application.SetStatusBarMessage("Cadastrado com sucesso.", SAPbouiCOM.BoMessageTime.bmt_Short, false);

        //        return true;

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
    }
