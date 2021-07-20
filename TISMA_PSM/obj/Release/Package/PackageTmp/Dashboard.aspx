<%@ Page Title="Dashboard :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TISMA_PSM.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Dashboard" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" style="padding: 2rem 2rem 2rem 2rem;" id="dashboard">
        <!--CARDS: Doctor/Waiting/Patients Registered-->
        <div class="row">
            <p style="font-size: 40px; font-weight: 700; color: red">TISMA<span style="color: grey">PKU</span>&nbsp<span style="font-size: 11px; color: #000">Total Information System for Medical Administration</span></p>
            <hr />
        </div>
        <div class="row justify-content-center text-center">
            <div class="card col align-self-start" style="background: #5c001f; width: 100%; height: 100px; padding: 1rem 1rem 1.5rem 1rem;">
                <h3>Today's Summary</h3>
                <h4>( <asp:Literal runat="server" ID="getTodayDayName"></asp:Literal>,
                    <asp:Literal runat="server" ID="getTodayDate"></asp:Literal> )</h4>
            </div>
        </div>
        <div class="row justify-content-center mt-4">
            <!--CARD 1-->
            <div class="card col align-self-start me-4" style="background: #38909a; height: 120px">
                <div class="row w-auto card-body">
                    <div class="col align-self-start">
                        <div class="row align-self-start">
                            <h1>
                                <asp:Literal runat="server" ID="getDoctorAvailable"></asp:Literal></h1>
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
            <div class="card col align-self-start me-4" style="background: #fe8c00; height: 120px">
                <div class="row w-auto card-body">
                    <div class="col align-self-start">
                        <div class="row align-self-start">
                            <h1>
                                <asp:Literal runat="server" ID="getPatientWaiting"></asp:Literal></h1>
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
            <div class="card col align-self-start" style="background: #c23737; height: 120px">
                <div class="row w-auto card-body">
                    <div class="col align-self-start">
                        <div class="row row-cols-lg-1 align-self-start">
                            <h1>
                                <asp:Literal runat="server" ID="getPatientRegistered"></asp:Literal></h1>
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
        <div class="mt-4"></div>
        <div class="row justify-content-center">
            <!--CARD 4-->
            <div class="card col align-self-start me-4" style="background: #6abec7; height: 120px">
                <div class="row w-auto card-body">
                    <div class="col align-self-start">
                        <div class="row align-self-start">
                            <h1>
                                <asp:Literal runat="server" ID="getEMCGenerate"></asp:Literal></h1>
                        </div>
                        <div class="row align-self-end">
                            <h5>e-MC Generated</h5>
                        </div>
                    </div>
                    <div class="col align-self-end">
                        <i class="fas fa-paperclip fa-5x float-end" style="color: #000000; opacity: 0.35;"></i>
                    </div>
                </div>
            </div>
            <!--CARD 5-->
            <div class="card col align-self-start me-4" style="background: #f7b15b; height: 120px">
                <div class="row w-auto card-body">
                    <div class="col align-self-start">
                        <div class="row align-self-start">
                            <h1>
                                <asp:Literal runat="server" ID="getPatientCheckedIn"></asp:Literal></h1>
                        </div>
                        <div class="row align-self-end">
                            <h5>Patients In-Room</h5>
                        </div>
                    </div>
                    <div class="col align-self-end">
                        <i class="fas fa-pen-square fa-5x float-end" style="color: #000000; opacity: 0.3;"></i>
                    </div>
                </div>
            </div>
            <!--CARD 6-->
            <div class="card col align-self-start" style="background: #c46363; height: 120px">
                <div class="row w-auto card-body">
                    <div class="col align-self-start">
                        <div class="row row-cols-lg-1 align-self-start">
                            <h1>
                                <asp:Literal runat="server" ID="getPatientCheckedOut"></asp:Literal></h1>
                        </div>
                        <div class="row align-self-end">
                            <h5>Patients Checked-Out</h5>
                        </div>
                    </div>
                    <div class="col align-self-end">
                        <i class="fas fa-check-circle fa-5x float-end" style="color: #000000; opacity: 0.35;"></i>
                    </div>
                </div>
            </div>
        </div>
        <div id="ChartDashboard" class="text-center mt-5">
            <asp:Chart runat="server" ID="Chart1" Width="600px" Height="300px" BackColor="#e9e9e9">
                <Series>
                    <asp:Series Name="Series" ChartType="Column" Color="#c23737" YValuesPerPoint="1" />
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea" Area3DStyle-Enable3D="true" BackColor="#e9e9e9"  />
                </ChartAreas>
            </asp:Chart>
        </div>
    </div>
</asp:Content>
