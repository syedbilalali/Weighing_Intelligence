<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServiceMaster.aspx.cs" Inherits="Avery_Weigh.ServiceMaster" %>

<%@ Register Src="~/View/Header.ascx" TagPrefix="uc1" TagName="Header" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Avery Weigh Tronix - Weigh</title>
<link href="css/style.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" href="css/fonts.css" type="text/css" charset="utf-8" />
<link rel="icon" href="images/favicon.png" type="image/gif" sizes="16x16">
<script src="js/jquery.min.js"></script>
<script type="text/javascript">
function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}
function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}
</script>
<script>
$(document).ready(function(){
 $(".MainPopBtn").click(function(){
	 $(".Popup").show();
	 });
	 $(".Close").click(function(){
	 $(".Popup").hide();
	 });
	 	
});
</script>
</head>
<body onload="MM_preloadImages('images/contact/contact_hover.png','images/contact/info_hover.png','images/contact/login_hover.png','images/close_hover.png','images/fullscreen_hover.png','images/exit_fullscreen_hover.png','images/type3/copy_hover.png','images/type3/previous_page_hover.png','images/type3/previous_record_hover.png','images/type3/next_record_hover.png','images/type3/next_page_hover.png','images/masters/vehicle_classification_hover.png')">
<div class="Wrapper WeighMain">
<!--Header start here -->
<uc1:Header runat="server" ID="Header" />
<!--Header start here-->


<!--MiddelSection Starts -->
<div class="MiddleSection">
<div class="LeftSec">
<ul>
<li><a href="UWS-AUTO-WEIGHMENT.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image4','','images/type1/weigh_hover.png',1)"><img src="images/type1/weigh_normal.png" alt="Weigh" name="Image4" width="166" height="40" border="0" id="Image4" /></a></li>
<li><a href="/ConfigurationMaster" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image5','','images/type1/configure_hover.png',1)"><img src="images/type1/configure_normal.png" name="Image5" width="166" height="40" border="0" id="Image5" /></a></li>
<li><a href="Masters-supplier-search.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image6','','images/type1/masters_select.png',1)"><img src="images/type1/masters_select.png" name="Image6" width="166" height="40" border="0" id="Image6" /></a></li>
<li><a href="/dashboardmaster" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image7','','images/type1/dashboard_hover.png',1)"><img src="images/type1/dashboard_normal.png" name="Image7" width="166" height="40" border="0" id="Image7" /></a></li>
<li><a href="service.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image8','','images/type1/service_hover.png',1)"><img src="images/type1/service_normal.png" name="Image8" width="166" height="40" border="0" id="Image8" /></a></li>
<li><a href="diagnostics.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image9','','images/type1/diagnostics_hover.png',1)"><img src="images/type1/diagnostics_normal.png" name="Image9" width="166" height="40" border="0" id="Image9" /></a></li>
</ul>
<span><img src="images/intellegence-left-logo.png" alt="" /></span>
</div>

<div class="RightSec">
<div class="RightInn11">
<h4>MASTERS</h4>
</div>
<div class="RightInn11">
<!-- <div class="RightInn22">3333</div> -->
<div class="MastersPageMasterDiv">

<div class="MastersPageContent" >
<h5>HEADING HERE</h5>
<ul>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_A_1','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_A_1" width="144" height="55" border="0" id="v_c_A_1" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_A_2','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_A_2" width="144" height="55" border="0" id="v_c_A_2" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_A_3','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_A_3" width="144" height="55" border="0" id="v_c_A_3" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_A_4','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_A_4" width="144" height="55" border="0" id="v_c_A_4" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_A_5','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_A_5" width="144" height="55" border="0" id="v_c_A_5" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_A_6','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_A_6" width="144" height="55" border="0" id="v_c_A_6" /></a></li>
</ul>
</div>

<div class="MastersPageContent" >
<h5>HEADING HERE</h5>
<ul>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_B_1','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_B_1" width="144" height="55" border="0" id="v_c_B_1" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_B_2','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_B_2" width="144" height="55" border="0" id="v_c_B_2" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_B_3','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_B_3" width="144" height="55" border="0" id="v_c_B_3" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_B_4','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_B_4" width="144" height="55" border="0" id="v_c_B_4" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_B_5','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_B_5" width="144" height="55" border="0" id="v_c_B_5" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_B_6','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_B_6" width="144" height="55" border="0" id="v_c_B_6" /></a></li>
</ul>
</div>

<div class="MastersPageContent" >
<h5>HEADING HERE</h5>
<ul>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_C_1','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_C_1" width="144" height="55" border="0" id="v_c_C_1" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_C_2','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_C_2" width="144" height="55" border="0" id="v_c_C_2" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_C_3','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_C_3" width="144" height="55" border="0" id="v_c_C_3" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_C_4','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_C_4" width="144" height="55" border="0" id="v_c_C_4" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_C_5','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_C_5" width="144" height="55" border="0" id="v_c_C_5" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_C_6','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_C_6" width="144" height="55" border="0" id="v_c_C_6" /></a></li>
</ul>
</div>

<div class="MastersPageContent" >
<h5>HEADING HERE</h5>
<ul>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_D_1','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_D_1" width="144" height="55" border="0" id="v_c_D_1" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_D_2','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_D_2" width="144" height="55" border="0" id="v_c_D_2" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_D_3','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_D_3" width="144" height="55" border="0" id="v_c_D_3" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_D_4','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_D_4" width="144" height="55" border="0" id="v_c_D_4" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_D_5','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_D_5" width="144" height="55" border="0" id="v_c_D_5" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_D_6','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_D_6" width="144" height="55" border="0" id="v_c_D_6" /></a></li>
</ul>
</div>

<div class="MastersPageContent" >
<h5>HEADING HERE</h5>
<ul>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_E_1','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_E_1" width="144" height="55" border="0" id="v_c_E_1" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_E_2','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_E_2" width="144" height="55" border="0" id="v_c_E_2" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_E_3','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_E_3" width="144" height="55" border="0" id="v_c_E_3" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_E_4','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_E_4" width="144" height="55" border="0" id="v_c_E_4" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_E_5','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_E_5" width="144" height="55" border="0" id="v_c_E_5" /></a></li>
<li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('v_c_E_6','','images/masters/vehicle_classification_hover.png',1)"><img src="images/masters/vehicle_classification_normal.png" name="v_c_E_6" width="144" height="55" border="0" id="v_c_E_6" /></a></li>
</ul>
</div>


</div>
</div>
</div>

</div>
<!--MiddelSection Ends-->
<!--Footer Ends -->
<div class="footer">
<ul class="selected">
<li><a href="Masters-supplier-search.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image11','','images/type4/search_hover.png',1)"><img src="images/type4/search_normal.png" name="Image11" width="80" height="50" border="0" id="Image11" /></a></li>
<li><a href="Masters-supplier-edit.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image12','','images/type4/edit_hover.png',1)"><img src="images/type4/edit_normal.png" name="Image12" width="80" height="50" border="0" id="Image12" /></a></li>
<li><a href="Masters-supplier-save.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image13','','images/type4/save_hover.png',1)"><img src="images/type4/save_normal.png" name="Image13" width="80" height="50" border="0" id="Image13" /></a></li>
<li><a href="Masters-supplier-export.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Imag14','','images/type4/export_hover.png',1)"><img src="images/type4/export_normal.png" alt="Export" name="Imag14" width="80" height="50" border="0" id="Imag14" /></a></li>
<li><a href="Masters-supplier-import.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image15','','images/type4/import_hover.png',1)"><img src="images/type4/import_normal.png" name="Image15" width="80" height="50" border="0" id="Image15" /></a></li>
<li><a href="Masters-supplier-print.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image16','','images/type4/print_hover.png',1)"><img src="images/type4/print_normal.png" name="Image16" width="80" height="50" border="0" id="Image16" /></a></li>
<li><a href="Masters-supplier-delete.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image17','','images/type4/delete_hover.png',1)"><img src="images/type4/delete_normal.png" name="Image17" width="80" height="50" border="0" id="Image17" /></a></li>
</ul>
</div>
<!--Footer Ends-->
</div></body></html>
