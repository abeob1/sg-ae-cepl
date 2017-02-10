Module modMarketSegmentChecklist

    Function MarketSegmentChecklist_LoadScreen(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oMatrixAttach As SAPbouiCOM.Matrix
        Try

            sFuncName = "MarketSegmentChecklist_LoadScreen()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oForm.EnableMenu("1282", False)
            oForm.EnableMenu("1283", False)
            oForm.EnableMenu("1284", False)

            oMatrix = oForm.Items.Item("matContent").Specific
            oMatrixAttach = oForm.Items.Item("matAttach").Specific

            sQueryString = "SELECT TOP 1 DocEntry  FROM [@AB_MKTSGTCHK] ORDER BY DocEntry DESC"

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
                If MarketSegmentChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
            Else
                p_oSBOApplication.StatusBar.SetText("There is No Records Found!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                If oForm.Visible = True Then
                    oForm.Items.Item("2").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                End If
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            MarketSegmentChecklist_LoadScreen = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            MarketSegmentChecklist_LoadScreen = RTN_ERROR
        End Try
    End Function

    Function MarketSegmentChecklist_EnableControl(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "MarketSegmentChecklist_EnableControl()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oForm.Items.Item("txtDocNum").Enabled = True
            oForm.Items.Item("txtDocDate").Enabled = True
            oForm.Items.Item("txtProject").Enabled = True
            oForm.Items.Item("txtMarSeg").Enabled = True
            oForm.Items.Item("matContent").Enabled = True
            oForm.Items.Item("matAttach").Enabled = True
            oForm.Items.Item("picESign").Enabled = True
            oForm.Items.Item("txtRemarks").Enabled = True

            oForm.ActiveItem = "txtDocNum"
 
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            MarketSegmentChecklist_EnableControl = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            MarketSegmentChecklist_EnableControl = RTN_ERROR
        End Try
    End Function

    Function MarketSegmentChecklist_DisableControl(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "CustomerSurvey_DisableControl()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oForm.ActiveItem = "txtDocEntr"

            oForm.Items.Item("txtDocNum").Enabled = False
            oForm.Items.Item("txtDocDate").Enabled = False
            oForm.Items.Item("txtProject").Enabled = False
            oForm.Items.Item("txtMarSeg").Enabled = False
            oForm.Items.Item("matContent").Enabled = False
            oForm.Items.Item("matAttach").Enabled = False
            oForm.Items.Item("picESign").Enabled = False
            oForm.Items.Item("txtRemarks").Enabled = False


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            MarketSegmentChecklist_DisableControl = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            MarketSegmentChecklist_DisableControl = RTN_ERROR
        End Try
    End Function

End Module
