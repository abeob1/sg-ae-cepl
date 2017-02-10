USE [PWCL]
GO
/****** Object:  StoredProcedure [dbo].[AE_SP007_BUDGETREPORT_Test]    Script Date: 29/06/2016 03:29:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--[AE_SP007_BUDGETREPORT_Test]'%','Prj001','2016', '71111100'

ALTER PROCEDURE [dbo].[AE_SP007_BUDGETREPORT_Test]
@NonProject as varchar(100),
@Project as varchar(100),
@Year as varchar(10)
--@Glcode as varchar(10)

AS

Declare @MainBudget as varchar(200)
Declare @ReforecastBudget as varchar(200)

BEGIN

SELECT @MainBudget = T0.[Name] FROM OBGS T0 WHERE year( T0.[FinancYear] ) = @Year and UPPER( T0.[U_AB_MAINBUDGET]) = 'YES'
SELECT @ReforecastBudget = T0.[Name] FROM OBGS T0 WHERE year( T0.[FinancYear] ) = @Year and   UPPER(T0.[U_AB_ACTIVE])  = 'YES'

-----  Project budget Table
-----  Fetching the Information based on the condition 
SELECT isnull(T0.[U_BUCode],'') [U_BUCode], isnull(T0.[U_PrjCode],'') [U_PrjCode], T0.[U_Account] [U_Account] ,
case when isnull(T0.U_PrjCode ,'') = '' then 'BU' else 'PRJ' end [Cat]
INTO #TMPBUDGET1
FROM [dbo].[@AB_PROJECTBUDGET]  T0
WHERE T0.U_PrjCode LIKE @Project AND T0.U_BUCode LIKE @NonProject 
group by T0.[U_BUCode], T0.[U_PrjCode] , T0.[U_Account]

------ PR / PO Draf Table 
SELECT isnull(T1.U_AB_NONPROJECT,'') [U_BUCode], isnull(T1.Project,'') [U_PrjCode], T1.AcctCode [U_Account],
case when isnull(T1.Project ,'') = '' then 'BU' else 'PRJ' end [Cat]  
into #TMPBUDGET2
 FROM ODRF T0  INNER JOIN DRF1 T1 ON T0.[DocEntry] = T1.[DocEntry] 
left outer join  OWDD T2 on T0.DocEntry = T2.DocEntry 
left outer join   OWTM T3 on T2.[WtmCode] = T3.[WtmCode]
where T1.LineStatus = 'O' and isnull(T2.[Status],'N') <> 'N' and T0.DocStatus = 'O' and T0.ObjType in ('22','1470000113')
and cast(year(T0.DocDate) as varchar) =@Year and isnull(T3.[Active],'Y') = 'Y'
and isnull(T1.Project,'') LIKE @Project and isnull(T1.U_AB_NONPROJECT ,'')  like @NonProject
and isnull(T1.AcctCode ,'') not in (select T1.[U_Account]  from #TMPBUDGET1 T1 )

------- PO 
select isnull(T1.U_AB_NONPROJECT,'') [U_BUCode], isnull(T1.Project,'') [U_PrjCode], T1.AcctCode [U_Account],
case when isnull(T1.Project ,'') = '' then 'BU' else 'PRJ' end [Cat] 
into #TMPBUDGET3
  from OPOR T0 join POR1 T1 on T0.DocEntry = T1.DocEntry where T1.LineStatus = 'O' and T0.DocStatus = 'O'
and cast(year(T0.DocDate) as varchar) =@Year 
and isnull(T1.Project,'') LIKE @Project and isnull(T1.U_AB_NONPROJECT ,'')  like @NonProject
and isnull(T1.AcctCode ,'') not in (select T1.[U_Account]  from #TMPBUDGET1 T1 )

------- PR
select  isnull(T1.U_AB_NONPROJECT,'') [U_BUCode], isnull(T1.Project,'') [U_PrjCode], T1.AcctCode [U_Account],
case when isnull(T1.Project ,'') = '' then 'BU' else 'PRJ' end [Cat] 
into #TMPBUDGET4
from OPRQ T0 join PRQ1 T1 on T0.DocEntry = T1.DocEntry 
left outer join DRF1 T2 on T2.BaseEntry = T1.DocEntry and T2.BaseLine = T1.LineNum and T2.BaseType = '1470000113'
left outer join ODRF T3 on T2.DocEntry = T3.Docentry and T3.DocStatus = 'O'
where T1.LineStatus = 'O' and T0.DocStatus = 'O'
and cast(year(T0.DocDate) as varchar) = @Year
and isnull(T1.Project,'') LIKE @Project and isnull(T1.U_AB_NONPROJECT ,'')  like @NonProject
and isnull(T1.AcctCode ,'') not in (select T1.[U_Account]  from #TMPBUDGET1 T1 )

-------- Creating the TMP Table
select Top (1) * into #TMPBUDGET from #TMPBUDGET1 
Delete from [#TMPBUDGET] 

-------- Dumping all the information in to TMP Table
insert into [#TMPBUDGET] 
select * from [#TMPBUDGET1] 
union all
select * from [#TMPBUDGET2] 
union all
select * from [#TMPBUDGET3]
union all
select * from [#TMPBUDGET4]

-------- Fetching information for #TMPBUDGET
SELECT T0.[U_BUCode] [BU], T0.[U_PrjCode] [Project], T0.[U_Account] [GL Account] ,
T0.Cat  [Cat],
case when isnull(T0.U_PrjCode ,'') = '' then 
(SELECT sum( isnull(TT0.[U_BudAmount],0)) FROM [dbo].[@AB_PROJECTBUDGET]  TT0 WHERE TT0.[U_BUCode] = T0.U_BUCode  and  TT0.[U_Account] = T0.U_Account and TT0.[U_BudName] = @ReforecastBudget )
else 0 end [Non ProjectLatest],
case when isnull(T0.U_PrjCode ,'') <> '' then
(SELECT sum(isnull( TT0.[U_BudAmount],0)) FROM [dbo].[@AB_PROJECTBUDGET]  TT0 WHERE TT0.[U_PrjCode] = T0.U_PrjCode  and  TT0.[U_Account] = T0.U_Account and TT0.[U_BudName] = @ReforecastBudget)
else 0 end [ProjectLatest],
case when isnull(T0.U_PrjCode ,'') = '' then 
(SELECT sum(isnull(TT0.[U_BudAmount],0)) FROM [dbo].[@AB_PROJECTBUDGET]  TT0 WHERE TT0.[U_BUCode] = T0.U_BUCode  and  TT0.[U_Account] = T0.U_Account and TT0.[U_BudName] = @ReforecastBudget )
else (SELECT sum( isnull(TT0.[U_BudAmount],0)) FROM [dbo].[@AB_PROJECTBUDGET]  TT0 WHERE TT0.[U_PrjCode] = T0.U_PrjCode  and  TT0.[U_Account] = T0.U_Account and TT0.[U_BudName] = @MainBudget) 
end [OriginalBudget],
case when isnull(T0.U_PrjCode ,'') = '' then 
(SELECT sum(isnull(TT0.[U_BudAmount],0)) FROM [dbo].[@AB_PROJECTBUDGET]  TT0 WHERE TT0.[U_BUCode] = T0.U_BUCode  and  TT0.[U_Account] = T0.U_Account and TT0.[U_BudName] = @ReforecastBudget )
else (SELECT sum( isnull(TT0.[U_BudAmount],0)) FROM [dbo].[@AB_PROJECTBUDGET]  TT0 WHERE TT0.[U_PrjCode] = T0.U_PrjCode  and  TT0.[U_Account] = T0.U_Account and TT0.[U_BudName] = @ReforecastBudget) 
end [RevisedBudget]
INTO #TMPBUDGETResult
FROM #TMPBUDGET  T0
WHERE T0.U_PrjCode LIKE @Project AND T0.U_BUCode LIKE @NonProject 
--AND T0.U_Account LIKE @Glcode 
--------------------  Calling the function to calculate the Committed amount & Actual amount
 SELECT T0.BU , T0.Project , T1.PrjName , T0.[GL Account] , T2.AcctName  ,T0.[Non ProjectLatest] , T0.ProjectLatest , 
 [dbo].[AE_FN003_BUDGET_COMMITTEDAMOUNT] (@Year,T0.Cat ,T0.BU , T0.Project ,T0.[GL Account] )  [Committed],
 [dbo].[AE_FN004_BUDGET_ACTUALSPEND] (@Year,T0.Cat ,T0.BU , T0.Project ,T0.[GL Account] ) [Actual],
 --[dbo].[AE_FN005_BUDGET_PRAMOUNT] (@Year,T0.Cat ,T0.BU , T0.Project ,T0.[GL Account] ) 
 0 [PR], T0.Cat ,T0.OriginalBudget , T0.RevisedBudget 
  INTO #TMPFINAL
  FROM #TMPBUDGETResult T0
  left outer join OPRJ T1 on T0.Project = T1.PrjCode 
  left outer join OACT T2 on T0.[GL Account] = T2.AcctCode 
  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
  SELECT T0.BU , T0.Project , T0.PrjName , T0.[GL Account] , T0.AcctName, T0.[Non ProjectLatest] , T0.ProjectLatest , T0.Committed ,T0.Actual ,
  (T0.Committed + T0.Actual ) [CONSUMED],
  CASE WHEN T0.Cat = 'PRJ' THEN T0.ProjectLatest - (T0.Committed + T0.Actual) ELSE
   T0.[Non ProjectLatest]  - (T0.Committed + T0.Actual) END  [ABALANCE] , T0.PR , T0.OriginalBudget , T0.RevisedBudget 
   INTO #BUDGET
   FROM #TMPFINAL T0

    SELECT T0.BU , T0.Project , T0.PrjName , T0.[GL Account] , T0.AcctName, isnull(T0.[Non ProjectLatest],0) [Non ProjectLatest] , isnull(T0.ProjectLatest,0) ProjectLatest
	, isnull(T0.Committed,0) Committed , isnull(T0.Actual,0) Actual ,
  isnull( T0.[CONSUMED],0) [CONSUMED], isnull(T0.ABALANCE,0) ABALANCE , isnull(T0.PR,0) [PR] , ( isnull(T0.ABALANCE,0) - isnull(T0.PR,0) ) [APR] , isnull(T0.OriginalBudget,0) [OriginalBudget]
   , isnull(T0.RevisedBudget ,0) [RevisedBudget]
   FROM #BUDGET T0

END