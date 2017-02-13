<?php
//echo nl2br(print_r($_FILES,1));

$WebURL ="E:\\LIVE\\TempAttachments\\";
$SAPURL = "C:\\Program Files (x86)\\SAP\\SAP Business One\\Attachments\\color-920150716131323.png";

$target_dir = "E:\LIVE\TempAttachments/";
		$fileName = $_FILES["afile"]["name"];	
		$id = $_POST['id'];				
		$target_file = $target_dir . basename($fileName);		
		move_uploaded_file($_FILES["afile"]["tmp_name"], $target_file);	
		$rarr = array('name'=>$fileName,'web'=>$WebURL,'sap'=>$SAPURL,'id'=>$id);
		echo json_encode($rarr);	
//print_r($arr);




?>
