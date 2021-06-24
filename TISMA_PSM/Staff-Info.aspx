<%@ Page Title="Staff :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Staff-Info.aspx.cs" Inherits="TISMA_PSM.Staff_Info" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxStaffInfo" %>

<asp:Content ID="StaffInfo" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid content-p-custom" id="staff-info">
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
        <div class="row justify-content-center mt-1" id="staff-info-content-1">
            <!--HEADER-->
            <div class="card border-0">
                <div class="row">
                    <div class="col align-self-start">
                        <div>
                            <asp:LinkButton runat="server" PostBackUrl="~/Staff.aspx">
                                <i class="fas fa-search me-1"></i>
                                <span>Search Registered Staff/User</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="col align-self-end">
                    </div>
                </div>
            </div>
        </div>

        <!--CONTENT 2-->
        <div class="row mt-3 mb-3" id="patient-info-content-2">
            <div class="col-2 p-0 m-0 me-4">
                <!--PROFILE PICTURE-->
                <div class="row-cols-1">
                    <div class="card">
                        <div class="row justify-content-center card-body">
                            <img src="https://iupac.org/wp-content/uploads/2018/05/default-avatar.png" />
                        </div>
                        <div class="row text-center">
                            <h6><asp:Literal runat="server" ID="displayAccNo"></asp:Literal></h6>
                            <br />
                            <p style="font-size:12px; font-weight:500"><asp:Literal runat="server" ID="displayUsername"></asp:Literal></p>
                        </div>
                    </div>
                </div>
                <br />
            </div>
            <div class="col p-0 m-0">
                <div class="card p-0">
                    <ajaxStaffInfo:TabContainer runat="server" ID="TabContainer1" ActiveTabIndex="0" BackColor="#0072c6">
                        <ajaxStaffInfo:TabPanel runat="server" ID="TabPanel1" HeaderText="Basic Info" BorderStyle="None">
                            <ContentTemplate>
                                <div class="row m-0 card-body pt-2 pb-1 ps-1 pe-1 note">
                                    <h5><b>Note</b></h5>
                                    <h6>The basic information were reffered from UTMHR SYSTEM</h6>
                                </div>
                                <div class="row m-0 card-body justify-content-center">
                                    <p style="font-size:11px">&nbsp <span style="color:red"><b>*</b></span> Changes of this detail is allowed</p>
                                    <table id="register-form-table" style="text-align: left;">
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
                                                <asp:TextBox runat="server" ID="getCategory" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Account No.-->
                                            <td class="pe-3" style="text-align: right">Account No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAccNo" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px" ></asp:TextBox>
                                            </td>
                                            <!--Username-->
                                            <td class="pe-3" style="text-align: right">Username</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getUsername" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Password-->
                                            <td class="pe-3" style="text-align: right">Change Password <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getPassword" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" ControlToValidate="getPassword" 
                                                    ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=_]).*$" Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Invalid" ForeColor="Red" Font-Size="10px">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                            <!--TISMA Role-->
                                            <td class="pe-3" style="text-align: right">TISMA Role</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getTismaRoleDdl" Enabled="false" CssClass="dropdown-custom" BackColor="White" AppendDataBoundItems="true">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                                                    <asp:ListItem Text="Medical Officer" Value="Medical Officer"></asp:ListItem>
                                                    <asp:ListItem Text="Receptionist" Value="Receptionist"></asp:ListItem>
                                                </asp:DropDownList>
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
                                            <td class="pe-3" style="text-align: right">Passport No. <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getPassportNo" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="getPassportNo" 
                                                    ValidationExpression="^[A-Za-z0-9]{10,12}$" Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Invalid" ForeColor="Red" Font-Size="10px">
                                                </asp:RegularExpressionValidator>
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
                                            <td class="pe-3" style="text-align: right">Marital Status <span style="color:red">*</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getMaritalStat" CssClass="dropdown-custom" AutoPostBack="true" BackColor="White" Width="250px">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Single" Value="Single"></asp:ListItem>
                                                    <asp:ListItem Text="Married" Value="Married"></asp:ListItem>
                                                    <asp:ListItem Text="Divorced " Value="Divorced"></asp:ListItem>
                                                    <asp:ListItem Text="Widowed " Value="Widowed"></asp:ListItem>
                                                    <asp:ListItem Text="Separated " Value="Separated"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="getMaritalStat" 
                                                    InitialValue="Select" 
                                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Religion-->
                                            <td class="pe-3" style="text-align: right">Religion <span style="color:red">*</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getReligion" CssClass="dropdown-custom" AutoPostBack="true"  BackColor="White" Width="250px">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Muslim" Value="Muslim"></asp:ListItem>
                                                    <asp:ListItem Text="Buddha" Value="Buddha"></asp:ListItem>
                                                    <asp:ListItem Text="Christian" Value="Christian"></asp:ListItem>
                                                    <asp:ListItem Text="Hindu" Value="Hindu"></asp:ListItem>
                                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="getReligion" 
                                                    InitialValue="Select" 
                                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
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
                                            <td class="pe-3" style="text-align: right">Phone No. <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getPhone" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator4" ControlToValidate="getPhone" 
                                                    ValidationExpression="[0-9]{10,15}" 
                                                    Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Invalid" ForeColor="Red" Font-Size="10px">
                                                </asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" ControlToValidate="getPhone"
                                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <!--Email-->
                                            <td class="pe-3" style="text-align: right">Email</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getEmail" ReadOnly="true" Enabled="false"  CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3" ControlToValidate="getEmail" 
                                                    ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" 
                                                    Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="Invalid" ForeColor="Red" Font-Size="10px">
                                                </asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ControlToValidate="getEmail"
                                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Designation-->
                                            <td class="pe-3" style="text-align: right">Designation</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getDesignation" ReadOnly="true" Enabled="false" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="50px"></asp:TextBox>
                                            </td>
                                            <!--Department-->
                                            <td class="pe-3" style="text-align: right">Faculty/Department</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getFacDep" ReadOnly="true" Enabled="false" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Address-->
                                            <td class="pe-3" style="text-align: right">Address <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAddress" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="getAddress"
                                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <!--Session-->
                                            <td class="pe-3" style="text-align: right">Session <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getSession" CssClass="textbox-custom" Width="250px"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="getSession"
                                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Button runat="server" ID="BtnUpdate" Text="Save" OnClick="UpdateToTisma" Width="80px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#00cc00" />
                                    &nbsp&nbsp&nbsp
                                    <asp:Button runat="server" ID="BtnDelete" Text="Delete" OnClick="DeleteConfirmation" Width="80px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#ff0000" />
                                </div>
                            </ContentTemplate>
                        </ajaxStaffInfo:TabPanel>
                    </ajaxStaffInfo:TabContainer>
                </div>
            </div>
        </div>

        <!--MODUL POPUP: UPDATE POPUP MESSAGE-->
        <div style="display:none">
            <asp:Button runat="server" ID="BtnPopupUpdate" Text="Open Popup" CssClass="btn-custom mt-2" Width="100%" ForeColor="White" BackColor="#0066ff" />
        </div>
        <ajaxStaffInfo:ModalPopupExtender runat="server" ID="ModalPopupMessageUpdate" PopupControlID="PanelPopupUpdate" TargetControlID="BtnPopupUpdate" CancelControlID="BtnOK" BackgroundCssClass="Background">
        </ajaxStaffInfo:ModalPopupExtender>
        <asp:Panel runat="server" ID="PanelPopupUpdate" CssClass="Popup" Width="600px" Style="display: none">
            <div class="card">
                <div class="row">
                    <div class="col align-self-center">
                        <span>System Message</span>
                    </div>
                    <div class="col align-self-end">
                    </div>
                </div>
            </div>
            <div class="card-body text-center">
                <div class="row">
                    <p>All changes has been updated!</p>
                </div>
                <div>
                    <!--OK BTN-->
                    <asp:Button runat="server" ID="BtnOK" Text="OK" CssClass="btn-custom" ForeColor="White" BackColor="#0A9E00" />
                </div>
            </div>
        </asp:Panel>
        <!--MODUL POPUP: DELETE POPUP MESSAGE-->
        <div style="display:none">
            <asp:Button runat="server" ID="BtnPopupDelete" Text="Open Popup" CssClass="btn-custom mt-2" Width="100%" ForeColor="White" BackColor="#0066ff" />
        </div>
        <ajaxStaffInfo:ModalPopupExtender runat="server" ID="ModalPopupMessageDelete" PopupControlID="PanelPopupDelete" TargetControlID="BtnPopupDelete" CancelControlID="BtnCancel" BackgroundCssClass="Background">
        </ajaxStaffInfo:ModalPopupExtender>
        <asp:Panel runat="server" ID="PanelPopupDelete" CssClass="Popup" Width="600px" Style="display: none">
            <div class="card">
                <div class="row">
                    <div class="col align-self-center">
                        <span>System Message</span>
                    </div>
                    <div class="col align-self-end">
                    </div>
                </div>
            </div>
            <div class="card-body text-center">
                <div class="row">
                    <p>Are you sure want to <b>DELETE</b> this staff from the system?</p>
                </div>
                <div>
                    <!--CONFIRM BTN-->
                    <asp:Button runat="server" ID="BtnConfirm" Text="Confirm" OnClick="DeleteFromTisma" CssClass="btn-custom mt-1 mb-2" ForeColor="White" BackColor="#ff0000"></asp:Button>
                    <!--CANCEL BTN-->
                    <asp:Button runat="server" ID="BtnCancel" Text="Cancel" CssClass="btn-custom" ForeColor="White" BackColor="#0A9E00" />
                </div>
            </div>
        </asp:Panel>
        <!--MODUL POPUP: VALIDATION POPUP MESSAGE-->
        <div style="display:none">
            <asp:Button runat="server" ID="BtnPopupValidation" Text="Open Popup" CssClass="btn-custom mt-2" Width="100%" ForeColor="White" BackColor="#0066ff" />
        </div>
        <ajaxStaffInfo:ModalPopupExtender runat="server" ID="ModalPopupMessageValidation" PopupControlID="PanelPopupValidation" TargetControlID="BtnPopupValidation" CancelControlID="BtnRetry" BackgroundCssClass="Background">
        </ajaxStaffInfo:ModalPopupExtender>
        <asp:Panel runat="server" ID="PanelPopupValidation" CssClass="Popup" Width="600px" Style="display: none">
            <div class="card">
                <div class="row">
                    <div class="col align-self-center">
                        <span>System Message</span>
                    </div>
                    <div class="col align-self-end">
                    </div>
                </div>
            </div>
            <div class="card-body text-center">
                <div class="row">
                    <p>All changes FAILED to update.</p><br />
                    <p><asp:Literal runat="server" ID="getValidationMsg"></asp:Literal></p>
                </div>
                <div>
                    <!--CANCEL BTN-->
                    <asp:Button runat="server" ID="BtnRetry" Text="Retry" CssClass="btn-custom" ForeColor="White" BackColor="#0A9E00" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
