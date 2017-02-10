Option Explicit On



Module modMain


    Public p_iDebugMode As Int16
    Public p_iErrDispMethod As Int16
    Public p_iDeleteDebugLog As Int16

    Public Const RTN_SUCCESS As Int16 = 1
    Public Const RTN_ERROR As Int16 = 0

    Public Const DEBUG_ON As Int16 = 1
    Public Const DEBUG_OFF As Int16 = 0

    Public Const ERR_DISPLAY_STATUS As Int16 = 1
    Public Const ERR_DISPLAY_DIALOGUE As Int16 = 2


    Public p_oApps As SAPbouiCOM.SboGuiApi
    Public p_oEventHandler As clsEventHandler
    Public WithEvents p_oSBOApplication As SAPbouiCOM.Application
    Public p_oDICompany As SAPbobsCOM.Company
    Public p_oUICompany As SAPbouiCOM.Company
    Public sFuncName As String
    Public sErrDesc As String
    Public sProjectName As String = String.Empty
    Public p_Closeflag As Boolean = False

    Public p_sSelectedFilepath As String = String.Empty
    Public p_sSelectedFileName As String = String.Empty

    Public iFormType_IT As Integer = 0
    Public iFormType_GI As Integer = 0

    'Public p_sRefNuber(100, 4) As String
    'Public p_iArrayCount As Integer = 0
    'Public p_iArrayAcctCount As Integer = 0
    'Public p_iArrayAcctActiveCount As Integer = 0
    'Public p_sAccountCodes(100) As String
    'Public p_sAccountCodes_ActiveAccount(100) As String
    Public p_iRowCount As Integer = 0
    Public p_formCount_IN As Integer = 0
    Public p_formCount_GI As Integer = 0
    Public p_Dragdrop As Boolean = False
    Public p_CSQDragdrop As Boolean = False
    Public p_PestDragdrop As Boolean = False
    Public p_iPestRow As Integer = 0
    Public p_ishowaround_question As Integer = 0
    Public p_isurveyquestion As Integer = 0
    Public p_iservicequestion As Integer = 0
    Public p_ijobareaquestion As Integer = 0
    Public p_iGardenerTemplate As Integer = 0
    Public p_iLandscapeTemplate As Integer = 0

    Public p_MLTDragdrop As Boolean = False
    Public p_JCTDragdrop As Boolean = False
    Public p_JJADragdrop As Boolean = False
    Public p_MGTDragdrop As Boolean = False
    Public p_MSTDragdrop As Boolean = False
    Public p_sDocEntry As String = String.Empty
    Public p_sCompanyTheme As String = Nothing
    Dim sSQL As String = String.Empty
    Public p_dtTable As New DataTable
    Public p_oDtDailySchedule As New DataTable

    ''  Public bCFL As Boolean = False


    Sub main(ByVal Args() As String)

        Try

            sFuncName = "Main()"
            p_iDebugMode = DEBUG_ON
            p_iErrDispMethod = ERR_DISPLAY_STATUS

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Addon startup function", sFuncName)
            p_oApps = New SAPbouiCOM.SboGuiApi
            'p_oApps.Connect(Args(0))

            Dim sconn As String = Environment.GetCommandLineArgs.GetValue(1)
            p_oApps.Connect(sconn)


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Initializing public SBO Application object", sFuncName)
            p_oSBOApplication = p_oApps.GetApplication

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Retriving SBO application company handle", sFuncName)
            p_oUICompany = p_oSBOApplication.Company


            p_oDICompany = New SAPbobsCOM.Company
            If Not p_oDICompany.Connected Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConnectDICompSSO()", sFuncName)
                If ConnectDICompSSO(p_oDICompany, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
            End If

       
            Call WriteToLogFile_Debug("Calling DisplayStatus()", sFuncName)
            'Call DisplayStatus(Nothing, "Addon starting.....please wait....", sErrDesc)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Creating Event handler class", sFuncName)
            p_oEventHandler = New clsEventHandler(p_oSBOApplication, p_oDICompany)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling SetApplication Function", sFuncName)
            ' Call p_oEventHandler.SetApplication(sErrDesc)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Addon started successfully", "Main()")

            Call WriteToLogFile_Debug("Calling EndStatus()", sFuncName)
            ' Call EndStatus(sErrDesc)
            p_oSBOApplication.StatusBar.SetText("Addon Started Successfully", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

            sSQL = "SELECT T0.[U_Theme] FROM [dbo].[@AB_COMPANYSETUP]  T0 where  T0.[U_DBName] = '" & p_oDICompany.CompanyDB & "'"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Company Data SQL" & sSQL, "Main()")
            Dim oRset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRset.DoQuery(sSQL)
            p_sCompanyTheme = oRset.Fields.Item("U_Theme").Value

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling AddMenus Functions", sFuncName)
            Call AddMenuItems()
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("AddMenus Functions Completed Successfully.", sFuncName)

            System.Windows.Forms.Application.Run()

        Catch exp As Exception
            Call WriteToLogFile(exp.Message, "Main()")
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", "Main()")
        Finally
        End Try
    End Sub

End Module





