-- declare @a varchar(max); exec spcNMSObsAutoNFRecursive 'oinv', '', @a output; select @a
create proc spcNMSObsAutoNFRecursive
  @tablename varchar(100)
, @parent varchar(72)
, @texto varchar(max) output
as

  declare @sql varchar(max)
  declare @vcode       varchar(30)   
  declare @vtexto      varchar(max)  
  declare @vlineid     int 
  declare @vitemcode   varchar(80)   
  declare @vusage      varchar(80)   
  declare @vtaxcode    varchar(32)   
  declare @vcfop       int 
  declare @vncm        varchar(80)   
  declare @vcardcode   varchar(60)   
  declare @vgroupcode   varchar(200)
  declare @vstate      char(4)       
  declare @vcity       varchar(200)  
  declare @vbplid      int
  declare @vsql        varchar(max)
  declare @vitmsgrpnam varchar(200)
  declare @inserir     bit
  declare @vcodepai    varchar(72)

  set @texto = isnull(@texto, '')

  set @parent = isnull(@parent, 0)

  create table #nmsobsaut1 (
    code       varchar(30)  collate database_default null
  , codepai    varchar(30)  collate database_default null
  , texto      varchar(max) collate database_default null
  , lineid     int null
  , itemcode   varchar(80)  collate database_default null
  , usage      varchar(80)  collate database_default null
  , taxcode    varchar(32)  collate database_default null
  , cfop       int null
  , ncm        varchar(80)  collate database_default null
  , cardcode   varchar(60)  collate database_default null
  , groupcode   varchar(200) collate database_default null
  , state      char(4)      collate database_default null
  , city       varchar(200) collate database_default null
  , bplid      int null
  , sql        varchar(max) collate database_default null
  , itmsgrpnam varchar(100) collate database_default null
  )

select @sql = '
  insert into #nmsobsaut1 (code, codepai, texto, lineid, itemcode, usage, taxcode, cfop, ncm, cardcode, groupcode, state, city, bplid, sql, itmsgrpnam)
    select a.Code, b.Code, b.U_texto, a.LineId, a.U_itemcode, a.U_usage, a.U_taxcode, a.U_cfop, a.U_ncm, a.U_cardcode, a.U_groupnum, a.U_state, a.U_city, a.U_bplid, a.U_sql, a.U_itmsgrpnam
      from [@NMSOBSAUTO] b
      left join [@NMSOBSAUT1] a
        on a.code = b.code
       and isnull(a.U_status, ''A'') = ''A''
     where b.U_status = ''A''
       and a.U_itemcode is null
       and a.u_usage    is null
       and a.U_taxcode  is null
       and a.U_cfop     is null
       and a.U_ncm      is null
       and a.U_cardcode is null
       and a.U_state    is null
       and a.U_city     is null
       and a.U_groupnum  is null
       and a.U_itmsgrpnam is null
       and isnull(b.U_parent, '''') = ''' + rtrim(@parent) + '''
       and b.U_' + @tablename + ' = ''S'' 
     union all
    select a.Code, b.Code, b.U_texto, a.LineId, a.U_itemcode, a.U_usage, a.U_taxcode, a.U_cfop, a.U_ncm, a.U_cardcode, a.U_groupnum, a.U_state, a.U_city, a.U_bplid, a.U_sql, a.U_itmsgrpnam
      from [@NMSOBSAUTO] b
      left join [@NMSOBSAUT1] a
        on a.code = b.code
       and isnull(a.U_status, ''A'') = ''A''
     where b.U_status = ''A''
       and a.U_itemcode in (select distinct itemcode from #notaitem)
       and isnull(b.U_parent, '''') = ''' + rtrim(@parent) + '''
       and b.U_' + @tablename + ' = ''S'' 
     union all
    select a.Code, b.Code, b.U_texto, a.LineId, a.U_itemcode, a.U_usage, a.U_taxcode, a.U_cfop, a.U_ncm, a.U_cardcode, a.U_groupnum, a.U_state, a.U_city, a.U_bplid, a.U_sql, a.U_itmsgrpnam
      from [@NMSOBSAUTO] b
      left join [@NMSOBSAUT1] a
        on a.code = b.code
       and isnull(a.U_status, ''A'') = ''A''
     where b.U_status = ''A''
       and a.u_usage    in (select distinct usage from #notaitem)
       and isnull(b.U_parent, '''') = ''' + rtrim(@parent) + '''
       and b.U_' + @tablename + ' = ''S''  
     union all
    select a.Code, b.Code, b.U_texto, a.LineId, a.U_itemcode, a.U_usage, a.U_taxcode, a.U_cfop, a.U_ncm, a.U_cardcode, a.U_groupnum, a.U_state, a.U_city, a.U_bplid, a.U_sql, a.U_itmsgrpnam
      from [@NMSOBSAUTO] b
      left join [@NMSOBSAUT1] a
        on a.code = b.code
       and isnull(a.U_status, ''A'') = ''A''
     where b.U_status = ''A''
       and a.u_taxcode  in (select distinct taxcode from #notaitem)
       and isnull(b.U_parent, '''') = ''' + rtrim(@parent) + '''
       and b.U_' + @tablename + ' = ''S'' 
     union all
    select a.Code, b.Code, b.U_texto, a.LineId, a.U_itemcode, a.U_usage, a.U_taxcode, a.U_cfop, a.U_ncm, a.U_cardcode, a.U_groupnum, a.U_state, a.U_city, a.U_bplid, a.U_sql, a.U_itmsgrpnam
      from [@NMSOBSAUTO] b
      left join [@NMSOBSAUT1] a
        on a.code = b.code
       and isnull(a.U_status, ''A'') = ''A''
     where b.U_status = ''A''
       and a.U_cfop     in (select distinct cfop from #notaitem)
       and isnull(b.U_parent, '''') = ''' + rtrim(@parent) + '''
       and b.U_' + @tablename + ' = ''S'' 
     union all
    select a.Code, b.Code, b.U_texto, a.LineId, a.U_itemcode, a.U_usage, a.U_taxcode, a.U_cfop, a.U_ncm, a.U_cardcode, a.U_groupnum, a.U_state, a.U_city, a.U_bplid, a.U_sql, a.U_itmsgrpnam
      from [@NMSOBSAUTO] b
      left join [@NMSOBSAUT1] a
        on a.code = b.code
       and isnull(a.U_status, ''A'') = ''A''
     where b.U_status = ''A''
       and a.U_ncm      in (select distinct ncm from #notaitem)
       and isnull(b.U_parent, '''') = ''' + rtrim(@parent) + '''
       and b.U_' + @tablename + ' = ''S'' 
     union all
    select a.Code, b.Code, b.U_texto, a.LineId, a.U_itemcode, a.U_usage, a.U_taxcode, a.U_cfop, a.U_ncm, a.U_cardcode, a.U_groupnum, a.U_state, a.U_city, a.U_bplid, a.U_sql, a.U_itmsgrpnam
      from [@NMSOBSAUTO] b
      left join [@NMSOBSAUT1] a
        on a.code = b.code
       and isnull(a.U_status, ''A'') = ''A''
     where b.U_status = ''A''
       and a.U_itmsgrpnam      in (select distinct itmsgrpnam from #notaitem)
       and isnull(b.U_parent, '''') = ''' + rtrim(@parent) + '''
       and b.U_' + @tablename + ' = ''S'' 
     union all
    select a.Code, b.Code, b.U_texto, a.LineId, a.U_itemcode, a.U_usage, a.U_taxcode, a.U_cfop, a.U_ncm, a.U_cardcode, a.U_groupnum, a.U_state, a.U_city, a.U_bplid, a.U_sql, a.U_itmsgrpnam
      from [@NMSOBSAUTO] b
      left join [@NMSOBSAUT1] a
        on a.code = b.code
       and isnull(a.U_status, ''A'') = ''A''
     where b.U_status = ''A''
       and (
            a.u_cardcode in (select cardcode from #nota)
         or a.U_groupnum in (select groupcode from #nota)
         or a.u_state    in (select state from #nota)
         or a.u_city     in (select city from #nota)
           )
       and isnull(b.U_parent, '''') = ''' + rtrim(@parent) + '''
       and b.U_' + @tablename + ' = ''S'' 
       '
   
  print @sql

  exec(@sql)
 
  --select * from #nmsobsaut1
  
  declare cp1 cursor local fast_forward read_only for
    select distinct code, codepai, texto, lineid, itemcode, usage, taxcode, cfop, ncm, cardcode, groupcode, state, city, bplid, sql, itmsgrpnam
      from #nmsobsaut1
     where codepai not in (select code from #verificados)
     
  open cp1
  
  while 1 = 1
  begin
    fetch next from cp1 into @vcode, @vcodepai, @vtexto, @vlineid, @vitemcode, @vusage, @vtaxcode, @vcfop, @vncm, @vcardcode, @vgroupcode, @vstate, @vcity, @vbplid, @vsql, @vitmsgrpnam
    if @@FETCH_STATUS <> 0 break
    
    set @inserir = 0
    
    if RTRIM(ltrim(ISNULL(@vitemcode, ''))) = '' set @vitemcode = null
    if rtrim(ltrim(ISNULL(@vusage, ''))) = '' set @vusage = null
    if RTRIM(ltrim(ISNULL(@vtaxcode, ''))) = ''  set @vtaxcode = null
    if ISNULL(@vcfop, 0) = 0 set @vcfop = null
    if RTRIM(ltrim(ISNULL(@vncm, ''))) = '' set @vncm = null
    if RTRIM(ltrim(ISNULL(@vcardcode, ''))) = '' set @vcardcode = null
    if RTRIM(ltrim(ISNULL(@vstate, ''))) = '' set @vstate = null
    if RTRIM(ltrim(ISNULL(@vcity, ''))) = '' set @vcity = null
    if ISNULL(@vbplid, 0) = 0 set @vbplid = null
    if rtrim(ltrim(isnull(@vgroupcode, ''))) = '' set @vgroupcode = null
    if rtrim(ltrim(isnull(@vitmsgrpnam, ''))) = '' set @vitmsgrpnam = NULL
    
    --select @vcode, @vtexto, @vlineid, @vitemcode, @vusage, @vtaxcode, @vcfop, @vncm, @vcardcode, @vstate, @vcity, @vsql
    
    --if @vitemcode is null and @vusage is null and @vtaxcode is null and @vcfop is null and @vncm is null and @vcardcode is null and @vgroupcode is null and @vstate is null and @vcity is null and @vbplid is null and @vitmsgrpnam is null
    --  set @inserir = 1
      
    --else    
    if (@vitemcode is not null or @vusage is not null or @vtaxcode is not null or @vcfop is not null or @vncm is not null or @vitmsgrpnam is not null or     
    @vcardcode is not null or @vgroupcode is not null or @vstate is not null or @vcity is not null or @vbplid is not null)
       and exists (select 0
                     from #nota a, #notaitem b
                    where b.itemcode = ISNULL(@vitemcode, itemcode)
                      and b.usage    = ISNULL(@vusage, usage)
                      and b.taxcode  = ISNULL(@vtaxcode, taxcode)
                      and b.cfop     = ISNULL(@vcfop, cfop)
                      and b.ncm      = ISNULL(@vncm, ncm)
                      and b.itmsgrpnam = isnull(@vitmsgrpnam, itmsgrpnam)
                      and a.cardcode = ISNULL(@vcardcode, cardcode)
                      and a.groupcode = isnull(@vgroupcode, groupcode)
                      and a.state    = ISNULL(@vstate, state)
                      and a.city     = ISNULL(@vcity, city)
                      and a.bplid    = ISNULL(@vbplid, bplid)
                      
                   )
      set @inserir = 1

      print @inserir
      print @vcode

    insert into #verificados (code)
      select @vcodepai

    if @inserir = 1 or @vcode is null
    begin
     
      if CHARINDEX(isnull(@vtexto, ''), isnull(@texto, '')) = 0
        select @texto = ISNULL(@texto, '') + case when ISNULL(@texto, '') <> '' then ' ' else '' end + RTRIM(isnull(@vtexto, ''))
      

    end
    else
    begin
    
      if exists (select 0
                   from [@nmsobsauto]
                  where U_parent = @vcodepai
                    and isnull(U_parent, '') <> '')
        exec spcNMSObsAutoNFRecursive @tablename, @vcodepai, @texto output
      
    end
    
  end
  close cp1
  deallocate cp1








