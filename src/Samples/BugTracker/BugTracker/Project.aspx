<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Project.aspx.cs" Inherits="BugTracker.Project" %>

<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="Main" runat="server">
    <div>
    <div class="subtitle" >Project</div>
        <asp:DetailsView runat="server" Height="50px" Width="314px" ID="ProjectDetailView"
            AutoGenerateRows="False" DataKeyNames="Id" 
            DataSourceID="ProjectDataSource" 
            OnModeChanging="ProjectDetailView_ModeChanging" CssClass="details" 
            BorderWidth="0px">
            <Fields>
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id"
                    InsertVisible="False" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
                <asp:BoundField DataField="StartDate" HeaderText="StartDate" 
                    SortExpression="StartDate" />
                <asp:CommandField ShowEditButton="True" ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
        <div class="subtitle">Project team</div>
        <asp:GridView ID="TeamGridView" runat="server"  AutoGenerateColumns="False" 
            DataKeyNames="Id" DataSourceID="TeamDataSource" AllowSorting="True" 
            Width="314px" BorderWidth="0px" CssClass="grid">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" 
                    SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="Project.Id" HeaderText="Project.Id" 
                    SortExpression="Project.Id" Visible="False" />
                <asp:TemplateField HeaderText="Role" SortExpression="it.Role.Name">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" 
                            DataSourceID="RolesDataSource" DataTextField="Name" DataValueField="Id" 
                            SelectedValue='<%# Bind("Role.Id") %>'>
                        </asp:DropDownList>
                        <asp:EntityDataSource ID="RolesDataSource" runat="server" 
                            ConnectionString="name=DBContext" DefaultContainerName="DBContext" 
                            EntitySetName="Role" Select="it.[Id], it.[Name]">
                        </asp:EntityDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Role.Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="User" SortExpression="it.User.Name">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server" 
                            DataSourceID="UserDataSource" DataTextField="Name" DataValueField="Id" 
                            SelectedValue='<%# Bind("User.Id") %>'>
                        </asp:DropDownList>
                        <asp:EntityDataSource ID="UserDataSource" runat="server" 
                            ConnectionString="name=DBContext" DefaultContainerName="DBContext" 
                            EntitySetName="User" Select="it.[Id], it.[Name]">
                        </asp:EntityDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("User.Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:EntityDataSource ID="ProjectDataSource" runat="server" ConnectionString="name=DBContext"
            DefaultContainerName="DBContext" EnableDelete="True" EnableInsert="True" EnableUpdate="True"
            EntitySetName="Project" Include="" AutoGenerateWhereClause="false"
            Where="it.Id = @Id" >
            <WhereParameters>
            <asp:QueryStringParameter ConvertEmptyStringToNull="true"
                QueryStringField="Id" DbType="Int32" Name="Id" />
            </WhereParameters>
        </asp:EntityDataSource>
        
        <asp:HyperLink ID="AddMemberLink" runat="server" 
            >Add member...</asp:HyperLink>
        <asp:EntityDataSource ID="TeamDataSource" runat="server" 
            ConnectionString="name=DBContext" DefaultContainerName="DBContext" 
            EnableDelete="True" EnableInsert="True" EnableUpdate="True" 
            EntitySetName="ProjectTeam" AutoGenerateWhereClause="false"
            Include="Project,User,Role"
            Where="it.Project.Id = @ProjectId">
            <WhereParameters>
            <asp:ControlParameter 
                ControlID="ProjectDetailView" 
                DbType="Int32" Name="ProjectId" />
            </WhereParameters>
        </asp:EntityDataSource>
    </div>
</asp:Content>