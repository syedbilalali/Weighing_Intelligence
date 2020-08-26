<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="Avery_Weigh.View.Header" %>
<div class="WeightHeader">
    <div class="One">
        <div class="LeftOne">
            <img src="/images/login/gold_yes_warrantee_yes_guarantee.png" alt="" />

        </div>
        <div class="RightOne">
            <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Avery-Weight-Tronix-Logo.png" />
        </div>
    </div>
    <asp:ScriptManager ID="sc1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div class="Two">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="110" align="right" class="fontbold">OPERATOR</td>
                <td class="height25">
                    <asp:Label ID="txtoperatorName" runat="server" CssClass="margin45"></asp:Label>
                    <%--<input type="text" id="txtoperatorName" readonly runat="server" name="" value="NAME OF OPERATOR" placeholder="NAME OF OPERATOR" onfocus="if(this.value == 'NAME OF OPERATOR'){this.value = '';}" /></td>--%>
            </tr>
            <tr>
                <td align="right" class="fontbold">INSTALLED ON</td>
                <td class="height25">
                    <asp:Label ID="txtInstalledOn" runat="server" CssClass="margin45"></asp:Label>
                    <%--<input type="text" name="" id="txtInstalledOn" runat="server" value="00.00.0000" placeholder="00.00.0000" onfocus="if(this.value == '00.00.0000'){this.value = '';}" />--%></td>
            </tr>
            <tr>
                <td align="right" class="fontbold">WB ID.</td>
                <td class="height25">
                    <asp:Label ID="txtWBId" runat="server" CssClass="margin45"></asp:Label>
                    <%--<input type="text" name="" id="txtWBId" runat="server" value="XXXXXXXXXXXXXXXXXXXXX" placeholder="XXXXXXXXXXXXXXXXXXXXX" onfocus="if(this.value == 'XXXXXXXXXXXXXXXXXXXXX'){this.value = '';}" />--%></td>
            </tr>
            <tr>
                <td align="right" class="fontbold">PLANT ID.</td>
                <td class="height25">
                    <asp:Label ID="txtPlantId" runat="server" CssClass="margin45"></asp:Label>
                    <%--<input type="text" name="" id="txtPlantId" runat="server" value="XXXXXXXXXXXXXXXXXXXXX" placeholder="XXXXXXXXXXXXXXXXXXXXX" onfocus="if(this.value == 'XXXXXXXXXXXXXXXXXXXXX'){this.value = '';}" /></td>--%>
            </tr>
        </table>
    </div>
    <div class="Three">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="max-width: 223px;">
            <tr>
                <td>1
                        <%--<img src="/images/type6/camera_red.png" alt="" align="absmiddle" />--%>
                        <asp:UpdatePanel ID="up_imagecamera1" runat="server" style="width: 75%; float: right;">
                            <ContentTemplate>
                                     
                                    <asp:ImageButton ID="cameraImageButton1" runat="server" ImageUrl="~/images/type6/camera_grey.png" Height="20px" Width="25px" BorderStyle="None" />
                       
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
                <td>1
                                <%--<img src="/images/type6/rfid_grey.png" alt="" align="absmiddle" />--%>
                       <asp:UpdatePanel ID="Up_rfidreader1" runat="server" style="width: 75%; float: right;">
                            <ContentTemplate>
                                     
                                    <asp:ImageButton ID="rfidImageButton1" runat="server" ImageUrl="~/images/type6/rfid_grey.png" Height="20px" Width="25px" BorderStyle="None" />
                       
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
                 <td>1
                        <%--<img  src="/images/type6/reader_grey.png" alt="" align="absmiddle" />--%>
                        <%-- <asp:Image ID="Image6" runat="server"  ImageUrl="~/images/type6/reader_grey.png" ImageAlign="absmiddle"/>--%>
                        <asp:UpdatePanel ID="up_image21" runat="server" style="width: 75%; float: right;">
                            <ContentTemplate>
                       
                                                <%--<asp:Image ID="Image6" runat="server"  ImageUrl="~/images/type6/reader_grey.png" ImageAlign="absmiddle" Height="20px"/>--%>
                                    <asp:ImageButton ID="SensorImageBtn1" runat="server" ImageUrl="~/images/type6/reader_grey.png" Height="17px" Width="30px" BorderStyle="None" />
                       
                            </ContentTemplate>
                        </asp:UpdatePanel>
                 </td>

                 <td>1
                                <img src="/images/type6/barrier_close_grey.png" alt="" align="absmiddle" /></td>
            </tr>
            <tr>
                <td>2
                               <%-- <img src="/images/type6/camera_grey.png" alt="" align="absmiddle" />--%>
                        <asp:UpdatePanel ID="up_imagecamera12" runat="server" style="width: 75%; float: right;">
                            <ContentTemplate>
                                     
                                    <asp:ImageButton ID="cameraImageButton2" runat="server" ImageUrl="~/images/type6/camera_grey.png" Height="20px" Width="25px" BorderStyle="None" />
                       
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
                <td>2
                               <%-- <img src="/images/type6/rfid_red.png" alt="" align="absmiddle" />--%>
                         <asp:UpdatePanel ID="Up_rfidreader2" runat="server" style="width: 75%; float: right;">
                            <ContentTemplate>
                                     
                                    <asp:ImageButton ID="rfidImageButton2" runat="server" ImageUrl="~/images/type6/rfid_grey.png" Height="20px" Width="25px" BorderStyle="None" />
                       
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
                <td>2
                <%-- <img  src="/images/type6/reader_red.png" alt="" align="absmiddle" />--%>
                    <asp:UpdatePanel ID="up_image" runat="server" style="width: 75%; float: right;">
                        <ContentTemplate>

                            <%--<asp:Image ID="Image5" runat="server" ImageUrl="~/images/type6/reader_red.png" ImageAlign="absmiddle"  Height="20px"/>--%>
                            <asp:ImageButton ID="SensorImageBtn2" runat="server" ImageUrl="~/images/type6/reader_grey.png" Height="17px" Width="30px" BorderStyle="None" />

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </td>
                <td>2
                                <img src="/images/type6/barrier_close_grey.png" alt="" align="absmiddle" /></td>
            </tr>
            <tr>
                <td>3
                          <%--      <img src="/images/type6/camera_green.png" alt="" align="absmiddle" />--%>
                        <asp:UpdatePanel ID="up_imagecamera123" runat="server" style="width: 75%; float: right;">
                            <ContentTemplate>
                                     
                                    <asp:ImageButton ID="cameraImageButton3" runat="server" ImageUrl="~/images/type6/camera_grey.png" Height="20px" Width="25px" BorderStyle="None" />
                       
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
                <td>3
                                <%--<img src="/images/type6/rfid_red.png" alt="" align="absmiddle" />--%>
                     <asp:UpdatePanel ID="Up_rfidreader3" runat="server" style="width: 75%; float: right;">
                            <ContentTemplate>
                                     
                                    <asp:ImageButton ID="rfidImageButton3" runat="server" ImageUrl="~/images/type6/rfid_grey.png" Height="20px" Width="25px" BorderStyle="None" />
                       
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
                <td>3
                <asp:UpdatePanel ID="up_image11" runat="server" style="width: 75%; float: right;">
                    <ContentTemplate>

                        <%--<asp:Image ID="Image7" runat="server"  ImageUrl="~/images/type6/reader_green.png" ImageAlign="absmiddle" Height="20px"/>--%>
                        <asp:ImageButton ID="SensorImageBtn3" runat="server" ImageUrl="~/images/type6/reader_grey.png" Height="17px" Width="30px" BorderStyle="None" />

                    </ContentTemplate>
                </asp:UpdatePanel>
                </td>
                <%-- <asp:Image ID="Image4" runat="server"  ImageUrl="~/images/type6/reader_green.png" ImageAlign="absmiddle"/>--%>
                <%--<img  src="/images/type6/reader_green.png" alt="" align="absmiddle" />--%>
                <td>
                    <img src="/images/type6/server_grey.png" alt="" align="absmiddle" /></td>
            </tr>
        </table>

    </div>
    <div class="Four">
        <div class="TopRLogo">
            <img src="/images/Avery-Weight-Tronix-Logo.png" alt="" />
        </div>
        <div class="ConatctIcons1">
            <ul>
                <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image2','','/images/contact/info_hover.png',1)">
                    <img src="/images/contact/info_normal.png" name="Image2" width="42" height="42" border="0" id="Image2" /></a></li>
                <li><a href="javascript:void(0);" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image1','','/images/contact/contact_hover.png',1)">
                    <img src="/images/contact/contact_normal.png" name="Image1" width="42" height="42" border="0" id="Image1" /></a></li>
                <li><a href="/logout" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image3','','/images/contact/login_hover.png',1)">
                    <img src="/images/contact/login_normal.png" name="Image3" width="42" height="42" border="0" id="Image3" /></a></li>
            </ul>
        </div>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('input[type=text]').focus(function () {
            $(this).addClass("form-control").siblings().removeClass("form-control");
        });
    });
    $(document).ready(function () {
        debugger;
        $(window).bind('beforeunload', function () {
            $ajax({
                url: "Logout.aspx",
            });
        });
    });
</script>
