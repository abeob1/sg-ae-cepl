USE [PWCL]
GO
/****** Object:  StoredProcedure [dbo].[AE_SP007_BUDGETREPORT]    Script Date: 29/06/2016 03:29:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[AE_SP007_BUDGETREPORT]'%','%','2016','71111100'

ALTER PROCEDURE [dbo].[AE_SP007_BUDGETREPORT]
@NonProject as varchar(100),
@Project as varchar(100),
@Year as varchar(10),
@Glcode as varchar(10)

AS

Declare @MainBudget as varchar(200)
Declare @ReforecastBudget as varchar(200)

BEGIN

SELECT @MainBudget = T0.[Name] FROM OBGS T0 WHERE year( T0.[FinancYear] ) = @Year and UPPER( T0.[U_AB_MAINBUDGET]) = 'YES'
SELECT @ReforecastBudget = T0.[Name] FROM OBGS T0 WHERE year( T0.[FinancYear] ) = @Year and   UPPER(T0.[U_AB_ACTIVE])  = 'YES'

SELECT T0.[U_BUCode] [BU], T0.[U_PrjCode] [Project], T0.[U_Account] [GL Account] ,
case when isnull(T0.U_PrjCode ,'') = '' then 'BU' else 'PRJ' end [Cat]
,
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
else (SELECT sum( isnull(TT0.[U_BudAmount],0)) FROM [dbo].[@AB_PROJECTBUDGET]  TT0 WHERE TT0.[U_PrjCode] = T0.U_PrjCode  and  TT0.[U_Account] = T0.U_Account and T0.[U_BudName] = @ReforecastBudget) 
end [RevisedBudget]
INTO #TMPBUDGET
FROM [dbo].[@AB_PROJECTBUDGET]  T0
WHERE isnull(T0.U_PrjCode,'') LIKE @Project AND isnull(T0.U_BUCode,'') LIKE @NonProject 
AND T0.U_Account LIKE @Glcode 

--select * from #TMPBUDGET 
--------------------  Calling the function to calculate the Committed amount & Actual amount
 SELECT T0.BU , T0.Project , T1.PrjName , T0.[GL Account] , T2.AcctName  ,T0.[Non ProjectLatest] , T0.ProjectLatest , 
 [dbo].[AE_FN003_BUDGET_COMMITTEDAMOUNT] (@Year,T0.Cat ,T0.BU , T0.Project ,T0.[GL Account] )  [Committed],
 [dbo].[AE_FN004_BUDGET_ACTUALSPEND] (@Year,T0.Cat ,T0.BU , T0.Project ,T0.[GL Account] ) [Actual],
 --[dbo].[AE_FN005_BUDGET_PRAMOUNT] (@Year,T0.Cat ,T0.BU , T0.Project ,T0.[GL Account] ) 
 0 [PR], T0.Cat ,T0.OriginalBudget , T0.RevisedBudget 
  INTO #TMPFINAL
  FROM #TMPBUDGET T0
  left outer join OPRJ T1 on T0.Project = T1.PrjCode 
  left outer join OACT T2 on T0.[GL Account] = T2.AcctCode 
  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
  SELECT T0.BU , T0.Project , T0.PrjName , T0.[GL Account] , T0.AcctName, T0.[Non ProjectLatest] , T0.ProjectLatest , T0.Committed ,T0.Actual ,
  (T0.Committed + T0.Actual ) [CONSUMED],
  CASE WHEN T0.Cat = 'PRJ' THEN T0.ProjectLatest - (T0.Committed + T0.Actual) ELSE
   T0.[Non ProjectLatest]  - (T0.Committed + T0.Actual) END  [ABALANCE] , T0.PR , T0.OriginalBudget , T0.RevisedBudget 
   INTO #BUDGET
   FROM #TMPFINAL T0

    SELECT T0.BU , T0.Project , T0.PrjName , T0.[GL Account] , T0.AcctName, T0.[Non ProjectLatest] , T0.ProjectLatest , T0.Committed ,T0.Actual ,
  T0.[CONSUMED], T0.ABALANCE , T0.PR , ( T0.ABALANCE - T0.PR ) [APR] , T0.OriginalBudget , T0.RevisedBudget 
   FROM #BUDGET T0


END