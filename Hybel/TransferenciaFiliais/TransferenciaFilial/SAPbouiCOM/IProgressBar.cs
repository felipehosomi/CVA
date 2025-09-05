// Decompiled with JetBrains decompiler
// Type: SAPbouiCOM.IProgressBar
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SAPbouiCOM
{
  [CompilerGenerated]
  [Guid("F71AF054-37D7-4033-A975-4CFBFEDCEC02")]
  [TypeIdentifier]
  [ComImport]
  public interface IProgressBar
  {
    [DispId(3)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Stop();

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void _VtblGap1_2();

    [DispId(9)]
    int Value { [DispId(9), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(9), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }
  }
}
