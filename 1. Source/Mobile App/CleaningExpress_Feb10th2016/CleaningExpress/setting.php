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
							<div class="col-md-12 col-sm-12 col-xs-12">
								<!--div class="pull-left">
									<h3><i class="fa fa-angle-left"></i> Settings</h3>
								</div-->
								<div class="stock">
									<div class="col-md-6 col-sm-6 col-xs-6">
										<h3><i class="fa fa-angle-left"></i> Settings</h3>
									</div>
									<div class="col-md-6 col-sm-6 col-xs-6">
										<div class="pull-right header">
											<a href="dashboard.php"><i class="fa fa-backward"></i> Back</a>
										</div>
									</div>
								</div>
							</div>
						</div>
						<div class="clearfix"></div>
						<div class="row">
							<div class="col-md-12 col-sm-12 col-xs-12">
								<div class="setting">
									<div class="border-bottom">
										<h3>Configuration</h3>
									</div>
									<ul class="list-unstyled">
										<li>
											<div class="border-bott">
												<h4>url</h4>
												<p>http://0.0.0.0/prod</p>
											</div>
										</li>
										<li>
											<div class="border-bott">
												<h4>App Version</h4>
												<p>1.0</p>
											</div>
										</li>
										<li>
											<div class="border-bott">
												<h4>Release Date</h4>
												<p>01 Mar 2015</p>
											</div>
										</li>
									<!-- 	<li>
											<div class="border-bott">
												<h4>Expiry Date</h4>
												<p>01 Mar 2016</p>
											</div>
										</li> -->
										<!-- <li>
											<div class="border-bott">
												<h4>Min Android Version</h4>
												<p>4.0</p>
											</div>
										</li> -->
									</ul>
									<div class="border-bottom">
										<h3>Developer</h3>
									</div>
									<div class="border-bott">
										<h5>Abeo Electra(S) Pte Ltd</h5>
										<p>All Rights Reserved.</p>
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
		<script>$("#settings").addClass('active');</script>
		<script>$("#tree4").addClass('treeview active');</script>
	</body>
	<!-- end: BODY -->
</html>