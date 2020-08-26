<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorLogs.aspx.cs" Inherits="Avery_Weigh.ErrorLogs" %>

<%@ Register Src="~/View/Header.ascx" TagPrefix="uc1" TagName="Header" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Avery Weigh Tronix - Weigh</title>

    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/css/fonts.css" type="text/css" charset="utf-8" />
    <link rel="icon" href="/images/favicon.png" type="image/gif" sizes="16x16" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />
    <script src="/js/jquery.min.js"></script>
    <script src="../js/jquery-1.12.4.js"></script>

    <script src="../Scripts/jquery-3.3.1.min.js"></script>
    <script src="../js/jquery-ui.js"></script>

    <script>
        $(function () {
            // When any checkbox is checked
            $(':checkbox').click(function () {
                // Uncheck any other checkboxes except this one
                $(':checkbox').not($(this)).prop('checked', false);
            });
        })
</script>

</head>

<body onload="MM_preloadImages('images/contact/contact_hover.png','images/contact/info_hover.png','images/contact/login_hover.png','images/close_hover.png','images/fullscreen_hover.png','images/exit_fullscreen_hover.png','images/type3/copy_hover.png','images/type3/previous_page_hover.png','images/type3/previous_record_hover.png','images/type3/next_record_hover.png','images/type3/next_page_hover.png')">
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
                            <img src="images/type1/masters_normal.png" name="Image6" width="166" height="40" border="0" id="Image6" /></a></li>
                        <li><a href="/dashboard" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image7','','/images/type1/dashboard_hover.png',1)">
                            <img src="images/type1/dashboard_select.png" name="Image7" width="166" height="40" border="0" id="Image7" /></a></li>
                        <li><a href="/ManageServices" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image8','','/images/type1/service_hover.png',1)">
                            <img src="/images/type1/service_normal.png" name="Image8" width="166" height="40" border="0" id="Image8" /></a></li>
                        <li><a href="diagnostics.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image9','','/images/type1/diagnostics_hover.png',1)">
                            <img src="/images/type1/diagnostics_normal.png" name="Image9" width="166" height="40" border="0" id="Image9" /></a></li>
                    </ul>
                    <span>
                        <img src="/images/intellegence-left-logo.png" alt="" /></span>
                </div>

                <div class="RightSec">
                    <div class="RightInn11 textcon" style="overflow: hidden">
                        <h4>DASHBOARD</h4>
                        <h4 class="SupplierSearchBig">Error Logs Reports <a href="/dashboardmaster" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image32','','images/close_hover.png',1)" class="CloseButtonA">
                            <img src="/images/close_normal.png" class="Close" alt="Close" name="Image32" width="26" height="25" border="0" id="Image32" /></a></h4>
                    </div>
                    <div class="RightInn11 dashboardmidsec">
                        <asp:HiddenField ID="RecordId" runat="server" />
                        <!-- <div class="RightInn22">3333</div> -->
                        <div class="RightInnMasters ">
                            <div class="ConatctIcons2">
                                <ul style="display: none;">
                                    <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Copy','','images/type3/copy_hover.png',1)">
                                        <img src="/images/type3/copy_normal.png" name="Copy" width="50" height="50" border="0" id="Copy" /></a></li>
                                    <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('previous_page','','images/type3/previous_page_hover.png',1)">
                                        <img src="/images/type3/previous_page_normal.png" name="previous_page" width="50" height="50" border="0" id="previous_page" /></a></li>
                                    <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('previous_record','','images/type3/previous_record_hover.png',1)">
                                        <img src="/images/type3/previous_record_normal.png" name="previous_record" width="50" height="50" border="0" id="previous_record" /></a></li>
                                    <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('next_record','','images/type3/next_record_hover.png',1)">
                                        <img src="/images/type3/next_record_normal.png" name="next_record" width="50" height="50" border="0" id="next_record" /></a></li>
                                    <li><a href="login.html" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('next_page','','images/type3/next_page_hover.png',1)">
                                        <img src="/images/type3/next_page_normal.png" name="next_page" width="50" height="50" border="0" id="next_page" /></a></li>
                                </ul>
                            </div>


                            <div id="dbMain" runat="server" class="tablemaster">
                                <table style="width: 50%;" border="0" class="MainLTable">
                                    <tr>
                                       <%-- <td>Transaction Type :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="FilddHalf112" Width="287px">
                                            <asp:ListItem Text="Completed Transaction" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Pending Transaction" Value="1"></asp:ListItem>
                                            </asp:DropDownList>

                                        </td>--%>
                                        
                                        <td>From :</td>
                                        <td>
                                            <asp:TextBox ID="txtfrom" runat="server" CssClass="field FilddHalf70"></asp:TextBox></td>
                                        <td>To :</td>
                                        <td>
                                            <asp:TextBox ID="txtTo" runat="server" CssClass="field FilddHalf70"></asp:TextBox></td>
                                        <td>
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" class="my_btn" />

                                        </td>
                                    </tr>
                                </table>
                                 <asp:Panel ID="Panel1" runat="server">
                                <table style="width: 100%;" border="1" bordercolor="#72aac0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" class="MainLTable">


                                    <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">
                                        <HeaderTemplate>
                                            <tr>
                                                <%--<td align="center" valign="middle" bgcolor="#d8e7ee">Select</td>--%>
                                                <td align="center" valign="middle" bgcolor="#d8e7ee">ID</td>
                                                <td align="center" valign="middle" bgcolor="#d8e7ee">User Name</td>
                                                <td align="center" valign="middle" bgcolor="#d8e7ee">Plant Code</td>
                                                <td align="center" valign="middle" bgcolor="#d8e7ee">Error Title</td>
                                                <td align="center" valign="middle" bgcolor="#d8e7ee">Error Description</td>
                                                <td align="center" valign="middle" bgcolor="#d8e7ee">URL</td>
                                                <td align="center" valign="middle" bgcolor="#d8e7ee">Error Date/Time</td>
                                               
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="trclass">
                                               <%-- <td align="center" valign="middle">
                                                    <asp:CheckBox ID="checkRecord" runat="server" OnCheckedChanged="checkRecord_CheckedChanged" AutoPostBack="true" /></td>--%>
                                                <%--<td align="center" valign="middle" data-rowid="<%#Eval("Id") %>"></td>--%>

             

                                                <td align="center" valign="middle">
                                                    <asp:Label ID="lblTripId" runat="server" Text='<%#Eval("ID") %>'></asp:Label></td>
                                                <td align="center" valign="middle"><%#Eval("UserId") %></td>
                                                <td align="center" valign="middle"><%#Eval("PlantCode") %></td>
                                                <td align="center" valign="middle"><%#Eval("LogTitle") %></td>
                                                <td align="center" valign="middle"><%#Eval("LogDescription") %></td>
                                                <td align="center" valign="middle"><%#Eval("URL") %></td>
                                                <td align="center" valign="middle"><%#Eval("LogDate") %></td>
                                                <%--<td align="center" valign="middle"><%#Eval("MaterialName") %></td>
                                                <td align="center" valign="middle"><%#Eval("SupplierName") %></td>
                                                <td align="center" valign="middle"><%#Eval("WeighingUnit") %></td>--%>



                                            </tr>

                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                                     </asp:Panel>
                            </div>
                            <table id="tblNone" runat="server" visible="false" style="width: 100%;" border="1" bordercolor="#72aac0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" class="MainLTable">
                                <thead>
                                    <tr>
                                        <td align="center" valign="middle" bgcolor="#d8e7ee">No.</td>
                                        <td align="center" valign="middle" bgcolor="#d8e7ee">Plant Code</td>
                                        <td align="center" valign="middle" bgcolor="#d8e7ee">Plant Name</td>
                                        <td align="center" valign="middle" bgcolor="#d8e7ee">Address</td>
                                    </tr>
                                </thead>
                                <tr>
                                    <td colspan="4" style="text-align: center;">No Record Found</td>
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
                   <%-- <li><a href="PlantSearch.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image11','','images/type4/search_hover.png',1)">
                        <img src="/images/type4/search_normal.png" name="Image11" width="80" height="50" border="0" id="Image11" /></a></li>
                    <li><asp:LinkButton ID="LnkAddEdit" OnClick="LnkAddEdit_Click1" runat="server" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image12','','images/type4/edit_hover.png',1)">
                        <img src="/images/type4/edit_normal.png" name="Image12" width="80" height="50" border="0" id="Image12" />
                        </asp:LinkButton></li>
                    <li><a href="javascript:void(0)" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image13','','images/type4/save_hover.png',1)">
                        <img src="/images/type4/save_normal.png" name="Image13" width="80" height="50" border="0" id="Image13" /></a></li>
                    <li><a href="PlantImport.aspx" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Imag14','','images/type4/export_hover.png',1)">
                        <img src="/images/type4/export_normal.png" alt="Export" name="Imag14" width="80" height="50" border="0" id="Imag14" /></a></li>
                    <li><asp:LinkButton runat="server" ID="BtnLinkExport" OnClick="BtnLinkExport_Click" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image15','','images/type4/import_hover.png',1)">
                        <img src="/images/type4/import_normal.png" name="Image15" width="80" height="50" border="0" id="Image15" /></asp:LinkButton></li>--%>
                    <li style="float:right">
                        <asp:LinkButton ID="linkPrint" runat="server" OnClick="linkPrint_Click"
                      onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image16','','images/type4/print_hover.png',1)">
                        <img src="/images/type4/print_normal.png" name="Image16" width="80" height="50" border="0" id="Image16" /></asp:LinkButton></li>
                   <%-- <li><asp:LinkButton ID="BtnDelete" runat="server" OnClick="BtnDelete_Click" OnClientClick="return Delete();" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image17','','images/type4/delete_hover.png',1)">
                        <img src="/images/type4/delete_normal.png" name="Image17" width="80" height="50" border="0" id="Image17" /></asp:LinkButton></li>--%>
                </ul>
            </div>
            <script type="text/javascript">
                $(function () {
                    $('.trclass').click(function () {
                        if ($('.trclass').hasClass("selectedrow")) {
                            $('.trclass').removeClass('selectedrow');
                            $("#RecordId").val('');
                        }
                        else if (!$('.trclass').hasClass("selectedrow")) {
                            $(this).addClass('selectedrow');
                            $("#RecordId").val(($(this).attr("data-RowId")));
                            $(this).siblings().removeClass('selectedrow');
                        }
                    });
                });

                function Delete() {
                    var result = true;
                    var value = $('#RecordId').val();
                    if (value != '') {
                        return confirm('Do You want to delete');
                    } else {
                        toastr.warning('Please select a record first.');
                        result = false;
                    }
                    return result;
                }
            </script>
            <script>
                $(function () {
                    $("#txtTo").datepicker({ dateFormat: 'dd-mm-yy' });
                    $("#txtfrom").datepicker({ dateFormat: 'dd-mm-yy' });

                });
            </script>
        </div>
    </form>
</body>
</html>

