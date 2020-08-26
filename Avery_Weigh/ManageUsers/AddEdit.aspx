<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEdit.aspx.cs" Inherits="Avery_Weigh.ManageUsers.AddEdit" %>

<%@ Register Src="~/View/Header.ascx" TagPrefix="uc1" TagName="Header" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Avery Weigh Tronix - Weigh</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/scripts/averyJs/jquery-3.4.1.min.js" integrity="sha256-CSXorXvZcTkaix6Yvo6HppcZGetbYMGWSFlBw8HfCJo=" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="/css/fonts.css" type="text/css" charset="utf-8" />
    <link rel="icon" href="/images/favicon.png" type="image/gif" sizes="16x16" />
    <script src="/js/jquery.min.js"></script>
    <link href="/Content/toastr.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery-3.3.1.min.js"></script>
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
        $(document).ready(function () {
            $(".MainPopBtn").click(function () {
                $(".Popup").show();
            });
            $(".Close").click(function () {
                $(".Popup").hide();
            });

        });
    </script>
    <style type="text/css">
        .toast {
            opacity: 1 !important;
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
    <style type="text/css">
        ​#chkWeighbridgeId tr {
            display: inline-block;
            margin-right: 20px;
        }

        #chkWeighbridgeId tr {
            float: left;
            margin-right: 20px;
        }

            #chkWeighbridgeId tr label {
                margin-left: 5px;
            }

            #chkWeighbridgeId tr input {
                width: auto !important;
            }

        ​
    </style>
</head>

<body onload="MM_preloadImages('/images/contact/contact_hover.png','/images/contact/info_hover.png','/images/contact/login_hover.png','/images/close_hover.png','/images/fullscreen_hover.png','/images/exit_fullscreen_hover.png','/images/type3/copy_hover.png','/images/type3/previous_page_hover.png','/images/type3/previous_record_hover.png','/images/type3/next_record_hover.png','/images/type3/next_page_hover.png')">
    <form id="form1" runat="server" defaultfocus="txtUserName">
        <div class="Wrapper WeighMain">
            <asp:HiddenField ID="imgLogo" runat="server" />
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
                            <img src="/images/type1/configure_select.png" name="Image5" width="166" height="40" border="0" id="Image5" /></a></li>
                        <li><a href="/ManageMasters" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image6','','/images/type1/masters_select.png',1)">
                            <img src="/images/type1/masters_normal.png" name="Image6" width="166" height="40" border="0" id="Image6" /></a></li>
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
                    <div class="RightInn11 textcon" style="overflow :hidden ">
                        <h4>CONFIGURATION</h4>
                        <h4 class="SupplierSearchBig">USER MASTER<a href="List.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image32','','/images/close_hover.png',1)" class="CloseButtonA">
                            <img src="/images/close_normal.png" class="Close" alt="Close" name="Image32" width="26" height="25" border="0" id="Image32" /></a></h4>
                    </div>
                    <div class="RightInn11 supplier">
                        <!-- <div class="RightInn22">3333</div> -->
                        <div class="RightInnMasters">
                            <div class="ConatctIcons2" id="divoptions" style="display: none;" runat="server">
                                <ul>
                                    <li><a href="List.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Copy','','/images/type3/copy_hover.png',1)">
                                        <img src="/images/type3/copy_normal.png" name="Copy" width="50" height="50" border="0" id="Copy" /></a></li>
                                    <li>
                                        <asp:LinkButton ID="First_Record" runat="server" ToolTip="First Record" OnClick="First_Record_Click" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('previous_page','','/images/type3/previous_page_hover.png',1)">
                                        <img src="/images/type3/previous_page_normal.png" name="previous_page" width="50" height="50" border="0" id="previous_page" /></asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="Previous_Record" runat="server" ToolTip="Previous Record" OnClick="Previous_Record_Click" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('previous_record','','/images/type3/previous_record_hover.png',1)">
                                        <img src="/images/type3/previous_record_normal.png" name="previous_record" width="50" height="50" border="0" id="previous_record" /></asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton runat="server" ID="Next_Record" ToolTip="Next Record" OnClick="Next_Record_Click" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('next_record','','/images/type3/next_record_hover.png',1)">
                                        <img src="/images/type3/next_record_normal.png" name="next_record" width="50" height="50" border="0" id="next_record" /></asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton runat="server" ID="Last_Record" ToolTip="Last Record" OnClick="Last_Record_Click" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('next_page','','/images/type3/next_page_hover.png',1)">
                                        <img src="/images/type3/next_page_normal.png" name="next_page" width="50" height="50" border="0" id="next_page" /></asp:LinkButton></li>
                                </ul>
                            </div>
                            <table border="0" cellspacing="0" cellpadding="2">
                                <tr>
                                    <td align="right" width="150px;">User Name</td>
                                    <td>
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="FilddHalf112" MaxLength="25"></asp:TextBox></td>
                                    <td align="right" width="150px;">Password</td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="FilddHalf112" MaxLength="15" TextMode="Password"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;">User Type</td>
                                    <td>
                                        <asp:DropDownList ID="ddlUserTypeList" Width="287px" CssClass="FilddHalf112" runat="server" DataTextField="Name" DataValueField="Id">
                                        </asp:DropDownList>

                                    </td>
                                    <td align="right" width="150px;">Plant Name</td>
                                    <td>
                                        <asp:DropDownList ID="ddlPlantList" Width="287px" CssClass="FilddHalf112" runat="server" DataTextField="PlantName" DataValueField="PlantCode"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;">WeighBridge Id</td>
                                    <td colspan="3">
                                        <asp:CheckBoxList ID="chkWeighbridgeId" runat="server">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

            </div>
            <!--MiddelSection Ends-->



            <!--Footer Ends-->
            <div class="footer">
                <ul class="selected">
                    <li>
                        <a href="Search.aspx" runat="server" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image11','','/images/type4/search_hover.png',1)">
                            <img src="/images/type4/search_normal.png" name="Image11" width="80" height="50" border="0" id="Image11" />
                        </a></li>
                    <li><a href="AddEdit.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image12','','/images/type4/edit_hover.png',1)">
                        <img src="/images/type4/edit_normal.png" name="Image12" width="80" height="50" border="0" id="Image12" /></a></li>

                    <li>
                        <asp:LinkButton runat="server" ID="btnsave" OnClientClick="return validateForm();" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image13','','/images/type4/save_hover.png',1)" OnClick="btnsave_Click">
                        <img src="/images/type4/save_normal.png" name="Image13" width="80" height="50" border="0" id="Image13" />
                        </asp:LinkButton></li>
                    <li><a href="Import.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Imag14','','/images/type4/export_hover.png',1)">
                        <img src="/images/type4/export_normal.png" alt="Export" name="Imag14" width="80" height="50" border="0" id="Imag14" /></a></li>
                    <li><a href="List.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image15','','/images/type4/import_hover.png',1)">
                        <img src="/images/type4/import_normal.png" name="Image15" width="80" height="50" border="0" id="Image15" /></a></li>
                    <li><a href="javascript:void(0)" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image16','','/images/type4/print_hover.png',1)">
                        <img src="/images/type4/print_normal.png" name="Image16" width="80" height="50" border="0" id="Image16" /></a></li>
                    <li><a href="javascript:void(0)" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image17','','/images/type4/delete_hover.png',1)">
                        <img src="/images/type4/delete_normal.png" name="Image17" width="80" height="50" border="0" id="Image17" /></a></li>
                </ul>
            </div>
 
            <script src="../Scripts/select2.js"></script>
            <link href="../Content/select2.css" rel="stylesheet" />
        </div>
        <script type="text/javascript">
            $('#ddlUserTypeList').select2();
            $('#ddlPlantList').select2();
            $(document).ready(function () {
          
                $('#txtUserName').change(function () {
                    var username = $('#txtUserName').val();
                    debugger;
                    $.ajax({
                        type: "POST",
                        url: "/AveryService/WebService1.asmx/Check_UserMaster",
                        data: "{'username':'" + username + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            debugger;
                            if (data.d != "") {
                                var obj = JSON.parse(data.d);
                                console.log(obj);
                                toastr.error('Same UserName already exist !');
                                $('#txtPlantCode').focus();
                            }
                        },
                        error: function (data) {
                            console.log(data);
                        }
                    });
                });

                document.addEventListener('keydown', function (event) {
                    if (event.keyCode === 13 && event.target.nodeName === 'INPUT') {
                        var form = event.target.form;
                        var index = Array.prototype.indexOf.call(form, event.target);
                        form.elements[index + 1].focus();
                        event.preventDefault();
                    }
                });

                 $('input[type=text]').focus(function () {
                    $(this).addClass("form-control").siblings().removeClass("form-control");
                });

            });
        </script>

    </form>
    <script type="text/javascript">
        function validateForm() {
            var result = true;
            var txtUserName = $('#txtUserName').val();
            var txtPassword = $('#txtPassword').val();
            var ddlUserTypeList = $('#ddlUserTypeList').select().val();

            if (txtUserName.trim() == '' || txtPassword.trim() == '' || ddlUserTypeList == '') {
                toastr.error('Please fill all mandatory fields.');
                result = false;
            }
            return result;
        }
        $(document).ready(function () {         
            $('#txtUserName').on("keypress", function (e) {
                if (e.keyCode == 13) {
                    $('#txtPassword').focus();
                    return false; // prevent the button click from happening
                }
            });
            $('#txtPassword').on("keypress", function (e) {
                if (e.keyCode == 13) {
                    $('#ddlUserTypeList').select2().focus();
                    return false; // prevent the button click from happening
                }
            });
        });
    </script>
</body>
</html>
