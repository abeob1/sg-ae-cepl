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
						<!-- <div class="stock">
							<div class="col-md-6 col-sm-6 col-xs-6">
								<a href="stock-request.php"><h3 class=""><i class="fa fa-angle-left"></i> View Stock Request</h3></a>
							</div>
							<div class="col-md-6 col-sm-6 col-xs-6">
								<div class="pull-right header">
									<a href="stock-request.php"><i class="fa fa-backward"></i> Back</a>
									<a href="edit-stock.php"><i class="fa fa-exchange"></i> Edit</a>
								</div>
							</div>
						</div> -->
						<div class="row">
							<div class="col-md-12 col-sm-12 col-xs-12 stock">
								<div class="pull-left">
									<a href="stock-request.php"><h3 class=""><i class="fa fa-angle-left"></i> View Stock Request</h3></a>
								</div>	
								<div class="pull-right header">
									<a href="stock-request.php"><i class="fa fa-backward"></i> Back</a>
									<a href="edit-stock.php"><i class="fa fa-exchange"></i> Edit</a>
								</div>
							</div>
						</div>
						<div class="clearfix"></div>
							<div class="ProjectDetails">
								<!-- <table class="table-responsive">
									  <tr>
										<td class="view1">Project No.</td>
										<td class="desc1">1001</td>
									  </tr>
									  <tr>
										<td class="view1">Project Name</td>
										<td class="desc1">P001 Project 1</td>
									  </tr>
									  <tr>
										<td class="view1">Requested By</td>
										<td  class="desc1">Anthony Chen</td>
									  </tr>
									  <tr>
										<td class="view1">Required Date</td>
										<td  class="desc1">05 Jan 2015</td>
									  </tr>
									  <tr>
										<td class="view1">Status</td>
										<td  class="desc1">Open</td>
									  </tr>
								</table> -->
							</div>
							<div class="clearfix"></div>
							<br/>
							<!--div>
								<div class="col-md-6 col-sm-8 col-xs-5 itemcode">Item Code</div>
								<div class="col-md-4 col-sm-5 col-xs-7">
									<span class="orderqty">Req. Qty</span>
									<span class="uomH">UOM</span>
								</div>
							</div-->
							<table class="table-responsive">
								<tbody>
								<tr>
									<td class="vname1">Item Code</td>
									<td class="reqty">Req. Qty</td>
									<td class="uom1">UOM</td>
								</tr>
								</tbody>
							</table>
							<div class="clearfix"></div>
							<div class="itemDetails"> 
							
								<!-- <div class="view">
									<table class="table-responsive">
									  <tr>
										<td class="vname">SG1001</td>
										<td></td>
									  </tr>
									  <tr>
										<td class="vname1">Toilet Roll</td>
										<td class="vname2">20</td>
									  </tr>
									</table>
								</div>
								<div class="view">
									<table class="table-responsive">
									  <tr>
										<td class="vname">SG1002</td>
										<td></td>
									  </tr>
									  <tr>
										<td class="vname1">Wet wipes 50pcs/Pack</td>
										<td class="vname2">10</td>
									  </tr>
									</table>
								</div>
								<div class="view">
									<table class="table-responsive">
									  <tr>
										<td class="vname">SG1003</td>
										<td></td>
									  </tr>
									  <tr>
										<td class="vname1">Liquid Soap 30g/Bottle</td>
										<td class="vname2">35</td>
									  </tr>
									</table>
								</div>
								<div class="view">
									<table class="table-responsive">
									  <tr>
										<td class="vname">SG1004</td>
										<td></td>
									  </tr>
									  <tr>
										<td class="vname1">Air Wick 250mL/bottle</td>
										<td class="vname2">35</td>
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
		<script type="text/javascript" src="js/view-stock.js"></script>
		<script>$("#stock-request").addClass('active');</script>
		<script>$("#tree1").addClass('treeview active');</script>
	</body>
	<!-- end: BODY -->
</html>