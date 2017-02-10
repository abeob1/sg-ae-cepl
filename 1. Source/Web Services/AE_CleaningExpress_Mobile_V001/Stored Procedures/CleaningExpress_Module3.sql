--AE_SP019_Mobile_JS_GetProject 'CleaningExpress','manager',''
ALTER PROC [dbo].[AE_SP019_Mobile_JS_GetProject]
@Company varchar(60),
@UserName varchar(100),
@UserRole varchar(100)
AS
BEGIN
IF(@UserRole <> 'Sales')
BEGIN
	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) 
	WHERE T0.[DimCode] = 1 and  ISNULL(T0.[Active],'N') = 'Y' AND 
	(T0.[U_AB_MUser1]= @UserName or T0.[U_AB_MUser2]= @UserName or T0.[U_AB_MUser3]= @UserName)
	ORDER BY T0.[PrcCode] 
END
ELSE
BEGIN
	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) WHERE IsNull(Active,'N') = 'Y'
END
END

--{
--"sCompany":"CleaningExpress",
--"sCurrentUserName":"manager",
--"sUserRole":""
--}

GO

-- AE_SP020_Mobile_JS_ListofJobs 'CleaningExpress','8NAPIER'
-- AE_SP020_Mobile_JS_ListofJobs 'CleaningExpress','ALLSWORT'

ALTER PROC [dbo].[AE_SP020_Mobile_JS_ListofJobs]
@Company varchar(60), 
@ProjectCode varchar(60)
AS
BEGIN

SELECT DocNum ,DocEntry ,U_DocDate [DocDate], 
(Case When [U_DocStatus] = 'O' Then 'Open' When [U_DocStatus] = 'C' Then 'Closed' end) [DocStatus] ,
T0.[U_OcrCode] [PrcCode] , T1.PrcName
,T0.U_StartDate [StartDate], T0.U_EndDate [EndDate]   
FROM [@AB_JOBSCH] T0 LEFT JOIN [OPRC] T1 ON T1.PrcCode = T0.[U_OcrCode]
WHERE [U_OCRCODE] = @ProjectCode AND [U_DocStatus] = 'O' AND IsNull(T1.Active,'N') = 'Y'

END

--{ "Company":"CleaningExpress", "ProjectCode":"8NAPIER" }

GO

 -- AE_SP021_Mobile_JS_JobDetails 'CleaningExpress','3'
ALTER PROC [dbo].[AE_SP021_Mobile_JS_JobDetails]
@Company varchar(60), 
@DocEntry varchar(60)
AS
BEGIN
    SELECT T1.LineId,T1.VisOrder ,T1.U_ScheduleDate [ScheduledDate],T1.U_DayStatus [DayStatus],T2.U_CleaningType [CleaningType],
    T2.U_Frequency [Frequency] 
    FROM [@AB_JOBSCH3] T0 
    INNER join [@AB_JOBSCH2] T1 ON T0.DocEntry =T1.DocEntry AND T1.LineId =T0.U_DayLineNum 
    INNER JOIN [@AB_JOBSCH1] T2 ON T1.DocEntry =T2.DocEntry AND T2.LineId =T0.U_ActivityLineNum 
    INNER JOIN [@AB_JOBSCH] T3 ON T3.DocEntry = T2.DocEntry
    WHERE T0.[DocEntry] = @DocEntry
END

--{ "Company":"CleaningExpress", "DocEntry":"1" }

GO

-- AE_SP022_Mobile_JS_ScheduledDayInfo 'CleaningExpress','3','04/26/2015'
ALTER PROC [dbo].[AE_SP022_Mobile_JS_ScheduledDayInfo]
@Company varchar(60), 
@DocEntry varchar(60),
@ScheduledDate Date
AS
BEGIN
    SELECT @DocEntry [DocEntry],@Company [Company] ,T0.U_ActivityLineNum [ActivityLineNum],T0.[U_DayLineNum] [DayLineNum] , T3.U_StartDate [StartDate],T3.U_EndDate [EndDate] , 
    T0.LineId [Sch3LineId] , T0.VisOrder [Sch3VisOrder],T1.[VisOrder] [Sch2VisOrder],T1.U_ScheduleDate [ScheduledDate] ,T1.U_DayStatus [DayStatus], T1.[U_CompletedDate] [CompletedDate] , 
    T1.[U_UserSign] [CompletedBy],T2.U_CleaningType [CleaningType], T2.U_Location [Location],T2.U_StartDate [Sch1StartDate],
     (SELECT dbo.[AE_FN002_ChangeTimeFormat] (T2.U_StartTime)) [StartTime], (SELECT dbo.[AE_FN002_ChangeTimeFormat] (T2.U_EndTime)) [EndTime],T2.U_Frequency [Frequency],
    (SELECT dbo.[AE_FN002_ChangeTimeFormatto24Hrs] (T0.U_ActualStartTime)) [ActualStartTime], (SELECT dbo.[AE_FN002_ChangeTimeFormatto24Hrs](T0.U_ActualEndTime)) [ActualEndTime] , T0.U_TaskStatus [TaskStatus], T0.U_Reason [Reason], T0.[U_AlertHQ] [AlertHQ],
    T0.U_NewSchedDate [NewScheduleDate], T1.U_ESignature [ESignature], T1.U_AtcEntry [AtcEntry],
    T3.U_OcrCode [ProcCode],'' PreparedBy ,'' PreparedDate,'' PreparedById
    FROM [@AB_JOBSCH3] T0
    INNER join [@AB_JOBSCH2] T1 ON T0.DocEntry =T1.DocEntry AND T1.LineId =T0.U_DayLineNum 
    INNER JOIN [@AB_JOBSCH1] T2 ON T1.DocEntry =T2.DocEntry AND T2.LineId =T0.U_ActivityLineNum 
    INNER JOIN [@AB_JOBSCH] T3 ON T3.DocEntry = T2.DocEntry
    WHERE T0.[DocEntry] = @DocEntry AND T1.[U_ScheduleDate] = @ScheduledDate
END

--{ "Company":"CleaningExpress", "DocEntry":"1", "ScheduledDate" : "04/01/2015" }

GO


--AE_SP023_Mobile_INSQA_GetProject 'CleaningExpress','manager',''
ALTER PROC [dbo].[AE_SP023_Mobile_INSQA_GetProject]
@Company varchar(60),
@UserName varchar(100),
@UserRole varchar(100)
AS
BEGIN
IF(@UserRole <> 'Sales')
BEGIN
	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) 
	WHERE T0.[DimCode] = 1 and  ISNULL(T0.[Active],'N') = 'Y' AND 
	(T0.[U_AB_MUser1]= @UserName or T0.[U_AB_MUser2]= @UserName or T0.[U_AB_MUser3]= @UserName)
	ORDER BY T0.[PrcCode] 
END
ELSE
BEGIN
	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) WHERE IsNull(Active,'N') = 'Y'
END
END

--{
--"sCompany":"CleaningExpress",
--"sCurrentUserName":"manager",
--"sUserRole":""
--}

GO

-- AE_SP024_Mobile_INSQA_ListofMSC 'CleaningExpress','8NAPIER'
ALTER PROC [dbo].[AE_SP024_Mobile_INSQA_ListofMSC]
@Company varchar(60), 
@ProjectCode varchar(60)
AS
BEGIN

--Declare @ProjectName varchar(500)

--Set @ProjectName = (Select PrcName   from OPRC where prcCode = @ProjectCode )

select DocEntry ,U_DocDate [DocDate] , U_MktSegment [MktSegment] from [@AB_MKTSGTCHK] Where U_Project = @ProjectCode

END

--{
--"Company":"CleaningExpress",
--"ProjectCode":"8NAPIER"
--}

GO

--AE_SP025_Mobile_INSQA_ViewMSCDetails 'CleaningExpress','1'
ALTER PROC [dbo].[AE_SP025_Mobile_INSQA_ViewMSCDetails]
@Company varchar(60), 
@DocEntry varchar(60)
AS
BEGIN
	select --T0.DocEntry , T0.U_DocDate [DocDate], 
	U_Project [Project], U_MktSegment [MKTSegment], U_Remarks [Remarks], U_ESignature [ESignature],
	U_AtcEntry [AttEntry], 
	--T1.LineId, T1. VisOrder , T1.U_BaseEntry [BaseEntry], T1.U_BaseLineNum [BaseLineNum],
	U_Category [Category],	[U_Item] [Item],
	(Case when [U_Rating1] = 'Y' Then 1 
	When [U_Rating2] = 'Y' Then 2 
	When [U_Rating3] = 'Y' Then 3
	When [U_Rating4] = 'Y' Then 4
	When [U_Rating5] = 'Y' Then 5 End) [Rating] 
	from [@AB_MKTSGTCHK] T0 INNER JOIN [@AB_MKTSGTCHK1] T1 ON T1.DocEntry = T0.DocEntry
	where T0.DocEntry = @DocEntry
	Order by LineId asc
END

--{
--"Company":"CleaningExpress",
--"DocEntry":"1"
--}

GO

-- AE_SP026_Mobile_INSQA_GetMarketingSegments 'CleaningExpress'
ALTER PROC [dbo].[AE_SP026_Mobile_INSQA_GetMarketingSegments]
@Company varchar(60)
AS
BEGIN
    Select DocEntry [BaseEntry] ,U_MktSegment [MktSegment] from [@AB_MKTSGTTEMPLATE] order by U_MktSegment asc
END

--{
--"Company":"CleaningExpress"
--}

GO

 -- AE_SP027_Mobile_INSQA_GetCategoryItems 'CleaningExpress','3'
ALTER PROC [dbo].[AE_SP027_Mobile_INSQA_GetCategoryItems]
@Company varchar(60),
@DocEntry Varchar(60)
AS
BEGIN
    Select DocEntry,LineId [BaseLineNum], U_Category [Category], U_Item [Item] from [@AB_MKTSGTTEMPLATE1] 
    Where DocEntry = @DocEntry  order by U_Sort asc
END

--{
--"Company":"CleaningExpress",
--"DocEntry":"1"
--}

GO

--AE_SP028_Mobile_GSLGardener_GetProject 'CleaningExpress','manager','Sales'
ALTER PROC [dbo].[AE_SP028_Mobile_GSLGardener_GetProject]
@Company varchar(60),
@UserName varchar(100),
@UserRole varchar(100)
AS
BEGIN
IF(@UserRole <> 'Sales')
BEGIN
	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) 
	WHERE T0.[DimCode] = 1 and  ISNULL(T0.[Active],'N') = 'Y' AND 
	(T0.[U_AB_MUser1]= @UserName or T0.[U_AB_MUser2]= @UserName or T0.[U_AB_MUser3]= @UserName)
	ORDER BY T0.[PrcCode] 
END
ELSE
BEGIN
	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) WHERE IsNull(Active,'N') = 'Y'
END
END

--{
--"sCompany":"CleaningExpress",
--"sCurrentUserName":"manager",
--"sUserRole":""
--}


GO

--AE_SP029_Mobile_GSLGardener_ListofGardener 'CleaningExpress','8NAPIER'
ALTER PROC [dbo].[AE_SP029_Mobile_GSLGardener_ListofGardener]
@Company varchar(60), 
@ProjectCode varchar(60)
AS
BEGIN

select DocEntry,U_DocDate [DocDate], U_Supervisor [SupervisorName], U_Client [ClientName] from [@AB_GARDENERCHK] where U_Project = @ProjectCode

END

--{
--"Company":"CleaningExpress",
--"ProjectCode":"8NAPIER"
--}

GO

--AE_SP030_Mobile_GSLGardener_ViewGardenerDetails 'CleaningExpress','2'
ALTER PROC [dbo].[AE_SP030_Mobile_GSLGardener_ViewGardenerDetails]
@Company varchar(60), 
@DocEntry varchar(60)
AS
BEGIN
	SELECT T0.U_Project [Project] ,T0.DocEntry , T0. U_Reference [Reference], T0.U_TimeIn [TimeIn], T0.U_TimeOut [TimeOut] , T0.U_IssuedDate [IssuedDate] , 
	T0. U_ReceivedDate [ReceivedDate], T0. U_Supervisor [SupervisorName], T0. U_SupervisorSign [SupervisorSign], 
	T0.U_SignedDate1 [SignedDate1], T0.U_Client [ClientName], T0.U_ClientSign [ClientSign], T0. U_SignedDate2 [SignedDate2], T0.U_AtcEntry [AtcEntry]
	, T1.U_Description [Description],T1.U_Done [Done],T1.U_Remark [Remark]
	FROM  [@AB_GARDENERCHK] T0 INNER JOIN [@AB_GARDENERCHK1] T1 ON T1.DocEntry = T0.DocEntry
	WHERE T0.DocEntry = @DocEntry
END

--{
--"Company":"CleaningExpress",
--"DocEntry":"1"
--}

GO

--AE_SP031_Mobile_GSLGardener_GetTemplate 'CleaningExpress'
ALTER PROC [dbo].[AE_SP031_Mobile_GSLGardener_GetTemplate]
@Company varchar(60)
AS
BEGIN
   SELECT U_Question [Question] FROM [@AB_GARDENERTEMPLINE] WHERE ISNULL(U_Active,'N') = 'Y' 
   AND U_Question != ''
   ORDER BY U_Sort ASC
END

--{
--"Company":"CleaningExpress"
--}

GO

--AE_SP032_Mobile_GSLLandscape_GetProject 'CleaningExpress','manager',''
ALTER PROC [dbo].[AE_SP032_Mobile_GSLLandscape_GetProject]
@Company varchar(60),
@UserName varchar(100),
@UserRole varchar(100)
AS
BEGIN
IF(@UserRole <> 'Sales')
BEGIN
	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) 
	WHERE T0.[DimCode] = 1 and  ISNULL(T0.[Active],'N') = 'Y' AND 
	(T0.[U_AB_MUser1]= @UserName or T0.[U_AB_MUser2]= @UserName or T0.[U_AB_MUser3]= @UserName)
	ORDER BY T0.[PrcCode] 
END
ELSE
BEGIN
	SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WITH (NOLOCK) WHERE IsNull(Active,'N') = 'Y'
END
END

--{
--"sCompany":"CleaningExpress",
--"sCurrentUserName":"manager",
--"sUserRole":""
--}


GO

--AE_SP033_Mobile_GSLLandscape_ListofLandscape 'CleaningExpress','7PALMS'
ALTER PROC [dbo].[AE_SP033_Mobile_GSLLandscape_ListofLandscape]
@Company varchar(60), 
@ProjectCode varchar(60)
AS
BEGIN

select DocEntry,U_DocDate [DocDate], U_Supervisor [SupervisorName], U_Client [ClientName] from [@AB_LANDSCAPECHK] where U_Project = @ProjectCode

END

--{
--"Company":"CleaningExpress",
--"ProjectCode":"7PALMS"
--}

GO

-- AE_SP034_Mobile_GSLLandscape_ViewLandscape 'CleaningExpress','1'
ALTER PROC [dbo].[AE_SP034_Mobile_GSLLandscape_ViewLandscape]
@Company varchar(60), 
@DocEntry varchar(60)
AS
BEGIN
   	SELECT T0.U_Project [Project] ,T0.DocEntry , T0.U_Reference [Reference], T0.U_TimeIn [TimeIn], T0.U_TimeOut [TimeOut] , T0.U_IssuedDate [IssuedDate] , 
	T0. U_ReceivedDate [ReceivedDate], T0. U_Supervisor [SupervisorName], T0. U_SupervisorSign [SupervisorSign], 
	T0.U_SignedDate1 [SignedDate1], T0.U_Client [ClientName], T0.U_ClientSign [ClientSign], T0. U_SignedDate2 [SignedDate2], T0.U_AtcEntry [AtcEntry]
	, T1.U_Description [Description],T1.U_Done [Done],T1.U_Remark [Remark]
	FROM  [@AB_LANDSCAPECHK] T0 INNER JOIN [@AB_LANDSCAPECHK1] T1 ON T1.DocEntry = T0.DocEntry
	WHERE T0.DocEntry = @DocEntry
END

--{
--"Company":"CleaningExpress",
--"DocEntry":"1"
--}

GO

-- AE_SP035_Mobile_GSLLandscape_GetTemplate 'CleaningExpress'
ALTER PROC [dbo].[AE_SP035_Mobile_GSLLandscape_GetTemplate]
@Company varchar(60)
AS
BEGIN
   SELECT U_Question [Question] FROM [@AB_LANDSCAPTEMPLINE] WHERE ISNULL(U_Active,'N') = 'Y'  
   AND U_Question != ''
   ORDER BY U_Sort ASC
    
END

--{
--"Company":"CleaningExpress"
--}

GO


--AE_SP036_Mobile_ServiceReport_GetProject 'CleaningExpress','manager','SAles'
Alter PROC [dbo].[AE_SP036_Mobile_ServcieReport_GetProject]
@Company varchar(60),
@UserName varchar(100),
@UserRole varchar(100)
AS
BEGIN

	select T0.CardCode [PrcCode],T0.CardName [PrcName]  from OCRD T0 where CardType = 'C' and IsNull(frozenFor,'N') = 'N'
Order by T0.CardCode asc


END

--{
--"sCompany":"CleaningExpress",
--"sCurrentUserName":"manager",
--"sUserRole":""
--}


GO


--AE_SP037_Mobile_ServiceReport_ListofServiceReport 'CleaningExpress','CEPC10003'
ALTER PROC [dbo].[AE_SP037_Mobile_ServiceReport_ListofServiceReport]
@Company varchar(60), 
@ProjectCode varchar(60)
AS
BEGIN

select DocEntry,U_DocDate [DocDate], U_Supervisor [SupervisorName], U_Client [ClientName] from [@AB_PESTRPT] where U_Project = @ProjectCode

END

--{
--"Company":"CleaningExpress",
--"ProjectCode":"CEPC10003"
--}

GO

--AE_SP038_Mobile_ServiceReport_ViewServiceReport 'CleaningExpress', '1'

ALTER PROC [dbo].[AE_SP038_Mobile_ServiceReport_ViewServiceReport]
@Company varchar(60), 
@DocEntry varchar(60)
AS
BEGIN
	SELECT T0.[U_DocDate] DocDate,T0.[U_Project] Project,T0.[U_Address] [Address],T0.[U_Block] Block,T0.[U_Unit] Unit,T0.[U_Postal] Postal,T0.[U_ReportOth] ReportOth,
	T0.[U_ACommonArea] ACommonArea,T0.[U_Abuilding] Abuilding,T0.[U_ACorridor] ACorridor,T0.[U_AGarden] AGarden,T0.[U_ADrainage] ADrainage,T0.[U_APlayground] APlayground,
	T0.[U_ABinCentre] ABinCentre,T0.[U_ACarPark] ACarPark,T0.[U_ALightning] ALightning,T0.[U_AElectrical] AElectrical,T0.[U_AStoreroom] AStoreroom,T0.[U_ARooftop] ARooftop,
	T0.[U_AManhole] AManhole,T0.[U_ARiser] ARiser,T0.[U_AOffice] AOffice,T0.[U_ACanteen] ACanteen,T0.[U_AKitchen] AKitchen,T0.[U_ACabinet] ACabinet,T0.[U_AGullyTrap] AGullyTrap,
	T0.[U_AOthers] AOthers,T0.[U_AOthersDesc] AOthersDesc,T0.[U_Scope] Scope,T0.[U_IHouseKeeping] IHouseKeeping,T0.[U_ISanitation] ISanitation,T0.[U_IStructural] IStructural,
	T0.[U_IOthers] IOthers,T0.[U_IOthersDesc] IOthersDesc,T0.[U_IComments] IComments,T0.[U_Supervisor] Supervisor,T0.[U_SupervisorSign] SupervisorSign,
	T0.[U_Client] Client,T0.[U_ClientSign] ClientSign,T0.[U_Feedback] Feedback,T0.[U_AtcEntry] AtcEntry,T0.[U_Report] Report,
	(SELECT dbo.[AE_FN002_ChangeTimeFormatto24Hrs] (T0.[U_TimeIn])) TimeIn,(SELECT dbo.[AE_FN002_ChangeTimeFormatto24Hrs] (T0.[U_TimeOut]))[TimeOut],
	T0.[U_AToilet] AToilet,T1.[U_Description] [Description],T1.[U_DescriptionOth] DescriptionOth,T1.[U_Include] [Include],T1.[U_Active] Active,T1.[U_Nonactive] Nonactive,
	T1.[U_TFogging] TFogging,T1.[U_TMisting] TMisting,T1.[U_TResidual] TResidual,T1.[U_TBaits] TBaits,T1.[U_TDusting] TDusting,T1.[U_TTraps] TTraps,T1.[U_TOthers] TOthers,
	T1.[U_Location] Location,T1.[U_IGS] IGS,T1.[U_AGS] AGS,T2.[U_Pesticide] Pesticide,T2.[U_Quantity] Quantity 
	FROM [@AB_PESTRPT] T0 INNER JOIN [@AB_PESTRPT1] T1 ON T1.DocEntry = T0.DocEntry INNER JOIN [@AB_PESTRPT2] T2 ON T2.DocEntry = T1.DocEntry  
	WHERE T0.[DocEntry] = @DocEntry
END

--{
--"Company":"CleaningExpress",
--"DocEntry":"1"
--}

GO

-- AE_SP039_Mobile_ServiceReport_GetTemplate 'CleaningExpress'
ALTER PROC [dbo].[AE_SP039_Mobile_ServiceReport_GetTemplate]
@Company varchar(60)
AS
BEGIN
        SELECT U_Question [Question] FROM [@AB_PESTTEMPLATE] WHERE ISNULL(U_Active,'N') = 'Y' ORDER BY U_Sort ASC
END

--{
--"Company":"CleaningExpress"
--}

GO

-- AE_SP040_Mobile_ServiceReport_GetScopeofWork 'CleaningExpress','CEPC10003'
ALTER PROC [dbo].[AE_SP040_Mobile_ServiceReport_GetScopeofWork]
@Company varchar(60),
@Project Varchar(60)
AS
BEGIN
select T1.Address ,T1.Block ,T1.StreetNo [Unit],T1.ZipCode [Postal] ,T1.Street [Street]
,T0.U_AB_Scope [ScopeofWork]   from OCRD T0
INNER JOIN CRD1 T1 ON T0.CardCode =T1.CardCode and T0.BillToDef =T1.Address  
where CardType ='C' and T1.AdresType ='B' And T0.CardCode = @Project AND IsNull(T0.frozenFor,'N') = 'N'
END

--{
--"Company":"CleaningExpress",
--"Project" : "CEPC10003"
--}

GO

GO

