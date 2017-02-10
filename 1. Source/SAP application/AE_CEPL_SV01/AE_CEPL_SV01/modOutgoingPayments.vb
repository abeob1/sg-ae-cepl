Module modOutgoingPayments

    Public Function OutgoingPayments_Posting(ByVal oDVPayments As DataView, ByRef oCompany As SAPbobsCOM.Company, ByRef sErrDesc As String) As Long

        Dim sFuncName As String = String.Empty
        Dim sEntity As String = String.Empty
        Dim ddate As Date = Nothing
        Dim ival As Integer
        Dim IsError As Boolean
        Dim iErr As Integer = 0
        Dim sErr As String = String.Empty
        Dim sOP As String = String.Empty

        Try
            sFuncName = "OutgoingPayments_Posting()"
            Console.WriteLine("Starting Function ", sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Starting Function ", sFuncName)
            'Fetching Entity from the Dataview object


            Dim oPayments As SAPbobsCOM.Payments = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments)
            oPayments.DocType = SAPbobsCOM.BoRcptTypes.rAccount
            oPayments.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments

            ' Loop for fetching the datas from the dataview
            For Each drv As DataRowView In oDVPayments

                ' Outgoing Payments Document Header Information
                ddate = drv(4).ToString.Substring(4, 4) & "/" & drv(4).ToString.Substring(2, 2) & "/" & drv(4).ToString.Substring(0, 2)
                oPayments.DocDate = ddate
                oPayments.DueDate = ddate
                oPayments.TaxDate = ddate
                oPayments.DocCurrency = drv(5).ToString.Trim
                '   oPayments.CounterReference = drv(1).ToString.Trim
                oPayments.UserFields.Fields.Item("U_AI_EMPID").Value = drv(1).ToString.Trim
                oPayments.CardName = drv(2).ToString.Trim
                oPayments.Remarks = drv(7).ToString.Trim

                'Journal Entry Document Line Information
                'Account Code
                oPayments.AccountPayments.AccountCode = drv(8).ToString.Trim
                'Amount
                oPayments.AccountPayments.SumPaid = drv(10).ToString.Trim
                'Line Remarks
                oPayments.AccountPayments.Decription = drv(11).ToString.Trim

                'Payment Means - Cheque Payment

                oPayments.Checks.AccounttNum = p_oCompDef.sSAPBankAccount
                oPayments.Checks.BankCode = p_oCompDef.sSAPBankCode
                oPayments.Checks.CheckSum = drv(10).ToString.Trim
                oPayments.Checks.DueDate = ddate
                oPayments.Checks.ManualCheck = SAPbobsCOM.BoYesNoEnum.tNO
                oPayments.Checks.Add()

                Console.WriteLine("Attempting to Add the Outgoing Payments ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Attempting to Add the Outgoing Payments", sFuncName)
                oPayments.AccountPayments.Add()
                ival = oPayments.Add()

                If ival <> 0 Then
                    IsError = True
                    oCompany.GetLastError(iErr, sErr)
                    p_sOutgoingPaymentError = "Error Code :- " & iErr & " Error Description :- " & sErr
                    Call WriteToLogFile("Completed with ERROR ---" & sErr, sFuncName)
                    Console.WriteLine("Completed with ERROR " & sErr, sFuncName)
                    If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR " & sErr, sFuncName)
                    OutgoingPayments_Posting = RTN_ERROR
                    Exit Function
                End If

                Console.WriteLine("Completed with SUCCESS ", sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with SUCCESS ", sFuncName)
                oCompany.GetNewObjectCode(sOP)
                Console.WriteLine("Outgoing Payment DocEntry  " & sOP, sFuncName)
                If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Outgoing Payment DocEntry  " & sOP, sFuncName)

            Next

            OutgoingPayments_Posting = RTN_SUCCESS

        Catch ex As Exception
            p_sOutgoingPaymentError = " Error Description :- " & ex.Message
            Call WriteToLogFile(ex.Message, sFuncName)
            Console.WriteLine("Completed with ERROR " & ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call WriteToLogFile_Debug("Completed with ERROR " & ex.Message, sFuncName)
            OutgoingPayments_Posting = RTN_ERROR
            Exit Function

        End Try

    End Function





End Module
