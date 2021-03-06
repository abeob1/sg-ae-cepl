﻿Module modJobType

    Function JobType_Loadscreen(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Try
            sFuncName = "JobType_Loadscreen()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oForm.Freeze(True)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("txtMatrix").Specific
            sQueryString = "SELECT Code FROM [@AB_JOBTYPE]"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            If oRS.RecordCount > 0 Then
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                oForm.Items.Item("txtCode").Specific.string = oRS.Fields.Item("Code").Value
                oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)

                oForm.EnableMenu("1282", False)

                If (oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.string <> "") Then

                    oForm.DataSources.DBDataSources.Item("@AB_JOBTYPELINE").Clear()
                    oMatrix.AddRow(1)
                    oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oMatrix.Columns.Item("V_4").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific
                    oCheckBox.Checked = True

                End If
            Else

                oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE
                oForm.Items.Item("txtCode").Specific.string = "1"
                oForm.Items.Item("5").Specific.string = "1"
                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(1).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_0").Cells.Item(1).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_3").Cells.Item(1).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_4").Cells.Item(1).Specific.value = oMatrix.VisualRowCount
                oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(1).Specific
                oCheckBox.Checked = True
                oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.active = True
            End If

            oMatrix.AutoResizeColumns()

            oMatrix.Columns.Item("V_2").Editable = True

            oForm.EnableMenu("1293", True)
            oForm.EnableMenu("1283", False)
            oForm.EnableMenu("1281", False)

            oForm.Freeze(False)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobType_Loadscreen = RTN_SUCCESS
        Catch ex As Exception
            oForm.Freeze(False)
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobType_Loadscreen = RTN_ERROR
        End Try
    End Function

    Function JobType_ValidationLocation(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim sQueryString As String = String.Empty
        Dim oRS As SAPbobsCOM.Recordset
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            sFuncName = "JobType_ValidationLocation()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("txtMatrix").Specific

            sQueryString = "SELECT Code FROM [@AB_JOBTYPELINE] WHERE Code='" & oForm.Items.Item("txtCode").Specific.string & "' AND U_Sort='" & p_iservicequestion & "'"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)

            oRS.DoQuery(sQueryString)

            If (oRS.RecordCount > 0) Then
                sErrDesc = "You Cannot Delete the Row "
                Return RTN_ERROR
            End If


            For iDelRow As Integer = iCurrentRow To oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_0").Cells.Item(iDelRow).Specific.string = oMatrix.Columns.Item("V_0").Cells.Item(iDelRow).Specific.string - 1
                oMatrix.Columns.Item("V_-1").Cells.Item(iDelRow).Specific.string = oMatrix.Columns.Item("V_-1").Cells.Item(iDelRow).Specific.string - 1
                oMatrix.Columns.Item("V_3").Cells.Item(iDelRow).Specific.string = oMatrix.Columns.Item("V_3").Cells.Item(iDelRow).Specific.string - 1
            Next


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobType_ValidationLocation = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobType_ValidationLocation = RTN_ERROR
        End Try
    End Function

    Function JobType_DeleteEmptyRows(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty

        Try
            sFuncName = "JobType_DeleteEmptyRows()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            sQueryString = "DELETE FROM [@AB_JOBTYPELINE] WHERE Code ='" & oForm.Items.Item("txtCode").Specific.string & "' AND isnull(U_JobTypeName,'') = ''"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobType_DeleteEmptyRows = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobType_DeleteEmptyRows = RTN_ERROR
        End Try
    End Function

    Function JobType_Validation(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long
        Dim oMatrix As SAPbouiCOM.Matrix
        Try

            sFuncName = "JobType_Validation()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oMatrix = oForm.Items.Item("txtMatrix").Specific

            If oMatrix.RowCount = 1 Then
                If (oMatrix.Columns.Item("V_3").Cells.Item(1).Specific.string <> "" Or oMatrix.Columns.Item("V_2").Cells.Item(1).Specific.string <> "") Then

                    If (oMatrix.Columns.Item("V_2").Cells.Item(1).Specific.string) = "" Then
                        sErrDesc = "Job Type Name Field. Cannot be Blank!. RowNo : " & 1
                        oMatrix.Columns.Item("V_3").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                        Return RTN_ERROR
                    End If
                End If
            Else
                For iRowCount As Integer = 1 To oMatrix.RowCount - 1

                    If (oMatrix.Columns.Item("V_3").Cells.Item(iRowCount).Specific.string <> "" Or oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string <> "") Then

                        If (oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string) = "" Then
                            sErrDesc = "Job Type Name Field. Cannot be Blank!. RowNo : " & iRowCount
                            oMatrix.Columns.Item("V_3").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            Return RTN_ERROR
                        End If
                    End If

                Next

            End If



            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobType_Validation = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobType_Validation = RTN_ERROR
        End Try
    End Function

    Function JobType_MatrixAddRow(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Try
            sFuncName = "JobType_MatrixAddRow()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oMatrix = oForm.Items.Item("txtMatrix").Specific

            If (oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.string <> "") Then

                oForm.DataSources.DBDataSources.Item(1).Clear()
                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_4").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific
                oCheckBox.Checked = True
                Try
                    oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific.active = True
                Catch ex As Exception
                End Try
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            JobType_MatrixAddRow = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            JobType_MatrixAddRow = RTN_ERROR
        End Try
    End Function

End Module
