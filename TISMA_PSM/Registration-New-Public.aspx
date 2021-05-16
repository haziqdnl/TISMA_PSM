﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Registration-New-Public.aspx.cs" Inherits="TISMA_PSM.Registration_New_Public" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxTab" %>

<asp:Content ID="RegistrationNewPublic" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid content-p-custom" id="registration-new">
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
                        <i class="fas fa-clipboard-list me-1"></i>
                        <span>New Public Patient Registration</span>
                    </div>
                    <div class="col align-self-end">
                        <div class="float-end">
                            <asp:LinkButton runat="server" PostBackUrl="~/Registration.aspx">
                        <i class="fas fa-search me-1"></i>
                        <span>Search Registered Patient</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-3" id="registration-new-content-2">
            <div class="col-2 p-0 m-0 me-4">
                <div class="card">
                    <div class="row justify-content-center card-body">
                        <img src="https://iupac.org/wp-content/uploads/2018/05/default-avatar.png" />
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
                                    <h6>The basic information is registered as public</h6>
                                </div>
                                <div class="row m-0 card-body justify-content-center">
                                    <table id="register-form-table" style="text-align: left;">
                                        <tr>
                                            <!--Branch-->
                                            <td class="pe-3" style="text-align: right">Branch <span style="color:red">*</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getBranchDdl" CssClass="dropdown-custom" BackColor="White" AppendDataBoundItems="true">
                                                    <asp:ListItem Text="-Select-" Value="Select" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="UTM-JB" Value="UTM-JB"></asp:ListItem>
                                                    <asp:ListItem Text="UTM-KL" Value="UTM-KL"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="getBranchDdl" 
                                                    InitialValue="Select" 
                                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                            </td>
                                            <!--Category-->
                                            <td class="pe-3" style="text-align: right">Category</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getCategory" Text="Public" Value="Public" Disabled="true" CssClass="textbox-custom disabled-textbox" Width="100px" ></asp:TextBox>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Account No.-->
                                            <td class="pe-3" style="text-align: right">Account No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAccNo" Disabled="true" CssClass="textbox-custom disabled-textbox" ></asp:TextBox>
                                                <p class="fas fa-exclamation-circle" style="font-size:10px; color: green">&nbsp Auto-generated by the system</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Name-->
                                            <td class="pe-3" style="text-align: right">Name <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getName" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="80px" required="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--IC No.-->
                                            <td class="pe-3" style="text-align: right">IC No. <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getIcNo" CssClass="textbox-custom" Width="140px" required="true"></asp:TextBox>
                                                <p class="fas fa-question-circle" style="font-size:10px; color: green">&nbsp without ' - '</p>
                                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="getIcNo" 
                                                    ValidationExpression="^[A-Za-z0-9]*$" Display="Dynamic" SetFocusOnError="true"
                                                    ErrorMessage="!!!"  ForeColor="Red" Font-Size="12px" Font-Bold="true">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                            <!--Passport No.-->
                                            <td class="pe-3" style="text-align: right">Passport No.</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getPassportNo" CssClass="textbox-custom" Width="140px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--DOB-->
                                            <td class="pe-3" style="text-align: right">DOB <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getDob" CssClass="textbox-custom" TextMode="Date" Width="160px" required="true"></asp:TextBox>
                                            </td>
                                            <!--Age-->
                                            <td class="pe-3" style="text-align: right">Age</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAge" Disabled="true" CssClass="textbox-custom disabled-textbox" Width="40px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Gender-->
                                            <td class="pe-3" style="text-align: right">Gender <span style="color:red">*</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getGenderDdl" CssClass="dropdown-custom" BackColor="White" Width="120px">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="getGenderDdl" 
                                                    InitialValue="Select" 
                                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                            </td>
                                            <!--Marital Status-->
                                            <td class="pe-3" style="text-align: right">Marital Status <span style="color:red">*</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getMaritalStatDdl" CssClass="dropdown-custom" BackColor="White" Width="140px">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Single" Value="Single"></asp:ListItem>
                                                    <asp:ListItem Text="Married" Value="Married"></asp:ListItem>
                                                    <asp:ListItem Text="Divorced " Value="Divorced"></asp:ListItem>
                                                    <asp:ListItem Text="Widowed " Value="Widowed"></asp:ListItem>
                                                    <asp:ListItem Text="Separated " Value="Separated"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="getMaritalStatDdl" 
                                                    InitialValue="Select" 
                                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Religion-->
                                            <td class="pe-3" style="text-align: right">Religion <span style="color:red">*</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getReligionDdl" CssClass="dropdown-custom" BackColor="White" Width="120px">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Muslim" Value="Muslim"></asp:ListItem>
                                                    <asp:ListItem Text="Buddha" Value="Buddha"></asp:ListItem>
                                                    <asp:ListItem Text="Christian" Value="Christian"></asp:ListItem>
                                                    <asp:ListItem Text="Hindu" Value="Hindu"></asp:ListItem>
                                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="getReligionDdl" 
                                                    InitialValue="Select" 
                                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                            </td>
                                            <!--Race-->
                                            <td class="pe-3" style="text-align: right">Race <span style="color:red">*</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getRaceDdl" CssClass="dropdown-custom" BackColor="White" Width="180px">
                                                    <asp:ListItem Text="-Select-" Value="Select"></asp:ListItem>
                                                    <asp:ListItem Text="Malay/Bumiputera" Value="Malay/Bumiputera"></asp:ListItem>
                                                    <asp:ListItem Text="Chinese" Value="Chinese"></asp:ListItem>
                                                    <asp:ListItem Text="Indian" Value="Indian"></asp:ListItem>
                                                    <asp:ListItem Text="Non-Malaysian" Value="Non-Malaysian"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="getRaceDdl" 
                                                    InitialValue="Select" 
                                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Nationality-->
                                            <td class="pe-3" style="text-align: right">Nationality <span style="color:red">*</span></td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="getNationDdl" CssClass="dropdown-custom" BackColor="White" Width="200px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="getNationDdl" 
                                                    InitialValue="-Select-" 
                                                    ErrorMessage="Please select" ForeColor="Red" Font-Size="10px"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Phone No.-->
                                            <td class="pe-3" style="text-align: right">Phone No. <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getPhone" CssClass="textbox-custom" required="true" Width="140px"></asp:TextBox>
                                                <p class="fas fa-question-circle" style="font-size:10px; color: green">&nbsp with ' - '</p>
                                            </td>
                                            <!--Email-->
                                            <td class="pe-3" style="text-align: right">Email <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getEmail" CssClass="textbox-custom" required="true" Width="250px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Designation-->
                                            <td class="pe-3" style="text-align: right">Occupation <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getDesignation" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="50px" required="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--Address-->
                                            <td class="pe-3" style="text-align: right">Home Address <span style="color:red">*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getAddress" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="100px" required="true"></asp:TextBox>
                                            </td>
                                            <!--Remarks-->
                                            <td class="pe-3" style="text-align: right">Remarks</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="getRemarks" CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:Button runat="server" ID="BtnAddtoTISMA" Text="Add to TISMA" OnClick="AddToTisma" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#00cc00" />
                                </div>
                            </ContentTemplate>
                        </ajaxTab:TabPanel>
                    </ajaxTab:TabContainer>
                </div>
            </div>
        </div>
    </div>
    <script type='text/javascript'>
        $(function(){
            // When your textbox is changed (i.e. a date of birth is set)
            $("#<%= getDob.ClientID %>").change(function(){
                $("#<%= getAge.ClientID %>").val(_calculateAge(new Date($(this).val())));
            });
        });

        // Define a function to calculate age via a birthdate (http://stackoverflow.com/a/21984136/557445)
        function _calculateAge(birthday) { // birthday is a date
            var ageDifMs = Date.now() - birthday.getTime();
            var ageDate = new Date(ageDifMs); // miliseconds from epoch
            return Math.abs(ageDate.getUTCFullYear() - 1970);
        }
    </script>
</asp:Content>
