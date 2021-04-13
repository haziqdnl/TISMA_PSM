<%@ Page Title="Dashboard :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TISMA_PSM.Dashboard" %>

<asp:Content ID="Dashboard" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" style="padding: 2rem 2rem 2rem 2rem" id="dashboard">
        <!--CARDS: Doctor/Waiting/Patients Registered-->
        <!--TO-DO: Logic for the number on each card-->
        <div class="row justify-content-center">
            <!--CARD 1-->
            <div class="card col align-self-start me-4" style="background: #38909a;">
                <div class="row w-auto card-body">
                    <div class="col align-self-start">
                        <div class="row align-self-start">
                            <h1>0</h1>
                        </div>
                        <div class="row align-self-end">
                            <h5>Doctors Available</h5>
                        </div>
                    </div>
                    <div class="col align-self-end">
                        <i class="fas fa-stethoscope fa-5x float-end" style="color: #000000; opacity: 0.35;"></i>
                    </div>
                </div>
            </div>
            <!--CARD 2-->
            <div class="card col align-self-start me-4" style="background: #fe8c00;">
                <div class="row w-auto card-body">
                    <div class="col align-self-start">
                        <div class="row align-self-start">
                            <h1>0</h1>
                        </div>
                        <div class="row align-self-end">
                            <h5>Patients Waiting</h5>
                        </div>
                    </div>
                    <div class="col align-self-end">
                        <i class="fas fa-clock fa-5x float-end" style="color: #000000; opacity: 0.3;"></i>
                    </div>
                </div>
            </div>
            <!--CARD 3-->
            <div class="card col align-self-start" style="background: #c23737;">
                <div class="row w-auto card-body">
                    <div class="col align-self-start">
                        <div class="row row-cols-lg-1 align-self-start">
                            <h1>0</h1>
                        </div>
                        <div class="row align-self-end">
                            <h5>Patients Registered</h5>
                        </div>
                    </div>
                    <div class="col align-self-end">
                        <i class="fas fa-clipboard-check fa-5x float-end" style="color: #000000; opacity: 0.35;"></i>
                    </div>
                </div>
            </div>
        </div>
        <!--GOOGLE CALENDAR-->
        <!--TO-DO: Logic that passing the user email to be embedded into the src-->
        <div class="row justify-content-center mt-4" id="g-calendar">
            <iframe src="https://calendar.google.com/calendar/embed?src=furalv7398%40gmail.com&ctz=Asia%2FKuala_Lumpur&src=ZW4ubWFsYXlzaWEjaG9saWRheUBncm91cC52LmNhbGVuZGFyLmdvb2dsZS5jb20&amp&showTitle=0&amp&showTz=1&amp&showPrint=0"
                style="border-width: 0" width="800" height="600" frameborder="0" scrolling="no"></iframe>
        </div>
    </div>
</asp:Content>
