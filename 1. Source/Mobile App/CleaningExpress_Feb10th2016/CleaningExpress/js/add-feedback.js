$(document).ready(function(){

	// Initialize plugin

	$("#esign").jSignature({color:"#000000",height:'110px',width:'100%'});

	$("#save").click(function(){

		validateRequired(function(errorMsg){
			if(errorMsg != ""){
				alert(errorMsg);
			}
			else{

				if(confirm("Confirm Save?")){

					var url = CONFIG.BASEPATH + "/MSave_CustomerFeedback";
					var companyUser = sessionStorage["user"];
					if(companyUser){
						companyUser = JSON.parse(companyUser);
					}

					var feedback = JSON.parse(sessionStorage["addFeedback"]);

					$("input[type='radio']:checked").each(function(){
						var rating = $(this).val();
						var category = $(this).parents(":eq(1)").data("category");
						var subCatogory = $(this).parents(":eq(1)").data("subcategory");
						var question = $(this).data("question");

						$(feedback).each(function(i,item){
							if(item["Category"] == category && item["SubCategory"] == subCatogory && item["Question"] == question){
								item["Rating"] = rating;
								return false;
							}
						});

					});

					// Now append text box values..
					var feedProject = JSON.parse(sessionStorage.feedProject);
					var signature60Format = $("#esign").jSignature("getData");
					$(feedback).each(function(i,item){

						item["TImprovements"] = encodeURIComponent($("#feedText1").val());
						item["TCompliments"] = encodeURIComponent($("#feedText2").val());
						item["TRecommend"] = $("#feedText3").val();
						item["ClientName"] = encodeURIComponent($("#NameFeed").val());
						item["Project"] = decodeURIComponent(feedProject["val"]);
						item["ClientPhone"] = $("#custNumFeed").val();
						item["Remarks"] = encodeURIComponent($("#remarks").val());

						item["LoginUser"] = companyUser["UserName"];
						item["OwnerName"] = companyUser["EmployeeName"];
						item["Owner"] = companyUser["EmpId"]

						item["Signature"] = signature60Format.split(",")[1];
					});
					console.log(JSON.stringify(feedback));

					showLoading();
					postToServer(url,"POST",JSON.stringify(feedback),function(data){
					console.log(data);
						if(data[0] && data[0].Result=="Success"){
							alert("Survey document is created successfully in SAP");
							window.location.href = "list-feedback.php";
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
		window.location.href = "list-feedback.php";
	});

	$("#custNumFeed").keyup(function(){
		validateNumber();
	});

	$("#clear").click(function(){
		$("#esign").jSignature("reset");
	});

});

loadQuestions();
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
		sessionStorage["addFeedback"] = JSON.stringify(data);

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

								itemHtm += '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+decodeURIComponent(item["Category"])+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-7">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["SubCatText"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'" data-question="'+decodeURIComponent(item["Question"])+'"/>'+
								'</div></div>';

							}
							else{

								itemHtm += '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+decodeURIComponent(item["Category"])+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-7">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["SubCatText"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="sradio-grid" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'">'+
								'<li><input type="radio" name="question'+i+'" value="1" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="2" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="3" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="4" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="5" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div></div>'

							}

							

						}
						else{

							if(itemHtm["IsRating"] == "N"){

								
								itemHtm += '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+decodeURIComponent(item["Category"])+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["SubCatText"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'" data-question="'+decodeURIComponent(item["Question"])+'"/>'+
								'</div></div>';

							}
							else{

								itemHtm += '<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+decodeURIComponent(item["Category"])+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["SubCatText"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="sradio-grid" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'">'+
								'<li><input type="radio" name="question'+i+'" value="1" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="2" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="3" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="4" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="5" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div></div>'
								

							}

							

						}

						

					}
					else if(category != item["Category"] && i>0){

						category = item["Category"];
						SubCategory = item["SubCategory"];

						if(item["Question"] != ""){

							if(item["IsRating"] == "N"){

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+decodeURIComponent(item["Category"])+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-7">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["SubCatText"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'" data-question="'+decodeURIComponent(item["Question"])+'"/>'+
								'</div>';

							}
							else{

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+decodeURIComponent(item["Category"])+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-7">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["SubCatText"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="sradio-grid" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'">'+
								'<li><input type="radio" name="question'+i+'" value="1" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="2" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="3" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="4" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="5" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div>'

							}

							

						}
						else{

							if(item["IsRating"] == "N"){

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+decodeURIComponent(item["Category"])+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["SubCatText"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'" data-question="'+decodeURIComponent(item["Question"])+'"/>'+
								'</div>';

							}
							else{

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h4>'+decodeURIComponent(item["Category"])+'</h4>'+
								'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["SubCatText"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="sradio-grid" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'">'+
								'<li><input type="radio" name="question'+i+'" value="1" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="2" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="3" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="4" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="5" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div>'

							}

							

						}


					}

					else if(category == item["Category"]){

						if(SubCategory == item["SubCategory"]){

							if(item["IsRating"] == "N"){

								itemHtm += '<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
									'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'" data-question="'+decodeURIComponent(item["Question"])+'"/>'+
									'</div>';

							}
							else{

								itemHtm += '<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="sradio-grid" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'">'+
								'<li><input type="radio" name="question'+i+'" value="1" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="2" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="3" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="4" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="5" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div>'

							}

							
						}
						else{

							if(item["IsRating"] == "N"){

								itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["SubCatText"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control ratingTxt" type="text" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'" data-question="'+decodeURIComponent(item["Question"])+'"/>'+
								'</div>';

							}
							else{

								if(item["Question"] != ""){

									itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span></span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="sradio-grid" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'">'+
								'<li><input type="radio" name="question'+i+'" value="1" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="2" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="3" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="4" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="5" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div>';

								}
								else{

									itemHtm += '</div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6">'+
								'<h5>'+decodeURIComponent(item["SubCategory"])+'<span class="required">*</span></h5><span>'+decodeURIComponent(item["SubCatText"])+'</span></div>'+
								'<div class="col-md-8 col-sm-7 col-xs-6"><span>'+decodeURIComponent(item["Question"])+'</span></div>'+
								'<div class="col-md-4 col-sm-5 col-xs-6"><ul class="sradio-grid" data-category="'+decodeURIComponent(item["Category"])+'" data-subcategory="'+decodeURIComponent(item["SubCategory"])+'">'+
								'<li><input type="radio" name="question'+i+'" value="1" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="2" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="3" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="4" data-question="'+item["Question"]+'"/><label></label></li>'+
								'<li><input type="radio" name="question'+i+'" value="5" data-question="'+item["Question"]+'"/><label></label></li>'+
								'</ul></div>';

								}

								

							}

							

						}
					}
					
					
					
					
				});

				/*itemHtm +='<div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h5>Innovating to Improve!<span class="required">*</span></h5>'+
						'</div><div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control" type="text" id="fedText1" />'+
						'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h5>Striving harder with YOUR Compliments!  <span class="required">*</span></h5>'+
						'</div><div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control" type="text" id="fedText2" />'+
						'</div></div><div class="row"><div class="col-md-8 col-sm-7 col-xs-6"><h5>Would you recommend us to your stakeholders?   <span class="required">*</span></h5>'+
						'</div><div class="col-md-4 col-sm-5 col-xs-6 fedtext"><input class="form-control" type="text" id="fedText3" />'+
						'</div></div>';*/

				$(".fQuestions").html(itemHtm);

			}

			else{
				$(".fQuestions").html("<div class='mrt-20'> No Details Found</div>");
			}

			var feedProject = JSON.parse(sessionStorage.feedProject);

			$("#projectFeed").val(feedProject["txt"]);


			hideLoading();


	});
}

function validateRequired(callBack){

	var selected = $("input[type='radio']:checked").length;
	var total = $("input[type='radio']").length/5;
	var errorMsg  = "";
	var pattern = /^[\d+\-\ \,]+$/;

	if(selected != total){
		errorMsg = "Please select rating to required questions";

	}
	else if($("#feedText3").val()==""){
		errorMsg = "Please fill required Questions";
		$(".mandatory").removeClass("mandatory");
		$("#feedText3").addClass("mandatory");

	}
	else if($("#NameFeed").val() == ""){
		errorMsg = "Please enter Name";
		$(".mandatory").removeClass("mandatory");
		$("#NameFeed").addClass("mandatory");
	}
	else if($("#custNumFeed").val() == ""){
		errorMsg = "Please enter Contact Number/s";
		$(".mandatory").removeClass("mandatory");
		$("#custNumFeed").addClass("mandatory");
	}
	else if($("#custNumFeed").val().length < 8 ||$("#custNumFeed").val().length > 8 && !pattern.test($("#custNumFeed").val())){
		errorMsg = "Please enter valid Contact Number/s that should not be less than 8 digits.";
		$(".mandatory").removeClass("mandatory");
		$("#custNumFeed").addClass("mandatory");
	}
	else if(checkSignature($("#esign")) <= 0){
		errorMsg = "Please fill Signature";
		$(".mandatory").removeClass("mandatory");
		$("#esign").addClass("mandatory");
	}

	callBack(errorMsg);

}

function validateNumber(){
	var pattern = /^[\d+\-\ \,]+$/;
	if($("#custNumFeed").val() != ""){
		if(!pattern.test($("#custNumFeed").val())){
		alert("Please enter valid Contact Number/s");
		$("#custNumFeed").val("");
		}
	}
	
	
}