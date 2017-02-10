USE [PWCL]
GO
/****** Object:  UserDefinedFunction [dbo].[AE_FN004_BUDGET_ACTUALSPEND]    Script Date: 29/06/2016 03:30:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




--AE_SP006_BUDGET_ACTUALSPEND'PWCL',2016,'True','Prj001','71111100'


--select [dbo].[AE_FN004_BUDGET_ACTUALSPEND]('2016','PRJ','','Prj001','71111100')
ALTER function [dbo].[AE_FN004_BUDGET_ACTUALSPEND]
(
@Year varchar(10),
@Cat Varchar(100),
@BU varchar(100),
@Project varchar(100),
@GLCode  varchar(100)
)
Returns Decimal(19,2)
as begin
Declare @Column1 as decimal(19,2)
Declare @Column2 as decimal(19,2)
Declare @Column3 as decimal(19,2)
Declare @Column4 as decimal(19,2)
Declare @Column5 as decimal(19,2)
Declare @Dimension Varchar(100)
   
if @cat = 'PRJ'
 begin
  set @Dimension = @Project
 end
else
 begin
  set @Dimension = @BU
 end

--- Add AP INVOICE with status Open - Base Document PO,GRPO & Direct
select @Column1= isnull((SELECT sum(T1.[LineTotal]) FROM OPCH T0  INNER JOIN PCH1 T1 ON T0.[DocEntry] = T1.[DocEntry]
WHERE t1.LineStatus = 'O' AND T1.BaseType IN ('20','22','-1')
and cast(year(T0.DocDate) as varchar) =@Year  
and (case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode)
,0) 
, 
--- Add JE & Direct
@Column2 = isnull((SELECT sum(T2.[Debit] - T2.[Credit]) FROM [OJDT]  T1 INNER JOIN JDT1 T2 
ON T1.[TransId] = T2.[TransId] WHERE T1.[TransType] = 30
and T1.[TransId] NOT IN (SELECT StornoToTr FROM ojdt WHERE ISNULL(StornoToTr,'') <> '') AND T1.[StornoToTr] IS NULL 
and cast(year(T1.TaxDate) as varchar) =@Year
and (case when @Cat = 'PRJ' then isnull(T2.Project,'') else  isnull(T2.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T2.Account ,'') = @GLCode),0) ,

@Column3 = isnull((SELECT sum(T1.[LineTotal]) FROM OPCH T0  INNER JOIN PCH1 T1 ON T0.[DocEntry] = T1.[DocEntry]
join RPC1 T2 on T2.BaseEntry = T1.DocEntry and T2.BaseLine = T1.LineNum INNER JOIN ORPC T3 ON T2.[DocEntry] = T3.[DocEntry] 
WHERE T1.[LineStatus]  = 'C'
and cast(year(T0.DocDate) as varchar) =@Year 
and (case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode),0) ,
  
--- Less AP Credit memo standalone
@Column4 = isnull((SELECT sum(T1.[LineTotal]) FROM ORPC T0  INNER JOIN RPC1 T1 ON T0.[DocEntry] = T1.[DocEntry] WHERE T1.[LineStatus]  = 'O' and [BaseType] = -1
and cast(year(T0.DocDate) as varchar) =@Year 
and (case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode)
,0)  , 

--- Less AP INVOICE Cancell
@Column5 = isnull((SELECT sum(T1.[LineTotal]) FROM OPCH T0  INNER JOIN PCH1 T1 ON T0.[DocEntry] = T1.[DocEntry]
WHERE t1.LineStatus = 'O' AND T1.BaseType IN ('18')
and cast(year(T0.DocDate) as varchar) =@Year 
and (case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode)
,0)  

return ((@Column1 + @Column2  ) - @Column4 )

end
