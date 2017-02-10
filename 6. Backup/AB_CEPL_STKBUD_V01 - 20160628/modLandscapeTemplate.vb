Module modLandscapeTemplate

    Function LandscapeTemplate_Loadscreen(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Try
            sFuncName = "LandscapeTemplate_Loadscreen()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oForm.Freeze(True)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("matContent").Specific
            sQueryString = "SELECT Code FROM [@AB_LANDSCAPTEMPLATE]"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            If oRS.RecordCount > 0 Then
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                oForm.Items.Item("txtCode").Specific.string = oRS.Fields.Item("Code").Value
                oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)

                oForm.EnableMenu("1282", False)

                If (oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.string <> "") Then

                    oForm.DataSources.DBDataSources.Item("@AB_LANDSCAPTEMPLINE").Clear()
                    oMatrix.AddRow(1)
                    oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific
                    oCheckBox.Checked = True

                End If
            Else

                oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE
                oForm.Items.Item("txtCode").Specific.value = "1"
                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(1).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_0").Cells.Item(1).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_3").Cells.Item(1).Specific.value = oMatrix.VisualRowCount
                oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(1).Specific
                oCheckBox.Checked = True
            End If

            oMatrix.AutoResizeColumns()


            oForm.EnableMenu("1293", True)
            oForm.EnableMenu("1283", False)
            oForm.EnableMenu("1281", False)

            oForm.Freeze(False)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            LandscapeTemplate_Loadscreen = RTN_SUCCESS
        Catch ex As Exception
            oForm.Freeze(False)
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            LandscapeTemplate_Loadscreen = RTN_ERROR
        End Try
    End Function

    Function LandscapeTemplate_Matrix_AddRow(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Try
            sFuncName = "LandscapeTemplate_Matrix_AddRow()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oMatrix = oForm.Items.Item("matContent").Specific

            If (oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.string <> "") Then

                'oForm.DataSources.DBDataSources.Item("@AB_SHOWAROUNDMLINE").Clear()
                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific
                oCheckBox.Checked = True

            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            LandscapeTemplate_Matrix_AddRow = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            LandscapeTemplate_Matrix_AddRow = RTN_ERROR
        End Try
    End Function

    Function LandscapeTemplate_ValidateQuestion(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim sQueryString As String = String.Empty
        Dim oRS As SAPbobsCOM.Recordset
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            sFuncName = "LandscapeTemplate_Validation()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("matContent").Specific

            sQueryString = "SELECT Code FROM [@AB_LANDSCAPTEMPLINE] WHERE Code='" & oForm.Items.Item("txtCode").Specific.string & "' AND U_Sort='" & p_iLandscapeTemplate & "'"
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
            LandscapeTemplate_ValidateQuestion = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            LandscapeTemplate_ValidateQuestion = RTN_ERROR
        End Try
    End Function

    Function LandscapeTemplate_DeleteEmptyRows(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty


        Try
            sFuncName = "LandscapeTemplate_DeleteEmptyRows()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            sQueryString = "DELETE FROM [@AB_LANDSCAPTEMPLINE] WHERE Code ='" & oForm.Items.Item("txtCode").Specific.string & "' AND U_Question IS NULL"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            LandscapeTemplate_DeleteEmptyRows = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            LandscapeTemplate_DeleteEmptyRows = RTN_ERROR
        End Try
    End Function

    Function LandscapeTemplate_Validation(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix

        Try

            sFuncName = "LandscapeTemplate_Validation()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oMatrix = oForm.Items.Item("matContent").Specific

            For iRowCount As Integer = 1 To oMatrix.VisualRowCount

                If oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string <> "" Then
                    If (oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string) = "" Then
                        oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                        sErrDesc = "Question Field. Cannot be Blank!. RowNo : " & iRowCount
                        Return RTN_ERROR
                    End If
                End If

            Next


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            LandscapeTemplate_Validation = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            LandscapeTemplate_Validation = RTN_ERROR
        End Try
    End Function

End Module
