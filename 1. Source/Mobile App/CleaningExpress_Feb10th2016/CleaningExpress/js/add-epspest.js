loadProject();
loadContentsTab();

$(document).ready(function(){

	// Signature
	// initialize signature
	$("#supSign,#clSign").jSignature({color:"#000000",height:'110px',width:'100%'});
	$("#supsigndate,#clsigndate").val(getToday());

	$("#cancel").click(function(){
		window.location.href = "list-epspest.php";
	});

	$("#clClear").click(function(){
		$("#clSign").jSignature("reset");
	});

	$("#supClear").click(function(){
		$("#supSign").jSignature("reset");
	});

	$(".pQuantity").keyup(function(){
		validateQuantity($(this));
	});

    $("#form-field-select-1").change(function() {



		if($(this).val() != ""){



			var mProjectData = {};

			

			var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_ServiceReport_GetShipToAddress"

			var companyUser = sessionStorage["user"];

			if(companyUser){

				companyUser = JSON.parse(companyUser);

			}

			

			mProjectData["Company"] =  companyUser["CompanyCode"];

			mProjectData["Project"] = $(this).val();		

			showLoading();

			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

				if(data!="" && data.length>0){

				var addList = data;

								

				var opt ='<option value="">Select</option>';

				if(addList && addList[0] && addList[0].AddressId){



					$(data).each(function(i,add){

						

						//var addid = add["AddressId"];

						//var proj_name = decodeURIComponent(addid);	

						

						opt += '<option value="'+add["AddressId"]+'">'+add["AddressId"]+'</option>';



					});

					$("#addressid").html(opt);
					$("#addressid").change();



				}



				// Preselect project if exists

				//$("#form-field-select-1").val(sessionStorage["listepsProject"] || "");

				

				//$("#supervisor").val(companyUser["EmployeeName"]);

			}



			else{

				 //$(".showlistdocs").html("<div class='mrt-20'> No Details Found</div>");

				 alert("No Address Id Found");

			}

			hideLoading();

					

					

			});



		}



	}); //on 

    //Address drop down change
 $("#addressid").change(function() {



		if($(this).val() != ""){



			var mProjectData = {};

			

			var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_ServiceReport_GetScopeofWork"

			var companyUser = sessionStorage["user"];

			if(companyUser){

				companyUser = JSON.parse(companyUser);

			}

			

			mProjectData["Company"] =  companyUser["CompanyCode"];

			mProjectData["Project"] = $("#form-field-select-1").val();	

			mProjectData["AddId"] = encodeURIComponent($(this).val());

			showLoading();

			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

				if(data!="" && data.length>0){



					data = data[0];

					

					var add = data["Address"];

					var proj_add = decodeURIComponent(add);

					$("#address").val(proj_add);

					

					var block = data["Block"];

					var proj_block = decodeURIComponent(block);

					$("#blockno").val(proj_block);

					

					var p_unit = data["Unit"];

					var proj_unit = decodeURIComponent(p_unit);

					$("#unitno").val(proj_unit);

					

					$("#postalcode").val(data["Postal"]);

					$("#scopeofwork").val(data["ScopeofWork"]);



					// Clear all values inside tab

					$(".tabbable").find("input[type='text']").val("");

					$(".tabbable").find("input[type='checkbox']").attr("checked",false);					



				}

				else{

					

				}

					

					

					hideLoading();

					

					

			});



		}

	});

	// Need to work
	$("#save").click(function(){

		validateRequired(function(errorMsg){
			if(errorMsg != ""){
				alert(errorMsg);
			}
			else{

				if(confirm("Confirm Save?")){

					var url = CONFIG.OPERATIONS_BASEPATH + "/MSave_ServiceReport";
					var companyUser = sessionStorage["user"];
					if(companyUser){
						companyUser = JSON.parse(companyUser);
					}

					var eps = {};
					eps["Company"] = companyUser["CompanyCode"];
					eps["Project"] = $("#form-field-select-1").val();
					if ($("#addressid").val() == null){eps["AddId"] = "";}else {eps["AddId"] = $("#addressid").val();}
					
					
					eps["Address"] = encodeURIComponent($("#address").val());
					eps["Block"] = encodeURIComponent($("#blockno").val());
					eps["Unit"] = encodeURIComponent($("#unitno").val());
					eps["Postal"] = $("#postalcode").val();
					eps["TimeIn"] = $("#timein").val();
					eps["TimeOut"] = $("#timeout").val();
					eps["Report"] = encodeURIComponent($("#report").val());
					eps["ReportOth"] = encodeURIComponent($("#reportoth").val());
					eps["ACommonArea"] = getCheckboxStatus($(".ACommonArea"));
					eps["Abuilding"] = getCheckboxStatus($(".Abuilding"));
					eps["ACorridor"] = getCheckboxStatus($(".ACorridor"));
					eps["AGarden"] = getCheckboxStatus($(".AGarden"));
					eps["ADrainage"] = getCheckboxStatus($(".ADrainage"));
					eps["APlayground"] = getCheckboxStatus($(".APlayground"));
					eps["ABinCentre"] = getCheckboxStatus($(".ABinCentre"));
					eps["ACarPark"] = getCheckboxStatus($(".ACarPark"));
					eps["ALightning"] = getCheckboxStatus($(".ALightning"));
					eps["AElectrical"] = getCheckboxStatus($(".AElectrical"));
					eps["AStoreroom"] = getCheckboxStatus($(".AStoreroom"));
					eps["ARooftop"] = getCheckboxStatus($(".ARooftop"));
					eps["AManhole"] = getCheckboxStatus($(".AManhole"));
					eps["AToilet"] = getCheckboxStatus($(".AToilet"));
					eps["ARiser"] = getCheckboxStatus($(".ARiser"));
					eps["AOffice"] = getCheckboxStatus($(".AOffice"));

					eps["ACanteen"] = getCheckboxStatus($(".ACanteen"));
					eps["AKitchen"] = getCheckboxStatus($(".AKitchen"));
					eps["ACabinet"] = getCheckboxStatus($(".ACabinet"));
					eps["AGullyTrap"] = getCheckboxStatus($(".AGullyTrap"));
					eps["AOthers"] = getCheckboxStatus($(".AOthers"));
					eps["AOthersDesc"] = encodeURIComponent($("#specify").val());

					eps["Scope"] = encodeURIComponent($("#scopeofwork").val());
					eps["Feedback"] = encodeURIComponent($("#feedback").val());
					eps["IHousekeeping"] = getCheckboxStatus($(".IHousekeeping"));
					eps["ISanitation"] = getCheckboxStatus($(".ISanitation"));
					eps["IStructural"] = getCheckboxStatus($(".IStructural"));
					eps["IOthers"] = getCheckboxStatus($(".IOthers"));
					eps["IOthersDesc"] = encodeURIComponent($(".IOthersDesc").val());
					eps["IComments"] = encodeURIComponent($(".IComments").val());

					eps["Supervisor"] = encodeURIComponent($("#supervisor").val());
					eps["Client"] = encodeURIComponent($("#client").val());
					
					/*gardener["SignedDate1"] = convertDate($("#supsigndate").val());
					gardener["SignedDate2"] = convertDate($("#clsigndate").val());*/
					
					eps["SignedDate1"] = convertDate($("#supsigndate").val());
					eps["SignedDate2"] = convertDate($("#clsigndate").val())

					var signature60Format = $("#supSign").jSignature("getData");

					if(signature60Format.split(",")[1] != emptySign){
						eps["SupervisorSign"] = signature60Format.split(",")[1];
					}
					else{
						eps["SupervisorSign"] = "";
					}


					signature60Format = $("#clSign").jSignature("getData");

					if(signature60Format.split(",")[1] != emptySign){
						eps["ClientSign"] = signature60Format.split(",")[1];
					}
					else{
						eps["ClientSign"] = "";
					}

					eps["Content"] = [];

					$(".descoth").each(function(){
						
						var contObj = {};
						contObj["Description"] = encodeURIComponent($(this).parents(":eq(1)").find(".quest").text().trim());
						contObj["DescriptionOth"] = encodeURIComponent($(this).val());
						contObj["Include"] = getCheckboxStatus($(this).parents(":eq(1)").find(".Include"));
						contObj["Active"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".Active"));
						contObj["Nonactive"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".Nonactive"));
						contObj["TFogging"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".TFogging"));
						contObj["TMisting"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".TMisting"));
						contObj["TResidual"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".TResidual"));
						contObj["TBaits"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".TBaits"));
						contObj["TDusting"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".TDusting"));
						contObj["TTraps"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".TTraps"));
						contObj["TOthers"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".TOthers"));
						contObj["IGS"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".IGS"));
						contObj["AGS"] =  getCheckboxStatus($(this).parents(":eq(1)").find(".AGS"));
						contObj["Location"] = encodeURIComponent( $(this).parents(":eq(1)").find(".Location").val());

						eps["Content"].push(contObj);
					});
					
					eps["Pesticide"] = [];

					$(".pDesc").each(function(){
						var pest = {};
						if($(this).val() != ""){

							pest["Pesticide"] = encodeURIComponent($(this).val());
							pest["Quantity"] = $(this).parents(":eq(1)").find(".pQuantity").val();

							eps["Pesticide"].push(pest);

						}
						
					});

					eps["Report"] = [];

					var reportObj = {};
					reportObj["rWeekly"] = getCheckboxStatus($(".rWeekly"));
					reportObj["rFortnightly"] = getCheckboxStatus($(".rFortnightly"));
					reportObj["rMonthly"] = getCheckboxStatus($(".rMonthly"));
					reportObj["rBimonthly"] = getCheckboxStatus($(".rBimonthly"));
					reportObj["rQuarterly"] = getCheckboxStatus($(".rQuarterly"));
					reportObj["rOnetime"] = getCheckboxStatus($(".rOnetime"));
					reportObj["rFollowup"] = getCheckboxStatus($(".rFollowup"));
					
					eps["Report"] = reportObj;

					// Attachment

					eps["Attachments"] = [];
					
					var attachmentFiles = sessionStorage["epsFile"] || "";
					if(attachmentFiles){
						attachmentFiles = JSON.parse(attachmentFiles);

						$(attachmentFiles).each(function(j,file){
							var aFile = {};
							aFile["WebURL"] = file["WebURL"];
							aFile["SAPURL"] = file["SAPURL"];
							aFile["FileName"] = getFilename(file["WebURL"]);
							aFile["Remarks"] = encodeURIComponent(file["Remarks"]);
							eps["Attachments"].push(aFile);
						});
					}
					sessionStorage["epsFile"] = "";


					/*eps["WebURL"] = sessionStorage["epsFile"] || "";
					eps["SAPURL"] = sessionStorage["epsFileSAP"] || "";
					eps["FileName"] = getFilename(eps["WebURL"]);*/

					console.log(JSON.stringify(eps));

					showLoading();
					postToServer(url,"POST",JSON.stringify(eps),function(data){
						// console.log(data);
						if(data[0] && data[0].Result=="Success"){
							alert(data[0].DisplayMessage);
							window.location.href = "list-epspest.php";
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
    $("#attach").click(function() {
          

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
				sessionStorage["epsFile"] = JSON.stringify(data[0].Attachments);
				// sessionStorage["epsFileSAP"] = data[0].SAPURL;
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

function loadContentsTab(){

	var mProjectData = {};
			
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_ServiceReport_GetTemplate"
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	
	mProjectData["Company"] =  companyUser["CompanyCode"];
			
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
		if(data!="" && data.length>0){

            $("#contentTblBody").html("");
            $(data).each(function(i, item) {

             	
			var itemHtm = '<tr class="crow_'+i+'"><td>'+(i+1)+'</td><td class="quest">'+decodeURIComponent(item["Question"])+'</td><td class="text-center"><input type="checkbox" name="include" class="Include" value="Y"/></td>'+
							'<td class="text-center"><input type="checkbox" name="active" class="Active" value="Y"/></td><td class="text-center"><input type="checkbox" name="nonactive" class="Nonactive" value="Y"/></td>'+
							'<td class="text-center"><input type="checkbox" name="f" class="TFogging"/></td><td class="text-center"><input type="checkbox" name="m" class="TMisting"/></td>'+
							'<td class="text-center"><input type="checkbox" name="r" class="TResidual"/></td><td class="text-center"><input type="checkbox" name="b" class="TBaits"/></td>'+
							'<td class="text-center"><input type="checkbox" name="d" class="TDusting"/></td><td class="text-center"><input type="checkbox" name="t" class="TTraps"/></td>'+
							'<td class="text-center"><input type="checkbox" name="o" class="TOthers"/></td><td class="text-center"><input type="checkbox" name="igs" class="IGS" /></td>'+
							'<td class="text-center"><input type="checkbox" name="ags" class="AGS"/></td><td><input type="text" class="Location"/></td><td><input type="text" class="descoth" value=""/></td></tr>';			

				$("#contentTblBody").append(itemHtm);
				
				
			});

		} else {
            $("#contentTblBody").html("<div class='mrt-20'> No Details Found</div>");

        }
        hideLoading();

    });
}


function loadProject(){
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_ServiceReport_Project";
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
						var proj_name = decodeURIComponent(proj);	
						
						opt += '<option value="'+project["PrcCode"]+'">'+proj_name+'</option>';

					});
					$("#form-field-select-1").html(opt);

				}

				// Preselect project if exists
				$("#form-field-select-1").val(sessionStorage["listepsProject"] || "");

				$("#form-field-select-1").change();

				//$("#supervisor").val(companyUser["EmployeeName"]);

        } else { // $(".showlistdocs").html("<div class='mrt-20'> No Details Found</div>");	

        }
        hideLoading();
    });
}

function getCheckboxStatus($chk){
	if($chk.is(":checked")){
		return "Yes";
	}
	else{
		return "No";
	}
}

function validateRequired(callBack) {
	var errorMsg = "";

	if($("#address").val() == ""){
		errorMsg = "Please enter address";
	}
	/*else if($("#blockno").val() == ""){
		errorMsg = "Please enter block no";
	}*/
	else if($("#timein").val() == ""){
		errorMsg = "Please fill Time in";
	}
	else if($("#timeout").val() == ""){
		errorMsg = "Please fill Time out";
	}
	else if($("#timeout").val() < $("#timein").val()){
		errorMsg = "Time Out should be greater than Time In";
	}
	else if($(".rRepOthers").is(":checked") && $("#reportoth").val() == ""){
		errorMsg = "Please specify if you select Others";
	}
	else if($(".AOthers").is(":checked") && $("#specify").val() == ""){
		errorMsg = "Please specify if you select Others";
	}
	else if($("#supervisor").val() == ""){
		errorMsg = "Please fill Supervisor name";
	}
	else if($("#client").val() == ""){
		errorMsg = "Please fill client name";
	}
	else if(checkSignature($("#supSign")) <= 0){
		errorMsg = "Please fill Supervisor Signature";
	}
	else if(checkSignature($("#clSign")) <= 0){
		errorMsg = "Please fill Client Signature";
	}
	//else if($(".upform").val() != "" && (typeof sessionStorage["epsFile"] == "undefined" || sessionStorage["epsFile"] == "")){
	//	errorMsg = "Please click Attach to upload selected file";
	//}

	callBack(errorMsg);
}

    function validateQuantity($obj) {
        if (isNaN($obj.val().trim())) {
            alert("Please enter valid quantity");

            $obj.val("");
        } else if ($obj.val().trim() != "" && parseFloat($obj.val().trim()) < 0) {
            alert("Please enter positive quantity ");	}	}