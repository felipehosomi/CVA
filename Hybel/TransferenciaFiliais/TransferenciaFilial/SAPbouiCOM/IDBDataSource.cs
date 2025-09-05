// Decompiled with JetBrains decompiler
// Type: SAPbouiCOM.IDBDataSource
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SAPbouiCOM
{
  [CompilerGenerated]
  [Guid("9ADFD286-86EB-4A9B-83F7-F65592970A8D")]
  [TypeIdentifier]
  [ComImport]
  public interface IDBDataSource
  {
    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void _VtblGap1_4();

    [DispId(22)]
    int Size { [DispId(22), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void _VtblGap2_5();

    [DispId(34)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    string GetValue([MarshalAs(UnmanagedType.Struct), In] object Index, [In] int RecordNumber);
  }
}
