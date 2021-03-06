USE [PWCL]
GO
/****** Object:  StoredProcedure [dbo].[AE_SP005_BUDGET_COMMITTEDAMOUNT]    Script Date: 29/06/2016 03:30:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

----AE_SP005_BUDGET_COMMITTEDAMOUNT'PWCL','2015','True','Test123',''

--AE_SP005_BUDGET_COMMITTEDAMOUNT'PWCL',2016,'True','Test123',''
--AE_SP005_BUDGET_COMMITTEDAMOUNT'PWCL','2016','True','Test789','71111100'
--AE_SP006_BUDGET_ACTUALSPEND'PWCL','2014','True','Prj002','71111100'
--[AE_SP005_BUDGET_COMMITTEDAMOUNT_Test]'PWCL','2014','True','Prj002','71111100'
--[AE_SP005_BUDGET_COMMITTEDAMOUNT]'PWCL','2014','True','Prj002','71111100'
--select sum( T0.U_BudAmount ) from [@AB_PROJECTBUDGET] T0 where T0.U_PrjCode = 'Test789'





ALTER PROCEDURE [dbo].[AE_SP005_BUDGET_COMMITTEDAMOUNT]

@Entity as  Varchar(100),
@Year as varchar(10),
@Flag as Varchar(100),
@Dimension as Varchar(100),
@GLCode as varchar(100)

as

DECLARE @SQL as varchar(max)
DECLARE @Cond as varchar(1000)


begin

select top(1) T1.LineTotal [Column1], T1.LineTotal [Column2], T1.LineTotal [Column3], T1.LineTotal [Column4], T1.LineTotal [Column5],
 T1.LineTotal [Column6], T1.LineTotal [Column7], T1.LineTotal [Column8]
 into #Tmp from DRF1 T1 where T1.DocEntry = 1 

delete from #tmp

   set @Cond = '(''22'',''1470000113'')'
  

--- Add PO Draft with status Open Raised manually - save as draft
set @sql =
' isnull((SELECT sum(T1.LineTotal ) LineTotal
 FROM '+@Entity+' ..ODRF T0  INNER JOIN '+@Entity+' ..DRF1 T1 ON T0.[DocEntry] = T1.[DocEntry] 
left outer join  '+@Entity+' ..OWDD T2 on T0.DocEntry = T2.DocEntry 
left outer join  '+@Entity+' .. OWTM T3 on T2.[WtmCode] = T3.[WtmCode] 
where T1.LineStatus  = ''O'' and isnull(T2.[Status],'''') = '''' and T0.DocStatus = ''O'' and T0.ObjType in '+ @Cond +'
and cast(year(T0.DocDate) as varchar) ='+ @Year +' and isnull(T3.[Active],''Y'') = ''Y'' and
(case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +'''
),0) Column1 , '


--- Add PO Draft with status Open  - Pending for Approval / Approved but not converted to PO
set @sql +=
'isnull((SELECT sum(T1.LineTotal) LineTotal  
 FROM '+@Entity+' ..ODRF T0  INNER JOIN '+@Entity+' ..DRF1 T1 ON T0.[DocEntry] = T1.[DocEntry] 
left outer join  '+@Entity+' ..OWDD T2 on T0.DocEntry = T2.DocEntry 
left outer join  '+@Entity+' .. OWTM T3 on T2.[WtmCode] = T3.[WtmCode]
where T1.LineStatus = ''O'' and  isnull(T2.[Status],'''') <> ''N'' and T0.DocStatus = ''O'' and T0.ObjType in '+ @Cond +'
and cast(year(T0.DocDate) as varchar) ='+ @Year +' and isnull(T3.[Active],'''') = ''Y''
and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +'''
 ),0) Column2 ,'

 --- Add PR Document with status Open
set @sql +=
'isnull((select sum( case when isnull(T2.LineTotal,0) = 0 then  T1.LineTotal  else 0 end  )  from '+@Entity+' ..OPRQ T0 join '+@Entity+' ..PRQ1 T1 on T0.DocEntry = T1.DocEntry 
left outer join DRF1 T2 on T2.BaseEntry = T1.DocEntry and T2.BaseLine = T1.LineNum and T2.BaseType = ''1470000113''
left outer join ODRF T3 on T2.DocEntry = T3.Docentry and T3.DocStatus = ''O''
where T1.LineStatus = ''O'' and T0.DocStatus = ''O''
and cast(year(T0.DocDate) as varchar) ='+ @Year +' 
and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +'''
),0) Column3,'

--- Add PO Document with status Open
set @sql +=
'isnull((select sum(T1.LineTotal) LineTotal  from '+@Entity+' ..OPOR T0 join '+@Entity+' ..POR1 T1 on T0.DocEntry = T1.DocEntry where T1.LineStatus = ''O'' and T0.DocStatus = ''O''
and cast(year(T0.DocDate) as varchar) ='+ @Year +' 
and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +'''
),0) Column4,'

--- Add PO Document with status Closed But GRN in Open Status
set @sql +=
'isnull((SELECT sum(T1.[LineTotal]) LineTotal FROM '+@Entity+' ..OPOR T0  INNER JOIN '+@Entity+' ..POR1 T1 ON T0.[DocEntry] = T1.[DocEntry] join 
 '+@Entity+' ..PDN1 T2 on T2.BaseEntry = T1.DocEntry and T2.BaseLine = T1.LineNum  INNER JOIN '+@Entity+' ..OPDN T3 ON T2.[DocEntry] = T3.[DocEntry] WHERE T2.[LineStatus] = ''O''
 and cast(year(T0.DocDate) as varchar) ='+ @Year +' 
 and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
 and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +'''
 ),0) Column5,'

--- Less PO Draft with status Open  - Approval status Rejected
set @sql +=
'isnull((SELECT sum(T1.LineTotal) LineTotal  
 FROM '+@Entity+' ..ODRF T0  INNER JOIN '+@Entity+' .. DRF1 T1 ON T0.[DocEntry] = T1.[DocEntry] 
left outer join  '+@Entity+' ..OWDD T2 on T0.DocEntry = T2.DocEntry 
left outer join  '+@Entity+' .. OWTM T3 on T2.[WtmCode] = T3.[WtmCode]
where T1.LineStatus = ''O'' and isnull(T2.[Status],'''') = ''N'' and T0.DocStatus = ''O'' and T0.ObjType in '+ @Cond +'
and cast(year(T0.DocDate) as varchar) ='+ @Year +' and isnull( T3.[Active],'''') = ''Y''
and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +'''
),0) Column6,'

--- Less PO Document with status Cancel  - User manually Cancel
set @sql +=
'isnull((select sum(T1.LineTotal) LineTotal   from '+@Entity+' ..OPOR T0 join '+@Entity+' ..POR1 T1 on T0.DocEntry = T1.DocEntry where T1.LineStatus = ''C'' and T0.CANCELED = ''Y''
and cast(year(T0.DocDate) as varchar) ='+ @Year +' 
and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +'''
),0) Column7,'

--- Less PO Document with status Close  - User manually Close
set @sql +=
'isnull((select sum(T1.LineTotal) LineTotal   from '+@Entity+' ..OPOR T0 join '+@Entity+' ..POR1 T1 on T0.DocEntry = T1.DocEntry where T1.LineStatus = ''C'' and T0.CANCELED = ''N'' 
and T1.TargetType = -1
and cast(year(T0.DocDate) as varchar) ='+ @Year +' 
and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +'''
),0) Column8'

insert into #tmp
EXEC ('Select' + @SQL)


--select (T1.Column1 + T1.Column2 + T1.Column3 + T1.Column4 + T1.Column5) - T1.Column6 - T1.Column7 - T1.Column8 [CommittedAmount] from #tmp T1
select (T1.Column2 + T1.Column3 + T1.Column4 + T1.Column5) [CommittedAmount] from #tmp T1

end
