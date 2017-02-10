using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using AE_CleaningExpress_Common;
using System.Data;
using SAP.Admin.DAO;

namespace AE_CleaningExpress_BLL
{
    public class clsLogin
    {
        clsLog oLog = new clsLog();

        SAPbobsCOM.Company oDICompany;

        public Int16 p_iDebugMode = DEBUG_ON;

        public const Int16 RTN_SUCCESS = 1;
        public const Int16 RTN_ERROR = 0;
        public const Int16 DEBUG_ON = 1;
        public const Int16 DEBUG_OFF = 0;
        public string sErrDesc = string.Empty;

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

        public DataSet Get_CompanyList()
        {
            DataSet oDataset;
            string sFuncName = string.Empty;
            string sProcName = string.Empty;

            try
            {
                sFuncName = "Get_CompanyList()";
                sProcName = "AE_SP001_Mobile_GetCompanyList";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);

                oDataset = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.StoredProcedure, "AE_SP001_Mobile_GetCompanyList");

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public DataSet Get_Acknowledgement(DataSet oDTCompanyList, string sUserName, string sPassword, string sCompany)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_Acknowledgement()";
                sProcName = "AE_SP002_Mobile_GetAcknowledgement";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);

                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, "AE_SP002_Mobile_GetAcknowledgement",
                            Data.CreateParameter("@UserName", sUserName), Data.CreateParameter("@Password", sPassword), Data.CreateParameter("@Company", sCompany));
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        return oDataset;
                    }
                }
                else
                {
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                    return oDataset;
                }

                return oDataset;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public SAPbobsCOM.Company ConnectToTargetCompany(string sCompanyDB)
        {
            string sFuncName = string.Empty;
            string sReturnValue = string.Empty;
            DataSet oDTCompanyList = new DataSet();
            DataSet oDSResult = new DataSet();
            //SAPbobsCOM.Company oDICompany = new SAPbobsCOM.Company();
            string sConnString = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "ConnectToTargetCompany()";

                //oSessionCompany = oSessionCompany +  sSessionUserName;
                // SAPbobsCOM.Company Convert.ToString(Session["sLoginUserName"]);
                //SAPbobsCOM.Company = sSessionUserName + oSessionCompany;
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

               // oDICompany = (SAPbobsCOM.Company)Session["SAPCompany"];

                if (oDICompany != null)
                {
                    if (oDICompany.CompanyDB == sCompanyDB)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("ODICompany Name " + oDICompany.CompanyDB , sFuncName);
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("SCompanyDB " + sCompanyDB, sFuncName);
                        return oDICompany;
                    }

                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Get_Company_Details() ", sFuncName);
                oDTCompanyList = Get_CompanyList();

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Filter Based on Company DB() ", sFuncName);
                oDTView = oDTCompanyList.Tables[0].DefaultView;
                oDTView.RowFilter = "U_DBName= '" + sCompanyDB + "'";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling ConnectToTargetCompany() ", sFuncName);

                sConnString = oDTView[0]["U_ConnString"].ToString();

                oDICompany = ConnectToTargetCompany(oDICompany, oDTView[0]["U_SAPUserName"].ToString(), oDTView[0]["U_SAPPassword"].ToString()
                                   , oDTView[0]["U_DBName"].ToString(), oDTView[0]["U_Server"].ToString(), oDTView[0]["U_LicenseServer"].ToString()
                                   , oDTView[0]["U_DBUserName"].ToString(), oDTView[0]["U_DBPassword"].ToString(), sErrDesc);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);

                //SAPCompany(oDIComapny, sConnString);

                return oDICompany;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }

        }

        public SAPbobsCOM.Company ConnectToTargetCompany(SAPbobsCOM.Company oCompany, string sUserName, string sPassword, string sDBName,
                                                        string sServer, string sLicServerName, string sDBUserName
                                                       , string sDBPassword, string sErrDesc)
        {
            string sFuncName = string.Empty;
            //SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();
            long lRetCode;

            try
            {
                sFuncName = "ConnectToTargetCompany()";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (oCompany != null)
                {
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Disconnecting the Company object - Company Name " + oCompany.CompanyName, sFuncName);
                    oCompany.Disconnect();
                }
                oCompany = new SAPbobsCOM.Company();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Initializing Company Connection ", sFuncName);
                oCompany.Server = sServer;
                oCompany.LicenseServer = sLicServerName;
                oCompany.DbUserName = sDBUserName;
                oCompany.DbPassword = sDBPassword;
                oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;
                oCompany.UseTrusted = false;
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;


                oCompany.CompanyDB = sDBName;// sDataBaseName;
                oCompany.UserName = sUserName;
                oCompany.Password = sPassword;

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting the Database...", sFuncName);

                lRetCode = oCompany.Connect();

                if (lRetCode != 0)
                {

                    throw new ArgumentException(oCompany.GetLastErrorDescription());
                }
                else
                {
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Company Connection Established", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS", sFuncName);
                    return oCompany;
                }

            }
            catch (Exception Ex)
            {

                sErrDesc = Ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }

        public Int32 ChangePassword(DataSet oDTCompanyList, string sCompany, string sUserName, string sNewPassword)
        {
            int iReturnResult = 0;
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sQuery = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "ChangePassword()";
                sQuery = "UPDATE OUSR SET U_MobilePwd = @MobilePwd WHERE USER_CODE = @UserName ; Select @@ROWCOUNT as ReturnResult";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sQuery, sFuncName);

                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.Text, sQuery,
                            Data.CreateParameter("@MobilePwd", sNewPassword), Data.CreateParameter("@UserName", sUserName));
                        iReturnResult = Convert.ToInt32(oDataset.Tables[0].Rows[0][0]);
                        if (iReturnResult > 0)
                        {
                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                        }
                    }
                    else
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        return iReturnResult;
                    }
                }
                else
                {
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                    return iReturnResult;
                }

                return iReturnResult;
            }
            catch (Exception Ex)
            {
                sErrDesc = Ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                throw Ex;
            }
        }
    }
}
