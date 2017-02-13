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
			<!--header-->
		<?php include('include/header.php'); ?>
		<!--header-->
	
		<div class="main-wrapper">
			<!-- Sidebar -->
				<?php include("include/sidebar.php"); ?>
			<!-- sidebar -->
			<!-- start: MAIN CONTAINER -->
			<div class="main-container inner">
				<!-- start: PAGE -->
				<div class="main-content">
					<div class="container">
						<div class="col-md-12 col-sm-12 col-xs-12 stock">
							<div class="pull-left">
								<a href="stock-request.php"><h3><i class="fa fa-angle-left"></i>Pest Management Service Report</h3></a>
							</div>	
							<div class="pull-right header">
								<a href="dashboard.php"><i class="fa fa-backward"></i> Back</a>
								<a href="add-epspest.php" class="addeps cHide"><i class="fa fa-plus"></i>New</a>
							</div>
						</div>
						<div class="clearfix"></div>
						<!--h3 class="stock"><i class="fa fa-angle-left"></i> Pending Good Issue</h3-->
							<form role="form" class="form-horizontal" method="post" action="">
								<div class="form-group">
									<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-1">
										Bill To
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
							</form>	
							<div class="container mrt-20">	
								<div class="row">
									<div class="col-md-12 col-sm-12 col-xs-12">	
											<div class="row">
												<div class="col-md-3 col-sm-3 col-xs-2"><strong>Doc Num</strong></div>
												<div class="col-md-3 col-sm-3 col-xs-3"><strong>Doc Date</strong></div>
												<div class="col-md-3 col-sm-3 col-xs-3 mrgl-15"><strong>Supervisor Name</strong></div>
												<div class="col-md-3 col-sm-3 col-xs-4"><strong>Client Name</strong></div>
											</div>
											<div class="clearfix"></div>
											<div id="scrollbox3">
											<div class="projectList">
								
											</div>
												<!-- <div class="list">		
													<div class="row">
														<div class="col-md-3 col-sm-3 col-xs-3">1001</div>
														<div class="col-md-3 col-sm-3 col-xs-3">23 Apr 2015</div>
														<div class="col-md-3 col-sm-3 col-xs-3"><a href="view-epspest.php">Navin Krishna</a></div>
														<div class="col-md-3 col-sm-3 col-xs-3">John</div>
													</div>	
												</div>
												<div class="list">		
													<div class="row">
														<div class="col-md-3 col-sm-3 col-xs-3">1001</div>
														<div class="col-md-3 col-sm-3 col-xs-3">23 Apr 2015</div>
														<div class="col-md-3 col-sm-3 col-xs-3"><a href="view-epspest.php">Navin Krishna</a></div>
														<div class="col-md-3 col-sm-3 col-xs-3">John</div>
													</div>	
												</div> -->
											<div class="clearfix"></div>	
										</div>
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
		<script>$("#epspest").addClass('active');</script>
		<script>$("#tree2").addClass('treeview active');</script>

		<script type="text/javascript" src="js/list-epspest.js"></script>
	</body>
	<!-- end: BODY -->
</html>