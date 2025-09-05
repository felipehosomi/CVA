// Decompiled with JetBrains decompiler
// Type: SAPbouiCOM.IComboBox
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SAPbouiCOM
{
  [CompilerGenerated]
  [Guid("BB08FCAE-635E-4CE9-B95F-909C77B1924D")]
  [TypeIdentifier]
  [ComImport]
  public interface IComboBox
  {
    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void _VtblGap1_2();

    [DispId(3)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Select([MarshalAs(UnmanagedType.Struct), In] object Index, [In] BoSearchKey SearchKey = BoSearchKey.psk_ByValue);
  }
}
