<div id="fade" class="black-mask cHide"></div>
<div id="loader" class="cHide"><img src="assets/images/custom_loading1.gif"/></div>
<!-- start: TOPBAR -->
<header class="topbar navbar navbar-inverse navbar-fixed-top inner">
	<!-- start: TOPBAR CONTAINER -->
	<div class="container">
		<div class="navbar-header">
			<a class="sb-toggle-left" href="#main-navbar">
				<i class="fa fa-bars"></i>
			</a>
			<a class="navbar-brand" href="dashboard.php" id="headerTitle">
				Cleaning Express Pte Ltd
			</a>
		</div>
		<div class="topbar-tools">
			<ul class="nav navbar-right">
				<!-- start: USER DROPDOWN -->
				<li class="dropdown current-user">
					<a data-toggle="dropdown" data-hover="dropdown" class="dropdown-toggle" data-close-others="true" href="#">
						<img src="assets/images/anonymous.jpg" class="img-circle" alt="">
					</a>
					<ul class="dropdown-menu dropdown-dark">
						<li class="user-header">
							<img src="assets/images/anonymous.jpg" alt="" class="img-circle" />
							<p id="headerUser">Mr/Mrs.Shankar</p>
						</li>
						<li class="text-center">
							<a href="password.php">
								Change Password
							</a>
						</li>
						<li class="text-center">
							<a href="javascript:logout()">
								Log Out
							</a>
						</li>
					</ul>
				</li>
				<!-- end: USER DROPDOWN -->
			</ul>
		</div>
	</div>
	<!-- end: TOPBAR CONTAINER -->
</header>
<!-- end: TOPBAR -->
<script type="text/javascript">
	manageUser();
</script>