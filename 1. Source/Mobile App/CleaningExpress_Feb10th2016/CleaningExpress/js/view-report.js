loadReportDetails();

function loadReportDetails(){
	var url = CONFIG.BASEPATH + "/MGet_ShowAround_DocumentDetails";
	var docEntry = sessionStorage.docEntry;
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["sCompany"] =  companyUser["CompanyCode"];
	mProjectData["sDocEntry"] = sessionStorage["sDoc"];
	showLoading();
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){

		var report = data[0];

		var htm = '<table class="table-responsive"><tr><td class="view1">Document No.</td>' +
					'<td class="desc1">'+report["DocNum"]+'</td>' + ' </tr> <tr><td class="view1">Document Date</td>' +
					'<td class="desc1">'+localeDate(report["DocDate"].split(" ")[0])+'</td>' + '</tr><tr>' +
					'<td class="view1">Owner</td>' + '<td class="desc1" data-code="'+report["OwnerCode"]+'">'+report["OwnerName"]+'</td>' +
					'</tr></table>';
		$(".vReportDetails").html(htm);

		report = data;

		$(".reportItems").html("");
		$(report).each(function(i,item){

			/*var itemHtm = '<div class="view"><table class="table-responsive"><tr>' +
						'<td class="repname">'+item["Category"]+'</td>' + '<td></td></tr><tr>'+
						'<td class="viewrep">'+item["Question"]+'</td>' + '<td class="viewrep">'+item["OwnerName"]+'</td>' +
						'</tr><tr><td class="viewrep">Nos</td><td class="viewrep1">'+parseFloat(item["Quantity"] || "0.00").toFixed(2)+'</td>'+
						'</tr><tr><td class="viewrep">Description</td><td class="viewrep">'+item["Description"]+'</td>' +
						'</tr></table></div>';*/
						var valextngQTY= "";
			
						var chkexstngQty=parseFloat(item["Quantity"] || " ").toFixed(2);
						if(chkexstngQty==0.00)
						{
						// valextngQTY=null;
						}
						else
						{
							valextngQTY=chkexstngQty;
						}
						
			var itemHtm = '<div class="view"><div class="repname">'+decodeURIComponent(item["Category"])+'</div><div class="viewrep">'+decodeURIComponent(item["Question"])+'</div><table class="table-responsive"><tr>' +
						'<td></td>' + '<td></td></tr><tr>'+
						'<td class="viewrep"></td>' + '<td class="viewrep"></td>' +
						'</tr><tr><td class="viewrep">Nos</td><td class="viewrep1">'+valextngQTY+'</td>'+
						'</tr><tr><td class="viewrep">Description</td><td class="viewrep">'+decodeURIComponent(item["Description"])+'</td>' +
						'</tr></table></div>';
			$(".reportItems").append(itemHtm);

		});
		
		var Remarks = report[0]["Remarks"] || "";
		var filename_decode = decodeURIComponent(Remarks);
		$("#remarks").val(filename_decode);
		// Attachment
		var attachFiles = report[0].Attachments;
			var atag = "";
			$(attachFiles).each(function(j,file){

				var filename = getFilename(file["WebURL"] || "");
				var filename_decode = decodeURIComponent(filename);
				atag += '<a href="'+(CONFIG.ATTACHMENT_DIR+filename)+'" download>'+filename_decode+'</a><br/>';

			});
			
			$("#atcEntry").html(atag);
		/*var filename = getFilename(data["WebURL"] || "");
		
		$("#atcEntry").attr("href",CONFIG.ATTACHMENT_DIR+filename).text(filename);*/

			
			hideLoading();
			
	});
}