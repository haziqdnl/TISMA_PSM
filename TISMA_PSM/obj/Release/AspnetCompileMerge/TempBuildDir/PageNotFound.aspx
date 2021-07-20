<%@ Page Title="404 :: TISMA" Language="C#" MasterPageFile="~/MasterWeb.Master" AutoEventWireup="true" CodeBehind="PageNotFound.aspx.cs" Inherits="TISMA_PSM.PageNotFound" %>

<asp:Content ID="PageNotFound" ContentPlaceHolderID="WebContent" runat="server">
    <div class="container justify-content-center align-items-center center-custom">
        <div id="Title" class="card text-center">
            <p>TISMA<span>PKU</span></p>
        </div>
        <div id="ErrorCard" class="card w-100">
            <div class="card-body text-center" >
                <p style="font-weight: 800; font-size:100px; color:darkred; letter-spacing:4px;">
                    4<span style="font-size:85px" class="fas fa-exclamation-circle"></span>4
                </p>
                <p style="font-weight: 700;">
                    Oops! Page Not Found
                </p>
                <p style="font-weight: 600; color:grey">
                    It seems that the page you're looking for doesn't exist or is temporary unavailable.
                </p><br />
                <asp:Button runat="server" ID="btnBackToLogin" Text="Back to Login" PostBackUrl="~/Login.aspx" CausesValidation="false" CssClass="btn-custom" ForeColor="White" BackColor="#0066ff" />
                <asp:Button runat="server" ID="btnBackToHome" Text="Back to Dashboard" PostBackUrl="~/Dashboard.aspx" Visible="false" CausesValidation="false" CssClass="btn-custom" ForeColor="White" BackColor="#0066ff" />
            </div>
        </div>
    </div>
</asp:Content>
