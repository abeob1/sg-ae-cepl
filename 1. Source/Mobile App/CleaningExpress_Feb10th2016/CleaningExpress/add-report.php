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
        <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>
        <script src="js/angular.js"></script>		
        <script type="text/javascript" src="js/file_upload.js"></script>
        <script type="text/javascript">
		function filechange(data)
		{
        	var as =  angular.element(data).scope();
			as.$apply(as.uploadFile(data.files))
		}
		</script>
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
			<div class="main-container inner" ng-app="myApp" id="angular" ng-controller="MainCtrl" >
				<!-- start: PAGE -->
				<div class="main-content">
					<div class="container">
						<div class="row">
							<div class="col-md-12 col-sm-12 col-xs-12 stock">
								<div class="pull-left">
									<a href="search-report.php"><h3><i class="fa fa-angle-left"></i>Add ShowRound</h3></a>
								</div>
								<div class="pull-right header">
									<a href="search-report.php"><i class="fa fa-backward"></i> Back</a>
									
								</div>		
							</div>
						</div>
						<div class="reportItems">
								<!-- <div class="list mrt-20">
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<h4 class="sname">A.Prospect</h4>
											<form role="form" class="form-horizontal" method="post" action="">
												<div class="form-group">
													<label class="col-xs-12 col-sm-12 col-md-12" for="form-field-1">
														Name
													</label>
												</div>
												<div class="form-group">
													<label class="col-xs-4 col-sm-2 col-md-2" for="form-field-1">
														Nos
													</label>
													<div class="col-xs-7 col-sm-9 col-md-9">
														<input type="text" name="name" id="name" class="form-control textform">
													</div>
												</div>
												<div class="form-group">
													<label class="col-xs-4 col-sm-2 col-md-2" for="form-field-1">
														Description
													</label>
													<div class="col-xs-7 col-sm-9 col-md-9">
														<input type="text" name="name" id="name" class="form-control">
													</div>
												</div>
											</form>
										</div>
									</div>
								</div>
								<div class="list">
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<h4 class="sname">A.Prospect</h4>
											<form role="form" class="form-horizontal" method="post" action="">
												<div class="form-group">
													<label class="col-xs-12 col-sm-12 col-md-12" for="form-field-1">
														Address
													</label>
												</div>
												<div class="form-group">
													<label class="col-xs-4 col-sm-2 col-md-2" for="form-field-1">
														Nos
													</label>
													<div class="col-xs-7 col-sm-9 col-md-9">
														<input type="text" name="name" id="name" class="form-control">
													</div>
												</div>
												<div class="form-group">
													<label class="col-xs-4 col-sm-2 col-md-2" for="form-field-1">
														Description
													</label>
													<div class="col-xs-7 col-sm-9 col-md-9">
														<input type="text" name="name" id="name" class="form-control">
													</div>
												</div>
											</form>
										</div>
									</div>
								</div>
								<div class="list">
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<h4 class="sname">C.Building Internal</h4>
											<form role="form" class="form-horizontal" method="post" action="">
												<div class="form-group">
													<label class="col-xs-12 col-sm-12 col-md-12" for="form-field-1">
														No.of Storeys
													</label>
												</div>
												<div class="form-group">
													<label class="col-xs-4 col-sm-2 col-md-2" for="form-field-1">
														Nos
													</label>
													<div class="col-xs-7 col-sm-9 col-md-9">
														<input type="text" name="name" id="name" class="form-control">
													</div>
												</div>
												<div class="form-group">
													<label class="col-xs-4 col-sm-2 col-md-2" for="form-field-1">
														Description
													</label>
													<div class="col-xs-7 col-sm-9 col-md-9">
														<input type="text" name="name" id="name" class="form-control">
													</div>
												</div>
											</form>
										</div>
									</div>
								</div> -->
						</div>
						
						<div class="remarkField"  >
				
							<table class="table-responsive">
								<tr>
									<td  class="viewrep">Remarks</td>
								  <td class="viewrep1">
									<!--<input type="text" name="remarks" id="remarks" class="form-control remarks"> -->
									<textarea style="width:97%;border-color:Grey;" type="text" name="remarks" id="remarks"  class="form-control remarks"ols="" rows="2"></textarea></td>
								</tr>
								<tr>
								<td style="height:15px;"></td>
								</tr>
								<tr>
									<td class="viewrep">Attachment</td>
									<td class="viewrep1">
										<!-- <input type="file" name="upload" id="upload" class="remarks" style="padding-top:5px;" > -->
                                      <form name="folderfrm" action="" method="POST" enctype="multipart/form-data" id="upform"  class="folder-form">
                                         <input  style="width:97%; height:50px" type="file" id="fileUpload"  onchange="filechange(this);" ng-model="files" name="fileUpload" ng-disabled="isDisabled" multiple>
                                            <input type="hidden"  name="companyname" ng-model ="companyname" class="companyname"/>
									</form>
									</td>
									</tr>
								</table>
                                     
                                            <!-- in case of error show error message with file name-->                                         
                                            <table style="width:100%" cellpadding="5px"class="the-table table-responsive">
                                              <tr>
                                                <td ></td>
                                                <td>Name</td>
                                                <td >Remarks</td>                                                
                                                <td >Status</td>                                                
                                              </tr>
                                              <tr ng-repeat="file in ts" style="height:45px">
                                               <td style=" background-color:#FFFFFF;  vertical-align: top; padding-left:30px">
             <button style="background-color:#FFFFFF" type="button" name="del" ng-click="removeRow($index)"><span class="minus" title="Remove" ><i class="fa fa-minus-circle"></i></span></button>            </td>
                                                <td>{{file.FileName}}</span></td>
                                                <td><input style="width:55%" type="text" id="remarks" ng-model="file.Remarks" ng-keypress="getValue()"></td>                                                 
                                                <td style="vertical-align: top; padding-left:20px"><i ng-show="file.status" style="color:#009900" class="fa fa-check"></i><i ng-show="file.loading" class="fa fa-refresh fa-spin"></i><i ng-hide="file.status || file.loading" class="fa fa-times"> </i></td>
                                               	
                                              </tr>
                                            </table>                                            
									<!--	<form name="folderfrm" action="" method="POST" enctype="multipart/form-data" id="upform" class="folder-form">
											<input type="file" id="fileAttach" name="fileName" class="form-control remarks upform" multiple/>
											<input type="hidden"  name="companyname" class="companyname"/>
										</form>-->
								
						</div>

							<div class="form-group">
                          
								<button type="button" name="attach" ng-click="att();" class="btn btn-primary" ng-disabled="attachfiles"  >Attach</button>
								<!-- <button id="save" class="btn btn-success btn-block">Save</button> -->
                               
								<button type="button"  id="save" name="save"   class="btn btn-success" ng-disabled="savefiles" >Save</button>
							</div>
					   </div><!--/.tab-content -->
					</div>
					
				</div>
				<!-- end: PAGE -->
			</div>
			<!-- end: MAIN CONTAINER -->
			<script type="text/javascript" src="js/add-report.js"></script>
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

		
	</body>
	<!-- end: BODY -->
</html