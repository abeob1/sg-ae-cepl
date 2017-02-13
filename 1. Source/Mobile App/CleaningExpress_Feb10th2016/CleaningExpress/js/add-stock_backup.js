 var datet = "";
$(document).ready(function(){

	// initialize date picker to current date as minimum

	//$("#form-field-1").attr("minDate",getToday());
	 $('#form-field-1').datepicker({
        format: "mm/dd/yyyy"
    })

	// disable add item and save button on load.
	$("#add,#save").attr("disabled", true);

	
	//Listen for the change even on the input

.on('changeDate', dateChanged);
	
function dateChanged(ev){
		datet = $('#form-field-1').val();
		var mProjectData = {};
		//hardcoded need to be changed...
		//var url="data/MGet_Project.json";
		if(datet != ""){
			
		//To compare only date part
		var myDate  = new Date(datet).setHours(0,0,0,0);
		var todaysdate=new Date(getToday());
		var today = todaysdate.setHours(0,0,0,0);
		//To Compare only Date Part
		

			if (myDate >= today) {

                if (datet.split("/")[2].length == 4) {

				var url = CONFIG.BASEPATH + "/MGet_Project";
				var companyUser = sessionStorage["user"];
				if(companyUser){
					companyUser = JSON.parse(companyUser);
				}

				var reqDate = convertDate($("#form-field-1").val());

				mProjectData["sCompany"] = companyUser["CompanyCode"];
				mProjectData["sCurrentUserName"] = companyUser["UserName"];
				mProjectData["dtRequiredDate"] = reqDate;

				showLoading();
				postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
						var opt ='<option value="">Select</option>';
						if(data && data[0] && data[0].PrcCode){

							$(data).each(function(i,project){
								opt += '<option value="'+project["PrcCode"]+'">'+project["PrcName"]+'</option>';

							});
							$("#form-field-select-1").html(opt);

						}
						else{
							// alert("Please select valid date");
						}
						

						// Reset warehouse & items
						$("#form-field-select-2").html("");
						$(".projectList").html("");

						hideLoading();
				});

			}
			else{
				// alert("Date should not be less than today");
			}

			

			}
			else{

				// Reset warehouse & items
					$("#form-field-select-1").html("");
					$("#form-field-select-2").html("");
					$(".projectList").html("");
					$("#add,#save").attr("disabled", true);
				// alert("Please select valid date");

			}

			

		}
		else{
			// Reset warehouse & items
					$("#form-field-select-1").html("");
					$("#form-field-select-2").html("");
					$(".projectList").html("");
					$("#add,#save").attr("disabled", true);
			// alert("Please select valid date");
		}
		

	});

	$("#form-field-select-1").change(function(){

		if($(this).val() != ""){

			var mProjectData = {};
			//hardcoded need to be changed...
			// var url="data/MGet_WareHouse.json";
			var url = CONFIG.BASEPATH + "/MGet_WareHouse";
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}


			mProjectData["sCompany"] = companyUser["CompanyCode"];
			mProjectData["sProjectCode"] = $(this).val();

			showLoading();
			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
					var opt ='<option value="">Select</option>';
					$(data).each(function(i,warehouse){
						opt += '<option value="'+warehouse["WhsCode"]+'">'+warehouse["WhsName"]+'</option>';

					});
					$("#form-field-select-2").html(opt);

					// Reset items
					$(".projectList").html("");

					hideLoading();
			});

		}
		else{
			// Reset warehouse & items
					$("#form-field-select-2").html("");
					$(".projectList").html("");
					$("#add,#save").attr("disabled", true);
		}

		

	});

	$("#form-field-select-2").change(function(){

		if($(this).val() != ""){

			var mProjectData = {};
			//hardcoded need to be changed...
			// var url="data/MGet_ItemList.json";
			var url = CONFIG.BASEPATH + "/MGet_ItemList"
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}
			var reqDate = convertDate($("#form-field-1").val());

			mProjectData["sCompany"] = companyUser["CompanyCode"];
			mProjectData["sProjectCode"] = $("#form-field-select-1").val();
			mProjectData["sWarehouseCode"] = $(this).val();
			mProjectData["dtRequiredDate"] = reqDate;

			showLoading();
			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
					$(".projectList").html("");
					if(data.length>0){

						$(data).each(function(i,item){
						if(parseFloat(item["AvailableQty"] || "0.00").toFixed(2) > 0){

							/*var htm = '<div class="list"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6">'+
								   '<span class="sname itemName" data-unitprice="'+item["UnitPrice"]+'" data-docentry="'+item["BKTDocEntry"]+'" data-lineid="'+item["BKTLineId"]+'" data-ocrcode="'+item["OcrCode2"]+'">'+item["ItemCode"]+'</span>' + '<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'"><i class="fa fa-minus-circle"></i></span>' +'<span class="sname1 itemDesc">'+item["Description"]+'</span>'+
								   '</div>' + '<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">'+
								   '<div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + '<input type="text" size="1" class="rQuantity" data-code="'+item["ItemCode"]+'" value="0" />'+
								   '</div></div></div>';*/

							/*var htm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-2 col-xs-2 ricon">' +
									'<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'"><i class="fa fa-minus-circle"></i></span>' +
									'</div><div class="col-md-6 col-sm-6 col-xs-4">' +
									'<span class="addname itemName" data-unitprice="'+item["UnitPrice"]+'" data-docentry="'+item["BKTDocEntry"]+
									'" data-lineid="'+item["BKTLineId"]+'" data-ocrcode="'+item["OcrCode2"]+'">'+item["ItemCode"]+'</span>' +
									'<span class="sdesc itemDesc">'+item["Description"]+'</span></div>' + '<div class="col-md-2 col-sm-2 col-xs-6 quantity">' +
									'<div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + '</div><div class="col-md-2 col-sm-2 col-xs-6 quantity">' +
									'<input type="text" size="1" class="rQuantity" data-code="'+item["ItemCode"]+'" value="0" />' +
									'</div></div></div>';*/

							var htm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-1 col-xs-1 ricon">' +
									'<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'"><i class="fa fa-minus-circle"></i></span>'+
									'</div><div class="col-md-6 col-sm-6 col-xs-3"><span class="addname itemName" data-unitprice="'+item["UnitPrice"]+'" data-docentry="'+item["BKTDocEntry"] +'"'+
									'data-lineid="'+item["BKTLineId"]+'" data-ocrcode="'+item["OcrCode2"]+'">'+item["ItemCode"]+'</span>' +
									'<span class="sdesc itemDesc">'+item["Description"]+'</span></div>' +
									'<div class="col-md-4 col-sm-4 col-xs-7 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + 
									'<input type="tel" size="3.5" class="rQuantity itemqty textsize" data-code="'+item["ItemCode"]+'" value="" />' + '<span class="uom">'+item["UOM"]+'</span></div></div></div>';
							
							if(parseFloat(item["AvailableQty"] || "0.00") > 0){
								
								$(".projectList").append(htm);
							}
							
							sProjectSpecific.push(item["ItemCode"]);

						}
						
					});
					sessionStorage.stockItems = JSON.stringify(data);
					hideLoading();
					// Enable buttons
					$("#add,#save").attr("disabled", false);

					}
					else{
						hideLoading();
						$(".projectList").html("");
						$("#add,#save").attr("disabled", true);

					}
					
					
			});

		}
		else{

			$(".projectList").html("");
			$("#add,#save").attr("disabled", true);

		}

		

	});

	$(".projectList").on('click','.minus',function(){

		// if(confirm("Are you sure you want to remove the item ?")){
			var itemCode = $(this).data("code");
			removeItems.push(itemCode);
			$(this).parents(":eq(2)").remove();
			var index = sProjectSpecific.indexOf(itemCode);
			if(index != -1){
				sProjectSpecific.splice(index,1);
			}

			if($(".rQuantity").length<=0){
				$("#save").attr("disabled",true);
			}
		// }

	});

	$("#add").click(function(e){

		
		showLoading();
		var stockItems = JSON.parse(sessionStorage.stockItems);
		var filterArray = stockItems.filter(function(obj){
			return removeItems.indexOf(obj["ItemCode"])!=-1;
		});

		$(".rItems").html("");


		if(removeItems.length<=0){
			$(".rItems").html("<div> There are no items to display</div>");
			$("#addItem").attr('disabled',true);
			$(".adHeader").hide();
		}
		else{

			$(filterArray).each(function(i,item){
				/*var htm = '<div class="list"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6">'+
							   '<span class="sname itemName">'+item["ItemCode"]+'</span>' + '<span class="tick" title="select" data-code="'+item["ItemCode"]+'"><i class="fa fa-square"></i></span>' +'<span class="sname1 itemDesc">'+item["Description"]+'</span>'+
							   '</div>' + '<div class="col-md-6 col-sm-6 col-xs-6 mrt-20 qty">'+
							   '<div class="items">'+parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div><div class="uom">'+item["UOM"]+'</div>' +
							   '</div></div></div>';*/
				var htm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-1 col-xs-1 ticon">'+
					'<span class="tick" title="select" data-code="'+item["ItemCode"]+'"><i class="fa fa-square"></i></span>'+
					'</div><div class="col-md-6 col-sm-6 col-xs-3"><span class="addname itemName">'+item["ItemCode"]+'</span>' +
					'<span class="sdesc itemDesc">'+item["Description"]+'</span></div>' +
					'<div class="col-md-4 col-sm-4 col-xs-7 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + 
					'<span class="uom">'+item["UOM"]+'</span></div></div></div>';
				$(".rItems").append(htm);
			});

			$("#addItem").attr('disabled',false);

		}

		
		showAddItems();
		hideLoading();
		
	});

	$(".rItems").on('click','.tick',function(){
		var item = $(this).find("i");
		var itemCode = $(this).data("code");
		if($(item).hasClass("fa-square")){
			$(item).removeClass('fa-square').addClass("fa-check-square-o");
			addItems.push(itemCode);
			removeItems.splice(removeItems.indexOf(itemCode),1);
		}
		else{
			$(item).addClass('fa-square').removeClass("fa-check-square-o");
			if(addItems.indexOf(itemCode) !=-1){
				addItems.splice(addItems.indexOf(itemCode),1);
				removeItems.push(itemCode);

			}

		}

	});

	$("#addItem").click(function(){
		showLoading();
		var stockItems = JSON.parse(sessionStorage.stockItems);
		var filterArray = stockItems.filter(function(obj){
				return addItems.indexOf(obj["ItemCode"])!=-1;
		});
		$(filterArray).each(function(i,item){
			/*var htm = '<div class="list"><div class="row"><div class="col-md-6 col-sm-6 col-xs-6">'+
					   '<span class="sname itemName">'+item["ItemCode"]+'</span>' + '<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'"><i class="fa fa-minus-circle"></i></span>' +'<span class="sname1 itemDesc">'+item["Description"]+'</span>'+
					   '</div>' + '<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">'+
					   '<div class="items">'+parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + '<input type="text" size="1" class="rQuantity" data-code="'+item["ItemCode"]+'" value="0" />'+
					   '</div></div></div>';*/
			var htm = '<div class="list"><div class="row"><div class="col-md-2 col-sm-1 col-xs-1 ricon">' +
						'<span class="minus" title="Remove" data-code="'+item["ItemCode"]+'"><i class="fa fa-minus-circle"></i></span>'+
						'</div><div class="col-md-6 col-sm-6 col-xs-3"><span class="addname itemName">'+item["ItemCode"]+'</span>' +
						'<span class="sdesc itemDesc">'+item["Description"]+'</span></div>' +
						'<div class="col-md-4 col-sm-4 col-xs-7 quantity"><div class="items">'+ parseFloat(item["AvailableQty"] || "0.00").toFixed(2)+'</div>' + 
						'<input type="tel" size="3.5" class="rQuantity itemqty textsize" data-code="'+item["ItemCode"]+'" value="" />' + '<span class="uom">'+item["UOM"]+'</span></div></div></div>'

			$(".projectList").append(htm);
			sProjectSpecific.push(item["ItemCode"]);
			addItems=[];
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

	$("#cancel").click(function(){
		//Reset all the fields
		/*$("#form-field-1").val("");
		$("#form-field-select-1").html("");
		$("#form-field-select-2").html("");
		$(".projectList").html("");
		$("#add,#save").attr("disabled", true);*/
		window.location.href = "stock-request.php";

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
		
				var url = CONFIG.BASEPATH + "/MSave_StockRequest";
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

				var stockItems = JSON.parse(sessionStorage.stockItems);
				var filterArray = stockItems.filter(function(obj){
					return sProjectSpecific.indexOf(obj["ItemCode"])!=-1;
				});

				var rQuantity = {};
				$(".rQuantity").each(function(i,q){
					var itemCode = $(this).data("code");
					var quantity = $(this).val();
					rQuantity[itemCode] = quantity;
				});

				for(i=0; i<filterArray.length;i++){
					filterArray[i]["Quantity"] = rQuantity[filterArray[i]["ItemCode"]]; 
					// extra fields. maybe changed in future.
					/*filterArray[i]["BKTDocEntry"] = filterArray[i]["DocEntry"];
					filterArray[i]["BKTLineId"] = filterArray[i]["LineId"];*/
				}

				mProjectData["Items"] = filterArray;

				console.log(JSON.stringify(mProjectData));

				showLoading();
				postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
						// console.log(data);
						if(data[0] && data[0].Result=="Success"){
							alert("PR document is created successfully in SAP");
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

	$(".projectList").on('keyup','.rQuantity',function(){

		validateQuantity($(this));
		
	});

	

});

var removeItems = [];
var addItems=[];
var sProjectSpecific=[];

function showStock(){
	$("#stock-list").show();
	$("#remove-list").hide();
	$(".addStock").show();
	$(".addItem").hide();
}

function showAddItems(){
	$("#stock-list").hide();
	$("#remove-list").show();
	$(".addStock").hide();
	$(".addItem").show();
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
	});

	callBack(errorMsg);


}

