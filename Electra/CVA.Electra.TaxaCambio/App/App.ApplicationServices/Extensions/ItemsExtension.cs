using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices
{
    public static class ItemsExtension
    {
        //public static List<Item> AllItems(this Items Component)
        //{
        //    try
        //    {
        //        List<Item> vListItem = new List<Item>();

        //        vListItem = Component.AsParallel().Cast<SAPbouiCOM.Item>().ToList();

        //        return vListItem;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static Item GetItem(this Items Component, string UID)
        //{
        //    try
        //    {
        //        Item vItem = null;

        //        try
        //        {
        //            vItem = Component.Item(UID);
        //        }
        //        catch
        //        {

        //        }

        //        return vItem;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static T GetOrAdd<T>(this Items Componente, string UID, SAPbouiCOM.BoFormItemTypes TipoItem) where T : class
        {
            SAPbouiCOM.Item vItem = null;

            try
            {
                try
                {
                    vItem = Componente.Item(UID);
                }
                catch
                {
                    vItem = null;
                }

                if (vItem == null)
                {
                    vItem = Componente.Add(UID, TipoItem);
                }

                return (T)vItem.Specific;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T GetOrAdd<T>(this Items Componente, string UID) where T : class
        {
            SAPbouiCOM.Item vItem = null;
            T vSpecifc = default(T);

            try
            {
                try
                {
                    vItem = Componente.Item(UID);
                }
                catch
                {
                    vItem = null;
                }

                if (vItem != null)
                {
                    vSpecifc = (T)vItem.Specific;
                }

                return vSpecifc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Release<T>(this T Obj) where T : class
        {
            try
            {
                if (Obj != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(Obj);
                    Obj = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SetAutoManagedAttribute_Editable(this Item vItem, BoAutoFormMode Mask, BoModeVisualBehavior ModeBehavior)
        {
            try
            {
                int iMask = (int)Mask;
                vItem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, iMask, ModeBehavior);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetAutoManagedAttribute_Editable_True(this Item vItem, params SAPbouiCOM.BoAutoFormMode[] Masks)
        {
            try
            {
                BoAutoFormMode vMask;

                for (int i = 0; i < Masks.Length; i++)
                {
                    vMask = Masks[i];

                    vItem.SetAutoManagedAttribute_Editable(vMask, BoModeVisualBehavior.mvb_True);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetAutoManagedAttribute_Editable_False(this Item vItem, params BoAutoFormMode[] Masks)
        {
            try
            {
                BoAutoFormMode vMask;

                for (int i = 0; i < Masks.Length; i++)
                {
                    vMask = Masks[i];

                    vItem.SetAutoManagedAttribute_Editable(vMask, BoModeVisualBehavior.mvb_False);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SetAutoManagedAttribute_Visible(this Item vItem, BoAutoFormMode Mask, BoModeVisualBehavior ModeBehavior)
        {
            try
            {
                int iMask = (int)Mask;
                vItem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, iMask, ModeBehavior);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetAutoManagedAttribute_Visible_True(this Item vItem, params BoAutoFormMode[] Masks)
        {
            try
            {
                BoAutoFormMode vMask;

                for (int i = 0; i < Masks.Length; i++)
                {
                    vMask = Masks[i];

                    vItem.SetAutoManagedAttribute_Visible(vMask, BoModeVisualBehavior.mvb_True);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetAutoManagedAttribute_Visible_False(this Item vItem, params BoAutoFormMode[] Masks)
        {
            try
            {
                BoAutoFormMode vMask;

                for (int i = 0; i < Masks.Length; i++)
                {
                    vMask = Masks[i];

                    vItem.SetAutoManagedAttribute_Visible(vMask, BoModeVisualBehavior.mvb_False);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
