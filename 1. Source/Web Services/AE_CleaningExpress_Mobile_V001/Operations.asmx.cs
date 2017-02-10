using System;
using System.Collections.Generic;
using System.Linq;
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
using SAP.Admin.DAO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Net.Mail;
using Utils;
using System.Data.SqlClient;

namespace AE_CleaningExpress_Mobile_V001
{
    /// <summary>
    /// Summary description for Operations
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]

    public class Operations : System.Web.Services.WebService
    {
        #region Objects
        public string sErrDesc = string.Empty;

        public Int16 p_iDebugMode = DEBUG_ON;

        public const Int16 RTN_SUCCESS = 1;
        public const Int16 RTN_ERROR = 0;
        public const Int16 DEBUG_ON = 1;
        public const Int16 DEBUG_OFF = 0;
        public static string signPath = ConfigurationManager.AppSettings["SignaturePath"].ToString();
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

        clsLogin oLogin = new clsLogin();
        clsShowAround oShowAround = new clsShowAround();
        clsJobScheduling oJobSchedule = new clsJobScheduling();
        clsInspectionQA oInspectionQA = new clsInspectionQA();
        clsGSLGardener oGSLGardener = new clsGSLGardener();
        clsGSLLandscape oGSLLandscape = new clsGSLLandscape();
        clsEPSPestReport oEPSPestReport = new clsEPSPestReport();

        clsLog oLog = new clsLog();
        DocumentXML oDocxml = new DocumentXML();
        List<result> lstResult = new List<result>();
        List<AttachmentResult> lstAttResult = new List<AttachmentResult>();
        List<AttachmentResultWithRemarks> lstAttResultWithRemarks = new List<AttachmentResultWithRemarks>();
        List<AttachmentURLWithRemarks> lstAttachmentURL = new List<AttachmentURLWithRemarks>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        SAPbobsCOM.Company oDICompany;

        #endregion

        #region WebMethods for Job Scheduling

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
        public void MGet_JobSchedule_Project(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_JobSchedule_Project";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sCurrentUserName = string.Empty;
                string sUserRole = string.Empty;

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
                    sUserRole = objProject.sUserRole;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_JobSchedule_Project() ", sFuncName);
                DataSet ds = oJobSchedule.Get_JobSchedule_Project(dsCompanyList, sCompany, sCurrentUserName, sUserRole);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_JobSchedule_Project() ", sFuncName);
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
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With Success  ", sFuncName);
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
        public void MGet_JobSchedule_ListofJobs(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_JobSchedule_ListofJobs";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_ListofJobs> lstDeserialize = js.Deserialize<List<Json_ListofJobs>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_ListofJobs objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sProjectCode = objProject.ProjectCode;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_JobSchedule_ListofJobs() ", sFuncName);
                DataSet ds = oJobSchedule.Get_JobSchedule_ListofJobs(dsCompanyList, sCompany, sProjectCode);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_JobSchedule_ListofJobs() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<ListofJobs> lstJobs = new List<ListofJobs>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ListofJobs _jobs = new ListofJobs();
                        _jobs.DocNum = r["DocNum"].ToString();
                        _jobs.DocEntry = r["DocEntry"].ToString();
                        _jobs.DocStatus = r["DocStatus"].ToString();
                        _jobs.PrcCode = r["PrcCode"].ToString();
                        _jobs.PrcName = r["PrcName"].ToString();
                        _jobs.StartDate = ChangeDateWithPrefix0(r["StartDate"].ToString());
                        _jobs.EndDate = ChangeDateWithPrefix0(r["EndDate"].ToString());
                        lstJobs.Add(_jobs);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Job list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstJobs));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Job list, the serialized data is ' " + js.Serialize(lstJobs) + " '", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With Success  ", sFuncName);
                }
                else
                {
                    List<ListofJobs> lstJobs = new List<ListofJobs>();
                    Context.Response.Output.Write(js.Serialize(lstJobs));
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
        public void MGet_JobSchedule_JobDetails(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_JobSchedule_JobDetails";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sDocEntry = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_JobDetails> lstDeserialize = js.Deserialize<List<Json_JobDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_JobDetails objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sDocEntry = objProject.DocEntry;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_JobSchedule_JobDetails() ", sFuncName);
                DataSet ds = oJobSchedule.Get_JobSchedule_JobDetails(dsCompanyList, sCompany, sDocEntry);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_JobSchedule_JobDetails() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<JobDetails> lstJobs = new List<JobDetails>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        JobDetails _jobs = new JobDetails();
                        _jobs.LineId = r["LineId"].ToString();
                        _jobs.VisOrder = r["VisOrder"].ToString();
                        _jobs.ScheduledDate = ChangeDateWithPrefix0(r["ScheduledDate"].ToString());
                        _jobs.DayStatus = r["DayStatus"].ToString();
                        _jobs.CleaningType = r["CleaningType"].ToString();
                        _jobs.Frequency = r["Frequency"].ToString();
                        _jobs.StartTime = r["StartTime"].ToString();
                        _jobs.EndTime = r["EndTime"].ToString();
                        lstJobs.Add(_jobs);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Job Details ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstJobs));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Job Details, the serialized data is ' " + js.Serialize(lstJobs) + " '", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With Success  ", sFuncName);
                }
                else
                {
                    List<JobDetails> lstJobs = new List<JobDetails>();
                    Context.Response.Output.Write(js.Serialize(lstJobs));
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
        public void MGet_JobSchedule_ScheduledDayInfo(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_JobSchedule_ScheduledDayInfo";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sDocEntry = string.Empty;
                DateTime dtScheduledDate = new DateTime();
                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_ScheduleDayInfo> lstDeserialize = js.Deserialize<List<Json_ScheduleDayInfo>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_ScheduleDayInfo objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sDocEntry = objProject.DocEntry;
                    dtScheduledDate = objProject.ScheduledDate;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_JobSchedule_JobDetails() ", sFuncName);
                DataSet ds = oJobSchedule.Get_JobSchedule_ScheduledDayInfo(dsCompanyList, sCompany, sDocEntry, dtScheduledDate);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_JobSchedule_JobDetails() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<ScheduleDayInfo> lstScheduledInfo = new List<ScheduleDayInfo>();
                    List<JS_Attachments> lstAttach = new List<JS_Attachments>();

                    string sItem = "CleaningType = '" + ds.Tables[0].Rows[0]["CleaningType"].ToString() + "'";

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = sItem;

                    foreach (DataRowView rowView in dv)
                    {
                        DataRow row = rowView.Row;
                        if (row["WebURL"].ToString() != string.Empty)
                        {
                            JS_Attachments _docAttachment = new JS_Attachments();
                            _docAttachment.WebURL = row["WebURL"].ToString();
                            _docAttachment.Remarks = row["AttachmentRemarks"].ToString();
                            lstAttach.Add(_docAttachment);
                        }
                        // Do something //
                    }

                    DataTable dtRemove = ds.Tables[0];
                    dtRemove.Columns.Remove("WebURL");
                    dtRemove.Columns.Remove("AttachmentRemarks");

                    DataView view = new DataView(dtRemove);
                    DataTable distinctValues = view.ToTable(true, "DocEntry", "Company", "ActivityLineNum", "DayLineNum", "StartDate", "EndDate", "Sch3LineId",
                        "Sch3VisOrder", "Sch2VisOrder", "ScheduledDate", "DayStatus", "CompletedDate", "CompletedBy", "CleaningType", "Location", "Sch1StartDate", "StartTime",
                        "EndTime", "Frequency", "ActualStartTime", "ActualEndTime", "TaskStatus", "Reason", "AlertHQ", "NewScheduleDate", "SupervisorName", "SupervisorSign",
                        "SignedDate1", "ClientName", "ClientSign", "SignedDate2", "AtcEntry", "ProcCode", "PreparedBy", "PreparedDate", "PreparedById", "CmplStartDate",
                        "CmplEndDate", "TaskCompletedBy", "InspectedBy", "VerifiedBy", "Remarks");

                    //DataTable dt = dtRemove.AsEnumerable()
                    //       .GroupBy(r => new { CleaningType = r["CleaningType"], Location = r["Location"], TaskStatus = r["TaskStatus"] }).Select(s=>s.
                    //.Select(g => g.OrderBy(r => r["DocEntry"])).First().CopyToDataTable();

                    foreach (DataRow r in distinctValues.Rows)
                    {
                        ScheduleDayInfo _schInfo = new ScheduleDayInfo();
                        _schInfo.DocEntry = r["DocEntry"].ToString();
                        _schInfo.Company = r["Company"].ToString();
                        _schInfo.ActivityLineNum = r["ActivityLineNum"].ToString();
                        _schInfo.DayLineNum = r["DayLineNum"].ToString();
                        _schInfo.StartDate = ChangeDateWithPrefix0(r["StartDate"].ToString());
                        _schInfo.EndDate = ChangeDateWithPrefix0(r["EndDate"].ToString());
                        _schInfo.Sch3LineId = r["Sch3LineId"].ToString();
                        _schInfo.Sch3VisOrder = r["Sch3VisOrder"].ToString();
                        _schInfo.Sch2VisOrder = r["Sch2VisOrder"].ToString();
                        _schInfo.ScheduledDate = ChangeDateWithPrefix0(r["ScheduledDate"].ToString());
                        _schInfo.DayStatus = r["DayStatus"].ToString();
                        _schInfo.CompletedDate = ChangeDateWithPrefix0(r["CompletedDate"].ToString());
                        _schInfo.CompletedBy = r["CompletedBy"].ToString();
                        _schInfo.CleaningType = r["CleaningType"].ToString();
                        _schInfo.Location = r["Location"].ToString();
                        _schInfo.Sch1StartDate = ChangeDateWithPrefix0(r["Sch1StartDate"].ToString());
                        _schInfo.StartTime = r["StartTime"].ToString();
                        _schInfo.EndTime = r["EndTime"].ToString();
                        _schInfo.Frequency = r["Frequency"].ToString();
                        _schInfo.ActualStartTime = r["ActualStartTime"].ToString();
                        _schInfo.ActualEndTime = r["ActualEndTime"].ToString();
                        _schInfo.TaskStatus = r["TaskStatus"].ToString();
                        _schInfo.Reason = r["Reason"].ToString();
                        _schInfo.AlertHQ = r["AlertHQ"].ToString();
                        _schInfo.NewScheduleDate = ChangeDateWithPrefix0(r["NewScheduleDate"].ToString());


                        _schInfo.SupervisorName = r["SupervisorName"].ToString();
                        _schInfo.SupervisorSign = r["SupervisorSign"].ToString();
                        _schInfo.SignedDate1 = ChangeDateWithPrefix0(r["SignedDate1"].ToString());
                        _schInfo.ClientName = r["ClientName"].ToString();
                        _schInfo.ClientSign = r["ClientSign"].ToString();
                        _schInfo.SignedDate2 = ChangeDateWithPrefix0(r["SignedDate2"].ToString());

                        //_schInfo.ESignature = r["ESignature"].ToString();
                        _schInfo.AtcEntry = r["AtcEntry"].ToString();
                        _schInfo.ProcCode = r["ProcCode"].ToString();
                        _schInfo.PreparedBy = r["PreparedBy"].ToString();
                        _schInfo.PreparedById = r["PreparedById"].ToString();
                        _schInfo.PreparedDate = ChangeDateWithPrefix0(r["PreparedDate"].ToString());
                        _schInfo.CompletedStartDate = ChangeDateWithPrefix0(r["CmplStartDate"].ToString());
                        _schInfo.CompletedEndDate = ChangeDateWithPrefix0(r["CmplEndDate"].ToString());
                        _schInfo.TaskCompleteBy = r["TaskCompletedBy"].ToString();
                        _schInfo.InspectedBy = r["InspectedBy"].ToString();
                        _schInfo.VerifiedBy = r["VerifiedBy"].ToString();
                        _schInfo.remarks = r["Remarks"].ToString();
                        if (lstAttach.Count > 0)
                        {
                            _schInfo.Attachments = lstAttach;
                        }

                        lstScheduledInfo.Add(_schInfo);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Scheduled day Info ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstScheduledInfo));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Scheduled day Info, the serialized data is ' " + js.Serialize(lstScheduledInfo) + " '", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With Success  ", sFuncName);
                }
                else
                {
                    List<ScheduleDayInfo> lstScheduledInfo = new List<ScheduleDayInfo>();
                    Context.Response.Output.Write(js.Serialize(lstScheduledInfo));
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
        public void MJobCompletion_CreatePDF(string sJsonInput)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "MJobCompletion_CreatePDF";
                sProcName = "AB_SP_RPT004_JobCompletionForm";
                string sCompany = string.Empty;
                string sFromDate = string.Empty;
                string sToDate = string.Empty;
                string sProject = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_JobCompletion> lstDeserialize = js.Deserialize<List<Json_JobCompletion>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_JobCompletion objCreatePDF = lstDeserialize[0];
                    sCompany = objCreatePDF.Company;
                    sFromDate = objCreatePDF.FromDate;
                    sToDate = objCreatePDF.ToDate;
                    sProject = objCreatePDF.Project;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);

                ReportDocument cryRpt = new ReportDocument();
                ReportDocument ERRPT = new ReportDocument();
                CrystalDecisions.Shared.ConnectionInfo objConInfo = new CrystalDecisions.Shared.ConnectionInfo();
                CrystalDecisions.Shared.TableLogOnInfo oLogonInfo = new CrystalDecisions.Shared.TableLogOnInfo();
                //Dim ConInfo As New CrystalDecisions.Shared.TableLogOnInfo
                int intCounter = 0;
                //Dim Formula As String
                if (dsCompanyList != null && dsCompanyList.Tables.Count > 0)
                {
                    oDTView = dsCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@Date", sFromDate), Data.CreateParameter("@ToDate", sToDate), Data.CreateParameter("@ProjectCode", sProject));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        //return "No Data in OUSR table for the selected Company";
                    }


                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Creating PDF file for Project Code : " + sProject, sFuncName);
                    //Create PDF file

                    string sFileName = "/PDF/JOBCompletion/" + sFromDate.Replace("/", "") + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + DateTime.Now.Millisecond + ".pdf";
                    string directory = HttpContext.Current.Server.MapPath("TEMP") + "/PDF/JOBCompletion";
                    string AttachFile = HttpContext.Current.Server.MapPath("TEMP") + sFileName;
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    cryRpt.Load(Server.MapPath("Report") + "/Job Completion Report.rpt");

                    ParameterValues crParameterValues = new ParameterValues();
                    ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                    string SerNam = null;
                    string DbName = null;
                    string UID = null;
                    SerNam = oDTView[0]["U_Server"].ToString();
                    DbName = oDTView[0]["U_DBName"].ToString();
                    UID = oDTView[0]["U_DBUserName"].ToString();

                    oLogonInfo.ConnectionInfo.ServerName = SerNam;
                    oLogonInfo.ConnectionInfo.DatabaseName = DbName;
                    oLogonInfo.ConnectionInfo.UserID = UID;
                    oLogonInfo.ConnectionInfo.Password = oDTView[0]["U_DBPassword"].ToString();

                    for (intCounter = 0; intCounter <= cryRpt.Database.Tables.Count - 1; intCounter++)
                    {
                        cryRpt.Database.Tables[intCounter].ApplyLogOnInfo(oLogonInfo);
                    }

                    cryRpt.SetParameterValue("@Date", sFromDate);
                    cryRpt.SetParameterValue("@ToDate", sToDate);
                    cryRpt.SetParameterValue("@ProjectCode", sProject);

                    ExportOptions CrExportOptions = default(ExportOptions);
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    ExcelFormatOptions CrExcelFormat = new ExcelFormatOptions();
                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                    ExcelFormatOptions CrExcelTypeOptions = new ExcelFormatOptions();

                    CrDiskFileDestinationOptions.DiskFileName = AttachFile;
                    CrExportOptions = cryRpt.ExportOptions;
                    var _with1 = CrExportOptions;
                    _with1.ExportDestinationType = ExportDestinationType.DiskFile;
                    _with1.ExportFormatType = ExportFormatType.PortableDocFormat;
                    _with1.DestinationOptions = CrDiskFileDestinationOptions;
                    _with1.FormatOptions = CrFormatTypeOptions;
                    cryRpt.Export();

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("PDF Created successfully for Project Code : " + sProject, sFuncName);
                    result objResult = new result();
                    objResult.Result = "Success";
                    objResult.DisplayMessage = "/TEMP" + sFileName;
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else
                {
                    oLog.WriteToErrorLogFile("There is No Company List in the Holding Company", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR : There is No Company List in the Holding Company ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "Failure";
                    objResult.DisplayMessage = "There is No Company List in the Holding Company";
                    lstResult.Add(objResult);
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
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MonthlyJobSchedule_CreatePDF(string sJsonInput)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "MonthlyJobSchedule_CreatePDF";
                sProcName = "AB_SP_RPT009_MonthlyJobSchedule";
                string sCompany = string.Empty;
                string sMonthName = string.Empty;
                string sProject = string.Empty;
                string sYear = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_MonthlyJobSchedule> lstDeserialize = js.Deserialize<List<Json_MonthlyJobSchedule>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_MonthlyJobSchedule objCreatePDF = lstDeserialize[0];
                    sCompany = objCreatePDF.Company;
                    sMonthName = objCreatePDF.MonthName;
                    sProject = objCreatePDF.Project;
                    sYear = objCreatePDF.Year;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);

                ReportDocument cryRpt = new ReportDocument();
                ReportDocument ERRPT = new ReportDocument();
                CrystalDecisions.Shared.ConnectionInfo objConInfo = new CrystalDecisions.Shared.ConnectionInfo();
                CrystalDecisions.Shared.TableLogOnInfo oLogonInfo = new CrystalDecisions.Shared.TableLogOnInfo();
                //Dim ConInfo As New CrystalDecisions.Shared.TableLogOnInfo
                int intCounter = 0;
                //Dim Formula As String
                if (dsCompanyList != null && dsCompanyList.Tables.Count > 0)
                {
                    oDTView = dsCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@MONTHNAME", sMonthName), Data.CreateParameter("@PROJECT", sProject), Data.CreateParameter("@Year", sYear));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        //return "No Data in OUSR table for the selected Company";
                    }


                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Creating PDF file for Month : " + sMonthName + " And for Year : " + sYear + " And for the Project : " + sProject, sFuncName);
                    //Create PDF file

                    string sFileName = "/PDF/MonthlyJobSchedule/" + sMonthName + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + DateTime.Now.Millisecond + ".pdf";
                    string directory = HttpContext.Current.Server.MapPath("TEMP") + "/PDF/MonthlyJobSchedule";
                    string AttachFile = HttpContext.Current.Server.MapPath("TEMP") + sFileName;
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    cryRpt.Load(Server.MapPath("Report") + "/Monthly JobSchedule Report.rpt");

                    ParameterValues crParameterValues = new ParameterValues();
                    ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                    string SerNam = null;
                    string DbName = null;
                    string UID = null;
                    SerNam = oDTView[0]["U_Server"].ToString();
                    DbName = oDTView[0]["U_DBName"].ToString();
                    UID = oDTView[0]["U_DBUserName"].ToString();

                    oLogonInfo.ConnectionInfo.ServerName = SerNam;
                    oLogonInfo.ConnectionInfo.DatabaseName = DbName;
                    oLogonInfo.ConnectionInfo.UserID = UID;
                    oLogonInfo.ConnectionInfo.Password = oDTView[0]["U_DBPassword"].ToString();

                    for (intCounter = 0; intCounter <= cryRpt.Database.Tables.Count - 1; intCounter++)
                    {
                        cryRpt.Database.Tables[intCounter].ApplyLogOnInfo(oLogonInfo);
                    }

                    cryRpt.SetParameterValue("@MONTHNAME", sMonthName);
                    cryRpt.SetParameterValue("@PROJECT", sProject);
                    cryRpt.SetParameterValue("@Year", Convert.ToInt32(sYear));

                    ExportOptions CrExportOptions = default(ExportOptions);
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    ExcelFormatOptions CrExcelFormat = new ExcelFormatOptions();
                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                    ExcelFormatOptions CrExcelTypeOptions = new ExcelFormatOptions();

                    CrDiskFileDestinationOptions.DiskFileName = AttachFile;
                    CrExportOptions = cryRpt.ExportOptions;
                    var _with1 = CrExportOptions;
                    _with1.ExportDestinationType = ExportDestinationType.DiskFile;
                    _with1.ExportFormatType = ExportFormatType.PortableDocFormat;
                    _with1.DestinationOptions = CrDiskFileDestinationOptions;
                    _with1.FormatOptions = CrFormatTypeOptions;
                    cryRpt.Export();

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("PDF Created successfully for for Month : " + sMonthName + " And for Year : " + sYear + "  And for the Project : " + sProject, sFuncName);
                    result objResult = new result();
                    objResult.Result = "Success";
                    objResult.DisplayMessage = "/TEMP" + sFileName;
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else
                {
                    oLog.WriteToErrorLogFile("There is No Company List in the Holding Company", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR : There is No Company List in the Holding Company ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "Failure";
                    objResult.DisplayMessage = "There is No Company List in the Holding Company";
                    lstResult.Add(objResult);
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
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        //SAP Interaction
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void Msave_JobSchedule(string sJsonInput)
        {
            string sFuncName = string.Empty;
            string xmlResult = string.Empty;
            int SaveCount = 0;
            int UpdateCount = 0;

            try
            {
                sFuncName = "Msave_JobSchedule";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                //sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<SaveJobSchedule> JobSchedule = js.Deserialize<List<SaveJobSchedule>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                oDICompany = oLogin.ConnectToTargetCompany(JobSchedule[0].Company.ToString());
                SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                for (int i = 0; i <= JobSchedule.Count - 1; i++)
                {
                    List<JS_Attachments> attachments = JobSchedule[i].Attachments;

                    DataTable attachmentTable = Save_JS_ConvertAttachment(attachments);
                    DataTable attachmentCopy = attachmentTable.Copy();

                    //If there is Reschdule the following code will execute otherwise else part will work.
                    if (JobSchedule[i].NewScheduleDate != string.Empty && JobSchedule[i].NewScheduleDate != null && JobSchedule[i].NewScheduleDate != "")
                    {
                        SaveJobSchedule JobScheduleNew = new SaveJobSchedule();

                        JobScheduleNew = JobSchedule[i];
                        DataTable dtTable1 = new DataTable();
                        // ProcCode, StartDate, EndDate,PreparedBy, PreparedDate, PreparedById
                        DataTable dtTable2 = new DataTable();
                        // Location,CleaningType, Sch1StartDate, StartTime, EndTime, Frequency
                        DataTable dtTable3 = new DataTable();
                        // ScheduledDate, DayStatus
                        DataTable dtTable4 = new DataTable();
                        // ScheduledDate
                        DataTable dtTable5 = new DataTable();
                        // DayLineNum, ActivityLineNum, TaskStatus

                        dtTable1 = Save_SchTable0(JobScheduleNew);
                        dtTable2 = Save_SchTable1(JobScheduleNew);

                        //oDICompany = oLogin.ConnectToTargetCompany(JobScheduleNew.Company);

                        //SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        string FinalStartDate = string.Empty; string FinalEndDate = string.Empty;

                        // For Changing the Date format to dd/MM/yyyy
                        string startDate = dtTable1.Rows[0]["StartDate"].ToString();
                        string endDate = dtTable1.Rows[0]["EndDate"].ToString();
                        string[] sStartDate = startDate.Split('/');
                        string[] sEndDateDate = endDate.Split('/');
                        if (sStartDate[0].Length == 1)
                        {
                            sStartDate[0] = '0' + sStartDate[0];
                        }

                        if (sStartDate[1].Length == 1)
                        {
                            sStartDate[1] = '0' + sStartDate[1];
                        }

                        FinalStartDate = sStartDate[1] + '/' + sStartDate[0] + '/' + sStartDate[2];


                        if (sEndDateDate[0].Length == 1)
                        {
                            sEndDateDate[0] = '0' + sEndDateDate[0];
                        }

                        if (sEndDateDate[1].Length == 1)
                        {
                            sEndDateDate[1] = '0' + sEndDateDate[1];
                        }

                        FinalEndDate = sEndDateDate[1] + '/' + sEndDateDate[0] + '/' + sStartDate[2];

                        string sQuery = "DECLARE @MinDate DATE = CONVERT(DATE,'" + FinalStartDate + "',103), @MaxDate DATE = CONVERT(DATE,'" + FinalEndDate + "',103);" +
                        " SELECT  TOP (DATEDIFF(DD, @MinDate, @MaxDate) + 1) Date = convert(varchar, DATEADD(DD, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " +
                        " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b;";

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("SQL Query " + sQuery, sFuncName);
                        oRS.DoQuery(sQuery);

                        if (oRS.RecordCount > 0)
                        {
                            dtTable3 = Save_SchTable2(ConvertRecordset(oRS), JobScheduleNew.DayStatus);
                        }

                        dtTable4 = SchTable4();
                        dtTable5 = SchTable5();
                        int DayLineNum = 0;
                        foreach (DataRow item in dtTable2.Rows)
                        {
                            DayLineNum = DayLineNum + 1;
                            DataTable dt = GenerateTable(oDICompany, item[5].ToString(), FinalStartDate, FinalEndDate);
                            foreach (DataRow dr in dt.Rows)
                            {
                                DataRow rowNew = dtTable4.NewRow();
                                rowNew["ScheduledDate"] = dr[0];
                                dtTable4.Rows.Add(rowNew);

                                DateTime dtConvert1 = Convert.ToDateTime(dr[0]);
                                int ActLineNum = 0;
                                foreach (DataRow dr3 in dtTable3.Rows)
                                {
                                    ActLineNum = ActLineNum + 1;
                                    DateTime dtConvert2 = Convert.ToDateTime(dr3[0]);
                                    if (dtConvert1.Date == dtConvert2.Date)
                                    {
                                        DataRow rowNew1 = dtTable5.NewRow();
                                        rowNew1["DayLineNum"] = ActLineNum; // jobSch 2 LineId
                                        rowNew1["ActivityLineNum"] = DayLineNum;// JobSch 1 LineId
                                        rowNew1["TaskStatus"] = dtTable1.Rows[0]["TaskStatus"].ToString(); // Task Status
                                        dtTable5.Rows.Add(rowNew1);
                                    }
                                }
                            }
                        }

                        if (dtTable1 != null && dtTable1.Rows.Count > 0)
                        {
                            //Declare the objects:

                            if (!oDICompany.InTransaction) oDICompany.StartTransaction();

                            SAPbobsCOM.GeneralService oGeneralService = null;
                            SAPbobsCOM.GeneralData oGeneralData;
                            SAPbobsCOM.GeneralDataCollection oChildren = null;
                            SAPbobsCOM.GeneralData oChild = null;
                            SAPbobsCOM.CompanyService oCompanyService = oDICompany.GetCompanyService();
                            oGeneralService = oCompanyService.GetGeneralService("JobSchedule");

                            oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);

                            //Adding the Header Informations
                            oGeneralData.SetProperty("U_DocDate", DateTime.Now.Date);
                            oGeneralData.SetProperty("U_OcrCode", dtTable1.Rows[0]["ProcCode"].ToString());
                            oGeneralData.SetProperty("U_DocStatus", "O");
                            oGeneralData.SetProperty("U_StartDate", Convert.ToDateTime(dtTable1.Rows[0]["StartDate"].ToString()).Date);
                            oGeneralData.SetProperty("U_EndDate", Convert.ToDateTime(dtTable1.Rows[0]["EndDate"].ToString()).Date);
                            oGeneralData.SetProperty("U_PreparedBy", dtTable1.Rows[0]["PreparedBy"].ToString());
                            oGeneralData.SetProperty("U_PreparedDate", dtTable1.Rows[0]["PreparedDate"].ToString());
                            oGeneralData.SetProperty("U_PreparedByID", dtTable1.Rows[0]["PreparedById"].ToString());
                            oGeneralData.SetProperty("U_ApprovedBy", dtTable1.Rows[0]["PreparedBy"].ToString());
                            oGeneralData.SetProperty("U_ApprovedDate", dtTable1.Rows[0]["PreparedDate"].ToString());
                            oGeneralData.SetProperty("U_ApprovedByID", dtTable1.Rows[0]["PreparedById"].ToString());

                            oChildren = oGeneralData.Child("AB_JOBSCH1");
                            //Looping  the  Line Details
                            for (int iRowCount = 0; iRowCount <= dtTable2.Rows.Count - 1; iRowCount++)
                            {
                                oChild = oChildren.Add();

                                string[] startTimeSplit = dtTable2.Rows[iRowCount]["StartTime"].ToString().Split(':');
                                string[] endTimeSplit = dtTable2.Rows[iRowCount]["EndTime"].ToString().Split(':');

                                DateTime dtInTime = DateTime.Now.Date;
                                if (startTimeSplit[1].Substring(3, 2).ToUpper() == "PM")
                                {
                                    dtInTime = dtInTime.AddHours(Convert.ToDouble(startTimeSplit[0]) + 12);
                                }
                                else
                                {
                                    dtInTime = dtInTime.AddHours(Convert.ToDouble(startTimeSplit[0]));
                                }
                                dtInTime = dtInTime.AddMinutes(Convert.ToDouble(startTimeSplit[1].Substring(0, 2)));

                                DateTime dtOutTime = DateTime.Now.Date;
                                if (endTimeSplit[1].Substring(3, 2).ToUpper() == "PM")
                                {
                                    dtOutTime = dtOutTime.AddHours(Convert.ToDouble(endTimeSplit[0]) + 12);
                                }
                                else
                                {
                                    dtOutTime = dtOutTime.AddHours(Convert.ToDouble(endTimeSplit[0]));
                                }

                                dtOutTime = dtOutTime.AddMinutes(Convert.ToDouble(endTimeSplit[1].Substring(0, 2)));

                                oChild.SetProperty("U_Location", dtTable2.Rows[iRowCount]["Location"].ToString());
                                oChild.SetProperty("U_CleaningType", dtTable2.Rows[iRowCount]["CleaningType"].ToString());
                                oChild.SetProperty("U_StartDate", dtTable2.Rows[iRowCount]["Sch1StartDate"].ToString());
                                oChild.SetProperty("U_StartTime", dtInTime);
                                oChild.SetProperty("U_EndTime", dtOutTime);
                                oChild.SetProperty("U_Frequency", "Once");
                            }

                            oChildren = oGeneralData.Child("AB_JOBSCH2");
                            string sSupSignature = string.Empty;
                            string sConvertedSupSign = string.Empty;
                            string sClintSignature = string.Empty;
                            string sConvertedClintSign = string.Empty;
                            Random rnd = new Random();
                            int fileId = rnd.Next(1, 100000000);
                            Random rnd1 = new Random();
                            int fileId1 = rnd.Next(1, 100000000);

                            sConvertedSupSign = JobScheduleNew.SupervisorSign.ToString().Replace(" ", "+").ToString();
                            sSupSignature = MGet_SaveESignature(sConvertedSupSign, JobScheduleNew.Company, "JSSupvrSign" + fileId);

                            sConvertedClintSign = JobScheduleNew.ClientSign.ToString().Replace(" ", "+").ToString();
                            sClintSignature = MGet_SaveESignature(sConvertedClintSign, JobScheduleNew.Company, "JSClientSign" + fileId1);

                            //Looping  the  Line Details
                            for (int iRowCount = 0; iRowCount <= dtTable3.Rows.Count - 1; iRowCount++)
                            {
                                oChild = oChildren.Add();

                                oChild.SetProperty("U_ScheduleDate", Convert.ToDateTime(dtTable3.Rows[iRowCount]["ScheduledDate"].ToString()).Date);
                                //oChild.SetProperty("U_CompletedDate", DateTime.Now.Date);
                                oChild.SetProperty("U_DayStatus", dtTable3.Rows[iRowCount]["DayStatus"].ToString());
                                //oChild.SetProperty("U_UserSign", JobScheduleNew.PreparedBy);

                                oChild.SetProperty("U_AtcEntry", JobScheduleNew.AtcEntry);
                            }

                            oChildren = oGeneralData.Child("AB_JOBSCH3");

                            //Looping  the  Line Details
                            for (int iRowCount = 0; iRowCount <= dtTable5.Rows.Count - 1; iRowCount++)
                            {
                                oChild = oChildren.Add();

                                if (JobScheduleNew.ActualStartTime.ToString() == string.Empty || JobScheduleNew.ActualStartTime.ToString() == "")
                                {
                                    JobScheduleNew.ActualStartTime = "00:00";
                                }
                                string[] startTimeSplit = JobScheduleNew.ActualStartTime.Split(':');
                                if (JobScheduleNew.ActualEndTime.ToString() == string.Empty || JobScheduleNew.ActualEndTime.ToString() == "")
                                {
                                    JobScheduleNew.ActualEndTime = "00:00";
                                }
                                string[] endTimeSplit = JobScheduleNew.ActualEndTime.Split(':');

                                DateTime dtInTime = DateTime.Now.Date;
                                dtInTime = dtInTime.AddHours(Convert.ToDouble(startTimeSplit[0]));
                                dtInTime = dtInTime.AddMinutes(Convert.ToDouble(startTimeSplit[1]));
                                DateTime dtOutTime = DateTime.Now.Date;
                                dtOutTime = dtOutTime.AddHours(Convert.ToDouble(endTimeSplit[0]));
                                dtOutTime = dtOutTime.AddMinutes(Convert.ToDouble(endTimeSplit[1]));

                                oChild.SetProperty("U_DayLineNum", dtTable5.Rows[iRowCount]["DayLineNum"].ToString());
                                oChild.SetProperty("U_ActivityLineNum", dtTable5.Rows[iRowCount]["ActivityLineNum"].ToString());
                                //oChild.SetProperty("U_ActualStartTime", dtInTime);
                                //oChild.SetProperty("U_ActualEndTime", dtOutTime);
                                oChild.SetProperty("U_TaskStatus", "Open");
                                //oChild.SetProperty("U_Reason", JobScheduleNew.Reason);
                                //oChild.SetProperty("U_NewSchedDate", JobScheduleNew.NewScheduleDate);
                                //oChild.SetProperty("U_AlertHQ", JobScheduleNew.AlertHQ);
                                oChild.SetProperty("U_ScheduleDate", Convert.ToDateTime(dtTable3.Rows[iRowCount]["ScheduledDate"].ToString()).Date);
                            }

                            //Add/Update the header document;
                            oGeneralService.Add(oGeneralData);
                            SaveCount = SaveCount + 1;

                            // After Reschedule the new document, Update the data in Exiting Original Document
                            SAPbobsCOM.GeneralDataParams oGeneralDataParam = null;
                            oGeneralService = oCompanyService.GetGeneralService("JobSchedule");
                            oGeneralDataParam = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);

                            oGeneralDataParam.SetProperty("DocEntry", JobSchedule[i].DocEntry);
                            oGeneralData = oGeneralService.GetByParams(oGeneralDataParam);

                            oChildren = oGeneralData.Child("AB_JOBSCH2");
                            //Looping  the  Line Details

                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_DayStatus", "Closed");
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_SupervisorSign", sSupSignature);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_SupvrSignText", sConvertedSupSign);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_ClientSign", sClintSignature);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_ClintSignText", sConvertedClintSign);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_SignedDate1", JobSchedule[i].SignedDate1);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_SignedDate2", JobSchedule[i].SignedDate2);

                            oChildren = oGeneralData.Child("AB_JOBSCH3");
                            //Looping  the  Line Details

                            if (JobSchedule[i].Reason != null && JobSchedule[i].Reason != string.Empty && JobSchedule[i].Reason != "")
                            {
                                oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_Reason", JobSchedule[i].Reason);
                            }


                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_NewSchedDate", JobSchedule[i].NewScheduleDate);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_TaskStatus", JobSchedule[i].TaskStatus);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_AlertHQ", JobSchedule[i].AlertHQ);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_CmplStartDate", JobSchedule[i].CompletedStartDate);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_CmplEndDate", JobSchedule[i].CompletedEndDate);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_CompletedBy", JobSchedule[i].TaskCompleteBy);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_InspectedBy", JobSchedule[i].InspectedBy);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_VerifiedBy", JobSchedule[i].VerifiedBy);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_Remarks", JobSchedule[i].remarks);

                            oChildren = oGeneralData.Child("AB_JOBSCH3");
                            //Looping  the  Line Details
                            if (JobSchedule[i].Reason != null && JobSchedule[i].Reason != string.Empty && JobSchedule[i].Reason != "")
                            {
                                oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_Reason", JobSchedule[i].Reason);
                            }
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_NewSchedDate", JobSchedule[i].NewScheduleDate);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_TaskStatus", JobSchedule[i].TaskStatus);
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_AlertHQ", JobSchedule[i].AlertHQ);

                            oChildren = oGeneralData.Child("AB_JOBSCH4");
                            if (attachmentCopy.Rows.Count > 0)
                            {
                                //Delete the previous records from the line table:
                                SqlConnection con = new SqlConnection(ConnectionString);
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = con.CreateCommand();
                                cmd.CommandText = "DELETE FROM [@AB_JOBSCH4] WHERE U_LineNum = '" + JobSchedule[i].Sch3LineId + "' AND DOCENTRY = '"+ JobSchedule[i].DocEntry +"'";
                                cmd.ExecuteNonQuery();
                                con.Close();
                                //for (int iDocRow = oChildren.Count - 1; iDocRow >= 0; iDocRow--)
                                //{
                                //    oChildren.Remove(iDocRow);
                                //}
                            }
                            foreach (DataRow item in attachmentCopy.Rows)
                            {
                                if ((item["SAPURL"].ToString() != string.Empty || item["SAPURL"].ToString() != "") && item["DelFlag"].ToString() != "Y")
                                {
                                    //Looping  the  Line Details

                                    oChild = oChildren.Add();
                                    oChild.SetProperty("U_Path", item["SAPURL"]);
                                    oChild.SetProperty("U_FileName", item["fileName"]);
                                    oChild.SetProperty("U_AttachDate", DateTime.Now.Date);
                                    oChild.SetProperty("U_LineNum", JobSchedule[i].Sch3LineId);
                                    oChild.SetProperty("U_Remarks", item["Remarks"].ToString());
                                }
                            }

                            //oGeneralData.ToXMLFile("E:\\CleaningExpress_Log\\job.xml");

                            oGeneralService.Update(oGeneralData);
                        }
                    }
                    else
                    {
                        if (!oDICompany.InTransaction) oDICompany.StartTransaction();

                        if (JobSchedule[i].ActualStartTime.ToString() == string.Empty || JobSchedule[i].ActualStartTime.ToString() == "")
                        {
                            JobSchedule[i].ActualStartTime = "00:00";
                        }
                        string[] startTimeSplit = JobSchedule[i].ActualStartTime.Split(':');
                        if (JobSchedule[i].ActualEndTime.ToString() == string.Empty || JobSchedule[i].ActualEndTime.ToString() == "")
                        {
                            JobSchedule[i].ActualEndTime = "00:00";
                        }
                        string[] endTimeSplit = JobSchedule[i].ActualEndTime.Split(':');

                        DateTime dtInTime = DateTime.Now.Date;
                        dtInTime = dtInTime.AddHours(Convert.ToDouble(startTimeSplit[0]));
                        dtInTime = dtInTime.AddMinutes(Convert.ToDouble(startTimeSplit[1]));
                        DateTime dtOutTime = DateTime.Now.Date;
                        dtOutTime = dtOutTime.AddHours(Convert.ToDouble(endTimeSplit[0]));
                        dtOutTime = dtOutTime.AddMinutes(Convert.ToDouble(endTimeSplit[1]));

                        string sSupSignature = string.Empty;
                        string sConvertedSupSign = string.Empty;
                        string sClintSignature = string.Empty;
                        string sConvertedClintSign = string.Empty;
                        Random rnd = new Random();
                        int fileId = rnd.Next(1, 100000000);
                        Random rnd1 = new Random();
                        int fileId1 = rnd.Next(1, 100000000);

                        sConvertedSupSign = JobSchedule[i].SupervisorSign.ToString().Replace(" ", "+").ToString();
                        sSupSignature = MGet_SaveESignature(sConvertedSupSign, JobSchedule[i].Company, "JSSupvrSign" + fileId);

                        sConvertedClintSign = JobSchedule[i].ClientSign.ToString().Replace(" ", "+").ToString();
                        sClintSignature = MGet_SaveESignature(sConvertedClintSign, JobSchedule[i].Company, "JSClientSign" + fileId1);


                        //string sESignature = string.Empty;
                        //string sConvertedESign = string.Empty;
                        //Random rnd = new Random();
                        //int fileId = rnd.Next(1, 100000000);

                        //sConvertedESign = JobScheduleNew[i].ESignature.ToString().Replace(" ", "+").ToString();
                        //sESignature = MGet_SaveESignature(sConvertedESign, JobSchedule[i].Company, "JobSchSignature" + fileId);

                        SAPbobsCOM.GeneralService oGeneralService = null;
                        SAPbobsCOM.GeneralData oGeneralData;
                        SAPbobsCOM.GeneralDataCollection oChildren = null;
                        SAPbobsCOM.GeneralData oChild = null;
                        SAPbobsCOM.GeneralDataParams oGeneralDataParam = null;
                        SAPbobsCOM.CompanyService oCompanyService = oDICompany.GetCompanyService();
                        oGeneralService = oCompanyService.GetGeneralService("JobSchedule");
                        oGeneralDataParam = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);

                        oGeneralDataParam.SetProperty("DocEntry", JobSchedule[i].DocEntry);
                        oGeneralData = oGeneralService.GetByParams(oGeneralDataParam);

                        oChildren = oGeneralData.Child("AB_JOBSCH2");

                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_CompletedDate", DateTime.Now.Date);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_DayStatus", "Closed");
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_UserSign", JobSchedule[i].PreparedBy);

                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_SupervisorSign", sSupSignature);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_SupvrSignText", sConvertedSupSign);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_ClientSign", sClintSignature);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_ClintSignText", sConvertedClintSign);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_SignedDate1", JobSchedule[i].SignedDate1);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_SignedDate2", JobSchedule[i].SignedDate2);

                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch2VisOrder)).SetProperty("U_AtcEntry", JobSchedule[i].AtcEntry);

                        //oGeneralService.Update(oGeneralData);

                        oChildren = oGeneralData.Child("AB_JOBSCH3");

                        //Looping  the  Line Details

                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_ActualStartTime", dtInTime);//JobSchedule[i].ActualStartTime);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_ActualEndTime", dtOutTime); //JobSchedule[i].ActualEndTime);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_TaskStatus", JobSchedule[i].TaskStatus);
                        if (JobSchedule[i].Reason != null && JobSchedule[i].Reason != string.Empty && JobSchedule[i].Reason != "")
                        {
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_Reason", JobSchedule[i].Reason);
                        }
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_NewSchedDate", JobSchedule[i].NewScheduleDate);

                        if (JobSchedule[i].AlertHQ.ToUpper() == "YES")
                        {
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_AlertHQ", "Yes");
                        }
                        else if (JobSchedule[i].AlertHQ.ToUpper() == "NO")
                        {
                            oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_AlertHQ", "No");
                        }

                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_CmplStartDate", JobSchedule[i].CompletedStartDate);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_CmplEndDate", JobSchedule[i].CompletedEndDate);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_CompletedBy", JobSchedule[i].TaskCompleteBy);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_InspectedBy", JobSchedule[i].InspectedBy);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_VerifiedBy", JobSchedule[i].VerifiedBy);
                        oChildren.Item(Convert.ToInt32(JobSchedule[i].Sch3LineId) - 1).SetProperty("U_Remarks", JobSchedule[i].remarks);



                        oChildren = oGeneralData.Child("AB_JOBSCH4");

                        if (attachmentCopy.Rows.Count > 0)
                        {
                            //Delete the previous records from the line table:
                            SqlConnection con = new SqlConnection(ConnectionString);
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = con.CreateCommand();
                            cmd.CommandText = "DELETE FROM [@AB_JOBSCH4] WHERE U_LineNum = '" + JobSchedule[i].Sch3LineId + "' AND DOCENTRY = '" + JobSchedule[i].DocEntry + "'";
                            cmd.ExecuteNonQuery();
                            con.Close();
                            //for (int iDocRow = oChildren.Count - 1; iDocRow >= 0; iDocRow--)
                            //{
                            //    oChildren.Remove(iDocRow);
                            //}
                        }

                        foreach (DataRow item in attachmentCopy.Rows)
                        {
                            if ((item["SAPURL"].ToString() != string.Empty || item["SAPURL"].ToString() != "") && item["DelFlag"].ToString() != "Y")
                            {
                                //Looping  the  Line Details
                                oChild = oChildren.Add();
                                oChild.SetProperty("U_Path", item["SAPURL"]);
                                oChild.SetProperty("U_FileName", item["fileName"]);
                                oChild.SetProperty("U_AttachDate", DateTime.Now.Date);
                                oChild.SetProperty("U_LineNum", JobSchedule[i].Sch3LineId);
                                oChild.SetProperty("U_Remarks", item["Remarks"].ToString());
                            }
                        }
                        //oGeneralData.ToXMLFile("E:\\CleaningExpress_Log\\job.xml");
                        oGeneralService.Update(oGeneralData);
                        UpdateCount = UpdateCount + 1;
                    }
                }

                string sqlQuery = "select COUNT(U_DayStatus) [OpenStatusCount] from [@AB_JOBSCH2]  where DocEntry = '" + JobSchedule[0].DocEntry + "' and U_DayStatus = 'Open'";
                string sResult = string.Empty;
                int oRSReturnValue = 0;
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("SQL Query " + sqlQuery, sFuncName);
                oRS.DoQuery(sqlQuery);

                oRSReturnValue = Convert.ToInt32(oRS.Fields.Item(0).Value);
                if (oRSReturnValue == 0)
                {
                    sResult = UpdateDocumentStatus(JobSchedule[0].DocEntry);
                }
                if (sResult == "SUCCESS" || sResult == "" || sResult == string.Empty)
                {
                    if (oDICompany.InTransaction)
                    {
                        oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                        //return the result to mobile
                        result objResult = new result();
                        objResult.Result = "Success";
                        if (SaveCount > 0)
                        {
                            objResult.DisplayMessage = "Document is created successfully in SAP";
                        }
                        else if (UpdateCount > 0)
                        {
                            objResult.DisplayMessage = "Document is updated successfully in SAP";
                        }
                        lstResult.Add(objResult);
                        Context.Response.Output.Write(js.Serialize(lstResult));
                    }
                }
                else
                {
                    result objResult = new result();
                    objResult.Result = "Failure";
                    objResult.DisplayMessage = sResult;
                    lstResult.Add(objResult);
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

        #region Classes

        class result
        {
            public string Result { get; set; }
            public string DisplayMessage { get; set; }
        }

        class AttachmentResult
        {
            public string Result { get; set; }
            public string DisplayMessage { get; set; }
            public List<AttachmentURL> Attachments { get; set; }
        }

        class AttachmentResultWithRemarks
        {
            public string Result { get; set; }
            public string DisplayMessage { get; set; }
            public List<AttachmentURLWithRemarks> Attachments { get; set; }
        }

        class AttachmentURL
        {
            public string WebURL { get; set; }
            public string SAPURL { get; set; }
        }

        class AttachmentURLWithRemarks
        {
            public string WebURL { get; set; }
            public string SAPURL { get; set; }
            public string Remarks { get; set; }
            public string DelFlag { get; set; }
            public string FileMethod { get; set; }
        }

        class AttachmentWithRemarks
        {
            public string Company { get; set; }
            public string FileName { get; set; }
            public string Remarks { get; set; }
            public string DelFlag { get; set; }
            public string FileMethod { get; set; }
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
            public string sUserRole { get; set; }
        }

        class ListofJobs
        {
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string DocDate { get; set; }
            public string DocStatus { get; set; }
            public string PrcCode { get; set; }
            public string PrcName { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        class Json_ListofJobs
        {
            public string Company { get; set; }
            public string ProjectCode { get; set; }
        }

        class JobDetails
        {
            public string LineId { get; set; }
            public string VisOrder { get; set; }
            public string ScheduledDate { get; set; }
            public string DayStatus { get; set; }
            public string CleaningType { get; set; }
            public string Frequency { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
        }

        class Json_JobDetails
        {
            public string Company { get; set; }
            public string DocEntry { get; set; }
        }

        class Json_EPSDetails
        {
            public string Company { get; set; }
            public string DocNum { get; set; }
        }

        class Json_JobCompletion
        {
            public string Company { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string Project { get; set; }
        }

        class Json_MonthlyJobSchedule
        {
            public string Company { get; set; }
            public string MonthName { get; set; }
            public string Project { get; set; }
            public string Year { get; set; }
        }


        class ScheduleDayInfo
        {
            public string DocEntry { get; set; }
            public string Company { get; set; }
            public string ActivityLineNum { get; set; }
            public string DayLineNum { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Sch3LineId { get; set; }
            public string Sch3VisOrder { get; set; }
            public string Sch2VisOrder { get; set; }
            public string ScheduledDate { get; set; }
            public string DayStatus { get; set; }
            public string CompletedDate { get; set; }
            public string CompletedBy { get; set; }
            public string CleaningType { get; set; }
            public string Location { get; set; }
            public string Sch1StartDate { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string Frequency { get; set; }
            public string ActualStartTime { get; set; }
            public string ActualEndTime { get; set; }
            public string TaskStatus { get; set; }
            public string Reason { get; set; }
            public string AlertHQ { get; set; }
            public string NewScheduleDate { get; set; }

            public string SupervisorName { get; set; }
            public string SupervisorSign { get; set; }
            public string SignedDate1 { get; set; }
            public string ClientName { get; set; }
            public string ClientSign { get; set; }
            public string SignedDate2 { get; set; }

            //public string ESignature { get; set; }
            public string AtcEntry { get; set; }
            public string ProcCode { get; set; }
            public string PreparedBy { get; set; }
            public string PreparedDate { get; set; }
            public string PreparedById { get; set; }
            public string CompletedStartDate { get; set; }
            public string CompletedEndDate { get; set; }
            public string TaskCompleteBy { get; set; }
            public string InspectedBy { get; set; }
            public string VerifiedBy { get; set; }
            public string remarks { get; set; }
            public List<JS_Attachments> Attachments { get; set; }
        }

        class JS_Attachments
        {
            public string SAPURL { get; set; }
            public string WebURL { get; set; }
            public string filename { get; set; }
            public string Remarks { get; set; }
            public string DelFlag { get; set; }
        }

        class Json_ScheduleDayInfo
        {
            public string Company { get; set; }
            public string DocEntry { get; set; }
            public DateTime ScheduledDate { get; set; }
        }

        class SaveJobSchedule
        {
            public string DocEntry { get; set; }
            public string Company { get; set; }
            public string ActivityLineNum { get; set; }
            public string DayLineNum { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Sch3LineId { get; set; }
            public string Sch3VisOrder { get; set; }
            public string Sch2VisOrder { get; set; }
            public string ScheduledDate { get; set; }
            public string DayStatus { get; set; }
            public string CompletedDate { get; set; }
            public string CompletedBy { get; set; }
            public string CleaningType { get; set; }
            public string Location { get; set; }
            public string Sch1StartDate { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string Frequency { get; set; }
            public string ActualStartTime { get; set; }
            public string ActualEndTime { get; set; }
            public string TaskStatus { get; set; }
            public string Reason { get; set; }
            public string AlertHQ { get; set; }
            public string NewScheduleDate { get; set; }

            public string SupervisorName { get; set; }
            public string SupervisorSign { get; set; }
            public string SignedDate1 { get; set; }
            public string ClientName { get; set; }
            public string ClientSign { get; set; }
            public string SignedDate2 { get; set; }

            //public string ESignature { get; set; }
            public string AtcEntry { get; set; }
            public string ProcCode { get; set; }
            public string PreparedBy { get; set; }
            public string PreparedDate { get; set; }
            public string PreparedById { get; set; }
            public string CompletedStartDate { get; set; }
            public string CompletedEndDate { get; set; }
            public string TaskCompleteBy { get; set; }
            public string InspectedBy { get; set; }
            public string VerifiedBy { get; set; }
            public string remarks { get; set; }

            public List<JS_Attachments> Attachments { get; set; }

            //public string SAPURL { get; set; }
            //public string filename { get; set; }
        }
        #endregion

        #region Temp tables

        private DataTable SchTable0()
        {
            DataTable schTable0 = new DataTable();
            schTable0.Columns.Add("Company");
            schTable0.Columns.Add("ProcCode");
            schTable0.Columns.Add("StartDate");
            schTable0.Columns.Add("EndDate");
            schTable0.Columns.Add("PreparedBy");
            schTable0.Columns.Add("PreparedDate");
            schTable0.Columns.Add("PreparedById");
            schTable0.Columns.Add("TaskStatus");

            return schTable0;
        }

        private DataTable SchTable1()
        {
            DataTable schTable1 = new DataTable();
            schTable1.Columns.Add("Location");
            schTable1.Columns.Add("CleaningType");
            schTable1.Columns.Add("Sch1StartDate");
            schTable1.Columns.Add("StartTime");
            schTable1.Columns.Add("EndTime");
            schTable1.Columns.Add("Frequency");

            return schTable1;
        }

        private DataTable SchTable2()
        {
            DataTable schTable2 = new DataTable();
            schTable2.Columns.Add("ScheduledDate");
            schTable2.Columns.Add("DayStatus");

            return schTable2;
        }

        private DataTable SchTable3()
        {
            DataTable schTable3 = new DataTable();
            schTable3.Columns.Add("DayLineNum");
            schTable3.Columns.Add("ActivityLineNum");
            schTable3.Columns.Add("TaskStatus");

            return schTable3;
        }

        private DataTable SchTable4()
        {
            DataTable schTable4 = new DataTable();
            schTable4.Columns.Add("ScheduledDate");

            return schTable4;
        }

        private DataTable SchTable5()
        {
            DataTable schTable5 = new DataTable();
            schTable5.Columns.Add("DayLineNum");
            schTable5.Columns.Add("ActivityLineNum");
            schTable5.Columns.Add("TaskStatus");

            return schTable5;
        }

        private DataTable Save_SchTable0(SaveJobSchedule clsJobSch)
        {
            DataTable tbNew = new DataTable();
            tbNew = SchTable0();

            DataRow rowNew = tbNew.NewRow();
            rowNew["Company"] = clsJobSch.Company;
            rowNew["ProcCode"] = clsJobSch.ProcCode;
            rowNew["StartDate"] = clsJobSch.NewScheduleDate; //clsJobSch[0].StartDate;
            rowNew["EndDate"] = clsJobSch.NewScheduleDate; //clsJobSch[0].EndDate;
            rowNew["PreparedBy"] = clsJobSch.PreparedBy;
            rowNew["PreparedDate"] = clsJobSch.PreparedDate;
            rowNew["PreparedById"] = clsJobSch.PreparedById;
            rowNew["TaskStatus"] = "Daily"; //clsJobSch[0].TaskStatus;

            tbNew.Rows.Add(rowNew);

            return tbNew.Copy();
        }

        private DataTable Save_SchTable1(SaveJobSchedule clsJobSch)
        {
            DataTable tbNew = new DataTable();
            tbNew = SchTable1();
            //foreach (var item in clsJobSch)
            //{
            DataRow rowNew = tbNew.NewRow();
            rowNew["Location"] = clsJobSch.Location;
            rowNew["CleaningType"] = clsJobSch.CleaningType;
            rowNew["Sch1StartDate"] = clsJobSch.Sch1StartDate;
            rowNew["StartTime"] = clsJobSch.StartTime;
            rowNew["EndTime"] = clsJobSch.EndTime;
            rowNew["Frequency"] = clsJobSch.Frequency;

            tbNew.Rows.Add(rowNew);
            // }
            return tbNew.Copy();
        }

        private DataTable Save_SchTable2(DataTable dtRecordSet, string dayStatus)
        {
            DataTable tbNew = new DataTable();
            tbNew = SchTable2();
            foreach (DataRow item in dtRecordSet.Rows)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["ScheduledDate"] = item[0];
                rowNew["DayStatus"] = dayStatus;

                tbNew.Rows.Add(rowNew);
            }
            return tbNew.Copy();
        }

        private DataTable Save_SchTable3(string sDayLineNum, string sActivitylineNum, string sTaskStatus)
        {
            DataTable tbNew = new DataTable();
            tbNew = SchTable3();

            DataRow rowNew = tbNew.NewRow();
            rowNew["DayLineNum"] = sDayLineNum;
            rowNew["ActivityLineNum"] = sActivitylineNum;
            rowNew["TaskStatus"] = sTaskStatus;

            tbNew.Rows.Add(rowNew);

            return tbNew.Copy();
        }

        private DataTable Save_JS_ConvertAttachment(List<JS_Attachments> lstAttachments)
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

        #endregion

        #region public methods

        public DataTable ConvertRecordset(SAPbobsCOM.Recordset SAPRecordset)
        {
            DataTable functionReturnValue = default(DataTable);

            //\ This function will take an SAP recordset from the SAPbobsCOM library and convert it to a more
            //\ easily used ADO.NET datatable which can be used for data binding much easier.
            string sFuncName = "ConvertRecordset";
            DataTable dtTable = new DataTable();
            DataColumn NewCol = default(DataColumn);
            DataRow NewRow = default(DataRow);
            int ColCount = 0;

            try
            {
                for (ColCount = 0; ColCount <= SAPRecordset.Fields.Count - 1; ColCount++)
                {
                    NewCol = new DataColumn(SAPRecordset.Fields.Item(ColCount).Name);
                    dtTable.Columns.Add(NewCol);
                }


                while (!(SAPRecordset.EoF))
                {
                    NewRow = dtTable.NewRow();
                    //populate each column in the row we're creating

                    for (ColCount = 0; ColCount <= SAPRecordset.Fields.Count - 1; ColCount++)
                    {
                        //NewRow.Item(SAPRecordset.Fields.Item(ColCount).Name) = SAPRecordset.Fields.Item(ColCount).Value;
                        NewRow[SAPRecordset.Fields.Item(ColCount).Name] = SAPRecordset.Fields.Item(ColCount).Value;
                    }

                    //Add the row to the datatable
                    dtTable.Rows.Add(NewRow);

                    SAPRecordset.MoveNext();
                }

                return dtTable;

            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR " + sErrDesc, sFuncName);
                return functionReturnValue;
            }
            //return functionReturnValue;

        }

        public DataTable GenerateTable(SAPbobsCOM.Company oDICompany, string sFrequency, string txtFromDate, string txtToDate)
        {
            DataTable dtResult = new DataTable();
            string sQueryString = string.Empty;
            SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)oDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            if (sFrequency == "Daily")
            {
                sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" + txtFromDate + "',103), @MaxDate DATE = CONVERT(DATE,'" + txtToDate + "',103);" + " SELECT  TOP (DATEDIFF(DD, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(DD, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " + " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; ";
                oRS.DoQuery(sQueryString);

                if (oRS.RecordCount > 0)
                {
                    dtResult = ConvertRecordset(oRS);
                }
            }
            else if (sFrequency == "Weekly")
            {
                sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" + txtFromDate + "',103), @MaxDate DATE = CONVERT(DATE,'" + txtToDate + "',103);" + " SELECT  TOP (DATEDIFF(WK, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(WK, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " + " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; ";
                oRS.DoQuery(sQueryString);

                if (oRS.RecordCount > 0)
                {
                    dtResult = ConvertRecordset(oRS);
                }
            }
            else if (sFrequency == "Monthly")
            {
                sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" + txtFromDate + "',103), @MaxDate DATE = CONVERT(DATE,'" + txtToDate + "',103);" + " SELECT  TOP (DATEDIFF(MONTH, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(MONTH, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " + " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; ";
                oRS.DoQuery(sQueryString);

                if (oRS.RecordCount > 0)
                {
                    dtResult = ConvertRecordset(oRS);
                }
            }
            else if (sFrequency == "Quarterly")
            {
                sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" + txtFromDate + "',103), @MaxDate DATE = CONVERT(DATE,'" + txtToDate + "',103);" + " SELECT  TOP (DATEDIFF(QUARTER, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(QUARTER, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " + " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; ";
                oRS.DoQuery(sQueryString);

                if (oRS.RecordCount > 0)
                {
                    dtResult = ConvertRecordset(oRS);
                }
            }
            else if (sFrequency == "Yearly")
            {
                sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" + txtFromDate + "',103), @MaxDate DATE = CONVERT(DATE,'" + txtToDate + "',103);" + " SELECT  TOP (DATEDIFF(YEAR, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(YEAR, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " + " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; ";
                oRS.DoQuery(sQueryString);

                if (oRS.RecordCount > 0)
                {
                    dtResult = ConvertRecordset(oRS);
                }
            }
            else if (sFrequency == "Once")
            { // For Once Frequency
                sQueryString = "DECLARE @MinDate DATE = CONVERT(DATE,'" + txtFromDate + "',103), @MaxDate DATE = CONVERT(DATE,'" + txtToDate + "',103);" + " SELECT  TOP (DATEDIFF(DD, @MinDate, @MaxDate) + 1) Date =convert(varchar, DATEADD(DD, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate),102) " + " FROM    sys.all_objects a  CROSS JOIN sys.all_objects b; ";
                oRS.DoQuery(sQueryString);

                if (oRS.RecordCount > 0)
                {
                    dtResult = ConvertRecordset(oRS);
                }
            }
            return dtResult;
        }

        //public string GetDate(string sDate, ref SAPbobsCOM.Company oCompany)
        //{

        //    DateTime dateValue = default(DateTime);
        //    string DateString = string.Empty;
        //    string sSQL = string.Empty;
        //    SAPbobsCOM.Recordset oRs = default(SAPbobsCOM.Recordset);
        //    string sDatesep = null;

        //    oRs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

        //    sSQL = "SELECT DateFormat,DateSep FROM OADM";

        //    oRs.DoQuery(sSQL);

        //    if (!oRs.EoF)
        //    {
        //        sDatesep = oRs.Fields.Item("DateSep").Value;

        //        switch (oRs.Fields.Item("DateFormat").Value)
        //        {
        //            case 0:
        //                if (System.DateTime.TryParseExact(sDate, "dd" + sDatesep + "MM" + sDatesep + "yy", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
        //                {
        //                    DateString = dateValue.ToString("yyyyMMdd");
        //                }
        //                break;
        //            case 1:
        //                if (System.DateTime.TryParseExact(sDate, "dd" + sDatesep + "MM" + sDatesep + "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
        //                {
        //                    DateString = dateValue.ToString("yyyyMMdd");

        //                }
        //                else if (System.DateTime.TryParseExact(sDate, "yyyy" + sDatesep + "MM" + sDatesep + "dd", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
        //                {
        //                    DateString = dateValue.ToString("yyyyMMdd");
        //                }
        //                break;
        //            case 2:
        //                if (System.DateTime.TryParseExact(sDate, "MM" + sDatesep + "dd" + sDatesep + "yy", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
        //                {
        //                    DateString = dateValue.ToString("yyyyMMdd");
        //                }
        //                break;
        //            case 3:
        //                if (System.DateTime.TryParseExact(sDate, "MM" + sDatesep + "dd" + sDatesep + "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
        //                {
        //                    DateString = dateValue.ToString("yyyyMMdd");
        //                }
        //                break;
        //            case 4:
        //                if (System.DateTime.TryParseExact(sDate, "yyyy" + sDatesep + "MM" + sDatesep + "dd", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
        //                {
        //                    DateString = dateValue.ToString("yyyyMMdd");
        //                }
        //                break;
        //            case 5:
        //                if (System.DateTime.TryParseExact(sDate, "dd" + sDatesep + "MMMM" + sDatesep + "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
        //                {
        //                    DateString = dateValue.ToString("yyyyMMdd");
        //                }
        //                break;
        //            case 6:
        //                if (System.DateTime.TryParseExact(sDate, "yy" + sDatesep + "MM" + sDatesep + "dd", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
        //                {
        //                    DateString = dateValue.ToString("yyyyMMdd");
        //                }
        //                break;
        //            default:
        //                DateString = dateValue.ToString("yyyyMMdd");
        //                break;
        //        }

        //    }

        //    return DateString;

        //}

        public string ChangeDateWithPrefix0(string sDate)
        {
            string sFinalDateDate = string.Empty;
            if (sDate != string.Empty && sDate != "" && sDate != null)
            {
                string[] sStartDate = sDate.ToString().Split('/');
                if (sStartDate[0].Length == 1)
                {
                    sStartDate[0] = '0' + sStartDate[0];
                }

                if (sStartDate[1].Length == 1)
                {
                    sStartDate[1] = '0' + sStartDate[1];
                }

                sFinalDateDate = sStartDate[0] + '/' + sStartDate[1] + '/' + sStartDate[2];
            }
            return sFinalDateDate;
        }


        public string UpdateDocumentStatus(string sDocEntry)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "UpdateDocumentStatus";
                SAPbobsCOM.GeneralService oGeneralService = null;
                SAPbobsCOM.GeneralData oGeneralData;
                //  SAPbobsCOM.GeneralData oChild = null;
                SAPbobsCOM.GeneralDataParams oGeneralDataParam = null;
                SAPbobsCOM.CompanyService oCompanyService = oDICompany.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("JobSchedule");
                oGeneralDataParam = (SAPbobsCOM.GeneralDataParams)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);

                oGeneralDataParam.SetProperty("DocEntry", sDocEntry);
                oGeneralData = oGeneralService.GetByParams(oGeneralDataParam);
                oGeneralData.SetProperty("U_DocStatus", "C");

                oGeneralService.Update(oGeneralData);
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
        #endregion

        #endregion

        #region WebMethods for Inspection/QA

        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        //public string MGet_SaveESignature(string sJsonInput, string sCompanyName, string sImageName)
        //{
        //    //sJsonInput = "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";
        //    //sJsonInput = "R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==";
        //    DataSet dsCompanyList = oLogin.Get_CompanyList();
        //    string path = string.Empty;
        //    DataSet ds = oShowAround.Get_BitMapPath(dsCompanyList, sCompanyName);
        //    if (ds != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(sJsonInput)))
        //        {
        //            using (Bitmap bm2 = new Bitmap(ms))
        //            {
        //                path = ds.Tables[0].Rows[0][0].ToString() + sImageName + ".Png";
        //                bm2.Save(path);
        //                //Context.Response.Output.Write(js.Serialize("Success"));
        //            }
        //        }
        //    }
        //    return path;
        //}

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string MGet_SaveESignature(string sJsonInput, string sCompanyName, string sImageName)
        {
            //sJsonInput = "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";
            //sJsonInput = "R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==";
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
                    bitmapImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            return path;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_InspectionQA_Project(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_InspectionQA_Project";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sCurrentUserName = string.Empty;
                string sUserRole = string.Empty;

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
                    sUserRole = objProject.sUserRole;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_InspectionQA_Project() ", sFuncName);
                DataSet ds = oInspectionQA.Get_InspectionQA_Project(dsCompanyList, sCompany, sCurrentUserName, sUserRole);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_InspectionQA_Project() ", sFuncName);
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
        public void MGet_InspectionQA_ListofMSC(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_InspectionQA_ListofMSC";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_ListofJobs> lstDeserialize = js.Deserialize<List<Json_ListofJobs>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_ListofJobs objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sProjectCode = objProject.ProjectCode;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_InspectionQA_ListofMSC() ", sFuncName);
                DataSet ds = oInspectionQA.Get_InspectionQA_ListofMSC(dsCompanyList, sCompany, sProjectCode);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_InspectionQA_ListofMSC() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<ListofMSC> lstQA_MSC = new List<ListofMSC>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ListofMSC lstMSC = new ListofMSC();
                        lstMSC.DocNum = r["DocNum"].ToString();
                        lstMSC.DocEntry = r["DocEntry"].ToString();
                        lstMSC.DocDate = ChangeDateWithPrefix0(r["DocDate"].ToString());
                        lstMSC.MktSegment = r["MktSegment"].ToString();
                        //lstMSC.MktSegmentId = r["MktSegmentId"].ToString();

                        lstQA_MSC.Add(lstMSC);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the MSC list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstQA_MSC));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the MSC list, the serialized data is ' " + js.Serialize(lstQA_MSC) + " '", sFuncName);
                }
                else
                {
                    List<ListofMSC> lstQA_MSC = new List<ListofMSC>();
                    Context.Response.Output.Write(js.Serialize(lstQA_MSC));
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
        public void MGet_InspectionQA_MSCMappedId(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_InspectionQA_MSCMappedId";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_ListofJobs> lstDeserialize = js.Deserialize<List<Json_ListofJobs>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_ListofJobs objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sProjectCode = objProject.ProjectCode;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method MGet_InspectionQA_MSCMappedId() ", sFuncName);
                DataSet ds = oInspectionQA.Get_InspectionQA_GETMappedMKTSegmentId(dsCompanyList, sCompany, sProjectCode);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method MGet_InspectionQA_MSCMappedId() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<MappedMKTSegmentId> lstQA_MSC = new List<MappedMKTSegmentId>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        MappedMKTSegmentId lstMSC = new MappedMKTSegmentId();
                        lstMSC.MktSegmentId = r["MktSegmentId"].ToString();

                        lstQA_MSC.Add(lstMSC);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the MSC list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstQA_MSC));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the MSC list, the serialized data is ' " + js.Serialize(lstQA_MSC) + " '", sFuncName);
                }
                else
                {
                    List<MappedMKTSegmentId> lstQA_MSC = new List<MappedMKTSegmentId>();
                    Context.Response.Output.Write(js.Serialize(lstQA_MSC));
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
        public void MGet_InspectionQA_ViewMSCDetails(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_InspectionQA_ViewMSCDetails";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sDocEntry = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_JobDetails> lstDeserialize = js.Deserialize<List<Json_JobDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_JobDetails objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sDocEntry = objProject.DocEntry;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_InspectionQA_ViewMSCDetails() ", sFuncName);
                DataSet ds = oInspectionQA.Get_InspectionQA_ViewMSCDetails(dsCompanyList, sCompany, sDocEntry);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_InspectionQA_ViewMSCDetails() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    //this loop is for adding the Inspection QA Line items in Array
                    List<Json_SaveInspectionQADetail> lstItems = new List<Json_SaveInspectionQADetail>();
                    SaveInspectionQA _inspectionDetails = new SaveInspectionQA();
                    List<SaveInspectionQA> lstInsQADetails = new List<SaveInspectionQA>();
                    List<SA_Attachments> lstAttach = new List<SA_Attachments>();

                    foreach (DataRow drItems in ds.Tables[0].Rows)
                    {
                        var match = lstItems.Where(c => c.Item == Convert.ToString(drItems["Item"])).ToList();
                        if (match.Count == 0)
                        {
                            lstItems.Add(new Json_SaveInspectionQADetail
                            {
                                Category = drItems["Category"].ToString(),
                                Item = drItems["Item"].ToString(),
                                Rating = drItems["Rating"].ToString()
                            });
                        }
                    }

                    string sItem = "Item = '" + ds.Tables[0].Rows[0]["Item"].ToString() + "'";

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = sItem;

                    foreach (DataRowView rowView in dv)
                    {
                        DataRow row = rowView.Row;
                        SA_Attachments _docAttachment = new SA_Attachments();
                        _docAttachment.WebURL = row["WebURL"].ToString();
                        _docAttachment.Remarks = row["AttachmentRemarks"].ToString();
                        lstAttach.Add(_docAttachment);
                        // Do something //
                    }

                    //for adding the  Inspection QA details in Header and merge the line of Array items
                    _inspectionDetails.Company = sCompany;
                    _inspectionDetails.Project = ds.Tables[0].Rows[0]["Project"].ToString();
                    _inspectionDetails.MKTSegment = ds.Tables[0].Rows[0]["MKTSegment"].ToString();
                    _inspectionDetails.Remarks = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    _inspectionDetails.InspectedBy = ds.Tables[0].Rows[0]["InspectedBy"].ToString();
                    _inspectionDetails.AcknowledgeBy = ds.Tables[0].Rows[0]["AcknowledgeBy"].ToString();
                    _inspectionDetails.InspectedSign = ds.Tables[0].Rows[0]["InspectedSign"].ToString();
                    _inspectionDetails.AcknowledgeSign = ds.Tables[0].Rows[0]["AcknowledgeSign"].ToString();
                    _inspectionDetails.AttEntry = ds.Tables[0].Rows[0]["AttEntry"].ToString();
                    _inspectionDetails.MarketingSegment = lstItems;
                    _inspectionDetails.Attachments = lstAttach;

                    lstInsQADetails.Add(_inspectionDetails);

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the MSC Details ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstInsQADetails));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the MSC Details, the serialized data is ' " + js.Serialize(lstInsQADetails) + " '", sFuncName);
                }
                else
                {
                    List<MSCDetails> lstMSCDetails = new List<MSCDetails>();
                    Context.Response.Output.Write(js.Serialize(lstMSCDetails));
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
        public void MGet_InspectionQA_MarketSegment(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_InspectionQA_MarketSegment";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_ListofJobs> lstDeserialize = js.Deserialize<List<Json_ListofJobs>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_ListofJobs objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sProjectCode = objProject.ProjectCode;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_InspectionQA_MarketSegment() ", sFuncName);
                DataSet ds = oInspectionQA.Get_InspectionQA_MarketSegment(dsCompanyList, sCompany, sProjectCode);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_InspectionQA_MarketSegment() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<MarketSegment> lstMKTSegment = new List<MarketSegment>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        MarketSegment oMarketingSegment = new MarketSegment();

                        oMarketingSegment.BaseEntry = r["BaseEntry"].ToString();
                        oMarketingSegment.MktSegment = r["MktSegment"].ToString();

                        lstMKTSegment.Add(oMarketingSegment);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Marketing Segment ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstMKTSegment));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Marketing Segment, the serialized data is ' " + js.Serialize(lstMKTSegment) + " '", sFuncName);
                }
                else
                {
                    List<MarketSegment> lstMKTSegment = new List<MarketSegment>();
                    Context.Response.Output.Write(js.Serialize(lstMKTSegment));
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
        public void MGet_InspectionQA_CategoryItems(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_InspectionQA_CategoryItems";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sDocEntry = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_CategoryItems> lstDeserialize = js.Deserialize<List<Json_CategoryItems>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_CategoryItems objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sDocEntry = objProject.DocEntry;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_InspectionQA_CategoryItems() ", sFuncName);
                DataSet ds = oInspectionQA.Get_InspectionQA_CategoryItems(dsCompanyList, sCompany, sDocEntry);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_InspectionQA_CategoryItems() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<CategoryItems> lstCategoryItems = new List<CategoryItems>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        CategoryItems oCategoryItems = new CategoryItems();

                        oCategoryItems.DocEntry = r["DocEntry"].ToString();
                        oCategoryItems.BaseLineNum = r["BaseLineNum"].ToString();
                        oCategoryItems.Category = r["Category"].ToString();
                        oCategoryItems.Item = r["Item"].ToString();

                        lstCategoryItems.Add(oCategoryItems);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Category Items Details ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstCategoryItems));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Category Items Details, the serialized data is ' " + js.Serialize(lstCategoryItems) + " '", sFuncName);
                }
                else
                {
                    List<CategoryItems> lstCategoryItems = new List<CategoryItems>();
                    Context.Response.Output.Write(js.Serialize(lstCategoryItems));
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
        public void MSave_InspectionQA(string sJsonInput)
        {
            string sFuncName = string.Empty;
            string xmlResult = string.Empty;
            string sProject = string.Empty; string sMKTSegment = string.Empty; string sRemarks = string.Empty;
            string sInspectedBy = string.Empty; string sAcknowledgeBy = string.Empty; string sInspectedSign = string.Empty; string sAcknowledgeSign = string.Empty;
            string sConvertInspSign = string.Empty; string sConvertAckSign = string.Empty;
            DataSet dsInspectionDetails = new DataSet();
            try
            {
                sFuncName = "MSave_InspectionQA";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<SaveInspectionQA> inspectionList = js.Deserialize<List<SaveInspectionQA>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (inspectionList.Count > 0)
                {
                    SaveInspectionQA InspectionDetails = inspectionList[0];
                    List<SA_Attachments> attach = inspectionList[0].Attachments;
                    List<Json_SaveInspectionQADetail> items = InspectionDetails.MarketingSegment;

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Header Datatable ", sFuncName);
                    DataTable HeaderTable = Save_ConvertToInsQAHeaderTable(InspectionDetails);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Line Datatable ", sFuncName);
                    DataTable LineTable = Save_ConvertToInsQALineTable(items);
                    DataTable dtAttachments = Save_Ins_ConvertAttachment(attach);

                    DataTable headerCopy = HeaderTable.Copy();
                    DataTable lineCopy = LineTable.Copy();
                    DataTable attachCopy = dtAttachments.Copy();
                    dsInspectionDetails.Tables.Add(headerCopy);
                    dsInspectionDetails.Tables.Add(lineCopy);
                    dsInspectionDetails.Tables.Add(attachCopy);
                    dsInspectionDetails.Tables[0].TableName = "tblHeader";
                    dsInspectionDetails.Tables[1].TableName = "tblLine";
                    dsInspectionDetails.Tables[2].TableName = "tblAttachment";

                    if (dsInspectionDetails != null && dsInspectionDetails.Tables.Count > 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                        oDICompany = oLogin.ConnectToTargetCompany(InspectionDetails.Company);
                        //Declare the objects:

                        Random rnd = new Random();
                        int fileId = rnd.Next(1, 100000000);

                        Random rnd1 = new Random();
                        int fileId1 = rnd1.Next(1, 100000000);

                        SAPbobsCOM.GeneralService oGeneralService = null;
                        SAPbobsCOM.GeneralData oGeneralData;
                        SAPbobsCOM.GeneralDataCollection oChildren = null;
                        SAPbobsCOM.GeneralData oChild = null;
                        SAPbobsCOM.CompanyService oCompanyService = oDICompany.GetCompanyService();
                        oGeneralService = oCompanyService.GetGeneralService("MarketSegChecklist");

                        oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);

                        //Assigining the values to the Varibles:
                        sProject = dsInspectionDetails.Tables[0].Rows[0]["Project"].ToString();
                        sMKTSegment = dsInspectionDetails.Tables[0].Rows[0]["MKTSegment"].ToString();
                        sConvertInspSign = dsInspectionDetails.Tables[0].Rows[0]["InspectedSign"].ToString().Replace(" ", "+").ToString();
                        sInspectedSign = MGet_SaveESignature(sConvertInspSign, InspectionDetails.Company, "IInsSignature" + fileId);
                        sConvertAckSign = dsInspectionDetails.Tables[0].Rows[0]["AcknowledgeSign"].ToString().Replace(" ", "+").ToString();
                        sAcknowledgeSign = MGet_SaveESignature(sConvertAckSign, InspectionDetails.Company, "IAckSignature" + fileId1);
                        sRemarks = dsInspectionDetails.Tables[0].Rows[0]["Remarks"].ToString();
                        sInspectedBy = dsInspectionDetails.Tables[0].Rows[0]["InspectedBy"].ToString();
                        sAcknowledgeBy = dsInspectionDetails.Tables[0].Rows[0]["AcknowledgeBy"].ToString();
                        //sInspectedSign = dsInspectionDetails.Tables[0].Rows[0]["InspectedSign"].ToString();
                        //sAcknowledgeSign = dsInspectionDetails.Tables[0].Rows[0]["AcknowledgeSign"].ToString();

                        //Adding the Header Informations
                        oGeneralData.SetProperty("U_DocDate", DateTime.Now.Date);
                        oGeneralData.SetProperty("U_Project", sProject);
                        oGeneralData.SetProperty("U_MktSegment", sMKTSegment);
                        oGeneralData.SetProperty("U_InspectedBy", sInspectedBy);
                        oGeneralData.SetProperty("U_AcknowledgeBy", sAcknowledgeBy);
                        oGeneralData.SetProperty("U_InspectedSign", sInspectedSign);
                        oGeneralData.SetProperty("U_AcknowledgeSign", sAcknowledgeSign);
                        oGeneralData.SetProperty("U_InsSignText", sConvertInspSign);
                        oGeneralData.SetProperty("U_AckSignText", sConvertAckSign);
                        oGeneralData.SetProperty("U_Remarks", sRemarks);

                        oChildren = oGeneralData.Child("AB_MKTSGTCHK1");

                        //Looping  the  Line Details
                        for (int iRowCount = 0; iRowCount <= dsInspectionDetails.Tables[1].Rows.Count - 1; iRowCount++)
                        {
                            oChild = oChildren.Add();

                            oChild.SetProperty("U_BaseEntry", dsInspectionDetails.Tables[1].Rows[iRowCount]["BaseEntry"].ToString());
                            oChild.SetProperty("U_BaseLineNum", dsInspectionDetails.Tables[1].Rows[iRowCount]["BaseLineNum"].ToString());
                            oChild.SetProperty("U_Category", dsInspectionDetails.Tables[1].Rows[iRowCount]["Category"].ToString());
                            oChild.SetProperty("U_Item", dsInspectionDetails.Tables[1].Rows[iRowCount]["Item"].ToString());

                            if (dsInspectionDetails.Tables[1].Rows[iRowCount]["Rating"].ToString() == "1")
                                oChild.SetProperty("U_Rating1", "Y");
                            else if (dsInspectionDetails.Tables[1].Rows[iRowCount]["Rating"].ToString() == "2")
                                oChild.SetProperty("U_Rating2", "Y");
                            else if (dsInspectionDetails.Tables[1].Rows[iRowCount]["Rating"].ToString() == "3")
                                oChild.SetProperty("U_Rating3", "Y");
                            else if (dsInspectionDetails.Tables[1].Rows[iRowCount]["Rating"].ToString() == "4")
                                oChild.SetProperty("U_Rating4", "Y");
                            else if (dsInspectionDetails.Tables[1].Rows[iRowCount]["Rating"].ToString() == "5")
                                oChild.SetProperty("U_Rating5", "Y");
                        }

                        foreach (DataRow item in dsInspectionDetails.Tables[2].Rows)
                        {
                            if (item["SAPURL"].ToString() != string.Empty || item["SAPURL"].ToString() != "")
                            {
                                // This is for Attachment.
                                oChildren = oGeneralData.Child("AB_MKTSGTCHK2");
                                oChild = oChildren.Add();

                                oChild.SetProperty("U_Path", item["SAPURL"].ToString());
                                oChild.SetProperty("U_FileName", item["filename"].ToString());
                                oChild.SetProperty("U_AttachDate", DateTime.Now.Date);
                                oChild.SetProperty("U_Remarks", item["Remarks"].ToString());
                            }
                        }
                        //Add/Update the header document;
                        oGeneralService.Add(oGeneralData);

                        //clear the dataset
                        dsInspectionDetails.Tables.Remove(headerCopy);
                        dsInspectionDetails.Tables.Remove(lineCopy);
                        dsInspectionDetails.Tables.Remove(attachCopy);
                        dsInspectionDetails.Clear();

                        //return the result to mobile
                        result objResult = new result();
                        objResult.Result = "Success";
                        objResult.DisplayMessage = "Document is created successfully in SAP";
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
                dsInspectionDetails.Clear();
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MInspection_CreatePDF(string sJsonInput)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "MInspection_CreatePDF";
                sProcName = "SP_AB_FRM_InspectionQA";
                string sCompany = string.Empty;
                string sDocNum = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_EPSDetails> lstDeserialize = js.Deserialize<List<Json_EPSDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_EPSDetails objCreatePDF = lstDeserialize[0];
                    sCompany = objCreatePDF.Company;
                    sDocNum = objCreatePDF.DocNum;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);

                ReportDocument cryRpt = new ReportDocument();
                ReportDocument ERRPT = new ReportDocument();
                CrystalDecisions.Shared.ConnectionInfo objConInfo = new CrystalDecisions.Shared.ConnectionInfo();
                CrystalDecisions.Shared.TableLogOnInfo oLogonInfo = new CrystalDecisions.Shared.TableLogOnInfo();
                //Dim ConInfo As New CrystalDecisions.Shared.TableLogOnInfo
                int intCounter = 0;
                //Dim Formula As String
                if (dsCompanyList != null && dsCompanyList.Tables.Count > 0)
                {
                    oDTView = dsCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DocKey", sDocNum));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        //return "No Data in OUSR table for the selected Company";
                    }


                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Creating PDF file for DocNum : " + sDocNum, sFuncName);
                    //Create PDF file

                    string sFileName = "/PDF/INSQA/" + sDocNum + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + DateTime.Now.Millisecond + ".pdf";
                    string directory = HttpContext.Current.Server.MapPath("TEMP") + "/PDF/INSQA";
                    string AttachFile = HttpContext.Current.Server.MapPath("TEMP") + sFileName;
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    cryRpt.Load(Server.MapPath("Report") + "/Inspection QA.rpt");

                    ParameterValues crParameterValues = new ParameterValues();
                    ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                    string SerNam = null;
                    string DbName = null;
                    string UID = null;
                    SerNam = oDTView[0]["U_Server"].ToString();
                    DbName = oDTView[0]["U_DBName"].ToString();
                    UID = oDTView[0]["U_DBUserName"].ToString();

                    oLogonInfo.ConnectionInfo.ServerName = SerNam;
                    oLogonInfo.ConnectionInfo.DatabaseName = DbName;
                    oLogonInfo.ConnectionInfo.UserID = UID;
                    oLogonInfo.ConnectionInfo.Password = oDTView[0]["U_DBPassword"].ToString();

                    for (intCounter = 0; intCounter <= cryRpt.Database.Tables.Count - 1; intCounter++)
                    {
                        cryRpt.Database.Tables[intCounter].ApplyLogOnInfo(oLogonInfo);
                    }

                    cryRpt.SetParameterValue("@DocKey", sDocNum);

                    ExportOptions CrExportOptions = default(ExportOptions);
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    ExcelFormatOptions CrExcelFormat = new ExcelFormatOptions();
                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                    ExcelFormatOptions CrExcelTypeOptions = new ExcelFormatOptions();

                    CrDiskFileDestinationOptions.DiskFileName = AttachFile;
                    CrExportOptions = cryRpt.ExportOptions;
                    var _with1 = CrExportOptions;
                    _with1.ExportDestinationType = ExportDestinationType.DiskFile;
                    _with1.ExportFormatType = ExportFormatType.PortableDocFormat;
                    _with1.DestinationOptions = CrDiskFileDestinationOptions;
                    _with1.FormatOptions = CrFormatTypeOptions;
                    cryRpt.Export();

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("PDF Created successfully for Doc Num : " + sDocNum, sFuncName);
                    result objResult = new result();
                    objResult.Result = "Success";
                    objResult.DisplayMessage = "/TEMP" + sFileName;
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else
                {
                    oLog.WriteToErrorLogFile("There is No Company List in the Holding Company", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR : There is No Company List in the Holding Company ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "Failure";
                    objResult.DisplayMessage = "There is No Company List in the Holding Company";
                    lstResult.Add(objResult);
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
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        #region Classes

        class Json_CategoryItems
        {
            public string Company { get; set; }
            public string DocEntry { get; set; }
        }

        class CategoryItems
        {
            public string DocEntry { get; set; }
            public string BaseLineNum { get; set; }
            public string Category { get; set; }
            public string Item { get; set; }
        }

        class ListofMSC
        {
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string DocDate { get; set; }
            public string MktSegment { get; set; }
            //public string MktSegmentId { get; set; }
        }

        class MappedMKTSegmentId
        {
            public string MktSegmentId { get; set; }
        }

        class MSCDetails
        {
            public string MKTSegment { get; set; }
            public string Remarks { get; set; }
            public string InspectedBy { get; set; }
            public string AcknowledgeBy { get; set; }
            public string InspectedSign { get; set; }
            public string AcknowledgeSign { get; set; }
            //public string ESignature { get; set; }
            public string AttEntry { get; set; }
            public string Category { get; set; }
            public string Item { get; set; }
            public string Rating { get; set; }
        }

        class MarketSegment
        {
            public string BaseEntry { get; set; }
            public string MktSegment { get; set; }
        }

        class Json_Company
        {
            public string Company { get; set; }
        }

        class SaveInspectionQA
        {
            public string Company { get; set; }
            public string Project { get; set; }
            public string MKTSegment { get; set; }
            public string Remarks { get; set; }
            public string InspectedBy { get; set; }
            public string AcknowledgeBy { get; set; }
            public string InspectedSign { get; set; }
            public string AcknowledgeSign { get; set; }
            //public string ESignature { get; set; }
            public string AttEntry { get; set; }
            //public string SAPURL { get; set; }
            //public string WebURL { get; set; }
            //public string filename { get; set; }
            public List<Json_SaveInspectionQADetail> MarketingSegment { get; set; }
            public List<SA_Attachments> Attachments { get; set; }
        }

        class Json_SaveInspectionQADetail
        {
            public string BaseEntry { get; set; }
            public string BaseLineNum { get; set; }
            public string Category { get; set; }
            public string Item { get; set; }
            public string Rating { get; set; }
        }

        class SA_Attachments
        {
            public string SAPURL { get; set; }
            public string WebURL { get; set; }
            public string filename { get; set; }
            public string Remarks { get; set; }
            public string DelFlag { get; set; }
        }

        class Json_ScopeofWork
        {
            public string Company { get; set; }
            public string Project { get; set; }
            public string AddId { get; set; }
        }

        class Json_GetShipToAddress
        {
            public string Company { get; set; }
            public string Project { get; set; }
        }

        class Json_SendEmail
        {
            public string Company { get; set; }
            public string DocNum { get; set; }
            public string EmailId { get; set; }
        }


        #endregion

        #region tempTables

        private DataTable Save_ConvertToInsQAHeaderTable(SaveInspectionQA clsInsQA)
        {
            DataTable tbNew = new DataTable();
            tbNew = CreateHeaderTableInsQA();

            DataRow rowNew = tbNew.NewRow();
            rowNew["Project"] = clsInsQA.Project;
            rowNew["MKTSegment"] = clsInsQA.MKTSegment;
            //rowNew["ESignature"] = clsInsQA.ESignature;
            rowNew["InspectedBy"] = clsInsQA.InspectedBy;
            rowNew["AcknowledgeBy"] = clsInsQA.AcknowledgeBy;
            rowNew["InspectedSign"] = clsInsQA.InspectedSign;
            rowNew["AcknowledgeSign"] = clsInsQA.AcknowledgeSign;
            rowNew["Remarks"] = clsInsQA.Remarks;

            tbNew.Rows.Add(rowNew);

            return tbNew.Copy();
        }

        private DataTable Save_ConvertToInsQALineTable(List<Json_SaveInspectionQADetail> lstInsQA)
        {
            DataTable tbNew = new DataTable();

            tbNew = CreateLineTableInsQA();

            foreach (var item in lstInsQA)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["BaseEntry"] = item.BaseEntry;
                rowNew["BaseLineNum"] = item.BaseLineNum;
                rowNew["Category"] = item.Category;
                rowNew["Item"] = item.Item;
                rowNew["Rating"] = item.Rating;

                tbNew.Rows.Add(rowNew);
            }

            return tbNew.Copy();
        }

        private DataTable Save_Ins_ConvertAttachment(List<SA_Attachments> lstAttachments)
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

        private DataTable CreateHeaderTableInsQA()
        {
            DataTable tbInsQA = new DataTable();
            //tbInsQA.Columns.Add("Company");
            tbInsQA.Columns.Add("Project");
            tbInsQA.Columns.Add("MktSegment");
            //tbInsQA.Columns.Add("ESignature");
            tbInsQA.Columns.Add("InspectedBy");
            tbInsQA.Columns.Add("AcknowledgeBy");
            tbInsQA.Columns.Add("InspectedSign");
            tbInsQA.Columns.Add("AcknowledgeSign");
            tbInsQA.Columns.Add("Remarks");
            tbInsQA.Columns.Add("SAPURL");
            tbInsQA.Columns.Add("filename");

            return tbInsQA;
        }

        private DataTable CreateLineTableInsQA()
        {
            DataTable tbInsQA = new DataTable();
            tbInsQA.Columns.Add("BaseEntry");
            tbInsQA.Columns.Add("BaseLineNum");
            tbInsQA.Columns.Add("Category");
            tbInsQA.Columns.Add("Item");
            tbInsQA.Columns.Add("Rating");

            return tbInsQA;
        }

        #endregion

        #endregion

        #region WebMethods for GSL Gardener

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_GSLGardener_Project(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GSLGardener_Project";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sCurrentUserName = string.Empty;
                string sUserRole = string.Empty;

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
                    sUserRole = objProject.sUserRole;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GSLGardener_Project() ", sFuncName);
                DataSet ds = oGSLGardener.Get_GSLGardener_Project(dsCompanyList, sCompany, sCurrentUserName, sUserRole);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GSLGardener_Project() ", sFuncName);
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
        public void MGet_GSLGardener_ListofGardener(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GSLGardener_ListofGardener";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_ListofJobs> lstDeserialize = js.Deserialize<List<Json_ListofJobs>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_ListofJobs objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sProjectCode = objProject.ProjectCode;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GSLGardener_ListofGardener() ", sFuncName);
                DataSet ds = oGSLGardener.Get_GSLGardener_ListofGardener(dsCompanyList, sCompany, sProjectCode);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GSLGardener_ListofGardener() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<ListofGSLGardener> lstGarden = new List<ListofGSLGardener>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ListofGSLGardener _gardener = new ListofGSLGardener();
                        _gardener.DocNum = r["DocNum"].ToString();
                        _gardener.DocEntry = r["DocEntry"].ToString();
                        _gardener.DocDate = ChangeDateWithPrefix0(r["DocDate"].ToString());
                        _gardener.SupervisorName = r["SupervisorName"].ToString();
                        _gardener.ClientName = r["ClientName"].ToString();
                        lstGarden.Add(_gardener);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Gardener list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstGarden));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Gardener list, the serialized data is ' " + js.Serialize(lstGarden) + " '", sFuncName);
                }
                else
                {
                    List<ListofGSLGardener> lstGardener = new List<ListofGSLGardener>();
                    Context.Response.Output.Write(js.Serialize(lstGardener));
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
        public void MGet_GSLGardener_ViewGardenerDetails(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GSLGardener_ViewGardenerDetails";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sDocEntry = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_JobDetails> lstDeserialize = js.Deserialize<List<Json_JobDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_JobDetails objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sDocEntry = objProject.DocEntry;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_InspectionQA_ViewMSCDetails() ", sFuncName);
                DataSet ds = oGSLGardener.Get_GSLGardener_ViewGardenerDetails(dsCompanyList, sCompany, sDocEntry);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_InspectionQA_ViewMSCDetails() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    //this loop is for adding the Inspection QA Line items in Array
                    List<JSON_SaveGSLGardnerDetail> lstItems = new List<JSON_SaveGSLGardnerDetail>();
                    SaveGSLGardener _GardenerDetails = new SaveGSLGardener();
                    List<SaveGSLGardener> lstGardenerDetails = new List<SaveGSLGardener>();
                    List<SA_Attachments> lstAttach = new List<SA_Attachments>();

                    foreach (DataRow drItems in ds.Tables[0].Rows)
                    {
                        var match = lstItems.Where(c => c.Description == Convert.ToString(drItems["Description"])).ToList();
                        if (match.Count == 0)
                        {
                            lstItems.Add(new JSON_SaveGSLGardnerDetail
                            {
                                Description = drItems["Description"].ToString(),
                                Done = drItems["Done"].ToString(),
                                Remark = drItems["Remark"].ToString()
                            });
                        }
                    }

                    string sItem = "Description = '" + ds.Tables[0].Rows[0]["Description"].ToString() + "'";

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = sItem;

                    foreach (DataRowView rowView in dv)
                    {
                        DataRow row = rowView.Row;
                        SA_Attachments _docAttachment = new SA_Attachments();
                        _docAttachment.WebURL = row["WebURL"].ToString();
                        lstAttach.Add(_docAttachment);
                        // Do something //
                    }

                    //for adding the  Inspection QA details in Header and merge the line of Array items
                    _GardenerDetails.Company = sCompany;

                    _GardenerDetails.Project = ds.Tables[0].Rows[0]["Project"].ToString();
                    _GardenerDetails.Reference = ds.Tables[0].Rows[0]["Reference"].ToString();
                    _GardenerDetails.TimeIn = ds.Tables[0].Rows[0]["TimeIn"].ToString();
                    _GardenerDetails.TimeOut = ds.Tables[0].Rows[0]["TimeOut"].ToString();
                    _GardenerDetails.IssuedDate = ChangeDateWithPrefix0(ds.Tables[0].Rows[0]["IssuedDate"].ToString());
                    _GardenerDetails.ReceivedDate = ChangeDateWithPrefix0(ds.Tables[0].Rows[0]["ReceivedDate"].ToString());
                    _GardenerDetails.SupervisorName = ds.Tables[0].Rows[0]["SupervisorName"].ToString();
                    _GardenerDetails.SupervisorSign = ds.Tables[0].Rows[0]["SupervisorSign"].ToString();
                    _GardenerDetails.SignedDate1 = ChangeDateWithPrefix0(ds.Tables[0].Rows[0]["SignedDate1"].ToString());
                    _GardenerDetails.ClientName = ds.Tables[0].Rows[0]["ClientName"].ToString();
                    _GardenerDetails.ClientSign = ds.Tables[0].Rows[0]["ClientSign"].ToString();
                    _GardenerDetails.SignedDate2 = ChangeDateWithPrefix0(ds.Tables[0].Rows[0]["SignedDate2"].ToString());
                    //_GardenerDetails.filename = 

                    _GardenerDetails.GardenerChk = lstItems;
                    _GardenerDetails.Attachments = lstAttach;

                    lstGardenerDetails.Add(_GardenerDetails);

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the MSC Details ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstGardenerDetails));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the MSC Details, the serialized data is ' " + js.Serialize(lstGardenerDetails) + " '", sFuncName);
                }
                else
                {
                    List<SaveGSLGardener> lstGardenerDetails = new List<SaveGSLGardener>();
                    Context.Response.Output.Write(js.Serialize(lstGardenerDetails));
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
        public void MGet_GSLGardener_GetTemplate(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GSLGardener_GetTemplate";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_Company> lstDeserialize = js.Deserialize<List<Json_Company>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_Company objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GSLGardener_GetTemplate() ", sFuncName);
                DataSet ds = oGSLGardener.Get_GSLGardener_GetTemplate(dsCompanyList, sCompany);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GSLGardener_GetTemplate() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<GSLGardenerTemplate> lstGardenerTmpl = new List<GSLGardenerTemplate>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        GSLGardenerTemplate _gslTemplate = new GSLGardenerTemplate();
                        _gslTemplate.Question = r["Question"].ToString();
                        lstGardenerTmpl.Add(_gslTemplate);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Marketing Segment ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstGardenerTmpl));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Marketing Segment, the serialized data is ' " + js.Serialize(lstGardenerTmpl) + " '", sFuncName);
                }
                else
                {
                    List<GSLGardenerTemplate> lstGardenerTmpl = new List<GSLGardenerTemplate>();
                    Context.Response.Output.Write(js.Serialize(lstGardenerTmpl));
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
        public void MSave_GSLGardener(string sJsonInput)
        {
            string sFuncName = string.Empty;
            string xmlResult = string.Empty;
            DataSet dsGardenerDetails = new DataSet();
            string sClientSign = string.Empty; string sSupervisorSign = string.Empty;
            string sConvertClientSign = string.Empty; string sConvertSupSign = string.Empty;

            try
            {
                sFuncName = "MSave_GSLGardener";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<SaveGSLGardener> gardenerList = js.Deserialize<List<SaveGSLGardener>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (gardenerList.Count > 0)
                {
                    SaveGSLGardener GardenerDetails = gardenerList[0];
                    List<JSON_SaveGSLGardnerDetail> items = GardenerDetails.GardenerChk;
                    List<SA_Attachments> attachments = gardenerList[0].Attachments;

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Header Datatable ", sFuncName);
                    DataTable HeaderTable = Save_ConvertToGSLGardenerHeader(GardenerDetails);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Line Datatable ", sFuncName);
                    DataTable LineTable = Save_ConvertToGSLGardenerLine(items);
                    DataTable attachmentTable = Save_Ins_ConvertAttachment(attachments);

                    DataTable headerCopy = HeaderTable.Copy();
                    DataTable lineCopy = LineTable.Copy();
                    DataTable attachmentCopy = attachmentTable.Copy();
                    dsGardenerDetails.Tables.Add(headerCopy);
                    dsGardenerDetails.Tables.Add(lineCopy);
                    dsGardenerDetails.Tables.Add(attachmentCopy);
                    dsGardenerDetails.Tables[0].TableName = "tblHeader";
                    dsGardenerDetails.Tables[1].TableName = "tblLine";
                    dsGardenerDetails.Tables[2].TableName = "tblAttachment";

                    if (dsGardenerDetails != null && dsGardenerDetails.Tables.Count > 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                        oDICompany = oLogin.ConnectToTargetCompany(GardenerDetails.Company);
                        //Declare the objects:

                        SAPbobsCOM.GeneralService oGeneralService = null;
                        SAPbobsCOM.GeneralData oGeneralData;
                        SAPbobsCOM.GeneralDataCollection oChildren = null;
                        SAPbobsCOM.GeneralData oChild = null;
                        SAPbobsCOM.CompanyService oCompanyService = oDICompany.GetCompanyService();
                        oGeneralService = oCompanyService.GetGeneralService("GardenerChecklist");

                        oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);

                        Random rnd = new Random();
                        int fileId = rnd.Next(1, 100000000);

                        Random rnd1 = new Random();
                        int fileId1 = rnd.Next(1, 100000000);

                        sConvertClientSign = dsGardenerDetails.Tables[0].Rows[0]["ClientSign"].ToString().Replace(" ", "+").ToString();
                        sClientSign = MGet_SaveESignature(sConvertClientSign, GardenerDetails.Company, "GClintSignature" + fileId);
                        sConvertSupSign = dsGardenerDetails.Tables[0].Rows[0]["SupervisorSign"].ToString().Replace(" ", "+").ToString();
                        sSupervisorSign = MGet_SaveESignature(sConvertSupSign, GardenerDetails.Company, "GSupSignature" + fileId1);

                        //Adding the Header Informations
                        oGeneralData.SetProperty("U_DocDate", DateTime.Now.Date);
                        oGeneralData.SetProperty("U_Project", dsGardenerDetails.Tables[0].Rows[0]["Project"].ToString());
                        oGeneralData.SetProperty("U_Reference", dsGardenerDetails.Tables[0].Rows[0]["Reference"].ToString());

                        string[] startTimeSplit = dsGardenerDetails.Tables[0].Rows[0]["TimeIn"].ToString().Split(':');
                        string[] endTimeSplit = dsGardenerDetails.Tables[0].Rows[0]["TimeOut"].ToString().Split(':');
                        DateTime dtInTime = DateTime.Now.Date;
                        dtInTime = dtInTime.AddHours(Convert.ToDouble(startTimeSplit[0]));
                        dtInTime = dtInTime.AddMinutes(Convert.ToDouble(startTimeSplit[1]));

                        DateTime dtOutTime = DateTime.Now.Date;
                        dtOutTime = dtOutTime.AddHours(Convert.ToDouble(endTimeSplit[0]));
                        dtOutTime = dtOutTime.AddMinutes(Convert.ToDouble(endTimeSplit[1]));

                        oGeneralData.SetProperty("U_TimeIn", dtInTime);
                        oGeneralData.SetProperty("U_TimeOut", dtOutTime);
                        oGeneralData.SetProperty("U_IssuedDate", Convert.ToDateTime(dsGardenerDetails.Tables[0].Rows[0]["IssuedDate"].ToString()).Date);
                        oGeneralData.SetProperty("U_ReceivedDate", Convert.ToDateTime(dsGardenerDetails.Tables[0].Rows[0]["ReceivedDate"].ToString()).Date);
                        oGeneralData.SetProperty("U_Supervisor", dsGardenerDetails.Tables[0].Rows[0]["SupervisorName"].ToString());
                        oGeneralData.SetProperty("U_SupvrSignText", sConvertSupSign);
                        oGeneralData.SetProperty("U_SupervisorSign", sSupervisorSign);
                        oGeneralData.SetProperty("U_SignedDate1", Convert.ToDateTime(dsGardenerDetails.Tables[0].Rows[0]["SignedDate1"].ToString()).Date);
                        oGeneralData.SetProperty("U_Client", dsGardenerDetails.Tables[0].Rows[0]["ClientName"].ToString());
                        oGeneralData.SetProperty("U_ClintSignText", sConvertClientSign);
                        oGeneralData.SetProperty("U_ClientSign", sClientSign);
                        oGeneralData.SetProperty("U_SignedDate2", Convert.ToDateTime(dsGardenerDetails.Tables[0].Rows[0]["SignedDate2"].ToString()).Date);
                        oGeneralData.SetProperty("U_AtcEntry", dsGardenerDetails.Tables[0].Rows[0]["AtcEntry"].ToString());

                        oChildren = oGeneralData.Child("AB_GARDENERCHK1");

                        //Looping  the  Line Details
                        for (int iRowCount = 0; iRowCount <= dsGardenerDetails.Tables[1].Rows.Count - 1; iRowCount++)
                        {
                            oChild = oChildren.Add();

                            oChild.SetProperty("U_Description", dsGardenerDetails.Tables[1].Rows[iRowCount]["Description"].ToString());
                            oChild.SetProperty("U_Done", dsGardenerDetails.Tables[1].Rows[iRowCount]["Done"].ToString());
                            oChild.SetProperty("U_Remark", dsGardenerDetails.Tables[1].Rows[iRowCount]["Remark"].ToString());
                        }

                        foreach (DataRow item in dsGardenerDetails.Tables[2].Rows)
                        {
                            if (item["SAPURL"].ToString() != string.Empty || item["SAPURL"].ToString() != "")
                            {
                                // This is for Attachment.
                                oChildren = oGeneralData.Child("AB_GARDENERCHK2");
                                oChild = oChildren.Add();

                                oChild.SetProperty("U_Path", item["SAPURL"].ToString());
                                oChild.SetProperty("U_FileName", item["filename"].ToString());
                                oChild.SetProperty("U_AttachDate", DateTime.Now.Date);
                            }
                        }
                        //Add/Update the header document;
                        oGeneralService.Add(oGeneralData);

                        //clear the dataset
                        dsGardenerDetails.Tables.Remove(headerCopy);
                        dsGardenerDetails.Tables.Remove(lineCopy);
                        dsGardenerDetails.Tables.Remove(attachmentCopy);
                        dsGardenerDetails.Clear();

                        //return the result to mobile
                        result objResult = new result();
                        objResult.Result = "Success";
                        objResult.DisplayMessage = "Document is created successfully in SAP";
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
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        #region Classes

        class ListofGSLGardener
        {
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string DocDate { get; set; }
            public string SupervisorName { get; set; }
            public string ClientName { get; set; }
        }

        class GSLGardenerDetails
        {
            public string DocEntry { get; set; }
            public string Reference { get; set; }
            public string TimeIn { get; set; }
            public string TimeOut { get; set; }
            public string IssuedDate { get; set; }
            public string ReceivedDate { get; set; }
            public string SupervisorName { get; set; }
            public string SupervisorSign { get; set; }
            public string SignedDate1 { get; set; }
            public string ClientName { get; set; }
            public string ClientSign { get; set; }
            public string SignedDate2 { get; set; }
            public string AtcEntry { get; set; }
            public string Description { get; set; }
            public string Done { get; set; }
            public string Remark { get; set; }
        }

        class GSLGardenerTemplate
        {
            public string Question { get; set; }
        }

        class SaveGSLGardener
        {
            public string Company { get; set; }
            public string Project { get; set; }
            public string Reference { get; set; }
            public string TimeIn { get; set; }
            public string TimeOut { get; set; }
            public string IssuedDate { get; set; }
            public string ReceivedDate { get; set; }
            public string SupervisorName { get; set; }
            public string SupervisorSign { get; set; }
            public string SignedDate1 { get; set; }
            public string ClientName { get; set; }
            public string ClientSign { get; set; }
            public string SignedDate2 { get; set; }
            public string atcEntry { get; set; }
            //public string SAPURL { get; set; }
            //public string WebURL { get; set; }
            //public string filename { get; set; }
            public List<JSON_SaveGSLGardnerDetail> GardenerChk { get; set; }
            public List<SA_Attachments> Attachments { get; set; }

        }

        class JSON_SaveGSLGardnerDetail
        {
            public string Description { get; set; }
            public string Done { get; set; }
            public string Remark { get; set; }
        }

        #endregion

        #region tempTables

        private DataTable Save_ConvertToGSLGardenerHeader(SaveGSLGardener clsGSLGarden)
        {
            DataTable tbNew = new DataTable();
            tbNew = HeaderTableGSLGardener();

            DataRow rowNew = tbNew.NewRow();
            rowNew["Company"] = clsGSLGarden.Company;
            rowNew["Project"] = clsGSLGarden.Project;
            rowNew["Reference"] = clsGSLGarden.Reference;
            rowNew["TimeIn"] = clsGSLGarden.TimeIn;
            rowNew["TimeOut"] = clsGSLGarden.TimeOut;
            rowNew["IssuedDate"] = clsGSLGarden.IssuedDate;
            rowNew["ReceivedDate"] = clsGSLGarden.ReceivedDate;
            rowNew["SupervisorName"] = clsGSLGarden.SupervisorName;
            rowNew["SupervisorSign"] = clsGSLGarden.SupervisorSign;
            rowNew["SignedDate1"] = clsGSLGarden.SignedDate1;
            rowNew["ClientName"] = clsGSLGarden.ClientName;
            rowNew["ClientSign"] = clsGSLGarden.ClientSign;
            rowNew["SignedDate2"] = clsGSLGarden.SignedDate2;
            rowNew["atcEntry"] = clsGSLGarden.atcEntry;
            //rowNew["SAPURL"] = clsGSLGarden.SAPURL;
            //rowNew["filename"] = clsGSLGarden.filename;

            tbNew.Rows.Add(rowNew);

            return tbNew.Copy();
        }

        private DataTable Save_ConvertToGSLGardenerLine(List<JSON_SaveGSLGardnerDetail> lstGSLGarden)
        {
            DataTable tbNew = new DataTable();

            tbNew = LineTableGSLGardener();

            foreach (var item in lstGSLGarden)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["Description"] = item.Description;
                rowNew["Done"] = item.Done;
                rowNew["Remark"] = item.Remark;

                tbNew.Rows.Add(rowNew);
            }

            return tbNew.Copy();
        }

        private DataTable HeaderTableGSLGardener()
        {
            DataTable tbGSL = new DataTable();
            tbGSL.Columns.Add("Company");
            tbGSL.Columns.Add("Project");
            tbGSL.Columns.Add("Reference");
            tbGSL.Columns.Add("TimeIn");
            tbGSL.Columns.Add("TimeOut");
            tbGSL.Columns.Add("IssuedDate");
            tbGSL.Columns.Add("ReceivedDate");
            tbGSL.Columns.Add("SupervisorName");
            tbGSL.Columns.Add("SupervisorSign");
            tbGSL.Columns.Add("SignedDate1");
            tbGSL.Columns.Add("ClientName");
            tbGSL.Columns.Add("ClientSign");
            tbGSL.Columns.Add("SignedDate2");
            tbGSL.Columns.Add("atcEntry");
            //tbGSL.Columns.Add("SAPURL");
            //tbGSL.Columns.Add("filename");

            return tbGSL;
        }

        private DataTable LineTableGSLGardener()
        {
            DataTable tbGSL = new DataTable();
            tbGSL.Columns.Add("Description");
            tbGSL.Columns.Add("Done");
            tbGSL.Columns.Add("Remark");

            return tbGSL;
        }

        #endregion

        #endregion

        #region WebMethods for GSL Landscape

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_GSLLandscape_Project(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GSLLandscape_Project";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sCurrentUserName = string.Empty;
                string sUserRole = string.Empty;

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
                    sUserRole = objProject.sUserRole;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GSLLandscape_Project() ", sFuncName);
                DataSet ds = oGSLLandscape.Get_GSLLandscape_Project(dsCompanyList, sCompany, sCurrentUserName, sUserRole);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GSLLandscape_Project() ", sFuncName);
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
        public void MGet_GSLLandscape_ListofLandscape(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GSLLandscape_ListofLandscape";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_ListofJobs> lstDeserialize = js.Deserialize<List<Json_ListofJobs>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_ListofJobs objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sProjectCode = objProject.ProjectCode;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GSLLandscape_ListofLandscape() ", sFuncName);
                DataSet ds = oGSLLandscape.Get_GSLLandscape_ListofLandscape(dsCompanyList, sCompany, sProjectCode);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GSLLandscape_ListofLandscape() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<ListofGSLLandscape> lstLandscape = new List<ListofGSLLandscape>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ListofGSLLandscape _landscape = new ListofGSLLandscape();
                        _landscape.DocNum = r["DocNum"].ToString();
                        _landscape.DocEntry = r["DocEntry"].ToString();
                        _landscape.DocDate = ChangeDateWithPrefix0(r["DocDate"].ToString());
                        _landscape.SupervisorName = r["SupervisorName"].ToString();
                        _landscape.ClientName = r["ClientName"].ToString();
                        lstLandscape.Add(_landscape);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Landscape list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstLandscape));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Landscape list, the serialized data is ' " + js.Serialize(lstLandscape) + " '", sFuncName);
                }
                else
                {
                    List<ListofGSLLandscape> lstLandscape = new List<ListofGSLLandscape>();
                    Context.Response.Output.Write(js.Serialize(lstLandscape));
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
        public void MGet_GSLLandscape_ViewLandscape(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GSLLandscape_ViewLandscape";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sDocEntry = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_JobDetails> lstDeserialize = js.Deserialize<List<Json_JobDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_JobDetails objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sDocEntry = objProject.DocEntry;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GSLLandscape_ViewLandscape() ", sFuncName);
                DataSet ds = oGSLLandscape.Get_GSLLandscape_ViewLandscape(dsCompanyList, sCompany, sDocEntry);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GSLLandscape_ViewLandscape() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    //this loop is for adding the Inspection QA Line items in Array
                    List<SA_Attachments> lstAttach = new List<SA_Attachments>();
                    List<JSON_SaveGSLLandscapeDetail> lstItems = new List<JSON_SaveGSLLandscapeDetail>();
                    SaveGSLLandscape _LandscapeDetails = new SaveGSLLandscape();
                    List<SaveGSLLandscape> lstLandscapeDetails = new List<SaveGSLLandscape>();

                    foreach (DataRow drItems in ds.Tables[0].Rows)
                    {
                        var match = lstItems.Where(c => c.Description == Convert.ToString(drItems["Description"])).ToList();
                        if (match.Count == 0)
                        {
                            lstItems.Add(new JSON_SaveGSLLandscapeDetail
                            {
                                Description = drItems["Description"].ToString(),
                                Done = drItems["Done"].ToString(),
                                Remark = drItems["Remark"].ToString()
                            });
                        }
                    }

                    string sItem = "Description = '" + ds.Tables[0].Rows[0]["Description"].ToString() + "'";

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = sItem;

                    foreach (DataRowView rowView in dv)
                    {
                        DataRow row = rowView.Row;
                        SA_Attachments _docAttachment = new SA_Attachments();
                        _docAttachment.WebURL = row["WebURL"].ToString();
                        _docAttachment.Remarks = row["AttachmentRemarks"].ToString();
                        lstAttach.Add(_docAttachment);
                        // Do something //
                    }

                    //for adding the  Inspection QA details in Header and merge the line of Array items
                    _LandscapeDetails.Company = sCompany;

                    _LandscapeDetails.Project = ds.Tables[0].Rows[0]["Project"].ToString();
                    _LandscapeDetails.Reference = ds.Tables[0].Rows[0]["Reference"].ToString();
                    _LandscapeDetails.TimeIn = ds.Tables[0].Rows[0]["TimeIn"].ToString();
                    _LandscapeDetails.TimeOut = ds.Tables[0].Rows[0]["TimeOut"].ToString();
                    _LandscapeDetails.IssuedDate = ChangeDateWithPrefix0(ds.Tables[0].Rows[0]["IssuedDate"].ToString());
                    _LandscapeDetails.ReceivedDate = ChangeDateWithPrefix0(ds.Tables[0].Rows[0]["ReceivedDate"].ToString());
                    _LandscapeDetails.SupervisorName = ds.Tables[0].Rows[0]["SupervisorName"].ToString();
                    _LandscapeDetails.SupervisorSign = ds.Tables[0].Rows[0]["SupervisorSign"].ToString();
                    _LandscapeDetails.SignedDate1 = ChangeDateWithPrefix0(ds.Tables[0].Rows[0]["SignedDate1"].ToString());
                    _LandscapeDetails.ClientName = ds.Tables[0].Rows[0]["ClientName"].ToString();
                    _LandscapeDetails.ClientSign = ds.Tables[0].Rows[0]["ClientSign"].ToString();
                    _LandscapeDetails.SignedDate2 = ChangeDateWithPrefix0(ds.Tables[0].Rows[0]["SignedDate2"].ToString());
                    _LandscapeDetails.AtcEntry = ds.Tables[0].Rows[0]["AtcEntry"].ToString();

                    _LandscapeDetails.LandscapeChk = lstItems;
                    _LandscapeDetails.Attachments = lstAttach;

                    lstLandscapeDetails.Add(_LandscapeDetails);

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Landscape Details ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstLandscapeDetails));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Landscape Details, the serialized data is ' " + js.Serialize(lstLandscapeDetails) + " '", sFuncName);
                }
                else
                {
                    List<GSLLandscapeDetails> lstLandscape = new List<GSLLandscapeDetails>();
                    Context.Response.Output.Write(js.Serialize(lstLandscape));
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
        public void MGet_GSLLandscape_GetTemplate(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_GSLLandscape_GetTemplate";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_Company> lstDeserialize = js.Deserialize<List<Json_Company>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_Company objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GSLLandscape_GetTemplate() ", sFuncName);
                DataSet ds = oGSLLandscape.Get_GSLLandscape_GetTemplate(dsCompanyList, sCompany);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GSLLandscape_GetTemplate() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<GSLLandscapeTemplate> lstLandscapeTmpl = new List<GSLLandscapeTemplate>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        GSLLandscapeTemplate _gslTemplate = new GSLLandscapeTemplate();
                        _gslTemplate.Question = r["Question"].ToString();
                        lstLandscapeTmpl.Add(_gslTemplate);
                    }


                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Landscape Template List", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstLandscapeTmpl));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Landscape Template List, the serialized data is ' " + js.Serialize(lstLandscapeTmpl) + " '", sFuncName);
                }
                else
                {
                    List<GSLLandscapeTemplate> lstGardenerTmpl = new List<GSLLandscapeTemplate>();
                    Context.Response.Output.Write(js.Serialize(lstGardenerTmpl));
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
        public void MSave_GSLLandscape(string sJsonInput)
        {
            string sFuncName = string.Empty;
            string xmlResult = string.Empty;
            DataSet dsLandscapeDetails = new DataSet();
            string sClientSign = string.Empty; string sSupervisorSign = string.Empty;
            string sConvertClientSign = string.Empty; string sConvertSupSign = string.Empty;

            try
            {
                sFuncName = "MSave_GSLLandscape";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<SaveGSLLandscape> landscapeList = js.Deserialize<List<SaveGSLLandscape>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                if (landscapeList.Count > 0)
                {
                    List<SA_Attachments> attachments = landscapeList[0].Attachments;
                    SaveGSLLandscape LandscapeDetails = landscapeList[0];
                    List<JSON_SaveGSLLandscapeDetail> items = LandscapeDetails.LandscapeChk;

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Header Datatable ", sFuncName);
                    DataTable HeaderTable = Save_ConvertToGSLLandscapeHeader(LandscapeDetails);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Converting Json to Line Datatable ", sFuncName);
                    DataTable LineTable = Save_ConvertToGSLLandscapeLine(items);
                    DataTable attachmentTable = Save_Ins_ConvertAttachment(attachments);

                    DataTable headerCopy = HeaderTable.Copy();
                    DataTable lineCopy = LineTable.Copy();
                    DataTable attachmentCopy = attachmentTable.Copy();
                    dsLandscapeDetails.Tables.Add(headerCopy);
                    dsLandscapeDetails.Tables.Add(lineCopy);
                    dsLandscapeDetails.Tables.Add(attachmentCopy);
                    dsLandscapeDetails.Tables[0].TableName = "tblHeader";
                    dsLandscapeDetails.Tables[1].TableName = "tblLine";
                    dsLandscapeDetails.Tables[2].TableName = "tblAttachment";

                    if (dsLandscapeDetails != null && dsLandscapeDetails.Tables.Count > 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                        oDICompany = oLogin.ConnectToTargetCompany(LandscapeDetails.Company);
                        //Declare the objects:

                        SAPbobsCOM.GeneralService oGeneralService = null;
                        SAPbobsCOM.GeneralData oGeneralData;
                        SAPbobsCOM.GeneralDataCollection oChildren = null;
                        SAPbobsCOM.GeneralData oChild = null;
                        SAPbobsCOM.CompanyService oCompanyService = oDICompany.GetCompanyService();
                        oGeneralService = oCompanyService.GetGeneralService("LandscapeChecklist");

                        oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);

                        Random rnd = new Random();
                        int fileId = rnd.Next(1, 100000000);

                        Random rnd1 = new Random();
                        int fileId1 = rnd1.Next(1, 100000000);

                        //Adding the Header Informations
                        oGeneralData.SetProperty("U_DocDate", DateTime.Now.Date);
                        oGeneralData.SetProperty("U_Project", dsLandscapeDetails.Tables[0].Rows[0]["Project"].ToString());
                        oGeneralData.SetProperty("U_Reference", dsLandscapeDetails.Tables[0].Rows[0]["Reference"].ToString());

                        string[] startTimeSplit = dsLandscapeDetails.Tables[0].Rows[0]["TimeIn"].ToString().Split(':');
                        string[] endTimeSplit = dsLandscapeDetails.Tables[0].Rows[0]["TimeOut"].ToString().Split(':');
                        DateTime dtInTime = DateTime.Now.Date;
                        dtInTime = dtInTime.AddHours(Convert.ToDouble(startTimeSplit[0]));
                        dtInTime = dtInTime.AddMinutes(Convert.ToDouble(startTimeSplit[1]));

                        DateTime dtOutTime = DateTime.Now.Date;
                        dtOutTime = dtOutTime.AddHours(Convert.ToDouble(endTimeSplit[0]));
                        dtOutTime = dtOutTime.AddMinutes(Convert.ToDouble(endTimeSplit[1]));

                        sConvertClientSign = dsLandscapeDetails.Tables[0].Rows[0]["ClientSign"].ToString().Replace(" ", "+").ToString();
                        sClientSign = MGet_SaveESignature(sConvertClientSign, LandscapeDetails.Company, "LClintSignature" + fileId);
                        sConvertSupSign = dsLandscapeDetails.Tables[0].Rows[0]["SupervisorSign"].ToString().Replace(" ", "+").ToString();
                        sSupervisorSign = MGet_SaveESignature(sConvertSupSign, LandscapeDetails.Company, "LSupSignature" + fileId1);

                        oGeneralData.SetProperty("U_TimeIn", dtInTime);
                        oGeneralData.SetProperty("U_TimeOut", dtOutTime);
                        oGeneralData.SetProperty("U_IssuedDate", Convert.ToDateTime(dsLandscapeDetails.Tables[0].Rows[0]["IssuedDate"].ToString()).Date);
                        oGeneralData.SetProperty("U_ReceivedDate", Convert.ToDateTime(dsLandscapeDetails.Tables[0].Rows[0]["ReceivedDate"].ToString()).Date);
                        oGeneralData.SetProperty("U_Supervisor", dsLandscapeDetails.Tables[0].Rows[0]["SupervisorName"].ToString());
                        oGeneralData.SetProperty("U_SupervisorSign", sSupervisorSign);
                        oGeneralData.SetProperty("U_SupvrSignText", sConvertSupSign);
                        oGeneralData.SetProperty("U_SignedDate1", Convert.ToDateTime(dsLandscapeDetails.Tables[0].Rows[0]["SignedDate1"].ToString()).Date);
                        oGeneralData.SetProperty("U_Client", dsLandscapeDetails.Tables[0].Rows[0]["ClientName"].ToString());
                        oGeneralData.SetProperty("U_ClientSign", sClientSign);
                        oGeneralData.SetProperty("U_ClintSignText", sConvertClientSign);
                        oGeneralData.SetProperty("U_SignedDate2", Convert.ToDateTime(dsLandscapeDetails.Tables[0].Rows[0]["SignedDate2"].ToString()).Date);
                        oGeneralData.SetProperty("U_AtcEntry", dsLandscapeDetails.Tables[0].Rows[0]["AtcEntry"].ToString());

                        oChildren = oGeneralData.Child("AB_LANDSCAPECHK1");

                        //Looping  the  Line Details
                        for (int iRowCount = 0; iRowCount <= dsLandscapeDetails.Tables[1].Rows.Count - 1; iRowCount++)
                        {
                            oChild = oChildren.Add();

                            oChild.SetProperty("U_Description", dsLandscapeDetails.Tables[1].Rows[iRowCount]["Description"].ToString());
                            oChild.SetProperty("U_Done", dsLandscapeDetails.Tables[1].Rows[iRowCount]["Done"].ToString());
                            oChild.SetProperty("U_Remark", dsLandscapeDetails.Tables[1].Rows[iRowCount]["Remark"].ToString());
                        }

                        foreach (DataRow item in dsLandscapeDetails.Tables[2].Rows)
                        {
                            if (item["SAPURL"].ToString() != string.Empty || item["SAPURL"].ToString() != "")
                            {
                                // This is for Attachment.
                                oChildren = oGeneralData.Child("AB_LANDSCAPECHK2");
                                oChild = oChildren.Add();

                                oChild.SetProperty("U_Path", item["SAPURL"].ToString());
                                oChild.SetProperty("U_FileName", item["filename"].ToString());
                                oChild.SetProperty("U_AttachDate", DateTime.Now.Date);
                                oChild.SetProperty("U_Remarks", item["Remarks"].ToString());
                            }
                        }
                        //Add/Update the header document;
                        oGeneralService.Add(oGeneralData);

                        //clear the dataset
                        dsLandscapeDetails.Tables.Remove(headerCopy);
                        dsLandscapeDetails.Tables.Remove(lineCopy);
                        dsLandscapeDetails.Tables.Remove(attachmentCopy);

                        dsLandscapeDetails.Clear();

                        //return the result to mobile
                        result objResult = new result();
                        objResult.Result = "Success";
                        objResult.DisplayMessage = "Document is created successfully in SAP";
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
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Rollback the SAP Transaction ", sFuncName);
                if (oDICompany.InTransaction) oDICompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        #region Classes

        class ListofGSLLandscape
        {
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string DocDate { get; set; }
            public string SupervisorName { get; set; }
            public string ClientName { get; set; }
        }

        class GSLLandscapeDetails
        {
            public string DocEntry { get; set; }
            public string Reference { get; set; }
            public string TimeIn { get; set; }
            public string TimeOut { get; set; }
            public string IssuedDate { get; set; }
            public string ReceivedDate { get; set; }
            public string SupervisorName { get; set; }
            public string SupervisorSign { get; set; }
            public string SignedDate1 { get; set; }
            public string ClientName { get; set; }
            public string ClientSign { get; set; }
            public string SignedDate2 { get; set; }
            public string AtcEntry { get; set; }
            public string Description { get; set; }
            public string Done { get; set; }
            public string Remark { get; set; }
        }

        class GSLLandscapeTemplate
        {
            public string Question { get; set; }
        }

        class SaveGSLLandscape
        {
            public string Company { get; set; }
            public string Project { get; set; }
            public string Reference { get; set; }
            public string TimeIn { get; set; }
            public string TimeOut { get; set; }
            public string IssuedDate { get; set; }
            public string ReceivedDate { get; set; }
            public string SupervisorName { get; set; }
            public string SupervisorSign { get; set; }
            public string SignedDate1 { get; set; }
            public string ClientName { get; set; }
            public string ClientSign { get; set; }
            public string SignedDate2 { get; set; }
            public string AtcEntry { get; set; }
            //public string SAPURL { get; set; }
            //public string WebURL { get; set; }
            //public string filename { get; set; }
            public List<JSON_SaveGSLLandscapeDetail> LandscapeChk { get; set; }
            public List<SA_Attachments> Attachments { get; set; }

        }

        class JSON_SaveGSLLandscapeDetail
        {
            public string Description { get; set; }
            public string Done { get; set; }
            public string Remark { get; set; }
        }


        #endregion

        #region tempTables

        private DataTable Save_ConvertToGSLLandscapeHeader(SaveGSLLandscape clsGSLLandscape)
        {
            DataTable tbNew = new DataTable();
            tbNew = HeaderTableGSLLandscape();

            DataRow rowNew = tbNew.NewRow();
            rowNew["Company"] = clsGSLLandscape.Company;
            rowNew["Project"] = clsGSLLandscape.Project;
            rowNew["Reference"] = clsGSLLandscape.Reference;
            rowNew["TimeIn"] = clsGSLLandscape.TimeIn;
            rowNew["TimeOut"] = clsGSLLandscape.TimeOut;
            rowNew["IssuedDate"] = clsGSLLandscape.IssuedDate;
            rowNew["ReceivedDate"] = clsGSLLandscape.ReceivedDate;
            rowNew["SupervisorName"] = clsGSLLandscape.SupervisorName;
            rowNew["SupervisorSign"] = clsGSLLandscape.SupervisorSign;
            rowNew["SignedDate1"] = clsGSLLandscape.SignedDate1;
            rowNew["ClientName"] = clsGSLLandscape.ClientName;
            rowNew["ClientSign"] = clsGSLLandscape.ClientSign;
            rowNew["SignedDate2"] = clsGSLLandscape.SignedDate2;
            rowNew["AtcEntry"] = clsGSLLandscape.AtcEntry;
            //rowNew["SAPURL"] = clsGSLLandscape.SAPURL;
            //rowNew["filename"] = clsGSLLandscape.filename;

            tbNew.Rows.Add(rowNew);

            return tbNew.Copy();
        }

        private DataTable Save_ConvertToGSLLandscapeLine(List<JSON_SaveGSLLandscapeDetail> lstGSLGarden)
        {
            DataTable tbNew = new DataTable();

            tbNew = LineTableGSLLandscape();

            foreach (var item in lstGSLGarden)
            {
                DataRow rowNew = tbNew.NewRow();
                rowNew["Description"] = item.Description;
                rowNew["Done"] = item.Done;
                rowNew["Remark"] = item.Remark;

                tbNew.Rows.Add(rowNew);
            }

            return tbNew.Copy();
        }

        private DataTable HeaderTableGSLLandscape()
        {
            DataTable tbLandscape = new DataTable();
            tbLandscape.Columns.Add("Company");
            tbLandscape.Columns.Add("Project");
            tbLandscape.Columns.Add("Reference");
            tbLandscape.Columns.Add("TimeIn");
            tbLandscape.Columns.Add("TimeOut");
            tbLandscape.Columns.Add("IssuedDate");
            tbLandscape.Columns.Add("ReceivedDate");
            tbLandscape.Columns.Add("SupervisorName");
            tbLandscape.Columns.Add("SupervisorSign");
            tbLandscape.Columns.Add("SignedDate1");
            tbLandscape.Columns.Add("ClientName");
            tbLandscape.Columns.Add("ClientSign");
            tbLandscape.Columns.Add("SignedDate2");
            tbLandscape.Columns.Add("AtcEntry");
            tbLandscape.Columns.Add("SAPURL");
            tbLandscape.Columns.Add("filename");

            return tbLandscape;
        }

        private DataTable LineTableGSLLandscape()
        {
            DataTable tbLandscape = new DataTable();
            tbLandscape.Columns.Add("Description");
            tbLandscape.Columns.Add("Done");
            tbLandscape.Columns.Add("Remark");

            return tbLandscape;
        }

        #endregion

        #endregion

        #region WebMethods for EPS Management Service Report

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_ServiceReport_Project(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_ServiceReport_Project";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sCurrentUserName = string.Empty;
                string sUserRole = string.Empty;

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
                    sUserRole = objProject.sUserRole;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_GSLLandscape_Project() ", sFuncName);
                DataSet ds = oEPSPestReport.Get_EPSPestReport_Project(dsCompanyList, sCompany, sCurrentUserName, sUserRole);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_GSLLandscape_Project() ", sFuncName);
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
        public void MGet_ServiceReport_ListofServiceReport(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_ServiceReport_ListofServiceReport";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProjectCode = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_ListofJobs> lstDeserialize = js.Deserialize<List<Json_ListofJobs>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_ListofJobs objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sProjectCode = objProject.ProjectCode;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_EPSPestReport_ListofServiceReport() ", sFuncName);
                DataSet ds = oEPSPestReport.Get_EPSPestReport_ListofServiceReport(dsCompanyList, sCompany, sProjectCode);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_EPSPestReport_ListofServiceReport() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<ListofServiceReport> lstSrvRpt = new List<ListofServiceReport>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ListofServiceReport _srvReport = new ListofServiceReport();
                        _srvReport.DocNum = r["DocNum"].ToString();
                        _srvReport.DocEntry = r["DocEntry"].ToString();
                        _srvReport.DocDate = ChangeDateWithPrefix0(r["DocDate"].ToString());
                        _srvReport.SupervisorName = r["SupervisorName"].ToString();
                        _srvReport.ClientName = r["ClientName"].ToString();
                        lstSrvRpt.Add(_srvReport);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Service Report list ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstSrvRpt));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Service Report list, the serialized data is ' " + js.Serialize(lstSrvRpt) + " '", sFuncName);
                }
                else
                {
                    List<ListofServiceReport> lstSrvRpt = new List<ListofServiceReport>();
                    Context.Response.Output.Write(js.Serialize(lstSrvRpt));
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
        public void MGet_ServiceReport_ViewServiceReport(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_ServiceReport_ViewServiceReport";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sDocNum = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_EPSDetails> lstDeserialize = js.Deserialize<List<Json_EPSDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_EPSDetails objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sDocNum = objProject.DocNum;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_EPSPestReport_ViewServiceReport() ", sFuncName);
                DataSet ds = oEPSPestReport.Get_EPSPestReport_ViewServiceReport(dsCompanyList, sCompany, sDocNum);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_EPSPestReport_ViewServiceReport() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    ////Automapper is the DLL, which is used to convert the dataset to List.
                    //DataTableReader dataTableReader = ds.Tables[0].CreateDataReader();
                    //Mapper.CreateMap<IDataReader, List<ServiceReportDetails>>();
                    //List<ServiceReportDetails> lstServiceReport = Mapper.Map<IDataReader, List<ServiceReportDetails>>(dataTableReader);

                    List<JSON_Content> lstContents = new List<JSON_Content>();
                    List<JSON_Pesticide> lstPesticide = new List<JSON_Pesticide>();
                    List<SA_Attachments> lstAttach = new List<SA_Attachments>();
                    JSON_Reports lstReport = new JSON_Reports();
                    SaveServiceReport _ServiceReportDetails = new SaveServiceReport();
                    List<SaveServiceReport> lstServiceReportDetails = new List<SaveServiceReport>();

                    foreach (DataRow drItems in ds.Tables[0].Rows)
                    {
                        var match = lstPesticide.Where(c => c.Pesticide == Convert.ToString(drItems["Pesticide"])).ToList();
                        if (match.Count == 0)
                        {
                            lstPesticide.Add(new JSON_Pesticide
                            {
                                Pesticide = drItems["Pesticide"].ToString(),
                                Quantity = drItems["Quantity"].ToString()
                            });
                        }
                    }

                    foreach (DataRow drItems in ds.Tables[0].Rows)
                    {
                        var match = lstContents.Where(c => c.Description == Convert.ToString(drItems["Description"])).ToList();
                        if (match.Count == 0)
                        {
                            lstContents.Add(new JSON_Content
                            {
                                Description = drItems["Description"].ToString(),
                                DescriptionOth = drItems["DescriptionOth"].ToString(),
                                Include = drItems["Include"].ToString(),
                                Active = drItems["Active"].ToString(),
                                Nonactive = drItems["Nonactive"].ToString(),
                                TFogging = drItems["TFogging"].ToString(),
                                TMisting = drItems["TMisting"].ToString(),
                                TResidual = drItems["TResidual"].ToString(),
                                TBaits = drItems["TBaits"].ToString(),
                                TDusting = drItems["TDusting"].ToString(),
                                TTraps = drItems["TTraps"].ToString(),
                                TOthers = drItems["TOthers"].ToString(),
                                Location = drItems["Location"].ToString(),
                                IGS = drItems["IGS"].ToString(),
                                AGS = drItems["AGS"].ToString()
                            });
                        }
                    }

                    lstReport.rWeekly = ds.Tables[0].Rows[0]["rWeekly"].ToString();
                    lstReport.rFortnightly = ds.Tables[0].Rows[0]["rFortnightly"].ToString();
                    lstReport.rMonthly = ds.Tables[0].Rows[0]["rMonthly"].ToString();
                    lstReport.rBimonthly = ds.Tables[0].Rows[0]["rBimonthly"].ToString();
                    lstReport.rQuarterly = ds.Tables[0].Rows[0]["rQuarterly"].ToString();
                    lstReport.rOnetime = ds.Tables[0].Rows[0]["rOnetime"].ToString();
                    lstReport.rFollowup = ds.Tables[0].Rows[0]["rFollowup"].ToString();

                    //for adding the  Service Report details in Header and merge the line of Array items
                    _ServiceReportDetails.Company = sCompany;

                    _ServiceReportDetails.Project = ds.Tables[0].Rows[0]["Project"].ToString();
                    _ServiceReportDetails.AddId = ds.Tables[0].Rows[0]["AddId"].ToString();
                    _ServiceReportDetails.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    _ServiceReportDetails.Block = ds.Tables[0].Rows[0]["Block"].ToString();
                    _ServiceReportDetails.Unit = ds.Tables[0].Rows[0]["Unit"].ToString();
                    _ServiceReportDetails.Postal = ds.Tables[0].Rows[0]["Postal"].ToString();
                    _ServiceReportDetails.ReportOth = ds.Tables[0].Rows[0]["ReportOth"].ToString();
                    _ServiceReportDetails.EmailId = ds.Tables[0].Rows[0]["EmailId"].ToString();
                    _ServiceReportDetails.ACommonArea = ds.Tables[0].Rows[0]["ACommonArea"].ToString();
                    _ServiceReportDetails.Abuilding = ds.Tables[0].Rows[0]["Abuilding"].ToString();
                    _ServiceReportDetails.ACorridor = ds.Tables[0].Rows[0]["ACorridor"].ToString();
                    _ServiceReportDetails.AGarden = ds.Tables[0].Rows[0]["AGarden"].ToString();
                    _ServiceReportDetails.ADrainage = ds.Tables[0].Rows[0]["ADrainage"].ToString();
                    _ServiceReportDetails.APlayground = ds.Tables[0].Rows[0]["APlayground"].ToString();
                    _ServiceReportDetails.ABinCentre = ds.Tables[0].Rows[0]["ABinCentre"].ToString();
                    _ServiceReportDetails.ACarPark = ds.Tables[0].Rows[0]["ACarPark"].ToString();
                    _ServiceReportDetails.ALightning = ds.Tables[0].Rows[0]["ALightning"].ToString();
                    _ServiceReportDetails.AElectrical = ds.Tables[0].Rows[0]["AElectrical"].ToString();
                    _ServiceReportDetails.AStoreroom = ds.Tables[0].Rows[0]["AStoreroom"].ToString();
                    _ServiceReportDetails.ARooftop = ds.Tables[0].Rows[0]["ARooftop"].ToString();
                    _ServiceReportDetails.AManhole = ds.Tables[0].Rows[0]["AManhole"].ToString();
                    _ServiceReportDetails.ARiser = ds.Tables[0].Rows[0]["ARiser"].ToString();
                    _ServiceReportDetails.AOffice = ds.Tables[0].Rows[0]["AOffice"].ToString();
                    _ServiceReportDetails.ACanteen = ds.Tables[0].Rows[0]["ACanteen"].ToString();
                    _ServiceReportDetails.AKitchen = ds.Tables[0].Rows[0]["AKitchen"].ToString();
                    _ServiceReportDetails.ACabinet = ds.Tables[0].Rows[0]["ACabinet"].ToString();
                    _ServiceReportDetails.AGullyTrap = ds.Tables[0].Rows[0]["AGullyTrap"].ToString();
                    _ServiceReportDetails.AOthers = ds.Tables[0].Rows[0]["AOthers"].ToString();
                    _ServiceReportDetails.AOthersDesc = ds.Tables[0].Rows[0]["AOthersDesc"].ToString();
                    _ServiceReportDetails.Scope = ds.Tables[0].Rows[0]["Scope"].ToString();
                    _ServiceReportDetails.IHousekeeping = ds.Tables[0].Rows[0]["IHousekeeping"].ToString();
                    _ServiceReportDetails.ISanitation = ds.Tables[0].Rows[0]["ISanitation"].ToString();
                    _ServiceReportDetails.IStructural = ds.Tables[0].Rows[0]["IStructural"].ToString();
                    _ServiceReportDetails.IOthers = ds.Tables[0].Rows[0]["IOthers"].ToString();
                    _ServiceReportDetails.IOthersDesc = ds.Tables[0].Rows[0]["IOthersDesc"].ToString();
                    _ServiceReportDetails.IComments = ds.Tables[0].Rows[0]["IComments"].ToString();
                    _ServiceReportDetails.Supervisor = ds.Tables[0].Rows[0]["Supervisor"].ToString();
                    _ServiceReportDetails.SupervisorSign = ds.Tables[0].Rows[0]["SupervisorSign"].ToString();
                    _ServiceReportDetails.SignedDate1 = ChangeDateWithPrefix0(ds.Tables[0].Rows[0]["SignedDate1"].ToString());
                    _ServiceReportDetails.Client = ds.Tables[0].Rows[0]["Client"].ToString();
                    _ServiceReportDetails.ClientSign = ds.Tables[0].Rows[0]["ClientSign"].ToString();
                    _ServiceReportDetails.SignedDate2 = ChangeDateWithPrefix0(ds.Tables[0].Rows[0]["SignedDate2"].ToString());
                    _ServiceReportDetails.Feedback = ds.Tables[0].Rows[0]["Feedback"].ToString();
                    _ServiceReportDetails.AtcEntry = ds.Tables[0].Rows[0]["AtcEntry"].ToString();
                    //_ServiceReportDetails.Report = ds.Tables[0].Rows[0]["Report"].ToString();
                    _ServiceReportDetails.TimeIn = ds.Tables[0].Rows[0]["TimeIn"].ToString();
                    _ServiceReportDetails.TimeOut = ds.Tables[0].Rows[0]["TimeOut"].ToString();
                    _ServiceReportDetails.AToilet = ds.Tables[0].Rows[0]["AToilet"].ToString();

                    string sItem = "Description = '" + ds.Tables[0].Rows[0]["Description"].ToString() + "'";

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = sItem;

                    foreach (DataRowView rowView in dv)
                    {
                        DataRow row = rowView.Row;
                        SA_Attachments _docAttachment = new SA_Attachments();
                        _docAttachment.WebURL = row["WebURL"].ToString();
                        _docAttachment.Remarks = row["AttachmentRemarks"].ToString();
                        lstAttach.Add(_docAttachment);
                        // Do something //
                    }

                    _ServiceReportDetails.Content = lstContents;
                    _ServiceReportDetails.Pesticide = lstPesticide;
                    _ServiceReportDetails.Report = lstReport;
                    _ServiceReportDetails.Attachments = lstAttach;

                    lstServiceReportDetails.Add(_ServiceReportDetails);

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Service Report Details ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstServiceReportDetails));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Service Report Details, the serialized data is ' " + js.Serialize(lstServiceReportDetails) + " '", sFuncName);
                }
                else
                {
                    List<ServiceReportDetails> lstServiceReport = new List<ServiceReportDetails>();
                    Context.Response.Output.Write(js.Serialize(lstServiceReport));
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
        public void MGet_ServiceReport_GetTemplate(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_ServiceReport_GetTemplate";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_Company> lstDeserialize = js.Deserialize<List<Json_Company>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_Company objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_EPSPestReport_GetTemplate() ", sFuncName);
                DataSet ds = oEPSPestReport.Get_EPSPestReport_GetTemplate(dsCompanyList, sCompany);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_EPSPestReport_GetTemplate() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<ServiceReportTemplate> lstLandscapeTmpl = new List<ServiceReportTemplate>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ServiceReportTemplate _srvTemplate = new ServiceReportTemplate();
                        _srvTemplate.Question = r["Question"].ToString();
                        lstLandscapeTmpl.Add(_srvTemplate);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Service Report Template List", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstLandscapeTmpl));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Service Report Template List, the serialized data is ' " + js.Serialize(lstLandscapeTmpl) + " '", sFuncName);
                }
                else
                {
                    List<ServiceReportTemplate> lstSrvRptTmpl = new List<ServiceReportTemplate>();
                    Context.Response.Output.Write(js.Serialize(lstSrvRptTmpl));
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
        public void MGet_ServiceReport_GetScopeofWork(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_ServiceReport_GetScopeofWork";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProject = string.Empty;
                string sAddId = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_ScopeofWork> lstDeserialize = js.Deserialize<List<Json_ScopeofWork>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_ScopeofWork objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                    sProject = objProject.Project;
                    sAddId = objProject.AddId;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_EPSPestReport_GetScopeofWork() ", sFuncName);
                DataSet ds = oEPSPestReport.Get_EPSPestReport_GetScopeofWork(dsCompanyList, sCompany, sProject, sAddId);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_EPSPestReport_GetScopeofWork() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<ServiceReportScopeofWork> lstSrvRptScope = new List<ServiceReportScopeofWork>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ServiceReportScopeofWork _scopeOfWork = new ServiceReportScopeofWork();
                        _scopeOfWork.Address = r["Address"].ToString();
                        _scopeOfWork.Block = r["Block"].ToString();
                        _scopeOfWork.Unit = r["Unit"].ToString();
                        _scopeOfWork.Postal = r["Postal"].ToString();
                        _scopeOfWork.Street = r["Street"].ToString();
                        _scopeOfWork.ScopeofWork = r["ScopeofWork"].ToString();
                        _scopeOfWork.EmailId = r["EmailId"].ToString();
                        lstSrvRptScope.Add(_scopeOfWork);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Service Report Template List", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstSrvRptScope));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Service Report Template List, the serialized data is ' " + js.Serialize(lstSrvRptScope) + " '", sFuncName);
                }
                else
                {
                    List<ServiceReportScopeofWork> lstSrvRptScope = new List<ServiceReportScopeofWork>();
                    Context.Response.Output.Write(js.Serialize(lstSrvRptScope));
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
        public void MGet_ServiceReport_GetShipToAddress(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_ServiceReport_GetShipToAddress";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sProject = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_GetShipToAddress> lstDeserialize = js.Deserialize<List<Json_GetShipToAddress>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_GetShipToAddress objShipToAddr = lstDeserialize[0];
                    sCompany = objShipToAddr.Company;
                    sProject = objShipToAddr.Project;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_EPSPestReport_GetScopeofWork() ", sFuncName);
                DataSet ds = oEPSPestReport.Get_EPSPestReport_GetScopeofWork(dsCompanyList, sCompany, sProject, string.Empty);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_EPSPestReport_GetScopeofWork() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<ServiceReportShipToAddress> lstSrvRptShipToAddr = new List<ServiceReportShipToAddress>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ServiceReportShipToAddress _shipToAddress = new ServiceReportShipToAddress();
                        _shipToAddress.Address = r["ShipToAddress"].ToString();
                        _shipToAddress.AddressId = r["ShipToAddress"].ToString();
                        lstSrvRptShipToAddr.Add(_shipToAddress);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the Service Report Address List", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstSrvRptShipToAddr));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the Service Report Address List, the serialized data is ' " + js.Serialize(lstSrvRptShipToAddr) + " '", sFuncName);
                }
                else
                {
                    List<ServiceReportShipToAddress> lstSrvRptScope = new List<ServiceReportShipToAddress>();
                    Context.Response.Output.Write(js.Serialize(lstSrvRptScope));
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
        public void MSave_ServiceReport(string sJsonInput)
        {
            string sFuncName = string.Empty;
            string xmlResult = string.Empty;
            string sProject = string.Empty;
            string sWarehouse = string.Empty;
            string sItemCode = string.Empty;
            string sClientSign = string.Empty; string sSupervisorSign = string.Empty;
            string sConvertClientSign = string.Empty; string sConvertSupSign = string.Empty;

            try
            {
                sFuncName = "MSave_ServiceReport";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<SaveServiceReport> serviceList = js.Deserialize<List<SaveServiceReport>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                SaveServiceReport serviceDetails = serviceList[0];
                List<SA_Attachments> attachments = serviceDetails.Attachments;
                List<JSON_Content> content = serviceDetails.Content;
                List<JSON_Pesticide> pesticide = serviceDetails.Pesticide;
                JSON_Reports reports = serviceDetails.Report;

                if (serviceList != null && serviceList.Count > 0)
                {
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Connecting to target company ", sFuncName);
                    oDICompany = oLogin.ConnectToTargetCompany(serviceList[0].Company);

                    //Declare the objects:

                    SAPbobsCOM.GeneralService oGeneralService = null;
                    SAPbobsCOM.GeneralData oGeneralData;
                    SAPbobsCOM.GeneralDataCollection oChildren = null;
                    SAPbobsCOM.GeneralData oChild = null;
                    SAPbobsCOM.CompanyService oCompanyService = oDICompany.GetCompanyService();
                    oGeneralService = oCompanyService.GetGeneralService("PESTServiceReport");

                    oGeneralData = (SAPbobsCOM.GeneralData)oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData);

                    string sAddId = serviceDetails.AddId;
                    //Adding the Header Informations
                    oGeneralData.SetProperty("U_DocDate", DateTime.Now.Date);
                    oGeneralData.SetProperty("U_Project", serviceDetails.Project);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before adding AddId : " + sAddId, sFuncName);
                    oGeneralData.SetProperty("U_AddId", sAddId);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After adding AddId : " + sAddId, sFuncName);
                    oGeneralData.SetProperty("U_Address", serviceDetails.Address);
                    oGeneralData.SetProperty("U_Block", serviceDetails.Block);
                    oGeneralData.SetProperty("U_Unit", serviceDetails.Unit);
                    oGeneralData.SetProperty("U_Postal", serviceDetails.Postal);

                    string[] startTimeSplit = serviceDetails.TimeIn.ToString().Split(':');
                    string[] endTimeSplit = serviceDetails.TimeOut.ToString().Split(':');
                    DateTime dtInTime = DateTime.Now.Date;
                    dtInTime = dtInTime.AddHours(Convert.ToDouble(startTimeSplit[0]));
                    dtInTime = dtInTime.AddMinutes(Convert.ToDouble(startTimeSplit[1]));

                    DateTime dtOutTime = DateTime.Now.Date;
                    dtOutTime = dtOutTime.AddHours(Convert.ToDouble(endTimeSplit[0]));
                    dtOutTime = dtOutTime.AddMinutes(Convert.ToDouble(endTimeSplit[1]));

                    Random rnd = new Random();
                    int fileId = rnd.Next(1, 100000000);

                    Random rnd1 = new Random();
                    int fileId1 = rnd1.Next(1, 100000000);

                    sConvertClientSign = serviceDetails.ClientSign.ToString().Replace(" ", "+").ToString();
                    sClientSign = MGet_SaveESignature(sConvertClientSign, serviceList[0].Company, "PClintSignature" + fileId);
                    sConvertSupSign = serviceDetails.SupervisorSign.ToString().Replace(" ", "+").ToString();
                    sSupervisorSign = MGet_SaveESignature(sConvertSupSign, serviceList[0].Company, "PSupSignature" + fileId1);

                    oGeneralData.SetProperty("U_TimeIn", dtInTime);
                    oGeneralData.SetProperty("U_TimeOut", dtOutTime);
                    //oGeneralData.SetProperty("U_Report", serviceDetails.Report);
                    oGeneralData.SetProperty("U_ReportOth", serviceDetails.ReportOth);

                    oGeneralData.SetProperty("U_AOthersDesc", serviceDetails.AOthersDesc);
                    oGeneralData.SetProperty("U_Scope", serviceDetails.Scope);

                    oGeneralData.SetProperty("U_IOthersDesc", serviceDetails.IOthersDesc);
                    oGeneralData.SetProperty("U_IComments", serviceDetails.IComments);
                    oGeneralData.SetProperty("U_Supervisor", serviceDetails.Supervisor);
                    oGeneralData.SetProperty("U_SupervisorSign", sSupervisorSign);
                    oGeneralData.SetProperty("U_SupvrSignText", sConvertSupSign);
                    oGeneralData.SetProperty("U_SignedDate1", serviceDetails.SignedDate1);
                    oGeneralData.SetProperty("U_Client", serviceDetails.Client);
                    oGeneralData.SetProperty("U_ClientSign", sClientSign);
                    oGeneralData.SetProperty("U_ClintSignText", sConvertClientSign);
                    oGeneralData.SetProperty("U_SignedDate2", serviceDetails.SignedDate2);
                    if (serviceDetails.Feedback != null)
                    {
                        oGeneralData.SetProperty("U_Feedback", serviceDetails.Feedback);
                    }
                    if (serviceDetails.AtcEntry != null)
                    {
                        oGeneralData.SetProperty("U_AtcEntry", serviceDetails.AtcEntry);
                    }
                    oChildren = oGeneralData.Child("AB_PESTRPT1");

                    //Looping  the  Line Details

                    for (int i = 0; i <= content.Count - 1; i++)
                    {
                        oChild = oChildren.Add();
                        oChild.SetProperty("U_Description", content[i].Description.ToString());
                        oChild.SetProperty("U_DescriptionOth", content[i].DescriptionOth.ToString());
                        oChild.SetProperty("U_Include", ConvertToSingleValue(content[i].Include.ToString()));
                        oChild.SetProperty("U_Active", ConvertToSingleValue(content[i].Active.ToString()));
                        oChild.SetProperty("U_Nonactive", ConvertToSingleValue(content[i].Nonactive.ToString()));
                        oChild.SetProperty("U_TFogging", ConvertToSingleValue(content[i].TFogging.ToString()));
                        oChild.SetProperty("U_TMisting", ConvertToSingleValue(content[i].TMisting.ToString()));
                        oChild.SetProperty("U_TResidual", ConvertToSingleValue(content[i].TResidual.ToString()));
                        oChild.SetProperty("U_TBaits", ConvertToSingleValue(content[i].TBaits.ToString()));
                        oChild.SetProperty("U_TDusting", ConvertToSingleValue(content[i].TDusting.ToString()));
                        oChild.SetProperty("U_TTraps", ConvertToSingleValue(content[i].TTraps.ToString()));
                        oChild.SetProperty("U_TOthers", ConvertToSingleValue(content[i].TOthers.ToString()));
                        oChild.SetProperty("U_Location", content[i].Location.ToString());
                        oChild.SetProperty("U_IGS", ConvertToSingleValue(content[i].IGS.ToString()));
                        oChild.SetProperty("U_AGS", ConvertToSingleValue(content[i].AGS.ToString()));
                    }

                    oChildren = oGeneralData.Child("AB_PESTRPT2");

                    //Looping  the  Line Details

                    for (int i = 0; i <= pesticide.Count - 1; i++)
                    {
                        oChild = oChildren.Add();
                        oChild.SetProperty("U_Pesticide", pesticide[i].Pesticide.ToString());
                        oChild.SetProperty("U_Quantity", Convert.ToDouble(pesticide[i].Quantity.ToString()));
                    }

                    oChildren = oGeneralData.Child("AB_PESTRPT3");

                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Common Area");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ACommonArea));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Apron of Building");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.Abuilding));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Common Corridor");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ACorridor));
                    if (serviceDetails.AGarden != null)
                    {
                        oChild = oChildren.Add();
                        oChild.SetProperty("U_AreaOfInsp", "Landscape / Garden");
                        oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.AGarden));
                    }
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Drainage");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ADrainage));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Playground");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.APlayground));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Bin Centre / Chute");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ABinCentre));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Car Park");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ACarPark));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Lightning Conductor Pit");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ALightning));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Electrical / Gas Room");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.AElectrical));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Store room");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.AStoreroom));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Roof Top");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ARooftop));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Manhole");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.AManhole));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Toilet ");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.AToilet));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Riser");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ARiser));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Office / Pantry / Rooms");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.AOffice));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Food Outlet / Canteen");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ACanteen));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Kitchen");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.AKitchen));
                    if (serviceDetails.ACabinet != null)
                    {
                        oChild = oChildren.Add();
                        oChild.SetProperty("U_AreaOfInsp", "Cabinet / Racks");
                        oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ACabinet));
                    }
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Gully Trap");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.AGullyTrap));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_AreaOfInsp", "Others");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.AOthers));


                    oChildren = oGeneralData.Child("AB_PESTRPT4");
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_ToImprove", "Housekeeping ");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.IHousekeeping));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_ToImprove", "Sanitation");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.ISanitation));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_ToImprove", "Structural Defects");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.IStructural));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_ToImprove", "Others");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(serviceDetails.IOthers));

                    oChildren = oGeneralData.Child("AB_PESTRPT5");
                    //for (int i = 0; i <= reports.Count - 1; i++)
                    //{
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_Report", "Weekly ");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(reports.rWeekly.ToString()));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_Report", "Fortnightly");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(reports.rFortnightly.ToString()));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_Report", "Monthly");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(reports.rMonthly.ToString()));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_Report", "Bi-Monthly");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(reports.rBimonthly.ToString()));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_Report", "Quarterly");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(reports.rQuarterly.ToString()));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_Report", "One-time/Ad-hoc");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(reports.rOnetime.ToString()));
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_Report", "Follow up");
                    oChild.SetProperty("U_Include", ConvertToSingleValue(reports.rFollowup.ToString()));
                    //}

                    for (int i = 0; i <= attachments.Count - 1; i++)
                    {
                        if (attachments[i].SAPURL.ToString() != string.Empty || attachments[i].SAPURL.ToString() != "")
                        {
                            // This is for Attachment.
                            oChildren = oGeneralData.Child("AB_PESTRPT6");
                            oChild = oChildren.Add();

                            oChild.SetProperty("U_Path", attachments[i].SAPURL);
                            oChild.SetProperty("U_FileName", attachments[i].filename);
                            oChild.SetProperty("U_AttachDate", DateTime.Now.Date);
                            oChild.SetProperty("U_Remarks", attachments[i].Remarks);
                        }
                    }
                    //Add/Update the header document;
                    oGeneralService.Add(oGeneralData);

                    //return the result to mobile
                    result objResult = new result();
                    objResult.Result = "Success";
                    objResult.DisplayMessage = "Document is created successfully in SAP";
                    lstResult.Add(objResult);
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
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MGet_ServiceReport_BPEmailIds(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "MGet_ServiceReport_BPEmailIds";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                //Split JSON to Individual String
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_Company> lstDeserialize = js.Deserialize<List<Json_Company>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_Company objProject = lstDeserialize[0];
                    sCompany = objProject.Company;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method Get_EPSPestReport_GetBPEmailIds() ", sFuncName);
                DataSet ds = oEPSPestReport.Get_EPSPestReport_GetBPEmailIds(dsCompanyList, sCompany);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method Get_EPSPestReport_GetBPEmailIds() ", sFuncName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<EPSBPEmaiIds> lstEmailIds = new List<EPSBPEmaiIds>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        EPSBPEmaiIds _EmailId = new EPSBPEmaiIds();
                        _EmailId.CardCode = r["CardCode"].ToString();
                        _EmailId.EmailId = r["EmailId"].ToString();
                        lstEmailIds.Add(_EmailId);
                    }

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Serializing the EmailId ", sFuncName);
                    Context.Response.Output.Write(js.Serialize(lstEmailIds));
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Serializing the EmailId, the serialized data is ' " + js.Serialize(lstEmailIds) + " '", sFuncName);
                }
                else
                {
                    List<EPSBPEmaiIds> lstEmailIds = new List<EPSBPEmaiIds>();
                    Context.Response.Output.Write(js.Serialize(lstEmailIds));
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
        }

        #region Classes

        class ListofServiceReport
        {
            public string DocNum { get; set; }
            public string DocEntry { get; set; }
            public string DocDate { get; set; }
            public string SupervisorName { get; set; }
            public string ClientName { get; set; }
        }

        class ServiceReportDetails
        {

        }

        class ServiceReportTemplate
        {
            public string Question { get; set; }
        }

        class ServiceReportScopeofWork
        {
            public string Address { get; set; }
            public string Block { get; set; }
            public string Unit { get; set; }
            public string Postal { get; set; }
            public string Street { get; set; }
            public string ScopeofWork { get; set; }
            public string EmailId { get; set; }
        }

        class ServiceReportShipToAddress
        {
            public string AddressId { get; set; }
            public string Address { get; set; }
        }

        class SaveServiceReport
        {
            public string Company { get; set; }
            public string Project { get; set; }
            public string AddId { get; set; }
            public string Address { get; set; }
            public string Block { get; set; }
            public string Unit { get; set; }
            public string Postal { get; set; }
            public string TimeIn { get; set; }
            public string TimeOut { get; set; }
            //public string Report { get; set; }
            public string ReportOth { get; set; }
            public string EmailId { get; set; }
            public string ACommonArea { get; set; }
            public string Abuilding { get; set; }
            public string ACorridor { get; set; }
            public string AGarden { get; set; }
            public string ADrainage { get; set; }
            public string APlayground { get; set; }
            public string ABinCentre { get; set; }
            public string ACarPark { get; set; }
            public string ALightning { get; set; }
            public string AElectrical { get; set; }
            public string AStoreroom { get; set; }
            public string ARooftop { get; set; }
            public string AManhole { get; set; }
            public string AToilet { get; set; }
            public string ARiser { get; set; }
            public string AOffice { get; set; }
            public string ACanteen { get; set; }
            public string AKitchen { get; set; }
            public string ACabinet { get; set; }
            public string AGullyTrap { get; set; }
            public string AOthers { get; set; }
            public string AOthersDesc { get; set; }
            public string Scope { get; set; }
            public string IHousekeeping { get; set; }
            public string ISanitation { get; set; }
            public string IStructural { get; set; }
            public string IOthers { get; set; }
            public string IOthersDesc { get; set; }
            public string IComments { get; set; }
            public string Supervisor { get; set; }
            public string SupervisorSign { get; set; }
            public string SignedDate1 { get; set; }
            public string Client { get; set; }
            public string ClientSign { get; set; }
            public string SignedDate2 { get; set; }
            public string Feedback { get; set; }
            public string AtcEntry { get; set; }
            public string Weekly { get; set; }
            public string Fortnightly { get; set; }
            public string Monthly { get; set; }
            public string BiMonthly { get; set; }
            public string Quarterly { get; set; }
            public string Onetime { get; set; }
            public string Followup { get; set; }
            //public string SAPURL { get; set; }
            //public string WebURL { get; set; }
            //public string filename { get; set; }
            public List<JSON_Content> Content { get; set; }
            public List<JSON_Pesticide> Pesticide { get; set; }
            public List<SA_Attachments> Attachments { get; set; }
            public JSON_Reports Report { get; set; }
        }

        class JSON_Content
        {

            public string Description { get; set; }
            public string DescriptionOth { get; set; }
            public string Include { get; set; }
            public string Active { get; set; }
            public string Nonactive { get; set; }
            public string TFogging { get; set; }
            public string TMisting { get; set; }
            public string TResidual { get; set; }
            public string TBaits { get; set; }
            public string TDusting { get; set; }
            public string TTraps { get; set; }
            public string TOthers { get; set; }
            public string IGS { get; set; }
            public string AGS { get; set; }
            public string Location { get; set; }
        }

        class JSON_Pesticide
        {
            public string Pesticide { get; set; }
            public string Quantity { get; set; }
        }

        class JSON_Reports
        {
            public string rWeekly { get; set; }
            public string rFortnightly { get; set; }
            public string rMonthly { get; set; }
            public string rBimonthly { get; set; }
            public string rQuarterly { get; set; }
            public string rOnetime { get; set; }
            public string rFollowup { get; set; }
        }

        class EPSBPEmaiIds
        {
            public string CardCode { get; set; }
            public string EmailId { get; set; }
        }

        #endregion

        #region public methods

        public string ConvertToSingleValue(string sValue)
        {
            string sResult = string.Empty;
            if (sValue.ToUpper() == "YES")
            {
                sResult = "Y";
            }
            else if (sValue.ToUpper() == "NO")
            {
                sResult = "N";
            }
            else if (sValue == "")
            {
                sResult = "N";
            }
            return sResult;
        }

        #endregion

        #endregion

        [WebMethod(EnableSession = true)]
        public void Attachments()
        {
            string sFuncName = string.Empty;
            AttachmentResult objResult = new AttachmentResult();
            List<AttachmentURL> lstAttachment = new List<AttachmentURL>();
            try
            {
                sFuncName = "Attachments";
                int i;
                HttpContext postedContext = HttpContext.Current;

                for (i = 0; i < postedContext.Request.Files.Count; i++)
                {
                    HttpPostedFile hpf = postedContext.Request.Files[i];

                    DataSet dsCompanyList = oLogin.Get_CompanyList();
                    string PathToShowInSap = string.Empty;
                    string PathToShowInWeb = string.Empty;
                    string sTimeStampValue = string.Empty;
                    string sCompanyName = (string)postedContext.Request.Form["companyname"];
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Company name in request form : " + sCompanyName, sFuncName);
                    DataSet ds = oShowAround.Get_AttachPath(dsCompanyList, sCompanyName);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        PathToShowInSap = ds.Tables[0].Rows[0][0].ToString();
                        PathToShowInWeb = Server.MapPath("~/Attachments/");
                        sTimeStampValue = MyExtensions.AppendTimeStamp(hpf.FileName);
                        hpf.SaveAs(PathToShowInSap + sTimeStampValue);
                        hpf.SaveAs(PathToShowInWeb + sTimeStampValue);
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With Success  ", sFuncName);

                        objResult.Result = "Success";
                        objResult.DisplayMessage = "Attachment Successfully Added. Now Click the Save Button to Create a Document";
                        AttachmentURL objAttachment = new AttachmentURL();
                        objAttachment.WebURL = Convert.ToString(PathToShowInWeb + sTimeStampValue).Replace("&", "%26");
                        objAttachment.SAPURL = Convert.ToString(PathToShowInSap + sTimeStampValue).Replace("&", "%26");
                        lstAttachment.Add(objAttachment);
                        objResult.Attachments = lstAttachment;
                        lstAttResult.Add(objResult);
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Attachment Path from Mobile for SAP : " + objResult.Attachments[i].SAPURL, sFuncName);
                    }
                    else
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                        // AttachmentResult objResult = new AttachmentResult();
                        objResult.Result = "Error";
                        objResult.DisplayMessage = "Attachment Path is Empty in  SAP. Check with SAP";
                        objResult.Attachments = lstAttachment;
                        lstAttResult.Add(objResult);
                    }
                }
                Context.Response.Output.Write(js.Serialize(lstAttResult));

            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                objResult.Attachments = lstAttachment;
                lstAttResult.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstAttResult));
            }
        }

        [WebMethod(EnableSession = true)]
        public void AttachmentsWithRemarks(string sJsonInput)
        {
            string sFuncName = string.Empty;
            int i = 0;
            AttachmentResultWithRemarks objResult = new AttachmentResultWithRemarks();
            List<AttachmentURLWithRemarks> lstAttachment = new List<AttachmentURLWithRemarks>();
            try
            {
                sFuncName = "AttachmentsWithRemarks";

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                //sJsonInput = "[" + sJsonInput + "]";
                //Convert JSON to Array
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<AttachmentWithRemarks> attachWithRemarks = js.Deserialize<List<AttachmentWithRemarks>>(sJsonInput);
                List<AttachmentURLWithRemarks> attachWithRemarksURL = js.Deserialize<List<AttachmentURLWithRemarks>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);

                foreach (AttachmentWithRemarks item in attachWithRemarks)
                {
                    //string directory = HttpContext.Current.Server.MapPath("TempAttachments");
                    string directory = ConfigurationManager.AppSettings["AttachmentPath"].ToString();
                    string directorytoMove = HttpContext.Current.Server.MapPath("Attachments");

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("AttachmentPath : " + "=" + directory, sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("directorytoMove : " + "=" + directorytoMove, sFuncName);
                    var files = Directory.GetFiles(directory, item.FileName.ToString());
                    var Editedfiles = Directory.GetFiles(directorytoMove, item.FileName.ToString());
                    if (files != null && files.Length > 0)
                    {
                        DataSet dsCompanyList = oLogin.Get_CompanyList();
                        string PathToShowInSap = string.Empty;
                        string PathToShowInWeb = string.Empty;
                        string sTimeStampValue = string.Empty;
                        DataSet ds = oShowAround.Get_AttachPath(dsCompanyList, item.Company.ToString());
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            string rootFolderPath = directory;
                            string destinationPath = directorytoMove;
                            string filesToDelete = item.FileName.ToString();
                            //string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                            //foreach (string file in fileList)
                            //{
                            PathToShowInSap = ds.Tables[0].Rows[0][0].ToString();
                            PathToShowInWeb = Server.MapPath("~/Attachments/");
                            sTimeStampValue = MyExtensions.AppendTimeStamp(filesToDelete);
                            string fileToDelete = rootFolderPath + "/" + filesToDelete;
                            //string moveTo = destinationPath + "/" + filesToDelete;
                            //moving file
                            File.Copy(fileToDelete, PathToShowInSap + sTimeStampValue);
                            File.Copy(fileToDelete, PathToShowInWeb + sTimeStampValue);
                            File.Delete(fileToDelete);

                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With Success  ", sFuncName);

                            //objResult.Result = "Success";
                            //objResult.DisplayMessage = "";
                            AttachmentURLWithRemarks objAttachment = new AttachmentURLWithRemarks();
                            objAttachment.WebURL = Convert.ToString(PathToShowInWeb + sTimeStampValue).Replace("&", "%26");
                            objAttachment.SAPURL = Convert.ToString(PathToShowInSap + sTimeStampValue).Replace("&", "%26");
                            objAttachment.Remarks = item.Remarks;
                            objAttachment.DelFlag = item.DelFlag;
                            lstAttachment.Add(objAttachment);
                            //objResult.Attachments = lstAttachment;
                            //lstAttResultWithRemarks.Add(objResult);
                            //}
                        }
                        else
                        {
                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                            // AttachmentResult objResult = new AttachmentResult();
                            objResult.Result = "Error";
                            objResult.DisplayMessage = "Attachment Path is Empty in  SAP. Check with SAP";
                            objResult.Attachments = lstAttachment;
                            lstAttResultWithRemarks.Add(objResult);
                            goto ExitLoop;
                        }
                    }
                    else if (Editedfiles != null && Editedfiles.Length > 0)
                    {
                        if (item.DelFlag == "Y")
                        {
                            DataSet dsCompanyList = oLogin.Get_CompanyList();
                            DataSet ds = oShowAround.Get_AttachPath(dsCompanyList, item.Company.ToString());
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                var fileCheck = Directory.GetFiles(ds.Tables[0].Rows[0][0].ToString() + "/", item.FileName.ToString());
                                if (fileCheck != null)
                                {
                                    File.Delete(ds.Tables[0].Rows[0][0].ToString() + "/" + item.FileName.ToString());
                                }
                            }
                            string fileToDelete = directorytoMove + "/" + item.FileName.ToString();
                            File.Delete(fileToDelete);
                        }
                        AttachmentURLWithRemarks objAttachment = new AttachmentURLWithRemarks();
                        objAttachment.WebURL = Convert.ToString(attachWithRemarksURL[i].WebURL).Replace("&", "%26");
                        objAttachment.SAPURL = Convert.ToString(attachWithRemarksURL[i].WebURL).Replace("&", "%26"); // In the name of Web URL, we are passing the SAP URL
                        objAttachment.Remarks = item.Remarks;
                        objAttachment.DelFlag = item.DelFlag;
                        lstAttachment.Add(objAttachment);
                    }
                    else
                    {
                        if (item.FileMethod != "old")
                        {
                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                            objResult.Result = "Error";
                            objResult.DisplayMessage = "There is no files in the Attachment Path";
                            objResult.Attachments = lstAttachment;
                            lstAttResultWithRemarks.Add(objResult);
                            goto ExitLoop;
                        }
                        else
                        {
                            AttachmentURLWithRemarks objAttachment = new AttachmentURLWithRemarks();
                            objAttachment.WebURL = Convert.ToString(attachWithRemarksURL[i].WebURL).Replace("&", "%26");
                            objAttachment.SAPURL = Convert.ToString(attachWithRemarksURL[i].WebURL).Replace("&", "%26");// In the name of Web URL, we are passing the SAP URL
                            objAttachment.Remarks = item.Remarks;
                            objAttachment.DelFlag = item.DelFlag;
                            lstAttachment.Add(objAttachment);
                        }
                    }
                    i = i + 1;
                }
                objResult.Result = "Success";
                objResult.DisplayMessage = "Attachment Successfully Added. Now Click the Save Button to Create a Document";
                objResult.Attachments = lstAttachment;
                lstAttResultWithRemarks.Add(objResult);
            ExitLoop:
                Context.Response.Output.Write(js.Serialize(lstAttResultWithRemarks));
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                objResult.Result = "Error";
                objResult.DisplayMessage = sErrDesc;
                objResult.Attachments = lstAttachment;
                lstAttResultWithRemarks.Add(objResult);
                Context.Response.Output.Write(js.Serialize(lstAttResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void SendEmail(string sJsonInput)
        {
            string sFuncName = string.Empty;
            try
            {
                sFuncName = "SendEmail";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Starting Function ", sFuncName);
                string sCompany = string.Empty;
                string sDocNum = string.Empty;
                string sEmailId = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_SendEmail> lstDeserialize = js.Deserialize<List<Json_SendEmail>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_SendEmail objSendMail = lstDeserialize[0];
                    sCompany = objSendMail.Company;
                    sDocNum = objSendMail.DocNum;
                    sEmailId = objSendMail.EmailId;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Calling the method CreatePDFandSendEmail() ", sFuncName);
                string sResult = CreatePDFandSendEmail(dsCompanyList, sCompany, sEmailId, sDocNum);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Calling the method CreatePDFandSendEmail() ", sFuncName);
                if (sResult == "Success")
                {
                    result objResult = new result();
                    objResult.Result = "Success";
                    objResult.DisplayMessage = "Email Sent Successfully.";
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else
                {
                    oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "Error";
                    objResult.DisplayMessage = sResult;
                    lstResult.Add(objResult);
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
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void CreatePDF(string sJsonInput)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            try
            {
                sFuncName = "CreatePDF";
                sProcName = "SP_AB_FRM_EPSServiceReport";
                string sCompany = string.Empty;
                string sDocNum = string.Empty;

                sJsonInput = "[" + sJsonInput + "]";
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Getting the Json Input from Mobile  '" + sJsonInput + "'", sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Deserialize the Json Input ", sFuncName);
                List<Json_EPSDetails> lstDeserialize = js.Deserialize<List<Json_EPSDetails>>(sJsonInput);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Deserialize the Json Input ", sFuncName);
                if (lstDeserialize.Count > 0)
                {
                    Json_EPSDetails objCreatePDF = lstDeserialize[0];
                    sCompany = objCreatePDF.Company;
                    sDocNum = objCreatePDF.DocNum;
                }

                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Retrieving the Company list ", sFuncName);
                DataSet dsCompanyList = oLogin.Get_CompanyList();
                //DataSet dsCompanyList = (DataSet)Session["ODTCompanyList"];
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("After Retrieving the Company list ", sFuncName);

                ReportDocument cryRpt = new ReportDocument();
                ReportDocument ERRPT = new ReportDocument();
                CrystalDecisions.Shared.ConnectionInfo objConInfo = new CrystalDecisions.Shared.ConnectionInfo();
                CrystalDecisions.Shared.TableLogOnInfo oLogonInfo = new CrystalDecisions.Shared.TableLogOnInfo();
                //Dim ConInfo As New CrystalDecisions.Shared.TableLogOnInfo
                int intCounter = 0;
                //Dim Formula As String
                if (dsCompanyList != null && dsCompanyList.Tables.Count > 0)
                {
                    oDTView = dsCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DocKey", sDocNum));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        //return "No Data in OUSR table for the selected Company";
                    }


                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Creating PDF file for DocNum : " + sDocNum, sFuncName);
                    //Create PDF file

                    string sFileName = "/PDF/EPS/" + sDocNum + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + DateTime.Now.Millisecond + ".pdf";
                    string directory = HttpContext.Current.Server.MapPath("TEMP") + "/PDF/EPS";
                    string AttachFile = HttpContext.Current.Server.MapPath("TEMP") + sFileName;
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    cryRpt.Load(Server.MapPath("Report") + "/EPS Pest Management Service Report.rpt");

                    ParameterValues crParameterValues = new ParameterValues();
                    ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                    string SerNam = null;
                    string DbName = null;
                    string UID = null;
                    SerNam = oDTView[0]["U_Server"].ToString();
                    DbName = oDTView[0]["U_DBName"].ToString();
                    UID = oDTView[0]["U_DBUserName"].ToString();

                    oLogonInfo.ConnectionInfo.ServerName = SerNam;
                    oLogonInfo.ConnectionInfo.DatabaseName = DbName;
                    oLogonInfo.ConnectionInfo.UserID = UID;
                    oLogonInfo.ConnectionInfo.Password = oDTView[0]["U_DBPassword"].ToString();

                    for (intCounter = 0; intCounter <= cryRpt.Database.Tables.Count - 1; intCounter++)
                    {
                        cryRpt.Database.Tables[intCounter].ApplyLogOnInfo(oLogonInfo);
                    }

                    cryRpt.SetParameterValue("@DocKey", sDocNum);

                    ExportOptions CrExportOptions = default(ExportOptions);
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    ExcelFormatOptions CrExcelFormat = new ExcelFormatOptions();
                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                    ExcelFormatOptions CrExcelTypeOptions = new ExcelFormatOptions();

                    CrDiskFileDestinationOptions.DiskFileName = AttachFile;
                    CrExportOptions = cryRpt.ExportOptions;
                    var _with1 = CrExportOptions;
                    _with1.ExportDestinationType = ExportDestinationType.DiskFile;
                    _with1.ExportFormatType = ExportFormatType.PortableDocFormat;
                    _with1.DestinationOptions = CrDiskFileDestinationOptions;
                    _with1.FormatOptions = CrFormatTypeOptions;
                    cryRpt.Export();

                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("PDF Created successfully for Doc Num : " + sDocNum, sFuncName);
                    result objResult = new result();
                    objResult.Result = "Success";
                    objResult.DisplayMessage = "/TEMP" + sFileName;
                    lstResult.Add(objResult);
                    Context.Response.Output.Write(js.Serialize(lstResult));
                }
                else
                {
                    oLog.WriteToErrorLogFile("There is No Company List in the Holding Company", sFuncName);
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR : There is No Company List in the Holding Company ", sFuncName);
                    result objResult = new result();
                    objResult.Result = "Failure";
                    objResult.DisplayMessage = "There is No Company List in the Holding Company";
                    lstResult.Add(objResult);
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
                Context.Response.Output.Write(js.Serialize(lstResult));
            }
        }

        public string CreatePDFandSendEmail(DataSet oDTCompanyList, string sCompany, string sEmailId, string sDocNum)
        {
            DataSet oDataset = new DataSet();
            string sFuncName = string.Empty;
            string sProcName = string.Empty;
            DataView oDTView = new DataView();
            string errMsg = "XXX";
            try
            {
                string EPS = sDocNum;
                sFuncName = "CreatePDFandSendEmail";
                sProcName = "SP_AB_FRM_EPSServiceReport";
                ReportDocument cryRpt = new ReportDocument();
                ReportDocument ERRPT = new ReportDocument();
                CrystalDecisions.Shared.ConnectionInfo objConInfo = new CrystalDecisions.Shared.ConnectionInfo();
                CrystalDecisions.Shared.TableLogOnInfo oLogonInfo = new CrystalDecisions.Shared.TableLogOnInfo();
                //Dim ConInfo As New CrystalDecisions.Shared.TableLogOnInfo
                int intCounter = 0;
                //Dim Formula As String
                if (oDTCompanyList != null && oDTCompanyList.Tables.Count > 0)
                {
                    oDTView = oDTCompanyList.Tables[0].DefaultView;

                    oDTView.RowFilter = "U_DBName= '" + sCompany + "'";
                    if (oDTView != null && oDTView.Count > 0)
                    {
                        oDataset = SqlHelper.ExecuteDataSet(oDTView[0]["U_ConnString"].ToString(), CommandType.StoredProcedure, sProcName,
                            Data.CreateParameter("@DocKey", sDocNum));

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With SUCCESS  ", sFuncName);
                    }
                    else
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("No Data in OUSR table for the selected Company", sFuncName);
                        return "No Data in OUSR table for the selected Company";
                    }

                    if (sEmailId.Length > 0)
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Before Creating PDF file and Send Email for DocEntry : " + sDocNum + " with Id : " + sEmailId, sFuncName);
                        //Create PDF file

                        string directory = HttpContext.Current.Server.MapPath("TEMP") + "/PDF/EPS";
                        string AttachFile = HttpContext.Current.Server.MapPath("TEMP") + "/PDF/EPS/" + EPS + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + DateTime.Now.Millisecond + ".pdf";
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        cryRpt.Load(Server.MapPath("Report") + "/EPS Pest Management Service Report.rpt");


                        ParameterValues crParameterValues = new ParameterValues();
                        ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                        string SerNam = null;
                        string DbName = null;
                        string UID = null;
                        SerNam = oDTView[0]["U_Server"].ToString();
                        DbName = oDTView[0]["U_DBName"].ToString();
                        UID = oDTView[0]["U_DBUserName"].ToString();

                        oLogonInfo.ConnectionInfo.ServerName = SerNam;
                        oLogonInfo.ConnectionInfo.DatabaseName = DbName;
                        oLogonInfo.ConnectionInfo.UserID = UID;
                        oLogonInfo.ConnectionInfo.Password = oDTView[0]["U_DBPassword"].ToString();

                        for (intCounter = 0; intCounter <= cryRpt.Database.Tables.Count - 1; intCounter++)
                        {
                            cryRpt.Database.Tables[intCounter].ApplyLogOnInfo(oLogonInfo);
                        }


                        cryRpt.SetParameterValue("@DocKey", EPS);


                        ExportOptions CrExportOptions = default(ExportOptions);
                        DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                        ExcelFormatOptions CrExcelFormat = new ExcelFormatOptions();
                        PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                        ExcelFormatOptions CrExcelTypeOptions = new ExcelFormatOptions();


                        CrDiskFileDestinationOptions.DiskFileName = AttachFile;
                        CrExportOptions = cryRpt.ExportOptions;
                        var _with1 = CrExportOptions;
                        _with1.ExportDestinationType = ExportDestinationType.DiskFile;
                        _with1.ExportFormatType = ExportFormatType.PortableDocFormat;
                        _with1.DestinationOptions = CrDiskFileDestinationOptions;
                        _with1.FormatOptions = CrFormatTypeOptions;
                        cryRpt.Export();

                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("PDF Created successfully for Doc Entry : " + sDocNum + " with Id: " + sEmailId, sFuncName);
                        //Send Email

                        if (File.Exists(AttachFile))
                        {
                            Attachment att = new Attachment(AttachFile);
                            att.Name = sDocNum + ".pdf";


                            string body = string.Empty;
                            using (StreamReader reader = new StreamReader(Server.MapPath("~/Email/EmailTemplate.htm")))
                            {
                                body = reader.ReadToEnd();
                                body = body.Replace("{DeliDate}", sDocNum);
                            }
                            string subject = "EPS Pest Management Service Report: " + att.Name;
                            if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Ready to send Email for Doc Entry : " + sDocNum + " with Id: " + sEmailId, sFuncName);

                            string[] emailArray = sEmailId.Split(',');
                            errMsg = Emails.SendEmail(System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString(), emailArray, subject, body, new Attachment[] { att }, null);

                        }
                    }
                    else
                    {
                        if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("There is No Email Id in SAP Buisness Partner for this Document", sFuncName);
                        return "There is No Email Id in SAP Buisness Partner for this Document";
                    }
                }
                else
                {
                    if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("There is No Company List in the Holding Company ", sFuncName);
                    return "There is No Company List in the Holding Company";
                }
                return errMsg;
            }
            catch (Exception ex)
            {
                sErrDesc = ex.Message.ToString();
                oLog.WriteToErrorLogFile(sErrDesc, sFuncName);
                if (p_iDebugMode == DEBUG_ON) oLog.WriteToDebugLogFile("Completed With ERROR  ", sFuncName);
                return sErrDesc;
            }
        }

        
    }
}
