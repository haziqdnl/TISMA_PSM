<%@ Page Title="Registration :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="TISMA_PSM.Registration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxRegister" %>

<asp:Content ID="Registration" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid content-p-custom" id="registration">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row header-p-custom">
                <div class="col align-self-start">
                    <h5>Module Registration</h5>
                </div>
                <div class="col align-self-end">
                    <div class="float-end subheader-custom">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;>&nbsp;&nbsp;Modules&nbsp;&nbsp;></b>&nbsp;&nbsp;Registration</span>
                    </div>
                </div>
            </div>
        </div>

        <!--CONTENT-->
        <div class="row justify-content-center mt-1" id="registration-content">
            <!--HEADER-->
            <div class="card border-0">
                <div class="col">
                    <i class="fas fa-search me-1"></i>
                    <span>Search Registered Patient</span>
                </div>
            </div>

            <!--SEARCH PATIENT FORM-->
            <div class="card-body">
                <!--SELECT/DROPDOWN INPUT: Patient Type-->
                Filter patient category:
                <asp:DropDownList ID="DdlPatientType" runat="server" CssClass="dropdown-custom" OnSelectedIndexChanged="CategoryChanged"
                    AutoPostBack="true" AppendDataBoundItems="true">
                    <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="UTM-ACAD (Student)" Value="student"></asp:ListItem>
                    <asp:ListItem Text="UTM-HR (Staff)" Value="staff"></asp:ListItem>
                    <asp:ListItem Text="Public" Value="public"></asp:ListItem>
                </asp:DropDownList>

                <!--NEW REGISTRATION BTN-->
                <asp:Button runat="server" ID="BtnRegister" Text="Register New Patient" CssClass="btn-custom" ForeColor="White" BackColor="#0A9E00" />
                <ajaxRegister:ModalPopupExtender runat="server" ID="ModalPopupRegister" PopupControlID="PanelRegister" TargetControlID="BtnRegister"
                    CancelControlID="BtnCancelReg" BackgroundCssClass="Background">
                </ajaxRegister:ModalPopupExtender>
                <br />
                <br />
                <p style="font-size:11px">&nbsp <b>*</b> Click the <b>Account No.</b> to view the full details of the patient</p>

                <!--SEARCH PATIENT TABLE-->
                <div class="mt-4">
                    <asp:GridView runat="server" ID="DisplayRegisteredData" CssClass="display compact cell-border" AutoGenerateColumns="False" AllowPaging="true"
                        OnPageIndexChanging="OnPaging" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>No.</HeaderTemplate>
                                <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="p_account_no" HeaderText="Account No." SortExpression="p_account_no" />--%>
                            <asp:HyperLinkField DataTextField="p_account_no" HeaderText="Account No." SortExpression="p_account_no"
                                DataNavigateUrlFields="p_account_no" DataNavigateUrlFormatString="Patient-Info.aspx?accno={0}" 
                                ItemStyle-Font-Bold="true" ItemStyle-Width="60px" ItemStyle-ForeColor="#0066ff" />
                            <asp:BoundField DataField="p_name" HeaderText="Name" SortExpression="p_name" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="p_ic_no" HeaderText="IC No." ReadOnly="True" SortExpression="p_ic_no" />
                            <asp:BoundField DataField="p_passport_no" HeaderText="Passport No." SortExpression="p_passport_no" />
                            <asp:BoundField DataField="p_category" HeaderText="Category" SortExpression="p_category" />
                        </Columns>
                    </asp:GridView>
                </div>

                <!--MODUL POPUP: NEW REGISTRATION-->
                <asp:Panel runat="server" ID="PanelRegister" CssClass="Popup" Style="display: none">
                    <div class="card">
                        <span>Searching for new registration</span>
                    </div>
                    <br />
                    <div class="card-body">
                        <!--SELECT/DROPDOWN INPUT: Patient Type-->
                        <asp:DropDownList runat="server" ID="DdlUTMType" CssClass="dropdown-custom" onchange="ShowHideDiv()">
                            <asp:ListItem Text="-Select patient type-" Value="0" />
                            <asp:ListItem Text="UTM-ACAD (Student)" Value="1" />
                            <asp:ListItem Text="UTM-HR (Staff)" Value="2" />
                        </asp:DropDownList>

                        <!--PUBLIC PATIENT REGISTER BTN-->
                        <asp:Button runat="server" ID="BtnPublicRegister" Text="Register New as Public" PostBackUrl="Registration-New-Public.aspx"
                            CssClass="btn-custom" ForeColor="White" BackColor="#0000FF" />

                        <!--CANCEL BTN-->
                        <asp:Button runat="server" ID="BtnCancelReg" Text="Cancel Register" CssClass="btn-custom" ForeColor="White" BackColor="#ff0000" />
                        <br />
                        <br />
                        <p style="font-size:11px">&nbsp <b>*</b> Click the <b>IC No.</b> to view the full details of the patient</p>

                        <!--DUMMY TABLE-->
                        <div id="ShowHideDummyTable" class="mt-3"">
                            <table id="DummyTable" class="cell-border hover order-column mt-2" style="width: 100%;">
                                <thead>
                                    <tr>
                                        <th>No.</th>
                                        <th>Name</th>
                                        <th>IC No.</th>
                                        <th>Matric No.</th>
                                        <th>Designation</th>
                                        <th>Faculty</th>
                                        <th>Branch</th>
                                        <th>Session</th>
                                    </tr>
                                </thead>
                            </table>
                            <br />
                        </div>

                        <!--UTMACAD TABLE-->
                        <div id="ShowHideUTMACAD" class="mt-3" style="display: none;">
                            <asp:GridView runat="server" ID="DisplayUTMACADData" CssClass="display compact cell-border" AutoGenerateColumns="False" DataKeyNames="ic_no" Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>No.</HeaderTemplate>
                                        <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" ItemStyle-HorizontalAlign="Left" />
                                    <%--<asp:BoundField DataField="ic_no" HeaderText="IC No." ReadOnly="True" SortExpression="ic_no" />--%>
                                    <asp:HyperLinkField DataTextField="ic_no" HeaderText="IC No." SortExpression="ic_no"
                                        DataNavigateUrlFields="ic_no" DataNavigateUrlFormatString="Registration-New.aspx?pid={0}&stat=utmacad" 
                                        ItemStyle-Font-Bold="true" ItemStyle-Width="60px" ItemStyle-ForeColor="#0066ff" />
                                    <asp:BoundField DataField="matric_no" HeaderText="Matric No." SortExpression="matric_no" />
                                    <asp:BoundField DataField="faculty" HeaderText="Faculty" SortExpression="faculty" />
                                    <asp:BoundField DataField="branch" HeaderText="Branch" SortExpression="branch" />
                                    <asp:BoundField DataField="session_no" HeaderText="Session" SortExpression="session_no" />
                                </Columns>
                            </asp:GridView>
                            <br />
                        </div>

                        <!--UTMHR TABLE-->
                        <div id="ShowHideUTMHR" class="mt-3" style="display: none;">
                            <asp:GridView runat="server" ID="DisplayUTMHRData" CssClass="display compact cell-border" AutoGenerateColumns="False" DataKeyNames="ic_no" Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>No.</HeaderTemplate>
                                        <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" ItemStyle-HorizontalAlign="Left" />
                                    <%--<asp:BoundField DataField="ic_no" HeaderText="IC No." ReadOnly="True" SortExpression="ic_no" />--%>
                                    <asp:HyperLinkField DataTextField="ic_no" HeaderText="IC No." SortExpression="ic_no"
                                        DataNavigateUrlFields="ic_no" DataNavigateUrlFormatString="Registration-New.aspx?pid={0}&stat=utmhr"
                                        ItemStyle-Font-Bold="true" ItemStyle-Width="60px" ItemStyle-ForeColor="#0066ff" />
                                    <asp:BoundField DataField="staff_id" HeaderText="Staff ID" SortExpression="staff_id" />
                                    <asp:BoundField DataField="designation" HeaderText="Designation" SortExpression="designation" />
                                    <asp:BoundField DataField="department" HeaderText="Department" SortExpression="department" />
                                    <asp:BoundField DataField="branch" HeaderText="Branch" SortExpression="branch" />
                                    <asp:BoundField DataField="session_no" HeaderText="Session" SortExpression="session_no" />
                                </Columns>
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
            $("[id*=DisplayRegisteredData]").DataTable({
                lengthMenu: [[5, 10, -1], [5, 10, "All"]],
                language: {
                    searchPlaceholder: "Search",
                    search: "",
                },
            });

            $("#DummyTable").DataTable({
                lengthMenu: [[5, 10], [5, 10]],
                language: {
                    searchPlaceholder: "Search",
                    search: "",
                },
            });

            $("[id*=DisplayUTMACADData]").DataTable({
                lengthMenu: [[5, 10], [5, 10]],
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

        function ShowHideDiv() {
            //Get dropdown selected value
            var SelectedValue = $('#<%= DdlUTMType.ClientID %> option:selected').val();

            // check selected value.
            if (SelectedValue == 0) {
                $('#ShowHideUTMHR').css("display", "none");
                $('#ShowHideUTMACAD').css("display", "none");
                $('#ShowHideDummyTable').css("display", "block");
            }
            else if (SelectedValue == 1) {
                $('#ShowHideUTMHR').css("display", "none");
                $('#ShowHideUTMACAD').css("display", "block");
                $('#ShowHideDummyTable').css("display", "none");

            }
            else {
                $('#ShowHideUTMHR').css("display", "block");
                $('#ShowHideUTMACAD').css("display", "none");
                $('#ShowHideDummyTable').css("display", "none");
            }
        }
    </script>
</asp:Content>
