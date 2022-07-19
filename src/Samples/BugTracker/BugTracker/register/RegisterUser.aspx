<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegisterUser.aspx.cs" Inherits="BugTracker.RegisterUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<div class="subtitle">Register new user</div>
<div>
    <table cellspacing="0" border="0" class="details" >
         <tr>
            <td >
                &nbsp;
                <asp:Label ID="Label1" runat="server" Text="User Name"></asp:Label>
                :</td>
            <td>
                &nbsp;
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="*" ControlToValidate="txtName"></asp:RequiredFieldValidator>
            </td>

        </tr>
        <tr style="background-color: #EFF3FB;">
            <td class="style1">
                &nbsp;
                <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
                :</td>
            <td>
                &nbsp;
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtPassword" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
 
        </tr>
        <tr>
            <td class="style1">
                &nbsp;
            </td>
            <td>
                &nbsp;
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Register" 
                     style="color:#284E98;background-color:White;border-color:#507CD1;border-width:1px;border-style:Solid;font-family:Verdana;font-size:0.8em;" />
            </td>
 
        </tr>
    </table>
</div>
   </asp:Content>
