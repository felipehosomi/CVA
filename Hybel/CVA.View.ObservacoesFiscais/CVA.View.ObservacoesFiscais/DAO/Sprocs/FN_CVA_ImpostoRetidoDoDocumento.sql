IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'FN_CVA_ImpostoRetidoDoDocumento')
	DROP FUNCTION FN_CVA_ImpostoRetidoDoDocumento
GO
CREATE FUNCTION [dbo].[FN_CVA_ImpostoRetidoDoDocumento] (
  @objType varchar(30)
, @DocEntry int
, @imposto varchar(30)

)
returns decimal(19, 9)

as
begin

  declare @vl_imposto       decimal(19, 9)
  
  declare @vl_imposto_pdn   decimal(19, 9)
  declare @vl_imposto_pch   decimal(19, 9)
  declare @vl_imposto_rpd   decimal(19, 9)
  declare @vl_impostO_rpc   decimal(19, 9)
  
  declare @vl_imposto_dln   decimal(19, 9) 
  declare @vl_imposto_rdn   decimal(19, 9)
  declare @vl_imposto_inv   decimal(19, 9)
  declare @vl_imposto_rin   decimal(19, 9)

  select @vl_imposto     = 0
       , @vl_imposto_pdn = 0
       , @vl_imposto_pch = 0
       , @vl_imposto_rpd = 0
       , @vl_impostO_rpc = 0
       , @vl_imposto_dln = 0
       , @vl_imposto_rdn = 0
       , @vl_imposto_inv = 0
       , @vl_imposto_rin = 0 
  
  
  select @vl_imposto_pdn = SUM(a.WTAmnt)
    from PDN5 a
   inner join OPDN d
      on a.AbsEntry = d.DocEntry 
   inner join OWHT b
      on b.WTCode = a.WTCode 
   inner join OWTT c
      on b.WTTypeId = c.WTTypeId 
   where d.DocEntry = @DocEntry
     and d.ObjType = @objType 
     and c.WTType = @imposto

  select @vl_imposto_pch = SUM(a.WTAmnt)
    from pch5 a
   inner join opch d
      on a.AbsEntry = d.DocEntry 
   inner join OWHT b
      on b.WTCode = a.WTCode 
   inner join OWTT c
      on b.WTTypeId = c.WTTypeId 
   where d.DocEntry = @DocEntry
     and d.ObjType = @objType 
     and c.WTType = @imposto

  select @vl_imposto_rpd = SUM(a.WTAmnt)
    from rpd5 a
   inner join ORPD d
      on a.AbsEntry = d.DocEntry 
   inner join OWHT b
      on b.WTCode = a.WTCode 
   inner join OWTT c
      on b.WTTypeId = c.WTTypeId 
   where d.DocEntry = @DocEntry
     and d.ObjType = @objType 
     and c.WTType = @imposto

  select @vl_imposto_rpc = SUM(a.WTAmnt)
    from RPC5 a
   inner join ORPC d
      on a.AbsEntry = d.DocEntry 
   inner join OWHT b
      on b.WTCode = a.WTCode 
   inner join OWTT c
      on b.WTTypeId = c.WTTypeId 
   where d.DocEntry = @DocEntry
     and d.ObjType = @objType 
     and c.WTType = @imposto




  select @vl_imposto_dln = SUM(a.WTAmnt)
    from dln5 a
   inner join Odln d
      on a.AbsEntry = d.DocEntry 
   inner join OWHT b
      on b.WTCode = a.WTCode 
   inner join OWTT c
      on b.WTTypeId = c.WTTypeId 
   where d.DocEntry = @DocEntry
     and d.ObjType = @objType 
     and c.WTType = @imposto

  select @vl_imposto_rdn = SUM(a.WTAmnt)
    from rdn5 a
   inner join Ordn d
      on a.AbsEntry = d.DocEntry 
   inner join OWHT b
      on b.WTCode = a.WTCode 
   inner join OWTT c
      on b.WTTypeId = c.WTTypeId 
   where d.DocEntry = @DocEntry
     and d.ObjType = @objType 
     and c.WTType = @imposto

  select @vl_imposto_inv = SUM(a.WTAmnt)
    from inv5 a
   inner join Oinv d
      on a.AbsEntry = d.DocEntry 
   inner join OWHT b
      on b.WTCode = a.WTCode 
   inner join OWTT c
      on b.WTTypeId = c.WTTypeId 
   where d.DocEntry = @DocEntry
     and d.ObjType = @objType 
     and c.WTType = @imposto

  select @vl_imposto_rin = SUM(a.WTAmnt)
    from RIN5 a
   inner join ORIN d
      on a.AbsEntry = d.DocEntry 
   inner join OWHT b
      on b.WTCode = a.WTCode 
   inner join OWTT c
      on b.WTTypeId = c.WTTypeId 
   where d.DocEntry = @DocEntry
     and d.ObjType = @objType 
     and c.WTType = @imposto

  select @vl_imposto = ISNULL(@vl_imposto_pdn, 0)
                     + ISNULL(@vl_imposto_pch, 0)
                     + ISNULL(@vl_imposto_rpd, 0)
                     + ISNULL(@vl_imposto_rpc, 0)
                     
                     + ISNULL(@vl_imposto_dln, 0)
                     + ISNULL(@vl_imposto_rdn, 0)
                     + ISNULL(@vl_imposto_inv, 0)
                     + ISNULL(@vl_imposto_rin, 0)
  
  return @vl_imposto


end