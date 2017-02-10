Imports System.Data.SqlClient
Imports System.Globalization

Module modJobSchedule

    Public sJobSchKey As String

    Function JobSchedule_FormLoad(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim sDocDate As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox
        Try
            sFuncName = "JobSchedule_FormLoad()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("matJobSch").Specific

            oForm.Freeze(True)

            sQueryString = "select COUNT( CAST(ISNULL(DocNum,0) AS INT))+1 [DocEntry] from [@AB_JOBSCH] "
            '' sQueryString = "select Top 1 NextNumber [DocEntry] from NNM1 where ObjectCode ='JobSchedule' and Locked ='N' "
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            oForm.Items.Item("txtDocNum").Specific.string = oRS.Fields.Item("DocEntry").Value
            oForm.Items.Item("txtDocEntr").Specific.string = oRS.Fields.Item("DocEntry").Value

            If Today.Month.ToString().Length = 1 And Today.Day.ToString().Length = 1 Then
                sDocDate = "0" & Today.Day & "0" & Today.Month.ToString() & Today.Year.ToString()
            ElseIf Today.Month.ToString().Length = 1 And Today.Day.ToString().Length = 2 Then
                sDocDate = Today.Day & "0" & Today.Month.ToString() & Today.Year.ToString()
            ElseIf Today.Month.ToString().Length = 2 And Today.Day.ToString().Length = 2 Then
                sDocDate = "0" & Today.Day & Today.Month.ToString() & Today.Year.ToString()
            End If

            oForm.Items.Item("txtDocDate").Specific.string = sDocDate
            oCombo = oForm.Items.Item("cmbStatus").Specific
            oCombo.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly
            oCombo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index)


            oForm.Items.Item("txtDocEntr").Click(SAPbouiCOM.BoCellClickType.ct_Regular)

            oForm.EnableMenu("1293", True)
            oForm.EnableMenu("1283", False)

            oMatrix.AddRow(1)
            oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.string = oMatrix.VisualRowCount

            oMatrix.AutoResizeColumns()

            If JobSchedule_EditableControls(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
            oForm.Freeze(False)

            oForm.DataBrowser.BrowseBy = "txtDocEntr"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_FormLoad = RTN_SUCCESS
        Catch ex As Exception
            oForm.Freeze(False)
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_FormLoad = RTN_ERROR
        End Try

    End Function

    Function JobSchedule_MatrixAddRow(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            sFuncName = "JobSchedule_MatrixAddRow()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oMatrix = oForm.Items.Item("matJobSch").Specific

            If (oMatrix.Columns.Item("V_6").Cells.Item(oMatrix.VisualRowCount).Specific.string <> "" Or oMatrix.Columns.Item("V_5").Cells.Item(oMatrix.VisualRowCount).Specific.string <> "") Then

                oForm.DataSources.DBDataSources.Item("@AB_JOBSCH1").Clear()

                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount

            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_MatrixAddRow = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_MatrixAddRow = RTN_ERROR
        End Try
    End Function

    Function JobSchedule_ChooseFromList(ByRef oForm As SAPbouiCOM.Form, ByRef pVal As SAPbouiCOM.ItemEvent, _
                                        ByVal sItemUID As String, ByRef sErrDesc As String) As Long


        Dim oDT As SAPbouiCOM.DataTable
        Dim OCFLEvent As SAPbouiCOM.ChooseFromListEvent
        Dim sOwnerName As String = String.Empty

        Try
            sFuncName = "JobSchedule_ChooseFromList()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            OCFLEvent = pVal

            Dim sCFL_ID As String
            sCFL_ID = OCFLEvent.ChooseFromListUID

            oDT = OCFLEvent.SelectedObjects
            Try

                sOwnerName = oDT.GetValue("firstName", 0)

                If (oDT.GetValue("middleName", 0)) <> "" Then
                    sOwnerName = sOwnerName & " " & oDT.GetValue("middleName", 0)
                End If

                If (oDT.GetValue("lastName", 0)) <> "" Then
                    sOwnerName = sOwnerName & " " & oDT.GetValue("lastName", 0)
                End If

                If (sItemUID = "txtApprBy") Then
                    oForm.Items.Item("txtAppByNa").Specific.string = sOwnerName

                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                    End If
                    oForm.Items.Item("txtApprBy").Specific.string = oDT.GetValue("empID", 0)

                ElseIf (sItemUID = "txtPreBy") Then
                    oForm.Items.Item("txtPreByNa").Specific.string = sOwnerName

                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                    End If
                    oForm.Items.Item("txtPreBy").Specific.string = oDT.GetValue("empID", 0)

                End If

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
                JobSchedule_ChooseFromList = RTN_SUCCESS
            Catch ex As Exception
                sErrDesc = ex.Message().ToString()
                Call WriteToLogFile(sErrDesc, sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                JobSchedule_ChooseFromList = RTN_SUCCESS
            End Try

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_ChooseFromList = RTN_SUCCESS
        End Try
    End Function

    Function JobSchedule_RightClickMenu(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMenus As SAPbouiCOM.Menus
        Dim oMenuItem As SAPbouiCOM.MenuItem

        Try

            sFuncName = "JobSchedule_RightClickMenu()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            ' Create menu popup MyUserMenu01 and add it to Tools menu
            Dim oCreationPackage As SAPbouiCOM.MenuCreationParams
            oCreationPackage = p_oSBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)
            oMenuItem = p_oSBOApplication.Menus.Item("1280") 'Data'
            oMenus = oMenuItem.SubMenus
            'Create sub menu MySubMenu1 and add it to popup MyUserMenu01
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "JDS"
            oCreationPackage.String = "Daily Schedule"
            oCreationPackage.Enabled = True
            If Not p_oSBOApplication.Menus.Exists("JDS") Then
                'If the menu already exists this code will fail
                oMenus.AddEx(oCreationPackage)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_RightClickMenu = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_RightClickMenu = RTN_SUCCESS
        End Try
    End Function

    Function JobSchedule_AddDocument(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim sQuery As String = String.Empty
        Dim oSQLConnection As SqlConnection = New SqlConnection
        Dim oSQLTran As SqlTransaction
        Dim oSQLCommand As SqlCommand = New SqlCommand
        Dim sConString As String = String.Empty
        Dim sPeriod As String = String.Empty
        Dim sSeries As String = String.Empty
        Dim oRS As SAPbobsCOM.Recordset

        Try
            sFuncName = "JobSchedule_AddDocument()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            sConString = String.Empty
            sConString = "Data Source=" & p_oDICompany.Server & ";Initial Catalog=" & p_oDICompany.CompanyDB & ";User ID=" & p_oDICompany.DbUserName & "; Password=" & p_oDICompany.DbPassword

            'Taking the Document Series from the DataBase 
            sQuery = "select AbsEntry from OFPR where F_RefDate <=convert(date,GETDATE (),103) and T_RefDate >=convert(date,GETDATE (),103)  "

            oRS.DoQuery(sQuery)

            sPeriod = oRS.Fields.Item(0).Value

            sQuery = "select Series  from NNM1 where ObjectCode ='JobSchedule' and Locked ='N' "

            oRS.DoQuery(sQuery)

            sSeries = oRS.Fields.Item(0).Value


            oSQLConnection = New SqlConnection(sConString)
            If oSQLConnection.State = ConnectionState.Closed Then
                oSQLConnection.Open()
            End If
            oSQLTran = oSQLConnection.BeginTransaction


            sQuery = "INSERT INTO [@AB_JOBSCH](DocEntry,DocNum,Period,Instance,Series,Handwrtten,Canceled,Object,LogInst,UserSign," & _
                " Transfered,Status,CreateDate,CreateTime,UpdateDate,UpdateTime,DataSource,U_DocDate,U_OcrCode,U_StartDate,U_EndDate " & _
                " ,U_PreparedBy,U_PreparedDate,U_ApprovedBy,U_ApprovedDate,U_DocStatus,U_PreparedByID,U_ApprovedByID  "

            sQuery += ") VALUES ("

            sQuery += "@DocEntry,@DocNum,@Period,@Instance,@Series,@Handwrtten,@Canceled,@Object,@LogInst,@UserSign,@Transfered,@Status " & _
                " ,@CreateDate,@CreateTime,@UpdateDate,@UpdateTime,@DataSource @DocDate,@OcrCode,@StartDate,@EndDate,@PreparedBy,@PreparedDate,@ApprovedBy" & _
                " ,@ApprovedDate,@DocStatus,@PreparedByID,@ApprovedByID );"

            oSQLCommand.Parameters.Add("@DocEntry", SqlDbType.VarChar).Value = oForm.Items.Item("txtDocEntry").string
            oSQLCommand.Parameters.Add("@DocNum", SqlDbType.VarChar).Value = oForm.Items.Item("txtDocNum").string
            oSQLCommand.Parameters.Add("@Period", SqlDbType.NVarChar).Value = sPeriod
            oSQLCommand.Parameters.Add("@Instance", SqlDbType.NVarChar).Value = "0"
            oSQLCommand.Parameters.Add("@Series", SqlDbType.NVarChar).Value = sSeries
            oSQLCommand.Parameters.Add("@Handwrtten", SqlDbType.NChar).Value = "N"
            oSQLCommand.Parameters.Add("@Canceled", SqlDbType.NChar).Value = "N"
            oSQLCommand.Parameters.Add("@Object", SqlDbType.NVarChar).Value = "JobSchedule"
            oSQLCommand.Parameters.Add("@LogInst", SqlDbType.NVarChar).Value = "NULL"
            oSQLCommand.Parameters.Add("@UserSign", SqlDbType.NVarChar).Value = p_oDICompany.UserSignature
            oSQLCommand.Parameters.Add("@Transfered", SqlDbType.NChar).Value = "N"
            oSQLCommand.Parameters.Add("@Status", SqlDbType.NChar).Value = "O"
            oSQLCommand.Parameters.Add("@CreateDate", SqlDbType.Date).Value = Today.Date
            oSQLCommand.Parameters.Add("@CreateTime", SqlDbType.Time).Value = Today.TimeOfDay
            oSQLCommand.Parameters.Add("@UpdateDate", SqlDbType.Date).Value = "NULL"
            oSQLCommand.Parameters.Add("@UpdateTime", SqlDbType.Time).Value = "NULL"
            oSQLCommand.Parameters.Add("@DataSource", SqlDbType.NChar).Value = "O"
            oSQLCommand.Parameters.Add("@DocDate", SqlDbType.Date).Value = oForm.Items.Item("txtDocDate").string
            oSQLCommand.Parameters.Add("@OcrCode", SqlDbType.NVarChar).Value = oForm.Items.Item("txtJobSite").string
            oSQLCommand.Parameters.Add("@StartDate", SqlDbType.Date).Value = oForm.Items.Item("txtStrDate").string
            oSQLCommand.Parameters.Add("@EndDate", SqlDbType.Date).Value = oForm.Items.Item("txtEndDate").string
            oSQLCommand.Parameters.Add("@PreparedBy", SqlDbType.NVarChar).Value = oForm.Items.Item("txtPreparedBy").string
            oSQLCommand.Parameters.Add("@PreparedDate", SqlDbType.Date).Value = oForm.Items.Item("txtPreDate").string
            oSQLCommand.Parameters.Add("@ApprovedBy", SqlDbType.NVarChar).Value = oForm.Items.Item("txtApprovedBy").string
            oSQLCommand.Parameters.Add("@ApprovedDate", SqlDbType.Date).Value = oForm.Items.Item("txtApprDat").string
            oSQLCommand.Parameters.Add("@DocStatus", SqlDbType.NChar).Value = "O"
            oSQLCommand.Parameters.Add("@PreparedByID", SqlDbType.NVarChar).Value = oForm.Items.Item("txtPreBy").string
            oSQLCommand.Parameters.Add("@ApprovedByID", SqlDbType.NVarChar).Value = oForm.Items.Item("txtApprBy").string

            oSQLCommand.ExecuteNonQuery()

            'For iDetailCount = 0 To oDTDetails.Rows.Count - 1 ''Step i + 1

            '    sQuery = "INSERT INTO SalesOrderDET( [OrderNumber] ,[OrderLine],[VisualOrder],[SalesOultlet] ,[ProductCode],[PrtOnRcpt] ,[Price]  ,[Quantity] ,[GrossAmount] ,[RevenueCenter] ,[LineText] "
            '    sQuery += ") VALUES ("
            '    sQuery += "@OrderNumber,@OrderLine,@VisualOrder,@SalesOultlet,@ProductCode,@PrtOnRcpt,@Price,@Quantity,@GrossAmount,@RevenueCenter,@LineText );"

            '    oSQLCommand = New SqlCommand
            '    If oSQLConnection.State = ConnectionState.Closed Then
            '        oSQLConnection.Open()
            '    End If

            '    oSQLCommand.Connection = oSQLConnection
            '    oSQLCommand.CommandText = sQuery
            '    oSQLCommand.Transaction = oSQLTran
            '    oSQLCommand.CommandTimeout = 180


            '    oSQLCommand.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = oDTDetails.Rows(iDetailCount)("OrderNumber").ToString()
            '    oSQLCommand.Parameters.Add("@OrderLine", SqlDbType.Int).Value = oDTDetails.Rows(iDetailCount)("OrderLine").ToString()
            '    oSQLCommand.Parameters.Add("@VisualOrder", SqlDbType.Int).Value = oDTDetails.Rows(iDetailCount)("VisualOrder").ToString()
            '    oSQLCommand.Parameters.Add("@SalesOultlet", SqlDbType.NVarChar).Value = oDTDetails.Rows(iDetailCount)("SalesOultlet").ToString()



            'oSQLCommand.ExecuteNonQuery()

            'Next

            oSQLTran.Commit()

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_AddDocument = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            oSQLTran.Rollback()
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_AddDocument = RTN_SUCCESS

        Finally
            oSQLConnection.Dispose()
            oSQLCommand.Dispose()
        End Try

    End Function

    Function JobSchedule_FillDailySchedule(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim sForthnightDeclare As String = String.Empty
        Dim sForthnightSQL As String = String.Empty

        Dim oMatrix, oMatrix_1 As SAPbouiCOM.Matrix
        Dim sStatus As String = String.Empty
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim sFrequency As String = String.Empty
        Dim oDT_DailySchedule As DataTable = Nothing
        Dim oDV_DailySchedule As DataView = Nothing
        oDT_DailySchedule = New DataTable
        Dim IRowCount As Integer = 0
        Dim oRow() As Data.DataRow = Nothing

        Dim sDateMin As String = String.Empty
        Dim sDateMax As String = String.Empty

        Try
            sFuncName = "JobSchedule_FillDailySchedule()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("matDSch").Specific
            oForm.Items.Item("matDSch").Enabled = False
            oMatrix_1 = oForm.Items.Item("matJobSch").Specific
            oCombo = oForm.Items.Item("cmbStatus").Specific
            sStatus = oCombo.Selected.Value
            p_dtTable = New DataTable


            For imjs As Integer = 1 To oMatrix_1.RowCount
                If String.IsNullOrEmpty(oMatrix_1.Columns.Item("V_6").Cells.Item(imjs).Specific.String) Then Continue For
                sFrequency = oMatrix_1.Columns.Item("V_0").Cells.Item(imjs).Specific.value
                sQueryString = String.Empty

                '' sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtStrDate").Specific.value & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtEndDate").Specific.value & "',103);"
                sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & GetDate(oMatrix_1.Columns.Item("V_4").Cells.Item(imjs).Specific.string, p_oDICompany) & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtEndDate").Specific.value & "',103);"
                '' MsgBox(oMatrix_1.Columns.Item("V_4").Cells.Item(imjs).Specific.string & "   " & oForm.Items.Item("txtStrDate").Specific.value)
                sDateMin = GetDate(oMatrix_1.Columns.Item("V_4").Cells.Item(imjs).Specific.string, p_oDICompany)
                sDateMax = GetDate(oForm.Items.Item("txtEndDate").Specific.String, p_oDICompany)

                Select Case sFrequency

                    Case "Once"
                        sQueryString += "SELECT convert(varchar, @MinDate, 102) [Date] "
                        ''  sQueryString = "SELECT convert(varchar,'" & sDateMin & "' , 102) [Date] "

                    Case "Daily"
                        sQueryString += "WITH Dates AS (SELECT [Date] = CONVERT(DATETIME,@MinDate) UNION ALL SELECT [Date] = DATEADD(DAY, 1, [Date]) FROM Dates WHERE " & _
                            "Date < @MaxDate ) SELECT convert(varchar,[Date],102) [Date] FROM Dates where Date <= @MaxDate OPTION (MAXRECURSION 1000) "
                        ''sQueryString = ";WITH Dates AS (SELECT [Date] = CONVERT(DATETIME,'" & sDateMin & "') UNION ALL SELECT [Date] = DATEADD(DAY, 1, [Date]) FROM Dates WHERE " & _
                        ''    "Date < '" & sDateMax & "' ) SELECT convert(varchar,[Date],102) [Date] FROM Dates where Date <= '" & sDateMax & "' OPTION (MAXRECURSION 1000) "

                    Case "Weekly"
                        'sQueryString += "WITH Dates AS (SELECT [Date] = CONVERT(DATETIME,@MinDate) UNION ALL SELECT [Date] = DATEADD(DAY, 7, [Date]) FROM Dates WHERE " & _
                        '    "Date < @MaxDate ) SELECT convert(varchar,[Date],102) [Date] FROM Dates where Date <= @MaxDate OPTION (MAXRECURSION 1000) "

                        sQueryString = "[AE_SP041_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','7'"

                    Case "Fortnightly"

                        'sQueryString += "WITH Dates AS (SELECT [Date] = CONVERT(DATETIME,@MinDate) UNION ALL SELECT [Date] = DATEADD(DAY, 14, [Date]) FROM Dates WHERE " & _
                        '    "Date < @MaxDate ) SELECT convert(varchar,[Date],102) [Date] FROM Dates where Date <= @MaxDate OPTION (MAXRECURSION 1000) "
                        sQueryString = "[AE_SP041_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','14'"

                    Case "Monthly"
                        '  sQueryString += " SELECT  TOP (DATEDIFF(MONTH, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(MONTH, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                        '" FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                        sQueryString = "[AE_SP042_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','1'"

                    Case "Quarterly"
                        ' sQueryString += " SELECT  TOP (DATEDIFF(QUARTER, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(QUARTER, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                        '" FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                        sQueryString = "[AE_SP042_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','1'"
                    Case "Yearly"
                        ' sQueryString += " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                        '" FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                        sQueryString = "[AE_SP042_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','1'"
                    Case "Bi-Monthly"
                        ' sQueryString += " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                        '" FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                        sQueryString = "[AE_SP044_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "'"
                    Case "Twice a Week"
                        ' sQueryString += " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                        '" FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                        sQueryString = "[AE_SP043_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','1'"
                    Case "Half-Yearly"
                        ' sQueryString += " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                        '" FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                        Dim dHyear As Double = 365 / 2
                        sQueryString = "[AE_SP041_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "', " & dHyear & " "



                End Select
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
                oRS.DoQuery(sQueryString)
                If ConvertRecordsetII(oRS, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
            Next imjs

            oDT_DailySchedule = p_dtTable

            oDV_DailySchedule = oDT_DailySchedule.DefaultView
            oDT_DailySchedule = New DataTable
            oDT_DailySchedule = oDV_DailySchedule.Table.DefaultView.ToTable(True, "Date", "Day")
            oDT_DailySchedule.DefaultView.Sort = "Date"
            oMatrix.Clear()
            For Each row As DataRowView In oDT_DailySchedule.DefaultView
                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(IRowCount + 1).Specific.value = IRowCount + 1
                oRow = oDT_DailySchedule.Select("Date = '" & row("Date") & "'")
                oRow(0)("Day") = IRowCount + 1
                oMatrix.Columns.Item("V_5").Cells.Item(IRowCount + 1).Specific.value = JobSchedule_GetDate(row("Date"), p_oDICompany)

                oCombo = oMatrix.Columns.Item("V_3").Cells.Item(IRowCount + 1).Specific
                oCombo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index)
                IRowCount += 1
            Next

            p_oDtDailySchedule = New DataTable
            p_oDtDailySchedule = oDT_DailySchedule


            ' ''If oRS.RecordCount > 0 Then

            ' ''    For IRowCount As Integer = 0 To oRS.RecordCount - 1
            ' ''        oMatrix.AddRow(1)
            ' ''        oMatrix.Columns.Item("V_-1").Cells.Item(IRowCount + 1).Specific.value = IRowCount + 1
            ' ''        oMatrix.Columns.Item("V_5").Cells.Item(IRowCount + 1).Specific.value = JobSchedule_GetDate(oRS.Fields.Item(0).Value, p_oDICompany)

            ' ''        oCombo = oMatrix.Columns.Item("V_3").Cells.Item(IRowCount + 1).Specific
            ' ''        oCombo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index)

            ' ''        oRS.MoveNext()

            ' ''    Next

            ' ''End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_FillDailySchedule = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_FillDailySchedule = RTN_ERROR
        End Try
    End Function

    Function JobSchedule_FillDailySchedule_Old(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim sForthnightDeclare As String = String.Empty
        Dim sForthnightSQL As String = String.Empty

        Dim oMatrix, oMatrix_1 As SAPbouiCOM.Matrix
        Dim sStatus As String = String.Empty
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim sFrequency As String = String.Empty
        Dim oDT_DailySchedule As DataTable = Nothing
        Dim oDV_DailySchedule As DataView = Nothing
        oDT_DailySchedule = New DataTable
        Dim IRowCount As Integer = 0
        Try
            sFuncName = "JobSchedule_FillDailySchedule()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("matDSch").Specific
            oForm.Items.Item("matDSch").Enabled = False
            oMatrix_1 = oForm.Items.Item("matJobSch").Specific
            oCombo = oForm.Items.Item("cmbStatus").Specific
            sStatus = oCombo.Selected.Value
            p_dtTable = New DataTable


            For imjs As Integer = 1 To oMatrix_1.RowCount
                If String.IsNullOrEmpty(oMatrix_1.Columns.Item("V_6").Cells.Item(imjs).Specific.String) Then Continue For
                sFrequency = oMatrix_1.Columns.Item("V_0").Cells.Item(imjs).Specific.value
                sQueryString = String.Empty

                sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtStrDate").Specific.value & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtEndDate").Specific.value & "',103);"
                Select Case sFrequency

                    Case "Once"
                        sQueryString += "SELECT convert(varchar, @MinDate, 102) [Date] "

                    Case "Daily"
                        sQueryString += " SELECT  TOP (DATEDIFF(DD, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(DD, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
              " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "

                    Case "Weekly"
                        sQueryString += " SELECT  TOP (DATEDIFF(WW, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(WW, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                      " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "

                    Case "Fortnightly"

                        sQueryString += "WITH Dates AS (SELECT [Date] = CONVERT(DATETIME,@MinDate) UNION ALL SELECT [Date] = DATEADD(DAY, 14, [Date]) FROM Dates WHERE " & _
                            "Date < @MaxDate ) SELECT convert(varchar,[Date],102) [Date] FROM Dates where Date <= @MaxDate OPTION (MAXRECURSION 45) "

                    Case "Monthly"
                        sQueryString += " SELECT  TOP (DATEDIFF(MONTH, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(MONTH, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                      " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "


                    Case "Quarterly"
                        sQueryString += " SELECT  TOP (DATEDIFF(QUARTER, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(QUARTER, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                       " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "

                    Case "Yearly"
                        sQueryString += " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                       " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "

                End Select
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
                oRS.DoQuery(sQueryString)
                If ConvertRecordsetII(oRS, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
            Next imjs

            oDT_DailySchedule = p_dtTable

            oDV_DailySchedule = oDT_DailySchedule.DefaultView
            oDT_DailySchedule = New DataTable
            oDT_DailySchedule = oDV_DailySchedule.Table.DefaultView.ToTable(True, "Date")
            oDT_DailySchedule.DefaultView.Sort = "Date"
            oMatrix.Clear()
            For Each row As DataRowView In oDT_DailySchedule.DefaultView
                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(IRowCount + 1).Specific.value = IRowCount + 1
                oMatrix.Columns.Item("V_5").Cells.Item(IRowCount + 1).Specific.value = JobSchedule_GetDate(oRS.Fields.Item(0).Value, p_oDICompany)

                oCombo = oMatrix.Columns.Item("V_3").Cells.Item(IRowCount + 1).Specific
                oCombo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index)
                IRowCount += 1
            Next




            ' ''If oRS.RecordCount > 0 Then

            ' ''    For IRowCount As Integer = 0 To oRS.RecordCount - 1
            ' ''        oMatrix.AddRow(1)
            ' ''        oMatrix.Columns.Item("V_-1").Cells.Item(IRowCount + 1).Specific.value = IRowCount + 1
            ' ''        oMatrix.Columns.Item("V_5").Cells.Item(IRowCount + 1).Specific.value = JobSchedule_GetDate(oRS.Fields.Item(0).Value, p_oDICompany)

            ' ''        oCombo = oMatrix.Columns.Item("V_3").Cells.Item(IRowCount + 1).Specific
            ' ''        oCombo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index)

            ' ''        oRS.MoveNext()

            ' ''    Next

            ' ''End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_FillDailySchedule_Old = RTN_ERROR

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_FillDailySchedule_Old = RTN_ERROR
        End Try
    End Function

    Function JobSchedule_FillTaskSchedule(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatJobSch As SAPbouiCOM.Matrix
        Dim oMatDailySch As SAPbouiCOM.Matrix
        Dim oMatTaskSch As SAPbouiCOM.Matrix
        Dim sStatus As String = String.Empty
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim sJobLineNum As String = String.Empty
        Dim sDSchLineNum As String = String.Empty
        Dim sFrequency As String = String.Empty
        Dim oDT_DailySchedule As New DataTable
        Dim IRowCount As Integer = 1
        Dim iRowNo As Integer = -1
        Dim iLoop As Integer = 1
        Dim oRow() As Data.DataRow = Nothing
        Dim sDateMin As String = String.Empty
        Dim sDateMax As String = String.Empty

        Try
            sFuncName = "JobSchedule_FillTaskSchedule()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oCombo = oForm.Items.Item("cmbStatus").Specific
            sStatus = oCombo.Selected.Value

            oMatJobSch = oForm.Items.Item("matJobSch").Specific
            oMatDailySch = oForm.Items.Item("matDSch").Specific
            oMatTaskSch = oForm.Items.Item("matTaskSch").Specific
            oForm.Items.Item("matTaskSch").Enabled = False
            oMatTaskSch.Clear()

            p_dtTable = New DataTable


            For iJobRow As Integer = 0 To oMatJobSch.RowCount - 1
                sQueryString = String.Empty
                If oMatJobSch.Columns.Item("V_6").Cells.Item(iJobRow + 1).Specific.string = "" Then Continue For
                ''sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtStrDate").Specific.value & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtEndDate").Specific.value & "',103);"
                sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & GetDate(oMatJobSch.Columns.Item("V_4").Cells.Item(iJobRow + 1).Specific.String, p_oDICompany) & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtEndDate").Specific.value & "',103);"
                
                sFrequency = oMatJobSch.Columns.Item("V_0").Cells.Item(iJobRow + 1).Specific.value
                '' sFrequency = oCombo.Selected.Description

                sDateMin = GetDate(oMatJobSch.Columns.Item("V_4").Cells.Item(iJobRow + 1).Specific.String, p_oDICompany)
                sDateMax = GetDate(oForm.Items.Item("txtEndDate").Specific.String, p_oDICompany)

                If sFrequency = "Daily" Then

                    sQueryString += "WITH Dates AS (SELECT [Date] = CONVERT(DATETIME,@MinDate) UNION ALL SELECT [Date] = DATEADD(DAY, 1, [Date]) FROM Dates WHERE " & _
                         "Date < @MaxDate ) SELECT convert(varchar,[Date],102) [Date] FROM Dates where Date <= @MaxDate OPTION (MAXRECURSION 1000) "

                ElseIf sFrequency = "Once" Then
                    sQueryString += "SELECT convert(varchar, @MinDate, 102) [Date] "

                ElseIf sFrequency = "Weekly" Then
                    'sQueryString += "WITH Dates AS (SELECT [Date] = CONVERT(DATETIME,@MinDate) UNION ALL SELECT [Date] = DATEADD(DAY, 7, [Date]) FROM Dates WHERE " & _
                    '        "Date < @MaxDate ) SELECT convert(varchar,[Date],102) [Date] FROM Dates where Date <= @MaxDate OPTION (MAXRECURSION 1000) "
                    sQueryString = "[AE_SP041_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','7'"
                ElseIf sFrequency = "Fortnightly" Then
                    'sQueryString += "WITH Dates AS (SELECT [Date] = CONVERT(DATETIME,@MinDate) UNION ALL SELECT [Date] = DATEADD(DAY, 14, [Date]) FROM Dates WHERE " & _
                    '          "Date < @MaxDate ) SELECT convert(varchar,[Date],102) [Date] FROM Dates where Date <= @MaxDate OPTION (MAXRECURSION 1000) "
                    sQueryString = "[AE_SP041_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','14'"

                ElseIf sFrequency = "Monthly" Then
                    'sQueryString += " SELECT  TOP (DATEDIFF(MONTH, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(MONTH, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                    '   " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                    sQueryString = "[AE_SP042_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','1'"
                ElseIf sFrequency = "Quarterly" Then
                    'sQueryString += " SELECT  TOP (DATEDIFF(QUARTER, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(QUARTER, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                    '   " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                    sQueryString = "[AE_SP042_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','1'"
                ElseIf sFrequency = "Yearly" Then
                    'sQueryString += " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                    '   " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                    sQueryString = "[AE_SP042_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','1'"
                ElseIf sFrequency = "Bi-Monthly" Then
                    'sQueryString += " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                    '   " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                    sQueryString = "[AE_SP044_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "'"
                ElseIf sFrequency = "Twice a Week" Then
                    'sQueryString += " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                    '   " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                    sQueryString = "[AE_SP043_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "','1'"
                ElseIf sFrequency = "Half-Yearly" Then
                    'sQueryString += " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                    '   " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b "
                    Dim dHyear As Double = 365 / 2
                    sQueryString = "[AE_SP041_JobScheduleDateCal]'" & sDateMin & "','" & sDateMax & "', " & dHyear & " "
                End If

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
                oRS.DoQuery(sQueryString)
                If ConvertRecordsetII(oRS, iJobRow + 1, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
            Next
            oDT_DailySchedule = p_dtTable

            ''oDV_DailySchedule = oDT_DailySchedule.DefaultView
            ''oDT_DailySchedule = oDV_DailySchedule.Table.DefaultView.ToTable(True, "Date", "Day", "RowNo")
            '' oDT_DailySchedule.DefaultView.Sort = "Date"

            For Each row As DataRowView In oDT_DailySchedule.DefaultView

                oMatTaskSch.AddRow(1)
                oMatTaskSch.Columns.Item("V_-1").Cells.Item(IRowCount).Specific.string = IRowCount

                ''If iRowNo <> row("RowNo") And iRowNo <> -1 Then
                ''    iLoop = 1
                ''End If
                oRow = p_oDtDailySchedule.Select("Date='" & row("Date") & "'")
                oMatTaskSch.Columns.Item("V_7").Cells.Item(IRowCount).Specific.string = oRow(0)("Day") 'iLoop 'row("Day").ToString.TrimStart("0"c)
                oMatTaskSch.Columns.Item("V_0").Cells.Item(IRowCount).Specific.string = row("RowNo")
                oMatTaskSch.Columns.Item("V_8").Cells.Item(IRowCount).Specific.string = JobSchedule_GetDate(row("Date"), p_oDICompany)

                oCombo = oMatTaskSch.Columns.Item("V_4").Cells.Item(oMatTaskSch.VisualRowCount).Specific
                oCombo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index)
                ''iRowNo = row("RowNo")
                IRowCount += 1
                ''iLoop += 1
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_FillTaskSchedule = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_FillTaskSchedule = RTN_SUCCESS
        End Try
    End Function

    Function JobSchedule_FillTaskSchedule_Old(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatJobSch As SAPbouiCOM.Matrix
        Dim oMatDailySch As SAPbouiCOM.Matrix
        Dim oMatTaskSch As SAPbouiCOM.Matrix
        Dim sStatus As String = String.Empty
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim sJobLineNum As String = String.Empty
        Dim sDSchLineNum As String = String.Empty
        Dim sFrequency As String = String.Empty



        Try
            sFuncName = "JobSchedule_FillTaskSchedule()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oCombo = oForm.Items.Item("cmbStatus").Specific
            sStatus = oCombo.Selected.Value

            oMatJobSch = oForm.Items.Item("matJobSch").Specific
            oMatDailySch = oForm.Items.Item("matDSch").Specific
            oMatTaskSch = oForm.Items.Item("matTaskSch").Specific
            oForm.Items.Item("matTaskSch").Enabled = False
            oMatTaskSch.Clear()

            For iJobRow As Integer = 0 To oMatJobSch.RowCount - 1

                If oMatJobSch.Columns.Item("V_6").Cells.Item(iJobRow + 1).Specific.string = "" Then Continue For

                oCombo = oMatJobSch.Columns.Item("V_0").Cells.Item(iJobRow + 1).Specific
                sFrequency = oCombo.Selected.Description

                If sFrequency = "Daily" Then

                    sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtStrDate").Specific.value & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtEndDate").Specific.value & "',103);" & _
                        " SELECT  TOP (DATEDIFF(DD, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(DD, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                        " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; "

                ElseIf sFrequency = "Once" Then
                    sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtStrDate").Specific.value & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtStrDate").Specific.value & "',103);" & _
                        " SELECT  TOP 1 (DATEDIFF(DD, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(DD, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                        " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; "

                ElseIf sFrequency = "Weekly" Then
                    sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtStrDate").Specific.value & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtEndDate").Specific.value & "',103);" & _
                       " SELECT  TOP (DATEDIFF(WK, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(WK, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                       " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; "

                ElseIf sFrequency = "Monthly" Then
                    sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtStrDate").Specific.value & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtEndDate").Specific.value & "',103);" & _
                       " SELECT  TOP (DATEDIFF(MONTH, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(MONTH, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                       " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; "

                ElseIf sFrequency = "Quarterly" Then
                    sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtStrDate").Specific.value & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtEndDate").Specific.value & "',103);" & _
                       " SELECT  TOP (DATEDIFF(QUARTER, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(QUARTER, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                       " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; "

                ElseIf sFrequency = "Yearly" Then
                    sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtStrDate").Specific.value & "',103), @MaxDate DATE = CONVERT(DATE,'" & oForm.Items.Item("txtEndDate").Specific.value & "',103);" & _
                       " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " & _
                       " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; "
                End If

                For iDSchRow As Integer = 0 To oMatDailySch.RowCount - 1

                    oRS.DoQuery(sQueryString)
                    Dim iRowCount As Integer = oRS.RecordCount
                    'MsgBox(GetDate(oMatDailySch.Columns.Item("V_5").Cells.Item(iDSchRow + 1).Specific.string, p_oDICompany))
                    'MsgBox(GetDate(oRS.Fields.Item(0).Value, p_oDICompany))
                    For oRSCount As Integer = 0 To iRowCount - 1
                        If oMatDailySch.Columns.Item("V_5").Cells.Item(iDSchRow + 1).Specific.value = GetDate(oRS.Fields.Item(0).Value, p_oDICompany) Then

                            oMatTaskSch.AddRow(1)
                            oMatTaskSch.Columns.Item("V_-1").Cells.Item(oMatTaskSch.VisualRowCount).Specific.string = oMatTaskSch.VisualRowCount
                            oMatTaskSch.Columns.Item("V_7").Cells.Item(oMatTaskSch.VisualRowCount).Specific.string = iDSchRow + 1
                            oMatTaskSch.Columns.Item("V_0").Cells.Item(oMatTaskSch.VisualRowCount).Specific.string = iJobRow + 1

                            oCombo = oMatTaskSch.Columns.Item("V_4").Cells.Item(oMatTaskSch.VisualRowCount).Specific
                            oCombo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index)
                            Exit For
                        End If
                        oRS.MoveNext()
                    Next
                Next
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_FillTaskSchedule_Old = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_FillTaskSchedule_Old = RTN_SUCCESS
        End Try
    End Function

    Function JobSchedule_DeleteEmptyRows(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty


        Try
            sFuncName = "JobSchedule_DeleteEmptyRows()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            sQueryString = "DELETE FROM [@AB_JOBSCH1] WHERE DocEntry ='" & oForm.Items.Item("txtDocEntr").Specific.string & "' AND U_Location IS NULL;"

            sQueryString += " DELETE FROM [@AB_JOBSCH4] WHERE DocEntry ='" & oForm.Items.Item("txtDocEntr").Specific.string & "' AND U_Path IS NULL"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_DeleteEmptyRows = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_DeleteEmptyRows = RTN_ERROR
        End Try
    End Function

    Function JobSchedule_NonEditableControls(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox

        Try
            sFuncName = "JobSchedule_DeleteEmptyRows()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oMatrix = oForm.Items.Item("matJobSch").Specific
            oCombo = oForm.Items.Item("cmbStatus").Specific
            oForm.ActiveItem = "txtDocEntr"

            oForm.Items.Item("txtDocEntr").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
            oForm.Items.Item("txtDocNum").Enabled = False
            oForm.Items.Item("txtDocDate").Enabled = False
            oForm.Items.Item("txtJobSite").Enabled = False
            oForm.Items.Item("cmbStatus").Enabled = False
            oForm.Items.Item("txtStrDate").Enabled = False
            oForm.Items.Item("txtEndDate").Enabled = False
            oForm.Items.Item("txtPreBy").Enabled = False
            oForm.Items.Item("txtPreByNa").Enabled = False
            oForm.Items.Item("txtApprBy").Enabled = False
            oForm.Items.Item("txtAppByNa").Enabled = False
            oForm.Items.Item("txtPreDate").Enabled = False
            oForm.Items.Item("txtApprDat").Enabled = False
            oForm.Items.Item("matJobSch").Enabled = False

            ''If oCombo.Selected.Value = "O" Then
            ''    ''If JobSchedule_EditableControls(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            ''    oForm.Items.Item("cmbStatus").Enabled = True
            ''End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_NonEditableControls = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_NonEditableControls = RTN_ERROR
        End Try
    End Function

    Function JobSchedule_EditableControls(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
      
        Try
            sFuncName = "JobSchedule_EditableControls()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oMatrix = oForm.Items.Item("matJobSch").Specific
            '' oForm.ActiveItem = "txtDocNum"

            oForm.Items.Item("txtDocEntr").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
            oForm.Items.Item("txtDocNum").Enabled = True
            oForm.Items.Item("txtDocDate").Enabled = True
            oForm.Items.Item("txtJobSite").Enabled = True
            ''  oForm.Items.Item("cmbStatus").Enabled = True
            oForm.Items.Item("txtStrDate").Enabled = True
            oForm.Items.Item("txtEndDate").Enabled = True
            oForm.Items.Item("txtPreBy").Enabled = True
            '' oForm.Items.Item("txtPreByNa").Enabled = True
            oForm.Items.Item("txtApprBy").Enabled = True
            '' oForm.Items.Item("txtAppByNa").Enabled = True
            oForm.Items.Item("txtPreDate").Enabled = True
            oForm.Items.Item("txtApprDat").Enabled = True
            oForm.Items.Item("matJobSch").Enabled = True

            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                oForm.ActiveItem = "txtDocNum"
            End If

         
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_EditableControls = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_EditableControls = RTN_ERROR
        End Try
    End Function

    Function JobSchedule_Validation(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox
        Try

            sFuncName = "JobSchedule_Validation()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oMatrix = oForm.Items.Item("matJobSch").Specific
            oCombo = oForm.Items.Item("cmbStatus").Specific

            If oForm.Items.Item("txtDocNum").Specific.string = "" Then
                sErrDesc = "Document Number Field. Cannot be Blank!."
                oForm.Items.Item("txtDocNum").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oForm.Items.Item("txtDocDate").Specific.string = "" Then
                sErrDesc = "Document Date Field. Cannot be Blank!."
                oForm.Items.Item("txtDocDate").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oForm.Items.Item("txtJobSite").Specific.string = "" Then
                sErrDesc = "Job Site Field. Cannot be Blank!."
                oForm.Items.Item("txtJobSite").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oCombo.Value = "" Then
                sErrDesc = "Status Field. Cannot be Blank!."
                oForm.Items.Item("Status").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oForm.Items.Item("txtStrDate").Specific.string = "" Then
                sErrDesc = "Start Date Field. Cannot be Blank!."
                oForm.Items.Item("txtStrDate").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oForm.Items.Item("txtEndDate").Specific.string = "" Then
                sErrDesc = "End Date Field. Cannot be Blank!."
                oForm.Items.Item("txtEndDate").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oForm.Items.Item("txtPreBy").Specific.string = "" Then
                sErrDesc = "Prepared By ID Field. Cannot be Blank!."
                oForm.Items.Item("txtPreBy").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oForm.Items.Item("txtPreByNa").Specific.string = "" Then
                sErrDesc = "Prepared By Name Field. Cannot be Blank!."
                oForm.Items.Item("txtPreByNa").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oForm.Items.Item("txtPreDate").Specific.string = "" Then
                sErrDesc = "Prepared Date Field. Cannot be Blank!."
                oForm.Items.Item("txtPreDate").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oForm.Items.Item("txtApprBy").Specific.string = "" Then
                sErrDesc = "Approved By ID Field. Cannot be Blank!."
                oForm.Items.Item("txtApprBy").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oForm.Items.Item("txtAppByNa").Specific.string = "" Then
                sErrDesc = "Approved By Name Field. Cannot be Blank!."
                oForm.Items.Item("txtAppByNa").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oForm.Items.Item("txtApprDat").Specific.string = "" Then
                sErrDesc = "Approved Date Field. Cannot be Blank!."
                oForm.Items.Item("txtApprDat").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR

            ElseIf oMatrix.RowCount = 1 Then

                If oMatrix.Columns.Item("V_6").Cells.Item(1).Specific.string = "" Then
                    sErrDesc = "Location Field. Cannot be Blank!. RowNo : 1"
                    oMatrix.Columns.Item("V_6").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    Return RTN_ERROR
                ElseIf oMatrix.Columns.Item("V_5").Cells.Item(1).Specific.string = "" Then
                    sErrDesc = "Cleaning Type Field. Cannot be Blank!. RowNo : 1 "
                    oMatrix.Columns.Item("V_5").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    Return RTN_ERROR
                ElseIf oMatrix.Columns.Item("V_4").Cells.Item(1).Specific.string = "" Then
                    sErrDesc = "Start Date Field. Cannot be Blank!. RowNo : 1 "
                    oMatrix.Columns.Item("V_4").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    Return RTN_ERROR
                ElseIf oMatrix.Columns.Item("V_3").Cells.Item(1).Specific.string = "" Then
                    sErrDesc = "Start Time Field. Cannot be Blank!. RowNo : 1"
                    oMatrix.Columns.Item("V_3").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    Return RTN_ERROR
                ElseIf oMatrix.Columns.Item("V_2").Cells.Item(1).Specific.string = "" Then
                    sErrDesc = "End Time Field. Cannot be Blank!. RowNo : 1"
                    oMatrix.Columns.Item("V_2").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    Return RTN_ERROR
                ElseIf oCombo.Value = "" Then
                    sErrDesc = "Frequency Field. Cannot be Blank!. RowNo : 1"
                    oMatrix.Columns.Item("V_0").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    Return RTN_ERROR
                End If

            ElseIf oMatrix.RowCount > 0 Then

                For iRowCount As Integer = 1 To oMatrix.VisualRowCount - 1

                    oCombo = oMatrix.Columns.Item("V_0").Cells.Item(iRowCount).Specific

                    If (oMatrix.Columns.Item("V_6").Cells.Item(iRowCount).Specific.string <> "" Or _
                        oMatrix.Columns.Item("V_5").Cells.Item(iRowCount).Specific.string <> "" Or _
                        oMatrix.Columns.Item("V_4").Cells.Item(iRowCount).Specific.string <> "" Or _
                        oMatrix.Columns.Item("V_3").Cells.Item(iRowCount).Specific.string <> "") Then

                        If oMatrix.Columns.Item("V_6").Cells.Item(iRowCount).Specific.string = "" Then
                            sErrDesc = "Location Field. Cannot be Blank!. RowNo : " & iRowCount
                            oMatrix.Columns.Item("V_6").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            Return RTN_ERROR
                        ElseIf oMatrix.Columns.Item("V_5").Cells.Item(iRowCount).Specific.string = "" Then
                            sErrDesc = "Cleaning Type Field. Cannot be Blank!. RowNo : " & iRowCount
                            oMatrix.Columns.Item("V_5").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            Return RTN_ERROR
                        ElseIf oMatrix.Columns.Item("V_4").Cells.Item(iRowCount).Specific.string = "" Then
                            sErrDesc = "Start Date Field. Cannot be Blank!. RowNo : " & iRowCount
                            oMatrix.Columns.Item("V_4").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            Return RTN_ERROR
                        ElseIf oMatrix.Columns.Item("V_3").Cells.Item(iRowCount).Specific.string = "" Then
                            sErrDesc = "Start Time Field. Cannot be Blank!. RowNo : " & iRowCount
                            oMatrix.Columns.Item("V_3").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            Return RTN_ERROR
                        ElseIf oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string = "" Then
                            sErrDesc = "End Time Field. Cannot be Blank!. RowNo : " & iRowCount
                            oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            Return RTN_ERROR

                        ElseIf oCombo.Value = "" Then
                            sErrDesc = "Frequency Field. Cannot be Blank!. RowNo : " & iRowCount
                            oMatrix.Columns.Item("V_0").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            Return RTN_ERROR
                        End If
                    End If
                Next
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobSchedule_Validation = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_Validation = RTN_ERROR
        End Try
    End Function

    Function JobSchedule_CloseStatus(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oRs As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim sDocEntry As String = String.Empty


        Try
            sFuncName = "JobSchedule_CloseStatus()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oCombo = oForm.Items.Item("cmbStatus").Specific
            oRs = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oCombo.Select(1, SAPbouiCOM.BoSearchKey.psk_Index)

            sDocEntry = oForm.Items.Item("txtDocEntr").Specific.string

            sQueryString = "update [@AB_JOBSCH2] set U_DayStatus = (CASE WHEN U_DayStatus ='Open' THEN 'Canceled' ELSE U_DayStatus  END)WHERE DocEntry ='" & sDocEntry & "' "
            sQueryString += " update [@AB_JOBSCH3] set U_TaskStatus = (CASE WHEN U_TaskStatus ='Open' THEN 'Canceled' ELSE U_TaskStatus END)WHERE DocEntry ='" & sDocEntry & "'"
            sQueryString += " update [@AB_JOBSCH] set U_DocStatus = (CASE WHEN U_DocStatus ='O' THEN 'C' ELSE U_DocStatus END)WHERE DocEntry ='" & sDocEntry & "'"

            oRs.DoQuery(sQueryString)

            oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

            JobSchedule_CloseStatus = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobSchedule_CloseStatus = RTN_ERROR
        End Try
    End Function

    Public Function JobSchedule_GetDate(ByVal sDate As String, ByRef oCompany As SAPbobsCOM.Company) As String

        Dim dateValue As DateTime
        Dim DateString As String = String.Empty
        Dim sSQL As String = String.Empty
        Dim oRs As SAPbobsCOM.Recordset
        Dim sDatesep As String

        oRs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        sSQL = "SELECT DateFormat,DateSep FROM OADM"

        oRs.DoQuery(sSQL)

        If Not oRs.EoF Then
            sDatesep = oRs.Fields.Item("DateSep").Value

            Select Case oRs.Fields.Item("DateFormat").Value
                Case 0
                    If Date.TryParseExact(sDate, "dd" & sDatesep & "MM" & sDatesep & "yy", _
                       New CultureInfo("en-US"), _
                       DateTimeStyles.None, _
                       dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    ElseIf Date.TryParseExact(sDate, "yyyy" & sDatesep & "MM" & sDatesep & "dd", _
                   New CultureInfo("en-US"), _
                   DateTimeStyles.None, _
                   dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")

                    End If
                Case 1
                    If Date.TryParseExact(sDate, "yyyy" & sDatesep & "MM" & sDatesep & "dd", _
                       New CultureInfo("en-US"), _
                       DateTimeStyles.None, _
                       dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 2
                    If Date.TryParseExact(sDate, "MM" & sDatesep & "dd" & sDatesep & "yy", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 3
                    If Date.TryParseExact(sDate, "MM" & sDatesep & "dd" & sDatesep & "yyyy", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 4
                    If Date.TryParseExact(sDate, "yyyy" & sDatesep & "MM" & sDatesep & "dd", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 5
                    If Date.TryParseExact(sDate, "dd" & sDatesep & "MMMM" & sDatesep & "yyyy", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 6
                    If Date.TryParseExact(sDate, "yy" & sDatesep & "MM" & sDatesep & "dd", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case Else
                    DateString = dateValue.ToString("yyyyMMdd")
            End Select

        End If

        Return DateString

    End Function

End Module
