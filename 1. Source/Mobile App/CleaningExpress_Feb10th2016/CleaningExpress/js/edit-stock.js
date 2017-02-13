$(document).ready(function(){

	showLoading();
	getStockDetailsData(function(res){
		hideLoading();
	});

	$(".itemList").on('click','.minus',function(){

		// if(confirm("Are you sure you want to remove the item ?")){
			var itemCode = $(this).data("code");
			var lineId = $(this).data("lineid").toString();
			eremoveItems.push(lineId);
			$(this).parents(":eq(2)").remove();
			var index = eProjectSpecific.indexOf(lineId);
			if(index != -1){
				eProjectSpecific.splice(index,1);
			}

			var stockRequestDetails = JSON.parse(sessionStorage["stockRequestDetails"]);
			var filterItem = stockRequestDetails["Items"].filter(function(obj){
				return obj.BKTLineId == lineId;
			});

			if(filterItem.length >0){
				if(eDetailsRemoved.indexOf(filterItem[0]["BKTLineId"].toString()) == -1){
					eDetailsRemoved.push(filterItem[0]["BKTLineId"]);
				}
			}
			if($(".rQuantity").length<=0){
				$("#save").attr("disabled",true);
			}
		// }

	});

	$("#add").click(function(e){

		var mProjectData = {};
		//hardcoded need to be changed...
		// var url="data/MGet_ItemList.json";
		var url = CONFIG.BASEPATH + "/MGet_ItemList"
		var companyUser = sessionStorage["user"];
		if(companyUser){
			companyUser = JSON.parse(companyUser);
		}

		var stockRdetails = JSON.parse(sessionStorage["stockRequestDetails"]);
		mProjectData["sCompany"] = companyUser["CompanyCode"];
		mProjectData["sProjectCode"] = stockRdetails["ProjectCode"];
		mProjectData["sWarehouseCode"] = stockRdetails["WhsCode"];
		mProjectData["dtRequiredDate"] = stockRdetails["ReqDate"];

		showLoading();
		postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
				
				var completeItems = data;
				sessionStorage["completeStockItems"] = JSON.stringify(data);
console.log(eProjectSpecific);
				var filterArray = completeItems.filter(function(obj){
					return eProjectSpecific.indexOf(obj["BKTLineId"].toString()) == -1;
				});
console.log(filterArray);
				$(".rItems").html("");
				if(filterArray.length<=0){

					$(".rItems").html("<div> There are no items to display</div>");
					$("#addItem").attr("disabled",true);
					$(".adHeader").hide();

				}
				else{
					$(filterArray).each(function(i,item){
						/*var htm = '<div class="list"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6">'+
							   '<span class="sname itemName">'+item["ItemCode"]+'</span>' + '<span class="tick" title="select" data-code="'+item["ItemCode"]+'"><i class="fa fa-square"></i></span>' +'<span class="sname1 itemDesc">'+item["Description"]+'</span>'+
							   '</div>' + '<div class="col-md-6 col-sm-6 col-xs-6 mrt-20 qty">'+
							   '<div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div><div class="uom">'+item["UOM"]+'</div>' +
							   '</div></div></div>';*/
						var htm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-1 col-xs-1 ticon">'+
									'<span class="tick" title="select" data-code="'+decodeURIComponent(item["ItemCode"])+'" data-lineid="'+item["BKTLineId"]+'"><i class="fa fa-square"></i></span>'+
									'</div><div class="col-md-6 col-sm-6 col-xs-3"><span class="addname itemName">'+item["ItemCode"]+'</span>' +
									'<span class="sdesc itemDesc">'+item["Description"]+'</span></div>' +
									'<div class="col-md-4 col-sm-4 col-xs-7 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + 
									'<span class="uom">'+decodeURIComponent(item["UOM"])+'</span></div></div></div>';
						$(".rItems").append(htm);
					});

					$("#addItem").attr("disabled",false);
				}

				showAddItems();
				hideLoading();
				
		});

	});

	$(".rItems").on('click','.tick',function(){
		var item = $(this).find("i");
		var itemCode = $(this).data("code");
		var lineId = $(this).data("lineid").toString();
		if($(item).hasClass("fa-square")){
			$(item).removeClass('fa-square').addClass("fa-check-square-o");
			eaddItems.push(lineId);
		}
		else{
			$(item).addClass('fa-square').removeClass("fa-check-square-o");
			if(eaddItems.indexOf(lineId) !=-1){
				eaddItems.splice(eaddItems.indexOf(lineId),1);

			}

		}

	});

	$("#addItem").click(function(){
		showLoading();
		var existingDetails = JSON.parse(sessionStorage.stockRequestDetails);
		var stockItems = JSON.parse(sessionStorage.completeStockItems);
		var filterArray = stockItems.filter(function(obj){
				return eaddItems.indexOf(obj["BKTLineId"])!=-1;
		});
		$(filterArray).each(function(i,item){

			var existingItem = existingDetails.Items.filter(function(obj){
								return obj.BKTLineId == item.BKTLineId;
								});
			var existingQty = "";
			if(existingItem.length>0){
				existingQty = existingItem[0]["Quantity"];
			}
			/*var htm = '<div class="list"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6">'+
					   '<span class="sname itemName">'+item["ItemCode"]+'</span>' + '<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'"><i class="fa fa-minus-circle"></i></span>' +'<span class="sname1 itemDesc">'+item["Description"]+'</span>'+
					   '</div>' + '<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">'+
					   '<div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + '<input type="text" size="1" class="rQuantity" data-code="'+item["ItemCode"]+'" data-quantity="0.00" value="0" />'+
					   '</div></div></div>';*/
					   var valextngQTY= " ";
						var chkexstngQty=parseFloat(existingQty || 0).toFixed(2);
						if(chkexstngQty==0.00)
						{
						// valextngQTY=null;
						}
						else
						{
							valextngQTY=chkexstngQty;
						}
			var htm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-1 col-xs-1 ricon">' +
					'<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'" data-lineid="'+item["BKTLineId"]+'"><i class="fa fa-minus-circle"></i></span>'+
					'</div><div class="col-md-6 col-sm-6 col-xs-3"><span class="addname itemName">'+item["ItemCode"]+'</span>' +
					'<span class="sdesc itemDesc">'+decodeURIComponent(item["Description"])+'</span></div>' +
					'<div class="col-md-4 col-sm-4 col-xs-7 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + 
					'<input type="tel" size="3.5" class="rQuantity itemqty textsize" data-code="'+decodeURIComponent(item["ItemCode"])+'" data-lineid="'+item["BKTLineId"]+'" data-quantity="'+(existingQty || 0.00)+'" value="'+valextngQTY+'" />' + '<span class="uom">'+decodeURIComponent(item["UOM"])+'</span></div></div></div>';

			$(".itemList").append(htm);
			eProjectSpecific.push(item["BKTLineId"]);
			eaddItems=[];
			if($("#save").is(":disabled")){
				$("#save").attr("disabled",false);
			}
		});

		showStock();
		hideLoading();

	});

	$("#cancelItem").click(function(){
		showStock();
	});

	$(".addItem").click(function(){
		showStock();
	});

	$(".itemList").on('keyup','.rQuantity',function(){

		validateQuantity($(this));
		
	});

	$("#cancel").click(function(){
		window.location.href = "view-stock.php";
	});

	$("#save").click(function(){

		// if(confirm("Confirm Save?")){

			checkQuantity(function(err){

			if(err != ""){
				alert(err);
			}
			else{

				if(confirm("Confirm Save?")){

					var mProjectData = {};
		
				var url = CONFIG.BASEPATH + "/MUpdate_StockRequest";
				var companyUser = sessionStorage["user"];
				if(companyUser){
					companyUser = JSON.parse(companyUser);
				}

				var reqDate = convertDate($("#form-field-1").val());

				mProjectData["sCompany"] = companyUser["CompanyCode"];
				mProjectData["sCurrentUserName"] = companyUser["UserName"];
				mProjectData["dtRequiredDate"] = reqDate;
				mProjectData["sProjectCode"] = $("#form-field-select-1").val();
				mProjectData["sWarehouseCode"] = $("#form-field-select-2").val();

				var temp = JSON.parse(sessionStorage.stockRequestDetails);

				mProjectData["sDocEntry"] = temp["DocEntry"];

				var stockItems="";
				var newlyAddedItems = [];

				/*if(typeof sessionStorage.completeStockItems != "undefined"){
					stockItems = JSON.parse(sessionStorage.completeStockItems);

				}
				else{*/
					stockItems = JSON.parse(sessionStorage.stockRequestDetails);
					stockItems = stockItems.Items;
				// }
				var filterArray = stockItems.filter(function(obj){
					
					return eProjectSpecific.indexOf(obj["BKTLineId"])!=-1;
				});

				for(i=0;i<eProjectSpecific.length;i++){

					var elementPos = stockItems.map(function(x) {return x.BKTLineId; }).indexOf(eProjectSpecific[i]);
					if(elementPos == -1){
						newlyAddedItems.push(eProjectSpecific[i]);
					}
				}

				if(newlyAddedItems.length > 0){
					var completeStockItems = JSON.parse(sessionStorage["completeStockItems"]);

					for(k=0;k<newlyAddedItems.length;k++){
						var elementPos = completeStockItems.map(function(x) {return x.BKTLineId; }).indexOf(newlyAddedItems[k]);
						var tempObj = $.extend(true, {}, completeStockItems[elementPos]);
						filterArray.push(tempObj);
					}

				}
				

				var givenStockItems = JSON.parse(sessionStorage.stockRequestDetails);
				givenStockItems = givenStockItems.Items;
				/*for(i=0;i<filterArray.length;i++){
					var itemCode = filterArray[i]["ItemCode"];

					var elementPos = givenStockItems.map(function(x) {return x.ItemCode; }).indexOf(itemCode);
					if(elementPos != -1){
						filterArray[i] = givenStockItems[elementPos];
					}
				}*/


				var rQuantity = {};
				var prevQuantity={};
				$(".rQuantity").each(function(i,q){
					var itemCode = $(this).data("code");
					var lineId = $(this).data("lineid");
					var quantity = $(this).val();
					rQuantity[lineId] = quantity;
					prevQuantity[lineId] = $(this).data("quantity");
				});

				var filterArrayCodes = filterArray.map(function(x) {return x.BKTLineId; });
				var givenStockItemsCodes = givenStockItems.map(function(x) {return x.BKTLineId; });

				for(i=0;i<givenStockItemsCodes.length;i++){

					var codeIndex = filterArrayCodes.indexOf(givenStockItemsCodes[i].toString());
					if(codeIndex == -1){
						givenStockItems[i]["DelFlag"] = "Y";
						
						givenStockItems[i]["ExistingPRQty"] = prevQuantity[givenStockItems[i]["BKTLineId"]] || givenStockItems[i]["Quantity"];
						givenStockItems[i]["Quantity"] = rQuantity[givenStockItems[i]["BKTLineId"]] || givenStockItems[i]["Quantity"]; 

						givenStockItems[i]["ItemName"] = givenStockItems[i]["Description"];
						// Delete unwanted fields

						delete givenStockItems[i]["Description"];
						delete givenStockItems[i]["IssueQty"];
						delete givenStockItems[i]["TransferQty"];
						delete givenStockItems[i]["sCurrentUserName"];
						
						filterArray.push(givenStockItems[i]);
					}
				}

				for(i=0; i<filterArray.length;i++){

					
					// extra fields. maybe changed in future.
					if(!filterArray[i].hasOwnProperty("DelFlag")){
						filterArray[i]["DelFlag"] = "N";
						filterArray[i]["ExistingPRQty"] = prevQuantity[filterArray[i]["BKTLineId"]];
						filterArray[i]["Quantity"] = rQuantity[filterArray[i]["BKTLineId"]]; 

						// append other fields with null value.
						filterArray[i]["BKTDocEntry"] = filterArray[i]["BKTDocEntry"] || "";
						filterArray[i]["BKTLineId"] = filterArray[i]["BKTLineId"] || "";
						// filterArray[i]["IssueQty"] = filterArray[i]["IssueQty"] || "";
						// filterArray[i]["TransferQty"] = filterArray[i]["TransferQty"] || "";
						// filterArray[i]["sCurrentUserName"] = filterArray[i]["sCurrentUserName"] || mProjectData["sCurrentUserName"];
						filterArray[i]["ItemName"] = filterArray[i]["Description"];
						// Delete unwanted fields

						delete filterArray[i]["Description"];
						delete filterArray[i]["IssueQty"];
						delete filterArray[i]["TransferQty"];
						delete filterArray[i]["sCurrentUserName"];

					}

				}

				// check for deleted items and give extra entry with flag Y
				for(j=0;j<eDetailsRemoved.length;j++){
					var itemCode = eDetailsRemoved[j];
					var lineId = eDetailsRemoved[j];
					var elementPos = filterArray.map(function(x) {return x.BKTLineId; }).indexOf(lineId.toString());
					// Deep copy objects without being referenced. Else it will affect changing in one object to other.
					var oldDeletedObj = $.extend(true, {}, filterArray[elementPos]);
					if(oldDeletedObj["DelFlag"] != "Y"){

						oldDeletedObj["DelFlag"] = "Y";
						filterArray.push(oldDeletedObj);

					}
					
				}


				mProjectData["Items"] = filterArray;

				console.log(JSON.stringify(mProjectData));

				showLoading();
				postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
						// console.log(data);
						if(data[0] && data[0].Result=="Success"){
							alert("PR document is updated successfully in SAP");
							window.location.href = "stock-request.php";
						}
						else{
							alert("Error ! Message: " + data[0].DisplayMessage);
						}

						hideLoading();
				});

				}

				

			}
		});

		// }
		
		

	});


	$(".aItem").click(function(){
		showStock();
	});


});

function fillStockListDetails(Stockdata,callBack){
	$("#form-field-1").val(showDate(Stockdata["ReqDate"]));
	$("#form-field-select-1").val(Stockdata["ProjectCode"]);
	$("#form-field-select-2").val(Stockdata["WhsCode"]);
	$(".itemList").html("");
	var status = Stockdata["Status"];
	$(Stockdata.Items).each(function(i,item){
		var htm;
		if(status == "Open" && parseFloat(item["TransferQty"] || "0.00").toFixed(2)== 0.00 && parseFloat(item["IssueQty"] || "0.00").toFixed(2)== 0.00){

			/*htm = '<div class="list"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6">'+
				   '<span class="sname itemName" data-docentry="'+item["BKTDocEntry"]+'" data-lineid="'+item["BKTLineId"]+'" data-unitprice="'+item["UnitPrice"]+'">'+item["ItemCode"]+'</span>' + '<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'"><i class="fa fa-minus-circle"></i></span>' +'<span class="sname1 itemDesc">'+item["Description"]+'</span>'+
				   '</div>' + '<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">'+
				   '<div class="items">'+ parseFloat(item["AvailableQty"] || "0.000").toFixed(2)+'</div>' + '<input type="text" size="1" class="rQuantity" data-code="'+item["ItemCode"]+'" data-quantity="'+ parseFloat(item["Quantity"] || "0.00").toFixed(2)+'" value="'+parseFloat(item["Quantity"] || "0.000").toFixed(2)+'" />'+
				   '</div></div></div>';*/

			htm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-1 col-xs-1 ricon">' +
					'<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'" data-lineid="'+item["BKTLineId"]+'"><i class="fa fa-minus-circle"></i></span>'+
					'</div><div class="col-md-6 col-sm-6 col-xs-3"><span class="addname itemName" data-unitprice="'+item["UnitPrice"]+'" data-docentry="'+item["BKTDocEntry"] +'"'+
					'data-lineid="'+item["BKTLineId"]+'" data-ocrcode="'+item["OcrCode2"]+'">'+item["ItemCode"]+'</span>' +
					'<span class="sdesc itemDesc">'+decodeURIComponent(item["Description"])+'</span></div>' +
					'<div class="col-md-4 col-sm-4 col-xs-7 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + 
					'<input type="tel" size="3.5" class="rQuantity itemqty textsize" data-code="'+decodeURIComponent(item["ItemCode"])+'" data-lineid="'+decodeURIComponent(item["BKTLineId"])+'" data-quantity="'+ parseFloat(item["Quantity"] || "0.00").toFixed(2)+'" value="'+parseFloat(item["Quantity"] || "0.000").toFixed(2)+'" />' + '<span class="uom">'+decodeURIComponent(item["UOM"])+'</span></div></div></div>';

		}
		else{
			/*htm = '<div class="list"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6">'+
				   '<span class="sname itemName" data-docentry="'+item["BKTDocEntry"]+'" data-lineid="'+item["BKTLineId"]+'" data-unitprice="'+item["UnitPrice"]+'">'+item["ItemCode"]+'</span>' +'<span class="sname1 itemDesc">'+item["Description"]+'</span>'+
				   '</div>' + '<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">'+
				   '<div class="items">'+ parseFloat(item["AvailableQty"] || "0.000").toFixed(2)+'</div>' + '<input type="text" size="1" class="rQuantity" disabled data-code="'+item["ItemCode"]+'" data-quantity="'+ parseFloat(item["Quantity"] || "0.00").toFixed(2)+'" value="'+parseFloat(item["Quantity"] || "0.000").toFixed(2)+'" />'+
				   '</div></div></div>';*/

			htm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-1 col-xs-1"></div>' +
					'<div class="col-md-6 col-sm-6 col-xs-3"><span class="addname itemName" data-unitprice="'+item["UnitPrice"]+'" data-docentry="'+item["BKTDocEntry"] +'"'+
					'data-lineid="'+item["BKTLineId"]+'" data-ocrcode="'+item["OcrCode2"]+'">'+item["ItemCode"]+'</span>' +
					'<span class="sdesc itemDesc">'+decodeURIComponent(item["Description"])+'</span></div>' +
					'<div class="col-md-4 col-sm-4 col-xs-7 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' +
					 '<input type="tel" size="3.5" class="rQuantity itemqty textsize" disabled data-code="'+decodeURIComponent(item["ItemCode"])+'" data-lineid="'+ item["BKTLineId"] +'" data-quantity="'+ parseFloat(item["Quantity"] || "0.00").toFixed(2)+'" value="'+parseFloat(item["Quantity"] || "0.000").toFixed(2)+'" />' + '<span class="uom">'+decodeURIComponent(item["UOM"])+'</span></div></div></div>'
		}
		
		$(".itemList").append(htm);
		eProjectSpecific.push(item["BKTLineId"]);
	});

	/*var itemHtm =   '<div class="list"><table class="table-responsive"><tr>' + '<td class="minus"><i class="fa fa-minus-circle"></i></td>'+
				   '<td class="sname">'+Stockdata["ItemCode"]+'</td>' + '<td class="items pull-left availQ">'+ parseInt(Stockdata["AvailableQty"])+'</td>' + '<td class="reqQ"><input type="text" size="1" value="'+parseInt(Stockdata["Quantity"])+'" class="qty" /></td>' +
				   '</tr><tr><td></td>' + '<td class="sname1">'+Stockdata["Description"]+'</td>' + '<td></td>' + '<td></td>' +
				   '</tr></table></div>';*/

			   
								
	callBack("DONE");
}

function getValidData(Stockdata,callBack){

	var url = CONFIG.BASEPATH + "/MGet_Project";
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}

	//var reqDate = convertDate($("#form-field-1").val());
	mProjectData["sCompany"] = companyUser["CompanyCode"];
	mProjectData["sCurrentUserName"] = companyUser["UserName"];
	mProjectData["dtRequiredDate"] = Stockdata["ReqDate"];
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
		var opt ='<option value="">Select</option>';
		$(data).each(function(i,project){
							  
			var proj_name = project["PrcName"];
			var proj_name_decode = decodeURIComponent(proj_name);
			opt += '<option value="'+project["PrcCode"]+'">'+proj_name_decode+'</option>';

		});
		$("#form-field-select-1").html(opt);
		url = CONFIG.BASEPATH + "/MGet_WareHouse";
		mProjectData={};
		mProjectData["sCompany"] = companyUser["CompanyCode"];
		console.log(Stockdata["ProjectCode"]);
		mProjectData["sProjectCode"] = Stockdata["ProjectCode"];
		
		postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
				var opt ='<option value="">Select</option>';
				$(data).each(function(i,warehouse){
									  
					var warehouse_name = warehouse["WhsName"];
					var warehouse_name_decode = decodeURIComponent(warehouse_name);
					opt += '<option value="'+warehouse["WhsCode"]+'">'+warehouse_name_decode+'</option>';

				});
				$("#form-field-select-2").html(opt);

				fillStockListDetails(Stockdata,function(res){
					callBack("DONE");
				})
		});

	});




}

function getStockDetailsData(callBack){
	// var url = CONFIG.BASEPATH + "/MGet_StockRequestDetails";
	// Edit so new service.
	var url = CONFIG.BASEPATH + "/MGet_EditStockRequestDetails";

	var docEntry = sessionStorage.docEntry;
	var mProjectData = {};
	var companyUser = sessionStorage["user"];
	if(companyUser){
		companyUser = JSON.parse(companyUser);
	}
	mProjectData["sCompany"] =  companyUser["CompanyCode"];
	mProjectData["sDocEntry"] = docEntry;
	postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
			sessionStorage["stockRequestDetails"] = JSON.stringify(data[0]);
			console.log(data[0]);
			getValidData(data[0],function(res){
				callBack("DONE");
			});		
			
	});
}

var eremoveItems = [];
var eaddItems=[];
var eProjectSpecific=[];
var eDetailsRemoved = [];

function showStock(){
	$("#eStockItems").show();
	$("#remove-list").hide();
	$(".eItem").show();
	$(".aItem").hide();
}

function showAddItems(){
	$("#eStockItems").hide();
	$("#remove-list").show();
	$(".eItem").hide();
	$(".aItem").show();
}
function validateQuantity($obj){
	// New logic for edit ..
	// Trim not supporting in updated chrome. Changing logic
	// var existingQty = $obj.data("quantity").trim();
	var existingQty = $obj.data("quantity");
	if(isNaN($obj.val().trim())){
		alert("Please enter valid quantity");

		$obj.val(existingQty);
	}
	else if($obj.val().trim() != "" && parseFloat($obj.val().trim())<0){
		alert("Please enter positive quantity");
		$obj.val(existingQty);
	}
	else{

		var reqQuantity = parseFloat($obj.val().trim() || "0");
		var availQauntity = parseFloat(parseFloat($obj.parent().find(".items").text().trim() || "0").toFixed(2));
			existingQty = parseFloat(parseFloat(existingQty).toFixed(2));

		if((reqQuantity - existingQty) > availQauntity){
			alert("Please reduce quanity. Max is "+(existingQty+availQauntity));
			$obj.val(existingQty);
		}

	}

}

function checkQuantity(callBack){
	var errorMsg = "";

	$(".rQuantity").each(function(i,q){
			if($(this).val() == "" || parseFloat($(this).val()) <= "0"){
				errorMsg = "Quantity must be greater than 0";
				return false;

			}
			// Trim not supporting in updated chrome. Changing logic
			// var existingQty = $(this).data("quantity").trim();
			var existingQty = $(this).data("quantity");
			var reqQuantity = parseFloat($(this).val().trim() || "0");
			var availQauntity = parseFloat(parseFloat($(this).parent().find(".items").text().trim() || "0").toFixed(2));
			existingQty = parseFloat(parseFloat(existingQty).toFixed(2));

			if((reqQuantity - existingQty) > availQauntity){
				errorMsg = "Please reduce quanity. Max is "+(existingQty+availQauntity);
				$(this).val(existingQty);
				return false;
			}
	});

	callBack(errorMsg);


}