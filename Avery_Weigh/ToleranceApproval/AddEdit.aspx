<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEdit.aspx.cs" Inherits="Avery_Weigh.ToleranceApproval.AddEdit" %>

<%@ Register Src="~/View/Header.ascx" TagPrefix="uc1" TagName="Header" %>
<%@ Register Src="~/View/UserMenu.ascx" TagPrefix="uc1" TagName="UserMenu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Avery Weigh Tronix - Weigh</title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/css/fonts.css" type="text/css" charset="utf-8" />
    <link rel="icon" href="/images/favicon.png" type="image/gif" sizes="16x16" />
    <script src="/js/jquery.min.js"></script>
    <script src="/Scripts/select2.js"></script>
    <link href="/Content/select2.css" rel="stylesheet" />
    <link href="/Content/toastr.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
     <script src="/scripts/averyJs/jquery-ui.js"></script>
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
        #chkIsMultiProduct{
            max-width:0px !important;
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
    <script src="/Scripts/date.format.js"></script>
</head>

<body onload="MM_preloadImages('/images/contact/contact_hover.png','/images/contact/info_hover.png','/images/contact/login_hover.png','/images/close_hover.png','/images/fullscreen_hover.png','/images/exit_fullscreen_hover.png','/images/type3/copy_hover.png','/images/type3/previous_page_hover.png','/images/type3/previous_record_hover.png','/images/type3/next_record_hover.png','/images/type3/next_page_hover.png')">
    <form id="form1" runat="server" defaultfocus="txtTripID">
        <div class="Wrapper WeighMain">
            <asp:HiddenField ID="imgLogo" runat="server" />
            <!--Header start here-->
            <uc1:Header runat="server" ID="Header" />
            <!--Header start here-->


            <!--MiddelSection Starts-->
            <div class="MiddleSection">
                <div class="LeftSec">
                    <%--<uc1:UserMenu runat="server" ID="UserMenu" />--%>
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
                        <img src="images/intellegence-left-logo.png" alt="" /></span>
                </div>

                <div class="RightSec">
                    <div class="RightInn11 textcon" style="overflow:hidden">
                        <h4>MASTERS</h4>
                        <h4 class="SupplierSearchBig">TARE TOLERANCE RECORDS <a href="List.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image32','','/images/close_hover.png',1)" class="CloseButtonA">
                            <img src="/images/close_normal.png" class="Close" alt="Close" name="Image32" width="26" height="25" border="0" id="Image32" /></a></h4>
                    </div>
                    <div class="RightInn11 supplier">
                        <!-- <div class="RightInn22">3333</div> -->
                        <div class="RightInnMasters">
                            <div class="ConatctIcons2">
                                <ul style="display: none;">
                                    <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Copy','','/images/type3/copy_hover.png',1)">
                                        <img src="/images/type3/copy_normal.png" name="Copy" width="50" height="50" border="0" id="Copy" /></a></li>
                                    <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('previous_page','','/images/type3/previous_page_hover.png',1)">
                                        <img src="/images/type3/previous_page_normal.png" name="previous_page" width="50" height="50" border="0" id="previous_page" /></a></li>
                                    <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('previous_record','','/images/type3/previous_record_hover.png',1)">
                                        <img src="/images/type3/previous_record_normal.png" name="previous_record" width="50" height="50" border="0" id="previous_record" /></a></li>
                                    <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('next_record','','/images/type3/next_record_hover.png',1)">
                                        <img src="/images/type3/next_record_normal.png" name="next_record" width="50" height="50" border="0" id="next_record" /></a></li>
                                    <li><a href="login.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('next_page','','/images/type3/next_page_hover.png',1)">
                                        <img src="/images/type3/next_page_normal.png" name="next_page" width="50" height="50" border="0" id="next_page" /></a></li>
                                </ul>
                            </div>
                            <table border="0" cellspacing="0" cellpadding="2">
                                <tr>
                                    <td align="right" width="150px;">Trip ID</td>
                                    <td>
                                        <asp:TextBox ID="txtTripID" CssClass="FilddHalf112" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td align="right" width="150px;">Trip Type</td>
                                    <td>
                                        <asp:DropDownList ID="ddlTripType" Width="287px" CssClass="FilddHalf112" runat="server">
                                            <asp:ListItem Selected="True" Value="0">NA</asp:ListItem>
                                            <asp:ListItem Value="1">Inbound</asp:ListItem>
                                            <asp:ListItem Value="2">Outbound</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td align="right" width="150px;">Is Multi Product </td>
                                    <td style="text-align:left;">
                                        <asp:CheckBox ID="chkIsMultiProduct" runat="server" Enabled="false" style="max-width:0px !important;" />
                                    </td>
                                    <td align="right" width="150px;">Gate Entry No</td>
                                    <td>
                                        <asp:TextBox ID="txtGateEntryNo" CssClass="FilddHalf112" runat="server"></asp:TextBox>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td align="right" width="150px;">Truck No</td>
                                    <td>
                                        <asp:TextBox ID="txtTruckNo" CssClass="FilddHalf112" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td align="right" width="150px;">Supplier</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSupplier" Width="287px" CssClass="FilddHalf112" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;">Transport</td>
                                    <td>
                                        <asp:DropDownList ID="ddlTransport" Width="287px" CssClass="FilddHalf112" runat="server"></asp:DropDownList>
                                    </td>
                                    <td align="right" width="150px;">Packing</td>
                                    <td>
                                        <asp:DropDownList ID="ddlPacking" Width="287px" CssClass="FilddHalf112" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;">Challan No</td>
                                    <td>
                                        <asp:TextBox ID="txtChallanNo" CssClass="FilddHalf112" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="right" width="150px;">Challan Date</td>
                                    <td>
                                        <asp:TextBox ID="txtChallanDate" CssClass="FilddHalf112" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;">Challan Weight</td>
                                    <td>
                                        <asp:TextBox ID="txtChallanWeight" CssClass="FilddHalf112" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="right" width="150px;">Remarks</td>
                                    <td>
                                        <asp:TextBox ID="txtRemarks" CssClass="FilddHalf112" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;">Record Create Date</td>
                                    <td>
                                        <asp:TextBox ID="txtCreateDate" CssClass="FilddHalf112" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="right" width="150px;">Created By</td>
                                    <td>
                                        <asp:TextBox ID="txtCreatedBy" CssClass="FilddHalf112" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="150px;" style="display:none;">Shift</td>
                                    <td style="display:none;">
                                        <asp:TextBox ID="txtShift" CssClass="FilddHalf112" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="right" width="150px;">WeighBridge Id</td>
                                    <td>
                                        <asp:TextBox ID="txtWeighBridgeID" CssClass="FilddHalf112" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    
                                    <td>
                                        <asp:Button ID="txtApproved" runat="server" Text="Approved"  class="my_btn" OnClick="txtApproved_Click" />
                                    </td>
                                    <td></td>
                                     <td>
                                        <asp:Button ID="txtReject" runat="server" Text="Rejected"  class="my_btn" OnClick="txtReject_Click" />
                                    </td>
                                </tr>
                            </table>
                            
                            <h3>Weighing Details</h3>
                            <table style="width: 98%;" border="1" bordercolor="#72aac0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" class="MainLTable">
                                        <tr>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">First Weight</td>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">First Weight Date/Time</td>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">Second Weight</td>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">Second Weight Date/Time</td>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">Net Weight</td> 
                                        </tr>
                                        <tr class="trclass">
                                            <td align="center" valign="middle" ><asp:Label ID="lblFirstWt" runat="server"></asp:Label></td>
                                            <td align="center" valign="middle" ><asp:Label ID="lblFirstWtDateTime" runat="server"></asp:Label></td>
                                            <td align="center" valign="middle"><asp:Label ID="lblSecondWeight" runat="server" Text="---"></asp:Label></td>
                                            <td align="center" valign="middle" ><asp:Label ID="lblSecondWeightDateTime" runat="server" Text="---"></asp:Label></td>
                                            <td align="center" valign="middle" ><asp:Label ID="lblNetWeight" runat="server" Text="---"></asp:Label></td> 
                                        </tr>
                            </table>
                            <h3>Materials Details</h3>
                            <table style="width: 98%;" border="1" bordercolor="#72aac0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" class="MainLTable">
                                <asp:Repeater ID="rptMaterials" runat="server" OnItemCreated="rptMaterials_ItemCreated">
                                    <HeaderTemplate>
                                        <tr>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">No.</td>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">Material Name</td>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">Material Classification</td>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">Weight</td> 
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="trclass" data-RowId="<%#Eval("Id")%>"">
                                            <td align="center" valign="middle"><asp:Label ID="lblindex" runat="server"></asp:Label></td>
                                            <td align="center" valign="middle"><%#Eval("MaterialName") %></td>
                                            <td align="center" valign="middle"><%#Eval("MaterialClassificationName") %></td>
                                            <td align="center" valign="middle"><%#Eval("Weight") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <!--MiddelSection Ends-->

            <!--Footer Ends-->
            <div class="footer">
               <%-- <ul class="selected">
                    <li>
                        <asp:LinkButton ID="btnsearch" PostBackUrl="javascript:void(0)" runat="server" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image11','','/images/type4/search_hover.png',1)">
                         <img src="/images/type4/search_normal.png" name="Image11" width="80" height="50" border="0" id="Image11" />
                        </asp:LinkButton></li>
                    <li><a href="AddEdit.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image12','','/images/type4/edit_hover.png',1)">
                        <img src="/images/type4/edit_normal.png" name="Image12" width="80" height="50" border="0" id="Image12" /></a></li>
                    <li>
                        <asp:LinkButton runat="server" ID="Btnsave" OnClick="Btnsave_Click" OnClientClick="return validateForm();" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image13','','/images/type4/save_hover.png',1)">
                        <img src="/images/type4/save_normal.png" name="Image13" width="80" height="50" border="0" id="Image13" />
                        </asp:LinkButton></li>
                    <li><a href="javascript:void(0)" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Imag14','','/images/type4/export_hover.png',1)">
                        <img src="/images/type4/export_normal.png" alt="Export" name="Imag14" width="80" height="50" border="0" id="Imag14" /></a></li>
                    <li><a href="javascript:void(0)" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image15','','/images/type4/import_hover.png',1)">
                        <img src="/images/type4/import_normal.png" name="Image15" width="80" height="50" border="0" id="Image15" /></a></li>
                    <li><a href="javascript:void(0)" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image16','','/images/type4/print_hover.png',1)">
                        <img src="/images/type4/print_normal.png" name="Image16" width="80" height="50" border="0" id="Image16" /></a></li>
                    <li><a href="javascript:void(0)" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image17','','/images/type4/delete_hover.png',1)">
                        <img src="/images/type4/delete_normal.png" name="Image17" width="80" height="50" border="0" id="Image17" /></a></li>
                </ul>--%>
            </div>
            <!--Footer Ends-->
            <script type="text/javascript">
                $('#ddlTripType').select2();
                $('#ddlSupplier').select2();
                $('#ddlTransport').select2();
                $('#ddlPacking').select2();
            </script>

        </div>
    </form>
    <script type="text/javascript">
        function validateForm() {
            var result = true;
            var plantcode = $('#ddlplantcode').val();
            var machineid = $('#ddlmachineid').val();
            var cidentification = $('#txtidentification').val();
            var cameraip = $('#txtip').val();
            var cameraport = $('#txtport').val();
            if (plantcode == '' || machineid == '' || cidentification.trim() == '' || cameraip.trim() == '' || cameraport.trim() == '') {
                toastr.error('All fields are mandatory');
                result = false;
            }
            return result;
        }
        $(document).ready(function () {
            $('#txtidentification').keypress(function () {
                var maxLength = $(this).val().length;
                if (maxLength >= 10) {
                    return false;
                }
            });
            $('#txtip').keypress(function () {
                var maxLength = $(this).val().length;
                if (maxLength >= 15) {
                    return false;
                }
            });
            $('#txtport').keypress(function () {
                var maxLength = $(this).val().length;
                if (maxLength >= 5) {
                    return false;
                }
            });

        });

    </script>
    <script>
        $(function () {
            $("#txtCreateDate").datepicker();
            $("#txtChallanDate").datepicker();
        });
    </script>
</body>
</html>

