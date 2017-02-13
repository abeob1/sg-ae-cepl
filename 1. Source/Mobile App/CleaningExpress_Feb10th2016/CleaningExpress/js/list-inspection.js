loadProject();

$(document).ready(function(){

	$("#form-field-select-1").change(function(){

		if($(this).val() != ""){
			sessionStorage.listinsProject = $(this).val();
			$(".addInsp").show();
			
			var mProjectData = {};
			
			var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_InspectionQA_ListofMSC"
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}
			
			mProjectData["Company"] =  companyUser["CompanyCode"];
			mProjectData["ProjectCode"] = encodeURIComponent($("#form-field-select-1").val());

					
			showLoading();
			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
				if(data!="" && data.length>0){

					$(".projectList").html("");
					$(data).each(function(i,item){
						//<a class="sDoc" href="" data-docentry="'+item["DocEntry"]+'">'
						/*var itemHtm = '<div class="list fullList"><table class="table-responsive"><tr><td class="sname"><a class="sDoc" href="" data-docentry="'+item["DocNum"]+'">'+(item["DocEntry"].split(" ")[0])+'</a></td>'+
          '<td></td> </tr> <tr><td class="sname1">'+item["MktSegment"]+'</td><td class="sname2"></td>' +
          ' </tr></table></div>'*/
		  				//sessionStorage.MktSegmentId = item["MktSegmentId"];
						var itemHtm = '<div class="list fullList"><table class="table-responsive"><tr><td class="sname"><a class="sDoc" href="" data-docentry="'+item["DocNum"]+'">'+(item["DocEntry"].split(" ")[0])+'</a></td>'+
          '<td></td> </tr> <tr><td class="sname1">'+decodeURIComponent(item["MktSegment"])+'</td><td class="sname2"></td>' +
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
			$(".addInsp").hide();
			

		}

	});

	/*$(".projectList").on('click','.sDoc',function(){
		sessionStorage.fDoc = $(this).data("docentry");
		window.location.href = "view-feedback.php";
	});*/

	$(".projectList").on('click','.list',function(e){
		e.preventDefault();
		sessionStorage.insdocEntry = $(this).find("a").data("docentry");
		window.location.href = "view-inspection.php";
	});

});

function loadProject(){
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_InspectionQA_Project";
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
										  
						var proj = project["PrcName"];
						var proj_decode = decodeURIComponent(proj);
						opt += '<option value="'+project["PrcCode"]+'">'+proj_decode+'</option>';

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

	if(sessionStorage.listinsProject){
		$("#form-field-select-1").val(sessionStorage.listinsProject);
		$("#form-field-select-1").change();

	}

}