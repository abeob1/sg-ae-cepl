loadProject();

$(document).ready(function(){

	$("#form-field-select-1").change(function(){

		if($(this).val() != ""){
			sessionStorage.listjbProject = $(this).val();
			
			var mProjectData = {};
			
			var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_JobSchedule_ListofJobs"
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}
			
			mProjectData["Company"] =  companyUser["CompanyCode"];
			mProjectData["ProjectCode"] = $("#form-field-select-1").val();
		}
	});
});

   // Print pdf to Open Job completion report in new tab

    $("#print").click(function () {
								
		validateRequired(function(errorMsg){
			if(errorMsg != ""){
				alert(errorMsg);
			}
			else{
								
        var mProjectData = {};

        var url = CONFIG.OPERATIONS_BASEPATH + "/MonthlyJobSchedule_CreatePDF"
        var companyUser = sessionStorage["user"];
        if (companyUser) {
            companyUser = JSON.parse(companyUser);
        }
		
		var month = $("#form-field-select-2").val();
		var year  = $("#form-field-select-3").val();
		
        mProjectData["Company"] = companyUser["CompanyCode"];
		mProjectData["Project"] = $("#form-field-select-1").val();	
		mProjectData["MonthName"] = month;
		mProjectData["Year"] = year;
	   
        showLoading();
        postToServer(url, "POST", JSON.stringify(mProjectData), function (data) {
	 	hideLoading();
            if (data != "" && data.length > 0) {
                data = data[0];

                var displaymsg = data["DisplayMessage"]; // URL coming from webservice, [{"Result":"Success","DisplayMessage":"E:\\Abeo-Projects\\Cleaning Express\\Publish\\UAT\\CleaningExpress_Webservice\\TEMP/PDF/EPS/1_07072015090010177.pdf"}]
                 //alert(displaymsg, false);
                // testing if PDF is opening with correct URL or not
                displaymsg = CONFIG.WEBSERVICEURL + displaymsg;// when you have URL like this comment this line
				showLoading();
				window.open(displaymsg, '_blank');// Opens PDF in new window.
				hideLoading();
				
				
            }
        });
     	}
	});
    });	

function loadProject(){
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_JobSchedule_Project";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["sCompany"] =  companyUser["CompanyCode"];
	mProjectData["sCurrentUserName"] = companyUser["UserName"];
	mProjectData["sUserRole"] = companyUser["RoleName"];
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		if(data!="" && data.length>0){
				var projList = data;
								
				var opt ='<option value="">Select</option>';
				if(projList && projList[0] && projList[0].PrcCode){

					$(data).each(function(i,project){
										  
						var proj_name = project["PrcName"];
						var proj_name_decode = decodeURIComponent(proj_name);				  
						opt += '<option value="'+project["PrcCode"]+'">'+proj_name_decode+'</option>';

					});
					$("#form-field-select-1").html(opt);

				}

			}

			else{
				// $(".showlistdocs").html("<div class='mrt-20'> No Details Found</div>");
			}
			hideLoading();
			preSelectProject();

	});
}

function preSelectProject(){

	if(sessionStorage.listjbProject){
		$("#form-field-select-1").val(sessionStorage.listjbProject);
		$("#form-field-select-1").change();

	}

}

function validateRequired(callBack){
	var errorMsg  = "";
	if($("#form-field-select-1").val()==""){
		errorMsg = "Please select the Project.";

	}
	else if($("#form-field-select-2").val() == ""){
		errorMsg = "Please select Month.";
	}
	else if($("#form-field-select-3").val() == ""){
		errorMsg = "Please select year.";
	}
	callBack(errorMsg);

}