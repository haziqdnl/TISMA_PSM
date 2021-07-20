<%@ Page Title="Queue List :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Queue.aspx.cs" Inherits="TISMA_PSM.Queue" %>

<asp:Content ID="Queue" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" style="padding: 1.5rem 1rem 1.5rem 1rem" id="q-list">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row" style="padding-left: 0; padding-right: 0;">
                <div class="col align-self-start">
                    <h5>Queue List</h5>
                </div>
                <div class="col align-self-end">
                    <div class="float-end " style="font-size: 12px; padding-bottom: 0.5rem;">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;></b>&nbsp;&nbsp;Queue List</span>
                    </div>
                </div>
            </div>
        </div>
        <!--CONTENT-->
        <div class="row justify-content-center mt-1" id="q-list-content">
            <!--HEADER-->
            <div class="card border-0">
                <div class="col">
                    <span>Today Registered List</span>
                </div>
            </div>
            <div class="card-body">
                <!--TABLE-->
                <div class="mt-3">
                    <asp:GridView runat="server" ID="QueueListTable" CssClass="display compact cell-border" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>No.</HeaderTemplate>
                                <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="queue_no" HeaderText="Queue No." SortExpression="queue_no" />
                            <asp:BoundField DataField="p_account_no" HeaderText="Account No." SortExpression="p_account_no" />
                            <asp:BoundField DataField="p_name" HeaderText="Name" SortExpression="p_name" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="p_category" HeaderText="Category" SortExpression="p_category" />
                            <asp:BoundField DataField="p_remarks" HeaderText="Remarks" SortExpression="p_remarks" />
                        </Columns>
                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(function () {
            $("[id*=QueueListTable]").DataTable({
                lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                language: {
                    searchPlaceholder: "Search queue no...",
                    search: "",
                },
            });
        });
    </script>
</asp:Content>
