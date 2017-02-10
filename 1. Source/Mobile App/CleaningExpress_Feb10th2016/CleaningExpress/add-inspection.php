<!DOCTYPE html>
<!--[if IE 8]><html class="ie8" lang="en"><![endif]-->
<!--[if IE 9]><html class="ie9" lang="en"><![endif]-->
<!--[if !IE]><!-->
<html lang="en">
	<!--<![endif]-->
	<!-- start: HEAD -->
<head>
	<title>Cleaning Express</title>
	<!-- start: META -->
	<meta charset="utf-8" />
	<!--[if IE]><meta http-equiv='X-UA-Compatible' content="IE=edge,IE=9,IE=8,chrome=1" /><![endif]-->
	<meta name="viewport" content="initial-scale=1">
	<meta name="apple-mobile-web-app-capable" content="yes">
	<meta name="apple-mobile-web-app-status-bar-style" content="black">
	<meta content="" name="description" />
	<meta content="" name="author" />
	
	<!-- end: META -->
	<!--head-script-->
	<?php include('include/head-script.php'); ?>
	<script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>
        <script src="js/angular.js"></script>		
        <script type="text/javascript" src="js/file_upload.js"></script>
        <script type="text/javascript">
		function filechange(data)
		{
        	var as =  angular.element(data).scope();
			as.$apply(as.uploadFile(data.files))
		}
		</script>
	<!--head-script-->
</head>
<!-- end: HEAD -->
<!-- start: BODY -->
<body>
		<div class="main-wrapper">
			<!--Header-->
				<?php include("include/header.php"); ?>
			<!--Header-->
			<div class="main-wrapper">
				<!-- Sidebar -->
					<?php include("include/sidebar.php"); ?>
				<!-- sidebar -->
				<!-- start: MAIN CONTAINER -->
				<div class="main-container inner"   ng-app="myApp" id="angular" ng-controller="InspectionMainCtrl" >
					<!-- start: PAGE -->
					<div class="main-content">
						<div class="container">
							<a href="list-inspection.php" class="addStock"><h3 class="stock"><i class="fa fa-angle-left"></i> Add Inspection Q/A</h3></a>
							<div id="stock-list">
								<form role="form" class="form-horizontal" method="post" action="">
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
											Market Segment
										</label>
										<div class="col-xs-8 col-md-9 col-sm-9">
											<select id="form-field-select-1" class="form-control" disabled="disabled">
												<!-- <option value="">Select</option>
												<option value="1">Project 1</option>
												<option value="2">Project 2</option>
												<option value="3">Project 3</option>
												<option value="3">Project 4</option>
												<option value="3">Project 5</option> -->
											</select>
										</div>
									</div>
									
								
									<p id="demo"></p><br/>
									

									<div class="form-group inspect-item">

										<div class="row">
											<div class="col-md-15 col-sm-15 col-xs-15">
												<!-- <h5><span class="required">*</span> Required</h5> -->
												<div class="col-md-4 col-sm-4 col-xs-3">
												<!-- <h5><span class="required">*</span> Required</h5> -->
												</div>
												<div class="col-md-8 col-sm-8 col-xs-8">
												<h5 style="float:right;">1- Unsatisfactory 2- Satisfactory 3- Fair 4- Good 5- Excellent</h5>
												</div>
											</div>
											<!-- <div class="col-md-3 col-sm-3 col-xs-6">
												<h5><span class="cLeft">1-Good</span><span class="cRight">3-Poor</span></h5>
											</div> -->
											<div class="col-md-3 col-sm-3 col-xs-6">
												<!-- <h5><span style="padding-right:5px">Good</span><span style="padding-right:5px">Fair</span><span style="padding-right:5px">Satisfactory</span></h5> -->
												<h5></h5>
											</div>

										</div>

										<div class="row" >
											<div class="col-md-8 col-sm-7 col-xs-6" style="width:61%">
												<div class="ques">
													<h5>Items</h5>
												</div>
											</div>
											<div style="background:#6fa63e!important">
											<div class="col-md-3 col-sm-5 col-xs-6">
												<h5 class="ques1">
													<ul class="checkbox-grid">
														<li><label for="text3"></label>1</li>
														<li><label for="text4"></label>2</li>
														<li><label for="text5"></label>3</li>
														<li><label for="text6"></label>4</li>
														<li><label for="text7"></label>5</li>
													</ul>
												</h5>
											</div></div>
										</div>
										<div class="vQuestions scrolllist">
									<!-- 	<div class="row">
											<div class="col-md-8 col-sm-7 col-xs-6">
												<h4>1.Category</h4>
											</div>
										</div>
										<div class="row">
											<div class="col-md-8 col-sm-7 col-xs-6">
												<h5>1.1 Item</h5>
											</div>
											<div class="col-md-4 col-sm-5 col-xs-6">
												<ul class="sradio-grid">
													<li><input type="radio" name="" value="1" /><label></label></li>
													<li><input type="radio" name="" value="2" /><label></label></li>
													<li><input type="radio" name="" value="3" /><label></label></li>
													<li><input type="radio" name="" value="4" /><label></label></li>
													<li><input type="radio" name="" value="5" /><label></label></li>
												</ul>
											</div>
										</div>
										<div class="row">
											<div class="col-md-8 col-sm-7 col-xs-6">
												<h5>1.2 Item</h5>
											</div>
											<div class="col-md-4 col-sm-5 col-xs-6">
												<ul class="sradio-grid">
													<li><input type="radio" name="" value="1" /><label></label></li>
													<li><input type="radio" name="" value="2" /><label></label></li>
													<li><input type="radio" name="" value="3" /><label></label></li>
													<li><input type="radio" name="" value="4" /><label></label></li>
													<li><input type="radio" name="" value="5" /><label></label></li>
												</ul>
											</div>
										</div> -->
									</div>
									</div>
									<div class="boxes">
										<!-- <div class="form-group">
											<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
												Esignature
											</label>
											<div class="col-xs-8 col-md-9 col-sm-9">
												<div id="sigCaptureTask" style="text-align:center"><div id="esign" ></div><i class="fa fa-refresh" id="clear" title="clear"></i></div>
											</div>
										</div> -->
										<div class="row">
											<div class="col-md-6 col-sm-6 col-xs-12">
												<div class="form-group">
													<label class="col-xs-4 col-sm-3 col-md-3" for="form-field-select-1">
														Inspected By
													</label>
													<div class="col-xs-7 col-md-8 col-sm-8">
														<input type="text" class="form-control" name="supervisor" id="supervisor" />
													</div>
												</div>
											</div>
											<div class="col-md-6 col-sm-6 col-xs-12">
												<div class="form-group">
													<label class="col-xs-4 col-sm-3 col-md-3" for="form-field-select-1">Acknowledge By
													</label>
													<div class="col-xs-7 col-md-8 col-sm-8">
														<input type="text" class="form-control" name="client" id="client" />
													</div>
												</div>
											</div>
											<div class="clearfix"></div>
											<div class="col-md-6 col-sm-6 col-xs-12 marg-btm">
												<div class="form-group">
													<label class="col-xs-4 col-sm-3 col-md-3" for="form-field-select-1">
														E-Signature
													</label>
													<div class="col-xs-7 col-md-8 col-sm-8">
														<div class="sigCapture" style="text-align:center"><div id="supSign"></div><i class="fa fa-refresh clear" id="supClear" title="clear"></i></div>
													</div>
												</div>
											</div>

											<div class="col-md-6 col-sm-6 col-xs-12 marg-btm">
												<div class="form-group">
													<label class="col-xs-4 col-sm-3 col-md-3" for="form-field-select-1">
														E-Signature
													</label>
													<div class="col-xs-7 col-md-8 col-sm-8">
														<div class="sigCapture" style="text-align:center"><div id="clSign"></div><i class="fa fa-refresh clear" id="clClear" title="clear"></i></div>
													</div>
												</div>
											</div>
										</div>

									<!--	<div class="form-group">
											<label class="col-xs-4 col-sm-3 col-md-2" for="form-field-select-1">
												Remarks
											</label>
											<div class="col-xs-7 col-md-9 col-sm-9">
											<textarea class="size" name="remarks" id="remarks" cols="" rows="2" disabled></textarea>
										</div>
										</div>-->
   
									</div>
                                        
									</form>	
									<div class="clearfix"></div>
										<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<!--<label class="col-xs-4 col-sm-3 col-md-2 pad0" for="form-field-select-1">
												Attachment
											</label>
											<div class="col-xs-7 col-md-9 col-sm-8">-->
												<!--<form name="folderfrm" action="" method="POST" enctype="multipart/form-data" id="upform" class="folder-form">
													<input type="file" id="fileAttach" name="fileName" class="form-control upform" multiple/>
													<input type="hidden"  name="companyname" class="companyname"/>
												</form>-->
												<div class="remarkField"  >
												<table class="table-responsive">		
								<tr>
									<td  class="viewrep" style="font-size: 14px;font-weight: 700; color:#858585">Remarks</td>
								  <td class="col-xs-12 " style="padding-left:4%;">
									<!--<input type="text" name="remarks" id="remarks" class="form-control remarks"></td>-->
								<textarea style="width:100%"  name="remarks" class="form-control remarks" id="remarks" cols="" rows="2"></textarea>		</td>
								</tr>
								<tr>
								<td style="height:15px;"></td>
								</tr>
								<tr>
									<td class="viewrep" style="font-size: 14px;font-weight: 700; color:#858585">Attachment</td>
									<td class="col-xs-12 " style="padding-left:4%; ">
										<!-- <input type="file" name="upload" id="upload" class="remarks" style="padding-top:5px;" > -->
                                      <form name="folderfrm" action="" method="POST" enctype="multipart/form-data" id="upform"  class="folder-form">
                                         <input  style="width:100%; height:50px" type="file" id="fileUpload"  onchange="filechange(this);" ng-model="files" name="fileUpload" ng-disabled="isDisabled" multiple>
                                            <input type="hidden"  name="companyname" ng-model ="companyname" class="companyname"/>
									</form>
                                     </td>
									 </tr>
									 </table>
                                            <!-- in case of error show error message with file name-->                                         
                                           <table style="width:100%" cellpadding="5px"class="the-table table-responsive">
                                              <tr>
                                                <td ></td>
                                                <td>Name</td>
                                                <td >Remarks</td>                                                
                                                <td >Status</td>                                                
                                              </tr>
                                              <tr ng-repeat="file in ts" style="height:45px">
                                               <td style=" background-color:#FFFFFF;  vertical-align: top; padding-left:30px">
             <button style="background-color:#FFFFFF" type="button" name="del" ng-click="removeRow($index)"><span class="minus" title="Remove" ><i class="fa fa-minus-circle"></i></span></button>            </td>
                                                <td>{{file.FileName}}</span></td>
                                                <td><input style="width:55%" type="text" id="remarks" ng-model="file.Remarks" ng-keypress="getValue()"></td>                                                 
                                                <td style="vertical-align: top; padding-left:20px"><i ng-show="file.status" style="color:#009900" class="fa fa-check"></i><i ng-show="file.loading" class="fa fa-refresh fa-spin"></i><i ng-hide="file.status || file.loading" class="fa fa-times"> </i></td>
                                               	
                                              </tr>
                                            </table>                                          
									
									
												</div>
											</div>
										</div>
										<div class="clearfix"></div>

										
									<div class="form-group">
										<div class="btn-leftp" style="margin-left:25px">
											<button type="button" name="attach" ng-click="att();" class="btn btn-primary"  ng-disabled="attachfiles" >Attach</button>
											<button type="button" name="save" class="btn btn-success" id="save" ng-disabled="savefiles">Save</button>
											<button type="button" name="cancel" class="btn btn-danger" id="cancel">Cancel</button>
										</div>
									</div>
								
							</div>	
						</div>
						<!-- end: PAGE -->
					</div>
				</div>	
					<!-- end: MAIN CONTAINER -->
					<!--footer-->
						<?php include('include/footer.php'); ?>
					<!--footer-->
					
			</div>
		</div>
		<!--foot-script-->
		<?php include('include/foot-script.php'); ?>
		<!--foot-script-->
		<!-- <script src="assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>
		<script src="assets/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js"></script>
		<script>
			/*jQuery(document).ready(function() {
				Main.init();
				SVExamples.init();
				FormElements.init();
			});*/
		</script> -->
		<script>$("#inspection").addClass('active');</script>
		<script>$("#tree2").addClass('treeview active');</script>

		<script src="js/jsign/jSignature.min.js"></script>
		<script type="text/javascript" src="js/add-inspection.js"></script>
</body>
<!-- end: BODY -->
</html>