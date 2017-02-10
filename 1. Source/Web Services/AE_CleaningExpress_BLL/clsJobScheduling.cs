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
    public class clsJobScheduling
    {
        clsLog oLog = new clsLog();

        public Int16 p_iDebugMode = DEBUG_ON;

        public const Int16 RTN_SUCCESS = 1;
        public const Int16 RTN_ERROR = 0;
        public const Int16 DEBUG_ON = 1;
        public const Int16 DEBUG_OFF = 0;
        public string sErrDesc = string.Empty;

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

        public DataSet Get_JobSchedule_Project(DataSet oDTCompanyList, string sCompany, string sCurrentUserName, string sUserRole)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_JobSchedule_Project()";
                sProcName = "AE_SP019_Mobile_JS_GetProject";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@Company", sCompany), Data.CreateParameter("@UserName", sCurrentUserName), Data.CreateParameter("@UserRole", sUserRole));

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

        public DataSet Get_JobSchedule_ListofJobs(DataSet oDTCompanyList, string sCompany, string sProjectCode)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_JobSchedule_ListofJobs()";
                sProcName = "AE_SP020_Mobile_JS_ListofJobs";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@Company", sCompany), Data.CreateParameter("@ProjectCode", sProjectCode));

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

        public DataSet Get_JobSchedule_JobDetails(DataSet oDTCompanyList, string sCompany, string sDocEntry)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_JobSchedule_JobDetails()";
                sProcName = "AE_SP021_Mobile_JS_JobDetails";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@Company", sCompany), Data.CreateParameter("@DocEntry", sDocEntry));

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

        public DataSet Get_JobSchedule_ScheduledDayInfo(DataSet oDTCompanyList, string sCompany, string sDocEntry, DateTime dtScheduledDate)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_JobSchedule_ScheduledDayInfo()";
                sProcName = "AE_SP022_Mobile_JS_ScheduledDayInfo";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@Company", sCompany), Data.CreateParameter("@DocEntry", sDocEntry), Data.CreateParameter("@ScheduledDate", dtScheduledDate));

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
    }
}
