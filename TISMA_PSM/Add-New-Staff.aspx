﻿<%@ Page Title="Staff :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Add-New-Staff.aspx.cs" Inherits="TISMA_PSM.Add_New_Staff" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxNewStaff" %>

<asp:Content ID="NewStaff" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid content-p-custom" id="new-staff">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row header-p-custom">
                <div class="col ">
                    <h5>Module Staff</h5>
                </div>
                <div class="col align-self-end">
                    <div class="float-end subheader-custom">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;>&nbsp;&nbsp;Modules&nbsp;&nbsp;></b>&nbsp;&nbsp;Staff</span>
                    </div>
                </div>
            </div>
        </div>

        <!--CONTENT 1-->
        <div class="row justify-content-center mt-1" id="new-staff-content-1">
            <!--HEADER-->
            <div class="card border-0">
                <div class="row">
                    <div class="col align-self-start">
                        <i class="fas fa-clipboard-list me-1"></i>
                        <span>Adding New PKU Staff/User</span>
                    </div>
                    <div class="col align-self-end">
                        <div class="float-end">
                            <asp:LinkButton runat="server" PostBackUrl="~/Staff.aspx">
                        <i class="fas fa-search me-1"></i>
                        <span>Search Added Staff/User</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--CONTENT 2-->
        <div class="row mt-3 mb-3" id="new-staff-content-2">
            <div class="col-2 p-0 m-0 me-4">
                <div class="card">
                    <div class="row justify-content-center card-body">
                        <img src="https://iupac.org/wp-content/uploads/2018/05/default-avatar.png" />
                    </div>
                </div>
            </div>
            <div class="col p-0 m-0">
                <div class="card p-0">
                    <ajaxNewStaff:TabContainer runat="server" ID="TabContainer1" BackColor="#0072c6">
                        <ajaxNewStaff:TabPanel runat="server" ID="TabPanel1" HeaderText="Basic Info" BorderStyle="None">
                            <ContentTemplate>
                                <div class="row m-0 card-body pt-2 pb-1 ps-1 pe-1 note">
                                    <h5><b>Note</b></h5>
                                    <h6>The basic information were reffered from UTMHR SYSTEM</h6>
                                </div>
                                <div class="row m-0 card-body justify-content-center">
                                    <p class="fas fa-exclamation-circle" style="font-size:10px; color: green">&nbsp Auto-generated by the system</p>
                                    <table id="add-staff-form-table" style="text-align: left;">
                                        <tr>
                                            <!--Branch-->
                                            <td class="pe-3" style="text-align: right">Branch</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getBranch" Enabled="false" CssClass="dropdown-custom" BackColor="White" AppendDataBoundItems="true">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="UTM-JB" Value="UTM-JB"></asp:ListItem>
                                                    <asp:ListItem Text="UTM-KL" Value="UTM-KL"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <!--Category-->
                                            <td class="pe-3" style="text-align: right">Category</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getCategory" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="100px" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Account No.-->
                                            <td class="pe-3" style="text-align: right">Account No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAccNo" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                                <p class="fas fa-exclamation-circle" style="font-size:10px; color: green"></p>
                                            </td>
                                            <!--Username-->
                                            <td class="pe-3" style="text-align: right">Username</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getUsername" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                                <p class="fas fa-exclamation-circle" style="font-size:10px; color: green"></p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Password-->
                                            <td class="pe-3" style="text-align: right">Password</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getPassword" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                                <p class="fas fa-exclamation-circle" style="font-size:10px; color: green"></p>
                                            </td>
                                            <!--TISMA Role-->
                                            <td class="pe-3" style="text-align: right">TISMA Role <span style="color:red">*</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getTismaRoleDdl" CssClass="dropdown-custom" BackColor="White" AppendDataBoundItems="true">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                                                    <asp:ListItem Text="Medical Officer" Value="Medical Officer"></asp:ListItem>
                                                    <asp:ListItem Text="Receptionist" Value="Receptionist"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="getTismaRoleDdl" 
                                                    InitialValue="Select" 
                                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Name-->
                                            <td class="pe-3" style="text-align: right">Name</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getName" ReadOnly="true" Enabled="false" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="80px"></asp:TextBox>
                                            </td>
                                            <!--Staff ID-->
                                            <td class="pe-3" style="text-align: right">Staff ID</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getStaffId" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--IC No.-->
                                            <td class="pe-3" style="text-align: right">IC No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getIcNo" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                            </td>
                                            <!--Passport No.-->
                                            <td class="pe-3" style="text-align: right">Passport No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getPassportNo" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--DOB-->
                                            <td class="pe-3" style="text-align: right">DOB</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getDob" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                            </td>
                                            <!--Age-->
                                            <td class="pe-3" style="text-align: right">Age</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAge" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="40px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Gender-->
                                            <td class="pe-3" style="text-align: right">Gender</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getGender" Enabled="false" CssClass="dropdown-custom" BackColor="White" Width="250px">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <!--Marital Status-->
                                            <td class="pe-3" style="text-align: right">Marital Status</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getMaritalStat" Enabled="false" CssClass="dropdown-custom" BackColor="White" Width="250px">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Single" Value="Single"></asp:ListItem>
                                                    <asp:ListItem Text="Married" Value="Married"></asp:ListItem>
                                                    <asp:ListItem Text="Divorced " Value="Divorced"></asp:ListItem>
                                                    <asp:ListItem Text="Widowed " Value="Widowed"></asp:ListItem>
                                                    <asp:ListItem Text="Separated " Value="Separated"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Religion-->
                                            <td class="pe-3" style="text-align: right">Religion</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getReligion" Enabled="false" CssClass="dropdown-custom" BackColor="White" Width="250px">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Muslim" Value="Muslim"></asp:ListItem>
                                                    <asp:ListItem Text="Buddha" Value="Buddha"></asp:ListItem>
                                                    <asp:ListItem Text="Christian" Value="Christian"></asp:ListItem>
                                                    <asp:ListItem Text="Hindu" Value="Hindu"></asp:ListItem>
                                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <!--Race-->
                                            <td class="pe-3" style="text-align: right">Race</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getRace" Enabled="false" CssClass="dropdown-custom" BackColor="White" Width="250px">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Malay/Bumiputera" Value="Malay/Bumiputera"></asp:ListItem>
                                                    <asp:ListItem Text="Chinese" Value="Chinese"></asp:ListItem>
                                                    <asp:ListItem Text="Indian" Value="Indian"></asp:ListItem>
                                                    <asp:ListItem Text="Non-Malaysian" Value="Non-Malaysian"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Nationality-->
                                            <td class="pe-3" style="text-align: right">Nationality</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getNation" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                            </td
                                        </tr>
                                        <tr>
                                            <!--Phone No.-->
                                            <td class="pe-3" style="text-align: right">Phone No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getPhone" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                            </td>
                                            <!--Email-->
                                            <td class="pe-3" style="text-align: right">Email</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getEmail" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Designation-->
                                            <td class="pe-3" style="text-align: right">Designation</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getDesignation" ReadOnly="true" Enabled="false" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                                            </td>
                                            <!--Department-->
                                            <td class="pe-3" style="text-align: right">Faculty/Department</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getFacDep" ReadOnly="true" Enabled="false" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Address-->
                                            <td class="pe-3" style="text-align: right">Home Address</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAddress" ReadOnly="true" Enabled="false" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                                            </td>
                                            <!--Session-->
                                            <td class="pe-3" style="text-align: right">Session</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getSession" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Button runat="server" ID="BtnAddtoTISMA" Text="Add to TISMA" OnClick="AddToTisma" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#00cc00" />
                                </div>
                            </ContentTemplate>
                        </ajaxNewStaff:TabPanel>
                    </ajaxNewStaff:TabContainer>
                </div>
            </div>
        </div>

        <!--MODUL POPUP: POPUP MESSAGE-->
        <div style="display:none">
            <asp:Button runat="server" ID="BtnPopup" Text="Open Popup" CssClass="btn-custom mt-2" Width="100%" ForeColor="White" BackColor="#0066ff" />
        </div>
        <ajaxNewStaff:ModalPopupExtender runat="server" ID="ModalPopupMessage" PopupControlID="PanelPopup" TargetControlID="BtnPopup" CancelControlID="BtnCancel" BackgroundCssClass="Background">
        </ajaxNewStaff:ModalPopupExtender>
        <asp:Panel runat="server" ID="PanelPopup" CssClass="Popup" Width="600px" Style="display: none">
            <div class="card">
                <div class="row">
                    <div class="col align-self-center">
                        <span>System Message</span>
                    </div>
                    <div class="col align-self-end">
                        <div class="float-end" style="display:none">
                            <!--CANCEL BTN-->
                            <asp:Button runat="server" ID="BtnCancel" Font-Size="Larger" Text="X" Font-Bold="true" BorderStyle="None" CssClass="btn-custom" ForeColor="#808080" BackColor="White" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-body text-center">
                <div class="row">
                    <p>This staff/user already added to TISMA. Please check again!</p>
                </div>
                <div>
                    <asp:Button runat="server" ID="BtnBackto" Text="Back to Staff Module" PostBackUrl="~/Staff.aspx" CausesValidation="false" CssClass="btn-custom mt-1 mb-2" ForeColor="White" BackColor="#0066ff"></asp:Button>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
