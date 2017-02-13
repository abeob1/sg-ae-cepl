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
							<div class="col-md-12 col-sm-12 col-xs-12">
								<!--div class="pull-left">
									<h3><i class="fa fa-angle-left"></i> View Customer Feedback</h3>
								</div-->	
								<!--div class="stock">
									<div class="col-md-6 col-sm-6 col-xs-6">
										<h3><i class="fa fa-angle-left"></i> View Customer Feedback</h3>
									</div>
									<div class="col-md-6 col-sm-6 col-xs-6">
										<div class="pull-right header">
											<a href="dashboard.php"><i class="fa fa-backward"></i> Back</a>
										</div>
									</div>
								</div-->
								<div class="col-md-12 col-sm-12 col-xs-12 stock">
									<div class="pull-left">
										<h3><i class="fa fa-angle-left"></i> View Customer Feedback</h3>
									</div>	
									<div class="pull-right header">
										<a href="dashboard.php"><i class="fa fa-backward"></i> Back</a>
									</div>
								</div>
							</div>
						</div>
						<div class="questions">
							<div class="row">
								<div class="col-md-12 col-sm-12 col-xs-12">
									<p class="text-justify">Dear Business Partner</p>
									<p class="text-justify">Thank you for your continuous support. In order to serve you better & to continue improving our services, we seek your inputs to be a commendable service provider worthy of praise & customer satisfaction. To accomplish this, your inputs are essential. Please give us 7 minutes of your time to help raise our benchmarks.</p>
									<!--p class="text-justify">We seek generosity to feedback on how we have performed so far.your response will greatly help us to continually improve our service.please tick in the appropriate box.</p-->
								</div>
							</div>
							<div class="row">
								<div class="col-md-6 col-sm-6 col-xs-6">
									<h5>3-Satisfied</h5>
								</div>
								<div class="col-md-6 col-sm-6 col-xs-6">
									<h5>1-Dissatisfied</h5>
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
											<ul class="checkbox-grid">
												<li><label for="text3"></label>3</li>
												<li><label for="text4"></label>2</li>
												<li><label for="text5"></label>1</li>
											</ul>
										</h5>
									</div>
								</div>
								<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-6">
										<h4>SITE CREW COMPETENCY</h4>
									</div>
								</div>
								<div class="fedscroll fQuestions">
									<!-- <div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Response Effectiveness (in emergency) <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Service Effectiveness <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Output Quality <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Safety Practices <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
								</div>
								<div class="clearfix"></div>
								<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-6">
										<h4>STAFF COMPETENCY</h4>
									</div>
								</div>
								<div class="fedscroll">
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Cheerful & Courteous <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Attentive & Proactive <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Efficient & Helpful <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Able to cope under pressure <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Good & Quality work delivered <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Punctual <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>Open to Feedback & Suggestions <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
									<div class="row">
										<div class="col-md-8 col-sm-7 col-xs-6">
											<h5>HQ Staff - Professionalism & Efficiency <span class="required">*</span></h5>
										</div>
										<div class="col-md-4 col-sm-5 col-xs-6">
											<ul class="checkbox-grid">
												<li><input type="checkbox" name="text1" value="value1" /><label for="text1"></label></li>
												<li><input type="checkbox" name="text2" value="value2" /><label for="text2"></label></li>
												<li><input type="checkbox" name="text3" value="value3" /><label for="text3"></label></li>
											</ul>
										</div>
									</div>
								</div>	
								<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-6">
										<h5>Innovating to Improve!  <span class="required">*</span></h5>
									</div>
									<div class="col-md-4 col-sm-5 col-xs-6 fedtext">
										<input class="form-control" type="text" name="text1" id="text1" />
									</div>
								</div>
								<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-6">
										<h5>Striving harder with YOUR Compliments!  <span class="required">*</span></h5>
									</div>
									<div class="col-md-4 col-sm-5 col-xs-6 fedtext">
										<input class="form-control" type="text" name="text2" id="text2" />
									</div>
								</div>
								<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-6">
										<h5>Would you recommend us to your stakeholders?   <span class="required">*</span></h5>
									</div>
									<div class="col-md-4 col-sm-5 col-xs-6 fedtext">
										<input class="form-control" type="text" name="text3" id="text3" />
									</div>
								</div> -->
							</div>
								<div class="clearfix"></div>
								<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-6">
										<h5><strong>Please provide with the appended details</strong></h5>
									</div>
								</div>
								<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-6">
										<h5>Name:</h5>
									</div>
									<div class="col-md-4 col-sm-5 col-xs-6 fedtext">
										<input class="form-control" id="NameFeed" />
									</div>
								</div>
								<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-6">
										<h5>Project Site:</h5>
									</div>
									<div class="col-md-4 col-sm-5 col-xs-6 fedtext">
										<input class="form-control" type="text" id="projectFeed" disabled/>
									</div>
								</div>
								<div class="row">
									<div class="col-md-8 col-sm-7 col-xs-6">
										<h5>Contact Number/s:</h5>
									</div>
									<div class="col-md-4 col-sm-5 col-xs-6 fedtext">
										<input class="form-control" type="text" id="custNumFeed" />
									</div>
								</div>
								<div class="col-md-12 col-sm-12 col-xs-12">
									<div class="form-group">
										<button type="button" name="save" class="btn btn-info">Save Feedback</button>
										<button type="button" name="cancel" class="btn btn-danger" >Cancel</button>
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

		<script type="text/javascript" src="view-feedback.js"></script>
	</body>
	<!-- end: BODY -->
</html>