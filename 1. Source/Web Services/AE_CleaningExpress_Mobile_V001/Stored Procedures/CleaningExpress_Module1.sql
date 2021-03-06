

/****** Object:  StoredProcedure [dbo].[AE_SP001_Mobile_GetCompanyList]    Script Date: 03/02/2015 11:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[AE_SP001_Mobile_GetCompanyList]
AS
BEGIN
	SELECT T0.[U_DBName], T0.[U_CompName], T0.[U_SAPUserName], T0.[U_SAPPassword], 
	T0.[U_DBUserName], T0.[U_DBPassword], T0.[U_ConnString] ,T0.[U_Server],T0.[U_LicenseServer] FROM [dbo].[@AB_COMPANYSETUP] T0 WITH (NOLOCK)
END
GO


/****** Object:  StoredProcedure [dbo].[AE_SP002_Mobile_GetAcknowledgement]    Script Date: 03/02/2015 11:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[AE_SP002_Mobile_GetAcknowledgement]
@UserName varchar(60),
@Password varchar(60),
@Company varchar(60)
AS
BEGIN

DECLARE @CompanyName VARCHAR(254);
DECLARE @Theme VARCHAR(10);
DECLARE @RoleId BigInt;
DECLARE @RoleIdCount BigInt;
Declare @Locked VARCHAR(10)
DECLARE @RoleName VARCHAR(254);
DECLARE @UserCheck VARCHAR(254);
DECLARE @UserCheckCount BIGINT;

SELECT @CompanyName= [U_CompName],@Theme = [U_Theme] FROM [dbo].[@AB_COMPANYSETUP] WITH (NOLOCK) WHERE [U_DBName] = @Company
 
SELECT T0.USER_CODE AS 'UserName',T0.U_MobilePwd As 'Password' ,ISNULL (T1.firstName ,'')+' '+ISNULL (T1.middleName,'')+' '+ ISNULL (T1.lastName ,'') [EmployeeName],
@Company AS 'CompanyCode', @CompanyName AS 'CompanyName','Login Successful' AS 'Status', 'Login Successful' AS [Message] ,
T1.empID AS EmpId, @Theme AS Theme,IsNull(T0.Locked,'N') AS Locked
INTO #Login
 FROM dbo.OUSR T0 WITH (NOLOCK)  
LEFT OUTER JOIN OHEM T1 WITH (NOLOCK) ON T1.UserId = T0.UserId
--INNER JOIN HEM6 T2 WITH (NOLOCK) ON T2.empID = T1.empID
WHERE T0.USER_CODE = @UserName AND T0.U_MobilePwd = @Password 
--AND  IsNull(T0.Locked,'N') = 'N' 


SET @UserCheck = (SELECT UserName from #Login)
SET @UserCheckCount = Count(@UserCheck)
SET @RoleId = (SELECT TOP 1 IsNull(roleID,0) from HEM6 where empID = (select EmpId from #Login))
SET @Locked = (SELECT Locked from #Login)
Set @RoleIdCount = (Count(@RoleId))

IF(@UserCheckCount = 0)
BEGIN
	SELECT @UserName AS 'UserName',@Password As 'Password' ,(Select EmployeeName from #Login) [EmployeeName],
	(Select CompanyCode from #Login) AS 'CompanyCode', (Select CompanyName from #Login) AS 'CompanyName',
	'Login Failed' AS 'Status', 'Username or Password is Incorrect.' AS [Message],'' AS EmpId, '' AS Theme ,'' AS RoleId,''AS RoleName 
END
ELSE If(@RoleIdCount = 0)
BEGIN
	SELECT @UserName AS 'UserName',@Password As 'Password' ,(Select EmployeeName from #Login) [EmployeeName],
	(Select CompanyCode from #Login) AS 'CompanyCode', (Select CompanyName from #Login) AS 'CompanyName',
	'Login Failed' AS 'Status', 'User Role is not Assigned in SAP.' AS [Message],'' AS EmpId, '' AS Theme ,'' AS RoleId,''AS RoleName 
END
ELSE IF(@Locked != 'N')
BEGIN
	SELECT @UserName AS 'UserName',@Password As 'Password' ,(Select EmployeeName from #Login) [EmployeeName],
	(Select CompanyCode from #Login) AS 'CompanyCode', (Select CompanyName from #Login) AS 'CompanyName',
	'Login Failed' AS 'Status', 'User is Locked in SAP.' AS [Message],'' AS EmpId, '' AS Theme ,'' AS RoleId,''AS RoleName 
END
ELSE
BEGIN
	select *,@RoleId As RoleId , (select name from OHTY where typeID = @RoleId and IsNull(locked,'N') = 'N') AS RoleName from #Login
END

drop table #Login
END
GO


/****** Object:  StoredProcedure [dbo].[AE_SP003_Mobile_GetProject]    Script Date: 03/02/2015 11:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[AE_SP003_Mobile_GetProject]
@Company varchar(60),
@UserName varchar(100),
@RequiredDate Date
AS
BEGIN
    SELECT T0.U_OcrCode [Project] INTO #Project FROM [@AB_STKBGT] T0 WITH (NOLOCK)
    INNER JOIN [@AB_STKBGT1] T1 WITH (NOLOCK) ON T0.DocEntry = T1.DocEntry 
    WHERE RIGHT(T1.U_BgtMonth,2) = MONTH(@RequiredDate) AND LEFT(T1.U_BgtMonth,4) = YEAR(@RequiredDate ) GROUP BY T0.U_OcrCode

	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) 
	INNER JOIN #project T1 ON T0.[PrcCode] = t1.Project WHERE T0.[DimCode] = 1 and  T0.[Active] = 'Y' AND 
	(T0.[U_AB_MUser1]= @UserName or T0.[U_AB_MUser2]= @UserName or T0.[U_AB_MUser3]= @UserName)
	ORDER BY T0.[PrcCode] 
	DROP TABLE #Project
END
GO

/****** Object:  StoredProcedure [dbo].[AE_SP004_Mobile_GetWareHouse]    Script Date: 03/02/2015 11:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[AE_SP004_Mobile_GetWareHouse]
@Company varchar(60),
@ProjectCode varchar(60)
AS
BEGIN
     SELECT T0.[U_WhsCode] INTO #Warehouse FROM [@AB_STKBGT1] T0 WITH (NOLOCK) INNER JOIN [@AB_STKBGT] T1 WITH (NOLOCK) ON T0.DocEntry = T1.DocEntry  
     WHERE T1.[U_OcrCode] = @ProjectCode AND ISNULL(T0.[U_WhsCode],'') <> ''
      GROUP BY T0.[U_WhsCode]
     

	 SELECT T0.[WhsCode], T0.[WhsName] FROM OWHS T0 WITH (NOLOCK) INNER JOIN #Warehouse T1 ON T0.WhsCode = T1.U_WhsCode  WHERE T0.[Inactive]  <> 'Y'
	 ORDER BY T0.[WhsCode]
	 
	 DROP TABLE  #Warehouse 
END
GO

/****** Object:  StoredProcedure [dbo].[AE_SP005_Mobile_GetItemList]    Script Date: 03/02/2015 11:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/** Procedure to list all the Items based on project,warehouse and selected Required date 
    Created Date : 24/02/2015  
    Created By : VIVEK RM **/  
ALTER PROC [dbo].[AE_SP005_Mobile_GetItemList] --'','8NAPIER','ASQWH','20150315'
@Company varchar(60),
@ProjectCode varchar(60),
@WareHouseCode varchar(60),
@RequiredDate date
AS
BEGIN
    SELECT T1.U_ItemCode, T1.U_AvailQty, T1.U_UPrice [UnitPrice], T1.[DocEntry], T1.[LineId], T0.[U_OcrCode2],IsNull(T1.U_UOM,'') UOM
    INTO #Itemcode 
    FROM [@AB_STKBGT] T0 WITH (NOLOCK) INNER JOIN [@AB_STKBGT1] T1 WITH (NOLOCK) ON T0.DocEntry = T1.DocEntry 
    WHERE T0.U_OcrCode = @ProjectCode AND T1.U_WhsCode = @WareHouseCode AND 
    (RIGHT(T1.U_BgtMonth,2) = MONTH(@RequiredDate) AND LEFT(T1.U_BgtMonth,4) = YEAR(@RequiredDate )) AND ISNULL(T1.U_AvailQty,0) > 0
    
	SELECT T0.[ItemCode], T0.[ItemName] [Description], T1.U_AvailQty  AS [AvailableQty], T1.UnitPrice, T1.DocEntry [BKTDocEntry] , 
	T1.LineId  [BKTLineId] , IsNull(T1.U_OcrCode2,'') [OcrCode2], [UOM],@ProjectCode [ProjectCode], @WareHouseCode [WareHouseCode]
	,@RequiredDate [RequiredDate] FROM OITM T0 WITH (NOLOCK) INNER JOIN #Itemcode T1 ON T0.ItemCode = T1.U_ItemCode 
	 WHERE T0.[frozenFor]  <> 'Y'ORDER BY T0.[ItemCode] 
	 
	 DROP TABLE  #Itemcode 
END

/****** Object:  StoredProcedure [dbo].[AE_SP006_Mobile_GetStockRequestDetails]    Script Date: 03/02/2015 11:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/** Procedure to view the stock request based on the DocEntry 
    Created Date : 27/02/2015  
    Created By : VIVEK RM **/  
ALTER PROC [dbo].[AE_SP006_Mobile_GetStockRequestDetails]
@Company varchar(60),
@DocEntry varchar(60)
AS
BEGIN
	SELECT T1.OcrCode  AS [ProjectCode] ,T4.PrcName  AS [ProjectName],T2.WhsCode ,T3.WhsName ,T0.DocNum ,T0.DocEntry,
	CONVERT(VARCHAR(10), T0.ReqDate, 101) AS [ReqDate],T0.ReqName,
	T1.ItemCode, T1.Dscription AS [Description] ,T1.Quantity , T1.U_AB_BKTDocEntry [BKTDocEntry], T1.U_AB_BKTLineId [BKTLineId],
	T0.U_AB_FillStatus AS [Status]  ,IsNull(T1.OcrCode2,'')[OcrCode2],T1.unitMsr [UOM]
	 FROM OPRQ T0 WITH (NOLOCK)  INNER JOIN PRQ1 T1 WITH (NOLOCK) ON T0.[DocEntry] = T1.[DocEntry]
	 INNER JOIN OPRC T4 WITH (NOLOCK) ON T4.PrcCode = T1.OcrCode 
	 INNER JOIN OITW T2 WITH (NOLOCK) ON T2.ItemCode = T1.ItemCode AND T2.WhsCode = T1.WhsCode
	 LEFT JOIN OWHS T3 WITH (NOLOCK) ON T3.WhsCode = T2.WhsCode
	WHERE T0.[DocEntry] = @DocEntry 
END
GO


/****** Object:  StoredProcedure [dbo].[AE_SP007_Mobile_MGetStockRequestList]    Script Date: 03/02/2015 11:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/** Procedure to list of stock request based on the Category (Open /My / All)
    Created Date : 27/02/2015  
    Created By : VIVEK RM **/  
 ALTER PROC [dbo].[AE_SP007_Mobile_MGetStockRequestList]
@Company varchar(60),
@StockCategory varchar(60),
@UserName varchar(60)
AS
BEGIN

     SELECT T0.PrcCode, T0.PrcName  INTO #Project FROM OPRC T0 WITH (NOLOCK) WHERE (T0.U_AB_MUser1 = @UserName or T0.U_AB_MUser2 = @UserName or T0.U_AB_MUser3 = @UserName )
      
     
	IF(@StockCategory = 'OPEN')
	BEGIN
		SELECT T1.OcrCode  AS [ProjectCode] , T2.PrcName  AS [ProjectName],T0.DocNum ,T0.DocEntry,CONVERT(VARCHAR(10), 
		T0.ReqDate, 101) AS [ReqDate], T0.ReqName, T0.U_AB_FillStatus AS [Status]  
		FROM OPRQ T0 WITH (NOLOCK) INNER JOIN PRQ1 T1 WITH (NOLOCK) ON T0.DocEntry = T1.DocEntry 
		INNER JOIN #Project T2 ON T2.PrcCode = T1.OcrCode 
		 WHERE T0.U_AB_FillStatus  = 'Open' AND IsNull(T1.U_AB_BKTDocEntry,'') != '' AND ISNULL(T1.U_AB_BKTLineId,'') != ''
		 GROUP BY T1.OcrCode  , T2.PrcName ,T0.DocNum ,T0.DocEntry,T0.ReqDate, T0.ReqName, T0.U_AB_FillStatus
		 ORDER BY T0.DocNum DESC
	END
	ELSE IF(@StockCategory = 'ALL')
	BEGIN
		SELECT T1.OcrCode  AS [ProjectCode] , T2.PrcName  AS [ProjectName],T0.DocNum ,T0.DocEntry,CONVERT(VARCHAR(10), 
		T0.ReqDate, 101) AS [ReqDate], T0.ReqName, T0.U_AB_FillStatus AS [Status]  
		FROM OPRQ T0 WITH (NOLOCK) INNER JOIN PRQ1 T1 WITH (NOLOCK) ON T0.DocEntry = T1.DocEntry 
		INNER JOIN #Project T2 ON T2.PrcCode = T1.OcrCode WHERE IsNull(T1.U_AB_BKTDocEntry,'') != '' AND ISNULL(T1.U_AB_BKTLineId,'') != ''
		GROUP BY T1.OcrCode  , T2.PrcName ,T0.DocNum ,T0.DocEntry,T0.ReqDate, T0.ReqName, T0.U_AB_FillStatus
		ORDER BY T0.DocNum DESC
		 --WHERE T0.U_AB_FillStatus  = 'Open'
	END
	ELSE IF(@StockCategory = 'MY')
	BEGIN
		SELECT T1.OcrCode  AS [ProjectCode] , T2.PrcName  AS [ProjectName],T0.DocNum ,T0.DocEntry,CONVERT(VARCHAR(10), 
		T0.ReqDate, 101) AS [ReqDate], T0.ReqName, T0.U_AB_FillStatus AS [Status]  
		FROM OPRQ T0 WITH (NOLOCK) INNER JOIN PRQ1 T1 WITH (NOLOCK) ON T0.DocEntry = T1.DocEntry 
		INNER JOIN #Project T2 ON T2.PrcCode = T1.OcrCode 
		 WHERE T0.U_AB_FillStatus  = 'Open' AND T0.Requester =@UserName AND IsNull(T1.U_AB_BKTDocEntry,'') != '' AND ISNULL(T1.U_AB_BKTLineId,'') != ''
		 GROUP BY T1.OcrCode  , T2.PrcName ,T0.DocNum ,T0.DocEntry,T0.ReqDate, T0.ReqName, T0.U_AB_FillStatus
		 ORDER BY T0.DocNum DESC 
	END
	
	DROP TABLE #Project
END
GO

/****** Object:  StoredProcedure [dbo].[AE_SP008_Mobile_GetGoodsIssueProject]    Script Date: 03/02/2015 11:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/** Procedure to list all the Project for Dropdown 
    Created Date : 28/02/2015  
    Created By : VIVEK RM **/  
ALTER PROC [dbo].[AE_SP008_Mobile_GetGoodsIssueProject]
@Company varchar(60),
@UserName varchar(100)
AS
BEGIN

    SELECT T1.OcrCode INTO #Project FROM OPRQ T0 WITH(NOLOCK)INNER JOIN PRQ1 T1 WITH(NOLOCK) ON T0.DocEntry = T1.DocEntry 
    WHERE T0.U_AB_FillStatus = 'Open'
    AND T1.U_AB_TransferQty - T1.U_AB_IssueQty > 0 
    GROUP BY T1.OcrCode

	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) 
	INNER JOIN #project T1 ON T0.[PrcCode] = T1.OcrCode  WHERE T0.[DimCode] = 1 and  T0.[Active] = 'Y' AND 
	(T0.[U_AB_MUser1]= @UserName or T0.[U_AB_MUser2]= @UserName or T0.[U_AB_MUser3]= @UserName)
	ORDER BY T0.[PrcCode] 
	
	DROP TABLE #Project
	
END
GO

/** Procedure to list of Goods based on the selected Project
    Created Date : 02/03/2015  
    Created By : VIVEK RM **/  
ALTER PROC [dbo].[AE_SP009_Mobile_GetGoodsIssueList]
@Company varchar(60),
@ProjectCode varchar(60)
AS
BEGIN
	SELECT T1.OcrCode  AS [ProjectCode] , T2.PrcName  AS [ProjectName],T0.DocNum ,T0.DocEntry,CONVERT(VARCHAR(10), 
		T0.ReqDate, 101) AS [ReqDate], T0.ReqName, T0.U_AB_FillStatus AS [Status]  
		FROM OPRQ T0 WITH (NOLOCK) INNER JOIN PRQ1 T1 WITH (NOLOCK) ON T0.DocEntry = T1.DocEntry 
		INNER JOIN OPRC T2 ON T2.PrcCode = T1.OcrCode 
		 WHERE UPPER(T0.U_AB_FillStatus)  = 'Open' and T1.OcrCode = @ProjectCode AND T1.U_AB_TransferQty - T1.U_AB_IssueQty > 0 
		 AND IsNull(T1.U_AB_BKTDocEntry,'') != '' AND ISNULL(T1.U_AB_BKTLineId,'') != ''
		 GROUP BY T1.OcrCode , T2.PrcName,T0.DocNum ,T0.DocEntry,
		T0.ReqDate, T0.ReqName, T0.U_AB_FillStatus 
		ORDER BY T0.DocNum DESC 
END
GO

/** Procedure to Perform the goods Issue based on the DocEntry 
    Created Date : 27/02/2015  
    Created By : VIVEK RM **/  
ALTER PROC [dbo].[AE_SP010_Mobile_GetGoodsIssueDetails]
@Company varchar(60),
@DocEntry varchar(60)
AS
BEGIN
	SELECT T1.OcrCode  AS [ProjectCode] ,T4.PrcName  AS [ProjectName],T2.WhsCode ,T3.WhsName ,T0.DocNum ,T0.DocEntry,
	CONVERT(VARCHAR(10), T0.ReqDate, 101) AS [ReqDate],T0.ReqName,
	T1.ItemCode, T1.Dscription AS [Description] , 
	--(SELECT TT0.U_AvailQty  FROM [@AB_STKBGT1] TT0 WITH(NOLOCK) INNER JOIN [@AB_STKBGT] TT1 WITH(NOLOCK) ON TT0.DocEntry = TT1.DocEntry 
	--WHERE LEFT(TT0.U_BgtMonth,4) = YEAR(T0.ReqDate) AND RIGHT(TT0.U_BgtMonth ,2) = MONTH(T0.ReqDate) 
	--AND TT1.U_OcrCode = T1.OcrCode AND TT0.U_ItemCode = T1.ItemCode ) AS [AvailableQty]
	ABS(ISNULL(T1.U_AB_TransferQty,0) - ISNULL(T1.U_AB_IssueQty,0)) AS [AvailableQty] 
	, T1.Quantity ,	T1.U_AB_BKTDocEntry [BKTDocEntry], T1.U_AB_BKTLineId [BKTLineId],T0.Requester [sCurrentUserName] , T1.VisOrder [LineId] ,
	 T1.Price [UnitPrice], T1.U_AB_IssueQty [IssueQty], T1.U_AB_TransferQty [TransferQty],
	T0.U_AB_FillStatus AS [Status]  , IsNull(T1.OcrCode2,'') [OcrCode2], T1.unitMsr [UOM]
	 FROM OPRQ T0 WITH (NOLOCK)  INNER JOIN PRQ1 T1 WITH (NOLOCK) ON T0.[DocEntry] = T1.[DocEntry]
	 INNER JOIN OPRC T4 WITH (NOLOCK) ON T4.PrcCode = T1.OcrCode 
	 INNER JOIN OITW T2 WITH (NOLOCK) ON T2.ItemCode = T1.ItemCode AND T2.WhsCode = T1.WhsCode
	 LEFT JOIN OWHS T3 WITH (NOLOCK) ON T3.WhsCode = T2.WhsCode
	WHERE T0.[DocEntry] = @DocEntry 
	AND T1.U_AB_TransferQty - T1.U_AB_IssueQty > 0
END
GO
Alter PROC [dbo].[AE_SP0011_Mobile_EditStockRequestDetails]
@Company varchar(60),
@DocEntry varchar(60)
AS
BEGIN
	SELECT T1.OcrCode  AS [ProjectCode] ,T4.PrcName  AS [ProjectName],T2.WhsCode ,T3.WhsName ,T0.DocNum ,T0.DocEntry,
	CONVERT(VARCHAR(10), T0.ReqDate, 101) AS [ReqDate],T0.ReqName,
	T1.ItemCode, T1.Dscription AS [Description] , 
	(SELECT TT0.U_AvailQty  FROM [@AB_STKBGT1] TT0 WITH(NOLOCK) INNER JOIN [@AB_STKBGT] TT1 WITH(NOLOCK) ON TT0.DocEntry = TT1.DocEntry 
	WHERE LEFT(TT0.U_BgtMonth,4) = YEAR(T0.ReqDate) AND RIGHT(TT0.U_BgtMonth ,2) = MONTH(T0.ReqDate) 
	AND TT1.U_OcrCode = T1.OcrCode AND TT0.U_ItemCode = T1.ItemCode ) AS [AvailableQty] ,T1.Quantity,
	T1.U_AB_BKTDocEntry [BKTDocEntry], T1.U_AB_BKTLineId [BKTLineId],T0.Requester [sCurrentUserName] , T1.VisOrder [LineId] ,
	 T1.Price [UnitPrice] , T1.U_AB_IssueQty [IssueQty], T1.U_AB_TransferQty [TransferQty], IsNull(T1.OcrCode2,'') [OcrCode2], 
	T0.U_AB_FillStatus AS [Status], T1.unitMsr [UOM]  
	 FROM OPRQ T0 WITH (NOLOCK)  INNER JOIN PRQ1 T1 WITH (NOLOCK) ON T0.[DocEntry] = T1.[DocEntry]
	 INNER JOIN OPRC T4 WITH (NOLOCK) ON T4.PrcCode = T1.OcrCode 
	 INNER JOIN OITW T2 WITH (NOLOCK) ON T2.ItemCode = T1.ItemCode AND T2.WhsCode = T1.WhsCode
	 LEFT JOIN OWHS T3 WITH (NOLOCK) ON T3.WhsCode = T2.WhsCode
	WHERE T0.[DocEntry] = @DocEntry 
END

GO


ALTER PROC [dbo].[AE_SP0013_Mobile_GetGoodsIssueItemList]

@Company varchar(60),
@ProjectCode varchar(60),
@WareHouseCode varchar(60),
@RequiredDate date,
@DocEntry varchar(60)
AS
BEGIN --, T1.U_AvailQty
  SELECT T1.U_ItemCode,(T2.U_AB_TransferQty - T2.U_AB_IssueQty)  [U_AvailQty], T1.U_UPrice [UnitPrice], T1.[DocEntry] [BKTDocEntry], T1.[LineId] [BKTLineId], T0.[U_OcrCode2],T2.unitMsr AS UOM 
  INTO #Itemcode FROM [@AB_STKBGT] T0 
  WITH (NOLOCK) INNER JOIN [@AB_STKBGT1] T1 WITH (NOLOCK) ON T0.DocEntry = T1.DocEntry 
  INNER JOIN PRQ1 T2 ON T2.ItemCode = T1.U_ItemCode 
    WHERE T0.U_OcrCode = @ProjectCode AND T1.U_WhsCode = @WareHouseCode AND 
    (RIGHT(T1.U_BgtMonth,2) = MONTH(@RequiredDate) AND LEFT(T1.U_BgtMonth,4) = YEAR(@RequiredDate )) 
    AND (T2.U_AB_TransferQty - T2.U_AB_IssueQty) > 0 AND T2.DocEntry = @DocEntry 
    
	SELECT T0.[ItemCode], T0.[ItemName] [Description], T1.U_AvailQty  AS [AvailableQty], T1.UnitPrice, T1.BKTDocEntry , T1.BKTLineId , 
	IsNull(T1.U_OcrCode2,'') [OcrCode2] , UOM
	 FROM OITM T0 WITH (NOLOCK) INNER JOIN #Itemcode T1 ON T0.ItemCode = T1.U_ItemCode 
	 WHERE T0.[frozenFor]  <> 'Y'ORDER BY T0.[ItemCode] 
	 
	 DROP TABLE  #Itemcode 
END

