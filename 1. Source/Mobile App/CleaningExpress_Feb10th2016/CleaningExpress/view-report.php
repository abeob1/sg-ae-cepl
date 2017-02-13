<?php ?> 
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
          
         <script src="js/angular.js"></script>	
         <script type="text/javascript" src="js/file_upload.js"></script>	
        
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
			<div class="main-container inner" ng-app="myApp" id="angular" ng-controller="ViewCtrl">
				<!-- start: PAGE -->
				<div class="main-content">
					<div class="container">
						<div class="row">
							<div class="col-md-12 col-sm-12 col-xs-12 stock">
								<div class="pull-left">
									<a href="search-report.php" class="addStock"><h3><i class="fa fa-angle-left"></i> View ShowRound</h3></a>
								</div>	
								<div class="pull-right header">
									<a href="search-report.php"><i class="fa fa-backward"></i> Back</a>
									<a href="edit-report.php"><i class="fa fa-exchange"></i> Edit</a>
								</div>
							</div>
						</div>
						<div class="clearfix"></div>
						<div class="docdetails vReportDetails">
							<!-- <table class="table-responsive">
								<tr>
									<td class="view1">Document No.</td>
									<td class="desc1">10004</td>
								</tr>
								<tr>
									<td class="view1">Document Name</td>
									<td class="desc1">31 Jan 2015</td>
								</tr>
								<tr>
									<td class="view1">Owner</td>
									<td class="desc1">Anthony Chen</td>
								</tr>
							</table> --> 
						</div>	
						<div class="reportItems">
							<!-- <div class="view">
								<table class="table-responsive">
								  <tr>
									<td class="repname">A.Prospect</td>
									<td></td>
								  </tr>
								  <tr>
									<td class="viewrep">Name</td>
									<td class="viewrep1"></td>
								  </tr>
								  <tr>
									<td class="viewrep">Nos</td>
									<td class="viewrep1"></td>
								  </tr>
								  <tr>
									<td class="viewrep">Description</td>
									<td class="viewrep1">Jack Corner</td>
								  </tr>
								</table>
							</div>
							<div class="view">
								<table class="table-responsive">
								  <tr>
									<td class="repname">A.Prospect</td>
									<td></td>
								  </tr>
								  <tr>
									<td class="viewrep">Address</td>
									<td class="viewrep1"></td>
								  </tr>
								  <tr>
									<td class="viewrep">Nos</td>
									<td class="viewrep1"></td>
								  </tr>
								  <tr>
									<td class="viewrep">Description</td>
									<td class="viewrep1">Marina Bay Sands Hotel & Cosino</td>
								  </tr>
								</table>
							</div>
							<div class="view">
								<table class="table-responsive">
								  <tr>
									<td class="repname">C.Building Internal</td>
									<td></td>
								  </tr>
								 <tr>
									<td class="viewrep">No of Storeys</td>
									<td class="viewrep1"></td>
								  </tr>
								  <tr>
									<td class="viewrep">Nos</td>
									<td class="viewrep1">25</td>
								  </tr>
								  <tr>
									<td class="viewrep">Description</td>
									<td class="viewrep1"></td>
								  </tr>
								</table>
							</div>
					</div> -->
					</div>
					<div class="remarkField">
							<table class="table-responsive">
								<tr>
									<td class="viewrep">Remarks</td>
									<td class="viewrep1">
									<!--<input type="text" name="remarks" id="remarks" class="form-control remarks" disabled></td>-->
									<textarea type="text" name="remarks" id="remarks" class="form-control remarks" disabled cols="" rows=""></textarea>
								</tr>
								<tr>
									<td class="viewrep">Attachment</td>
									<!--<td class="viewrep1"><div id="atcEntry">
                                    </div></td>-->
                                    <td class="viewrep1">
                                 
                                    <table width="100%" border="0" class="table the-table"  >
                                      <tr>
                                        <td>Name</td>
                                        <td>Remarks</td>
                                      </tr>				               
                                       
                                      <tr ng-repeat="file in files" >
                                        <td><a href="http://203.125.57.116:3870/LIVE/CleaningExpress_WebService/Attachments/{{getFilename(file.WebURL)}}">{{getFilename(file.WebURL)}}</a></td>
                                        <td>{{file.Remarks}}</td>
                                      </tr>
                                     
                                    </table>
                                  
                                    </td>
								</tr>
							</table>
						</div>
				</div>
				<!-- end: PAGE -->
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
		<script type="text/javascript" src="js/view-report.js"></script>
      
		<script>$("#show-around").addClass('active');</script>
		<script>$("#tree3").addClass('treeview active');</script>

		
	</body>
	<!-- end: BODY -->
</html>