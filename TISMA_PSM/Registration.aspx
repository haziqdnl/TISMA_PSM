<%@ Page Title="Registration :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="TISMA_PSM.Registration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxRegister" %>

<asp:Content ID="Registration" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" style="padding: 1.5rem 1rem 1.5rem 1rem" id="registration">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row" style="padding-left: 0; padding-right: 0;">
                <div class="col align-self-start">
                    <h5>Module Registration</h5>
                </div>
                <div class="col align-self-end">
                    <div class="float-end " style="font-size: 12px; padding-bottom: 0.5rem;">
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
                <asp:DropDownList runat="server" ID="DropdownPatientType" Width="200px" Height="30px" ForeColor="Black" BackColor="LightGray" Style="padding: 0 10px 0 10px; border: 1px solid lightgrey;">
                    <asp:ListItem Text="--Select patient type--" Value="" />
                    <asp:ListItem Text="UTM-ACAD (Student)" Value="utm_acad" />
                    <asp:ListItem Text="UTM-HR (Staff)" Value="utm_acad" />
                    <asp:ListItem Text="Public" Value="utm_acad" />
                </asp:DropDownList>
                <!--SEARCH TEXTBOX-->
                <asp:TextBox runat="server" Width="500px" Height="30px" TextMode="Search" placeholder="Search" Style="padding: 0 10px 0 10px; border: 1px solid lightgrey;"></asp:TextBox>
                <!--SEARCH BTN-->
                <asp:Button runat="server" ID="BtnSearch1" Text="Search" ForeColor="White" BackColor="#00bfbf" BorderStyle="None" Height="32px" Width="70px" />
                <!--NEW REGISTRATION BTN-->
                <asp:ScriptManager runat="server" ID="ToolkitScriptManager1">
                </asp:ScriptManager>
                <asp:Button runat="server" ID="BtnRegister" Text="New Registration" ForeColor="White" BackColor="#ff0000" BorderStyle="None" Height="32px" Width="150px" />
                <ajaxRegister:ModalPopupExtender runat="server" ID="ModalPopupRegister" PopupControlID="PanelRegister" TargetControlID="BtnRegister" CancelControlID="BtnCancelReg" BackgroundCssClass="Background">
                </ajaxRegister:ModalPopupExtender>
                <br />

                <!--MODUL POPUP: NEW REGISTRATION-->
                <asp:Panel runat="server" ID="PanelRegister" CssClass="Popup" Style="display: none">
                    <div class="card">
                        <span>Searching for new registration</span>
                    </div>
                    <br />
                    <div class="card-body">
                        <!--SELECT/DROPDOWN INPUT: Patient Type-->
                        <asp:DropDownList runat="server" ID="DropDownList1" Width="200px" Height="30px" ForeColor="Black" BackColor="LightGray" Style="padding: 0 10px 0 10px; border: 1px solid lightgrey;">
                            <asp:ListItem Text="--Select patient type--" Value="" />
                            <asp:ListItem Text="UTM-ACAD (Student)" Value="utm_acad" />
                            <asp:ListItem Text="UTM-HR (Staff)" Value="utm_acad" />
                            <asp:ListItem Text="Public" Value="utm_acad" />
                        </asp:DropDownList>
                        <!--SELECT/DROPDOWN INPUT: Filter By-->
                        <asp:DropDownList runat="server" ID="DropDownList2" Width="125px" Height="30px" ForeColor="Black" BackColor="LightGray" Style="padding: 0 10px 0 10px; border: 1px solid lightgrey;">
                            <asp:ListItem Text="--Filter by--" Value="" />
                            <asp:ListItem Text="Matric No." Value="matric_no" />
                            <asp:ListItem Text="IC No." Value="ic_no" />
                            <asp:ListItem Text="Name" Value="name" />
                        </asp:DropDownList>
                        <!--SEARCH TEXTBOX-->
                        <asp:TextBox runat="server" Width="500px" Height="30px" TextMode="Search" placeholder="Search" Style="padding: 0 10px 0 10px; border: 1px solid lightgrey;"></asp:TextBox>
                        <!--SEARCH BTN-->
                        <asp:Button runat="server" ID="BtnSearch2" Text="Search" ForeColor="White" BackColor="#00bfbf" BorderStyle="None" Height="32px" Width="70px" />
                        <!--CANCEL BTN-->
                        <asp:Button runat="server" ID="BtnCancelReg" Text="Cancel Register" ForeColor="Black" BackColor="#e5e5e5" BorderStyle="None" Height="32px" Width="130px" />
                        <br />
                        <!--SEARCHED PATIENT TABLE-->
                        <div class="mt-3" id="register-form">
                            <table id="registration-table" class="cell-border hover order-column mt-2" style="width: 100%;">
                                <thead>
                                    <tr>
                                        <th>No.</th>
                                        <th>Matric No.</th>
                                        <th>IC No.</th>
                                        <th>Name</th>
                                        <th>Desgination</th>
                                        <th>Faculty</th>
                                        <th>DOB</th>
                                        <th>Branch</th>
                                        <th>Status</th>
                                        <th>Session</th>
                                        <th>Remark</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <br />
                        <!--PUBLIC PATIENT REGISTER BTN-->
                        <asp:Button runat="server" ID="BtnPublicRegister" Text="Register New as Public" ForeColor="White" BackColor="#ff0000" BorderStyle="None" Height="32px" Width="180px" />
                    </div>
                </asp:Panel>

                <!--SEARCH PATIENT TABLE-->
                <div class="mt-3" id="display-form">
                    <table id="display-table" class="cell-border hover order-column mt-2" style="width: 100%;">
                        <thead>
                            <tr>
                                <th>No.</th>
                                <th>Account No.</th>
                                <th>Matric No.</th>
                                <th>Name</th>
                                <th>Passport No.</th>
                                <th>IC No.</th>
                                <th>DOB</th>
                                <th>Category</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('#registration-table').DataTable();
            $('#display-table').DataTable();
        });
    </script>
</asp:Content>
