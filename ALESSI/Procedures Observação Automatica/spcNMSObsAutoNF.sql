-- declare @texto varchar(max); exec spcNMSObsAutoNF 'ORDR', 11790, @texto output; select @texto  
create proc spcNMSObsAutoNF
  @tablename varchar(30)  
, @docentry int  
, @texto       varchar(max) output  
as  

  -- Variáveis disponíveis para utilização na observação:  
  --@vl_nota  
  --@vl_ii  
  --@vl_cofins  
  --@vl_pis  
  --@vl_iss  
  --@vl_icms  
  --@vl_st_icms  
  --@vl_base_st_icms  
  --@vl_ipi  
  --@vl_rcofins  
  --@vl_rcsll  
  --@vl_rinss  
  --@vl_rirrf  
  --@vl_rissf  
  --@vl_rpis  
  --@vl_rpcc  
  --@origem  
  --@vl_base_dif_icms  
  --@vl_dif_icms  
  --@vl_fundo_pobreza  
  --@pr_fundo_pobreza  
  --@vl_mercadorias  

  create table #verificados (code varchar(72) collate database_default null)  
  
  create table #nota (  
    cardcode       varchar(30)  collate database_default null  
  , cardname       varchar(200) collate database_default null  
  , groupcode      varchar(200) collate database_default null  
  , bplid          int          null  
  , Valor          money        null  
  , II             money        null  
  , COFINS         money        null  
  , PIS            money        null  
  , ISS            money        null  
  , ICMS           money        null  
  , ICMSST         money        null  
  , BASEST         money        null  
  , IPI            money        null  
  , ICMS_Dif       money        null  
  , RCOFINS        money        null  
  , RCSLL          money        null  
  , RINSS          money        null  
  , RIRRF          money        null  
  , RISSF          money        null  
  , RPIS           money        null  
  , RPCC           money        null  
  , state          char(2)      collate database_default null  
  , city           varchar(200) collate database_default null  
  , cnpj           varchar(20)  collate database_default null  
  , inscricao_est  varchar(20)  collate database_default null  
  , base_icms_dif  money        null  
  , vl_icms_dif    money        null  
  , vl_mercadorias money        null  
  , suframa        varchar(200) collate database_default null

  )  
  create table #notaitem (  
    linenum    int null  
  , itemcode   varchar(30)  collate database_default null  
  , itemname   varchar(250) collate database_default null
  , quantity   decimal(19, 4)                        null
  , price      decimal(19, 4)                        null
  , total      decimal(19, 4)                        null
  , ncm        varchar(30)  collate database_default null  
  , usage      varchar(200) collate database_default null  
  , cfop       varchar(12)  collate database_default null  
  , taxcode    varchar(30)  collate database_default null  
  , baseentry  int null  
  , baseline   int null  
  , basetype   int null  
  , itmsgrpnam varchar(200) collate database_default null  
  )  

  create table #notaparcela (
    parcela    int            null
  , vencimento smalldatetime  null
  , valor      decimal(19, 4) null
  )
-- select top 10 * from inv6
 
    
  create table #serial (docentry int null, serial int null, docdate smalldatetime null)  
    
  declare @sql         varchar(max)  
  declare @U_OINV      char(1)  
  declare @U_OQUT      char(1)  
  declare @U_ORDR      char(1)  
  declare @U_ODLN      char(1)  
  declare @U_ORIN      char(1)  
  declare @U_ORDN      char(1)  
  declare @U_OINV_F    char(1)  
  declare @U_OPCH      char(1)  
  declare @U_OPQT      char(1)  
  declare @U_OPOR      char(1)  
  declare @U_OPDN      char(1)  
  declare @U_ORPC      char(1)  
  declare @U_ORPD      char(1)  
  declare @U_OPCH_F    char(1)  
  declare @c_Valor     varchar(50)  
  declare @c_II        varchar(50)  
  declare @c_COFINS    varchar(50)  
  declare @c_PIS       varchar(50)  
  declare @c_ISS       varchar(50)  
  declare @c_ICMS      varchar(50)  
  declare @c_ICMSST    varchar(50)  
  declare @c_BASEST    varchar(50)  
  declare @c_IPI       varchar(50)  
  declare @c_RCOFINS   varchar(50)  
  declare @c_RCSLL     varchar(50)  
  declare @c_RINSS     varchar(50)  
  declare @c_RIRRF     varchar(50)  
  declare @c_RISSF     varchar(50)  
  declare @c_RPIS      varchar(50)  
  declare @c_RPCC      varchar(50)  
  declare @origem      varchar(1000)  
  declare @vBaseEntry  int  
  declare @vBaseType   int  
  declare @vDocEntry   int  
  declare @vSerial     int  
  declare @vDocDate    smalldatetime  
  declare @tabela      varchar(10)  
  declare @c_BICMSD    varchar(50)  
  declare @c_VICMSD    varchar(50)  
  declare @vl_fundo_pobreza money  
  declare @c_vl_fundo_pobreza varchar(50)  
  declare @c_pr_fundo_pobreza varchar(50)  
  declare @c_vl_mercadorias varchar(50)  
  declare @pr_pis_suframa decimal(19, 2)  
  declare @pr_cofins_suframa decimal(19, 2)  
  declare @c_pr_suframa_pis varchar(50)  
  declare @c_vl_suframa_pis varchar(50)  
  declare @c_pr_suframa_cofins varchar(50)  
  declare @c_vl_suframa_cofins varchar(50)  
  declare @suframa varchar(200)
    
  set @pr_pis_suframa = 1.65  
    
  set @pr_cofins_suframa = 3  
    
  select @U_OINV   = case when @tablename = 'OINV' then 'S' else 'N' end  
       , @U_OQUT   = CASE when @tablename = 'OQUT' then 'S' else 'N' end  
       , @U_ORDR   = case when @tablename = 'ORDR' then 'S' else 'N' end  
       , @U_ODLN   = case when @tablename = 'ODLN' then 'S' else 'N' end  
       , @U_ORIN   = case when @tablename = 'ORIN' then 'S' else 'N' end  
       , @U_ORDN   = case when @tablename = 'ORDN' then 'S' else 'N' end  
       , @U_OINV_F = 'N' -- case when @tablename = 'OINV' then 'S' else 'N' end  
       , @U_OPCH   = case when @tablename = 'OPCH' then 'S' else 'N' end  
       , @U_OPQT   = case when @tablename = 'OPQT' then 'S' else 'N' end  
       , @U_OPOR   = case when @tablename = 'OPOR' then 'S' else 'N' end  
       , @U_OPDN   = case when @tablename = 'OPDN' then 'S' else 'N' end  
       , @U_ORPC   = case when @tablename = 'ORPC' then 'S' else 'N' end  
       , @U_ORPD   = case when @tablename = 'ORPD' then 'S' else 'N' end  
       , @U_OPCH_F = 'N' -- case when @tablename = 'OINV' then 'S' else 'N' end  
   
 
  select @sql = '  
  insert into #nota (cardcode  
                   , cardname  
                   , groupcode  
                   , bplid  
                   , Valor, II, COFINS, PIS, ISS, ICMS, ICMSST, BASEST, IPI, ICMS_Dif  
                   , RCOFINS, RCSLL, RINSS, RIRRF, RISSF, RPIS, RPCC  
                   , state, city, cnpj, inscricao_est  
                   , base_icms_dif, vl_icms_dif, vl_mercadorias, suframa)  
    select a.cardcode  
         , b.cardname  
         , d.groupname  
         , a.bplid  
         , a.DocTotal  
         , dbo.fncNMSImpostoDoDocumento(a.ObjType, a.DocEntry, ''II'')   
         , dbo.fncNMSImpostoDoDocumento(a.ObjType, a.DocEntry, ''COFINS'')  
         , dbo.fncNMSImpostoDoDocumento(a.ObjType, a.DocEntry, ''PIS'')   
         , dbo.fncNMSImpostoDoDocumento(a.ObjType, a.DocEntry, ''ISS'')  
         , dbo.fncNMSImpostoDoDocumento(a.ObjType, a.DocEntry, ''ICMS'')  
         , dbo.fncNMSImpostoDoDocumento(a.ObjType, a.DocEntry, ''ICMS-ST-ALIQ'')   
         , dbo.fncNMSBaseImpostoDoDocumento(a.ObjType, a.DocEntry, ''ICMS-ST-ALIQ'')   
         , dbo.fncNMSImpostoDoDocumento(a.ObjType, a.DocEntry, ''IPI'')  
           
         , dbo.fncNMSImpostoDoDocumento(a.ObjType, a.DocEntry, ''ICMS Dif'')   
           
         , dbo.fncNMSImpostoRetidoDoDocumento(a.ObjType, a.DocEntry, ''COFINS'')  
         , dbo.fncNMSImpostoRetidoDoDocumento(a.ObjType, a.DocEntry, ''CSLL'')  
         , dbo.fncNMSImpostoRetidoDoDocumento(a.ObjType, a.DocEntry, ''INSS'')  
         , dbo.fncNMSImpostoRetidoDoDocumento(a.ObjType, a.DocEntry, ''IRRF'')  
         , dbo.fncNMSImpostoRetidoDoDocumento(a.ObjType, a.DocEntry, ''ISSF'')  
         , dbo.fncNMSImpostoRetidoDoDocumento(a.ObjType, a.DocEntry, ''PIS'')  
         , dbo.fncNMSImpostoRetidoDoDocumento(a.ObjType, a.DocEntry, ''PCC'')  
         , c.StateS  
         , c.CityS  
         , dbo.fncNMSMascaraCNPJ(rtrim(ltrim(isnull(c.TaxId0, ''''))))  
         , c.taxid1  
         , a.U_nmsbaseicmsdif  
         , a.U_nmsvlicmsdif  
         , a.DocTotal - a.VatSum  
         , isnull(c.TaxId8, '''')
      from ' + RTRIM(@tablename) + ' a  
     inner join OCRD b  
        on a.cardcode = b.cardcode  
     inner join ' + substring(RTRIM(@tablename), 2, 3) + '12' + ' c  
        on a.docentry = c.docentry  
     inner join OCRG d  
        on b.groupcode = d.groupcode  
     where a.docentry = ' + RTRIM(@docentry) + '  
       and a.canceled = ''N'';   
       
  
  insert into #notaitem (linenum, itemcode, ncm, usage, cfop, taxcode  
                       , baseentry, baseline, basetype, itmsgrpnam, itemname, quantity, price, total)  
    select a.linenum, a.itemcode, c.NCMCode, d.Usage, a.CFOPCode, a.taxcode  
         , a.baseentry, a.baseline, a.basetype, e.itmsgrpnam, a.dscription, a.quantity, a.price, a.linetotal
      from ' + substring(RTRIM(@tablename), 2, 3) + '1 a  
     inner join OITM b  
        on a.itemcode = b.ItemCode   
      left join ONCM c  
        on b.NCMCode = c.AbsEntry   
      left join OUSG d  
        on a.usage = d.id  
      left join OITB e  
        on b.itmsgrpcod = e.itmsgrpcod  
     where a.docentry = ' + RTRIM(@docentry) + '; 
     
  insert into #notaparcela (parcela, vencimento, valor)
    select a.InstlmntID, a.duedate, a.instotal
      from ' + substring(rtrim(@tablename), 2, 3) + '6 a
     where a.docentry = ' + rtrim(@docentry) + ';'  
       
     print @sql  
     
  exec(@sql)  

  select * from #notaitem
    
  exec spcNMSObsAutoNFRecursive @tablename, '', @texto output  
    
  --declare @c_suframa_pr_pis_suframa varchar(50)  
  --declare @c_suframa_vl_pis_suframa varchar(50)  
  --declare @c_suframa_pr_cofins_suframa varchar(50)  
  --declare @c_suframa_vl_cofins_suframa varchar(50)  
      
  select @c_Valor    = rtrim(ltrim(replace(replace(replace(convert(char(15), ISNULL(valor, 0), 10),'.',';'),',','.'),';',',')))   
       , @c_II       = rtrim(ltrim(replace(replace(replace(convert(char(15), ISNULL(ii, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_COFINS   = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(COFINS, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_PIS      = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(PIS, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_ISS      = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(ISS, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_ICMS     = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(ICMS, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_ICMSST   = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(ICMSST, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_BASEST   = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(BASEST, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_IPI      = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(IPI, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_RCOFINS  = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(RCOFINS, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_RCSLL    = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(RCSLL, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_RINSS    = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(RINSS, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_RIRRF    = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(RIRRF, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_RISSF    = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(RISSF, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_RPIS     = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(RPIS, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_RPCC     = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(RPCC, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_BICMSD   = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(base_icms_dif, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_VICMSD   = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(vl_icms_dif, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_vl_fundo_pobreza   = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(Convert(money, BASEST * .01), 0), 10),'.',';'),',','.'),';',',')))  
       , @c_pr_fundo_pobreza = '1'  
       , @c_vl_mercadorias = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(vl_mercadorias, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_pr_suframa_pis = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(@pr_pis_suframa, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_pr_suframa_cofins = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(@pr_cofins_suframa, 0), 10),'.',';'),',','.'),';',',')))  
       , @c_vl_suframa_pis = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(Convert(money, vl_mercadorias * (@pr_pis_suframa / 100)), 0), 10),'.',';'),',','.'),';',',')))  
       , @c_vl_suframa_cofins = rtrim(ltrim(replace(replace(replace(convert(char(15), isnull(Convert(money, vl_mercadorias * (@pr_cofins_suframa / 100)), 0), 10),'.',';'),',','.'),';',',')))  
       , @suframa = rtrim(ltrim(isnull(suframa, '')))
    from #nota   
    
  set @origem = ''  
    
  set @sql = ''  
  
  declare cp1 cursor local fast_forward for  
    select distinct baseentry, basetype  
      from #notaitem  
        
  open cp1  
    
  while 1 = 1  
  begin  
    fetch next from cp1 into @vbaseentry, @vbasetype  
    if @@FETCH_STATUS <> 0 break  
      
    set @tabela = ''  
      
    set @tabela = case @vBaseType  
      when '13' then 'OINV' -- nota fiscal de saída  
      when '23' then 'OQUT' -- cotação  
      when '17' then 'ORDR' -- pedido   
      when '15' then 'ODLN' -- entrega   
      when '14' then 'ORIN' -- Dev. nota saída  
      when '16' then 'ORDN' -- Dev. de enrega  
      when '18' then 'OPCH' -- nota de entrada  
      when '540000006' then 'OPQT' -- cotação compra  
      when '22' then 'OPOR' -- pedido de compra  
      when '20' then 'OPDN' -- recebimento de mercadorias  
      when '19' then 'ORPC' -- devolução nf entrada  
      when '21' then 'ORPD' -- devolução de recebimento  
      else '' end  
        
    if @tabela <> ''  
    begin  
      
      select @sql = @sql + 'insert into #serial (docentry, serial, docdate)  
        select docentry, serial, docdate  
          from ' + RTRIM(@tabela) + '  
         where docentry = ' + RTRIM(@vBaseEntry) + ';'  
      
    end  
      
  end  
  close cp1  
  deallocate cp1   
  
  if @sql <> ''  
    exec(@sql)  
  
  declare cp1 cursor local fast_forward read_only for  
    select distinct docentry, serial, docdate  
      from #serial  
     order by docdate, serial  
       
 open cp1  
   
  while 1 = 1  
  begin  
    fetch next from cp1 into @vdocentry, @vserial, @vdocdate  
    if @@FETCH_STATUS <> 0 break  
      
    if @origem = ''  
      set @origem = RTRIM(@vserial) + ' de ' + CONVERT(char(10), @vdocdate, 103)  
    else  
      set @origem = @origem + ', ' + RTRIM(@vserial) + ' de ' + CONVERT(char(10), @vdocdate, 103)  
      
  end  
  close cp1  
  deallocate cp1  

  set @texto = REPLACE(@texto, '@vl_nota'          , isnull(@c_Valor, '?'))  
  set @texto = REPLACE(@texto, '@vl_ii'            , isnull(@c_II, '?'))  
  set @texto = REPLACE(@texto, '@vl_cofins'        , isnull(@c_COFINS, '?'))  
  set @texto = REPLACE(@texto, '@vl_pis'           , isnull(@c_PIS, '?'))  
  set @texto = REPLACE(@texto, '@vl_iss'     , isnull(@c_ISS, '?'))  
  set @texto = REPLACE(@texto, '@vl_icms'          , isnull(@c_ICMS, '?'))  
  set @texto = REPLACE(@texto, '@vl_st_icms'       , isnull(@c_ICMSST, '?'))  
  set @texto = REPLACE(@texto, '@vl_base_st_icms'  , isnull(@c_BASEST, '?'))  
  set @texto = REPLACE(@texto, '@vl_ipi'           , isnull(@c_IPI, '?'))  
  set @texto = REPLACE(@texto, '@vl_rcofins'       , isnull(@c_RCOFINS, '?'))  
  set @texto = REPLACE(@texto, '@vl_rcsll'         , isnull(@c_RCSLL, '?'))  
  set @texto = REPLACE(@texto, '@vl_rinss'         , isnull(@c_RINSS, '?'))  
  set @texto = REPLACE(@texto, '@vl_rirrf'         , isnull(@c_RIRRF, '?'))  
  set @texto = REPLACE(@texto, '@vl_rissf'         , isnull(@c_RISSF, '?'))  
  set @texto = REPLACE(@texto, '@vl_rpis'          , isnull(@c_RPIS, '?'))  
  set @texto = REPLACE(@texto, '@vl_rpcc'          , isnull(@c_RPCC, '?'))  
  set @texto = REPLACE(@texto, '@origem'           , isnull(@origem, '?'))  
  set @texto = REPLACE(@texto, '@vl_base_dif_icms' , isnull(@c_BICMSD, '?'))  
  set @texto = REPLACE(@texto, '@vl_dif_icms'      , isnull(@c_VICMSD, '?'))  
  set @texto = REPLACE(@texto, '@vl_fundo_pobreza' , isnull(@c_vl_fundo_pobreza, '?'))  
  set @texto = REPLACE(@texto, '@pr_fundo_pobreza' , isnull(@c_pr_fundo_pobreza, '?'))  
  set @texto = REPLACE(@texto, '@vl_mercadorias' , isnull(@c_vl_mercadorias, '?'))  
  set @texto = REPLACE(@texto, '@pr_suframa_pis' , isnull(@c_pr_suframa_pis, '?'))  
  set @texto = REPLACE(@texto, '@vl_suframa_pis' , isnull(@c_vl_suframa_pis, '?'))  
  set @texto = REPLACE(@texto, '@pr_suframa_cofins' , isnull(@c_pr_suframa_cofins, '?'))  
  set @texto = REPLACE(@texto, '@vl_suframa_cofins' , isnull(@c_vl_suframa_cofins, '?'))  
  set @texto = REPLACE(@texto, '@pin_suframa' , isnull(@suframa, '?'))  
    
  declare @obs_servico varchar(max)
  declare @vlinenum  int
  declare @vitemcode varchar(72)
  declare @vitemname varchar(250)
  declare @vquantity decimal(19, 2)
  declare @vprice    decimal(19, 2)
  declare @vtotal    decimal(19, 2)
  declare @vparcela  int
  declare @vvencimento smalldatetime
  declare @vvalor    decimal(19, 2) 
  declare @servico   bit

  set @servico = 0

  if @tablename = 'OINV'
  begin

    declare cp1 cursor local fast_forward read_only for
      select a.linenum, a.itemcode, a.itemname, a.quantity, a.price, a.total
        from #notaitem a
       inner join oitm b
          on a.itemcode = b.itemcode
       where b.itemclass = 1
       order by a.linenum

    open cp1

    while 1 = 1
    begin
      fetch next from cp1 into @vlinenum, @vitemcode, @vitemname, @vquantity, @vprice, @vtotal
      if @@fetch_status <> 0 break

      set @servico = 1

      if isnull(@obs_servico, '') = ''
        set @obs_servico = rtrim(@vItemcode) + ' ' + rtrim(@vitemname) + ' / Qtd.: ' + replace(rtrim(@vquantity), '.', ',') + ' / Preço: R$ ' + replace(rtrim(@vprice), '.', ',') + ' / Total: R$ ' + replace(rtrim(@vtotal), '.', ',')
      else
        set @obs_servico = rtrim(@obs_servico) + char(13) + char(10) + 
                           rtrim(@vItemcode) + ' ' + rtrim(@vitemname) + ' / Qtd.: ' + replace(rtrim(@vquantity), '.', ',') + ' / Preço: R$ ' + replace(rtrim(@vprice), '.', ',') + ' / Total: R$ ' + replace(rtrim(@vtotal), '.', ',')

    end
    close cp1
    deallocate cp1

    if @servico = 1
    begin

      select @obs_servico = @obs_servico + char(13) + char(10) + char(13) + char(10)
                          + 'Parcelas' + char(13) + char(10) 

      declare cp2 cursor local fast_forward read_only for
        select parcela, vencimento, valor
          from #notaparcela
         order by parcela
      open cp2
      while 1 = 1
      begin
        fetch next from cp2 into @vparcela, @vvencimento, @vvalor
        if @@fetch_status <> 0 break

        select @obs_servico = @obs_servico + char(13) + char(10)
                      + 'Parcela ' + rtrim(@vparcela) + ' / Vencimento: ' + convert(char(10), @vvencimento, 103) + ' / Valor: R$ ' + replace(rtrim(@vvalor), '.', ',')

      end
      close cp2
      deallocate cp2

  	  set @texto = rtrim(ltrim(isnull(@obs_servico, ''))) + char(13) + char(10) + char(13) + char(10) + isnull(@texto, '')

    end

  end
  
  select @texto = isnull(@texto, '')  
  
  
  

