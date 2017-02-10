Module modPestMgmtService


    Public Function PestMaster_Binding(ByRef oform As SAPbouiCOM.Form, ByRef oCompany As SAPbobsCOM.Company, ByRef oApplication As SAPbouiCOM.Application, ByRef sErrDesc As String) As Long

        ' ***********************************************************************************
        '   Function   :    PestMaster_Binding()
        '   Purpose    :    This function is handles - Delete the Schema file
        '   Parameters :    ByVal csvFileFolder As String
        '                       csvFileFolder = Passing file name
        '   Author     :    JOHN
        '   Date       :    26/06/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************
        Try
            sFuncName = "PestMaster_Binding()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)
            Dim oMatrix As SAPbouiCOM.Matrix = oform.Items.Item("txtMatrix").Specific
            Dim orset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            orset.DoQuery("SELECT T0.[Code], T0.[Name] FROM [dbo].[@AB_PESTTEMPLATEMAS]  T0")
            oform.Freeze(True)
            oMatrix.Columns.Item("V_3").Width = 600
            oMatrix.Columns.Item("V_1").Visible = True
            oMatrix.Columns.Item("V_2").Visible = False
            oMatrix.Columns.Item("V_4").Visible = False
            oMatrix.Columns.Item("V_5").Visible = False
            If orset.RecordCount = 0 Then
                oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE
                oMatrix.AddRow()
                oMatrix.Columns.Item("V_-1").Cells.Item(1).Specific.String = "1"
                oMatrix.Columns.Item("V_4").Cells.Item(1).Specific.String = "1"
                oMatrix.Columns.Item("V_5").Cells.Item(1).Specific.String = "1"
                oMatrix.Columns.Item("V_1").Cells.Item(1).Specific.checked = True
                oform.Items.Item("5").Specific.string = "1"
                oform.Items.Item("6").Specific.string = "1"
            Else
                oform.Items.Item("5").Visible = True
                oform.Items.Item("5").Specific.string = "1"
                oform.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                oform.Items.Item("5").Visible = False
            End If
            oform.EnableMenu("1293", True)
            ''If Not p_oSBOApplication.Menus.Exists("1293") Then
            ''    'If the menu already exists this code will fail
            ''    p_oSBOApplication.Menus.AddEx("1293")
            ''End If
            oform.Freeze(False)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            PestMaster_Binding = RTN_SUCCESS
        Catch ex As Exception
            oform.Freeze(False)
            oApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            PestMaster_Binding = RTN_ERROR
        End Try
    End Function











End Module
