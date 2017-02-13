$(document).ready(function(){

	loadEvents();

	$('#full-calendar').fullCalendar({
	 	header: {

	                left: 'prev,next today',

	                center: 'title',

	                right: 'month,agendaWeek,agendaDay'

	            },
	    eventClick: function (calEvent, jsEvent, view) {

   	 		sessionStorage.cEvent = calEvent.ScheduledDate;
   	 		window.location.href = "view-task.php";
   	 	}


	});


});


function loadEvents(){

	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_JobSchedule_JobDetails";
	var mProjectData = {};
	var schedule="";
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["Company"] =  companyUser["CompanyCode"];
	mProjectData["DocEntry"] = sessionStorage["jbdocEntry"];
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		if(data!="" && data.length>0){
			
			//var eventt = data[0];
			$(data).each(function(i,eventt){
				eventt["title"] = decodeURIComponent(eventt["CleaningType"]);
				eventt["start"] = eventt["ScheduledDate"];
				eventt["allDay"] = false;

				schedule = eventt["ScheduledDate"];
				if(schedule != ""){
					schedule = schedule.split(" ");
					schedule = showDate(schedule[0]);
					schedule = schedule + "T" + eventt["StartTime"] + ":00";
					eventt["start"] = schedule;
				}

				var scheduledDate = new Date(showDate(eventt["ScheduledDate"].split(" ")[0])).getTime();
				var currentDate = new Date(getToday()).getTime();
				if(scheduledDate == currentDate && eventt["DayStatus"] == "Open"){
					//Dark Green
					eventt["color"] = "#013220";
				}else if(scheduledDate == currentDate && eventt["DayStatus"] == "Closed"){
					//Blue
					eventt["color"] = "#0000ff";
				}
				else if(scheduledDate > currentDate && eventt["DayStatus"] == "Open"){
					//Green
					//alert("green");
					eventt["color"] = "#00ff00";
				}
				else if(scheduledDate > currentDate && eventt["DayStatus"] == "Closed"){
					//Blue
					eventt["color"] = "#0000ff";
				}
				else if(scheduledDate < currentDate && eventt["DayStatus"] == "Open"){
					//Red
					eventt["color"] = "#ff0000";
				}else if(scheduledDate < currentDate && eventt["DayStatus"] == "Closed"){
					//Blue
					eventt["color"] = "#0000ff";
				}
				else if(eventt["DayStatus"] == "Completed"){
					//eventt["color"] = "#00AEFF";
				}
				
			});

			$('#full-calendar').fullCalendar('addEventSource',data);
				

		}

		else{
			// $(".showlistdocs").html("<div class='mrt-20'> No Details Found</div>");
		}
		hideLoading();
			

	});

}