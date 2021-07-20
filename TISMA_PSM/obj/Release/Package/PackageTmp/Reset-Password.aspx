<%@ Page Title="Reset Password :: TISMA" Language="C#" MasterPageFile="~/MasterWeb.Master" AutoEventWireup="true" CodeBehind="Reset-Password.aspx.cs" Inherits="TISMA_PSM.Reset_Password" %>

<asp:Content ID="ResetPassword" ContentPlaceHolderID="WebContent" runat="server">
    <div class="container justify-content-center align-items-center center-custom">
        <div id="Title" class="card text-center">
            <p>TISMA<span>PKU</span></p>
        </div>
        <div id="ResetPasswordCard" class="card w-100">
            <div runat="server" id="ResetPasswordMain" class="card-body text-center">
                <p style="font-weight: 600; color: grey"><span class="fas fa-key me-2"></span>Reset Password</p>
                <br />
                <table id="ResetPasswordTable" class="mb-4">
                    <tr>
                        <td style="width: 120px">
                            <p>New Password</p>
                        </td>
                        <td>
                            <div class="input-group h-100">  
                                <asp:TextBox runat="server" ID="getNewPassword" TextMode="Password" MaxLength="70" CssClass="textbox-custom" Width="85%"></asp:TextBox>  
                                <div class="input-group-append">
                                    <span class="input-group-text">
                                        <input type="checkbox" onclick="showPassword1()" style="height:16.5px"> 
                                    </span>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="getNewPassword"
                                ErrorMessage="Please enter new password" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" ControlToValidate="getNewPassword" 
                                ValidationExpression="^.*(?=.{8,70})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=_]).*$" 
                                Display="Dynamic" SetFocusOnError="true"
                                ErrorMessage="Invalid password" ForeColor="Red" Font-Size="10px">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="m-2 p-2"></td>
                    </tr>
                    <tr>
                        <td style="width: 120px">
                            <p>Re-enter New Password</p>
                        </td>
                        <td>
                            <div class="input-group">  
                                <asp:TextBox runat="server" ID="getConfirmNewPassword" TextMode="Password" CssClass="textbox-custom" Width="85%"></asp:TextBox>  
                                <div class="input-group-append">
                                    <span class="input-group-text">  
                                        <input type="checkbox" onclick="showPassword2()" style="height:16.5px"> 
                                    </span>
                                </div>  
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="getConfirmNewPassword"
                                ErrorMessage="Please re-enter new password" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="getConfirmNewPassword" 
                                ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=_]).*$" 
                                Display="Dynamic" SetFocusOnError="true"
                                ErrorMessage="Invalid password" ForeColor="Red" Font-Size="10px">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
                <div runat="server" id="PasswordNotMatchMsg" visible="false" style="color:red; font-size:12px; padding:10px 10px 0.5px 10px">
                    <p>Passwords not match!</p>
                </div>
                <div style="background-color:lightgrey; font-size:12px; padding:10px 5px 0.5px 5px">
                    <p>Password must contain at least 8 characters long, 1 upper case letter, 1 special character and 1 number</p>
                </div>
                <asp:Button runat="server" ID="btnPassword" Text="Confirm" OnClick="ResetPassword" CssClass="btn-custom mt-4" ForeColor="White" BackColor="#0066ff" Width="100px"/>
            </div>
            <div runat="server" id="ResetPasswordError" visible="false" class="card-body text-center" style="font-weight: 600; color: grey">
                <p class="fas fa-exclamation-triangle fa-4x"></p>
                <p>Oops! This reset password link is expired or has been used</p>
                <asp:Button runat="server" ID="btnBack" Text="Back to Login" PostBackUrl="~/Login.aspx" CausesValidation="false" CssClass="btn-custom" ForeColor="White" BackColor="#0066ff" Width="150px"/>
            </div>
        </div>
    </div>
    <script lang="javascript" type="text/javascript">
        function showPassword1() {
            var x = document.getElementById('<%= getNewPassword.ClientID %>');
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        }
        function showPassword2() {
            var x = document.getElementById('<%= getConfirmNewPassword.ClientID %>');
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        }
    </script>
</asp:Content>
