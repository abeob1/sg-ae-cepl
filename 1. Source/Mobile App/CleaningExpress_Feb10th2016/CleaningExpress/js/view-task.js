loadEvents();

$(document).ready(function(){

	$("#clear").click(function(){
		$("#esign").jSignature("reset");
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
		
		//sessionStorage["vGardener"] = JSON.stringify(data);

		if(data!="" && data.length>0){

			var eventt = data[0];
			
			$("#form-field-1").val(showDate(eventt["ScheduledDate"].split(" ")[0]));
			$("#form-field-2").val(getToday());
			$("#form-field-3").val(eventt["DayStatus"]);
			
			var CompletedBy = eventt["CompletedBy"] || companyUser["EmployeeName"];
			var CompletedBy_decode = decodeURIComponent(CompletedBy);
			$("#form-field-4").val(CompletedBy_decode);
			
			var scheduledDate = new Date(showDate(eventt["ScheduledDate"].split(" ")[0])).getTime();
			var currentDate = new Date(getToday()).getTime();
			// Edit button validation
			if(eventt["DayStatus"] == "Completed"){
				$("#editBtn").hide();
			}
			else if(scheduledDate<=currentDate && eventt["DayStatus"] == "Open"){
				$("#editBtn").show();
			}
			else if(scheduledDate>currentDate && eventt["DayStatus"] == "Open"){
				$("#editBtn").hide();
			}

			$(".taskList").html("");
			$(data).each(function(i,item){
				
				
				var htm = '<div class="list"><div class="row"><div class="col-md-12 col-sm-12 col-xs-12">' +
							'<h4 class="sname">'+decodeURIComponent(item["CleaningType"])+'</h4><h5 class="subtitle">'+decodeURIComponent(item["Location"])+'</h5></div></div>'+
							'<div class="row time"><div class="col-md-3 col-md-offset-2 col-sm-4 col-sm-offset-2 col-xs-4 col-xs-offset-3">'+
							'<h4 class="sname">'+item["StartTime"]+'</h4><div class="input-group"><input type="time" class="form-control fromtime" name="fromtime" value="'+item["ActualStartTime"]+'" disabled/>'+
							'</div></div><div class="col-md-3 col-md-offset-2 col-sm-4 col-sm-offset-2 col-xs-4">'+
							'<h4 class="sname">'+item["EndTime"]+'</h4><div class="input-group">'+
							'<input type="time" class="form-control totime" name="totime" value="'+item["ActualEndTime"]+'" disabled/></div></div></div>'+
							
							/*Task Secheduled Not Completed*/
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Status</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<select  class="form-control taskStatus" disabled><option value="">Select</option><option value="Open">Open</option>'+
							'<option value="Completed">Completed</option><option value="Not Completed">Not Completed</option>'+
							'<option value="Canceled">Canceled</option></select></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Reschedule Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control taskReschedule" value="'+showDate(item["NewScheduleDate"].split(" ")[0])+'" disabled/></div></div>'+
							
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Reason</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control taskReason" value="'+decodeURIComponent(item["Reason"])+'" disabled/></div></div>'+
							
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Alert HQ</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control taskAlert" value="'+item["AlertHQ"]+'" disabled/></div></div>'+
							
							/*Task Secheduled Completed*/
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed Start Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control completeStartDate" value="'+showDate(item["CompletedStartDate"].split(" ")[0])+'" disabled/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed End Date</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="date" class="form-control completeEndDate" value="'+showDate(item["CompletedEndDate"].split(" ")[0])+'" disabled/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Completed By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control completeBy" value="'+decodeURIComponent(item["TaskCompleteBy"])+'" disabled/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Inspected By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control inspectedBy" value="'+decodeURIComponent(item["InspectedBy"])+'" disabled/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Verified By</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control verifiedBy" value="'+decodeURIComponent(item["VerifiedBy"])+'" disabled/></div></div>'+
							'<div class="form-group"><label class="col-xs-3 col-sm-2 col-md-2">Remarks</label><div class="col-xs-8 col-sm-9 col-md-9">'+
							'<input type="text" class="form-control remarks" value="'+decodeURIComponent(item["remarks"])+'" disabled/></div></div>'+
							'</div>';

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

			$("#esign").jSignature("setData",string60Format.join(","));
			$("#esign").jSignature("disable");*/
			
			$("#supSign,#clSign").jSignature({color:"#000000",height:'110px',width:'100%'});
			
		

			var string60Format = [];
			string60Format[0] = "data:image/png;base64";
			string60Format[1] = eventt["SupervisorSign"] || "";

			$("#supSign").jSignature("setData",string60Format.join(","));

			string60Format[1] = eventt["ClientSign"] || "";

			$("#clSign").jSignature("setData",string60Format.join(","));

			// disable signature
			$("#clSign").jSignature("disable");
			$("#supSign").jSignature("disable");
			
			//Signature Date show
			$("#supsigndate").val(showDate(eventt["SignedDate1"].split(" ")[0]));
			$("#clsigndate").val(showDate(eventt["SignedDate2"].split(" ")[0]));
			

			// Attachment
			var attachFiles = eventt["Attachments"];
			var atag = "";
			$(attachFiles).each(function(j,file){

				var filename = getFilename(file["WebURL"] || "");
				var filename_decode = decodeURIComponent(filename);
				atag += '<a href="'+(CONFIG.ATTACHMENT_DIR+filename)+'" download>'+filename_decode+'</a><br/>';

			});
			
			$("#atcEntry").html(atag);
			/*var filename = eventt["WebURL"] || "";
			if(filename != ""){
				filename = filename.substr(filename.lastIndexOf("\\")+1);
			}
			$("#atcEntry").attr("href",CONFIG.ATTACHMENT_DIR+filename).text(filename);*/


		}

		else{
			// $(".showlistdocs").html("<div class='mrt-20'> No Details Found</div>");
		}
		hideLoading();
			

	});

}