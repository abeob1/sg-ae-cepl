Module modGoodsIssue

    Function CreateButton_GoodsIssue(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "CreateButton_GoodsIssue()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Startung Function", sFuncName)


            Dim oitem As SAPbouiCOM.Item
            Dim Buttoncombo As SAPbouiCOM.ButtonCombo
            Dim oColumn As SAPbouiCOM.Column
            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("13").Specific
            oForm.Freeze(True)
            oitem = oForm.Items.Add("10000331", SAPbouiCOM.BoFormItemTypes.it_BUTTON_COMBO)
            oitem.Left = oForm.Items.Item("21").Left - 5
            oitem.Top = oForm.Items.Item("2").Top
            oitem.Height = oForm.Items.Item("2").Height
            oitem.Width = 100
            Buttoncombo = oForm.Items.Item("10000331").Specific
            Buttoncombo.Caption = "Copy From PR"
            Buttoncombo.ValidValues.Add("Copy From PR", "Purchase Request")


            oMatrix.DeleteRow(1)

            oColumn = oMatrix.Columns.Add("AB_BType", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_BaseType"
            oColumn.DataBind.SetBound(True, "IGE1", "U_AB_BaseType")
            oColumn.Editable = False
            oColumn.Visible = False

            oColumn = oMatrix.Columns.Add("AB_BEntity", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_BaseEntity"
            oColumn.DataBind.SetBound(True, "IGE1", "U_AB_BaseEntry")
            oColumn.Editable = False
            oColumn.Visible = False

            oColumn = oMatrix.Columns.Add("AB_BLineNo", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_BaseLineNum"
            oColumn.DataBind.SetBound(True, "IGE1", "U_AB_BaseLineNum")
            oColumn.Editable = False
            oColumn.Visible = False

            oColumn = oMatrix.Columns.Add("AB_VQty", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_ValidationQty"
            oColumn.DataBind.SetBound(True, "IGE1", "U_AB_ValidationQty")
            oColumn.Editable = False
            oColumn.Visible = False

            oColumn = oMatrix.Columns.Add("AB_BSEntry", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_BKTDocEntry"
            oColumn.DataBind.SetBound(True, "IGE1", "U_AB_BKTDocEntry")
            oColumn.Editable = False
            oColumn.Visible = False

            oColumn = oMatrix.Columns.Add("AB_BSLId", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_BKTLineId"
            oColumn.DataBind.SetBound(True, "IGE1", "U_AB_BKTLineId")
            oColumn.Editable = False
            oColumn.Visible = False

            oMatrix.AddRow(1)

            oForm.Freeze(False)


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            CreateButton_GoodsIssue = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            CreateButton_GoodsIssue = RTN_ERROR
        End Try

    End Function

    Function Fill_MatrixValues_GoodsIssue(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        ' Dim oDBDSHeader, oDBDSDetail As SAPbouiCOM.DBDataSource
        Dim iRowCount As Integer = 0


        Try

            sFuncName = "Fill_MatrixValues_GoodsIssue()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Startung Function", sFuncName)

            'oDBDSHeader = oForm.DataSources.DBDataSources.Item(0)
            'oDBDSDetail = oForm.DataSources.DBDataSources.Item(1)
            oMatrix = oForm.Items.Item("13").Specific

            iRowCount = oMatrix.VisualRowCount

            p_oSBOApplication.StatusBar.SetText("Please Wait While Loading the Values...", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

            oForm.Freeze(True)

            If Not oDTSelectedRows Is Nothing And oDTSelectedRows.Rows.Count > 0 Then

                For iDTRowCount As Integer = 0 To oDTSelectedRows.Rows.Count - 1

                    'Fill the  Details

                    iRowCount = iRowCount + 1

                    oMatrix.Columns.Item("1").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(2).ToString()

                    oMatrix.Columns.Item("9").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(7).ToString()

                    oMatrix.Columns.Item("15").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(6).ToString()

                    oMatrix.Columns.Item("10001004").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(4).ToString()
                    oMatrix.Columns.Item("10001010").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(5).ToString()

                    oMatrix.Columns.Item("AB_BType").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(9).ToString()
                    oMatrix.Columns.Item("AB_BEntity").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(10).ToString()
                    oMatrix.Columns.Item("AB_BLineNo").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(11).ToString()
                    oMatrix.Columns.Item("AB_VQty").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(12).ToString()
                    oMatrix.Columns.Item("AB_BSEntry").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(13).ToString()
                    oMatrix.Columns.Item("AB_BSLId").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(14).ToString()
                    
                Next

                ' oMatrix.LoadFromDataSourceEx()

                p_oSBOApplication.StatusBar.SetText("Values Loaded Based on Selection", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
            End If
            oForm.Freeze(False)
            oMatrix.Columns.Item("1").Cells.Item(oMatrix.RowCount).Specific.active = True
            oMatrix.AutoResizeColumns()

            Fill_MatrixValues_GoodsIssue = RTN_SUCCESS
        Catch ex As Exception
            p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            oForm.Freeze(False)
            Fill_MatrixValues_GoodsIssue = RTN_ERROR
        End Try
    End Function

    Function PRUpdate_GI(oDT As DataTable, ByRef sErrDesc As String) As Long

        Dim sFuncName As String = String.Empty
        Dim lRetCode As Long

        Try
            sFuncName = "PRUpdate_GI()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            Dim oPurchaseRequest As SAPbobsCOM.Documents = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest)
            Dim oDTDocEntry As DataTable = Nothing
            Dim oRset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim oDView As DataView = New DataView(oDT)

            p_oSBOApplication.StatusBar.SetText("Updating the Issue Quantity In Purchase Request", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

            oDTDocEntry = oDView.ToTable(True, "PRBaseEntry")

            For imjs As Integer = 0 To oDTDocEntry.Rows.Count - 1

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting Update for PR " & oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString, sFuncName)
                oDView.RowFilter = "PRBaseEntry = '" & oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString & "'"

                If oPurchaseRequest.GetByKey(oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString.Trim) Then
                    For Each drv As DataRowView In oDView
                        oPurchaseRequest.Lines.SetCurrentLine(CInt(drv("PRLineNo").ToString.Trim))
                        oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_IssueQty").Value = oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_IssueQty").Value + CDbl(drv("IssueQty").ToString.Trim)
                    Next
                End If

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Before Update for PR " & oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString, sFuncName)
                lRetCode = oPurchaseRequest.Update()
                If lRetCode <> 0 Then
                    sErrDesc = p_oDICompany.GetLastErrorDescription
                    Call WriteToLogFile(sErrDesc, sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                Else
                    oRset.DoQuery("SELECT T0.[DocEntry] FROM PRQ1 T0 WHERE (T0.[Quantity] -  T0.[U_AB_IssueQty])  <> 0 and  T0.[DocEntry] = '" & oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString & "'")
                    If oRset.RecordCount = 0 Then
                        If oPurchaseRequest.GetByKey(oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString.Trim) Then
                            oPurchaseRequest.UserFields.Fields.Item("U_AB_FillStatus").Value = "Closed"
                            oPurchaseRequest.Update()
                            oPurchaseRequest.Close()
                        End If
                    End If
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS : " & oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString, sFuncName)
                End If
            Next imjs
            p_oSBOApplication.StatusBar.SetText("Update Completed Successfully .............. !", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
            PRUpdate_GI = RTN_SUCCESS
        Catch ex As Exception
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            PRUpdate_GI = RTN_ERROR
        End Try


    End Function

    Function BRUpdate_GI(oDT As DataTable, ByRef sErrDesc As String) As Long

        Dim sFuncName As String = String.Empty
        Dim lRetCode As Long
        Dim oRset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Dim oDView As DataView = oDT.DefaultView
        Dim sSQL As String = String.Empty

        Try
            sFuncName = "PRUpdate_GI()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            p_oSBOApplication.StatusBar.SetText("Updating the Issue Quantity In Budget Setup ........... !", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

            For Each drv As DataRowView In oDView
                '' sSQL = "[AE_SP003_BudgetIssueQty] '" & drv("Project").ToString.Trim & "', '" & drv("Year").ToString.Trim & "'," & drv("Month").ToString.Trim & ",'" & drv("ItemCode").ToString.Trim & "'," & drv("IssueQty").ToString.Trim & ""
                '' sSQL = "[AE_SP003_BudgetIssueQty] '" & drv("BKTDocEntry").ToString.Trim & "', '" & drv("BKTLineId").ToString.Trim & "'," & drv("IssueQty").ToString.Trim & ""

                sSQL = "update [@AB_STKBGT1] set U_IssueQty += " & CDbl(drv("IssueQty").ToString.Trim) & " where DocEntry = '" & drv("BKTDocEntry").ToString.Trim & "'  and LineId =  '" & drv("BKTLineId").ToString.Trim & "' "

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Update Budget Stock " & sSQL, sFuncName)
                oRset.DoQuery(sSQL)
            Next

            p_oSBOApplication.StatusBar.SetText("Update Completed Successfully ........... !", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

            BRUpdate_GI = RTN_SUCCESS
        Catch ex As Exception
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            BRUpdate_GI = RTN_ERROR

        End Try
    End Function

End Module
