<%@ Page Title="Example" Language="C#" MasterPageFile="~/X.Master" AutoEventWireup="true" CodeBehind="X-Modal-Test.aspx.cs" Inherits="TISMA_PSM.X_Modal_Test" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:ScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Button ID="Button1" runat="server" Text="fill form" />
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" CancelControlID="Button2" TargetControlID="Button1" PopupControlID="Panel1">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" CssClass="Popup" align="center" Style="display: none" Width="700px" BackColor="Black">
            <iframe style="width: 350px; height: 300px;" id="irm1" src="Dashboard.aspx" runat="server"></iframe>
            <br />
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
                <!--SELECT/DROPDOWN INPUT-->
                <asp:DropDownList runat="server" ID="DropdownPatientType" Width="180px" Height="30px">
                    <asp:ListItem Text="--Select patient type--" Value="" />
                    <asp:ListItem Text="UTM-ACAD (Student)" Value="utm_acad" />
                    <asp:ListItem Text="UTM-HR (Staff)" Value="utm_acad" />
                    <asp:ListItem Text="Public" Value="utm_acad" />
                </asp:DropDownList>
                <!--SEARCH-->
                <asp:TextBox runat="server" Width="400px" Height="30px" TextMode="Search" placeholder="Search"></asp:TextBox>
                <!--SUBMIT BTN-->
                <asp:Button runat="server" ID="BtnSearch" Text="Enter" ForeColor="White" BackColor="#00bfbf" BorderStyle="None" Height="32px" Width="70px" />
                <!--REGISTER BTN-->
                <asp:Button runat="server" ID="Button3" Text="New Registration" ForeColor="White" BackColor="#ff0000" BorderStyle="None" Height="32px" Width="150px" />
                <!---->
                <div>
                </div>
                <div class="mt-3" id="register-form">
                    <table id="registration-table" class="cell-border hover order-column mt-2" style="width: 100%;">
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
            <br />
            <asp:Button ID="Button2" runat="server" Text="Close" />
        </asp:Panel>
    </div>
    <script>
            $(document).ready(function () {
                $(".iframe").colorbox({ iframe: true, fastIframe: false, width: "650px", height: "480px", transition: "fade", scrolling: false });
            });
    </script>
</asp:Content>
