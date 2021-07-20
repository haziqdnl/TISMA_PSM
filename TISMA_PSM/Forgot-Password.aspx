<%@ Page Title="Forgot Password :: TISMA" Language="C#" MasterPageFile="~/MasterWeb.Master" AutoEventWireup="true" CodeBehind="Forgot-Password.aspx.cs" Inherits="TISMA_PSM.Forgot_Password" %>

<asp:Content ID="ForgotPassword" ContentPlaceHolderID="WebContent" runat="server">
    <div class="container justify-content-center align-items-center center-custom">
        <div id="Title" class="card text-center">
            <p>TISMA<span>PKU</span></p>
        </div>
        <div id="ForgotPasswordCard" class="card w-100">
            <div runat="server" id="ForgotPasswordRequest" class="card-body text-center">
                <p style="font-weight: 500; color: grey">Please enter your <b>username</b>. An email containing a link to reset your password will be sent to your email address.</p>
                <br />
                <table id="ForgotPasswordTable" class="mb-4">
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="getUsername" MaxLength="30" CssClass="textbox-custom text-center" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p runat="server" id="UsernameIncorrectMsg" visible="false" style="color:red; font-size:10px">Username incorrect</p>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="getUsername"
                                ErrorMessage="Please enter username" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" ControlToValidate="getUsername" 
                                ValidationExpression="^([a-z]|[A-Z]|[0-9]|[._-]){3,30}$" Display="Dynamic" SetFocusOnError="true"
                                ErrorMessage="Invalid username" ForeColor="Red" Font-Size="10px">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Button runat="server" ID="btnCancel" Text="Back" PostBackUrl="~/Login.aspx" CausesValidation="false" CssClass="btn-custom float-start" ForeColor="White" BackColor="#0066ff" Width="100px"/>
                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="ConfirmForgotPassword" CssClass="btn-custom float-end" ForeColor="White" BackColor="#00cc00" Width="100px"/>
                </div>
            </div>
            <div runat="server" id="ForgotPasswordSuccess" visible="false" class="card-body text-center">
                <p style="font-weight: 500; color: grey">An email containing a password reset link has been sent to your email address. Please check for further action.</p>
                <p style="font-weight: 500; color: grey">You may dismiss this window.</p>
                <asp:Button runat="server" ID="btnBack" Text="Back to Login" PostBackUrl="~/Login.aspx" CausesValidation="false" CssClass="btn-custom" ForeColor="White" BackColor="#0066ff" Width="150px"/>
            </div>
        </div>
    </div>
</asp:Content>
