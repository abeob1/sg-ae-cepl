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
							<a href="stock-request.php" class="addStock"><h3 class="stock"><i class="fa fa-angle-left"></i> Add Stock Request</h3></a>
							<a href="javascript:void(0)" class="cHide addItem"><h3 class="stock"><i class="fa fa-angle-left"></i> Add item</h3></a>
							<div id="stock-list">
								<form role="form" class="form-horizontal" method="post" action="">
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-1">
											Required Date
										</label>
										<div class="col-xs-8 col-sm-9 col-md-9">
											<input type="text" class="form-control" id="form-field-1">
										</div>
									</div>
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-select-1">
											Project
										</label>
										<div class="col-xs-8 col-md-9 col-sm-9">
											<select id="form-field-select-1" class="form-control">
												<!-- <option value="1">Project 1</option>
												<option value="2">Project 2</option>
												<option value="3">Project 3</option>
												<option value="3">Project 4</option>
												<option value="3">Project 5</option> -->
											</select>
										</div>
									</div>
									<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-select-2">
											Warehouse
										</label>
										<div class="col-xs-8 col-md-9 col-sm-9">
											<select id="form-field-select-2" class="form-control">
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
										<!--div class="col-md-12 col-sm-12 col-xs-12 itmclr">
											<div class="col-md-6 col-sm-6 col-xs-6">
												<span style="padding-left:25px;">Item Code</span>
											</div>
											<div class="col-md-6 col-sm-6 col-xs-6">
												<span style="margin-left:-20px;">Availbale Qty</span>
												<span style="padding-left:5px;">Order Qty</span>
											</div>
										</div-->
									</div>
									<br/>
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<div id="scrollbox3">
												<div class="projectList">
												<!-- <div class="list">
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
														<td class="sname">SG1003</td>
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
														<td class="minus">  <i class="fa fa-minus-circle"></i> </td>
														<td class="sname">SG1004</td>
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
												</div> -->
												</div>
											</div>
											<div class="form-group">
												<div class="btn-leftp">
													<button type="button" name="add" class="btn btn-info" id="add">Add Item</button>
													<button type="button" name="save" class="btn btn-success" id="save">Save</button>
													<button type="button" name="cancel" class="btn btn-danger" id="cancel">Cancel</button>
												</div>
											</div>
										</div>
									</div>
								
								</div>
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
											<span class="availableqty">Available Qty</span>
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
		<script src="assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>
		<script src="assets/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js"></script>
		<script>
			/*jQuery(document).ready(function() {
				Main.init();
				SVExamples.init();
				FormElements.init();
			});*/
		</script>
		<script type="text/javascript" src="js/add-stock.js"></script>
		<script>$("#stock-request").addClass('active');</script>
		<script>$("#tree1").addClass('treeview active');</script>
</body>
<!-- end: BODY -->
</html>