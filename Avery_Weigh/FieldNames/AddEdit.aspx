<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEdit.aspx.cs" Inherits="Avery_Weigh.FieldNames.AddEdit" %>


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
    <form id="form1" runat="server" defaultfocus="select2-ddlplantid-container">
        <div class="Wrapper WeighMain">
            <asp:HiddenField ID="imgLogo" runat="server" />
            <!--Header start here-->
            <uc1:Header runat="server" ID="Header" />
            <!--Header start here-->


            <!--MiddelSection Starts-->
            <div class="MiddleSection">
                <div class="LeftSec">
                    <uc1:UserMenu runat="server" ID="UserMenu" />

                </div>

                <div class="RightSec">
                    <div class="RightInn11 textcon" style="overflow:hidden">
                        <h4>MASTERS</h4>
                        <h4 class="SupplierSearchBig">FIELD NAMES <a href="List.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image32','','/images/close_hover.png',1)" class="CloseButtonA">
                            <img src="/images/close_normal.png" class="Close" alt="Close" name="Image32" width="26" height="25" border="0" id="Image32" /></a></h4>
                    </div>
                    <div class="RightInn11 supplier">
                        <!-- <div class="RightInn22">3333</div> -->
                        <div class="RightInnMasters">
                            <div class="ConatctIcons2" id="divoptions" style="display: none;" runat="server">
                               
                            </div>
                            <table border="0" cellspacing="0" cellpadding="2">
                                <tr>
                                    <td align="right" width="150px;">Plant Code </td>
                                    <td>
                                        <asp:DropDownList ID="ddlplantid" Width="287px" OnSelectedIndexChanged="ddlplantid_SelectedIndexChanged" AutoPostBack="true" DataTextField="Name" DataValueField="PlantCode" CssClass="FilddHalf112" runat="server"></asp:DropDownList>
                                    </td>
                                    <td align="right" width="150px;">Machin ID</td>
                                    <td>
                                        <asp:DropDownList ID="ddlmachinid" DataTextField="MachineId" DataValueField="MachineId" Width="287px" OnSelectedIndexChanged="ddlmachinid_SelectedIndexChanged" AutoPostBack="true" CssClass="FilddHalf112" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div id="dbMain" runat="server">
                               <asp:HiddenField ID="RecordId" runat="server" />
                            <table style="width: 100%;" border="1" bordercolor="#72aac0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" class="MainLTable">
                                <asp:Repeater ID="rptList" runat="server">
                                    <HeaderTemplate>
                                        <tr>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">Field Name</td>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">Field Value</td>
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">1st Weight Mandatory</td> 
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">2nd Weight Mandatory</td> 
                                            <td align="center" valign="middle" bgcolor="#d8e7ee">Weigh Form Fields(Not Required)</td> 
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="trclass" >
                                            <td align="center" valign="middle"><asp:Label ID="lblname" runat="server" Text='<%#Eval("FieldName") %>' ></asp:Label></td>
                                            <td align="center" valign="middle"><asp:TextBox ID="txtValue" style="width:80%;" runat="server" Text='<%#Eval("FieldValue") %>'
                                                ></asp:TextBox></td>
                                            <td align="center" valign="middle"> <asp:CheckBox ID="chk1" runat="server" Checked='<%#Eval("IsMandatory1").ToString() == "True" ? true : false %>' /></td>
                                            <td align="center" valign="middle"> <asp:CheckBox ID="chk2" runat="server" Checked='<%#Eval("IsMandatory2").ToString() == "True" ? true : false %>' /></td>
                                            <td align="center" valign="middle"> <asp:CheckBox ID="chk3" runat="server" Checked='<%#Eval("IsRequired").ToString() == "True" ? true : false %>' /></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                                </div>
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
                        <asp:LinkButton runat="server" ID="Btnsave" OnClick="Btnsave_Click" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image13','','/images/type4/save_hover.png',1)">
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
                $('#ddlplantid').select2();
                $('#ddlmachinid').select2();
                $('#ddlscheme').select2();
            </script>

        </div>
    </form>
    <script type="text/javascript">
        function validateForm() {
            var result = true;
            var plantid = $('#ddlplantid option:selected').val();
            var machinid = $('#ddlmachinid option:selected').val();
            if (plantid == '' || machinid == '') {
                toastr.error('Plant Code And Machine Id are mandatory fields');
                event.preventDefault();
                result = false;
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

            $('#ddlmachinid').change(function () {
                var plantcode = $('#ddlplantid option:selected').val();
                var machineid = $('#ddlmachinid option:selected').val();
                $.ajax({
                    url: '/AveryService/Webservice1.asmx/Check_BarrierMaster',
                    type: 'post',
                    contentType: 'application/json;charset=utf-8',
                    dataType: 'json',
                    data: "{plantcode:'" + plantcode + "',machineid:'" + machineid + "'}",
                    success: function (data) {
                        if (data.d != '') {
                            toastr.warning('Same barrier configuration already exist!');
                        }
                    },
                    error: function (data) {
                        console.log(data);
                    }
                })
            });
            $('#txtip').change(function () {
                var barrierip = $('#txtip').val();
                $.ajax({
                    url: '/AveryService/Webservice1.asmx/Check_BarrierMasterIP',
                    type: 'post',
                    contentType: 'application/json;charset=utf-8',
                    dataType: 'json',
                    data: "{barrierip:'" + barrierip + "'}",
                    success: function (data) {
                        if (data.d != '') {
                            toastr.warning('Same barrier IP already exist!');
                        }
                    },
                    error: function (data) {
                        console.log(data);
                    }
                });
            });
        });
    </script>
</body>
</html>

