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
									<h3><i class="fa fa-angle-left"></i> Change Password</h3>
								</div>	
								<div class="pull-right header">
									<a href="dashboard.php"><i class="fa fa-backward"></i> Back</a>
								</div>
							</div>
						</div>
						<div class="row">
							<div class="col-md-12 col-sm-12 col-xs-12">
								<form role="form" class="form-horizontal" method="post" action="">
									<div class="form-group">
										<label class="col-xs-4 col-sm-2 col-md-2 control-label" for="form-field-1">
											Password
										</label>
										<div class="col-xs-7 col-sm-9 col-md-9">
											<input type="password" class="form-control" id="oldPwd">
										</div>
									</div>
									<div class="form-group">
										<label class="col-xs-4 col-sm-2 col-md-2 control-label" for="form-field-1">
											New Password
										</label>
										<div class="col-xs-7 col-sm-9 col-md-9">
											<input type="password" class="form-control" id="newPwd">
										</div>
									</div>
									<div class="form-group">
										<label class="col-xs-4 col-sm-2 col-md-2 control-label" for="form-field-select-1">
											Confirm Password
										</label>
										<div class="col-xs-7 col-md-9 col-sm-9">
											<input type="password" class="form-control" id="confirmPwd">
										</div>
									</div>
									<h5 style="text-align:center;">Password is not case sensitive</h5>
									<div class="clearfix"></div>
									<div class="form-group">
										<div class="btn-leftp">
											<button type="button" name="save" class="btn btn-success" id="save">Save Password</button>
											<button type="button" name="cancel" class="btn btn-danger" id="cancel">Cancel</button>
										</div>
									</div>
								</form>	
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
		<script>$("#stock-request").addClass('active');</script>
		<script>$("#tree1").addClass('treeview active');</script>

		<script type="text/javascript" src="js/password.js"></script>
	</body>
	<!-- end: BODY -->
</html>