using System;
using System.Xml;
using SAPbouiCOM;
using DAL.Connection;

namespace DAL.UserInterface
{
    public class FormsDao
    {
        public static Form LoadForm(string formPath, string formId)
        {
            var oXmlDoc = new XmlDocument();
            var oCreationPackage = (FormCreationParams)ConnectionDao.Instance.Application.CreateObject(BoCreatableObjectType.cot_FormCreationParams);

            oCreationPackage.UniqueID = formId;

            oXmlDoc.Load(formPath);
            oCreationPackage.XmlData = oXmlDoc.InnerXml;
            return ConnectionDao.Instance.Application.Forms.AddEx(oCreationPackage);
        }
    }
}
