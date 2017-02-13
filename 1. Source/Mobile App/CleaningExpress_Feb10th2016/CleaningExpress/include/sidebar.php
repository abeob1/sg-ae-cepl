	<!-- start: PAGESLIDE LEFT -->
		<a class="closedbar inner hide" href="#">
		</a>
			<nav id="pageslide-left" class="pageslide inner">
				<div class="navbar-content sidebar">
					<!-- start: SIDEBAR -->
					<div class="main-navigation left-wrapper transition-left">
						<div class="user-profile border-top padding-horizontal-10 block">
							<div class="inline-block">
								<!--h4 class="no-margin">CEPL MOBILE</h4-->
								<img id="companySlogo" src="" alt="" class="img-responsive"  />
							</div>
						</div>
						<!-- start: MAIN NAVIGATION MENU -->
						<ul class="main-navigation-menu sidebar-menu" id="mainlist">
							<li id="tree" class="treeview">
								<a id="lnk_dashbrd" href="dashboard.php"><i class="fa fa-home"></i> <span class="title"> Dashboard </span></a>
							</li>
							<li id="tree1" class="treeview">
								<a id="lnk_Inventory" href="javascript:void(0)" class="sInventory"><i class="fa fa-desktop"></i> <span class="title"> Inventory </span><i class="icon-arrow"></i> </a>
								<ul class="sub-menu treeview-menu">
									<li id="stock-request">
										<a id="lnk_stckreq" href="stock-request.php">
											<i class="fa fa-rocket"></i>  Stock Request
										</a>
									</li>
									<li id="pending-goods">
										<a id="lnk_PG" href="javascript:clearProject()">
											<i class="fa fa-tasks"></i>  Pending Goods Issue
										</a>
									</li>
								</ul>
							</li>
							<li id="tree2" class="treeview">
								<a id="lnk_Opr" href="javascript:void(0)" class="sOperations"><i class="fa fa-cogs"></i> <span class="title">Operations</span><i class="icon-arrow"></i> </a>
								<ul id="ul_submenu" class="sub-menu treeview-menu">
									<li id="job-schedule">
										<a id="lnk_JS" href="list-job.php">
											<i class="fa fa-calendar"></i>  <span class="title">Job schedule </span>
										</a>
									</li>
									
									<li id="inspection">
										<a id="lnk_insQA" href="list-inspection.php">
											<i class="fa fa-check-square-o"></i>  <span class="title">Inspection/QA</span>
										</a>
									</li>
									<li id="gardener">
										<a id="lnk_GSL" href="list-gardener.php">
											<i class="fa fa-tree"></i>  <span class="title">GSL Gardener Checklist</span>
										</a>
									</li>
									<li id="landscape">
										<a id="lnk_GSLLandscape" href="list-landscape.php">
											<i class="fa fa-fire"></i>  <span class="title">GSL Landscape Checklist</span>
										</a>
									</li>
									<li id="epspest">
										<a id="lnk_EPS" href="list-epspest.php">
											<i class="fa fa-recycle"></i>  <span class="title">EPS Pest Management Service Report</span>
										</a>
									</li>
									<!-- <li id="work-order">
										<a href="#">
											<i class="fa fa-gavel"></i>  <span class="title">Work order</span>
										</a>
									</li> -->
								</ul>
							</li>
							<li id="tree3" class="treeview">
								<a id="lnk_MKT" href="javascript:void(0)" class="sMarketing"><i class="fa fa-th-large"></i> <span class="title"> Marketing </span><i class="icon-arrow"></i> </a>
								<ul id="List_MKT" class="sub-menu treeview-menu">
									<li id="customer-feedback">
										<a id="lnk_feedback"  href="list-feedback.php">
											<i class="fa fa-file-text"></i>  Customer Feedback
										</a>
									</li>
									<li id="show-around">
										<a id="lnk_SR" href="search-report.php">
											<i class="fa fa-database"></i>  ShowRound
										</a>
									</li>
									<li id="inspection1">
										<a id="lnk_insQA" href="list-inspectionForSales.php">
											<i class="fa fa-check-square-o"></i>  <span class="title">Inspection/QA</span>
										</a>
									</li>
								</ul>
							</li>
							
							
							<li id="tree4" class="treeview">
								<a id="lnk_Print_Job" href="javascript:void(0)" class="sPrint"><i class="fa fa-desktop"></i> <span class="title">Report</span><i class="icon-arrow"></i> </a>
								<ul class="sub-menu treeview-menu">
								<li id="print-job">
										<a id="lnk_JS" href="print-job.php">
											<i class="fa fa-calendar"></i>  <span class="title">Print Job Completion Form</span>
										</a>
								</li>
								<li id="print-monthly-report">
										<a id="lnk_JS" href="monthly-job-report.php">
											<i class="fa fa-calendar"></i>  <span class="title">Monthly Job Schedule Report</span>
										</a>
								</li>
								</ul>
							</li>
							
							<li id="tree5" class="treeview">
								<a href="javascript:void(0)" class="sAccount"><i class="fa fa-pencil-square-o"></i> <span class="title">Account</span><i class="icon-arrow"></i> </a>
								<ul class="sub-menu treeview-menu">
									<li id="settings">
										<a href="setting.php">
											<i class="fa fa-cog"></i>  Settings
										</a>
									</li>
									<li id="">
										<a href="password.php">
											<i class="fa fa-adjust"></i>  Change Password
										</a>
									</li>
									<li>
										<a href="javascript:logout()">
											<i class="fa fa-arrow-circle-o-up"></i>  Logout
										</a>
									</li>
								</ul>
							</li>
							
						</ul>
						<!-- end: MAIN NAVIGATION MENU -->
					</div>
					<!-- end: SIDEBAR -->
				</div>
			</nav>
			<!-- end: PAGESLIDE LEFT -->

			<script type="text/javascript">
				roleMatrix();
					ModifyListValues();
					
						function ModifyListValues(){

	if(sessionStorage.user){

		var companyUser = JSON.parse(sessionStorage.user);

		if(companyUser.Theme == "1" ){
		 // alert($("#mainlist #tree").length);
		
		$("#mainlist #gardener").hide();
		$("#mainlist #landscape").hide();
		$("#mainlist #epspest").hide();
		if(companyUser.RoleName != "Sales")
			{
				$("#mainlist #inspection1").hide();
			}
			else
			{
				$("#mainlist #inspection1").show();
			}
		}
		else if(companyUser.Theme == "2"){
		$("#mainlist #gardener").hide();
		$("#mainlist #epspest").hide();
		if(companyUser.RoleName != "Sales")
			{
				$("#mainlist #inspection1").hide();
			}
			else
			{
				$("#mainlist #inspection1").show();
			}
		}
		else if(companyUser.Theme == "3"){
		
		$("#mainlist #gardener").hide();
		$("#mainlist #landscape").hide();
		if(companyUser.RoleName != "Sales")
			{
				$("#mainlist #inspection1").hide();
			}
			else
			{
				$("#mainlist #inspection1").show();
			}		
		}
		else
		{}
	}
	
}
			</script>