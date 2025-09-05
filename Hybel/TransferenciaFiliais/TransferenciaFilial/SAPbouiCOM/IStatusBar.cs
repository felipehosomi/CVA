// Decompiled with JetBrains decompiler
// Type: SAPbouiCOM.IStatusBar
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SAPbouiCOM
{
  [CompilerGenerated]
  [Guid("C1DC17AD-B039-4E27-87C2-84F7EC82313F")]
  [TypeIdentifier]
  [ComImport]
  public interface IStatusBar
  {
    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void _VtblGap1_1();

    [DispId(4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    ProgressBar CreateProgressBar([MarshalAs(UnmanagedType.BStr), In] string Text, [In] int Maximum, [In] bool Stoppable);
  }
}
