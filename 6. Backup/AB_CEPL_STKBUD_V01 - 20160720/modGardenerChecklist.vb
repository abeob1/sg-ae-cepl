Module modGardenerChecklist

    Function GardenerChecklist_LoadScreen(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oMatrixAttach As SAPbouiCOM.Matrix
        Try

            sFuncName = "GardenerChecklist_LoadScreen()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oForm.Items.Item("35").Enabled = False
            Dim Tmp_val As String = oForm.BusinessObject.GetNextSerialNumber(NextSerialNo(p_oDICompany, p_oSBOApplication, "GardenerChecklist"))
            oForm.Items.Item("35").Specific.String = Tmp_val

            oForm.EnableMenu("1282", False)
            oForm.EnableMenu("1283", False)
            oForm.EnableMenu("1284", False)

            oMatrix = oForm.Items.Item("matContent").Specific
            oMatrixAttach = oForm.Items.Item("matAttach").Specific

            sQueryString = "SELECT TOP 1 DocEntry  FROM [@AB_GARDENERCHK] ORDER BY DocEntry DESC"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            If oRS.RecordCount > 0 Then
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE

                oForm.Items.Item("txtDocEntr").Specific.string = oRS.Fields.Item("DocEntry").Value
                oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                oForm.DataBrowser.BrowseBy = "txtDocEntr"
                oMatrix.AutoResizeColumns()
                oMatrixAttach.AutoResizeColumns()
                oForm.Items.Item("10").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                If GardenerChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            Else
                p_oSBOApplication.StatusBar.SetText("There is No Records Found!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                If oForm.Visible = True Then
                    oForm.Items.Item("2").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                End If
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            GardenerChecklist_LoadScreen = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GardenerChecklist_LoadScreen = RTN_ERROR
        End Try
    End Function

    Function GardenerChecklist_EnableControl(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "GardenerChecklist_EnableControl()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oForm.Items.Item("35").Enabled = True
            oForm.Items.Item("txtProject").Enabled = True
            oForm.Items.Item("txtTimeIn").Enabled = True
            oForm.Items.Item("txtDateIss").Enabled = True
            oForm.Items.Item("txtConRef").Enabled = True
            oForm.Items.Item("txtTimeOut").Enabled = True
            oForm.Items.Item("txtDateRec").Enabled = True
            oForm.Items.Item("matContent").Enabled = True
            oForm.Items.Item("matAttach").Enabled = True
            oForm.Items.Item("txtSupName").Enabled = True

            oForm.Items.Item("txtESign").Enabled = True
            oForm.Items.Item("txtSupDate").Enabled = True
            oForm.Items.Item("txtCliName").Enabled = True
            oForm.Items.Item("txtECSign").Enabled = True
            oForm.Items.Item("txtCliDate").Enabled = True

            oForm.ActiveItem = "txtProject"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            GardenerChecklist_EnableControl = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GardenerChecklist_EnableControl = RTN_ERROR
        End Try
    End Function

    Function GardenerChecklist_DisableControl(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "GardenerChecklist_DisableControl()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oForm.ActiveItem = "txtDocEntr"

            oForm.Items.Item("35").Enabled = False
            oForm.Items.Item("txtProject").Enabled = False
            oForm.Items.Item("txtTimeIn").Enabled = False
            oForm.Items.Item("txtDateIss").Enabled = False
            oForm.Items.Item("txtConRef").Enabled = False
            oForm.Items.Item("txtTimeOut").Enabled = False
            oForm.Items.Item("txtDateRec").Enabled = False
            oForm.Items.Item("matContent").Enabled = False
            oForm.Items.Item("matAttach").Enabled = False
            oForm.Items.Item("txtSupName").Enabled = False

            oForm.Items.Item("txtESign").Enabled = False
            oForm.Items.Item("txtSupDate").Enabled = False
            oForm.Items.Item("txtCliName").Enabled = False
            oForm.Items.Item("txtECSign").Enabled = False
            oForm.Items.Item("txtCliDate").Enabled = False

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            GardenerChecklist_DisableControl = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GardenerChecklist_DisableControl = RTN_ERROR
        End Try
    End Function

End Module
