Create Procedure AlertaMinimo()
as
Begin

select 'Atenção, faltam apenas ' || ("U_CVA_SerieFin" - "U_CVA_SerieAtu")||
       ' sequencias para esgotar o limite de códigos de rastreio do tipo despacho '||
        "U_CVA_TipoDespacho"||'/'||"U_CVA_CardCode" as "Alerta"
  from "@CVA_CFGDESPACHO"
 where ("U_CVA_SerieFin" - "U_CVA_SerieAtu") < "U_CVA_AlertaMinimo";
 
 end;
 
 Call AlertaMinimo();
 
 
 
