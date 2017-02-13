$(document).ready(function(){

	$("#mainTabs a").click(function(){
		var tab = $(this).attr("href");
		var category = $(this).data("type");
		listStock(tab,category);

	});

	/*$(".tab-content").on('click','.vStock',function(e){
		e.preventDefault();
		sessionStorage.docEntry = $(this).data("docentry");
		window.location.href = "view-stock.php";
	});*/

	$(".tab-content").on('click','.list',function(e){
		e.preventDefault();
		sessionStorage.docEntry = $(this).find("a").data("docentry");
		window.location.href = "view-stock.php";
	});

});

listStock("#open","open");

function listStock(tab,category){
	var url = CONFIG.BASEPATH + "/MGet_StockRequestList";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["sCompany"] =  companyUser["CompanyCode"];
	mProjectData["sUserName"] = companyUser["UserName"];
	mProjectData["sCategory"] = category;
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
			fillTabs(tab,data,function(res){
				hideLoading();
			});
			
	});
}

function fillTabs(tab,data,callBack){

	$(tab).html("");
	$(data).each(function(i,stock){
		var htm = '<div class="list fullList"><table class="table-responsive"><tr><td class="sname"><a href="javascript:void(0)" class="vStock" data-docentry="'+stock["DocEntry"]+'">'+stock["DocNum"]+'</a>' +
					'</td><td></td></tr>' + '<tr><td class="sname1">'+decodeURIComponent(stock["ProjectCode"])+ " " +decodeURIComponent(stock["ProjectName"])+'</td>' + '<td class="sname2">'+localeDate(stock["ReqDate"])+'</td>' +
					'</tr>' + '<tr><td class="sname1">'+stock["ReqName"]+'</td>' + '<td class="sname2">'+stock["Status"]+'</td>' + '</tr></table></div>';
		$(tab).append(htm);
	});
	if(data == "" || data.length<=0){
		$(tab).html('<div class=""> No Stock Found </div>');
	}
	callBack("DONE");
}