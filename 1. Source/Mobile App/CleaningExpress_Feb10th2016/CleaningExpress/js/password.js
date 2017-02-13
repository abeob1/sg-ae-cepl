$(document).ready(function(){

	$("#save").click(function(){
		validatePwd(function(err){
			if(err!=""){
				alert(err);
			}
			else{

				var url = CONFIG.BASEPATH + "/M_ChangePassword";
				var companyUser = sessionStorage["user"];
				if(companyUser){
					companyUser = JSON.parse(companyUser);
				}
				var pwd = {};
				pwd["EmpId"] = companyUser["EmpId"];
				pwd["EmpName"] = companyUser["EmployeeName"];
				pwd["UserName"] = companyUser["UserName"];
				pwd["CurrentPwd"] = $("#newPwd").val();
				pwd["OldPwd"] = $("#oldPwd").val();
				pwd["Company"] = companyUser["CompanyCode"];


				showLoading();
				postToServer(url,"POST",JSON.stringify(pwd),function(data){
						// console.log(data);
						if(data[0] && data[0].Result=="Success"){
							alert("Password changed Successfully");
							window.location.href = "index.php";
						}
						else{
							alert("Error ! Message: " + data[0].DisplayMessage);
						}

						hideLoading();
				});
			}
		});

	});

	$("#cancel").click(function(){
		window.location.href = "dashboard.php";
	});


});

function validatePwd(callBack){
	var errorMsg = "";

	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}



	if($("#newPwd").val() != $("#confirmPwd").val()){
		errorMsg = "New Password and Confirm password are not same";
	}
	else if($("#oldPwd").val() == $("#newPwd").val()){
		errorMsg = "New Password should not be same as old password";
	}
	else if(companyUser["Password"] != $("#oldPwd").val()){
		errorMsg = "Old Password is inCorrect";
	}
	else if($("#newPwd").val() == "" || $("#oldPwd").val()=="" || $("#confirmPwd").val()==""){
		errorMsg = "Please enter password";
	}

	callBack(errorMsg);
}