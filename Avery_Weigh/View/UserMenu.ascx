<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserMenu.ascx.cs" Inherits="Avery_Weigh.View.UserMenu" %>
<ul>
    <li><a href="Manual_Weightment" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image4','','/images/type1/weigh_select.png',1)">
        <img src="/images/type1/weigh_select.png" alt="Weigh" name="Image4" width="166" height="40" border="0" id="Image4" /></a></li>
    <li><a href="/ConfigurationMaster" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image5','','/images/type1/configure_hover.png',1)">
        <img src="/images/type1/configure_normal.png" name="Image5" width="166" height="40" border="0" id="Image5" /></a></li>
    <li><a href="/ManageMasters" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image6','','/images/type1/masters_hover.png',1)">
        <img src="/images/type1/masters_normal.png" name="Image6" width="166" height="40" border="0" id="Image6" /></a></li>
    <li><a href="/dashboardmaster" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image7','','/images/type1/dashboard_hover.png',1)">
        <img src="/images/type1/dashboard_normal.png" name="Image7" width="166" height="40" border="0" id="Image7" /></a></li>
    <li><a href="service.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image8','','/images/type1/service_hover.png',1)">
        <img src="/images/type1/service_normal.png" name="Image8" width="166" height="40" border="0" id="Image8" /></a></li>
    <li><a href="diagnostics.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image9','','/images/type1/diagnostics_hover.png',1)">
        <img src="/images/type1/diagnostics_normal.png" name="Image9" width="166" height="40" border="0" id="Image9" /></a></li>
</ul>
