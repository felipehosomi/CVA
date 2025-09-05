// Decompiled with JetBrains decompiler
// Type: SAPbobsCOM.IUserFields
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SAPbobsCOM
{
  [CompilerGenerated]
  [Guid("CC279FBE-7088-42E9-A579-EB2E3E971076")]
  [TypeIdentifier]
  [ComImport]
  public interface IUserFields
  {
    [DispId(0)]
    [IndexerName("Fields")]
    Fields this[] { [DispId(0), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
  }
}
