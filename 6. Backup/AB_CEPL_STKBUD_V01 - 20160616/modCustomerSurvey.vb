Module modCustomerSurvey

    Function CustomerSurvey_LoadScreen(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Try

            sFuncName = "CustomerSurvey_LoadScreen()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)
            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oForm.EnableMenu("1282", False)
            oForm.EnableMenu("1283", False)
            oForm.EnableMenu("1284", False)

            oMatrix = oForm.Items.Item("19").Specific

            sQueryString = "SELECT TOP 1 DocEntry  FROM [@AB_SURVEYHEADER] ORDER BY DocEntry DESC"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)

            If oRS.RecordCount > 0 Then
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE

                oForm.Items.Item("txtDocEnt").Specific.string = oRS.Fields.Item("DocEntry").Value
                oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                oForm.DataBrowser.BrowseBy = "txtDocEnt"
                oMatrix.AutoResizeColumns()
            Else
                p_oSBOApplication.StatusBar.SetText("There is No Records Found!!!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                If oForm.Visible = True Then
                    oForm.Items.Item("2").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                End If
            End If



            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            CustomerSurvey_LoadScreen = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            CustomerSurvey_LoadScreen = RTN_ERROR
        End Try
    End Function

    Function CustomerSurvey_EnableControl(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "CustomerSurvey_EnableControl()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oForm.Items.Item("txtName").Enabled = True
            oForm.Items.Item("txtContNum").Enabled = True
            oForm.Items.Item("txtProject").Enabled = True
            oForm.Items.Item("txtDocNum").Enabled = True
            oForm.Items.Item("txtDate").Enabled = True
            oForm.Items.Item("txtImprove").Enabled = True
            oForm.Items.Item("txtComp").Enabled = True
            oForm.Items.Item("txtRecomnd").Enabled = True


            oForm.Items.Item("txtTRemark").Enabled = True
            oForm.Items.Item("txtOwnCode").Enabled = True
            oForm.Items.Item("txtOwnName").Enabled = True

            oForm.ActiveItem = "txtDocNum"
            oForm.Items.Item("txtDocNum").Refresh()
 


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            CustomerSurvey_EnableControl = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            CustomerSurvey_EnableControl = RTN_ERROR
        End Try
    End Function

    Function CustomerSurvey_DisableControl(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Try
            sFuncName = "CustomerSurvey_DisableControl()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oForm.ActiveItem = "txtDocEnt"

            oForm.Items.Item("txtName").Enabled = False
            oForm.Items.Item("txtContNum").Enabled = False
            oForm.Items.Item("txtProject").Enabled = False
            oForm.Items.Item("txtDocNum").Enabled = False
            oForm.Items.Item("txtDate").Enabled = False
            oForm.Items.Item("txtImprove").Enabled = False
            oForm.Items.Item("txtComp").Enabled = False
            oForm.Items.Item("txtRecomnd").Enabled = False
            oForm.Items.Item("txtTRemark").Enabled = False
            oForm.Items.Item("txtOwnCode").Enabled = False
            oForm.Items.Item("txtOwnName").Enabled = False

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            CustomerSurvey_DisableControl = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            CustomerSurvey_DisableControl = RTN_ERROR
        End Try
    End Function

    Function CustomerSurvey_DeleteEmptyRows(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oRS As SAPbobsCOM.Recordset
        Dim sQueryString As String = String.Empty


        Try
            sFuncName = "CustomerSurvey_DeleteEmptyRows()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oRS = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            sQueryString = "DELETE FROM [@AB_SURVEYMASTERLINE] WHERE Code ='" & oForm.Items.Item("txtCode").Specific.string & "' AND U_Category IS NULL"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing Query : " & sQueryString, sFuncName)
            oRS.DoQuery(sQueryString)


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            CustomerSurvey_DeleteEmptyRows = RTN_SUCCESS

        Catch ex As Exception
            sErrDesc = ex.Message().ToString()
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            CustomerSurvey_DeleteEmptyRows = RTN_ERROR
        End Try
    End Function

End Module
