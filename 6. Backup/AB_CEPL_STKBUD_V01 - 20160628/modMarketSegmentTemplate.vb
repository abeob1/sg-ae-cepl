Module modMarketSegmentTemplate

    Function MarketSegmentTemplate_FormLoad(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim sDocDate As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            sFuncName = "MarketSegmentTemplate_FormLoad()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("matContent").Specific

            ''sQueryString = "select COUNT( CAST(ISNULL(DocNum,0) AS INT))+1 [DocEntry] from [@AB_SHOWAROUND] "
            ''sQueryString = "select Top 1 NextNumber [DocEntry] from NNM1 where ObjectCode ='MarketSegTemplate' and Locked ='N' "
            ''If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            ''oRS.DoQuery(sQueryString)

            ''oForm.Items.Item("txtDocEntr").Specific.string = oRS.Fields.Item("DocEntry").Value

            'sDocDate = Today.Day & Today.Month.ToString() & Today.Year.ToString()

            ''If Today.Month.ToString().Length = 1 And Today.Day.ToString().Length = 1 Then
            ''    sDocDate = "0" & Today.Day & "0" & Today.Month.ToString() & Today.Year.ToString()
            ''ElseIf Today.Month.ToString().Length = 1 And Today.Day.ToString().Length = 2 Then
            ''    sDocDate = Today.Day & "0" & Today.Month.ToString() & Today.Year.ToString()
            ''ElseIf Today.Month.ToString().Length = 2 And Today.Day.ToString().Length = 2 Then
            ''    sDocDate = "0" & Today.Day & Today.Month.ToString() & Today.Year.ToString()
            ''End If

            '' oForm.Items.Item("txtDocDate").Specific.string = sDocDate   ''GetDate(Now.Date, p_oDICompany)


            'oForm.Items.Item("txtDocEntr").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
            '' oForm.PaneLevel = 1

            'oForm.Items.Item("txtDocNum").Enabled = False
            'oForm.Items.Item("txtDocDate").Enabled = False
            If oMatrix.RowCount = 0 Then
                oForm.DataSources.DBDataSources.Item("@AB_MKTSGTTEMPLATE1").Clear()
                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
            Else
                If Not String.IsNullOrEmpty(oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.RowCount).Specific.String) Then
                    oForm.DataSources.DBDataSources.Item("@AB_MKTSGTTEMPLATE1").Clear()
                    oMatrix.AddRow(1)
                    oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                    oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                End If
            End If

            '' oMatrix.AutoResizeColumns()
            oForm.DataBrowser.BrowseBy = "txtMarSeg"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            MarketSegmentTemplate_FormLoad = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            MarketSegmentTemplate_FormLoad = RTN_ERROR
        End Try


    End Function

    Function MarketSegmentTemplate_MatrixAddRow(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            sFuncName = "MarketSegmentTemplate_MatrixAddRow()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oMatrix = oForm.Items.Item("matContent").Specific

            If (oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.string <> "") Then
                oForm.DataSources.DBDataSources.Item("@AB_MKTSGTTEMPLATE1").Clear()
                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            MarketSegmentTemplate_MatrixAddRow = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            MarketSegmentTemplate_MatrixAddRow = RTN_ERROR
        End Try
    End Function

    Function MarketSegmentTemplate_DeleteEmptyRows(ByRef oForm As SAPbouiCOM.Form, ByVal sDocentry As String, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty


        Try
            sFuncName = "MarketSegmentTemplate_DeleteEmptyRows()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            sQueryString = "DELETE FROM [@AB_MKTSGTTEMPLATE1] WHERE DocEntry ='" & sDocentry & "' AND isnull(U_Category,'') = '' AND isnull(U_Item,'') = ''"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            MarketSegmentTemplate_DeleteEmptyRows = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            MarketSegmentTemplate_DeleteEmptyRows = RTN_ERROR
        End Try
    End Function

    Function MarketSegmentTemplate_Validation(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix

        Try

            sFuncName = "MarketSegmentTemplate_Validation()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oMatrix = oForm.Items.Item("matContent").Specific

            If Not String.IsNullOrEmpty(oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.VisualRowCount).Specific.String) Or _
                 Not String.IsNullOrEmpty(oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific.String) Then
                oForm.DataSources.DBDataSources.Item("@AB_MKTSGTTEMPLATE1").Clear()
                oMatrix.AddRow(1)
                oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
                oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.value = oMatrix.VisualRowCount
            End If

            If oForm.Items.Item("txtMarSeg").Specific.string = "" Then
                sErrDesc = "Marketing Segment Field. Cannot be Blank!."
                oForm.Items.Item("txtMarSeg").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                Return RTN_ERROR
            ElseIf oMatrix.RowCount = 1 Then
                If (oMatrix.Columns.Item("V_2").Cells.Item(1).Specific.string = "" Or oMatrix.Columns.Item("V_1").Cells.Item(1).Specific.string = "") Then
                    If (oMatrix.Columns.Item("V_2").Cells.Item(1).Specific.string = "") Then
                        sErrDesc = "Category. Cannot be Blank!. RowNo : 1"
                        oMatrix.Columns.Item("V_2").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    ElseIf (oMatrix.Columns.Item("V_1").Cells.Item(1).Specific.string = "") Then
                        sErrDesc = "Item Field. Cannot be Blank!. RowNo : 1"
                        oMatrix.Columns.Item("V_1").Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    End If
                    Return RTN_ERROR
                End If

            ElseIf oMatrix.RowCount > 0 Then
                For iRowCount As Integer = 1 To oMatrix.VisualRowCount - 1

                    If (oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string = "" Or oMatrix.Columns.Item("V_1").Cells.Item(iRowCount).Specific.string = "") Then
                        If (oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Specific.string = "") Then
                            oMatrix.Columns.Item("V_2").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            sErrDesc = "Category. Cannot be Blank!. RowNo : " & iRowCount
                        ElseIf (oMatrix.Columns.Item("V_1").Cells.Item(iRowCount).Specific.string = "") Then
                            oMatrix.Columns.Item("V_1").Cells.Item(iRowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            sErrDesc = "Item Field. Cannot be Blank!. RowNo : " & iRowCount
                        End If
                        Return RTN_ERROR
                    End If
                Next
            End If

            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                If p_MSTDragdrop = True Then
                    sErrDesc = "Drag flag is activated - Drop the category which you draged "
                    Return RTN_ERROR
                End If
                For imjs As Integer = 1 To oMatrix.RowCount
                    oMatrix.Columns.Item("V_-1").Cells.Item(imjs).Specific.String = imjs
                    oMatrix.Columns.Item("V_0").Cells.Item(imjs).Specific.String = imjs
                Next
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            MarketSegmentTemplate_Validation = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            MarketSegmentTemplate_Validation = RTN_ERROR
        End Try
    End Function

    Function MarketSegmentTemplate_Question(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim sQueryString As String = String.Empty
        Dim oRS As SAPbobsCOM.Recordset
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            sFuncName = "MarketSegmentTemplate_Question()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = oForm.Items.Item("matContent").Specific

            sQueryString = "SELECT Code FROM [@AB_MKTSGTTEMPLATE1] WHERE Code='" & oForm.Items.Item("txtCode").Specific.string & "' AND U_Sort='" & iCurrentRow & "'"
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
            MarketSegmentTemplate_Question = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            MarketSegmentTemplate_Question = RTN_ERROR
        End Try
    End Function

End Module
