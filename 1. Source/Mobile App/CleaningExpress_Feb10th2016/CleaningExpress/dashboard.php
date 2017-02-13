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
		<!--div class="loader">
			<img src="assets/images/loading.gif" alt="" />
		</div-->
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
						<div class="row">
							<div class="col-md-3 col-sm-3 col-xs-6 sInventory">
								<div class="box red">
									<div class="col-md-12 col-sm-12 col-xs-12">
										<a href="stock-request.php"><i class="fa fa-rocket"></i></a>
									</div>
									<div class="col-md-12 col-sm-12 col-xs-12 sInventory">
										<p><a href="stock-request.php">Stock Request</a></p>
									</div>
								</div>
							</div>
							<div class="col-md-3 col-sm-3 col-xs-6 sInventory">
								<div class="box blue">
									<div class="col-md-12 col-sm-12 col-xs-12">
										<a href="list-project.php"><i class="fa fa-tasks"></i></a>
									</div>
									<div class="col-md-12 col-sm-12 col-xs-12">
										<p><a href="list-project.php">Pending Goods Issue</a></p>
									</div>
								</div>
							</div>
							<div class="col-md-3 col-sm-3 col-xs-6 sOperations">
								<div class="box yellow">
									<div class="col-md-12 col-sm-12 col-xs-12">
										<a href="list-job.php"><i class="fa fa-calendar"></i></a>
									</div>
									<div class="col-md-12 col-sm-12 col-xs-12">
										<p><a href="list-job.php">Job Schedule</a></p>
									</div>
								</div>
							</div>
							<div class="col-md-3 col-sm-3 col-xs-6 sOperations">
								<div class="box violet">
									<div class="col-md-12 col-sm-12 col-xs-12">
										<a href="inspection.php"><i class="fa fa-check-square-o"></i></a>
									</div>
									<div class="col-md-12 col-sm-12 col-xs-12">
										<p><a href="list-inspection.php">Inspection</a></p>
									</div>
								</div>
							</div>
						</div>
					
						<div class="row">
							<!-- <div class="col-md-3 col-sm-3 col-xs-6">
								<div class="box lgreen">
									<div class="col-md-12 col-sm-12 col-xs-12">
										<a href="work-order.php"><i class="fa fa-gavel"></i></a>
									</div>
									<div class="col-md-12 col-sm-12 col-xs-12">
										<p><a href="work-order.php">Work Order</a></p>
									</div>
								</div>
							</div> -->
							<div class="col-md-3 col-sm-3 col-xs-6 sMarketing">
								<div class="box pink">
									<div class="col-md-12 col-sm-12 col-xs-12">
										<a href="list-feedback.php"><i class="fa fa-file-text"></i></a>
									</div>
									<div class="col-md-12 col-sm-12 col-xs-12">
										<p><a href="list-feedback.php">Customer Feedback</a></p>
									</div>
								</div>
							</div>
							<div class="col-md-3 col-sm-3 col-xs-6 sMarketing">
								<div class="box orange">
									<div class="col-md-12 col-sm-12 col-xs-12">
										<a href="search-report.php"><i class="fa fa-database"></i></a>
									</div>
									<div class="col-md-12 col-sm-12 col-xs-12">
										<p><a href="search-report.php">ShowRound</a></p>
									</div>
								</div>
							</div>
							<div class="col-md-3 col-sm-3 col-xs-6">
								<div class="box lblue">
									<div class="col-md-12 col-sm-12 col-xs-12">
										<a href="setting.php"><i class="fa fa-cog"></i></a>
									</div>
									<div class="col-md-12 col-sm-12 col-xs-12">
										<p><a href="setting.php">Settings</a></p>
									</div>
								</div>
							</div>
						</div>
						<!--div class="row">
							<div class="col-md-3 col-sm-3 col-xs-3">
								<div class="box lblue">
									<div class="col-md-12 col-sm-12 col-xs-12">
										<a href="setting.php"><i class="fa fa-home"></i></a>
									</div>
									<div class="col-md-12 col-sm-12 col-xs-12">
										<p><a href="setting.php">Settings</a></p>
									</div>
								</div>
							</div>
						</div-->
					</div>
					<div class="subviews">
						<div class="subviews-container"></div>
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
		<script type="text/javascript">
			$(window).load(function() {
				$(".loader").fadeOut("15000");
		
			})
		</script>
		<script>$("#tree").addClass('treeview active');
	
		</script>
		<script type="text/javascript">
			roleMatrix();
		</script>
	</body>
	<!-- end: BODY -->
</html>