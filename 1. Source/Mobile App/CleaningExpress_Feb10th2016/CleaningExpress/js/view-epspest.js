loadProject();


var addid = "";

$(document).ready(function () {



    $("#cancel").click(function () {
        window.location.href = "list-epspest.php";
    });

    $("#clClear").click(function () {
        $("#clSign").jSignature("reset");
    });

    $("#supClear").click(function () {
        $("#supSign").jSignature("reset");
    });

    $("#form-field-select-1").change(function () {

        if ($(this).val() != "") {

            var mProjectData = {};

            var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_ServiceReport_GetShipToAddress"
            var companyUser = sessionStorage["user"];
            if (companyUser) {
                companyUser = JSON.parse(companyUser);
            }

            mProjectData["Company"] = companyUser["CompanyCode"];
            mProjectData["Project"] = $(this).val();
            showLoading();
            postToServer(url, "POST", JSON.stringify(mProjectData), function (data) {
               if(data!="" && data.length>0){
				var addList = data;
								
				var opt ='<option value="">Select</option>';
				if(addList && addList[0] && addList[0].AddressId){

					$(data).each(function(i,add){
						
						//var addid = add["AddressId"];
						//var proj_name = decodeURIComponent(addid);	
						
						opt += '<option value="'+add["AddressId"]+'">'+add["AddressId"]+'</option>';

					});
					$("#addressid").html(opt);
					$("#addressid").val(addid);
					$("#addressid").change();

                }
                else {

                }


                hideLoading();


            }

        });
		}

    });

    // Need to work
    $("#save").click(function () {

        validateRequired(function (errorMsg) {
            if (errorMsg != "") {
                alert(errorMsg);
            }
            else {

                if (confirm("Confirm Save?")) {

                    var url = CONFIG.OPERATIONS_BASEPATH + "/MSave_ServiceReport";
                    var companyUser = sessionStorage["user"];
                    if (companyUser) {
                        companyUser = JSON.parse(companyUser);
                    }

                    var eps = {};
                    eps["Company"] = companyUser["CompanyCode"];
                    eps["Project"] = $("#form-field-select-1").val();
                    eps["Address"] = $("#address").val();
                    eps["Block"] = $("#blockno").val();
                    eps["Unit"] = $("#unitno").val();
                    eps["Postal"] = $("#postalcode").val();
                    eps["TimeIn"] = $("#timein").val();
                    eps["TimeOut"] = $("#timeout").val();
                    eps["Report"] = $("#report").val();
                    eps["ReportOth"] = $("#reportoth").val();
                    eps["ACommonArea"] = getCheckboxStatus($(".ACommonArea"));
                    eps["Abuilding"] = getCheckboxStatus($(".Abuilding"));
                    eps["ACorridor"] = getCheckboxStatus($(".ACorridor"));
                    eps["ADrainage"] = getCheckboxStatus($(".ADrainage"));
                    eps["APlayground"] = getCheckboxStatus($(".APlayground"));
                    eps["ABinCentre"] = getCheckboxStatus($(".ABinCentre"));
                    eps["ACarPark"] = getCheckboxStatus($(".ACarPark"));
                    eps["ALightning"] = getCheckboxStatus($(".ALightning"));
                    eps["AElectrical"] = getCheckboxStatus($(".AElectrical"));
                    eps["AStoreroom"] = getCheckboxStatus($(".AStoreroom"));
                    eps["ARooftop"] = getCheckboxStatus($(".ARooftop"));
                    eps["AManhole"] = getCheckboxStatus($(".AManhole"));
                    eps["AToilet"] = getCheckboxStatus($(".AToilet"));
                    eps["ARiser"] = getCheckboxStatus($(".ARiser"));
                    eps["AOffice"] = getCheckboxStatus($(".AOffice"));

                    eps["ACanteen"] = getCheckboxStatus($(".ACanteen"));
                    eps["AKitchen"] = getCheckboxStatus($(".AKitchen"));
                    eps["AGullyTrap"] = getCheckboxStatus($(".AGullyTrap"));
                    eps["AOthers"] = getCheckboxStatus($(".AOthers"));
                    eps["AOthersDesc"] = $("#specify").val();

                    eps["Scope"] = $("#scopeofwork").val();
                    eps["IHousekeeping"] = getCheckboxStatus($(".IHousekeeping"));
                    eps["ISanitation"] = getCheckboxStatus($(".ISanitation"));
                    eps["IStructural"] = getCheckboxStatus($(".IStructural"));
                    eps["IOthers"] = getCheckboxStatus($(".IOthers"));
                    eps["IOthersDesc"] = $(".IOthersDesc").val();
                    eps["IComments"] = getCheckboxStatus($(".IComments"));

                    eps["Supervisor"] = $("#supervisor").val();
                    eps["Client"] = $("#client").val();



                    var signature60Format = $("#supSign").jSignature("getData");

                    if (signature60Format.split(",")[1] != emptySign) {
                        eps["SupervisorSign"] = signature60Format.split(",")[1];
                    }
                    else {
                        eps["SupervisorSign"] = "";
                    }


                    signature60Format = $("#clSign").jSignature("getData");

                    if (signature60Format.split(",")[1] != emptySign) {
                        eps["ClientSign"] = signature60Format.split(",")[1];
                    }
                    else {
                        eps["ClientSign"] = "";
                    }


                    eps["Content"] = [];

                    $(".descoth").each(function () {

                        var contObj = {};
                        contObj["Description"] = $(this).parents(":eq(1)").find(".quest").text().trim();
                        contObj["DescriptionOth"] = $(this).val();
                        contObj["Include"] = getCheckboxStatus($(this).parents(":eq(1)").find(".include"));
                        contObj["Active"] = getCheckboxStatus($(this).parents(":eq(1)").find(".active"));
                        contObj["Nonactive"] = getCheckboxStatus($(this).parents(":eq(1)").find(".nonactive"));
                        contObj["TFogging"] = getCheckboxStatus($(this).parents(":eq(1)").find(".f"));
                        contObj["TMisting"] = getCheckboxStatus($(this).parents(":eq(1)").find(".m"));
                        contObj["TResidual"] = getCheckboxStatus($(this).parents(":eq(1)").find(".r"));
                        contObj["TBaits"] = getCheckboxStatus($(this).parents(":eq(1)").find(".b"));
                        contObj["TDusting"] = getCheckboxStatus($(this).parents(":eq(1)").find(".d"));
                        contObj["TTraps"] = getCheckboxStatus($(this).parents(":eq(1)").find(".t"));
                        contObj["TOthers"] = getCheckboxStatus($(this).parents(":eq(1)").find(".o"));
                        contObj["IGS"] = getCheckboxStatus($(this).parents(":eq(1)").find(".igs"));
                        contObj["AGS"] = getCheckboxStatus($(this).parents(":eq(1)").find(".ags"));
                        contObj["Location"] = $(this).parents(":eq(1)").find(".loc").val();

                        eps["Content"].push(contObj);
                    });

                    eps["Pesticide"] = [];

                    $(".pDesc").each(function () {
                        var pest = {};
                        if ($(this).val() != "") {

                            pest["Pesticide"] = $(this).val();
                            pest["Quantity"] = $(this).parents(":eq(1)").find(".pQuantity").val();

                            eps["Pesticide"].push(pest);

                        }

                    });


                    console.log(JSON.stringify(eps));

                    showLoading();
                    postToServer(url, "POST", JSON.stringify(eps), function (data) {
                        // console.log(data);
                        if (data[0] && data[0].Result == "Success") {
                            alert(data[0].DisplayMessage);
                            window.location.href = "list-eps.php";
                        }
                        else {
                            alert("Error ! Message: " + data[0].DisplayMessage);
                        }

                        hideLoading();
                    });


                }

            }
        });

    });
	
// to send Emails
    $("#SendEmail").click(function () {
		//showLoading();
        var mProjectData = {};

        var url = CONFIG.OPERATIONS_BASEPATH + "/SendEmail"
        var companyUser = sessionStorage["user"];
        if (companyUser) {
            companyUser = JSON.parse(companyUser);
        }

        mProjectData["Company"] = companyUser["CompanyCode"];
		mProjectData["DocNum"] = sessionStorage.epsDocNum;

        //var EmailID_Cust = sessionStorage["EmailID_Cust"] || "";
		var EmailID_Cust = $("#emails").val().toString();
		
        if (EmailID_Cust == "") {
            alert("EmailId is empty");
        }
        else {
			//showLoading();
            //alert(EmailID_Cust); // for testing the email id which is coming in session
            //EmailID_Cust = "satyak@abeo-electra.com"; // to prevent sendng email to user, for testing purpose only. comment this line when you will be uploading te project.
        }
		
        mProjectData["EmailId"] = EmailID_Cust;
				showLoading();	
             postToServer(url, "POST", JSON.stringify(mProjectData), function (data) {
				 hideLoading();															   
				//showLoading();															   
            if (data != "" && data.length > 0) {

                data = data[0];

                var displaymsg = data["DisplayMessage"];
				
                alert(displaymsg, false); // to display success msg which is coming from Web service-[{"Result":"Success","DisplayMessage":"Email Sent Successfully."}]
				window.location.href = "view-epspest.php";
            }
			//hideLoading();
        });
       // hideLoading();
    });	


// to Open PDF report in new tab

    $("#print").click(function () {
        var mProjectData = {};

        var url = CONFIG.OPERATIONS_BASEPATH + "/CreatePDF"
        var companyUser = sessionStorage["user"];
        if (companyUser) {
            companyUser = JSON.parse(companyUser);
        }

        mProjectData["Company"] = companyUser["CompanyCode"];
        mProjectData["DocNum"] = sessionStorage.epsDocNum;
        showLoading();
        postToServer(url, "POST", JSON.stringify(mProjectData), function (data) {
	 	hideLoading();
            if (data != "" && data.length > 0) {
                data = data[0];

                var displaymsg = data["DisplayMessage"]; // URL coming from webservice, [{"Result":"Success","DisplayMessage":"E:\\Abeo-Projects\\Cleaning Express\\Publish\\UAT\\CleaningExpress_Webservice\\TEMP/PDF/EPS/1_07072015090010177.pdf"}]
                //  alert(displaymsg, false);
                // testing if PDF is opening with correct URL or not
                displaymsg = CONFIG.WEBSERVICEURL +displaymsg;// when you have URL like this comment this line
				showLoading();
				window.open(displaymsg, '_blank');// Opens PDF in new window.
				hideLoading();
				
				
            }
        });
     
    });	
});

function loadEps() {

    var mProjectData = {};

    var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_ServiceReport_ViewServiceReport"
    var companyUser = sessionStorage["user"];
    if (companyUser) {
        companyUser = JSON.parse(companyUser);
    }

    mProjectData["Company"] = companyUser["CompanyCode"];
    mProjectData["DocNum"] = sessionStorage.epsDocNum;

    showLoading();
    postToServer(url, "POST", JSON.stringify(mProjectData), function (data) {
        if (data != "" && data.length > 0) {

            data = data[0];
			
            $("#form-field-select-1").val(data["Project"]);
			$("#form-field-select-1").change();
			
			
			
			addid = data["AddId"];
			
			
			var address = data["Address"];
			var address_decode = decodeURIComponent(address);
            $("#address").val(address_decode);
			
			var block = data["Block"];
			var block_decode = decodeURIComponent(block);
            $("#blockno").val(block_decode);
			
			var unit_d = data["Unit"];
			var unit_d_decode = decodeURIComponent(unit_d);
            $("#unitno").val(unit_d_decode);
            $("#postalcode").val(data["Postal"]);
            $("#timein").val(data["TimeIn"]);
            $("#timeout").val(data["TimeOut"]);
			
			var report = data["Report"];
			var report_decode = decodeURIComponent(report);
            $("#report").val(report_decode);
			
			var report_oath = data["ReportOth"];
			var  report_oath_decode = decodeURIComponent(report_oath);
            $("#reportoth").val(report_oath_decode);
            sessionStorage["EmailID_Cust"] = data["EmailId"];

            /*--$("#supsigndate").val(data["SignedDate1"]);
			$("#clsigndate").val(data["SignedDate2"]);*/


            if (data["ReportOth"] != "") {
                $(".rRepOthers").attr("checked", true);
            }

            // set checkboxes
            setCheckboxStatus($(".ACommonArea"), data["ACommonArea"]);
            setCheckboxStatus($(".Abuilding"), data["Abuilding"]);
            setCheckboxStatus($(".ACorridor"), data["ACorridor"]);
            setCheckboxStatus($(".AGarden"), data["AGarden"]);
            setCheckboxStatus($(".ADrainage"), data["ADrainage"]);
            setCheckboxStatus($(".APlayground"), data["APlayground"]);
            setCheckboxStatus($(".ABinCentre"), data["ABinCentre"]);
            setCheckboxStatus($(".ACarPark"), data["ACarPark"]);
            setCheckboxStatus($(".ALightning"), data["ALightning"]);
            setCheckboxStatus($(".AElectrical"), data["AElectrical"]);
            setCheckboxStatus($(".AStoreroom"), data["AStoreroom"]);
            setCheckboxStatus($(".ARooftop"), data["ARooftop"]);
            setCheckboxStatus($(".AManhole"), data["AManhole"]);
            setCheckboxStatus($(".AToilet"), data["AToilet"]);
            setCheckboxStatus($(".ARiser"), data["ARiser"]);

            setCheckboxStatus($(".AOffice"), data["AOffice"]);
            setCheckboxStatus($(".ACanteen"), data["ACanteen"]);
            setCheckboxStatus($(".AKitchen"), data["AKitchen"]);
            setCheckboxStatus($(".ACabinet"), data["ACabinet"]);
            setCheckboxStatus($(".AGullyTrap"), data["AGullyTrap"]);
            setCheckboxStatus($(".AOthers"), data["AOthers"]);
			
			var AOthers = data["AOthersDesc"];
			var AOthers_decode = decodeURIComponent(AOthers);
            $(".AOthersDesc").val(AOthers_decode);
			
			var Scope_val = data["Scope"];
			var Scope_val_decode = decodeURIComponent(Scope_val);
            $("#scopeofwork").val(Scope_val_decode);
			
			var Feedback = data["Feedback"];
			var Feedback_decode = decodeURIComponent(Feedback);
            $("#improve").val(Feedback_decode);
            setCheckboxStatus($(".IHousekeeping"), data["IHousekeeping"]);
            setCheckboxStatus($(".ISanitation"), data["ISanitation"]);
            setCheckboxStatus($(".IStructural"), data["IStructural"]);
            setCheckboxStatus($(".IOthers"), data["IOthers"]);
            // setCheckboxStatus($(".IOthersDesc"),data["IOthersDesc"]);
			
			var IOthersDesc = data["IOthersDesc"];
			var IOthersDesc_decode = decodeURIComponent(IOthersDesc);
            $(".IOthersDesc").val(IOthersDesc_decode);
			
			var IComments = data["IComments"];
			var IComments_decode = decodeURIComponent(IComments);
            $(".IComments").val(IComments_decode);
			
			var Supervisor = data["Supervisor"];
			var Supervisor_decode = decodeURIComponent(Supervisor);
            $("#supervisor").val(Supervisor_decode);
			
			var Client = data["Client"];
			var Client_decode = decodeURIComponent(Client);
            $("#client").val(Client_decode);

            var reportObj = data["Report"];

            setCheckboxStatus($(".rWeekly"), reportObj["rWeekly"]);
            setCheckboxStatus($(".rFortnightly"), reportObj["rFortnightly"]);
            setCheckboxStatus($(".rMonthly"), reportObj["rMonthly"]);
            setCheckboxStatus($(".rBimonthly"), reportObj["rBimonthly"]);
            setCheckboxStatus($(".rQuarterly"), reportObj["rQuarterly"]);
            setCheckboxStatus($(".rOnetime"), reportObj["rOnetime"]);
            setCheckboxStatus($(".rFollowup"), reportObj["rFollowup"]);

            // Signature
            // initialize signature
            $("#supSign,#clSign").jSignature({ color: "#000000", height: '110px', width: '100%' });

            var string60Format = [];
            string60Format[0] = "data:image/png;base64";
            string60Format[1] = data["SupervisorSign"];

            $("#supSign").jSignature("setData", string60Format.join(","));

            string60Format[1] = data["ClientSign"];

            $("#clSign").jSignature("setData", string60Format.join(","));


            // disable signature
            $("#clSign").jSignature("disable");
            $("#supSign").jSignature("disable");

            //E-Singnature Date
            $("#supsigndate").val(showDate(data["SignedDate1"].split(" ")[0]));
            $("#clsigndate").val(showDate(data["SignedDate2"].split(" ")[0]));
			
			var Remarks = data["Remarks"] || "";
			var filename_decode = decodeURIComponent(Remarks);
			$("#remarks").val(filename_decode);

            // Attachment
            var attachFiles = data["Attachments"];
            var atag = "";
            $(attachFiles).each(function (j, file) {

                var filename = getFilename(file["WebURL"] || "");
				var filename_decode = decodeURIComponent(filename);
                atag += '<a href="' + (CONFIG.ATTACHMENT_DIR + filename) + '" download>' + filename_decode + '</a><br/>';

            });

            $("#atcEntry").html(atag);
            /*var filename = getFilename(data["WebURL"] || "");
			
			$("#atcEntry").attr("href",CONFIG.ATTACHMENT_DIR+filename).text(filename);*/


            $("#contentTblBody").html("");
            $(data.Content).each(function (i, item) {

              var itemHtm = '<tr class="crow_'+i+'"><td>'+(i+1)+'</td><td class="quest">'+decodeURIComponent(item["Description"])+'</td><td class="text-center"><input type="checkbox" name="include" class="Include" value="Y" disabled/></td>'+
							'<td class="text-center"><input type="checkbox" name="active" class="Active" value="Y" disabled/></td><td class="text-center"><input type="checkbox" name="nonactive" class="Nonactive" value="Y" disabled/></td>'+
							'<td class="text-center"><input type="checkbox" name="f" class="TFogging" disabled/></td><td class="text-center"><input type="checkbox" name="m" class="TMisting" disabled/></td>'+
							'<td class="text-center"><input type="checkbox" name="r" class="TResidual" disabled/></td><td class="text-center"><input type="checkbox" name="b" class="TBaits" disabled/></td>'+
							'<td class="text-center"><input type="checkbox" name="d" class="TDusting" disabled/></td><td class="text-center"><input type="checkbox" name="t" class="TTraps" disabled/></td>'+
							'<td class="text-center"><input type="checkbox" name="o" class="TOthers" disabled/></td><td class="text-center"><input type="checkbox" name="igs" class="IGS" disabled/></td>'+
							'<td class="text-center"><input type="checkbox" name="ags" class="AGS" disabled/></td><td><input type="text" class="Location" disabled/></td><td><input type="text" class="descoth" value="'+decodeURIComponent(item["DescriptionOth"])+'" disabled/></td></tr>';				


                $("#contentTblBody").append(itemHtm);

                setCheckboxStatus($('.crow_' + i).find(".Include"), item["Include"]);
                setCheckboxStatus($('.crow_' + i).find(".Active"), item["Active"]);
                setCheckboxStatus($('.crow_' + i).find(".Nonactive"), item["Nonactive"]);
                setCheckboxStatus($('.crow_' + i).find(".TFogging"), item["TFogging"]);
                setCheckboxStatus($('.crow_' + i).find(".TMisting"), item["TMisting"]);
                setCheckboxStatus($('.crow_' + i).find(".TResidual"), item["TResidual"]);
                setCheckboxStatus($('.crow_' + i).find(".TBaits"), item["TBaits"]);
                setCheckboxStatus($('.crow_' + i).find(".TDusting"), item["TDusting"]);
                setCheckboxStatus($('.crow_' + i).find(".TTraps"), item["TTraps"]);
                setCheckboxStatus($('.crow_' + i).find(".TOthers"), item["TOthers"]);
                setCheckboxStatus($('.crow_' + i).find(".IGS"), item["IGS"]);
                setCheckboxStatus($('.crow_' + i).find(".AGS"), item["AGS"]);
				
				var location = item["Location"];
				var location_decode = decodeURIComponent(location);
                $(".crow_" + i).find(".Location").val(location_decode);

            });

            $(data.Pesticide).each(function (i, item) {
											 
				var Pesticide = item["Pesticide"];
				var Pesticide_code = decodeURIComponent(Pesticide);
                $(".pDesc").eq(i).val(Pesticide_code);
                $(".pQuantity").eq(i).val(item["Quantity"]);
            });



            hideLoading();
            getEmailId();
        }

    });

}


//on Address drop down change
	$("#addressid").change(function(){

		if($(this).val() != ""){

			var mProjectData = {};
			
			var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_ServiceReport_GetScopeofWork"
			var companyUser = sessionStorage["user"];
			if(companyUser){
				companyUser = JSON.parse(companyUser);
			}
			
			mProjectData["Company"] =  companyUser["CompanyCode"];
			mProjectData["Project"] = $("#form-field-select-1").val();	
			mProjectData["AddId"] = $(this).val();
			showLoading();
			postToServer(url,"POST",JSON.stringify(mProjectData),function(data){
				if(data!="" && data.length>0){

					data = data[0];
					
					var add = data["Address"];
					var proj_add = decodeURIComponent(add);
					$("#address").val(proj_add);
					
					var block = data["Block"];
					var proj_block = decodeURIComponent(block);
					$("#blockno").val(proj_block);
					
					var p_unit = data["Unit"];
					var proj_unit = decodeURIComponent(p_unit);
					$("#unitno").val(proj_unit);
					
					$("#postalcode").val(data["Postal"]);
					$("#scopeofwork").val(data["ScopeofWork"]);

					// Clear all values inside tab
					$(".tabbable").find("input[type='text']").val("");
					$(".tabbable").find("input[type='checkbox']").attr("checked",false);					

				}
				else{
					
				}
					
					
					hideLoading();
					
					
			});

		}

	});
	
	
function loadContentsTab() {

    var mProjectData = {};

    var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_ServiceReport_GetTemplate"
    var companyUser = sessionStorage["user"];
    if (companyUser) {
        companyUser = JSON.parse(companyUser);
    }

    mProjectData["Company"] = companyUser["CompanyCode"];

    showLoading();
    postToServer(url, "POST", JSON.stringify(mProjectData), function (data) {
        if (data != "" && data.length > 0) {

            $("#contentTblBody").html("");
            $(data).each(function (i, item) {

                var itemHtm = '<tr><td>' + (i + 1) + '</td><td class="quest">' +decodeURIComponent(item["Question"]) + '</td><td><input type="text" class="descoth"/></td><td class="text-center"><input type="checkbox" name="include" class="include" value="Y"/></td>' +
							'<td class="text-center"><input type="checkbox" name="active" class="active" value="Y"/></td><td class="text-center"><input type="checkbox" name="nonactive" class="nonactive" value="Y"/></td>' +
							'<td class="text-center"><input type="checkbox" name="f" class="f"/></td><td class="text-center"><input type="checkbox" name="m" class="m"/></td>' +
							'<td class="text-center"><input type="checkbox" name="r" class="r"/></td><td class="text-center"><input type="checkbox" name="b" class="b"/></td>' +
							'<td class="text-center"><input type="checkbox" name="d" class="d"/></td><td class="text-center"><input type="checkbox" name="t" class="t"/></td>' +
							'<td class="text-center"><input type="checkbox" name="o" class="o"/></td><td class="text-center"><input type="checkbox" name="igs" class="igs" /></td>' +
							'<td class="text-center"><input type="checkbox" name="ags" class="ags"/></td><td><input type="text" class="loc"/></td></tr>';

                $("#contentTblBody").append(itemHtm);

            });

        }
        else {
            $("#contentTblBody").html("<div class='mrt-20'> No Details Found</div>");
        }


        hideLoading();


    });

}


function loadProject() {
    var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_ServiceReport_Project";
    var mProjectData = {};
    var companyUser = sessionStorage["user"];
    if (companyUser) {
        companyUser = JSON.parse(companyUser);
    }
    mProjectData["sCompany"] = companyUser["CompanyCode"];
    mProjectData["sCurrentUserName"] = companyUser["UserName"];
    mProjectData["sUserRole"] = companyUser["RoleName"];
    showLoading();
    postToServer(url, "POST", JSON.stringify(mProjectData), function (data) {

        if (data != "" && data.length > 0) {
            var projList = data;

            var opt = '<option value="">Select</option>';
            if (projList && projList[0] && projList[0].PrcCode) {

                $(data).each(function (i, project) {
					var proj = project["PrcName"];
					var proj_decode = decodeURIComponent(proj);
                    opt += '<option value="' + project["PrcCode"] + '">' +proj_decode+ '</option>';

                });

                $("#form-field-select-1").html(opt);

            }

        }

        else {
            // $(".showlistdocs").html("<div class='mrt-20'> No Details Found</div>");
        }
        hideLoading();
		loadEps();


    });
}

function setCheckboxStatus($chk, val) {
    if (val == "Yes") {
        $chk.attr("checked", true);
    }
}

function validateRequired(callBack) {
    var errorMsg = "";

    if ($("#address").val() == "") {
        errorMsg = "Please enter address";
    }
    else if ($("#blockno").val() == "") {
        errorMsg = "Please enter block no";
    }
    else if ($("timein").val() == "") {
        errorMsg = "Please enter Time in";
    }
    else if ($("#timeout").val() == "") {
        errorMsg = "Please enter Time out";
    }
    else if ($("#report").val() == "Follow up" && $("#reportoth").val() == "") {
        errorMsg = "Please enter Report Oth";
    }
    else if ($(".AOthers").is(":checked") && $("#specify").val() == "") {
        errorMsg = "Please specify if you select Others";
    }

    callBack(errorMsg);
}


function getEmailId()
{

    var mProjectData = {};

    var url = CONFIG.OPERATIONS_BASEPATH + "/MGet_ServiceReport_BPEmailIds"
    var companyUser = sessionStorage["user"];
    if (companyUser) {
        companyUser = JSON.parse(companyUser);
    }

    mProjectData["Company"] = companyUser["CompanyCode"];
    mProjectData["Project"] = $("#form-field-select-1").val();
   	//showLoading();
    postToServer(url, "POST", JSON.stringify(mProjectData), function (data) {
																	  
      
//alert(data);
            //data = data[0];		
           
			
			//var address = data["Address"];
			
			//console.log("Emailids");
			//console.log(data);
			
			//$("#testthom").html(sessionStorage["EmailID_Cust"]);
			
			for (var i in data) {
  $("#emails").append('<option value="'+data[i].EmailId+'">'+data[i].EmailId+'</option>');
}

loadmulti();
			
			
			
		});
}

function displayval()
{
	$("#testthom").html($("#emails").val().toString());
}