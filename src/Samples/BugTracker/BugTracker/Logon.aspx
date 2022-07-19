<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="BugTracker.Logon" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <div>
        <asp:Login ID="Login1" runat="server" CssClass="details" CreateUserText="Register" 
            CreateUserUrl="~/register/RegisterUser.aspx" 
            onauthenticate="Login1_Authenticate" BackColor="#EFF3FB" >
            
        </asp:Login>
    </div>
</asp:Content>