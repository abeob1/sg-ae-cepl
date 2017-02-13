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
			mProjectData["ProjectCode"] = encodeURIComponent($("#form-field-select-1").val());
//console.log("Company:"+companyUser["CompanyCode"]);
//console.log("Data:"+$("#form-field-select-1").val());
					
			showLoading();
			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
				if(data!="" && data.length>0){

					$(".projectList").html("");
					$(data).each(function(i,item){
						
						var itemHtm = '<div class="list fullList"><table class="table-responsive"><tr><td class="sname"><a class="sDoc" href="" data-docentry="'+item["DocEntry"]+'">'+item["DocNum"]+'</a></td>'+
										'<td></td> </tr> <tr><td class="sname1">'+decodeURIComponent(item["PrcCode"])+ " " + decodeURIComponent(item["PrcName"]) +'</td><td class="sname2">'+item["DocStatus"]+'</td>' +
										' </tr></table></div>'

						$(".projectList").append(itemHtm);
						
					});

				}
				else{
					$(".projectList").html("<div class='mrt-20'> No Details Found</div>");
				}
					
					
					hideLoading();
					
					
			});

		}
		else{

			$(".projectList").html("");
			$(".addFeed").hide();
			

		}

	});

	/*$(".projectList").on('click','.sDoc',function(){
		sessionStorage.fDoc = $(this).data("docentry");
		window.location.href = "view-feedback.php";
	});*/

	$(".projectList").on('click','.list',function(e){
		e.preventDefault();
		sessionStorage.jbdocEntry = $(this).find("a").data("docentry");
		window.location.href = "view-calender.php";
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
						var proj_name_decode =decodeURIComponent(proj_name);
						/*var first = project["PrcCode"].charAt(0);
						
						console.log(companyUser["CompanyCode"]);
						if(first!="m" || first!="M" ){
						   opt += '<option value="'+project["PrcCode"]+'">'+proj_name_decode+'</option>';
						}*/
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