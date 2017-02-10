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
				<div class="main-container inner" ng-app="myApp" id="angular" ng-controller="LandscapeMainCtrl">
					<!-- start: PAGE -->
					<div class="main-content">
						<div class="container">
							<a href="list-landscape.php" class="addStock"><h3 class="stock"><i class="fa fa-angle-left"></i> Add GSL Landscape Checklist</h3></a>
							<div class="row">
								<form role="form" class="form-horizontal" method="post" action="">
									<div class="row">
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Project
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<select id="form-field-select-1" class="form-control">
														<!-- <option value="">Select</option>
														<option value="1">Project 1</option>
														<option value="2">Project 2</option>
														<option value="3">Project 3</option>
														<option value="3">Project 4</option>
														<option value="3">Project 5</option> -->
													</select>
												</div>
											</div>
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group cHide">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Contact Ref
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="text" class="form-control" name="reference" id="reference" />
												</div>
											</div>
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Time In
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="time" class="form-control" name="timein" id="timein" />
												</div>
											</div>
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Time Out
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="time" class="form-control" name="timeout" id="timeout" />
												</div>
											</div>
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group cHide">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Issue Date
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="date" class="form-control" name="idate" id="idate" />
												</div>
											</div>
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group cHide">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Receive Date
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="date" class="form-control" name="rdate" id="rdate" />
												</div>
											</div>
										</div>
									</div>
									<div class="clearfix"></div>
									<div class="row fontsize">
										<div class="col-md-1 col-sm-1 col-xs-1" style="padding-left:40px">#</div>
										<div class="col-md-4 col-sm-4 col-xs-4">Work Activity</div>
										<div class="col-md-3 col-sm-3 col-xs-3">Done</div>
										<div class="col-md-4 col-sm-4 col-xs-3">Remarks</div>
									</div>
									<div class="scrolllist">
										<div class="gQuestions">
											<!-- <div class="row mrb-5">
												<div class="col-md-1 col-sm-1 col-xs-1">1</div>
												<div class="col-md-4 col-sm-4 col-xs-4">Watering</div>
												<div class="col-md-3 col-sm-3 col-xs-3"><input type="text" class="form-control" /></div>
												<div class="col-md-4 col-sm-4 col-xs-3"><input type="text" class="form-control" /></div>
											</div><br />
											<div class="clearfix"></div>
											<div class="row mrb-5">
												<div class="col-md-1 col-sm-1 col-xs-1">2</div>
												<div class="col-md-4 col-sm-4 col-xs-4">Fungicide Application</div>
												<div class="col-md-3 col-sm-3 col-xs-3"><input type="text" class="form-control" /></div>
												<div class="col-md-4 col-sm-4 col-xs-3"><input type="text" class="form-control" /></div>
											</div><br />
											<div class="clearfix"></div>
											<div class="row mrb-5">
												<div class="col-md-1 col-sm-1 col-xs-1">3</div>
												<div class="col-md-4 col-sm-4 col-xs-4">Chemical Application</div>
												<div class="col-md-3 col-sm-3 col-xs-3"><input type="text" class="form-control" /></div>
												<div class="col-md-4 col-sm-4 col-xs-3"><input type="text" class="form-control" /></div>
											</div><br />
											<div class="clearfix"></div>
											<div class="row mrb-5">
												<div class="col-md-1 col-sm-1 col-xs-1">4</div>
												<div class="col-md-4 col-sm-4 col-xs-4">Watering</div>
												<div class="col-md-3 col-sm-3 col-xs-3"><input type="text" class="form-control" /></div>
												<div class="col-md-4 col-sm-4 col-xs-3"><input type="text" class="form-control" /></div>
											</div><br />
											<div class="clearfix"></div> -->
										</div>
									</div>
									<div class="row">
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Supervisor
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9" style="padding-left:5%">
													<input type="text" class="form-control" name="supervisor" id="supervisor"/>
												</div>
											</div>
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Client
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9" style="padding-right:6%">
													<input type="text" class="form-control" name="client" id="client" />
												</div>
											</div>
										</div>
										<div class="clearfix"></div>
										<div class="col-md-6 col-sm-6 col-xs-12 marg-btm">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Supervisor Sign
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9" style="padding-left:5%">
													<div class="sigCapture" style="text-align:center"><div id="supSign"></div><i class="fa fa-refresh clear" id="supClear" title="clear"></i></div>
												</div>
											</div>
										</div>

										<div class="col-md-6 col-sm-6 col-xs-12 marg-btm">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Client Sign
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9" style="padding-right:30px">
													<div class="sigCapture" style="text-align:center"><div id="clSign"></div><i class="fa fa-refresh clear" id="clClear" title="clear"></i></div>
												</div>
											</div>
										</div>
										
										
										<div class="clearfix"></div>
                                        </div>
					
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Date
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9">
													<input type="date" class="form-control" name="supsigndate" id="supsigndate" />
												</div>
											</div>
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Date
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9">
													<input type="date" class="form-control" name="clsigndate" id="clsigndate" />
												</div>
											</div>
										</div>
									
								</form>	
									<div class="row">
									<!--<div class="col-md-12 col-sm-12 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-1 pad0" for="form-field-select-1">
													Attachment
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<form name="folderfrm" action="" method="POST" enctype="multipart/form-data" id="upform" class="folder-form">
														<input type="file" id="fileAttach" name="fileName" class="form-control upform" multiple/>
														<input type="hidden"  name="companyname" class="companyname"/>
													</form>
												</div>
											</div>
										</div>-->
                                        <div class="remarkField"  >
													<table class="table-responsive">								
								<tr>
									<td class="viewrep" style="font-size: 14px;font-weight: 700; color:#858585">Attachment</td>
									<td class="col-xs-12 " style="padding-left:3%; padding-right:3% ">
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
									<br/>
									<div class="form-group">
										<div class="btn-leftp">
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
		<script>$("#landscape").addClass('active');</script>
		<script>$("#tree2").addClass('treeview active');</script>

		<script src="js/jsign/jSignature.min.js"></script>
		<script type="text/javascript" src="js/add-landscape.js"></script>
</body>
<!-- end: BODY -->
</html>