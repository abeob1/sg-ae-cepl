Module modDailySchedule

    Dim iDTCount As Integer = 0
    Public sDocEntry As String
    Public sLineNum As String

    Function DailySchedule_LoadScreen(ByRef oForm As SAPbouiCOM.Form, ByVal sErrDesc As String) As Long

        Dim oGrid As SAPbouiCOM.Grid
        Dim sQuery As String = String.Empty

        Try

            sFuncName = "Display_PurchaseRequest()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Startung Function", sFuncName)

            oForm.Freeze(True)
            ''CASE WHEN T1.U_DocStatus='O' THEN 'Open' WHEN T1.U_DocStatus='C' THEN 'Closed' ELSE '' END
            ' ''sQuery = "select DocNum ,LineId [Day Line Number], U_ScheduleDate [Scheduled Date] ,T0.U_CompletedDate [Complated Date] " & _
            ' ''    ", T0.[U_DayStatus] [Status] ,T0.U_UserSign [Completed By] ,T0.U_SupervisorSign  [E-Supervisor Signature] , T0.U_SupvrSignText [Supervisor text], " & _
            ' ''    "T0.U_ClientSign [E- Client Signature] , T0.U_ClintSignText [Client Text],T0.U_AtcEntry  [Attachment Entry] " & _
            ' ''    " from [@AB_JOBSCH2] T0 INNER JOIN [@AB_JOBSCH] T1 ON T0.DocEntry =T1.DocEntry where T0.DocEntry ='" & sDocEntry & "' "

            sQuery = "select  DocNum  , ROW_NUMBER() over (order by T0.U_ScheduleDate) as LineId,  T0.U_ScheduleDate [Scheduled Date] , T2.U_CompletedDate [Complated Date],  " & _
                "T2.U_DayStatus [Status] , T2.U_UserSign [Completed By] " & _
       "from [@AB_JOBSCH3] T0 INNER JOIN [@AB_JOBSCH] T1 ON T0.DocEntry =T1.DocEntry " & _
"join [@AB_JOBSCH2] T2 on T2.DocEntry = T0.DocEntry and T2.LineId = T0.U_DayLineNum " & _
" where T0.DocEntry = '" & sDocEntry & "' " & _
" group by T0.U_ScheduleDate,DocNum,T2.U_DayStatus , T2.U_UserSign, " & _
            "T2.U_CompletedDate "

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Query " & sQuery, sFuncName)

            oGrid = oForm.Items.Item("grdDisplay").Specific
            oGrid.DataTable = oForm.DataSources.DataTables.Add("DT1" & iDTCount)
            oGrid.DataTable.ExecuteQuery(sQuery)

            oGrid.Columns.Item(0).Visible = False
            oGrid.Columns.Item(1).Visible = False
            oGrid.Columns.Item(3).Visible = False
            oGrid.Columns.Item(5).Visible = False
            ''oGrid.Columns.Item(6).Visible = False
            ''oGrid.Columns.Item(7).Visible = False
            ''oGrid.Columns.Item(8).Visible = False
            ''oGrid.Columns.Item(9).Visible = False
            ''oGrid.Columns.Item(10).Visible = False

            For IntRow As Integer = 0 To oGrid.Columns.Count - 1
                oGrid.Columns.Item(IntRow).Editable = False
            Next IntRow

            Dim rowheader As SAPbouiCOM.RowHeaders
            rowheader = oGrid.RowHeaders
            rowheader.TitleObject.Caption = "#"

            For iRowCount = 0 To oGrid.Rows.Count - 1
                rowheader.SetText(iRowCount, iRowCount + 1)
            Next

            oGrid.AutoResizeColumns()

            oForm.EnableMenu("1281", False)
            oForm.EnableMenu("1283", False)

            oForm.Freeze(False)
            oForm.Refresh()
            oForm.Update()

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            DailySchedule_LoadScreen = RTN_SUCCESS
            iDTCount = iDTCount + 1
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            oForm.Freeze(False)
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            DailySchedule_LoadScreen = RTN_ERROR
        End Try

    End Function

    Function DailySchedule_OpenTaskSchedule(ByRef oForm As SAPbouiCOM.Form, ByVal iRowNum As Integer, ByRef sErrDesc As String) As Long

        Dim oGrid As SAPbouiCOM.Grid
        Dim sScheduleDate As String = String.Empty

        Dim sCompletedDate As String = String.Empty

        Dim sStatus As String = String.Empty
        Dim sCompletedBy As String = String.Empty

        Try

            sFuncName = "Display_PurchaseRequest()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Startung Function", sFuncName)

            oGrid = oForm.Items.Item("grdDisplay").Specific

            sScheduleDate = oGrid.DataTable.GetValue(2, iRowNum)
            sCompletedDate = oGrid.DataTable.GetValue(3, iRowNum)
            sStatus = oGrid.DataTable.GetValue(4, iRowNum)
            sCompletedBy = oGrid.DataTable.GetValue(5, iRowNum)

            sLineNum = oGrid.DataTable.GetValue(1, iRowNum)

            If TaskSchedule_LoadScreen(sScheduleDate, sCompletedDate, sStatus, sCompletedBy, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            DailySchedule_OpenTaskSchedule = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            oForm.Freeze(False)
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            DailySchedule_OpenTaskSchedule = RTN_ERROR
        End Try

    End Function

End Module
