using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.HTTP;
using System.Web;
using System.Web.Services;
using AE_CleaningExpress_Common;
using AE_CleaningExpress_BLL;
using System.Web.Script.Services;
using System.Data;
using System.Web.Script.Serialization;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;

namespace AE_CleaningExpress_Mobile_V001
{
    /// <summary>
    /// Summary description for Mobile
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]

    public class Mobile : System.Web.Services.WebService
    {
        #region Objects
        public string sErrDesc = string.Empty;

        public Int16 p_iDebugMode = DEBUG_ON;

        public const Int16 RTN_SUCCESS = 1;
        public const Int16 RTN_ERROR = 0;
        public const Int16 DEBUG_ON = 1;
        public const Int16 DEBUG_OFF = 0;
        public static string signPath = ConfigurationManager.AppSettings["SignaturePath"].ToString();

        clsLogin oLogin = new clsLogin();
        clsStockRequest oStockRequest = new clsStockRequest();
        clsPendingGoodsIssue oPendingGoodsIssue = new clsPendingGoodsIssue();
        clsCustomerFeedback oCustomerFeedback = new clsCustomerFeedback();
        clsShowAround oShowAround = new clsShowAround();
        clsLog oLog = new clsLog();
        DocumentXML oDocxml = new DocumentXML();
        List<result> lstResult = new List<result>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        SAPbobsCOM.Company oDICompany;
        #endregion

        #region WebMethods for Stock Request & Goods Issue

        #region WebMethods
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void MGet_CompanyList()
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_CompanyList";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                DataSet ds = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<Company> lstCompany = new List<Company>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Company _company = new Company();
                        _company.U_DBName = r["U_DBName"].ToString();
                        _company.U_CompName = r["U_CompName"].ToString();
                        _company.U_SAPUserName = r["U_SAPUserName"].ToString();
                        _company.U_SAPPassword = r["U_SAPPassword"].ToString();
                        _company.U_DBUserName = r["U_DBUserName"].ToString();
                        _company.U_DBPassword = r["U_DBPassword"].ToString();
                        _company.U_ConnString = r["U_ConnString"].ToString();
                        _company.U_Server = r["U_Server"].ToString();
                        _company.U_LicenseServer = r["U_LicenseServer"].ToString();
                        lstCompany.Add(_company);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Company List ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstCompany));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Company List , the Serialized data is ' " + js.Serialize(lstCompany) + " '", sFuncName);
                }
                else
                {
                    List<Company> lstCompany = new List<Company>();
                    Context.Response.Output.Write(js.Serialize(lstCompany));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_Acknowledgement(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_Acknowledgement";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sUserName = string.Empty;
                string sPassword = string.Empty;
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_UserInfo> lstDeserialize = js.Deserialize<List<Json_UserInfo>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_UserInfo objUserInfo = lstDeserialize[0];
                    sUserName = objUserInfo.sUserName;
                    sPassword = objUserInfo.sPassword;
                    sCompany = objUserInfo.sCompany;
                }

                DataSet oDTCompanyList = new DataSet();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before calling the Get_CompanyList() ", sFuncName);
                oDTCompanyList = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After calling the Get_CompanyList() ", sFuncName);
                Session["ODTCompanyList"] = oDTCompanyList;
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before calling the Method Get_Acknowledgement() ", sFuncName);
                DataSet ds = oLogin.Get_Acknowledgement(oDTCompanyList, sUserName, sPassword, sCompany);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After calling the Method Get_Acknowledgement() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<UserInfo> lstUserInfo = new List<UserInfo>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        UserInfo _userInfo = new UserInfo();
                        _userInfo.UserName = r["UserName"].ToString();
                        _userInfo.Password = r["Password"].ToString();
                        _userInfo.EmployeeName = r["EmployeeName"].ToString();
                        _userInfo.CompanyCode = r["CompanyCode"].ToString();
                        _userInfo.CompanyName = r["CompanyName"].ToString();
                        _userInfo.Status = r["Status"].ToString();
                        _userInfo.EmpId = r["EmpId"].ToString();
                        _userInfo.Theme = r["Theme"].ToString();
                        _userInfo.Message = r["Message"].ToString();
                        _userInfo.RoleId = r["RoleId"].ToString();
                        _userInfo.RoleName = r["RoleName"].ToString();
                        lstUserInfo.Add(_userInfo);
                    }
                    if (lstUserInfo.Count == 0)
                    {
                        List<UserInfo> lstUserInfo1 = new List<UserInfo>();
                        UserInfo objUserInfo = new UserInfo();
                        objUserInfo.UserName = sUserName;
                        objUserInfo.Password = sPassword;
                        objUserInfo.CompanyCode = sCompany;
                        objUserInfo.Status = clsAppConstants.Failure;
                        lstUserInfo1.Add(objUserInfo);

                        Context.Response.Output.Write(js.Serialize(lstUserInfo1));
                    }
                    else
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the UserInformation ", sFuncName);
                        Context.Response.Output.Write(js.Serialize(lstUserInfo));
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the UserInformation , the Serialized data is ' " + js.Serialize(lstUserInfo) + " '", sFuncName);
                    }
                }
                else
                {
                    List<UserInfo> lstUserInfo = new List<UserInfo>();
                    UserInfo objUserInfo = new UserInfo();
                    objUserInfo.UserName = sUserName;
                    objUserInfo.Password = sPassword;
                    objUserInfo.CompanyCode = sCompany;
                    objUserInfo.Status = clsAppConstants.Failure;
                    lstUserInfo.Add(objUserInfo);

                    Context.Response.Output.Write(js.Serialize(lstUserInfo));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_Project(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_Project";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sCurrentUserName = string.Empty;
                DateTime dtRequiredDate = new DateTime();

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_Project> lstDeserialize = js.Deserialize<List<Json_Project>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_Project objProject = lstDeserialize[0];
                    sCompany = objProject.sCompany;
                    sCurrentUserName = objProject.sCurrentUserName;
                    dtRequiredDate = objProject.dtRequiredDate;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list from session ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list from session ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_Project() ", sFuncName);
                DataSet ds = oStockRequest.Get_Project(dsCompanyList, sCompany, sCurrentUserName, dtRequiredDate);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_Project() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<Project> lstProject = new List<Project>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Project _project = new Project();
                        _project.PrcCode = r["PrcCode"].ToString();
                        _project.PrcName = r["PrcName"].ToString();
                        lstProject.Add(_project);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Project list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstProject));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Project list, the serialized data is ' " + js.Serialize(lstProject) + " '", sFuncName);
                }
                else
                {
                    List<Project> lstProject = new List<Project>();
                    Context.Response.Output.Write(js.Serialize(lstProject));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_WareHouse(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_WareHouse";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_WareHouse> lstDeserialize = js.Deserialize<List<Json_WareHouse>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_WareHouse objWarehouse = lstDeserialize[0];
                    sCompany = objWarehouse.sCompany;
                    sProjectCode = objWarehouse.sProjectCode;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list from session ", sFuncName);

                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list from session ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_WareHouse() ", sFuncName);
                DataSet ds = oStockRequest.Get_WareHouse(dsCompanyList, sCompany, sProjectCode);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_WareHouse() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<WareHouse> lstWarehouse = new List<WareHouse>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        WareHouse _warehouse = new WareHouse();
                        _warehouse.WhsCode = r["WhsCode"].ToString();
                        _warehouse.WhsName = r["WhsName"].ToString();
                        lstWarehouse.Add(_warehouse);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the warehouse list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstWarehouse));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the warehouse list, the serialized data is ' " + js.Serialize(lstWarehouse) + " '", sFuncName);

                }
                else
                {
                    List<WareHouse> lstWarehouse = new List<WareHouse>();
                    Context.Response.Output.Write(js.Serialize(lstWarehouse));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_ItemList(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_ItemList";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;
                string sWarehouseCode = string.Empty;
                DateTime dtRequiredDate = new DateTime();

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_Item> lstDeserialize = js.Deserialize<List<Json_Item>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_Item objWarehouse = lstDeserialize[0];
                    sCompany = objWarehouse.sCompany;
                    sProjectCode = objWarehouse.sProjectCode;
                    sWarehouseCode = objWarehouse.sWarehouseCode;
                    dtRequiredDate = objWarehouse.dtRequiredDate;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list from session ", sFuncName);

                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list from session ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_ItemList() ", sFuncName);
                DataSet ds = oStockRequest.Get_ItemList(dsCompanyList, sCompany, sProjectCode, sWarehouseCode, dtRequiredDate);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_ItemList() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<Item> lstItem = new List<Item>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Item _item = new Item();
                        _item.ItemCode = r["ItemCode"].ToString();
                        _item.Description = r["Description"].ToString();
                        _item.AvailableQty = r["AvailableQty"].ToString();
                        _item.UnitPrice = r["UnitPrice"].ToString();
                        _item.BKTDocEntry = r["BKTDocEntry"].ToString();
                        _item.BKTLineId = r["BKTLineId"].ToString();
                        _item.OcrCode2 = r["OcrCode2"].ToString();
                        _item.UOM = r["UOM"].ToString();
                        _item.ProjectCode = r["ProjectCode"].ToString();
                        _item.WareHouseCode = r["WareHouseCode"].ToString();
                        _item.RequiredDate = r["RequiredDate"].ToString();
                        lstItem.Add(_item);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the item list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstItem));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the item list, the serialized data is ' " + js.Serialize(lstItem) + " '", sFuncName);
                }
                else
                {
                    List<Item> lstItem = new List<Item>();
                    Context.Response.Output.Write(js.Serialize(lstItem));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_StockRequestDetails(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_StockRequestDetails";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sDocEntry = string.Empty;
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_StockRequestDetails> lstDeserialize = js.Deserialize<List<Json_StockRequestDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_StockRequestDetails objDetails = lstDeserialize[0];
                    sDocEntry = objDetails.sDocEntry;
                    sCompany = objDetails.sCompany;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list from session ", sFuncName);

                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list from session ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_StockRequestDetails() ", sFuncName);
                DataSet ds = oStockRequest.Get_StockRequestDetails(dsCompanyList, sCompany, sDocEntry);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_StockRequestDetails() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<StockRequestItemDetails> lstItems = new List<StockRequestItemDetails>();
                    List<StockRequestDetails> lststkReqDetails = new List<StockRequestDetails>();
                    StockRequestItemDetails _stkItems = new StockRequestItemDetails();
                    StockRequestDetails _stkReqDetails = new StockRequestDetails();
                    //this loop is for adding the Stock Request Line items in Array
                    foreach (DataRow drItems in ds.Tables[0].Rows)
                    {
                        lstItems.Add(new StockRequestItemDetails
                        {
                            ItemCode = drItems["ItemCode"].ToString(),
                            Description = drItems["Description"].ToString(),
                            Quantity = drItems["Quantity"].ToString(),
                            BKTDocEntry = drItems["BKTDocEntry"].ToString(),
                            BKTLineId = drItems["BKTLineId"].ToString(),
                            OcrCode2 = drItems["OcrCode2"].ToString(),
                            UOM = drItems["UOM"].ToString()
                        });
                    }

                    //for adding the Stock Request details in Header and merge the line of Array items

                    _stkReqDetails.ProjectCode = ds.Tables[0].Rows[0]["ProjectCode"].ToString();
                    _stkReqDetails.ProjectName = ds.Tables[0].Rows[0]["ProjectName"].ToString();
                    _stkReqDetails.WhsCode = ds.Tables[0].Rows[0]["WhsCode"].ToString();
                    _stkReqDetails.WhsName = ds.Tables[0].Rows[0]["WhsName"].ToString();
                    _stkReqDetails.DocNum = ds.Tables[0].Rows[0]["DocNum"].ToString();
                    _stkReqDetails.DocEntry = ds.Tables[0].Rows[0]["DocEntry"].ToString();
                    _stkReqDetails.ReqDate = ds.Tables[0].Rows[0]["ReqDate"].ToString();
                    _stkReqDetails.ReqName = ds.Tables[0].Rows[0]["ReqName"].ToString();
                    _stkReqDetails.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                    _stkReqDetails.Items = lstItems;

                    lststkReqDetails.Add(_stkReqDetails);

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Request Details ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lststkReqDetails));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Request Details, the serialized data is ' " + js.Serialize(lststkReqDetails) + " '", sFuncName);
                }
                else
                {
                    List<StockRequestDetails> lststkReqDetails = new List<StockRequestDetails>();
                    Context.Response.Output.Write(js.Serialize(lststkReqDetails));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_EditStockRequestDetails(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_EditStockRequestDetails";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sDocEntry = string.Empty;
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_StockRequestDetails> lstDeserialize = js.Deserialize<List<Json_StockRequestDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_StockRequestDetails objDetails = lstDeserialize[0];
                    sDocEntry = objDetails.sDocEntry;
                    sCompany = objDetails.sCompany;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list from session ", sFuncName);

                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list from session ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_EditStockRequestDetails() ", sFuncName);
                DataSet ds = oStockRequest.Get_EditStockRequestDetails(dsCompanyList, sCompany, sDocEntry);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_EditStockRequestDetails() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    // the following code is as same as Goods Issue Item details, thats y copied the same code
                    List<GoodsIssueItemDetails> lstItems = new List<GoodsIssueItemDetails>();
                    List<GoodsIssueDetails> lstGoodsIssueDetails = new List<GoodsIssueDetails>();
                    GoodsIssueItemDetails _goodsItems = new GoodsIssueItemDetails();
                    GoodsIssueDetails _goodsIssueDetails = new GoodsIssueDetails();
                    //this loop is for adding the Stock Request Line items in Array
                    foreach (DataRow drItems in ds.Tables[0].Rows)
                    {
                        lstItems.Add(new GoodsIssueItemDetails
                        {
                            ItemCode = drItems["ItemCode"].ToString(),
                            Description = drItems["Description"].ToString(),
                            Quantity = drItems["Quantity"].ToString(),
                            AvailableQty = drItems["AvailableQty"].ToString(),
                            BKTDocEntry = drItems["BKTDocEntry"].ToString(),
                            BKTLineId = drItems["BKTLineId"].ToString(),
                            sCurrentUserName = drItems["sCurrentUserName"].ToString(),
                            LineId = drItems["LineId"].ToString(),
                            UnitPrice = drItems["UnitPrice"].ToString(),
                            IssueQty = drItems["IssueQty"].ToString(),
                            TransferQty = drItems["TransferQty"].ToString(),
                            OcrCode2 = drItems["OcrCode2"].ToString(),
                            UOM = drItems["UOM"].ToString()
                        });
                    }

                    //for adding the Stock Request details in Header and merge the line of Array items

                    _goodsIssueDetails.ProjectCode = ds.Tables[0].Rows[0]["ProjectCode"].ToString();
                    _goodsIssueDetails.ProjectName = ds.Tables[0].Rows[0]["ProjectName"].ToString();
                    _goodsIssueDetails.WhsCode = ds.Tables[0].Rows[0]["WhsCode"].ToString();
                    _goodsIssueDetails.WhsName = ds.Tables[0].Rows[0]["WhsName"].ToString();
                    _goodsIssueDetails.DocNum = ds.Tables[0].Rows[0]["DocNum"].ToString();
                    _goodsIssueDetails.DocEntry = ds.Tables[0].Rows[0]["DocEntry"].ToString();
                    _goodsIssueDetails.ReqDate = ds.Tables[0].Rows[0]["ReqDate"].ToString();
                    _goodsIssueDetails.ReqName = ds.Tables[0].Rows[0]["ReqName"].ToString();
                    _goodsIssueDetails.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                    _goodsIssueDetails.Items = lstItems;

                    lstGoodsIssueDetails.Add(_goodsIssueDetails);

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Edit stock Request Details ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstGoodsIssueDetails));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Edit stock Request Details, the serialized data is ' " + js.Serialize(lstGoodsIssueDetails) + " '", sFuncName);
                }
                else
                {
                    List<GoodsIssueDetails> lststkReqDetails = new List<GoodsIssueDetails>();
                    Context.Response.Output.Write(js.Serialize(lststkReqDetails));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_StockRequestList(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_StockRequestList";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sCategory = string.Empty;
                string sUserName = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_StockRequestList> lstDeserialize = js.Deserialize<List<Json_StockRequestList>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_StockRequestList objList = lstDeserialize[0];
                    sCompany = objList.sCompany;
                    sCategory = objList.sCategory;
                    sUserName = objList.sUserName;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list from session ", sFuncName);

                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list from session ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_StockRequestList() ", sFuncName);
                DataSet ds = oStockRequest.Get_StockRequestList(dsCompanyList, sCompany, sCategory, sUserName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_StockRequestList() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<StockRequestList> lststkReqList = new List<StockRequestList>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        StockRequestList _stkReqList = new StockRequestList();
                        _stkReqList.ProjectCode = r["ProjectCode"].ToString();
                        _stkReqList.ProjectName = r["ProjectName"].ToString();
                        _stkReqList.DocNum = r["DocNum"].ToString();
                        _stkReqList.DocEntry = r["DocEntry"].ToString();
                        _stkReqList.ReqDate = r["ReqDate"].ToString();
                        _stkReqList.ReqName = r["ReqName"].ToString();
                        _stkReqList.Status = r["Status"].ToString();
                        lststkReqList.Add(_stkReqList);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Request List ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lststkReqList));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Request List, the serialized data is ' " + js.Serialize(lststkReqList) + " '", sFuncName);
                }
                else
                {
                    List<StockRequestList> lststkReqList = new List<StockRequestList>();
                    Context.Response.Output.Write(js.Serialize(lststkReqList));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_GoodsIssueProject(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GoodsIssueProject";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sCurrentUserName = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_GoodsIssueProject> lstDeserialize = js.Deserialize<List<Json_GoodsIssueProject>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_GoodsIssueProject objProject = lstDeserialize[0];
                    sCompany = objProject.sCompany;
                    sCurrentUserName = objProject.sCurrentUserName;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list from session ", sFuncName);
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list from session ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GoodsIssueProject() ", sFuncName);
                DataSet ds = oPendingGoodsIssue.Get_GoodsIssueProject(dsCompanyList, sCompany, sCurrentUserName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GoodsIssueProject() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<Project> lstProject = new List<Project>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Project _project = new Project();
                        _project.PrcCode = r["PrcCode"].ToString();
                        _project.PrcName = r["PrcName"].ToString();
                        lstProject.Add(_project);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Project list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstProject));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Project list, the serialized data is ' " + js.Serialize(lstProject) + " '", sFuncName);
                }
                else
                {
                    List<Project> lstProject = new List<Project>();
                    Context.Response.Output.Write(js.Serialize(lstProject));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_GoodsIssueList(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GoodsIssueList";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_GoodsIssueList> lstDeserialize = js.Deserialize<List<Json_GoodsIssueList>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_GoodsIssueList objProject = lstDeserialize[0];
                    sCompany = objProject.sCompany;
                    sProjectCode = objProject.sProjectCode;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list from session ", sFuncName);
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list from session ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GoodsIssueList() ", sFuncName);
                DataSet ds = oPendingGoodsIssue.Get_GoodsIssueList(dsCompanyList, sCompany, sProjectCode);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GoodsIssueList() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<GoodsIssueList> lstGoodsIssue = new List<GoodsIssueList>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        GoodsIssueList _goodsIssue = new GoodsIssueList();
                        _goodsIssue.ProjectCode = r["ProjectCode"].ToString();
                        _goodsIssue.ProjectName = r["ProjectName"].ToString();
                        _goodsIssue.DocNum = r["DocNum"].ToString();
                        _goodsIssue.DocEntry = r["DocEntry"].ToString();
                        _goodsIssue.ReqDate = r["ReqDate"].ToString();
                        _goodsIssue.ReqName = r["ReqName"].ToString();
                        _goodsIssue.Status = r["Status"].ToString();
                        lstGoodsIssue.Add(_goodsIssue);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Goods Issue list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstGoodsIssue));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Goods Issue list, the serialized data is ' " + js.Serialize(lstGoodsIssue) + " '", sFuncName);
                }
                else
                {
                    List<GoodsIssueList> lstProject = new List<GoodsIssueList>();
                    Context.Response.Output.Write(js.Serialize(lstProject));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_GoodsIssueDetails(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GoodsIssueDetails";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sDocEntry = string.Empty;
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_GoodsIssueDetails> lstDeserialize = js.Deserialize<List<Json_GoodsIssueDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_GoodsIssueDetails objDetails = lstDeserialize[0];
                    sDocEntry = objDetails.sDocEntry;
                    sCompany = objDetails.sCompany;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list from session ", sFuncName);

                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list from session ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GoodsIssueDetails() ", sFuncName);
                DataSet ds = oPendingGoodsIssue.Get_GoodsIssueDetails(dsCompanyList, sCompany, sDocEntry);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GoodsIssueDetails() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<GoodsIssueItemDetails> lstItems = new List<GoodsIssueItemDetails>();
                    List<GoodsIssueDetails> lstGoodsIssueDetails = new List<GoodsIssueDetails>();
                    GoodsIssueItemDetails _goodsItems = new GoodsIssueItemDetails();
                    GoodsIssueDetails _goodsIssueDetails = new GoodsIssueDetails();
                    //this loop is for adding the Stock Request Line items in Array
                    foreach (DataRow drItems in ds.Tables[0].Rows)
                    {
                        lstItems.Add(new GoodsIssueItemDetails
                        {
                            ItemCode = drItems["ItemCode"].ToString(),
                            Description = drItems["Description"].ToString(),
                            Quantity = drItems["Quantity"].ToString(),
                            AvailableQty = drItems["AvailableQty"].ToString(),
                            BKTDocEntry = drItems["BKTDocEntry"].ToString(),
                            BKTLineId = drItems["BKTLineId"].ToString(),
                            sCurrentUserName = drItems["sCurrentUserName"].ToString(),
                            LineId = drItems["LineId"].ToString(),
                            UnitPrice = drItems["UnitPrice"].ToString(),
                            IssueQty = drItems["IssueQty"].ToString(),
                            TransferQty = drItems["TransferQty"].ToString(),
                            OcrCode2 = drItems["OcrCode2"].ToString(),
                            UOM = drItems["UOM"].ToString()
                        });
                    }

                    //for adding the Stock Request details in Header and merge the line of Array items

                    _goodsIssueDetails.ProjectCode = ds.Tables[0].Rows[0]["ProjectCode"].ToString();
                    _goodsIssueDetails.ProjectName = ds.Tables[0].Rows[0]["ProjectName"].ToString();
                    _goodsIssueDetails.WhsCode = ds.Tables[0].Rows[0]["WhsCode"].ToString();
                    _goodsIssueDetails.WhsName = ds.Tables[0].Rows[0]["WhsName"].ToString();
                    _goodsIssueDetails.DocNum = ds.Tables[0].Rows[0]["DocNum"].ToString();
                    _goodsIssueDetails.DocEntry = ds.Tables[0].Rows[0]["DocEntry"].ToString();
                    _goodsIssueDetails.ReqDate = ds.Tables[0].Rows[0]["ReqDate"].ToString();
                    _goodsIssueDetails.ReqName = ds.Tables[0].Rows[0]["ReqName"].ToString();
                    _goodsIssueDetails.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                    _goodsIssueDetails.Items = lstItems;

                    lstGoodsIssueDetails.Add(_goodsIssueDetails);

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Issue Details ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstGoodsIssueDetails));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Issue Details, the serialized data is ' " + js.Serialize(lstGoodsIssueDetails) + " '", sFuncName);
                }
                else
                {
                    List<GoodsIssueDetails> lstGoodsIssueDetails = new List<GoodsIssueDetails>();
                    Context.Response.Output.Write(js.Serialize(lstGoodsIssueDetails));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_GoodsIssueItemList(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GoodsIssueItemList";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;
                string sWarehouseCode = string.Empty;
                DateTime dtRequiredDate = new DateTime();
                string sDocEntry = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_GoodsItem> lstDeserialize = js.Deserialize<List<Json_GoodsItem>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_GoodsItem objGoodsItem = lstDeserialize[0];
                    sCompany = objGoodsItem.sCompany;
                    sProjectCode = objGoodsItem.sProjectCode;
                    sWarehouseCode = objGoodsItem.sWarehouseCode;
                    dtRequiredDate = objGoodsItem.dtRequiredDate;
                    sDocEntry = objGoodsItem.sDocEntry;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list from session ", sFuncName);

                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list from session ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GoodsIssueItemList() ", sFuncName);
                DataSet ds = oPendingGoodsIssue.Get_GoodsIssueItemList(dsCompanyList, sCompany, sProjectCode, sWarehouseCode, dtRequiredDate, sDocEntry);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GoodsIssueItemList() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<GoodsItem> lstItem = new List<GoodsItem>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        GoodsItem _item = new GoodsItem();
                        _item.ItemCode = r["ItemCode"].ToString();
                        _item.Description = r["Description"].ToString();
                        _item.AvailableQty = r["AvailableQty"].ToString();
                        _item.UnitPrice = r["UnitPrice"].ToString();
                        _item.BKTDocEntry = r["BKTDocEntry"].ToString();
                        _item.BKTLineId = r["BKTLineId"].ToString();
                        //_item.OcrCode2 = r["OcrCode2"].ToString();
                        _item.UOM = r["UOM"].ToString();
                        lstItem.Add(_item);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the item list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstItem));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the item list, the serialized data is ' " + js.Serialize(lstItem) + " '", sFuncName);
                }
                else
                {
                    List<GoodsItem> lstItem = new List<GoodsItem>();
                    Context.Response.Output.Write(js.Serialize(lstItem));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        //SAP Interaction

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MSave_StockRequest(string sJsonInput)
        {
            string sFuncName = string.Empty;
            string xmlResult = string.Empty;
            SAPbobsCOM.Documents oPurchaseRequest;
            double lRetCode;
            string sProject = string.Empty;
            string sWarehouse = string.Empty;
            DateTime dtRequiredDate;
            string sItemCode = string.Empty;

            try
            {
                sFuncName = "MSave_StockRequest";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<SaveStockRequest> stockList = js.Deserialize<List<SaveStockRequest>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (stockList.Count > 0)
                {
                    DataSet dsStockDetails = new DataSet();

                    DataTable tbNew = new DataTable();
                    tbNew = AddBudgetTable();
                    SaveStockRequest stockDetails = stockList[0];
                    List<Json_SaveStockRequestDetail> items = stockDetails.Items;

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Header Datatable ", sFuncName);
                    DataTable HeaderTable = Save_ConvertToStockHeaderTable(stockDetails);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Line Datatable ", sFuncName);
                    DataTable LineTable = Save_ConvertToStockLineTable(items, stockDetails.sProjectCode, stockDetails.sWarehouseCode);

                    DataTable headerCopy = HeaderTable.Copy();
                    DataTable lineCopy = LineTable.Copy();
                    dsStockDetails.Tables.Add(headerCopy);
                    dsStockDetails.Tables.Add(lineCopy);
                    dsStockDetails.Tables[0].TableName = "tblHeader";
                    dsStockDetails.Tables[1].TableName = "tblLine";

                    if (dsStockDetails != null && dsStockDetails.Tables.Count > 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                        oDICompany = oLogin.ConnectToTargetCompany(stockDetails.sCompany);
                        SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Business Object ", sFuncName);
                        oPurchaseRequest = (SAPbobsCOM.Documents)(oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting to assign the Header data ", sFuncName);
                        oPurchaseRequest.DocDate = Convert.ToDateTime(dsStockDetails.Tables[0].Rows[0]["DocDate"].ToString());
                        oPurchaseRequest.DocDueDate = Convert.ToDateTime(dsStockDetails.Tables[0].Rows[0]["DocDueDate"].ToString());
                        oPurchaseRequest.RequriedDate = Convert.ToDateTime(dsStockDetails.Tables[0].Rows[0]["ReqDate"].ToString());
                        oPurchaseRequest.Requester = dsStockDetails.Tables[0].Rows[0]["Requester"].ToString();
                        oPurchaseRequest.UserFields.Fields.Item("U_AB_FillStatus").Value = "Open";
                        oPurchaseRequest.UserFields.Fields.Item("U_AI_PROJECTCODE").Value = stockDetails.sProjectCode;
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting to assign the Line data ", sFuncName);

                        sProject = stockDetails.sProjectCode;
                        sWarehouse = stockDetails.sWarehouseCode;
                        dtRequiredDate = Convert.ToDateTime(stockDetails.dtRequiredDate.ToString());
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("No Error 1", sFuncName);

                        foreach (DataRow item in dsStockDetails.Tables[1].Rows)
                        {
                            //if (Convert.ToString ( item["Quantity"]) == null) continue;
                            if (item["Quantity"].ToString() != string.Empty)
                            {
                                oPurchaseRequest.Lines.ItemCode = item["ItemCode"].ToString();
                                oPurchaseRequest.Lines.Quantity = Convert.ToDouble(item["Quantity"]);
                                if (item["UnitPrice"].ToString() != string.Empty)
                                {
                                    oPurchaseRequest.Lines.Price = Convert.ToDouble(item["UnitPrice"].ToString());
                                }
                                else
                                {
                                    oPurchaseRequest.Lines.Price = 0.0;
                                }
                                oPurchaseRequest.Lines.WarehouseCode = item["WhsCode"].ToString();
                                oPurchaseRequest.Lines.CostingCode = item["OcrCode"].ToString();
                                oPurchaseRequest.Lines.CostingCode2 = item["OcrCode2"].ToString();
                                oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_TransferQty").Value = 0.0;
                                oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_IssueQty").Value = 0.0;
                                oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_BKTDocEntry").Value = item["BKTDocEntry"].ToString();
                                oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_BKTLineId").Value = item["BKTLineId"].ToString();

                                DataRow rowNew = tbNew.NewRow();
                                rowNew["BDocEntry"] = item["BKTDocEntry"].ToString();
                                rowNew["BLineNum"] = item["BKTLineId"].ToString();
                                rowNew["Quantity"] = item["Quantity"];

                                tbNew.Rows.Add(rowNew);


                                oPurchaseRequest.Lines.Add();
                            }
                        }
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("No Error 2", sFuncName);
                        foreach (DataRow item in dsStockDetails.Tables[1].Rows)
                        {
                            if (item["Quantity"].ToString() != string.Empty)
                            {
                                int oRSReturnValue = 0;
                                oRS.DoQuery("SELECT  dbo.AE_FN001_QUANTITYCHECKING ('" + sProject + "','" + sWarehouse + "','" + dtRequiredDate + "',"
                                    + Convert.ToDouble(item["Quantity"].ToString()) + ", '" + item["ItemCode"].ToString() + "')");

                                oRSReturnValue = Convert.ToInt32(oRS.Fields.Item(0).Value);
                                if (oRSReturnValue == 0)
                                {
                                    sItemCode = item["ItemCode"].ToString();
                                    goto CleanUp_MultipleAccessAtSameTime;
                                }
                            }
                        }

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before purchase request Add ", sFuncName);

                        //SAP Transaction
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Start the SAP Transaction ", sFuncName);
                        if (!oDICompany.InTransaction) oDICompany.StartTransaction();

                        lRetCode = oPurchaseRequest.Add();

                        if (lRetCode != 0)
                        {
                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                            if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Add the purchase request - Failure ", sFuncName);
                            sErrDesc = oDICompany.GetLastErrorDescription();
                            oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                            result objResult = new result();
                            objResult.Result = "Failure";
                            objResult.DisplayMessage = sErrDesc;
                            lstResult.Add(objResult);
                            //oDICompany.Disconnect();
                            Context.Response.Output.Write(js.Serialize(lstResult));
                        }
                        else
                        {
                            string result = BudgetStockRequestUpdate(oDICompany, tbNew);
                            if (result != "SUCCESS")
                            {
                                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                                if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Update the stock budget update - Failure ", sFuncName);
                                result objResult_update = new result();
                                objResult_update.Result = "Failure";
                                objResult_update.DisplayMessage = result;
                                lstResult.Add(objResult_update);
                                Context.Response.Output.Write(js.Serialize(lstResult));
                                goto CleanUp;
                            }
                            //BudgetStockRequestUpdate(oDICompany, tbNew);
                            result objResult = new result();
                            objResult.Result = "Success";
                            objResult.DisplayMessage = "";
                            lstResult.Add(objResult);
                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Add the purchase request - Success ", sFuncName);
                            //oDICompany.Disconnect();
                            Context.Response.Output.Write(js.Serialize(lstResult));
                        }
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Committed the Transaction ", sFuncName);
                    if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);

                CleanUp:
                    dsStockDetails.Tables.Remove(headerCopy);
                    dsStockDetails.Tables.Remove(lineCopy);
                    dsStockDetails.Clear();
                    return;

                CleanUp_MultipleAccessAtSameTime:
                    //dsStockDetails.Tables.Remove(headerCopy);
                    //dsStockDetails.Tables.Remove(lineCopy);
                    //dsStockDetails.Clear();

                    sErrDesc = "Cannot Save !!! Req. Quantity should not be greater than Available Quantity for the Item '" + sItemCode + "'.";
                    oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                    result objResultMAcess = new result();
                    objResultMAcess.Result = "Error";
                    objResultMAcess.DisplayMessage = sErrDesc;
                    lstResult.Add(objResultMAcess);
                    // oDICompany.Disconnect();
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                    if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    Context.Response.Output.Write(js.Serialize(lstResult));


                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                // oDICompany.Disconnect();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MUpdate_StockRequest(string sJsonInput)
        {
            string sFuncName = string.Empty;
            SAPbobsCOM.Documents oPurchaseRequest;
            string sItemCode = string.Empty;

            double lRetCode;
            try
            {
                sFuncName = "MUpdate_StockRequest";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<UpdateStockRequest> stockList = js.Deserialize<List<UpdateStockRequest>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (stockList.Count > 0)
                {
                    DataSet dsStockDetails = new DataSet();
                    List<result> lstResult = new List<result>();
                    DataTable tbNew = new DataTable();
                    tbNew = UpdateBudgetTable();
                    UpdateStockRequest stockDetails = stockList[0];
                    List<Json_UpdateStockRequestDetail> items = stockDetails.Items;

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Header Datatable ", sFuncName);
                    DataTable HeaderTable = Update_ConvertToStockHeaderTable(stockDetails);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Line Datatable ", sFuncName);
                    DataTable LineTable = Update_ConvertToStockLineTable(items, stockDetails.sProjectCode, stockDetails.sWarehouseCode);

                    DataTable headerCopy = HeaderTable.Copy();
                    DataTable lineCopy = LineTable.Copy();
                    dsStockDetails.Tables.Add(headerCopy);
                    dsStockDetails.Tables.Add(lineCopy);
                    dsStockDetails.Tables[0].TableName = "tblHeader";
                    dsStockDetails.Tables[1].TableName = "tblLine";

                    if (dsStockDetails != null && dsStockDetails.Tables.Count > 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                        oDICompany = oLogin.ConnectToTargetCompany(stockDetails.sCompany);

                        SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Business Object ", sFuncName);
                        oPurchaseRequest = (SAPbobsCOM.Documents)(oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting to assign the Header data ", sFuncName);

                        if (oPurchaseRequest.GetByKey(Convert.ToInt32(dsStockDetails.Tables[0].Rows[0]["DocEntry"].ToString())))
                        {
                            oPurchaseRequest.DocDate = Convert.ToDateTime(dsStockDetails.Tables[0].Rows[0]["DocDate"].ToString());
                            oPurchaseRequest.DocDueDate = Convert.ToDateTime(dsStockDetails.Tables[0].Rows[0]["DocDueDate"].ToString());
                            oPurchaseRequest.RequriedDate = Convert.ToDateTime(dsStockDetails.Tables[0].Rows[0]["ReqDate"].ToString());
                            oPurchaseRequest.Requester = dsStockDetails.Tables[0].Rows[0]["Requester"].ToString();
                            oPurchaseRequest.UserFields.Fields.Item("U_AB_FillStatus").Value = "Open";

                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting to assign the Line data ", sFuncName);
                            foreach (DataRow item in dsStockDetails.Tables[1].Rows)
                            { // Update the Purchase Request
                                if (item["DelFlag"].ToString() != "Y" && Convert.ToDouble(item["ExistingPRQty"].ToString()) > 0)
                                {
                                    oPurchaseRequest.Lines.SetCurrentLine(Convert.ToInt32(item["LineId"].ToString()));
                                    //oPurchaseRequest.Lines.ItemCode = item["ItemCode"].ToString();
                                    oPurchaseRequest.Lines.Quantity = Convert.ToDouble(item["Quantity"]);
                                    //oPurchaseRequest.Lines.WarehouseCode = item["WhsCode"].ToString();
                                    //oPurchaseRequest.Lines.CostingCode = item["OcrCode"].ToString();
                                    //oPurchaseRequest.Lines.CostingCode2 = item["OcrCode2"].ToString();
                                    //oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_TransferQty").Value = 0.0;
                                    //oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_IssueQty").Value = 0.0;
                                    //oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_BKTDocEntry").Value = item["BKTDocEntry"].ToString();
                                    //oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_BKTLineId").Value = item["BKTLineId"].ToString();

                                    DataRow rowNew = tbNew.NewRow();
                                    rowNew["BDocEntry"] = item["BKTDocEntry"].ToString();
                                    rowNew["BLineNum"] = item["BKTLineId"].ToString();
                                    //                    New Quantity                      -   Previous Quantity
                                    rowNew["Quantity"] = Convert.ToDouble(item["Quantity"]) - Convert.ToDouble(item["ExistingPRQty"]);


                                    tbNew.Rows.Add(rowNew);
                                }
                                // Adding the new in purchase request 
                                else if (Convert.ToDouble(item["ExistingPRQty"].ToString()) == 0 && item["DelFlag"].ToString() == "N")
                                {
                                    oPurchaseRequest.Lines.Add();
                                    oPurchaseRequest.Lines.ItemCode = item["ItemCode"].ToString();
                                    oPurchaseRequest.Lines.Quantity = Convert.ToDouble(item["Quantity"]);
                                    oPurchaseRequest.Lines.WarehouseCode = item["WhsCode"].ToString();
                                    oPurchaseRequest.Lines.CostingCode = item["OcrCode"].ToString();
                                    oPurchaseRequest.Lines.CostingCode2 = item["OcrCode2"].ToString();
                                    oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_TransferQty").Value = 0.0;
                                    oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_IssueQty").Value = 0.0;
                                    oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_BKTDocEntry").Value = item["BKTDocEntry"].ToString();
                                    oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_BKTLineId").Value = item["BKTLineId"].ToString();

                                    DataRow rowNew = tbNew.NewRow();
                                    rowNew["BDocEntry"] = item["BKTDocEntry"].ToString();
                                    rowNew["BLineNum"] = item["BKTLineId"].ToString();
                                    //                    New Quantity                      -   Previous Quantity
                                    rowNew["Quantity"] = Convert.ToDouble(item["Quantity"]) - Convert.ToDouble(item["ExistingPRQty"]);
                                    tbNew.Rows.Add(rowNew);
                                }
                            }
                            // Deleting the line items in purchase request
                            int iRowCount = oPurchaseRequest.Lines.Count - 1;
                            for (int iRow = iRowCount; iRow >= 0; iRow--)
                            {
                                if (dsStockDetails.Tables[1].Rows[iRow]["DelFlag"].ToString() == "Y")
                                {
                                    //  oPurchaseRequest.Lines.SetCurrentLine(iRow);
                                    oPurchaseRequest.Lines.SetCurrentLine(Convert.ToInt32(dsStockDetails.Tables[1].Rows[iRow]["LineId"].ToString()));
                                    oPurchaseRequest.Lines.Delete();

                                    DataRow rowNew = tbNew.NewRow();
                                    rowNew["BDocEntry"] = dsStockDetails.Tables[1].Rows[iRow]["BKTDocEntry"].ToString();
                                    rowNew["BLineNum"] = dsStockDetails.Tables[1].Rows[iRow]["BKTLineId"].ToString();
                                    //                 New Quantity   -   Previous Quantity
                                    rowNew["Quantity"] = 0 - Convert.ToDouble(dsStockDetails.Tables[1].Rows[iRow]["ExistingPRQty"]);
                                    tbNew.Rows.Add(rowNew);
                                }
                            }

                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before purchase request Update ", sFuncName);

                            foreach (DataRow item in dsStockDetails.Tables[1].Rows)
                            {
                                if (item["Quantity"].ToString() != string.Empty)
                                {
                                    int oRSReturnValue = 0;

                                    string sQuery = "SELECT  dbo.[AE_FN001_QUANTITYCHECKING_FORUPDATE] ('" + item["BKTDocEntry"].ToString() + "','" + item["BKTLineId"].ToString()
                                        + "','" + item["LineId"].ToString() + "','" + item["Quantity"].ToString() + "','" + item["ItemCode"].ToString() + "')";
                                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile(sQuery, sFuncName);

                                    oRS.DoQuery(sQuery);

                                    oRSReturnValue = Convert.ToInt32(oRS.Fields.Item(0).Value);
                                    if (oRSReturnValue == 0)
                                    {
                                        sItemCode = item["ItemCode"].ToString();
                                        goto CleanUp_MultipleAccessAtSameTime;
                                    }
                                }
                            }

                            //SAP Transaction
                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Start the SAP Transaction ", sFuncName);
                            if (!oDICompany.InTransaction) oDICompany.StartTransaction();

                            lRetCode = oPurchaseRequest.Update();

                            if (lRetCode != 0)
                            {
                                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                                if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After purchase request Update Failure ", sFuncName);
                                sErrDesc = oDICompany.GetLastErrorDescription();
                                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                                result objResult = new result();
                                objResult.Result = "Failure";
                                objResult.DisplayMessage = sErrDesc;
                                lstResult.Add(objResult);
                                // oDICompany.Disconnect();
                                Context.Response.Output.Write(js.Serialize(lstResult));
                            }
                            else
                            {
                                string sResult = BudgetStockRequestUpdate(oDICompany, tbNew);
                                if (sResult != "SUCCESS")
                                {
                                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                                    if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                    result objFail = new result();
                                    objFail.Result = "Failure";
                                    objFail.DisplayMessage = sResult;
                                    lstResult.Add(objFail);
                                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After stock Update Failure ", sFuncName);
                                    // oDICompany.Disconnect();
                                    Context.Response.Output.Write(js.Serialize(lstResult));
                                    goto CleanUp;
                                }
                                result objResult = new result();
                                objResult.Result = "Success";
                                objResult.DisplayMessage = "";
                                lstResult.Add(objResult);
                                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After purchase request Update Success ", sFuncName);
                                // oDICompany.Disconnect();
                                Context.Response.Output.Write(js.Serialize(lstResult));
                            }
                        }
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Committed the Transaction ", sFuncName);
                    if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                CleanUp:
                    dsStockDetails.Tables.Remove(headerCopy);
                    dsStockDetails.Tables.Remove(lineCopy);
                    dsStockDetails.Clear();

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    return;
                CleanUp_MultipleAccessAtSameTime:
                    //dsStockDetails.Tables.Remove(headerCopy);
                    //dsStockDetails.Tables.Remove(lineCopy);
                    //dsStockDetails.Clear();

                    sErrDesc = "Cannot Save !!! Req. Quantity should not be greater than Available Top up Quantity for the Item " + sItemCode + ".";
                    oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                    result objResultMAcess = new result();
                    objResultMAcess.Result = "Error";
                    objResultMAcess.DisplayMessage = sErrDesc;
                    lstResult.Add(objResultMAcess);
                    // oDICompany.Disconnect();
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                    if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    Context.Response.Output.Write(js.Serialize(lstResult));

                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MSave_GoodsIssue(string sJsonInput)
        {
            string sFuncName = string.Empty;
            SAPbobsCOM.Documents oInventoryExit;
            double lRetCode;
            string sPRDocEntry = string.Empty;
            SAPbobsCOM.Recordset oRecordSet = null;
            try
            {
                sFuncName = "MSave_GoodsIssue";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<SaveGoodsIssue> goodsList = js.Deserialize<List<SaveGoodsIssue>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (goodsList.Count > 0)
                {
                    DataSet dsIssueDetails = new DataSet();
                    List<result> lstResult = new List<result>();
                    DataTable tbNew = new DataTable();
                    tbNew = AddBudgetTable();
                    SaveGoodsIssue goodsDetails = goodsList[0];
                    List<Json_SaveGoodsIssueDetail> items = goodsDetails.Items;

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Header Datatable ", sFuncName);
                    DataTable HeaderTable = Save_ConvertToGoodsHeaderTable(goodsDetails);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Line Datatable ", sFuncName);
                    DataTable LineTable = Save_ConvertToGoodsLineTable(items, goodsDetails.sProjectCode, goodsDetails.sWarehouseCode);

                    DataTable headerCopy = HeaderTable.Copy();
                    DataTable lineCopy = LineTable.Copy();
                    dsIssueDetails.Tables.Add(headerCopy);
                    dsIssueDetails.Tables.Add(lineCopy);
                    dsIssueDetails.Tables[0].TableName = "tblHeader";
                    dsIssueDetails.Tables[1].TableName = "tblLine";

                    if (dsIssueDetails != null && dsIssueDetails.Tables.Count > 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                        oDICompany = oLogin.ConnectToTargetCompany(goodsDetails.sCompany);

                        oRecordSet = ((SAPbobsCOM.Recordset)(oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));

                        sPRDocEntry = dsIssueDetails.Tables[0].Rows[0]["DocEntry"].ToString();

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Business Object ", sFuncName);
                        oInventoryExit = (SAPbobsCOM.Documents)(oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting to assign the Header data ", sFuncName);
                        oInventoryExit.DocDate = Convert.ToDateTime(dsIssueDetails.Tables[0].Rows[0]["DocDate"].ToString());
                        oInventoryExit.DocDueDate = Convert.ToDateTime(dsIssueDetails.Tables[0].Rows[0]["DocDueDate"].ToString());
                        oInventoryExit.TaxDate = Convert.ToDateTime(dsIssueDetails.Tables[0].Rows[0]["DocDate"].ToString());

                        oInventoryExit.UserFields.Fields.Item("U_AB_FillStatus").Value = "Open";

                        foreach (DataRow item in dsIssueDetails.Tables[1].Rows)
                        {
                            if (item["Quantity"].ToString() != null)
                            {
                                oInventoryExit.Lines.ItemCode = item["ItemCode"].ToString();
                                oInventoryExit.Lines.Quantity = Convert.ToDouble(item["Quantity"]);
                                //oInventoryExit.Lines.Price = Convert.ToDouble(item["UnitPrice"].ToString());
                                oInventoryExit.Lines.WarehouseCode = item["WhsCode"].ToString();
                                oInventoryExit.Lines.CostingCode = item["OcrCode"].ToString();
                                oInventoryExit.Lines.CostingCode2 = item["OcrCode2"].ToString();

                                //------------------------------------ PR Line Update
                                oInventoryExit.Lines.UserFields.Fields.Item("U_AB_BaseEntry").Value = dsIssueDetails.Tables[0].Rows[0]["DocEntry"].ToString();
                                oInventoryExit.Lines.UserFields.Fields.Item("U_AB_BaseType").Value = "1470000113";
                                oInventoryExit.Lines.UserFields.Fields.Item("U_AB_BaseLineNum").Value = item["LineId"].ToString();
                                //------------------------------------ Budget Stock Update
                                oInventoryExit.Lines.UserFields.Fields.Item("U_AB_BKTDocEntry").Value = item["BKTDocEntry"].ToString();
                                oInventoryExit.Lines.UserFields.Fields.Item("U_AB_BKTLineId").Value = item["BKTLineId"].ToString();
                                oInventoryExit.Lines.UserFields.Fields.Item("U_AB_ValidationQty").Value = Convert.ToDouble(item["ExistingPRQty"].ToString());

                                DataRow rowNew = tbNew.NewRow();
                                rowNew["BDocEntry"] = item["BKTDocEntry"].ToString();
                                rowNew["BLineNum"] = item["BKTLineId"].ToString();
                                rowNew["Quantity"] = item["Quantity"];

                                tbNew.Rows.Add(rowNew);

                                oInventoryExit.Lines.Add();
                            }
                        }

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before purchase request Add ", sFuncName);

                        //SAP Transaction
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Start the SAP Transaction ", sFuncName);
                        if (!oDICompany.InTransaction) oDICompany.StartTransaction();

                        lRetCode = oInventoryExit.Add();

                        if (lRetCode != 0)
                        {
                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                            if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Add the Goods Issue - Failure ", sFuncName);
                            sErrDesc = oDICompany.GetLastErrorDescription();
                            oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                            result objResult = new result();
                            objResult.Result = "Failure";
                            objResult.DisplayMessage = sErrDesc;
                            lstResult.Add(objResult);
                            // oDICompany.Disconnect();
                            Context.Response.Output.Write(js.Serialize(lstResult));
                            goto cleanUp;
                        }
                        else
                        {
                            //Update the quantity inthe PR Docuemnt:
                            SAPbobsCOM.Documents oPurchaseRequest = (SAPbobsCOM.Documents)(oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseRequest));
                            oPurchaseRequest.GetByKey(Convert.ToInt32(sPRDocEntry));

                            double dQuantity = 0;

                            foreach (DataRow item in dsIssueDetails.Tables[1].Rows)
                            {
                                oPurchaseRequest.Lines.SetCurrentLine(Convert.ToInt32(item["LineId"].ToString()));
                                dQuantity = Convert.ToDouble(oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_IssueQty").Value) + Convert.ToDouble(item["Quantity"].ToString());
                                oPurchaseRequest.Lines.UserFields.Fields.Item("U_AB_IssueQty").Value = dQuantity;
                            }


                            lRetCode = oPurchaseRequest.Update();

                            if (lRetCode != 0)
                            {
                                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                                if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Add the purchase request - Failure ", sFuncName);
                                sErrDesc = oDICompany.GetLastErrorDescription();
                                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                                result objResult = new result();
                                objResult.Result = "Failure";
                                objResult.DisplayMessage = sErrDesc;
                                lstResult.Add(objResult);
                                // oDICompany.Disconnect();
                                Context.Response.Output.Write(js.Serialize(lstResult));
                                goto cleanUp;
                            }
                            else
                            {
                                oRecordSet.DoQuery("SELECT T0.[DocEntry] FROM PRQ1 T0 WHERE T0.[DocEntry] = '" + sPRDocEntry + "' and ( T0.[Quantity] -  T0.[U_AB_IssueQty] ) <> 0");
                                if (oRecordSet.RecordCount == 0)
                                {
                                    oPurchaseRequest.GetByKey(Convert.ToInt32(sPRDocEntry));
                                    oPurchaseRequest.UserFields.Fields.Item("U_AB_FillStatus").Value = "Closed";
                                    oPurchaseRequest.Update();
                                    lRetCode = oPurchaseRequest.Close();
                                    if (lRetCode != 0)
                                    {
                                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                                        if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Add the purchase request - Failure ", sFuncName);
                                        sErrDesc = oDICompany.GetLastErrorDescription();
                                        oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                                        result objResult = new result();
                                        objResult.Result = "Failure";
                                        objResult.DisplayMessage = sErrDesc;
                                        lstResult.Add(objResult);
                                        // oDICompany.Disconnect();
                                        Context.Response.Output.Write(js.Serialize(lstResult));
                                        goto cleanUp;
                                        //break;
                                    }
                                    else
                                    {
                                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Update the purchase request - Success ", sFuncName);
                                    }
                                }
                            }
                        }
                    }

                    string sResult = BudgetGoodsIssuetUpdate(oDICompany, lineCopy);
                    if (sResult != "SUCCESS")
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                        if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        result objError = new result();
                        objError.Result = "Failure";
                        objError.DisplayMessage = sResult;
                        lstResult.Add(objError);
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Update the Budget Goods Issue - Failure  ", sFuncName);
                        Context.Response.Output.Write(js.Serialize(lstResult));
                        goto cleanUp;
                    }
                    result objFinalResult = new result();
                    objFinalResult.Result = "Success";
                    objFinalResult.DisplayMessage = "";
                    lstResult.Add(objFinalResult);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                    // oDICompany.Disconnect();

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Committed the Transaction ", sFuncName);
                    if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                cleanUp:
                    dsIssueDetails.Tables.Remove(headerCopy);
                    dsIssueDetails.Tables.Remove(lineCopy);
                    dsIssueDetails.Clear();
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        public string BudgetGoodsIssuetUpdate(SAPbobsCOM.Company oDICompamy, DataTable dt)
        {
            string sFuncName = string.Empty;
            double PRqty;
            try
            {
                sFuncName = "BudgetGoodsIssuetUpdate";
                SAPbobsCOM.GeneralService oGeneralService = null;
                SAPbobsCOM.GeneralData oGeneralData = null;
                SAPbobsCOM.GeneralDataParams oGeneralDataParam = null;
                SAPbobsCOM.CompanyService oCompanyService = oDICompamy.GetCompanyService();

                oGeneralService = oCompanyService.GetGeneralService("StockBudget");
                oGeneralDataParam = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);

                var result = from r in dt.AsEnumerable() group r by new { Group = r["BKTDocEntry"] } into g select new { Group = g.Key.Group };

                foreach (var docEntry in result)
                {
                    DataTable dtFilter = new DataTable();
                    oGeneralDataParam.SetProperty("DocEntry", docEntry.Group);
                    oGeneralData = oGeneralService.GetByParams(oGeneralDataParam);

                    SAPbobsCOM.GeneralDataCollection oChildTableRows = oGeneralData.Child("AB_STKBGT1");
                    SAPbobsCOM.GeneralData oChildTableRow = default(SAPbobsCOM.GeneralData);
                    dt.DefaultView.RowFilter = "BKTDocEntry = " + docEntry.Group + "";
                    dtFilter = dt.DefaultView.ToTable();
                    foreach (DataRow item in dtFilter.Rows)
                    {
                        oChildTableRow = oChildTableRows.Item(Convert.ToInt32(item["BKTLineId"]) - 1);
                        //LINENUM_MACTHING
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Loop", sFuncName);
                        if (Convert.ToInt32(oChildTableRow.GetProperty("LineId")) == Convert.ToInt32(item["BKTLineId"]))
                        {

                            PRqty = Convert.ToDouble(oChildTableRow.GetProperty("U_IssueQty")) + Convert.ToDouble(item["Quantity"]);

                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Inside the Loop " + PRqty, sFuncName);

                            oChildTableRow.SetProperty("U_IssueQty", PRqty);
                        }
                    }
                    oGeneralService.Update(oGeneralData);
                }

                return "SUCCESS";
            }

            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                return sErrDesc;
                //throw ex;
            }
        }

        public string BudgetStockRequestUpdate(SAPbobsCOM.Company oDICompamy, DataTable dt)
        {
            string sFuncName = string.Empty;
            double PRqty, AvailQty;

            try
            {
                sFuncName = "BudgetStockRequestUpdate";
                SAPbobsCOM.GeneralService oGeneralService = null;
                SAPbobsCOM.GeneralData oGeneralData = null;
                SAPbobsCOM.GeneralDataParams oGeneralDataParam = null;
                SAPbobsCOM.CompanyService oCompanyService = oDICompamy.GetCompanyService();

                oGeneralService = oCompanyService.GetGeneralService("StockBudget");
                oGeneralDataParam = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);



                var result = from r in dt.AsEnumerable() group r by new { Group = r["BDocEntry"] } into g select new { Group = g.Key.Group };

                foreach (var docEntry in result)
                {
                    DataTable dtFilter = new DataTable();
                    oGeneralDataParam.SetProperty("DocEntry", docEntry.Group);
                    oGeneralData = oGeneralService.GetByParams(oGeneralDataParam);

                    SAPbobsCOM.GeneralDataCollection oChildTableRows = oGeneralData.Child("AB_STKBGT1");
                    SAPbobsCOM.GeneralData oChildTableRow = default(SAPbobsCOM.GeneralData);
                    dt.DefaultView.RowFilter = "BDocEntry = " + docEntry.Group + "";
                    dtFilter = dt.DefaultView.ToTable();
                    foreach (DataRow item in dtFilter.Rows)
                    {
                        oChildTableRow = oChildTableRows.Item(Convert.ToInt32(item["BLineNum"]) - 1);//oChildTableRows.Item(orset.Fields.Item("LineID").Value - 1);
                        //LINENUM_MACTHING
                        if (Convert.ToInt32(oChildTableRow.GetProperty("LineId")) == Convert.ToInt32(item["BLineNum"]))
                        {
                            //string itemcode = oChildTableRow.GetProperty("U_ItemCode").ToString();
                            PRqty = Convert.ToDouble(oChildTableRow.GetProperty("U_PRQty")) + Convert.ToDouble(item["Quantity"]);
                            AvailQty = Convert.ToDouble(oChildTableRow.GetProperty("U_AvailQty")) - Convert.ToDouble(item["Quantity"]);
                            oChildTableRow.SetProperty("U_PRQty", PRqty);
                            oChildTableRow.SetProperty("U_AvailQty", AvailQty);
                            // oGeneralService.Update(oGeneralData)
                        }
                    }

                    oGeneralService.Update(oGeneralData);
                }
                return "SUCCESS";

            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                //throw ex;
                return sErrDesc;
            }
        }

        #endregion

        #region Class

        class result
        {
            public string Result { get; set; }
            public string DisplayMessage { get; set; }
        }


        class UserInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string EmployeeName { get; set; }
            public string CompanyCode { get; set; }
            public string CompanyName { get; set; }
            public string Status { get; set; }
            public string EmpId { get; set; }
            public string Theme { get; set; }
            public string Message { get; set; }
            public string RoleId { get; set; }
            public string RoleName { get; set; }
        }

        class Json_UserInfo
        {
            public string sUserName { get; set; }
            public string sPassword { get; set; }
            public string sCompany { get; set; }
        }

        class Company
        {
            public string U_DBName { get; set; }
            public string U_CompName { get; set; }
            public string U_SAPUserName { get; set; }
            public string U_SAPPassword { get; set; }
            public string U_DBUserName { get; set; }
            public string U_DBPassword { get; set; }
            public string U_ConnString { get; set; }
            public string U_Server { get; set; }
            public string U_LicenseServer { get; set; }
        }

        class Project
        {
            public string PrcCode { get; set; }
            public string PrcName { get; set; }
        }

        class Json_Project
        {
            public string sCompany { get; set; }
            public string sCurrentUserName { get; set; }
            public DateTime dtRequiredDate { get; set; }
        }

        class WareHouse
        {
            public string WhsCode { get; set; }
            public string WhsName { get; set; }
        }

        class Json_WareHouse
        {
            public string sCompany { get; set; }
            public string sProjectCode { get; set; }
        }

        class Item
        {
            public string ItemCode { get; set; }
            public string Description { get; set; }
            public string AvailableQty { get; set; }
            public string UnitPrice { get; set; }
            public string BKTDocEntry { get; set; }
            public string BKTLineId { get; set; }
            public string OcrCode2 { get; set; }
            public string UOM { get; set; }
            public string ProjectCode { get; set; }
            public string WareHouseCode { get; set; }
            public string RequiredDate { get; set; }
        }

        class GoodsItem
        {
            public string ItemCode { get; set; }
            public string Description { get; set; }
            public string AvailableQty { get; set; }
            public string UnitPrice { get; set; }
            public string BKTDocEntry { get; set; }
            public string BKTLineId { get; set; }
            public string OcrCode2 { get; set; }
            public string UOM { get; set; }
        }

        class Json_Item
        {
            public string sCompany { get; set; }
            public string sProjectCode { get; set; }
            public string sWarehouseCode { get; set; }
            public DateTime dtRequiredDate { get; set; }
        }

        class Json_GoodsItem
        {
            public string sCompany { get; set; }
            public string sProjectCode { get; set; }
            public string sWarehouseCode { get; set; }
            public DateTime dtRequiredDate { get; set; }
            public string sDocEntry { get; set; }
        }

        class StockRequestDetails
        {
            public string ProjectCode { get; set; }
            public string ProjectName { get; set; }
            public string WhsCode { get; set; }
            public string WhsName { get; set; }
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string ReqDate { get; set; }
            public string ReqName { get; set; }
            public string Status { get; set; }
            public List<StockRequestItemDetails> Items { get; set; }
        }

        class Json_StockRequestDetails
        {
            public string sDocEntry { get; set; }
            public string sCompany { get; set; }
        }

        class StockRequestItemDetails
        {
            public string ItemCode { get; set; }
            public string Description { get; set; }
            //public string AvailableQty { get; set; }
            public string Quantity { get; set; }
            public string BKTDocEntry { get; set; }
            public string BKTLineId { get; set; }
            public string OcrCode2 { get; set; }
            public string UOM { get; set; }
        }

        class StockRequestList
        {
            public string ProjectCode { get; set; }
            public string ProjectName { get; set; }
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string ReqDate { get; set; }
            public string ReqName { get; set; }
            public string Status { get; set; }
        }

        class Json_StockRequestList
        {
            public string sCompany { get; set; }
            public string sCategory { get; set; }
            public string sUserName { get; set; }
        }

        class Json_GoodsIssueProject
        {
            public string sCompany { get; set; }
            public string sCurrentUserName { get; set; }
        }

        class GoodsIssueList
        {
            public string ProjectCode { get; set; }
            public string ProjectName { get; set; }
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string ReqDate { get; set; }
            public string ReqName { get; set; }
            public string Status { get; set; }
        }

        class Json_GoodsIssueList
        {
            public string sCompany { get; set; }
            public string sProjectCode { get; set; }
        }

        class GoodsIssueDetails
        {
            public string ProjectCode { get; set; }
            public string ProjectName { get; set; }
            public string WhsCode { get; set; }
            public string WhsName { get; set; }
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string ReqDate { get; set; }
            public string ReqName { get; set; }
            public string Status { get; set; }
            public List<GoodsIssueItemDetails> Items { get; set; }
        }

        class Json_GoodsIssueDetails
        {
            public string sDocEntry { get; set; }
            public string sCompany { get; set; }
        }

        class GoodsIssueItemDetails
        {
            public string ItemCode { get; set; }
            public string Description { get; set; }
            public string AvailableQty { get; set; }
            public string Quantity { get; set; }
            public string BKTDocEntry { get; set; }
            public string BKTLineId { get; set; }
            public string sCurrentUserName { get; set; }
            public string LineId { get; set; }
            public string UnitPrice { get; set; }
            public string IssueQty { get; set; }
            public string TransferQty { get; set; }
            public string OcrCode2 { get; set; }
            public string UOM { get; set; }
        }

        class SaveStockRequest
        {
            public string sCompany { get; set; }
            public string sCurrentUserName { get; set; }
            public string dtRequiredDate { get; set; }
            public string sProjectCode { get; set; }
            public string sWarehouseCode { get; set; }
            public List<Json_SaveStockRequestDetail> Items { get; set; }
        }

        class Json_SaveStockRequestHeader
        {
            public string sCompany { get; set; }
            public string sCurrentUserName { get; set; }
            public string sProjectCode { get; set; }
            public string sWarehouseCode { get; set; }
            public DateTime dtRequiredDate { get; set; }
        }

        class Json_SaveStockRequestDetail
        {
            public string ItemCode { get; set; }
            public string Description { get; set; }
            public string AvailableQty { get; set; }
            public string Quantity { get; set; }
            public string UnitPrice { get; set; }
            public string BKTDocEntry { get; set; }
            public string BKTLineId { get; set; }
            public string OcrCode2 { get; set; }
            //public string ProjectCode { get; set; }
            //public string WareHouseCode { get; set; }
            //public string RequiredDate { get; set; }
        }

        class UpdateStockRequest
        {
            public string sCompany { get; set; }
            public string sCurrentUserName { get; set; }
            public string dtRequiredDate { get; set; }
            public string sProjectCode { get; set; }
            public string sWarehouseCode { get; set; }
            public string sDocEntry { get; set; }
            public List<Json_UpdateStockRequestDetail> Items { get; set; }
        }

        class Json_UpdateStockRequestDetail
        {
            public string ItemCode { get; set; }
            public string Description { get; set; }
            public string AvailableQty { get; set; }
            public string Quantity { get; set; }
            public string ExistingPRQty { get; set; }
            public string LineId { get; set; }
            public string UnitPrice { get; set; }
            public string BKTDocEntry { get; set; }
            public string BKTLineId { get; set; }
            public string OcrCode2 { get; set; }
            public string DelFlag { get; set; }
        }

        class SaveGoodsIssue
        {
            public string sCompany { get; set; }
            public string sCurrentUserName { get; set; }
            public string dtRequiredDate { get; set; }
            public string sProjectCode { get; set; }
            public string sWarehouseCode { get; set; }
            public string sDocEntry { get; set; }
            public List<Json_SaveGoodsIssueDetail> Items { get; set; }
        }

        class Json_SaveGoodsIssueHeader
        {
            public string sCompany { get; set; }
            public string sCurrentUserName { get; set; }
            public string sProjectCode { get; set; }
            public string sWarehouseCode { get; set; }
            public DateTime dtRequiredDate { get; set; }
        }

        class Json_SaveGoodsIssueDetail
        {
            public string ItemCode { get; set; }
            public string Description { get; set; }
            public string AvailableQty { get; set; }
            public string Quantity { get; set; }
            public string ExistingPRQty { get; set; }
            public string LineId { get; set; }
            public string UnitPrice { get; set; }
            public string BKTDocEntry { get; set; }
            public string BKTLineId { get; set; }
            public string OcrCode2 { get; set; }
        }

        #endregion

        #region tempTables

        private DataTable Save_ConvertToStockHeaderTable(SaveStockRequest clsStock)
        {
            DataTable tbNew = new DataTable();
            tbNew = CreateHeaderTable();

            DataRow rowNew = tbNew.NewRow();
            rowNew["Requester"] = clsStock.sCurrentUserName;
            rowNew["DocDate"] = clsStock.dtRequiredDate;
            rowNew["DocDueDate"] = clsStock.dtRequiredDate;
            rowNew["ReqDate"] = clsStock.dtRequiredDate;

            tbNew.Rows.Add(rowNew);

            return tbNew.Copy();
        }

        private DataTable Save_ConvertToStockLineTable(List<Json_SaveStockRequestDetail> lstStock, string sProjectCode, string sWareHouseCode)
        {
            DataTable tbNew = new DataTable();

            tbNew = CreateLineTable();

            foreach (var item in lstStock)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["ItemCode"] = item.ItemCode;
                rowNew["Dscription"] = item.Description;
                rowNew["Quantity"] = item.Quantity;
                rowNew["AvailableQty"] = item.AvailableQty;
                rowNew["OcrCode"] = sProjectCode;
                rowNew["WhsCode"] = sWareHouseCode;
                rowNew["UnitPrice"] = item.UnitPrice;
                rowNew["BKTDocEntry"] = item.BKTDocEntry;
                rowNew["BKTLineId"] = item.BKTLineId;
                rowNew["OcrCode2"] = item.OcrCode2;
                //rowNew["ProjectCode"] = item.ProjectCode;
                //rowNew["WareHouseCode"] = item.WareHouseCode;
                //rowNew["RequiredDate"] = item.RequiredDate;

                tbNew.Rows.Add(rowNew);
            }

            return tbNew.Copy();
        }

        private DataTable Update_ConvertToStockHeaderTable(UpdateStockRequest clsStock)
        {
            DataTable tbNew = new DataTable();
            tbNew = CreateHeaderTableForUpdate();

            DataRow rowNew = tbNew.NewRow();
            rowNew["DocEntry"] = clsStock.sDocEntry;
            rowNew["Requester"] = clsStock.sCurrentUserName;
            rowNew["DocDate"] = clsStock.dtRequiredDate;
            rowNew["DocDueDate"] = clsStock.dtRequiredDate;
            rowNew["ReqDate"] = clsStock.dtRequiredDate;

            tbNew.Rows.Add(rowNew);

            return tbNew.Copy();
        }

        private DataTable Update_ConvertToStockLineTable(List<Json_UpdateStockRequestDetail> lstStock, string sProjectCode, string sWareHouseCode)
        {
            DataTable tbNew = new DataTable();

            tbNew = CreateLineTableForUpdate();

            foreach (var item in lstStock)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["ItemCode"] = item.ItemCode;
                rowNew["Dscription"] = item.Description;
                rowNew["AvailableQty"] = item.AvailableQty;
                rowNew["Quantity"] = item.Quantity;
                rowNew["ExistingPRQty"] = item.ExistingPRQty;
                rowNew["OcrCode"] = sProjectCode;
                rowNew["WhsCode"] = sWareHouseCode;
                rowNew["LineId"] = item.LineId;
                rowNew["DelFlag"] = item.DelFlag;
                rowNew["BKTLineId"] = item.BKTLineId;
                rowNew["BKTDocEntry"] = item.BKTDocEntry;
                rowNew["OcrCode2"] = item.OcrCode2;
                tbNew.Rows.Add(rowNew);
            }
            return tbNew.Copy();
        }

        private DataTable Save_ConvertToGoodsHeaderTable(SaveGoodsIssue clsStock)
        {
            DataTable tbNew = new DataTable();
            tbNew = Goods_CreateHeaderTable();

            DataRow rowNew = tbNew.NewRow();

            rowNew["Requester"] = clsStock.sCurrentUserName;
            rowNew["DocDate"] = clsStock.dtRequiredDate;
            rowNew["DocDueDate"] = clsStock.dtRequiredDate;
            rowNew["ReqDate"] = clsStock.dtRequiredDate;
            rowNew["DocEntry"] = clsStock.sDocEntry;
            //rowNew["ReqDate"] = clsStock.ReqDate;
            //rowNew["ReqName"] = clsStock.ReqName;
            //rowNew["Status"] = clsStock.Status;

            tbNew.Rows.Add(rowNew);

            return tbNew.Copy();
        }

        private DataTable Save_ConvertToGoodsLineTable(List<Json_SaveGoodsIssueDetail> lstStock, string sProjectCode, string sWareHouseCode)
        {
            DataTable tbNew = new DataTable();

            tbNew = Goods_CreateLineTable();

            foreach (var item in lstStock)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["ItemCode"] = item.ItemCode;
                rowNew["Dscription"] = item.Description;
                rowNew["AvailableQty"] = item.AvailableQty;
                rowNew["Quantity"] = item.Quantity;
                rowNew["OcrCode"] = sProjectCode;
                rowNew["WhsCode"] = sWareHouseCode;
                rowNew["OcrCode2"] = item.OcrCode2;
                rowNew["LineId"] = item.LineId;
                rowNew["UnitPrice"] = item.UnitPrice;
                rowNew["BKTDocEntry"] = item.BKTDocEntry;
                rowNew["BKTLineId"] = item.BKTLineId;
                rowNew["ExistingPRQty"] = item.ExistingPRQty;

                tbNew.Rows.Add(rowNew);
            }

            return tbNew.Copy();
        }

        private DataTable CreateHeaderTable()
        {
            DataTable tbStock = new DataTable();
            tbStock.Columns.Add("Requester");
            tbStock.Columns.Add("DocDate");
            tbStock.Columns.Add("DocDueDate");
            tbStock.Columns.Add("ReqDate");

            return tbStock;
        }

        private DataTable CreateLineTable()
        {
            DataTable tbStock = new DataTable();
            tbStock.Columns.Add("ItemCode");
            tbStock.Columns.Add("Dscription");
            tbStock.Columns.Add("AvailableQty");
            tbStock.Columns.Add("Quantity");
            tbStock.Columns.Add("OcrCode");
            tbStock.Columns.Add("WhsCode");
            tbStock.Columns.Add("OcrCode2");
            //tbStock.Columns.Add("LineId");
            tbStock.Columns.Add("UnitPrice");
            tbStock.Columns.Add("BKTDocEntry");
            tbStock.Columns.Add("BKTLineId");

            return tbStock;
        }

        private DataTable CreateHeaderTableForUpdate()
        {
            DataTable tbStock = new DataTable();
            tbStock.Columns.Add("DocEntry");
            tbStock.Columns.Add("Requester");
            tbStock.Columns.Add("DocDate");
            tbStock.Columns.Add("DocDueDate");
            tbStock.Columns.Add("ReqDate");

            return tbStock;
        }

        private DataTable CreateLineTableForUpdate()
        {
            DataTable tbStock = new DataTable();
            tbStock.Columns.Add("ItemCode");
            tbStock.Columns.Add("Dscription");
            tbStock.Columns.Add("AvailableQty");
            tbStock.Columns.Add("Quantity");
            tbStock.Columns.Add("ExistingPRQty");
            tbStock.Columns.Add("OcrCode");
            tbStock.Columns.Add("WhsCode");
            tbStock.Columns.Add("LineId");
            tbStock.Columns.Add("DelFlag");
            tbStock.Columns.Add("OcrCode2");
            tbStock.Columns.Add("BKTLineId");
            tbStock.Columns.Add("BKTDocEntry");

            return tbStock;
        }

        private DataTable Goods_CreateHeaderTable()
        {
            DataTable tbStock = new DataTable();
            tbStock.Columns.Add("DocEntry");
            tbStock.Columns.Add("Requester");
            tbStock.Columns.Add("DocDate");
            tbStock.Columns.Add("DocDueDate");
            tbStock.Columns.Add("ReqDate");

            return tbStock;
        }

        private DataTable Goods_CreateLineTable()
        {
            DataTable tbStock = new DataTable();
            tbStock.Columns.Add("ItemCode");
            tbStock.Columns.Add("Dscription");
            tbStock.Columns.Add("AvailableQty");
            tbStock.Columns.Add("Quantity");
            tbStock.Columns.Add("OcrCode");
            tbStock.Columns.Add("WhsCode");
            tbStock.Columns.Add("OcrCode2");
            tbStock.Columns.Add("LineId");
            tbStock.Columns.Add("UnitPrice");
            tbStock.Columns.Add("BKTDocEntry");
            tbStock.Columns.Add("BKTLineId");
            tbStock.Columns.Add("ExistingPRQty");

            return tbStock;
        }

        private DataTable AddBudgetTable()
        {
            DataTable tbBudget = new DataTable();

            tbBudget.Columns.Add("Quantity");
            tbBudget.Columns.Add("BDocEntry");
            tbBudget.Columns.Add("BLineNum");

            return tbBudget;
        }

        private DataTable UpdateBudgetTable()
        {
            DataTable tbBudget = new DataTable();

            tbBudget.Columns.Add("BDocEntry");
            tbBudget.Columns.Add("BLineNum");
            tbBudget.Columns.Add("Quantity");
            return tbBudget;
        }

        #endregion

        #endregion

        #region WebMethods for Customer Feedback and Show around

        #region Web Methods

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string MGet_SaveESignature(string sJsonInput, string sCompanyName, string sImageName)
        {
            DataSet dsCompanyList = oLogin.Get_CompanyList();
            string path = string.Empty;
            DataSet ds = oShowAround.Get_BitMapPath(dsCompanyList, sCompanyName);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                byte[] bytes = Convert.FromBase64String(sJsonInput);
                Image img;
                path = ds.Tables[0].Rows[0][0].ToString() + sImageName + ".Png";
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    img = Image.FromStream(ms);
                    //   img.Save(path, ImageFormat.Png);
                }

                using (var bitmapImage = new Bitmap(img.Width, img.Height))
                {
                    bitmapImage.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                    using (var g = Graphics.FromImage(bitmapImage))
                    {
                        g.Clear(Color.White);
                        g.DrawImageUnscaled(img, 0, 0);
                    }

                    // Now save b as a JPEG like you normally would
                    bitmapImage.Save(path, ImageFormat.Png);
                }
            }
            return path;
        }

        //Customer Survey
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_CustomerFeedBack_Project(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_CustomerFeedBack_Project";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sCurrentUserName = string.Empty;
                string sUserRole = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_CF_Project> lstDeserialize = js.Deserialize<List<Json_CF_Project>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_CF_Project objProject = lstDeserialize[0];
                    sCompany = objProject.sCompany;
                    sCurrentUserName = objProject.sCurrentUserName;
                    sUserRole = objProject.sUserRole;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_CF_Project() ", sFuncName);
                DataSet ds = oCustomerFeedback.Get_CF_Project(dsCompanyList, sCompany, sCurrentUserName, sUserRole);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_CF_Project() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<Project> lstProject = new List<Project>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Project _project = new Project();
                        _project.PrcCode = r["PrcCode"].ToString();
                        _project.PrcName = r["PrcName"].ToString();
                        lstProject.Add(_project);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Project list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstProject));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Project list, the serialized data is ' " + js.Serialize(lstProject) + " '", sFuncName);
                }
                else
                {
                    List<Project> lstProject = new List<Project>();
                    Context.Response.Output.Write(js.Serialize(lstProject));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_CustomerFeedback_Questions(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_CustomerFeedback_Questions";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_CF_Questions> lstDeserialize = js.Deserialize<List<Json_CF_Questions>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_CF_Questions objQuestions = lstDeserialize[0];
                    sCompany = objQuestions.sCompany;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_CF_Questions() ", sFuncName);
                DataSet ds = oCustomerFeedback.Get_CF_Questions(dsCompanyList, sCompany);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_CF_Questions() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<CF_GetQuestions> lstQuestion = new List<CF_GetQuestions>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        CF_GetQuestions _question = new CF_GetQuestions();

                        _question.Company = r["Company"].ToString();
                        _question.DocDate = r["DocDate"].ToString();
                        _question.ClientName = r["ClientName"].ToString();
                        _question.ClientPhone = r["ClientPhone"].ToString();
                        _question.Project = r["Project"].ToString();
                        _question.TImprovements = r["TImprovements"].ToString();
                        _question.TCompliments = r["TCompliments"].ToString();
                        _question.TRecommend = r["TRecommend"].ToString();
                        _question.Category = r["Category"].ToString();
                        _question.SubCategory = r["SubCategory"].ToString();
                        _question.SubCatText = r["SubCatText"].ToString();
                        _question.Question = r["Question"].ToString();
                        _question.Rating = r["Rating"].ToString();
                        _question.BaseEntry = r["BaseEntry"].ToString();
                        _question.BaseLineNum = r["BaseLineNum"].ToString();
                        lstQuestion.Add(_question);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Question list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstQuestion));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Question list, the serialized data is ' " + js.Serialize(lstQuestion) + " '", sFuncName);
                }
                else
                {
                    List<CF_GetQuestions> lstQuestions = new List<CF_GetQuestions>();
                    Context.Response.Output.Write(js.Serialize(lstQuestions));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_CustomerFeedback_List(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_CustomerFeedback_List";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProject = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_CF_ListofFeedback> lstDeserialize = js.Deserialize<List<Json_CF_ListofFeedback>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_CF_ListofFeedback objList = lstDeserialize[0];
                    sCompany = objList.sCompany;
                    sProject = objList.sProject;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_CF_ListofFeedback() ", sFuncName);
                DataSet ds = oCustomerFeedback.Get_CF_ListofFeedback(dsCompanyList, sCompany, sProject);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_CF_ListofFeedback() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<CF_ListofFeedback> lstFeedback = new List<CF_ListofFeedback>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        CF_ListofFeedback _feedbackList = new CF_ListofFeedback();
                        _feedbackList.DocEntry = r["DocEntry"].ToString();
                        _feedbackList.DocNum = r["DocNum"].ToString();
                        _feedbackList.DocDate = r["DocDate"].ToString();
                        lstFeedback.Add(_feedbackList);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the feedback list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstFeedback));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the feedback list, the serialized data is ' " + js.Serialize(lstFeedback) + " '", sFuncName);
                }
                else
                {
                    List<CF_ListofFeedback> lstProject = new List<CF_ListofFeedback>();
                    Context.Response.Output.Write(js.Serialize(lstProject));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        // Customer Survey - SAP Interaction
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MSave_CustomerFeedback(string sJsonInput)
        {
            string sFuncName = string.Empty;
            string xmlResult = string.Empty;
            string sClientName = string.Empty; string sClientPhone = string.Empty; string sProject = string.Empty; string sTImprovements = string.Empty;
            string sTCompliments = string.Empty; string sTRecommend = string.Empty; DateTime sDocDate;
            string sConvertESignature = string.Empty; string sESignature = string.Empty;
            DataSet dsQuestionDetails = new DataSet();
            DataTable dtCopy = new DataTable();
            string sDocEntry = string.Empty;
            //SAPbobsCOM.Documents oPurchaseRequest;
            //double lRetCode;

            try
            {
                sFuncName = "MSave_CustomerFeedback";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                //sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<CF_GetQuestions> questionList = js.Deserialize<List<CF_GetQuestions>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (questionList.Count > 0)
                {

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Datatable ", sFuncName);
                    DataTable dt1 = Save_CF_ConvertToHeaderDataTable(questionList);
                    DataTable dt2 = Save_CF_ConvertToLineDataTable(questionList);

                    DataTable dtCopy1 = dt1.Copy();
                    DataTable dtCopy2 = dt2.Copy();
                    dsQuestionDetails.Tables.Add(dtCopy1);
                    dsQuestionDetails.Tables.Add(dtCopy2);
                    dsQuestionDetails.Tables[0].TableName = "tblHeader";
                    dsQuestionDetails.Tables[1].TableName = "tblDetail";

                    if (dsQuestionDetails != null && dsQuestionDetails.Tables.Count > 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                        oDICompany = oLogin.ConnectToTargetCompany(questionList[0].Company);

                        Random rnd = new Random();
                        int fileId = rnd.Next(1, 100000000);
                        //Declare the objects:

                        SAPbobsCOM.GeneralService oGeneralService = null;
                        SAPbobsCOM.GeneralData oGeneralData;
                        SAPbobsCOM.GeneralDataCollection oChildren = null;
                        SAPbobsCOM.GeneralData oChild = null;
                        SAPbobsCOM.CompanyService oCompanyService = oDICompany.GetCompanyService();
                        oGeneralService = oCompanyService.GetGeneralService("CustomerSurvey");

                        oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);

                        //Assigining the values to the Varibles:
                        sDocDate = Convert.ToDateTime(dsQuestionDetails.Tables[0].Rows[0]["DocDate"].ToString());
                        sClientName = dsQuestionDetails.Tables[0].Rows[0]["ClientName"].ToString();
                        sClientPhone = dsQuestionDetails.Tables[0].Rows[0]["ClientPhone"].ToString();
                        sProject = dsQuestionDetails.Tables[0].Rows[0]["Project"].ToString();
                        sTImprovements = dsQuestionDetails.Tables[0].Rows[0]["TImprovements"].ToString();
                        sTCompliments = dsQuestionDetails.Tables[0].Rows[0]["TCompliments"].ToString();
                        sTRecommend = dsQuestionDetails.Tables[0].Rows[0]["TRecommend"].ToString();
                        sConvertESignature = questionList[0].Signature.ToString().Replace(" ", "+").ToString();
                        sESignature = MGet_SaveESignature(sConvertESignature, questionList[0].Company, "CustFdbckSignature" + fileId);

                        //Adding the Header Informations
                        oGeneralData.SetProperty("U_DocDate", sDocDate);
                        oGeneralData.SetProperty("U_ClientName", sClientName);
                        oGeneralData.SetProperty("U_ClientPhone", sClientPhone);
                        oGeneralData.SetProperty("U_Project", sProject);
                        oGeneralData.SetProperty("U_TImprovements", sTImprovements);
                        oGeneralData.SetProperty("U_TCompliments", sTCompliments);
                        oGeneralData.SetProperty("U_TRecommend", sTRecommend);
                        oGeneralData.SetProperty("U_TRemarks", dsQuestionDetails.Tables[0].Rows[0]["Remarks"].ToString());
                        oGeneralData.SetProperty("U_Owner", dsQuestionDetails.Tables[0].Rows[0]["Owner"].ToString());
                        oGeneralData.SetProperty("U_OwnName", dsQuestionDetails.Tables[0].Rows[0]["OwnerName"].ToString());
                        oGeneralData.SetProperty("U_LoginUser", dsQuestionDetails.Tables[0].Rows[0]["LoginUser"].ToString());
                        oGeneralData.SetProperty("U_ESignature", sESignature);
                        oGeneralData.SetProperty("U_SignText", sConvertESignature);

                        oChildren = oGeneralData.Child("AB_SURVEYDETAIL");

                        //Looping  the  Line Details
                        for (int iRowCount = 0; iRowCount <= dsQuestionDetails.Tables[1].Rows.Count - 1; iRowCount++)
                        {
                            oChild = oChildren.Add();

                            oChild.SetProperty("U_Category", dsQuestionDetails.Tables[1].Rows[iRowCount]["Category"].ToString());
                            oChild.SetProperty("U_SubCategory", dsQuestionDetails.Tables[1].Rows[iRowCount]["SubCategory"].ToString());
                            oChild.SetProperty("U_SubCatText", dsQuestionDetails.Tables[1].Rows[iRowCount]["SubCatText"].ToString());
                            oChild.SetProperty("U_Question", dsQuestionDetails.Tables[1].Rows[iRowCount]["Question"].ToString());

                            if (dsQuestionDetails.Tables[1].Rows[iRowCount]["Rating"].ToString() == "1")
                                oChild.SetProperty("U_Rating1", "Y");
                            else if (dsQuestionDetails.Tables[1].Rows[iRowCount]["Rating"].ToString() == "2")
                                oChild.SetProperty("U_Rating2", "Y");
                            else if (dsQuestionDetails.Tables[1].Rows[iRowCount]["Rating"].ToString() == "3")
                                oChild.SetProperty("U_Rating3", "Y");
                            else if (dsQuestionDetails.Tables[1].Rows[iRowCount]["Rating"].ToString() == "4")
                                oChild.SetProperty("U_Rating4", "Y");
                            else if (dsQuestionDetails.Tables[1].Rows[iRowCount]["Rating"].ToString() == "5")
                                oChild.SetProperty("U_Rating5", "Y");

                            oChild.SetProperty("U_BaseEntry", dsQuestionDetails.Tables[1].Rows[iRowCount]["BaseEntry"].ToString());
                            oChild.SetProperty("U_BaseLineNum", dsQuestionDetails.Tables[1].Rows[iRowCount]["BaseLineNum"].ToString());
                        }

                        //Add/Update the header document;
                        oGeneralService.Add(oGeneralData);

                        //clear the dataset
                        dsQuestionDetails.Tables.Remove(dtCopy1);
                        dsQuestionDetails.Tables.Remove(dtCopy2);
                        dsQuestionDetails.Clear();

                        //return the result to mobile
                        result objResult = new result();
                        objResult.Result = "Success";
                        objResult.DisplayMessage = "Survey document is created successfully in SAP";
                        lstResult.Add(objResult);
                        Context.Response.Output.Write(js.Serialize(lstResult));

                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                // oDICompany.Disconnect();
                Context.Response.Output.Write(js.Serialize(lstResult));
                //dsQuestionDetails.Tables.Remove(dtCopy);
                dsQuestionDetails.Clear();
            }
        }

        //Show around
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_ShowAround_ListofDocuments(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_ShowAround_ListofDocuments";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sCurrentUserName = string.Empty;
                string sOwnerCode = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_SA_DocumentList> lstDeserialize = js.Deserialize<List<Json_SA_DocumentList>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_SA_DocumentList objProject = lstDeserialize[0];
                    sCompany = objProject.sCompany;
                    sCurrentUserName = objProject.sCurrentUserName;
                    sOwnerCode = objProject.sOwnerCode;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_SA_DocumentList() ", sFuncName);
                DataSet ds = oShowAround.Get_SA_DocumentList(dsCompanyList, sCompany, sOwnerCode);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_SA_DocumentList() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<SA_DocumentList> lstDocument = new List<SA_DocumentList>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        SA_DocumentList _docList = new SA_DocumentList();
                        _docList.DocEntry = r["DocEntry"].ToString();
                        _docList.DocNum = r["DocNum"].ToString();
                        _docList.DocDate = r["DocDate"].ToString();
                        _docList.OwnerCode = r["OwnerCode"].ToString();
                        _docList.OwnerName = r["OwnerName"].ToString();
                        _docList.ProspectName = r["ProspectName"].ToString();
                        _docList.ProspectAddress = r["ProspectAddress"].ToString();
                        lstDocument.Add(_docList);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Document list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstDocument));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Document list, the serialized data is ' " + js.Serialize(lstDocument) + " '", sFuncName);
                }
                else
                {
                    List<SA_DocumentList> lstProject = new List<SA_DocumentList>();
                    Context.Response.Output.Write(js.Serialize(lstProject));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_ShowAround_DocumentDetails(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_ShowAround_DocumentDetails";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sDocEntry = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_SA_DocumentDetails> lstDeserialize = js.Deserialize<List<Json_SA_DocumentDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_SA_DocumentDetails objProject = lstDeserialize[0];
                    sCompany = objProject.sCompany;
                    sDocEntry = objProject.sDocEntry;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_SA_DocumentDetails() ", sFuncName);
                DataSet ds = oShowAround.Get_SA_DocumentDetails(dsCompanyList, sCompany, sDocEntry);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_SA_DocumentDetails() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<SA_DocumentDetails> lstDocumentDetail = new List<SA_DocumentDetails>();
                    List<Attachments> lstAttach = new List<Attachments>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        var match = lstDocumentDetail.Where(c => c.LineNum == Convert.ToString(r["LineNum"])).ToList();
                        if (match.Count == 0)
                        {
                            SA_DocumentDetails _docDetails = new SA_DocumentDetails();
                            _docDetails.Company = r["Company"].ToString();
                            _docDetails.DocEntry = r["DocEntry"].ToString();
                            _docDetails.DocNum = r["DocNum"].ToString();
                            _docDetails.DocDate = r["DocDate"].ToString();
                            _docDetails.OwnerCode = r["OwnerCode"].ToString();
                            _docDetails.OwnerName = r["OwnerName"].ToString();
                            _docDetails.Category = r["Category"].ToString();
                            _docDetails.Question = r["Question"].ToString();
                            _docDetails.Quantity = r["Quantity"].ToString();
                            _docDetails.Description = r["Description"].ToString();
                            _docDetails.LineNum = r["LineNum"].ToString();
                            _docDetails.Remarks = r["Remarks"].ToString();
                            lstDocumentDetail.Add(_docDetails);
                        }
                    }

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "Isnull(WebURL,'') <> '' AND LineNum = '1'";

                    foreach (DataRowView rowView in dv)
                    {
                        DataRow row = rowView.Row;
                        Attachments _docAttachment = new Attachments();
                        _docAttachment.WebURL = row["WebURL"].ToString();
                        _docAttachment.Remarks = row["AttachmentRemarks"].ToString();
                        lstAttach.Add(_docAttachment);
                        // Do something //
                    }

                    //foreach (DataRow item in ds.Tables[0].Rows)
                    //{
                    //    SA_DocumentDetails_Attachment _docAttachment = new SA_DocumentDetails_Attachment();
                    //    _docAttachment.WebURL = item["WebURL"].ToString();
                    //    lstAttach.Add(_docAttachment);
                    //}

                    lstDocumentDetail[0].Attachments = lstAttach;

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Document Details ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstDocumentDetail));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Document Details, the serialized data is ' " + js.Serialize(lstDocumentDetail) + " '", sFuncName);
                }
                else
                {
                    List<SA_DocumentDetails> lstProject = new List<SA_DocumentDetails>();
                    Context.Response.Output.Write(js.Serialize(lstProject));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_ShowAround_GetQuestions(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "AE_SP016_Mobile_SA_GetQuestions";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_SA_GetQuestions> lstDeserialize = js.Deserialize<List<Json_SA_GetQuestions>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_SA_GetQuestions objQuestions = lstDeserialize[0];
                    sCompany = objQuestions.sCompany;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_SA_Questions() ", sFuncName);
                DataSet ds = oShowAround.Get_SA_Questions(dsCompanyList, sCompany);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_SA_Questions() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<SA_GetQuestions> lstQuestions = new List<SA_GetQuestions>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        SA_GetQuestions _Questions = new SA_GetQuestions();
                        _Questions.Company = r["Company"].ToString();
                        _Questions.DocDate = r["DocDate"].ToString();
                        _Questions.OwnerCode = r["OwnerCode"].ToString();
                        _Questions.OwnerName = r["OwnerName"].ToString();
                        _Questions.Category = r["Category"].ToString();
                        _Questions.Question = r["Question"].ToString();
                        _Questions.Quantity = r["Quantity"].ToString();
                        _Questions.Description = r["Description"].ToString();
                        //_Questions.BaseEntry = r["BaseEntry"].ToString();
                        //_Questions.BaseLineNum = r["BaseLineNum"].ToString();
                        lstQuestions.Add(_Questions);
                    }
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Questions ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstQuestions));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Questions, the serialized data is ' " + js.Serialize(lstQuestions) + " '", sFuncName);
                }
                else
                {
                    List<SA_GetQuestions> lstProject = new List<SA_GetQuestions>();
                    Context.Response.Output.Write(js.Serialize(lstProject));
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
            //return string.Empty;
        }

        //[WebMethod(EnableSession = true)]
        ////[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
        //public string MSave_ShowAround_Attachments(byte[] byteValue, string fileName)
        //{
        //    // the byte array argument contains the content of the file
        //    // the string argument contains the name and extension
        //    // of the file passed in the byte array
        //    try
        //    {
        //        // instance a memory stream and pass the
        //        // byte array to its constructor
        //        MemoryStream ms = new MemoryStream(byteValue);

        //        // instance a filestream pointing to the
        //        // storage folder, use the original file name
        //        // to name the resulting file
        //        FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath
        //                    ("~/Attachments/") + fileName, FileMode.Create);

        //        // write the memory stream containing the original
        //        // file as a byte array to the filestream
        //        ms.WriteTo(fs);

        //        // clean up
        //        ms.Close();
        //        fs.Close();
        //        fs.Dispose();

        //        // return OK if we made it this far
        //        return "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        // return the error message if the operation fails
        //        return ex.Message.ToString();
        //    }
        //}

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public bool MSave_ShowAround_Attachments()
        {
            try
            {
                //MultipartParser parser = new MultipartParser(stream);
                //if (parser.Success)
                //{
                //    // Save the file
                //    SaveFile(parser.Filename, parser.ContentType, parser.FileContents);
                //}

                string sFuncName = string.Empty;
                sFuncName = "MSave_ShowAround_Attachments";
                //HTTP Context to get access to the submitted data
                HttpContext postedContext = HttpContext.Current;
                //        //File Collection that was submitted with posted data
                HttpFileCollection Files = postedContext.Request.Files;
                //        //Make sure a file was posted
                string fileName =
                (string)postedContext.Request.Form["filename"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Files : " + Files, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Files Count : " + Files.Count, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Files content length : " + Files[0].ContentLength, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Files content type: " + Files[0].ContentType, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("FileName : " + fileName, sFuncName);
                if (Files.Count == 1 && Files[0].ContentLength > 1)
                {
                    //The byte array we'll use to write the file with
                    byte[] binaryWriteArray = new
                    byte[Files[0].InputStream.Length];
                    //Read in the file from the InputStream
                    Files[0].InputStream.Read(binaryWriteArray, 0,
                    (int)Files[0].InputStream.Length);
                    //Open the file stream
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Creating the file in D:" + fileName, sFuncName);
                    FileStream objfilestream = new FileStream("D:\\mSite_feedback3.txt", FileMode.Create, FileAccess.ReadWrite);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Creating the file in D:", sFuncName);
                    //Write the file and close it
                    objfilestream.Write(binaryWriteArray, 0, binaryWriteArray.Length);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After writing the file in D:", sFuncName);
                    objfilestream.Close();
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed Success", sFuncName);
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex1)
            {
                throw new Exception("Problem uploading file: " + ex1.Message);
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public bool Test_WriteFile()
        {
            FileStream objfilestream = new FileStream("D:\\AAAa.txt", FileMode.Create, FileAccess.ReadWrite);
            //string activeDir = @"D:\testdir2";

            ////Create a new subfolder under the current active folder
            //string newPath = System.IO.Path.Combine(activeDir, "mySubDir");

            //// Create the subfolder
            //System.IO.Directory.CreateDirectory(newPath);

            //// Create a new file name. This example generates a random string.
            //string newFileName = System.IO.Path.GetRandomFileName() + ".txt";

            //// Combine the new file name with the path
            //newPath = System.IO.Path.Combine(newPath, newFileName);

            //// Create the file and write to it.
            //// DANGER: System.IO.File.Create will overwrite the file
            //// if it already exists. This can occur even with random file names.
            //if (!System.IO.File.Exists(newPath))
            //{
            //    using (System.IO.FileStream fs = System.IO.File.Create(newPath))
            //    {
            //        for (byte i = 0; i < 100; i++)
            //        {
            //            fs.WriteByte(i);
            //        }
            //    }
            //}
            return true;
        }

        //Show around - SAP Interaction

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MSave_ShowAround(string sJsonInput)
        {
            string sFuncName = string.Empty;
            string xmlResult = string.Empty;
            //SAPbobsCOM.Documents oPurchaseRequest;
            //double lRetCode;
            string sOwnerCode = string.Empty; string sOwnerName = string.Empty;
            DateTime dtDocDate;
            DataSet dsQuestionDetails = new DataSet();
            DataTable dtCopy = new DataTable();
            DataTable dtCopy1 = new DataTable();
            string sDocEntry = string.Empty;
            string sQueryString = string.Empty;

            try
            {
                sFuncName = "MSave_ShowAround";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                //sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<SA_GetQuestions> questionList = js.Deserialize<List<SA_GetQuestions>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (questionList.Count > 0)
                {
                    List<Attachments> items = questionList[0].Attachments;
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Datatable ", sFuncName);
                    DataTable dt = Save_SA_ConvertToDataTable(questionList);
                    DataTable dtLine = Save_SA_ConvertAttachment(items);

                    dtCopy = dt.Copy();
                    dtCopy1 = dtLine.Copy();
                    dsQuestionDetails.Tables.Add(dtCopy);
                    dsQuestionDetails.Tables.Add(dtCopy1);
                    dsQuestionDetails.Tables[0].TableName = "tblQuestions";
                    dsQuestionDetails.Tables[1].TableName = "tblAttachments";

                    if (dsQuestionDetails != null && dsQuestionDetails.Tables.Count > 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                        oDICompany = oLogin.ConnectToTargetCompany(questionList[0].Company);
                        SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                        //Declare the objects:

                        SAPbobsCOM.GeneralService oGeneralService = null;
                        SAPbobsCOM.GeneralData oGeneralData;
                        SAPbobsCOM.GeneralDataCollection oChildren = null;
                        SAPbobsCOM.GeneralData oChild = null;
                        SAPbobsCOM.CompanyService oCompanyService = oDICompany.GetCompanyService();
                        SAPbobsCOM.GeneralDataParams oGeneralParams = null;
                        oGeneralService = oCompanyService.GetGeneralService("ShowRound");

                        oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);

                        //Assigining the values to the Varibles:
                        sOwnerCode = dsQuestionDetails.Tables[0].Rows[0]["EmpId"].ToString();
                        sOwnerName = dsQuestionDetails.Tables[0].Rows[0]["OwnerName"].ToString();
                        dtDocDate = Convert.ToDateTime(dsQuestionDetails.Tables[0].Rows[0]["DocDate"].ToString());

                        //Adding the Header Informations
                        oGeneralData.SetProperty("U_DocDate", dtDocDate);
                        oGeneralData.SetProperty("U_OwnerCode", sOwnerCode);
                        oGeneralData.SetProperty("U_LoginUser", dsQuestionDetails.Tables[0].Rows[0]["OwnerCode"].ToString());
                        oGeneralData.SetProperty("U_OwnerName", sOwnerName);
                        oGeneralData.SetProperty("U_Remarks", dsQuestionDetails.Tables[0].Rows[0]["Remarks"].ToString());

                        oChildren = oGeneralData.Child("AB_SHOWAROUND1");

                        //Looping  the  Line Details

                        for (int iRowCount = 0; iRowCount <= dsQuestionDetails.Tables[0].Rows.Count - 1; iRowCount++)
                        {

                            oChild = oChildren.Add();
                            oChild.SetProperty("U_LineNum", Convert.ToString(iRowCount + 1));
                            oChild.SetProperty("U_Category", dsQuestionDetails.Tables[0].Rows[iRowCount]["Category"].ToString());
                            oChild.SetProperty("U_Question", dsQuestionDetails.Tables[0].Rows[iRowCount]["Question"].ToString());
                            double dValue;
                            string sQuantity = dsQuestionDetails.Tables[0].Rows[iRowCount]["Quantity"].ToString().Trim();
                            if (sQuantity != "")
                            {
                                dValue = Convert.ToDouble(dsQuestionDetails.Tables[0].Rows[iRowCount]["Quantity"].ToString());
                                oChild.SetProperty("U_Quantity", dValue);
                            }

                            oChild.SetProperty("U_Description", dsQuestionDetails.Tables[0].Rows[iRowCount]["Description"].ToString());
                            //oChild.SetProperty("U_BaseEntry", Convert.ToDouble(dsQuestionDetails.Tables[0].Rows[iRowCount]["BaseEntry"].ToString()));
                            //oChild.SetProperty("U_BaseLineNum", dsQuestionDetails.Tables[0].Rows[iRowCount]["BaseLineNum"].ToString());
                        }

                        foreach (DataRow item in dsQuestionDetails.Tables[1].Rows)
                        {
                            if (item["SAPURL"].ToString() != string.Empty || item["SAPURL"].ToString() != "")
                            {
                                // This is for Attachment.
                                oChildren = oGeneralData.Child("AB_SHOWAROUND2");
                                oChild = oChildren.Add();

                                oChild.SetProperty("U_Path", item["SAPURL"].ToString());
                                oChild.SetProperty("U_FileName", item["filename"].ToString());
                                oChild.SetProperty("U_AttachDate", DateTime.Now.Date);
                                oChild.SetProperty("U_Remarks", item["Remarks"].ToString());
                            }
                        }

                        //Add/Update the header document;
                        //oGeneralService.Add(oGeneralData);
                        oGeneralParams = oGeneralService.Add(oGeneralData);

                        string sSADocEntry = string.Empty;
                        sSADocEntry = System.Convert.ToString(oGeneralParams.GetProperty("DocEntry"));

                        oRS.DoQuery("UPDATE [@AB_SHOWAROUND1] SET U_Quantity = NULL WHERE U_Quantity <= 0 AND DOCENTRY = '" + sSADocEntry + "'");

                        //clear the dataset
                        dsQuestionDetails.Tables.Remove(dtCopy);
                        dsQuestionDetails.Tables.Remove(dtCopy1);
                        dsQuestionDetails.Clear();

                        //return the result to mobile
                        result objResult = new result();
                        objResult.Result = "Success";
                        objResult.DisplayMessage = "ShowRound document is created successfully in SAP";
                        lstResult.Add(objResult);
                        Context.Response.Output.Write(js.Serialize(lstResult));
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                // oDICompany.Disconnect();
                Context.Response.Output.Write(js.Serialize(lstResult));
                //dsQuestionDetails.Tables.Remove(dtCopy);
                dsQuestionDetails.Clear();

            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MUpdate_ShowAround(string sJsonInput)
        {
            string sFuncName = string.Empty;
            string xmlResult = string.Empty;
            string sOwnerCode = string.Empty; string sOwnerName = string.Empty;
            DateTime dtDocDate;
            DataSet dsQuestionDetails = new DataSet();
            DataTable dtCopy = new DataTable();
            DataTable dtCopy1 = new DataTable();
            string sDocEntry = string.Empty;

            try
            {
                sFuncName = "MUpdate_ShowAround";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                //sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<SA_DocumentDetails> questionList = js.Deserialize<List<SA_DocumentDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (questionList.Count > 0)
                {
                    List<Attachments> items = questionList[0].Attachments;
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Datatable ", sFuncName);
                    DataTable dt = Update_SA_ConvertToDataTable(questionList);
                    DataTable dtLine = Save_SA_ConvertAttachment(items);

                    dtCopy = dt.Copy();
                    dsQuestionDetails.Tables.Add(dtCopy);
                    dtCopy1 = dtLine.Copy();
                    dsQuestionDetails.Tables.Add(dtCopy1);
                    dsQuestionDetails.Tables[0].TableName = "tblQuestions";
                    dsQuestionDetails.Tables[1].TableName = "tblAttachments";

                    if (dsQuestionDetails != null && dsQuestionDetails.Tables.Count > 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                        oDICompany = oLogin.ConnectToTargetCompany(questionList[0].Company);
                        SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                        //Declare the objects:

                        SAPbobsCOM.GeneralService oGeneralService = null;
                        SAPbobsCOM.GeneralData oGeneralData;
                        SAPbobsCOM.GeneralDataCollection oChildren = null;
                        SAPbobsCOM.GeneralData oChild = null;
                        SAPbobsCOM.CompanyService oCompanyService = oDICompany.GetCompanyService();
                        SAPbobsCOM.GeneralDataParams oGeneralParams = null;
                        oGeneralService = oCompanyService.GetGeneralService("ShowRound");

                        oGeneralParams = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                        sDocEntry = dsQuestionDetails.Tables[0].Rows[0]["DocEntry"].ToString();
                        oGeneralParams.SetProperty("DocEntry", sDocEntry);
                        oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                        //Assigining the values to the Varibles:
                        sOwnerCode = dsQuestionDetails.Tables[0].Rows[0]["EmpId"].ToString();
                        sOwnerName = dsQuestionDetails.Tables[0].Rows[0]["OwnerName"].ToString();
                        dtDocDate = Convert.ToDateTime(dsQuestionDetails.Tables[0].Rows[0]["DocDate"].ToString());

                        //Adding the Header Informations
                        oGeneralData.SetProperty("U_DocDate", dtDocDate);
                        oGeneralData.SetProperty("U_OwnerCode", sOwnerCode);
                        oGeneralData.SetProperty("U_LoginUser", dsQuestionDetails.Tables[0].Rows[0]["OwnerCode"].ToString());
                        oGeneralData.SetProperty("U_OwnerName", sOwnerName);
                        oGeneralData.SetProperty("U_Remarks", dsQuestionDetails.Tables[0].Rows[0]["Remarks"].ToString());

                        oChildren = oGeneralData.Child("AB_SHOWAROUND1");

                        //Delete the previous records from the line table:
                        for (int iDocRow = oChildren.Count - 1; iDocRow >= 0; iDocRow--)
                        {
                            oChildren.Remove(iDocRow);
                        }

                        //Looping  the  Line Details

                        for (int iRowCount = 0; iRowCount <= dsQuestionDetails.Tables[0].Rows.Count - 1; iRowCount++)
                        {
                            double dValue = 0;
                            string sQuantity = dsQuestionDetails.Tables[0].Rows[iRowCount]["Quantity"].ToString().Trim();
                            if (sQuantity != "")
                            {
                                dValue = Convert.ToDouble(dsQuestionDetails.Tables[0].Rows[iRowCount]["Quantity"].ToString());
                            }

                            oChild = oChildren.Add();

                            oChild.SetProperty("U_LineNum", Convert.ToString(iRowCount + 1));
                            oChild.SetProperty("U_Category", dsQuestionDetails.Tables[0].Rows[iRowCount]["Category"].ToString());
                            oChild.SetProperty("U_Question", dsQuestionDetails.Tables[0].Rows[iRowCount]["Question"].ToString());
                            oChild.SetProperty("U_Quantity", dValue);
                            oChild.SetProperty("U_Description", dsQuestionDetails.Tables[0].Rows[iRowCount]["Description"].ToString());
                            //oChild.SetProperty("U_BaseEntry", Convert.ToDouble(dsQuestionDetails.Tables[0].Rows[iRowCount]["BaseEntry"].ToString()));
                            //oChild.SetProperty("U_BaseLineNum", dsQuestionDetails.Tables[0].Rows[iRowCount]["BaseLineNum"].ToString());
                        }

                        oChildren = oGeneralData.Child("AB_SHOWAROUND2");

                        if (dsQuestionDetails.Tables[1].Rows.Count > 0)
                        {
                            //Delete the previous records from the line table:
                            for (int iDocRow = oChildren.Count - 1; iDocRow >= 0; iDocRow--)
                            {
                                oChildren.Remove(iDocRow);
                            }
                        }

                        foreach (DataRow item in dsQuestionDetails.Tables[1].Rows)
                        {
                            if ((item["SAPURL"].ToString() != string.Empty || item["SAPURL"].ToString() != "") && item["DelFlag"].ToString() != "Y")
                            {
                                // This is for Attachment.
                                //oChildren = oGeneralData.Child("AB_SHOWAROUND2");
                                oChild = oChildren.Add();

                                oChild.SetProperty("U_Path", item["SAPURL"].ToString());
                                oChild.SetProperty("U_FileName", item["filename"].ToString());
                                oChild.SetProperty("U_AttachDate", DateTime.Now.Date);
                                oChild.SetProperty("U_Remarks", item["Remarks"].ToString());
                            }
                        }


                        //Add/Update the header document;
                        oGeneralService.Update(oGeneralData);

                        oRS.DoQuery("UPDATE [@AB_SHOWAROUND1] SET U_Quantity = NULL WHERE U_Quantity <= 0 AND DOCENTRY = '" + sDocEntry + "'");

                        //clear the dataset
                        dsQuestionDetails.Tables.Remove(dtCopy);
                        dsQuestionDetails.Tables.Remove(dtCopy1);
                        dsQuestionDetails.Clear();

                        //return the result to mobile
                        result objResult = new result();
                        objResult.Result = "Success";
                        objResult.DisplayMessage = "ShowRound document is updated successfully in SAP";
                        lstResult.Add(objResult);
                        Context.Response.Output.Write(js.Serialize(lstResult));

                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                // oDICompany.Disconnect();
                Context.Response.Output.Write(js.Serialize(lstResult));
                //dsQuestionDetails.Tables.Remove(dtCopy);
                dsQuestionDetails.Clear();
            }
        }

        #endregion

        #region Class

        class Json_CF_Project
        {
            public string sCompany { get; set; }
            public string sCurrentUserName { get; set; }
            public string sUserRole { get; set; }
        }

        class Json_CF_ListofFeedback
        {
            public string sCompany { get; set; }
            public string sProject { get; set; }
        }

        class Json_CF_Questions
        {
            public string sCompany { get; set; }
        }

        class Json_SA_DocumentList
        {
            public string sCompany { get; set; }
            public string sCurrentUserName { get; set; }
            public string sOwnerCode { get; set; }
        }

        class SA_DocumentList
        {
            public string DocEntry { get; set; }
            public string DocNum { get; set; }
            public string DocDate { get; set; }
            public string OwnerCode { get; set; }
            public string OwnerName { get; set; }
            public string ProspectName { get; set; }
            public string ProspectAddress { get; set; }
        }

        class Json_SA_DocumentDetails
        {
            public string sCompany { get; set; }
            public string sDocEntry { get; set; }
        }

        class SA_DocumentDetails
        {
            public string Company { get; set; }
            public string DocEntry { get; set; }
            public string DocNum { get; set; }
            public string DocDate { get; set; }
            public string OwnerCode { get; set; }
            public string OwnerName { get; set; }
            public string Category { get; set; }
            public string Question { get; set; }
            public string Quantity { get; set; }
            public string Description { get; set; }
            public string LineNum { get; set; }
            //public string BaseEntry { get; set; }
            //public string BaseLineNum { get; set; }
            public string EmpId { get; set; }
            public string Remarks { get; set; }
            public List<Attachments> Attachments { get; set; }
        }

        class SA_DocumentDetails_Attachment
        {
            public string WebURL { get; set; }
        }

        class SA_GetQuestions
        {
            public string Company { get; set; }
            public string DocDate { get; set; }
            public string OwnerCode { get; set; }
            public string OwnerName { get; set; }
            public string Category { get; set; }
            public string Question { get; set; }
            public string Quantity { get; set; }
            public string Description { get; set; }
            //public string BaseEntry { get; set; }
            //public string BaseLineNum { get; set; }
            public string EmpId { get; set; }
            public string Remarks { get; set; }
            public List<Attachments> Attachments { get; set; }
        }

        class Attachments
        {
            public string SAPURL { get; set; }
            public string WebURL { get; set; }
            public string filename { get; set; }
            public string Remarks { get; set; }
            public string DelFlag { get; set; }
        }

        class Json_SA_GetQuestions
        {
            public string sCompany { get; set; }
        }

        class CF_ListofFeedback
        {
            public string DocEntry { get; set; }
            public string DocNum { get; set; }
            public string DocDate { get; set; }
        }

        class CF_GetQuestions
        {
            public string Company { get; set; }
            public string DocDate { get; set; }
            public string ClientName { get; set; }
            public string ClientPhone { get; set; }
            public string Project { get; set; }
            public string TImprovements { get; set; }
            public string TCompliments { get; set; }
            public string TRecommend { get; set; }
            public string Category { get; set; }
            public string SubCategory { get; set; }
            public string SubCatText { get; set; }
            public string Question { get; set; }
            public string Rating { get; set; }
            public string BaseEntry { get; set; }
            public string BaseLineNum { get; set; }
            public string Remarks { get; set; }
            public string Owner { get; set; }
            public string OwnerName { get; set; }
            public string LoginUser { get; set; }
            public string Signature { get; set; }
        }

        #endregion

        #region temp Tables

        private DataTable Save_SA_ConvertToDataTable(List<SA_GetQuestions> lstQuestions)
        {
            DataTable tbNew = new DataTable();

            tbNew = CreateTable_Save();

            foreach (var item in lstQuestions)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["Company"] = item.Company;
                rowNew["DocDate"] = item.DocDate;
                rowNew["OwnerCode"] = item.OwnerCode;
                rowNew["OwnerName"] = item.OwnerName;
                rowNew["Category"] = item.Category;
                rowNew["Question"] = item.Question;
                rowNew["Quantity"] = item.Quantity;
                rowNew["Description"] = item.Description;
                //rowNew["BaseEntry"] = item.BaseEntry;
                //rowNew["BaseLineNum"] = item.BaseLineNum;
                rowNew["EmpId"] = item.EmpId;
                rowNew["Remarks"] = item.Remarks;
                //rowNew["SAPURL"] = item.SAPURL;
                //rowNew["filename"] = item.filename;


                tbNew.Rows.Add(rowNew);
            }
            return tbNew.Copy();
        }

        private DataTable CreateTable_Save()
        {
            DataTable tbQuestions = new DataTable();
            tbQuestions.Columns.Add("Company");
            tbQuestions.Columns.Add("DocDate");
            tbQuestions.Columns.Add("OwnerCode");
            tbQuestions.Columns.Add("OwnerName");
            tbQuestions.Columns.Add("Category");
            tbQuestions.Columns.Add("Question");
            tbQuestions.Columns.Add("Quantity");
            tbQuestions.Columns.Add("Description");
            //tbQuestions.Columns.Add("BaseEntry");
            //tbQuestions.Columns.Add("BaseLineNum");
            tbQuestions.Columns.Add("EmpId");
            tbQuestions.Columns.Add("Remarks");
            //tbQuestions.Columns.Add("SAPURL");
            //tbQuestions.Columns.Add("filename");

            return tbQuestions;
        }

        private DataTable Save_SA_ConvertAttachment(List<Attachments> lstAttachments)
        {
            DataTable tbNew = new DataTable();

            tbNew = CreateTable_AttachmentSave();

            foreach (var item in lstAttachments)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["fileName"] = item.filename;
                rowNew["SAPURL"] = item.SAPURL;
                rowNew["WebURL"] = item.WebURL;
                rowNew["Remarks"] = item.Remarks;
                rowNew["DelFlag"] = item.DelFlag;

                tbNew.Rows.Add(rowNew);
            }
            return tbNew.Copy();
        }

        private DataTable CreateTable_AttachmentSave()
        {
            DataTable tbAttachment = new DataTable();
            tbAttachment.Columns.Add("fileName");
            tbAttachment.Columns.Add("SAPURL");
            tbAttachment.Columns.Add("WebURL");
            tbAttachment.Columns.Add("Remarks");
            tbAttachment.Columns.Add("DelFlag");

            return tbAttachment;
        }

        private DataTable Update_SA_ConvertToDataTable(List<SA_DocumentDetails> lstQuestions)
        {
            DataTable tbNew = new DataTable();

            tbNew = CreateTable_Update();

            foreach (var item in lstQuestions)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["Company"] = item.Company;
                rowNew["DocEntry"] = item.DocEntry;
                rowNew["DocNum"] = item.DocNum;
                rowNew["DocDate"] = item.DocDate;
                rowNew["OwnerCode"] = item.OwnerCode;
                rowNew["OwnerName"] = item.OwnerName;
                rowNew["Category"] = item.Category;
                rowNew["Question"] = item.Question;
                rowNew["Quantity"] = item.Quantity;
                rowNew["Description"] = item.Description;
                rowNew["LineNum"] = item.LineNum;
                //rowNew["BaseEntry"] = item.BaseEntry;
                //rowNew["BaseLineNum"] = item.BaseLineNum;
                rowNew["EmpId"] = item.EmpId;
                rowNew["Remarks"] = item.Remarks;
                //rowNew["SAPURL"] = item.SAPURL;
                //rowNew["filename"] = item.filename;

                tbNew.Rows.Add(rowNew);
            }
            return tbNew.Copy();
        }

        private DataTable CreateTable_Update()
        {
            DataTable tbQuestions = new DataTable();
            tbQuestions.Columns.Add("Company");
            tbQuestions.Columns.Add("DocEntry");
            tbQuestions.Columns.Add("DocNum");
            tbQuestions.Columns.Add("DocDate");
            tbQuestions.Columns.Add("OwnerCode");
            tbQuestions.Columns.Add("OwnerName");
            tbQuestions.Columns.Add("Category");
            tbQuestions.Columns.Add("Question");
            tbQuestions.Columns.Add("Quantity");
            tbQuestions.Columns.Add("Description");
            tbQuestions.Columns.Add("LineNum");
            //tbQuestions.Columns.Add("BaseEntry");
            //tbQuestions.Columns.Add("BaseLineNum");
            tbQuestions.Columns.Add("EmpId");
            tbQuestions.Columns.Add("Remarks");
            tbQuestions.Columns.Add("SAPURL");
            tbQuestions.Columns.Add("filename");

            return tbQuestions;
        }

        private DataTable Save_CF_ConvertToHeaderDataTable(List<CF_GetQuestions> lstQuestions)
        {
            DataTable tbNew = new DataTable();

            tbNew = CreateHeaderTable_CF();

            if (lstQuestions.Count > 0)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["Company"] = lstQuestions[0].Company;
                rowNew["DocDate"] = lstQuestions[0].DocDate;
                rowNew["ClientName"] = lstQuestions[0].ClientName;
                rowNew["ClientPhone"] = lstQuestions[0].ClientPhone;
                rowNew["Project"] = lstQuestions[0].Project;
                rowNew["TImprovements"] = lstQuestions[0].TImprovements;
                rowNew["TCompliments"] = lstQuestions[0].TCompliments;
                rowNew["TRecommend"] = lstQuestions[0].TRecommend;
                rowNew["Remarks"] = lstQuestions[0].Remarks;
                rowNew["Owner"] = lstQuestions[0].Owner;
                rowNew["OwnerName"] = lstQuestions[0].OwnerName;
                rowNew["LoginUser"] = lstQuestions[0].LoginUser;

                tbNew.Rows.Add(rowNew);
            }
            return tbNew.Copy();
        }

        private DataTable Save_CF_ConvertToLineDataTable(List<CF_GetQuestions> lstQuestions)
        {
            DataTable tbNew = new DataTable();

            tbNew = CreateLineTable_CF();

            foreach (var item in lstQuestions)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["Category"] = item.Category;
                rowNew["SubCategory"] = item.SubCategory;
                rowNew["SubCatText"] = item.SubCatText;
                rowNew["Question"] = item.Question;
                rowNew["Rating"] = item.Rating;
                rowNew["BaseEntry"] = item.BaseEntry;
                rowNew["BaseLineNum"] = item.BaseLineNum;

                tbNew.Rows.Add(rowNew);
            }
            return tbNew.Copy();
        }

        private DataTable CreateHeaderTable_CF()
        {

            DataTable tbHeaderQuestions = new DataTable();
            tbHeaderQuestions.Columns.Add("Company");
            tbHeaderQuestions.Columns.Add("DocDate");
            tbHeaderQuestions.Columns.Add("ClientName");
            tbHeaderQuestions.Columns.Add("ClientPhone");
            tbHeaderQuestions.Columns.Add("Project");
            tbHeaderQuestions.Columns.Add("TImprovements");
            tbHeaderQuestions.Columns.Add("TCompliments");
            tbHeaderQuestions.Columns.Add("TRecommend");
            tbHeaderQuestions.Columns.Add("Remarks");
            tbHeaderQuestions.Columns.Add("Owner");
            tbHeaderQuestions.Columns.Add("OwnerName");
            tbHeaderQuestions.Columns.Add("LoginUser");

            return tbHeaderQuestions;
        }

        private DataTable CreateLineTable_CF()
        {
            DataTable tbLineQuestions = new DataTable();
            tbLineQuestions.Columns.Add("Category");
            tbLineQuestions.Columns.Add("SubCategory");
            tbLineQuestions.Columns.Add("SubCatText");
            tbLineQuestions.Columns.Add("Question");
            tbLineQuestions.Columns.Add("Rating");
            tbLineQuestions.Columns.Add("BaseEntry");
            tbLineQuestions.Columns.Add("BaseLineNum");

            return tbLineQuestions;
        }

        #endregion

        #endregion

        #region WebMethods for Change Password

        #region Web Methods
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void M_ChangePassword(string sJsonInput)
        {
            string sFuncName = string.Empty;
            string sQueryString = string.Empty;

            try
            {
                sFuncName = "M_ChangePassword";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  from Mobile '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<ChangePassword> PasswordList = js.Deserialize<List<ChangePassword>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (PasswordList.Count > 0)
                {
                    DataSet dsCompanyList = oLogin.Get_CompanyList();

                    Int32 iReturnResult = oLogin.ChangePassword(dsCompanyList, PasswordList[0].Company.ToString(), PasswordList[0].UserName, PasswordList[0].CurrentPwd);

                    if (iReturnResult > 0)
                    {
                        //return the result to mobile
                        result objResult = new result();
                        objResult.Result = "Success";
                        objResult.DisplayMessage = "Password Changed Successfully ";
                        lstResult.Add(objResult);
                        Context.Response.Output.Write(js.Serialize(lstResult));
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                }
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                result objResult = new result();
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                lstResult.Add(objResult);
                // oDICompany.Disconnect();
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        #endregion

        #region Class

        class ChangePassword
        {
            public string EmpId { get; set; }
            public string EmpName { get; set; }
            public string UserName { get; set; }
            public string CurrentPwd { get; set; }
            public string OldPwd { get; set; }
            public string Company { get; set; }
        }

        #endregion

        #endregion
    }
}
