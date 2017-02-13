$(document).ready(function(){

	loadProjectData();

	$("#form-field-select-1").change(function(){
		pendingIssue($(this).val());
		sessionStorage.listprProject = $(this).val();

	});

	/*$("#projList").on('click','.gProject',function(){
		sessionStorage.gDocEntry = $(this).data("docentry");
		window.location.href = "list-goods.php";
	});*/

	$("#projList").on('click','.list',function(e){
		e.preventDefault();
		sessionStorage.gDocEntry = $(this).find("a").data("docentry");
		window.location.href = "list-goods.php";
	});

});

function loadProjectData(){
	var url = CONFIG.BASEPATH + "/MGet_GoodsIssueProject";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["sCompany"] =  companyUser["CompanyCode"];
	mProjectData["sCurrentUserName"] = companyUser["UserName"];
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
			
		var opt ='<option value="">Select</option>';
		$(data).each(function(i,project){
							  
			var proj_name = project["PrcName"];
			var proj_name_decode = decodeURIComponent(proj_name);
			opt += '<option value="'+project["PrcCode"]+'">'+proj_name_decode+'</option>';

		});
		$("#form-field-select-1").html(opt);
		hideLoading();
		// preSelect if exists
		preSelectProject();
	});
}

function pendingIssue(proj){
	var url = CONFIG.BASEPATH + "/MGet_GoodsIssueList";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["sCompany"] =  companyUser["CompanyCode"];
	mProjectData["sCurrentUserName"] = companyUser["UserName"];
	mProjectData["sProjectCode"] = proj;
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
		$("#projList").html("");
		$(data).each(function(i,project){
			var opt = '<div class="list fullList"><table class="table-responsive"><tr>' +
					'<td class="sname"><a href="javascript:void(0)" class="gProject" data-docentry="'+project["DocEntry"]+'">'+project["DocNum"]+'</a></td><td></td>'+
					'</tr> <tr>' + '<td class="sname1">'+decodeURIComponent(project["ProjectCode"])+ " " +decodeURIComponent(project["ProjectName"])+'</td>' + '<td class="sname2">'+localeDate(project["ReqDate"])+'</td>'+
					'</tr><tr>' + '<td class="sname1">'+project["ReqName"]+'</td>' + '<td class="sname2">'+project["Status"]+'</td>'+
					'</tr></table></div>';

			$("#projList").append(opt);
		});
		
		hideLoading();
	});

}

function preSelectProject(){

	if(sessionStorage.listprProject){
		$("#form-field-select-1").val(sessionStorage.listprProject);
		pendingIssue(sessionStorage.listprProject);

	}

}