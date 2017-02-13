loadEvents();

$(document).ready(function(){

	/*$("#clear").click(function(){
		$("#esign").jSignature("reset");
	});*/
	
	//Initializing Signature Date
	$("#supsigndate,#clsigndate,#idate,#rdate").val(getToday());

	$("#clClear").click(function(){
		$("#clSign").jSignature("reset");
	});

	$("#supClear").click(function(){
		$("#supSign").jSignature("reset");
	});

	$(".taskList").on('change',".taskStatus",function(){
		if($(this).val() != "Completed"){
			$(this).parents(":eq(1)").next().find('.taskReschedule').parents(":eq(1)").show();
			$(this).parents(":eq(1)").next().next().find('.taskReason').parents(":eq(1)").show();
			$(this).parents(":eq(1)").next().next().next().find('.taskAlert').parents(":eq(1)").show();

			$(this).parents(":eq(1)").nextAll(":eq(3)").find(".completeStartDate").parents(":eq(1)").hide();
			$(this).parents(":eq(1)").nextAll(":eq(4)").find(".completeEndDate").parents(":eq(1)").hide();
			$(this).parents(":eq(1)").nextAll(":eq(5)").find(".completeBy").parents(":eq(1)").hide();
			$(this).parents(":eq(1)").nextAll(":eq(6)").find(".inspectedBy").parents(":eq(1)").hide();
			$(this).parents(":eq(1)").nextAll(":eq(7)").find(".verifiedBy").parents(":eq(1)").hide();
			$(this).parents(":eq(1)").nextAll(":eq(8)").find(".remarks").parents(":eq(1)").hide();
		}
		else{

			$(this).parents(":eq(1)").next().find('.taskReschedule').parents(":eq(1)").hide();
			$(this).parents(":eq(1)").next().next().find('.taskReason').parents(":eq(1)").hide();
			$(this).parents(":eq(1)").next().next().next().find('.taskAlert').parents(":eq(1)").hide();

			$(this).parents(":eq(1)").nextAll(":eq(3)").find(".completeStartDate").parents(":eq(1)").show();
			$(this).parents(":eq(1)").nextAll(":eq(4)").find(".completeEndDate").parents(":eq(1)").show();
			$(this).parents(":eq(1)").nextAll(":eq(5)").find(".completeBy").parents(":eq(1)").show();
			$(this).parents(":eq(1)").nextAll(":eq(6)").find(".inspectedBy").parents(":eq(1)").show();
			$(this).parents(":eq(1)").nextAll(":eq(7)").find(".verifiedBy").parents(":eq(1)").show();
			$(this).parents(":eq(1)").nextAll(":eq(8)").find(".remarks").parents(":eq(1)").show();

		}
	});

	$("#save").click(function(){
		var aFile = {};
		validateStatus(function(err){
			if(err != ""){
				alert(err);
			}
			else{

				if(confirm("Confirm Save?")){

			var url = CONFIG.OPERATIONS_BASEPATH + "/Msave_JobSchedule";
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}



			var jobTask = JSON.parse(sessionStorage["taskInfoObj"]);
			$(".taskStatus").each(function(i,task){

				var startTime = $(this).parents(":eq(1)").prev().find('.fromtime').val(); 
				var endTime = $(this).parents(":eq(1)").prev().find('.totime').val(); 
				var status = $(this).val();
				var reSchedule = $(this).parents(":eq(1)").next().find('.taskReschedule').val();
				var reason = $(this).parents(":eq(1)").next().next().find('.taskReason').val();
				var alertHQ = $(this).parents(":eq(1)").next().next().next().find('.taskAlert').val();
				var completeStartDate = $(this).parents(":eq(1)").nextAll(":eq(3)").find(".completeStartDate").val();
				var completeEndDate = $(this).parents(":eq(1)").nextAll(":eq(4)").find(".completeEndDate").val();
				var taskCompleteBy = $(this).parents(":eq(1)").nextAll(":eq(5)").find(".completeBy").val();
				var inspectedBy = $(this).parents(":eq(1)").nextAll(":eq(6)").find(".inspectedBy").val();
				var verifiedBy = $(this).parents(":eq(1)").nextAll(":eq(7)").find(".verifiedBy").val();
				var remarks = $(this).parents(":eq(1)").nextAll(":eq(8)").find(".remarks").val();

				jobTask[i]["ActualStartTime"] = startTime;
				jobTask[i]["ActualEndTime"] = endTime;
				jobTask[i]["TaskStatus"] = status;
				jobTask[i]["Reason"] = encodeURIComponent(reason);
				jobTask[i]["NewScheduleDate"] = convertDate(reSchedule);
				jobTask[i]["AlertHQ"] = alertHQ;
				jobTask[i]["PreparedBy"] = companyUser["EmployeeName"];
				jobTask[i]["PreparedById"] = companyUser["EmpId"];
				jobTask[i]["PreparedDate"] = convertDate(getToday());
				jobTask[i]["CompletedStartDate"] = convertDate(completeStartDate);
				jobTask[i]["CompletedEndDate"] = convertDate(completeEndDate);
				jobTask[i]["TaskCompleteBy"] = encodeURIComponent(taskCompleteBy);
				jobTask[i]["InspectedBy"] = encodeURIComponent(inspectedBy);
				jobTask[i]["VerifiedBy"] = encodeURIComponent(verifiedBy);
				jobTask[i]["remarks"] = encodeURIComponent(remarks);

				jobTask[i]["CompletedBy"] = $("#form-field-4").val();

				if(reSchedule){
					jobTask[i]["ApprovedBy"] = companyUser["EmployeeName"];
					jobTask[i]["ApprovedDate"] = convertDate(getToday());
					jobTask[i]["PreparedBy"] = companyUser["EmployeeName"];
					jobTask[i]["PreparedDate"] = convertDate(getToday());
				}

							

				/*var signature60Format = $("#esign").jSignature("getData");

				if(checkSignature($("#esign")) > 0){
					jobTask[i]["ESignature"] = signature60Format.split(",")[1];
				}
				else{
					jobTask[i]["ESignature"] = "";
				}*/
				var signature60Format = $("#supSign").jSignature("getData");

				if(checkSignature($("#supSign")) > 0){
					jobTask[i]["SupervisorSign"] = signature60Format.split(",")[1];
				}
				else{
					jobTask[i]["SupervisorSign"] = "";
				}


				signature60Format = $("#clSign").jSignature("getData");

				if(checkSignature($("#clSign")) > 0){
					jobTask[i]["ClientSign"] = signature60Format.split(",")[1];
				}
				else{
					jobTask[i]["ClientSign"] = "";
				}

				// Signature
				jobTask[i]["SignedDate1"] = convertDate($("#supsigndate").val());
				jobTask[i]["SignedDate2"] = convertDate($("#clsigndate").val());	


				// Attachment
				jobTask[i]["Attachments"] = [];
				
				var attachmentFiles = sessionStorage["taskFile"] || "";
				if(attachmentFiles){
					attachmentFiles = JSON.parse(attachmentFiles);

					$(attachmentFiles).each(function(j,file){
						var aFile = {};
						aFile["WebURL"] = file["WebURL"];
						aFile["SAPURL"] = file["SAPURL"];
						aFile["FileName"] = getFilename(file["WebURL"]);
						aFile["Remarks"] = encodeURIComponent(file["Remarks"]);
						jobTask[i]["Attachments"].push(aFile);
					});
					sessionStorage["taskFile"] = null;
				}
				sessionStorage["taskFile"] = "";
				/*jobTask[i]["WebURL"] = sessionStorage["taskFile"] || "";
				jobTask[i]["SAPURL"] = sessionStorage["taskFileSAP"] || "";
				jobTask[i]["FileName"] = getFilename(jobTask[i]["WebURL"]);*/

			});
			console.log(JSON.stringify(jobTask));
			

			showLoading();
			postToServer(url,"POST",JSON.stringify(jobTask),function(data){
					// console.log(data);
					if(data[0] && data[0].Result=="Success"){
						alert(data[0].DisplayMessage);
						window.location.href = "list-job.php";
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

	$("#cancel").click(function(){
		window.location.href = "view-task.php";
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
				sessionStorage["taskFile"] = JSON.stringify(data[0].Attachments);
				// sessionStorage["taskFileSAP"] = data[0].SAPURL;
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

function loadEvents(){

	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_JobSchedule_ScheduledDayInfo";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["Company"] =  companyUser["CompanyCode"];
	mProjectData["DocEntry"] = sessionStorage["jbdocEntry"];
	mProjectData["ScheduledDate"] = sessionStorage["cEvent"];
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		if(data!="" && data.length>0){
			sessionStorage.taskInfoObj = JSON.stringify(data);
			var eventt = data[0];
			$("#form-field-1").val(showDate(eventt["ScheduledDate"].split(" ")[0]));
			$("#form-field-2").val(getToday());
			$("#form-field-3").val(eventt["DayStatus"]);
			
			var CompletedBy = eventt["CompletedBy"] || companyUser["EmployeeName"];
			var CompletedBy_decode = decodeURIComponent(CompletedBy);
			$("#form-field-4").val(CompletedBy_decode);

			$(".taskList").html("");
			$(data).each(function(i,item){
				
				var htm;
				// Removed item["Reason"] as it should not be displayed in Edit
				// Similarly item["ActualEndTime"] item["ActualStartTime"] is removed
				if(item["DayStatus"] == "Completed"){

					htm = '<div class="list"><div class="row"><div class="col-md-12 col-sm-12 col-xs-12">' +
							'<h4 class="sname">'+decodeURIComponent(item["CleaningType"])+'</h4><h5 class="subtitle">'+decodeURIComponent(item["Location"])+'</h5></div></div>'+
							'<div class="row time"><div class="col-md-3 col-md-offset-2 col-sm-4 col-sm-offset-2 col-xs-4 col-xs-offset-3">'+
							'<h4 class="sname">'+item["StartTime"]+'</h4><div class="input-group"><input type="time" class="form-control fromtime" name="fromtime" value="'+item["ActualStartTime"]+'" disabled/>'+
							'</div></div><div class="col-md-3 col-md-offset-2 col-sm-4 col-sm-offset-2 col-xs-4">'+
							'<h4 class="sname">'+item["EndTime"]+'</h4><div class="input-group">'+
							'<input type="time" class="form-control totime" name="totime" value="'+item["ActualEndTime"]+'" disabled/></div></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Status</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<select  class="form-control taskStatus">'+
							'<option value="Completed">Completed</option><option value="Not Completed">Not Completed</option>'+
							'</select></div></div>'+
							'<div class="form-group cHide"><label class="col-xs-3 col-sm-2 col-md-2">Reschedule Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control taskReschedule" disabled/></div></div>'+
							'<div class="form-group cHide"><label class="col-xs-3 col-sm-2 col-md-2">Reason</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control taskReason" value="" disabled/></div></div>'+
							'<div class="form-group cHide">'+
							'<label class="col-xs-3 col-sm-2 col-md-2">Alert HQ</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<select class="form-control taskAlert" disabled><option value="No">No</option><option value="Yes">Yes</option></select></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed Start Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control completeStartDate" disabled/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed End Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control completeEndDate" disabled/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control completeBy" disabled/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Inspected By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control inspectedBy" disabled/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Verified By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control verifiedBy" disabled/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Remarks</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control remarks" disabled/></div></div>'+
							'</div>';


				}
				else if(item["DayStatus"] == "Not Completed"){

					htm = '<div class="list"><div class="row"><div class="col-md-12 col-sm-12 col-xs-12">' +
							'<h4 class="sname">'+decodeURIComponent(item["CleaningType"])+'</h4><h5 class="subtitle">'+decodeURIComponent(item["Location"])+'</h5></div></div>'+
							'<div class="row time"><div class="col-md-3 col-md-offset-2 col-sm-4 col-sm-offset-2 col-xs-4 col-xs-offset-3">'+
							'<h4 class="sname">'+item["StartTime"]+'</h4><div class="input-group"><input type="time" class="form-control fromtime" name="fromtime" value="'+item["ActualStartTime"]+'" disabled/>'+
							'</div></div><div class="col-md-3 col-md-offset-2 col-sm-4 col-sm-offset-2 col-xs-4">'+
							'<h4 class="sname">'+item["EndTime"]+'</h4><div class="input-group">'+
							'<input type="time" class="form-control totime" name="totime" value="'+item["ActualEndTime"]+'" disabled/></div></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Status</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<select  class="form-control taskStatus">'+
							'<option value="Completed">Completed</option><option value="Not Completed">Not Completed</option>'+
							'</select></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Reschedule Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control taskReschedule"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Reason</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control taskReason" value=""/></div></div>'+
							'<div class="form-group">'+
							'<label class="col-xs-3 col-sm-2 col-md-2">Alert HQ</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<select class="form-control taskAlert"><option value="No">No</option><option value="Yes">Yes</option></select></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed Start Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control completeStartDate"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed End Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control completeEndDate"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control completeBy"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Inspected By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control inspectedBy"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Verified By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control verifiedBy"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Remarks</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control remarks"/></div></div>'+
							'</div>';


				}
				else{

					htm = '<div class="list"><div class="row"><div class="col-md-12 col-sm-12 col-xs-12">' +
							'<h4 class="sname">'+decodeURIComponent(item["CleaningType"])+'</h4><h5 class="subtitle">'+decodeURIComponent(item["Location"])+'</h5></div></div>'+
							'<div class="row time"><div class="col-md-3 col-md-offset-2 col-sm-4 col-sm-offset-2 col-xs-4 col-xs-offset-3">'+
							'<h4 class="sname">'+item["StartTime"]+'</h4><div class="input-group"><input type="time" class="form-control fromtime" name="fromtime" value=""/>'+
							'</div></div><div class="col-md-3 col-md-offset-2 col-sm-4 col-sm-offset-2 col-xs-4">'+
							'<h4 class="sname">'+item["EndTime"]+'</h4><div class="input-group">'+
							'<input type="time" class="form-control totime" name="totime" value=""/></div></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Status</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<select  class="form-control taskStatus">'+
							'<option value="Completed">Completed</option><option value="Not Completed">Not Completed</option>'+
							'</select></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Reschedule Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control taskReschedule"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Reason</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control taskReason" value=""/></div></div>'+
							'<div class="form-group">'+
							'<label class="col-xs-3 col-sm-2 col-md-2">Alert HQ</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<select class="form-control taskAlert"><option value="No">No</option><option value="Yes">Yes</option></select></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed Start Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control completeStartDate"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed End Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control completeEndDate"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control completeBy"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Inspected By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control inspectedBy"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Verified By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control verifiedBy"/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Remarks</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control remarks"/></div></div>'+
							'</div>';


				}
				

				$(".taskList").append(htm);


			});
			
			// Select task status values
			$(".taskStatus").each(function(i,Status){
				$(this).val(data[i]["TaskStatus"]);
			});

			$(".taskStatus").change();

			// initialize signature
			/*$("#esign").jSignature({color:"#000000",height:'110px',width:'100%'});

			var string60Format = [];
			string60Format[0] = "data:image/png;base64";
			string60Format[1] = eventt["ESignature"];

			$("#esign").jSignature("setData",string60Format.join(","));*/
			$("#supSign,#clSign").jSignature({color:"#000000",height:'110px',width:'100%'});

			var string60Format = [];
			string60Format[0] = "data:image/png;base64";
			string60Format[1] = data["SupervisorSign"];

			$("#supSign").jSignature("setData",string60Format.join(","));

			string60Format[1] = data["ClientSign"];

			$("#clSign").jSignature("setData",string60Format.join(","));

			
		}

		else{
			// $(".showlistdocs").html("<div class='mrt-20'> No Details Found</div>");
		}
		hideLoading();
			

	});

}

function validateStatus(callBack){
	var errorMsg = "";
	$(".taskStatus").each(function(i,task){

				
		var status = $(this).val();
		var reason = decodeURIComponent($(this).parents(":eq(1)").next().next().find('.taskReason').val());
		var startTime = $(this).parents(":eq(1)").prev().find('.fromtime').val(); 
		var endTime = $(this).parents(":eq(1)").prev().find('.totime').val(); 
		var completeStartDate = $(this).parents(":eq(1)").nextAll(":eq(3)").find(".completeStartDate").val();
		var completeEndDate = $(this).parents(":eq(1)").nextAll(":eq(4)").find(".completeEndDate").val();
		var taskCompleteBy = $(this).parents(":eq(1)").nextAll(":eq(5)").find(".completeBy").val();
		var inspectedBy = $(this).parents(":eq(1)").nextAll(":eq(6)").find(".inspectedBy").val();
		var verifiedBy = $(this).parents(":eq(1)").nextAll(":eq(7)").find(".verifiedBy").val();

		var dStart = new Date(completeStartDate + " " + startTime);
		var dEnd = new Date(completeEndDate + " " + endTime);
		
		
		if(status == "Not Completed" && reason == ""){
			errorMsg = "Please enter reason";
		}
		else if(status == "Completed"){

			if(startTime == ""){
				errorMsg = "Please enter Start Time";
			}
			else if(endTime == ""){
				errorMsg = "Please enter End Time";
			}

			else if(completeStartDate == ""){
				errorMsg = "Please enter Completed Start Date";
			}
			else if(completeEndDate == ""){
				errorMsg = "Please enter Completed End Date";
			}
			else if(dEnd < dStart){
				errorMsg = "End Time should not be less than Start Time";
			}
			else if(taskCompleteBy == ""){
				errorMsg = "Please enter task Complete By";
			}
			else if(inspectedBy == ""){
				errorMsg = "Please enter Inspected By";
			}
			else if(verifiedBy == ""){
				errorMsg = "Please enter Verified By";
			}
			

		}
		
		
		if(errorMsg != ""){
			return false;
		}

		

	});
	
	if(errorMsg == "" && (checkSignature($("#supSign")) || checkSignature($("#clSign"))) == 0){
		errorMsg = "Please enter signature";
	}

	callBack(errorMsg);

}