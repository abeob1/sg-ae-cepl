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
		<link rel="stylesheet" href="assets/plugins/BootStrap_DatePicker/css/datepicker.css">
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
					<div class="container" style="height:580px;">
						<div class="stock">
							<div class="col-md-6 col-sm-6 col-xs-6">
								<a href="dashboard.php"><h3><i class="fa fa-angle-left"></i>Monthly Job Report</h3></a>
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
							
							<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-1">
											Month
										</label>
										<div class="col-xs-8 col-sm-9 col-md-9">
										<!--	<input type="date" class="form-control" id="form-field-1">-->
										<select id="form-field-select-2" class="form-control">
											<option selected value=''>Select</option>
											<option value='January'>January</option>
											<option value='February'>February</option>
											<option value='March'>March</option>
											<option value='April'>April</option>
											<option value='May'>May</option>
											<option value='June'>June</option>
											<option value='July'>July</option>
											<option value='August'>August</option>
											<option value='September'>September</option>
											<option value='October'>October</option>
											<option value='November'>November</option>
											<option value='December'>December</option>
										</select>
										</div>
							</div>
							<div class="form-group">
										<label class="col-xs-3 col-sm-2 col-md-2" for="form-field-1">
											Year
										</label>
										<div class="col-xs-8 col-sm-9 col-md-9">
											<!--<input type="date" class="form-control" id="form-field-1">-->
											<select id="form-field-select-3" class="form-control">
											<!--<option selected="selected"  value="0">Select</option>
											<option value="2">Project 2</option>
											<option value="3">Project 3</option>
											<option value="3">Project 4</option>
											<option value="3">Project 5</option>-->
										</select>
										</div>
							</div>
							
							<div class="clearfix"></div>
									<div class="col-md-12 col-sm-12 col-xs-12">
										<div class="form-group pull-left">
											<div class="btn-leftp">
				                                <button type="button" name="print" class="btn btn-success" id="print">Print</button>
											</div>
										</div>
									</div>
							
						</form>	

					
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
		<script>$("#print-monthly-report").addClass('active');</script>
		<script>$("#tree4").addClass('treeview active');</script>

		<script type="text/javascript" src="js/monthly-job-report.js"></script>
		 
			
			//   
		<!--year-->
		<script>
		/*var start = 1900;
		var end = new Date().getFullYear();
		var options = "<option>Select</option>";
		for(var year = start ; year <=end; year++){
		  options += "<option>"+ year +"</option>";
		}
		document.getElementById("form-field-select-3").innerHTML = options;*/
		
			var time = new Date();
			var options = "<option selected value=''>Select</option>";
			
			var currentYear = new Date().getFullYear();
			var date = currentYear + 1; /*change the '101' to the number of years in the past you want to show */
			var pastYear = currentYear - 5; /*change the '100' to the number of years in the future you want to show */ 
			document.writeln ("<FORM><SELECT><OPTION value=\"\">Year");
			do {
			document.write (options += "<option>"+ date +"</option>");
			date--;
			}
			while (date > pastYear)
			document.getElementById("form-field-select-3").innerHTML = options;
		</script>		
	</body>
	<!-- end: BODY -->
</html>