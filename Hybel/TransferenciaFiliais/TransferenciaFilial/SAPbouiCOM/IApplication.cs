// Decompiled with JetBrains decompiler
// Type: SAPbouiCOM.IApplication
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SAPbouiCOM
{
  [CompilerGenerated]
  [Guid("D1F75D47-137C-4335-AC2A-3FE209831B6A")]
  [TypeIdentifier]
  [ComImport]
  public interface IApplication
  {
    [DispId(1)]
    Forms Forms { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void _VtblGap1_5();

    [DispId(50)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int MessageBox([MarshalAs(UnmanagedType.BStr), In] string Text, [In] int DefaultBtn = 1, [MarshalAs(UnmanagedType.BStr), In] string Btn1Caption = "Ok", [MarshalAs(UnmanagedType.BStr), In] string Btn2Caption = "", [MarshalAs(UnmanagedType.BStr), In] string Btn3Caption = "");

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void _VtblGap2_7();

    [DispId(64)]
    StatusBar StatusBar { [DispId(64), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void _VtblGap3_11();

    [DispId(84)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    Form OpenForm([In] BoFormObjectEnum sysObjectType, [MarshalAs(UnmanagedType.BStr), In] string bstrUDOObjectType, [MarshalAs(UnmanagedType.BStr), In] string bstrObjectKey);
  }
}
