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
				<div class="main-container inner" ng-app="myApp" id="angular" ng-controller="EpspestMainCtrl">
					<!-- start: PAGE -->
					<div class="main-content">
						<div class="container">
							<div class="col-md-12 col-sm-12 col-xs-12 stock">
								<div class="pull-left">
									<a href="list-epspest.php"><h3 class=""><i class="fa fa-angle-left"></i>Add Pest Management Service Report</h3></a>
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
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Address ID
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<select id="addressid" class="form-control">
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
                                                   <input type="text" class="form-control" name="address" id="address" />
												</div>
											</div>
										</div><div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Block No
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="text" class="form-control" name="blockno" id="blockno" />
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
													<input type="text" class="form-control" name="unitno" id="unitno" />
												</div>
											</div>
										</div><div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Postal Code
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="text" class="form-control" name="postalcode" id="postalcode" />
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
													<input type="time" class="form-control" name="timein" id="timein" />
												</div>
											</div>
										</div><div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Time Out
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="time" class="form-control" name="timeout" id="timeout" />
												</div>
											</div>
										</div>
										
										
										<!-- <div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group cHide">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Others
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<input type="text" class="form-control" name="reportoth" id="reportoth" />
												</div>
											</div>
										</div> -->
									</div>
									<!-- <div class="row"> -->
										
										
										
									<!-- </div> -->
									<div class="clearfix"></div>
									<div class="row">
										<div class="col-md-6 col-sm-6 col-xs-12">
											<div class="form-group cHide">
												<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
													Report
												</label>
												<div class="col-xs-8 col-md-9 col-sm-9">
													<!-- <input type="text" class="form-control" name="report" id="report" /> -->
													<select id="report" class="form-control">
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
										</div><div class="col-sm-12 col-md-12 col-xs-12">
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
													<!-- <li>
														<a href="#reports" data-toggle="tab">
															Report 
														</a>
													</li> -->
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
																				<input type="checkbox" class="flat-grey foocheck rWeekly">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Fortnightly</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rFortnightly">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Monthly</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rMonthly">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Bi-Monthly</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rBimonthly">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Quarterly</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rQuarterly">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">One-time/Ad-hoc</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rOnetime">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Follow up</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rFollowup">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Others</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck rRepOthers">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Pls Specfiy</td>
																		<td class="center">
																		  <input type="text" class="" name="reportoth" id="reportoth" />
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
																				<input type="checkbox" class="flat-grey foocheck ACommonArea">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Apron of Builiding
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck Abuilding">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Common Corridor</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ACorridor">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Landscape / Garden</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AGarden">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Drainage</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ADrainage">
																			</label>
																		</div></td>
																	</tr>
																		<tr>
																		<td class="center">Playground</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck APlayground">
																			</label>
																		</div></td>
																	</tr>
																		<tr>
																		<td class="center">Bin Centre / Chute</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ABinCentre">
																			</label>
																		</div></td>
																	</tr>
																		<tr>
																		<td class="center">Car Park</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ACarPark">
																			</label>
																		</div></td>
																	</tr>
																		<tr>
																		<td class="center">Lightning Conductor Pit</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ALightning">
																			</label>
																		</div></td>
																	</tr>
																	</tr>
																		<tr>
																		<td class="center">Electrical / Gas Room</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AElectrical">
																			</label>
																		</div></td>
																	</tr>
																	</tr>
																		<tr>
																		<td class="center">Store room</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AStoreroom">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Roof Top</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ARooftop">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Manhole</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AManhole">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Toilet</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AToilet">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Riser</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ARiser">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Office / Pantry / Rooms</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AOffice">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Food Outlet / Canteen</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ACanteen">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Kitchen</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AKitchen">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Cabinet / Racks</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ACabinet">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Gully Trap</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AGullyTrap">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Others</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck AOthers">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Pls Specfiy</td>
																		<td class="center">
																		  <input type="text" class="" name="specify" id="specify" /></td>
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
														<p>*Legend &nbsp;&nbsp; F-Fogging &nbsp;&nbsp;  M-Misting &nbsp;&nbsp; R-Residual Spray &nbsp;&nbsp; B-Baits &nbsp;&nbsp; D-Dusting &nbsp;&nbsp;
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
																		<td><input type="text" class="pDesc"/></td>
																		<td><input type="text" class="pQuantity"/></td>
																	</tr>
																	<tr>
																		<td>2</td>
																		<td><input type="text" class="pDesc"/></td>
																		<td><input type="text" class="pQuantity"/></td>
																	</tr>
																	<tr>
																		<td>3</td>
																		<td><input type="text" class="pDesc"/></td>
																		<td><input type="text" class="pQuantity"/></td>
																	</tr>
																	<tr>
																		<td>4</td>
																		<td><input type="text" class="pDesc"/></td>
																		<td><input type="text" class="pQuantity"/></td>
																	</tr>
																	<tr>
																		<td>5</td>
																		<td><input type="text" class="pDesc"/></td>
																		<td><input type="text" class="pQuantity"/></td>
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
																<input type="text" id="scopeofwork" class="form-control"/>
															</div>
														</div>
														<div class="form-group">
															<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-select-1">
																Client's Feedback
															</label>
															<div class="col-xs-8 col-md-9 col-sm-9">
																 <textarea class="size" name="improve" id="feedback" cols="" rows="2"></textarea>
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
																				<input type="checkbox" class="flat-grey foocheck IHousekeeping">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Santitation
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck ISanitation">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Structural  Defects</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck IStructural">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">others</td>
																		<td class="center">
																		<div class="checkbox-table">
																			<label>
																				<input type="checkbox" class="flat-grey foocheck IOthers">
																			</label>
																		</div></td>
																	</tr>
																	<tr>
																		<td class="center">Please Specfiy</td>
																		<td class="center">
																		  <input type="text" class="IOthersDesc" name="specify" /></td>
																	</tr>
																	<tr>
																		<td class="center">Improvement Comments</td>
																		<td class="center">
																		<textarea class="size IComments" name="improve" id="improve" cols="" rows="2"></textarea></td>
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
											<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
												Supervisor Name
											</label>
											<div class="col-xs-8 col-sm-8 col-md-9">
												<input type="text" class="form-control" name="supervisor" id="supervisor"/>
											</div>
										</div>
									</div>
									<div class="col-md-6 col-sm-6 col-xs-12">
										<div class="form-group">
										<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
											Client Name
										</label>
										<div class="col-xs-8 col-sm-8 col-md-9">
											<input type="text" class="form-control" name="client" id="client" />
										</div>
									</div>
									</div>
									<div class="col-md-6 col-sm-6 col-xs-12 marg-btm">
										<div class="form-group">
											<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
												Supervisor Sign
											</label>
											<div class="col-xs-8 col-sm-8 col-md-9">
												<div class="sigCapture" style="text-align:center"><div id="supSign"></div><i class="fa fa-refresh clear" id="supClear" title="clear"></i></div>
											</div>
										</div>
									</div>
									<div class="col-md-6 col-sm-6 col-xs-12 marg-btm">
										<div class="form-group">
											<label class="col-xs-3 col-sm-3 col-md-2" for="form-field-select-1">
												Client Sign
											</label>
											<div class="col-xs-8 col-sm-8 col-md-9">
												<div class="sigCapture" style="text-align:center"><div id="clSign"></div><i class="fa fa-refresh clear" id="clClear" title="clear"></i></div>
											</div>
										</div>
									</div>
									
									<!--Start Signature Date -->
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
									<!-- End Signature Date -->
									
								</form>	
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
													<table class="table-responsive">								
								<tr>
									<td class="viewrep" style="font-size: 14px;font-weight: 700; color:#858585">Attachment</td>
									<td class="col-xs-12 " style="padding-left:1.5%; ">
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

									<div class="clearfix"></div>
									<div class="col-md-12 col-sm-12 col-xs-12">
										<div class="form-group pull-left">
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
		<script>$("#epspest").addClass('active');</script>
		<script>$("#tree2").addClass('treeview active');</script>

		<script src="js/jsign/jSignature.min.js"></script>
		<script type="text/javascript" src="js/add-epspest.js"></script>
</body>
<!-- end: BODY -->
</html>