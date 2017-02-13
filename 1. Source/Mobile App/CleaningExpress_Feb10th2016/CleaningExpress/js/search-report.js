listShowAroundDocs();

$("document").ready(function(){

	/*$(".showlistdocs").on('click','.sDoc',function(){
		sessionStorage.sDoc = $(this).data("docentry");
		window.location.href = "view-report.php";
	});*/

$(".showlistdocs").on('click','.list',function(e){
		e.preventDefault();
		sessionStorage.sDoc = $(this).find("a").data("docentry");
		window.location.href = "view-report.php";
	});

	$("#search-btn").click(function(){
		/*var searchTxt = $("#search").val().trim().toLowerCase();

		if(searchTxt == ""){
			$(".sAround").show();
		}
		else{

			$(".sDoc").each(function(i,doc){
				var doc = $(this).text().trim().toLowerCase();

				if(doc.indexOf(searchTxt) == -1){
					$(this).parents(":eq(3)").hide();
				}
				else{
					$(this).parents(":eq(3)").show();
				}

			});

		}*/
		search();
		
	});

	$("#search").keyup(function(){
		search();
	});

});

function search(){
	var searchTxt = $("#search").val().trim().toLowerCase();

		if(searchTxt == ""){
			$(".sAround").show();
			$(".showListErr").hide();
		}
		else{
			var count=0;
			/*$(".sDoc").each(function(i,doc){
				var doc = $(this).text().trim().toLowerCase();
				doc += $(this).parent().next().text().trim().toLowerCase();

				if(doc.indexOf(searchTxt) == -1){
					$(this).parents(":eq(3)").hide();
				}
				else{
					$(this).parents(":eq(3)").show();
					count++;
				}

			});*/
			$(".sDoc").each(function(i,doc){
				var doc = $(this).text().trim().toLowerCase();
				doc += $(this).parents(":eq(1)").next().find(".sname1").text().trim().toLowerCase();
				doc += $(this).parents(":eq(1)").next().find(".sname2").text().trim().toLowerCase();
				doc += $(this).parents(":eq(1)").next().next().find(".sname1").text().trim().toLowerCase();

				if(doc.indexOf(searchTxt) == -1){
					$(this).parents(":eq(4)").hide();
				}
				else{
					$(this).parents(":eq(4)").show();
					count++;
				}

			});

			if(count ==0){
				$(".showListErr").show();
			}
			else{
				$(".showListErr").hide();
			}

		}
}


function listShowAroundDocs(){
	var url = CONFIG.BASEPATH + "/MGet_ShowAround_ListofDocuments";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["sCompany"] =  companyUser["CompanyCode"];
	mProjectData["sCurrentUserName"] = companyUser["UserName"];
	mProjectData["sOwnerCode"] = companyUser["EmpId"];
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		if(data!="" && data.length>0){
				var showList = data;
								
				$(".showlistdocs").html("");
				$(showList).each(function(i,item){
					/*var itemHtm = '<div class="list sAround mrt-20 pad20 fullList"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6 pad0">' +
								'<h4 class="sname"><a href="javascript:void(0)" class="sDoc" data-docentry="'+item["DocEntry"]+'">'+item["DocNum"]+'</a></h4>' +
								'<h5 class="sname2">'+localeDate(item["DocDate"].split(" ")[0])+'</h5>' + '</div></div></div>';
					*/

					var itemHtm = '<div class="list sAround mrt-20 pad20 fullList"><table class="table-responsive"><tr><td class="sname"><a href="javascript:void(0)" class="sDoc" data-docentry="'+item["DocEntry"]+'">'+item["DocNum"]+'</a>' +
									'</td><td></td></tr>' + '<tr><td class="sname1">'+decodeURIComponent(item["ProspectName"])+'</td>' + '<td class="sname2">'+localeDate(item["DocDate"].split(" ")[0])+'</td>' +
									'</tr>' + '<tr><td class="sname1">'+decodeURIComponent(item["ProspectAddress"])+'</td>' + '<td class="sname2"></td>' + '</tr></table></div>';
					
					$(".showlistdocs").append(itemHtm);
				});

			}

			else{
				$(".showlistdocs").html("<div class='mrt-20'> No Details Found</div>");
			}
			hideLoading();


	});
}