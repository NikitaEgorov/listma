<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectTeam.aspx.cs" Inherits="BugTracker.ProjectTeam" %>

<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="Main" runat="server">
    <div>
        <asp:DetailsView ID="ProjectTeamDetailsView" runat="server" AutoGenerateRows="False"
            DataKeyNames="Id" DataSourceID="ProjectTeamEntityDataSource" Height="50px" 
            Width="222px" onmodechanging="ProjectTeamDetailsView_ModeChanging" 
            oniteminserting="ProjectTeamDetailsView_ItemInserting" 
            DefaultMode="Insert" oniteminserted="ProjectTeamDetailsView_ItemInserted" 
            CssClass="details">
            <Fields>
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" 
                    SortExpression="Id" InsertVisible="False" />
                <asp:TemplateField HeaderText="Project" SortExpression="Project.Id">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("[Project.Id]") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="ProjectDropDown" runat="server" 
                            DataSourceID="ProjectDataSource" DataTextField="Name" DataValueField="Id" 
                             >
                        </asp:DropDownList>
                        <asp:EntityDataSource ID="ProjectDataSource" runat="server" 
                            ConnectionString="name=DBContext" DefaultContainerName="DBContext" 
                            EntitySetName="Project" Select="it.[Id], it.[Name]" AutoGenerateWhereClause="false"
                            Where="it.[Id] = @ProjId">
                            <WhereParameters>
                            <asp:QueryStringParameter QueryStringField="ProjectId" DbType="Int32" Name="ProjId" />
                            </WhereParameters>
                        </asp:EntityDataSource>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Project.Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Role" SortExpression="Role.Id">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("[Role.Id]") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="RoleDropDown" runat="server" 
                            DataSourceID="RoleDataSource" DataTextField="Name" DataValueField="Id" 
                            >
                        </asp:DropDownList>
                        <asp:EntityDataSource ID="RoleDataSource" runat="server" 
                            ConnectionString="name=DBContext" DefaultContainerName="DBContext" 
                            EntitySetName="Role" Select="it.[Id], it.[Name]">
                        </asp:EntityDataSource>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Role.Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="User" SortExpression="User.Id">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("[User.Id]") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="UserDropDown" runat="server" 
                            DataSourceID="UserDataSource" DataTextField="Name" DataValueField="Id" >
                        </asp:DropDownList>
                        <asp:EntityDataSource ID="UserDataSource" runat="server" 
                            ConnectionString="name=DBContext" DefaultContainerName="DBContext" 
                            EntitySetName="User" Select="it.[Id], it.[Name]">
                        </asp:EntityDataSource>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("User.Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
    </div>
    <asp:EntityDataSource ID="ProjectTeamEntityDataSource" runat="server" ConnectionString="name=DBContext"
        DefaultContainerName="DBContext" EnableDelete="True" EnableInsert="True" EnableUpdate="True"
        EntitySetName="ProjectTeam" Include="Project, Role, User">
    </asp:EntityDataSource>
</asp:Content>