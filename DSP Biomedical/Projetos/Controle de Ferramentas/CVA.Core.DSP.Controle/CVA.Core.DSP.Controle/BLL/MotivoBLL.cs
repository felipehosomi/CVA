using CVA.Core.DSP.Controle.DAO;
using CVA.Core.DSP.Controle.HELPER;
using CVA.Core.DSP.Controle.VIEW;
using Dover.Framework.DAO;
using SAPbobsCOM;
using System;

namespace CVA.Core.DSP.Controle.BLL
{
    public class MotivoBLL
    {
        #region Atributos

        private BusinessOneDAO _businessOneDAO { get; set; }
        public SAPbouiCOM.Application _application { get; set; }
        public Company _company { get; set; }
        
        public MotivoDAO _MotivoDAO { get; set; }     
        public MotivoView _motivoView { get; set; }

        #endregion

        #region Construtor

        public MotivoBLL()
        {
            

        }
#endregion

        //private bool ValidateFields(string Code, string Name)
        //{

        //    if (!string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(Name))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

        //public bool Cadastrar(string Code, string Name)
        //{
        //    var IsValid = ValidateFields(Code, Name);

        //    if (IsValid)
        //    {
               
        //        _MotivoDAO.Insert(Code, Name);               
        //        return true;
        //    }
        //    else
        //    {
        //        _Application.MessageBox("Existe campos a serem preenchidos! ");
        //        return false;

        //    }

        //}

    }
}
