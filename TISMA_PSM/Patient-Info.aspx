<%@ Page Title="Patient Info :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Patient-Info.aspx.cs" Inherits="TISMA_PSM.Patient_Info" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxTab" %>

<asp:Content ID="PatientInfo" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid content-p-custom" id="patient-info">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row header-p-custom">
                <div class="col ">
                    <h5>Module Registration</h5>
                </div>
                <div class="col align-self-end">
                    <div class="float-end subheader-custom">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;>&nbsp;&nbsp;Modules&nbsp;&nbsp;></b>&nbsp;&nbsp;Registration</span>
                    </div>
                </div>
            </div>
        </div>

        <!--CONTENT 1-->
        <div class="row justify-content-center mt-1" id="registration-new-content-1">
            <!--HEADER-->
            <div class="card border-0">
                <div class="row">
                    <div class="col align-self-start">
                        <div>
                            <asp:LinkButton runat="server" PostBackUrl="~/Registration.aspx">
                        <i class="fas fa-search me-1"></i>
                        <span>Search Registered Patient</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="col align-self-end">
                    </div>
                </div>
            </div>
        </div>

        <!--CONTENT 2-->
        <div class="row mt-3 mb-3" id="registration-new-content-2">
            <div class="col-2 p-0 m-0 me-4">
                <div class="row-cols-1">
                    <div class="card">
                        <div class="row justify-content-center card-body">
                            <img src="https://iupac.org/wp-content/uploads/2018/05/default-avatar.png" />
                        </div>
                        <div class="row text-center">
                            <h6><asp:Literal runat="server" ID="displayAccNo"></asp:Literal></h6>
                            <br />
                            <p style="font-size:12px; font-weight:500"><asp:Literal runat="server" ID="displayCategory"></asp:Literal></p>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row-cols-1">
                    <div class="card">
                        <div class="row justify-content-center card-body">
                        </div>
                        <div class="row card-body text-center">
                        </div>
                    </div>
                </div>
            </div>
            <div class="col p-0 m-0">
                <div class="card p-0">
                    <ajaxTab:TabContainer runat="server" ID="TabContainer1" BackColor="#0072c6">
                        <ajaxTab:TabPanel runat="server" ID="TabPanel1" HeaderText="Basic Info" BorderStyle="None">
                            <ContentTemplate>
                                <div class="row m-0 card-body pt-2 pb-1 ps-1 pe-1 note">
                                    <h5><b>Notes</b></h5>
                                    <h6><asp:Literal runat="server" ID="note"></asp:Literal></h6>
                                </div>
                                <div class="row m-0 card-body justify-content-center">
                                    <table id="register-form-table" style="text-align: left;">
                                        <tr>
                                            <!--Branch-->
                                            <td class="pe-3" style="text-align: right">Branch</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getBranch" Disabled="true" CssClass="textbox-custom disabled-textbox" Width="100px" ></asp:TextBox>
                                            </td>
                                            <!--Status-->
                                            <td class="pe-3" style="text-align: right">Status (UTM-ACAD/HR)</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getStat" Disabled="true" CssClass="textbox-custom disabled-textbox float-start" Width="100px" ></asp:TextBox>
                                                <p class="float" style="font-size:10px; color: green">&nbsp<asp:Literal runat="server" ID="statusText"></asp:Literal></p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Account No.-->
                                            <td class="pe-3" style="text-align: right">Account No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAccNo" Disabled="true" CssClass="textbox-custom disabled-textbox" ></asp:TextBox>
                                            </td>
                                            <!--Category-->
                                            <td class="pe-3" style="text-align: right">Category</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getCategory" CssClass="textbox-custom" Width="100px" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Name-->
                                            <td class="pe-3" style="text-align: right">Name</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getName" Disabled="true" CssClass="textbox-custom disabled-textbox pt-1" TextMode="MultiLine" Width="250px" Height="80px"></asp:TextBox>
                                            </td>
                                            <!--Staff/Matric No.-->
                                            <td class="pe-3" style="text-align: right">Staff/Matric No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getMatricNo" Disabled="true" CssClass="textbox-custom disabled-textbox" Width="140px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--IC No.-->
                                            <td class="pe-3" style="text-align: right">IC No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getIcNo" Disabled="true" CssClass="textbox-custom disabled-textbox" Width="140px"></asp:TextBox>
                                            </td>
                                            <!--Passport No.-->
                                            <td class="pe-3" style="text-align: right">Passport No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getPassportNo" CssClass="textbox-custom" Width="140px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--DOB-->
                                            <td class="pe-3" style="text-align: right">DOB</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getDob" Disabled="true" CssClass="textbox-custom disabled-textbox" Width="140px"></asp:TextBox>
                                            </td>
                                            <!--Age-->
                                            <td class="pe-3" style="text-align: right">Age</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAge" Disabled="true" CssClass="textbox-custom disabled-textbox" Width="40px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Gender-->
                                            <td class="pe-3" style="text-align: right">Gender</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getGender" Disabled="true" CssClass="textbox-custom disabled-textbox" Width="120px"></asp:TextBox>
                                            </td>
                                            <!--Marital Status-->
                                            <td class="pe-3" style="text-align: right">Marital Status</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getMaritalStat" CssClass="textbox-custom" Width="140px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Religion-->
                                            <td class="pe-3" style="text-align: right">Religion</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getReligion" Disabled="true" CssClass="textbox-custom disabled-textbox" Width="120px"></asp:TextBox>
                                            </td>
                                            <!--Race-->
                                            <td class="pe-3" style="text-align: right">Race</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getRace" Disabled="true" CssClass="textbox-custom disabled-textbox"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Nationality-->
                                            <td class="pe-3" style="text-align: right">Nationality</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getNation" Disabled="true" CssClass="textbox-custom disabled-textbox"></asp:TextBox>
                                            </td
                                        </tr>
                                        <tr>
                                            <!--Phone No.-->
                                            <td class="pe-3" style="text-align: right">Phone No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getPhone" CssClass="textbox-custom" Width="140px"></asp:TextBox>
                                            </td>
                                            <!--Email-->
                                            <td class="pe-3" style="text-align: right">Email</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getEmail" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Designation-->
                                            <td class="pe-3" style="text-align: right">Designation</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getDesignation" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="50px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Course-->
                                            <td class="pe-3" style="text-align: right">Course</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getCourse" Disabled="true" CssClass="textbox-custom disabled-textbox pt-1" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                                            </td>
                                            <!--Faculty/Department-->
                                            <td class="pe-3" style="text-align: right">Faculty/Department</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getFacDep" Disabled="true" CssClass="textbox-custom disabled-textbox pt-1" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Semester-->
                                            <td class="pe-3" style="text-align: right">Semester</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getSem" CssClass="textbox-custom text-center" Width="50px"></asp:TextBox>
                                            </td>
                                            <!--Session-->
                                            <td class="pe-3" style="text-align: right">Session</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getSession" CssClass="textbox-custom" Width="120px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Address-->
                                            <td class="pe-3" style="text-align: right">Home Address</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAddress" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                                            </td>
                                            <!--Remarks-->
                                            <td class="pe-3" style="text-align: right">Remarks</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getRemarks" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Button runat="server" ID="BtnUpdate" Text="Update" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#00cc00" />
                                    &nbsp&nbsp&nbsp
                                    <asp:Button runat="server" ID="BtnDelete" Text="Delete" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#ff0000" />
                                </div>
                            </ContentTemplate>
                        </ajaxTab:TabPanel>
                    </ajaxTab:TabContainer>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
