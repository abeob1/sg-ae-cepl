﻿Option Explicit On
Imports System.Xml
Imports System.IO
Imports System.Windows.Forms
Imports System.Globalization


Module modCommon

    Public Function ConnectDICompSSO(ByRef objCompany As SAPbobsCOM.Company, ByRef sErrDesc As String) As Long
        ' ***********************************************************************************
        '   Function   :    ConnectDICompSSO()
        '   Purpose    :    Connect To DI Company Object
        '
        '   Parameters :    ByRef objCompany As SAPbobsCOM.Company
        '                       objCompany = set the SAP Company Object
        '                   ByRef sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    Sri
        '   Date       :    29 April 2013
        '   Change     :
        ' ***********************************************************************************
        Dim sCookie As String = String.Empty
        Dim sConnStr As String = String.Empty
        Dim sFuncName As String = String.Empty
        Dim lRetval As Long
        Dim iErrCode As Int32
        Try
            sFuncName = "ConnectDICompSSO()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            objCompany = New SAPbobsCOM.Company

            sCookie = objCompany.GetContextCookie
            sConnStr = p_oUICompany.GetConnectionContext(sCookie)
            lRetval = objCompany.SetSboLoginContext(sConnStr)

            If Not lRetval = 0 Then
                Throw New ArgumentException("SetSboLoginContext of Single SignOn Failed.")
            End If
            p_oSBOApplication.StatusBar.SetText("Please Wait While Company Connecting... ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
            lRetval = objCompany.Connect
            If lRetval <> 0 Then
                objCompany.GetLastError(iErrCode, sErrDesc)
                Throw New ArgumentException("Connect of Single SignOn failed : " & sErrDesc)
            Else
                p_oSBOApplication.StatusBar.SetText("Company Connection Has Established with the " & objCompany.CompanyName, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

            End If
            ConnectDICompSSO = RTN_SUCCESS
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch exc As Exception
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            ConnectDICompSSO = RTN_ERROR
        End Try
    End Function

    Public Function ConnectTargetDB(ByRef oTargetCmp As SAPbobsCOM.Company, _
                                    ByVal sTargetDB As String, _
                                    ByVal sSAPUser As String, _
                                    ByVal sSAPPwd As String, _
                                    ByRef sErrDesc As String) As Long
        ' **********************************************************************************
        'Function   :   ConnectTargetDB()
        'Purpose    :   Connect To Target Database
        '               This is for Intercompany Features
        '               
        'Parameters :   ByRef sErrDesc As String
        '                   sErrDesc=Error Description to be returned to calling function
        '               
        '                   =
        'Return     :   0 - FAILURE
        '               1 - SUCCESS
        'Author     :   Sri
        'Date       :   30 April 2013
        'Change     :
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim lRetval As Long
        Dim iErrCode As Integer
        Try
            sFuncName = "ConnectTargetDB()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oTargetCmp = Nothing
            oTargetCmp = New SAPbobsCOM.Company

            With oTargetCmp
                .Server = p_oDICompany.Server                           'Name of the DB Server 
                .DbServerType = p_oDICompany.DbServerType 'Database Type
                .CompanyDB = sTargetDB                        'Enter the name of Target company
                .UserName = sSAPUser                           'Enter the B1 user name
                .Password = sSAPPwd                           'Enter the B1 password
                .language = SAPbobsCOM.BoSuppLangs.ln_English          'Enter the logon language
                .UseTrusted = False
            End With

            lRetval = oTargetCmp.Connect()
            If lRetval <> 0 Then
                oTargetCmp.GetLastError(iErrCode, sErrDesc)
                oTargetCmp.CompanyDB = sTargetDB                        'Enter the name of Target company
                p_oSBOApplication.MessageBox("Connect to Target Company Failed :  " & sTargetDB & ". " & sErrDesc)
                Throw New ArgumentException("Connect to Target Company Failed :  " & sTargetDB & ". " & sErrDesc)
            End If

            ConnectTargetDB = RTN_SUCCESS
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

        Catch exc As Exception
            ConnectTargetDB = RTN_ERROR
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        Finally

        End Try
    End Function

    Public Function AddButton(ByRef oForm As SAPbouiCOM.Form, _
                              ByVal sButtonID As String, _
                              ByVal sCaption As String, _
                              ByVal sItemNo As String, _
                              ByVal iSpacing As Integer, _
                              ByVal iWidth As Integer, _
                              ByVal blnVisable As Boolean, _
                              ByRef sErrDesc As String, _
                              Optional ByVal oType As SAPbouiCOM.BoButtonTypes = 0, _
                              Optional ByVal sCFLObjType As String = "") As Long
        ' ***********************************************************************************
        '   Function   :    AddButton()
        '   Purpose    :    Add Button To Form
        '
        '   Parameters :    ByVal oForm As SAPbouiCOM.Form
        '                       oForm = set the SAP UI Form Object
        '                   ByVal sButtonID As String
        '                       sButtonID = Button UID
        '                   ByVal sCaption As String
        '                       sCaption = Caption
        '                   ByVal sItemNo As String
        '                       sItemNo = Next to Item UID
        '                   ByVal iSpacing As Integer
        '                       iSpacing = Spacing between sItemNo
        '                   ByVal iWidth As Integer
        '                       iWidth = Width
        '                   ByVal blnVisable As Boolean
        '                       blnVisible = True/False
        '                   ByRef sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '                   Optional ByVal oType As SAPbouiCOM.BoButtonTypes
        '                       oType = set the SAP UI Button Type Object
        '                   Optional ByVal sCFLObjType As String = ""
        '                       sCFLObjType = CFL Object Type
        '                   Optional ByVal sImgPath As String = ""
        '                       sImgPath = Image Path
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    Jason Ham
        '   Date       :    9 Jan 2007
        '   Change     :
        '                   9 Jan 2008 (Jason) Add Object Link
        ' ***********************************************************************************
        Dim oItems As SAPbouiCOM.Items
        Dim oItem As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "AddButton()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oItems = oForm.Items
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Add BUTTON Item", sFuncName)
            oItem = oItems.Add(sButtonID, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            If sCaption <> "" Then
                oItem.Specific.Caption = sCaption
            End If
            oItem.Visible = blnVisable
            oItem.Left = oItems.Item(sItemNo).Left + oItems.Item(sItemNo).Width + iSpacing
            oItem.Height = oItems.Item(sItemNo).Height
            oItem.Top = oItems.Item(sItemNo).Top
            oItem.Width = iWidth
            oButton = oItem.Specific
            oButton.Type = oType    'default is Caption type.

            If oType = 1 Then oButton.Image = "CHOOSE_ICON" 'This line will fire if the button type is image

            If sCFLObjType <> "" Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Add User Data Source :" & sButtonID, sFuncName)
                oForm.DataSources.UserDataSources.Add(sButtonID, SAPbouiCOM.BoDataType.dt_SHORT_TEXT)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("AddChooseFromList" & sButtonID, sFuncName)
                AddChooseFromList(oForm, sCFLObjType, sButtonID, sErrDesc)
                oButton.ChooseFromListUID = sButtonID
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            AddButton = RTN_SUCCESS
        Catch exc As Exception
            AddButton = RTN_ERROR
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        Finally
            oItems = Nothing
            oItem = Nothing
        End Try

    End Function

    Public Function AddChooseFromList(ByVal oForm As SAPbouiCOM.Form, ByVal sCFLObjType As String, ByVal sItemUID As String, ByRef sErrDesc As String) As Long
        ' ***********************************************************************************
        '   Function   :    AddChooseFromList()
        '   Purpose    :    Create Choose From List For User Define Form
        '
        '   Parameters :    ByVal oForm As SAPbouiCOM.Form
        '                       oForm = set the SAP UI Form Object
        '                   ByVal sCFLObjType As String
        '                       sCFLObjType = set SAP UI Choose From List Object Type
        '                   ByRef sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    Jason Ham
        '   Date       :    30/12/2007
        '   Change     :
        ' ***********************************************************************************
        Dim sFuncName As String = String.Empty
        Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams

        Try

            sFuncName = "AddChooseFromList"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Creating 'ChooseFromLists' and 'cot_ChooseFromListCreationParams' objects", sFuncName)
            oCFLs = oForm.ChooseFromLists
            oCFLCreationParams = p_oSBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Setting Choose From List Parameter properties", sFuncName)
            'Only Single Selection
            oCFLCreationParams.MultiSelection = False
            'Determine the Object Type
            oCFLCreationParams.ObjectType = sCFLObjType
            'Item UID as Unique ID for CFL
            oCFLCreationParams.UniqueID = sItemUID

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Adding Choose From List Parameter", sFuncName)
            oCFL = oCFLs.Add(oCFLCreationParams)

            AddChooseFromList = RTN_SUCCESS
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

        Catch exc As Exception
            AddChooseFromList = RTN_ERROR
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        End Try

    End Function

    Public Function AddUserDataSrc(ByRef oForm As SAPbouiCOM.Form, ByVal sDSUID As String, _
                                   ByRef sErrDesc As String, ByVal oDataType As SAPbouiCOM.BoDataType, _
                                   Optional ByVal lLen As Long = 0) As Long
        ' ***********************************************************************************
        '   Function   :    AddUserDataSrc()
        '   Purpose    :    Add User Data Source
        '
        '   Parameters :    ByVal oForm As SAPbouiCOM.Form
        '                       oForm = set the SAP UI Form Object
        '                   ByVal sDSUID As String
        '                       sDSUID = Data Set UID
        '                   ByRef sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '                   ByVal oDataType As SAPbouiCOM.BoDataType
        '                       oDataType = set the SAP UI BoDataType Object
        '                   Optional ByVal lLen As Long = 0
        '                       lLen= Length
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    Dev
        '   Date       :    23 Jan 2007
        '   Change     :
        ' ***********************************************************************************
        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "AddUserDataSrc()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If lLen = 0 Then
                oForm.DataSources.UserDataSources.Add(sDSUID, oDataType)
            Else
                oForm.DataSources.UserDataSources.Add(sDSUID, oDataType, lLen)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            AddUserDataSrc = RTN_SUCCESS
        Catch exc As Exception
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            AddUserDataSrc = RTN_ERROR
        End Try
    End Function

    Public Function AddItem(ByRef oForm As SAPbouiCOM.Form, ByVal sItemUID As String, ByVal bEnable As Boolean, _
                            ByVal oItemType As SAPbouiCOM.BoFormItemTypes, ByRef sErrDesc As String, _
                            Optional ByVal sCaption As String = "", Optional ByVal iPos As Integer = 0, _
                            Optional ByVal sPosItemUID As String = "", Optional ByVal lSpace As Long = 5, _
                            Optional ByVal lLeft As Long = 0, Optional ByVal lTop As Long = 0, _
                            Optional ByVal lHeight As Long = 0, Optional ByVal lWidth As Long = 0, _
                            Optional ByVal lFromPane As Long = 0, Optional ByVal lToPane As Long = 0, _
                            Optional ByVal sCFLObjType As String = "", Optional ByVal sCFLAlias As String = "", _
                            Optional ByVal oLinkedObj As SAPbouiCOM.BoLinkedObject = 0, _
                            Optional ByVal sBindTbl As String = "", Optional ByVal sAlias As String = "", _
                            Optional ByVal bDisplayDesc As Boolean = False) As Long
        ' ***********************************************************************************
        '   Function   :    AddItem()
        '   Purpose    :    Add Form's Item
        '
        '   Parameters :    ByVal oForm As SAPbouiCOM.Form
        '                       oForm = set the SAP UI Form Type
        '                   ByVal sItemUID As String
        '                       sItemUID = Item's ID
        '                   ByVal bEnable As Boolean
        '                       bEnable = Enable or Disable The Item
        '                   ByVal oItemType As SAPbouiCOM.BoFormItemTypes
        '                       oItemType = Item's Type
        '                   ByRef sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '                   Optional ByVal sCaption As String = ""
        '                       sCaption = Caption
        '                   Optional ByVal iPos As Integer = 0
        '                       iPos = Position.
        '                           Case 1 Left os sPosItemUID
        '                           Case 2 Right of sPosItemUID
        '                           Case 3 Top of sPosItemUID
        '                           Case Else Below sPosItemUID
        '                   Optional ByVal sPosItemUID As String = ""
        '                       sPosItemUID=Returns or sets the beginning of range specifying on which panes the item is visible. 0 by default
        '                   Optional ByVal lSpace As Long = 5
        '                       lSpace=sets the item space between oItem and sPosItemUID
        '                   Optional ByVal lLeft As Long = 0
        '                       lLeft=sets the item Left.
        '                   Optional ByVal lTop As Long = 0
        '                       lTop=sets the item top.
        '                   Optional ByVal lHeight As Long = 0
        '                       lHeight=sets the item height.
        '                   Optional ByVal lWidth As Long = 0
        '                       lWidth=sets the item weight.
        '                   Optional ByVal lFromPane As Long = 0
        '                       lFromPane=sets the beginning of range specifying on which panes the item is visible. 0 by default.
        '                   Optional ByVal lToPane As Long = 0
        '                       lToPane=sets the beginning of range specifying on which panes the item is visible. 0 by default.
        '                   Optional ByVal sCFLObjType As String = ""
        '                       sCFLObjType=CFL Obj Type
        '                   Optional ByVal sCFLAlias As String = ""
        '                       sCFLAlias=CFL Alias
        '                   Optional ByVal sBindTbl As String = ""
        '                       sBindTbl=Bind Table 
        '                   Optional ByVal sAlias As String = ""
        '                       sAlias=Alias
        '                   Optional ByVal bDisplayDesc As Boolean = False
        '                       bDisplayDesc=Returns or sets a a boolean value specifying whether or not to show the description of valid values of a ComboBox item. 
        '                                   True - displays the description of the valid value.
        '                                   False - displays the value of the selected valid value. 
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    Sri
        '   Date       :    29/04/2013
        ' ***********************************************************************************

        Dim oItem As SAPbouiCOM.Item
        Dim oPosItem As SAPbouiCOM.Item
        Dim oEdit As SAPbouiCOM.EditText
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "AddItem()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function. Item: " & sItemUID, sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Adding item", sFuncName)
            oItem = oForm.Items.Add(sItemUID, oItemType)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Setting item properties", sFuncName)
            If Trim(sPosItemUID) <> "" Then
                oPosItem = oForm.Items.Item(sPosItemUID)
                oItem.Enabled = bEnable
                oItem.Height = oPosItem.Height
                oItem.Width = oPosItem.Width
                Select Case iPos
                    Case 1      'Left of sPosItemUID
                        oItem.Left = oPosItem.Left - lSpace
                        oItem.Top = oPosItem.Top
                    Case 2      '2=Right of sPosItemUID
                        oItem.Left = oPosItem.Left + oPosItem.Width + lSpace
                        oItem.Top = oPosItem.Top
                    Case 3      '3=Top of sPosItemUID
                        oItem.Left = oPosItem.Left
                        oItem.Top = oPosItem.Top - lSpace
                    Case 4
                        oItem.Left = oPosItem.Left + lSpace
                        oItem.Top = oPosItem.Top + lSpace
                    Case Else   'Below sPosItemUID
                        oItem.Left = oPosItem.Left
                        oItem.Top = oPosItem.Top + oPosItem.Height + lSpace
                End Select
            End If

            If lTop <> 0 Then oItem.Top = lTop
            If lLeft <> 0 Then oItem.Left = lLeft
            If lHeight <> 0 Then oItem.Height = lHeight
            If lWidth <> 0 Then oItem.Width = lWidth

            If Trim(sBindTbl) <> "" Or Trim(sAlias) <> "" Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Adding item DataSource", sFuncName)
                oItem.Specific.DataBind.SetBound(True, sBindTbl, sAlias)
            End If

            oItem.FromPane = lFromPane
            oItem.ToPane = lToPane
            oItem.DisplayDesc = bDisplayDesc

            If Trim(sCaption) <> "" Then oItem.Specific.Caption = sCaption

            If sCFLObjType <> "" And oItem.Type = SAPbouiCOM.BoFormItemTypes.it_EDIT Then
                'If Choose From List Item
                oForm.DataSources.UserDataSources.Add(sItemUID, SAPbouiCOM.BoDataType.dt_SHORT_TEXT)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling AddChooseFromList()", sFuncName)
                AddChooseFromList(oForm, sCFLObjType, sItemUID, sErrDesc)
                oEdit = oItem.Specific
                oEdit.DataBind.SetBound(True, "", sItemUID)
                oEdit.ChooseFromListUID = sItemUID
                oEdit.ChooseFromListAlias = sCFLAlias
            End If

            If oLinkedObj <> 0 Then
                Dim oLink As SAPbouiCOM.LinkedButton
                oItem.LinkTo = sPosItemUID 'ID of the edit text used to idenfity the object to open
                oLink = oItem.Specific
                oLink.LinkedObject = oLinkedObj
            End If
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            AddItem = RTN_SUCCESS
        Catch exc As Exception
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            AddItem = RTN_ERROR
        Finally
            oItem = Nothing
            oPosItem = Nothing
            GC.Collect()
        End Try
    End Function

    Public Function StartTransaction(ByRef sErrDesc As String) As Long
        ' ***********************************************************************************
        '   Function   :    StartTransaction()
        '   Purpose    :    Start DI Company Transaction
        '
        '   Parameters :    ByRef sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :   Sri
        '   Date       :   29 April 2013
        '   Change     :
        ' ***********************************************************************************
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "StartTransaction()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If p_oDICompany.InTransaction Then
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Found hanging transaction.Rolling it back.", sFuncName)
                p_oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If

            p_oDICompany.StartTransaction()

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            StartTransaction = RTN_SUCCESS
        Catch exc As Exception
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            StartTransaction = RTN_ERROR
        End Try

    End Function

    Public Function RollBackTransaction(ByRef sErrDesc As String) As Long
        ' ***********************************************************************************
        '   Function   :    RollBackTransaction()
        '   Purpose    :    Roll Back DI Company Transaction
        '
        '   Parameters :    ByRef sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    Sri
        '   Date       :    29 April 2013
        '   Change     :
        ' ***********************************************************************************
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "RollBackTransaction()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If p_oDICompany.InTransaction Then
                p_oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            Else
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("No active transaction found for rollback", sFuncName)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            RollBackTransaction = RTN_SUCCESS
        Catch exc As Exception
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            RollBackTransaction = RTN_ERROR
        Finally
            GC.Collect()
        End Try

    End Function

    Public Function CommitTransaction(ByRef sErrDesc As String) As Long
        ' ***********************************************************************************
        '   Function   :    CommitTransaction()
        '   Purpose    :    Commit DI Company Transaction
        '
        '   Parameters :    ByRef sErrDesc As String
        '                       sErrDesc=Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    Sri
        '   Date       :    29 April 2013
        '   Change     :
        ' ***********************************************************************************
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "CommitTransaction()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If p_oDICompany.InTransaction Then
                p_oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
            Else
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("No active transaction found for commit", sFuncName)
            End If

            CommitTransaction = RTN_SUCCESS
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch exc As Exception
            CommitTransaction = RTN_ERROR
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        End Try

    End Function

    Public Function DisplayStatus(ByVal oFrmParent As SAPbouiCOM.Form, ByVal sMsg As String, ByRef sErrDesc As String) As Long
        ' ***********************************************************************************
        '   Function   :    DisplayStatus()
        '   Purpose    :    Display Status Message while loading 
        '
        '   Parameters :    ByVal oFrmParent As SAPbouiCOM.Form
        '                       oFrmParent = set the SAP UI Form Object
        '                   ByVal sMsg As String
        '                       sMsg = set the Display Message information
        '                   ByRef sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    Sri
        '   Date       :   29 April 2013
        '   Change     :
        ' ***********************************************************************************
        Dim oForm As SAPbouiCOM.Form
        Dim oItem As SAPbouiCOM.Item
        Dim oTxt As SAPbouiCOM.StaticText
        Dim creationPackage As SAPbouiCOM.FormCreationParams
        Dim iCount As Integer
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "DisplayStatus"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)
            'Check whether the form exists.If exists then close the form
            For iCount = 0 To p_oSBOApplication.Forms.Count - 1
                oForm = p_oSBOApplication.Forms.Item(iCount)
                If oForm.UniqueID = "dStatus" Then
                    oForm.Close()
                    Exit For
                End If
            Next iCount
            'Add Form
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Creating form Assign Department", sFuncName)
            creationPackage = p_oSBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            creationPackage.UniqueID = "dStatus"
            creationPackage.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_FixedNoTitle
            creationPackage.FormType = "AB_dStatus"
            oForm = p_oSBOApplication.Forms.AddEx(creationPackage)
            With oForm
                .AutoManaged = False
                .Width = 300
                .Height = 100
                If oFrmParent Is Nothing Then
                    .Left = (Screen.PrimaryScreen.WorkingArea.Width - oForm.Width) / 2
                    .Top = (Screen.PrimaryScreen.WorkingArea.Height - oForm.Height) / 2.5
                Else
                    .Left = ((oFrmParent.Left * 2) + oFrmParent.Width - oForm.Width) / 2
                    .Top = ((oFrmParent.Top * 2) + oFrmParent.Height - oForm.Height) / 2
                End If
            End With

            'Add Label
            oItem = oForm.Items.Add("3", SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oItem.Top = 40
            oItem.Left = 40
            oItem.Width = 250
            oTxt = oItem.Specific
            oTxt.Caption = sMsg
            oForm.Visible = True

            DisplayStatus = RTN_SUCCESS
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch exc As Exception
            DisplayStatus = RTN_ERROR
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        Finally
            creationPackage = Nothing
            oForm = Nothing
            oItem = Nothing
            oTxt = Nothing
        End Try

    End Function

    Public Function EndStatus(ByRef sErrDesc As String) As Long
        ' ***********************************************************************************
        '   Function   :    EndStatus()
        '   Purpose    :    Close Status Window
        '
        '   Parameters :    ByRef sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    Sri
        '   Date       :    29 April 2013
        '   Change     :
        ' ***********************************************************************************
        Dim oForm As SAPbouiCOM.Form
        Dim iCount As Integer
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "EndStatus()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)
            'Check whether the form is exist. If exist then close the form
            For iCount = 0 To p_oSBOApplication.Forms.Count - 1
                oForm = p_oSBOApplication.Forms.Item(iCount)
                If oForm.UniqueID = "dStatus" Then
                    oForm.Close()
                    Exit For
                End If
            Next iCount
            EndStatus = RTN_SUCCESS
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch exc As Exception
            EndStatus = RTN_ERROR
            sErrDesc = exc.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        Finally
            oForm = Nothing
        End Try

    End Function

    Public Sub ShowErr(ByVal sErrMsg As String)
        ' ***********************************************************************************
        '   Function   :    ShowErr()
        '   Purpose    :    Show Error Message
        '   Parameters :  
        '                   ByVal sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    Dev
        '   Date       :    23 Jan 2007
        '   Change     :
        ' ***********************************************************************************
        Try
            If sErrMsg <> "" Then
                If Not p_oSBOApplication Is Nothing Then
                    If p_iErrDispMethod = ERR_DISPLAY_STATUS Then

                        p_oSBOApplication.SetStatusBarMessage("Error : " & sErrMsg, SAPbouiCOM.BoMessageTime.bmt_Short)
                    ElseIf p_iErrDispMethod = ERR_DISPLAY_DIALOGUE Then
                        p_oSBOApplication.MessageBox("Error : " & sErrMsg)
                    End If
                End If
            End If
        Catch exc As Exception
            WriteToLogFile(exc.Message, "ShowErr()")
        End Try
    End Sub

    Public Sub UpdateXML(ByVal oDICompany As SAPbobsCOM.Company, ByVal oDITargetComp As SAPbobsCOM.Company, _
                             ByVal sNode As String, ByVal sTblName As String, ByVal sField1 As String, ByVal sField2 As String, _
                             ByVal bIsNumeric As Boolean, ByRef oXMLDoc As XmlDocument, ByRef sXMLFile As String)

        Dim oNode As XmlNode
        Dim sFuncName As String = String.Empty
        Dim sSQL As String = String.Empty
        Dim oRs As SAPbobsCOM.Recordset
        Dim iCode As Integer
        Dim sCode As String = String.Empty

        Try
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Updating " & sField1 & " in XML file..", sFuncName)
            oRs = oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oNode = oXMLDoc.SelectSingleNode(sNode)

            If Not IsNothing(oNode) Then
                If Not oNode.InnerText = String.Empty Then
                    If bIsNumeric Then
                        iCode = CInt(oNode.InnerText)

                        If sTblName = "OLGT" Then
                            If CInt(oNode.InnerText) = 0 Then iCode = 1
                        End If


                        sSQL = " SELECT " & sField1 & " from  [" & oDITargetComp.CompanyDB.ToString & "].[dbo]." & sTblName & _
                               " WHERE " & sField2 & " in (select " & sField2 & " from " & sTblName & " WHERE " & sField1 & "=" & iCode & ")"
                    Else
                        sCode = oNode.InnerText
                        sSQL = " SELECT " & sField1 & " from  [" & oDITargetComp.CompanyDB.ToString & "].[dbo]." & sTblName & _
                               " WHERE " & sField2 & " in (select " & sField2 & " from " & sTblName & " WHERE " & sField1 & "='" & sCode & "')"
                    End If

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Executing SQL Query" & sSQL, sFuncName)
                    oRs.DoQuery(sSQL)
                    If Not oRs.EoF Then
                        oNode.InnerText = oRs.Fields.Item(0).Value
                    Else
                        oNode.ParentNode.RemoveChild(oNode)
                        oXMLDoc.Save(sXMLFile)
                    End If
                    oXMLDoc.Save(sXMLFile)
                Else
                    oNode.ParentNode.RemoveChild(oNode)
                    oXMLDoc.Save(sXMLFile)
                End If
            End If

        Catch ex As Exception

        End Try

    End Sub

    Public Sub AddMenuItems()
        Dim oMenus As SAPbouiCOM.Menus
        Dim oMenuItem As SAPbouiCOM.MenuItem
        oMenus = p_oSBOApplication.Menus

        Dim oCreationPackage As SAPbouiCOM.MenuCreationParams
        oCreationPackage = (p_oSBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams))
        oMenuItem = p_oSBOApplication.Menus.Item("43520") 'Modules

        oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP
        oCreationPackage.UniqueID = "MB"
        oCreationPackage.String = "Mobile"
        oCreationPackage.Enabled = True
        oCreationPackage.Position = 13
        oCreationPackage.Image = System.Windows.Forms.Application.StartupPath & "\Icon.bmp"
        'oCreationPackage.Image = Application.StartupPath.ToString & "\interface.jpg"

        oMenus = oMenuItem.SubMenus
        Try
            If Not p_oSBOApplication.Menus.Exists("MB") Then
                'If the menu already exists this code will fail
                oMenus.AddEx(oCreationPackage)
            End If
        Catch
        End Try


        Try
            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("MB")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "SBS"
            oCreationPackage.String = "Stock Budget Setup"
            If Not p_oSBOApplication.Menus.Exists("SBS") Then
                oMenus.AddEx(oCreationPackage)
            End If

            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("MB")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP
            oCreationPackage.UniqueID = "SA"
            oCreationPackage.String = "ShowRound"

            If Not p_oSBOApplication.Menus.Exists("SA") Then
                oMenus.AddEx(oCreationPackage)
            End If

            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("SA")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "SAQ"
            oCreationPackage.String = "ShowRound Questions"

            If Not p_oSBOApplication.Menus.Exists("SAQ") Then
                oMenus.AddEx(oCreationPackage)
            End If


            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("SA")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "SAS"
            oCreationPackage.String = "ShowRound"

            If Not p_oSBOApplication.Menus.Exists("SAS") Then
                oMenus.AddEx(oCreationPackage)
            End If

            '================================================== SURVEY MASTER ======================
            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("MB")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP
            oCreationPackage.UniqueID = "SM"
            oCreationPackage.String = "Customer Survey"

            If Not p_oSBOApplication.Menus.Exists("SM") Then
                oMenus.AddEx(oCreationPackage)
            End If

            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("SM")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "CSQ"
            oCreationPackage.String = "Survey Questions"

            If Not p_oSBOApplication.Menus.Exists("CSQ") Then
                oMenus.AddEx(oCreationPackage)
            End If


            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("SM")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "CSH"
            oCreationPackage.String = "Customer Survey"

            If Not p_oSBOApplication.Menus.Exists("CSH") Then
                oMenus.AddEx(oCreationPackage)
            End If

            '====================================  Job Schedule   =========================================================================

            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("MB")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP
            oCreationPackage.UniqueID = "JS"
            oCreationPackage.String = "Job Scheduling"

            If Not p_oSBOApplication.Menus.Exists("JS") Then
                oMenus.AddEx(oCreationPackage)
            End If

            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("JS")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "JCT"
            oCreationPackage.String = "Services"

            If Not p_oSBOApplication.Menus.Exists("JCT") Then
                oMenus.AddEx(oCreationPackage)
            End If


            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("JS")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "JJA"
            oCreationPackage.String = "Job Area"

            If Not p_oSBOApplication.Menus.Exists("JJA") Then
                oMenus.AddEx(oCreationPackage)
            End If


            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("JS")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "JJS"
            oCreationPackage.String = "Job Schedule"

            If Not p_oSBOApplication.Menus.Exists("JJS") Then
                oMenus.AddEx(oCreationPackage)
            End If


            '====================================  Inspection QA  =========================================================================
            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("MB")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP
            oCreationPackage.UniqueID = "IQA"
            oCreationPackage.String = "Inspection/QA "

            If Not p_oSBOApplication.Menus.Exists("IQA") Then
                oMenus.AddEx(oCreationPackage)
            End If

            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("IQA")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "MST"
            oCreationPackage.String = "Market Segment Template"

            If Not p_oSBOApplication.Menus.Exists("MST") Then
                oMenus.AddEx(oCreationPackage)
            End If


            'Get the menu collection of the newly added pop-up item
            oMenuItem = p_oSBOApplication.Menus.Item("IQA")
            oMenus = oMenuItem.SubMenus

            'Create s sub menu
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
            oCreationPackage.UniqueID = "MSC"
            oCreationPackage.String = "Market Segment Checklist"

            If Not p_oSBOApplication.Menus.Exists("MSC") Then
                oMenus.AddEx(oCreationPackage)
            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Company Theme " & p_sCompanyTheme, sFuncName)

            'Get the menu collection of the newly added pop-up item
            If p_sCompanyTheme = 2 Then
                ''oMenuItem = p_oSBOApplication.Menus.Item("IQA")
                ''oMenus = oMenuItem.SubMenus

                ' ''Create s sub menu
                ''oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
                ''oCreationPackage.UniqueID = "MGT"
                ''oCreationPackage.String = "Gardener Template"

                ''If Not p_oSBOApplication.Menus.Exists("MGT") Then
                ''    oMenus.AddEx(oCreationPackage)
                ''End If

                ' ''Get the menu collection of the newly added pop-up item
                ''oMenuItem = p_oSBOApplication.Menus.Item("IQA")
                ''oMenus = oMenuItem.SubMenus

                ' ''Create s sub menu
                ''oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
                ''oCreationPackage.UniqueID = "MGC"
                ''oCreationPackage.String = "Gardener Checklist"

                ''If Not p_oSBOApplication.Menus.Exists("MGC") Then
                ''    oMenus.AddEx(oCreationPackage)
                ''End If

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Company Theme 2 Entering ", sFuncName)

                'Get the menu collection of the newly added pop-up item
                oMenuItem = p_oSBOApplication.Menus.Item("IQA")
                oMenus = oMenuItem.SubMenus
                'Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
                oCreationPackage.UniqueID = "MLT"
                oCreationPackage.String = "Landscape Template"
                If Not p_oSBOApplication.Menus.Exists("MLT") Then
                    oMenus.AddEx(oCreationPackage)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Created succesfully Landscape Template ", sFuncName)
                End If

                'Get the menu collection of the newly added pop-up item
                oMenuItem = p_oSBOApplication.Menus.Item("IQA")
                oMenus = oMenuItem.SubMenus

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Creating Landscape Checklist ", sFuncName)

                'Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
                oCreationPackage.UniqueID = "MLC"
                oCreationPackage.String = "Landscape Checklist"

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("attempting Landscape Checklist ", sFuncName)
                If Not p_oSBOApplication.Menus.Exists("MLC") Then
                    oMenus.AddEx(oCreationPackage)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Created Successfully Landscape Checklist ", sFuncName)
                End If
            End If

            '====================================  Pest Management Service   =========================================================================
            'Get the menu collection of the newly added pop-up item
            If p_sCompanyTheme = 3 Then

                oMenuItem = p_oSBOApplication.Menus.Item("IQA")
                oMenus = oMenuItem.SubMenus
                'Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
                oCreationPackage.UniqueID = "PMSM"
                oCreationPackage.String = "Type of Pests/Service"

                If Not p_oSBOApplication.Menus.Exists("PMSM") Then
                    oMenus.AddEx(oCreationPackage)
                End If

                'Get the menu collection of the newly added pop-up item
                oMenuItem = p_oSBOApplication.Menus.Item("IQA")
                oMenus = oMenuItem.SubMenus

                'Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
                oCreationPackage.UniqueID = "PMS"
                oCreationPackage.String = "Pest Management Service Report"

                If Not p_oSBOApplication.Menus.Exists("PMS") Then
                    oMenus.AddEx(oCreationPackage)
                End If
            End If
            '=====================================================================================================================

        Catch
            'Menu already exists
            p_oSBOApplication.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, True)
        End Try
    End Sub

    Public Function LoadScreenXML(ByVal FileName As String, ByVal FormType As String, ByVal FormUID As String) As SAPbouiCOM.Form
        Dim objForm As SAPbouiCOM.Form
        Dim objXML As New Xml.XmlDocument
        Dim strResource As String
        Dim objFrmCreationPrams As SAPbouiCOM.FormCreationParams


        strResource = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name & "." & FileName
        objXML.Load(System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream(strResource))
        objFrmCreationPrams = p_oSBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
        objFrmCreationPrams.FormType = FormType
        objFrmCreationPrams.UniqueID = FormUID
        objFrmCreationPrams.XmlData = objXML.InnerXml
        objForm = p_oSBOApplication.Forms.AddEx(objFrmCreationPrams)

        p_oSBOApplication.LoadBatchActions(objXML.InnerXml)

        Return objForm

    End Function

    Public Sub LoadFromXML(ByVal FileName As String, ByVal Sbo_application As SAPbouiCOM.Application)
        Try
            Dim oXmlDoc As New Xml.XmlDocument
            Dim sPath As String
            'sPath = IO.Directory.GetParent(Application.StartupPath).ToString
            sPath = Application.StartupPath.ToString
            'oXmlDoc.Load(sPath & "\AE_FleetMangement\" & FileName)
            oXmlDoc.Load(sPath & "\" & FileName)
            ' MsgBox(Application.StartupPath)

            Sbo_application.LoadBatchActions(oXmlDoc.InnerXml)
        Catch ex As Exception
            Sbo_application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    Public Sub ShowFileDialog()

        Dim oDialogBox As New OpenFileDialog
        Dim sMdbFilePath As String
        Dim sFuncName As String = String.Empty
        '' Dim oProcesses() As System.Diagnostics.Process
        Try

            sFuncName = "ShowFileDialog()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)


            Dim OpenFilePath As Browse
            OpenFilePath = New Browse
            OpenFilePath.Show()
            OpenFilePath.Visible = False
            OpenFilePath.TopMost = True
            OpenFilePath.OpenFileDialog1.Multiselect = False
            OpenFilePath.OpenFileDialog1.Title = "SAP Business One"
            OpenFilePath.OpenFileDialog1.InitialDirectory = "C:\"
            OpenFilePath.OpenFileDialog1.FileName = String.Empty

            If OpenFilePath.OpenFileDialog1.ShowDialog = DialogResult.OK Then
                sMdbFilePath = OpenFilePath.OpenFileDialog1.FileName
                p_sSelectedFilepath = OpenFilePath.OpenFileDialog1.FileName
                p_sSelectedFileName = OpenFilePath.OpenFileDialog1.SafeFileName
                OpenFilePath.OpenFileDialog1.Dispose()
                OpenFilePath.Close()

            Else
                p_sSelectedFilepath = String.Empty
                OpenFilePath.Close()
                System.Windows.Forms.Application.ExitThread()
            End If


            ' ''oDialogBox.Multiselect = False
            ' ''oDialogBox.Filter = "All files (*.*)|*.*|All files (*.*)|*.*" '"All files(*.CSV)|*.CSV"
            ' ''Dim filterindex As Integer = 0
            ' ''Try
            ' ''    filterindex = 0
            ' ''Catch ex As Exception
            ' ''End Try

            ' ''oDialogBox.FilterIndex = filterindex
            ' ''oDialogBox.InitialDirectory = "C:\" ' orset.Fields.Item("C:\").Value
            ' ''oDialogBox.RestoreDirectory = True

            ' ''oProcesses = System.Diagnostics.Process.GetProcessesByName("SAP Business One")
            '' ''If oProcesses.Length <> 0 Then
            '' ''    For i As Integer = 0 To oProcesses.Length - 1
            ' ''Dim MyWindow As New clsWindowWrapper(oProcesses(0).MainWindowHandle)
            ' ''Dim ret As DialogResult = oDialogBox.ShowDialog(MyWindow)
            ' ''If ret = DialogResult.OK Then
            ' ''    sMdbFilePath = oDialogBox.FileName
            ' ''    p_sSelectedFilepath = oDialogBox.FileName
            ' ''    p_sSelectedFileName = oDialogBox.SafeFileName
            ' ''    oDialogBox.Dispose()
            ' ''Else
            ' ''    p_sSelectedFilepath = String.Empty
            ' ''    System.Windows.Forms.Application.ExitThread()
            ' ''End If
            '    Next
            'End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch ex As Exception
            p_sSelectedFilepath = String.Empty
            sErrDesc = ex.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        Finally
        End Try
    End Sub

    Public Function fillopen() As String
        Dim sFuncName As String = String.Empty
        Dim ShowFolderBrowserThread As Threading.Thread
        Try
            sFuncName = "fillopen()"
            ' If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)
            ShowFolderBrowserThread = New Threading.Thread(AddressOf ShowFileDialog)
            If ShowFolderBrowserThread.ThreadState = System.Threading.ThreadState.Unstarted Then
                ShowFolderBrowserThread.SetApartmentState(System.Threading.ApartmentState.STA)
                ShowFolderBrowserThread.Start()
                ShowFolderBrowserThread.Join()
            ElseIf ShowFolderBrowserThread.ThreadState = System.Threading.ThreadState.Stopped Then
                ShowFolderBrowserThread.Start()
                ShowFolderBrowserThread.Join()

            End If
            While ShowFolderBrowserThread.ThreadState = Threading.ThreadState.Running
                Windows.Forms.Application.DoEvents()
            End While

            If Not String.IsNullOrEmpty(p_sSelectedFilepath) Then
                Return p_sSelectedFilepath
            End If

            ' If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch ex As Exception
            fillopen = String.Empty
            sErrDesc = ex.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        End Try
    End Function

    Function HeaderValidation(FormUID As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long
        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "HeaderValidation()"
            ' If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)

            If FormUID.Items.Item("Item_3").Specific.string = String.Empty Then
                p_oSBOApplication.StatusBar.SetText("Date is Missing", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                FormUID.ActiveItem = "Item_3"
                Return RTN_ERROR

            ElseIf FormUID.Items.Item("Item_4").Specific.string = String.Empty Then
                p_oSBOApplication.StatusBar.SetText("Remarks is Missing", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                FormUID.ActiveItem = "Item_4"
                Return RTN_ERROR

            ElseIf FormUID.Items.Item("Item_5").Specific.string = String.Empty Then
                p_oSBOApplication.StatusBar.SetText("File Path is Missing", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                FormUID.ActiveItem = "Item_5"
                Return RTN_ERROR
            End If
            HeaderValidation = RTN_SUCCESS
            '  If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)

        Catch ex As Exception
            p_oSBOApplication.StatusBar.SetText("HeadValidation Function : " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            sErrDesc = ex.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            HeaderValidation = RTN_ERROR
        End Try
        Return RTN_SUCCESS
    End Function

    Public Function GetDataViewFromCSV(ByVal CurrFileToUpload As String, ByVal Filename As String) As DataView

        ' **********************************************************************************
        '   Function    :   GetDataViewFromCSV()
        '   Purpose     :   This function will upload the data from CSV file to Dataview
        '   Parameters  :   ByRef CurrFileToUpload AS String 
        '                       CurrFileToUpload = File Name
        '   Author      :   JOHN
        '   Date        :   MAY 2014 20
        ' **********************************************************************************

        Dim da As OleDb.OleDbDataAdapter
        Dim dt As DataTable
        Dim dv As DataView
        Dim sLocalPath As String = String.Empty
        Dim sLocalFile As String = String.Empty
        Dim sFuncName As String = String.Empty

        Try
            Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.IO.Path.GetDirectoryName(CurrFileToUpload) & "\;Extended Properties=""text;HDR=NO;FMT=Delimited"""
            'Dim sConnectionString As String = "provider=Microsoft.Jet.OLEDB.4.0; " & _
            '  "data source='" & CurrFileToUpload & " '; " & "Extended Properties=Excel 8.0;"
            Dim objConn As New System.Data.OleDb.OleDbConnection(sConnectionString)

            If String.IsNullOrEmpty(p_sSelectedFilepath) Then
                Dim oForm As SAPbouiCOM.Form = p_oSBOApplication.Forms.ActiveForm
                Dim iIndex As Integer = InStrRev(oForm.Items.Item("Item_5").Specific.String, "\", -1)
                sLocalPath = Left(oForm.Items.Item("Item_5").Specific.String, iIndex)
                sLocalFile = Mid(oForm.Items.Item("Item_5").Specific.String, iIndex + 1, oForm.Items.Item("Item_5").Specific.string.ToString.Length - iIndex)
            Else
                sLocalPath = p_sSelectedFilepath.ToString.Substring(0, (p_sSelectedFilepath.Length - p_sSelectedFileName.Length))
                sLocalFile = Filename
            End If


            sFuncName = "GetDataViewFromCSV"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Create_schema() ", sFuncName)
            Create_schema(sLocalPath, sLocalFile)

            'Open Data Adapter to Read from Excel file

            da = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" & System.IO.Path.GetFileName(CurrFileToUpload) & "]", objConn)
            dt = New DataTable("JE")
            'Fill dataset using dataadapter
            da.Fill(dt)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS", sFuncName)

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Del_schema() ", sFuncName)
            Del_schema(sLocalPath)

            dv = New DataView(dt)
            Return dv

        Catch ex As Exception
            p_oSBOApplication.StatusBar.SetText(ex.Message & " -:  " & sLocalFile, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error occured while reading content of  " & ex.Message, sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
            Return Nothing
        End Try

    End Function

    Public Function GetSingleValue(ByVal sAccountCode As String, ByVal sGDC As String) As String
        Try
            Dim objRS As SAPbobsCOM.Recordset = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim sSqlString As String = String.Empty

            If sGDC = "G" Then
                sSqlString = "SELECT T0.U_BibbySGCode [Name] FROM [dbo].[@BIBBY_ACCT_MAPPING]  T0 WHERE T0.U_BibbyAFCode ='" & sAccountCode & "'"
            Else
                sSqlString = "SELECT T0.U_BibbySGCode [Name] FROM [dbo].[@BIBBY_ACCT_MAPPING]  T0 WHERE T0.U_BibbyAFCode ='" & sGDC & "'"
            End If

            objRS.DoQuery(sSqlString)
            If objRS.RecordCount > 0 Then
                Return objRS.Fields.Item(0).Value.ToString
            End If
        Catch ex As Exception
            Return ""
        End Try
        Return Nothing
    End Function

    Public Function Del_schema(ByVal csvFileFolder As String) As Long

        ' ***********************************************************************************
        '   Function   :    Del_schema()
        '   Purpose    :    This function is handles - Delete the Schema file
        '   Parameters :    ByVal csvFileFolder As String
        '                       csvFileFolder = Passing file name
        '   Author     :    JOHN
        '   Date       :    26/06/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************
        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "Del_schema()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)
            Console.WriteLine("Starting Function... " & sFuncName)

            Dim FileToDelete As String
            FileToDelete = csvFileFolder & "\\schema.ini"
            If System.IO.File.Exists(FileToDelete) = True Then
                System.IO.File.Delete(FileToDelete)
            End If
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Console.WriteLine("Completed with SUCCESS " & sFuncName)
            Del_schema = RTN_SUCCESS
        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Console.WriteLine("Completed with Error " & sFuncName)
            Del_schema = RTN_ERROR
        End Try
    End Function

    Public Function Create_schema(ByVal csvFileFolder As String, ByVal FileName As String) As Long

        ' ***********************************************************************************
        '   Function   :    Create_schema()
        '   Purpose    :    This function is handles - Create the Schema file
        '   Parameters :    ByVal csvFileFolder As String
        '                       csvFileFolder = Passing file name
        '   Author     :    JOHN
        '   Date       :    26/06/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************
        Dim sFuncName As String = String.Empty
        Try
            sFuncName = "Create_schema()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)
            Console.WriteLine("Starting Function... " & sFuncName)

            Dim csvFileName As String = FileName
            Dim fsOutput As FileStream = New FileStream(csvFileFolder & "\\schema.ini", FileMode.Create, FileAccess.Write)
            Dim srOutput As StreamWriter = New StreamWriter(fsOutput)
            Dim s1, s2, s3, s4, s5 As String
            s1 = "[" & csvFileName & "]"
            s2 = "ColNameHeader=False"
            s3 = "Format=CSVDelimited"
            s4 = "MaxScanRows=0"
            s5 = "CharacterSet=OEM"
            srOutput.WriteLine(s1.ToString() + ControlChars.Lf + s2.ToString() + ControlChars.Lf + s3.ToString() + ControlChars.Lf + s4.ToString() + ControlChars.Lf)
            srOutput.Close()
            fsOutput.Close()

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Console.WriteLine("Completed with SUCCESS " & sFuncName)
            Create_schema = RTN_SUCCESS

        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Console.WriteLine("Completed with Error " & sFuncName)
            Create_schema = RTN_ERROR
        End Try

    End Function

    Public Sub Write_TextFile_Account(ByVal sAccount() As String)
        Try
            Dim irow As Integer
            Dim sPath As String = System.Windows.Forms.Application.StartupPath & "\"
            Dim sFileName As String = "AccountCode_NotMap.txt"
            Dim sbuffer As String = String.Empty

            If File.Exists(sPath & sFileName) Then
                Try
                    File.Delete(sPath & sFileName)
                Catch ex As Exception
                End Try
            End If

            Dim sw As StreamWriter = New StreamWriter(sPath & sFileName)
            ' Add some text to the file.
            sw.WriteLine("")
            sw.WriteLine("Error!  The following AccNumbers do not have a corresponding SAP G/L Account in the mapping table! ")
            sw.WriteLine("")
            sw.WriteLine("Account Code                       ")
            sw.WriteLine("=============================================================")
            sw.WriteLine(" ")

            For irow = 0 To sAccount.Length
                If Not String.IsNullOrEmpty(sAccount(irow)) Then
                    sw.WriteLine(sAccount(irow).ToString.PadRight(40, " "c))
                Else
                    Exit For
                End If
            Next irow

            sw.WriteLine(" ")
            sw.WriteLine("===============================================================")
            sw.WriteLine("Please create an entry for each of these invalid AccNumbers.")
            sw.Close()
            Process.Start(sPath & sFileName)


        Catch ex As Exception

        End Try

    End Sub

    Public Sub Write_TextFile_ActiveAccount(ByVal sAccount() As String)
        Try
            Dim irow As Integer
            Dim sPath As String = System.Windows.Forms.Application.StartupPath & "\"
            Dim sFileName As String = "AccountCode_ExistorInactive.txt"
            Dim sbuffer As String = String.Empty

            If File.Exists(sPath & sFileName) Then
                Try
                    File.Delete(sPath & sFileName)
                Catch ex As Exception
                End Try
            End If

            Dim sw As StreamWriter = New StreamWriter(sPath & sFileName)
            ' Add some text to the file.
            sw.WriteLine("")
            sw.WriteLine("Error!The following SAP G/L Accounts are not found in the Chart of Accounts or the Account is not an Active ! ")
            sw.WriteLine("")
            sw.WriteLine("Account Code                       ")
            sw.WriteLine("=============================================================")
            sw.WriteLine(" ")

            For irow = 0 To sAccount.Length
                If Not String.IsNullOrEmpty(sAccount(irow)) Then
                    sw.WriteLine(sAccount(irow).ToString.PadRight(40, " "c))
                Else
                    Exit For
                End If
            Next irow

            sw.WriteLine(" ")
            sw.WriteLine("===============================================================")
            sw.WriteLine("Please create an entry for each of these invalid Account Numbers in Chart of Accounts or make sure these accounts are Active.")
            sw.Close()
            Process.Start(sPath & sFileName)


        Catch ex As Exception

        End Try

    End Sub

    Public Sub Write_TextFile_Amount(ByVal sAmount(,) As String)
        Try
            Dim irow As Integer
            Dim sPath As String = System.Windows.Forms.Application.StartupPath & "\"
            Dim sFileName As String = "AccountCode_NotMap.txt"
            Dim sbuffer As String = String.Empty

            If File.Exists(sPath & sFileName) Then
                Try
                    File.Delete(sPath & sFileName)
                Catch ex As Exception
                End Try
            End If

            Dim sw As StreamWriter = New StreamWriter(sPath & sFileName)
            ' Add some text to the file.
            sw.WriteLine("")
            sw.WriteLine("Error!  The Total Debit is not equal to the Total Credit for the following group(RefNo)")
            sw.WriteLine("")
            sw.WriteLine("Debit Amount                 Credit Amount                Difference                       RefNo")
            sw.WriteLine("================================================================================================")
            sw.WriteLine(" ")

            For irow = 0 To UBound(sAmount, 1) - 1
                If Not String.IsNullOrEmpty(sAmount(irow, 0)) Then
                    sw.WriteLine(sAmount(irow, 0).ToString.PadRight(30, " "c) & sAmount(irow, 1).ToString.PadRight(30, " "c) & " " & sAmount(irow, 2).ToString.PadRight(30, " "c) & " " & sAmount(irow, 3))
                Else
                    Exit For
                End If
            Next irow

            sw.WriteLine(" ")
            sw.WriteLine("================================================================================================")
            sw.WriteLine("Please check the grouping of entries in the CSV file")
            sw.Close()
            Process.Start(sPath & sFileName)

        Catch ex As Exception

        End Try

    End Sub

    Public Function ConvertStringToDate(ByRef sDate As String) As Date
        Try
            'Dim iIndex As Integer = 0
            'Dim iDay As String
            'Dim iMonth As String
            Dim sMonth() As String

            sMonth = sDate.Split("/")
            ' Year  /  Month  / Day
            Return sMonth(2) & "/" & sMonth(1).PadLeft(2, "0"c) & "/" & sMonth(0).PadLeft(2, "0"c)
        Catch ex As Exception
            p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return "1/1/1"
        End Try

    End Function

    Public Function StockBudget_Binding(ByRef oform As SAPbouiCOM.Form, ByRef oCompany As SAPbobsCOM.Company, ByRef oApplication As SAPbouiCOM.Application, ByRef sErrDesc As String) As Long

        ' ***********************************************************************************
        '   Function   :    StockBudget_Validation()
        '   Purpose    :    This function is handles - Delete the Schema file
        '   Parameters :    ByVal csvFileFolder As String
        '                       csvFileFolder = Passing file name
        '   Author     :    JOHN
        '   Date       :    26/06/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************
        Dim sFuncName As String = String.Empty
        Dim oCFLs As SAPbouiCOM.ChooseFromList
        Dim oCons As SAPbouiCOM.Conditions
        Dim oCon As SAPbouiCOM.Condition
        Dim empty As New SAPbouiCOM.Conditions

        Try
            sFuncName = "StockBudget_Validation()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)
            Dim oMatrix As SAPbouiCOM.Matrix = oform.Items.Item("Item_3").Specific
            Dim orset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            orset.DoQuery("SELECT T0.[USER_CODE], T0.[U_NAME] FROM OUSR T0 WHERE T0.[USER_CODE] = '" & oCompany.UserName & "'")
            oMatrix.Columns.Item("Col_1").Editable = False
            Dim Tmp_val As String = oform.BusinessObject.GetNextSerialNumber(NextSerialNo(oCompany, oApplication, "StockBudget"))
            oform.Items.Item("Item_13").Specific.String = Tmp_val
            oform.Items.Item("Item_5").Specific.String = PostDate(oCompany)
            oform.Items.Item("17").Specific.String = oCompany.UserName
            oform.Items.Item("18").Specific.String = orset.Fields.Item("U_NAME").Value

            oMatrix.AddRow()
            oMatrix.Columns.Item("#").Cells.Item(1).Specific.String = "1"
            oMatrix.CommonSetting.SetCellEditable(1, 1, True)
            oMatrix.CommonSetting.SetCellEditable(1, 3, True)
            oMatrix.Columns.Item("V_0").Visible = False
            oMatrix.Columns.Item("Col_11").Editable = False
            oform.PaneLevel = 1

            oCFLs = oform.ChooseFromLists.Item("Item")
            oCFLs.SetConditions(empty)
            oCons = oCFLs.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = "frozenfor"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCon.CondVal = "N"
            oCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
            oCon = oCons.Add()
            oCon.Alias = "InvntItem"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCon.CondVal = "Y"
            oCFLs.SetConditions(oCons)

            oCFLs = oform.ChooseFromLists.Item("Warehouse")
            oCFLs.SetConditions(empty)
            oCons = oCFLs.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = "Inactive"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCon.CondVal = "N"
            oCFLs.SetConditions(oCons)

            oform.State = SAPbouiCOM.BoFormStateEnum.fs_Maximized
            oform.DataBrowser.BrowseBy = "Item_13"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            StockBudget_Binding = RTN_SUCCESS
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            StockBudget_Binding = RTN_ERROR
        End Try
    End Function

    Public Function PostDate(ByRef oCompany As SAPbobsCOM.Company) As String

        Dim DateString As String = String.Empty
        Dim sSQL As String = String.Empty
        Dim oRs As SAPbobsCOM.Recordset
        Dim sDatesep As String

        oRs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        sSQL = "SELECT DateFormat,DateSep FROM OADM"

        oRs.DoQuery(sSQL)

        If Not oRs.EoF Then
            sDatesep = oRs.Fields.Item("DateSep").Value

            Select Case oRs.Fields.Item("DateFormat").Value
                Case 0

                    DateString = Format(Now.Date, "dd" & sDatesep & "MM" & sDatesep & "yy")

                Case 1
                    DateString = Format(Now.Date, "dd" & sDatesep & "MM" & sDatesep & "yyyy")

                Case 2
                    DateString = Format(Now.Date, "MM" & sDatesep & "dd" & sDatesep & "yy")
                Case 3
                    DateString = Format(Now.Date, "MM" & sDatesep & "dd" & sDatesep & "yyyy")
                Case 4
                    DateString = Format(Now.Date, "yyyy" & sDatesep & "MM" & sDatesep & "dd")
                Case 5
                    DateString = Format(Now.Date, "dd" & sDatesep & "MMMM" & sDatesep & "yyyy")
                Case 6
                    DateString = Format(Now.Date, "yy" & sDatesep & "MM" & sDatesep & "dd")
            End Select

        End If

        Return DateString

    End Function

    Public Function NextSerialNo(ByRef oCompany As SAPbobsCOM.Company, ByRef oApplication As SAPbouiCOM.Application, ByVal ObjectCode As String) As String
        Try

            Dim orset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            orset.DoQuery("SELECT T0.Series FROM NNM1 T0 where  T0.ObjectCode = '" & ObjectCode & "'")
            Return orset.Fields.Item("Series").Value

        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return -1
        End Try
    End Function

    Public Function MatrixDataToDataTable(ByRef oMatrix As SAPbouiCOM.Matrix, ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As DataTable

        Dim oDocDatatable As New DataTable
        Try

            sFuncName = "MatrixDataToDataTable()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)

            oDocDatatable.Columns.Add("LineNum", GetType(Integer))
            oDocDatatable.Columns.Add("Projects", GetType(String))
            oDocDatatable.Columns.Add("ItemCode", GetType(String))
            oDocDatatable.Columns.Add("Month", GetType(String))
            oDocDatatable.Columns.Add("Warehouse", GetType(String))
            oDocDatatable.Columns.Add("Description", GetType(String))

            For imjs As Integer = 1 To oMatrix.VisualRowCount
                If Not String.IsNullOrEmpty(oMatrix.Columns.Item("Col_0").Cells.Item(imjs).Specific.String) Then
                    oDocDatatable.Rows.Add(imjs, oForm.Items.Item("Item_14").Specific.String, _
                                         oMatrix.Columns.Item("Col_0").Cells.Item(imjs).Specific.String, _
                                          oMatrix.Columns.Item("Col_2").Cells.Item(imjs).Specific.String, _
                                           oMatrix.Columns.Item("Col_3").Cells.Item(imjs).Specific.String, _
                                           oMatrix.Columns.Item("Col_1").Cells.Item(imjs).Specific.String)
                End If
            Next imjs

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            MatrixDataToDataTable = oDocDatatable

        Catch ex As Exception
            sErrDesc = ex.Message
            p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            ''sErrDesc = RTN_ERROR
        End Try
    End Function

    Public Function ProjectValidation(ByVal oDocDatatable As DataTable, ByRef oCompany As SAPbobsCOM.Company, ByVal sDocEntry As String, ByRef sErrDesc As String) As DataTable

        Dim oDVFillter As DataView = Nothing
        Dim oDVFillter_Table As DataView = Nothing
        Dim oDVDuplication_Check As DataView = Nothing
        Dim oResultDT As DataTable = New DataTable
        Dim oDTDisplay As DataTable = New DataTable
        Dim oRowCount As Integer = 0

        Dim orset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Try
            sFuncName = "ProjectValidation()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)

            oDTDisplay.Columns.Add("LineNum", GetType(Integer))
            oDTDisplay.Columns.Add("ItemCode", GetType(String))
            oDTDisplay.Columns.Add("Description", GetType(String))
            oDTDisplay.Columns.Add("DocEntry", GetType(String))
            oDTDisplay.Columns.Add("Line", GetType(String))


            Dim sSQL As String = "SELECT T0.[U_OcrCode] [Projects], T1.[U_ItemCode] [ItemCode], T1.[U_IDesc] [ItemDescription], " & _
                "T1.[U_BgtMonth] [Month], T0.[DocNum] [DocEntry], T1.[LineId] FROM [dbo].[@AB_STKBGT]  T0 inner join  [dbo].[@AB_STKBGT1]  T1 " & _
                "on T0.DocEntry = T1.DocEntry WHERE T0.[U_OcrCode]  = '" & oDocDatatable.Rows(0).Item("Projects").ToString & "' and T1.[U_ItemCode] <> '' and T1.DocEntry not in ('" & sDocEntry & "')"
            oDVFillter = oDocDatatable.DefaultView
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("SQL " & sSQL, sFuncName)
            orset.DoQuery(sSQL)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling  ConvertRecordset() ", sFuncName)
            oResultDT = ConvertRecordset(orset, sErrDesc)
            If Not String.IsNullOrEmpty(sErrDesc) Then
                sErrDesc = "Error"
                Exit Function
            End If
            oDVFillter_Table = oResultDT.DefaultView
            For imjs As Integer = 0 To oDocDatatable.Rows.Count - 1
                oDVFillter.RowFilter = "Projects='" & oDocDatatable.Rows(imjs).Item("Projects").ToString & "' and ItemCode='" & oDocDatatable.Rows(imjs).Item("ItemCode").ToString & "' and Month='" & oDocDatatable.Rows(imjs).Item("Month").ToString & "'"
                If oDVFillter.Count = 1 Then
                    If oDVFillter_Table.Count > 0 Then
                        ''MsgBox(oDVFillter.Item(0).Row(0).ToString)
                        ''MsgBox(oDVFillter.Item(0).Row("Projects").ToString) ' & "   " & oDVFillter.Item("ItemCode").Row(0).ToString & "  " & oDVFillter.Item("Month").Row(0).ToString)
                        oDVFillter_Table.RowFilter = "Projects='" & oDVFillter.Item(0).Row("Projects").ToString & "' and ItemCode='" & oDVFillter.Item(0).Row("ItemCode").ToString & "' and Month='" & oDVFillter.Item(0).Row("Month").ToString & "'"
                        If oDVFillter_Table.Count > 0 Then
                            For Each drv As DataRowView In oDVFillter_Table
                                oDTDisplay.Rows.Add(oDVFillter.Item(0).Row("LineNum").ToString, drv(1).ToString.Trim, _
                                    drv(2).ToString.Trim, drv(4).ToString.Trim, drv(5).ToString.Trim)
                            Next
                        End If

                        oDVFillter_Table.RowFilter = Nothing

                    End If
                Else
                    oDVDuplication_Check = oDTDisplay.DefaultView
                    For Each drv As DataRowView In oDVFillter
                        oDVDuplication_Check.RowFilter = "LineNum='" & drv(0).ToString.Trim & "' and ItemCode='" & drv(2).ToString.Trim & "' and Description='" & drv(5).ToString.Trim & "'"
                        'oDVDuplication_Check.RowFilter = "LineNum='"& drv(0).ToString.Trim &"' and ItemCode='"& drv(1).ToString.Trim &"' and Description='"& drv(5).ToString.Trim &"' and DocEntry='"& &"' and Line='"&  &"' "
                        If oDVDuplication_Check.Count = 0 Then
                            oDTDisplay.Rows.Add(drv(0).ToString.Trim, drv(2).ToString.Trim, _
                                                       drv(5).ToString.Trim, "", "")
                        End If
                    Next
                End If
            Next imjs

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            ProjectValidation = oDTDisplay

        Catch ex As Exception
            sErrDesc = ex.Message
            p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        End Try
    End Function

    Public Function StockBudget_Validation(ByRef oForm As SAPbouiCOM.Form, ByRef sErrDesc As String) As Long

        Dim oMatrix As SAPbouiCOM.Matrix = Nothing
        Dim irow As Integer = 0

        Try
            oMatrix = oForm.Items.Item("Item_3").Specific

            If String.IsNullOrEmpty(oForm.Items.Item("Item_14").Specific.String) Then
                oForm.Items.Item("Item_14").Specific.active = True
                p_oSBOApplication.StatusBar.SetText("Project cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                StockBudget_Validation = RTN_ERROR
                Exit Function
            End If

            If p_sCompanyTheme = 1 Then
                If String.IsNullOrEmpty(oForm.Items.Item("Item_17").Specific.String) Then
                    oForm.Items.Item("Item_17").Specific.active = True
                    p_oSBOApplication.StatusBar.SetText("Profit Center cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    StockBudget_Validation = RTN_ERROR
                    Exit Function
                End If
            End If


            If oMatrix.RowCount = 1 Then
                If String.IsNullOrEmpty(oMatrix.Columns.Item("Col_0").Cells.Item(1).Specific.String) Then
                    ' oEdit = oMatrix.Columns.Item("Col_0").Cells.Item(1).Specific
                    'oEdit.Active = True
                    p_oSBOApplication.StatusBar.SetText("Item Code cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    StockBudget_Validation = RTN_ERROR
                    Exit Function
                End If
                If String.IsNullOrEmpty(oMatrix.Columns.Item("Col_2").Cells.Item(1).Specific.String) Then
                    ' oMatrix.Columns.Item("Col_2").Cells.Item(1).Specific.active = True
                    p_oSBOApplication.StatusBar.SetText("Budget Month cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    StockBudget_Validation = RTN_ERROR
                    Exit Function
                End If
                If String.IsNullOrEmpty(oMatrix.Columns.Item("Col_3").Cells.Item(1).Specific.String) Then
                    ' oMatrix.Columns.Item("Col_3").Cells.Item(1).Specific.active = True
                    p_oSBOApplication.StatusBar.SetText("Warehouse cannot be Empty ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    StockBudget_Validation = RTN_ERROR
                    Exit Function
                End If

                If CDbl(oMatrix.Columns.Item("Col_7").Cells.Item(1).Specific.String) > CDbl(oMatrix.Columns.Item("Col_4").Cells.Item(1).Specific.String) Then
                    oMatrix.Columns.Item("Col_4").Cells.Item(1).Specific.active = True
                    p_oSBOApplication.StatusBar.SetText("Budget Qty cannot be less than PR Qty.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    StockBudget_Validation = RTN_ERROR
                    Exit Function
                Else
                    oMatrix.Columns.Item("Col_9").Cells.Item(1).Specific.String = CDbl(oMatrix.Columns.Item("Col_4").Cells.Item(1).Specific.String) - CDbl(oMatrix.Columns.Item("Col_7").Cells.Item(1).Specific.String)
                End If
                oMatrix.Columns.Item("Col_6").Cells.Item(1).Specific.String = CDbl(oMatrix.Columns.Item("Col_4").Cells.Item(1).Specific.String) * CDbl(oMatrix.Columns.Item("Col_5").Cells.Item(1).Specific.String)

            Else

                If Not String.IsNullOrEmpty(oMatrix.Columns.Item("Col_0").Cells.Item(oMatrix.RowCount).Specific.string) Or _
                    Not String.IsNullOrEmpty(oMatrix.Columns.Item("Col_2").Cells.Item(oMatrix.RowCount).Specific.string) Or _
                    Not String.IsNullOrEmpty(oMatrix.Columns.Item("Col_3").Cells.Item(oMatrix.RowCount).Specific.string) Or _
                     oMatrix.Columns.Item("Col_4").Cells.Item(oMatrix.RowCount).Specific.string > 0 Then

                    irow = oMatrix.RowCount
                Else
                    irow = oMatrix.RowCount - 1
                End If

                For imjs As Integer = 1 To irow
                    If String.IsNullOrEmpty(oMatrix.Columns.Item("Col_0").Cells.Item(imjs).Specific.String) Then
                        ' oMatrix.Columns.Item("Col_0").Cells.Item(imjs).Specific.active = True
                        p_oSBOApplication.StatusBar.SetText("Item Code cannot be Empty in Line " & imjs, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        StockBudget_Validation = RTN_ERROR
                        Exit Function
                    End If
                    If String.IsNullOrEmpty(oMatrix.Columns.Item("Col_2").Cells.Item(imjs).Specific.String) Then
                        ' oMatrix.Columns.Item("Col_2").Cells.Item(imjs).Specific.active = True
                        p_oSBOApplication.StatusBar.SetText("Budget Month cannot be Empty in Line " & imjs, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        StockBudget_Validation = RTN_ERROR
                        Exit Function
                    End If
                    If String.IsNullOrEmpty(oMatrix.Columns.Item("Col_3").Cells.Item(imjs).Specific.String) Then
                        'oMatrix.Columns.Item("Col_3").Cells.Item(imjs).Specific.active = True
                        p_oSBOApplication.StatusBar.SetText("Warehouse cannot be Empty in Line " & imjs, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        StockBudget_Validation = RTN_ERROR
                        Exit Function
                    End If

                    If CDbl(oMatrix.Columns.Item("Col_7").Cells.Item(imjs).Specific.String) > CDbl(oMatrix.Columns.Item("Col_4").Cells.Item(imjs).Specific.String) Then
                        oMatrix.Columns.Item("Col_4").Cells.Item(imjs).Specific.active = True
                        p_oSBOApplication.StatusBar.SetText("Budget Qty cannot be less than PR Qty.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        StockBudget_Validation = RTN_ERROR
                        Exit Function
                    Else
                        oMatrix.Columns.Item("Col_9").Cells.Item(imjs).Specific.String = CDbl(oMatrix.Columns.Item("Col_4").Cells.Item(imjs).Specific.String) - CDbl(oMatrix.Columns.Item("Col_7").Cells.Item(imjs).Specific.String)

                    End If
                    oMatrix.Columns.Item("Col_6").Cells.Item(imjs).Specific.String = CDbl(oMatrix.Columns.Item("Col_4").Cells.Item(imjs).Specific.String) * CDbl(oMatrix.Columns.Item("Col_5").Cells.Item(imjs).Specific.String)
                Next imjs
            End If
            StockBudget_Validation = RTN_SUCCESS

        Catch ex As Exception
            p_oSBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            StockBudget_Validation = RTN_ERROR
        End Try



    End Function

    Public Function ConvertRecordset(ByVal SAPRecordset As SAPbobsCOM.Recordset, ByRef sErrDesc As String) As DataTable

        '\ This function will take an SAP recordset from the SAPbobsCOM library and convert it to a more
        '\ easily used ADO.NET datatable which can be used for data binding much easier.

        Dim dtTable As New DataTable
        Dim NewCol As DataColumn
        Dim NewRow As DataRow
        Dim ColCount As Integer

        Try

            For ColCount = 0 To SAPRecordset.Fields.Count - 1
                NewCol = New DataColumn(SAPRecordset.Fields.Item(ColCount).Name)
                dtTable.Columns.Add(NewCol)
            Next

            Do Until SAPRecordset.EoF

                NewRow = dtTable.NewRow
                'populate each column in the row we're creating
                For ColCount = 0 To SAPRecordset.Fields.Count - 1

                    NewRow.Item(SAPRecordset.Fields.Item(ColCount).Name) = SAPRecordset.Fields.Item(ColCount).Value

                Next

                'Add the row to the datatable
                dtTable.Rows.Add(NewRow)


                SAPRecordset.MoveNext()
            Loop

            Return dtTable

        Catch ex As Exception
            sErrDesc = ex.Message
            p_oSBOApplication.StatusBar.SetText(ex.ToString & Chr(10) & "Error converting SAP Recordset to DataTable", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

            Exit Function
        End Try


    End Function

    Public Function ConvertRecordsetII(ByVal SAPRecordset As SAPbobsCOM.Recordset, ByRef sErrDesc As String) As Long '' As DataTable

        '\ This function will take an SAP recordset from the SAPbobsCOM library and convert it to a more
        '\ easily used ADO.NET datatable which can be used for data binding much easier.


        Dim NewCol As DataColumn
        Dim NewRow As DataRow
        Dim ColCount As Integer

        Try
            If p_dtTable.Columns.Count = 0 Then
                For ColCount = 0 To SAPRecordset.Fields.Count - 1
                    NewCol = New DataColumn(SAPRecordset.Fields.Item(ColCount).Name)
                    p_dtTable.Columns.Add(NewCol)
                Next
                p_dtTable.Columns.Add("Day")
            End If


            Do Until SAPRecordset.EoF

                NewRow = p_dtTable.NewRow
                'populate each column in the row we're creating
                For ColCount = 0 To SAPRecordset.Fields.Count - 1

                    NewRow.Item(SAPRecordset.Fields.Item(ColCount).Name) = SAPRecordset.Fields.Item(ColCount).Value
                    NewRow.Item("Day") = Right(SAPRecordset.Fields.Item(ColCount).Value, 2)

                Next

                'Add the row to the datatable
                p_dtTable.Rows.Add(NewRow)


                SAPRecordset.MoveNext()
            Loop

            Return RTN_SUCCESS '' p_dtTable

        Catch ex As Exception
            sErrDesc = ex.Message
            p_oSBOApplication.StatusBar.SetText(ex.ToString & Chr(10) & "Error converting SAP Recordset to DataTable", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return RTN_ERROR
            Exit Function
        End Try


    End Function

    Public Function ConvertRecordsetII(ByVal SAPRecordset As SAPbobsCOM.Recordset, ByVal srowno As String, ByRef sErrDesc As String) As Long '' As DataTable

        '\ This function will take an SAP recordset from the SAPbobsCOM library and convert it to a more
        '\ easily used ADO.NET datatable which can be used for data binding much easier.


        Dim NewCol As DataColumn
        Dim NewRow As DataRow
        Dim ColCount As Integer

        Try
            If p_dtTable.Columns.Count = 0 Then
                For ColCount = 0 To SAPRecordset.Fields.Count - 1
                    NewCol = New DataColumn(SAPRecordset.Fields.Item(ColCount).Name)
                    p_dtTable.Columns.Add(NewCol)
                Next
                p_dtTable.Columns.Add("Day")
                p_dtTable.Columns.Add("RowNo")
            End If


            Do Until SAPRecordset.EoF

                NewRow = p_dtTable.NewRow
                'populate each column in the row we're creating
                For ColCount = 0 To SAPRecordset.Fields.Count - 1

                    NewRow.Item(SAPRecordset.Fields.Item(ColCount).Name) = SAPRecordset.Fields.Item(ColCount).Value
                    NewRow.Item("Day") = Right(SAPRecordset.Fields.Item(ColCount).Value, 2)
                    NewRow.Item("RowNo") = srowno
                Next
                'Add the row to the datatable
                p_dtTable.Rows.Add(NewRow)
                SAPRecordset.MoveNext()
            Loop

            Return RTN_SUCCESS '' p_dtTable

        Catch ex As Exception
            sErrDesc = ex.Message
            p_oSBOApplication.StatusBar.SetText(ex.ToString & Chr(10) & "Error converting SAP Recordset to DataTable", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return RTN_ERROR
            Exit Function
        End Try


    End Function

    Public Function Write_TextFile(ByVal oDTDisplay As DataTable, ByRef sErrDesc As String) As Long
        Try
            Dim sFuncName As String = String.Empty
            Dim irow As Integer
            Dim sPath As String = System.Windows.Forms.Application.StartupPath & "\"
            Dim sFileName As String = "Validation.txt"
            Dim sbuffer As String = String.Empty

            sFuncName = "Write_TextFile()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)

            If File.Exists(sPath & sFileName) Then
                Try
                    File.Delete(sPath & sFileName)
                Catch ex As Exception
                End Try
            End If

            Dim sw As StreamWriter = New StreamWriter(sPath & sFileName)
            ' Add some text to the file.
            sw.WriteLine("")
            sw.WriteLine("Validation Error!  The following Items are Duplicated / Existed Already  ")
            sw.WriteLine("")
            sw.WriteLine("Line No.  Item Code        Item Description                                       Doc Number       Doc Line No.     ")
            sw.WriteLine("====================================================================================================================")
            sw.WriteLine(" ")

            For irow = 0 To oDTDisplay.Rows.Count - 1
                If Not String.IsNullOrEmpty(oDTDisplay.Rows(irow).Item(0).ToString) Then
                    sw.WriteLine(oDTDisplay.Rows(irow).Item(0).ToString.PadRight(10, " "c) + oDTDisplay.Rows(irow).Item(1).ToString.PadRight(17, " "c) _
                                 + oDTDisplay.Rows(irow).Item(2).ToString.PadRight(57, " "c) + oDTDisplay.Rows(irow).Item(3).ToString.PadRight(15, " "c) _
                                 + oDTDisplay.Rows(irow).Item(4).ToString.PadRight(15, " "c))
                Else
                    Exit For
                End If
            Next irow

            sw.WriteLine(" ")
            sw.WriteLine("====================================================================================================================")
            sw.WriteLine("Please Check.")
            sw.Close()
            Process.Start(sPath & sFileName)

            Write_TextFile = RTN_SUCCESS
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS ", sFuncName)

        Catch ex As Exception
            Write_TextFile = RTN_ERROR
            sErrDesc = ex.Message
            Call WriteToLogFile(sErrDesc, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
        End Try

    End Function

    Public Function GetDate(ByVal sDate As String, ByRef oCompany As SAPbobsCOM.Company) As String

        Dim dateValue As DateTime
        Dim DateString As String = String.Empty
        Dim sSQL As String = String.Empty
        Dim oRs As SAPbobsCOM.Recordset
        Dim sDatesep As String



        oRs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        sSQL = "SELECT DateFormat,DateSep FROM OADM"

        oRs.DoQuery(sSQL)

        If Not oRs.EoF Then
            sDatesep = oRs.Fields.Item("DateSep").Value

            Select Case oRs.Fields.Item("DateFormat").Value
                Case 0
                    If Date.TryParseExact(sDate, "dd" & sDatesep & "MM" & sDatesep & "yy", _
                       New CultureInfo("en-US"), _
                       DateTimeStyles.None, _
                       dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")

                    ElseIf Date.TryParseExact(sDate, "yyyy" & sDatesep & "MM" & sDatesep & "dd", _
                   New CultureInfo("en-US"), _
                   DateTimeStyles.None, _
                   dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")

                    ElseIf Date.TryParseExact(sDate, "dd" & "MM" & "yy", _
                       New CultureInfo("en-US"), _
                       DateTimeStyles.None, _
                       dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")

                    End If
                Case 1
                    If Date.TryParseExact(sDate, "dd" & sDatesep & "MM" & sDatesep & "yyyy", _
                       New CultureInfo("en-US"), _
                       DateTimeStyles.None, _
                       dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")

                    ElseIf Date.TryParseExact(sDate, "yyyy" & sDatesep & "MM" & sDatesep & "dd", _
                   New CultureInfo("en-US"), _
                   DateTimeStyles.None, _
                   dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")

                    ElseIf Date.TryParseExact(sDate, "yyyy" & "MM" & "dd", _
                  New CultureInfo("en-US"), _
                  DateTimeStyles.None, _
                  dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    ElseIf Date.TryParseExact(sDate, "yy" & "MM" & "dd", _
            New CultureInfo("en-US"), _
            DateTimeStyles.None, _
            dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 2
                    If Date.TryParseExact(sDate, "MM" & sDatesep & "dd" & sDatesep & "yy", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 3
                    If Date.TryParseExact(sDate, "MM" & sDatesep & "dd" & sDatesep & "yyyy", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 4
                    If Date.TryParseExact(sDate, "yyyy" & sDatesep & "MM" & sDatesep & "dd", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 5
                    If Date.TryParseExact(sDate, "dd" & sDatesep & "MMMM" & sDatesep & "yyyy", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case 6
                    If Date.TryParseExact(sDate, "yy" & sDatesep & "MM" & sDatesep & "dd", _
                        New CultureInfo("en-US"), _
                        DateTimeStyles.None, _
                        dateValue) Then
                        DateString = dateValue.ToString("yyyyMMdd")
                    End If
                Case Else
                    DateString = dateValue.ToString("yyyyMMdd")
            End Select

        End If

        Return DateString

    End Function

    Public Function GetDateTimeValue(ByVal DateString As String) As DateTime
        Dim objBridge As SAPbobsCOM.SBObob
        objBridge = p_oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        Return objBridge.Format_StringToDate(DateString).Fields.Item(0).Value
    End Function

End Module
