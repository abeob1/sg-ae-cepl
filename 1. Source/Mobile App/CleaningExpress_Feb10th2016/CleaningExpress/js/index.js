$(document).ready(function(){
	console.log("ready");
	$("#login").click(function(e){
		e.preventDefault();
		validateLogin(function(res){
			if(res == ""){
				showLoading();
				authenticateUser(function(response){
					hideLoading();
					if(response == "SUCCESS"){
						window.location.href = "dashboard.php";
					}
					else{
						$(".cError").text(response);
						$(".cError").show();
					}

				});
			}
			else{
				alert(res);
			}
			
		});
		
	});

});

//var url = "data/MGet_CompanyList.json";
var url = CONFIG.BASEPATH + "/MGet_CompanyList"
getFromServer(url,"GET",function(data){
		sessionStorage.MGet_CompanyList = JSON.stringify(data);
		var opt ='<option value="">Select</option>';
		$(data).each(function(i,company){
			opt += '<option value="'+company["U_DBName"]+'">'+company["U_CompName"]+'</option>';

		});
		$("#form-field-select-1").html(opt);
		checkSession();
		
});

function authenticateUser(callBack){
	var user = {};
	//Hardcoded need to remove..
	// var url = "data/MGet_Acknowledgement.json";
	var url = CONFIG.BASEPATH + "/MGet_Acknowledgement"
	user["sUserName"] = $("#form-field-1").val().trim();
	user["sPassword"] = $("#form-field-2").val().trim();
	user["sCompany"] = 	$("#form-field-select-1").val().trim();
	user = JSON.stringify(user);
	//var data = "sJsonInput="+user+"";
	

	postToServer(url,"POST",user,function(res){
		if(res && res[0] && res[0].Status && res[0].Status != "Login Failed"){
			sessionStorage["user"] = JSON.stringify(res[0]);
			callBack("SUCCESS");
		}
		else{
			callBack(res[0]["Message"]);
		}

	});

}

function validateLogin(callBack){
	var erorMsg = "";

	if($("#form-field-1").val() == ""){
		erorMsg = "Kindly Enter Username";
	}
	else if($("#form-field-2").val() == ""){
		erorMsg = "Kindly Enter Password";
	}
	else if($("#form-field-select-1").val() == ""){
		erorMsg = "Please Select Company";
	}

	callBack(erorMsg);
}

function checkSession(){
	$("#form-field-1").val(sessionStorage.userName || "");
	$("#form-field-select-1").val(sessionStorage.companyCode || "");
}
