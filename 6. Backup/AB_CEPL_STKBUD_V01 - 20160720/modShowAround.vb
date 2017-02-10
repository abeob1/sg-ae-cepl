Imports System.Globalization
Imports System.IO

Module modShowAround

    Dim oDBDSHeader, oDBDSDetail As SAPbouiCOM.DBDataSource

    Function Form_Load(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim sDocDate As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            sFuncName = "Form_Load()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("matContent").Specific

            ''sQueryString = "select COUNT( CAST(ISNULL(DocNum,0) AS INT))+1 [DocEntry] from [@AB_SHOWAROUND] "
            sQueryString = "select Top 1 NextNumber [DocEntry] from NNM1 where ObjectCode ='ShowRound' and Locked ='N' "
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            oForm.Items.Item("txtDocNum").Specific.string = oRS.Fields.Item("DocEntry").Value

            'sDocDate = Today.Day & Today.Month.ToString() & Today.Year.ToString()

            If Today.Month.ToString().Length = 1 And Today.Day.ToString().Length = 1 Then
                sDocDate = "0" & Today.Day & "0" & Today.Month.ToString() & Today.Year.ToString()
            ElseIf Today.Month.ToString().Length = 1 And Today.Day.ToString().Length = 2 Then
                sDocDate = Today.Day & "0" & Today.Month.ToString() & Today.Year.ToString()
            ElseIf Today.Month.ToString().Length = 2 And Today.Day.ToString().Length = 2 Then
                sDocDate = "0" & Today.Day & Today.Month.ToString() & Today.Year.ToString()
            End If

            oForm.Items.Item("txtDocDate").Specific.string = sDocDate   ''GetDate(Now.Date, p_oDICompany)
            oForm.Items.Item("txtLogUsr").Specific.string = p_oDICompany.UserName


            oForm.Items.Item("10").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
            oForm.PaneLevel = 1

            oForm.Items.Item("txtDocNum").Enabled = False
            oForm.Items.Item("txtDocDate").Enabled = False
            oForm.Items.Item("txtOwnName").Enabled = False

            oDBDSHeader = oForm.DataSources.DBDataSources.Item(1)
            oDBDSDetail = oForm.DataSources.DBDataSources.Item(2)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Display_ShowAroundMaster() for Dispaly the Show Amound Masters", sFuncName)
            If Display_ShowAroundMaster(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)


            sQueryString = "SELECT T1.[empID] FROM OUSR T0  INNER JOIN OHEM T1 ON T0.[USERID] = T1.[userId] WHERE T0.[USER_CODE] ='" & p_oDICompany.UserName & "'"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            If oRS.RecordCount > 0 Then
                oForm.Items.Item("txtOwner").Specific.string = oRS.Fields.Item("empID").Value
            End If


            'oForm.DataSources.DBDataSources.Item("@AB_SHOWAROUND1").Clear()
            'oMatrix.AddRow(1)
            'oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount

            oForm.DataBrowser.BrowseBy = "txtDocNum"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Form_Load = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Form_Load = RTN_ERROR
        End Try


    End Function

    Function Display_ShowAroundMaster(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Try

            sFuncName = "Display_ShowAroundMaster()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("matContent").Specific

            sQueryString = "SELECT T0.[U_Category], T0.[U_Question] FROM [dbo].[@AB_SHOWAROUNDMLINE]  T0 WHERE T0.[U_Active] ='Y' AND ISNULL(T0.[U_Category],'') <> ''"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)

            ''Try
            ''    oForm.DataSources.DataTables.Add("oMatrixDT")
            ''Catch ex As Exception

            ''End Try
            ''oForm.DataSources.DataTables.Item("oMatrixDT").ExecuteQuery(sQueryString)

            ''oMatrix.Columns.Item("V_3").DataBind.Bind("oMatrixDT", "U_Category")
            ''oMatrix.Columns.Item("V_2").DataBind.Bind("oMatrixDT", "U_Question")
            ' ''  oMatrix.Columns.Item("V_1").DataBind.Bind("oMatrixDT", "U_Quantity")
            ''oMatrix.Columns.Item("V_-1").DataBind.Bind("oMatrixDT", "LineNo")
            ''oMatrix.Clear()
            ''oMatrix.LoadFromDataSource()
            ''oMatrix.AutoResizeColumns()


            oRS.DoQuery(sQueryString)

            oDBDSDetail.Clear()

            oMatrix.FlushToDataSource()


            If oRS.RecordCount > 0 Then
                p_oSBOApplication.StatusBar.SetText("Please Wait Values are Loading....", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                oForm.Freeze(True)
                For iRowCount As Integer = 0 To oRS.RecordCount - 1

                    'Fill the  Details

                    oDBDSDetail.SetValue("LineId", oDBDSDetail.Size - 1, iRowCount + 1)
                    oDBDSDetail.SetValue("U_Category", oDBDSDetail.Size - 1, oRS.Fields.Item("U_Category").Value)
                    oDBDSDetail.SetValue("U_Question", oDBDSDetail.Size - 1, oRS.Fields.Item("U_Question").Value)
                    oDBDSDetail.SetValue("U_LineNum", oDBDSDetail.Size - 1,iRowCount + 1)

                    oDBDSDetail.InsertRecord(oDBDSDetail.Size)
                    oDBDSDetail.Offset = oDBDSDetail.Size - 1
                    oRS.MoveNext()
                Next iRowCount

                ' oMatrix.FlushToDataSource()
                oMatrix.LoadFromDataSourceEx()
                oMatrix.AutoResizeColumns()
                If oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.RowCount).Specific.String = "" Then
                    oMatrix.DeleteRow(oMatrix.RowCount)
                End If

                For imjs As Integer = 1 To oMatrix.RowCount
                    oMatrix.Columns.Item("V_1").Cells.Item(imjs).Specific.String = ""
                Next imjs

                oMatrix.Columns.Item("V_0").Cells.Item(1).Specific.active = True
                oForm.Freeze(False)
                p_oSBOApplication.StatusBar.SetText("Operation Completed Successfully", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
            Else
                p_oSBOApplication.StatusBar.SetText("There Is No Matching Record Found in Show Around Master", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("There Is No Matching Record Found in Show Around Master", sFuncName)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Display_ShowAroundMaster = RTN_SUCCESS

        Catch ex As Exception
            oForm.Freeze(False)
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Display_ShowAroundMaster = RTN_ERROR
        End Try
    End Function

    Function PaneLevel(ByRef oForm As SAPbouiCOM.Form, ByVal sPaneLevel As String, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "PaneLevel()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If sPaneLevel = "1" Then
                oForm.PaneLevel = 1
            Else
                oForm.PaneLevel = 2
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            PaneLevel = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            PaneLevel = RTN_ERROR
        End Try
    End Function

    Function ChooseFromList(ByRef oForm As SAPbouiCOM.Form, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef sErrDesc As String) As Long


        Dim oDT As SAPbouiCOM.DataTable
        Dim OCFLEvent As SAPbouiCOM.ChooseFromListEvent
        Dim sOwnerName As String = String.Empty

        Try
            sFuncName = "ChooseFromList()"
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


                oForm.Items.Item("txtOwnName").Specific.string = sOwnerName ' oDT.GetValue("firstName", 0) & " " & oDT.GetValue("middleName", 0) & " " & oDT.GetValue("lastName", 0)
                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                End If

                oForm.Items.Item("txtOwner").Specific.string = oDT.GetValue("empID", 0) '& "," & oDT.GetValue("middleName", 0) & "," & oDT.GetValue("lastName", 0)

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
                ChooseFromList = RTN_SUCCESS
            Catch ex As Exception
                sErrDesc = ex.Message().ToString()
                Call WriteToLogFile(sErrDesc, sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                ChooseFromList = RTN_SUCCESS
            End Try

        Catch ex As Exception

        End Try
    End Function

    Function Attachment_Path(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oRS As SAPbobsCOM.Recordset
        Dim sFilePath As String = String.Empty

        Try
            sFuncName = "Attachment_Path()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : SELECT attachpath FROM OADP", sFuncName)

            oRS.DoQuery("SELECT attachpath FROM OADP")

            sFilePath = oRS.Fields.Item("attachpath").Value

            If String.IsNullOrEmpty(sFilePath) Then
                p_oSBOApplication.StatusBar.SetText("Select the Attachment folder path in the General Settings", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                Return RTN_ERROR
                Exit Function
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling fillopen() for Selecting the File", sFuncName)
            fillopen()

            If Not String.IsNullOrEmpty(p_sSelectedFilepath) Then
                Dim oFileInfo = New FileInfo(p_sSelectedFilepath)

                If (File.Exists(sFilePath & "\" & p_sSelectedFileName)) Then
                    
                    If p_oSBOApplication.MessageBox("A File with this name Already Exist, Would you like to replace " & _
                                                    " this file? " & sFilePath & "\" & p_sSelectedFileName, 2, "Yes", "Cancel") = 1 Then

                        GoTo gotoReplace

                    Else
                        Return RTN_SUCCESS
                    End If

                Else

                    ''oFileInfo.CopyTo(Path.Combine(sFilePath, oFileInfo.Name), True)
gotoReplace:

                    oMatrix = oForm.Items.Item("matAttach").Specific

                    If oMatrix.RowCount > 0 Then
                        If (oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.string = "") Then
                        Else
                            oMatrix.AddRow(1)
                        End If
                    Else
                        oMatrix.AddRow(1)
                    End If

                    oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.value = sFilePath
                    oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific.value = p_sSelectedFileName
                    oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.value = p_sSelectedFilepath
                    oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.string = Get_TodayDate(sErrDesc)
                End If

                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                End If

            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Attachment_Path = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Attachment_Path = RTN_ERROR
        End Try

    End Function

    Function ShowAround_FileAttachment(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim sSourcePath As String = String.Empty
        Dim sTarPath As String = String.Empty
        Dim sFileName As String = String.Empty

        Dim oMatrix As SAPbouiCOM.Matrix


        Try

            sFuncName = "ShowAround_FileAttachment()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oMatrix = oForm.Items.Item("matAttach").Specific

            For iAttchRowCount As Integer = 1 To oMatrix.RowCount
                oMatrix.Columns.Item("V_-1").Cells.Item(iAttchRowCount).Specific.string = iAttchRowCount
                sSourcePath = oMatrix.Columns.Item("V_3").Cells.Item(iAttchRowCount).Specific.string
                sTarPath = oMatrix.Columns.Item("V_2").Cells.Item(iAttchRowCount).Specific.string
                sFileName = oMatrix.Columns.Item("V_1").Cells.Item(iAttchRowCount).Specific.string
                If Not String.IsNullOrEmpty(sSourcePath) Then
                    Dim oFileInfo As FileInfo = New FileInfo(sSourcePath)

                    If (File.Exists(sTarPath & "\" & sFileName)) Then

                        File.Delete(sTarPath & "\" & sFileName)
                        oFileInfo.CopyTo(Path.Combine(sTarPath, oFileInfo.Name), True)
                    Else
                        oFileInfo.CopyTo(Path.Combine(sTarPath, oFileInfo.Name), True)
                    End If
                End If
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            ShowAround_FileAttachment = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            ShowAround_FileAttachment = RTN_ERROR
        End Try
    End Function

    Function Get_TodayDate(ByRef sErrDesc As String) As String

        Dim sDocDate As String = String.Empty

        Try
            sFuncName = "Get_TodayDate()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If Today.Month.ToString().Length = 1 And Today.Day.ToString().Length = 1 Then
                sDocDate = "0" & Today.Day & "0" & Today.Month.ToString() & Today.Year.ToString()
            ElseIf Today.Month.ToString().Length = 1 And Today.Day.ToString().Length = 2 Then
                sDocDate = Today.Day & "0" & Today.Month.ToString() & Today.Year.ToString()
            ElseIf Today.Month.ToString().Length = 2 And Today.Day.ToString().Length = 2 Then
                sDocDate = "0" & Today.Day & Today.Month.ToString() & Today.Year.ToString()
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Get_TodayDate = sDocDate
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Get_TodayDate = RTN_ERROR
        End Try

    End Function

    Function DeleteRow_Attachment(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim iSelectRow As Integer = 0
        Try
            sFuncName = "DeleteRow_Attachment()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oMatrix = oForm.Items.Item("matAttach").Specific

            For iRowCount As Integer = 1 To oMatrix.RowCount
                If oMatrix.IsRowSelected(iRowCount) Then
                    oMatrix.DeleteRow(iRowCount)
                    iSelectRow = iRowCount
                    Exit For

                End If
            Next

            For iDelRow As Integer = iSelectRow To oMatrix.RowCount
                oMatrix.Columns.Item("V_-1").Cells.Item(iDelRow).Specific.string = oMatrix.Columns.Item("V_-1").Cells.Item(iDelRow).Specific.string - 1
            Next

            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            DeleteRow_Attachment = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            DeleteRow_Attachment = RTN_ERROR
        End Try
    End Function

    Function File_Display(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim sFilePath As String = String.Empty

        Try
            sFuncName = "File_Display()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oMatrix = oForm.Items.Item("matAttach").Specific


            For iRowCount As Integer = 1 To oMatrix.RowCount
                If oMatrix.IsRowSelected(iRowCount) Then
                    If Right(oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string, 1) = "\" Then
                        sFilePath = oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string & oMatrix.Columns.Item("V_1").Cells.Item(iRowCount).Specific.string
                    Else
                        sFilePath = oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string '' & oMatrix.Columns.Item("V_1").Cells.Item(iRowCount).Specific.string
                    End If

                    Exit For
                End If
            Next



            If Not String.IsNullOrEmpty(sFilePath) Then
                If System.IO.File.Exists(sFilePath) = True Then
                    Process.Start(sFilePath)
                Else
                    sErrDesc = "File Does Not Exist"
                    Return RTN_ERROR
                End If
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            File_Display = RTN_SUCCESS


        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            File_Display = RTN_ERROR
        End Try
    End Function

    Function ShowAround_DeleteEmptyRows(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty


        Try
            sFuncName = "ShowAround_DeleteEmptyRows()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            sQueryString = "DELETE FROM [@AB_SHOWAROUND1] WHERE  DocEntry IN(SELECT DocEntry  FROM [@AB_SHOWAROUND] " & _
                " WHERE DocNum ='" & oForm.Items.Item("txtDocNum").Specific.string & "') AND U_Category IS NULL"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            sQueryString = "UPDATE [@AB_SHOWAROUND1] SET U_Quantity = NULL WHERE  DocEntry IN(SELECT DocEntry  FROM [@AB_SHOWAROUND] " & _
               " WHERE DocNum ='" & oForm.Items.Item("txtDocNum").Specific.string & "') AND U_Quantity = 0.00"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            ShowAround_DeleteEmptyRows = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            ShowAround_DeleteEmptyRows = RTN_ERROR
        End Try
    End Function

End Module
