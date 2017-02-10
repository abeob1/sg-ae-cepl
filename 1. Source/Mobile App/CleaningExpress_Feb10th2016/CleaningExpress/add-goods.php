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
							<h3 class="stock"><i class="fa fa-angle-left"></i> Add Stock Request</h3>
							<form role="form" class="form-horizontal" method="post" action="">
								<div class="form-group">
									<label class="col-xs-4 col-sm-2 col-md-2 control-label" for="form-field-1">
										Required Date
									</label>
									<div class="col-xs-8 col-sm-10 col-md-10">
										<input type="text" data-date-format="dd-mm-yyyy" data-date-viewmode="years" class="form-control">
									</div>
								</div>
								<div class="form-group">
									<label class="col-xs-4 col-sm-2 col-md-2 control-label" for="form-field-1">
										Project
									</label>
									<div class="col-xs-8 col-md-10 col-sm-10">
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
									<label class="col-xs-4 col-sm-2 col-md-2 control-label" for="form-field-1">
										Warehouse
									</label>
									<div class="col-xs-8 col-md-10 col-sm-10">
										<select id="form-field-select-1" class="form-control">
											<option value="1">HQ Warehouse</option>
											<option value="2">HQ Warehouse 2</option>
											<option value="3">HQ Warehouse 3</option>
											<option value="4">HQ Warehouse 4</option>
											<option value="5">HQ Warehouse 5</option>
										</select>
									</div>
								</div>
							</form>	
						
						<div class="container mrt-20">
							<div class="row">
								<div class="col-md-12 col-sm-12 col-xs-12">
									<div class="list">
										<div class="row">
											<div class="col-md-6 col-sm-6 col-xs-6">
												<h4 class="sname">SG1001</h4>
												<h5 class="sname1">wet pipes 50pcs/pack</h5>
											</div>
											<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">
												<div class="items">100</div>
												<input type="text" size="1" value="80" />
											</div>
										</div>
									</div>
									<div class="list">
										<div class="row">
											<div class="col-md-6 col-sm-6 col-xs-6">
												<h4 class="sname">SG1002</h4>
												<h5 class="sname1">wet pipes 50pcs/pack</h5>
											</div>
											<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">
												<div class="items">200</div>
												<input type="text" size="1" value="50" />
											</div>
										</div>
									</div>
									<div class="list">
										<div class="row">
											<div class="col-md-6 col-sm-6 col-xs-6">
												<h4 class="sname">SG1003</h4>
												<h5 class="sname1">wet pipes 50pcs/pack</h5>
											</div>
											<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">
												<div class="items">50</div>
												<input type="text" size="1" value="90" />
											</div>
										</div>
									</div>
									<div class="list">
										<div class="row">
											<div class="col-md-6 col-sm-6 col-xs-6">
												<h4 class="sname">SG1004</h4>
												<h5 class="sname1">wet pipes 50pcs/pack</h5>
											</div>
											<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">
												<div class="items">40</div>
												<input type="text" size="1" value="100" />
											</div>
										</div>
									</div>
									<div class="form-group">
										<button type="submit" name="submit" class="btn btn-info">Add Item</button>
										<button type="submit" name="submit" class="btn btn-success">Save</button>
										<button type="submit" name="submit" class="btn btn-danger">Cancel</button>
									</div>
								</div>
							</div>
						</div>
						<!-- end: PAGE -->
					</div>	
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
			jQuery(document).ready(function() {
				Main.init();
				SVExamples.init();
				FormElements.init();
			});
		</script>
		<script>$("#pending-goods").addClass('active');</script>
		<script>$("#tree1").addClass('treeview active');</script>
</body>
<!-- end: BODY -->
</html>