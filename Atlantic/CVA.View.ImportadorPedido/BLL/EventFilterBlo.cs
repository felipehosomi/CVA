using DAL.UserInterface;
using SAPbouiCOM;

namespace BLL
{
    public class EventFilterBlo
    {
        public static void SetFilters()
        {
            FiltersDao.Add("IMPPED", BoEventTypes.et_MENU_CLICK);
            FiltersDao.Add("IMPNF", BoEventTypes.et_MENU_CLICK);
            FiltersDao.Add("CANCELDOC", BoEventTypes.et_MENU_CLICK);

            FiltersDao.Add("2000010001", BoEventTypes.et_CLICK);
            FiltersDao.Add("2000010002", BoEventTypes.et_CLICK);
            FiltersDao.Add("2000010002", BoEventTypes.et_DOUBLE_CLICK);
            FiltersDao.Add("2000010003", BoEventTypes.et_CLICK);
            FiltersDao.Add("2000010003", BoEventTypes.et_FORM_LOAD);
        }
    }
}
