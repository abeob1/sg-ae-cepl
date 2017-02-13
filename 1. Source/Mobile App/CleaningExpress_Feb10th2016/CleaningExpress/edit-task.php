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
			<!--header-->
		<?php include('include/header.php'); ?>
		<!--header-->
		<div class="main-wrapper">
			<!-- Sidebar -->
				<?php include("include/sidebar.php"); ?>
			<!-- sidebar -->
			<!-- start: MAIN CONTAINER -->
			<div class="main-container inner" ng-app="myApp" id="angular" ng-controller="ViewtaskEditCtrl">
				<!-- start: PAGE -->
				<div class="main-content">
					<div class="container">
						<div class="stock">
							<div class="col-md-6 col-sm-6 col-xs-6">
								<a href="view-task.php"><h3><i class="fa fa-angle-left"></i> Edit Task Schedule</h3></a>
							</div>
							<div class="col-md-6 col-sm-6 col-xs-6">
								<div class="pull-right header">
									<a href="view-task.php"><i class="fa fa-backward"></i> Back</a>
								</div>
							</div>
						</div>
						<div class="clearfix"></div>
						<div class="container">
							<form role="form" class="form-horizontal" method="post" action="">
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-1">
											Schedule Date
										</label>
										<div class="col-xs-8 col-sm-9 col-md-9">
											<input type="date" class="form-control" id="form-field-1" disabled/>
										</div>
									</div>
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-2">
											Completed Date
										</label>
										<div class="col-xs-8 col-sm-9 col-md-9">
											<input type="date" class="form-control" id="form-field-2" disabled/>
										</div>
									</div>
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-3">
											Status
										</label>
										<div class="col-xs-8 col-sm-9 col-md-9">
											<input type="text" class="form-control" id="form-field-3" disabled/>
										</div>
									</div>
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-4">
											Completed By
										</label>
										<div class="col-xs-8 col-sm-9 col-md-9">
											<input type="text" class="form-control" id="form-field-4" disabled/>
										</div>
									</div>

								<div class="scrolllist">
									<div class="taskList">
								
									<!-- <div class="list mrt-20">
										<div class="row">
											<div class="col-md-12 col-sm-12 col-xs-12">
												<h4 class="sname">Floor Mopping</h4>
												<h5 class="subtitle">Basement</h5>
											</div>
										</div>
										<div class="row time">
											<div class="col-md-3 col-md-offset-1 col-sm-4 col-sm-offset-1 col-xs-4 col-xs-offset-1">
												<h4 class="sname">9.00AM</h4>
											<div class="input-group">
												<input type="time" class="form-control" name="fromtime"/>
											</div>		
											</div>
											<div class="col-md-3 col-md-offset-1 col-sm-4 col-sm-offset-1 col-xs-4 col-xs-offset-1">
												<h4 class="sname">10.00AM</h4>
												<div class="input-group">	
												<input type="time" class="form-control" name="totime"/>
												</div>
											</div>
										</div>
										
										<div class="form-group">
											<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-1">
												Project
											</label>
											<div class="col-xs-8 col-sm-9 col-md-9">
												<select id="form-field-select-1" class="form-control">
													<option value="1">Project 1</option>
													<option value="2">Project 2</option>
													<option value="3">Project 3</option>
													<option value="3">Project 4</option>
													<option value="3">Project 5</option>
												</select>
											</div>
										</div>
										<div class="form-group">
											<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-1">
												Reason
											</label>
											<div class="col-xs-8 col-sm-9 col-md-9">
												<input type="text" class="form-control" id="form-field-1" value="Traffic" >
											</div>
										</div>
									</div> -->
									</div>
							</div>

								<!-- <div class="form-group">
									<label for="form-field-22">E-Signature</label> -->
									<!-- <textarea name="sign" class="form-control" id="sign" cols="30" rows="3">
									</textarea> -->
									<!-- <div id="sigCaptureTask" style="text-align:center"><div id="esign"></div><i class="fa fa-refresh clear" id="clear" title="clear"></i></div>
								</div> -->

								<div class="col-md-6 col-sm-6 col-xs-12 tasksign marg-btm">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Supervisor Signature
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<div class="sigCapture" style="text-align:center"><div id="supSign"></div><i class="fa fa-refresh clear" id="supClear" title="clear"></i></div>
												</div>
											</div>
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12 tasksign marg-btm">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Client Signature
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<div class="sigCapture" style="text-align:center"><div id="clSign"></div><i class="fa fa-refresh clear" id="clClear" title="clear"></i></div>
												</div>
											</div>
										</div>
										
										<!-- Start Singnature Date -->
										<div class="clearfix"></div>
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
									</div>
								<!-- End Singnature Date -->		
								
								</form>	
								<div class="clearfix"></div>
										<div class="form-group">
											
                                            
							<table class="table-responsive">								
								<tr>
									<td class="viewrep" style="font-size: 14px;font-weight: 700; color:#858585">Attachment</td>
									<td class="col-xs-12" style="padding-left:3%; padding-right:5%">
										<!-- <input type="file" name="upload" id="upload" class="remarks" style="padding-top:5px;" > -->
                                      		  <form name="folderfrm" action="" method="POST" enctype="multipart/form-data" id="upform"  class="folder-form">
                                         <input style="width:100%; height:10px" type="file" id="fileUpload"  onchange="filechange(this);" ng-model="files" name="fileUpload" ng-disabled="isDisabled" multiple>
                                            <input type="hidden"  name="companyname" ng-model ="companyname" class="companyname"/>
									</form>
									</td>
									</tr>
                                     
                                     </table>                                   
									   
                                           <table style="width:97%; padding-left:10%"   cellpadding="5px"class="the-table table-responsive">
										   
                                      <tr>
                                      	<td></td>
                                        <td>Name</td>
                                        <td>Remarks</td>
                                        <td>Status</td>
                                       <!-- <td>Choice</td>
                                        <td>File</td>-->
                                      </tr>				               
                                       
                                      <tr ng-repeat="file in files" ng-if="file.Choice == 'A'" style="height:45px">
                                      <td style=" background-color:#FFFFFF;  vertical-align: top; padding-left:30px">
             <button style="background-color:#FFFFFF" type="button" name="del" ng-click="removeEditRow($index)"><span class="minus" title="Remove" ><i class="fa fa-minus-circle"></i></span></button></td>                                      
             
             						  <td><a href="http://203.125.57.116/LIVE/CleaningExpress_WebService/Attachments/{{file.FileName}}">{{file.FileName}}</a></td>
                                        <td><input style="width:55%" type="text" ng-model="file.Remarks" id="remarks" value="{{file.Remarks}}"  ng-keyup="getValue()"/></td>
                                        <td style="vertical-align: top; padding-left:10px"><i ng-show="file.status" style="color:#009900" class="fa fa-check"></i><i ng-show="file.loading" class="fa fa-refresh fa-spin"></i><i ng-hide="file.status || file.loading" class="fa fa-times"></i></td>
                                       <!-- <td>{{file.Choice}}</td>
                                        <td>{{file.FileMethod}}</td>-->
                                      </tr>
                                     
                                    </table>                                          
									<!--	<form name="folderfrm" action="" method="POST" enctype="multipart/form-data" id="upform" class="folder-form">
											<input type="file" id="fileAttach" name="fileName" class="form-control remarks upform" multiple/>
											<input type="hidden"  name="companyname" class="companyname"/>
										</form>-->
								
						
									</div>
									<div class="clearfix"></div>
										<br/>
										<br/>
								<div class="form-group"  style="margin-left:25px">
										<button type="button" name="attach" ng-click="att();" class="btn btn-primary"  ng-disabled="attachfile" >Attach</button>
											<button type="button" name="save" class="btn btn-success" id="save" ng-disabled="savefile">Save</button>
											<button type="button" name="cancel" class="btn btn-danger" id="cancel">Cancel</button>
								</div>
								
							
						</div>		
					</div>
				</div>
			</div>
		</div>	
		<!-- end: PAGE -->
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

		<script>$("#job-schedule").addClass('active');</script>
		<script>$("#tree2").addClass('treeview active');</script>

		
		<script src="js/jsign/jSignature.min.js"></script>
		<script type="text/javascript" src="js/edit-task.js"></script>

	</body>
	<!-- end: BODY -->
</html>