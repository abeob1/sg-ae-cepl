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
							<div class="col-md-6 col-sm-6 col-xs-5">
								<a href="dashboard.php"><h3><i class="fa fa-angle-left"></i> Stock Request</h3></a>
							</div>
							<div class="col-md-6 col-sm-6 col-xs-7">
								<div class="pull-right header">
									<a href="dashboard.php"><i class="fa fa-backward"></i> Back</a>
									<a href="add-stock.php"><i class="fa fa-plus"></i>New</a>
								</div>
							</div>
						</div>
						<div class="clearfix"></div>
						<!--begin tabs going in wide content -->
					   <ul class="nav nav-tabs navtitle" id="mainTabs" role="tablist">
						  <li class="active"><a href="#open" role="tab" data-type="open" data-toggle="tab">Open</a></li>
						  <li><a href="#my" role="tab" data-toggle="tab" data-type="my">My</a></li>
						  <li><a href="#all" role="tab" data-toggle="tab" data-type="all">All</a></li>
					   </ul><!--/.nav-tabs.content-tabs -->
					   
					   <div class="tab-content">
						  <div class="tab-pane fade in active" id="open">
							<!-- <div class="list">
								<table class="table-responsive">
								  <tr>
									<td class="sname"><a href="view-stock.php">1001</a></td>
									<td></td>
								  </tr>
								  <tr>
									<td class="sname1">P001 Project</td>
									<td class="sname2">05 Jan 2015</td>
								  </tr>
								  <tr>
									<td class="sname1">Anthony Chen</td>
									<td class="sname2">Open</td>
								  </tr>
								</table>
							</div>
							<div class="list">
								<table class="table-responsive">
								  <tr>
									<td class="sname"><a href="view-stock.php">1002</a></td>
									<td></td>
								  </tr>
								  <tr>
									<td class="sname1">P002 Project</td>
									<td class="sname2">10 Jan 2015</td>
								  </tr>
								  <tr>
									<td class="sname1">Mike Orando</td>
									<td class="sname2">Open</td>
								  </tr>
								</table>
							</div> -->
						  </div><!--/.tab-pane -->

						  <div class="tab-pane fade" id="my">
							<!-- <div class="list">
								<table class="table-responsive">
								  <tr>
									<td class="sname"><a href="view-stock.php">1003</a></td>
									<td></td>
								  </tr>
								  <tr>
									<td class="sname1">P003 Project</td>
									<td class="sname2">15 Jan 2015</td>
								  </tr>
								  <tr>
									<td class="sname1">Anthony Chen</td>
									<td class="sname2">Open</td>
								  </tr>
								</table>
							</div>
							<div class="list">
								<table class="table-responsive">
								  <tr>
									<td class="sname"><a href="view-stock.php">1004</a></td>
									<td></td>
								  </tr>
								  <tr>
									<td class="sname1">P004 Project</td>
									<td class="sname2">20 Jan 2015</td>
								  </tr>
								  <tr>
									<td class="sname1">Mike Orando</td>
									<td class="sname2">Open</td>
								  </tr>
								</table>
							</div> -->
						  </div><!--/.tab-pane -->

						  <div class="tab-pane fade" id="all">
							 <!-- <div class="list">
								<table class="table-responsive">
								  <tr>
									<td class="sname"><a href="view-stock.php">1001</a></td>
									<td></td>
								  </tr>
								  <tr>
									<td class="sname1">P001 Project</td>
									<td class="sname2">05 Jan 2015</td>
								  </tr>
								  <tr>
									<td class="sname1">Anthony Chen</td>
									<td class="sname2">Open</td>
								  </tr>
								</table>
							</div>
							<div class="list">
								<table class="table-responsive">
								  <tr>
									<td class="sname"><a href="view-stock.php">1002</a></td>
									<td></td>
								  </tr>
								  <tr>
									<td class="sname1">P002 Project</td>
									<td class="sname2">10 Jan 2015</td>
								  </tr>
								  <tr>
									<td class="sname1">Mike Orando</td>
									<td class="sname2">Open</td>
								  </tr>
								</table>
							</div>
						<div class="list">
								<table class="table-responsive">
								  <tr>
									<td class="sname"><a href="view-stock.php">1003</a></td>
									<td></td>
								  </tr>
								  <tr>
									<td class="sname1">P003 Project</td>
									<td class="sname2">15 Jan 2015</td>
								  </tr>
								  <tr>
									<td class="sname1">Anthony Chen</td>
									<td class="sname2">Open</td>
								  </tr>
								</table>
							</div>
							<div class="list">
								<table class="table-responsive">
								  <tr>
									<td class="sname"><a href="view-stock.php">1004</a></td>
									<td></td>
								  </tr>
								  <tr>
									<td class="sname1">P004 Project</td>
									<td class="sname2">20 Jan 2015</td>
								  </tr>
								  <tr>
									<td class="sname1">Mike Orando</td>
									<td class="sname2">Open</td>
								  </tr>
								</table>
							</div> -->
						  </div><!--/.tab-pane -->
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
		<script type="text/javascript" src="js/stock-request.js"></script>
		<script>$("#stock-request").addClass('active');</script>
		<script>$("#tree1").addClass('treeview active');</script>
	</body>
	<!-- end: BODY -->
</html>