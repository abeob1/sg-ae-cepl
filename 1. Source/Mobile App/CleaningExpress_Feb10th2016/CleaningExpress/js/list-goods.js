$(document).ready(function(){

	$(".goodsItems").on('click','.minus',function(){

		// if(confirm("Are you sure you want to remove the item ?")){
			var itemCode = $(this).data("code");
			var lineId = $(this).data("lineid").toString();
			gRemoveItems.push($(this).data("lineid").toString());
			$(this).parents(":eq(2)").remove();
			console.log(gRemoveItems);
			var index = gProjectSpecific.indexOf(lineId);
			console.log(index);
			if(index != -1){
				gProjectSpecific.splice(index,1);
			}

			if($(".rQuantity").length<=0){
				$("#save").attr("disabled",true);
			}
		// }

	});


	$("#add").click(function(e){

		/*if(gRemoveItems.length<=0){
			alert("There are no items to add");
		}
		else{*/
			showLoading();
			var goodsItems = JSON.parse(sessionStorage.goodsList);
			
			
			var url = CONFIG.BASEPATH + "/MGet_GoodsIssueItemList";
			var mProjectData = {};
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}
			mProjectData["sCompany"] =  companyUser["CompanyCode"];
			mProjectData["sProjectCode"] = goodsItems["ProjectCode"];
			mProjectData["sWarehouseCode"] = goodsItems["WhsCode"];
			mProjectData["dtRequiredDate"] = goodsItems["ReqDate"];

			mProjectData["sDocEntry"] = sessionStorage["gDocEntry"];
			showLoading();
			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
					
				var goodsItemList = data;
				sessionStorage["completeGoodsItems"] = JSON.stringify(data);

				// Need to work. show all items except already shown in list

				var filterArray = goodsItemList.filter(function(obj){
					return gProjectSpecific.indexOf(obj["BKTLineId"].toString()) == -1;
				});

				$(".rItems").html("");
				if(filterArray.length<=0){

					$(".rItems").html("<div> There are no items to display</div>");
					$("#addItems").hide();
					$(".adHeader").hide();

				}
				else{
					$(filterArray).each(function(i,item){
						/*var htm = '<div class="list"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6">'+
									   '<span class="sname itemName">'+item["ItemCode"]+'</span>' + '<span class="tick" title="select" data-code="'+item["ItemCode"]+'"><i class="fa fa-square"></i></span>' +'<span class="sname1 itemDesc">'+item["Description"]+'</span>'+
									   '</div>' + '<div class="col-md-6 col-sm-6 col-xs-6 mrt-20 qty">'+
									   '<div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + '</div></div></div>';*/
						var htm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-1 col-xs-1 ticon">'+
									'<span class="tick" title="select" data-code="'+decodeURIComponent(item["ItemCode"])+'" data-lineid="'+item["BKTLineId"]+'"><i class="fa fa-square"></i></span>'+
									'</div><div class="col-md-6 col-sm-6 col-xs-3"><span class="addname itemName">'+item["ItemCode"]+'</span>' +
									'<span class="sdesc itemDesc">'+decodeURIComponent(item["Description"])+'</span></div>' +
									'<div class="col-md-4 col-sm-4 col-xs-7 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + 
									'<span class="uom">'+decodeURIComponent(item["UOM"])+'</span></div></div></div>';
						$(".rItems").append(htm);
					});
				}

				
				showAddItems();
				hideLoading();


			});

		
		//}
	});

	$(".rItems").on('click','.tick',function(){
		var item = $(this).find("i");
		var itemCode = $(this).data("code");
		var lineId = $(this).data("lineid").toString();
		if($(item).hasClass("fa-square")){
			$(item).removeClass('fa-square').addClass("fa-check-square-o");
			gAddItems.push(lineId);
		}
		else{
			$(item).addClass('fa-square').removeClass("fa-check-square-o");
			if(gAddItems.indexOf(lineId) !=-1){
				gAddItems.splice(gAddItems.indexOf(lineId),1);

			}

		}

	});

	$("#addItem").click(function(){
		showLoading();
		var goodsItems = JSON.parse(sessionStorage.completeGoodsItems);
		
		var filterArray = goodsItems.filter(function(obj){
				return gAddItems.indexOf(obj["BKTLineId"].toString())!=-1;
		});
		$(filterArray).each(function(i,item){
			/*var htm = '<div class="list"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6">'+
					   '<span class="sname itemName">'+item["ItemCode"]+'</span>' + '<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'"><i class="fa fa-minus-circle"></i></span>' +'<span class="sname1 itemDesc">'+item["Description"]+'</span>'+
					   '</div>' + '<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">'+
					   '<div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + '<input type="text" size="1" class="rQuantity" data-code="'+item["ItemCode"]+'" data-quantity="0" value="0" />'+
					   '</div></div></div>';*/

			var htm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-1 col-xs-1 ricon">' +
						'<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'" data-lineid="'+item["BKTLineId"]+'"><i class="fa fa-minus-circle"></i></span>'+
						'</div><div class="col-md-6 col-sm-6 col-xs-3"><span class="addname itemName">'+item["ItemCode"]+'</span>' +
						'<span class="sdesc itemDesc">'+decodeURIComponent(item["Description"])+'</span></div>' +
						'<div class="col-md-4 col-sm-4 col-xs-7 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + 
						'<input type="tel" size="3.5" class="rQuantity itemqty textsize" data-code="'+decodeURIComponent(item["ItemCode"])+'"  data-lineid="'+item["BKTLineId"]+'" data-quantity="0" value="" />' + '<span class="uom">'+decodeURIComponent(item["UOM"])+'</span></div></div></div>';

			$(".goodsItems").append(htm);
			gProjectSpecific.push(item["BKTLineId"]);
			gAddItems=[];

			if($("#save").is(":disabled")){
				$("#save").attr("disabled",false);
			}
		});

		showGoods();
		hideLoading();

	});

	$("#cancelItem").click(function(){
		showGoods();
	});

	$(".addItem").click(function(){
		showGoods();
	});

	$(".goodsItems").on('keyup','.rQuantity',function(){

		validateQuantity($(this));
		
	});

	$("#cancel").click(function(){

		window.location.href = "list-project.php";

	});









	$("#save").click(function(){

		// if(confirm("Once saved, the request cannot be modified anymore.  Confirm save?")){

			checkQuantity(function(err){
				if(err != ""){
					alert(err);
				}
				else{
					if(confirm("Once saved, the request cannot be modified anymore.  Confirm save?")){
						var mProjectData = {};
				
						var url = CONFIG.BASEPATH + "/MSave_GoodsIssue";
						var companyUser = sessionStorage["user"];
						if(companyUser){
							companyUser = JSON.parse(companyUser);
						}

						var goodsList = JSON.parse(sessionStorage["goodsList"]);

						mProjectData["sCompany"] = companyUser["CompanyCode"];
						mProjectData["sCurrentUserName"] = companyUser["UserName"];
						mProjectData["dtRequiredDate"] = goodsList["ReqDate"];
						mProjectData["sProjectCode"] = goodsList["ProjectCode"];
						mProjectData["sWarehouseCode"] = goodsList["WhsCode"];

						mProjectData["sDocEntry"] = goodsList["DocEntry"];

						var goodsItems="";
						var newlyAddedItems = [];
						/*if(typeof sessionStorage.completeGoodsItems != "undefined"){
							goodsItems = JSON.parse(sessionStorage.completeGoodsItems);

						}*/
						// else{
							goodsItems = goodsList;
							goodsItems = goodsList.Items;
						// }
						var filterArray = goodsItems.filter(function(obj){
							return gProjectSpecific.indexOf(obj["BKTLineId"].toString())!=-1;
						});

						for(i=0;i<gProjectSpecific.length;i++){

							var elementPos = goodsItems.map(function(x) {return x.BKTLineId; }).indexOf(gProjectSpecific[i]);
							if(elementPos == -1){
								newlyAddedItems.push(gProjectSpecific[i]);
							}
						}

						if(newlyAddedItems.length > 0){
							var completeGoodsItems = JSON.parse(sessionStorage["completeGoodsItems"]);

							for(k=0;k<newlyAddedItems.length;k++){
								var elementPos = completeGoodsItems.map(function(x) {return x.BKTLineId; }).indexOf(newlyAddedItems[k]);
								var tempObj = $.extend(true, {}, completeGoodsItems[elementPos]);
								filterArray.push(tempObj);
							}

						}

						/*var givenGoodsItems = goodsList;
						givenGoodsItems = givenGoodsItems.Items;
						for(i=0;i<filterArray.length;i++){
							var itemCode = filterArray[i]["ItemCode"];

							var elementPos = givenGoodsItems.map(function(x) {return x.ItemCode; }).indexOf(itemCode);
							if(elementPos != -1){
								filterArray[i] = givenGoodsItems[elementPos];
							}
						}

						var filterArrayCodes = filterArray.map(function(x) {return x.ItemCode; });
						var givenGoodsItemsCodes = givenGoodsItems.map(function(x) {return x.ItemCode; });

						for(i=0;i<givenGoodsItemsCodes.length;i++){

							var codeIndex = filterArrayCodes.indexOf(givenGoodsItemsCodes[i]);
							if(codeIndex == -1){
								givenGoodsItems[i]["DelFlag"] = "Y";
								filterArray.push(givenGoodsItems[i]);
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

						for(i=0; i<filterArray.length;i++){

							
							// extra fields. maybe changed in future.
							// if(!filterArray[i].hasOwnProperty("DelFlag")){
								// filterArray[i]["DelFlag"] = "N";
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

							// }

						}

						mProjectData["Items"] = filterArray;

						console.log(JSON.stringify(mProjectData));

						showLoading();
						postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
								// console.log(data);
								if(data[0] && data[0].Result=="Success"){
									alert("Goods Issue document is created successfully in SAP");
									window.location.href = "list-project.php";
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

});

loadGoodsDetails();

function loadGoodsDetails(){
	var url = CONFIG.BASEPATH + "/MGet_GoodsIssueDetails";
	var docEntry = sessionStorage.gDocEntry;
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
				var goods = data[0];
				sessionStorage["goodsList"] = JSON.stringify(goods);
				$("#goodsDetail .desc1").text(docEntry);

				// Hardcoded for items as response should change.

				/*var itemHtm = '<div class="view"><table class="table-responsive">' + 
								'<tr><td class="vname">'+goods["ItemCode"]+'</td>' + '<td></td>'+ '</tr><tr>' +
								'<td class="vname1">'+goods["Description"]+'</td>' + '<td class="vname2">'+ parseInt(goods["AvailableQty"])+'</td>' +
								'</tr></table></div>';
				$(".itemDetails").html(itemHtm);*/

				//New logic...

				
				$(".goodsItems").html("");
				
				$(goods.Items).each(function(i,item){
					/*var itemHtm = '<div class="list"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6">'+
							   '<span class="sname itemName">'+item["ItemCode"]+'</span>' + '<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'"><i class="fa fa-minus-circle"></i></span>' +'<span class="sname1 itemDesc">'+item["Description"]+'</span>'+
							   '</div>' + '<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">'+
							   '<div class="items">'+parseFloat(item["AvailableQty"] || "0.000").toFixed(2)+'</div>' + '<input type="text" size="1" class="rQuantity" data-code="'+item["ItemCode"]+'" data-quantity="'+(item["Quantity"] || "0")+'" value="'+parseFloat(item["Quantity"] || "0.000").toFixed(2)+'" />'+
							   '</div></div></div>';*/

					/*var itemHtm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-2 col-xs-2 ricon">' +
									'<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'"><i class="fa fa-minus-circle"></i></span>'+
									'</div><div class="col-md-6 col-sm-6 col-xs-4"><span class="addname itemName">'+item["ItemCode"]+'</span>' +
									'<span class="sdesc itemDesc">'+item["Description"]+'</span></div>' +
									'<div class="col-md-4 col-sm-4 col-xs-6 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + 
									'<input type="text" size="1" class="rQuantity itemqty" data-code="'+item["ItemCode"]+'" data-quantity="'+(item["Quantity"] || "0")+'" value="'+parseFloat(item["Quantity"] || "0.000").toFixed(2)+'" />' + 
									'</div></div></div>';*/
						var itemHtm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-1 col-xs-1 ricon">' +
									'<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'" data-lineid="'+item["BKTLineId"]+'"><i class="fa fa-minus-circle"></i></span>'+
									'</div><div class="col-md-6 col-sm-6 col-xs-3"><span class="addname itemName">'+item["ItemCode"]+'</span>' +
									'<span class="sdesc itemDesc">'+decodeURIComponent(item["Description"])+'</span></div>' +
									'<div class="col-md-4 col-sm-4 col-xs-7 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + 
									'<input type="tel" size="3.5" class="rQuantity itemqty textsize" data-code="'+decodeURIComponent(item["ItemCode"])+'" data-lineid="'+item["BKTLineId"]+'" data-quantity="'+(item["Quantity"] || "0")+'" value="" />' + 
									'<span class="uom">'+decodeURIComponent(item["UOM"])+'</span></div></div></div>';

					$(".goodsItems").append(itemHtm);
					gProjectSpecific.push(item["BKTLineId"]);
				});

			}

			else{
				$(".goodsItems").html("<div> No Details Found</div>");
			}
			hideLoading();
			
	});
}


var gRemoveItems = [];
var gAddItems=[];
var gProjectSpecific=[];

function showGoods(){
	$("#goodsView").show();
	$("#itemView").hide();
	$(".addGoods").show();
	$(".addItem").hide();
	$(".gHeader").show();
}

function showAddItems(){
	$("#goodsView").hide();
	$("#itemView").show();
	$(".addGoods").hide();
	$(".addItem").show();
	$(".gHeader").hide();
}

function validateQuantity($obj){

	if(isNaN($obj.val().trim())){
		alert("Please enter valid quantity");
		$obj.val("0");
	}
	else if($obj.val().trim() != "" && parseFloat($obj.val().trim())<0){
		alert("Please enter positive quantity");
	}
	else{

		var reqQuantity = parseFloat($obj.val().trim() || "0");
		var availQauntity = parseFloat($obj.parent().find(".items").text().trim() || "0");
		if(reqQuantity > availQauntity){
			alert("Should not be greater than Available quantity");
			$obj.val("0");
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

			var reqQuantity = parseFloat($(this).val().trim() || "0");
			var availQauntity = parseFloat($(this).parent().find(".items").text().trim() || "0");
			if(reqQuantity > availQauntity){
				errorMsg = "Quantity should not be greater than Available quantity";
				$(this).val("0");
				return false;
			}
	});

	callBack(errorMsg);


}
