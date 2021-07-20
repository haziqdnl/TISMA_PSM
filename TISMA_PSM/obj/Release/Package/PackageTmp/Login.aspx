<%@ Page Title="Login :: TISMA" Language="C#" MasterPageFile="~/MasterWeb.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TISMA_PSM.Login" %>

<asp:Content ID="Login" ContentPlaceHolderID="WebContent" runat="server">
    <div class="container justify-content-center align-items-center center-custom">
        <div id="Title" class="card text-center">
            <p>TISMA<span>PKU</span></p>
        </div>
        <div id="LoginCard" class="card w-100">
            <div class="card-body text-center">
                <p style="font-weight: 600; color: grey">Sign in to start your session</p>
                <br />
                <table id="LoginTable" class="mb-4">
                    <tr>
                        <td style="width: 120px">
                            <p><span class="fas fa-user me-3"></span>Username</p>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="getUsername" MaxLength="30" CssClass="textbox-custom" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <p runat="server" id="UsernameIncorrectMsg" visible="false" style="color:red; font-size:10px">Username incorrect</p>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="getUsername"
                                ErrorMessage="Please enter username" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" ControlToValidate="getUsername" 
                                ValidationExpression="^([a-z]|[A-Z]|[0-9]|[._-]){3,30}$" 
                                Display="Dynamic" SetFocusOnError="true"
                                ErrorMessage="Username invalid" ForeColor="Red" Font-Size="10px">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="m-2 p-2"></td>
                    </tr>
                    <tr>
                        <td style="width: 120px">
                            <p><span class="fas fa-lock me-3"></span>Password</p>
                        </td>
                        <td>
                            <div class="input-group h-100">  
                                <asp:TextBox runat="server" ID="getPassword" TextMode="Password" MaxLength="70" CssClass="textbox-custom" Width="85%"></asp:TextBox>  
                                <div class="input-group-append">
                                    <span class="input-group-text">
                                        <input type="checkbox" onclick="showPassword()" style="height:16.5px"> 
                                    </span>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <p runat="server" id="PasswordIncorrectMsg" visible="false" style="color:red; font-size:10px">Password incorrect</p>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="getPassword"
                                ErrorMessage="Please enter password" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="getPassword" 
                                ValidationExpression="^([a-z]|[A-Z]|[0-9]|[!*@#$%^&+=_]){8,70}$" 
                                Display="Dynamic" SetFocusOnError="true"
                                ErrorMessage="Password invalid" ForeColor="Red" Font-Size="10px">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
                <div id="login-option">
                    <a class="float-start" href="Forgot-Password.aspx">I forgot my password</a>
                    <asp:Button runat="server" ID="btnPassword" Text="Sign In" OnClick="SignIn" CssClass="btn-custom float-end" ForeColor="White" BackColor="#0066ff" Width="100px"/>
                </div>
            </div>
        </div>
    </div>
    <script lang="javascript" type="text/javascript">
        function showPassword() {
            var x = document.getElementById('<%= getPassword.ClientID %>');
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        }
    </script>
</asp:Content>
