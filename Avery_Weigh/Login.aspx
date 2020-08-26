<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Avery_Weigh.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Avery Weigh Tronix - Login</title>
<meta name="viewport" content="width=device-width, initial-scale=1">
<link href="/css/style.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" href="/css/fonts.css" type="text/css" charset="utf-8" />
<link rel="icon" href="/images/favicon.png" type="image/gif" sizes="16x16" />
<link href="/Content/toastr.min.css" rel="stylesheet" />
<script type="text/javascript">
    function MM_swapImgRestore() { //v3.0
        var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
    }
    function MM_preloadImages() { //v3.0
        var d = document; if (d.images) {
            if (!d.MM_p) d.MM_p = new Array();
            var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
                if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; }
        }
    }

    function MM_findObj(n, d) { //v4.01
        var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
            d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
        }
        if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
        for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
        if (!x && d.getElementById) x = d.getElementById(n); return x;
    }

    function MM_swapImage() { //v3.0
        var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
            if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
    }
</script>
    <script src="/scripts/averyJs/jquery.min.js"></script>
    <script src="/js/Login.js"></script>
    <script src="/Scripts/toastr.min.js"></script>
    <script type="text/javascript">
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    </script>
    <style type="text/css">
        .error{
            border: 2px solid red;
        }
        .ConatctIcons ul {
    list-style-type: none;
    margin: 0px;
    padding: 0px;
    width: 140px !important;
    float: right;
}
    </style>
    
</head>
<body onload="MM_preloadImages('images/contact/contact_hover.png','images/contact/info_hover.png','images/contact/login_hover.png','images/login_hover.png','images/login/wi_base_hover_rail.png','images/login/wi_view_hover_rail.png','images/login/wi_sense_hover_rail.png','images/login/wi_connect_hover_rail.png')">
    <form id="form1" runat="server" defaultfocus="UserName">
        <div class="Wrapper Login">

<div class="LoginLeftSec">
<ul>
<li><a href="javascript:void(0);" class="loginimagesmain" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image5','','/images/Type5/wi_base_hover.png',1)"><asp:Image ID="Image5" runat="server" ImageUrl=""  /></a></li>
<li><a href="javascript:void(0);" class="loginimagesmain" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image6','','/images/Type5/wi_view_hover.png',1)"><asp:Image ID="Image6" runat="server" ImageUrl=""  /></a></li>
<li><a href="javascript:void(0);" class="loginimagesmain" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image7','','/images/Type5/wi_sense_hover.png',1)"><asp:Image ID="Image7" runat="server" ImageUrl=""  /></a></li>
<li><a href="javascript:void(0);" class="loginimagesmain" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image8','','/images/Type5/wi_tag_hover.png',1)"><asp:Image ID="Image8" runat="server" ImageUrl="" /></a></li>
<li><a href="javascript:void(0);" class="loginimagesmain" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image9','','/images/Type5/wi_connect_hover.png',1)"><asp:Image ID="Image9" runat="server" ImageUrl="" /></a></li>
</ul>
</div>
<div class="LoginRightSec">
<div class="ConatctIcons">
<ul>
<li><a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image2','','/images/contact/info_hover.png',1)"><img src="/images/contact/info_normal.png" name="Image2" width="42" height="42" border="0" id="Image2" /></a></li>
<li><a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image1','','/images/contact/contact_hover.png',1)"><img src="/images/contact/contact_normal.png" name="Image1" width="42" height="42" border="0" id="Image1" /></a></li>
<li style="display:none;"><a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image3','','/images/contact/login_hover.png',1)"><img src="/images/contact/login_normal.png" name="Image3" width="42" height="42" border="0" id="Image3" /></a></li>
</ul></div>

<div class="LoginSec">
<div class="loginLeft">
<span><lable>Username</lable><input type="text" name="" tabindex="1" runat="server" autocomplete="off" placeholder="UserName" id="UserName" /></span>
<span><lable>Password</lable><input type="password" name="" tabindex="2" runat="server" autocomplete="off" placeholder="Password" id="Password" /></span>
<span><asp:Panel runat="server" ID="Panel1"><lable>MAC-ID</lable></asp:Panel><input type="text" name="" tabindex="2" runat="server" autocomplete="off" placeholder="" id="SystemMacID" readonly="true" /></span>

        
<%--<span class="forgetpass"><a tabindex="5" href="javascript:void(0);">Forgot password?</a></span>--%>
</div>
    
<div class="loginLeft">
<span><lable>WB ID</lable><input type="text" name="" tabindex="3" runat="server" placeholder="WB ID" id="WBID" /></span>
<span><lable>Plant ID</lable><input type="text" name="" tabindex =" 4" runat="server" placeholder="Plant ID" id="PlantID" /></span>
<span><asp:Panel runat="server" ID="Panel2"> <lable>LOCK ID</lable></asp:Panel><input type="text" name="" tabindex =" 4" runat="server" placeholder="" id="UnlockCode" /></span>

</div>
    
<div class="loginRight"></div>
<div class="login">
    <asp:LinkButton ID="BtnLogin" runat="server" OnClick="BtnLogin_Click" TabIndex="6"><img src="/images/login_normal.png" name="Image4" width="121" height="53" border="0" id="Image4" /></asp:LinkButton>
   <%-- <a href="javascript:void(0);" id="BtnLogin" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image4','','images/login_hover.png',1)"><img src="images/login_normal.png" name="Image4" width="121" height="53" border="0" id="Image4" /></a></div>--%>
</div>

</div>

</div>
            </div>
    </form>
</body>
</html>
