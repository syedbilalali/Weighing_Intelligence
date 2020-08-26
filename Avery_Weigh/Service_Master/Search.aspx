<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Avery_Weigh.Service_Master.Search" %>

<%@ Register Src="~/View/Header.ascx" TagPrefix="uc1" TagName="Header" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Avery Weigh Tronix - Weigh</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/css/fonts.css" type="text/css" charset="utf-8" />
    <link rel="icon" href="/images/favicon.png" type="image/gif" sizes="16x16" />
    <script src="/js/jquery.min.js"></script>
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
    <script>
        $(document).on("click", ".MainPopBtn", function () {
            var UserType = $(this).attr("data-id");
            debugger;
            $.ajax({
                type: "POST",
                url: "/AveryService/WebService1.asmx/Get_UserClassificationByUserType",
                data: "{'type':'" + UserType + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    var obj = JSON.parse(response.d);
                    console.log(obj);
                    $('#txtusertype').val(obj.UserType);
                    $('#txtfileupdation').val(obj.MasterFileUpdation);
                    $('#txtmasterrecorddeletion').val(obj.MasterRecordDeletion);
                    $('#txtpendingrecorddeletion').val(obj.PendingRecordDeletion);
                    $('#txtdeletion').val(obj.TransactionDeletion);
                    $('#txtconfiguration').val(obj.Configuration);
                    $('#txtpolicy').val(obj.PasswordPolicy);
                    $('#txtreset').val(obj.PasswordReset);
                    $('#txtcreation').val(obj.UserCreation);
                    $('#txtentry').val(obj.GateEntry);
                    $('#txtissue').val(obj.RFIDIssue);
                    $('#txtweighment').val(obj.Weighment);
                    $('#txtoperation').val(obj.DatabaseOperation);
                },
                error: function (response) {
                    console.log(response);
                }
            });
            $(".Popup").show();
        });
        $(document).on("click", ".Close", function () {
            $(".Popup").hide();
        });
    </script>
    <style type="text/css">
        .toast {
            opacity: 1 !important;
        }
    </style>
    <style>
        /* The Modal (background) */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        /* Modal Content */
        .modal-content {
            background-color: #fefefe;
            margin: auto;
            padding: 20px;
            border: 1px solid #888;
            width: 37%;
        }

        /* The Close Button */
        .close {
            color: #aaaaaa;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }
    </style>
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
        function isEmail(email) {
            var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return regex.test(email);
        }
    </script>

    <!-- jQuery Modal -->
</head>

<body onload="MM_preloadImages('/images/contact/contact_hover.png','/images/contact/info_hover.png','/images/contact/login_hover.png','/images/close_hover.png','/images/fullscreen_hover.png','/images/exit_fullscreen_hover.png','/images/type3/copy_hover.png','/images/type3/previous_page_hover.png','/images/type3/previous_record_hover.png','/images/type3/next_record_hover.png','/images/type3/next_page_hover.png')">
    <form id="form1" runat="server">

        <div class="Wrapper WeighMain">
            <!--Header start here-->
            <uc1:Header runat="server" ID="Header" />
            <!--Header start here-->

            <!--MiddelSection Starts-->
            <div class="MiddleSection">
                <div class="LeftSec">
                    <ul>
                        <li><a href="/Manual_Weighment.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image4','','/images/type1/weigh_hover.png',1)">
                            <img src="/images/type1/weigh_normal.png" alt="Weigh" name="Image4" width="166" height="40" border="0" id="Image4" /></a></li>
                        <li><a href="/ConfigurationMaster" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image5','','/images/type1/configure_hover.png',1)">
                            <img src="/images/type1/configure_normal.png" name="Image5" width="166" height="40" border="0" id="Image5" /></a></li>
                        <li><a href="/ManageMasters" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image6','','/images/type1/masters_select.png',1)">
                            <img src="/images/type1/masters_select.png" name="Image6" width="166" height="40" border="0" id="Image6" /></a></li>
                        <li><a href="/dashboardmaster" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image7','','/images/type1/dashboard_hover.png',1)">
                            <img src="/images/type1/dashboard_normal.png" name="Image7" width="166" height="40" border="0" id="Image7" /></a></li>
                        <li><a href="/ManageServices" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image8','','/images/type1/service_hover.png',1)">
                            <img src="/images/type1/service_normal.png" name="Image8" width="166" height="40" border="0" id="Image8" /></a></li>
                        <li><a href="diagnostics.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image9','','/images/type1/diagnostics_hover.png',1)">
                            <img src="/images/type1/diagnostics_normal.png" name="Image9" width="166" height="40" border="0" id="Image9" /></a></li>
                    </ul>
                    <span>
                        <img src="/images/intellegence-left-logo.png" alt="" /></span>
                </div>

                <div class="RightSec">
                    <div class="RightInn11 textcon">
                        <h4>MASTERS</h4>
                        <h4 class="SupplierSearchBig">SERVICE MASTER | SEARCH <a href="/ManageMasters" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image32','','images/close_hover.png',1)" class="CloseButtonA">
                            <img src="/images/close_normal.png" class="Close" alt="Close" name="Image32" width="26" height="25" border="0" id="Image32" /></a></h4>
                    </div>
                    <div class="RightInn11" style="overflow-y: auto;">
                        <div class="RightInn22">
                            <table width="200" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="center" style="padding: 10px 0px;">
                                        <img src="/images/searchicon.png" alt="" align="absmiddle" width="22" />
                                        SEARCH</td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <table width="200" border="0" cellspacing="0" cellpadding="0" style="margin-top: 20px;">
                                            <tr>
                                                <td align="left"><span>Criteria Type</span></td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <select class="DropDown" id="ddlSearch1" style="width: 85%;">
                                                        <option value="UserType">AMC Type</option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <input type="text" name="search1" id="search1" style="width: 80%; margin-top: 12px;" />
                                                </td>
                                            </tr>
                                        </table>
                                        &nbsp;<br />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="RightInn33">
                            <table id="tblsearch" border="0" cellspacing="0" cellpadding="0" width="100%">
                                <tr>
                                    <td class="TableDiv">
                                        <table border="0" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td align="right" width="110px;">AMC Type</td>
                                                <td>
                                                    <input type="text" name="" id="" value="" placeholder="" class="FilddHalf11" /></td>
                                            </tr>
                                            <tr>
                                                <td align="right">AMC Contact No</td>
                                                <td>
                                                    <input type="text" name="" id="" value="" placeholder="" class="FilddHalf11" /></td>
                                            </tr>
                                            <tr>
                                                <td align="right">AMC Reminder</td>
                                                <td>
                                                    <input type="text" name="" id="" value="" placeholder="" class="FilddHalf11" /></td>
                                            </tr>
                                            <tr>
                                                <td align="right">AMC Valid Upto</td>
                                                <td>
                                                    <input type="text" name="" id="" value="" placeholder="" class="FilddHalf11" /></td>
                                            </tr>
                                            <tr>
                                                <td align="right"></td>
                                                <td align="right">
                                                    <a href="javascript:void(0);" class="MainPopBtn" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image33','','/images/fullscreen_hover.png',1)">
                                                        <img src="/images/fullscreen_normal.png" name="Image33" width="26" height="26" border="0" id="Image33" /></a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="center">
                                        <img src="/images/spacer.png" alt="" width="10" /></td>
                                    <td class="TableDiv">
                                        <table border="0" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td align="right" width="110px;">AMC Type</td>
                                                <td>
                                                    <input type="text" name="" id="" value="" placeholder="" class="FilddHalf11" /></td>
                                            </tr>
                                            <tr>
                                                <td align="right">AMC Contact No</td>
                                                <td>
                                                    <input type="text" name="" id="" value="" placeholder="" class="FilddHalf11" /></td>
                                            </tr>
                                            <tr>
                                                <td align="right">AMC Reminder</td>
                                                <td>
                                                    <input type="text" name="" id="" value="" placeholder="" class="FilddHalf11" /></td>
                                            </tr>
                                            <tr>
                                                <td align="right">AMC Valid Upto</td>
                                                <td>
                                                    <input type="text" name="" id="" value="" placeholder="" class="FilddHalf11" /></td>
                                            </tr>
                                            <tr>
                                                <td align="right"></td>
                                                <td align="right"><a href="javascript:void(0);" class="MainPopBtn" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image31','','/images/fullscreen_hover.png',1)">
                                                    <img src="/images/fullscreen_normal.png" name="Image31" width="26" height="26" border="0" id="Image31" /></a></td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="height: 10px;">
                                        <img src="/images/spacer.png" alt="" /></td>
                                </tr>
                            </table>


                            <!--Popup-->
                            <div class="Popup">
                                <table border="0" cellspacing="0" cellpadding="2" class="PopupDiv">
                                    <tr>
                                        <td align="left" class="PopupHeader" style="color: #ffffff; padding-left: 15px;">SERVICE MASTER</td>
                                        <td align="right" class="PopupHeader"><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image32','','/images/close_hover.png',1)">
                                            <img src="/images/close_normal.png" class="Close" alt="Close" name="Image32" width="26" height="25" border="0" id="Image32" /></a></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding: 10px 0 20px 0px;">
                                            <table border="0" cellspacing="0" cellpadding="2">
                                                <tr>
                                                    <td align="right" width="110px;">AMC Type</td>
                                                    <td>
                                                        <input type="text" name="" id="txtamctype" value="" placeholder="" class="FilddHalf112" /></td>
                                                    <td align="right" width="110px;">AMC Contact No</td>
                                                    <td>
                                                        <input type="text" name="" id="txtamccontactno" value="" placeholder="" class="FilddHalf112" /></td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="110px;">AMC Reminder</td>
                                                    <td>
                                                        <input type="text" name="" id="txtamcreminder" value="" placeholder="" class="FilddHalf112" /></td>
                                                    <td align="right" width="110px;">AMC Valid Upto</td>
                                                    <td>
                                                        <input type="text" name="" id="txtamcvalidupto" value="" placeholder="" class="FilddHalf112" /></td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="110px;">Stamping Reminder</td>
                                                    <td>
                                                        <input type="text" name="" id="txtstampingreminder" value="" placeholder="" class="FilddHalf112" /></td>
                                                    <td align="right" width="110px;">Stamping Date</td>
                                                    <td>
                                                        <input type="text" name="" id="txtstampingdate" value="" placeholder="" class="FilddHalf112" /></td>
                                                </tr>                                                                                                                                                                                        
                                            </table>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <!--Popup-->


                        </div>
                    </div>
                </div>

            </div>
            <!--MiddelSection Ends-->



            <!--Footer Ends-->
            <div class="footer">
                <ul class="selected">
                    <li><a href="Search.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image11','','/images/type4/search_hover.png',1)">
                        <img src="/images/type4/search_normal.png" name="Image11" width="80" height="50" border="0" id="Image11" /></a></li>
                    <li>
                        <a href="AddEdit.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image12','','/images/type4/edit_hover.png',1)">
                            <img src="/images/type4/edit_normal.png" name="Image12" width="80" height="50" border="0" id="Image12" /></a></li>
                     <li><a href="javascript:void(0)" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image13','','/images/type4/save_hover.png',1)">
                        <img src="/images/type4/save_normal.png" name="Image13" width="80" height="50" border="0" id="Image13" /></a></li>
                    <li><a href="Import.aspx" id="myBtn" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Imag14','','/images/type4/export_hover.png',1)">
                        <img src="/images/type4/export_normal.png" alt="Export" name="Imag14" width="80" height="50" border="0" id="Imag14" /></a></li>
                    <li>
                        <a href="List.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image15','','/images/type4/import_hover.png',1)">
                        <img src="/images/type4/import_normal.png" name="Image15" width="80" height="50" border="0" id="Image15" /></a></li>
                    <li><a href="javascript:void(0)" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image16','','/images/type4/print_hover.png',1)">
                        <img src="/images/type4/print_normal.png" name="Image16" width="80" height="50" border="0" id="Image16" /></a></li>
                    <li>
                        <a href="javascript:void(0)" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image17','','/images/type4/delete_hover.png',1)">
                        <img src="/images/type4/delete_normal.png" name="Image17" width="80" height="50" border="0" id="Image17" /></a></li>
                </ul>
            </div>

        </div>
        <script type="text/javascript">  
            $(document).ready(function () {
                $("#search1").keyup(function () {
                    var search1 = $('#search1').val();
                    var drop1 = $('#ddlSearch1 option:selected').val();
                    console.log($(this).val());
                    $.ajax({
                        type: "POST",
                        url: "/AveryService/WebService1.asmx/Get_UserClassification",
                        data: "{'search1':'" + search1 + "','drop1':'" + drop1 + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (response) {
                            var obj = JSON.parse(response.d);
                            console.log(obj);
                            $("#tblsearch").empty();
                            var row = '';
                            var count = 0;
                            $.each(obj, function (key, value) {
                                if (count == 0) {
                                    $("#tblsearch").append('<tr>');
                                    row = '';
                                    row = row + '<td class="TableDiv">';
                                    row = row + '<table border="0" cellspacing="0" cellpadding="2">';
                                    row = row + '<tr><td align="right" width="110px;">AMC Type</td> <td><input type="text" name="" id="" value="' + value.AMCType + '" placeholder="" class="FilddHalf11" /></td></tr>';
                                    row = row + '<tr><td align="right" width="110px;">AMC Contact No</td> <td><input type="text" name="" id="" value="' + value.AMCContactNo + '" placeholder="" class="FilddHalf11" /></td></tr>';
                                    row = row + '<tr><td align="right" width="110px;">AMC Reminder</td> <td><input type="text" name="" id="" value="' + value.AMCReminder + '" placeholder="" class="FilddHalf11" /></td></tr>';
                                    row = row + '<tr><td align="right" width="110px;">AMC Valid Upto</td> <td><input type="text" name="" id="" value="' + value.AMCValidUpto + '" placeholder="" class="FilddHalf11" /></td></tr>';
                                    row = row + '<tr><td align="right"></td><td align="right"><a href="javascript:void(0);" data-id="' + value.Id + '" class="MainPopBtn" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage("Image37","","/images/fullscreen_hover.png",1)"><img src="/images/fullscreen_normal.png" name="Image37" width="26" height="26" border="0" id="Image37" /></a></td></tr></table></td>';
                                    $("#tblsearch").append(row);
                                    count = 1;
                                }
                                else {
                                    row = '<td align="center"><img src="/images/spacer.png" alt="" width="10" /></td>';
                                    row = row + '<td class="TableDiv">';
                                    row = row + '<table border="0" cellspacing="0" cellpadding="2">';
                                    row = row + '<tr><td align="right" width="110px;">AMC Type</td> <td><input type="text" name="" id="" value="' + value.UserType + '" placeholder="" class="FilddHalf11" /></td></tr>';
                                    row = row + '<tr><td align="right" width="110px;">AMC Contact No</td> <td><input type="text" name="" id="" value="' + value.MasterFileUpdation + '" placeholder="" class="FilddHalf11" /></td></tr>';
                                    row = row + '<tr><td align="right" width="110px;">AMC Reminder</td> <td><input type="text" name="" id="" value="' + value.MasterRecordDeletion + '" placeholder="" class="FilddHalf11" /></td></tr>';
                                    row = row + '<tr><td align="right" width="110px;">AMC Valid Upto</td> <td><input type="text" name="" id="" value="' + value.PendingRecordDeletion + '" placeholder="" class="FilddHalf11" /></td>';
                                    row = row + '<tr><td align="right"></td><td align="right"><a href="javascript:void(0);" data-id="' + value.UserType + '" class="MainPopBtn" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage("Image37","","/images/fullscreen_hover.png",1)"><img src="/images/fullscreen_normal.png" name="Image37" width="26" height="26" border="0" id="Image37" /></a></td></tr></table></td>';
                                    $("#tblsearch").append(row);
                                    count = 0;
                                    $("#tblsearch").append('</tr><tr><td colspan="3" style="height: 10px;"><img src="/images/spacer.png" alt="" /></td></tr>');
                                }
                            });

                        },
                        error: function (response) {
                            console.log(response);
                        }
                    });
                });
               
            });
        </script>
    </form>
</body>
</html>