// Decompiled with JetBrains decompiler
// Type: TransferenciaFilial.Program
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using SapFramework.Connections.UI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace TransferenciaFilial
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      new ConnectUI().Connect(Assembly.Load("TransferenciaFilial").GetTypes());
      Application.Run();
    }
  }
}
