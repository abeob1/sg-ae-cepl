Module modOpenPRList

    Public oDTSelectedRows As DataTable
    Public oDTGI As DataTable
    Public oDTIT As DataTable
    Dim iDTCount As Integer = 0


    Function Display_PurchaseRequest(ByRef oForm As SAPbouiCOM.Form, ByVal sCheckValue As String, ByVal sDocType As String, ByVal sErrDesc As String) As Long

        Dim oGrid As SAPbouiCOM.Grid
        Dim sQuery As String = String.Empty

        Try

            sFuncName = "Display_PurchaseRequest()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Startung Function", sFuncName)

            If sDocType = "GI" Then
                sQuery = "AE_SP001_OpenPurchaseRequest_GoodIssue '" & sCheckValue & "'"
            Else
                sQuery = "AE_SP002_OpenPurchaseRequest_InventoryTransfer '" & sCheckValue & "'"
            End If

            oGrid = oForm.Items.Item("3").Specific
            oGrid.DataTable = oForm.DataSources.DataTables.Add("DT1" & iDTCount)
            oGrid.DataTable.ExecuteQuery(sQuery)


            'oGrid.Columns.Item(oGrid.Columns.Count - 1).Visible = False
            'oGrid.Columns.Item(oGrid.Columns.Count - 2).Visible = False

            oGrid.Columns.Item(0).Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox

            For IntRow As Integer = 1 To oGrid.Columns.Count - 1
                oGrid.Columns.Item(IntRow).Editable = False
            Next IntRow

            Dim rowheader As SAPbouiCOM.RowHeaders
            rowheader = oGrid.RowHeaders
            rowheader.TitleObject.Caption = "#"

            For iRowCount = 0 To oGrid.Rows.Count - 1
                rowheader.SetText(iRowCount, iRowCount + 1)
            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Display_PurchaseRequest = RTN_SUCCESS
            iDTCount = iDTCount + 1
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Display_PurchaseRequest = RTN_ERROR

        End Try


    End Function

    Function GetSelectedRows(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As DataTable

        Dim oGrid As SAPbouiCOM.Grid
        Dim sTitle As String = String.Empty

        Try
            sFuncName = "GetSelectedRows()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Startung Function", sFuncName)

            oGrid = oForm.Items.Item("3").Specific

            oDTSelectedRows = New DataTable


            oDTSelectedRows.Columns.Add("Select", GetType(String))
            oDTSelectedRows.Columns.Add("DocNum", GetType(Integer))
            oDTSelectedRows.Columns.Add("ItemCode", GetType(String))
            oDTSelectedRows.Columns.Add("Dscription", GetType(String))
            oDTSelectedRows.Columns.Add("OcrCode", GetType(String))
            oDTSelectedRows.Columns.Add("OcrCode2", GetType(String))

            oDTSelectedRows.Columns.Add("WhsCode", GetType(String))
            oDTSelectedRows.Columns.Add("Quanity", GetType(Double))
            oDTSelectedRows.Columns.Add("ProiceBefDi", GetType(Double))
            oDTSelectedRows.Columns.Add("BaseType", GetType(Integer))
            oDTSelectedRows.Columns.Add("BaseEntry", GetType(Integer))
            oDTSelectedRows.Columns.Add("BaseLineNum", GetType(Integer))
            oDTSelectedRows.Columns.Add("VQuanity", GetType(Double))
            oDTSelectedRows.Columns.Add("BUDDocEntry", GetType(String))
            oDTSelectedRows.Columns.Add("BUDLineNo", GetType(String))


            For iDTRows As Integer = 0 To oGrid.DataTable.Rows.Count - 1

                'MsgBox(oGrid.DataTable.GetValue(0, iDTRows))

                If oGrid.DataTable.GetValue(0, iDTRows) <> "Y" Then Continue For


                oDTSelectedRows.Rows.Add(oGrid.DataTable.GetValue(0, iDTRows).ToString(), CInt(oGrid.DataTable.GetValue(1, iDTRows).ToString()) _
                                         , oGrid.DataTable.GetValue(2, iDTRows).ToString(), oGrid.DataTable.GetValue(3, iDTRows).ToString() _
                                         , oGrid.DataTable.GetValue(4, iDTRows).ToString(), oGrid.DataTable.GetValue(5, iDTRows).ToString() _
                                         , oGrid.DataTable.GetValue(6, iDTRows).ToString(), CDbl(oGrid.DataTable.GetValue(7, iDTRows).ToString()) _
                                        , CDbl(oGrid.DataTable.GetValue(8, iDTRows).ToString()), CInt(oGrid.DataTable.GetValue(9, iDTRows).ToString()) _
                                         , CInt(oGrid.DataTable.GetValue(10, iDTRows).ToString()), CInt(oGrid.DataTable.GetValue(11, iDTRows).ToString()), _
                                         CDbl(oGrid.DataTable.GetValue(12, iDTRows).ToString()), oGrid.DataTable.GetValue(13, iDTRows).ToString(), oGrid.DataTable.GetValue(14, iDTRows).ToString())


            Next

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

            GetSelectedRows = oDTSelectedRows

        Catch ex As Exception
            p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GetSelectedRows = Nothing
        End Try
    End Function

End Module
