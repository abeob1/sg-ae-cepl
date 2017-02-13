authorize();
var CONFIG = new Array();
CONFIG.BASEURL = 'http://203.125.57.116:3870/LIVE/CleaningExpress/';
CONFIG.BASEPATH = 'http://203.125.57.116:3870/LIVE/CleaningExpress_WebService/Mobile.asmx';
CONFIG.OPERATIONS_BASEPATH = "http://203.125.57.116:3870/LIVE/CleaningExpress_WebService/Operations.asmx";
CONFIG.ATTACHMENT_DIR = "http://203.125.57.116:3870/LIVE/CleaningExpress_WebService/Attachments/";
CONFIG.WEBSERVICEURL = "http://203.125.57.116:3870/LIVE/CleaningExpress_WebService/";

function getFromServer(url,method,callBack){
	$.ajax({
		url:url,
		method:method,
		
		success:function(res){
			res = JSON.parse(res);
			callBack(res);
		},
		error:function(res){
			callBack(res);
		}
	});
}

function postToServer(url,method,data,callBack){
	var dJson = "sJsonInput="+data+"";
	$.ajax({
		url:url,
		method:method,
		
		data:dJson,
		success:function(res){
			res = JSON.parse(res);
			callBack(res);
		},
		error:function(res){
			callBack(res);
			alert("Some Error occurred in the server. Please check your Network connectivity and try again !");
		}
	});
}

function uploadFileToServer(url,method,form,callBack){
	var formData = new FormData($(form)[0]);

	$.ajax({
	        url:url,
	        type: 'POST',
            data: formData,
            success:function(res){
            	res = JSON.parse(res);
				callBack(res);
            },
            error:function(res){
            	callBack(res);
				alert("Some Error occurred in the server. Please check your Network connectivity and try again !");
            },
            cache: false,
            contentType: false,
            processData: false
	       
	       
	    });
}

function showLoading(){
	$("#fade").show();
	$("#loader").show();
}

function hideLoading(){
	$("#fade").hide();
	$("#loader").hide();

}

function convertDate(curDate){
	var reqDate = "";
	if(curDate){
		curDate = curDate.split("-");
		reqDate = curDate[1] + "/" + curDate[2] + "/" + curDate[0];
	}
	return reqDate;
}

function authorize(){
	var path = window.location.pathname;
	path = path.split("/");
	if((path[path.length-1]!="" && path[path.length-1]!="index.php") && (sessionStorage.user == undefined || sessionStorage.user == "" || JSON.parse(sessionStorage.user).Status!="Login Successful")){
		window.location = "index.php";
	}
}

function localeDate(date){
	var month = ["","Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"];
	var dArr = date.split("/");
	return dArr[1] + " " + month[+dArr[0]] + " " + dArr[2];
}

function showDate(date){
	var reqDate = "";
	if(date){
		date = date.split("/");
		reqDate = date[2] + "-" + date[0] + "-" + date[1];
	}
	return reqDate;
}

function jDatePickerSetFormat(date){
	var reqDate = "";
	var month = ["","Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"];
	if(date){
		date = date.split("/");
		reqDate = date[1] + "-" + month[+date[0]] + "-" + date[1];
	}
	return reqDate;
}

function jDatePickerGetFormat(date){
	var reqDate = "";
	var month = ["","Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"];
	if(date){
		date = date.split("/");
		reqDate = date[1] + "-" + month[+date[0]] + "-" + date[1];
	}
	return reqDate;
}

function manageUser(){
	var user = JSON.parse(sessionStorage["user"]);
	$("#headerTitle").text(user["CompanyName"]);
	$("#headerUser").text(user["EmployeeName"]);
}

function logout(){
	var USER = JSON.parse(sessionStorage.user);
	sessionStorage.clear();
	sessionStorage.userName = USER.UserName;
	sessionStorage.companyCode = USER.CompanyCode;
	window.location = "index.php";
}

function getToday(){
	var today = new Date();
	var dd = today.getDate();
	var mm = today.getMonth()+1; //January is 0!
	var yyyy = today.getFullYear();

	if(dd<10) {
	    dd='0'+dd
	} 

	if(mm<10) {
	    mm='0'+mm
	} 

	today = yyyy + "-" + mm + "-" + dd;

	return today;
}

function applyTheme(){

	if(sessionStorage.user){

		var companyUser = JSON.parse(sessionStorage.user);

		if(companyUser.Theme == "1" ){

			$('head').append('<link rel="stylesheet" href="assets/css/themes/blue.css" type="text/css" />');
			setTimeout(function(){
				$("#companySlogo").attr("src","CompanyLogo/CleaningExpress.png");
			},1000);

		}
		else if(companyUser.Theme == "2"){

			$('head').append('<link rel="stylesheet" href="assets/css/themes/green.css" type="text/css" />');
			setTimeout(function(){
				$("#companySlogo").attr("src","CompanyLogo/GreenServe.png");
			},1000);

		}
		else{
			
			$('head').append('<link rel="stylesheet" href="assets/css/themes/yellow.css" type="text/css" />');
			setTimeout(function(){
				$("#companySlogo").attr("src","CompanyLogo/ExpressPest.png");
			},1000);

			

		}

	}
	
}

function roleMatrix(){
//1-Operations Manager
//2-Operations Executive
//3-Site Supervisor
//4-Sales
 //var matrix = {"1":"","2":"","3":".sMarketing","4":".sInventory,.sOperations"};
 var matrix = {"Operations Manager":"","Operations Executive":"","Site Supervisor":".sMarketing","Sales":".sInventory,.sOperations,.sOperations1"};
 var companyUser = sessionStorage["user"];
 if(companyUser){
  companyUser = JSON.parse(companyUser);
 }
 if(matrix[companyUser["RoleName"]] != ""){
  $(matrix[companyUser["RoleName"]]).hide();
 }
}

function clearProject(){

	sessionStorage.removeItem("listprProject");
	window.location.href = "list-project.php";
}

var emptySign = "iVBORw0KGgoAAAANSUhEUgAAAWAAAABuCAYAAAAZHMmIAAADsElEQVR4Xu3WgQ3CMBAEwU9FQBcu2V0kdEQZi+ShgZOH1yrX+BEgQIBAInAlq0YJECBAYATYERAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASEOAI3iwBAgQE2A0QIEAgEhDgCN4sAQIEBNgNECBAIBIQ4AjeLAECBATYDRAgQCASyAO8975n5h293ywBAucKPGutT/n8fwjwMzOvEsE2AQJHCnzXWunHXx7gI/92jyZAgMDMCLAzIECAQCQgwBG8WQIECAiwGyBAgEAkIMARvFkCBAgIsBsgQIBAJCDAEbxZAgQICLAbIECAQCQgwBG8WQIECAiwGyBAgEAkIMARvFkCBAgIsBsgQIBAJCDAEbxZAgQICLAbIECAQCQgwBG8WQIECAiwGyBAgEAkIMARvFkCBAgIsBsgQIBAJCDAEbxZAgQICLAbIECAQCQgwBG8WQIECPwAlukKb7iQdY0AAAAASUVORK5CYII=";

function checkSignature($sign){

	return $sign.jSignature('getData', 'native').length;
}

function getFilename(ss){
	var filename = "";
	if(ss != ""){
		filename = ss.substr(ss.lastIndexOf("\\")+1);
	}
	return filename;
}