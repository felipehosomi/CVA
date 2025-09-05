// Decompiled with JetBrains decompiler
// Type: TransferenciaFilial.ControleBotao
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using Microsoft.CSharp.RuntimeBinder;
using SAPbobsCOM;
using SAPbouiCOM;
using SapFramework.Connections;
using SapFramework.dotNET.Atributos;
using SapFramework.SAP.UI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace TransferenciaFilial
{
  [Form("133", "Nota fiscal de saida")]
  public class ControleBotao : FormBase
  {
    public virtual void Form_Load_After(string formUID, ref ItemEvent pVal, ref bool bubbleEvent)
    {
      try
      {
        // ISSUE: variable of a compiler-generated type
        Item tem1 = this.GetItem("10000330");
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        Item tem2 = ((IForm) this.oForm)[].Add("btGeraDev", BoFormItemTypes.it_BUTTON);
        tem2.Top = tem1.Top;
        tem2.Left = tem1.Left - 100;
        tem2.Width = 100;
        // ISSUE: reference to a compiler-generated field
        if (ControleBotao.\u003C\u003Eo__0.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ControleBotao.\u003C\u003Eo__0.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Button>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Button), typeof (ControleBotao)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        Button button = ControleBotao.\u003C\u003Eo__0.\u003C\u003Ep__0.Target((CallSite) ControleBotao.\u003C\u003Eo__0.\u003C\u003Ep__0, tem2[]);
        button.Caption = "Gera Devolução";
      }
      catch
      {
      }
    }

    public virtual void ItemClicked_After(string formUID, ref ItemEvent pVal, ref bool bubbleEvent)
    {
      try
      {
        if (!(pVal.ItemUID == "btGeraDev"))
          return;
        // ISSUE: variable of a compiler-generated type
        Item tem = this.GetItem("btGeraDev");
        if (tem.Enabled)
        {
          // ISSUE: reference to a compiler-generated field
          if (ControleBotao.\u003C\u003Eo__1.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ControleBotao.\u003C\u003Eo__1.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (ControleBotao)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          GeracaoNota.GeraEsbocoDevolucaoNFSaida(Convert.ToInt32(ControleBotao.\u003C\u003Eo__1.\u003C\u003Ep__0.Target((CallSite) ControleBotao.\u003C\u003Eo__1.\u003C\u003Ep__0, this.GetItem("8")[]).Value));
        }
      }
      catch
      {
      }
    }

    public virtual void FormDataLoad_After(ref BusinessObjectInfo pVal, ref bool bubbleevent)
    {
      try
      {
        // ISSUE: variable of a compiler-generated type
        Item tem = this.GetItem("btGeraDev");
        string format = "select top 1 * from (\r\nselect 'Tipo' = 'E', DocEntry from ODRF where u_cva_nf_custo = '{0}' \r\nunion all\r\nselect 'Tipo' = 'D', DocEntry from ORIN where u_cva_nf_custo = '{0}' \r\n) as dados order by Tipo, DocEntry desc";
        // ISSUE: reference to a compiler-generated field
        if (ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (ControleBotao)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        string str = ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__0, ((IForm) this.oForm)[].Item((object) "2036")[]).Value.ToString();
        // ISSUE: variable of a compiler-generated type
        Recordset recordset = B1AppDomain.RSQuery(string.Format(format, (object) str));
        if (recordset.RecordCount <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        if (ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ControleBotao), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target = ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p2 = ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ControleBotao), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        object obj = ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__1.Target((CallSite) ControleBotao.\u003C\u003Eo__2.\u003C\u003Ep__1, recordset[].Item((object) "Tipo").Value, "D");
        if (target((CallSite) p2, obj))
          tem.Enabled = false;
      }
      catch
      {
      }
    }
  }
}
