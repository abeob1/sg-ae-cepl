loadProject();

loadQuestions();

$(document).ready(function(){

	$("#cancel").click(function(){
		window.location.href = "list-landscape.php";
	});

});

function loadQuestions(){
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_GSLLandscape_ViewLandscape";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["Company"] =  companyUser["CompanyCode"];
	mProjectData["DocEntry"] = sessionStorage.lsDocEntry;
	
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		sessionStorage["lsChk"] = JSON.stringify(data);

		if(data!="" && data.length>0){
												
			$(".gQuestions").html("");
			
			data = data[0];

			// setting common values
			$("#form-field-select-1").val(data["Project"]);
			$("#reference").val(data["Reference"]);
			$("#timein").val(data["TimeIn"]);
			$("#timeout").val(data["TimeOut"]);
			$("#idate").val(showDate(data["IssuedDate"].split(" ")[0]));
			$("#rdate").val(showDate(data["ReceivedDate"].split(" ")[0]));
			
			var supervisor = data["SupervisorName"];
			var supervisor_decode = decodeURIComponent(supervisor);
			$("#supervisor").val(supervisor_decode);
			
			var client = data["ClientName"];
			var client_decode = decodeURIComponent(client);
			$("#client").val(client_decode);

			// Signature
			// initialize signature
			$("#supSign,#clSign").jSignature({color:"#000000",height:'110px',width:'100%'});

			var string60Format = [];
			string60Format[0] = "data:image/png;base64";
			string60Format[1] = data["SupervisorSign"];

			$("#supSign").jSignature("setData",string60Format.join(","));

			string60Format[1] = data["ClientSign"];

			$("#clSign").jSignature("setData",string60Format.join(","));

			$("#supsigndate").val(showDate(data["SignedDate1"].split(" ")[0]));
			$("#clsigndate").val(showDate(data["SignedDate2"].split(" ")[0]));

			// disable signature
			$("#clSign").jSignature("disable");
			$("#supSign").jSignature("disable");

			var remarks = data["Remarks"];
			var remarks_decode = decodeURIComponent(remarks);
			$("#remarks").val(remarks_decode);

			// Attachment
			var attachFiles = data["Attachments"];
			var atag = "";
			$(attachFiles).each(function(j,file){

				var filename = getFilename(file["WebURL"] || "");
				var filename_decode = decodeURIComponent(filename);
				atag += '<a href="'+(CONFIG.ATTACHMENT_DIR+filename)+'" download>'+filename_decode+'</a><br/>';

			});
			
			$("#atcEntry").html(atag);
			/*var filename = getFilename(data["WebURL"] || "");
			
			$("#atcEntry").attr("href",CONFIG.ATTACHMENT_DIR+filename).text(filename);*/

			$(data.LandscapeChk).each(function(i,item){
			
				var htm = '<div class="row mrb-5"><div class="col-md-1 col-sm-1 col-xs-1">'+(i+1)+'</div><div class="col-md-4 col-sm-4 col-xs-4">'+decodeURIComponent(item["Description"])+'</div>'+
							'<div class="col-md-3 col-sm-3 col-xs-3"><input type="text" class="form-control done" value="'+decodeURIComponent(item["Done"])+'" disabled/></div>'+
							'<div class="col-md-4 col-sm-4 col-xs-3"><input type="text" class="form-control gremarks" value="'+decodeURIComponent(item["Remark"])+'" disabled/></div>'+
							'</div><br /><div class="clearfix"></div>';

				$(".gQuestions").append(htm);

				
			});

		
			

		}

		else{
			$(".gQuestions").html("<div class='mrt-20'> No Details Found</div>");
		}

		

			hideLoading();


	});
}



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
			

	});
}
