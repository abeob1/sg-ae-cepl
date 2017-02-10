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
    public class clsStockRequest
    {
        clsLog oLog = new clsLog();

        public Int16 p_iDebugMode = DEBUG_ON;

        public const Int16 RTN_SUCCESS = 1;
        public const Int16 RTN_ERROR = 0;
        public const Int16 DEBUG_ON = 1;
        public const Int16 DEBUG_OFF = 0;
        public string sErrDesc = string.Empty;

        public static string ConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

        public DataSet Get_Project(DataSet oDTCompanyList, string sCompany, string sCurrentUserName, DateTime dtRequiredDate)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_Project()";
                sProcName = "AE_SP003_Mobile_GetProject";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, "AE_SP003_Mobile_GetProject",
                            Data.CreateParameter("@Company", sCompany), Data.CreateParameter("@UserName", sCurrentUserName), Data.CreateParameter("@RequiredDate", dtRequiredDate));

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

        public DataSet Get_WareHouse(DataSet oDTCompanyList, string sCompany, string sProjectCode)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_WareHouse()";
                sProcName = "AE_SP004_Mobile_GetWareHouse";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, "AE_SP004_Mobile_GetWareHouse",
                            Data.CreateParameter("@Company", sCompany), Data.CreateParameter("@ProjectCode", sProjectCode));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
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

        public DataSet Get_ItemList(DataSet oDTCompanyList, string sCompany, string sProjectCode, string sWarehouseCode, DateTime dtRequiredDate)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_ItemList()";
                sProcName = "AE_SP005_Mobile_GetItemList";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, "AE_SP005_Mobile_GetItemList",
                            Data.CreateParameter("@Company", sCompany), Data.CreateParameter("@ProjectCode", sProjectCode),
                            Data.CreateParameter("@WareHouseCode", sWarehouseCode), Data.CreateParameter("@RequiredDate", dtRequiredDate));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
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

        public DataSet Get_StockRequestDetails(DataSet oDTCompanyList, string sCompany, string sDocEntry)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_StockRequestDetails()";
                sProcName = "AE_SP006_Mobile_GetStockRequestDetails";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, "AE_SP006_Mobile_GetStockRequestDetails",
                            Data.CreateParameter("@Company", sCompany), Data.CreateParameter("@DocEntry", sDocEntry));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
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

        public DataSet Get_EditStockRequestDetails(DataSet oDTCompanyList, string sCompany, string sDocEntry)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_EditStockRequestDetails()";
                sProcName = "AE_SP0011_Mobile_EditStockRequestDetails";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, "AE_SP0011_Mobile_EditStockRequestDetails",
                            Data.CreateParameter("@Company", sCompany), Data.CreateParameter("@DocEntry", sDocEntry));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
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

        public DataSet Get_StockRequestList(DataSet oDTCompanyList, string sCompany, string sCategory, string sUserName)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();

            try
            {
                sFuncName = "Get_StockRequestList()";
                sProcName = "AE_SP007_Mobile_MGetStockRequestList";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Calling Run_StoredProcedure() " + sProcName, sFuncName);
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, "AE_SP007_Mobile_MGetStockRequestList",
                            Data.CreateParameter("@Company", sCompany), Data.CreateParameter("@StockCategory", sCategory), Data.CreateParameter("@UserName", sUserName));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS ", sFuncName);
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
