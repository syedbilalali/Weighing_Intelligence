<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manual_Weighment.aspx.cs" Inherits="Avery_Weigh.Manual_Weighment" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/View/Header.ascx" TagPrefix="uc1" TagName="Header" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Avery Weigh Tronix - Weigh</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/fonts.css" type="text/css" charset="utf-8" />
    <link rel="icon" href="images/favicon.png" type="image/gif" sizes="16x16" />
  <script src="/scripts/averyJs/jquery-3.4.1.min.js" integrity="sha256-CSXorXvZcTkaix6Yvo6HppcZGetbYMGWSFlBw8HfCJo=" crossorigin="anonymous"></script>
    <script src="/scripts/averyJs/chosen.jquery.min.js"></script>
    <script src="/scripts/averyJs/chosen1.jquery.min.js" type="text/javascript"></script>
    <link href="/scripts/averyJs/chosen.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/scripts/averyJs/jquery-ui.css" />

    <link href="/Content/toastr.min.css" rel="stylesheet" />
    <script src="/scripts/averyJs/jquery-ui.js"></script>
    <style type="text/css">
        .toast {
            opacity: 1 !important;
        }

        #FirstWeight {
            display: inline-block;
            background: #fff;
        }

        .chosen-container {
            /*margin: 4px 0px 4px 15px !important;*/
            width: 290px !important;
        }
    </style>
    <script src="/scripts/averyJs/toastr.min.js">
    </script>
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
        function COnfirmClick() {
            $('#btnConfirm').click();
        }
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to Print Ticket?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
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
        $('input').keypress(function (e)
        {
         return false
        });
        </script>

    <script type = "text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT",);
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            
            if (confirm("Do you want to save data?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function AlertMsg() {
            $("#dialog-Alert").dialog({
                hide: 'explode',
                modal: 'true',
                title: 'Title goes here',
                draggable: false,
                resizable: false,
                height: 150,
                width: 500,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        $(document).ready(function () {
            debugger;
            $(window).bind('beforeunload', function () {
                $ajax({
                    url: "Logout.aspx",
                });
            });
        });
    </script>

    <%--<script type="text/javascript">
        $(function () {
            $("[id*=save]").removeAttr("onclick");
            debugger;
            $("#dialog").dialog({
                modal: true,
                autoOpen: false,
                title: "Confirmation",
                width: 350,
                height: 160,
                buttons: [
                    {
                        id: "Yes",
                        text: "Yes",
                        click: function () {
                            $("[id*=save]").attr("rel", "Save Records");
                            $("[id*=save]").click();
                        }
                    },
                    {
                        id: "No",
                        text: "No",
                        click: function () {
                            $(this).dialog('close');
                        }
                    }
                ]
            });
            debugger;
            $("[id*=save]").click(function () {
                debugger;
                if ($(this).attr("rel") != "Save Records") {
                    $('#dialog').dialog('open');
                    return false;
                } else {
                    __doPostBack(this.name, 'TEST');
                }
            });
        });
</script>--%>

    <%--<script src="/Scripts/date.format.js"></script>

     <script src="DDL/Scripts/jquery.min.js" type="text/javascript"></script>
		<script src="DDL/Scripts/chosen.jquery.js" type="text/javascript"></script>
		<script type="text/javascript"> $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
    <style>
		a img{border: none;}
		ol li{list-style: decimal outside;}
		div#container{width: 780px;margin: 0 auto;padding: 1em 0;}
		div.side-by-side{width: 100%;margin-bottom: 1em;}
		div.side-by-side > div{float: left;width: 50%;}
		div.side-by-side > div > em{margin-bottom: 10px;display: block;}
		.clearfix:after{content: "\0020";display: block;height: 0;clear: both;overflow: hidden;visibility: hidden;}
		
	</style>
	<link rel="stylesheet" href="DDL/Style/chosen.css" />--%>

</head>
<body   onload="MM_preloadImages('/images/contact/contact_hover.png','images/contact/info_hover.png','images/contact/login_hover.png','images/type1/weigh_hover.png','images/type1/configure_hover.png','images/type1/dashboard_hover.png','images/type1/service_hover.png','images/type1/diagnostics_hover.png','images/next_truck_hover.png','images/type4/search_hover.png','images/type4/edit_hover.png','images/type4/save_hover.png','images/type4/export_hover.png','images/type4/import_hover.png','images/type4/print_hover.png')">
    <form id="form1" runat="server" defaultfocus="txtTruckNo">

        <%--  <asp:ScriptManager ID="sc1" runat="server" EnablePageMethods="true"></asp:ScriptManager>--%>

        <asp:HiddenField ID="hdmachineid" runat="server" />
        <asp:HiddenField ID="hdPlantId" runat="server" />
        <asp:HiddenField ID="IsTareWeightEnabled" runat="server" ClientIDMode="Static" Value="0"  />
        <div class="Wrapper WeighMain weigh2">
            <div style="width: 0; height: 0;">
                <asp:Button ID="btnConfirm" runat="server" OnClick="OnConfirm" Text="Raise Confirm" OnClientClick="Confirm()" />
            </div>
            <!--Header start here-->
            <uc1:Header runat="server" ID="Header" />
            <!--Header start here-->
            <asp:UpdatePanel ID="upTimer" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Timer ID="timer1" runat="server" OnTick="timer1_Tick" Interval="100" Enabled="false"></asp:Timer>
                </ContentTemplate>

            </asp:UpdatePanel>
           <%-- <asp:UpdatePanel ID="upTimer1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Timer ID="timer2" runat="server" OnTick="timer2_Tick" Interval="150" Enabled="false"></asp:Timer>
                </ContentTemplate>

            </asp:UpdatePanel>

             <asp:UpdatePanel ID="upTimer22" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Timer ID="timer3" runat="server" OnTick="timer3_Tick" Interval="400" Enabled="false"></asp:Timer>
                </ContentTemplate>

            </asp:UpdatePanel>--%>

            <!--MiddelSection Starts-->
            <div class="MiddleSection midsec2">
                <div class="LeftSec">
                    <ul>
                        <li id="WeighMenu" runat="server"><a href="Manual_Weighment" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image4','','/images/type1/weigh_hover.png',1)">
                            <img src="/images/type1/weigh_select.png" alt="Weigh" name="Image4" width="166" height="40" border="0" id="Image4" /></a></li>
                        <li id="configurationMenu" runat="server"><a href="/ConfigurationMaster" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image5','','/images/type1/configure_hover.png',1)">
                            <img src="/images/type1/configure_normal.png" name="Image5" width="166" height="40" border="0" id="Image5" /></a></li>
                        <li id="ManageMasters" runat="server"><a href="/ManageMasters" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image6','','/images/type1/masters_hover.png',1)">
                            <img src="/images/type1/masters_normal.png" name="Image6" width="166" height="40" border="0" id="Image6" /></a></li>
                        <li><a href="/dashboardmaster" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image7','','/images/type1/dashboard_hover.png',1)">
                            <img src="/images/type1/dashboard_normal.png" name="Image7" width="166" height="40" border="0" id="Image7" /></a></li>
                        <li><a href="/ManageServices" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image8','','/images/type1/service_hover.png',1)">
                            <img src="/images/type1/service_normal.png" name="Image8" width="166" height="40" border="0" id="Image8" /></a></li>
                        <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image9','','/images/type1/diagnostics_hover.png',1)">
                            <img src="/images/type1/diagnostics_normal.png" name="Image9" width="166" height="40" border="0" id="Image9" /></a></li>
                    </ul>
                    <span>
                        <img src="/images/intellegence-left-logo.png" alt="" /></span>
                </div>

                <div class="RightSec">
                    <div class="RightInn1" style="overflow-y: scroll; height: 570px;">
                        <h4>MANUAL WEIGHMENT</h4>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="min-height: 500px;">
                            <tr>
                                <td align="right" id="lblTripId" runat="server">Trip ID</td>
                                <td>
                                    <%--<input type="text" name="" id="txttripid" value="" placeholder="Trip Id" class="FilddHalf" onfocus="if(this.value == '123456'){this.value = '';}" />--%>
                                    <asp:TextBox ID="txtTripId" runat="server" TabIndex="1" CssClass="FilddHalf" onkeypress="return false;" ></asp:TextBox>
                                   <%-- <asp:Label ID="txtTripId" runat="server" TabIndex="1" CssClass="FilddHalf" ></asp:Label>--%>
                                    <asp:TextBox ID="txtgateentryno" TabIndex="2" runat="server" placeholder="Gate Entry No" CssClass="FilddHalf1"></asp:TextBox>
                                    <asp:TextBox ID="txttarewtenabled" TabIndex="3" runat="server" Visible="false" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lblWeighingType" runat="server">Weighing Type</td>
                                <td style="padding-left: 15px; padding-bottom: 5px;">
                                    <asp:DropDownList ID="ddlinoutna" runat="server" TabIndex="3" CssClass="FilddHalf112">
                                        <asp:ListItem Selected="True" Value="0">NA</asp:ListItem>
                                        <asp:ListItem  Value="1">Inbound</asp:ListItem>
                                        <asp:ListItem Value="2">Outbound</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lblMultiProduct" runat="server">Multi Product</td>
                                <td align="left" style="float: left; margin-left: 9px;">
                                    <asp:CheckBox ID="checkmultiproduct" TabIndex="4" runat="server" Height="19px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lbltruckno" runat="server">Truck No.</td>
                                <td>
                                    <asp:TextBox ID="txtTruckNo" runat="server" CssClass="field" Style="text-transform: uppercase;" Width="265px" MaxLength="15" OnTextChanged="txtTruckNo_TextChanged"></asp:TextBox>
                                   <%-- <asp:DropDownList ID="txtTruckNo" Width="265px" runat="server" AppendDataBoundItems="True"></asp:DropDownList>--%>
                                    <%--<asp:DropDownList ID="txtTruckNo" Height="35px" Width="287px" CssClass="FilddHalf112" runat="server" DataTextField="TruckNo" DataValueField="TruckNo">
                                        </asp:DropDownList>    --%>
                                    <asp:HiddenField ID="hfCustomerId" runat="server" />
    
                                </td>
                            </tr>
                            <tr id="cc1" style="display: none;">
                                <td align="right" id="lblClssificationCOde" runat="server">Classification Code</td>
                                <td><span id="VCC" style="margin-left: 15px;"></span></td>
                            </tr>
                            <tr id="cc2" style="display: none;">
                                <td align="right" id="lblKerbWt" runat="server">Kerb Weight</td>
                                <td><span id="KW" style="margin-left: 15px;"></span></td>
                            </tr>
                            <tr id="cc3" style="display: none;">
                                <td align="right" id="lblNoofaxels" runat="server">No of Axles</td>
                                <td><span id="NoA" style="margin-left: 15px;"></span></td>
                            </tr>
                            <tr id="divMaterial">
                                <td align="right" id="lblMaterial" runat="server">Material</td>
                                <td style="padding-left: 15px; padding-bottom: 5px;">
                                    <asp:DropDownList ID="ddlmaterial" runat="server" DataTextField="Name" DataValueField="Id" Width="287px" CssClass="FilddHalf112" OnSelectedIndexChanged="ddlmaterial_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lblMC" runat="server">Material Classification</td>
                                <td style="padding-left: 15px; padding-bottom: 5px;">
                                   <%-- <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>--%>
                                            <asp:DropDownList runat="server" ID="ddlmc"  CssClass="FilddHalf112"  Width="287px"  ></asp:DropDownList>
                                         <%--</ContentTemplate>
                                     </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lblsupplier" runat="server">Supplier/Customer</td>
                                <td style="padding-left: 15px; padding-bottom: 5px;">
                                    <asp:DropDownList runat="server" DataTextField="Name" DataValueField="Id" ID="ddlsupplier" CssClass="FilddHalf112 chzn-select"  Width="287px"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lblTransporter" runat="server">Transporter</td>
                                <td style="padding-left: 15px; padding-bottom: 5px;">
                                    <asp:DropDownList ID="ddltransporter" DataTextField="Name" DataValueField="Id" runat="server" CssClass="FilddHalf112 chzn-select"  Width="287px"></asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lblPacking" runat="server">Packing</td>
                                <td style="padding-left: 15px; padding-bottom: 5px;">
                                     <%--<asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>--%>
                                    <asp:DropDownList ID="ddlpacking" DataTextField="Name" DataValueField="Id" runat="server" CssClass="FilddHalf112" Width="287px" OnSelectedIndexChanged="ddlpacking_SelectedIndexChanged" OnTextChanged="ddlpacking_TextChanged"></asp:DropDownList>
                                   <%--  </ContentTemplate>
                                     </asp:UpdatePanel>--%>

                                </td>

                            </tr>
                            <tr>
                                <td align="right" id="lblPackingQty" runat="server">Packing QTY</td>
                                <td>
                                   <%--  <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>--%>
                                            <asp:TextBox runat="server" ID="txtpackingqty"  Width="200" CssClass="field" onkeypress="return false;"></asp:TextBox>
                                            <asp:Label ID="lblPackingUnit" runat="server"  Width="50px"  ></asp:Label>
                                        <%-- </ContentTemplate>
                                     </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lblChallanNo" runat="server">Challan / Invoice No</td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="field FilddHalf"></asp:TextBox>
                                    <asp:Label ID="lblInvoiceDate" runat="server"  Width="30px" Text="Date"  ></asp:Label>
                                    <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="field FilddHalf1"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" id="lblChallanwt" runat="server">Challan Weight</td>
                                <td>
                                    <asp:TextBox ID="txtChallanWeight" runat="server" CssClass="field FilddHalf"></asp:TextBox>
                                    kg</td>
                            </tr>
                            <tr id="PODiv1">
                                <td align="right" id="lblPOSODONo" runat="server">PO/SO/DO No</td>
                                <td>
                                    <asp:TextBox ID="txtPONo" runat="server" CssClass="field FilddHalf"></asp:TextBox>
                                    <asp:Label ID="lblPODate" runat="server"  Width="30px" Text="Date"  ></asp:Label>
                                    <asp:TextBox ID="txtPODate" runat="server" CssClass="field FilddHalf1"></asp:TextBox></td>
                            </tr>
                            <tr id="PODiv2" style="display: none;">
                                <td align="right">PO/SO/DO Materials</td>
                                <td>
                                    <asp:DropDownList ID="ddlPOMaterials" runat="server" DataTextField="Name" DataValueField="Id" Width="287px" CssClass="FilddHalf112">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="PODiv3" style="display: none;">
                                <td colspan="3" id="multiMaterials">
                                    <table style="width: 100%; height: 0px;">
                                        <tr>
                                            <td style="text-align: center;">Material</td>
                                            <td style="text-align: center;">Date</td>
                                            <td style="text-align: center;">Weight</td>
                                        </tr>
                                        
                                    </table>

                                </td>
                               
                            </tr>

                           <tr id="PODiv4" style="display: none;">
                               <td align="right" id="Td1" runat="server"></td>
                                <td >
                                    <asp:Button  runat="server" ID="btnLoadUnload" Text="Loading/Unloading Complete" CssClass="field" Width="320px" OnClick="btnLoadUnload_Click"  ></asp:Button>
                                </td>
                            </tr>
                           
                            <tr>
                                <td align="right" id="lblRemrks" runat="server">Remarks</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtremarks" CssClass="field"></asp:TextBox>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td align="right" id="lblMsg" runat="server">Message Info</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="txtMsgInfo" CssClass="field" ReadOnly="true"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>--%>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server" >
                                        <ContentTemplate>
                                            <asp:TextBox  runat="server" ID="lblRangeLockWB1" CssClass="field" ReadOnly="true"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>


                    <div class="RightInn2">
                        <table width="280" border="0" cellspacing="0" cellpadding="0">
                            <tr style="margin-bottom: 20px;">
                                <td colspan="2" align="center" style="padding-top:2px;" >
                                    <asp:UpdatePanel ID="up1" runat="server" style="float: right;">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="RuntimeWeight" runat="server" value="0"   placeholder="" Text="No weight" Style="width: 400px; float: left; " CssClass="field2" onfocus="if(this.value == '00000000'){this.value = '';}"></asp:LinkButton>
                                            <span style="margin: 15px 0 0 4px; display: inline-block;"> <asp:Label ID="lblUnit1" runat="server" ReadOnly="true" ></asp:Label></span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lblFirstWt" runat="server">1st Weight</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="FirstWeight" runat="server" CssClass="field1" ReadOnly="true" height="20px" onfocus="if(this.value == '999999'){this.value = '';}"></asp:Label>
                                            <%--<asp:Label ID="Label1" runat="server" CssClass="field1" ReadOnly="true" Text="No weight" onfocus="if(this.value == '999999'){this.value = '';}"></asp:Label>--%>
                                            <%--<input type="text" name="" id="FirstWeight" readonly="true" runat="server" value="" placeholder="" class="field1" onfocus="if(this.value == '999999'){this.value = '';}" />--%>
                                            <asp:Label ID="lblUnit" runat="server" ReadOnly="true" ></asp:Label>
                                           
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Date</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <input type="text" name="" id="FirstDate" runat="server" readonly="true" value="" placeholder="" class="field1" onfocus="if(this.value == '99/99/9999'){this.value = '';}" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Time</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <input type="text" name="" id="FirstTime" runat="server" readonly="true" value="" placeholder="" class="field1" onfocus="if(this.value == '99.99.99'){this.value = '';}" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lbl2ndWt" runat="server">2nd Weight</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <input type="text" name="" id="SecondWeight" readonly="true" runat="server" value="" placeholder="" class="field1" onfocus="if(this.value == '999999'){this.value = '';}" />
                                             <asp:Label ID="lblUnit2" runat="server" ReadOnly="true" ></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Date</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                        <ContentTemplate>
                                            <input type="text" name="" id="SecondDate" runat="server" readonly="true" value="" placeholder="" class="field1" onfocus="if(this.value == '99/99/9999'){this.value = '';}" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Time</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <ContentTemplate>
                                            <input type="text" name="" id="SecondTime" runat="server" readonly="true" value="" placeholder="" class="field1" onfocus="if(this.value == '99.99.99'){this.value = '';}" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" id="lblNetWt" runat="server">Net Weight</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <input type="text" name="" id="txtFinalWeight" runat="server" value="" placeholder="" class="field1 blck" onfocus="if(this.value == '999999'){this.value = '';}" />
                                             <asp:Label ID="lblUnit3" runat="server" ReadOnly="true" ></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Date</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <input type="text" name="" id="txtFinalDate" runat="server" value="" placeholder="" class="field1 blck" onfocus="if(this.value == '99/99/9999'){this.value = '';}" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Time</td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <input type="text" name="" id="txtFinalTime" runat="server" value="" placeholder="" class="field1 blck" onfocus="if(this.value == '99.99.99'){this.value = '';}" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td align="right">&nbsp;</td>
                                <td><a href="javascript:void(0);" onclick="window.location.href='Manual_Weighment';" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image10','','images/next_truck_hover.png',1)">
                                    <img src="images/next_truck_normal.png" alt="Next" name="Image10" width="71" height="60" border="0" id="Image10" style="margin: 10px 0 0 15px;" /></a></td>
                            </tr>


                        </table>

                    </div>



                    <div class="RightInn3">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" >
                            <%--<tr>
                                <td   width="30" align="right" valign="bottom"><span>1</span> </td>
                                <td>CAMERA<br />
                                                                      
                                    <img  alt="Camera not connected" style="margin-top: 0px;" src="<%=mySrc1 %>" /></td>
                            </tr>
                            <tr>
                                <td align="right" valign="bottom"><span>2</span> </td>
                                <td>
                                    
                                    <img alt="Camera not connected" src="<%=mySrc2 %>" /></td>
                            </tr>
                            <tr>
                                <td align="right" valign="bottom"><span>3</span> </td>
                                <td>
                                    
                                    <img alt="Camera not connected" src="<%=mySrc3 %>"   /></td>
                            </tr>--%>
                          
                                <asp:GridView ID="GridView1" runat="server" Height="250" >
                                      <%--  <rowstyle Height="20px" />
                                            <alternatingrowstyle  Height="20px"/>--%>
                              
                                 </asp:GridView>
                            
                            
                        </table>

                    </div>


                </div>

            </div>
            <!--MiddelSection Ends-->



            <!--Footer Ends-->
            <div class="footer">
                <ul>
                    <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image11','','images/type4/search_hover.png',1)">
                        <img src="images/type4/search_normal.png" name="Image11" width="80" height="50" border="0" id="Image11"  /></a></li>
                    <li id="AddBtn" runat="server"><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image12','','images/type4/edit_hover.png',1)">
                        <img src="images/type4/edit_normal.png" name="Image12" width="80" height="50" border="0" id="Image12" /></a></li>
                    <li id="saveBtn" runat="server">
                        <asp:LinkButton ID="save" runat="server" OnClientClick="ValidateForm();Confirm();" OnClick="save_Click"   onmouseover="MM_swapImage('Imag13','','images/type4/save_hover.png',1)">
                        <img src="images/type4/save_normal.png" name="Image13" width="80" height="50" border="0" id="Image13" /></asp:LinkButton>
                       <%-- <div id="dialog" style="display: none" align="center">
                         Do you want to Save this record?
                        </div>--%>
                    </li>
                    <li id="exportBtn" runat="server"><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Imag14','','images/type4/export_hover.png',1)">
                        <img src="images/type4/export_normal.png" alt="Export" name="Imag14" width="80" height="50" border="0" id="Imag14" /></a></li>
                    <li id="importBtn" runat="server"><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image15','','images/type4/import_hover.png',1)">
                        <img src="images/type4/import_normal.png" name="Image15" width="80" height="50" border="0" id="Image15" /></a></li>
                    <li id="printBtn" runat="server">
                        <asp:LinkButton ID="lnkPrint" runat="server" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image16','','images/type4/print_hover.png',1)" OnClick="lnkPrint_Click" OnClientClick=".target ='_blank';"><img src="images/type4/print_normal.png" name="Image16" width="80" height="50" border="0" id="Image16" /></asp:LinkButton>
                    </li>
                    <li id="deleteBtn" runat="server" style="display: none;"><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image17','','images/type4/delete_hover.png',1)">
                        <img src="images/type4/delete_normal.png" name="Image17" width="80" height="50" border="0" id="Image17" /></a></li>
                </ul>
            </div>
            <!--Footer Ends-->
            <script type="text/javascript">
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_beginRequest(beginRequest);

                function beginRequest() {
                    prm._scrollPosition = null;
                }
            </script>
        </div>
    </form>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtTruckNo.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("/AveryService/WebService1.asmx/GetPendingVehicle") %>',
                    data: "{ 'prefix': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('-')[0],
                                val: item.split('-')[1]
                            }
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            select: function (e, i) {
                $("#<%=hfCustomerId.ClientID %>").val(i.item.val);
            },
            minLength: 1
        });

    }); 
</script> 
    <script type="text/javascript">
        $("#ddlinoutna").chosen();
        $("#ddlmaterial").chosen();
        $("#ddlmc").chosen();
        $("#ddlsupplier").chosen();
        $("#ddltransporter").chosen();
        $("#ddlpacking").chosen();
        $("#ddlPOMaterials").chosen();


        function ValidateForm() {
            debugger;
            var result = true;
            var tripid = $('#txtTripId').val();
            var ion = $('#ddlinoutna_chosen').val();
            var gateentryno = $('#txtgateentryno').val();
            var truckno = $('#txtTruckNo').val();
            var material = $('#ddlmaterial').val();
            var transporter = $('#ddltransporter').val();
            var matclassi = $('#ddlmc_chosen').val();
            var supplier = $('#ddlsupplier').val();
            var packing = $('#ddlpacking').val();
            var packingqty = $('#txtpackingqty').val();
            var invoiceno = $('#chainvoiceno').val();
            var invoicedate = $('#chainvoicedate').val();
            var challnawt = $('#txtchallanwt').val();
            var posodono = $('#txtposodono').val();
            var posododate = $('#txtposodate').val();
            var posodomaterial = $('#txtposodomaterials').val();
            var remarks = $('#txtremarks').val();
            var gateentrydate = $('#txtgateentrydate').val();
            if (tripid.trim() == '' || ion.length <= 0 || gateentryno.trim() == '' || truckno.trim() == '' || material.length <= 0 || transporter.length <= 0 || matclassi.length <= 0 ||
                supplier.length <= 0 || packing.length <= 0 || packingqty.trim() == '' || invoiceno.trim() == '' || invoicedate.trim() == '' || challnawt.trim() == '' || posodono.trim() == '' ||
                posododate.trim() == '' || posodomaterial.trim() == '' || remarks.trim() == '' || gateentrydate == '') {
                //toast.error("All fields are mandatory.")
                result = false;
            }
            return result;
        }

    </script>
    <script type="text/javascript">  
        $(document).ready(function () {

            function WeighFormFillUp() {
                var truckNo = $('#txtTruckNo').val();
                $.ajax({
                    type: "POST",
                    url: "/AveryService/WebService1.asmx/GetTransactionDetail",
                    data: "{'truckNo':'" + truckNo + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {
                        var obj = JSON.parse(response.d);
                        console.log(obj);
                        if (obj.VC != null) {
                            $('#cc1').show();
                            $('#cc2').show();
                            $('#cc3').show();
                            $('#VCC').text(obj.VC.ClassificationCode);
                            $('#KW').text(obj.VC.KerbWt);
                            $('#NoA').text(obj.VC.NoOfAxies);
                        }
                        else {
                            $('#cc1').hide();
                            $('#cc2').hide();
                            $('#cc3').hide();
                            $('#VCC').text('');
                            $('#KW').text('');
                            $('#NoA').text('');
                        }
                        if (obj.truckMaster != null)
                        {
                            if (obj.truckMaster.StoredTareWeight > 0 )
                            {
                               
                                    var isGood = confirm('Do you want to use Stored Tare Weight?');
                                    if (isGood) {
                                        $('#IsTareWeightEnabled').val(CheckStoredTarewt5);
                                        $.ajax({
                                            url: "/AveryService/WebService1.asmx/CreatePendingRecord",
                                            type: "post",
                                            dataType: "json",
                                            contentType: "application/json; charset=utf-8",
                                            data: '{TripId:"' + $('#txtTripId').val() + '",truckNo:"' + $('#txtTruckNo').val() + '"}',
                                            async: false,
                                            success: function (data) {

                                            },
                                            error: function (data) {

                                            }
                                        });

                                    }
                                
                            }
                        }
                        if (obj.trans != null) {
                            $('#ddlinoutna').val(obj.trans.TripType.toString().trim());
                            $('#ddlinoutna').trigger("chosen:updated");
                            if (obj.trans.MaterialCode != null) {
                                $('#ddlmaterial').val(obj.trans.MaterialCode.trim());
                                $('#ddlmaterial').trigger("chosen:updated");
                            }
                            if (obj.trans.MaterialCalssificationCode != null) {
                                $('#ddlmc').val(obj.trans.MaterialCalssificationCode.trim());
                                $('#ddlmc').trigger("chosen:updated");
                            }
                            if (obj.trans.SupplierCode != null) {
                                $('#ddlsupplier').val(obj.trans.SupplierCode.trim());
                                $('#ddlsupplier').trigger("chosen:updated");
                            }
                            if (obj.trans.TransporterCode != null) {
                                $('#ddltransporter').val(obj.trans.TransporterCode.trim());
                                $('#ddltransporter').trigger("chosen:updated");
                            }
                            //$('#ddlsupplier').val(obj.trans.SupplierCode.trim());
                            //$('#ddlsupplier').trigger("chosen:updated");
                            //$('#ddltransporter').val(obj.trans.TransporterCode.trim());
                            //$('#ddltransporter').trigger("chosen:updated");
                            if (obj.trans.PackingCode != null) {
                                $('#ddlpacking').val(obj.trans.PackingCode.trim());
                                $('#ddlpacking').trigger("chosen:updated");
                            }
                            //$('#ddlpacking').val(obj.trans.PackingCode.trim());
                           
                            $('#txtpackingqty').val(obj.trans.PackingQty);
                            $('#txtTripId').val(obj.trans.Id);
                            $('#txtInvoiceNo').val(obj.trans.ChallanNo);
                            
                            //$('#txtChallanWeight').val(obj.trans.ChallanWeight);
                            //$('#txtInvoiceDate').val(ConvertToDateFormat(obj.trans.ChallanDate.Value.ToString("dd/MM/yyyy")));
                            //$('#txtInvoiceDate').val(obj.trans.ChallanDate.Value.ToString("dd/MM/yyyy"));
                           
                            if (obj.trans.IsMultiProduct == true) {
                                $("#checkmultiproduct").prop("checked", true);
                                getMultiProducts(obj.transmaterials);
                                //getMultiProducts(obj.trans.Id);
                                $('#PODiv3').show();
                                $('#PODiv4').show();
                            }
                            else {
                                $("#checkmultiproduct").prop("checked", false);
                                $('#PODiv3').hide();
                                $('#PODiv4').hide();
                            }

                            $('#txtChallanWeight').val(obj.trans.ChallanWeight);
                            $('#txtremarks').val(obj.trans.Remarks);
                            $('#txtPONo').val(obj.trans.PONo);
                            
                            $('#txtInvoiceDate').val(ConvertToDateFormat(obj.trans.ChallanDate));
                            $('#txtPODate').val(ConvertToDateFormat(obj.trans.PODate));

                            $('#FirstWeight').val(obj.trans.FirstWeight.Value.ToString("0"));
                            $('#FirstDate').val(obj.trans.FirstWtDateTime.Value.ToString("dd/MM/yyyy"));
                            $('#FirstTime').val(obj.trans.FirstWtDateTime.Value.ToString("HH:mm:ss tt"));
                            //$('#txtInvoiceDate').val(obj.trans.ChallanDate.Value.ToString("dd/MM/yyyy"));
                        }
                        else {
                            $('#ddlPOMaterials').val('0');
                            $('#ddlPOMaterials').trigger("chosen:updated");
                            //$('#ddlinoutna').val('');
                            //$('#ddlinoutna').trigger("chosen:updated");
                            $('#ddlmaterial').val('0');
                            $('#ddlmaterial').trigger("chosen:updated");
                            $('#ddlmc').val('0');
                            $('#ddlmc').trigger("chosen:updated");
                            $('#ddlsupplier').val('0');
                            $('#ddlsupplier').trigger("chosen:updated");
                            $('#ddltransporter').val('0');
                            $('#ddltransporter').trigger("chosen:updated");
                            $('#ddlpacking').val('0');
                            $('#ddlpacking').trigger("chosen:updated");
                            $('#txtpackingqty').val('');
                            // $('#txtTripId').val('');
                            $('#txtInvoiceNo').val('');
                            $('#txtInvoiceDate').val('');
                            $('#txtChallanWeight').val('');
                            
                            $('#txtPONo').val('');
                            $('#txtPODate').val('');
                            $('#txtremarks').val('');
                            $('#FirstDate').val('');
                            $('#FirstTime').val('');
                            $('#FirstWeight').val('');
                            $('#SecondDate').val('');
                            $('#SecondTime').val('');
                            $('#SecondWeight').val('');
                            $('#txtFinalWeight').val('');
                            //$("#checkmultiproduct").prop("checked", false);
                            $('#PODiv3').hide();
                            $('#PODiv4').hide();
                            
                           
                        }
                        if ($('#checkmultiproduct').is(':checked')) {
                            if (obj.transWeight != null) {
                                if (obj.transWeight.length == 0) {
                                    $('#divMaterial').addClass("disable-div");
                                }
                                else {
                                    $('#divMaterial').removeClass("enable-div");
                                }
                            }
                            else {
                                $('#divMaterial').addClass("enable-div");
                            }
                        }

                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            }

            function WeighFormFillUpUsingGateEntry() {
                var gateNo = $('#txtgateentryno').val();
                $.ajax({
                    type: "POST",
                    url: "/AveryService/WebService1.asmx/GetTransactionDetailbygateEntry",
                    data: "{'gateNo':'" + gateNo + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {
                        var obj = JSON.parse(response.d);
                        console.log(obj);
                        $('#txtTruckNo').val(obj.TruckNo)
                        if (obj.VC != null) {
                            $('#cc1').show();
                            $('#cc2').show();
                            $('#cc3').show();
                            $('#VCC').text(obj.VC.ClassificationCode);
                            $('#KW').text(obj.VC.KerbWt);
                            $('#NoA').text(obj.VC.NoOfAxies);
                        }
                        else {
                            $('#cc1').hide();
                            $('#cc2').hide();
                            $('#cc3').hide();
                            $('#VCC').text('');
                            $('#KW').text('');
                            $('#NoA').text('');
                        }
                        if (obj.trans != null) {
                            $('#ddlinoutna').val(obj.trans.TripType.toString().trim());
                            $('#ddlinoutna').trigger("chosen:updated");
                            if (obj.trans.MaterialCode != null) {
                                $('#ddlmaterial').val(obj.trans.MaterialCode.trim());
                                $('#ddlmaterial').trigger("chosen:updated");
                            }
                            if (obj.trans.MaterialCalssificationCode != null) {
                                $('#ddlmc').val(obj.trans.MaterialCalssificationCode.trim());
                                $('#ddlmc').trigger("chosen:updated");
                            }
                            $('#ddlsupplier').val(obj.trans.SupplierCode.trim());
                            $('#ddlsupplier').trigger("chosen:updated");
                            $('#ddltransporter').val(obj.trans.TransporterCode.trim());
                            $('#ddltransporter').trigger("chosen:updated");
                            //$('#ddlpacking').val(obj.trans.PackingCode.trim());
                            //$('#ddlpacking').trigger("chosen:updated");
                            $('#txtpackingqty').val(obj.trans.PackingQty);
                            $('#txtTripId').val(obj.trans.Id);
                            $('#txtInvoiceNo').val(obj.trans.ChallanNo);
                            
                            $('#txtChallanWeight').val(obj.trans.ChallanWeight);
                            $('#txtInvoiceDate').val(ConvertToDateFormat(obj.trans.ChallanDate));
                            if (obj.trans.IsMultiProduct == true) {
                                $("#checkmultiproduct").prop("checked", true);
                                getMultiProducts(obj.transmaterials);
                                //getMultiProducts(obj.trans.Id);
                                $('#PODiv3').show();
                            }
                            else {
                                $("#checkmultiproduct").prop("checked", false);
                                $('#PODiv3').hide();
                            }

                        }
                        else {
                            $('#ddlPOMaterials').val('');
                            $('#ddlPOMaterials').trigger("chosen:updated");
                            //$('#ddlinoutna').val('');
                            //$('#ddlinoutna').trigger("chosen:updated");
                            $('#ddlmaterial').val('');
                            $('#ddlmaterial').trigger("chosen:updated");
                            $('#ddlmc').val('');
                            $('#ddlmc').trigger("chosen:updated");
                            $('#ddlsupplier').val('');
                            $('#ddlsupplier').trigger("chosen:updated");
                            $('#ddltransporter').val('');
                            $('#ddltransporter').trigger("chosen:updated");
                            $('#ddlpacking').val('');
                            $('#ddlpacking').trigger("chosen:updated");
                            $('#txtpackingqty').val('');
                            // $('#txtTripId').val('');
                            $('#txtInvoiceNo').val('');
                            $('#txtInvoiceDate').val('');
                            $('#txtChallanWeight').val('');
                        }
                        if ($('#checkmultiproduct').is(':checked')) {
                            if (obj.transWeight != null) {
                                if (obj.transWeight.length == 0) {
                                    $('#divMaterial').addClass("disable-div");
                                }
                                else {
                                    $('#divMaterial').removeClass("disable-div");
                                }
                            }
                            else {
                                $('#divMaterial').addClass("disable-div");
                            }
                        }

                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            }

            //function WeighFormFillUpPackingWeight() {
            //    var packingcode1 = $('#ddlpacking').val();
            //    $.ajax({
            //        type: "POST",
            //        url: "/AveryService/WebService1.asmx/Get_PackingByCode",
            //        data: "{'PackingCode':'" + packingcode1 + "'}",
            //        contentType: "application/json; charset=utf-8",
            //        dataType: "json",
            //        async: false,
            //        success: function (response) {
            //            var obj = JSON.parse(response.d);
            //            console.log(obj);
            //            $('#ddlpacking').val(obj.PackingCode)
            //            if (obj.VC != null) {
            //                $('#cc1').show();
            //                $('#cc2').show();
            //                $('#cc3').show();
            //                $('#VCC').text(obj.VC.ClassificationCode);
            //                $('#KW').text(obj.VC.KerbWt);
            //                $('#NoA').text(obj.VC.NoOfAxies);
            //            }
            //            else {
            //                $('#cc1').hide();
            //                $('#cc2').hide();
            //                $('#cc3').hide();
            //                $('#VCC').text('');
            //                $('#KW').text('');
            //                $('#NoA').text('');
            //            }
            //            if (obj.trans != null) {
            //                $('#ddlinoutna').val(obj.trans.TripType.toString().trim());
            //                $('#ddlinoutna').trigger("chosen:updated");
            //                if (obj.trans.MaterialCode != null) {
            //                    $('#ddlmaterial').val(obj.trans.MaterialCode.trim());
            //                    $('#ddlmaterial').trigger("chosen:updated");
            //                }
            //                if (obj.trans.MaterialCalssificationCode != null) {
            //                    $('#ddlmc').val(obj.trans.MaterialCalssificationCode.trim());
            //                    $('#ddlmc').trigger("chosen:updated");
            //                }
            //                $('#ddlsupplier').val(obj.trans.SupplierCode.trim());
            //                $('#ddlsupplier').trigger("chosen:updated");
            //                $('#ddltransporter').val(obj.trans.TransporterCode.trim());
            //                $('#ddltransporter').trigger("chosen:updated");
            //                //$('#ddlpacking').val(obj.trans.PackingCode.trim());
            //                //$('#ddlpacking').trigger("chosen:updated");
            //                $('#txtpackingqty').val(obj.trans.PackingQty);
            //                $('#txtTripId').val(obj.trans.Id);
            //                $('#txtInvoiceNo').val(obj.trans.ChallanNo);

            //                $('#txtChallanWeight').val(obj.trans.ChallanWeight);
            //                $('#txtInvoiceDate').val(ConvertToDateFormat(obj.trans.ChallanDate));
            //                if (obj.trans.IsMultiProduct == true) {
            //                    $("#checkmultiproduct").prop("checked", true);
            //                    getMultiProducts(obj.transmaterials);
            //                    //getMultiProducts(obj.trans.Id);
            //                    $('#PODiv3').show();
            //                }
            //                else {
            //                    $("#checkmultiproduct").prop("checked", false);
            //                    $('#PODiv3').hide();
            //                }

            //            }
            //            else {
            //                $('#ddlPOMaterials').val('');
            //                $('#ddlPOMaterials').trigger("chosen:updated");
            //                //$('#ddlinoutna').val('');
            //                //$('#ddlinoutna').trigger("chosen:updated");
            //                $('#ddlmaterial').val('');
            //                $('#ddlmaterial').trigger("chosen:updated");
            //                $('#ddlmc').val('');
            //                $('#ddlmc').trigger("chosen:updated");
            //                $('#ddlsupplier').val('');
            //                $('#ddlsupplier').trigger("chosen:updated");
            //                $('#ddltransporter').val('');
            //                $('#ddltransporter').trigger("chosen:updated");
            //                $('#ddlpacking').val('');
            //                $('#ddlpacking').trigger("chosen:updated");
            //                $('#txtpackingqty').val('');
            //                // $('#txtTripId').val('');
            //                $('#txtInvoiceNo').val('');
            //                $('#txtInvoiceDate').val('');
            //                $('#txtChallanWeight').val('');
            //            }
            //            if ($('#checkmultiproduct').is(':checked')) {
            //                if (obj.transWeight != null) {
            //                    if (obj.transWeight.length == 0) {
            //                        $('#divMaterial').addClass("disable-div");
            //                    }
            //                    else {
            //                        $('#divMaterial').removeClass("disable-div");
            //                    }
            //                }
            //                else {
            //                    $('#divMaterial').addClass("disable-div");
            //                }
            //            }

            //        },
            //        error: function (response) {
            //            console.log(response);
            //        }
            //    });
            //}

            function CheckStoredTarewt5() {
                //var gateNo = $('#txtgateentryno').val();
                $.ajax({
                    type: "POST",
                    url: "/AveryService/WebService1.asmx/CheckStoredtareWt",
                    //data: "{'gateNo':'" + gateNo + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {
                        var obj = JSON.parse(response.d);
                       
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            }

            function getMultiProducts(transWeight) {
                var tablediv = '<table style="width: 100%;" border="1">';
                tablediv = tablediv + '<tr><td style="text-align: center;">Material</td><td style="text-align: center;">Date</td><td style="text-align: center;">Weight(kg)</td></tr>';
                $.each(transWeight, function (key, value) {
                    var nowDate = new Date(parseInt(value.CreteDate.substr(6)));
                    tablediv = tablediv + '<tr><td style="text-align: center;">' + value.MaterialName + '</td><td style="text-align: center;">' + nowDate.format("dd/MM/yyyy HH:mm:ss tt") + '</td><td style="text-align: center;">' + value.Weight + '</td></tr>';
                });
                tablediv = tablediv + '</table>'
                $('#multiMaterials').empty();
                $('#multiMaterials').append(tablediv);
            }

           

            $("#txtTruckNo").change(function () {
                WeighFormFillUp();
            });

           

            $("#txtgateentryno").change(function () {
                WeighFormFillUpUsingGateEntry();
            });

            function ConvertToDateFormat(date) {
                var nowDate = new Date(parseInt(date.substr(6)));
                var result = "";
                result += nowDate.format("dd/MM/yyyy");
                return result;
            }

            $('#checkmultiproduct').change(function () {
                if ($('#checkmultiproduct').is(':checked')) {
                    WeighFormFillUp();
                }
                if ($(this).is(":checked")) {
                    $('#PODiv3').show();
                }
                else {
                    $('#PODiv3').hide();
                    $('#divMaterial').removeClass("disable-div");
                }
                //'unchecked' event code
            });
        });
    </script>
    <script>
        $(function () {
            $("#txtInvoiceDate").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#txtPODate").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
    <script language="javascript" type="text/javascript">
        function doCheck() {


            var keyCode = (event.which) ? event.which : event.keyCode;
            if ((keyCode == 8) || (keyCode == 46))
                event.returnValue = false;


        }
    </script>
    <script type="text/javascript">
        document.addEventListener('keydown', function (event) {
            if (event.keyCode === 13 && event.target.nodeName === 'INPUT' || event.target.nodeName === 'SELECT') {
                var form = event.target.form;
                var index = Array.prototype.indexOf.call(form, event.target);
                form.elements[index + 1].focus();
                event.preventDefault();
            }
        });
    </script>

    <script type="text/javascript">
        function killBackSpace(e) {
            e = e ? e : window.event;
            var t = e.target ? e.target : e.srcElement ? e.srcElement : null;
            if (t && t.tagName && (t.type && /(password)|(text)|(file)/.test(t.type.toLowerCase())) || t.tagName.toLowerCase() == 'textarea')
                return true;
            var k = e.keyCode ? e.keyCode : e.which ? e.which : null;
            if (k == 8) {
                if (e.preventDefault)
                    e.preventDefault();
                return false;
            };
            return true;
        };
        if (typeof document.addEventListener != 'undefined')
            document.addEventListener('keydown', killBackSpace, false);
        else if (typeof document.attachEvent != 'undefined')
            document.attachEvent('onkeydown', killBackSpace);
        else {
            if (document.onkeydown != null) {
                var oldOnkeydown = document.onkeydown;
                document.onkeydown = function (e) {
                    oldOnkeydown(e);
                    killBackSpace(e);
                };
            }
            else
                document.onkeydown = killBackSpace;
        }


     </script>
    <script type="text/javascript">
        var myclose = false;
        function ConfirmClose() {
            if (event.clientY < 0) {
                event.returnValue = 'You have closed the browser. Do you want to logout from your application?';
                setTimeout('myclose=false', 10);
                myclose = true;
            }
        }

        function HandleOnClose() {
            if (myclose == true) {
                //the url of your logout page which invalidate session on logout 
                location.replace('/contextpath/j_spring_security_logout');
            }
        }
    </script>

    <script type="text/javascript" language="Javascript">

        function DetectBrowserExit() {
            alert('Execute task which do you want before exit');
        }

        //window.onbeforeunload = function () { DetectBrowserExit(); }

    </script>

    
</body>
</html>
