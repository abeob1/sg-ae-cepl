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
							<a href="list-project.php" class="addGoods"><h3 class="stock"><i class="fa fa-angle-left"></i> Perform Goods Issue</h3></a>
							<a href="javascript:void(0)" class="cHide addItem"><h3 class="stock"><i class="fa fa-angle-left"></i> Add item</h3></a>
							<div id="goodsDetail">
								<table class="table-responsive">
								  <tr>
									<td class="view1">Request No.</td>
									<td class="desc1">1001</td>
								  </tr>
								</table>
							</div>
						
							<div class="container mrt-20">
								<div class="row fontsize gHeader">
									<div class="col-md-2 col-sm-1 col-xs-1">&nbsp;</div>
										<div class="col-md-6 col-sm-6 col-xs-2 itemcode">Itemcode</div>
										<div class="col-md-4 col-sm-4 col-xs-8 marginqty">
											<span>Available Qty</span>
											<span style="padding-left:7px;">Issue Qty</span>
											<span class="uomH">UOM</span>
										</div>
								</div>
								<br/>
								<div class="row">
									<div class="col-md-12 col-sm-12 col-xs-12">
										<div id="goodsView">
											<div id="scrollbox3">
												<div class="goodsItems">
													<!-- <div class="list">
														<div class="row">
															<div class="col-md-6 col-sm-6 col-xs-6">
																<h4 class="sname">SG1001</h4>
																<h5 class="sname1">wet pipes 50pcs/pack</h5>
															</div>
															<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">
																<div class="items">100</div>
																<input type="text" size="1" value="80" />
															</div>
														</div>
													</div>
													<div class="list">
														<div class="row">
															<div class="col-md-6 col-sm-6 col-xs-6">
																<h4 class="sname">SG1002</h4>
																<h5 class="sname1">wet pipes 50pcs/pack</h5>
															</div>
															<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">
																<div class="items">200</div>
																<input type="text" size="1" value="50" />
															</div>
														</div>
													</div>
													<div class="list">
														<div class="row">
															<div class="col-md-6 col-sm-6 col-xs-6">
																<h4 class="sname">SG1003</h4>
																<h5 class="sname1">wet pipes 50pcs/pack</h5>
															</div>
															<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">
																<div class="items">50</div>
																<input type="text" size="1" value="90" />
															</div>
														</div>
													</div>
													<div class="list">
														<div class="row">
															<div class="col-md-6 col-sm-6 col-xs-6">
																<h4 class="sname">SG1004</h4>
																<h5 class="sname1">wet pipes 50pcs/pack</h5>
															</div>
															<div class="col-md-6 col-sm-6 col-xs-6 mrt-20">
																<div class="items">40</div>
																<input type="text" size="1" value="100" />
															</div>
														</div>
													</div> -->
												</div>
											</div>
											<div class="form-group">
												<div class="btn-leftp">
													<button type="button" name="addItem" class="btn btn-info" id="add">Add Item</button>
													<button type="button" name="save" class="btn btn-success" id="save">Save</button>
													<button type="button" name="cancel" class="btn btn-danger" id="cancel">Cancel</button>
												</div>
											</div>
										</div>
										<div id="itemView" class="cHide">
											<div class="row fontsize">
												<div class="col-md-2 col-sm-1 col-xs-1">&nbsp;</div>
													<div class="col-md-6 col-sm-6 col-xs-2 itemcode">Itemcode</div>
													<div class="col-md-4 col-sm-4 col-xs-8 marginqty">
														<span style="padding-left:15px;">Available Qty</span>
														<span class="uomH" style="padding-left:10px;">UOM</span>
													</div>
											</div>
											<br/>
											<br/>
											<div class="rItems">
												
											</div>
											<div class="form-group">
												<button type="button" name="addItem" class="btn btn-info" id="addItem"> OK </button>
												<button type="button" name="cancel" class="btn btn-danger" id="cancelItem">Cancel</button>
											</div>



										</div>
									</div>
								</div>
							</div>
						<!-- end: PAGE -->
						</div>
					</div>
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
		<script src="assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>
		<script src="assets/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js"></script>
		<script type="text/javascript" src="js/list-goods.js"></script>
		<script>$("#pending-goods").addClass('active');</script>
		<script>$("#tree1").addClass('treeview active');</script>
</body>
<!-- end: BODY -->
</html>