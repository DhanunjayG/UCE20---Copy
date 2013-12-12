<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SavePhotosFolders.aspx.cs"
    Inherits="SavePhotosFolders" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="scrptmgr" runat="server">
    </cc1:ToolkitScriptManager>
    <div>
        <div style="float: left; text-align: left; width: 45%;">
            <asp:FileUpload ID="fupExcelFile" runat="server" /><br />
            <i>(Upload 1997-2003 excel file with .xls extension)</i>
        </div>
        <br />
        <table width="100%">
            <tr>
                <td style="width: 106px;">
                </td>
                <td align="left">
                    <asp:Button runat="server" ID="btnUpload" CssClass="button-primary" Text="Update"
                        OnClick="btnUpload_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="updtpnlCount" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblCount" runat="server"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblResult" runat="server"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
