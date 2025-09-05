// Decompiled with JetBrains decompiler
// Type: SAPbobsCOM.IRecordset
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SAPbobsCOM
{
  [CompilerGenerated]
  [Guid("8CA3AB92-1930-4511-4AA8-82D53C3150C3")]
  [TypeIdentifier]
  [ComImport]
  public interface IRecordset
  {
    [DispId(0)]
    [IndexerName("Fields")]
    Fields this[] { [DispId(0), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern void _VtblGap1_5();

    [DispId(8)]
    int RecordCount { [DispId(8), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
  }
}
