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
		<!--meta name="apple-mobile-web-app-capable" content="yes">
		<meta name="apple-mobile-web-app-status-bar-style" content="black"-->
		<!-- end: META -->
		<!--head-script-->
		<?php include('include/head-script.php'); ?>
		<!--head-script-->
	</head>
	<!-- end: HEAD -->
	<!-- start: BODY -->
	<body class="top">
	<div class="main-wrapper">
		<div class="container">
			<div class="row">
				<div class="col-md-12 col-sm-12 col-xs-12">
					<div class="panel panel-white">
						<div class="panel-heading">
							<div class="text-center">
								<img src="assets/images/logo.jpg" alt="" class="img-responsive img-center" />
							</div>
						</div>
						<div class="panel-body">
							<form role="form" class="form-horizontal" method="" action="">
								<div class="form-group">
									<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-1">
										Username
									</label>
									<div class="col-xs-8 col-sm-9 col-md-9">
										<input type="text" placeholder="Please Enter Name" id="form-field-1" class="form-control">
									</div>
								</div>
								<div class="form-group">
									<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-2">
										Password
									</label>
									<div class="col-xs-8 col-sm-9 col-md-9">
										<input type="password" placeholder="Please Enter password" id="form-field-2" class="form-control">
									</div>
								</div>
								<div class="form-group">
									<label class="col-xs-3 col-sm-2 col-md-2 control-label" for="form-field-select-1">
										Company
									</label>
									<div class="col-xs-8 col-md-9 col-sm-9">
										<select id="form-field-select-1" class="form-control">
											<!-- <option value="">Select Company Name</option>
											<option value="1">Cleaning Express Pte Ltd</option>
											<option value="2">Greenserve & Landscape Pte Ltd</option>
											<option value="2">Express Pest Solutions</option> -->
										</select>
									</div>
								</div>
								<div class="form-group">
									<label class="col-xs-3 col-sm-2 col-md-2 control-label" for=""></label>
									<div class="col-md-9 col-sm-9 col-xs-8 text-center">
										<button type="button" class="btn btn-default btn-block" id="login">Login</button>
									</div>
									<div class="error"></div>
									<div class="col-md-10 cError cHide">
										Username or Password is Incorrect
									</div>
								</div>
							</form>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div id="fade" class="black-mask cHide"></div>
	<div id="loader" class="cHide"><img src="assets/images/custom_loading1.gif"/></div>
		<!--foot-script-->
		<?php include('include/foot-script.php'); ?>

		<!--foot-script-->
		<script type="text/javascript" src="js/index.js"></script>
	</body>
	<!-- end: BODY -->
</html>