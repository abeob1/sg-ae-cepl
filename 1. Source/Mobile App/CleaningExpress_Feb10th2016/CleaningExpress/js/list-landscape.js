loadProject();

$(document).ready(function(){

	$("#form-field-select-1").change(function(){

		if($(this).val() != ""){
			sessionStorage.listlsProject = $(this).val();
			$(".addls").show();
			
			var mProjectData = {};
			
			var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_GSLLandscape_ListofLandscape"
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}
			
			mProjectData["Company"] =  companyUser["CompanyCode"];
			mProjectData["ProjectCode"] = $("#form-field-select-1").val();

					
			showLoading();
			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
				if(data!="" && data.length>0){

					$(".projectList").html("");
					$(data).each(function(i,item){
						
						/*var itemHtm = '<div class="list fullList"><table class="table-responsive"><tr><td class="sname"><a class="sDoc" href="" data-docentry="'+item["DocEntry"]+'">'+(item["DocEntry"].split(" ")[0])+'</a></td>'+
										'<td></td> </tr> <tr><td class="sname1">'+item["MktSegment"]+'</td><td class="sname2"></td>' +
										' </tr></table></div>'*/
						var itemHtm = '<div class="list fullList"><div class="row"><div class="col-md-3 col-sm-3 col-xs-3">'+item["DocNum"]+'</div>'+
										'<div class="col-md-3 col-sm-3 col-xs-3 mrgl-5">'+localeDate(item["DocDate"].split(" ")[0])+'</div>'+
										'<div class="col-md-3 col-sm-3 col-xs-3 mrgl-15"><a href="" data-docentry="'+item["DocEntry"]+'">'+decodeURIComponent(item["SupervisorName"])+'</a></div>'+
										'<div class="col-md-3 col-sm-3 col-xs-3 mrgl-5">'+decodeURIComponent(item["ClientName"])+'</div></div></div>';

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
			$(".addls").hide();
			

		}

	});

	$(".projectList").on('click','.list',function(e){
		e.preventDefault();
		sessionStorage.lsDocEntry = $(this).find("a").data("docentry");
		window.location.href = "view-landscape.php";
	});

});

function loadProject(){
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_GSLLandscape_Project";
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

	if(sessionStorage.listlsProject){
		$("#form-field-select-1").val(sessionStorage.listlsProject);
		$("#form-field-select-1").change();

	}

}