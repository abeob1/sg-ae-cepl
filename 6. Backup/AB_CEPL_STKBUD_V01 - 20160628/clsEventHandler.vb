Option Explicit On
'Imports SAPbouiCOM.Framework
Imports System.Windows.Forms
Imports System.IO


Public Class clsEventHandler
    Dim WithEvents SBO_Application As SAPbouiCOM.Application ' holds connection with SBO
    Dim p_oDICompany As New SAPbobsCOM.Company

    Public Sub New(ByRef oApplication As SAPbouiCOM.Application, ByRef oCompany As SAPbobsCOM.Company)
        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "Class_Initialize()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Retriving SBO Application handle", sFuncName)
            SBO_Application = oApplication
            p_oDICompany = oCompany

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch exc As Exception
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call WriteToLogFile(exc.Message, sFuncName)
        End Try
    End Sub

    Public Function SetApplication(ByRef sErrDesc As String) As Long
        ' **********************************************************************************
        '   Function   :    SetApplication()
        '   Purpose    :    This function will be calling to initialize the default settings
        '                   such as Retrieving the Company Default settings, Creating Menus, and
        '                   Initialize the Event Filters
        '               
        '   Parameters :    ByRef sErrDesc AS string
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        ' **********************************************************************************
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "SetApplication()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling SetMenus()", sFuncName)
            If SetMenus(sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling SetFilters()", sFuncName)
            If SetFilters(sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            SetApplication = RTN_SUCCESS
        Catch exc As Exception
            sErrDesc = exc.Message
            Call WriteToLogFile(exc.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            SetApplication = RTN_ERROR
        End Try
    End Function

    Private Function SetMenus(ByRef sErrDesc As String) As Long
        ' **********************************************************************************
        '   Function   :    SetMenus()
        '   Purpose    :    This function will be gathering to create the customized menu
        '               
        '   Parameters :    ByRef sErrDesc AS string
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        ' **********************************************************************************
        Dim sFuncName As String = String.Empty
        ' Dim oMenuItem As SAPbouiCOM.MenuItem
        Try
            sFuncName = "SetMenus()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            SetMenus = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            SetMenus = RTN_ERROR
        End Try
    End Function

    Private Function SetFilters(ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function   :    SetFilters()
        '   Purpose    :    This function will be gathering to declare the event filter 
        '                   before starting the AddOn Application
        '               
        '   Parameters :    ByRef sErrDesc AS string
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        ' **********************************************************************************

        Dim oFilters As SAPbouiCOM.EventFilters
        Dim oFilter As SAPbouiCOM.EventFilter
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "SetFilters()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Initializing EventFilters object", sFuncName)
            oFilters = New SAPbouiCOM.EventFilters



            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Adding filters", sFuncName)
            SBO_Application.SetFilter(oFilters)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            SetFilters = RTN_SUCCESS
        Catch exc As Exception
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            SetFilters = RTN_ERROR
        End Try
    End Function

    Private Sub SBO_Application_AppEvent(ByVal EventType As SAPbouiCOM.BoAppEventTypes) Handles SBO_Application.AppEvent
        ' **********************************************************************************
        '   Function   :    SBO_Application_AppEvent()
        '   Purpose    :    This function will be handling the SAP Application Event
        '               
        '   Parameters :    ByVal EventType As SAPbouiCOM.BoAppEventTypes
        '                       EventType = set the SAP UI Application Eveny Object        
        ' **********************************************************************************
        Dim sFuncName As String = String.Empty
        Dim sErrDesc As String = String.Empty
        Dim sMessage As String = String.Empty

        Try
            sFuncName = "SBO_Application_AppEvent()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            Select Case EventType
                Case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged, SAPbouiCOM.BoAppEventTypes.aet_ShutDown, SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition
                    sMessage = String.Format("Please wait for a while to disconnect the AddOn {0} ....", System.Windows.Forms.Application.ProductName)
                    p_oSBOApplication.SetStatusBarMessage(sMessage, SAPbouiCOM.BoMessageTime.bmt_Medium, False)
                    End
            End Select

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch ex As Exception
            sErrDesc = ex.Message
            WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            ShowErr(sErrDesc)
        Finally
            GC.Collect()  'Forces garbage collection of all generations.
        End Try
    End Sub

    Private Sub SBO_Application_MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean) Handles SBO_Application.MenuEvent
        ' **********************************************************************************
        '   Function   :    SBO_Application_MenuEvent()
        '   Purpose    :    This function will be handling the SAP Menu Event
        '               
        '   Parameters :    ByRef pVal As SAPbouiCOM.MenuEvent
        '                       pVal = set the SAP UI MenuEvent Object
        '                   ByRef BubbleEvent As Boolean
        '                       BubbleEvent = set the True/False        
        ' **********************************************************************************
        Dim oForm As SAPbouiCOM.Form = Nothing
        Dim sErrDesc As String = String.Empty
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "SBO_Application_MenuEvent()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            If Not p_oDICompany.Connected Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConnectDICompSSO()", sFuncName)
                If ConnectDICompSSO(p_oDICompany, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
            End If

            If pVal.BeforeAction = False Then
                Select Case pVal.MenuUID
                    Case "SBS"
                        Try
                            LoadFromXML("StockBudget.srf", SBO_Application)
                            oForm = SBO_Application.Forms.Item("SBS")
                            StockBudget_Binding(oForm, p_oDICompany, p_oSBOApplication, sErrDesc)

                            oForm.Visible = True
                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub


                    Case "PMSM"

                        Try
                            LoadFromXML("PestServiceMaster.srf", SBO_Application)
                            oForm = SBO_Application.Forms.Item("PMSM")
                            ' oForm.Visible = False
                            PestMaster_Binding(oForm, p_oDICompany, p_oSBOApplication, sErrDesc)
                            oForm.Visible = True

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "PMS"

                        Try
                            LoadFromXML("PestMgtService.srf", SBO_Application)
                            oForm = SBO_Application.Forms.Item("PMSR")
                            oForm.EnableMenu("1282", False)
                            oForm.Items.Item("52").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("38").Specific
                            oMatrix.AutoResizeColumns()
                            ' PestMaster_Binding(oForm, p_oDICompany, p_oSBOApplication, sErrDesc)
                            oForm.Visible = True
                            oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                            oForm.DataBrowser.BrowseBy = "txtDocEntr"
                            If PestManagementService_EnableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "3045646"
                        oForm = p_oSBOApplication.Forms.ActiveForm
                        If oForm.UniqueID = "SBS" Then
                            Dim omatrix As SAPbouiCOM.Matrix = oForm.Items.Item("Item_3").Specific
                            Dim DeleteFlag As Boolean = False
                            For mjs As Integer = omatrix.RowCount To 1 Step -1
                                'If mjs <= omatrix.RowCount Then
                                If omatrix.IsRowSelected(mjs) = True Then
                                    If Not String.IsNullOrEmpty(omatrix.Columns.Item("Col_1").Cells.Item(mjs).Specific.String) And String.IsNullOrEmpty(omatrix.Columns.Item("V_0").Cells.Item(mjs).Specific.String) Then
                                        omatrix.DeleteRow(mjs)
                                        DeleteFlag = True
                                    ElseIf omatrix.Columns.Item("V_0").Cells.Item(mjs).Specific.String = "C" Then
                                        p_oSBOApplication.StatusBar.SetText("Can`t delete the row after saved ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    Else
                                        p_oSBOApplication.StatusBar.SetText("Can`t delete the empty row ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    End If
                                End If
                                'End If
                            Next mjs
                            If DeleteFlag = True Then
                                For mjs As Integer = 1 To omatrix.RowCount
                                    omatrix.Columns.Item("#").Cells.Item(mjs).Specific.string = mjs
                                Next
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                End If
                            End If
                        End If

                    Case "1290", "1288", "1289", "1291"
                        oForm = p_oSBOApplication.Forms.ActiveForm
                        If oForm.UniqueID = "SBS" Then
                            Dim omatrix As SAPbouiCOM.Matrix = oForm.Items.Item("Item_3").Specific
                            Try
                                oForm.Freeze(True)
                                oForm.Items.Item("Item_14").Enabled = False
                                oForm.ActiveItem = "Item_13"
                                oForm.Items.Item("Item_5").Enabled = False

                                For imjs As Integer = 1 To omatrix.RowCount
                                    omatrix.CommonSetting.SetCellEditable(imjs, 1, False)
                                    omatrix.CommonSetting.SetCellEditable(imjs, 3, False)
                                Next imjs
                                If Not String.IsNullOrEmpty(omatrix.Columns.Item("Col_0").Cells.Item(omatrix.RowCount).Specific.String) Then
                                    oForm.DataSources.DBDataSources.Item(1).Clear()
                                    omatrix.AddRow()
                                    omatrix.CommonSetting.SetCellEditable(omatrix.RowCount, 1, True)
                                    omatrix.CommonSetting.SetCellEditable(omatrix.RowCount, 3, True)
                                    omatrix.Columns.Item("#").Cells.Item(omatrix.RowCount).Specific.String = omatrix.RowCount
                                    omatrix.Columns.Item("Col_7").Cells.Item(omatrix.RowCount).Specific.String = 0.0
                                    omatrix.Columns.Item("Col_8").Cells.Item(omatrix.RowCount).Specific.String = 0.0
                                    p_iRowCount = omatrix.RowCount - 1
                                End If
                                oForm.Freeze(False)
                            Catch ex As Exception
                                oForm.Freeze(False)
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                BubbleEvent = False
                                Exit Sub
                            End Try
                        ElseIf oForm.UniqueID = "CSH" Then
                            If CustomerSurvey_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        ElseIf oForm.UniqueID = "MGC" Then
                            If GardenerChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        ElseIf oForm.UniqueID = "MSC" Then
                            If MarketSegmentChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        ElseIf oForm.UniqueID = "MLC" Then
                            If LandscapeChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        ElseIf oForm.UniqueID = "JJS" Then
                            If JobSchedule_NonEditableControls(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        ElseIf oForm.UniqueID = "PMSR" Then
                            If PestManagementService_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        ElseIf oForm.UniqueID = "MST" Then
                            If MarketSegmentTemplate_FormLoad(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        ElseIf oForm.TypeEx = "940" Or oForm.TypeEx = "720" Then
                            If oForm.Type = "720" Then
                                oForm.Items.Item("10000331").Enabled = False
                            Else
                                oForm.Items.Item("100003311").Enabled = False
                            End If
                        End If

                    Case "1281"

                        ''                        oForm = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, 0)
                        oForm = p_oSBOApplication.Forms.ActiveForm
                        Select Case p_oSBOApplication.Forms.ActiveForm.UniqueID
                            Case "CSH"
                                If CustomerSurvey_EnableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "SBS"
                                oForm.Items.Item("Item_13").Enabled = True

                            Case "SAS"
                                oForm.Items.Item("txtDocNum").Enabled = True
                                oForm.Items.Item("txtDocDate").Enabled = True
                                oForm.Items.Item("txtOwnName").Enabled = True

                                oForm.ActiveItem = "txtDocNum"

                            Case "MGC"
                                If GardenerChecklist_EnableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "MLC"
                                If GardenerChecklist_EnableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "MSC"
                                If MarketSegmentChecklist_EnableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Case "MST"
                                oForm.ActiveItem = "txtMarSeg"
                            Case "JJS"
                                If JobSchedule_EditableControls(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Case "PMSR"
                                If PestManagementService_EnableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        End Select

                        Select Case oForm.TypeEx

                            Case "940", "720"

                                If oForm.TypeEx = "720" Then
                                    oForm.Items.Item("10000331").Enabled = False
                                Else
                                    oForm.Items.Item("100003311").Enabled = False
                                End If


                        End Select

                    Case "1282"

                        oForm = p_oSBOApplication.Forms.ActiveForm

                        Select Case oForm.UniqueID

                            Case "SBS"
                                oForm = SBO_Application.Forms.Item("SBS")
                                oForm.Items.Item("Item_14").Enabled = True
                                oForm.Items.Item("Item_5").Enabled = True
                                StockBudget_Binding(oForm, p_oDICompany, p_oSBOApplication, sErrDesc)
                                oForm.Items.Item("Item_13").Enabled = False

                            Case "SAS"
                                oForm = SBO_Application.Forms.Item("SAS")
                                If Form_Load(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "MST"
                                oForm = SBO_Application.Forms.Item("MST")
                                If MarketSegmentTemplate_FormLoad(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "JJS"
                                oForm = SBO_Application.Forms.Item("JJS")
                                If JobSchedule_FormLoad(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End Select

                        Select Case oForm.TypeEx

                            Case "940", "720"

                                If oForm.Type = "720" Then
                                    oForm.Items.Item("10000331").Enabled = True
                                Else
                                    oForm.Items.Item("100003311").Enabled = True
                                End If


                        End Select


                        'Case "1286"
                        '    oForm = p_oSBOApplication.Forms.ActiveForm
                        '    Select Case oForm.UniqueID
                        '        Case "JJS"
                        '            If JobSchedule_CloseStatus(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        '            If JobSchedule_NonEditableControls(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        '    End Select


                    Case "SAQ"
                        Try
                            LoadFromXML("ShowAroundQuestions.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("SAQ")
                            oForm.Visible = True

                            If Loadscreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "SAS"
                        Try
                            LoadFromXML("ShowAround.srf", SBO_Application)
                            oForm = SBO_Application.Forms.Item("SAS")
                            oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE
                            oForm.Visible = True
                            oForm.Freeze(True)
                            If Form_Load(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            oForm.Freeze(False)
                        Catch ex As Exception
                            oForm.Freeze(False)
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "CSQ"
                        Try
                            LoadFromXML("SurveryMaster.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("CSQ")
                            oForm.Visible = True

                            If SurveyQuestions_Loadscreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "CSH"
                        Try
                            LoadFromXML("CustomerSurvey.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("CSH")
                            oForm.Visible = True

                            If CustomerSurvey_LoadScreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub
                        '================================= Job Schedule Started ======================================================

                    Case "JCT"
                        Try
                            LoadFromXML("JobType.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("JCT")
                            oForm.Visible = True

                            If JobType_Loadscreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "JJA"
                        Try

                            '' oForm = LoadScreenXML("JobArea.xml", "JJA", "JJA")

                            LoadFromXML("JobArea.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("JJA")
                            oForm.Visible = True

                            If JobArea_Loadscreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "JJS"
                        Try
                            LoadFromXML("JobSchedule.srf", SBO_Application)
                            oForm = SBO_Application.Forms.Item("JJS")
                            oForm.Visible = True

                            If JobSchedule_FormLoad(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "JDS"
                        Try
                            LoadFromXML("DailySchedule.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("JDS")
                            oForm.Visible = True

                            If DailySchedule_LoadScreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                        '============================================== Inspection QA Starting ===========================================

                    Case "MST"
                        Try
                            LoadFromXML("MarketSegmentTemplate.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("MST")
                            oForm.Visible = True

                            If MarketSegmentTemplate_FormLoad(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "MSC"
                        Try
                            LoadFromXML("MarketSegmentChecklist.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("MSC")
                            oForm.Visible = True
                            If MarketSegmentChecklist_LoadScreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "MGT"
                        Try
                            LoadFromXML("GardenerTemplate.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("MGT")
                            oForm.Visible = True
                            If GardenerTemplate_Loadscreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "MGC"
                        Try
                            LoadFromXML("GardenerChecklist.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("MGC")
                            oForm.Visible = True
                            If GardenerChecklist_LoadScreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "MLT"
                        Try
                            LoadFromXML("LandscapeTemplate.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("MLT")
                            oForm.Visible = True
                            If LandscapeTemplate_Loadscreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub


                    Case "MLC"
                        Try
                            LoadFromXML("LandscapeChecklist.srf", SBO_Application)

                            oForm = SBO_Application.Forms.Item("MLC")
                            oForm.Visible = True
                            If LandscapeChecklist_LoadScreen(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Exit Try

                        Catch ex As Exception
                            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                        End Try
                        Exit Sub

                    Case "1293"
                        oForm = p_oSBOApplication.Forms.ActiveForm

                        Select Case oForm.UniqueID
                            Case "JJS"
                                If JobSchedule_DeleteRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                                'Case "PMSM"
                                '    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                                '    Try
                                '        If Not String.IsNullOrEmpty(oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.RowCount - 1).Specific.String) Then
                                '            oForm.Freeze(True)
                                '            oForm.DataSources.DBDataSources.Item(2).Clear()
                                '            oMatrix.AddRow()
                                '            oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.String = oMatrix.VisualRowCount
                                '            oMatrix.Columns.Item("V_4").Cells.Item(oMatrix.VisualRowCount).Specific.String = oMatrix.VisualRowCount
                                '            oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific.checked = True
                                '            oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.active = True
                                '            oForm.Freeze(False)
                                '        End If
                                '    Catch ex As Exception
                                '        oForm.Freeze(False)
                                '        SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                '        BubbleEvent = False
                                '    End Try
                        End Select


                End Select
            Else
                '===================================== Show Question Master Start ==========================================
                Select Case pVal.MenuUID

                    Case "1293"

                        oForm = p_oSBOApplication.Forms.ActiveForm

                        Select Case oForm.UniqueID

                            Case "SAQ"
                                If Validate_Question(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "CSQ"
                                If SurveyQuestions_Validate_Question(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "JJA"
                                If JobArea_ValidationLocation(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "JCT"
                                If JobType_ValidationLocation(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "MGT"

                                If GardenerTemplate_ValidateQuestion(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "MLT"
                                If LandscapeTemplate_ValidateQuestion(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "MST"
                                If MarketSegmentTemplate_Question(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            Case "PMSM"
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific

                                If p_iPestRow > 0 Then
                                    If String.IsNullOrEmpty(oMatrix.Columns.Item("V_3").Cells.Item(p_iPestRow).Specific.String) Then
                                        p_oSBOApplication.StatusBar.SetText("Can`t delete the empty row", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                    If Not String.IsNullOrEmpty(oMatrix.Columns.Item("V_2").Cells.Item(p_iPestRow).Specific.String) Then
                                        p_oSBOApplication.StatusBar.SetText("Can`t delete the row after saved", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                        End Select


                    Case "1286"
                        oForm = p_oSBOApplication.Forms.ActiveForm
                        Select Case oForm.UniqueID
                            Case "JJS"
                                'If JobSchedule_CloseStatus(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                If JobSchedule_NonEditableControls(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                BubbleEvent = False

                        End Select

                    Case "1287"
                        oForm = p_oSBOApplication.Forms.ActiveForm
                        Select Case oForm.UniqueID
                            Case "JJS"
                                If DuplicateJobSchedule(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                BubbleEvent = False
                        End Select


                        'Browse Menus
                    Case "1290", "1288", "1289", "1291"
                        oForm = p_oSBOApplication.Forms.ActiveForm
                        If oForm.UniqueID = "SBS" And oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                            Dim omatrix As SAPbouiCOM.Matrix = oForm.Items.Item("Item_3").Specific
                            Try
                                oForm.Freeze(True)
                                If String.IsNullOrEmpty(omatrix.Columns.Item("Col_0").Cells.Item(omatrix.RowCount).Specific.String) Then
                                    p_Closeflag = True
                                    omatrix.DeleteRow(omatrix.RowCount)
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                    oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                End If
                                oForm.Freeze(False)
                            Catch ex As Exception
                                oForm.Freeze(False)
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                BubbleEvent = False
                                Exit Sub
                            End Try
                        End If
                End Select
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch exc As Exception
            BubbleEvent = False
            ShowErr(exc.Message)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            WriteToLogFile(Err.Description, sFuncName)
        End Try
    End Sub

    Private Sub SBO_Application_ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, _
            ByRef BubbleEvent As Boolean) Handles SBO_Application.ItemEvent
        ' **********************************************************************************
        '   Function   :    SBO_Application_ItemEvent()
        '   Purpose    :    This function will be handling the SAP Menu Event
        '               
        '   Parameters :    ByVal FormUID As String
        '                       FormUID = set the FormUID
        '                   ByRef pVal As SAPbouiCOM.ItemEvent
        '                       pVal = set the SAP UI ItemEvent Object
        '                   ByRef BubbleEvent As Boolean
        '                       BubbleEvent = set the True/False        
        ' **********************************************************************************

        Dim sErrDesc As String = String.Empty
        Dim sFuncName As String = String.Empty
        Dim p_oDVJE As DataView = Nothing
        Dim oDTDistinct As DataTable = Nothing
        Dim oDTRowFilter As DataTable = Nothing

        Try
            sFuncName = "SBO_Application_ItemEvent()"
            '' If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            If Not IsNothing(p_oDICompany) Then
                If Not p_oDICompany.Connected Then
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConnectDICompSSO()", sFuncName)
                    If ConnectDICompSSO(p_oDICompany, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                End If
            End If


            If pVal.BeforeAction = False Then

                Select Case pVal.FormUID

                    Case "PMSR"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                            If pVal.ItemUID = "32" Or pVal.ItemUID = "33" Or pVal.ItemUID = "34" Or pVal.ItemUID = "35" Or pVal.ItemUID = "Item_0" Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Select Case pVal.ItemUID
                                    Case "32"
                                        oForm.PaneLevel = 1
                                    Case "33"
                                        oForm.PaneLevel = 2
                                    Case "34"
                                        oForm.PaneLevel = 3
                                    Case "35"
                                        oForm.PaneLevel = 4
                                    Case "Item_0"
                                        oForm.PaneLevel = 7
                                End Select
                            End If
                        End If
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                If EPSSaveFileToAttachFolder(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                If PestManagementService_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_GOT_FOCUS And pVal.ItemUID = "txtDocEntr" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If oForm.Items.Item("txtDocEntr").Specific.string <> "" Then
                                If PestManagementService_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "btnBrowse" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            If EPSAttachment_Path(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "btnDisplay" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            If EPSFile_Display(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "btnDelete" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            If EPSDeleteRow_Attachment(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If

                    Case "SBS"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                            If pVal.ItemUID = "20" Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                sFuncName = "'Browse' Button Click "
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling File Open Function", sFuncName)
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("19").Specific
                                Dim orset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                orset.DoQuery("SELECT attachpath from oadp")
                                fillopen()
                                If String.IsNullOrEmpty(orset.Fields.Item("attachpath").Value) Then
                                    p_oSBOApplication.StatusBar.SetText("Select the Attachment folder path in the General Settings", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End If
                                If p_sSelectedFileName <> "" Then
                                    Dim file = New FileInfo(p_sSelectedFilepath)
                                    file.CopyTo(Path.Combine(orset.Fields.Item("attachpath").Value, file.Name), True)
                                    oMatrix = oForm.Items.Item("19").Specific
                                    If oMatrix.RowCount = 0 Then
                                        oMatrix.AddRow()
                                    Else
                                        If oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.RowCount).Specific.String <> "" And oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.RowCount).Specific.String <> "" Then
                                            oMatrix.AddRow()
                                        End If
                                    End If

                                    oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.RowCount).Specific.string = oMatrix.RowCount
                                    oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.RowCount).Specific.string = p_sSelectedFileName
                                    oMatrix.Columns.Item("V_2").Cells.Item(oMatrix.RowCount).Specific.string = orset.Fields.Item("attachpath").Value & p_sSelectedFileName
                                    oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.RowCount).Specific.string = PostDate(p_oDICompany)
                                    'oMatrix.AutoResizeColumns()
                                    p_sSelectedFileName = ""
                                    p_sSelectedFilepath = ""

                                End If

                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                End If

                                oForm.Freeze(True)
                                oMatrix.AutoResizeColumns()
                                oForm.Freeze(False)
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With Success File Open Function", sFuncName)
                                ' oForm.Items.Item("Item_5").Specific.string = p_sSelectedFilepath
                                Exit Sub
                            End If

                            If pVal.ItemUID = "1" And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_OK_MODE) Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Dim oMAtrix As SAPbouiCOM.Matrix = oForm.Items.Item("Item_3").Specific

                                Try
                                    If p_Closeflag = True Then
                                        p_Closeflag = False
                                        Exit Sub
                                    End If

                                    For imjs As Integer = 1 To oMAtrix.RowCount
                                        If Not String.IsNullOrEmpty(oMAtrix.Columns.Item("Col_0").Cells.Item(imjs).Specific.String) Then
                                            oMAtrix.CommonSetting.SetCellEditable(imjs, 1, False)
                                            oMAtrix.CommonSetting.SetCellEditable(imjs, 3, False)
                                        End If
                                    Next imjs

                                    If Not String.IsNullOrEmpty(oMAtrix.Columns.Item("Col_0").Cells.Item(oMAtrix.RowCount).Specific.String) Then
                                        oForm.DataSources.DBDataSources.Item(1).Clear()
                                        oMAtrix.AddRow()
                                        oMAtrix.CommonSetting.SetCellEditable(oMAtrix.RowCount, 1, True)
                                        oMAtrix.CommonSetting.SetCellEditable(oMAtrix.RowCount, 3, True)
                                        oMAtrix.Columns.Item("#").Cells.Item(oMAtrix.RowCount).Specific.String = oMAtrix.RowCount
                                        oMAtrix.Columns.Item("Col_7").Cells.Item(oMAtrix.RowCount).Specific.String = 0.0
                                        oMAtrix.Columns.Item("Col_8").Cells.Item(oMAtrix.RowCount).Specific.String = 0.0
                                        p_iRowCount = oMAtrix.RowCount - 1
                                    End If

                                    oForm.Items.Item("Item_13").Enabled = False
                                    Exit Sub
                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End Try
                            End If

                            If pVal.ItemUID = "1" And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Try
                                    If pVal.Action_Success = True Then
                                        Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("Item_3").Specific
                                        StockBudget_Binding(oForm, p_oDICompany, p_oSBOApplication, sErrDesc)
                                        oMatrix.Columns.Item("Col_7").Cells.Item(1).Specific.String = 0
                                        oMatrix.Columns.Item("Col_8").Cells.Item(1).Specific.String = 0
                                        oMatrix.Columns.Item("Col_9").Cells.Item(1).Specific.String = 0
                                        oMatrix.AutoResizeColumns()
                                    End If
                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End Try
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                            Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
                            oCFLEvento = pVal
                            Dim sCFL_ID As String
                            sCFL_ID = oCFLEvento.ChooseFromListUID
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.Item(FormUID)
                            Dim oCFL As SAPbouiCOM.ChooseFromList
                            oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                            If oCFLEvento.BeforeAction = False Then
                                Dim oDataTable As SAPbouiCOM.DataTable
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("Item_3").Specific
                                oDataTable = oCFLEvento.SelectedObjects
                                If pVal.ItemUID = "Item_3" And pVal.ColUID = "Col_0" Then
                                    Try
                                        If oMatrix.RowCount = pVal.Row Then
                                            oMatrix.AddRow()
                                            oMatrix.CommonSetting.SetCellEditable(oMatrix.RowCount, 1, True)
                                            oMatrix.CommonSetting.SetCellEditable(oMatrix.RowCount, 3, True)
                                            oMatrix.Columns.Item("#").Cells.Item(oMatrix.RowCount).Specific.String = oMatrix.RowCount
                                            oMatrix.Columns.Item("Col_7").Cells.Item(oMatrix.RowCount).Specific.String = 0.0
                                            oMatrix.Columns.Item("Col_8").Cells.Item(oMatrix.RowCount).Specific.String = 0.0
                                        End If
                                        If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                            oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                        End If
                                        oMatrix.Columns.Item("Col_1").Cells.Item(pVal.Row).Specific.String = oDataTable.GetValue("ItemName", 0)
                                        oMatrix.Columns.Item("Col_11").Cells.Item(pVal.Row).Specific.String = oDataTable.GetValue("InvntryUom", 0) 'T0.[InvntryUom]
                                        oMatrix.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.String = oDataTable.GetValue("ItemCode", 0)

                                    Catch ex As Exception
                                    End Try
                                End If

                                If pVal.ItemUID = "Item_3" And pVal.ColUID = "Col_3" Then
                                    Try
                                        If oMatrix.RowCount = pVal.Row Then
                                            If Not String.IsNullOrEmpty(oMatrix.Columns.Item("Col_0").Cells.Item(oMatrix.RowCount).Specific.String) Then
                                                oMatrix.AddRow()
                                                oMatrix.CommonSetting.SetCellEditable(oMatrix.RowCount, 1, True)
                                                oMatrix.CommonSetting.SetCellEditable(oMatrix.RowCount, 3, True)
                                                oMatrix.Columns.Item("#").Cells.Item(oMatrix.RowCount).Specific.String = oMatrix.RowCount
                                                oMatrix.Columns.Item("Col_7").Cells.Item(oMatrix.RowCount).Specific.String = 0.0
                                                oMatrix.Columns.Item("Col_8").Cells.Item(oMatrix.RowCount).Specific.String = 0.0
                                            End If
                                        End If
                                        If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                            oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                        End If
                                        oMatrix.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.String = oDataTable.GetValue("WhsName", 0)
                                        oMatrix.Columns.Item("Col_3").Cells.Item(pVal.Row).Specific.String = oDataTable.GetValue("WhsCode", 0)

                                    Catch ex As Exception
                                    End Try
                                End If
                            End If
                        End If

                    Case "PMSM"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.FormMode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific

                            If pVal.ItemUID = "txtMatrix" And pVal.ColUID = "V_-1" Then
                                If p_PestDragdrop = False Then
                                    oMatrix.GetLineData(pVal.Row)
                                    oMatrix.DeleteRow(pVal.Row)
                                    p_PestDragdrop = True
                                ElseIf p_PestDragdrop = True Then
                                    oMatrix.AddRow(1, pVal.Row)
                                    oMatrix.SetLineData(pVal.Row + 1)
                                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                    End If
                                    p_PestDragdrop = False
                                End If
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "1" And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_OK_MODE) Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMAtrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific

                            Try
                                oForm.Freeze(True)
                                If Not String.IsNullOrEmpty(oMAtrix.Columns.Item("V_3").Cells.Item(oMAtrix.VisualRowCount).Specific.String) Then
                                    oForm.DataSources.DBDataSources.Item(1).Clear()
                                    oMAtrix.AddRow()
                                    oMAtrix.Columns.Item("V_-1").Cells.Item(oMAtrix.VisualRowCount).Specific.String = oMAtrix.VisualRowCount
                                    oMAtrix.Columns.Item("V_4").Cells.Item(oMAtrix.VisualRowCount).Specific.String = oMAtrix.VisualRowCount
                                    oMAtrix.Columns.Item("V_5").Cells.Item(oMAtrix.VisualRowCount).Specific.String = oMAtrix.VisualRowCount
                                    oMAtrix.Columns.Item("V_1").Cells.Item(oMAtrix.VisualRowCount).Specific.checked = True
                                    ' oMAtrix.Columns.Item("V_3").Cells.Item(oMAtrix.VisualRowCount).Specific.active = True
                                Else
                                    oMAtrix.Columns.Item("V_4").Cells.Item(oMAtrix.VisualRowCount).Specific.String = oMAtrix.VisualRowCount
                                    oMAtrix.Columns.Item("V_5").Cells.Item(oMAtrix.VisualRowCount).Specific.String = oMAtrix.VisualRowCount
                                    oMAtrix.Columns.Item("V_1").Cells.Item(oMAtrix.VisualRowCount).Specific.checked = True
                                    'oMAtrix.Columns.Item("V_3").Cells.Item(oMAtrix.VisualRowCount).Specific.active = True
                                End If
                                oForm.Freeze(False)
                            Catch ex As Exception
                                oForm.Freeze(False)
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE) Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            If pVal.ItemUID = "1" Then
                                If pVal.Action_Success = True Then
                                    Try
                                        oForm.Freeze(True)
                                        oForm.Close()
                                        LoadFromXML("PestServiceMaster.srf", SBO_Application)
                                        oForm = SBO_Application.Forms.Item("PMSM")
                                        'oForm.Visible = False
                                        PestMaster_Binding(oForm, p_oDICompany, p_oSBOApplication, sErrDesc)
                                        oForm.Visible = True
                                        oForm.Freeze(False)
                                    Catch ex As Exception
                                        oForm.Freeze(False)
                                        p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        BubbleEvent = False
                                        Exit Try
                                    End Try
                                    Exit Sub
                                End If
                            End If
                        End If


                    Case "PR_IT", "PR_GI"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DEACTIVATE And pVal.Before_Action = False And pVal.InnerEvent = False Then
                            Try
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.Item(pVal.FormUID)
                                oForm.Visible = True
                                BubbleEvent = False
                                Exit Sub
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try

                        End If

                        '==================== Show Around Master Start ================================
                    Case "SAQ"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_VALIDATE And pVal.ItemUID = "txtMatrix" And pVal.ColUID = "V_3" And pVal.InnerEvent = False Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Try
                                If Matrix_AddRow(oForm.Items.Item("txtMatrix").Specific, False, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try


                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "txtMatrix" And pVal.Row <> 0 Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Try
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                                If (oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.string <> "") Then
                                    iCurrentRow = oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.value
                                End If
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.Action_Success = True And pVal.ItemUID = "1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If (oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE) Then
                                Dim oCheckBox As SAPbouiCOM.CheckBox
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                                oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                                oForm.Items.Item("txtCode").Specific.string = "1"
                                oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                If Not String.IsNullOrEmpty(oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.RowCount).Specific.String) Then
                                    oForm.DataSources.DBDataSources.Item(1).Clear()
                                    oMatrix.AddRow()
                                    oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.RowCount).Specific.value = oMatrix.RowCount
                                    oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.RowCount).Specific.value = oMatrix.RowCount
                                    oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific
                                    oCheckBox.Checked = True
                                End If

                            End If
                        End If

                        '=============================================================================================================================================
                    Case "SAS"



                        If pVal.ItemUID = "11" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Try
                                If PaneLevel(oForm, 2, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try

                        ElseIf pVal.ItemUID = "10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Try
                                If PaneLevel(oForm, 1, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try
                        End If


                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS And pVal.ItemUID = "matContent" Then
                        '    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                        '    If Matrix_AddRow(oForm.Items.Item("matContent").Specific, True, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        'End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST And pVal.ItemUID = "txtOwner" Then

                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If ChooseFromList(oForm, pVal, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "btnBrowse" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If (Attachment_Path(oForm, sErrDesc) <> RTN_SUCCESS) Then Throw New ArgumentException(sErrDesc)

                            p_sSelectedFileName = String.Empty
                            p_sSelectedFilepath = String.Empty

                        ElseIf pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "btnDelete" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            ' iCurrentRow = pVal.Row

                            If (DeleteRow_Attachment(oForm, sErrDesc) <> RTN_SUCCESS) Then Throw New ArgumentException(sErrDesc)

                        ElseIf pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "btnDisplay" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If (File_Display(oForm, sErrDesc) <> RTN_SUCCESS) Then Throw New ArgumentException(sErrDesc)

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "matAttach" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matAttach").Specific
                            oMatrix.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Auto
                            oMatrix.SelectRow(pVal.Row, True, False)
                            oForm.Items.Item("btnDelete").Enabled = True
                            oForm.Items.Item("btnDisplay").Enabled = True
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_RESIZE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If oForm.Visible = True Then
                                oForm.Items.Item("20").Width = oForm.Items.Item("matContent").Width + 20
                                oForm.Items.Item("20").Height = oForm.Items.Item("matContent").Height + 20
                            End If
                        End If

                        If pVal.ItemUID = "1" And pVal.ActionSuccess = True And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                If Form_Load(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            ElseIf oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                If oForm.Items.Item("txtDocNum").Specific.string <> "" Then
                                    oForm.ActiveItem = "txtRemarks"
                                    oForm.Items.Item("txtDocNum").Enabled = False
                                    oForm.Items.Item("txtDocDate").Enabled = False
                                    oForm.Items.Item("txtOwnName").Enabled = False
                                End If
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_GOT_FOCUS And pVal.ItemUID = "txtDocNum" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If oForm.Items.Item("txtDocNum").Specific.string <> "" Then
                                oForm.ActiveItem = "txtRemarks"
                                oForm.Items.Item("txtDocNum").Enabled = False
                                oForm.Items.Item("txtDocDate").Enabled = False
                                oForm.Items.Item("txtOwnName").Enabled = False
                            End If
                        End If

                        '==================== Survey Master Start ================================
                    Case "CSQ"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_VALIDATE And pVal.ItemUID = "txtMatrix" And pVal.ColUID = "V_3" And pVal.InnerEvent = False Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If SurveyQuestions_Matrix_AddRow(oForm.Items.Item("txtMatrix").Specific, False, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "txtMatrix" And pVal.Row <> 0 Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If (oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.string <> "") Then
                                iCurrentRow = oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.value
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.Action_Success = True And pVal.ItemUID = "1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If (oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE) Then
                                Dim oCheckBox As SAPbouiCOM.CheckBox
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                                oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                                oForm.Items.Item("txtCode").Specific.string = "1"
                                oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                If Not String.IsNullOrEmpty(oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.RowCount).Specific.String) Then
                                    oForm.DataSources.DBDataSources.Item(1).Clear()
                                    oMatrix.AddRow()
                                    oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.RowCount).Specific.value = oMatrix.RowCount
                                    oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.RowCount).Specific.value = oMatrix.RowCount
                                    oCheckBox = oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific
                                    oCheckBox.Checked = True
                                End If

                            End If
                        End If

                    Case "CSH"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                If CustomerSurvey_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_GOT_FOCUS And pVal.ItemUID = "txtDocNum" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Items.Item("txtDocNum").Specific.string <> "" Then
                                If CustomerSurvey_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If

                        End If

                        '========================== Job Scheduling starting - Job Area ====================================
                    Case "JJA"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_VALIDATE And pVal.InnerEvent = False And pVal.ItemUID = "txtMatrix" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If JobArea_MatrixAddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If


                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.FormMode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                        '    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                        '    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific

                        '    If pVal.ItemUID = "txtMatrix" And pVal.ColUID = "V_-1" Then
                        '        If p_JJADragdrop = False Then
                        '            oMatrix.GetLineData(pVal.Row)
                        '            oMatrix.DeleteRow(pVal.Row)
                        '            p_JJADragdrop = True
                        '        ElseIf p_JJADragdrop = True Then
                        '            oMatrix.AddRow(1, pVal.Row)
                        '            oMatrix.SetLineData(pVal.Row + 1)
                        '            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        '                oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                        '            End If
                        '            p_JJADragdrop = False
                        '        End If
                        '    End If
                        'End If

                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                        '    p_JJADragdrop = False
                        'End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            If JobArea_MatrixAddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If pVal.ItemUID = "1" And pVal.Action_Success = True Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.Item("JJA")
                                oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                                oForm.Items.Item("txtCode").Specific.String = "1"
                                oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                If JobArea_MatrixAddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                        End If



                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "txtMatrix" And pVal.Row <> 0 Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If (oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.string <> "") Then
                                iCurrentRow = oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.value
                            End If

                        End If

                    Case "JCT"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_VALIDATE And pVal.InnerEvent = False And pVal.ItemUID = "txtMatrix" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If JobType_MatrixAddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If

                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.FormMode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                        '    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                        '    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific

                        '    If pVal.ItemUID = "txtMatrix" And pVal.ColUID = "V_-1" Then
                        '        If p_JCTDragdrop = False Then
                        '            oMatrix.GetLineData(pVal.Row)
                        '            oMatrix.DeleteRow(pVal.Row)
                        '            p_JCTDragdrop = True
                        '        ElseIf p_JCTDragdrop = True Then
                        '            oMatrix.AddRow(1, pVal.Row)
                        '            oMatrix.SetLineData(pVal.Row + 1)
                        '            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        '                oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                        '            End If
                        '            p_JCTDragdrop = False
                        '        End If
                        '    End If
                        'End If

                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                        '    p_JCTDragdrop = False
                        'End If
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            If JobType_MatrixAddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If pVal.ItemUID = "1" And pVal.Action_Success = True Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.Item("JCT")
                                oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                                oForm.Items.Item("txtCode").Specific.String = "1"
                                oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                If JobType_MatrixAddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "txtMatrix" And pVal.Row <> 0 Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If (oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.string <> "") Then
                                iCurrentRow = oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.value
                            End If

                        End If

                    Case "JJS"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS And pVal.ItemUID = "matJobSch" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If JobSchedule_MatrixAddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        ElseIf pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS And pVal.ItemUID = "txtStrDate" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If oForm.Items.Item("txtStrDate").Specific.string <> "" Then
                                If JobSchedule_MatrixAddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                        End If


                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST And (pVal.ItemUID = "txtPreBy" Or pVal.ItemUID = "txtApprBy") Then

                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If JobSchedule_ChooseFromList(oForm, pVal, pVal.ItemUID, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        End If

                        If pVal.ItemUID = "1" And pVal.ActionSuccess = True And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                If JobSchedule_FormLoad(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            ElseIf oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                If JobSchedule_NonEditableControls(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If

                            'If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            '    Dim sDocKey As String = oForm.Items.Item("txtDocEntr").Specific.string

                            '    oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                            '    oForm.Items.Item("txtDocEntr").Specific.string = sJobSchKey
                            '    oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)

                            '    If JobSchedule_NonEditableControls(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                            'End If

                        End If


                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_GOT_FOCUS And pVal.ItemUID = "txtDocNum" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Items.Item("txtDocNum").Specific.string <> "" And oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                If JobSchedule_NonEditableControls(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If

                        End If
                    Case "JDS"
                        If (pVal.EventType = SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK And pVal.ItemUID = "grdDisplay") Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If DailySchedule_OpenTaskSchedule(oForm, pVal.Row, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        End If

                    Case "JTS"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "btnDisplay" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If (TaskSchedule_FileDisplay(oForm, sErrDesc) <> RTN_SUCCESS) Then Throw New ArgumentException(sErrDesc)
                        End If
                        '============================   Inspection/QA =============================================================
                    Case "MST"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS And pVal.ItemUID = "matContent" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Try
                                If MarketSegmentTemplate_MatrixAddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub

                            End Try
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If pVal.ItemUID = "1" And pVal.Action_Success = True Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                If MarketSegmentTemplate_FormLoad(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                        End If

                        ''If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        ''    If pVal.ItemUID = "1" Then
                        ''        Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                        ''        If MarketSegmentTemplate_FormLoad(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        ''    End If
                        ''End If



                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.FormMode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                        '    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                        '    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific

                        '    If pVal.ItemUID = "matContent" And pVal.ColUID = "V_-1" Then
                        '        If p_MSTDragdrop = False Then
                        '            oMatrix.GetLineData(pVal.Row)
                        '            oMatrix.DeleteRow(pVal.Row)
                        '            p_MSTDragdrop = True
                        '        ElseIf p_MSTDragdrop = True Then
                        '            oMatrix.AddRow(1, pVal.Row)
                        '            oMatrix.SetLineData(pVal.Row + 1)
                        '            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        '                oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                        '            End If
                        '            p_MSTDragdrop = False
                        '        End If
                        '    End If
                        'End If

                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                        '    p_MSTDragdrop = False
                        'End If


                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "matContent" And pVal.Row <> 0 Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Try
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific
                                If (oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.string <> "") Then
                                    iCurrentRow = oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.value
                                End If
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try
                        End If

                    Case "MGT"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS And pVal.ItemUID = "matContent" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If GardenerTemplate_Matrix_AddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If


                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "1" And pVal.FormMode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                        '    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                        '    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific

                        '    If pVal.ItemUID = "matContent" And pVal.ColUID = "V_-1" Then
                        '        If p_MGTDragdrop = False Then
                        '            oMatrix.GetLineData(pVal.Row)
                        '            oMatrix.DeleteRow(pVal.Row)
                        '            p_MGTDragdrop = True
                        '        ElseIf p_MGTDragdrop = True Then
                        '            oMatrix.AddRow(1, pVal.Row)
                        '            oMatrix.SetLineData(pVal.Row + 1)
                        '            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        '                oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                        '            End If
                        '            p_MGTDragdrop = False
                        '        End If
                        '    End If
                        'End If

                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                        '    p_MGTDragdrop = False
                        'End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "matContent" And pVal.Row <> 0 Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Try
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific
                                If (oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.string <> "") Then
                                    iCurrentRow = oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.value
                                End If
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try
                        End If

                    Case "MGC"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And (pVal.ItemUID = "10" Or pVal.ItemUID = "11") Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If pVal.ItemUID = "10" Then
                                oForm.PaneLevel = 1
                            ElseIf pVal.ItemUID = "11" Then
                                oForm.PaneLevel = 2
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                If GardenerChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_GOT_FOCUS And pVal.ItemUID = "txtProject" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Items.Item("txtProject").Specific.string <> "" Then
                                If GardenerChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If

                        End If

                    Case "MLT"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS And pVal.ItemUID = "matContent" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If LandscapeTemplate_Matrix_AddRow(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If

                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.FormMode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                        '    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                        '    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific

                        '    If pVal.ItemUID = "matContent" And pVal.ColUID = "V_-1" Then
                        '        If p_MLTDragdrop = False Then
                        '            oMatrix.GetLineData(pVal.Row)
                        '            oMatrix.DeleteRow(pVal.Row)
                        '            p_MLTDragdrop = True
                        '        ElseIf p_MLTDragdrop = True Then
                        '            oMatrix.AddRow(1, pVal.Row)
                        '            oMatrix.SetLineData(pVal.Row + 1)
                        '            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        '                oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                        '            End If
                        '            p_MLTDragdrop = False
                        '        End If
                        '    End If
                        'End If

                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                        '    p_MLTDragdrop = False
                        'End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "matContent" And pVal.Row <> 0 Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            Try
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific
                                If (oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.string <> "") Then
                                    iCurrentRow = oMatrix.Columns.Item("V_0").Cells.Item(pVal.Row).Specific.value
                                End If
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try
                        End If
                    Case "MLC"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And (pVal.ItemUID = "10" Or pVal.ItemUID = "11" Or pVal.ItemUID = "btnDisplay") Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If pVal.ItemUID = "10" Then
                                oForm.PaneLevel = 1
                            ElseIf pVal.ItemUID = "11" Then
                                oForm.PaneLevel = 2

                            ElseIf pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "btnDisplay" Then
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matAttach").Specific
                                Dim sFilePath As String = String.Empty

                                Try
                                    For imjs As Integer = 1 To oMatrix.RowCount
                                        If oMatrix.IsRowSelected(imjs) = True Then
                                            sFilePath = oMatrix.Columns.Item("V_2").Cells.Item(imjs).Specific.String
                                            Exit For
                                        End If
                                    Next
                                    If Not String.IsNullOrEmpty(sFilePath) Then
                                        If System.IO.File.Exists(sFilePath) = True Then
                                            Process.Start(sFilePath)
                                        Else
                                            sErrDesc = "File Does Not Exist"
                                            p_oSBOApplication.StatusBar.SetText(sErrDesc, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub

                                        End If
                                    End If

                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End Try
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                If LandscapeChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_GOT_FOCUS And pVal.ItemUID = "txtProject" And pVal.FormMode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            'If oForm.Items.Item("txtProject").Specific.string <> "" Then
                            If LandscapeChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            'End If


                        End If

                    Case "MSC"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "btnDisplay" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matAttach").Specific
                            Dim sFilePath As String = String.Empty

                            Try
                                For imjs As Integer = 1 To oMatrix.RowCount
                                    If oMatrix.IsRowSelected(imjs) = True Then
                                        sFilePath = oMatrix.Columns.Item("V_2").Cells.Item(imjs).Specific.String
                                        Exit For
                                    End If
                                Next
                                If Not String.IsNullOrEmpty(sFilePath) Then
                                    If System.IO.File.Exists(sFilePath) = True Then
                                        Process.Start(sFilePath)
                                    Else
                                        sErrDesc = "File Does Not Exist"
                                        p_oSBOApplication.StatusBar.SetText(sErrDesc, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        BubbleEvent = False
                                        Exit Sub

                                    End If
                                End If

                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try

                        End If
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And (pVal.ItemUID = "10" Or pVal.ItemUID = "11") Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If pVal.ItemUID = "10" Then
                                oForm.PaneLevel = 1
                            ElseIf pVal.ItemUID = "11" Then
                                oForm.PaneLevel = 2
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_RESIZE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            If oForm.Visible = True Then
                                oForm.Items.Item("20").Width = oForm.Items.Item("matContent").Width + 15
                                oForm.Items.Item("20").Height = oForm.Items.Item("matContent").Height + 15
                            End If
                        End If


                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                If MarketSegmentChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_GOT_FOCUS And pVal.ItemUID = "txtDocNum" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Items.Item("txtDocNum").Specific.string <> "" Then
                                If MarketSegmentChecklist_DisableControl(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            End If

                        End If


                End Select

                Select Case pVal.FormTypeEx

                    ''Case "10003"

                    ''    If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD And pVal.Action_Success = True Then
                    ''        Dim oForm1 As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(139, pVal.FormTypeCount)

                    ''        If bCFL = True Then
                    ''            Dim omatrix As SAPbouiCOM.Matrix = oForm1.Items.Item("38").Specific
                    ''            Dim orset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    ''            For imjs As Integer = 1 To omatrix.RowCount - 1
                    ''                '' If Not String.IsNullOrEmpty(omatrix.Columns.Item("U_AE_Image").Cells.Item(imjs).Specific.String) Then
                    ''                orset.DoQuery("SELECT T0.[PicturName] FROM OITM T0 WHERE ItemCode = '" & omatrix.Columns.Item("1").Cells.Item(imjs).Specific.String & "'")
                    ''                Dim oPic1 As SAPbouiCOM.PictureBox = omatrix.Columns.Item("U_AE_Image").Cells.Item(imjs).Specific
                    ''                oPic1.Picture = orset.Fields.Item("PicturName").Value  ''"download.jpg"
                    ''                omatrix.Columns.Item("U_AE_Image").Width = 300
                    ''                '' End If
                    ''            Next
                    ''            bCFL = False
                    ''        End If

                    ''    End If

                    Case "810"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_COMBO_SELECT And pVal.ItemUID = "540003" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(810, pVal.FormTypeCount)
                            If Not String.IsNullOrEmpty(oForm.Items.Item("540003").Specific.value) Then
                                oForm.Items.Item("540004").Specific.String = oForm.Items.Item("540003").Specific.selected.description
                            End If
                            Exit Try
                        End If

                    Case "720"

                        If pVal.ItemChanged = False And (pVal.ItemUID = "11" Or pVal.ItemUID = "33" Or pVal.ItemUID = "38" Or pVal.ItemUID = "21") Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(720, pVal.FormTypeCount)
                            Try
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                    oForm.Items.Item("10000331").Enabled = False
                                End If
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                            p_formCount_GI = pVal.FormTypeCount
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(720, pVal.FormTypeCount)

                            If CreateButton_GoodsIssue(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_COMBO_SELECT Then

                            If pVal.ItemUID = "10000331" Then
                                Dim oForm As SAPbouiCOM.Form
                                iFormType_GI = pVal.FormTypeCount
                                LoadFromXML("PRList_GI.srf", SBO_Application)
                                oForm = SBO_Application.Forms.Item("PR_GI")
                                oForm.Visible = True
                                Display_PurchaseRequest(oForm, "", "GI", sErrDesc)

                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If pVal.Action_Success = True Then

                                If pVal.ItemUID = "1" Then
                                    Try
                                        If oDTGI.Rows.Count > 0 Then
                                            If PRUpdate_GI(oDTGI, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                                            If BRUpdate_GI(oDTGI, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                        End If


                                    Catch ex As Exception
                                        p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        sErrDesc = ex.Message
                                        Call WriteToLogFile(ex.Message, sFuncName)
                                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                    End Try

                                End If

                            End If

                        End If

                    Case "940"

                        If pVal.ItemChanged = False And (pVal.ItemUID = "31" Or pVal.ItemUID = "41" Or pVal.ItemUID = "22" Or pVal.ItemUID = "16") Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(940, pVal.FormTypeCount)
                            Try
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                    oForm.Items.Item("100003311").Enabled = False
                                    Exit Sub
                                End If
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_COMBO_SELECT Then

                            If pVal.ItemUID = "100003311" Then
                                Dim oForm As SAPbouiCOM.Form
                                iFormType_IT = pVal.FormTypeCount
                                LoadFromXML("PRList_IT.srf", SBO_Application)
                                oForm = SBO_Application.Forms.Item("PR_IT")
                                oForm.Visible = True
                                Display_PurchaseRequest(oForm, "", "IT", sErrDesc)
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                            p_formCount_IN = pVal.FormTypeCount
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If pVal.Action_Success = True Then
                                Try
                                    If pVal.ItemUID = "1" Then
                                        If oDTIT.Rows.Count > 0 Then
                                            p_oSBOApplication.StatusBar.SetText("Updating the Transfer Quantity in Purchase Request ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                            If PRUpdate_IT(oDTIT, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                        End If

                                    End If

                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    sErrDesc = ex.Message
                                    Call WriteToLogFile(ex.Message, sFuncName)
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                End Try
                            End If
                        End If



                End Select
            Else
                Select Case pVal.FormTypeEx

                    Case "139"

                        ''Try


                        ''If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST And pVal.ItemUID = "38" And pVal.ColUID = "1" Then

                        ''    bCFL = True
                        ''End If

                        ''If pVal.EventType = SAPbouiCOM.BoEventTypes.et_VALIDATE And pVal.InnerEvent = False Then
                        ''    If pVal.ItemUID = "38" And pVal.ColUID = "1" Then
                        ''        Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(139, pVal.FormTypeCount)
                        ''        Try
                        ''            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("38").Specific
                        ''            Dim orset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        ''            orset.DoQuery("SELECT T0.[PicturName] FROM OITM T0 WHERE ItemCode = '" & oMatrix.Columns.Item("1").Cells.Item(pVal.Row).Specific.String & "'")
                        ''            Dim oPic1 As SAPbouiCOM.PictureBox = oMatrix.Columns.Item("U_AE_Image").Cells.Item(pVal.Row).Specific
                        ''            oPic1.Picture = orset.Fields.Item("PicturName").Value  ''"download.jpg"
                        ''            oMatrix.Columns.Item("U_AE_Image").Width = 300


                        ''            'Dim oPic1 As SAPbouiCOM.PictureBox = oMatrix.Columns.Item("U_AE_Image").Cells.Item(1).Specific
                        ''            'oPic1.Picture = "CustFdbckSignature37947866.Png" ''"download.jpg"

                        ''        Catch ex As Exception
                        ''            p_oSBOApplication.MessageBox(ex.Message)
                        ''        End Try

                        ''    End If
                        ''End If

                        ''If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then

                        ''    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(139, pVal.FormTypeCount)
                        ''    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("38").Specific
                        ''    Dim oColumns As SAPbouiCOM.Columns
                        ''    Dim oColumn As SAPbouiCOM.Column
                        ''    '' Dim oUserDataSource As SAPbouiCOM.UserDataSource

                        ''    oMatrix.Clear()
                        ''    oMatrix = oForm.Items.Item("38").Specific
                        ''    oColumns = oMatrix.Columns
                        ''    'oCol = oCols.Item("U_MyCol1")
                        ''    oColumn = oColumns.Add("U_MyCol1", SAPbouiCOM.BoFormItemTypes.it_PICTURE)
                        ''    oColumn.TitleObject.Caption = "Show Picture"
                        ''    oColumn.Width = 40
                        ''    oColumn.Editable = True
                        ''    oColumn = oColumns.Item("U_MyCol1")
                        ''    oColumn.DataBind.SetBound(True, "RDR1", "U_AE_Image")
                        ''    oMatrix.AddRow()
                        ''    '' Dim oPic1 As SAPbouiCOM.PictureBox = oMatrix.Columns.Item("U_MyCol1").Cells.Item(1).Specific
                        ''    ''  oPic1.Picture = "C:\Program Files (x86)\SAP\SAP Business One\Bitmaps\CustFdbckSignature37947866.Png"
                        ''    ''  oPic1.Picture = "C:\download.jpg"

                        ''    ''oColumn = oColumns.Add("U_MyCol2", SAPbouiCOM.BoFormItemTypes.it_PICTURE)
                        ''    ''oColumn.TitleObject.Caption = "Show Picture"
                        ''    ''oColumn.Width = 40
                        ''    ''oColumn.Editable = True
                        ''    ''oForm.DataSources.UserDataSources.Add("udsPic", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254)
                        ''    ''oColumn = oMatrix.Columns.Item("U_MyCol2")
                        ''    '' ''  oColumn.DataBind.SetBound(True, "", "udsPic")
                        ''    ''oMatrix.AddRow()
                        ''    ''Dim oPic1 As SAPbouiCOM.PictureBox = oMatrix.Columns.Item("U_MyCol2").Cells.Item(1).Specific
                        ''    '' ''  oPic1.Picture = "C:\Program Files (x86)\SAP\SAP Business One\Bitmaps\CustFdbckSignature37947866.Png"
                        ''    ''oPic1.Picture = "C:\download.jpg"

                        ''    '' oMatrix.GetLineData(1) '*Loads UserDataSources from line X*'
                        ''    ''  oMatrix.d()
                        ''    ''oMatrix.DataSources.UserDataSources("udsPic").ValueEx = "C:\Program Files (x86)\SAP\SAP Business One\Bitmaps\CustFdbckSignature37947866.Png"
                        ''    ''oMatrix.SetLineData(1)
                        ''    '' oColumn = oColumns.Item("U_MyCol2")

                        ''    ''oUserDataSource = oForm.DataSources.UserDataSources.Item("Folder")
                        ''    ''oColumn.DataBind.SetBound(True, "", "Folder")

                        ''    ''oMatrix.AddRow()
                        ''    ''oMatrix.GetLineData(1)
                        ''    ''oUserDataSource.ValueEx = "C:\Program Files (x86)\SAP\SAP Business One\Bitmaps\CustFdbckSignature37947866.Png"
                        ''    ''oMatrix.SetLineData(1)

                        ''    ''Dim oPic1 As SAPbouiCOM.PictureColumn = oMatrix.Columns.Item("U_MyCol2")
                        ''    ''oPic1.Picture = "C:\Program Files (x86)\SAP\SAP Business One\Bitmaps\CustFdbckSignature37947866.Png"

                        ''    '' oColumn.DataBind.SetBound(True, "RDR1", "U_sPath")



                        ''    ''Dim oItem As SAPbouiCOM.Item
                        ''    ''oItem = oForm.Items.Add("540002", SAPbouiCOM.BoFormItemTypes.it_STATIC)
                        ''    ''oItem.Top = oForm.Items.Item("16").Top
                        ''    ''oItem.Left = oForm.Items.Item("16").Width + 10
                        ''    ''Dim oStaticText As SAPbouiCOM.StaticText = DirectCast(oItem.Specific, SAPbouiCOM.StaticText)
                        ''    ''oStaticText.Caption = "Picture"

                        ''    ''oItem = oForm.Items.Add("540003", SAPbouiCOM.BoFormItemTypes.it_PICTURE)
                        ''    ''oItem.Top = oForm.Items.Item("540002").Top
                        ''    ''oItem.Left = oForm.Items.Item("540002").Left + oForm.Items.Item("540002").Width + 10
                        ''    ''oItem.Width = 200
                        ''    ''Dim opic As SAPbouiCOM.PictureBox = oForm.Items.Item("540003").Specific
                        ''    '' '' oCombo.DataBind.SetBound(True, "OPRC", "U_AB_MktSegment")
                        ''    '' '' opic.DataBind.SetBound(True, "RDR1", "U_AE_Image")
                        ''    ''opic.Picture = "C:\download.jpg" ''"C:\Program Files (x86)\SAP\SAP Business One\Bitmaps\CustFdbckSignature37947866.Png"
                        ''    '' '' oForm.DataSources.UserDataSources.Add("udsPic", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254)
                        ''    '' ''opic
                        ''End If


                        '' ''Catch ex As Exception

                        '' ''End Try


                    Case "810"
                        Try

                            If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then

                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(810, pVal.FormTypeCount)
                                Dim oRset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                Dim oCombo As SAPbouiCOM.ComboBox = Nothing
                                Dim oEdit As SAPbouiCOM.EditText = Nothing

                                Dim oItem As SAPbouiCOM.Item
                                oItem = oForm.Items.Add("540002", SAPbouiCOM.BoFormItemTypes.it_STATIC)
                                oItem.Top = oForm.Items.Item("540002011").Top
                                oItem.Left = oForm.Items.Item("540002011").Width + 10
                                Dim oStaticText As SAPbouiCOM.StaticText = DirectCast(oItem.Specific, SAPbouiCOM.StaticText)
                                oStaticText.Caption = "Market Segment"

                                oItem = oForm.Items.Add("540003", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
                                oItem.Top = oForm.Items.Item("540002").Top
                                oItem.Left = oForm.Items.Item("540002006").Left
                                oItem.Width = oForm.Items.Item("540002006").Width
                                oCombo = oForm.Items.Item("540003").Specific
                                oCombo.DataBind.SetBound(True, "OPRC", "U_AB_MktSegName") '"U_AB_MktSegName")
                                oItem = oForm.Items.Item("540002")
                                oItem.LinkTo = "540003"

                                oItem = oForm.Items.Add("540004", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                                oItem.Top = oForm.Items.Item("540003").Top
                                oItem.Left = oForm.Items.Item("540003").Left + oForm.Items.Item("540003").Width + 10
                                oItem.Width = oForm.Items.Item("540003").Width
                                oEdit = oForm.Items.Item("540004").Specific
                                oEdit.DataBind.SetBound(True, "OPRC", "U_AB_MktSegment")
                                oForm.Items.Item("540004").Visible = False


                                oRset.DoQuery("SELECT T0.[U_MktSegment], T0.[U_DocEntry] FROM [dbo].[@AB_MKTSGTTEMPLATE]  T0")
                                For imjs As Integer = 1 To oRset.RecordCount
                                    oCombo.ValidValues.Add(oRset.Fields.Item("U_MktSegment").Value, oRset.Fields.Item("U_DocEntry").Value)
                                    oRset.MoveNext()
                                Next
                                Exit Try
                            End If

                        Catch ex As Exception
                            p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            BubbleEvent = False
                            Exit Sub
                        End Try
                        Exit Sub

                    Case "10066"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Try
                                If oForm.Title = "List of Inventory Transfers" Then
                                    Dim oForm_IN As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(940, p_formCount_IN)
                                    oForm_IN.Items.Item("100003311").Enabled = False
                                End If
                            Catch ex As Exception
                            End Try
                        End If

                    Case "10059"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Try
                                If oForm.Title = "List of Goods Issue" Then
                                    Dim oForm_IN As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(720, p_formCount_GI)
                                    oForm_IN.Items.Item("10000331").Enabled = False

                                End If
                            Catch ex As Exception

                            End Try
                        End If

                    Case "9999"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Try
                                If oForm.Title = "List of StockBudgetSetup" Then
                                    Dim oForm_SB As SAPbouiCOM.Form = p_oSBOApplication.Forms.Item("SBS")
                                    Dim omatrix As SAPbouiCOM.Matrix = oForm_SB.Items.Item("Item_3").Specific

                                    For imjs As Integer = 1 To omatrix.RowCount
                                        omatrix.CommonSetting.SetCellEditable(imjs, 1, False)
                                        omatrix.CommonSetting.SetCellEditable(imjs, 3, False)
                                    Next imjs
                                    If Not String.IsNullOrEmpty(omatrix.Columns.Item("Col_0").Cells.Item(omatrix.RowCount).Specific.String) Then
                                        oForm_SB.DataSources.DBDataSources.Item(1).Clear()
                                        omatrix.AddRow()
                                        omatrix.CommonSetting.SetCellEditable(omatrix.RowCount, 1, True)
                                        omatrix.CommonSetting.SetCellEditable(omatrix.RowCount, 3, True)
                                        omatrix.Columns.Item("#").Cells.Item(omatrix.RowCount).Specific.String = omatrix.RowCount
                                        omatrix.Columns.Item("Col_7").Cells.Item(omatrix.RowCount).Specific.String = 0.0
                                        omatrix.Columns.Item("Col_8").Cells.Item(omatrix.RowCount).Specific.String = 0.0
                                        p_iRowCount = omatrix.RowCount - 1
                                    End If
                                    'oForm_SB.Items.Item("Item_5").Specific.active = True
                                    oForm_SB.Items.Item("Item_14").Enabled = False
                                    oForm_SB.Items.Item("Item_5").Enabled = False
                                    oForm_SB.Items.Item("Item_13").Enabled = False

                                End If
                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                Exit Sub
                            End Try


                        End If

                    Case "2000057", "2000035", "2000010", "2000059" ' Projects  --- For AWS "2000059"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK Then
                            ' ''If pVal.ItemUID = "4" And (pVal.ColUID = "COL1" Or pVal.ColUID = "COL2") Then
                            ' ''    ''Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            ' ''    ''Try
                            ' ''    ''    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("4").Specific
                            ' ''    ''    sProjectName = oMatrix.Columns.Item("COL2").Cells.Item(pVal.Row).Specific.String
                            ' ''    ''Catch ex As Exception
                            ' ''    ''    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            ' ''    ''    BubbleEvent = False
                            ' ''    ''    Exit Sub
                            ' ''    ''End Try
                            ' ''    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            ' ''    Try
                            ' ''        Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("4").Specific
                            ' ''        Dim sSQL As String = String.Empty
                            ' ''        Dim sProfitCode As String = String.Empty
                            ' ''        Dim sProfitName As String = String.Empty
                            ' ''        Dim sProjectCode As String = String.Empty
                            ' ''        sSQL = "SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WHERE T0.[PrcCode] = (Select GrpCode  from OPRC where PrcCode = '" & oMatrix.Columns.Item("COL1").Cells.Item(pVal.Row).Specific.String & "')"
                            ' ''        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("SQL " & sSQL, sFuncName)
                            ' ''        Dim oRset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            ' ''        oRset.DoQuery(sSQL)
                            ' ''        sProfitCode = oRset.Fields.Item("PrcCode").Value
                            ' ''        sProfitName = oRset.Fields.Item("PrcName").Value
                            ' ''        sProjectName = oMatrix.Columns.Item("COL2").Cells.Item(pVal.Row).Specific.String
                            ' ''        sProjectCode = oMatrix.Columns.Item("COL1").Cells.Item(pVal.Row).Specific.String
                            ' ''        oForm.Close()
                            ' ''        oForm = p_oSBOApplication.Forms.Item("SBS")
                            ' ''        oForm.Items.Item("Item_15").Specific.String = sProjectName
                            ' ''        oForm.Items.Item("Item_18").Specific.String = sProfitName
                            ' ''        oForm.Items.Item("Item_17").Specific.String = sProfitCode
                            ' ''        oForm.Items.Item("Item_14").Specific.String = sProjectCode

                            ' ''    Catch ex As Exception
                            ' ''        p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            ' ''        BubbleEvent = False
                            ' ''        Exit Sub
                            ' ''    End Try
                            ' ''End If

                            If pVal.ItemUID = "1" Then
                                ''Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.Item("SBS")
                                ''Try
                                ''    oForm.Items.Item("Item_15").Specific.String = sProjectName
                                ''Catch ex As Exception
                                ''    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                ''    BubbleEvent = False
                                ''    Exit Sub
                                ''End Try
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                If oForm.Title <> "List of Projects" Then
                                    Exit Sub
                                End If

                                Try
                                    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("4").Specific
                                    Dim sSQL As String = String.Empty
                                    Dim sProfitCode As String = String.Empty
                                    Dim sProfitName As String = String.Empty
                                    Dim sProjectCode As String = String.Empty
                                    sSQL = "SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WHERE T0.[PrcCode] = (Select GrpCode  from OPRC where PrcCode = '" & oMatrix.Columns.Item("COL1").Cells.Item(pVal.Row).Specific.String & "')"
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("SQL " & sSQL, sFuncName)
                                    Dim oRset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    oRset.DoQuery(sSQL)
                                    sProfitCode = oRset.Fields.Item("PrcCode").Value
                                    sProfitName = oRset.Fields.Item("PrcName").Value
                                    sProjectName = oMatrix.Columns.Item("COL2").Cells.Item(pVal.Row).Specific.String
                                    sProjectCode = oMatrix.Columns.Item("COL1").Cells.Item(pVal.Row).Specific.String
                                    oForm = p_oSBOApplication.Forms.Item("SBS")
                                    oForm.Items.Item("Item_15").Specific.String = sProjectName
                                    oForm.Items.Item("Item_18").Specific.String = sProfitName
                                    oForm.Items.Item("Item_17").Specific.String = sProfitCode
                                    oForm.Items.Item("Item_14").Specific.String = sProjectCode
                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End Try
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK Then
                            If pVal.ItemUID = "4" And (pVal.ColUID = "COL1" Or pVal.ColUID = "COL2") Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Try
                                    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("4").Specific
                                    Dim sSQL As String = String.Empty
                                    Dim sProfitCode As String = String.Empty
                                    Dim sProfitName As String = String.Empty
                                    Dim sProjectCode As String = String.Empty
                                    sSQL = "SELECT T0.[PrcCode], T0.[PrcName] FROM OPRC T0 WHERE T0.[PrcCode] = (Select GrpCode  from OPRC where PrcCode = '" & oMatrix.Columns.Item("COL1").Cells.Item(pVal.Row).Specific.String & "')"
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("SQL " & sSQL, sFuncName)
                                    Dim oRset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    oRset.DoQuery(sSQL)
                                    sProfitCode = oRset.Fields.Item("PrcCode").Value
                                    sProfitName = oRset.Fields.Item("PrcName").Value
                                    sProjectName = oMatrix.Columns.Item("COL2").Cells.Item(pVal.Row).Specific.String
                                    sProjectCode = oMatrix.Columns.Item("COL1").Cells.Item(pVal.Row).Specific.String
                                    oForm = p_oSBOApplication.Forms.Item("SBS")
                                    oForm.Items.Item("Item_15").Specific.String = sProjectName
                                    oForm.Items.Item("Item_18").Specific.String = sProfitName
                                    oForm.Items.Item("Item_17").Specific.String = sProfitCode
                                    oForm.Items.Item("Item_14").Specific.String = sProjectCode
                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End Try
                            End If
                        End If

                    Case "720"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_VALIDATE And pVal.InnerEvent = False And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then

                            If pVal.ItemUID = "13" And pVal.ColUID = "9" Then

                                Try
                                    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(720, pVal.FormTypeCount)
                                    Dim oMAtrix As SAPbouiCOM.Matrix = oForm.Items.Item("13").Specific

                                    If Not String.IsNullOrEmpty(oMAtrix.Columns.Item("AB_BEntity").Cells.Item(pVal.Row).Specific.String) Or Not String.IsNullOrEmpty(oMAtrix.Columns.Item("AB_BLineNo").Cells.Item(pVal.Row).Specific.String) Then
                                        If Not String.IsNullOrEmpty(oMAtrix.Columns.Item("9").Cells.Item(pVal.Row).Specific.String) Then
                                            If CDbl(oMAtrix.Columns.Item("9").Cells.Item(pVal.Row).Specific.String) > CDbl(oMAtrix.Columns.Item("AB_VQty").Cells.Item(pVal.Row).Specific.String) Then
                                                oMAtrix.Columns.Item("9").Cells.Item(pVal.Row).Specific.active = True
                                                p_oSBOApplication.StatusBar.SetText("Quantity should not be greater than " & oMAtrix.Columns.Item("AB_VQty").Cells.Item(pVal.Row).Specific.String & " !", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                                Exit Sub
                                            End If

                                        Else
                                            oMAtrix.Columns.Item("9").Cells.Item(pVal.Row).Specific.active = True
                                            p_oSBOApplication.StatusBar.SetText("Quantity should not be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub
                                        End If
                                    End If
                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End Try
                            End If
                        End If



                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                            If pVal.ItemUID = "1" Then
                                Try

                                    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(720, pVal.FormTypeCount)
                                    Dim oMAtrix As SAPbouiCOM.Matrix = oForm.Items.Item("13").Specific
                                    oDTGI = New DataTable
                                    Dim PostingDate As Date = DateTime.ParseExact(GetDate(oForm.Items.Item("9").Specific.String, p_oDICompany), "yyyyMMdd", Nothing)

                                    oDTGI.Columns.Add("PRBaseEntry", GetType(String))
                                    oDTGI.Columns.Add("PRLineNo", GetType(Integer))
                                    oDTGI.Columns.Add("IssueQty", GetType(Double))
                                    oDTGI.Columns.Add("ItemCode", GetType(String))
                                    oDTGI.Columns.Add("Project", GetType(String))
                                    oDTGI.Columns.Add("Month", GetType(String))
                                    oDTGI.Columns.Add("Year", GetType(String))
                                    oDTGI.Columns.Add("BKTDocEntry", GetType(String))
                                    oDTGI.Columns.Add("BKTLineId", GetType(String))


                                    For imjs As Integer = 1 To oMAtrix.VisualRowCount - 1

                                        If String.IsNullOrEmpty(oMAtrix.Columns.Item("1").Cells.Item(imjs).Specific.String) Then
                                            oMAtrix.Columns.Item("1").Cells.Item(imjs).Specific.active = True
                                            p_oSBOApplication.StatusBar.SetText("Item No. cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub
                                        End If

                                        If String.IsNullOrEmpty(oMAtrix.Columns.Item("15").Cells.Item(imjs).Specific.String) Then
                                            oMAtrix.Columns.Item("15").Cells.Item(imjs).Specific.active = True
                                            p_oSBOApplication.StatusBar.SetText("Warehouse cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub
                                        End If

                                        If Not String.IsNullOrEmpty(oMAtrix.Columns.Item("AB_BEntity").Cells.Item(imjs).Specific.String) Or Not String.IsNullOrEmpty(oMAtrix.Columns.Item("AB_BEntity").Cells.Item(imjs).Specific.String) Then

                                            If CDbl(oMAtrix.Columns.Item("9").Cells.Item(imjs).Specific.String) > CDbl(oMAtrix.Columns.Item("AB_VQty").Cells.Item(imjs).Specific.String) Then
                                                oMAtrix.Columns.Item("9").Cells.Item(imjs).Specific.active = True
                                                p_oSBOApplication.StatusBar.SetText("Quantity cannot be greater than " & oMAtrix.Columns.Item("AB_VQty").Cells.Item(imjs).Specific.String & "! ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                                Exit Sub
                                            End If


                                            oDTGI.Rows.Add(oMAtrix.Columns.Item("AB_BEntity").Cells.Item(imjs).Specific.String, _
                                                           CInt(oMAtrix.Columns.Item("AB_BLineNo").Cells.Item(imjs).Specific.String), _
                                                           CDbl(oMAtrix.Columns.Item("9").Cells.Item(imjs).Specific.String), _
                                                           oMAtrix.Columns.Item("1").Cells.Item(imjs).Specific.String, _
                                                           oMAtrix.Columns.Item("10001004").Cells.Item(imjs).Specific.String, _
                                                           CStr(PostingDate.Month), CStr(PostingDate.Year), oMAtrix.Columns.Item("AB_BSEntry").Cells.Item(imjs).Specific.String, _
                                                           oMAtrix.Columns.Item("AB_BSLId").Cells.Item(imjs).Specific.String)
                                        End If



                                        p_oSBOApplication.StatusBar.SetText("Validating the Information ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    Next imjs

                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End Try
                            End If

                        End If
                    Case "940"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(940, pVal.FormTypeCount)
                            If CreateButton_InventoryTransfer(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_VALIDATE And pVal.InnerEvent = False And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then

                            If pVal.ItemUID = "23" And pVal.ColUID = "10" Then

                                Try
                                    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(940, pVal.FormTypeCount)
                                    Dim oMAtrix As SAPbouiCOM.Matrix = oForm.Items.Item("23").Specific


                                    If Not String.IsNullOrEmpty(oMAtrix.Columns.Item("AB_BEntity").Cells.Item(pVal.Row).Specific.String) Or Not String.IsNullOrEmpty(oMAtrix.Columns.Item("AB_BLineNo").Cells.Item(pVal.Row).Specific.String) Then
                                        If Not String.IsNullOrEmpty(oMAtrix.Columns.Item("10").Cells.Item(pVal.Row).Specific.String) Then
                                            If CDbl(oMAtrix.Columns.Item("10").Cells.Item(pVal.Row).Specific.String) > CDbl(oMAtrix.Columns.Item("AB_VQty").Cells.Item(pVal.Row).Specific.String) Then
                                                oMAtrix.Columns.Item("10").Cells.Item(pVal.Row).Specific.active = True
                                                p_oSBOApplication.StatusBar.SetText("Quantity cannot be greater than " & oMAtrix.Columns.Item("AB_VQty").Cells.Item(pVal.Row).Specific.String & "! ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                                Exit Sub
                                            End If
                                        Else
                                            oMAtrix.Columns.Item("10").Cells.Item(pVal.Row).Specific.active = True
                                            p_oSBOApplication.StatusBar.SetText("Quantity cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub
                                        End If
                                    End If


                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End Try
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then

                            If pVal.ItemUID = "1" Then
                                Try

                                    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(940, pVal.FormTypeCount)
                                    Dim oMAtrix As SAPbouiCOM.Matrix = oForm.Items.Item("23").Specific

                                    oDTIT = New DataTable
                                    Dim PostingDate As Date = DateTime.ParseExact(GetDate(oForm.Items.Item("14").Specific.String, p_oDICompany), "yyyyMMdd", Nothing)

                                    oDTIT.Columns.Add("PRBaseEntry", GetType(String))
                                    oDTIT.Columns.Add("PRLineNo", GetType(Integer))
                                    oDTIT.Columns.Add("TransferQty", GetType(Double))


                                    For imjs As Integer = 1 To oMAtrix.VisualRowCount - 1

                                        If String.IsNullOrEmpty(oMAtrix.Columns.Item("1").Cells.Item(imjs).Specific.String) Then
                                            oMAtrix.Columns.Item("1").Cells.Item(imjs).Specific.active = True
                                            p_oSBOApplication.StatusBar.SetText("Item No. cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub
                                        End If

                                        If String.IsNullOrEmpty(oMAtrix.Columns.Item("1470001039").Cells.Item(imjs).Specific.String) Then
                                            oMAtrix.Columns.Item("1470001039").Cells.Item(imjs).Specific.active = True
                                            p_oSBOApplication.StatusBar.SetText("From Warehouse cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub
                                        End If

                                        If String.IsNullOrEmpty(oMAtrix.Columns.Item("5").Cells.Item(imjs).Specific.String) Then
                                            oMAtrix.Columns.Item("5").Cells.Item(imjs).Specific.active = True
                                            p_oSBOApplication.StatusBar.SetText("To Warehouse cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub
                                        End If

                                        If Not String.IsNullOrEmpty(oMAtrix.Columns.Item("AB_BEntity").Cells.Item(imjs).Specific.String) Or Not String.IsNullOrEmpty(oMAtrix.Columns.Item("AB_BLineNo").Cells.Item(imjs).Specific.String) Then

                                            If CDbl(oMAtrix.Columns.Item("10").Cells.Item(imjs).Specific.String) > CDbl(oMAtrix.Columns.Item("AB_VQty").Cells.Item(imjs).Specific.String) Then
                                                oMAtrix.Columns.Item("10").Cells.Item(imjs).Specific.active = True
                                                p_oSBOApplication.StatusBar.SetText("Quantity cannot be greater than " & oMAtrix.Columns.Item("AB_VQty").Cells.Item(imjs).Specific.String & "! ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                                Exit Sub
                                            End If

                                            If String.IsNullOrEmpty(oMAtrix.Columns.Item("AB_BEntity").Cells.Item(imjs).Specific.String) Then
                                                oMAtrix.Columns.Item("AB_BEntity").Cells.Item(imjs).Specific.active = True
                                                p_oSBOApplication.StatusBar.SetText("Base Entry cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                                Exit Sub
                                            End If

                                            If String.IsNullOrEmpty(oMAtrix.Columns.Item("AB_BLineNo").Cells.Item(imjs).Specific.String) Then
                                                oMAtrix.Columns.Item("AB_BLineNo").Cells.Item(imjs).Specific.active = True
                                                p_oSBOApplication.StatusBar.SetText("Base Line Number cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                                Exit Sub
                                            End If

                                            oDTIT.Rows.Add(oMAtrix.Columns.Item("AB_BEntity").Cells.Item(imjs).Specific.String, _
                                                                                                   CInt(oMAtrix.Columns.Item("AB_BLineNo").Cells.Item(imjs).Specific.String), _
                                                                                                   CDbl(oMAtrix.Columns.Item("10").Cells.Item(imjs).Specific.String))
                                        End If


                                        p_oSBOApplication.StatusBar.SetText("Validating the Information ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    Next imjs
                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End Try
                            End If

                        End If



                End Select
                Select Case pVal.FormUID
                    Case "SBS"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then

                            If pVal.ItemUID = "23" Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Dim omatrix As SAPbouiCOM.Matrix = oForm.Items.Item("19").Specific
                                For imjs As Integer = 1 To omatrix.RowCount
                                    If omatrix.IsRowSelected(imjs) = True Then
                                        Process.Start(omatrix.Columns.Item("V_2").Cells.Item(imjs).Specific.String)
                                    End If
                                Next imjs
                            End If

                            If pVal.ItemUID = "21" Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Try

                                    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("19").Specific
                                    Dim rowcount As Integer = oMatrix.RowCount
                                    Dim DeleteFlag As Boolean = False
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Deleting Attachment File ", sFuncName)
                                    For mjs As Integer = 1 To rowcount
                                        If mjs <= oMatrix.RowCount Then
                                            If oMatrix.IsRowSelected(mjs) = True Then
                                                oMatrix.DeleteRow(mjs)
                                                DeleteFlag = True
                                                mjs = mjs - 1
                                            End If
                                        Else
                                            Exit For
                                        End If
                                    Next mjs

                                    If DeleteFlag = True Then
                                        For mjs As Integer = 1 To oMatrix.RowCount
                                            oMatrix.Columns.Item("V_-1").Cells.Item(mjs).Specific.string = mjs
                                        Next
                                        If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                            oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                        End If
                                    End If
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed Deleting Attachment File ", sFuncName)
                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    sErrDesc = ex.Message
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                    WriteToLogFile(Err.Description, sFuncName)
                                End Try

                                Exit Sub
                            End If

                            If pVal.ItemUID = "14" Or pVal.ItemUID = "15" Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Select Case pVal.ItemUID

                                    Case "14"
                                        oForm.PaneLevel = 1
                                    Case "15"
                                        oForm.PaneLevel = 2
                                End Select
                            End If

                            If pVal.ItemUID = "1" And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then

                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Dim oDocDatatable As DataTable = Nothing
                                Dim oDisplay As DataTable = Nothing
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("Item_3").Specific
                                Dim irow As Integer = 0
                                Dim oEdit As SAPbouiCOM.EditText
                                sErrDesc = String.Empty
                                Dim sDocEntry As String = String.Empty
                                sFuncName = "Add Button Click()"
                                Try
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)

                                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                        sDocEntry = oForm.Items.Item("1000002").Specific.String
                                        ''If Not String.IsNullOrEmpty(oMatrix.Columns.Item("Col_0").Cells.Item(oMatrix.RowCount).Specific.string) Or _
                                        ''    Not String.IsNullOrEmpty(oMatrix.Columns.Item("Col_2").Cells.Item(oMatrix.RowCount).Specific.string) Or _
                                        ''    Not String.IsNullOrEmpty(oMatrix.Columns.Item("Col_3").Cells.Item(oMatrix.RowCount).Specific.string) Or _
                                        ''    Not String.IsNullOrEmpty(oMatrix.Columns.Item("Col_4").Cells.Item(oMatrix.RowCount).Specific.string) Then

                                        ''    irow = oMatrix.RowCount
                                        ''Else
                                        ''    irow = oMatrix.RowCount - 1
                                        ''End If

                                        'If irow = p_iRowCount Then Exit Sub
                                    Else
                                        sDocEntry = 0
                                    End If

                                    If p_Closeflag = True Then
                                        Exit Sub
                                    End If

                                    p_oSBOApplication.StatusBar.SetText("Validating The Information ", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                                    If StockBudget_Validation(oForm, sErrDesc) <> RTN_SUCCESS Then
                                        BubbleEvent = False
                                        Exit Sub
                                    End If

                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling MatrixDataToDataTable()", sFuncName)

                                    oDocDatatable = MatrixDataToDataTable(oMatrix, oForm, sErrDesc)
                                    If Not String.IsNullOrEmpty(sErrDesc) Then
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ProjectValidation() ", sFuncName)
                                    oDisplay = ProjectValidation(oDocDatatable, p_oDICompany, sDocEntry, sErrDesc)
                                    If Not String.IsNullOrEmpty(sErrDesc) Then
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Write_TextFile() ", sFuncName)
                                    If oDisplay.Rows.Count > 0 Then
                                        p_oSBOApplication.StatusBar.SetText("Validation Error ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        Write_TextFile(oDisplay, sErrDesc)
                                        BubbleEvent = False
                                        Exit Sub
                                    End If

                                    For imjs As Integer = 1 To oMatrix.RowCount - 1
                                        oMatrix.Columns.Item("V_0").Cells.Item(imjs).Specific.String = "C"
                                    Next imjs

                                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        oMatrix.DeleteRow(oMatrix.RowCount)
                                    End If

                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ", sFuncName)
                                    'ProjectValidation
                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                    WriteToLogFile(Err.Description, sFuncName)
                                End Try


                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_OK_MODE) Then
                            Dim oForm As SAPbouiCOM.Form
                            Try
                                oForm = p_oSBOApplication.Forms.ActiveForm
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("Item_3").Specific
                                oForm.Freeze(True)
                                If String.IsNullOrEmpty(oMatrix.Columns.Item("Col_0").Cells.Item(oMatrix.RowCount).Specific.String) Then
                                    p_Closeflag = True
                                    oMatrix.DeleteRow(oMatrix.RowCount)
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                    oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                End If
                                oForm.Freeze(False)
                            Catch ex As Exception
                                oForm.Freeze(False)
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                WriteToLogFile(Err.Description, sFuncName)
                            End Try

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_VALIDATE And pVal.InnerEvent = False Then
                            If pVal.ItemUID = "Item_3" And (pVal.ColUID = "Col_4" Or pVal.ColUID = "Col_5") Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("Item_3").Specific

                                Try
                                    oForm.Freeze(True)
                                    If String.IsNullOrEmpty(oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific.String) Then
                                        oMatrix.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific.String = 0
                                    End If
                                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        oMatrix.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.String = 0
                                        oMatrix.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.String = 0
                                        oMatrix.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.String = oMatrix.Columns.Item("Col_4").Cells.Item(pVal.Row).Specific.String

                                    ElseIf oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                        If CDbl(oMatrix.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.String) > CDbl(oMatrix.Columns.Item("Col_4").Cells.Item(pVal.Row).Specific.String) Then
                                            oMatrix.Columns.Item("Col_4").Cells.Item(pVal.Row).Specific.active = True
                                            p_oSBOApplication.StatusBar.SetText("Budget Qty cannot be less than PR Qty.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            oForm.Freeze(False)
                                            BubbleEvent = False
                                            Exit Sub
                                        Else
                                            oMatrix.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.String = CDbl(oMatrix.Columns.Item("Col_4").Cells.Item(pVal.Row).Specific.String) - CDbl(oMatrix.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.String)
                                        End If
                                    End If
                                    oMatrix.Columns.Item("Col_6").Cells.Item(pVal.Row).Specific.String = CDbl(oMatrix.Columns.Item("Col_4").Cells.Item(pVal.Row).Specific.String) * CDbl(oMatrix.Columns.Item("Col_5").Cells.Item(pVal.Row).Specific.String)
                                    oMatrix.AutoResizeColumns()

                                    oForm.Freeze(False)
                                Catch ex As Exception
                                    oForm.Freeze(False)
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                    WriteToLogFile(Err.Description, sFuncName)
                                End Try
                            End If
                        End If

                    Case "PMSM"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            If pVal.ItemUID = "1" Then
                                Try
                                    Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                                    ''If Not String.IsNullOrEmpty(oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.RowCount).Specific.ToString) Then
                                    ''    oForm.DataSources.DBDataSources.Item(1).Clear()
                                    ''    oMatrix.AddRow()
                                    ''End If

                                    If p_PestDragdrop = True Then
                                        p_oSBOApplication.StatusBar.SetText("Drop the data for Continuing Save / Update ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        BubbleEvent = False
                                        Exit Try
                                    End If


                                    If oMatrix.VisualRowCount = 1 Then
                                        If String.IsNullOrEmpty(oMatrix.Columns.Item("V_3").Cells.Item(1).Specific.String) Then
                                            oMatrix.Columns.Item("V_3").Cells.Item(1).Specific.active = True
                                            p_oSBOApplication.StatusBar.SetText("Type of Pest/Service cannot be Empty", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Try
                                        Else
                                            oMatrix.Columns.Item("V_2").Cells.Item(1).Specific.String = "C"
                                            oMatrix.Columns.Item("V_4").Cells.Item(1).Specific.String = 1
                                            If String.IsNullOrEmpty(oMatrix.Columns.Item("V_5").Cells.Item(1).Specific.String) Then
                                                oMatrix.Columns.Item("V_5").Cells.Item(1).Specific.String = 1
                                            End If
                                            oMatrix.Columns.Item("V_-1").Cells.Item(1).Specific.String = 1
                                        End If
                                    Else
                                        For imjs As Integer = 1 To oMatrix.VisualRowCount

                                            If String.IsNullOrEmpty(oMatrix.Columns.Item("V_3").Cells.Item(imjs).Specific.String) And imjs = oMatrix.VisualRowCount Then Exit For

                                            If String.IsNullOrEmpty(oMatrix.Columns.Item("V_3").Cells.Item(imjs).Specific.String) Then
                                                oMatrix.Columns.Item("V_3").Cells.Item(imjs).Specific.active = True
                                                p_oSBOApplication.StatusBar.SetText("Type of Pest/Service cannot be Empty", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                                BubbleEvent = False
                                                Exit Try
                                            End If
                                            oMatrix.Columns.Item("V_2").Cells.Item(imjs).Specific.String = "C"
                                            If String.IsNullOrEmpty(oMatrix.Columns.Item("V_5").Cells.Item(imjs).Specific.String) Then
                                                oMatrix.Columns.Item("V_5").Cells.Item(imjs).Specific.String = imjs
                                            End If

                                            oMatrix.Columns.Item("V_4").Cells.Item(imjs).Specific.String = imjs
                                            oMatrix.Columns.Item("V_-1").Cells.Item(imjs).Specific.String = imjs
                                        Next imjs

                                        ' If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                        If String.IsNullOrEmpty(oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount).Specific.string) Then
                                            oMatrix.DeleteRow(oMatrix.VisualRowCount)
                                        End If
                                        'End If
                                    End If

                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                    WriteToLogFile(Err.Description, sFuncName)
                                End Try
                                Exit Sub
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_VALIDATE And pVal.InnerEvent = False And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or pVal.FormMode = SAPbouiCOM.BoFormMode.fm_OK_MODE) Then

                            Dim oForm As SAPbouiCOM.Form
                            Try
                                oForm = p_oSBOApplication.Forms.ActiveForm
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                                If pVal.Row = oMatrix.VisualRowCount Then
                                    If Not String.IsNullOrEmpty(oMatrix.Columns.Item("V_3").Cells.Item(pVal.Row).Specific.String) Then
                                        oForm.Freeze(True)
                                        oForm.DataSources.DBDataSources.Item(1).Clear()
                                        oMatrix.AddRow()
                                        oMatrix.Columns.Item("V_1").Cells.Item(oMatrix.VisualRowCount).Specific.checked = True
                                        oMatrix.Columns.Item("V_-1").Cells.Item(oMatrix.VisualRowCount).Specific.String = oMatrix.VisualRowCount
                                        oMatrix.Columns.Item("V_4").Cells.Item(oMatrix.VisualRowCount).Specific.String = oMatrix.VisualRowCount
                                        oMatrix.Columns.Item("V_5").Cells.Item(oMatrix.VisualRowCount).Specific.String = oMatrix.VisualRowCount
                                        oMatrix.Columns.Item("V_3").Cells.Item(oMatrix.VisualRowCount - 1).Specific.active = True
                                        oForm.Freeze(False)
                                    End If
                                End If

                            Catch ex As Exception
                                oForm.Freeze(False)
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                WriteToLogFile(Err.Description, sFuncName)
                            End Try
                            Exit Sub

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then

                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim sSQL As String = String.Empty
                            Dim oRset As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                            Try
                                sSQL = "Delete FROM [dbo].[@AB_PESTTEMPLATE]  where isnull(U_question,'') = ''"
                                oRset.DoQuery(sSQL)
                                p_PestDragdrop = False

                            Catch ex As Exception
                                p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                BubbleEvent = False
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                WriteToLogFile(Err.Description, sFuncName)
                            End Try
                        End If

                    Case "PR_IT"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            If pVal.ItemUID = "btnChoose" Then
                                GetSelectedRows(oForm, sErrDesc)
                                oForm.Items.Item("2").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                If Fill_MatrixValues_InventoryTransfer(p_oSBOApplication.Forms.GetFormByTypeAndCount(940, iFormType_IT), sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                ''If p_oSBOApplication.Forms.ActiveForm.TypeEx = "720" Then
                                ''    If Fill_MatrixValues_GoodsIssue(p_oSBOApplication.Forms.ActiveForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                ''ElseIf p_oSBOApplication.Forms.ActiveForm.TypeEx = "940" Then

                                ''End If
                            End If

                            If pVal.ItemUID = "4" Then
                                Dim oCheckBox As SAPbouiCOM.CheckBox
                                oCheckBox = oForm.Items.Item("4").Specific
                                If oCheckBox.Checked = True Then
                                    Display_PurchaseRequest(oForm, "Y", "IT", sErrDesc)
                                ElseIf oCheckBox.Checked = False Then
                                    Display_PurchaseRequest(oForm, "", "IT", sErrDesc)
                                End If
                            End If
                        End If

                    Case "PR_GI"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            If pVal.ItemUID = "btnChoose" Then
                                GetSelectedRows(oForm, sErrDesc)
                                oForm.Items.Item("2").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                If Fill_MatrixValues_GoodsIssue(p_oSBOApplication.Forms.GetFormByTypeAndCount(720, iFormType_GI), sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                ''If p_oSBOApplication.Forms.ActiveForm.TypeEx = "720" Then
                                ''    If Fill_MatrixValues_GoodsIssue(p_oSBOApplication.Forms.ActiveForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                ''ElseIf p_oSBOApplication.Forms.ActiveForm.TypeEx = "940" Then

                                ''End If
                            End If

                            If pVal.ItemUID = "4" Then
                                Dim oCheckBox As SAPbouiCOM.CheckBox
                                oCheckBox = oForm.Items.Item("4").Specific
                                If oCheckBox.Checked = True Then
                                    Display_PurchaseRequest(oForm, "Y", "GI", sErrDesc)
                                ElseIf oCheckBox.Checked = False Then
                                    Display_PurchaseRequest(oForm, "", "GI", sErrDesc)
                                End If
                            End If
                        End If

                    Case "CSQ"
                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                        '    Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                        '    If CustomerSurvey_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        'End If


                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                If SurveyQuestion_Validation(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                p_CSQDragdrop = False

                            End If

                        ElseIf pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "txtMatrix" And pVal.ColUID = "V_-1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If p_CSQDragdrop = False Then
                                oMatrix.GetLineData(pVal.Row)
                                oMatrix.DeleteRow(pVal.Row)
                                p_CSQDragdrop = True
                            ElseIf p_CSQDragdrop = True Then
                                oMatrix.AddRow(1, pVal.Row)
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                End If
                                p_CSQDragdrop = False
                            End If

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If pVal.ItemUID = "1" Then
                                oForm.Freeze(True)
                                Try
                                    For imjs As Integer = 1 To oMatrix.RowCount
                                        oMatrix.Columns.Item("V_-1").Cells.Item(imjs).Specific.String = imjs
                                        oMatrix.Columns.Item("V_0").Cells.Item(imjs).Specific.String = imjs
                                    Next
                                Catch ex As Exception
                                    oForm.Freeze(False)
                                End Try

                                oForm.Freeze(False)
                            End If

                        End If



                    Case "SAQ"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                            'Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            'If ShowAroundQuestions_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            p_Dragdrop = False
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If pVal.ItemUID = "1" Then
                                oForm.Freeze(True)
                                Try
                                    For imjs As Integer = 1 To oMatrix.RowCount
                                        oMatrix.Columns.Item("V_-1").Cells.Item(imjs).Specific.String = imjs
                                        oMatrix.Columns.Item("V_0").Cells.Item(imjs).Specific.String = imjs
                                    Next
                                Catch ex As Exception
                                    oForm.Freeze(False)
                                End Try

                                oForm.Freeze(False)

                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "txtMatrix" And pVal.ColUID = "V_-1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If p_Dragdrop = False Then
                                oMatrix.GetLineData(pVal.Row)
                                oMatrix.DeleteRow(pVal.Row)
                                p_Dragdrop = True
                            ElseIf p_Dragdrop = True Then
                                '' oForm.DataSources.DBDataSources.Item("@AB_SHOWAROUNDMLINE").Clear()
                                oMatrix.AddRow(1, pVal.Row)
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                End If
                                ''oMatrix.SetLineData(pVal.Row)
                                p_Dragdrop = False
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            If ShowAroundQuestion_Validation(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        End If

                    Case "JJA"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "txtMatrix" And pVal.ColUID = "V_-1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If p_JJADragdrop = False Then
                                oMatrix.GetLineData(pVal.Row)
                                oMatrix.DeleteRow(pVal.Row)
                                p_JJADragdrop = True
                            ElseIf p_JJADragdrop = True Then
                                oMatrix.AddRow(1, pVal.Row)
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                End If
                                p_JJADragdrop = False
                            End If

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE) Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If pVal.ItemUID = "1" Then
                                Try

                                    If JobArea_Validation(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                    oForm.Freeze(True)

                                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then

                                        If p_JJADragdrop = True Then
                                            sErrDesc = "Drag flag is activated - Drop the Area/Location which you draged "
                                            p_oSBOApplication.StatusBar.SetText(sErrDesc, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub
                                        End If

                                        For imjs As Integer = 1 To oMatrix.RowCount
                                            oMatrix.Columns.Item("V_-1").Cells.Item(imjs).Specific.String = imjs
                                            oMatrix.Columns.Item("V_0").Cells.Item(imjs).Specific.String = imjs
                                        Next
                                    End If
                                    oForm.Freeze(False)
                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    sErrDesc = ex.Message().ToString()
                                    Call WriteToLogFile(sErrDesc, sFuncName)
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                    oForm.Freeze(False)
                                    BubbleEvent = False
                                End Try
                                Exit Sub
                            End If

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                            p_JJADragdrop = False
                        End If

                    Case "JCT"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "txtMatrix" And pVal.ColUID = "V_-1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If String.IsNullOrEmpty(oMatrix.Columns.Item("V_2").Cells.Item(pVal.Row).Specific.String) Then Exit Sub
                            If p_JCTDragdrop = False Then
                                oMatrix.GetLineData(pVal.Row)
                                oMatrix.DeleteRow(pVal.Row)
                                p_JCTDragdrop = True
                            ElseIf p_JCTDragdrop = True Then
                                oMatrix.AddRow(1, pVal.Row)
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                End If
                                p_JCTDragdrop = False
                            End If
                            Exit Sub
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And (pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE) Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("txtMatrix").Specific
                            If pVal.ItemUID = "1" Then
                                Try

                                    If JobType_Validation(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                    oForm.Freeze(True)

                                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then

                                        If p_JCTDragdrop = True Then
                                            sErrDesc = "Drag flag is activated - Drop the category which you draged "
                                            p_oSBOApplication.StatusBar.SetText(sErrDesc, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub
                                        End If

                                        For imjs As Integer = 1 To oMatrix.RowCount
                                            oMatrix.Columns.Item("V_-1").Cells.Item(imjs).Specific.String = imjs
                                            oMatrix.Columns.Item("V_0").Cells.Item(imjs).Specific.String = imjs
                                        Next
                                    End If
                                    oForm.Freeze(False)
                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    sErrDesc = ex.Message().ToString()
                                    Call WriteToLogFile(sErrDesc, sFuncName)
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
                                    oForm.Freeze(False)
                                    BubbleEvent = False
                                End Try
                                Exit Sub
                            End If

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                            p_JCTDragdrop = False
                        End If

                    Case "MLT"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "matContent" And pVal.ColUID = "V_-1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific
                            If p_MLTDragdrop = False Then
                                oMatrix.GetLineData(pVal.Row)
                                oMatrix.DeleteRow(pVal.Row)
                                p_MLTDragdrop = True
                            ElseIf p_MLTDragdrop = True Then
                                oMatrix.AddRow(1, pVal.Row)
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                End If
                                p_MLTDragdrop = False
                            End If

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific
                            If pVal.ItemUID = "1" Then
                                oForm.Freeze(True)
                                Try
                                    For imjs As Integer = 1 To oMatrix.RowCount
                                        oMatrix.Columns.Item("V_-1").Cells.Item(imjs).Specific.String = imjs
                                        oMatrix.Columns.Item("V_0").Cells.Item(imjs).Specific.String = imjs
                                    Next
                                Catch ex As Exception
                                    oForm.Freeze(False)
                                End Try

                                oForm.Freeze(False)
                            End If

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                            p_MLTDragdrop = False
                        End If


                    Case "MGT"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "matContent" And pVal.ColUID = "V_-1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific
                            If p_MGTDragdrop = False Then
                                oMatrix.GetLineData(pVal.Row)
                                oMatrix.DeleteRow(pVal.Row)
                                p_MGTDragdrop = True
                            ElseIf p_MGTDragdrop = True Then
                                oMatrix.AddRow(1, pVal.Row)
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                End If
                                p_MGTDragdrop = False
                            End If

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific
                            If pVal.ItemUID = "1" Then
                                oForm.Freeze(True)
                                Try
                                    For imjs As Integer = 1 To oMatrix.RowCount
                                        oMatrix.Columns.Item("V_-1").Cells.Item(imjs).Specific.String = imjs
                                        oMatrix.Columns.Item("V_0").Cells.Item(imjs).Specific.String = imjs
                                    Next
                                Catch ex As Exception
                                    oForm.Freeze(False)
                                End Try

                                oForm.Freeze(False)
                            End If

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                            p_MGTDragdrop = False
                        End If
                        '-------------Before Action true

                    Case "MST"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And pVal.ItemUID = "1" And pVal.FormMode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)
                            '' If MarketSegmentTemplate_FormLoad(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                            If MarketSegmentTemplate_Validation(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "matContent" And pVal.ColUID = "V_-1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matContent").Specific
                            If p_MSTDragdrop = False Then
                                oMatrix.GetLineData(pVal.Row)
                                oMatrix.DeleteRow(pVal.Row)
                                p_MSTDragdrop = True
                            ElseIf p_MSTDragdrop = True Then
                                oMatrix.AddRow(1, pVal.Row)
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                                End If
                                p_MSTDragdrop = False
                            End If

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                            Dim oform As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                            p_MSTDragdrop = False

                            Exit Sub
                        End If


                    Case "SAS"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "1" Then

                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.GetFormByTypeAndCount(60004, pVal.FormTypeCount)

                            If ShowAround_FileAttachment(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        End If

                        'If pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_CLOSE Then
                        '    'Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                        '    'If ShowAround_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                        'End If

                        '=========================== Job Scheduling - Job Area Master ==============================================================

                    Case "JJS"

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED And (pVal.ItemUID = "29" Or pVal.ItemUID = "30" Or pVal.ItemUID = "31") Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm

                            Select Case pVal.ItemUID

                                Case "29"
                                    oForm.PaneLevel = 1

                                Case "30"
                                    oForm.PaneLevel = 2

                                Case "31"
                                    oForm.PaneLevel = 3
                            End Select
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_VALIDATE And pVal.InnerEvent = False And pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If pVal.ItemUID = "matJobSch" And pVal.ColUID = "V_4" Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Dim dStartDate, dEndDate, dDate As Date
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matJobSch").Specific
                                If Not String.IsNullOrEmpty(oMatrix.Columns.Item("V_4").Cells.Item(pVal.Row).Specific.String) Then
                                    If Not String.IsNullOrEmpty(oForm.Items.Item("txtStrDate").Specific.value) Then
                                        dStartDate = DateTime.ParseExact(oForm.Items.Item("txtStrDate").Specific.value, "yyyyMMdd", Nothing)
                                    Else
                                        p_oSBOApplication.SetStatusBarMessage("Start Date cannot be Empty", SAPbouiCOM.BoMessageTime.bmt_Short, True)
                                        oForm.Items.Item("txtStrDate").Specific.active = True
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                    If Not String.IsNullOrEmpty(oForm.Items.Item("txtEndDate").Specific.value) Then
                                        dEndDate = DateTime.ParseExact(oForm.Items.Item("txtEndDate").Specific.value, "yyyyMMdd", Nothing)
                                    Else
                                        p_oSBOApplication.SetStatusBarMessage("Start Date cannot be Empty", SAPbouiCOM.BoMessageTime.bmt_Short, True)
                                        oForm.Items.Item("txtEndDate").Specific.active = True
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                    dDate = DateTime.ParseExact(GetDate(oMatrix.Columns.Item("V_4").Cells.Item(pVal.Row).Specific.String, p_oDICompany), "yyyyMMdd", Nothing)

                                    If dStartDate <= dDate And dEndDate >= dDate Then
                                    Else
                                        p_oSBOApplication.SetStatusBarMessage("Invalid Date range ", SAPbouiCOM.BoMessageTime.bmt_Short, True)
                                        oMatrix.Columns.Item("V_4").Cells.Item(pVal.Row).Specific.active = True
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                            End If

                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.ItemUID = "1" Then
                            Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm


                            If (oForm.Mode <> SAPbouiCOM.BoFormMode.fm_OK_MODE AndAlso oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE) Then
                                p_oSBOApplication.StatusBar.SetText("Please Wait Validating the Values...", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                'If JobSchedule_Validation(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                If JobSchedule_Validation_New(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                p_oSBOApplication.StatusBar.SetText("Please Wait Validating on Daily Scheduling Information...", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                If JobSchedule_FillDailySchedule(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                p_oSBOApplication.StatusBar.SetText("Please Wait Validating on Task Scheduling Information...", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                If JobSchedule_FillTaskSchedule(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                                p_oSBOApplication.StatusBar.SetText("validation Completed Successfully!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                                sJobSchKey = oForm.Items.Item("txtDocEntr").Specific.string
                            End If
                        End If
                    Case "JTS"
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                            If (pVal.ItemUID = "folAttach" Or pVal.ItemUID = "folContent") Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Select Case pVal.ItemUID
                                    Case "folContent"
                                        oForm.PaneLevel = 1
                                    Case "folAttach"
                                        oForm.PaneLevel = 2
                                End Select

                            ElseIf (pVal.ItemUID = "matAttach" Or pVal.ItemUID = "folAttach") Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matAttach").Specific
                                If oMatrix.RowCount > 0 Then
                                    oForm.Items.Item("btnDisplay").Enabled = True
                                Else
                                    oForm.Items.Item("btnDisplay").Enabled = False
                                End If


                            ElseIf pVal.ItemUID = "btnDisplay" Then
                                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                                Dim oMatrix As SAPbouiCOM.Matrix = oForm.Items.Item("matAttach").Specific
                                Dim sFilePath As String = String.Empty

                                Try
                                    For imjs As Integer = 1 To oMatrix.RowCount
                                        If oMatrix.IsRowSelected(imjs) = True Then
                                            sFilePath = oMatrix.Columns.Item("V_2").Cells.Item(imjs).Specific.String
                                            Exit For
                                        End If
                                    Next
                                    If Not String.IsNullOrEmpty(sFilePath) Then
                                        If System.IO.File.Exists(sFilePath) = True Then
                                            Process.Start(sFilePath)
                                        Else
                                            sErrDesc = "File Does Not Exist"
                                            p_oSBOApplication.StatusBar.SetText(sErrDesc, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            BubbleEvent = False
                                            Exit Sub

                                        End If
                                    End If

                                Catch ex As Exception
                                    p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    BubbleEvent = False
                                    Exit Sub
                                End Try
                            End If



                        End If

            If pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                    If TaskSchedule_UpdateReSchedule(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                End If
            End If

                End Select
            End If

            'If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch exc As Exception
            sErrDesc = exc.Message
            p_oSBOApplication.StatusBar.SetText(sErrDesc, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            BubbleEvent = False
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            WriteToLogFile(Err.Description, sFuncName)
            ShowErr(sErrDesc)
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub SBO_Application_RightClickEvent(ByRef eventInfo As SAPbouiCOM.ContextMenuInfo, ByRef BubbleEvent As Boolean) Handles SBO_Application.RightClickEvent
        Dim oform As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
        Try
            If eventInfo.BeforeAction = True Then

                If oform.UniqueID = "JJS" And oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                    If JobSchedule_RightClickMenu(oform, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                    oform.EnableMenu("1286", True)

                    sDocEntry = oform.Items.Item("txtDocEntr").Specific.string
                    Dim oCombo As SAPbouiCOM.ComboBox = oform.Items.Item("cmbStatus").Specific
                    If oCombo.Selected.Value = "D" Then
                        oform.EnableMenu("1287", False)
                    Else
                        oform.EnableMenu("1287", True)
                    End If
                Else
                    oform.EnableMenu("1287", False)
                    If p_oSBOApplication.Menus.Exists("JDS") Then
                        'If the menu already exists this code will fail
                        p_oSBOApplication.Menus.RemoveEx("JDS")
                    End If
                End If

                If oform.UniqueID = "SAQ" Then
                    If eventInfo.BeforeAction = True Then
                        If eventInfo.EventType = SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK Then
                            p_ishowaround_question = eventInfo.Row
                        End If
                    End If
                End If

                If oform.UniqueID = "CSQ" Then
                    If eventInfo.BeforeAction = True Then
                        If eventInfo.EventType = SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK Then
                            p_isurveyquestion = eventInfo.Row
                        End If
                    End If
                End If


                If oform.UniqueID = "JCT" Then
                    If eventInfo.BeforeAction = True Then
                        If eventInfo.EventType = SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK Then
                            p_iservicequestion = eventInfo.Row
                        End If
                    End If
                End If

                If oform.UniqueID = "JJA" Then
                    If eventInfo.BeforeAction = True Then
                        If eventInfo.EventType = SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK Then
                            p_ijobareaquestion = eventInfo.Row
                        End If
                    End If
                End If

                If oform.UniqueID = "MGT" Then
                    If eventInfo.BeforeAction = True Then
                        If eventInfo.EventType = SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK Then
                            p_iGardenerTemplate = eventInfo.Row
                        End If
                    End If
                End If

                If oform.UniqueID = "MLT" Then
                    If eventInfo.BeforeAction = True Then
                        If eventInfo.EventType = SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK Then
                            p_iLandscapeTemplate = eventInfo.Row
                        End If
                    End If
                End If


                If oform.UniqueID = "PMSM" Then
                    If eventInfo.BeforeAction = True Then
                        If eventInfo.EventType = SAPbouiCOM.BoEventTypes.et_RIGHT_CLICK Then

                            If p_oSBOApplication.Menus.Exists("784") Then
                                'If the menu already exists this code will fail
                                p_oSBOApplication.Menus.RemoveEx("784")
                            End If
                            If p_oSBOApplication.Menus.Exists("6413") Then
                                'If the menu already exists this code will fail
                                p_oSBOApplication.Menus.RemoveEx("6413")
                            End If
                            If p_oSBOApplication.Menus.Exists("6414") Then
                                'If the menu already exists this code will fail
                                p_oSBOApplication.Menus.RemoveEx("6414")
                            End If
                            If p_oSBOApplication.Menus.Exists("774") Then
                                'If the menu already exists this code will fail
                                p_oSBOApplication.Menus.RemoveEx("774")
                            End If

                            p_iPestRow = eventInfo.Row

                        End If
                    End If
                End If

                If oform.UniqueID = "SBS" Then
                    Dim oMenuItem As SAPbouiCOM.MenuItem
                    Dim oMenus As SAPbouiCOM.Menus
                    ' Create menu popup MyUserMenu01 and add it to Tools menu
                    Dim oCreationPackage As SAPbouiCOM.MenuCreationParams
                    oCreationPackage = p_oSBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)
                    oMenuItem = p_oSBOApplication.Menus.Item("1280") 'Data'
                    oMenus = oMenuItem.SubMenus
                    'Create sub menu MySubMenu1 and add it to popup MyUserMenu01
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
                    oCreationPackage.UniqueID = "3045646"
                    oCreationPackage.String = "Delete Row"
                    oCreationPackage.Enabled = True
                    If Not p_oSBOApplication.Menus.Exists("3045646") Then
                        'If the menu already exists this code will fail
                        oMenus.AddEx(oCreationPackage)
                    End If

                    If p_oSBOApplication.Menus.Exists("772") Then
                        'If the menu already exists this code will fail
                        p_oSBOApplication.Menus.RemoveEx("772")
                    End If
                    If p_oSBOApplication.Menus.Exists("784") Then
                        'If the menu already exists this code will fail
                        p_oSBOApplication.Menus.RemoveEx("784")
                    End If
                    If p_oSBOApplication.Menus.Exists("6413") Then
                        'If the menu already exists this code will fail
                        p_oSBOApplication.Menus.RemoveEx("6413")
                    End If
                    If p_oSBOApplication.Menus.Exists("6414") Then
                        'If the menu already exists this code will fail
                        p_oSBOApplication.Menus.RemoveEx("6414")
                    End If
                    If p_oSBOApplication.Menus.Exists("774") Then
                        'If the menu already exists this code will fail
                        p_oSBOApplication.Menus.RemoveEx("774")
                    End If
                    ''oMenus.AddEx(oCreationPackage)
                Else
                    If p_oSBOApplication.Menus.Exists("3045646") Then
                        'If the menu already exists this code will fail
                        p_oSBOApplication.Menus.RemoveEx("3045646")
                    End If

                End If
            End If
        Catch ex As Exception

        End Try




    End Sub

    Private Sub SBO_Application_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles SBO_Application.FormDataEvent

        ' **********************************************************************************
        '   Function   :    SBO_Application_FormDataEvent()
        '   Purpose    :    This function will be handling the SAP Form Data Event
        '               
        '   Parameters :    ByVal BusinessObjectInfo As String
        '                       BusinessObjectInfo = set the ObjectID
        '                   ByRef BubbleEvent As Boolean
        '                       BubbleEvent = set the True/False        
        '   Author     :    Srinivasan
        '   Date       :    20 April 2015
        '   Change     :
        ' ***********************************************************************************


        Try
            sFuncName = "SBO_Application_FormDataEvent()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting the Function", sFuncName)


            If BusinessObjectInfo.FormUID = "SAS" And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Or BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) And BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                'p_sDocEntry = oForm.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0)
                If ShowAround_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            ElseIf BusinessObjectInfo.FormUID = "SAQ" And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Or BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) And BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                If ShowAroundQuestions_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            ElseIf BusinessObjectInfo.FormUID = "CSQ" And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Or BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) And BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                If CustomerSurvey_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            ElseIf BusinessObjectInfo.FormUID = "JJA" And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Or BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) And BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                If JobArea_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            ElseIf BusinessObjectInfo.FormUID = "JCT" And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Or BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) And BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                If JobType_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            ElseIf BusinessObjectInfo.FormUID = "MST" And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Or BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) And BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                p_sDocEntry = oForm.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0)
                If MarketSegmentTemplate_DeleteEmptyRows(oForm, p_sDocEntry, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            ElseIf BusinessObjectInfo.FormUID = "MGT" And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Or BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) And BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                If GardenerTemplate_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            ElseIf BusinessObjectInfo.FormUID = "MLT" And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Or BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) And BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                If LandscapeTemplate_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            ElseIf BusinessObjectInfo.FormUID = "JJS" And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Or BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) And BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                If JobSchedule_DeleteEmptyRows(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
                If JobSchedule_CloseStatus(oForm, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

        Catch ex As Exception

            p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            BubbleEvent = False
            sErrDesc = ex.Message
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            WriteToLogFile(Err.Description, sFuncName)
            ShowErr(sErrDesc)

        End Try
    End Sub

End Class
