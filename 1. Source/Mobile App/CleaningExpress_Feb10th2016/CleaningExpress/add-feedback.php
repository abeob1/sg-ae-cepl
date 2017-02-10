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
		<link rel="stylesheet" type="text/css" href="assets/plugins/bootstrap/css/bootstrap.css">
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
									<a href="list-feedback.php"><h3><i class="fa fa-angle-left"></i> Add Customer Feedback</h3></a>
								</div>	
							</div>
						</div>
						<div class="questions">
							<div class="row">
								<div class="col-md-12 col-sm-12 col-xs-12">
									<p class="text-justify">Dear Business Partner</p>
									<p class="text-justify">Thank you for your continuous support. In an effort to serve you better, we seek your valuable feedback that will enable us to be a commendable service provider worthy of praise & customer satisfaction. We would greatly appreciate it if you could take some time to complete the following survey, which we estimate will take not more than 5 minutes to complete. Please be assured that all information provided will be kept strictly confidential at all times.</p>
									<!--p class="text-justify">We seek generosity to feedback on how we have performed so far.your response will greatly help us to continually improve our service.please tick in the appropriate box.</p-->
								</div>
							</div>
							<div class="row">
								<div class="col-md-15 col-sm-15 col-xs-15">
									<!-- <h5><span class="required">*</span> Required</h5> -->
									<div class="col-md-4 col-sm-4 col-xs-3">
									<h5><span class="required">*</span> Required</h5>
									</div>
									<div class="col-md-8 col-sm-8 col-xs-8">
									<h5 style="float:right;">1- Unsatisfactory 2- Satisfactory 3- Fair 4- Good 5- Excellent</h5>
									</div>
								</div>
								<!-- <div class="col-md-3 col-sm-3 col-xs-6">
									<h5><span class="cLeft">1-Good</span><span class="cRight">3-Poor</span></h5>
								</div> -->
								<div class="col-md-3 col-sm-3 col-xs-6">
									<!-- <h5><span style="padding-right:5px">Good</span><span style="padding-right:5px">Fair</span><span style="padding-right:5px">Satisfactory</span></h5> -->
									<h5></h5>
								</div>

							</div>
							<form role="form" class="form-horizontal" method="post" action="">
								<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-6">
										<div class="ques">
											<h5>Questions</h5>
										</div>
									</div>
									<div class="col-md-4 col-sm-5 col-xs-6">
										<h5 class="ques1">
											<ul class="checkbox-grid padd-top">
												<li><label for="text3"></label>1</li>
												<li><label for="text4"></label>2</li>
												<li><label for="text5"></label>3</li>
												<li><label for="text6"></label>4</li>
												<li><label for="text7"></label>5</li>
											</ul>
										</h5>
									</div>
								</div>
								
								<div class="fedscroll ">
									<div class="fQuestions"></div>
										<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<h5>Innovating to Improve!  </h5>
											<span>(In an effort to serve you better, share with us your experience & innovative ideas for us to 'WOW’ you!)</span>
										</div>
										<!-- <div class="col-md-8 col-sm-7 col-xs-6">
											<span>(In an effort to serve you better, share with us your experience & innovative ideas for us to 'WOW’ you!)</span>
										</div> -->
										<div class="col-md-12 col-sm-12 col-xs-12">
											<!--<input class="form-control" type="text" id="feedText1" />-->
											<textarea class="form-control" maxlength="254" style="border-color:Black;" type = "text" id = "feedText1" cols="40" rows = "2"></textarea>
										</div>
									</div>
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<h5>Striving harder with YOUR Compliments!  </h5>
											<span>(Please share your feedback & compliments on our Site Crew / HQ staff so that we may improve further)</span>
										</div>
										<!-- <div class="col-md-8 col-sm-7 col-xs-6">
											<span>(Please share your feedback & compliments on our Site Crew / HQ staff so that we may improve further)</span>
										</div> -->
										
										<div class="col-md-12 col-sm-12 col-xs-12">
											<!--<input class="form-control" type="text" id="feedText2" />-->
											<textarea class="form-control" maxlength="254" style="border-color:Black;" type = "text" id = "feedText2" cols="40" rows = "2"></textarea>
										</div>
									</div>
									<br/>
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<h5>Would you recommend us to your stakeholders?<span class="required">*</span></h5>
										</div>
										<div class="col-md-12 col-sm-12 col-xs-12">
											<!-- <input class="form-control" type="text" id="feedText3" /> -->
											<select id="feedText3" class="form-control">
												<option value="">Select</option>
												<option value="yes">Yes</option>
												<option value="no">No</option>
												<option value="neutral">Neutral</option>
											</select>
										</div>
									</div>
									<br/>
									<div class="clearfix"></div>
									<div class="row">
										<div class="col-md-12 col-sm-12 col-xs-12">
											<h5>Any other feedback:</h5>
										</div>
										<div class="col-md-12 col-sm-12 col-xs-12">
											<!--<input class="form-control" type="text" id="remarks" />-->
											<textarea class="form-control" maxlength="254" style="border-color:Black;" type = "text" id = "remarks" cols="40" rows = "2"></textarea>
										</div>
									</div>
									
									<div class="clearfix"></div>

									<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-9">
										<h5><strong>Please provide us with the appended details</strong></h5>
									</div>
								</div>
								<div class="row">
									<div class="col-md-7 col-sm-7 col-xs-6">
										<h5>Name:<span class="required">*</span></h5>
									</div>
									<div class="col-md-5 col-sm-5 col-xs-6 fedtext">
										<input class="form-control" type="text" id="NameFeed"/>
									</div>
								</div>
								<div class="row">
									<div class="col-md-7 col-sm-7 col-xs-6">
										<h5>Project Site:</h5>
									</div>
									<div class="col-md-5 col-sm-5 col-xs-6 fedtext">
										<input class="form-control" type="text" id="projectFeed" disabled />
									</div>
								</div>
								<div class="row">
									<div class="col-md-7 col-sm-7 col-xs-6">
										<h5>Contact Number/s:<span class="required">*</span></h5>
									</div>
									<div class="col-md-5 col-sm-5 col-xs-6 fedtext">
										<input class="form-control" type="text" id="custNumFeed" maxlength="8"/>
									</div>
								</div>
								<br/>
								<div class="row">
									<div class="col-md-7 col-sm-7 col-xs-6">
										<h5>E-Signature:<span class="required">*</span></h5>
									</div>
									<div class="col-md-5 col-sm-5 col-xs-6 fedtext" style="text-align:center;">
										<!-- <input class="form-control" type="text" id="esign" maxlength="30"/> -->
										<div id="sigCapture" style="text-align:center" ><div id="esign"></div><i class="fa fa-refresh clear fedesign" id="clear" title="clear"></i></div>
									</div>
								</div>
								</div>
								<div class="clearfix"></div>
								<br/>
								<!-- <div class="col-md-12 col-sm-12 col-xs-12">
									<p class="text-justify">Do note that if the survey is not anonymous, the responses may be somewhat biased. Thus, do 
reconsider whether it’s really necessary for the respondent to provide personal details.</p>
								</div> -->
								<br/>
								<div class="text-center">
									<p>  We have come to the end of the Survey <br/>
									From all of us at <br />
									Cleaning Express Pte Ltd <br />
									Greenserve Landscape Pte Ltd <br />
									Express Pest Solutions</p>
									<p>
									Thank You!<br/>
									For your valuable time & support! <br/>
									</p>
								</div>
								<br />
								<div class="col-md-12 col-sm-12 col-xs-12">
									<div class="form-group">
										<button type="button" id="save" class="btn btn-info">Save Feedback</button>
										<button type="button" id="cancel" class="btn btn-danger" >Cancel</button>
									</div>
								</div>
								<div class="clearfix"></div>
							</form>
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
		<script>$("#customer-feedback").addClass('active');</script>
		<script>$("#tree3").addClass('treeview active');</script>
		<!-- Signature libs -->
		<!--[if lt IE 9]>
		<script type="text/javascript" src="js/jsign/flashcanvas.js"></script>
		<![endif]-->
		<script src="js/jsign/jSignature.min.js"></script>
		<script type="text/javascript" src="js/add-feedback.js"></script>
	</body>
	<!-- end: BODY -->
</html>