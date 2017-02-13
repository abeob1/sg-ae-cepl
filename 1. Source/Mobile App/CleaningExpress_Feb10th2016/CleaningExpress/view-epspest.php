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
	<meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimum-scale=1.0, maximum-scale=1.0">
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
	<!--Email Multiselect-->
	<link href="assets/css/jquery.multiselect.css" rel="stylesheet" type="text/css">
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
				<div class="main-container inner" ng-app="myApp" id="angular" ng-controller="EpspestViewCtrl">
					<!-- start: PAGE -->
					<div class="main-content">
						<div class="container">
							<div class="col-md-12 col-sm-12 col-xs-12 stock">
								<div class="pull-left">
									<a href="list-epspest.php"><h3 class=""><i class="fa fa-angle-left"></i>View Pest Management Service Report</h3></a>
								</div>	
								<div class="pull-right header">
									<a href="list-epspest.php"><i class="fa fa-backward"></i> Back</a>
								</div>
							</div>
							<div class="clearfix"></div>
								<form role="form" class="form-horizontal" method="post" action="">
									<div class="row">
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Bill To
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
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Address ID
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<select id="addressid" class="form-control" disabled>
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
										
									</div>
									<div class="row">
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Address
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
                                                   <input type="text" class="form-control" name="address" id="address" disabled/>
												</div>
											</div>
										</div><div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Block No
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="text" class="form-control" name="blockno" id="blockno" disabled/>
												</div>
											</div>
										</div>
										
									</div>
									<div class="row">
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Unit No
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="text" class="form-control" name="unitno" id="unitno" disabled/>
												</div>
											</div>
										</div><div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Postal Code
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="text" class="form-control" name="postalcode" id="postalcode" disabled/>
												</div>
											</div>
										</div>
										
									</div>
									<div class="row">	
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Time In
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="time" class="form-control" name="timein" id="timein" disabled/>
												</div>
											</div>
										</div><div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Time Out
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="time" class="form-control" name="timeout" id="timeout" disabled/>
												</div>
											</div>
										</div>
										
										
										
										<!-- <div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group cHide">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Others
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="text" class="form-control" name="reportoth" id="reportoth" disabled />
												</div>
											</div>
										</div> -->
										
									</div>
									<div class="clearfix"></div>
									<div class="row">
									<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group cHide">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Report
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<!-- <input type="text" class="form-control" name="report" id="report" /> -->
													<select id="report" class="form-control" disabled>
														<option value="">Select</option>
														<option value="Weekly">Weekly</option>
														<option value="Fortnightly">Fortnightly</option>
														<option value="Monthly">Monthly</option>
														<option value="Bi-Monthly">Bi-Monthly</option>
														<option value="Quarterly">Quarterly</option>
														<option value="One-time/Ad-hoc">One-time/Ad-hoc</option>
														<option value="Follow up">Follow up</option>

													</select>
												</div>
											</div>
										</div>
										<div class="col-sm-12 col-md-12 col-xs-12">
											<div class="tabbable">
												<ul id="myTab" class="nav nav-tabs navtitle">
													<li class="active">
														<a href="#reports" data-toggle="tab">
															Report 
														</a>
													</li>
													<li>
														<a href="#areainspection" data-toggle="tab">
															Area of Inspection
														</a>
													</li>
													<li>
														<a href="#contents" data-toggle="tab">
															Contents 
														</a>
													</li>
													<li>
														<a href="#pesticide" data-toggle="tab">
															Pesticide 
														</a>
													</li>

													<li>
														<a href="#comments" data-toggle="tab">
															Comments 
														</a>
													</li>
												</ul>
												<div class="tab-content tabscroll">
													<div class="tab-pane fade in active" id="reports">
														<div class="table-responsive">
															<table class="table table-bordered table-hover" id="">
																<thead>
																	<tr>
																		<th class="center">Report Frequency</th>
																		<th class="center">Yes / No</th>
																	</tr>
																</thead>
																<tbody>
																	
																	<tr>
																		<td class="center">Weekly</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rWeekly" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Fortnightly</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rFortnightly" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Monthly</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rMonthly" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Bi-Monthly</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rBimonthly" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Quarterly</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rQuarterly" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">One-time/Ad-hoc</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rOnetime" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Follow up</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rFollowup" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Others</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rRepOthers" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Pls Specfiy</td>
																		<td class="center">
																		  <input type="text" class="" name="reportoth" id="reportoth" disabled/>
																	</tr>
																	
																</tbody>
															</table>
														</div>
														<p class="pull-right">
															
															<a href="#areainspection" class="btn btn-green show-tab">
																Next
															</a>
														</p>
													</div>

													<div class="tab-pane fade" id="areainspection">
														<div class="table-responsive">
															<table class="table table-bordered table-hover" id="sample-table-1">
																<thead>
																	<tr>
																		<th class="center">Area of Inspection</th>
																		<th class="center">Tick for Yes</th>
																	</tr>
																</thead>
																<tbody>

																	<tr>
																		<td class="center">Common Area</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ACommonArea" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Apron of Builiding
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck Abuilding" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Common Corridor</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ACorridor" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Landscape / Garden</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ACorridor" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Drainage</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ADrainage" disabled>
																			</label>
																		</div></td>
																	</tr>
																		<tr>
																		<td class="center">Playground</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck APlayground" disabled>
																			</label>
																		</div></td>
																	</tr>
																		<tr>
																		<td class="center">Bin Centre / Chute</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ABinCentre" disabled>
																			</label>
																		</div></td>
																	</tr>
																		<tr>
																		<td class="center">Car Park</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ACarPark" disabled>
																			</label>
																		</div></td>
																	</tr>
																		<tr>
																		<td class="center">Lightning Conductor Pit</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ALightning" disabled>
																			</label>
																		</div></td>
																	</tr>
																	</tr>
																		<tr>
																		<td class="center">Electrical / Gas Room</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AElectrical" disabled>
																			</label>
																		</div></td>
																	</tr>
																	</tr>
																		<tr>
																		<td class="center">Store room</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AStoreroom" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Roof Top</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ARooftop" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Manhole</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AManhole" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Toilet</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AToilet" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Riser</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ARiser" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Office / Pantry / Rooms</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AOffice" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Food Outlet / Canteen</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ACanteen" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Kitchen</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AKitchen" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Cabinet / Racks</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ACabinet" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Gully Trap</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AGullyTrap" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Others</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AOthers" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Pls Specfiy</td>
																		<td class="center">
																		  <input type="text" class="AOthersDesc" name="specify" id="specify" disabled/></td>
																	</tr>
																</tbody>
															</table>
														</div>
														<p class="pull-right">
															<a href="#reports" class="btn btn-red show-tab">
																Back
															</a>
															<a href="#contents" class="btn btn-green show-tab">
																Next
															</a>
														</p>
													</div>
													<div class="tab-pane fade" id="contents">
														<div class="table-responsive">
															<table class="table table-bordered table-hover" id="contentTbl">
																<thead>
																	<tr>
																		<th>#</th>
																		<th>Test of Pest/Service</th>
																		<th>Include</th>
																		<th>Active</th>
																		<th>Non-Active</th>
																		<th>F</th>
																		<th>M</th>
																		<th>R</th>
																		<th>B</th>
																		<th>D</th>
																		<th>T</th>
																		<th>O</th>
																		<th>IGS</th>
																		<th>AGS</th>
																		<th>Location/Remarks</th>
																		<th>Others</th>
																	</tr>
																</thead>
																<tbody id="contentTblBody">
																	<!-- <tr>
																		<td>1</td>
																		<td>Mosquitos</td>
																		<td></td>
																		<td class="text-center"><input type="checkbox" name="include" /></td>
																		<td class="text-center"><input type="checkbox" name="active" /></td>
																		<td class="text-center"><input type="checkbox" name="nonactive" /></td>
																		<td class="text-center"><input type="checkbox" name="f" /></td>
																		<td class="text-center"><input type="checkbox" name="m" /></td>
																		<td class="text-center"><input type="checkbox" name="r" /></td>
																		<td class="text-center"><input type="checkbox" name="b" /></td>
																		<td class="text-center"><input type="checkbox" name="d" /></td>
																		<td class="text-center"><input type="checkbox" name="t" /></td>
																		<td class="text-center"><input type="checkbox" name="o" /></td>
																		<td class="text-center"><input type="checkbox" name="igs" /></td>
																		<td class="text-center"><input type="checkbox" name="ags" /></td>
																		<td></td>
																	</tr>
																	<tr>
																		<td>2</td>
																		<td>Commensal Rodents</td>
																		<td></td>
																		<td class="text-center"><input type="checkbox" name="include" /></td>
																		<td class="text-center"><input type="checkbox" name="active" /></td>
																		<td class="text-center"><input type="checkbox" name="nonactive" /></td>
																		<td class="text-center"><input type="checkbox" name="f" /></td>
																		<td class="text-center"><input type="checkbox" name="m" /></td>
																		<td class="text-center"><input type="checkbox" name="r" /></td>
																		<td class="text-center"><input type="checkbox" name="b" /></td>
																		<td class="text-center"><input type="checkbox" name="d" /></td>
																		<td class="text-center"><input type="checkbox" name="t" /></td>
																		<td class="text-center"><input type="checkbox" name="o" /></td>
																		<td class="text-center"><input type="checkbox" name="igs" /></td>
																		<td class="text-center"><input type="checkbox" name="ags" /></td>
																		<td></td>
																	</tr> -->
																</tbody>
															</table>
														</div>
														<p>*Legend &nbsp;&nbsp; F-Fogging &nbsp;&nbsp;  M-Missing &nbsp;&nbsp; R-Residual Spray &nbsp;&nbsp; B-Baits &nbsp;&nbsp; D-Dusting &nbsp;&nbsp;
															T-Traps &nbsp;&nbsp; O-Others &nbsp;&nbsp;</p>
														<p class="pull-right">
															<a href="#areainspection" class="btn btn-red show-tab">
																Back
															</a>
															<a href="#pesticide" class="btn btn-green show-tab">
																Next
															</a>
														</p>
													</div>
													<div class="tab-pane fade" id="pesticide">
														<div class="table-responsive">
															<table class="table table-bordered table-hover" id="pestTbl">
																<thead>
																	<tr>
																		<th>#</th>
																		<th>Pesticide/Material</th>
																		<th>Quantity</th>
																	</tr>
																</thead>
																<tbody class="pestTblBody">
																	<tr>
																		<td>1</td>
																		<td><input type="text" class="pDesc" disabled/></td>
																		<td><input type="text" class="pQuantity" disabled/></td>
																	</tr>
																	<tr>
																		<td>2</td>
																		<td><input type="text" class="pDesc" disabled/></td>
																		<td><input type="text" class="pQuantity" disabled/></td>
																	</tr>
																	<tr>
																		<td>3</td>
																		<td><input type="text" class="pDesc" disabled/></td>
																		<td><input type="text" class="pQuantity" disabled/></td>
																	</tr>
																	<tr>
																		<td>4</td>
																		<td><input type="text" class="pDesc" disabled/></td>
																		<td><input type="text" class="pQuantity" disabled/></td>
																	</tr>
																	<tr>
																		<td>5</td>
																		<td><input type="text" class="pDesc" disabled/></td>
																		<td><input type="text" class="pQuantity" disabled/></td>
																	</tr>
																	
																</tbody>
															</table>
														</div>
														<p class="pull-right">
															<a href="#contents" class="btn btn-red show-tab">
																Back
															</a>
															<a href="#comments" class="btn btn-green show-tab">
																Next
															</a>
														</p>
													</div>
													
													<div class="tab-pane fade" id="comments">
														<div class="form-group">
															<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
																Scope of Work
															</label>
															<div class="col-xs-8 col-md-9 col-sm-9">
																<!-- <select id="form-field-select-1" class="form-control">
																	<option value="">Select</option>
																	<option value="1">Project 1</option>
																	<option value="2">Project 2</option>
																	<option value="3">Project 3</option>
																	<option value="3">Project 4</option>
																	<option value="3">Project 5</option>
																</select> -->
																<input type="text" id="scopeofwork" class="form-control" disabled/>
															</div>
														</div>
														<div class="form-group">
															<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
																Client's Feedback
															</label>
															<div class="col-xs-8 col-md-9 col-sm-9">
																 <textarea class="size" name="improve" id="improve" cols="" rows="2" disabled></textarea>
															</div>
														</div>
														<div class="table-responsive">
															<table class="table table-bordered table-hover" id="sample-table-1">
																<thead>
																	<tr>
																		<th class="center">To Improve</th>
																		<th class="center">Yes / No</th>
																	</tr>
																</thead>
																<tbody>
																	<tr>
																		<td class="center">House keeping</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck IHousekeeping" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Santitation
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ISanitation" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Structural  Defects</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck IStructural" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">others</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck IOthers" disabled>
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Please Specfiy</td>
																		<td class="center">
																		  <input type="text" class="IOthersDesc" name="specify" disabled/></td>
																	</tr>
																	<tr>
																		<td class="center">Improvement Comments</td>
																		<td class="center">
																		<textarea class="size IComments" name="improve" id="improve" cols="" rows="2" disabled></textarea></td>
																	</tr>
																</tbody>
															</table>
														</div>
														<p class="pull-right">
															<a href="#pesticide" class="btn btn-red show-tab">
																Back
															</a>
															<!-- <a href="#" class="btn btn-purple show-tab">
																Save
															</a>
															<a href="#" class="btn btn-green show-tab">
																Cancel
															</a> -->
														</p>
													</div>
												</div>
											</div>
										</div>
									</div>
									<div class="col-md-6 col-sm-6 col-xs-12">
										<div class="form-group">
											<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
												Supervisor Name
											</label>
											<div class="col-xs-8 col-md-9 col-sm-9">
												<input type="text" class="form-control" name="supervisor" id="supervisor" disabled/>
											</div>
										</div>
									</div>
									<div class="col-md-6 col-sm-6 col-xs-12">
										<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
											Client Name
										</label>
										<div class="col-xs-8 col-md-9 col-sm-9">
											<input type="text" class="form-control" name="client" id="client" disabled/>
										</div>
									</div>
									</div>
									<div class="col-md-6 col-sm-6 col-xs-12">
										<div class="form-group">
											<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
												Supervisor Sign
											</label>
											<div class="col-xs-8 col-md-9 col-sm-9">
												<div class="sigCapture" style="text-align:center"><div id="supSign"></div></div>
											</div>
										</div>
									</div>
		
									<div class="col-md-6 col-sm-6 col-xs-12">
										<div class="form-group">
											<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
												Client Sign
											</label>
											<div class="col-xs-8 col-md-9 col-sm-9">
												<div class="sigCapture" style="text-align:center"><div id="clSign"></div></div>
											</div>
										</div>
									</div>
									<!--<div class="clearfix"></div>
									<br/>
									<br/>
									<div class="col-md-6 col-sm-6 col-xs-12 cHide">
										<div class="form-group">
											<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
												Date
											</label>
											<div class="col-xs-8 col-md-9 col-sm-9">
												<input type="text" class="form-control" name="signdate" id="signdate" disabled/>
											</div>
										</div>
									</div>
									<div class="col-md-6 col-sm-6 col-xs-12 cHide">
										<div class="form-group">
											<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Date
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9">
													<input type="text" class="form-control" name="clsigndate" id="clsigndate" disabled/>
												</div>
										</div>
									</div>-->
									<!-- Start Signatue Date -->
										<div class="clearfix"></div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Date
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9">
													<input type="text" class="form-control" name="supsigndate" id="supsigndate" disabled/>
												</div>
											</div>
										</div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Date
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9">
													<input type="text" class="form-control" name="clsigndate" id="clsigndate" disabled/>
												</div>
											</div>
										</div>
								
									<!-- End Signatue Date -->
									<div class="form-group">
											<div class="remarkField"  >
												<table class="table-responsive">
							
								<tr>
									<td class="viewrep" style="font-size: 14px;font-weight: 700; color:#858585">Attachment</td>
									<td class="col-xs-12 " style="padding-left:35px"><div style="width:100%; padding-right:20px">
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
											</div>
                        
									<!--<div class="col-md-6 col-sm-6 col-xs-12">
										<div class="form-group">
											<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
												Attachment
											</label>
											<div class="col-xs-8 col-md-9 col-sm-9">
												 <input type="file" style="padding-bottom:35px;" class="form-control" name="attach" id="attach" disabled/>
												<div id="atcEntry"></div>
											</div>
										</div>
									</div>-->
									<div class="clearfix"></div>
									<div id="testthom"></div>
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
													Select Email ID
												</label>
												<div class="col-xs-8 col-sm-8 col-md-9">
													<select id="emails" multiple >
											</select>
												</div>
											</div>
										</div>
									<div class="clearfix"></div>
									
									<div class="col-md-12 col-sm-12 col-xs-12">
										<div class="form-group pull-left">
											<div class="btn-leftp">
												<!-- <button type="button" name="save" class="btn btn-success" id="save">Save</button> -->
												<button type="button" name="cancel" class="btn btn-danger" id="cancel">Cancel</button>
												<button type="button" name="SendEmail" class="btn btn-success" id="SendEmail">Send Email</button>
				                                <button type="button" name="print" class="btn btn-success" id="print">Print</button>
											</div>
										</div>
									</div>
								</form>	
							
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
		<script>$("#epspest").addClass('active');</script>
		<script>$("#tree2").addClass('treeview active');</script>

		<script src="js/jsign/jSignature.min.js"></script>
		<script type="text/javascript" src="js/view-epspest.js"></script>
		
		<!-- Email multiselect js -->
		
		<script src="assets/js/jquery.multiselect.js"></script>
<script>
function loadmulti()
{
$('select[multiple]').multiselect({
    columns: 1,
    placeholder: 'Select options',
	selectAll     : true,
	search        : true,

});
}
</script>
</body>
<!-- end: BODY -->
</html>