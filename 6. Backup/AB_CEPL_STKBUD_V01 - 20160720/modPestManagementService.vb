Imports System.Globalization
Imports System.IO


Module modPestManagementService

    Function PestManagementService_EnableControl(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "PestManagementService_EnableControl()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oForm.Items.Item("txtDocNum").Enabled = True
            oForm.Items.Item("txtDocDate").Enabled = True
            oForm.Items.Item("txtTimeIn").Enabled = True
            oForm.Items.Item("txtTimeOut").Enabled = True
            oForm.Items.Item("txtProject").Enabled = True
            oForm.Items.Item("txtAddress").Enabled = True
            oForm.Items.Item("txtReport").Enabled = True
            oForm.Items.Item("txtEmpty").Enabled = True
            oForm.Items.Item("txtBlkNo").Enabled = True

            oForm.Items.Item("txtUnitNo").Enabled = True
            oForm.Items.Item("txtPstCode").Enabled = True
            oForm.Items.Item("matPestmgt").Enabled = True
            oForm.Items.Item("txtSprName").Enabled = True
            oForm.Items.Item("txtEsign").Enabled = True

            oForm.Items.Item("txtCntName").Enabled = True
            oForm.Items.Item("txtESignt").Enabled = True
            oForm.Items.Item("37").Enabled = True
            oForm.Items.Item("38").Enabled = True
            oForm.Items.Item("39").Enabled = True

            oForm.Items.Item("41").Enabled = True
            oForm.Items.Item("43").Enabled = True
            oForm.Items.Item("45").Enabled = True
            oForm.Items.Item("46").Enabled = True
            oForm.Items.Item("48").Enabled = True

            oForm.Items.Item("txtDocNum").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
            '' oForm.ActiveItem = "txtDocNum"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            PestManagementService_EnableControl = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            PestManagementService_EnableControl = RTN_ERROR
        End Try
    End Function

    Function PestManagementService_DisableControl(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "PestManagementService_DisableControl()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)



            oForm.Items.Item("txtDocNum").Enabled = False
            oForm.Items.Item("txtDocDate").Enabled = False
            oForm.Items.Item("txtTimeIn").Enabled = False
            oForm.Items.Item("txtTimeOut").Enabled = False
            oForm.Items.Item("txtProject").Enabled = False
            oForm.Items.Item("txtAddress").Enabled = False
            oForm.Items.Item("txtReport").Enabled = False
            oForm.Items.Item("txtEmpty").Enabled = False
            oForm.Items.Item("txtBlkNo").Enabled = False

            oForm.Items.Item("txtUnitNo").Enabled = False
            oForm.Items.Item("txtPstCode").Enabled = False
            oForm.Items.Item("matPestmgt").Enabled = False
            oForm.Items.Item("54").Enabled = False
            oForm.Items.Item("txtSprName").Enabled = False
            oForm.Items.Item("txtEsign").Enabled = False

            oForm.Items.Item("txtCntName").Enabled = False
            oForm.Items.Item("txtESignt").Enabled = False
            oForm.Items.Item("37").Enabled = False
            oForm.Items.Item("38").Enabled = False
            oForm.Items.Item("39").Enabled = False

            oForm.Items.Item("41").Enabled = False
            oForm.Items.Item("43").Enabled = False
            oForm.Items.Item("45").Enabled = False
            oForm.Items.Item("46").Enabled = False
            oForm.Items.Item("48").Enabled = False
            oForm.Items.Item("btnDisplay").Enabled = True
            oForm.Items.Item("btnDelete").Enabled = True

            'If oForm.Mode <> SAPbouiCOM.BoFormMode.fm_OK_MODE Then
            '    oForm.ActiveItem = "txtDocEntr"
            'End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            PestManagementService_DisableControl = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            PestManagementService_DisableControl = RTN_ERROR
        End Try
    End Function


    Function EPSAttachment_Path(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oRS As SAPbobsCOM.Recordset
        Dim sFilePath As String = String.Empty

        Try
            sFuncName = "EPSAttachment_Path()"
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
                    oForm.Update()
                End If

                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                End If

            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            EPSAttachment_Path = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            EPSAttachment_Path = RTN_ERROR
        End Try

    End Function
    Function EPSDeleteRow_Attachment(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim iSelectRow As Integer = 0
        Try
            sFuncName = "EPSDeleteRow_Attachment()"
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
            EPSDeleteRow_Attachment = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            EPSDeleteRow_Attachment = RTN_ERROR
        End Try

    End Function
    Function EPSFile_Display(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim sFilePath As String = String.Empty

        Try
            sFuncName = "EPSFile_Display()"
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
            EPSFile_Display = RTN_SUCCESS


        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            EPSFile_Display = RTN_ERROR
        End Try
    End Function

    Function EPSSaveFileToAttachFolder(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim sFilePath As String = String.Empty
        Dim sSource As String = String.Empty
        Dim sDestPath As String = String.Empty

        Try
            sFuncName = "EPSSaveFileToAttachFolder"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oMatrix = oForm.Items.Item("matAttach").Specific
            For i As Integer = 1 To oMatrix.RowCount
                If (oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.string <> "") Then
                    sFilePath = oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.string
                    sSource = oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.string

                    If Not String.IsNullOrEmpty(p_sSelectedFilepath) Then
                        sDestPath = sFilePath & p_sSelectedFileName
                        'System.IO.File.Copy("c:\\Test.pdf", "C:\\Program Files (x86)\\SAP\\SAP Business One\\Attachments\\Test.pdf", True)
                        System.IO.File.Copy(sSource, sDestPath, True)
                    End If
                End If
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            EPSSaveFileToAttachFolder = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            EPSSaveFileToAttachFolder = RTN_ERROR
        End Try
    End Function

End Module
