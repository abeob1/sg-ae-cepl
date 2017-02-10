
/****** Object:  UserDefinedFunction [dbo].[AE_FN002_ChangeTimeFormatto24Hrs]    Script Date: 04/27/2015 13:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Function [dbo].[AE_FN002_ChangeTimeFormatto24Hrs] (@24HRTime int)
Returns varchar(10)
AS
BEGIN
Declare @Date varchar(50)
Declare @Result varchar(50)
If (LEN(@24HRTime) = 3)
BEGIN
set @Date = '0'  + Convert(varchar(10),@24HRTime)
END
ELSE
BEGIN
Set @Date = @24HRTime
END

SET @Result =( LEFT(@Date,2) + ':' + RIGHT(@Date,2))

RETURN @Result
END
GO
/****** Object:  UserDefinedFunction [dbo].[AE_FN002_ChangeTimeFormat]    Script Date: 04/27/2015 13:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- SELECT dbo.[AE_FN002_ChangeTimeFormat] (100)
CREATE Function [dbo].[AE_FN002_ChangeTimeFormat] (@Time int)
Returns varchar(500)

AS
BEGIN

DECLARE @Result varchar(500)

Set @Result = (select case when len(@Time)=3 then  cast(cast('0' AS varchar)+CAST( LEFT (@Time ,1) AS varchar)AS varchar)+':'+ cast(RIGHT (@Time,2) AS varchar) +' AM'               
                  when LEN(@Time )=4 then CASE when LEFT (@Time,2)<13 then LEFT(@Time ,2)+':'+ RIGHT(@Time,2)+' AM'
                  else  CASE when len(cast(LEFT(@Time ,2)-12 AS varchar))=1 then '0'+
                  cast(LEFT(@Time ,2)-12 AS varchar)+':'+ cast(RIGHT(@Time ,2) as varchar)+' PM' else cast(LEFT(@Time ,2)-12 AS varchar)+':'+ cast(RIGHT(@Time ,2) as varchar)+' PM' end end
                  end)

RETURN @Result 

END
GO
/****** Object:  UserDefinedFunction [dbo].[AE_FN001_QUANTITYCHECKING_FORUPDATE]    Script Date: 04/27/2015 13:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select dbo.[AE_FN001_QUANTITYCHECKING_FORUPDATE]( '7PALMS','ASQWH','20150410',1203, 7,'MA2028')
CREATE FUNCTION [dbo].[AE_FN001_QUANTITYCHECKING_FORUPDATE](@BKTDocEntry varchar(10),@BKTLineId varchar(10),@LineNum Varchar(10), @Quantity NUMERIC (19,2), @ItemCode varchar(100))

RETURNS INT

AS

BEGIN

DECLARE @AvailQty NUMERIC(19,2)
DECLARE @ExistPRQty NUMERIC(19,2)
DECLARE @Result INT
DECLARE @BKTQTY NUMERIC(19,2)

SELECT @BKTQTY= SUM(U_AvailQty ) FROM [@AB_STKBGT1] WHERE DocEntry =@BKTDocEntry and LineId = @BKTLineId AND U_ItemCode =@ItemCode 

SELECT @ExistPRQty = SUM(Quantity ) FROM PRQ1  WHERE U_AB_BKTDocEntry = @BKTDocEntry and U_AB_BKTLineId = @BKTLineId and LineNum = @LineNum AND ItemCode  =@ItemCode 


--select @AvailQty = T1.Quantity ,
--@ExistPRQty = (SELECT TT0.U_AvailQty  FROM [@AB_STKBGT1] TT0 WITH(NOLOCK) INNER JOIN [@AB_STKBGT] TT1 WITH(NOLOCK) ON TT0.DocEntry = TT1.DocEntry 
--	WHERE LEFT(TT0.U_BgtMonth,4) = YEAR(T0.ReqDate) AND RIGHT(TT0.U_BgtMonth ,2) = MONTH(T0.ReqDate) 
--	AND TT1.U_OcrCode = T1.OcrCode AND TT0.U_ItemCode = T1.ItemCode)
--	from [OPRQ]  T0 inner join [PRQ1] T1 on T0.DocEntry =T1.DocEntry 
-- WHERE T0.DocEntry = @DocEntry
--    and T1.ItemCode = @ItemCode --AND ISNULL(U_AvailQty,0) > 0


IF @Quantity <= (@BKTQTY + @ExistPRQty) 
SET @Result= 1
ELSE
SET @Result= 0


RETURN @Result 

END
GO
/****** Object:  UserDefinedFunction [dbo].[AE_FN001_QUANTITYCHECKING]    Script Date: 04/27/2015 13:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--SELECT DBO.AE_FN001_QUANTITYCHECKING('7PALMS','ASQWH','20150409',0)
 --select dbo.[AE_FN001_QUANTITYCHECKING]( '7PALMS','ASQWH','20150410', 3,'MA2024')


CREATE FUNCTION [dbo].[AE_FN001_QUANTITYCHECKING](@ProjectCode VARCHAR(20),@WareHouseCode VARCHAR(40)
,@RequiredDate DATE,@Quantity NUMERIC (19,2), @ItemCode varchar(100))

RETURNS INT

AS

BEGIN

DECLARE @AvailQty NUMERIC(19,2)
DECLARE @Result INT

select @AvailQty = U_AvailQty  

from [@AB_STKBGT1] T0
inner join [@AB_STKBGT] T1 on T0.DocEntry =T1.DocEntry 

 WHERE U_OcrCode = @ProjectCode AND  U_WhsCode = @WareHouseCode AND 
    (RIGHT( U_BgtMonth,2) = MONTH(@RequiredDate) AND LEFT(U_BgtMonth,4) = YEAR(@RequiredDate ))
    and U_ItemCode = @ItemCode --AND ISNULL(U_AvailQty,0) > 0


IF @AvailQty >= @Quantity 
SET @Result= 1
ELSE
SET @Result= 0


RETURN @Result 

END
GO
