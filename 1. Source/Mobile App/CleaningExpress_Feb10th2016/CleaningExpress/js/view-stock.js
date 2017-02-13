loadStockDetails();

function loadStockDetails(){
	var url = CONFIG.BASEPATH + "/MGet_StockRequestDetails";
	var docEntry = sessionStorage.docEntry;
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["sCompany"] =  companyUser["CompanyCode"];
	mProjectData["sDocEntry"] = docEntry;
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
			if(data!="" && data.length>0){
				var stock = data[0];
				var htm = '<table class="table-responsive"><tr>' + '<td class="view1">Request No.</td>'+
						'<td class="desc1">'+stock["DocNum"]+'</td>' + '</tr><tr>' + '<td class="view1">Project Name</td>'+
						'<td class="desc1">'+ stock["ProjectCode"] +" "+ decodeURIComponent(stock["ProjectName"]) +'</td>' + '</tr><tr>' + '<td class="view1">Requested By</td>'+
						'<td  class="desc1">'+stock["ReqName"]+'</td>' + '</tr><tr>' + '<td class="view1">Required Date</td>'+
						'<td  class="desc1">'+ localeDate(stock["ReqDate"]) +'</td>' + '</tr><tr>' + '<td class="view1">Status</td>'+
						'<td  class="desc1">'+stock["Status"]+'</td>' + '</tr></table>';
				$(".ProjectDetails").html(htm);

				// Hardcoded for items as response should change.

				/*var itemHtm = '<div class="view"><table class="table-responsive">' + 
								'<tr><td class="vname">'+stock["ItemCode"]+'</td>' + '<td></td>'+ '</tr><tr>' +
								'<td class="vname1">'+stock["Description"]+'</td>' + '<td class="vname2">'+ parseInt(stock["AvailableQty"])+'</td>' +
								'</tr></table></div>';
				$(".itemDetails").html(itemHtm);*/

				//New logic...

				
				$(".itemDetails").html("");
				$(stock.Items).each(function(i,item){
					var itemHtm = '<div class="view"><table class="table-responsive">' + 
								'<tr><td class="vname" data-budgetdocentry="'+item["BKTDocEntry"]+'" data-budgetlineid="'+item["BKTLineId"]+'">'+item["ItemCode"]+'</td>' + '<td></td>'+ '</tr><tr>' +
								'<td class="vname1">'+decodeURIComponent(item["Description"])+'</td>' + '<td class="vname2 wid75">'+ parseFloat(item["Quantity"] || "0.000").toFixed(2)+'</td><td class="clrGreen">'+item["UOM"]+'</td>' +
								'</tr></table></div>';
					$(".itemDetails").append(itemHtm);
				});

			}

			else{
				$(".ProjectDetails").html("<div> No Details Found</div>");
			}
			hideLoading();
			
	});
}