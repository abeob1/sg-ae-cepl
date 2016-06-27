
Module modShowAroundQuestions


    Function Loadscreen(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Try
            sFuncName = "Loadscreen()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oForm.Freeze(True)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("txtMatrix").Specific
            sQueryString = "SELECT Code FROM [@AB_SHOWAROUNDMASTER]"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            If oRS.RecordCount > 0 Then
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                oForm.Items.Item("txtCode").Specific.string = oRS.Fields.Item("Code").Value
                oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)

                oForm.EnableMenu("1282", False)

                If (oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.string <> "") Then

                    oForm.DataSources.DBDataSources.Item("@AB_SHOWAROUNDMLINE").Clear()
                    oMatrix.AddRow(1)
                    oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific
                    oCheckBox.Checked = True

                End If
            Else

                oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE
                oForm.Items.Item("txtCode").Specific.value = "1"
                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(1).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_0").Cells.Item(1).Specific.value = oMatrix.VisualRowCount
                oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(1).Specific
                oCheckBox.Checked = True
            End If

            oMatrix.AutoResizeColumns()


            oForm.EnableMenu("1293", True)
            oForm.EnableMenu("1283", False)
            oForm.EnableMenu("1281", False)

            oForm.Freeze(False)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Loadscreen = RTN_SUCCESS
        Catch ex As Exception
            oForm.Freeze(False)
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Loadscreen = RTN_ERROR
        End Try
    End Function

    Function Matrix_AddRow(ByRef oMatrix As SAPbouiCOM.Matrix, ByVal bIsShowAround As Boolean, ByRef sErrDesc As String) As Long

        'Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Try
            sFuncName = "Matrix_AddRow()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            '  oMatrix = oForm.Items.Item("txtMatrix").Specific

            If (oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.string <> "") Then

                'oForm.DataSources.DBDataSources.Item("@AB_SHOWAROUNDMLINE").Clear()


                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                If bIsShowAround = False Then
                    oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific
                    oCheckBox.Checked = True
                End If
                oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount - 1).Specific.active = True

            End If


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Matrix_AddRow = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Matrix_AddRow = RTN_ERROR
        End Try
    End Function

    Function Validate_Question(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim sQueryString As String = String.Empty
        Dim oRS As SAPbobsCOM.Recordset
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            sFuncName = "Validate_Question()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("txtMatrix").Specific

            sQueryString = "SELECT Code FROM [@AB_SHOWAROUNDMLINE] WHERE Code='" & oForm.Items.Item("txtCode").Specific.string & "' AND U_Sort='" & p_ishowaround_question & "'"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)

            oRS.DoQuery(sQueryString)

            If (oRS.RecordCount > 0) Then
                sErrDesc = "You Cannot Delete the Row "
                Return RTN_ERROR
            End If


            For iDelRow As Integer = iCurrentRow To oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_0").Cells.Item(iDelRow).Specific.string = oMatrix.Columns.Item("V_0").Cells.Item(iDelRow).Specific.string - 1
                oMatrix.Columns.Item("V_-1").Cells.Item(iDelRow).Specific.string = oMatrix.Columns.Item("V_-1").Cells.Item(iDelRow).Specific.string - 1
            Next


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Validate_Question = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Validate_Question = RTN_ERROR
        End Try
    End Function

    Function Delete_EmptyRows(ByRef oForm As SAPbouiCOM.Form, sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            sFuncName = "Delete_EmptyRows()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oMatrix = oForm.Items.Item("txtMatrix").Specific

            For iRowCount As Integer = oMatrix.VisualRowCount To 1 Step -1
                If (oMatrix.Columns.Item("V_3").Cells.Item(iRowCount).Specific.string = "") Then
                    oMatrix.DeleteRow(iRowCount)
                End If
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Delete_EmptyRows = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Delete_EmptyRows = RTN_ERROR
        End Try
    End Function

    Function ShowAroundQuestions_DeleteEmptyRows(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty


        Try
            sFuncName = "ShowAroundQuestions_DeleteEmptyRows()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            sQueryString = "DELETE FROM [@AB_SHOWAROUNDMLINE] WHERE Code ='" & oForm.Items.Item("txtCode").Specific.string & "' AND U_Category IS NULL"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            ShowAroundQuestions_DeleteEmptyRows = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            ShowAroundQuestions_DeleteEmptyRows = RTN_ERROR
        End Try
    End Function

    Function ShowAroundQuestion_Validation(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix

        Try

            sFuncName = "ShowAroundQuestion_Validation()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oMatrix = oForm.Items.Item("txtMatrix").Specific

            For iRowCount As Integer = 1 To oMatrix.VisualRowCount

                If (oMatrix.Columns.Item("V_3").Cells.Item(iRowCount).Specific.string <> "" Or oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string <> "") Then

                    If (oMatrix.Columns.Item("V_3").Cells.Item(iRowCount).Specific.string) = "" Then
                        sErrDesc = "Category Field. Cannot be Blank!. RowNo : " & iRowCount
                        oMatrix.Columns.Item("V_3").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                        Return RTN_ERROR
                    ElseIf (oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string) = "" Then
                        oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                        sErrDesc = "Question Field. Cannot be Blank!. RowNo : " & iRowCount
                        Return RTN_ERROR
                    End If
                End If

            Next


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            ShowAroundQuestion_Validation = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            ShowAroundQuestion_Validation = RTN_ERROR
        End Try
    End Function

End Module
