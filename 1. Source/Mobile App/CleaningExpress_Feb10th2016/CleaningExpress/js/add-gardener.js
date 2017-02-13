loadProject();

loadQuestions();

$(document).ready(function(){

	// Signature
	// initialize signature
	$("#supSign,#clSign").jSignature({color:"#000000",height:'110px',width:'100%'});
	
		// initialize signature Date
	$("#supsigndate,#clsigndate,#idate,#rdate").val(getToday());

	$("#cancel").click(function(){
		window.location.href = "list-gardener.php";
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

					var url = CONFIG.OPERATIONS_BASEPATH + "/MSave_GSLGardener";
					var companyUser = sessionStorage["user"];
					if(companyUser){
						companyUser = JSON.parse(companyUser);
					}

					var gardener = {};
					gardener["Company"] = companyUser["CompanyCode"];
					gardener["Project"] = $("#form-field-select-1").val();
					gardener["Reference"] = $("#reference").val();
					gardener["TimeIn"] = $("#timein").val();
					gardener["TimeOut"] = $("#timeout").val();
					gardener["IssuedDate"] = convertDate($("#idate").val());
					gardener["ReceivedDate"] = convertDate($("#rdate").val());
					gardener["SupervisorName"] = $("#supervisor").val();
					gardener["ClientName"] = $("#client").val();
					gardener["SignedDate1"] = convertDate($("#supsigndate").val());
					gardener["SignedDate2"] = convertDate($("#clsigndate").val());


					var signature60Format = $("#supSign").jSignature("getData");

					if(checkSignature($("#supSign")) > 0){
						gardener["SupervisorSign"] = signature60Format.split(",")[1];
					}
					else{
						gardener["SupervisorSign"] = "";
					}


					signature60Format = $("#clSign").jSignature("getData");

					if(checkSignature($("#clSign")) > 0){
						gardener["ClientSign"] = signature60Format.split(",")[1];
					}
					else{
						gardener["ClientSign"] = "";
					}

					gardener["GardenerChk"] = [];

					$(".done").each(function(){
						var chkObj = {};
						chkObj["Done"] = $(this).val();
						chkObj["Remark"] = $(this).parent().next().find(".gremarks").val();
						chkObj["Description"] = $(this).parent().prev().text().trim();

						gardener["GardenerChk"].push(chkObj);

					});

					// Attachments
					gardener["Attachments"] = [];
					
					var attachmentFiles = sessionStorage["gardenerFile"] || "";
					if(attachmentFiles){
						attachmentFiles = JSON.parse(attachmentFiles);

						$(attachmentFiles).each(function(j,file){
							var aFile = {};
							aFile["WebURL"] = file["WebURL"];
							aFile["SAPURL"] = file["SAPURL"];
							aFile["FileName"] = getFilename(file["WebURL"]);

							gardener["Attachments"].push(aFile);
						});
					}
					/*gardener["WebURL"] = sessionStorage["gardenerFile"] || "";
					gardener["SAPURL"] = sessionStorage["gardenerFileSAP"] || "";
					gardener["FileName"] = getFilename(gardener["WebURL"]);*/
					
					console.log(JSON.stringify(gardener));

					showLoading();
					postToServer(url,"POST",encodeURIComponent(JSON.stringify(gardener)),function(data){
						// console.log(data);
						if(data[0] && data[0].Result=="Success"){
							alert(data[0].DisplayMessage);
							window.location.href = "list-gardener.php";
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
				sessionStorage["gardenerFile"] = JSON.stringify(data[0].Attachments);
				// sessionStorage["gardenerFileSAP"] = data[0].SAPURL;
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
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_GSLGardener_GetTemplate";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["Company"] =  companyUser["CompanyCode"];
	
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		sessionStorage["aGardener"] = JSON.stringify(data);

		if(data!="" && data.length>0){
												
			$(".gQuestions").html("");
			

			$(data).each(function(i,item){
			
				var htm = '<div class="row mrb-5"><div class="col-md-1 col-sm-1 col-xs-1">'+(i+1)+'</div><div class="col-md-4 col-sm-4 col-xs-4">'+item["Question"]+'</div>'+
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
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_GSLGardener_Project";
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
						opt += '<option value="'+project["PrcCode"]+'">'+project["PrcName"]+'</option>';

					});
					$("#form-field-select-1").html(opt);

				}

				// Populate supervisor
				$("#supervisor").val(companyUser["EmployeeName"]);
				$("#form-field-select-1").val(sessionStorage["listagProject"] || "");

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
		errorMsg = "Please fill ContactRef";
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
	else if($(".upform").val() != "" && (typeof sessionStorage["gardenerFile"] == "undefined" || sessionStorage["gardenerFile"] == "")){
		errorMsg = "Please click Attach to upload selected file";
	}

	callBack(errorMsg);
}
