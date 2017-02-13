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
						<div class="stock">
							<div class="col-md-6 col-sm-6 col-xs-6">
								<a href="dashboard.php"><h3><i class="fa fa-angle-left"></i> Job Schedule List</h3></a>
							</div>
							<div class="col-md-6 col-sm-6 col-xs-6">
								<div class="pull-right header">
									<a href="dashboard.php"><i class="fa fa-backward"></i> Back</a>
								</div>
							</div>
						</div>
						<div class="clearfix"></div>
						<form role="form" class="form-horizontal" method="post" action="">
							<div class="form-group">
								<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-1">
									Project
								</label>
								<div class="col-xs-8 col-sm-9 col-md-9">
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

						<div class="projectList">
							<!-- <div class="list">
								<table class="table-responsive">
								  <tr>
									<td class="sname"><a href="view-calender.php">1001</a></td>
									<td></td>
								  </tr>
								  <tr>
									<td class="sname1">P001 Project1</td>
									<td class="sname2">Open</td>
								  </tr>
								</table>
							</div>
							<div class="list">
								<table class="table-responsive">
								  <tr>
									<td class="sname"><a href="view-calender.php">1002</a></td>
									<td></td>
								  </tr>
								  <tr>
									<td class="sname1">P002 Project2</td>
									<td class="sname2">Open</td>
								  </tr>
								</table>
							</div> -->
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
		<script>$("#job-schedule").addClass('active');</script>
		<script>$("#tree2").addClass('treeview active');</script>

		<script type="text/javascript" src="js/list-job.js"></script>
	</body>
	<!-- end: BODY -->
</html>