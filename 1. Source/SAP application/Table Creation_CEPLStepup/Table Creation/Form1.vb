Public Class Form1

    Inherits System.Windows.Forms.Form

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub


    Public oCompany As New SAPbobsCOM.Company
    Private WithEvents SBO_Application As SAPbouiCOM.Application

    ' Error handling variables
    Public sErrMsg As String
    Public lErrCode As Integer
    Public lRetCode As Integer

    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click

        oCompany.Server = txtServer.Text
        oCompany.CompanyDB = TextBox1.Text
        oCompany.UserName = txtSBOUser.Text
        oCompany.Password = txtSBOPass.Text


        If cmbDBType.SelectedItem = "MSSQL2005" Then
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005
        ElseIf cmbDBType.SelectedItem = "MSSQL2008" Then
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
        ElseIf cmbDBType.SelectedItem = "MSSQL2012" Then
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
        End If


        ''oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English

        ''oCompany.UseTrusted = False

        oCompany.DbUserName = txtUser.Text
        oCompany.DbPassword = txtPass.Text


        lRetCode = oCompany.Connect

        If lRetCode <> 0 Then
            oCompany.GetLastError(lErrCode, sErrMsg)
            Label3.Text = sErrMsg
        Else
            Label3.Text = "Connected to " & oCompany.CompanyName
        End If

    End Sub


    Private Sub AddUserTable(ByVal Name As String, ByVal Description As String, _
       ByVal Type As SAPbobsCOM.BoUTBTableType)

        Try

            '//****************************************************************************
            '// The UserTablesMD represents a meta-data object which allows us
            '// to add\remove tables, change a table name etc.
            '//****************************************************************************

            Dim oUserTablesMD As SAPbobsCOM.UserTablesMD

            '//****************************************************************************
            '// In any meta-data operation there should be no other object "alive"
            '// but the meta-data object, otherwise the operation will fail.
            '// This restriction is intended to prevent a collisions
            '//****************************************************************************

            '// the meta-data object needs to be initialized with a
            '// regular UserTables object
            oUserTablesMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables)

            '//**************************************************
            '// when adding user tables or fields to the SBO DB
            '// use a prefix identifying your partner name space
            '// this will prevent collisions between different
            '// partners add-ons
            '//
            '// SAP's name space prefix is "BE_"
            '//**************************************************		

            '// set the table parameters
            oUserTablesMD.TableName = Name
            oUserTablesMD.TableDescription = Description
            oUserTablesMD.TableType = Type

            '// Add the table
            '// This action add an empty table with 2 default fields
            '// 'Code' and 'Name' which serve as the key
            '// in order to add your own User Fields
            '// see the AddUserFields.frm in this project
            '// a privat, user defined, key may be added
            '// see AddPrivateKey.frm in this project

            lRetCode = oUserTablesMD.Add
            '// check for errors in the process
            If lRetCode <> 0 Then
                If lRetCode = -1 Then
                Else
                    oCompany.GetLastError(lRetCode, sErrMsg)
                    Label3.Text = sErrMsg & oUserTablesMD.TableName
                End If
            Else
                Label3.Text = "Table: " & oUserTablesMD.TableName & " was added successfully"
            End If

            oUserTablesMD = Nothing

            GC.Collect() 'Release the handle to the table


        Catch ex As Exception

        End Try

    End Sub

    Private Sub Add_OUBudget_Fields()



        Dim oUserFieldsMD As SAPbobsCOM.UserFieldsMD
        oUserFieldsMD = Nothing
        GC.Collect()
        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        '************************************
        ' Adding "Name" field
        '************************************
        '// Setting the Field's properties

        oUserFieldsMD.TableName = "@AB_JOBSCH2"
        oUserFieldsMD.Name = "SupervisorSign"
        oUserFieldsMD.Description = "Supervisor Sign"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Memo
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Image
        '' oUserFieldsMD.EditSize = 150

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If
        '************************************
        ' Adding "Room" field
        '************************************
        '// Setting the Field's properties

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_JOBSCH2"
        oUserFieldsMD.Name = "ClientSign"
        oUserFieldsMD.Description = "ClientSign"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Memo
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Image

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If




        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_JOBSCH2"
        oUserFieldsMD.Name = "SupvrSignText"
        oUserFieldsMD.Description = "SupvrSignText"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Address

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If



        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_JOBSCH2"
        oUserFieldsMD.Name = "SupvrSignText"
        oUserFieldsMD.Description = "SupvrSignText"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Address

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_JOBSCH3"
        oUserFieldsMD.Name = "ScheduleDate"
        oUserFieldsMD.Description = "ScheduleDate"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Date
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_None

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_PESTRPT"
        oUserFieldsMD.Name = "SupvrSignText"
        oUserFieldsMD.Description = "SupvrSignText"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Address

        ''oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        'AE_Commision

        oUserFieldsMD = Nothing
        GC.Collect()


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_PESTRPT"
        oUserFieldsMD.Name = "ClintSignText"
        oUserFieldsMD.Description = "ClintSignText"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Address

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

         oUserFieldsMD.TableName = "@AB_PESTRPT"
        oUserFieldsMD.Name = "EmailId"
        oUserFieldsMD.Description = "EmailId"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        ''oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Address
        oUserFieldsMD.EditSize = 254
        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "OCRD"
        oUserFieldsMD.Name = "AB_Scope"
        oUserFieldsMD.Description = "Scope of Work"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        '' oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 254

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_STKBGT1"
        oUserFieldsMD.Name = "UOM"
        oUserFieldsMD.Description = "UOM"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        '' oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = Nothing
        GC.Collect()



        GC.Collect() 'Release the handle to the User Fields
    End Sub

    Private Sub Add_ProjectBudget_Fields()

        Dim oUserFieldsMD As SAPbobsCOM.UserFieldsMD
        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        '************************************
        ' Adding "Name" field
        '************************************
        '// Setting the Field's properties

        oUserFieldsMD.TableName = "@AB_PROJECTBUDGET"
        oUserFieldsMD.Name = "BudName"
        oUserFieldsMD.Description = "Budget Name"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 150

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If
        '************************************
        ' Adding "Room" field
        '************************************
        '// Setting the Field's properties

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_PROJECTBUDGET"
        oUserFieldsMD.Name = "Period"
        oUserFieldsMD.Description = "Period"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 50

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_PROJECTBUDGET"
        oUserFieldsMD.Name = "Account"
        oUserFieldsMD.Description = "GL Account"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 35

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If



        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_PROJECTBUDGET"
        oUserFieldsMD.Name = "Division"
        oUserFieldsMD.Description = "Division"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_PROJECTBUDGET"
        oUserFieldsMD.Name = "BudAmount"
        oUserFieldsMD.Description = "Budget Amount"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_PROJECTBUDGET"
        oUserFieldsMD.Name = "PrjCode"
        oUserFieldsMD.Description = "Project Code"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_PROJECTBUDGET"
        oUserFieldsMD.Name = "BUCode"
        oUserFieldsMD.Description = "BU Code"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        'AE_Commision

        oUserFieldsMD = Nothing
        GC.Collect()


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_PROJECTBUDGET"
        oUserFieldsMD.Name = "BalAmount"
        oUserFieldsMD.Description = "Balance Amount"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = Nothing
        GC.Collect()



        GC.Collect() 'Release the handle to the User Fields
    End Sub

    Private Sub Add_ConsolidateBudget_Fields()

        Dim oUserFieldsMD As SAPbobsCOM.UserFieldsMD
        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        '************************************
        ' Adding "Name" field
        '************************************
        '// Setting the Field's properties

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "BudName"
        oUserFieldsMD.Description = "Budget Name"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 150

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If
        '************************************
        ' Adding "Room" field
        '************************************
        '// Setting the Field's properties

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "Period"
        oUserFieldsMD.Description = "Period"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 50

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "Account"
        oUserFieldsMD.Description = "GL Account"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 35

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If



        oUserFieldsMD = Nothing
        GC.Collect()


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "BudAmount"
        oUserFieldsMD.Description = "Budget Amount"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "OUCode"
        oUserFieldsMD.Description = "OU Code"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        'AE_Commision

        oUserFieldsMD = Nothing
        GC.Collect()


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "BUCode"
        oUserFieldsMD.Description = "BU Code"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        'AE_Commision

        oUserFieldsMD = Nothing
        GC.Collect()


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "PrjCode"
        oUserFieldsMD.Description = "Project Code"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        'AE_Commision

        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "BuAmount"
        oUserFieldsMD.Description = "Bu Amount"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "PrjAmount"
        oUserFieldsMD.Description = "Project Amount"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = Nothing
        GC.Collect()


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "CumAmount"
        oUserFieldsMD.Description = "Cumulative Amount"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = Nothing
        GC.Collect()


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_CONSOLBUDGET"
        oUserFieldsMD.Name = "BalAmount"
        oUserFieldsMD.Description = "Balance Amount"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If



        oUserFieldsMD = Nothing

        GC.Collect() 'Release the handle to the User Fields
    End Sub

    Private Sub Add_BUBudget_Fields()

        Dim oUserFieldsMD As SAPbobsCOM.UserFieldsMD
        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        '************************************
        ' Adding "Name" field
        '************************************
        '// Setting the Field's properties

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "BudName"
        oUserFieldsMD.Description = "Budget Name"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 150

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If
        '************************************
        ' Adding "Room" field
        '************************************
        '// Setting the Field's properties

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Period"
        oUserFieldsMD.Description = "Period"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 50

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Account"
        oUserFieldsMD.Description = "GL Account"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 35

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If



        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Division"
        oUserFieldsMD.Description = "Division"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "BudAmount"
        oUserFieldsMD.Description = "Budget Amount"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "OUCode"
        oUserFieldsMD.Description = "OU Code"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        'AE_Commision

        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "BUCode"
        oUserFieldsMD.Description = "BU Code"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        'AE_Commision

        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "OUCode"
        oUserFieldsMD.Description = "OU Code"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha
        oUserFieldsMD.EditSize = 20

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = Nothing
        GC.Collect()

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "sAmount"
        oUserFieldsMD.Description = "Split Amount"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = Nothing
        GC.Collect()


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month1"
        oUserFieldsMD.Description = "July"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month2"
        oUserFieldsMD.Description = "August"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month3"
        oUserFieldsMD.Description = "September"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month4"
        oUserFieldsMD.Description = "October"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month5"
        oUserFieldsMD.Description = "November"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month6"
        oUserFieldsMD.Description = "December"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month7"
        oUserFieldsMD.Description = "January"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month8"
        oUserFieldsMD.Description = "February"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month9"
        oUserFieldsMD.Description = "March"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month10"
        oUserFieldsMD.Description = "April"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month11"
        oUserFieldsMD.Description = "May"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If


        oUserFieldsMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

        oUserFieldsMD.TableName = "@AB_BUBUDGET"
        oUserFieldsMD.Name = "Month12"
        oUserFieldsMD.Description = "June"
        oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Float
        oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Price
        oUserFieldsMD.EditSize = 15

        '// Adding the Field to the Table
        lRetCode = oUserFieldsMD.Add

        '// Check for errors
        If lRetCode <> 0 Then
            If lRetCode = -1 Then
            Else
                oCompany.GetLastError(lRetCode, sErrMsg)
                Label3.Text = sErrMsg & oUserFieldsMD.TableName & "  " & oUserFieldsMD.Name
            End If
        Else
            Label3.Text = "Field: '" & oUserFieldsMD.Name & "' was added successfuly to " & oUserFieldsMD.TableName & " Table"
        End If

        oUserFieldsMD = Nothing
        GC.Collect()



        GC.Collect() 'Release the handle to the User Fields
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        ''AddUserTable("AB_BUBUDGET", "BU Budget Table", SAPbobsCOM.BoUTBTableType.bott_Document)
        ''AddUserTable("AB_OUBUDGET", "OU Budget Table", SAPbobsCOM.BoUTBTableType.bott_Document)
        ''AddUserTable("AB_PROJECTBUDGET", "Project Budget Table", SAPbobsCOM.BoUTBTableType.bott_Document)
        ''AddUserTable("AB_CONSOLBUDGET", "Consolidation Budget Table", SAPbobsCOM.BoUTBTableType.bott_Document)


        ''Add_OUBudget_Fields()
        ' ''  Add_BUBudget_Fields()
        ''Add_ConsolidateBudget_Fields()
        ''Add_ProjectBudget_Fields()

        Add_OUBudget_Fields()


        Label3.Text = "Table Creation has been Finished ....... !"

    End Sub


End Class
