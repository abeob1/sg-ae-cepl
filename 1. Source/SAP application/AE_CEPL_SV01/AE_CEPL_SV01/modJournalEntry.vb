Module modJournalEntry


    Public Function JournalEntry_Posting(ByVal oDVJournal As DataView, ByRef oCompany As SAPbobsCOM.Company, ByRef sErrDesc As String) As Long

        Dim sFuncName As String = String.Empty
        Dim sEntity As String = String.Empty
        Dim ddate As Date = Nothing
        Dim ival As Integer
        Dim IsError As Boolean
        Dim iErr As Integer = 0
        Dim sErr As String = String.Empty
        Dim sJV As String = String.Empty
        Dim sEmpCat As String = String.Empty
        Dim sPaycode As String = String.Empty
        Dim sCreditGL As String = String.Empty
        Dim sRemarks As String = String.Empty
        Dim sdimension1 As String = String.Empty
        Dim sdimension2 As String = String.Empty
        Dim sdimension3 As String = String.Empty
        Dim iindex As Integer = 0
        Dim dCreditAmount As Double = 0.0
        Dim dDebitAmount As Double = 0.0
        Dim Amount As Double = 0.0

        Dim oRecordset As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Dim oProfitcenter As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

        Try
            sFuncName = "JournalEntry_Posting"
            Console.WriteLine("Starting Function ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)
            'Fetching Entity from the Dataview object

            ddate = oDVJournal.Table.Rows(1).Item(2).ToString.Substring(4, 4) & "/" & oDVJournal.Table.Rows(1).Item(2).ToString.Substring(2, 2) & "/" & oDVJournal.Table.Rows(1).Item(2).ToString.Substring(0, 2)
            Dim oJournalEntry As SAPbobsCOM.JournalEntries = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            ' Journal Entry Document Header Information
            oJournalEntry.ReferenceDate = ddate
            oJournalEntry.DueDate = ddate
            oJournalEntry.TaxDate = ddate
            oJournalEntry.Reference = oDVJournal.Table.Rows(1).Item(1).ToString.Trim
            oJournalEntry.Memo = oDVJournal.Table.Rows(1).Item(5).ToString.Trim
            'Journal Entry Document Line Information
            For Each drv As DataRowView In oDVJournal

                '-------------------------------------------------------------------------------
                '------- Checking sCreditGL is empty or not, First time this Credit GL varaibles is empty, so it will satisfied the condition.

                oProfitcenter.DoQuery("SELECT T0.[GrpCode] FROM OPRC T0 WHERE T0.[PrcCode]  = '" & drv(13).ToString.Trim & "' and  T0.[DimCode] = '" & p_oCompDef.ProjectDimension & "'")

                If String.IsNullOrEmpty(sCreditGL) Then

                    'Account Code
                    ' checking for 5 series or 6 series
                    If Left(drv(8).ToString.Trim, 1) = 5 Or Left(drv(8).ToString.Trim, 1) = 6 Then
                        Dim sSQL = "SELECT T0.[U_AE_GLAcct] FROM [dbo].[@AE_EMPCATGL]  T0 WHERE T0.[U_AE_Category] = '" & drv(6).ToString.Trim & "' and   T0.[U_AE_PayItem] = '" & drv(7).ToString.Trim & "'"
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("SQL " & sSQL, sFuncName)
                        oRecordset.DoQuery(sSQL)
                        If Not String.IsNullOrEmpty(oRecordset.Fields.Item("U_AE_GLAcct").Value) Then
                            oJournalEntry.Lines.AccountCode = oRecordset.Fields.Item("U_AE_GLAcct").Value.ToString.Trim
                        Else
                            oJournalEntry.Lines.AccountCode = drv(8).ToString.Trim
                        End If
                    Else
                        oJournalEntry.Lines.AccountCode = drv(8).ToString.Trim
                    End If

                    ' Debit Amount
                    If Not String.IsNullOrEmpty(drv(9).ToString.Trim) Then
                        oJournalEntry.Lines.Debit = CDbl(drv(9).ToString.Trim)
                        oJournalEntry.Lines.Credit = "0"
                    End If

                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Debit " & drv(9).ToString.Trim, sFuncName)

                    'Line Remarks
                    oJournalEntry.Lines.LineMemo = drv(12).ToString.Trim
                    'Line Project Code ( Dimension 1)
                    If Not String.IsNullOrEmpty(drv(13).ToString.Trim) Then
                        oJournalEntry.Lines.CostingCode = drv(13).ToString.Trim
                    End If

                    'Line Profit Center ( Dimension 2)
                    If Not String.IsNullOrEmpty(oProfitcenter.Fields.Item("GrpCode").Value) Then
                        oJournalEntry.Lines.CostingCode2 = oProfitcenter.Fields.Item("GrpCode").Value
                    End If

                    'Line Cost Center ( Dimension 3)
                    If Not String.IsNullOrEmpty(drv(14).ToString.Trim) Then
                        oJournalEntry.Lines.CostingCode3 = drv(14).ToString.Trim
                    End If
                    oJournalEntry.Lines.Add()

                Else
                    '-----------------------------------------------------------------------------------------
                    '-------- Here we check the sCreditGL is equal to Debit GL in the dataview ---------------
                    '-------- Its equal True part will executed otherwise Else part will execute -------------
                    If sCreditGL = drv(10).ToString.Trim And sdimension1 = drv(13).ToString.Trim Then

                        '----------------------- (sCreditGL and Debit GL) and (Project) in Dataview is equal we just accumulate the 
                        '                        Credit amount in the bCreditAmount Variable 
                        'Account Code
                        ' checking for 5 series or 6 series
                        If Left(drv(8).ToString.Trim, 1) = 5 Or Left(drv(8).ToString.Trim, 1) = 6 Then
                            Dim sSQL = "SELECT T0.[U_AE_GLAcct] FROM [dbo].[@AE_EMPCATGL]  T0 WHERE T0.[U_AE_Category] = '" & drv(6).ToString.Trim & "' and   T0.[U_AE_PayItem] = '" & drv(7).ToString.Trim & "'"
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("SQL " & sSQL, sFuncName)
                            oRecordset.DoQuery(sSQL)
                            If Not String.IsNullOrEmpty(oRecordset.Fields.Item("U_AE_GLAcct").Value) Then
                                oJournalEntry.Lines.AccountCode = oRecordset.Fields.Item("U_AE_GLAcct").Value.ToString.Trim
                            Else
                                oJournalEntry.Lines.AccountCode = drv(8).ToString.Trim
                            End If
                        Else
                            oJournalEntry.Lines.AccountCode = drv(8).ToString.Trim
                        End If

                        ' Debit Amount
                        If Not String.IsNullOrEmpty(drv(9).ToString.Trim) Then
                            oJournalEntry.Lines.Debit = CDbl(drv(9).ToString.Trim)
                            oJournalEntry.Lines.Credit = "0"
                        End If
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Debit " & drv(9).ToString.Trim, sFuncName)
                        'Line Remarks
                        oJournalEntry.Lines.LineMemo = drv(12).ToString.Trim
                        'Line Project Code ( Dimension 1)
                        If Not String.IsNullOrEmpty(drv(13).ToString.Trim) Then
                            oJournalEntry.Lines.CostingCode = drv(13).ToString.Trim
                        End If

                        'Line Profit Center ( Dimension 2)
                        If Not String.IsNullOrEmpty(oProfitcenter.Fields.Item("GrpCode").Value) Then
                            oJournalEntry.Lines.CostingCode2 = oProfitcenter.Fields.Item("GrpCode").Value
                        End If

                        'Line Cost Center ( Dimension 3)
                        If Not String.IsNullOrEmpty(drv(14).ToString.Trim) Then
                            oJournalEntry.Lines.CostingCode3 = drv(14).ToString.Trim
                        End If
                        oJournalEntry.Lines.Add()

                    Else  'If sCreditGL <> drv(10).ToString.Trim Then

                        '----------------------- sCreditGL and Credit GL in Dataview is not equal
                        '                        We adding Credit GL, Credit Amount, Remarks, Dimentions and reset the Credit amount to zero

                        'Checking for 5 / 6 series
                        '------------------------------------

                        'Account Code
                        If Left(sCreditGL, 1) = 5 Or Left(sCreditGL, 1) = 6 Then
                            Dim sSQL = "SELECT T0.[U_AE_GLAcct] FROM [dbo].[@AE_EMPCATGL]  T0 WHERE T0.[U_AE_Category] = '" & sEmpCat & "' and   T0.[U_AE_PayItem] = '" & sPaycode & "'"
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("SQL " & sSQL, sFuncName)
                            oRecordset.DoQuery(sSQL)
                            If Not String.IsNullOrEmpty(oRecordset.Fields.Item("U_AE_GLAcct").Value) Then
                                oJournalEntry.Lines.AccountCode = oRecordset.Fields.Item("U_AE_GLAcct").Value.ToString.Trim
                            Else
                                oJournalEntry.Lines.AccountCode = sCreditGL
                            End If
                        Else
                            oJournalEntry.Lines.AccountCode = sCreditGL
                        End If

                        ' oJournalEntry.Lines.AccountCode = sCreditGL
                        ' Credit Amount
                        oJournalEntry.Lines.Debit = 0
                        oJournalEntry.Lines.Credit = dCreditAmount
                        'Line Remarks
                        oJournalEntry.Lines.LineMemo = sRemarks
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Credit " & CStr(dCreditAmount), sFuncName)

                        'Line Project Code ( Dimension 1)
                        If Not String.IsNullOrEmpty(sdimension1) Then
                            oJournalEntry.Lines.CostingCode = sdimension1
                        End If

                        'Line Profit Center ( Dimension 2)
                        If Not String.IsNullOrEmpty(sdimension2) Then
                            oJournalEntry.Lines.CostingCode2 = sdimension2
                        End If

                        'Line Cost Center ( Dimension 3)
                        If Not String.IsNullOrEmpty(sdimension3) Then
                            oJournalEntry.Lines.CostingCode3 = sdimension3
                        End If

                        oJournalEntry.Lines.Add()

                        dCreditAmount = 0.0
                        '----------------------- Adding information regarding Debit side

                        'Account Code
                        ' checking for 5 series or 6 series
                        If Left(drv(8).ToString.Trim, 1) = 5 Or Left(drv(8).ToString.Trim, 1) = 6 Then
                            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("SELECT T0.[U_AE_GLAcct] FROM [dbo].[@AE_EMPCATGL]  T0 WHERE T0.[U_AE_Category] = '" & drv(6).ToString.Trim & "' and   T0.[U_AE_PayItem] = '" & drv(7).ToString.Trim & "'", sFuncName)
                            oRecordset.DoQuery("SELECT T0.[U_AE_GLAcct] FROM [dbo].[@AE_EMPCATGL]  T0 WHERE T0.[U_AE_Category] = '" & drv(6).ToString.Trim & "' and   T0.[U_AE_PayItem] = '" & drv(7).ToString.Trim & "'")
                            If Not String.IsNullOrEmpty(oRecordset.Fields.Item("U_AE_GLAcct").Value) Then
                                oJournalEntry.Lines.AccountCode = oRecordset.Fields.Item("U_AE_GLAcct").Value.ToString.Trim
                            Else
                                oJournalEntry.Lines.AccountCode = drv(8).ToString.Trim
                            End If
                        Else
                            oJournalEntry.Lines.AccountCode = drv(8).ToString.Trim
                        End If

                        ' Debit Amount
                        If Not String.IsNullOrEmpty(drv(9).ToString.Trim) Then
                            oJournalEntry.Lines.Debit = CDbl(drv(9).ToString.Trim)
                            oJournalEntry.Lines.Credit = "0"
                        End If
                        If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Debit " & drv(9).ToString.Trim, sFuncName)
                        'Line Remarks
                        oJournalEntry.Lines.LineMemo = drv(12).ToString.Trim
                        'Line Project Code ( Dimension 1)
                        If Not String.IsNullOrEmpty(drv(13).ToString.Trim) Then
                            oJournalEntry.Lines.CostingCode = drv(13).ToString.Trim
                        End If

                        'Line Profit Center ( Dimension 2)
                        If Not String.IsNullOrEmpty(oProfitcenter.Fields.Item("GrpCode").Value) Then
                            oJournalEntry.Lines.CostingCode2 = oProfitcenter.Fields.Item("GrpCode").Value
                        End If

                        'Line Cost Center ( Dimension 3)
                        If Not String.IsNullOrEmpty(drv(14).ToString.Trim) Then
                            oJournalEntry.Lines.CostingCode3 = drv(14).ToString.Trim
                        End If
                        oJournalEntry.Lines.Add()

                    End If

                End If
                sEmpCat = drv(6).ToString.Trim
                sPaycode = drv(7).ToString.Trim
                iindex += 1
                dCreditAmount += drv(11).ToString.Trim
                sCreditGL = drv(10).ToString.Trim
                sRemarks = drv(12).ToString.Trim
                sdimension1 = drv(13).ToString.Trim
                sdimension2 = oProfitcenter.Fields.Item("GrpCode").Value
                sdimension3 = drv(14).ToString.Trim

            Next

            'Checking for 5 / 6 series
            '------------------------------------

            'Account Code
            If Left(sCreditGL, 1) = 5 Or Left(sCreditGL, 1) = 6 Then
                Dim sSQL = "SELECT T0.[U_AE_GLAcct] FROM [dbo].[@AE_EMPCATGL]  T0 WHERE T0.[U_AE_Category] = '" & sEmpCat & "' and   T0.[U_AE_PayItem] = '" & sPaycode & "'"
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("SQL " & sSQL, sFuncName)
                oRecordset.DoQuery(sSQL)
                If Not String.IsNullOrEmpty(oRecordset.Fields.Item("U_AE_GLAcct").Value) Then
                    oJournalEntry.Lines.AccountCode = oRecordset.Fields.Item("U_AE_GLAcct").Value.ToString.Trim
                Else
                    oJournalEntry.Lines.AccountCode = sCreditGL
                End If
            Else
                oJournalEntry.Lines.AccountCode = sCreditGL
            End If

            ' Credit Amount
            oJournalEntry.Lines.Debit = 0
            oJournalEntry.Lines.Credit = dCreditAmount
            'Line Remarks
            oJournalEntry.Lines.LineMemo = sRemarks
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Credit " & dCreditAmount, sFuncName)

            'Line Project Code ( Dimension 1)
            If Not String.IsNullOrEmpty(sdimension1) Then
                oJournalEntry.Lines.CostingCode = sdimension1
            End If

            'Line Profit Center ( Dimension 2)
            If Not String.IsNullOrEmpty(sdimension2) Then
                oJournalEntry.Lines.CostingCode2 = sdimension2
            End If

            'Line Cost Center ( Dimension 3)
            If Not String.IsNullOrEmpty(sdimension3) Then
                oJournalEntry.Lines.CostingCode3 = sdimension3
            End If


            Console.WriteLine("Attempting to Add the Journal Entry ", sFuncName)

            '' Console.WriteLine("Total amount  ", dCreditAmount)
            ''WriteToLogFile_Debug("Total Amount ---", dCreditAmount)
            dCreditAmount = 0.0

            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Add the Journal Entry", sFuncName)
            ival = oJournalEntry.Add()

            If ival <> 0 Then
                IsError = True
                oCompany.GetLastError(iErr, sErr)
                p_sJournalEntryError = "Error Code :- " & iErr & " Error Description :- " & sErr
                Call WriteToLogFile("Completed with ERROR ---" & sErr, sFuncName)
                Console.WriteLine("Completed with ERROR ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR " & sErr, sFuncName)
                JournalEntry_Posting = RTN_ERROR
                Exit Function
            End If

            Console.WriteLine("Completed with SUCCESS", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ", sFuncName)
            oCompany.GetNewObjectCode(sJV)
            Console.WriteLine("Journal Entry DocEntry  " & sJV, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Journal Entry DocEntry  " & sJV, sFuncName)
            JournalEntry_Posting = RTN_SUCCESS

        Catch ex As Exception
            p_sJournalEntryError = "Error Description :- " & ex.Message
            Call WriteToLogFile(ex.Message, sFuncName)
            Console.WriteLine("Completed with ERROR ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR " & ex.Message, sFuncName)
            JournalEntry_Posting = RTN_ERROR
            Exit Function
        End Try

    End Function

End Module
