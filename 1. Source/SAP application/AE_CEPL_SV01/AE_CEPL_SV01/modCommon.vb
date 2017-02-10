Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.IO





Module modCommon


    Public Function GetSystemIntializeInfo(ByRef oCompDef As CompanyDefault, ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   GetSystemIntializeInfo()
        '   Purpose     :   This function will be providing information about the initialing variables
        '               
        '   Parameters  :   ByRef oCompDef As CompanyDefault
        '                       oCompDef =  set the Company Default structure
        '                   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '               
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   MAY 2014
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim sConnection As String = String.Empty
        Dim sSqlstr As String = String.Empty
        Try

            sFuncName = "GetSystemIntializeInfo()"
            Console.WriteLine("Starting Function", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            oCompDef.sDBName = String.Empty
            oCompDef.sServer = String.Empty
            oCompDef.sLicenseServer = String.Empty
            oCompDef.iServerLanguage = 3
            oCompDef.iServerType = 7
            oCompDef.sSAPUser = String.Empty
            oCompDef.sSAPPwd = String.Empty
            oCompDef.sSAPDBName = String.Empty

            oCompDef.sInboxDir = String.Empty
            oCompDef.sSuccessDir = String.Empty
            oCompDef.sFailDir = String.Empty
            oCompDef.sLogPath = String.Empty
            oCompDef.sDebug = String.Empty


            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Server")) Then
                oCompDef.sServer = ConfigurationManager.AppSettings("Server")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("LicenseServer")) Then
                oCompDef.sLicenseServer = ConfigurationManager.AppSettings("LicenseServer")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("ServerType")) Then
                oCompDef.iServerType = ConfigurationManager.AppSettings("ServerType")
            End If


            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPDBName")) Then
                oCompDef.sSAPDBName = ConfigurationManager.AppSettings("SAPDBName")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPUserName")) Then
                oCompDef.sSAPUser = ConfigurationManager.AppSettings("SAPUserName")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPPassword")) Then
                oCompDef.sSAPPwd = ConfigurationManager.AppSettings("SAPPassword")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPBankCode")) Then
                oCompDef.sSAPBankCode = ConfigurationManager.AppSettings("SAPBankCode")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SAPBankAccount")) Then
                oCompDef.sSAPBankAccount = ConfigurationManager.AppSettings("SAPBankAccount")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("DBUser")) Then
                oCompDef.sDBUser = ConfigurationManager.AppSettings("DBUser")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("DBPwd")) Then
                oCompDef.sDBPwd = ConfigurationManager.AppSettings("DBPwd")
            End If

            ' folder
            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("InboxDir")) Then
                oCompDef.sInboxDir = ConfigurationManager.AppSettings("InboxDir")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SuccessDir")) Then
                oCompDef.sSuccessDir = ConfigurationManager.AppSettings("SuccessDir")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("FailDir")) Then
                oCompDef.sFailDir = ConfigurationManager.AppSettings("FailDir")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("LogPath")) Then
                oCompDef.sLogPath = ConfigurationManager.AppSettings("LogPath")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("Debug")) Then
                oCompDef.sDebug = ConfigurationManager.AppSettings("Debug")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("EmailFrom")) Then
                oCompDef.sEmailFrom = ConfigurationManager.AppSettings("EmailFrom")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("EmailTo")) Then
                oCompDef.sEmailTo = ConfigurationManager.AppSettings("EmailTo")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("EmailSubject")) Then
                oCompDef.sEmailSubject = ConfigurationManager.AppSettings("EmailSubject")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SMTPServer")) Then
                oCompDef.sSMTPServer = ConfigurationManager.AppSettings("SMTPServer")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SMTPPort")) Then
                oCompDef.sSMTPPort = ConfigurationManager.AppSettings("SMTPPort")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SMTPUser")) Then
                oCompDef.sSMTPUser = ConfigurationManager.AppSettings("SMTPUser")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("SMTPPassword")) Then
                oCompDef.sSMTPPassword = ConfigurationManager.AppSettings("SMTPPassword")
            End If

            If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("ProjectDimension")) Then
                oCompDef.ProjectDimension = ConfigurationManager.AppSettings("ProjectDimension")
            End If

            Console.WriteLine("Completed with SUCCESS ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            GetSystemIntializeInfo = RTN_SUCCESS

        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            Console.WriteLine("Completed with ERROR ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GetSystemIntializeInfo = RTN_ERROR
        End Try
    End Function


    Public Function ExecuteSQLQuery_DT(ByVal sQuery As String) As DataTable

        '**************************************************************
        ' Function      : ExecuteQuery
        ' Purpose       : Execute SQL
        ' Parameters    : ByVal sSQL - string command Text
        ' Author        : JOHN
        ' Date          : MAY 2014 20
        ' Change        :
        '**************************************************************

        Dim sConstr As String = "Data Source=" & p_oCompDef.sServer & ";Initial Catalog=" & p_oCompDef.sSAPDBName & ";User ID=" & p_oCompDef.sDBUser & "; Password=" & p_oCompDef.sDBPwd

        Dim oCon As New SqlConnection(sConstr)
        Dim oCmd As New SqlCommand
        Dim oDs As New DataSet
        Dim sFuncName As String = String.Empty

        'Dim sConstr As String = "DRIVER={HDBODBC32};SERVERNODE={" & p_oCompDef.sServer & "};DSN=" & p_oCompDef.sDSN & ";UID=" & p_oCompDef.sDBUser & ";PWD=" & p_oCompDef.sDBPwd & ";"

        Try
            sFuncName = "ExecExecuteSQLQuery_DT()"
            Console.WriteLine("Starting Function.. ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)
            'oCon.ConnectionString = "DRIVER={HDBODBC};UID=" & p_oCompDef.sDBUser & ";PWD=" & p_oCompDef.sDBPwd & " ;SERVERNODE=" & p_oCompDef.sServer & ";CS=" & p_oCompDef.sSAPDBName & ""
            ' oCon.ConnectionString = "DRIVER={HDBODBC32};UID=" & p_oCompDef.sDBUser & ";PWD=" & p_oCompDef.sDBPwd & ";SERVERNODE=" & p_oCompDef.sServer & ";CS=" & p_oCompDef.sSAPDBName

            oCon.Open()
            oCmd.CommandType = CommandType.Text
            oCmd.CommandText = sQuery
            oCmd.Connection = oCon
            oCmd.CommandTimeout = 0
            Dim da As New SqlDataAdapter(oCmd)
            da.Fill(oDs)
            Console.WriteLine("Completed Successfully. ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed Successfully.", sFuncName)

        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            Console.WriteLine("Completed with ERROR ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Throw New Exception(ex.Message)
        Finally
            oCon.Dispose()
        End Try
        Return oDs.Tables(0)
    End Function

    Public Function GetEntitiesDetails(ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   GetEntitiesDetails()
        '   Purpose     :   This function will be providing information about the Entities, SAP username, SAP Password, Banking Details
        '               
        '   Parameters  :   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '               
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   MAY 2014 20
        ' **********************************************************************************


        Dim sSqlstr As String = String.Empty
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "GetEntitiesDetails()"
            ' Getting the details of Entity, SAP User name, Password and Banking from the COMPANYDATA Table
            sSqlstr = "SELECT T0.[Code], T0.[Name], T0.[U_AE_UName], T0.[U_AE_UPass] FROM [dbo].[@AE_COMPANYDATA]  T0"

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)
            Console.WriteLine("Starting Function", sFuncName)

            p_oEntitesDetails = ExecuteSQLQuery_DT(sSqlstr)

            ''Dim Findatarow() As DataRow = p_oEntitesDetails.Select("Code = 'CEP'")

            ''For Each row As DataRow In Findatarow
            ''    MsgBox(row(0) & ",  " & row(1) & ",  " & row(2) & ",  " & row(3) & ",  " & row(4) & ",  " & row(5))
            ''Next
            Console.WriteLine("Completed With SUCCESS ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS", sFuncName)
            GetEntitiesDetails = RTN_SUCCESS

        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            Console.WriteLine("Completed With ERROR ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With ERROR", sFuncName)
            GetEntitiesDetails = RTN_ERROR
        End Try

    End Function


    Public Function IdentifyCSVFile_JournalEntry(ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   IdentifyCSVFile_JournalEntry()
        '   Purpose     :   This function will identify the CSV file of Journal Entry
        '                    Upload the file into Dataview and provide the information to post transaction in SAP.
        '                     Transaction Success : Move the CSV file to SUCESS folder
        '                     Transaction Fail :    Move the CSV file to FAIL folder and send Error notification to concern person
        '               
        '   Parameters  :   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '               
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   MAY 2014 20
        ' **********************************************************************************


        Dim sSqlstr As String = String.Empty
        Dim bJEFileExist As Boolean
        Dim sFileType As String = String.Empty
        Dim oDTDistinct As DataTable = Nothing
        Dim oDTRowFilter As DataTable = Nothing
        Dim oDVJE As DataView = Nothing
        Dim oDICompany() As SAPbobsCOM.Company = Nothing

        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "IdentifyCSVFile_JournalEntry()"
            Console.WriteLine("Starting Function", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)


            Dim DirInfo As New System.IO.DirectoryInfo(p_oCompDef.sInboxDir)
            Dim files() As System.IO.FileInfo

            files = DirInfo.GetFiles("JE_*.csv")

            For Each File As System.IO.FileInfo In files
                bJEFileExist = True
                Console.WriteLine("Attempting File Name - " & File.Name, sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting File Name - " & File.Name, sFuncName)
                sFileType = Replace(File.Name, ".csv", "").Trim
                'upload the CSV to Dataview

                Console.WriteLine("GetDataViewFromCSV() ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("GetDataViewFromCSV() ", sFuncName)
                oDVJE = GetDataViewFromCSV(File.FullName, File.Name)

                Console.WriteLine("Getting Distinct of Entity", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Getting Distinct of Entity ", sFuncName)
                oDTDistinct = oDVJE.Table.DefaultView.ToTable(True, "F1")
                ReDim oDICompany(oDTDistinct.Rows.Count)

                For imjs As Integer = 0 To oDTDistinct.Rows.Count - 1

                    If Replace(oDTDistinct.Rows(imjs).Item(0).ToString.ToUpper, " ", "") = "OWNERENTITY" Then Continue For

                    oDICompany(imjs) = New SAPbobsCOM.Company

                    Console.WriteLine("Calling ConnectToTargetCompany()", sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConnectToTargetCompany()", sFuncName)
                    If ConnectToTargetCompany(oDICompany(imjs), oDTDistinct.Rows(imjs).Item(0).ToString, sErrDesc) <> RTN_SUCCESS Then
                        Throw New ArgumentException(sErrDesc)
                    End If

                    Console.WriteLine("Starting transaction on company database " & oDICompany(imjs).CompanyDB, sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting transaction on company database " & oDICompany(imjs).CompanyDB, sFuncName)
                    ' oDICompany(imjs).StartTransaction()


                    Console.WriteLine("Filtering data with respective Entity -  " & oDTDistinct.Rows(imjs).Item(0).ToString, sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Filtering data with respective Entity -  " & oDTDistinct.Rows(imjs).Item(0).ToString, sFuncName)
                    oDVJE.RowFilter = "F1 = '" & oDTDistinct.Rows(imjs).Item(0).ToString & "'"

                    Console.WriteLine("Calling Function JournalEntry_Posting() ", sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function JournalEntry_Posting() ", sFuncName)

                    If JournalEntry_Posting(oDVJE, oDICompany(imjs), sErrDesc) <> RTN_SUCCESS Then
                        For lCounter As Integer = 0 To UBound(oDICompany)
                            If Not oDICompany(lCounter) Is Nothing Then
                                If oDICompany(lCounter).Connected = True Then
                                    'If oDICompany(lCounter).InTransaction = True Then
                                    '    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Rollback transaction on company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                                    '    oDICompany(lCounter).EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                    'End If
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Disconnecting company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                                    oDICompany(lCounter).Disconnect()
                                    oDICompany(lCounter) = Nothing
                                End If
                            End If
                        Next

                        Console.WriteLine("Calling FileMoveToArchive for moving CSV file to archive folder ", sFuncName)
                        If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Calling FileMoveToArchive for moving CSV file to archive folder", sFuncName)
                        'AddDataToTable(p_oDtError, File.Name, "Error", sErrDesc)
                        FileMoveToArchive(File, File.FullName, RTN_ERROR, p_sJournalEntryError)

                        'Console.WriteLine("Error in updation. RollBack executed for ", sFuncName)
                        'If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Error in updation. RollBack executed for " & File.FullName, sFuncName)
                        IdentifyCSVFile_JournalEntry = RTN_ERROR
                        Exit Function

                    Else
                        Console.WriteLine("Calling FileMoveToArchive for moving CSV file to archive folder", sFuncName)
                        If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Calling FileMoveToArchive for moving CSV file to archive folder", sFuncName)
                        FileMoveToArchive(File, File.FullName, RTN_SUCCESS, "")
                    End If
                Next imjs
            Next

            If bJEFileExist = True Then
                For lCounter As Integer = 0 To UBound(oDICompany)
                    If Not oDICompany(lCounter) Is Nothing Then
                        If oDICompany(lCounter).Connected = True Then
                            'If oDICompany(lCounter).InTransaction = True Then
                            '    Console.WriteLine("Commit transaction on company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                            '    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Commit transaction on company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                            '    oDICompany(lCounter).EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            'End If
                            Console.WriteLine("Disconnecting company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Disconnecting company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                            oDICompany(lCounter).Disconnect()
                            oDICompany(lCounter) = Nothing
                        End If
                    End If
                Next

               
            Else
                Console.WriteLine("Journal Entry CSV File is not Identified ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Journal Entry CSV File is not Identified ", sFuncName)
            End If

            Console.WriteLine("Completed With SUCCESS ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS", sFuncName)
            IdentifyCSVFile_JournalEntry = RTN_SUCCESS

        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            Console.WriteLine("Completed With ERROR", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With ERROR", sFuncName)
            IdentifyCSVFile_JournalEntry = RTN_ERROR
        End Try

    End Function

    Public Function IdentifyCSVFile_OutgoingPayments(ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   IdentifyCSVFile_OutgoingPayments()
        '   Purpose     :   This function will identify the CSV file of Outgoing Payments 
        '                    Upload the file into Dataview and provide the information to post transaction in SAP.
        '                     Transaction Success : Move the CSV file to SUCESS folder
        '                     Transaction Fail :    Move the CSV file to FAIL folder and send Error notification to concern person
        '               
        '   Parameters  :   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '               
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   MAY 2014 22
        ' **********************************************************************************


        Dim sSqlstr As String = String.Empty
        Dim bOPFileExist As Boolean
        Dim sFileType As String = String.Empty
        Dim oDTDistinct As DataTable = Nothing
        Dim oDTRowFilter As DataTable = Nothing
        Dim oDVOP As DataView = Nothing
        Dim oDICompany() As SAPbobsCOM.Company = Nothing

        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "IdentifyCSVFile_OutgoingPayments()"
            Console.WriteLine("Starting Function ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)


            Dim DirInfo As New System.IO.DirectoryInfo(p_oCompDef.sInboxDir)
            Dim files() As System.IO.FileInfo

            files = DirInfo.GetFiles("OP_*.csv")

            For Each File As System.IO.FileInfo In files
                bOPFileExist = True
                Console.WriteLine("Attempting File Name - " & File.Name, sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting File Name - " & File.Name, sFuncName)
                sFileType = Replace(File.Name, ".csv", "").Trim
                'upload the CSV to Dataview
                Console.WriteLine("GetDataViewFromCSV() ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("GetDataViewFromCSV() ", sFuncName)
                oDVOP = GetDataViewFromCSV(File.FullName, File.Name)

                Console.WriteLine("Getting Distinct of Entity  ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Getting Distinct of Entity ", sFuncName)
                oDTDistinct = oDVOP.Table.DefaultView.ToTable(True, "F1")
                ReDim oDICompany(oDTDistinct.Rows.Count)

                For imjs As Integer = 0 To oDTDistinct.Rows.Count - 1

                    If Replace(oDTDistinct.Rows(imjs).Item(0).ToString.ToUpper, " ", "") = "OWNERENTITYCODE" Then Continue For

                    oDICompany(imjs) = New SAPbobsCOM.Company

                    Console.WriteLine("Calling ConnectToTargetCompany() ", sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling ConnectToTargetCompany()", sFuncName)
                    If ConnectToTargetCompany(oDICompany(imjs), oDTDistinct.Rows(imjs).Item(0).ToString, sErrDesc) <> RTN_SUCCESS Then
                        Throw New ArgumentException(sErrDesc)
                    End If

                    Console.WriteLine("Starting transaction on company database " & oDICompany(imjs).CompanyDB, sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting transaction on company database " & oDICompany(imjs).CompanyDB, sFuncName)
                    oDICompany(imjs).StartTransaction()

                    Console.WriteLine("Filtering data with respective Entity -  " & oDTDistinct.Rows(imjs).Item(0).ToString, sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Filtering data with respective Entity -  " & oDTDistinct.Rows(imjs).Item(0).ToString, sFuncName)
                    oDVOP.RowFilter = "F1 = '" & oDTDistinct.Rows(imjs).Item(0).ToString & "'"

                    Console.WriteLine("Calling Function OutgoingPayments_Posting() ", sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Calling Function OutgoingPayments_Posting() ", sFuncName)

                    If OutgoingPayments_Posting(oDVOP, oDICompany(imjs), sErrDesc) <> RTN_SUCCESS Then
                        For lCounter As Integer = 0 To UBound(oDICompany)
                            If Not oDICompany(lCounter) Is Nothing Then
                                If oDICompany(lCounter).Connected = True Then
                                    If oDICompany(lCounter).InTransaction = True Then
                                        Console.WriteLine("Rollback transaction on company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Rollback transaction on company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                                        oDICompany(lCounter).EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                    End If
                                    Console.WriteLine("Disconnecting company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Disconnecting company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                                    oDICompany(lCounter).Disconnect()
                                    oDICompany(lCounter) = Nothing
                                End If
                            End If
                        Next

                        Console.WriteLine("Calling FileMoveToArchive for moving CSV file to archive folder ", sFuncName)
                        If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Calling FileMoveToArchive for moving CSV file to archive folder", sFuncName)
                        ' AddDataToTable(p_oDtError, File.Name, "Error", sErrDesc)
                        FileMoveToArchive(File, File.FullName, RTN_ERROR, p_sOutgoingPaymentError)

                        Console.WriteLine("Error in updation. RollBack executed for " & File.FullName, sFuncName)
                        If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Error in updation. RollBack executed for " & File.FullName, sFuncName)
                        IdentifyCSVFile_OutgoingPayments = RTN_ERROR
                        Exit Function

                    Else
                        Console.WriteLine("Calling FileMoveToArchive for moving CSV file to archive folder ", sFuncName)
                        If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Calling FileMoveToArchive for moving CSV file to archive folder", sFuncName)
                        FileMoveToArchive(File, File.FullName, RTN_SUCCESS, "")
                    End If
                Next imjs
            Next

            If bOPFileExist = True Then
                For lCounter As Integer = 0 To UBound(oDICompany)
                    If Not oDICompany(lCounter) Is Nothing Then
                        If oDICompany(lCounter).Connected = True Then
                            If oDICompany(lCounter).InTransaction = True Then
                                Console.WriteLine("Commit transaction on company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Commit transaction on company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                                oDICompany(lCounter).EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            End If
                            Console.WriteLine("Disconnecting company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Disconnecting company database " & oDICompany(lCounter).CompanyDB, sFuncName)
                            oDICompany(lCounter).Disconnect()
                            oDICompany(lCounter) = Nothing
                        End If
                    End If
                Next
            Else
                Console.WriteLine("Outgoing Payment CSV File is not Identified ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Outgoing Payment CSV File is not Identified ", sFuncName)
            End If

            Console.WriteLine("Completed With SUCCESS  ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS", sFuncName)
            IdentifyCSVFile_OutgoingPayments = RTN_SUCCESS

        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            Console.WriteLine("Completed With ERROR  ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With ERROR", sFuncName)
            IdentifyCSVFile_OutgoingPayments = RTN_ERROR
        End Try

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

        Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.IO.Path.GetDirectoryName(CurrFileToUpload) & "\;Extended Properties=""text;HDR=NO;FMT=Delimited"""
        'Dim sConnectionString As String = "provider=Microsoft.Jet.OLEDB.4.0; " & _
        '  "data source='" & CurrFileToUpload & " '; " & "Extended Properties=Excel 8.0;"
        Dim objConn As New System.Data.OleDb.OleDbConnection(sConnectionString)
        Dim da As OleDb.OleDbDataAdapter
        Dim dt As DataTable
        Dim dv As DataView
       
        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "GetDataViewFromCSV"
            Console.WriteLine("Starting Function", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function", sFuncName)

            Console.WriteLine("Create_schema() ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Create_schema() ", sFuncName)
            Create_schema(p_oCompDef.sInboxDir, Filename)

            'Open Data Adapter to Read from Excel file
            da = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" & System.IO.Path.GetFileName(CurrFileToUpload) & "]", objConn)
            dt = New DataTable("BatchFile")
            'Fill dataset using dataadapter
            da.Fill(dt)
            Console.WriteLine("Completed With SUCCESS", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS", sFuncName)

            Console.WriteLine("Del_schema() ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Del_schema() ", sFuncName)
            Del_schema(p_oCompDef.sInboxDir)

            dv = New DataView(dt)
            Return dv

        Catch ex As Exception

            Console.WriteLine("Error occured while reading content of  " & ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error occured while reading content of  " & ex.Message, sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
            Return Nothing
        End Try

    End Function

    Public Function GetBankingDetails(ByVal sEntity As String, ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   GetBankingDetails()
        '   Purpose     :   This function will get the relavent Banking informations with respective Entities 
        '   Parameters  :   ByRef sEntity AS String 
        '                       sEntity = Entity Name
        '   Author      :   JOHN
        '   Date        :   MAY 2014 20
        ' **********************************************************************************
        Dim sFuncName As String = String.Empty
        sFuncName = "GetBankingDetails()"

        Try
            Console.WriteLine("Starting Function ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Entity String " & sEntity.ToString.Trim, sFuncName)

            Dim Findatarow() As DataRow = p_oEntitesDetails.Select("Code = '" & sEntity.ToString.Trim & "'")

            For Each row As DataRow In Findatarow
                p_sSAPEntityName = row(1)
                p_sSAPUName = row(2)
                p_sSAPUPass = row(3)

            Next

            Console.WriteLine("Completed With SUCCESS ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With SUCCESS", sFuncName)
            GetBankingDetails = RTN_SUCCESS

        Catch ex As Exception
            Console.WriteLine("Completed With ERROR ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed With ERROR  " & ex.Message, sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
            GetBankingDetails = RTN_ERROR
        End Try

    End Function

    Public Function ConnectToTargetCompany(ByRef oCompany As SAPbobsCOM.Company, _
                                          ByVal sEntity As String, _
                                          ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   ConnectToTargetCompany()
        '   Purpose     :   This function will be providing to proceed the connectivity of 
        '                   using SAP DIAPI function
        '               
        '   Parameters  :   ByRef oCompany As SAPbobsCOM.Company
        '                       oCompany =  set the SAP DI Company Object
        '                   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '               
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   MAY 2013 21
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim iRetValue As Integer = -1
        Dim iErrCode As Integer = -1
        Dim sSQL As String = String.Empty
        Dim oDs As New DataSet

        Try
            sFuncName = "ConnectToTargetCompany()"
            Console.WriteLine("Starting function", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting function", sFuncName)


            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug(" Calling GetBankingDetails ", sFuncName)
            Console.WriteLine("Calling GetBankingDetails ", sFuncName)
            If GetBankingDetails(sEntity, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

            If String.IsNullOrEmpty(p_sSAPUName) Then
                sErrDesc = "No Database login information found in COMPANYDATA Table. Please check"
                Throw New ArgumentException(sErrDesc)
                Console.WriteLine("No Database login information found in COMPANYDATA Table. Please check ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("No Database login information found in COMPANYDATA Table. Please check", sFuncName)

            Else

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Initializing the Company Object", sFuncName)
                Console.WriteLine("Initializing the Company Object ", sFuncName)
                oCompany = New SAPbobsCOM.Company

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Assigning the representing database name", sFuncName)
                Console.WriteLine("Assigning the representing database name ", sFuncName)
                oCompany.Server = p_oCompDef.sServer

                If p_oCompDef.iServerType = 2008 Then
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
                ElseIf p_oCompDef.iServerType = 2012 Then
                    oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
                End If

                oCompany.LicenseServer = p_oCompDef.sLicenseServer
                oCompany.CompanyDB = p_sSAPEntityName
                oCompany.UserName = p_sSAPUName
                oCompany.Password = p_sSAPUPass

                oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English

                oCompany.UseTrusted = False

                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Connecting to the Company Database.", sFuncName)
                Console.WriteLine("Connecting to the Company Database. ", sFuncName)
                iRetValue = oCompany.Connect()

                If iRetValue <> 0 Then
                    oCompany.GetLastError(iErrCode, sErrDesc)

                    sErrDesc = String.Format("Connection to Database ({0}) {1} {2} {3}", _
                        oCompany.CompanyDB, System.Environment.NewLine, _
                                    vbTab, sErrDesc)

                    Throw New ArgumentException(sErrDesc)
                End If

            End If

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Console.WriteLine("Completed with SUCCESS ", sFuncName)
            ConnectToTargetCompany = RTN_SUCCESS
        Catch ex As Exception
            sErrDesc = ex.Message
            Call WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Console.WriteLine("Completed with ERROR ", sFuncName)
            ConnectToTargetCompany = RTN_ERROR
        End Try
    End Function

    Public Sub FileMoveToArchive(ByVal oFile As System.IO.FileInfo, ByVal CurrFileToUpload As String, ByVal iStatus As Integer, ByVal sErrDesc As String)

        'Event      :   FileMoveToArchive
        'Purpose    :   For Renaming the file with current time stamp & moving to archive folder
        'Author     :   JOHN 
        'Date       :   21 MAY 2014

        Dim sFuncName As String = String.Empty

        Try
            sFuncName = "FileMoveToArchive"
            Console.WriteLine("Starting Function ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)
            'Dim RenameCurrFileToUpload = Replace(CurrFileToUpload.ToUpper, ".CSV", "") & "_" & Format(Now, "yyyyMMddHHmmss") & ".csv"
            Dim RenameCurrFileToUpload As String = Mid(oFile.Name, 1, oFile.Name.Length - 4) & "_" & Now.ToString("yyyyMMddhhmmss") & ".csv"

            If iStatus = RTN_SUCCESS Then
                Console.WriteLine("Moving CSV file to success folder ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Moving CSV file to success folder", sFuncName)
                oFile.MoveTo(p_oCompDef.sSuccessDir & "\" & RenameCurrFileToUpload)
            Else
                Console.WriteLine("Sending ERROR notification mail ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Sending ERROR notification mail", sFuncName)

                If SendEmailNotification(oFile.FullName, p_oCompDef.sEmailTo, sErrDesc) <> RTN_SUCCESS Then Throw New ArgumentException(sErrDesc)

                Console.WriteLine("Moving CSV file to Fail folder ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then WriteToLogFile_Debug("Moving CSV file to Fail folder", sFuncName)
                oFile.MoveTo(p_oCompDef.sFailDir & "\" & RenameCurrFileToUpload)
            End If
        Catch ex As Exception
            Console.WriteLine("Error in renaming/copying/moving ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Error in renaming/copying/moving", sFuncName)
            Call WriteToLogFile(ex.Message, sFuncName)
        End Try
    End Sub

    Public Function SendEmailNotification(ByVal sfileName As String, ByVal sSenderEmail As String, ByVal sErrDesc As String) As Long

        ' ***********************************************************************************
        '   Function   :    SendEmailNotification()
        '   Purpose    :    This function is handles - Sending notification mails
        '   Parameters :    ByVal sFileName As String
        '                       sFileName = Passing file name
        '                   ByRef sErrDesc As String
        '                       sErrDesc = Error Description to be returned to calling function
        '   Return     :    0 - FAILURE
        '                   1 - SUCCESS
        '   Author     :    JOHN
        '   Date       :    23/05/2014 
        '   Change     :   
        '                   
        ' ***********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim oSmtpServer As New SmtpClient()
        Dim oMail As New MailMessage
        Dim sBody As String = String.Empty

        Try
            sFuncName = "SendEmailNotification()"
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function...", sFuncName)
            Console.WriteLine("Starting Function... " & sFuncName)
            '------------  Date format
            p_SyncDateTime = Format(Now, "dddd") & ", " & Format(Now, "MMM") & " " & Format(Now, "dd") & ", " & Format(Now, "yyyy") & " " & Format(Now, "HH:mm:ss")
            '--------- Message Content in HTML tags
            sBody = sBody & "<div align=left style='font-size:10.0pt;font-family:Arial'>"
            sBody = sBody & " Dear Valued Customer,<br /><br />"
            sBody = sBody & p_SyncDateTime & " <br /><br />"
            sBody = sBody & " Please find the attached FAILED document in SAP and followed by the ERROR. <br /><br />"
            sBody = sBody & sErrDesc & "<br /><br />"
            sBody = sBody & "<br/> Note: This email message is computer generated and it will be used internal purpose usage only.<div/>"

            oSmtpServer.Credentials = New Net.NetworkCredential(p_oCompDef.sSMTPUser, p_oCompDef.sSMTPPassword)
            oSmtpServer.Port = p_oCompDef.sSMTPPort
            oSmtpServer.Host = p_oCompDef.sSMTPServer
            oSmtpServer.EnableSsl = True
            oMail.From = New MailAddress(p_oCompDef.sEmailFrom)
            oMail.To.Add(sSenderEmail)
            oMail.Attachments.Add(New Attachment(sfileName))
            oMail.Subject = p_oCompDef.sEmailSubject
            oMail.Body = sBody
            oMail.IsBodyHtml = True
            oSmtpServer.Send(oMail)
            oMail.Dispose()

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            Console.WriteLine("Completed with SUCCESS " & sFuncName)
            SendEmailNotification = RTN_SUCCESS
        Catch ex As Exception
            WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Console.WriteLine("Completed with Error " & sFuncName)
            SendEmailNotification = RTN_ERROR
        Finally

        End Try

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
            'Dim s1, s2, s3, s4, s5 As String

            srOutput.WriteLine("[" & csvFileName & "]")
            srOutput.WriteLine("ColNameHeader=False")
            srOutput.WriteLine("Format=CSVDelimited")
            srOutput.WriteLine("Col1=F1 Text")
            srOutput.WriteLine("Col2=F2 Text")
            srOutput.WriteLine("Col3=F3 Text")
            srOutput.WriteLine("Col4=F4 Text")
            srOutput.WriteLine("Col5=F5 Text")
            srOutput.WriteLine("Col6=F6 Text")
            srOutput.WriteLine("Col7=F7 Text")
            srOutput.WriteLine("Col8=F8 Text")
            srOutput.WriteLine("Col9=F9 Text")
            srOutput.WriteLine("Col10=F10 Double")
            srOutput.WriteLine("Col11=F11 Text")
            srOutput.WriteLine("Col12=F12 Double")
            srOutput.WriteLine("Col13=F13 Text")
            srOutput.WriteLine("Col14=F14 Text")
            srOutput.WriteLine("Col15=F15 Text")
            srOutput.WriteLine("MaxScanRows=0")
            srOutput.WriteLine("CharacterSet=OEM")
            'srOutput.WriteLine(s1.ToString() + ControlChars.Lf + s2.ToString() + ControlChars.Lf + s3.ToString() + ControlChars.Lf + s4.ToString() + ControlChars.Lf)
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

End Module