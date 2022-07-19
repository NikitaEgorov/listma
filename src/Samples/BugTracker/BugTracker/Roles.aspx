<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="BugTracker.Roles" %>

<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
   <div class="subtitle">Roles</div>
    <div>
        <asp:GridView ID="RoleGridView" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="Id" DataSourceID="RoleDataSource" Width="269px" 
            RowHeaderColumn="Id" CssClass="grid">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="Id" 
                    DataNavigateUrlFormatString="~/Role.aspx?Id={0}" Target="_self" Text="Edit" />
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" 
                    SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
            </Columns>
        </asp:GridView>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Role.aspx?Id=0" 
            Target="_self">Add new role...</asp:HyperLink>
    </div>
    <asp:EntityDataSource ID="RoleDataSource" runat="server" 
        ConnectionString="name=DBContext" DefaultContainerName="DBContext" 
        EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
        EntitySetName="Role">
    </asp:EntityDataSource>
    </form>
</asp:Content>
