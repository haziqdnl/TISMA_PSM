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
                <!--SEARCH TEXTBOX-->
                <asp:TextBox runat="server" TextMode="Search" placeholder="Search" CssClass="textbox-custom" Width="500px"></asp:TextBox>
                <!--SEARCH BTN-->
                <asp:Button runat="server" ID="BtnSearch1" Text="Search" CssClass="btn-custom" ForeColor="White" BackColor="#00bfbf" />
                <!--NEW REGISTRATION BTN-->
                <asp:Button runat="server" ID="BtnRegister" Text="Add New Staff" CssClass="btn-custom" ForeColor="White" BackColor="#ff0000" />
                <ajaxRegister:ModalPopupExtender runat="server" ID="ModalPopupRegister" PopupControlID="PanelRegister" TargetControlID="BtnRegister"
                    CancelControlID="BtnCancelReg" BackgroundCssClass="Background">
                </ajaxRegister:ModalPopupExtender>
                <br />

                <!--MODUL POPUP: NEW REGISTRATION-->
                <asp:Panel runat="server" ID="PanelRegister" CssClass="Popup" Style="display: none">
                    <div class="card">
                        <span>Searching for new staff from UTMHR</span>
                    </div>
                    <br />

                    <div class="card-body">
                        <!--SELECT/DROPDOWN INPUT: Filter By-->
                        <asp:DropDownList runat="server" ID="DropDownList2" CssClass="dropdown-custom">
                            <asp:ListItem Text="--Filter by--" Value="" />
                            <asp:ListItem Text="Matric No." Value="matric_no" />
                            <asp:ListItem Text="IC No." Value="ic_no" />
                            <asp:ListItem Text="Name" Value="name" />
                        </asp:DropDownList>
                        <!--SEARCH TEXTBOX-->
                        <asp:TextBox runat="server" TextMode="Search" placeholder="Search" CssClass="textbox-custom" Width="500px"></asp:TextBox>
                        <!--SEARCH BTN-->
                        <asp:Button runat="server" ID="BtnSearch2" Text="Search" CssClass="btn-custom" ForeColor="White" BackColor="#00bfbf" />
                        <!--CANCEL BTN-->
                        <asp:Button runat="server" ID="BtnCancelReg" Text="Cancel" CssClass="btn-custom" ForeColor="Black" BackColor="#e5e5e5" />
                        <br />

                        <!--SEARCHED PATIENT TABLE-->
                        <div class="mt-3">
                            <asp:GridView runat="server" ID="DisplayUTMHRData" CssClass="display compact cell-border" AutoGenerateColumns="False" DataKeyNames="ic_no" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="staff_id" HeaderText="Staff ID" SortExpression="staff_id" />
                                    <asp:BoundField DataField="ic_no" HeaderText="IC No." ReadOnly="True" SortExpression="ic_no" />
                                    <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" />
                                    <asp:BoundField DataField="designation" HeaderText="Designation" SortExpression="designation" />
                                    <asp:BoundField DataField="department" HeaderText="Department" SortExpression="department" />
                                    <asp:BoundField DataField="dob" HeaderText="DOB" SortExpression="dob" />
                                    <asp:BoundField DataField="branch" HeaderText="Branch" SortExpression="branch" />
                                    <asp:BoundField DataField="session_no" HeaderText="Session" SortExpression="session_no" />
                                    <asp:BoundField DataField="remarks" HeaderText="Remark" SortExpression="remarks" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                    </div>
                </asp:Panel>

                <!--SEARCH PATIENT TABLE-->
                <div class="mt-4">
                    <asp:GridView runat="server" ID="DisplayRegisteredData" CssClass="display compact cell-border" AutoGenerateColumns="False" DataKeyNames="s_ic_no" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="s_username" HeaderText="Username" SortExpression="s_username" />
                            <asp:BoundField DataField="s_staff_id" HeaderText="Staff ID" SortExpression="s_staff_id" />
                            <asp:BoundField DataField="s_name" HeaderText="Name" SortExpression="s_name" />
                            <asp:BoundField DataField="s_designation" HeaderText="Designation" SortExpression="s_designation" />
                            <asp:BoundField DataField="s_ic_no" HeaderText="IC No." ReadOnly="True" SortExpression="s_ic_no" />
                            <asp:BoundField DataField="s_dob" HeaderText="DOB" SortExpression="s_dob" />
                            <asp:BoundField DataField="s_tel_no" HeaderText="Tel No." SortExpression="s_tel_no" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $("[id*=DisplayRegisteredData]").DataTable({
                bLengthChange: true,
                lengthMenu: [[5, 10, -1], [5, 10, "All"]],
                bFilter: true,
                bSort: true,
                bPaginate: true,
            });
            $("[id*=DisplayUTMHRData]").DataTable({
                bLengthChange: true,
                lengthMenu: [[5, 10, -1], [5, 10, "All"]],
                bFilter: true,
                bSort: true,
                bPaginate: true,
            });
        });
    </script>
</asp:Content>
