<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEdit.aspx.cs" Inherits="Avery_Weigh.SensorMaster.AddEdit" %>

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
    <script src="../Scripts/select2.js"></script>
    <link href="../Content/select2.css" rel="stylesheet" />
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
</head>

<body onload="MM_preloadImages('/images/contact/contact_hover.png','/images/contact/info_hover.png','/images/contact/login_hover.png','/images/close_hover.png','/images/fullscreen_hover.png','/images/exit_fullscreen_hover.png','/images/type3/copy_hover.png','/images/type3/previous_page_hover.png','/images/type3/previous_record_hover.png','/images/type3/next_record_hover.png','/images/type3/next_page_hover.png')">
    <form id="form1" runat="server">
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
                    <div class="RightInn11" style="overflow:hidden">
                        <h4>CONFIGURATION</h4>
                        <h4 class="SupplierSearchBig">SENSOR MASTER <a href="List.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image32','','/images/close_hover.png',1)" class="CloseButtonA">
                            <img src="/images/close_normal.png" class="Close" alt="Close" name="Image32" width="26" height="25" border="0" id="Image32" /></a></h4>
                    </div>
                    <div class="RightInn11">
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
                                    <td align="right" width="150px;">Plant Code ID</td>
                                    <td>
                                        <asp:DropDownList ID="ddlplantcode" OnSelectedIndexChanged="ddlplantcode_SelectedIndexChanged" AutoPostBack="true" Width="287px" DataTextField="Name" DataValueField="PlantCode" CssClass="FilddHalf112" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;">Machine ID</td>
                                    <td>
                                        <asp:DropDownList ID="ddlmachineid" DataTextField="MachineId" DataValueField="MachineId" Width="287px" CssClass="FilddHalf112" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;">Sensor Identification</td>
                                    <td>
                                        <asp:TextBox ID="txtidentification" CssClass="FilddHalf112" runat="server" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;">Sensor IP</td>
                                    <td>
                                        <asp:TextBox ID="txtip" runat="server" CssClass="FilddHalf112" MaxLength="15"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;">Sensor PORT</td>
                                    <td>
                                        <asp:TextBox ID="txtport" runat="server" CssClass="FilddHalf112" MaxLength="5"></asp:TextBox>
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
                        <a href="Search.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image11','','/images/type4/search_hover.png',1)">
                         <img src="/images/type4/search_normal.png" name="Image11" width="80" height="50" border="0" id="Image11" />
                        </a></li>

                    <li><a href="AddEdit.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image12','','/images/type4/edit_hover.png',1)">
                        <img src="/images/type4/edit_normal.png" name="Image12" width="80" height="50" border="0" id="Image12" /></a></li>

                    <li>
                        <asp:LinkButton runat="server" ID="Btnsave" OnClick="Btnsave_Click" OnClientClick="return validateForm();" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image13','','/images/type4/save_hover.png',1)">
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
            <!--Footer Ends-->
            <script type="text/javascript">
                $('#ddlplantcode').select2();
                $('#ddlmachineid').select2();
            </script>

        </div>
    </form>
    <script type="text/javascript">
        function validateForm() {
            var result = true;
            var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            var plantcode = $('#ddlplantcode').val();
            var machineid = $('#ddlmachineid').val();
            var identification = $('#txtidentification').val();
            var IP = $('#txtip').val();
            var port = $('#txtport').val();
            if (plantcode == '' || machineid == '' || identification.trim() == '' || IP.trim() == '' || port.trim() == '') {
                toastr.error('All fields are mandatory');
                result = false;
            }
            if (IP != '') {
                if (!IP.match(ipformat)) {
                    toastr.error('Invalid IP Address');
                    $('#txtip').css('border-color', 'red');
                    result = false;
                }
            }
            return result;
        }
       
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            document.addEventListener('keydown', function (event) {
                if (event.keyCode === 13 && event.target.nodeName === 'INPUT') {
                    var form = event.target.form;
                    var index = Array.prototype.indexOf.call(form, event.target);
                    form.elements[index + 1].focus();
                    event.preventDefault();
                }
            });

            $('#ddlmachineid').change(function () {
                var plantcode = $('#ddlplantcode option:selected').val();
                var machineid = $('#ddlmachineid option:selected').val();
                $.ajax({
                    url: "/AveryService/WebService1.asmx/Check_Sensor",
                    type: "post",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: '{plantcode:"' + plantcode + '",machineid:"' + machineid + '"}',
                    async: false,
                    success: function (data) {                        
                        if (data.d != "") {
                            toastr.warning('Same Machine Configuration Exist!Please Try Again');
                        }
                    },
                    error: function (data) {
                        console.log(data);
                    }
                });
            });

            $('#txtip').change(function () {
                debugger;
                var plantcode = $('#ddlplantcode option:selected').val();
                var ip = $('#txtip').val();
                 $.ajax({
                    url: "/AveryService/WebService1.asmx/Check_SensorIP",
                    type: "post",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: '{plantcode:"' + plantcode + '",ip:"' + ip + '"}',
                    async: false,
                    success: function (data) {                        
                        if (data.d != "") {
                            toastr.warning('Same Sensor IP Exist!');
                        }
                    },
                    error: function (data) {
                        console.log(data);
                    }
                });
            });

             $('input[type=text]').focus(function () {
                    $(this).addClass("form-control").siblings().removeClass("form-control");
                });

        });
    </script>
</body>
</html>

