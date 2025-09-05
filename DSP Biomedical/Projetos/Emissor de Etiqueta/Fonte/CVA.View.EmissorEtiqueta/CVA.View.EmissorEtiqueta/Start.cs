using System;
using Dover.Framework.Attribute;
using SAPbouiCOM;
using System.IO;
using CVA.View.EmissorEtiqueta.Resources;

namespace CVA.View.EmissorEtiqueta
{
    [Menu(FatherUID = "4352", UniqueID = "CVAFormPrincipal", String = "CVA - Impressão e Controle de Etiquetas", Type = SAPbouiCOM.BoMenuType.mt_STRING, Position = 18, Image = "")]
    [AddIn(Name = "CVA.View.EmissorEtiqueta", Description = "Impressão e controle de etiquetas", Namespace = "CVA Consultoria", InitMethod = "StartAddon")]
    public class Start
    {
        private Application _application { get; set; }

        public Start(Application application)
        {
            _application = application;
        }

        public void StartAddon()
        {
            try
            {
                if (!Directory.Exists(@"C:\PrinterSAP\Etiquetas\"))
                {
                    _application.SetStatusBarMessage("CVA: Criando pastas de usuário", BoMessageTime.bmt_Short, false);
                    Directory.CreateDirectory(@"C:\PrinterSAP\Etiquetas");
                }
            }
            catch (Exception)
            {
                _application.SetStatusBarMessage(@"Não foi possível criar a pasta C:\PrinterSAP\Etiquetas, verifique as permissões do usuário", BoMessageTime.bmt_Medium);
            }
            
                
            _application.SetStatusBarMessage("CVA Consultoria: Impressão de etiquetas ativado", BoMessageTime.bmt_Short, false);
        }

        private void VerifyFolders()
        {
            //if (!Directory.Exists(Pastas.CVAConsultoria))
            //    Directory.CreateDirectory(Pastas.CVAConsultoria);

            //if (!Directory.Exists(Pastas.Etiqueta))
            //    Directory.CreateDirectory(Pastas.Etiqueta);
        }
    }
}