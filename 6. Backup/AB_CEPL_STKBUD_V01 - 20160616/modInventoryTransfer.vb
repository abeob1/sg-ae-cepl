Module modInventoryTransfer



    Function CreateButton_InventoryTransfer(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "CreateButton_InventoryTransfer()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Startung Function", sFuncName)

            Dim oitem As SAPbouiCOM.Item
            Dim Buttoncombo As SAPbouiCOM.ButtonCombo
            Dim oColumn As SAPbouiCOM.Column
            Dim oMatrix As SAPbouiCOM.Matrix = Nothing

            oMatrix = oForm.Items.Item("23").Specific

            oForm.Freeze(True)
            oitem = oForm.Items.Add("100003311", SAPbouiCOM.BoFormItemTypes.it_BUTTON_COMBO)
            oitem.Left = oForm.Items.Item("1250000074").Left + oForm.Items.Item("1250000074").Width + 5
            oitem.Top = oForm.Items.Item("2").Top
            oitem.Height = oForm.Items.Item("2").Height
            oitem.Width = 100
            Buttoncombo = oForm.Items.Item("100003311").Specific
            Buttoncombo.Caption = "Copy From PR"
            Buttoncombo.ValidValues.Add("Copy From PR", "Purchase Request")

            oMatrix.DeleteRow(1)

            oColumn = oMatrix.Columns.Add("AB_BType", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_BaseType"
            oColumn.DataBind.SetBound(True, "WTR1", "U_AB_BaseType")
            oColumn.Editable = False
            oColumn.Visible = False

            oColumn = oMatrix.Columns.Add("AB_BEntity", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_BaseEntity"
            oColumn.DataBind.SetBound(True, "WTR1", "U_AB_BaseEntry")
            oColumn.Editable = False
            oColumn.Visible = False

            oColumn = oMatrix.Columns.Add("AB_BLineNo", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_BaseLineNum"
            oColumn.DataBind.SetBound(True, "WTR1", "U_AB_BaseLineNum")
            oColumn.Editable = False
            oColumn.Visible = False

            oColumn = oMatrix.Columns.Add("AB_VQty", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_ValidationQty"
            oColumn.DataBind.SetBound(True, "WTR1", "U_AB_ValidationQty")
            oColumn.Editable = False
            oColumn.Visible = False

            oColumn = oMatrix.Columns.Add("AB_BSEntry", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_BKTDocEntry"
            oColumn.DataBind.SetBound(True, "WTR1", "U_AB_BKTDocEntry")
            oColumn.Editable = False
            oColumn.Visible = False

            oColumn = oMatrix.Columns.Add("AB_BSLId", SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oColumn.TitleObject.Caption = "U_AB_BKTLineId"
            oColumn.DataBind.SetBound(True, "WTR1", "U_AB_BKTLineId")
            oColumn.Editable = False
            oColumn.Visible = False

            oMatrix.AddRow(1)

            oForm.Freeze(False)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            CreateButton_InventoryTransfer = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            CreateButton_InventoryTransfer = RTN_ERROR
        End Try

    End Function

    Function Fill_MatrixValues_InventoryTransfer(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim iRowCount As Integer = 0

        Try

            sFuncName = "Fill_MatrixValues_InventoryTransfer()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Startung Function", sFuncName)

            oMatrix = oForm.Items.Item("23").Specific

            iRowCount = oMatrix.VisualRowCount

            p_oSBOApplication.StatusBar.SetText("Please Wait While Loading the Values...", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

            oForm.Freeze(True)

            If Not oDTSelectedRows Is Nothing And oDTSelectedRows.Rows.Count > 0 Then

                For iDTRowCount As Integer = 0 To oDTSelectedRows.Rows.Count - 1

                    'Fill the  Details

                    iRowCount = iRowCount + 1

                    oMatrix.Columns.Item("1").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(2).ToString()
                    oMatrix.Columns.Item("10").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(7).ToString()

                    'oMatrix.Columns.Item("1470001039").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(6).ToString()
                    oMatrix.Columns.Item("5").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(6).ToString()
                    oMatrix.Columns.Item("10001005").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(4).ToString()
                    oMatrix.Columns.Item("10001011").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(5).ToString()
                    oMatrix.Columns.Item("AB_BType").Cells.Item(iRowCount - 1).Specific.String = oDTSelectedRows.Rows(iDTRowCount)(9).ToString()
                    oMatrix.Columns.Item("AB_BEntity").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(10).ToString()
                    oMatrix.Columns.Item("AB_BLineNo").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(11).ToString()
                    oMatrix.Columns.Item("AB_VQty").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(12).ToString()
                    oMatrix.Columns.Item("AB_BSEntry").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(13).ToString()
                    oMatrix.Columns.Item("AB_BSLId").Cells.Item(iRowCount - 1).Specific.value = oDTSelectedRows.Rows(iDTRowCount)(14).ToString()
                Next

                p_oSBOApplication.StatusBar.SetText("Values Loaded Based on Selection", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

            End If
            oForm.Freeze(False)
            oMatrix.Columns.Item("1").Cells.Item(oMatrix.RowCount).Specific.active = True
            oMatrix.AutoResizeColumns()

            Fill_MatrixValues_InventoryTransfer = RTN_SUCCESS
        Catch ex As Exception
            p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            oForm.Freeze(False)
            Fill_MatrixValues_InventoryTransfer = RTN_ERROR
        End Try
    End Function

    Function PRUpdate_IT(oDT As DataTable, ByRef sErrDesc As String) As Long

        Dim sFuncName As String = String.Empty
        Dim lRetCode As Long

        Try
            sFuncName = "PRUpdate_GI()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            Dim oPurchaseRequest As SAPbobsCOM.Documents = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest)
            Dim oDView As DataView = New DataView(oDT)

            Dim oDTDocEntry As DataTable = Nothing
            Dim oRset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)


            oDTDocEntry = oDView.ToTable(True, "PRBaseEntry")

            For imjs As Integer = 0 To oDTDocEntry.Rows.Count - 1

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting Update for PR " & oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString, sFuncName)
                oDView.RowFilter = "PRBaseEntry = '" & oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString & "'"

                If oPurchaseRequest.GetByKey(oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString.Trim) Then
                    For Each drv As DataRowView In oDView
                        oPurchaseRequest.Lines.SetCurrentLine(CInt(drv("PRLineNo").ToString.Trim))
                        oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_TransferQty").Value = oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_TransferQty").Value + CDbl(drv("TransferQty").ToString.Trim)
                    Next

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Before Update for PR " & oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString, sFuncName)
                    lRetCode = oPurchaseRequest.Update()
                    If lRetCode <> 0 Then
                        sErrDesc = p_oDICompany.GetLastErrorDescription
                        Call WriteToLogFile(sErrDesc, sFuncName)
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                    Else

                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS : " & oDTDocEntry.Rows(imjs).Item("PRBaseEntry").ToString, sFuncName)
                    End If
                End If
            Next imjs

            p_oSBOApplication.StatusBar.SetText("Update Completed Successfully ......... !", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

            PRUpdate_IT = RTN_SUCCESS
        Catch ex As Exception
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            PRUpdate_IT = RTN_ERROR
        End Try


    End Function


End Module
