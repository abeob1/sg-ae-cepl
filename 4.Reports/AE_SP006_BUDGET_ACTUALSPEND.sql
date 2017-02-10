USE [PWCL]
GO
/****** Object:  StoredProcedure [dbo].[AE_SP006_BUDGET_ACTUALSPEND]    Script Date: 29/06/2016 03:30:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--AE_SP006_BUDGET_ACTUALSPEND'PWCL',2014,'True','Prj002',''



ALTER PROCEDURE [dbo].[AE_SP006_BUDGET_ACTUALSPEND]

@Entity as  Varchar(100),
@Year as varchar(10),
@Flag as Varchar(100),
@Dimension as Varchar(100),
@GLCode as varchar(100)

as

DECLARE @SQL as varchar(max)


begin

select top(1) T1.LineTotal [Column1], T1.LineTotal [Column2], T1.LineTotal [Column3], T1.LineTotal [Column4], T1.LineTotal [Column5]
 into #Tmp from DRF1 T1 where T1.DocEntry = 1 

delete from #tmp


--- Add AP INVOICE with status Open - Base Document PO,GRPO & Direct
set @SQL =
' isnull((SELECT sum(T1.[LineTotal]) FROM '+@Entity+' ..OPCH T0  INNER JOIN '+@Entity+' ..PCH1 T1 ON T0.[DocEntry] = T1.[DocEntry]
WHERE t1.LineStatus = ''O'' and T0.DocStatus = ''O'' AND T1.BaseType IN (''20'',''22'',''-1'')
and cast(year(T0.DocDate) as varchar) ='+ @Year +' 
and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +''')
,0) Column1 , '

--- Add JE & Direct
set @SQL +=
'isnull((SELECT sum(T2.[Debit] - T2.[Credit])  FROM '+@Entity+' ..[OJDT]  T1 INNER JOIN '+@Entity+' ..JDT1 T2 
ON T1.[TransId] = T2.[TransId] WHERE T1.[TransType] = 30
and T1.[TransId] NOT IN (SELECT StornoToTr FROM ojdt WHERE ISNULL(StornoToTr,'''') <> '''') AND T1.[StornoToTr] IS NULL 
and cast(year(T1.TaxDate) as varchar) ='+ @Year +'
and (case when '''+@Flag+''' = ''True'' then isnull(T2.Project,'''') else  isnull(T2.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T2.Account ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +''')
,0) Column2, '

--- Add  AP Invoice with link AP credit memo 

set @sql +=
'isnull((SELECT sum(T1.[LineTotal]) FROM '+@Entity+' ..OPCH T0  INNER JOIN '+@Entity+' ..PCH1 T1 ON T0.[DocEntry] = T1.[DocEntry]
join '+@Entity+' ..RPC1 T2 on T2.BaseEntry = T1.DocEntry and T2.BaseLine = T1.LineNum INNER JOIN '+@Entity+' ..ORPC T3 ON T2.[DocEntry] = T3.[DocEntry] 
WHERE T1.[LineStatus]  = ''C'' 
and cast(year(T0.DocDate) as varchar) ='+ @Year +' 
and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +'''),0)  Column3,'


--- Less AP Credit memo standalone
set @SQL +=
'isnull((SELECT sum(T1.[LineTotal]) FROM '+@Entity+' ..ORPC T0  INNER JOIN '+@Entity+' ..RPC1 T1 ON T0.[DocEntry] = T1.[DocEntry] WHERE T1.[LineStatus]  = ''O'' and [BaseType] = -1
and cast(year(T0.DocDate) as varchar) ='+ @Year +' 
and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +''')
,0) Column4 , '

--- Less AP INVOICE Cancell
set @SQL +=
'isnull((SELECT sum(T1.[LineTotal]) FROM '+@Entity+' ..OPCH T0  INNER JOIN '+@Entity+' ..PCH1 T1 ON T0.[DocEntry] = T1.[DocEntry]
WHERE t1.LineStatus = ''C'' and T0.DocStatus = ''C'' and T0.CANCELED = ''y''
and cast(year(T0.DocDate) as varchar) ='+ @Year +' 
and (case when '''+@Flag+''' = ''True'' then isnull(T1.Project,'''') else  isnull(T1.U_AB_NONPROJECT ,'''')  end) = '''+ @Dimension +'''
and (case when '''+@Flag+''' = ''False'' then isnull(T1.AcctCode ,'''') else '''+ @GLCode +'''  end) = '''+ @GLCode +''')
,0) Column5'
 
 

insert into #tmp
EXEC ('Select' + @SQL)



--select (T1.Column1 + T1.Column2  + T1.Column3 ) - T1.Column4 - T1.Column5   [ActualSpend] from #tmp T1

select (T1.Column1 + T1.Column2 ) - T1.Column4    [ActualSpend] from #tmp T1

end
