// Decompiled with JetBrains decompiler
// Type: TransferenciaFilial.GeracaoNota
// Assembly: TransferenciaFilial, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CD3926C-3C0C-41AC-BE03-0C638A4EB456
// Assembly location: D:\CVA\Development\Hybel\TransferenciaFiliais\Executável\TransferenciaFilial.exe

using Microsoft.CSharp.RuntimeBinder;
using SAPbobsCOM;
using SAPbouiCOM;
using SapFramework.Connections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace TransferenciaFilial
{
  public class GeracaoNota
  {
    public static void GeraDevolucaoNFSaida(int idSaida, Form formDev, Form formSaida)
    {
      try
      {
        // ISSUE: reference to a compiler-generated method
        formDev.Freeze(true);
        // ISSUE: reference to a compiler-generated field
        if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (GeracaoNota)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        EditText editText1 = GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__1.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__1, formDev[].Item((object) "4")[]);
        // ISSUE: reference to a compiler-generated field
        if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (GeracaoNota)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        string str1 = GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__0.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__0, formSaida[].Item((object) "4")[]).Value;
        editText1.Value = str1;
        // ISSUE: reference to a compiler-generated field
        if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (GeracaoNota)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        EditText editText2 = GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__3.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__3, formDev[].Item((object) "U_CVA_NF_CUSTO")[]);
        // ISSUE: reference to a compiler-generated field
        if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (GeracaoNota)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        string str2 = GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__2.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__2, formSaida[].Item((object) "2036")[]).Value;
        editText2.Value = str2;
        // ISSUE: reference to a compiler-generated method
        formDev.DataSources.DBDataSources.Item((object) "RIN1");
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        formDev[].Item((object) "14").Click(BoCellClickType.ct_Regular);
        // ISSUE: reference to a compiler-generated field
        if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, Matrix>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Matrix), typeof (GeracaoNota)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        Matrix matrix1 = GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__4.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__4, formDev[].Item((object) "38")[]);
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        DBDataSource dbDataSource = formSaida.DataSources.DBDataSources.Item((object) "INV1");
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        ProgressBar progressBar = B1AppDomain.get_Application().StatusBar.CreateProgressBar("Aguarde... Gerando Linhas", dbDataSource.Size, false);
        for (int RecordNumber = 0; RecordNumber < dbDataSource.Size; ++RecordNumber)
        {
          try
          {
            ++progressBar.Value;
          }
          catch
          {
          }
          // ISSUE: reference to a compiler-generated method
          string str3 = dbDataSource.GetValue((object) "ItemCode", RecordNumber).Trim();
          // ISSUE: reference to a compiler-generated method
          string str4 = dbDataSource.GetValue((object) "Quantity", RecordNumber);
          // ISSUE: reference to a compiler-generated method
          string str5 = dbDataSource.GetValue((object) "Price", RecordNumber);
          // ISSUE: reference to a compiler-generated field
          if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (GeracaoNota)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__5.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__5, matrix1[].Item((object) "1").Cells.Item((object) (RecordNumber + 1))[]).Value = str3;
          // ISSUE: reference to a compiler-generated field
          if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (GeracaoNota)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__6.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__6, matrix1[].Item((object) "11").Cells.Item((object) (RecordNumber + 1))[]).Value = Convert.ToDouble(str4.Replace(".", ",")).ToString();
          // ISSUE: reference to a compiler-generated field
          if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (GeracaoNota)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__7.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__7, matrix1[].Item((object) "14").Cells.Item((object) (RecordNumber + 1))[]).Value = Convert.ToDouble(str5.Replace(".", ",")).ToString("#0.00");
          // ISSUE: reference to a compiler-generated field
          if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, CheckBox>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (CheckBox), typeof (GeracaoNota)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__8.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__8, matrix1[].Item((object) "1470002169").Cells.Item((object) (RecordNumber + 1))[]).Checked = true;
          // ISSUE: reference to a compiler-generated field
          if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (GeracaoNota)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__9.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__9, matrix1[].Item((object) "1470002171").Cells.Item((object) (RecordNumber + 1))[]).Value = dbDataSource.GetValue((object) "StockPrice", RecordNumber);
        }
        try
        {
          // ISSUE: reference to a compiler-generated method
          progressBar.Stop();
        }
        catch
        {
        }
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        formDev[].Item((object) "2013").Click(BoCellClickType.ct_Regular);
        // ISSUE: reference to a compiler-generated field
        if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (GeracaoNota)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__10.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__10, formDev[].Item((object) "2021")[]).Value = "NF n°" + idSaida.ToString();
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        formDev[].Item((object) "498").Click(BoCellClickType.ct_Regular);
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        formDev[].Item((object) "3").Click(BoCellClickType.ct_Regular);
        // ISSUE: variable of a compiler-generated type
        Form activeForm = B1AppDomain.get_Application().Forms.ActiveForm;
        // ISSUE: reference to a compiler-generated method
        activeForm.Freeze(true);
        // ISSUE: reference to a compiler-generated field
        if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, Matrix>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Matrix), typeof (GeracaoNota)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        Matrix matrix2 = GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__11.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__11, activeForm[].Item((object) "5")[]);
        // ISSUE: reference to a compiler-generated field
        if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, ComboBox>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (ComboBox), typeof (GeracaoNota)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__12.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__12, matrix2[].Item((object) "1").Cells.Item((object) 1)[]).Select((object) "13", BoSearchKey.psk_ByValue);
        // ISSUE: reference to a compiler-generated field
        if (GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, EditText>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (EditText), typeof (GeracaoNota)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__13.Target((CallSite) GeracaoNota.\u003C\u003Eo__0.\u003C\u003Ep__13, matrix2[].Item((object) "3").Cells.Item((object) 1)[]).Value = idSaida.ToString();
        // ISSUE: reference to a compiler-generated method
        activeForm.Freeze(false);
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        activeForm[].Item((object) "540020001").Click(BoCellClickType.ct_Regular);
        // ISSUE: reference to a compiler-generated method
        formDev.Freeze(false);
      }
      catch (Exception ex)
      {
        // ISSUE: reference to a compiler-generated method
        B1AppDomain.get_Application().MessageBox(ex.Message, 1, "Ok", "", "");
      }
    }

    public static void GeraEsbocoDevolucaoNFSaida(int idSaida)
    {
      // ISSUE: reference to a compiler-generated field
      if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Documents>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Documents), typeof (GeracaoNota)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      Documents documents1 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__0.Target((CallSite) GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__0, B1AppDomain.get_Company().GetBusinessObject(BoObjectTypes.oDrafts));
      // ISSUE: reference to a compiler-generated field
      if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Documents>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Documents), typeof (GeracaoNota)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      Documents documents2 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__1.Target((CallSite) GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__1, B1AppDomain.get_Company().GetBusinessObject(BoObjectTypes.oInvoices));
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      ProgressBar progressBar = B1AppDomain.get_Application().StatusBar.CreateProgressBar("Aguarde... Gerando Linhas", documents2.Lines.Count, false);
      try
      {
        // ISSUE: reference to a compiler-generated method
        if (documents2.GetByKey(idSaida))
        {
          // ISSUE: variable of a compiler-generated type
          Recordset recordset1 = B1AppDomain.RSQuery(string.Format("select top 1 * from (\r\nselect 'Tipo' = 'E', DocEntry from ODRF where u_cva_nf_custo = '{0}' \r\nunion all\r\nselect 'Tipo' = 'D', DocEntry from ORIN where u_cva_nf_custo = '{0}' \r\n) as dados order by Tipo, DocEntry desc", (object) documents2.SequenceSerial.ToString()));
          if (recordset1.RecordCount > 0)
          {
            try
            {
              // ISSUE: reference to a compiler-generated method
              progressBar.Stop();
            }
            catch
            {
            }
            // ISSUE: reference to a compiler-generated field
            if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (GeracaoNota), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, bool> target1 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__3.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, bool>> p3 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__3;
            // ISSUE: reference to a compiler-generated field
            if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (GeracaoNota), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            object obj1 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__2.Target((CallSite) GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__2, recordset1[].Item((object) "Tipo").Value, "E");
            if (target1((CallSite) p3, obj1))
            {
              // ISSUE: reference to a compiler-generated field
              if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__5 == null)
              {
                // ISSUE: reference to a compiler-generated field
                GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, Form>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Form), typeof (GeracaoNota)));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, Form> target2 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__5.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, Form>> p5 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__5;
              // ISSUE: reference to a compiler-generated field
              if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__4 == null)
              {
                // ISSUE: reference to a compiler-generated field
                GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__4 = CallSite<Func<CallSite, Application, BoFormObjectEnum, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "OpenForm", (IEnumerable<Type>) null, typeof (GeracaoNota), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              object obj2 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__4.Target((CallSite) GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__4, B1AppDomain.get_Application(), (BoFormObjectEnum) 112, "", recordset1[].Item((object) "DocEntry").Value);
              // ISSUE: variable of a compiler-generated type
              Form form = target2((CallSite) p5, obj2);
              // ISSUE: reference to a compiler-generated method
              form.Select();
              return;
            }
            // ISSUE: reference to a compiler-generated field
            if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (GeracaoNota), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, bool> target3 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__7.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, bool>> p7 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__7;
            // ISSUE: reference to a compiler-generated field
            if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (GeracaoNota), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            object obj3 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__6.Target((CallSite) GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__6, recordset1[].Item((object) "Tipo").Value, "D");
            if (!target3((CallSite) p7, obj3))
              return;
            // ISSUE: reference to a compiler-generated field
            if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, Form>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Form), typeof (GeracaoNota)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, Form> target4 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__9.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, Form>> p9 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__9;
            // ISSUE: reference to a compiler-generated field
            if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__8 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__8 = CallSite<Func<CallSite, Application, BoFormObjectEnum, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "OpenForm", (IEnumerable<Type>) null, typeof (GeracaoNota), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            object obj4 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__8.Target((CallSite) GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__8, B1AppDomain.get_Application(), BoFormObjectEnum.fo_InvoiceCreditMemo, "", recordset1[].Item((object) "DocEntry").Value);
            // ISSUE: variable of a compiler-generated type
            Form form1 = target4((CallSite) p9, obj4);
            // ISSUE: reference to a compiler-generated method
            form1.Select();
            return;
          }
          // ISSUE: variable of a compiler-generated type
          Recordset recordset2 = B1AppDomain.RSQuery(string.Format("select top 1 CardCode from CRD7 where TaxId0 = (select TaxIdNum from obpl where BPLId = '{0}') and CardCode like 'C%'", (object) documents2.BPL_IDAssignedToInvoice));
          // ISSUE: variable of a compiler-generated type
          Recordset recordset3 = B1AppDomain.RSQuery(string.Format("select top 1 BPLId from OBPL where County = (select County from INV12 where DocEntry = '{0}') ", (object) documents2.DocEntry));
          // ISSUE: variable of a compiler-generated type
          Documents documents3 = documents1;
          // ISSUE: reference to a compiler-generated field
          if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (GeracaoNota)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          string str1 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__10.Target((CallSite) GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__10, recordset2[].Item((object) "CardCode").Value);
          documents3.CardCode = str1;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: variable of a compiler-generated type
          Field field = documents1.UserFields[].Item((object) "U_CVA_NF_CUSTO");
          int num1 = documents2.SequenceSerial;
          string str2 = num1.ToString();
          field.Value = (object) str2;
          // ISSUE: variable of a compiler-generated type
          Documents documents4 = documents1;
          // ISSUE: reference to a compiler-generated field
          if (GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (GeracaoNota)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          int num2 = GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__11.Target((CallSite) GeracaoNota.\u003C\u003Eo__1.\u003C\u003Ep__11, recordset3[].Item((object) "BPLId").Value);
          documents4.BPL_IDAssignedToInvoice = num2;
          documents1.DocDueDate = DateTime.Now;
          documents1.TaxDate = DateTime.Now;
          documents1.DocDate = DateTime.Now;
          documents1.SalesPersonCode = documents2.SalesPersonCode;
          documents1.DocObjectCode = BoObjectTypes.oCreditNotes;
          documents1.AddressExtension.BillToAddress2 = documents2.AddressExtension.BillToAddress2;
          documents1.AddressExtension.BillToAddress3 = documents2.AddressExtension.BillToAddress3;
          documents1.AddressExtension.BillToAddressType = documents2.AddressExtension.BillToAddressType;
          documents1.AddressExtension.BillToBlock = documents2.AddressExtension.BillToBlock;
          documents1.AddressExtension.BillToBuilding = documents2.AddressExtension.BillToBuilding;
          documents1.AddressExtension.BillToCity = documents2.AddressExtension.BillToCity;
          documents1.AddressExtension.BillToCountry = documents2.AddressExtension.BillToCountry;
          documents1.AddressExtension.BillToCounty = documents2.AddressExtension.BillToCounty;
          documents1.AddressExtension.BillToGlobalLocationNumber = documents2.AddressExtension.BillToGlobalLocationNumber;
          documents1.AddressExtension.BillToState = documents2.AddressExtension.BillToState;
          documents1.AddressExtension.BillToStreet = documents2.AddressExtension.BillToStreet;
          documents1.AddressExtension.BillToStreetNo = documents2.AddressExtension.BillToStreetNo;
          documents1.AddressExtension.BillToZipCode = documents2.AddressExtension.BillToZipCode;
          documents1.AddressExtension.PlaceOfSupply = documents2.AddressExtension.PlaceOfSupply;
          documents1.AddressExtension.ShipToAddress2 = documents2.AddressExtension.ShipToAddress2;
          documents1.AddressExtension.ShipToAddress3 = documents2.AddressExtension.ShipToAddress3;
          documents1.AddressExtension.ShipToAddressType = documents2.AddressExtension.ShipToAddressType;
          documents1.AddressExtension.ShipToBlock = documents2.AddressExtension.ShipToBlock;
          documents1.AddressExtension.ShipToBuilding = documents2.AddressExtension.ShipToBuilding;
          documents1.AddressExtension.ShipToCity = documents2.AddressExtension.ShipToCity;
          documents1.AddressExtension.ShipToCountry = documents2.AddressExtension.ShipToCountry;
          documents1.AddressExtension.ShipToCounty = documents2.AddressExtension.ShipToCounty;
          documents1.AddressExtension.ShipToGlobalLocationNumber = documents2.AddressExtension.ShipToGlobalLocationNumber;
          documents1.AddressExtension.ShipToState = documents2.AddressExtension.ShipToState;
          documents1.AddressExtension.ShipToStreet = documents2.AddressExtension.ShipToStreet;
          documents1.AddressExtension.ShipToStreetNo = documents2.AddressExtension.ShipToStreetNo;
          documents1.AddressExtension.ShipToZipCode = documents2.AddressExtension.ShipToZipCode;
          documents1.TaxExtension.BillOfEntryDate = documents2.TaxExtension.BillOfEntryDate;
          documents1.TaxExtension.BillOfEntryNo = documents2.TaxExtension.BillOfEntryNo;
          documents1.TaxExtension.BlockB = documents2.TaxExtension.BlockB;
          documents1.TaxExtension.BlockS = documents2.TaxExtension.BlockS;
          documents1.TaxExtension.Brand = documents2.TaxExtension.Brand;
          documents1.TaxExtension.Carrier = documents2.TaxExtension.Carrier;
          documents1.TaxExtension.CityB = documents2.TaxExtension.CityB;
          documents1.TaxExtension.CityS = documents2.TaxExtension.CityS;
          documents1.TaxExtension.CountryB = documents2.TaxExtension.CountryB;
          documents1.TaxExtension.CountryS = documents2.TaxExtension.CountryS;
          documents1.TaxExtension.County = documents2.TaxExtension.County;
          documents1.TaxExtension.CountyB = documents2.TaxExtension.CountyB;
          documents1.TaxExtension.CountyS = documents2.TaxExtension.CountyS;
          documents1.TaxExtension.GlobalLocationNumberB = documents2.TaxExtension.GlobalLocationNumberB;
          documents1.TaxExtension.GlobalLocationNumberS = documents2.TaxExtension.GlobalLocationNumberS;
          documents1.TaxExtension.GrossWeight = documents2.TaxExtension.GrossWeight;
          documents1.TaxExtension.ImportOrExport = documents2.TaxExtension.ImportOrExport;
          documents1.TaxExtension.ImportOrExportType = documents2.TaxExtension.ImportOrExportType;
          documents1.TaxExtension.Incoterms = documents2.TaxExtension.Incoterms;
          documents1.TaxExtension.MainUsage = 5;
          documents1.TaxExtension.NetWeight = documents2.TaxExtension.NetWeight;
          documents1.TaxExtension.NFRef = documents2.TaxExtension.NFRef;
          documents1.TaxExtension.OriginalBillOfEntryDate = documents2.TaxExtension.OriginalBillOfEntryDate;
          documents1.TaxExtension.OriginalBillOfEntryNo = documents2.TaxExtension.OriginalBillOfEntryNo;
          documents1.TaxExtension.PackDescription = documents2.TaxExtension.PackDescription;
          documents1.TaxExtension.PackQuantity = documents2.TaxExtension.PackQuantity;
          documents1.TaxExtension.PortCode = documents2.TaxExtension.PortCode;
          documents1.TaxExtension.ShipUnitNo = documents2.TaxExtension.ShipUnitNo;
          documents1.TaxExtension.State = documents2.TaxExtension.State;
          documents1.TaxExtension.StateB = documents2.TaxExtension.StateB;
          documents1.TaxExtension.StateS = documents2.TaxExtension.StateS;
          documents1.TaxExtension.StreetB = documents2.TaxExtension.StreetB;
          documents1.TaxExtension.StreetS = documents2.TaxExtension.StreetS;
          documents1.TaxExtension.TaxId0 = documents2.TaxExtension.TaxId0;
          documents1.TaxExtension.TaxId1 = documents2.TaxExtension.TaxId1;
          documents1.TaxExtension.TaxId12 = documents2.TaxExtension.TaxId12;
          documents1.TaxExtension.TaxId13 = documents2.TaxExtension.TaxId13;
          documents1.TaxExtension.TaxId2 = documents2.TaxExtension.TaxId2;
          documents1.TaxExtension.TaxId3 = documents2.TaxExtension.TaxId3;
          documents1.TaxExtension.TaxId4 = documents2.TaxExtension.TaxId4;
          documents1.TaxExtension.TaxId5 = documents2.TaxExtension.TaxId5;
          documents1.TaxExtension.TaxId6 = documents2.TaxExtension.TaxId6;
          documents1.TaxExtension.TaxId7 = documents2.TaxExtension.TaxId7;
          documents1.TaxExtension.TaxId8 = documents2.TaxExtension.TaxId8;
          documents1.TaxExtension.TaxId9 = documents2.TaxExtension.TaxId9;
          documents1.TaxExtension.Vehicle = documents2.TaxExtension.Vehicle;
          documents1.TaxExtension.VehicleState = documents2.TaxExtension.VehicleState;
          documents1.TaxExtension.ZipCodeB = documents2.TaxExtension.ZipCodeB;
          documents1.TaxExtension.ZipCodeS = documents2.TaxExtension.ZipCodeS;
          for (int LineNum1 = 0; LineNum1 < documents2.Lines.Count; ++LineNum1)
          {
            num1 = progressBar.Value++;
            // ISSUE: reference to a compiler-generated method
            documents2.Lines.SetCurrentLine(LineNum1);
            if (LineNum1 > 0)
            {
              // ISSUE: reference to a compiler-generated method
              documents1.Lines.Add();
            }
            documents1.Lines.ItemCode = documents2.Lines.ItemCode;
            documents1.Lines.Quantity = documents2.Lines.Quantity;
            documents1.Lines.Price = documents2.Lines.Price;
            documents1.Lines.PriceAfterVAT = documents2.Lines.PriceAfterVAT;
            documents1.Lines.UnitPrice = documents2.Lines.UnitPrice;
            documents1.Lines.EnableReturnCost = BoYesNoEnum.tYES;
            documents1.Lines.ReturnCost = documents2.Lines.GrossBuyPrice;
            documents1.Lines.Usage = "5";
            for (int LineNum2 = 0; LineNum2 < documents2.Lines.SerialNumbers.Count; ++LineNum2)
            {
              // ISSUE: reference to a compiler-generated method
              documents2.Lines.SerialNumbers.SetCurrentLine(LineNum2);
              if (!string.IsNullOrEmpty(documents2.Lines.SerialNumbers.InternalSerialNumber))
              {
                if (LineNum2 > 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  documents1.Lines.SerialNumbers.Add();
                }
                documents1.Lines.SerialNumbers.InternalSerialNumber = documents2.Lines.SerialNumbers.InternalSerialNumber;
                documents1.Lines.SerialNumbers.ManufacturerSerialNumber = documents2.Lines.SerialNumbers.ManufacturerSerialNumber;
              }
            }
            for (int LineNum2 = 0; LineNum2 < documents2.Lines.BatchNumbers.Count; ++LineNum2)
            {
              // ISSUE: reference to a compiler-generated method
              documents2.Lines.BatchNumbers.SetCurrentLine(LineNum2);
              if (!string.IsNullOrEmpty(documents2.Lines.BatchNumbers.BatchNumber))
              {
                if (LineNum2 > 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  documents1.Lines.BatchNumbers.Add();
                }
                documents1.Lines.BatchNumbers.BatchNumber = documents2.Lines.BatchNumbers.BatchNumber;
                documents1.Lines.BatchNumbers.InternalSerialNumber = documents2.Lines.BatchNumbers.InternalSerialNumber;
                documents1.Lines.BatchNumbers.Quantity = documents2.Lines.BatchNumbers.Quantity;
              }
            }
          }
        }
        try
        {
          // ISSUE: reference to a compiler-generated method
          progressBar.Stop();
        }
        catch
        {
        }
        string errMsg = "";
        // ISSUE: reference to a compiler-generated method
        int errCode = documents1.Add();
        if ((uint) errCode > 0U)
        {
          // ISSUE: reference to a compiler-generated method
          B1AppDomain.get_Company().GetLastError(out errCode, out errMsg);
          // ISSUE: reference to a compiler-generated method
          B1AppDomain.get_Application().MessageBox(errCode.ToString() + " :: " + errMsg, 1, "Ok", "", "");
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          B1AppDomain.get_Application().MessageBox("Esboço devolução criado com sucesso!", 1, "Ok", "", "");
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          B1AppDomain.get_Application().OpenForm((BoFormObjectEnum) 112, "", B1AppDomain.get_Company().GetNewObjectKey());
        }
      }
      catch (Exception ex)
      {
        try
        {
          // ISSUE: reference to a compiler-generated method
          progressBar.Stop();
        }
        catch
        {
        }
        // ISSUE: reference to a compiler-generated method
        B1AppDomain.get_Application().MessageBox(ex.Message, 1, "Ok", "", "");
      }
    }
  }
}
