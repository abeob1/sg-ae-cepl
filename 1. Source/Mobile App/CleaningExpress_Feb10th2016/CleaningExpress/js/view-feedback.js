$(document).ready(function(){

});

loadQuestions();
// change url, this is add feedback
function loadQuestions(){
	var url = CONFIG.BASEPATH + "/MGet_CustomerFeedback_Questions";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["sCompany"] =  companyUser["CompanyCode"];
	
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		/*data = [
    {
        "Company": "",
        "DocDate": "",
        "ClientName": "",
        "ClientPhone": "",
        "Project": "",
        "TImprovements": "",
        "TCompliments": "",
        "TRecommend": "",
        "IsRating": "",
        "Category": "SITE CREW COMPETENCY",
        "SubCategory": "Response Effectiveness (in emergency)",
        "SubCatText": "(Please note that for emergency call, we will need a minimum 2 hours for our team to be present at site)",
        "Question": "",
        "Rating": ""
    },
    {
        "Company": "",
        "DocDate": "",
        "ClientName": "",
        "ClientPhone": "",
        "Project": "",
        "TImprovements": "",
        "TCompliments": "",
        "TRecommend": "",
        "IsRating": "",
        "Category": "SITE CREW COMPETENCY",
        "SubCategory": "Service Effectiveness",
        "SubCatText": "(Please share with us on the effectiveness of our service @ site)",
        "Question": "",
        "Rating": ""
    },
    {
        "Company": "",
        "DocDate": "",
        "ClientName": "",
        "ClientPhone": "",
        "Project": "",
        "TImprovements": "",
        "TCompliments": "",
        "TRecommend": "",
        "IsRating": "",
        "Category": "STAFF COMPETENCY",
        "SubCategory": "In a gist!",
        "SubCatText": "",
        "Question": "Cheerful & Courteous",
        "Rating": ""
    },
    {
        "Company": "",
        "DocDate": "",
        "ClientName": "",
        "ClientPhone": "",
        "Project": "",
        "TImprovements": "",
        "TCompliments": "",
        "TRecommend": "",
        "IsRating": "",
        "Category": "STAFF COMPETENCY",
        "SubCategory": "In a gist!",
        "SubCatText": "",
        "Question": "Attentive & Proactive",
        "Rating": ""
    }
];*/


		if(data!="" && data.length>0){
												
				$(".fQuestions").html("");
				var category="";
				var SubCategory="";
				var itemHtm="";
				$(data).each(function(i,item){
					
					if(category != item["Category"] && i==0){
						category = item["Category"];
						SubCategory = item["SubCategory"];


						if(item["Question"] != ""){

							if(item["IsRating"] == "N"){

								itemHtm += '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+item["Category"]+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-7">'+
								'<h5>'+item["SubCategory"]+'<span class="required">*</span></h5><span>'+item["SubCatText"]+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'" data-question="'+item["Question"]+'"/>'+
								'</div></div>';

							}
							else{

								itemHtm += '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+item["Category"]+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-7">'+
								'<h5>'+item["SubCategory"]+'<span class="required">*</span></h5><span>'+item["SubCatText"]+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="checkbox-grid" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'">'+
								'<li><input type="checkbox" name="question'+i+'" value="three" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="two" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="one" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div></div>'

							}

							

						}
						else{

							if(itemHtm["IsRating"] == "N"){

								
								itemHtm += '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+item["Category"]+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+item["SubCategory"]+'<span class="required">*</span></h5><span>'+item["SubCatText"]+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'" data-question="'+item["Question"]+'"/>'+
								'</div></div>';

							}
							else{

								itemHtm += '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+item["Category"]+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+item["SubCategory"]+'<span class="required">*</span></h5><span>'+item["SubCatText"]+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="checkbox-grid" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'">'+
								'<li><input type="checkbox" name="question'+i+'" value="three" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="two" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="one" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div></div>'
								

							}

							

						}

						

					}
					else if(category != item["Category"] && i>0){

						category = item["Category"];
						SubCategory = item["SubCategory"];

						if(item["Question"] != ""){

							if(item["IsRating"] == "N"){

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+item["Category"]+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-7">'+
								'<h5>'+item["SubCategory"]+'<span class="required">*</span></h5><span>'+item["SubCatText"]+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'" data-question="'+item["Question"]+'"/>'+
								'</div>';

							}
							else{

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+item["Category"]+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-7">'+
								'<h5>'+item["SubCategory"]+'<span class="required">*</span></h5><span>'+item["SubCatText"]+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="checkbox-grid" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'">'+
								'<li><input type="checkbox" name="question'+i+'" value="three" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="two" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="one" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div>'

							}

							

						}
						else{

							if(item["IsRating"] == "N"){

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+item["Category"]+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+item["SubCategory"]+'<span class="required">*</span></h5><span>'+item["SubCatText"]+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'" data-question="'+item["Question"]+'"/>'+
								'</div>';

							}
							else{

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+item["Category"]+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+item["SubCategory"]+'<span class="required">*</span></h5><span>'+item["SubCatText"]+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="checkbox-grid" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'">'+
								'<li><input type="checkbox" name="question'+i+'" value="three" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="two" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="one" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div>'

							}

							

						}


					}

					else if(category == item["Category"]){

						if(SubCategory == item["SubCategory"]){

							if(item["IsRating"] == "N"){

								itemHtm += '<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
									'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'" data-question="'+item["Question"]+'"/>'+
									'</div>';

							}
							else{

								itemHtm += '<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="checkbox-grid" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'">'+
								'<li><input type="checkbox" name="question'+i+'" value="three" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="two" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="one" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div>'

							}

							
						}
						else{

							if(item["IsRating"] == "N"){

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+item["SubCategory"]+'<span class="required">*</span></h5><span>'+item["SubCatText"]+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'" data-question="'+item["Question"]+'"/>'+
								'</div>';

							}
							else{

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+item["SubCategory"]+'<span class="required">*</span></h5><span>'+item["SubCatText"]+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+item["Question"]+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="checkbox-grid" data-category="'+item["Category"]+'" data-subcategory="'+item["SubCategory"]+'">'+
								'<li><input type="checkbox" name="question'+i+'" value="three" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="two" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="checkbox" name="question'+i+'" value="one" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div>';

							}

							

						}
					}
					
					
					
					
				});
				$(".fQuestions").html(itemHtm);

			}

			else{
				$(".fQuestions").html("<div class='mrt-20'> No Details Found</div>");
			}

			var companyUser = JSON.parse(sessionStorage.user);

			$("#projectFeed").val(companyUser["CompanyName"]);


			hideLoading();


	});
}