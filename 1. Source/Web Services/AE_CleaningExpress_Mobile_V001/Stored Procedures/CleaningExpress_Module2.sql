
/****** Object:  StoredProcedure [dbo].[AE_SP012_Mobile_CF_GetProject]    Script Date: 4/8/2015 4:27:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--AE_SP012_Mobile_CF_GetProject 'CleaningExpress','manager'
ALTER PROC [dbo].[AE_SP012_Mobile_CF_GetProject]
@Company varchar(60),
@UserName varchar(100)
AS
BEGIN
	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) 
	WHERE T0.[DimCode] = 1 and  ISNULL(T0.[Active],'N') = 'Y' AND 
	(T0.[U_AB_MUser1]= @UserName or T0.[U_AB_MUser2]= @UserName or T0.[U_AB_MUser3]= @UserName)
	ORDER BY T0.[PrcCode] 
END


GO
/****** Object:  StoredProcedure [dbo].[AE_SP014_Mobile_SA_ListofDocuments]    Script Date: 4/8/2015 4:27:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[AE_SP014_Mobile_SA_ListofDocuments]
@Company varchar(60),
@OwnerCode varchar(100)
AS
BEGIN

DECLARE @CATEGORY VARCHAR(250);
SELECT DISTINCT @CATEGORY = U_Category from dbo.[@AB_SHOWAROUND1] WHERE U_Category LIKE '%PROSPECT%'

	SELECT T0.DocEntry, T0.DocNum, T0.U_DocDate [DocDate],T0.U_OwnerCode [OwnerCode], T0.U_OwnerName [OwnerName]
	,(SELECT U_Description from [@AB_SHOWAROUND1] WHERE U_Category = @CATEGORY AND U_Question = 'Name' AND DocEntry = T0.DocEntry) [ProspectName]
	,(SELECT U_Description from [@AB_SHOWAROUND1] WHERE U_Category = @CATEGORY AND U_Question = 'Address' AND DocEntry = T0.DocEntry) [ProspectAddress]
	--,	ISNULL (T3.firstName ,'')+' '+ISNULL (T3.middleName,'')+' '+ ISNULL (T3.lastName ,'') [UserName]  
	FROM [@AB_SHOWAROUND] T0  
	--inner JOIN [@AB_SHOWAROUND1] T1 ON T1.DocEntry = T0.DocEntry 
	--INNER JOIN [OUSR] T2 ON T2.USER_CODE = T0.U_LoginUser
	--LEFT JOIN [OHEM] T3 ON T3.userId = T2.USERID
	WHERE T0.U_OwnerCode = @OwnerCode
	ORDER BY T0.DocNum DESC
END


GO
/****** Object:  StoredProcedure [dbo].[AE_SP015_Mobile_SA_ViewDocumentDetails]    Script Date: 4/8/2015 4:27:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[AE_SP015_Mobile_SA_ViewDocumentDetails]
@Company varchar(60),
@DocEntry varchar(60)
AS
BEGIN
	SELECT @Company [Company],T0.DocEntry, T0.DocNum, T0.U_DocDate [DocDate],T0.U_LoginUser [OwnerCode], T0.U_OwnerName [OwnerName],
	--ISNULL (T3.firstName ,'')+' '+ISNULL (T3.middleName,'')+' '+ ISNULL (T3.lastName ,'') [UserName], 
	IsNull(T1.U_Category,'0') [Category], IsNull(T1.U_Question,'0') [Question], T1.U_Quantity [Quantity], T1.U_Description [Description],T0.U_Remarks [Remarks], 
	--(Select u_Sort from [@AB_SHOWAROUNDMLINE] T2 where T2.
	T1.U_LineNum [LineNum] FROM [@AB_SHOWAROUND] T0 
	
	INNER JOIN [@AB_SHOWAROUND1] T1 ON T1.DocEntry = T0.DocEntry 
	LEFT OUTER JOIN [@AB_SHOWAROUNDMLINE] T2 ON T2.U_Category = T1.U_Category AND T2.U_Question =T1.U_Question 
	--INNER JOIN [OUSR] T2 ON T2.USER_CODE = T0.U_OwnerCode
	--LEFT JOIN [OHEM] T3 ON T3.userId = T2.USERID
	WHERE T0.DocEntry = @DocEntry and IsNull(T1.U_Category,'0') != '0' and IsNull(T1.U_Question,'0') != '0'

	
	--UNION ALL
	
	--SELECT @Company [Company],@DocEntry 'DocEntry',''DocNum, GETDATE() [DocDate],''OwnerCode, '' OwnerName
	--,U_Category [Category],U_Question  [Question], 0 [Quantity], '' [Description] ,LineId [LineNum]
	--FROM [@AB_SHOWAROUNDMLINE] WHERE   U_Active = 'Y' AND (U_Category NOT IN (SELECT U_Category FROM [@AB_SHOWAROUND1] WHERE DocEntry = @DocEntry)
	--OR U_Question NOT IN (SELECT U_Question FROM [@AB_SHOWAROUND1] WHERE DocEntry = @DocEntry))
	
	ORDER BY T2.U_Sort ASC

END


GO
/****** Object:  StoredProcedure [dbo].[AE_SP016_Mobile_SA_GetQuestions]    Script Date: 4/8/2015 4:27:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--AE_SP016_Mobile_SA_GetQuestions 'CleaningExpress'
ALTER PROC [dbo].[AE_SP016_Mobile_SA_GetQuestions]
@Company varchar(60)
AS
BEGIN
SELECT  @Company [Company],CONVERT(VARCHAR(10),GETDATE(),101)[DocDate], '' [OwnerCode],'' [OwnerName] , IsNull(U_Category,'0') [Category], IsNull(U_Question,'0') [Question],
'' [Quantity],'' [Description]
FROM [@AB_SHOWAROUNDMLINE]  WHERE ISNULL(U_Active,'N') = 'Y' and U_Category != '0' and U_Question != '0' ORDER BY U_Sort ASC
END
GO
/****** Object:  StoredProcedure [dbo].[AE_SP017_Mobile_CF_GetQuestions]    Script Date: 4/8/2015 4:27:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[AE_SP017_Mobile_CF_GetQuestions]
@Company varchar(60)
AS
BEGIN
SELECT  @Company [Company],CONVERT(VARCHAR(10),GETDATE(),101)[DocDate] , '' [ClientName],'' [ClientPhone],
'' [Project],'' [TImprovements],'' [TCompliments],'' [TRecommend],
Isnull(U_Category,'0') [Category],IsNull(U_SubCategory,'0') [SubCategory],IsNull(U_SubCatText,'') [SubCatText]
,IsNull(U_Question,'')[Question],'' [Rating],Code [BaseEntry],LineId [BaseLineNum]

FROM [@AB_SURVEYMASTERLINE]  WHERE ISNULL(U_Active,'N') = 'Y' and U_Category != '0' and U_SubCategory != '0' ORDER BY U_Sort ASC 
END
GO
/****** Object:  StoredProcedure [dbo].[AE_SP018_Mobile_CF_ListofFeedback]    Script Date: 4/8/2015 4:27:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--AE_SP018_Mobile_CF_ListofFeedback 'CleaningExpress','6RQUAY-M'
ALTER PROC [dbo].[AE_SP018_Mobile_CF_ListofFeedback]
@Company varchar(60),
@Project varchar(100)
AS
BEGIN
	SELECT T0.DocEntry, T0.DocNum, T0.U_DocDate [DocDate]	FROM [@AB_SURVEYHEADER] T0 
	WHERE T0.U_Project = @Project
	ORDER BY T0.DocNum DESC
END



GO
