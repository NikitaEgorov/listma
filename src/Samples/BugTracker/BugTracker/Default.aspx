<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="BugTracker._Default" %>

<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ContentPlaceHolderID="Main" runat="server">
  
    <div>
        <table cellspacing="0" cellpadding="4" border="0" id="Login1">
            <tr>
                <td>
                    <asp:Button ID="AddIssue" runat="server" Text="New Issue..." OnClick="AddIssue_Click"
                         CssClass="button" 
                     />
                </td>
                <td>
                    Project:
                </td>
                <td>
                    <asp:DropDownList ID="ProjectList" runat="server" AutoPostBack="True" DataSourceID="ProjectDataSource"
                        DataTextField="Name" DataValueField="Id" OnDataBound="ProjectList_DataBound">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="ProjectEdit" runat="server" Text="Edit..." OnClick="ProjectEdit_Click" 
                        CssClass="button" />
                </td>
                <td>
                    <asp:HyperLink ID="AddProjectLink" runat="server" NavigateUrl="~/Project.aspx?Id=0"
                        Target="_self">Add new project...</asp:HyperLink>
                </td>
                <td>
                    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/roles.aspx" runat="server">Manage roles...</asp:HyperLink>
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            DataSourceID="IssueDataSource" AllowPaging="True" AllowSorting="True" 
            CssClass="grid" BorderWidth="0px">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="Id, Project.Id" DataNavigateUrlFormatString="~/Issue.aspx?Id={0}&amp;ProjectId={1}"
                    Text="Edit" />
                <asp:CommandField ShowDeleteButton="True" />
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />
                <asp:BoundField DataField="Type.Type" HeaderText="Issue Type" ReadOnly="True" 
                    SortExpression="Type.Type" />
                <asp:BoundField DataField="Short" HeaderText="Short" SortExpression="Short" />
                <asp:BoundField DataField="CreateDate" DataFormatString="{0:dd/MMMM/yyyy}" 
                    HeaderText="CreateDate" ReadOnly="True" SortExpression="CreateDate" />
                <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
                <asp:BoundField DataField="Result" HeaderText="Result"
                    SortExpression="Result" />
                <asp:TemplateField HeaderText="Project" SortExpression="it.Project.Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("[Project.Id]") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Project.Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Owner" SortExpression="it.Owner.Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("[Owner.Id]") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Owner.Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Assigned To" SortExpression="it.AssignedTo.Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("[AssignedTo.Id]") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("AssignedTo.Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:EntityDataSource ID="IssueDataSource" runat="server" ConnectionString="name=DBContext"
            DefaultContainerName="DBContext" EnableDelete="True" EnableInsert="True" EnableUpdate="True"
            EntitySetName="Issue" Include="Type,Project, Owner, AssignedTo"
            Where="it.Project.Id=@ProjectId">
            <WhereParameters>
                <asp:ControlParameter ControlID="ProjectList" Name="ProjectId" DbType="Int32" />
            </WhereParameters>
        </asp:EntityDataSource>
    </div>
    <asp:EntityDataSource ID="ProjectDataSource" runat="server" ConnectionString="name=DBContext"
        DefaultContainerName="DBContext" EntitySetName="Project" Select="it.[Id], it.[Name]">
    </asp:EntityDataSource>
 
</asp:Content>
