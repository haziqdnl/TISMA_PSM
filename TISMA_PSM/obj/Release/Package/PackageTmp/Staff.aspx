<%@ Page Title="Staff :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Staff.aspx.cs" Inherits="TISMA_PSM.Staff" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxRegister" %>

<asp:Content ID="Staff" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid content-p-custom" id="staff">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row header-p-custom">
                <div class="col align-self-start">
                    <h5>Module Staff</h5>
                </div>
                <div class="col align-self-end">
                    <div class="float-end subheader-custom">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;>&nbsp;&nbsp;Modules&nbsp;&nbsp;></b>&nbsp;&nbsp;Staff</span>
                    </div>
                </div>
            </div>
        </div>

        <!--CONTENT-->
        <div class="row justify-content-center mt-1" id="staff-content">
            <!--HEADER-->
            <div class="card border-0">
                <div class="col">
                    <i class="fas fa-search me-1"></i>
                    <span>Search Staff</span>
                </div>
            </div>

            <!--SEARCH STAFF FORM-->
            <div class="card-body">
                <!--NEW REGISTRATION BTN-->
                <asp:Button runat="server" ID="BtnRegister" Text="Add New Staff" CssClass="btn-custom" ForeColor="White" BackColor="#0A9E00" />
                <ajaxRegister:ModalPopupExtender runat="server" ID="ModalPopupRegister" PopupControlID="PanelRegister" TargetControlID="BtnRegister"
                    CancelControlID="BtnCancelReg" BackgroundCssClass="Background">
                </ajaxRegister:ModalPopupExtender>
                <br />
                <br />
                <p style="font-size:11px">&nbsp <b>*</b> Click the <b>Account No.</b> to view the full details of the user/staff</p>

                <!--SEARCH STAFF TABLE-->
                <div class="mt-4">
                    <asp:GridView runat="server" ID="RegisteredStaffTable" CssClass="display compact cell-border" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" DataKeyNames="s_ic_no" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>No.</HeaderTemplate>
                                <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText = "Account No." ItemStyle-Width="30">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" Font-Bold="true" ForeColor="#0066ff" 
                                        Text='<%# Eval("s_account_no") %>'
                                        NavigateUrl='<%# base.ResolveUrl("~/Staff-Info.aspx?accno=" + TISMA_PSM.Helper.EncryptURL((String)Eval("s_account_no"))) %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="s_username" HeaderText="Username" SortExpression="s_username" />
                            <asp:BoundField DataField="s_name" HeaderText="Name" SortExpression="s_name" ItemStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="s_designation" HeaderText="Designation" SortExpression="s_designation" />
                            <asp:BoundField DataField="s_staff_id" HeaderText="Staff ID" SortExpression="s_staff_id" />
                            <asp:BoundField DataField="s_ic_no" HeaderText="IC No." ReadOnly="True" SortExpression="s_ic_no" />
                            <asp:BoundField DataField="s_tel_no" HeaderText="Tel No." SortExpression="s_tel_no" />
                        </Columns>
                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                    </asp:GridView>
                </div>

                <!--MODUL POPUP: NEW REGISTRATION-->
                <asp:Panel runat="server" ID="PanelRegister" CssClass="Popup" Style="display: none">
                    <div class="card">
                        <div class="row">
                            <div class="col align-self-center">
                                <span>Searching for new staff from UTMHR</span>
                            </div>
                            <div class="col align-self-end">
                                <div class="float-end">
                                    <!--CANCEL BTN-->
                                    <asp:Button runat="server" ID="BtnCancelReg" Text="Cancel" CssClass="btn-custom" ForeColor="White" BackColor="#ff0000" />
                                </div>
                            </div>
                        </div>                        
                    </div>
                    <div class="card-body">
                        <p style="font-size:11px">&nbsp <b>*</b> Click the <b>IC No.</b> to view the full details of the patient</p>
                        <!--SEARCHED PATIENT TABLE-->
                        <div class="mt-3">
                            <asp:GridView runat="server" ID="DisplayUTMHRData" CssClass="display compact cell-border" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>No.</HeaderTemplate>
                                        <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderText = "IC No." ItemStyle-Width="30">
                                        <ItemTemplate>
                                            <asp:HyperLink runat="server" Font-Bold="true" ForeColor="#0066ff" 
                                                Text='<%# Eval("ic_no") %>'
                                                NavigateUrl='<%# base.ResolveUrl("~/Add-New-Staff.aspx?pid=" + TISMA_PSM.Helper.EncryptURL((String)Eval("ic_no"))) %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="staff_id" HeaderText="Staff ID" SortExpression="staff_id" />
                                    <asp:BoundField DataField="designation" HeaderText="Designation" SortExpression="designation" />
                                    <asp:BoundField DataField="department" HeaderText="Department" SortExpression="department" />
                                    <asp:BoundField DataField="branch" HeaderText="Branch" SortExpression="branch" />
                                    <asp:BoundField DataField="session_no" HeaderText="Session" SortExpression="session_no" />
                                </Columns>
                                <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                        <br />
                    </div>
                </asp:Panel>                
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $("[id*=RegisteredStaffTable]").DataTable({
                lengthMenu: [[10, 20], [10, 20]],
                language: {
                    searchPlaceholder: "Search",
                    search: "",
                },
            });

            $("[id*=DisplayUTMHRData]").DataTable({
                lengthMenu: [[5, 10], [5, 10]],
                language: {
                    searchPlaceholder: "Search",
                    search: "",
                },
            });
        });
    </script>
</asp:Content>
