<%@ Page Title="Login :: TISMA" Language="C#" MasterPageFile="~/MasterWeb.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TISMA_PSM.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="WebContent" runat="server">
    <div class="container d-flex justify-content-center align-items-center center-custom">
        <div id="LoginCard" class="card w-100">
            <div class="card-header text-center">
                <p style="font-size: 40px; font-weight: 700; color: red">TISMA<span style="color: grey">PKU</span></p>
            </div>
            <div class="card-body text-center">
                <p style="font-weight: 600; color: grey">Sign in to start your session</p>
                <br />
                <table id="LoginTable" class="mb-4">
                    <tr>
                        <td style="width: 120px">
                            <p><span class="fas fa-user me-3"></span>Username</p>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="getUsername" CssClass="textbox-custom" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="getUsername"
                                ErrorMessage="Please enter username" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" ControlToValidate="getUsername" 
                                ValidationExpression="^[^<>]+$" Display="Dynamic" SetFocusOnError="true"
                                ErrorMessage="Invalid username" ForeColor="Red" Font-Size="10px">
                            </asp:RegularExpressionValidator>
                            <p runat="server" id="UsernameIncorrectMsg" visible="false" style="color:red; font-size:10px">Username incorrect</p>
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
                            <asp:TextBox runat="server" ID="getPassword" TextMode="Password" CssClass="textbox-custom" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="getPassword"
                                ErrorMessage="Please enter password" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="getPassword" 
                                ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=_]).*$" Display="Dynamic" SetFocusOnError="true"
                                ErrorMessage="Invalid password" ForeColor="Red" Font-Size="10px">
                            </asp:RegularExpressionValidator>
                            <p runat="server" id="PasswordIncorrectMsg" visible="false" style="color:red; font-size:10px">Password incorrect</p>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnPassword" Text="Sign In" OnClick="SignIn" CssClass="btn-custom" ForeColor="White" BackColor="#0066ff" Width="100px"/>
            </div>
        </div>
    </div>
</asp:Content>
