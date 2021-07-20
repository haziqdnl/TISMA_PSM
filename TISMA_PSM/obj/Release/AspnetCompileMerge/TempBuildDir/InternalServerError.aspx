<%@ Page Title="500 :: TISMA" Language="C#" MasterPageFile="~/MasterWeb.Master" AutoEventWireup="true" CodeBehind="InternalServerError.aspx.cs" Inherits="TISMA_PSM.InternalServerError" %>

<asp:Content ID="InternalServerError" ContentPlaceHolderID="WebContent" runat="server">
    <div class="container justify-content-center align-items-center center-custom">
        <div id="Title" class="card text-center">
            <p>TISMA<span>PKU</span></p>
        </div>
        <div id="ErrorCard" class="card w-100">
            <div class="card-body text-center" >
                <p style="font-weight: 800; font-size:100px; color:darkred; letter-spacing:4px;">
                    5<span class="fas fa-cog"></span>0
                </p>
                <p style="font-weight: 700;">
                    Server Error
                </p>
                <p style="font-weight: 600; color:grey">
                    The server encountered an internal error and was unable to complete your request.
                </p>
                <p style="font-weight: 600; color:grey">
                    Please try again later and feel free to contact us if the problem persists.
                </p><br />
                <asp:Button runat="server" ID="btnBackToLogin" Text="Back to Login" PostBackUrl="~/Login.aspx" CausesValidation="false" CssClass="btn-custom" ForeColor="White" BackColor="#0066ff" />
                <asp:Button runat="server" ID="btnBackToHome" Text="Back to Dashboard" PostBackUrl="~/Dashboard.aspx" Visible="false" CausesValidation="false" CssClass="btn-custom" ForeColor="White" BackColor="#0066ff" />
            </div>
        </div>
    </div>
</asp:Content>
