<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default3.aspx.cs" Inherits="Default3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9">
    <title>Untitled Page</title>

    <script type="text/javascript" src="js/jquery-1.7.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="CarDetailsURLs" OnClick="Button1_Click" />
        
        <asp:Button ID="btnStateWiseURL" runat="server" Text="StateWiseURL" 
            onclick="btnStateWiseURL_Click" />
     

    </div>
    </form>
</body>
</html>
