-- select dbo.SP_CVA_BaseImpostoDoDocumento('13', 28, 'ICMS-ST-ALIQ')
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'FN_CVA_BaseImpostoDoDocumento')
	DROP FUNCTION FN_CVA_BaseImpostoDoDocumento
GO
CREATE FUNCTION [dbo].[FN_CVA_BaseImpostoDoDocumento] (
  @ObjType varchar(30)
, @DocEntry int
, @Imposto varchar(30)

)
returns decimal(19, 9)

as
begin

  --set @imposto = '%' + rtrim(ltrim(replace(isnull(@imposto, ''), '%', ''))) + '%'

  declare @vl_base_Imposto       decimal(19, 9)
  
  declare @vl_base_Imposto_pdn   decimal(19, 9)
  declare @vl_base_Imposto_pch   decimal(19, 9)
  declare @vl_base_Imposto_rpd   decimal(19, 9)
  declare @vl_base_Imposto_rpc   decimal(19, 9)
  declare @vl_base_Imposto_por   decimal(19, 9)
  declare @vl_base_Imposto_dln   decimal(19, 9) 
  declare @vl_base_Imposto_rdn   decimal(19, 9)
  declare @vl_base_Imposto_inv   decimal(19, 9)
  declare @vl_base_Imposto_rin   decimal(19, 9)
  declare @vl_base_Imposto_rdr   decimal(19, 9)
 
  declare @vl_base_Imposto_dpi   decimal(19, 9)
  
  select @vl_base_Imposto     = 0
       , @vl_base_Imposto_pdn = 0
       , @vl_base_Imposto_pch = 0
       , @vl_base_Imposto_rpd = 0
       , @vl_base_Imposto_dpi = 0
       , @vl_base_Imposto_rpc = 0
       , @vl_base_Imposto_dln = 0
       , @vl_base_Imposto_rdn = 0
       , @vl_base_Imposto_inv = 0
       , @vl_base_Imposto_rin = 0 
       , @vl_base_Imposto_por = 0 
       , @vl_base_Imposto_rdr = 0 
  
  select @vl_base_Imposto_dpi = sum(a.BaseSum)
    from DPI4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto
     
  select @vl_base_Imposto_pdn = sum(a.BaseSum)
    from PDN4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto

  select @vl_base_Imposto_pch = sum(a.BaseSum)
    from PCH4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto

  select @vl_base_Imposto_rpd = sum(a.BaseSum)
    from RPD4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto

  select @vl_base_Imposto_rpc = sum(a.BaseSum)
    from RPC4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto


  select @vl_base_Imposto_por = sum(a.BaseSum)
    from POR4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto

  select @vl_base_Imposto_dln = sum(a.BaseSum)
    from DLN4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto

  select @vl_base_Imposto_rdn = sum(a.BaseSum)
    from rdn4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto

  select @vl_base_Imposto_inv = sum(a.BaseSum)
    from INV4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto

  select @vl_base_Imposto_rin = sum(a.BaseSum)
    from RIN4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto


  select @vl_base_Imposto_rdr = sum(a.BaseSum)
    from RDR4 a
   inner join OSTT b
      on b.AbsId = a.staType 
   where a.DocEntry = @DocEntry
     and a.ObjectType = @ObjType 
     and b.Name = @Imposto
       
  select @vl_base_Imposto = ISNULL(@vl_base_Imposto_pdn, 0)
                     + ISNULL(@vl_base_Imposto_pch, 0)
                     + ISNULL(@vl_base_Imposto_rpd, 0)
                     + ISNULL(@vl_base_Imposto_rpc, 0)
                     + ISNULL(@vl_base_Imposto_dln, 0)
                     + ISNULL(@vl_base_Imposto_por, 0)
                     + ISNULL(@vl_base_Imposto_rdn, 0)
                     + ISNULL(@vl_base_Imposto_inv, 0)
                     + ISNULL(@vl_base_Imposto_rin, 0)
                     + ISNULL(@vl_base_Imposto_dpi, 0)
                     + ISNULL(@vl_base_Imposto_rdr, 0)
  
  return @vl_base_Imposto

end











