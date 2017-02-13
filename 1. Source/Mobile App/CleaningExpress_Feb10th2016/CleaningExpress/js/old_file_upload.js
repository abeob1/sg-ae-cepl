var app = angular.module('myApp',[]);
app.controller('MainCtrl', [ '$scope','$http',function($scope,$http){

console.log(CONFIG);
					
	$scope.session = angular.fromJson(sessionStorage["user"]);
	
	$scope.IsDisabled = false;
	$scope.attachfiles = true;
	$scope.savefiles = false; 
	$scope.ts = [];
	$scope.returndata =[];
	
	 $scope.uploadFile = function(files) { 
	 
         //$scope.Iserror=false;
		 var i;
			for(i=0;i< files.length;i++)
			{
		 		$scope.ts.push({"Company":$scope.session.CompanyCode,"FileName":files[i].name,"type":files[i].type,"file":files[i],"Remarks":"","status":false,"loading":false});
			}
		 
		// alert('testing');
		 console.log($scope.ts);
		 $scope.attachfiles = false;
	   	 $scope.savefiles = true;
		 }
		 
		 
		 
		  $scope.att = function()
	   {
		     
		    var i;
			var j=0;
		 for(i=0;i< $scope.ts.length;i++)
		 {
			 
			 
			 if(true)
				{
					$scope.$apply($scope.ts[i].status=false);
					 var file = $scope.ts[i].file;
						$scope.$apply($scope.ts[i].loading=true);
					  var fd = new FormData();
					  fd.append("afile", file);
					  // These extra params aren't necessary but show that you can include other data.
					  fd.append("id", i);
					  //fd.append("accountnum", 123456);
					
					  var xhr = new XMLHttpRequest();
					  //xhr.open('POST', CONFIG.OPERATIONS_BASEPATH+'/Attachments', true);
					  xhr.open('POST', CONFIG.BASEURL+'/upload.php', true);
		
					  
					  xhr.upload.onprogress = function(e) {
						if (e.lengthComputable) {
						  var percentComplete = (e.loaded / e.total) * 100;
						  //console.log(percentComplete + '% uploaded');
						}
					  };		
		
					
					  xhr.onload = function(response) {
						  
						   
						  
						if (this.status == 200) {
						var resp = JSON.parse(this.response);
						console.log(resp);
					
						 $scope.$apply($scope.ts[resp.id].status=true);
						 $scope.$apply($scope.ts[resp.id].loading=false);
						 j++;			 
						
						 
						 $scope.$apply($scope.ts[resp.id].WebURL = resp.web);
						 $scope.$apply($scope.ts[resp.id].SAPURL = resp.sap );
						  if($scope.ts.length==j)
			   					$scope.sendattachment();
							
						
						};
					  };
					
					  xhr.send(fd);
				}
				
			 }
			
		   
		 $scope.attachfiles = true;
	   	 $scope.savefiles = false;
		 
	   }
	   $scope.sendattachment = function() {
	  	$http({
    method: 'POST',
    url: CONFIG.OPERATIONS_BASEPATH+'/AttachmentsWithRemarks',
    data: "sJsonInput=" + JSON.stringify($scope.ts),
    headers: {'Content-Type': 'application/x-www-form-urlencoded'}
}).success(function(data){ $scope.returndata=data;
alert($scope.returndata[0].DisplayMessage);
sessionStorage["reportFile"] = JSON.stringify($scope.returndata[0].Attachments);
 })
					 .error(function(data)
					 {
					 	alert('error');
					
					 });
	   }



$scope.save = function() {	
	
	  	$http({
    method: 'POST',
    url: CONFIG.BASEPATH + "/MSave_ShowAround",
    data: "sJsonInput=" + JSON.stringify($scope.returndata),
    headers: {'Content-Type': 'application/x-www-form-urlencoded'}
}).success(function(data){ alert("ok"); })
					 .error(function(data)
					 {
					 	alert('error');
					
					 });
			$scope.IsDisabled = false;		 
	   }
	
	
  $scope.removeRow = function(index)
	{		
		$scope.ts.splice( index, 1 );		
	}


  
  
}]);

app.controller('ViewCtrl', [ '$scope','$http','$window',function($scope,$http,$window){									 
											 
																 
$scope.getFilename = function(ss)
	{
		var filename = "";
			if(ss != "")
			{
			filename = ss.substr(ss.lastIndexOf("\\")+1);
			}
		return filename;
	}
			 
	var url = CONFIG.BASEPATH + "/MGet_ShowAround_DocumentDetails";

	var companyUser = sessionStorage["user"];
	companyUser = JSON.parse(companyUser);
	
	$scope.mProjectData = {"sCompany":companyUser["CompanyCode"],"sDocEntry":sessionStorage["sDoc"]};	
	$http({
    method: 'POST',
    url: url,
    data:"sJsonInput=" + JSON.stringify($scope.mProjectData),
    headers: {'Content-Type': 'application/x-www-form-urlencoded'}
	}).success(function(data){ $scope.files= data[0].Attachments;
	console.log(data);})
	.error(function(data){alert('Server Connection error');});		


}]);




app.controller('EditCtrl', [ '$scope','$http','$window',function($scope,$http,$window){
													   
	//console.log(CONFIG);
					
	$scope.session = angular.fromJson(sessionStorage["user"]);
	
	$scope.changedata = function (data)
	{
		var i;
		for(i=0;i<data.length;i++)
		{
			data[i]['FileName']=$scope.getFilename(data[i]['WebURL']);
			data[i]['Company']=$scope.session.CompanyCode; //"Company":$scope.session.CompanyCode
		}
	}

													   
		
$scope.getFilename = function getFilename(ss)
	{
		if(angular.isUndefined(ss))
		{
			return "";
		}
		else
		{
		var filename = "";
		if(ss != ""){
		filename = ss.substr(ss.lastIndexOf("\\")+1);
		//alert(filename);
		}
		return filename;
		}
	}
					
			 
	var url = CONFIG.BASEPATH + "/MGet_ShowAround_DocumentDetails";
	var companyUser = sessionStorage["user"];
	companyUser = JSON.parse(companyUser);
	$scope.mProjectData = {"sCompany":companyUser["CompanyCode"],"sDocEntry":sessionStorage["sDoc"]};	
		$http({
		method: 'POST',
		url: url,
		data:"sJsonInput=" + JSON.stringify($scope.mProjectData),
	headers: {'Content-Type': 'application/x-www-form-urlencoded'}
	}).success(function(data){ $scope.files=data[0].Attachments ;
	console.log(data[0].Attachments);
	//$scope.changedata(data[0].Attachments)
		var i =0;
		for(i=0;i<data[0].Attachments.length;i++)
		{		
		data[0].Attachments[i].Company = $scope.session.CompanyCode;
		data[0].Attachments[i].FileName = $scope.getFilename(data[0].Attachments[i].WebURL);
		data[0].Attachments[i].DelFlag = "";
		data[0].Attachments[i].Choice = "A";
		data[0].Attachments[i].FileMethod = "old";
		}
	})
	
	.error(function(data){alert('Server Connection error');});
	
	
$scope.uploadFile = function(files) 
	 { 
		 var i;
	     for(i=0;i< files.length;i++)
			{
		 		$scope.files.push({"Company":$scope.session.CompanyCode,"FileName":files[i].name,"type":files[i].type,"file":files[i],"Remarks":"","status":false,"loading":false,"DelFlag":"","Choice":"A","FileMethod":"New"});
			}		 
		
		 console.log($scope.files);	
		 $scope.attachfiles = false;
	   	 $scope.savefiles = true;
	}
		 
$scope.removeEditRow = function(index)
	{				
		$scope.deletefiles(index);	
	}
	
$scope.deletefiles = function(index)
	{
		//console.log($scope.files[index]);
		if($scope.files[index].FileMethod == 'old')
			{			   
				$scope.$apply($scope.files[index].DelFlag = "Y");
				$scope.$apply($scope.files[index].Choice = "Y");
			}
			else
			{
				//$scope.files[index].DelFlag = "N";
				//scope.files[index].Choice = "N";
				$scope.files.splice( index, 1 );
			}	
			console.log(index);
			console.log($scope.files);
	}
	

	
$scope.att = function()
	{	
			//alert('working');
			console.log($scope.files.length);
		    var i;
			var j=0;
		 for(i=0;i< $scope.files.length;i++)
		 	{
			  console.log($scope.files[i].file);
 			  var file = $scope.files[i].file;
			  $scope.$apply($scope.files[i].loading=true);
			  console.log(file);
			  var fd = new FormData();
			  fd.append("afile", file);			
			  fd.append("id", i);			
			  var xhr = new XMLHttpRequest();			  
			  xhr.open('POST', CONFIG.BASEURL+'/upload.php', true);  
			  xhr.upload.onprogress = function(e) {
				  
					if (e.lengthComputable) {					
					  var percentComplete = (e.loaded / e.total) * 100;
					  console.log(percentComplete + '% uploaded');
					}
					
			  };				
			  xhr.onload = function(response) { 			   
				  
					if (this.status == 200) {
					var resp = JSON.parse(this.response);
					
					$scope.$apply($scope.files[resp.id].status=true);
					$scope.$apply($scope.files[resp.id].loading=false);
					j++;
					if($scope.files[resp.id].FileMethod=='New')
					{
						$scope.$apply($scope.files[resp.id].WebURL = resp.web);
						$scope.$apply($scope.files[resp.id].SAPURL = resp.sap );
					}
					if($scope.files.length==j)
				  		 $scope.sendattachment();
					
					};
			  };
			
			  xhr.send(fd);
			 }
				
		   
		 $scope.attachfiles = true;
	   	 $scope.savefiles = false;
		 
	}
	   
	   
$scope.sendattachment = function() 
	{
			$http({
		method: 'POST',
		url: CONFIG.OPERATIONS_BASEPATH+'/AttachmentsWithRemarks',
		data: "sJsonInput=" + JSON.stringify($scope.files),
		headers: {'Content-Type': 'application/x-www-form-urlencoded'}
		}).success(function(data){ $scope.returndata=data;
		console.log($scope.$apply($scope.returndata));
		alert($scope.returndata[0].DisplayMessage);
		sessionStorage["reportFileEdit"] = JSON.stringify($scope.returndata[0].Attachments);
		 }).error(function(data){alert('error');});
		
	}



$scope.save = function() 
	{	
	
			$http({
		method: 'POST',
		url: CONFIG.BASEPATH + "/MSave_ShowAround",
		data: "sJsonInput=" + JSON.stringify($scope.returndata),
		headers: {'Content-Type': 'application/x-www-form-urlencoded'}
		}).success(function(data){ alert("ok"); 
		})
		.error(function(data){alert('error');});
				$scope.IsDisabled = false;		 
	}
	
				
	
	
																 
}]);