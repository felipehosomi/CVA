// Decompiled with JetBrains decompiler
// Type: TransferenciaFilial.ControleApp
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using SAPbouiCOM;
using SapFramework.SAP.UI;

namespace TransferenciaFilial
{
  [SapFramework.dotNET.Atributos.Form("App", "App")]
  public class ControleApp : FormBase
  {
    public virtual void AppCompanyChanged(ref BoAppEventTypes pVal)
    {
      System.Windows.Forms.Application.Exit();
    }

    public virtual void AppShutdown(ref BoAppEventTypes pVal)
    {
      System.Windows.Forms.Application.Exit();
    }

    public virtual void AppServerTerminition(ref BoAppEventTypes pVal)
    {
      System.Windows.Forms.Application.Exit();
    }
  }
}
