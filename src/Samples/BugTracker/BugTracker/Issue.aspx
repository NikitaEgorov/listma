<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Issue.aspx.cs"
    Inherits="BugTracker.IssueForm" %>

<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register src="workflow/WfActions.ascx" tagname="WfActions" tagprefix="uc1" %>
<asp:Content ContentPlaceHolderID="Main" runat="server">
    <table>
    <tr>
    <td valign="top" style="border-right-width:2px; border-right-style: solid; border-right-color: #808080;">
    
        <uc1:WfActions ID="WfActions1" runat="server" OnDoCommand="WfActions1_DoCommand"  />
    
    </td>
    <td valign="top">
      <div>
        <asp:DetailsView ID="IssueDetailsView" CssClass="details" runat="server" AutoGenerateRows="False"
            DataKeyNames="Id" DataSourceID="IssueDataSource" Width="500px" OnModeChanging="DetailsView1_ModeChanging"
            OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting"
            BorderWidth="0px" onitemupdated="IssueDetailsView_ItemUpdated">
            <Fields>
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True"
                    SortExpression="Id" InsertVisible="False" />
                <asp:TemplateField HeaderText="Type" SortExpression="Type.Type">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList9" runat="server" 
                            DataSourceID="TypeDataSource" DataTextField="Type" DataValueField="Type" 
                            SelectedValue='<%# Eval("Type.Type") %>'
                            Enabled='<%# ((BugTracker.IssueForm)Page).StateList["Type"].Enabled %>'
                        Visible = '<%# ((BugTracker.IssueForm)Page).StateList["Type"].Visible %>' >
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="TypeDropDown" runat="server" DataSourceID="TypeDataSource"
                            DataTextField="Type" DataValueField="Type">
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("Type.Type") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Short" HeaderText="Short" SortExpression="Short" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" 
                    InsertVisible="False" ReadOnly="True" />
                <asp:BoundField DataField="Result" HeaderText="Result" InsertVisible="False" 
                    ReadOnly="True" SortExpression="Result" />
                <asp:TemplateField HeaderText="CreateDate" SortExpression="CreateDate">
                    <EditItemTemplate>
                        <asp:Calendar ID="Calendar2" runat="server" SelectedDate='<%# Bind("CreateDate") %>'
                        Enabled='<%# ((BugTracker.IssueForm)Page).StateList["CreateDate"].Enabled %>'
                        Visible = '<%# ((BugTracker.IssueForm)Page).StateList["CreateDate"].Visible %>' >
                        </asp:Calendar>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:Calendar ID="CreateDateInsert" runat="server" OnPreRender="CreateDateInsert_PreRender">
                        </asp:Calendar>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Calendar ID="Calendar1" runat="server" SelectedDate='<%# Eval("CreateDate") %>'>
                        </asp:Calendar>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField  HeaderText="Project" SortExpression="Project.Id">
                    <EditItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("Project.Name") %>'
                        Enabled = '<%# ((BugTracker.IssueForm)Page).StateList["Project"].Enabled %>'
                        Visible = '<%# ((BugTracker.IssueForm)Page).StateList["Project"].Visible %>' ></asp:Label>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="ProjectDropDown" runat="server" DataSourceID="ProjectDataSource"
                            DataTextField="Name" DataValueField="Id">
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Project.Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Owner" SortExpression="Owner.Id">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList5" runat="server" DataSourceID="UserDataSource"
                            DataTextField="Name" DataValueField="Id" 
                            SelectedValue='<%# Bind("Owner.Id") %>' 
                            Enabled='<%# ((BugTracker.IssueForm)Page).StateList["Owner"].Enabled %>'
                            Visible='<%# ((BugTracker.IssueForm)Page).StateList["Owner"].Visible %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="OwnerDropDown" runat="server" DataSourceID="UserDataSource"
                            DataTextField="Name" DataValueField="Id">
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("Owner.Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AssignedTo" SortExpression="AssignedTo.Id">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList8" runat="server" 
                            DataSourceID="UserDataSource" 
                            DataTextField="Name" DataValueField="Id" 
                            SelectedValue='<%# Bind("AssignedTo.Id") %>'
                            Enabled='<%# ((BugTracker.IssueForm)Page).StateList["AssignedTo"].Enabled %>'
                            Visible='<%# ((BugTracker.IssueForm)Page).StateList["AssignedTo"].Visible %>'
                            AppendDataBoundItems="True" >
                            <asp:ListItem Value="" Text="not assigned" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="AssignedDropDown" runat="server" DataSourceID="UserDataSource"
                            DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" >
                            <asp:ListItem Value="" Text="not assigned" Selected="True" />
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("AssignedTo.Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
        <asp:EntityDataSource ID="IssueDataSource" runat="server" ConnectionString="name=DBContext"
            DefaultContainerName="DBContext" EnableDelete="True" EnableInsert="True" EnableUpdate="True"
            EntitySetName="Issue" AutoGenerateWhereClause="false" Include="Type,Project,Owner,AssignedTo"
            Where="it.[ID] == @IssueId">
            
            <WhereParameters>
                <asp:QueryStringParameter Name="IssueId" QueryStringField="Id" DbType="Int32" />
            </WhereParameters>
        </asp:EntityDataSource>
        <asp:EntityDataSource ID="ProjectDataSource" runat="server" ConnectionString="name=DBContext"
            DefaultContainerName="DBContext" EntitySetName="Project" Select="it.[Id], it.[Name]"
            AutoGenerateWhereClause="false" Where="it.[ID]= @ProjectID">
            <WhereParameters>
                <asp:QueryStringParameter Name="ProjectId" QueryStringField="ProjectId" DbType="Int32" />
            </WhereParameters>
        </asp:EntityDataSource>
        <asp:EntityDataSource ID="TypeDataSource" runat="server" ConnectionString="name=DBContext"
            DefaultContainerName="DBContext" EntitySetName="IssueType" Select="it.[Type]">
        </asp:EntityDataSource>
        <asp:ObjectDataSource ID="UserDataSource" runat="server" SelectMethod="GetProjectTeam"
            TypeName="Bugtracker.BL.ProjectService">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="projectId" QueryStringField="ProjectId"
                    Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </td>
    </tr>
    </table>
  
</asp:Content>
