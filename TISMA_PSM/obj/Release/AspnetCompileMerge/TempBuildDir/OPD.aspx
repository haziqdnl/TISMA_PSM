﻿<%@ Page Title="OPD :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="OPD.aspx.cs" Inherits="TISMA_PSM.OPD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxOpd" %>

<asp:Content ID="OPD" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid content-p-custom" id="opd">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row header-p-custom">
                <div class="col align-self-start">
                    <span style="font-size: 19px;font-weight: 600;">Module OPD</span>&nbsp<span>Out Patient Department</span>
                </div>
                <div class="col align-self-end">
                    <div class="float-end subheader-custom">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;>&nbsp;&nbsp;Modules&nbsp;&nbsp;></b>&nbsp;&nbsp;OPD</span>
                    </div>
                </div>
            </div>
        </div>

        <!--CONTENT-->
        <div class="row justify-content-center mt-1" id="opd-content">
            <!--HEADER-->
            <div class="card border-0">
                <div class="col">
                    <i class="fas fa-search me-1"></i>
                    <span>Search</span>
                </div>
            </div>
            <div class="card-body">
                <p style="font-size:11px">&nbsp <b>*</b> Click the <b>Account No.</b> to <b>CHECK-IN</b> the patient</p>
                <!--TABLE-->
                <div class="mt-3">
                    <asp:GridView runat="server" ID="OpdTable" CssClass="display compact cell-border" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>No.</HeaderTemplate>
                                <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account No." ItemStyle-Width="30">
                                <ItemTemplate> 
                                    <asp:Button runat="server" Font-Bold="true" ForeColor="#0066ff" BorderStyle="None" BackColor="White"
                                        Text='<%# Eval("p_account_no") %>'
                                        OnClick="ShowModalPopupConfirmation"
                                        CommandName="ThisBtnClick" 
                                        CommandArgument='<%# Eval("p_account_no") + ";" + Eval("queue_no") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="queue_no" HeaderText="Queue No." SortExpression="queue_no" />
                            <asp:BoundField DataField="p_name" HeaderText="Name" SortExpression="p_name" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="p_category" HeaderText="Category" SortExpression="p_category" />
                            <asp:BoundField DataField="p_remarks" HeaderText="Remarks" SortExpression="p_remarks" />
                        </Columns>
                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!--MODUL POPUP: UPDATE POPUP MESSAGE-->
        <div style="display:none">
            <asp:Button runat="server" ID="BtnPopupConfirmation" Text="Open Popup" CssClass="btn-custom mt-2" Width="100%" ForeColor="White" BackColor="#0066ff" />
        </div>
        <ajaxOpd:ModalPopupExtender runat="server" ID="ModalPopupConfirmation" PopupControlID="PanelPopupConfirmation" TargetControlID="BtnPopupConfirmation" CancelControlID="BtnCancel" BackgroundCssClass="Background">
        </ajaxOpd:ModalPopupExtender>
        <asp:Panel runat="server" ID="PanelPopupConfirmation" CssClass="Popup" Width="600px" Style="display: none">
            <div class="card">
                <div class="row">
                    <div class="col align-self-center">
                        <span>System Message</span>
                    </div>
                    <div class="col align-self-end">
                    </div>
                </div>
            </div>
            <div class="card-body text-center">
                <div class="row">
                    <p>Account No: <b><asp:Literal runat="server" ID="getAccNo"></asp:Literal></b>, Queue No: <b><asp:Literal runat="server" ID="getQueueNo"></asp:Literal></b></p>
                    <p>Are you sure want to <b style="color:#0A9E00">CHECK-IN</b> this patient?</p>
                    <p>This action cannot be undone!</p>
                </div>
                <div class="mt-3">
                    <!--Redirect BTN-->
                    <asp:Button runat="server" ID="BtnRedirect" Text="Check-In Patient" OnClick="CheckInPatient" CssClass="btn-custom" ForeColor="White" BackColor="#0A9E00" />
                    <!--Cancel BTN-->
                    <asp:Button runat="server" ID="BtnCancel" Text="Cancel" CssClass="btn-custom" ForeColor="White" BackColor="#ff0000" />
                </div>
            </div>
        </asp:Panel>
    </div>
    <script>
        $(function () {
            $("[id*=OpdTable]").DataTable({
                lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                language: {
                    searchPlaceholder: "Search queue no...",
                    search: "",
                },
            });
        });
    </script>
</asp:Content>
