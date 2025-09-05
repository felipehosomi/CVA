// Decompiled with JetBrains decompiler
// Type: SAPbouiCOM.IDBDataSources
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SAPbouiCOM
{
  [CompilerGenerated]
  [Guid("A96FE0C4-B410-4F11-8EC5-E80527BC834F")]
  [TypeIdentifier]
  [ComImport]
  public interface IDBDataSources : IEnumerable
  {
    [DispId(0)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    DBDataSource Item([MarshalAs(UnmanagedType.Struct), In] object Index);
  }
}
