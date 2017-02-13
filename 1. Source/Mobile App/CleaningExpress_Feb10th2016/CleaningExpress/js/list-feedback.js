loadProject();

$(document).ready(function(){

	$("#form-field-select-1").change(function(){

		if($(this).val() != ""){
			sessionStorage.listfdProject = $(this).val();
			$(".addFeed").show();
			var mProjectData = {};
			//hardcoded need to be changed...
			// var url="data/MGet_ItemList.json";
			var url = CONFIG.BASEPATH + "/MGet_CustomerFeedback_List"
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}
			
			mProjectData["sCompany"] =  companyUser["CompanyCode"];
			mProjectData["sCurrentUserName"] = companyUser["UserName"];
			mProjectData["sProject"] = $("#form-field-select-1").val();

			var feedProject = {};
			feedProject.txt = $("#form-field-select-1 option:selected").text();
			feedProject.val = $("#form-field-select-1").val();
			sessionStorage.feedProject  = JSON.stringify(feedProject);
			
			showLoading();
			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
				if(data!="" && data.length>0){

					$(".projectList").html("");
					$(data).each(function(i,item){
						
						var itemHtm = '<div class="list sAround mrt-20 pad20"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6 pad0">' +
								'<h4 class="sname"><a class="sDoc" data-docentry="'+item["DocEntry"]+'">'+item["DocNum"]+'</a></h4>' +
								'<h5 class="sname2">'+localeDate(item["DocDate"].split(" ")[0])+'</h5>' + '</div></div></div>';

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

});

function loadProject(){
	var url = CONFIG.BASEPATH + "/MGet_CustomerFeedBack_Project";
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

	if(sessionStorage.listfdProject){
		$("#form-field-select-1").val(sessionStorage.listfdProject);
		$("#form-field-select-1").change();

	}

}