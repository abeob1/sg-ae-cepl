
loadProject();
//getMKTSegmentID();



$(document).ready(function(){

	$("#supSign,#clSign").jSignature({color:"#000000",height:'110px',width:'100%'});

	
	$("#save").click(function(){

		validateRequired(function(errorMsg){
			if(errorMsg != ""){
				alert(errorMsg);
			}
			else{

				if(confirm("Confirm Save?")){

					var url = CONFIG.OPERATIONS_BASEPATH + "/MSave_InspectionQA";
					var companyUser = sessionStorage["user"];
					if(companyUser){
						companyUser = JSON.parse(companyUser);
					}

					var marketSegement = JSON.parse(sessionStorage["vaInspection"]);
					var inspection = {};
					inspection["Company"] = companyUser["CompanyCode"];
					inspection["Project"] = sessionStorage["listinsProject"];
					
					//inspection["MKTSegment"] = $("#form-field-select-1").val();
					<!-- Start code for Encodng special character -->

					var e = document.getElementById("form-field-select-1");
					var strUser = e.options[e.selectedIndex].text;

					var MKTSegmt = strUser;/*$("#form-field-select-1").val();	*/		
					var mktseg_ = MKTSegmt;
					var mktseg_encoded = encodeURIComponent(mktseg_);
					<!-- End code for Encodng special character -->
					inspection["MKTSegment"]= mktseg_encoded;
					
					//inspection["Remarks"] = $("#remarks").val();
					<!-- Start code for Encodng special character -->	
					var RemSegmt = $("#remarks").val();			
					var Remseg_ = RemSegmt;
					var Remseg_encoded = encodeURIComponent(Remseg_);
					<!-- End code for Encodng special character -->
					inspection["Remarks"]= Remseg_encoded;
					
					inspection["InspectedBy"] = encodeURIComponent($("#supervisor").val());
					inspection["AcknowledgeBy"] = encodeURIComponent($("#client").val());

				/*	var signature60Format = $("#esign").jSignature("getData");

					if(signature60Format.split(",")[1] != emptySign){
						inspection["ESignature"] = signature60Format.split(",")[1];
					}
					else{
						inspection["ESignature"] = "";
					}*/

					var signature60Format = $("#supSign").jSignature("getData");

					if(checkSignature($("#supSign")) > 0){
						inspection["InspectedSign"] = signature60Format.split(",")[1];
					}
					else{
						inspection["InspectedSign"] = "";
					}


					signature60Format = $("#clSign").jSignature("getData");

					if(checkSignature($("#clSign")) > 0){
						inspection["AcknowledgeSign"] = signature60Format.split(",")[1];
					}
					else{
						inspection["AcknowledgeSign"] = "";
					}
					
					/*var chk = marketSegement;
					
					inspection["MarketingSegment"] = chk.replace("&","\u0026");
					for(var i=0; i < chk.length; i++) {
 					chk[i] = chk[i].replace(/&/g, "/u0026");
					}
					//= JSON.parse(chk.replace('&','\u0026'));
					*/
					
					inspection["MarketingSegment"] = marketSegement;

					$(inspection.MarketingSegment).each(function(i,item){

						item["Rating"] = $("input[type='radio'][name='question_"+i+"']:checked").val() || "";
						item["BaseEntry"] = $("#form-field-select-1 option:selected").data("doc");

					});

					// Check for attached file

					inspection["Attachments"] = [];
					
					var attachmentFiles = sessionStorage["inspectionFile"] || "";
					if(attachmentFiles){
						attachmentFiles = JSON.parse(attachmentFiles);

						$(attachmentFiles).each(function(j,file){
							var aFile = {};
							aFile["WebURL"] = file["WebURL"];
							aFile["SAPURL"] = file["SAPURL"];
							aFile["FileName"] = getFilename(file["WebURL"]);
							aFile["Remarks"] = encodeURIComponent(file["Remarks"]);
							inspection["Attachments"].push(aFile);
						});
					}

					sessionStorage["inspectionFile"] = "";
					/*inspection["WebURL"] = sessionStorage["inspectionFile"] || "";
					inspection["SAPURL"] = sessionStorage["inspectionFileSAP"] || "";
					inspection["FileName"] = getFilename(inspection["WebURL"]);*/

					console.log(JSON.stringify(inspection));
					

					showLoading();
					postToServer(url,"POST",JSON.stringify(inspection),function(data){
						// console.log(data);
						if(data[0] && data[0].Result=="Success"){
							alert(data[0].DisplayMessage);
							window.location.href = "list-inspection.php";
						}
						else{
							alert("Error ! Message: " + data[0].DisplayMessage);
						}

						hideLoading();
					});
					
					/* console.log(JSON.stringify(report));
			showLoading();
			postToServer(url,"POST",JSON.stringify(report),function(data){
					// console.log(data);
					if(data[0] && data[0].Result=="Success"){
						alert(data[0].DisplayMessage);
						window.location.href = "search-report.php";
					}
					else{
						alert("Error ! Message: " + data[0].DisplayMessage);
					}

					hideLoading();
			});  */

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
				sessionStorage["inspectionFile"] = JSON.stringify(data[0].Attachments);
				// sessionStorage["inspectionFileSAP"] = data[0].SAPURL;
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

	$("#clClear").click(function(){
		$("#clSign").jSignature("reset");
	});

	$("#supClear").click(function(){
		$("#supSign").jSignature("reset");
	});

	$("#cancel").click(function(){
		window.location.href = "list-inspection.php";
	});


});
var sInput;
function getMKTSegmentID(sInput){
	if(sInput != ""){
			var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_InspectionQA_CategoryItems";
			var mProjectData = {};
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}
			mProjectData["Company"] =  companyUser["CompanyCode"];
			mProjectData["DocEntry"] = sInput;
			
			showLoading();
			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
			 sessionStorage["vaInspection"] = JSON.stringify(data); 
			 
				/*<!-- Start code for Encodng special character -->				
					var sessionStorage_ = JSON.stringify(data);
					var sessionStorage_encoded = decodeURIComponent(sessionStorage_);
					//document.getElementById("demo").innerHTML = mktseg_encoded;
				<!-- End code for Encodng special character -->
				
				sessionStorage["vaInspection"] = sessionStorage_encoded;
				*/
				
				/*var dat = data;
				var dat_code = decodeURIComponent(dat);*/
				
				if(data!="" && data.length>0){
														
					$(".vQuestions").html("");
					var category = "";
					$(data).each(function(i,item){
						var htm;
						if(category != item["Category"]){
							category = item["Category"];

							htm = '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+decodeURIComponent(item["Category"])+'</h4></div></div>';

							htm+= '<div class="row"><div class="col-md-7 col-sm-7 col-xs-6"><h5>'+decodeURIComponent(item["Item"])+'</h5></div>'+
									'<div class="col-md-4 col-sm-5 col-xs-5"><ul class="sradio-grid">'+
									'<li><input type="radio" name="question_'+i+'" value="1" /><label></label></li>'+
									'<li><input type="radio" name="question_'+i+'" value="2" /><label></label></li>'+
									'<li><input type="radio" name="question_'+i+'" value="3" /><label></label></li>'+
									'<li><input type="radio" name="question_'+i+'" value="4" /><label></label></li>'+
									'<li><input type="radio" name="question_'+i+'" value="5" /><label></label></li>'+									
									'</ul></div><div class="col-md-1 col-sm-5 col-xs-1" onclick="ResetRadio('+i+');"><i class="fa fa-times"> </i></div></div>';

						}
						else{

							htm = '<div class="row"><div class="col-md-7 col-sm-7 col-xs-6"><h5>'+decodeURIComponent(item["Item"])+'</h5></div>'+
									'<div class="col-md-4 col-sm-5 col-xs-5"><ul class="sradio-grid">'+
									'<li><input type="radio" name="question_'+i+'" value="1" /><label></label></li>'+
									'<li><input type="radio" name="question_'+i+'" value="2" /><label></label></li>'+
									'<li><input type="radio" name="question_'+i+'" value="3" /><label></label></li>'+
									'<li><input type="radio" name="question_'+i+'" value="4" /><label></label></li>'+
									'<li><input type="radio" name="question_'+i+'" value="5" /><label></label></li>'+									
									'</ul></div><div class="col-md-1 col-sm-5 col-xs-1" onclick="ResetRadio('+i+');"><i class="fa fa-times"> </i></div></div>';

						}
						
						$(".vQuestions").append(htm);

						
							$("#save").attr("disabled",false);
							// $(".inspect-item,.boxes").show();
		
						
					});

				
					

				}

				else{
					$(".vQuestions").html("<div class='mrt-20'> No Details Found</div>");
				}

				

					hideLoading();


			});

		}
		else{
			$(".vQuestions").html("<div class='mrt-20'> No Details Found</div>");
			$("#save").attr("disabled",true);	
		}
						
}

function loadProject(){
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_InspectionQA_MarketSegment";
	

	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["Company"] =  companyUser["CompanyCode"];
	mProjectData["ProjectCode"] =  encodeURIComponent(sessionStorage["listinsProject"]);
	
	showLoading();																		
	
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		if(data!="" && data.length>0){
				var projList = data;
				var sessionId = projList[0].BaseEntry;
				sessionStorage.MktSegmentId = sessionId;
				var sInput = sessionId;
				var opt ='<option value="">Select</option>';
				if(projList && projList[0] && projList[0].BaseEntry){

					$(data).each(function(i,project){
					
					//decodeURI()
					//unescape()
					//decodeURIComponent()
					//encodeURI()
					//encodeURIComponent()project["BaseEntry"]
					var proj = project["MktSegment"];
					var proj_coded = decodeURIComponent(proj);
					
						opt += '<option value="'+project["BaseEntry"]+'" data-doc="'+project["BaseEntry"]+'">'+proj_coded+'</option>';

					});
					$("#form-field-select-1").html(opt);
					$("#form-field-select-1").val(sessionId);


				}
				//Code for text free Ins Q/A
				//$("#supervisor").val(companyUser["EmployeeName"]);
				getMKTSegmentID(sInput);

			}

			else{
				// $(".showlistdocs").html("<div class='mrt-20'> No Details Found</div>");
			}
			
			hideLoading();
			
	});
	
	

		
}

function ResetRadio(name)
{
//alert("question_"+name);
	$('input[name=question_'+name+']').attr('checked',false);
}
function validateRequired(callBack){

	var selected = $("input[type='radio']:checked").length;
	var total = $("input[type='radio']").length/5;
	var errorMsg  = "";
	
	/*if(selected != total){
		errorMsg = "Please select rating";

	}*/
	if($("#client").val() == ""){
		errorMsg = "Please enter Acknowledge By";
	}
	else if(checkSignature($("#supSign")) <= 0){
		errorMsg = "Please fill Inspector Signature";
	}
	else if(checkSignature($("#clSign")) <= 0){
		errorMsg = "Please fill Acknowledge Signature";
	}
	//else if($(".upform").val() != "" && (typeof sessionStorage["inspectionFile"] == "undefined" || sessionStorage["inspectionFile"] == "")){
	//	errorMsg = "Please click Attach to upload selected file";
	//}
	
	callBack(errorMsg);

}