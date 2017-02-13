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
		<meta http-equiv="Content-Type" content="text/html;charset=utf-8">
		<!--[if IE]><meta http-equiv='X-UA-Compatible' content="IE=edge,IE=9,IE=8,chrome=1" /><![endif]-->
		<meta name="viewport" content="initial-scale=1">
		<meta name="apple-mobile-web-app-capable" content="yes">
		<meta name="apple-mobile-web-app-status-bar-style" content="black">
		<meta content="" name="description" />
		<meta content="" name="author" />
		<!-- end: META -->
		<!--head-script-->
         <script src="js/angular.js"></script>	
         <script type="text/javascript" src="js/file_upload.js"></script>	
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
			<div class="main-container inner" ng-app="myApp" id="angular" ng-controller="ViewtaskViewCtrl">
				<!-- start: PAGE -->
				<div class="main-content">
					<div class="container">
						<div class="stock">
							<div class="col-md-6 col-sm-6 col-xs-6">
								<a href="view-calender.php"><h3><i class="fa fa-angle-left"></i> View Task Schedule</h3></a>
							</div>
							<div class="col-md-6 col-sm-6 col-xs-6">
								<div class="pull-right header viewtask">
									<a href="view-calender.php"><i class="fa fa-backward"></i> Back</a>
									<a href="edit-task.php" id="editBtn" class="cHide"><i class="fa fa-exchange"></i> Edit</a>
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
									<!-- <div id="sigCaptureTask" ><div id="esign"></div></div>
								</div> -->
								<div class="col-md-6 col-sm-6 col-xs-12 tasksign marg-btm">
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
											Supervisor Signature
										</label>
										<div class="col-xs-8 col-md-9 col-sm-9">
											<div class="sigCapture" style="text-align:center"><div id="supSign"></div></div>
										</div>
									</div>
								</div>

								<div class="col-md-6 col-sm-6 col-xs-12 tasksign marg-btm">
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
											Client Signature
										</label>
										<div class="col-xs-8 col-md-9 col-sm-9">
											<div class="sigCapture" style="text-align:center"><div id="clSign"></div></div>
										</div>
									</div>
								</div>
								<!-- Start Singature Date -->
								<div class="clearfix"></div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Date
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9">
													<input type="date" class="form-control" name="supsigndate" id="supsigndate" disabled/>
												</div>
											</div>
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Date
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9">
													<input type="date" class="form-control" name="clsigndate" id="clsigndate" disabled/>
												</div>
											</div>
										</div>
									</div>			
									<!-- End Singnature Date -->
                                    
                                    <div class="remarkField">
							<table class="table-responsive">								
								<tr>
									<td class="viewrep" style="font-size: 14px;font-weight: 700; color:#858585">Attachment</td>
									<!--<td class="viewrep1"><div id="atcEntry">
                                    </div></td>-->
                                   <td class="col-xs-12 " style="padding-left:30px">
									<div style="width:100%; padding-right:15px">
                                                <table width="100%" class="table the-table"   >
                                                  <tr>
                                                    <td>Name</td>
                                                    <td>Remarks</td>
                                                  </tr>				               
                                                   
                                                  <tr ng-repeat="file in files" >
                                                    <td><a href="http://203.125.57.116:3870/LIVE/CleaningExpress_WebService/Attachments/{{getFilename(file.WebURL)}}">{{getFilename(file.WebURL)}}</a></td>
                                                    <td>{{file.Remarks}}</td>
                                                  </tr>
                                                 
                                                </table>
													<!-- <input type="file" id="fileAttach" class="form-control" disabled/> -->
													<!--<div id="atcEntry"></div>-->
												</div></td>
								</tr>
							</table>
						</div>
								
								<!--<div class="form-group">
									<label for="form-field-22" >Attachments</label>
									<div id="atcEntry"></div>
								</div>-->
								
							</form>	
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
		<script type="text/javascript" src="js/view-task.js"></script>

	</body>
	<!-- end: BODY -->
</html>