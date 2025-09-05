// Decompiled with JetBrains decompiler
// Type: SAPbouiCOM.ICells
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SAPbouiCOM
{
  [CompilerGenerated]
  [Guid("29FF4EC5-8A4E-42C7-B213-09A7EE7FBB32")]
  [TypeIdentifier]
  [ComImport]
  public interface ICells : IEnumerable
  {
    [DispId(0)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    Cell Item([MarshalAs(UnmanagedType.Struct), In] object Index);
  }
}
