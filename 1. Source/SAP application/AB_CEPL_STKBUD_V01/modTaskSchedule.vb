Module modTaskSchedule
    Dim oDBDSHeader, oDBDSDetail, oDBDSDetail2 As SAPbouiCOM.DBDataSource

    Function TaskSchedule_LoadScreen(ByVal sScheduleDate As String, ByVal sCompletedDate As String _
                                     , ByVal sStatus As String, ByVal sComplatedBy As String, ByRef sErrDesc As String)

        Dim oForm As SAPbouiCOM.Form = Nothing
        Dim oCombo As SAPbouiCOM.ComboBox
        Try
            sFuncName = "TaskSchedule_LoadScreen()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            LoadFromXML("TaskScedule.srf", p_oSBOApplication)

            oForm = p_oSBOApplication.Forms.Item("JTS")
            oForm.Visible = True

            oCombo = oForm.Items.Item("txtStatus").Specific

            oForm.Items.Item("folContent").Click(SAPbouiCOM.BoCellClickType.ct_Regular)

            oForm.Items.Item("txtSchDate").Specific.string = GetDate(CDate(sScheduleDate).ToString("yyyy.MM.dd"), p_oDICompany)
            If Not sCompletedDate Is Nothing Then
                oForm.Items.Item("txtComDate").Specific.string = GetDate(CDate(sCompletedDate).ToString("yyyy.MM.dd"), p_oDICompany)
            End If

            oCombo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index)

            oForm.Items.Item("txtCompBy").Specific.string = sComplatedBy

            If TaskSchedule_LoadValues(oForm, CDate(sScheduleDate).ToString("yyyyMMdd"), sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE

            oForm.EnableMenu("1281", False)
            oForm.EnableMenu("1283", False)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            TaskSchedule_LoadScreen = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            TaskSchedule_LoadScreen = RTN_SUCCESS
        End Try
    End Function

    Function TaskSchedule_LoadValues(ByRef oForm As SAPbouiCOM.Form, ByVal sScheduleDate As String, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatrixCon As SAPbouiCOM.Matrix
        Dim oMatrixAttach As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim sNewSchDate As String = String.Empty
        Dim oPic1 As SAPbouiCOM.PictureBox = Nothing
        Dim oPic2 As SAPbouiCOM.PictureBox = Nothing

        Try
            sFuncName = "TaskSchedule_LoadValues()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)


            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrixCon = oForm.Items.Item("matContent").Specific
            oMatrixAttach = oForm.Items.Item("matAttach").Specific

            oForm.Freeze(True)
            'Load the Content Details 
            sQueryString = "select T0.[LineId], T2.U_Location [Location] ,T2.U_CleaningType [CleaningType],T2.U_StartTime [Start Time] ,T2.U_EndTime [End Time] " & _
                " , isnull(T0.U_ActualStartTime,'')  [Actual Start Time],isnull(T0.U_ActualEndTime,'')  [Actual End Time],T0.U_TaskStatus [TaskStatus] ,T1.[U_DayStatus] [DayStatus] ,T0.U_Reason  [Reason] " & _
                " ,T0.U_NewSchedDate [NewSchDate],T0.U_AlertHQ [AlertHQ], isnull(T1.[U_CompletedDate],'') [CompletedDate], T1.[U_UserSign] [CompletedBy], T1.[U_SupervisorSign], T1.[U_ClientSign]  " & _
                ",T0.[U_CmplStartDate], T0.[U_CmplEndDate], T0.[U_CompletedBy], T0.[U_InspectedBy], T0.[U_VerifiedBy], T0.[U_Remarks], T0.[U_ScheduleDate] [ScheduleDate] " & _
                "from [@AB_JOBSCH3] T0 INNER JOIN [@AB_JOBSCH2] T1 ON T0.DocEntry =T1.DocEntry AND T1.LineId =T0.U_DayLineNum  " & _
                " INNER JOIN [@AB_JOBSCH1] T2 ON T1.DocEntry =T2.DocEntry AND T2.LineId =T0.U_ActivityLineNum   " & _
                " WHERE T0. DocEntry ='" & sDocEntry & "' AND CONVERT(DATE, T0.U_ScheduleDate,103) ='" & sScheduleDate & "'"


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)
            If oRS.RecordCount = 0 Then
                oForm.Freeze(False)
                TaskSchedule_LoadValues = RTN_SUCCESS
                Exit Try
            End If

            'MsgBox(oRS.Fields.Item("Actual End Time").Value)
            oCombo = oForm.Items.Item("txtStatus").Specific
            oCombo.Select(oRS.Fields.Item("DayStatus").Value, SAPbouiCOM.BoSearchKey.psk_ByValue)

            If Not String.IsNullOrEmpty(oRS.Fields.Item("CompletedDate").Value) Then
                sNewSchDate = GetDate(CDate(oRS.Fields.Item("CompletedDate").Value).ToString("yyyy.MM.dd"), p_oDICompany)
                If sNewSchDate <> "19000101" Then
                    oForm.Items.Item("txtComDate").Specific.string = GetDate(CDate(oRS.Fields.Item("CompletedDate").Value).ToString("yyyy.MM.dd"), p_oDICompany)
                End If
            End If

            oPic1 = oForm.Items.Item("picSign").Specific
            oPic1.Picture = oRS.Fields.Item("U_SupervisorSign").Value
            oPic1 = oForm.Items.Item("21").Specific
            oPic1.Picture = oRS.Fields.Item("U_ClientSign").Value
            ''oForm.Items.Item("picSign").Specific.value = oRS.Fields.Item("U_SupervisorSign").Value
            ''oForm.Items.Item("21").Specific.value = oRS.Fields.Item("U_ClientSign").Value


            If oRS.RecordCount > 0 Then
                p_oSBOApplication.StatusBar.SetText("Please Wait Values are Loading Content Details....", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                For iRowCount As Integer = 0 To oRS.RecordCount - 1

                    'Fill the  Details
                    oMatrixCon.AddRow()
                    oMatrixCon.Columns.Item("V_-1").Cells.Item(iRowCount + 1).Specific.string = iRowCount + 1
                    oMatrixCon.Columns.Item("V_10").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("LineId").Value
                    oMatrixCon.Columns.Item("V_9").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("Location").Value
                    oMatrixCon.Columns.Item("V_8").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("CleaningType").Value
                    oMatrixCon.Columns.Item("V_7").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("Start Time").Value
                    oMatrixCon.Columns.Item("V_6").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("End Time").Value
                    If oRS.Fields.Item("Actual Start Time").Value <> "0" Then
                        oMatrixCon.Columns.Item("V_5").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("Actual Start Time").Value
                    End If
                    If oRS.Fields.Item("Actual End Time").Value <> "0" Then
                        oMatrixCon.Columns.Item("V_4").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("Actual End Time").Value
                    End If
                    sNewSchDate = GetDate(CDate(oRS.Fields.Item("NewSchDate").Value).ToString("yyyy.MM.dd"), p_oDICompany)
                    If sNewSchDate <> "18991230" Then
                        oMatrixCon.Columns.Item("V_1").Cells.Item(iRowCount + 1).Specific.string = sNewSchDate
                    End If
                    oCombo = oMatrixCon.Columns.Item("V_3").Cells.Item(iRowCount + 1).Specific
                    oCombo.Select(oRS.Fields.Item("TaskStatus").Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                    oMatrixCon.Columns.Item("V_2").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("Reason").Value

                    oCombo = oMatrixCon.Columns.Item("V_0").Cells.Item(iRowCount + 1).Specific
                    If (oRS.Fields.Item("AlertHQ").Value = "Yes") Then
                        oCombo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index)
                    ElseIf (oRS.Fields.Item("AlertHQ").Value = "No") Then
                        oCombo.Select(1, SAPbouiCOM.BoSearchKey.psk_Index)
                    End If
                    If Not String.IsNullOrEmpty(oRS.Fields.Item("U_CmplStartDate").Value) Then
                        sNewSchDate = GetDate(CDate(oRS.Fields.Item("U_CmplStartDate").Value).ToString("yyyy.MM.dd"), p_oDICompany)
                        If sNewSchDate <> "19000101" And sNewSchDate <> "18991230" Then
                            oMatrixCon.Columns.Item("V_16").Cells.Item(iRowCount + 1).Specific.string = GetDate(CDate(oRS.Fields.Item("U_CmplStartDate").Value).ToString("yyyy.MM.dd"), p_oDICompany)
                        End If
                    End If
                    If Not String.IsNullOrEmpty(oRS.Fields.Item("U_CmplEndDate").Value) Then
                        sNewSchDate = GetDate(CDate(oRS.Fields.Item("U_CmplEndDate").Value).ToString("yyyy.MM.dd"), p_oDICompany)
                        If sNewSchDate <> "19000101" And sNewSchDate <> "18991230" Then
                            oMatrixCon.Columns.Item("V_15").Cells.Item(iRowCount + 1).Specific.string = GetDate(CDate(oRS.Fields.Item("U_CmplEndDate").Value).ToString("yyyy.MM.dd"), p_oDICompany)
                        End If
                    End If

                    If Not String.IsNullOrEmpty(oRS.Fields.Item("ScheduleDate").Value) Then
                        sNewSchDate = GetDate(CDate(oRS.Fields.Item("ScheduleDate").Value).ToString("yyyy.MM.dd"), p_oDICompany)
                        If sNewSchDate <> "19000101" And sNewSchDate <> "18991230" Then
                            oMatrixCon.Columns.Item("V_17").Cells.Item(iRowCount + 1).Specific.string = GetDate(CDate(oRS.Fields.Item("ScheduleDate").Value).ToString("yyyy.MM.dd"), p_oDICompany)
                        End If
                    End If

                    oMatrixCon.Columns.Item("V_14").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("U_CompletedBy").Value
                    oMatrixCon.Columns.Item("V_13").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("U_InspectedBy").Value
                    oMatrixCon.Columns.Item("V_12").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("U_VerifiedBy").Value
                    oMatrixCon.Columns.Item("V_11").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("U_Remarks").Value

                    oRS.MoveNext()
                Next iRowCount
            End If

            oMatrixCon.AutoResizeColumns()

            'Load the Attachment Details 
            sQueryString = "select U_path [Path] ,U_FileName [FileName],U_AttachDate [AttachDate] from [@AB_JOBSCH4] where DocEntry ='" & sDocEntry & "'"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)


            oRS.DoQuery(sQueryString)

            If oRS.RecordCount > 0 Then
                p_oSBOApplication.StatusBar.SetText("Please Wait Values are Loading Attachment Details....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                For iRowCount As Integer = 0 To oRS.RecordCount - 1

                    'Fill the  Details
                    oMatrixAttach.AddRow()
                    oMatrixAttach.Columns.Item("V_-1").Cells.Item(iRowCount + 1).Specific.string = iRowCount + 1
                    oMatrixAttach.Columns.Item("V_2").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("Path").Value
                    oMatrixAttach.Columns.Item("V_1").Cells.Item(iRowCount + 1).Specific.string = oRS.Fields.Item("FileName").Value
                    oMatrixAttach.Columns.Item("V_0").Cells.Item(iRowCount + 1).Specific.string = GetDate(CDate(oRS.Fields.Item("AttachDate").Value).ToString("yyyy.MM.dd"), p_oDICompany)
                    oRS.MoveNext()
                Next iRowCount


            End If
            oMatrixCon.AutoResizeColumns()
            oMatrixAttach.AutoResizeColumns()

            If TaskSchedule_NonEditableMode(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            oForm.Freeze(False)

            p_oSBOApplication.StatusBar.SetText("Values Loaded Successfully!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            TaskSchedule_LoadValues = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            oForm.Freeze(False)
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            TaskSchedule_LoadValues = RTN_SUCCESS
        End Try
    End Function

    Function TaskSchedule_NonEditableMode(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix


        Try
            sFuncName = "TaskSchedule_NonEditableMode()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oForm.Items.Item("txtSchDate").Enabled = False
            oForm.Items.Item("txtComDate").Enabled = False
            oForm.Items.Item("txtStatus").Enabled = False
            oForm.Items.Item("txtCompBy").Enabled = False

            oMatrix = oForm.Items.Item("matContent").Specific

            oMatrix.Columns.Item("V_1").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)


            oMatrix.Columns.Item("V_9").Editable = False
            oMatrix.Columns.Item("V_8").Editable = False
            oMatrix.Columns.Item("V_7").Editable = False

            oMatrix.Columns.Item("V_6").Editable = False
            oMatrix.Columns.Item("V_5").Editable = False
            oMatrix.Columns.Item("V_4").Editable = False
            oMatrix.Columns.Item("V_3").Editable = False
            oMatrix.Columns.Item("V_2").Editable = False

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            TaskSchedule_NonEditableMode = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            oForm.Freeze(False)
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            TaskSchedule_NonEditableMode = RTN_SUCCESS
        End Try
    End Function

    Function TaskSchedule_UpdateReSchedule(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim sLineNumber As String = String.Empty
        Dim sReSchDate As String = String.Empty
        Dim sScheduleDate As String = String.Empty
        Dim sAlertHQ As String = String.Empty
        Dim oCombo As SAPbouiCOM.ComboBox

        Try
            sFuncName = "TaskSchedule_UpdateReSchedule()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("matContent").Specific

            For iRowCount As Integer = 1 To oMatrix.RowCount
                sLineNumber = oMatrix.Columns.Item("V_10").Cells.Item(iRowCount).Specific.string
                sReSchDate = oMatrix.Columns.Item("V_1").Cells.Item(iRowCount).Specific.value
                sScheduleDate = oMatrix.Columns.Item("V_17").Cells.Item(iRowCount).Specific.value
                oCombo = oMatrix.Columns.Item("V_0").Cells.Item(iRowCount).Specific
                If oCombo.Value = "" Then
                    sAlertHQ = "NULL"
                Else
                    sAlertHQ = oCombo.Selected.Value
                End If

                If String.IsNullOrEmpty(sScheduleDate) Then
                    p_oSBOApplication.StatusBar.SetText("Schedule Date cannot be a blank ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_None)
                    Return RTN_ERROR
                End If

                If String.IsNullOrEmpty(sReSchDate) Then
                    '' ''CONVERT(DATE,'" & CDate(sScheduleDate).ToString("yyyyMMdd") & "',103)
                    '' sQueryString += " UPDATE [@AB_JOBSCH3] SET U_ScheduleDate = '" & GetDate(sScheduleDate, p_oDICompany) & "' " & _
                    ''" WHERE DocEntry ='" & sDocEntry & "' AND LineId ='" & sLineNumber & "'"
                    sQueryString += "Exec [SP045_UpdateSCH2]'" & sDocEntry & "' , " & sLineNumber & ", '" & sScheduleDate & "','','" & sAlertHQ & "'"
                Else
                    '' sQueryString += " UPDATE [@AB_JOBSCH3] SET U_NewSchedDate = '" & GetDate(sReSchDate, p_oDICompany) & "', U_AlertHQ ='" & sAlertHQ & "',  U_ScheduleDate = '" & GetDate(sScheduleDate, p_oDICompany) & "' " & _
                    ''" WHERE DocEntry ='" & sDocEntry & "' AND LineId ='" & sLineNumber & "'"
                    sQueryString += "Exec [SP045_UpdateSCH2]'" & sDocEntry & "' , " & sLineNumber & ", '" & sScheduleDate & "','" & sReSchDate & "','" & sAlertHQ & "'"
                End If
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)

            If sQueryString.Length > 1 Then
                oRS.DoQuery(sQueryString)
            End If

            p_oSBOApplication.StatusBar.SetText("Operation Completed Successfully!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

            oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            TaskSchedule_UpdateReSchedule = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            oForm.Freeze(False)
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            TaskSchedule_UpdateReSchedule = RTN_SUCCESS
        End Try
    End Function

    Function TaskSchedule_FileDisplay(ByRef oForm As SAPbouiCOM.Form, ByRef osErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim sFilePath As String = String.Empty

        Try
            sFuncName = "TaskSchedule_FileDisplay()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oMatrix = oForm.Items.Item("matAttach").Specific

            For iRowCount As Integer = 1 To oMatrix.RowCount
                If oMatrix.IsRowSelected(iRowCount) Then
                    sFilePath = oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string & oMatrix.Columns.Item("V_1").Cells.Item(iRowCount).Specific.string
                    Exit For
                End If
            Next

            If Not String.IsNullOrEmpty(sFilePath) Then
                If System.IO.File.Exists(sFilePath) = True Then
                    Process.Start(sFilePath)
                Else
                    sErrDesc = "File Does Not Exist"
                End If
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            TaskSchedule_FileDisplay = RTN_SUCCESS


        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            TaskSchedule_FileDisplay = RTN_ERROR
        End Try
    End Function

End Module
