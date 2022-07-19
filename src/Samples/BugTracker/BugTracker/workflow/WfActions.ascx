<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WfActions.ascx.cs" Inherits="BugTracker.workflow.WfActions" %>
<link href="../site.css" rel="stylesheet" type="text/css" />
<asp:Repeater ID="Repeater1" runat="server" DataSourceID="WfActionsDataSource" 
    onitemcommand="Repeater1_ItemCommand">
<HeaderTemplate>
<div>Available actions:</div>
</HeaderTemplate>
<ItemTemplate>
<table>
<tr><td>
        <asp:Button ID="ActionButton" runat="server" Width="160 px" CssClass="button"
        CommandName="WfAction" 
        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id" ) %>'
        Text='<%# DataBinder.Eval(Container.DataItem, "Title" ) %>'
        />
</td></tr>
</table>
</ItemTemplate>
</asp:Repeater>

<asp:ObjectDataSource ID="WfActionsDataSource" runat="server" 
    SelectMethod="GetActions" TypeName="BugTracker.BL.IssueService">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="0" Name="issueId" QueryStringField="Id" 
            Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>


