<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Role.aspx.cs" Inherits="BugTracker.Role" %>

<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
   <div><span class="subtitle">Role</span></div>
    <div>
    
        <asp:DetailsView ID="RoleDetailsView" runat="server" AutoGenerateRows="False" 
            DataKeyNames="Id" DataSourceID="RoleDataSource" Height="50px" 
            Width="125px" 
             oniteminserted="RoleDetailsView_ItemInserted" 
            onitemupdated="RoleDetailsView_ItemUpdated" 
            onmodechanging="RoleDetailsView_ModeChanging" CssClass="details">
            <Fields>
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" 
                    SortExpression="Id" InsertVisible="False" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:CommandField ShowEditButton="True" 
                    ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
    
    </div>
    <asp:EntityDataSource ID="RoleDataSource" runat="server" 
        ConnectionString="name=DBContext" DefaultContainerName="DBContext" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="Role" 
        Where="">
    </asp:EntityDataSource>
</asp:Content>
