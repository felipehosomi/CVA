// Decompiled with JetBrains decompiler
// Type: SAPbouiCOM.IMatrix
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SAPbouiCOM
{
  [CompilerGenerated]
  [Guid("DD7804DC-811B-4829-8E87-2F5C061EE59C")]
  [TypeIdentifier]
  [ComImport]
  public interface IMatrix
  {
    [DispId(0)]
    [IndexerName("Columns")]
    Columns this[] { [DispId(0), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
  }
}
