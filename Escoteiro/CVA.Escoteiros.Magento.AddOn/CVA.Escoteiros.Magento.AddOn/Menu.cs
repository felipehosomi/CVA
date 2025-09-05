using CVA.Escoteiros.Magento.AddOn.Views;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CVA.Escoteiros.Magento.AddOn
{
    class Menu
    {
        public void AddMenuItems(string fileName)
        {
            var menus = Application.SBO_Application.Menus;
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            try
            {
                // Obtém todos os menus do tipo pop-up (Type = 2) para removê-los, caso já existam.
                var nodes = xmlDocument.SelectNodes("/Application/Menus/action/Menu[@Type='2']");

                // Se foi obtido algum menu do tipo pop-up...
                if (nodes != null)
                {
                    // Verifica se o menu já existe no sistema.
                    foreach (var node in nodes.Cast<XmlNode>().Where(node => menus.Exists(node.Attributes["UniqueID"].Value)))
                    {
                        // Caso exista, remove-o.
                        menus.RemoveEx(node.Attributes["UniqueID"].Value);
                    }
                }

                // Obtém todos os menus do tipo pop-up em que foi definido o atributo Image.
                foreach (var imageAttr in from XmlNode node in nodes select node.Attributes.Cast<XmlAttribute>().FirstOrDefault(a => a.Name == "Image"))
                {
                    // Concatena no nome do arquivo a sua localização.
                    imageAttr.Value = String.Format(imageAttr.Value, System.Windows.Forms.Application.StartupPath + @"\img");
                }

                // Realiza o carregamento do menu no sistema.
                var tmpStr = xmlDocument.InnerXml;
                Application.SBO_Application.LoadBatchActions(ref tmpStr);
                Application.SBO_Application.GetLastBatchResults();
            }
            catch (Exception er)
            {
                Application.SBO_Application.SetStatusBarMessage(String.Format("Exceção ao gerar o menu do add-on: {0}", er.Message), SAPbouiCOM.BoMessageTime.bmt_Short);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "BatchItemCategoryUpdater")
                {
                    var activeForm = new BatchItemCategoryUpdater();
                    activeForm.Show();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

    }
}
