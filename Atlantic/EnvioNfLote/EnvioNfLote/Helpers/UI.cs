using SAPbouiCOM;
using System;
using System.Xml;

namespace EnvioNfLote.Helpers
{
    public class UI
    {
        public static void AddMenuItem(string parentId, string menuId, string description, int position, BoMenuType menuType, string imagePath = null)
        {
            var oCreationPackage = (MenuCreationParams)B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams);
            MenuItem oMenuItem = B1Connection.Instance.Application.Menus.Item(parentId);
            Menus oMenus = oMenuItem.SubMenus;
            bool exist = (oMenus != null) && oMenuItem.SubMenus.Exists(menuId);

            if (exist)
            {
                oMenuItem.SubMenus.RemoveEx(menuId);
                exist = false;
            }

            if (!exist)
            {
                oCreationPackage.Type = menuType;
                oCreationPackage.UniqueID = menuId;
                oCreationPackage.String = description;
                oCreationPackage.Enabled = true;
                oCreationPackage.Position = position;
                oCreationPackage.Image = imagePath;

                if (oMenus == null)
                {
                    oMenuItem.SubMenus.Add(menuId, description, menuType, position);
                    oMenus = oMenuItem.SubMenus;
                }

                oMenus.AddEx(oCreationPackage);
            }
        }

        public static Form LoadForm(string formPath)
        {
            var oXmlDoc = new XmlDocument();
            var oCreationPackage = (FormCreationParams)B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_FormCreationParams);

            oCreationPackage.UniqueID = string.Format("{0}{1}", oCreationPackage.UniqueID,
                                                    Guid.NewGuid().ToString().Substring(2, 10));

            oXmlDoc.Load(formPath);

            oCreationPackage.XmlData = oXmlDoc.InnerXml;

            return B1Connection.Instance.Application.Forms.AddEx(oCreationPackage);
        }

        public static void AddFilter(ref EventFilters oFilters, string containerUID, BoEventTypes eventType)
        {
            try
            {
                int pos;
                if (!FilterExists(ref oFilters, eventType, out pos))
                {
                    var oFilter = oFilters.Add(eventType);
                    oFilter.AddEx(containerUID);
                    B1Connection.Instance.Application.SetFilter(oFilters);
                }
                else
                {
                    oFilters.Item(pos).AddEx(containerUID);
                    B1Connection.Instance.Application.SetFilter(oFilters);
                }
            }
            catch (Exception ex)
            {
                B1Connection.Instance.Application.StatusBar.SetText(ex.Message);
            }
        }

        public static void MontaConfiguracaoEmailForm(ref Form oForm)
        {
            UserDataSource dsEmpresa;
            UserDataSource dsSsl;
            UserDataSource dsEmail;
            UserDataSource dsUsuario;
            UserDataSource dsSenha;
            UserDataSource dsSmtp;
            UserDataSource dsPorta;
            UserDataSource dsEmailCopia;
            UserDataSource dsMsg;
            UserDataSource dsSubject;

            try
            {
                dsEmpresa = oForm.DataSources.UserDataSources.Add("dsComp", BoDataType.dt_SHORT_NUMBER, 10);
                ComboBox oComboBox = (ComboBox)oForm.Items.Item("cbComp").Specific;

                oComboBox.DataBind.SetBound(true, "", "dsComp");

                foreach (var item in DI.BuscaEmpresas())
                {
                    oComboBox.ValidValues.Add(item.Key.ToString(), item.Value);                    
                }

                oForm.Items.Item("cbComp").DisplayDesc = true;
            }
            catch
            {
                dsEmpresa = oForm.DataSources.UserDataSources.Item("dsComp");
                ComboBox oComboBox = (ComboBox)oForm.Items.Item("cbComp").Specific;

                oComboBox.DataBind.SetBound(true, "", "dsComp");

                if (oComboBox.ValidValues.Count == 0)
                {
                    foreach (var item in DI.BuscaEmpresas())
                    {
                        oComboBox.ValidValues.Add(item.Key.ToString(), item.Value);
                    } 
                }

                oForm.Items.Item("cbComp").DisplayDesc = true;
            }

            try
            {
                dsSsl = oForm.DataSources.UserDataSources.Add("dsSsl", BoDataType.dt_SHORT_TEXT, 1);
                ComboBox oComboBox = (ComboBox)oForm.Items.Item("cbSsl").Specific;

                oComboBox.DataBind.SetBound(true, "", "dsSsl");

                oComboBox.ValidValues.Add("Y", "Sim");
                oComboBox.ValidValues.Add("N", "Não");

                oForm.Items.Item("cbSsl").DisplayDesc = true;
            }
            catch
            {
                dsSsl = oForm.DataSources.UserDataSources.Item("dsSsl");
                ComboBox oComboBox = (ComboBox)oForm.Items.Item("cbSsl").Specific;

                oComboBox.DataBind.SetBound(true, "", "dsSsl");

                if (oComboBox.ValidValues.Count == 0)
                {
                    oComboBox.ValidValues.Add("Y", "Sim");
                    oComboBox.ValidValues.Add("N", "Não"); 
                }

                oForm.Items.Item("cbSsl").DisplayDesc = true;
            }

            try
            {
                dsEmail = oForm.DataSources.UserDataSources.Add("dsMail", BoDataType.dt_SHORT_TEXT);
                ((EditText)oForm.Items.Item("tbMail").Specific).DataBind.SetBound(true, "", "dsMail");
            }
            catch
            {
                dsEmail = oForm.DataSources.UserDataSources.Item("dsMail");
                ((EditText)oForm.Items.Item("tbMail").Specific).DataBind.SetBound(true, "", "dsMail");
            }

            try
            {
                dsUsuario = oForm.DataSources.UserDataSources.Add("dsUsr", BoDataType.dt_SHORT_TEXT);
                ((EditText)oForm.Items.Item("tbUsr").Specific).DataBind.SetBound(true, "", "dsUsr");
            }
            catch
            {
                dsUsuario = oForm.DataSources.UserDataSources.Item("dsUsr");
                ((EditText)oForm.Items.Item("tbUsr").Specific).DataBind.SetBound(true, "", "dsUsr");
            }

            try
            {
                dsSenha = oForm.DataSources.UserDataSources.Add("dsPwd", BoDataType.dt_SHORT_TEXT);
                ((EditText)oForm.Items.Item("tbPwd").Specific).DataBind.SetBound(true, "", "dsPwd");
                ((EditText)oForm.Items.Item("tbPwd").Specific).IsPassword = true;
            }
            catch
            {
                dsSenha = oForm.DataSources.UserDataSources.Item("dsPwd");
                ((EditText)oForm.Items.Item("tbPwd").Specific).DataBind.SetBound(true, "", "dsPwd");
                ((EditText)oForm.Items.Item("tbPwd").Specific).IsPassword = true;
            }

            try
            {
                dsSmtp = oForm.DataSources.UserDataSources.Add("dsSmtp", BoDataType.dt_SHORT_TEXT);
                ((EditText)oForm.Items.Item("tbSmtp").Specific).DataBind.SetBound(true, "", "dsSmtp");
            }
            catch
            {
                dsSmtp = oForm.DataSources.UserDataSources.Item("dsSmtp");
                ((EditText)oForm.Items.Item("tbSmtp").Specific).DataBind.SetBound(true, "", "dsSmtp");
            }

            try
            {
                dsPorta = oForm.DataSources.UserDataSources.Add("dsPort", BoDataType.dt_SHORT_NUMBER, 10);
                ((EditText)oForm.Items.Item("tbPort").Specific).DataBind.SetBound(true, "", "dsPort");
            }
            catch
            {
                dsPorta = oForm.DataSources.UserDataSources.Item("dsPort");
                ((EditText)oForm.Items.Item("tbPort").Specific).DataBind.SetBound(true, "", "dsPort");
            }

            try
            {
                dsEmailCopia = oForm.DataSources.UserDataSources.Add("dsMailCp", BoDataType.dt_SHORT_TEXT);
                ((EditText)oForm.Items.Item("tbMailCp").Specific).DataBind.SetBound(true, "", "dsMailCp");
            }
            catch
            {
                dsEmailCopia = oForm.DataSources.UserDataSources.Item("dsMailCp");
                ((EditText)oForm.Items.Item("tbMailCp").Specific).DataBind.SetBound(true, "", "dsMailCp");
            }

            try
            {
                dsMsg = oForm.DataSources.UserDataSources.Add("dsMsg", BoDataType.dt_LONG_TEXT);
                ((EditText)oForm.Items.Item("tbMsg").Specific).DataBind.SetBound(true, "", "dsMsg");
            }
            catch
            {
                dsMsg = oForm.DataSources.UserDataSources.Item("dsMsg");
                ((EditText)oForm.Items.Item("tbMsg").Specific).DataBind.SetBound(true, "", "dsMsg");
            }

            try
            {
                dsSubject = oForm.DataSources.UserDataSources.Add("dsSub", BoDataType.dt_SHORT_TEXT);
                ((EditText)oForm.Items.Item("tbSub").Specific).DataBind.SetBound(true, "", "dsSub");
            }
            catch
            {
                dsSubject = oForm.DataSources.UserDataSources.Item("dsSub");
                ((EditText)oForm.Items.Item("tbSub").Specific).DataBind.SetBound(true, "", "dsSub");
            }
        }

        public static void MontaEnvioForm(ref Form oForm)
        {
            UserDataSource dsComp;
            UserDataSource dsStatus;
            UserDataSource dsDataDe;
            UserDataSource dsDataAte;
            ChooseFromList cflDocDe;
            ChooseFromList cflDocAte;
            UserDataSource dsDocDe;
            UserDataSource dsDocAte;
            UserDataSource dsCol1;
            UserDataSource dsCol2;
            UserDataSource dsCol3;
            UserDataSource dsCol4;
            UserDataSource dsCol5;
            UserDataSource dsCol6;
            UserDataSource dsCol7;
            UserDataSource dsCol8;
            UserDataSource dsCol9;
            UserDataSource dsCol10;

            try
            {
                dsComp = oForm.DataSources.UserDataSources.Add("dsComp", BoDataType.dt_SHORT_NUMBER, 10);
                ComboBox oComboBox = (ComboBox)oForm.Items.Item("cbComp").Specific;

                oComboBox.DataBind.SetBound(true, "", "dsComp");

                foreach (var item in DI.BuscaEmpresas())
                {
                    oComboBox.ValidValues.Add(item.Key.ToString(), item.Value);
                }

                oForm.Items.Item("cbComp").DisplayDesc = true;
            }
            catch
            {
                dsComp = oForm.DataSources.UserDataSources.Item("dsComp");
                ComboBox oComboBox = (ComboBox)oForm.Items.Item("cbComp").Specific;

                oComboBox.DataBind.SetBound(true, "", "dsComp");

                if (oComboBox.ValidValues.Count == 0)
                {
                    foreach (var item in DI.BuscaEmpresas())
                    {
                        oComboBox.ValidValues.Add(item.Key.ToString(), item.Value);
                    }
                }

                oForm.Items.Item("cbComp").DisplayDesc = true;
            }

            try
            {
                dsStatus = oForm.DataSources.UserDataSources.Add("dsSt", BoDataType.dt_SHORT_TEXT, 1);
                ComboBox oComboBox = (ComboBox)oForm.Items.Item("cbSt").Specific;

                oComboBox.DataBind.SetBound(true, "", "dsSt");

                oComboBox.ValidValues.Add("0", "Não enviado");
                oComboBox.ValidValues.Add("1", "Enviado");

                oForm.Items.Item("cbSt").DisplayDesc = true;
            }
            catch
            {
                dsStatus = oForm.DataSources.UserDataSources.Item("dsSt");
                ComboBox oComboBox = (ComboBox)oForm.Items.Item("cbSt").Specific;

                oComboBox.DataBind.SetBound(true, "", "dsSt");

                if (oComboBox.ValidValues.Count == 0)
                {
                    oComboBox.ValidValues.Add("0", "Não enviado");
                    oComboBox.ValidValues.Add("1", "Enviado");
                }

                oForm.Items.Item("cbSt").DisplayDesc = true;
            }

            try
            {
                dsDataDe = oForm.DataSources.UserDataSources.Add("dsIni", BoDataType.dt_DATE);
                ((EditText)oForm.Items.Item("dtIni").Specific).DataBind.SetBound(true, "", "dsIni");
            }
            catch
            {
                dsDataDe = oForm.DataSources.UserDataSources.Item("dsIni");
                ((EditText)oForm.Items.Item("dtIni").Specific).DataBind.SetBound(true, "", "dsIni");
            }

            try
            {
                dsDataAte = oForm.DataSources.UserDataSources.Add("dsFin", BoDataType.dt_DATE);
                ((EditText)oForm.Items.Item("dtFin").Specific).DataBind.SetBound(true, "", "dsFin");
            }
            catch
            {
                dsDataAte = oForm.DataSources.UserDataSources.Item("dsFin");
                ((EditText)oForm.Items.Item("dtFin").Specific).DataBind.SetBound(true, "", "dsFin");
            }

            try
            {
                var oCFls = oForm.ChooseFromLists;
                var oCFLCreationParams = B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams);
                oCFLCreationParams.MultiSelection = false;
                oCFLCreationParams.ObjectType = "13";
                oCFLCreationParams.UniqueID = "cflDocIni";

                cflDocDe = oCFls.Add(oCFLCreationParams);
            } catch { }

            try
            {
                var oCFls = oForm.ChooseFromLists;
                var oCFLCreationParams = B1Connection.Instance.Application.CreateObject(BoCreatableObjectType.cot_ChooseFromListCreationParams);
                oCFLCreationParams.MultiSelection = false;
                oCFLCreationParams.ObjectType = "13";
                oCFLCreationParams.UniqueID = "cflDocFin";

                cflDocAte = oCFls.Add(oCFLCreationParams);
            }
            catch { }

            try
            {
                dsDocDe = oForm.DataSources.UserDataSources.Add("dsDocIni", BoDataType.dt_SHORT_TEXT, 10);
                ((EditText)oForm.Items.Item("docIni").Specific).DataBind.SetBound(true, "", "dsDocIni");
                ((EditText)oForm.Items.Item("docIni").Specific).ChooseFromListUID = "cflDocIni";
                ((EditText)oForm.Items.Item("docIni").Specific).ChooseFromListAlias = "DocEntry";
            }
            catch
            {
                dsDocDe = oForm.DataSources.UserDataSources.Item("dsDocIni");
                ((EditText)oForm.Items.Item("docIni").Specific).DataBind.SetBound(true, "", "dsDocIni");
                ((EditText)oForm.Items.Item("docIni").Specific).ChooseFromListUID = "cflDocIni";
                ((EditText)oForm.Items.Item("docIni").Specific).ChooseFromListAlias = "DocEntry";
            }

            try
            {
                dsDocAte = oForm.DataSources.UserDataSources.Add("dsDocFin", BoDataType.dt_SHORT_TEXT, 10);
                ((EditText)oForm.Items.Item("docFin").Specific).DataBind.SetBound(true, "", "dsDocFin");
                ((EditText)oForm.Items.Item("docFin").Specific).ChooseFromListUID = "cflDocFin";
                ((EditText)oForm.Items.Item("docFin").Specific).ChooseFromListAlias = "DocEntry";
            }
            catch
            {
                dsDocAte = oForm.DataSources.UserDataSources.Item("dsDocFin");
                ((EditText)oForm.Items.Item("docFin").Specific).DataBind.SetBound(true, "", "dsDocFin");
                ((EditText)oForm.Items.Item("docFin").Specific).ChooseFromListUID = "cflDocFin";
                ((EditText)oForm.Items.Item("docFin").Specific).ChooseFromListAlias = "DocEntry";
            }

            Matrix oMatrix = (Matrix)oForm.Items.Item("mtxDocs").Specific;

            try
            {
                dsCol1 = oForm.DataSources.UserDataSources.Add("dsCol1", BoDataType.dt_SHORT_NUMBER, 10);
                oMatrix.Columns.Item("Col0").DataBind.SetBound(true, "", "dsCol1");
            }
            catch
            {
                dsCol1 = oForm.DataSources.UserDataSources.Item("dsCol1");
                oMatrix.Columns.Item("Col0").DataBind.SetBound(true, "", "dsCol1");
            }

            try
            {
                dsCol2 = oForm.DataSources.UserDataSources.Add("dsCol2", BoDataType.dt_SHORT_TEXT, 1);
                oMatrix.Columns.Item("ColChk").DataBind.SetBound(true, "", "dsCol2");
                oMatrix.Columns.Item("ColChk").ValOn = "Y";
                oMatrix.Columns.Item("ColChk").ValOff = "N";
            }
            catch
            {
                dsCol2 = oForm.DataSources.UserDataSources.Item("dsCol2");
                oMatrix.Columns.Item("ColChk").DataBind.SetBound(true, "", "dsCol2");
                oMatrix.Columns.Item("ColChk").ValOn = "Y";
                oMatrix.Columns.Item("ColChk").ValOff = "N";
            }

            try
            {
                dsCol3 = oForm.DataSources.UserDataSources.Add("dsCol3", BoDataType.dt_SHORT_NUMBER, 10);
                oMatrix.Columns.Item("ColDoc").DataBind.SetBound(true, "", "dsCol3");
            }
            catch
            {
                dsCol3 = oForm.DataSources.UserDataSources.Item("dsCol3");
                oMatrix.Columns.Item("ColDoc").DataBind.SetBound(true, "", "dsCol3");
            }

            try
            {
                dsCol4 = oForm.DataSources.UserDataSources.Add("dsCol4", BoDataType.dt_SHORT_TEXT, 50);
                oMatrix.Columns.Item("ColCod").DataBind.SetBound(true, "", "dsCol4");
            }
            catch
            {
                dsCol4 = oForm.DataSources.UserDataSources.Item("dsCol4");
                oMatrix.Columns.Item("ColCod").DataBind.SetBound(true, "", "dsCol4");
            }

            try
            {
                dsCol5 = oForm.DataSources.UserDataSources.Add("dsCol5", BoDataType.dt_SHORT_TEXT, 100);
                oMatrix.Columns.Item("ColNome").DataBind.SetBound(true, "", "dsCol5");
            }
            catch
            {
                dsCol5 = oForm.DataSources.UserDataSources.Item("dsCol5");
                oMatrix.Columns.Item("ColNome").DataBind.SetBound(true, "", "dsCol5");
            }

            try
            {
                dsCol6 = oForm.DataSources.UserDataSources.Add("dsCol6", BoDataType.dt_DATE);
                oMatrix.Columns.Item("ColDt").DataBind.SetBound(true, "", "dsCol6");
            }
            catch
            {
                dsCol6 = oForm.DataSources.UserDataSources.Item("dsCol6");
                oMatrix.Columns.Item("ColDt").DataBind.SetBound(true, "", "dsCol6");
            }

            try
            {
                dsCol7 = oForm.DataSources.UserDataSources.Add("dsCol7", BoDataType.dt_LONG_NUMBER, 10);
                oMatrix.Columns.Item("ColSer").DataBind.SetBound(true, "", "dsCol7");
            }
            catch
            {
                dsCol7 = oForm.DataSources.UserDataSources.Item("dsCol7");
                oMatrix.Columns.Item("ColSer").DataBind.SetBound(true, "", "dsCol7");
            }

            try
            {
                dsCol8 = oForm.DataSources.UserDataSources.Add("dsCol8", BoDataType.dt_LONG_TEXT);
                oMatrix.Columns.Item("ColMl").DataBind.SetBound(true, "", "dsCol8");
            }
            catch
            {
                dsCol8 = oForm.DataSources.UserDataSources.Item("dsCol8");
                oMatrix.Columns.Item("ColMl").DataBind.SetBound(true, "", "dsCol8");
            }

            try
            {
                dsCol10 = oForm.DataSources.UserDataSources.Add("dsCol10", BoDataType.dt_LONG_TEXT);
                oMatrix.Columns.Item("ColMlCp").DataBind.SetBound(true, "", "dsCol10");
            }
            catch
            {
                dsCol10 = oForm.DataSources.UserDataSources.Item("dsCol10");
                oMatrix.Columns.Item("ColMlCp").DataBind.SetBound(true, "", "dsCol10");
            }

            try
            {
                dsCol9 = oForm.DataSources.UserDataSources.Add("dsCol9", BoDataType.dt_SUM);
                oMatrix.Columns.Item("ColTot").DataBind.SetBound(true, "", "dsCol9");
            }
            catch
            {
                dsCol9 = oForm.DataSources.UserDataSources.Item("dsCol9");
                oMatrix.Columns.Item("ColTot").DataBind.SetBound(true, "", "dsCol9");
            }

        }

        private static bool FilterExists(ref EventFilters oFilters, BoEventTypes eventType, out int pos)
        {
            var ret = false;
            pos = -1;

            try
            {
                for (var i = 0; i < oFilters.Count; i++)
                {
                    var f = oFilters.Item(i);
                    if (eventType.Equals(f.EventType))
                    {
                        ret = true;
                        pos = i;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }

    }
}
