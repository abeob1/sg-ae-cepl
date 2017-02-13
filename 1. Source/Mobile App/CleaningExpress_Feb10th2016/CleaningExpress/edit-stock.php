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
				<div class="main-container inner">
					<!-- start: PAGE -->
					<div class="main-content">
						<div class="container">
							<div class="row eItem">
								<div class="col-md-12 col-sm-12 col-xs-12 stock">
									<div class="pull-left">
										<a href="view-stock.php"><h3><i class="fa fa-angle-left"></i> Edit Stock Request</h3></a>
									</div>	
								</div>
							</div>
							<div class="row aItem cHide"> 
								<div class="col-md-12 col-sm-12 col-xs-12 stock">
									<div class="pull-left">
										<h3><i class="fa fa-angle-left"></i> Add Item</h3>
									</div>	
								</div>
							</div>
							<div id="eStockItems">
								<form role="form" class="form-horizontal" method="post" action="">
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-1">
											Required Date
										</label>
										<div class="col-xs-8 col-sm-9 col-md-9">
											<input type="date" name="date" class="form-control" id="form-field-1" disabled>
										</div>
									</div>
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-1">
											Project
										</label>
										<div class="col-xs-8 col-md-9 col-sm-9">
											<select id="form-field-select-1" class="form-control" disabled>
												<!-- <option value="1">Project 1</option>
												<option value="2">Project 2</option>
												<option value="3">Project 3</option>
												<option value="3">Project 4</option>
												<option value="3">Project 5</option> -->
											</select>
										</div>
									</div>
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-1">
											Warehouse
										</label>
										<div class="col-xs-8 col-md-9 col-sm-9">
											<select id="form-field-select-2" class="form-control" disabled>
												<!-- <option value="1">HQ Warehouse</option>
												<option value="2">HQ Warehouse 2</option>
												<option value="3">HQ Warehouse 3</option>
												<option value="4">HQ Warehouse 4</option>
												<option value="5">HQ Warehouse 5</option> -->
											</select>
										</div>
									</div>
								</form>
							<div class="container mrt-20">	
								<div class="row fontsize">
										<div class="col-md-2 col-sm-2 col-xs-hidden"></div>
										<div class="col-md-5 col-sm-4 col-xs-2 itemcode">Itemcode</div>
										<div class="col-md-5 col-sm-6 col-xs-9 marginqtyEdit">
											<span class="availableqty">Available Top Up</span>
											<span class="orderqty">Req. Qty</span>
											<span class="uomH">UOM</span>
										</div>
								</div>
								<br/>
								<div class="row">
							<div class="col-md-12 col-sm-12 col-xs-12">	
								<div id="scrollbox3">
								<div class="itemList">
									<!--div class="list">
										<table class="table-responsive">
										  <tr>
											<td class="minus"><i class="fa fa-minus-circle"></i></td>
											<td class="sname">SG1001</td>
											<td class="items pull-left">100</td>
											<td><input type="text" size="1" value="80" class="qty" /></td>
										  </tr>
										  <tr>
											<td></td>
											<td class="sname1">wet pipes 50pcs/pack</td>
											<td></td>
											<td></td>
										  </tr>
										</table>	
									</div>
									<div class="list">
										<table class="table-responsive">
										  <tr>
											<td class="minus"><i class="fa fa-minus-circle"></i></td>
											<td class="sname">SG1002</td>
											<td class="items pull-left">200</td>
											<td><input type="text" size="1" value="50" class="qty" /></td>
										  </tr>
										  <tr>
											<td></td>
											<td class="sname1">wet pipes 50pcs/pack</td>
											<td></td>
											<td></td>
										  </tr>
										</table>	
									</div>
									<div class="list">
										<table class="table-responsive">
										  <tr>
											<td class="minus"><i class="fa fa-minus-circle"></i></td>
											<td class="sname">SG1003</td>
											<td class="items pull-left">50</td>
											<td><input type="text" size="1" value="90" class="qty" /></td>
										  </tr>
										  <tr>
											<td></td>
											<td class="sname1">wet pipes 50pcs/pack</td>
											<td></td>
											<td></td>
										  </tr>
										</table>	
									</div>
									<div class="list">
										<table class="table-responsive">
										  <tr>
											<td class="minus"><i class="fa fa-minus-circle"></i></td>
											<td class="sname"> SG1004</td>
											<td class="items pull-left">40</td>
											<td><input type="text" size="1" value="100" class="qty" /></td>
										  </tr>
										  <tr>
											<td></td>
											<td class="sname1">wet pipes 50pcs/pack</td>
											<td></td>
											<td></td>
										  </tr>
										</table>	
									</div-->
								</div>
								</div>
							</div>
									<div class="">
										
										<div class="btn-leftp">
											<button type="button" name="add" class="btn btn-info" id="add">Add Item</button>
											<button type="button" name="save" class="btn btn-success" id="save">Update</button>
											<button type="button" name="cancel" class="btn btn-danger" id="cancel">Cancel</button>
										</div>
										
									</div>
									</div>
									</div>
							</div>
						<!-- end: PAGE -->
							</div>
							<div id="remove-list" class="cHide">
								<!-- <div class="col-md-6 col-sm-6 col-xs-3 itemcode">Item Code</div>
								<div class="col-md-4 col-sm-5 col-xs-7">
									<span class="availableqty">Available Qty</span>
									<span class="uomH">UOM</span>
								</div> -->
								<div class="row fontsize">
									<div class="col-md-2 col-sm-1 col-xs-1">&nbsp;</div>
										<div class="col-md-6 col-sm-6 col-xs-2 itemcode">Itemcode</div>
										<div class="col-md-4 col-sm-4 col-xs-8 marginqtyEdit">
											<span class="availableqty">Available Top Up</span>
											<span class="uomH">UOM</span>
										</div>
								</div>
								<br/>
								<br/>
								<div class="rItems"></div>
								<div class="form-group">
									<button type="button" name="add" class="btn btn-info" id="addItem"> OK </button>
									<button type="button" name="cancel" class="btn btn-danger" id="cancelItem">Cancel</button>
								</div>
							</div>
						</div>	
					</div>
				</div>	
					<!-- end: MAIN CONTAINER -->
					<!--footer-->
					<?php include('include/footer.php'); ?>
			</div>		
		</div>			
			<!--footer-->
			<!-- end: FOOTER -->
			<!-- *** READ NOTE *** -->
			<!--div id="readNote">
				<div class="barTopSubview">
					<a href="#newNote" class="new-note button-sv"><i class="fa fa-plus"></i> Add new note</a>
				</div>
				<div class="noteWrap col-md-8 col-md-offset-2">
					<div class="panel panel-note">
						<div class="e-slider owl-carousel owl-theme">
							<div class="item"></div>
						</div>
					</div>
				</div>
			</div-->
			<!-- *** SHOW CALENDAR *** -->
			<!--div id="showCalendar" class="col-md-10 col-md-offset-1">
				<div class="barTopSubview">
					<a href="#newEvent" class="new-event button-sv" data-subviews-options='{"onShow": "editEvent()"}'><i class="fa fa-plus"></i> Add new event</a>
				</div>
				<div id="calendar"></div>
			</div-->
			<!-- end: SUBVIEW SAMPLE CONTENTS -->
		
		<?php include("include/foot-script.php"); ?>
		<script type="text/javascript" src="js/edit-stock.js"></script>
		<script>$("#stock-request").addClass('active');</script>
		<script>$("#tree1").addClass('treeview active');</script>
	</body>
	<!-- end: BODY -->
</html>