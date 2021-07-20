<%@ Page Title="Reset Password :: TISMA" Language="C#" MasterPageFile="~/MasterWeb.Master" AutoEventWireup="true" CodeBehind="Reset-Password-Done.aspx.cs" Inherits="TISMA_PSM.Reset_Password_Done" %>

<asp:Content ID="ResetPasswordDone" ContentPlaceHolderID="WebContent" runat="server">
    <div class="container justify-content-center align-items-center center-custom">
        <div id="Title" class="card text-center">
            <p>TISMA<span>PKU</span></p>
        </div>
        <div id="ResetPasswordCard" class="card w-100">
            <div class="card-body text-center" style="font-weight: 600; color: grey">
                <p class="fas fa-check fa-4x"></p>
                <p>Your password has been reset successfully</p>
                <asp:Button runat="server" ID="btnBack" Text="Back to Login" PostBackUrl="~/Login.aspx" CausesValidation="false" CssClass="btn-custom" ForeColor="White" BackColor="#0066ff" Width="150px"/>
            </div>
        </div>
    </div>
</asp:Content>
