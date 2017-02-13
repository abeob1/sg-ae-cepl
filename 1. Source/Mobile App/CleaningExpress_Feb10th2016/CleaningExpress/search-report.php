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
						<div class="row">
							<div class="col-md-12 col-sm-12 col-xs-12 stock">
								<div class="pull-left">
									<a href="dashboard.php"><h3><i class="fa fa-angle-left"></i> ShowRound</h3></a>
								</div>	
								<div class="pull-right header">
									<a href="dashboard.php"><i class="fa fa-backward"></i> Back</a>
									<a href="add-report.php"><i class="fa fa-plus"></i>New</a>
								</div>
							</div>
						</div>
						<div class="clearfix"></div>
							<form role="form" class="form-horizontal" method="post" action="">
								<div class="input-group">
									<input type="text" id="search" class="form-control">
									<span class="input-group-btn">
										<button type="button" class="btn btn-danger" id="search-btn"><i class="fa fa-search"></i></button>
									</span>
								</div>
							</form>
							<div class="showlistdocs search-scroll">
								<!-- <div class="list mrt-20">
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<div class="sname"><a href="view-report.php">10004</a></div>
											<div class="report">25 Dec 2014</div>
										</div>
									</div>
								</div>
								<div class="list">
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<div class="sname"><a href="view-report.php">10003</a></div>
											<div class="report">25 Dec 2014</div>
										</div>
									</div>
								</div>
								<div class="list">
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<div class="sname"><a href="view-report.php">10002</a></div>
											<div class="report">25 Dec 2014</div>
										</div>
									</div>
								</div>
								<div class="list">
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<div class="sname"><a href="view-report.php">10001</a></div>
											<div class="report">25 Dec 2014</div>
										</div>
									</div>
								</div> -->
							</div>
							<div class="showListErr cHide mrt-20">
								<div> No Records to Display </div>
							</div>
					   </div><!--/.tab-content -->
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
		<script>$("#show-around").addClass('active');</script>
		<script>$("#tree3").addClass('treeview active');</script>

		<script type="text/javascript" src="js/search-report.js"></script>
	</body>
	<!-- end: BODY -->
</html>