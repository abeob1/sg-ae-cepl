loadProject();
loadQuestions();

$(document).ready(function(){

	$("#cancel").click(function(){
		window.location.href="list-inspection.php";
	});
	

});


function loadQuestions(){
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_InspectionQA_ViewMSCDetails";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["Company"] =  companyUser["CompanyCode"];
	mProjectData["DocEntry"] = sessionStorage.insdocEntry;
	
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
		sessionStorage["vInspection"] = JSON.stringify(data);

		if(data!="" && data.length>0){
												
			$(".vQuestions").html("");
			var category = "";
			console.log(data[0]);
			data = data[0];

			// setting common values
			$("#form-field-select-1").val(data["MKTSegment"]);
			$("#remarks").val(data["Remarks"]);
			$("#supervisor").val(data["InspectedBy"]);
			$("#client").val(data["AcknowledgeBy"]);

			// Signature
			// initialize signature
			/*$("#esign").jSignature({color:"#000000",height:'110px',width:'100%'});

			var string60Format = [];
			string60Format[0] = "data:image/png;base64";
			string60Format[1] = data["ESignature"];

			$("#esign").jSignature("setData",string60Format.join(","));
			$("#esign").jSignature("disable");*/
			// Signature
			// initialize signature
			$("#supSign,#clSign").jSignature({color:"#000000",height:'110px',width:'100%'});

			var string60Format = [];
			string60Format[0] = "data:image/png;base64";
			string60Format[1] = data["InspectedSign"];

			$("#supSign").jSignature("setData",string60Format.join(","));

			string60Format[1] = data["AcknowledgeSign"];

			$("#clSign").jSignature("setData",string60Format.join(","));

			// disable signature
			$("#clSign").jSignature("disable");
			$("#supSign").jSignature("disable");
			
			
			var Remarks = data[0]["Remarks"] || "";
			var filename_decode = decodeURIComponent(Remarks);
			$("#remarks").val(filename_decode);

			// Attachment
			var attachFiles = data[0].Attachments;
			var atag = "";
			$(attachFiles).each(function(j,file){

				var filename = getFilename(file["WebURL"] || "");
				var filename_decode = decodeURIComponent(filename);
				atag += '<a href="'+(CONFIG.ATTACHMENT_DIR+filename)+'" download>'+filename+'</a><br/>';

			});
			
			$("#atcEntry").html(atag);

			// var filename = getFilename(data["WebURL"] || "");
			
			// $("#atcEntry").attr("href",CONFIG.ATTACHMENT_DIR+filename).text(filename);

			$(data.MarketingSegment).each(function(i,item){
				var htm;
				if(category != item["Category"]){
					category = item["Category"];

					htm = '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+item["Category"]+'</h4></div></div>';

					htm+= '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h5>'+item["Item"]+'</h5></div>'+
							'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="sradio-grid">'+
							'<li><input type="radio" name="question_'+i+'" value="1" disabled/><label></label></li>'+
							'<li><input type="radio" name="question_'+i+'" value="2" disabled/><label></label></li>'+
							'<li><input type="radio" name="question_'+i+'" value="3" disabled/><label></label></li>'+
							'<li><input type="radio" name="question_'+i+'" value="4" disabled/><label></label></li>'+
							'<li><input type="radio" name="question_'+i+'" value="5" disabled/><label></label></li>'+
							'</ul></div></div>';

				}
				else{

					htm = '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h5>'+item["Item"]+'</h5></div>'+
							'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="sradio-grid">'+
							'<li><input type="radio" name="question_'+i+'" value="1" disabled/><label></label></li>'+
							'<li><input type="radio" name="question_'+i+'" value="2" disabled/><label></label></li>'+
							'<li><input type="radio" name="question_'+i+'" value="3" disabled/><label></label></li>'+
							'<li><input type="radio" name="question_'+i+'" value="4" disabled/><label></label></li>'+
							'<li><input type="radio" name="question_'+i+'" value="5" disabled/><label></label></li>'+
							'</ul></div></div>';

				}
				
				$(".vQuestions").append(htm);

				$("input[type='radio'][name='question_"+i+"'][value='"+item["Rating"]+"']").attr('checked',true);
				
				
			});

		
			

		}

		else{
			$(".vQuestions").html("<div class='mrt-20'> No Details Found</div>");
		}

		

			hideLoading();


	});
}

function loadProject(){
	var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_InspectionQA_MarketSegment";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["Company"] =  companyUser["CompanyCode"];
	
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		if(data!="" && data.length>0){
				var projList = data;
								
				var opt ='<option value="">Select</option>';
				if(projList && projList[0] && projList[0].BaseEntry){

					$(data).each(function(i,project){
						opt += '<option value="'+project["MktSegment"]+'">'+project["MktSegment"]+'</option>';

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