using CVA.AddOn.Common;
using CVA.AddOn.Control.Logic.MODEL;
using SAPbobsCOM;
using System;

namespace CVA.AddOn.Control.Logic.DAO.OACT
{
    public class AccountDAO
    {
        public string UpdateStatus(AccountModel model)
        {
            ChartOfAccounts oChartOfAccounts = (ChartOfAccounts)SBOApp.Company.GetBusinessObject(BoObjectTypes.oChartOfAccounts);
            oChartOfAccounts.GetByKey(model.AcctCode);

            oChartOfAccounts.ValidFor = model.ValidFor ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
            if (model.ValidFrom.HasValue)
            {
                oChartOfAccounts.ValidFrom = model.ValidFrom.Value;
            }
            if (model.ValidTo.HasValue)
            {
                oChartOfAccounts.ValidTo = model.ValidTo.Value;
            }
            oChartOfAccounts.ValidRemarks = model.ValidRemarks;

            oChartOfAccounts.FrozenFor = model.FrozenFor ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
            if (model.FrozenFrom.HasValue)
            {
                oChartOfAccounts.FrozenFrom = model.FrozenFrom.Value;
            }
            if (model.FrozenTo.HasValue)
            {
                oChartOfAccounts.FrozenTo = model.FrozenTo.Value;
            }
            oChartOfAccounts.FrozenRemarks = model.FrozenRemarks;

            if (oChartOfAccounts.Update() != 0)
            {
                return SBOApp.Company.GetLastErrorDescription();
            }

            return String.Empty;
        }
    }
}
