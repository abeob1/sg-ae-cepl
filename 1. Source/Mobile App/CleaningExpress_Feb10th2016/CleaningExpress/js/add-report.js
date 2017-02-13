
loadReportQuestions();

$(document).ready(function(){

	$("#save").click(function(){
        
		if(confirm("Confirm Save?")){
			var url = CONFIG.BASEPATH + "/MSave_ShowAround";
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}

			var report = JSON.parse(sessionStorage["addReport"]);
			$(".quantity").each(function(i,item){
				// var question = $(this).val();
				/*var desc = $(this).parents(":eq(1)").next().next().find('input').val();
				var quantity = $(this).parents(":eq(1)").next().find('input').val();*/

				var desc = $(this).parents(":eq(1)").next().next().find("textarea").val();
				var quantity = $(this).parents(":eq(1)").next().find("input").val();

				
				
					<!-- Start code for Encodng special character -->
					var desc_ = desc;
					var desc_encoded = encodeURIComponent(desc_);
					//document.getElementById("demo").innerHTML = res;
					<!-- End code for Encodng special character -->
					
				// report[i]["Question"] = question;
				report[i]["Description"] = desc_encoded;
				report[i]["Quantity"] = quantity;
				report[i]["OwnerCode"] = companyUser["UserName"];
				report[i]["OwnerName"] = companyUser["EmployeeName"];
				report[i]["EmpId"] = companyUser["EmpId"];
				report[i]["Remarks"] =encodeURIComponent($("#remarks").val());
				// Attachment

				report[i]["Attachments"] = [];
					
				var attachmentFiles = sessionStorage["reportFile"] || "";
				if(attachmentFiles){
					//attachmentFiles = JSON.parse(attachmentFiles);
					attachmentFiles = JSON.parse(attachmentFiles);

					$(attachmentFiles).each(function(j,file){
						var aFile = {};
						aFile["WebURL"] = file["WebURL"];
						//console.log(aFile["WebURL"]);
						aFile["SAPURL"] = file["SAPURL"];
						aFile["FileName"] = getFilename(file["WebURL"]);
						var ed = aFile["FileName"];		
						console.log(ed);
						aFile["FileName"] = ed;
						//aFile["Remarks"] = file["Remarks"];
						aFile["Remarks"] = encodeURIComponent(file["Remarks"]);

						report[i]["Attachments"].push(aFile);
					});
					//report[i]["Attachments"] = report[i]["Attachments"]);
				}
				sessionStorage["reportFile"] = "";
				/*report["WebURL"] = sessionStorage["reportFile"] || "";
				report["SAPURL"] = sessionStorage["reportFileSAP"] || "";
				report["FileName"] = getFilename(report["WebURL"]);*/
			});


			console.log(JSON.stringify(report));
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
			});

		}
	});

	$(".reportItems").on('keyup','.rNos',function(){
		validateNos($(this));
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
		uploadFileToServer(url,"POST",JSON.stringify($("#upform")),function(data){
			// console.log(data);
			if(data[0] && data[0].Result=="Success"){
				alert(data[0].DisplayMessage);
				sessionStorage["reportFile"] = JSON.stringify(data[0].Attachments);
				// sessionStorage["reportFileSAP"] = data[0].SAPURL;
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

function loadReportQuestions(){
	var url = CONFIG.BASEPATH + "/MGet_ShowAround_GetQuestions";
	var docEntry = sessionStorage.docEntry;
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["sCompany"] =  companyUser["CompanyCode"];
	
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
		sessionStorage.addReport = JSON.stringify(data);
		var report = data;
		if(report!="" && report.length>0){

			$(".reportItems").html("");
			$(report).each(function(i,item){

				/*var itemHtm = '<div class="list"><div class="row"><div class="col-md-12 col-sm-12 col-xs-12">'+
						'<h4 class="sname">'+item["Category"]+'</h4><form role="form" class="form-horizontal" method="post" action="">'+
						'<div class="form-group"><label class="col-xs-4 col-sm-2 col-md-2 control-label rName">'+item["Question"]+'</label>'+
						'<div class="col-xs-7 col-sm-9 col-md-9"><input type="hidden" class="form-control quantity" value=""/>'+
						'</div></div><div class="form-group"><label class="col-xs-4 col-sm-2 col-md-2 control-label">Nos</label><div class="col-xs-7 col-sm-9 col-md-9">'+
						'<input type="text" value="'+parseFloat(item["Quantity"] || "0.00").toFixed(2)+'" class="form-control textform rNos"></div></div>'+
						'<div class="form-group"><label class="col-xs-4 col-sm-2 col-md-2 control-label">Description</label>'+
						'<div class="col-xs-7 col-sm-9 col-md-9"><input type="text" class="form-control description" maxlength="254"/>'+
						'</div></div></form></div></div></div>';*/
				/*var itemHtm = '<div class="view"><table class="table-responsive"><tr>' +
						'<td class="repname">'+item["Category"]+'</td>' + '<td></td></tr><tr>'+
						'<td class="viewrep">'+item["Question"]+'</td>' + '<td class="viewrep"><input type="hidden" class="form-control quantity" value=""/></td>' +
						'</tr><tr><td class="viewrep">Nos</td><td class="viewrep1 cRep"><input type="tel" value="'+parseFloat(item["Quantity"] || "0.00").toFixed(2)+'" class="form-control textform rNos"></td>'+
						'</tr><tr><td class="viewrep">Description</td><td class="viewrep cRep"><input type="text" class="form-control description" maxlength="254"/></td>' +
						'</tr></table></div>';*/
				var itemHtm = '<div class="view"><div class="repname">'+decodeURIComponent(item["Category"])+'</div><table class="table-responsive"><tr>' +
						'<td></td>' + '<td></td></tr><tr>'+
						'<td class="viewrep">'+decodeURIComponent(item["Question"])+'</td>' + '<td class="viewrep"><input type="hidden" class="form-control quantity" value=""/></td>' +
						'</tr><tr><td class="viewrep">Nos</td><td class="viewrep1 cRep"><input type="tel" value=" " class="form-control textform rNos"></td>'+
						'</tr><tr><td class="viewrep">Description</td><td class="viewrep cRep"><textarea type="text" style="width:96%; border-color:Grey;text-indent: -4px;"  class="form-control description" maxlength="254"/></td>' +
						'</tr></table></div>';


				$(".reportItems").append(itemHtm);

			});

		}
		else{
			$(".reportItems").html("<div class='mrt-20'>There are no Questions for this selected Company. Kindly add the Questions and Try again</div>");
			$("#save").hide();
		}

		

			
			hideLoading();
			
	});
}


function validateNos($obj){

	var fVal = $obj.val().trim().split(".");


	if(isNaN($obj.val().trim())){
		alert("Please enter valid quantity");
		$obj.val("0.00");
	}
	else if($obj.val().trim() != "" && parseFloat($obj.val().trim())<0){
		alert("Please enter positive quantity");
	}
	// check for 16,4
	else if(fVal[1]){
		if(fVal[0].length>16 || fVal[1].length>4){
			alert("Please enter valid quanity");
			$obj.val("0.00");
		}
	}
	else if(fVal[0]){
		if(fVal[0].length>16){
			alert("Please enter valid quanity");
			$obj.val("0.00");

		}
	}
	
}