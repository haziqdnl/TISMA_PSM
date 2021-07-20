<%@ Page Title="Profile :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="TISMA_PSM.Profile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxProfile" %>

<asp:Content ID="Profile" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid content-p-custom" id="profile">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row header-p-custom">
                <div class="col">
                    <h5>Profile</h5>
                </div>
                <div class="col align-self-end">
                    <div class="float-end subheader-custom">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;>&nbsp;&nbsp;Options&nbsp;&nbsp;></b>&nbsp;&nbsp;Profile</span>
                    </div>
                </div>
            </div>
        </div>
        <!--CONTENT 1-->
        <div class="row justify-content-center mt-1" id="profile-content-1">
            <!--HEADER-->
            <div class="card border-0">
                <div class="row">
                    <div>
                        <i class="fas fa-user-edit me-1"></i>
                        <span>Update Your Information</span>
                    </div>
                </div>
            </div>
        </div>
        <!--CONTENT 2-->
        <div class="row mt-3" id="profile-content-2">
            <div class="card p-0 m-0">
                <div class="row align-self-center w-100 p-4">
                    <!--USER PROFILE DATA-->
                    <table id="profile-table" class="text-start p-4 w-100">
                        <tr class="text-center" style="vertical-align: top">
                            <!--Profile Picture, Role, Account No., Username-->
                            <td rowspan="20" style="width:20%">
                                <img src="https://iupac.org/wp-content/uploads/2018/05/default-avatar.png" style="border: 1px solid" /><br />
                                <br />
                                <p style="font-size: 12px; font-weight: 500; margin-bottom: 1px">Account No</p>
                                <h6><b><asp:Literal runat="server" ID="getAccNo"></asp:Literal></b></h6>
                                <p style="font-size: 12px; font-weight: 500; margin-bottom: 1px">Username</p>
                                <h6><b><asp:Literal runat="server" ID="getUsername"></asp:Literal></b></h6>
                                <p style="font-size: 12px; font-weight: 500; margin-bottom: 1px">TISMA Role</p>
                                <h6><b><asp:Literal runat="server" ID="getRole"></asp:Literal></b></h6>
                                <p style="font-size: 12px; font-weight: 500; margin-bottom: 1px">Session</p>
                                <h6><b><asp:Literal runat="server" ID="getSession"></asp:Literal></b></h6>
                            </td>
                            <!--Name-->
                            <td class="pe-3 text-end" style="width:10%">
                                Name
                            </td>
                            <td class="w-auto text-start" style="width:35%">
                                <asp:TextBox runat="server" ID="getName" ReadOnly="true" Enabled="false" CssClass="textbox-custom pt-1 w-75" TextMode="MultiLine" Height="80px"></asp:TextBox>
                            </td>
                            <!--Change Password-->
                            <td colspan="2" style="vertical-align:bottom; width:35%">
                                <p><b>Change Password</b></p>
                            </td>
                        </tr>
                        <tr>
                            <!--DOB-->
                            <td class="pe-3 text-end">
                                DOB
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getDOB" ReadOnly="true" Enabled="false" CssClass="textbox-custom w-75"></asp:TextBox>
                            </td>
                            <!--Currrent Password-->
                            <td class="pe-3 text-end">
                                Currrent Password
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getCurrentPassword" TextMode="Password" CssClass="textbox-custom w-75"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                            <td>
                                <span runat="server" id="PasswordIncorrectMsg" visible="false" style="color:red; font-size:10px">Oops! Incorrect password</span>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ValidationGroup="ChangePassword" ControlToValidate="getCurrentPassword"
                                    ErrorMessage="Please enter current password" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator5" ValidationGroup="ChangePassword" ControlToValidate="getCurrentPassword" 
                                    ValidationExpression="^([a-z]|[A-Z]|[0-9]|[!*@#$%^&+=_]){8,}$" 
                                    Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Password invalid" ForeColor="Red" Font-Size="10px">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <!--IC No-->
                            <td class="pe-3 text-end">
                                IC No
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getIcNo" ReadOnly="true" Enabled="false" CssClass="textbox-custom w-75"></asp:TextBox>
                            </td>
                            <!--New Password-->
                            <td class="pe-3 text-end">
                                New Password
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getNewPassword" TextMode="Password" CssClass="textbox-custom w-75"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                            <td>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="ChangePassword" ControlToValidate="getNewPassword"
                                    ErrorMessage="Please enter new password" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" ValidationGroup="ChangePassword" ControlToValidate="getNewPassword" 
                                    ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=_]).*$" 
                                    Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Invalid password" ForeColor="Red" Font-Size="10px">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <!--Passport No-->
                            <td class="pe-3 text-end">
                                Passport No
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getPassportNo" CssClass="textbox-custom w-75"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="getPassportNo" 
                                    ValidationExpression="^[A-Za-z0-9]{10,12}$" Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Invalid" ForeColor="Red" Font-Size="10px">
                                </asp:RegularExpressionValidator>
                            </td>
                            <!--Re-enter New Password-->
                            <td class="pe-3 text-end">
                                Re-enter New Password
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getConfirmNewPassword" TextMode="Password" CssClass="textbox-custom w-75"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                            <td>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ValidationGroup="ChangePassword" ControlToValidate="getConfirmNewPassword"
                                    ErrorMessage="Please re-enter new password" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3" ValidationGroup="ChangePassword" ControlToValidate="getConfirmNewPassword" 
                                    ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=_]).*$" 
                                    Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Invalid password" ForeColor="Red" Font-Size="10px">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <!--Staff ID-->
                            <td class="pe-3 text-end">
                                Staff ID
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getStaffId" ReadOnly="true" Enabled="false" CssClass="textbox-custom w-75"></asp:TextBox>
                            </td>
                            <!--Change Password Tips-->
                            <td class="pe-3 text-center" rowspan="3" colspan="2" style="vertical-align:top">
                                <p runat="server" id="PasswordNotMatchMsg" visible="false" style="color:red; font-size:11px;">Oops! New passwords not match</p>
                                <div class="text-end" style="font-size:12px">
                                      <input type="checkbox" onclick="showPassword()">&nbsp Show/Hide Password
                                </div>
                                <p style="color:grey; font-size:12px"">Password must contain at least 8 characters long, 1 upper case letter, 1 special character and 1 number</p>
                                <asp:Button runat="server" ID="BtnReset" Text="Change Password" OnClick="ChangePassword" ValidationGroup="ChangePassword" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#ff0000" />
                            </td>
                        </tr>
                        <tr>
                            <!--Email-->
                            <td class="pe-3 text-end">
                                Email
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getEmail" ReadOnly="true" Enabled="false" CssClass="textbox-custom w-75"></asp:TextBox>
                            </td>
                            <!--Null space-->
                            <td class="pe-3 text-center" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <!--Phone No.-->
                            <td class="pe-3 text-end">
                                Phone No.
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getPhone" CssClass="textbox-custom w-75"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator4" ControlToValidate="getPhone" 
                                    ValidationExpression="[0-9]{10,15}" 
                                    Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Invalid" ForeColor="Red" Font-Size="10px">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" ControlToValidate="getPhone"
                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                </asp:RequiredFieldValidator>
                            </td>
                            <!--Null space-->
                            <td class="pe-3 text-center" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <!--Address-->
                            <td class="pe-3 text-end">
                                Address
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getAddress" CssClass="textbox-custom pt-1 w-75" TextMode="MultiLine" Height="120px"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator6" ControlToValidate="getAddress" 
                                    ValidationExpression="([A-Z]|[a-z]|[0-9]|[^<>;=]){0,}" 
                                    Display="Dynamic" SetFocusOnError="true"
                                    ErrorMessage="Invalid" ForeColor="Red" Font-Size="10px">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="getAddress"
                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                </asp:RequiredFieldValidator>
                            </td>
                            <!--Null space-->
                            <td class="pe-3 text-center" colspan="2">
                        </tr>
                        <tr>
                            <!--Faculty/Department-->
                            <td class="pe-3 text-end">
                                Faculty/Department
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getFacDep" ReadOnly="true" Enabled="false" CssClass="textbox-custom pt-1 w-75" TextMode="MultiLine" Height="80px"></asp:TextBox>
                            </td>
                            <!--Designation	-->
                            <td class="pe-3 text-end">
                                Designation
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getDesignation" ReadOnly="true" Enabled="false" CssClass="textbox-custom pt-1 w-75" TextMode="MultiLine" Height="80px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <!--Gender-->
                            <td class="pe-3 text-end">
                                Gender
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getGender" ReadOnly="true" Enabled="false" CssClass="textbox-custom w-75"></asp:TextBox>
                            </td>
                            <!--Null space-->
                            <td class="pe-3 text-center" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <!--Race-->
                            <td class="pe-3 text-end">
                                Race
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getRace" ReadOnly="true" Enabled="false" CssClass="textbox-custom w-75"></asp:TextBox>
                            </td>
                            <!--Nationality-->
                            <td class="pe-3 text-end">
                                Nationality
                            </td>
                            <td class="w-auto text-start">
                                <asp:TextBox runat="server" ID="getNationality" ReadOnly="true" Enabled="false" CssClass="textbox-custom w-75"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <!--Religion-->
                            <td class="pe-3 text-end">
                                Religion
                            </td>
                            <td class="w-auto text-start">
                                <asp:DropDownList runat="server" ID="getReligion" CssClass="dropdown-custom w-75" AutoPostBack="true" BackColor="White">
                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                    <asp:ListItem Text="Muslim" Value="Muslim"></asp:ListItem>
                                    <asp:ListItem Text="Buddha" Value="Buddha"></asp:ListItem>
                                    <asp:ListItem Text="Christian" Value="Christian"></asp:ListItem>
                                    <asp:ListItem Text="Hindu" Value="Hindu"></asp:ListItem>
                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <!--Marital Status-->
                            <td class="pe-3 text-end">
                                Marital Status
                            </td>
                            <td class="w-auto text-start">
                                <asp:DropDownList runat="server" ID="getMaritalStat" CssClass="dropdown-custom w-75" AutoPostBack="true" BackColor="White">
                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                    <asp:ListItem Text="Single" Value="Single"></asp:ListItem>
                                    <asp:ListItem Text="Married" Value="Married"></asp:ListItem>
                                    <asp:ListItem Text="Divorced " Value="Divorced"></asp:ListItem>
                                    <asp:ListItem Text="Widowed " Value="Widowed"></asp:ListItem>
                                    <asp:ListItem Text="Separated " Value="Separated"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="getMaritalStat" 
                                    InitialValue="Select" 
                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td></td>
                            <td>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="getReligion" 
                                    InitialValue="Select" 
                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <div class="text-center">
                        <asp:Button runat="server" ID="BtnSave" Text="Save" OnClick="UpdateToTisma" CssClass="btn-custom mt-2" ForeColor="White" Width="150px" BackColor="#00cc00" />
                    </div>
                </div>
            </div>
        </div>
        
        <!--MODUL POPUP: VALIDATION POPUP MESSAGE-->
        <div style="display:none">
            <asp:Button runat="server" ID="BtnPopupMessage" Text="Open Popup" CssClass="btn-custom mt-2" Width="100%" ForeColor="White" BackColor="#0066ff" />
        </div>
        <ajaxProfile:ModalPopupExtender runat="server" ID="ModalPopupMessage" PopupControlID="PanelPopupMessage" TargetControlID="BtnPopupMessage" CancelControlID="BtnOK" BackgroundCssClass="Background">
        </ajaxProfile:ModalPopupExtender>
        <asp:Panel runat="server" ID="PanelPopupMessage" CssClass="Popup" Width="600px" Style="display: none">
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
                    <p><asp:Literal runat="server" ID="getMsgHeader"></asp:Literal></p>
                    <p><asp:Literal runat="server" ID="getMsgBody"></asp:Literal></p>
                </div>
                <div>
                    <!--CANCEL BTN-->
                    <asp:Button runat="server" ID="BtnOK" Text="Ok" CssClass="btn-custom" ForeColor="White" BackColor="#0A9E00" />
                </div>
            </div>
        </asp:Panel>
    </div>
    <script lang="javascript" type="text/javascript">
        function showPassword() {
            var x = document.getElementById('<%= getCurrentPassword.ClientID %>');
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
            var x = document.getElementById('<%= getNewPassword.ClientID %>');
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
            var x = document.getElementById('<%= getConfirmNewPassword.ClientID %>');
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        }
    </script>
</asp:Content>
