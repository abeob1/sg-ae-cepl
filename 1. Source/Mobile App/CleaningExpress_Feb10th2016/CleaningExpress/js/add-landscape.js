loadProject();

loadQuestions();

$(document).ready(function(){

	// Signature
	$("#supSign,#clSign").jSignature({color:"#000000",height:'110px',width:'100%'});
	// initialize signature Date
	$("#supsigndate,#clsigndate,#idate,#rdate").val(getToday());

	$("#cancel").click(function(){
		window.location.href = "list-landscape.php";
	});

	$("#clClear").click(function(){
		$("#clSign").jSignature("reset");
	});

	$("#supClear").click(function(){
		$("#supSign").jSignature("reset");
	});

	$("#save").click(function(){

		validateRequired(function(errorMsg){
			if(errorMsg != ""){
				alert(errorMsg);
			}
			else{

				if(confirm("Confirm Save?")){

					var url = CONFIG.OPERATIONS_BASEPATH + "/MSave_GSLLandscape";
					var companyUser = sessionStorage["user"];
					if(companyUser){
						companyUser = JSON.parse(companyUser);
					}

					var landscape = {};
					landscape["Company"] = companyUser["CompanyCode"];
					landscape["Project"] = $("#form-field-select-1").val();
					landscape["Reference"] = $("#reference").val();
					landscape["TimeIn"] = $("#timein").val();
					landscape["TimeOut"] = $("#timeout").val();
					landscape["IssuedDate"] = convertDate($("#idate").val());
					landscape["ReceivedDate"] = convertDate($("#rdate").val());
					landscape["SupervisorName"] = encodeURIComponent($("#supervisor").val());
					landscape["ClientName"] = encodeURIComponent($("#client").val());
					landscape["SignedDate1"] = convertDate($("#supsigndate").val());
					landscape["SignedDate2"] = convertDate($("#clsigndate").val());
					

					var signature60Format = $("#supSign").jSignature("getData");

					if(signature60Format.split(",")[1] != emptySign){
						landscape["SupervisorSign"] = signature60Format.split(",")[1];
					}
					else{
						landscape["SupervisorSign"] = "";
					}


					signature60Format = $("#clSign").jSignature("getData");

					if(signature60Format.split(",")[1] != emptySign){
						landscape["ClientSign"] = signature60Format.split(",")[1];
					}
					else{
						landscape["ClientSign"] = "";
					}

					landscape["LandscapeChk"] = [];

					$(".done").each(function(){
						var chkObj = {};
						chkObj["Done"] = $(this).val();
						var Remark =$(this).parent().next().find(".gremarks").val();
						var Remark_d = encodeURIComponent(Remark);
						chkObj["Remark"] = Remark_d; 
						
						var Desc =$(this).parent().prev().text().trim();
						var Desc_d = encodeURIComponent(Desc);
						chkObj["Description"] = Desc_d; 

						landscape["LandscapeChk"].push(chkObj);

					});

					// Attachments
					landscape["Attachments"] = [];
					
					var attachmentFiles = sessionStorage["landscapeFile"] || "";
					if(attachmentFiles){
						attachmentFiles = JSON.parse(attachmentFiles);

						$(attachmentFiles).each(function(j,file){
							var aFile = {};
							aFile["WebURL"] = file["WebURL"];
							aFile["SAPURL"] = file["SAPURL"];
							aFile["FileName"] = getFilename(file["WebURL"]);
							aFile["Remarks"] = encodeURIComponent(file["Remarks"]);
							landscape["Attachments"].push(aFile);
						});
					}
					sessionStorage["landscapeFile"] = "";

					/*landscape["WebURL"] = sessionStorage["landscapeFile"] || "";
					landscape["SAPURL"] = sessionStorage["landscapeFileSAP"] || "";
					landscape["FileName"] = getFilename(landscape["WebURL"]);
					*/
					console.log(JSON.stringify(landscape));

					showLoading();
					postToServer(url,"POST",JSON.stringify(landscape),function(data){
						// console.log(data);
						if(data[0] && data[0].Result=="Success"){
							alert(data[0].DisplayMessage);
							window.location.href = "list-landscape.php";
						}
						else{
							alert("Error ! Message: " + data[0].DisplayMessage);
						}

						hideLoading();
					});


				}

			}
		});

	});
	
	$("#attach").click(function(){

		var url = CONFIG.OPERATIONS_BASEPATH + "/Attachments";
		var companyUser = sessionStorage["user"];
		if(companyUser){
			companyUser = JSON.parse(companyUser);
		}
		$(".companyname").val(companyUser["CompanyCode"]);
		showLoading();
		if($(".upform").val() == ""){
			alert("Please select file to uplaod");
			return false;
		}
		uploadFileToServer(url,"POST",$("#upform"),function(data){
			// console.log(data);
			if(data[0] && data[0].Result=="Success"){
				alert(data[0].DisplayMessage);
				sessionStorage["landscapeFile"] = JSON.stringify(data[0].Attachments);
				// sessionStorage["landscapeFileSAP"] = data[0].SAPURL;
				$("#attach,.upform").attr("disabled",true);
				$("#save").attr("disabled",false);
			}
			else{
				alert("Error ! Message: " + data[0].DisplayMessage);
			}

			hideLoading();
		});

	});

	$(".upform").change(function(){
		if($(this).val() == ""){
			$("#attach").attr("disabled",true);
			$("#save").attr("disabled",false);
		}
		else{
			$("#attach").attr("disabled",false);
			$("#save").attr("disabled",true);
		}
	});

});

function loadQuestions(){
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_GSLLandscape_GetTemplate";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["Company"] =  companyUser["CompanyCode"];
	
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		sessionStorage["alandscape"] = JSON.stringify(data);

		if(data!="" && data.length>0){
												
			$(".gQuestions").html("");
			

			$(data).each(function(i,item){
			
				var htm = '<div class="row mrb-5"><div class="col-md-1 col-sm-1 col-xs-1">'+(i+1)+'</div><div class="col-md-4 col-sm-4 col-xs-4">'+decodeURIComponent(item["Question"])+'</div>'+
							'<div class="col-md-3 col-sm-3 col-xs-3"><select class="form-control done"><option value="Yes">Yes</option><option value="No">No</option></select></div>'+
							'<div class="col-md-4 col-sm-4 col-xs-3"><input type="text" class="form-control gremarks" value=""/></div>'+
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
						var proj = project["PrcName"];
						var proj_decode = decodeURIComponent(proj);
						opt += '<option value="'+project["PrcCode"]+'">'+proj_decode+'</option>';

					});
					$("#form-field-select-1").html(opt);

				}

				// initialize supervisor
				//$("#supervisor").val(companyUser["EmployeeName"]);

				// Preselect project
				$("#form-field-select-1").val(sessionStorage["listlsProject"] || "");

			}

			else{
				// $(".showlistdocs").html("<div class='mrt-20'> No Details Found</div>");
			}
			hideLoading();
			

	});
}

function validateRequired(callBack){

	var errorMsg = "";

	if($("#form-field-select-1").val() == ""){
		errorMsg = "Please select project";
	}
	/*else if($("#reference").val() == ""){
		errorMsg = "Please fill Contact Ref";
	}*/
	else if($("#timein").val() == ""){
		errorMsg = "Please fill Time In";
	}
	else if($("#timeout").val() == ""){
		errorMsg = "Please fill Time Out";
	}
	else if($("#timeout").val() < $("#timein").val()){
		errorMsg = "Time Out should be greater than Time In";
	}
	/*else if($("#idate").val() == ""){
		errorMsg = "Please fill Issue Date";
	}
	else if($("#rdate").val() == ""){
		errorMsg = "Please fill Receive Date";
	}*/
	else if($("#supervisor").val() == ""){
		errorMsg = "Please fill Supervisor name";
	}
	else if($("#client").val() == ""){
		errorMsg = "Please fill client name";
	}
	else if(checkSignature($("#supSign")) <= 0){
		errorMsg = "Please fill Supervisor Signature";
	}
	else if($("#supsigndate").val() == ""){
		errorMsg = "Please fill supervisor signed date";
	}
	else if(checkSignature($("#clSign")) <= 0){
		errorMsg = "Please fill Client Signature";
	}
	else if($("#clsigndate").val() == ""){
		errorMsg = "Please fill client signed date";
	}
	//else if($(".upform").val() != "" && (typeof sessionStorage["landscapeFile"] == "undefined" || sessionStorage["landscapeFile"] == "")){
	//	errorMsg = "Please click Attach to upload selected file";
	//}
	
	callBack(errorMsg);
}



