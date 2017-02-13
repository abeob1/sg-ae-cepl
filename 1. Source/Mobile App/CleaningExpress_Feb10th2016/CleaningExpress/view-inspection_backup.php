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
	<script src="js/angular.js"></script>	
         <script type="text/javascript" src="js/file_upload.js"></script>	
	<!-- end: META -->
	<!--head-script-->
	<?php include('include/head-script.php'); ?>
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
				<div class="main-container inner" ng-app="myApp" id="angular" ng-controller="InspectionViewCtrl">
					<!-- start: PAGE -->
					<div class="main-content">
						<div class="container">
							<a href="list-inspection.php" class="addStock"><h3 class="stock"><i class="fa fa-angle-left"></i> View Inspection Q/A</h3></a>
							<div id="stock-list">
								<form role="form" class="form-horizontal" method="post" action="">
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
											Market Segment
										</label>
										<div class="col-xs-8 col-md-9 col-sm-9">
											<select id="form-field-select-1" class="form-control" disabled>
												<!-- <option value="">Select</option>
												<option value="1">Project 1</option>
												<option value="2">Project 2</option>
												<option value="3">Project 3</option>
												<option value="3">Project 4</option>
												<option value="3">Project 5</option> -->
											</select>
										</div>
									</div>
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

											<div class="col-md-8 col-sm-7 col-xs-6">
												<div class="ques">
													<h5>Items</h5>
												</div>
											</div>
											<div class="col-md-4 col-sm-5 col-xs-6">
												<h5 class="ques1">
													<ul class="checkbox-grid">
														<li><label for="text3"></label>1</li>
														<li><label for="text4"></label>2</li>
														<li><label for="text5"></label>3</li>
														<li><label for="text6"></label>4</li>
														<li><label for="text7"></label>5</li>
													</ul>
												</h5>
											</div>
										</div>

									<div class="vQuestions scrolllist">
										<!-- <div class="row">
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
									<div class="form-group cHide">
										<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
											Esignature
										</label>
										<div class="col-xs-8 col-md-9 col-sm-9">
											<div id="sigCaptureTask" ><div id="esign"></div></div>
										</div>
									</div>
									<div class="row">
											<div class="col-md-6 col-sm-6 col-xs-12 tasksign">
												<div class="form-group">
													<label class="col-xs-4 col-sm-3 col-md-3" for="form-field-select-1">
														Inspected By
													</label>
													<div class="col-xs-7 col-md-8 col-sm-8">
														<input type="text" class="form-control" name="supervisor" id="supervisor" disabled/>
													</div>
												</div>
											</div>
											<div class="col-md-6 col-sm-6 col-xs-12 tasksign">
												<div class="form-group">
													<label class="col-xs-4 col-sm-3 col-md-3" for="form-field-select-1">Acknowledge By
													</label>
													<div class="col-xs-7 col-md-8 col-sm-8">
														<input type="text" class="form-control" name="client" id="client" disabled/>
													</div>
												</div>
											</div>
											<div class="clearfix"></div>
											<div class="col-md-6 col-sm-6 col-xs-12 tasksign marg-btm">
												<div class="form-group">
													<label class="col-xs-4 col-sm-2 col-md-2" for="form-field-select-1">
														E-Signature
													</label>
													<div class="col-xs-7 col-md-9 col-sm-9" style="padding-left:60px">
														<div class="sigCapture" style="text-align:center"><div id="supSign"></div><i class="fa fa-refresh clear" id="supClear" title="clear"></i></div>
													</div>
												</div>
											</div>
											<div class="col-md-6 col-sm-6 col-xs-12 tasksign marg-btm">
												<div class="form-group">
													<label class="col-xs-4 col-sm-2 col-md-2" for="form-field-select-1">
														E-Signature
													</label>
													<div class="col-xs-7 col-md-9 col-sm-9"  style="padding-left:60px">
														<div class="sigCapture" style="text-align:center"><div id="clSign"></div><i class="fa fa-refresh clear" id="clClear" title="clear"></i></div>
													</div>
												</div>
											</div>
										</div>
										<div class="form-group">
											<!--<label class="col-xs-4 col-sm-3 col-md-2 pad0" for="form-field-select-1">
												Attachment
											</label>
											<div class="col-xs-7 col-md-9 col-sm-8">-->
												<!--<form name="folderfrm" action="" method="POST" enctype="multipart/form-data" id="upform" class="folder-form">
													<input type="file" id="fileAttach" name="fileName" class="form-control upform" multiple/>
													<input type="hidden"  name="companyname" class="companyname"/>
												</form>-->
												<div class="remarkField"  >
												<table class="the-table">
								<tr>
									<td  class="viewrep" style="font-size: 14px;font-weight: 700; color:#858585">Remarks</td>
								  <td class="col-xs-12 " style="padding-left:65px">
									<!--<input type="text" name="remarks" id="remarks" class="form-control remarks"></td>-->
								<div style="width:100%; padding-right:45px">
                                                <textarea class="size" name="remarks" id="remarks" cols="" rows="2" disabled></textarea>
                                        </div>				</td>
								</tr>
								<tr>
								<td style="height:15px;"></td>
								</tr>
								<tr>
									<td class="viewrep" style="font-size: 14px;font-weight: 700; color:#858585">Attachment</td>
									<td class="col-xs-12 " style="padding-left:65px">
									<div style="width:100%; padding-right:45px">
                                                <table width="100%" class="table"   >
                                                  <tr>
                                                    <td>Name</td>
                                                    <td>Remarks</td>
                                                  </tr>				               
                                                   
                                                  <tr ng-repeat="file in files" >
                                                    <td><a href="http://54.251.51.69:3872/Attachments/{{getFilename(file.WebURL)}}">{{getFilename(file.WebURL)}}</a></td>
                                                    <td>{{file.Remarks}}</td>
                                                  </tr>
                                                 
                                                </table>
													<!-- <input type="file" id="fileAttach" class="form-control" disabled/> -->
													<!--<div id="atcEntry"></div>-->
												</div></td>
								</tr>
							</table>
												</div>
											</div>
									
									<div class="form-group">
										<div class="btn-leftp">
											<button type="button" name="cancel" class="btn btn-danger" id="cancel">Cancel</button>
				                            <button type="button" name="print" class="btn btn-success" id="print">Print</button>
										</div>
									</div>
									
								</form>	
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
		<script type="text/javascript" src="js/view-inspection.js"></script>
</body>
<!-- end: BODY -->
</html>