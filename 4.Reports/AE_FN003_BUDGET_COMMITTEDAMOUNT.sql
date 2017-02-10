USE [PWCL]
GO
/****** Object:  UserDefinedFunction [dbo].[AE_FN003_BUDGET_COMMITTEDAMOUNT]    Script Date: 29/06/2016 03:30:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--AE_SP005_BUDGET_COMMITTEDAMOUNT'PWCL',2014,'False','IFS 717','71111100'

--select [dbo].[AE_FN003_BUDGET_COMMITTEDAMOUNT]('2014','BU','IFS 717','71111100')

ALTER function [dbo].[AE_FN003_BUDGET_COMMITTEDAMOUNT]
(
@Year varchar(10),
@Cat varchar(10),
@BU varchar(100),
@Project varchar(100),
@GLCode varchar(100)
)
returns decimal(19,2)
as
begin

Declare @Column1 as decimal(19,2)
Declare @Column2 as decimal(19,2)
Declare @Column3 as decimal(19,2)
Declare @Column4 as decimal(19,2)
Declare @Column5 as decimal(19,2)
Declare @Column6 as decimal(19,2)
Declare @Column7 as decimal(19,2)
Declare @Column8 as decimal(19,2)
Declare @Dimension Varchar(100)
   
if @cat = 'PRJ'
 begin
  set @Dimension = @Project
 end
else
 begin
  set @Dimension = @BU
 end

   --- Add PO Draft with status Open - manually -save as draft
select @Column1 = isnull((SELECT sum(T1.LineTotal ) LineTotal
 FROM ODRF T0  INNER JOIN DRF1 T1 ON T0.[DocEntry] = T1.[DocEntry] 
left outer join  OWDD T2 on T0.DocEntry = T2.DocEntry 
left outer join   OWTM T3 on T2.[WtmCode] = T3.[WtmCode] 
where T1.LineStatus  = 'O' and isnull(T2.[Status],'') = '' and T0.DocStatus = 'O' and T0.ObjType in ('22','1470000113')
and cast(year(T0.DocDate) as varchar) = @Year and isnull(T3.[Active],'Y') = 'Y' and
(case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode
),0) , 

--- Add PO Draft with status Open  - Pending for Approval / Approved but not converted to PO
@Column2 = isnull((SELECT sum(T1.LineTotal) LineTotal  
 FROM ODRF T0  INNER JOIN DRF1 T1 ON T0.[DocEntry] = T1.[DocEntry] 
left outer join  OWDD T2 on T0.DocEntry = T2.DocEntry 
left outer join   OWTM T3 on T2.[WtmCode] = T3.[WtmCode]
where T1.LineStatus = 'O' and isnull(T2.[Status],'') <> 'N' and T0.DocStatus = 'O' and T0.ObjType in ('22','1470000113')
and cast(year(T0.DocDate) as varchar) =@Year and isnull(T3.[Active],'') = 'Y'
and (case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode
 ),0)  ,

  --- Add PR Document with status Open
@Column3 = isnull((select sum( case when isnull(T2.LineTotal,0) = 0 then  T1.LineTotal  else 0 end  )  from OPRQ T0 join PRQ1 T1 on T0.DocEntry = T1.DocEntry 
left outer join DRF1 T2 on T2.BaseEntry = T1.DocEntry and T2.BaseLine = T1.LineNum and T2.BaseType = '1470000113'
left outer join ODRF T3 on T2.DocEntry = T3.Docentry and T3.DocStatus = 'O'
where T1.LineStatus = 'O' and T0.DocStatus = 'O'
and cast(year(T0.DocDate) as varchar) = @Year and
(case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode
),0) ,

--- Add PO Document with status Open
@Column4 = isnull((select sum(T1.LineTotal) LineTotal  from OPOR T0 join POR1 T1 on T0.DocEntry = T1.DocEntry where T1.LineStatus = 'O' and T0.DocStatus = 'O'
and cast(year(T0.DocDate) as varchar) =@Year 
and (case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode
),0) ,

--- Add PO Document with status Closed But GRN in Open Status
@Column5 = isnull((SELECT sum(T1.[LineTotal]) LineTotal FROM OPOR T0  INNER JOIN POR1 T1 ON T0.[DocEntry] = T1.[DocEntry] join 
 PDN1 T2 on T2.BaseEntry = T1.DocEntry and T2.BaseLine = T1.LineNum  INNER JOIN OPDN T3 ON T2.[DocEntry] = T3.[DocEntry] WHERE T2.[LineStatus] = 'O'
 and cast(year(T0.DocDate) as varchar) =@Year 
 and (case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
 and isnull(T1.AcctCode ,'') = @GLCode
 ),0) ,

--- Less PO Draft with status Open  - Approval status Rejected
@Column6 = isnull((SELECT sum(T1.LineTotal) LineTotal  
 FROM ODRF T0  INNER JOIN  DRF1 T1 ON T0.[DocEntry] = T1.[DocEntry] 
left outer join  OWDD T2 on T0.DocEntry = T2.DocEntry 
left outer join   OWTM T3 on T2.[WtmCode] = T3.[WtmCode]
where T1.LineStatus = 'O' and isnull(T2.[Status],'') = 'N' and T0.DocStatus = 'O' and T0.ObjType in ('22','1470000113')
and cast(year(T0.DocDate) as varchar) =@Year and isnull(T3.[Active],'Y') = 'Y'
and (case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode
),0) ,

--- Less PO Document with status Cancel  - User manually Cancel
@Column7 =  isnull((select sum(T1.LineTotal) LineTotal   from OPOR T0 join POR1 T1 on T0.DocEntry = T1.DocEntry where T1.LineStatus = 'C' and T0.CANCELED = 'Y'
and cast(year(T0.DocDate) as varchar) =@Year 
and (case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode
),0) ,

--- Less PO Document with status Close  - User manually Close
@Column8  = isnull((select sum(T1.LineTotal) LineTotal   from OPOR T0 join POR1 T1 on T0.DocEntry = T1.DocEntry where T1.LineStatus = 'C' and T0.CANCELED = 'N' 
and T1.TargetType = -1
and cast(year(T0.DocDate) as varchar) =@Year 
and (case when @Cat = 'PRJ' then isnull(T1.Project,'') else  isnull(T1.U_AB_NONPROJECT ,'')  end) = @Dimension
and isnull(T1.AcctCode ,'') = @GLCode
),0) 

--return ((@Column1 + @Column2 + @Column3 + @Column4 ) - @Column5 - @Column6 - @Column7 )
return ( @Column2 + @Column3 + @Column4 + @Column5 )
end
