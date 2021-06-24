<%@ Page Title="OPD :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Consultation.aspx.cs" Inherits="TISMA_PSM.Consultation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxOpd" %>

<asp:Content ID="Consultation" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid content-p-custom" id="consult">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row header-p-custom">
                <div class="col align-self-start">
                    <span style="font-size: 19px; font-weight: 600;">Module OPD</span>&nbsp<span>Out Patient Department</span>
                </div>
                <div class="col align-self-end">
                    <div class="float-end subheader-custom">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;>&nbsp;&nbsp;Modules&nbsp;&nbsp;></b>&nbsp;&nbsp;OPD</span>
                    </div>
                </div>
            </div>
        </div>
        <!--CONTENT 1-->
        <div class="row justify-content-center mt-1" id="consult-content-1">
            <!--HEADER-->
            <div class="card border-0">
                <div class="row">
                    <div>
                        <i class="fas fa-check-double me-1"></i>
                        <span>Patient Check-Up and Consultation</span>
                    </div>
                </div>
            </div>
        </div>
        <!--CONTENT 2-->
        <div class="row mt-3" id="consult-content-2">
            <div class="card p-0 m-0">
                <div class="row align-self-center w-100 p-4" style="height: 300px">
                    <!--PATIENT BRIEF INFO-->
                    <table id="patient-info-table" class="text-start">
                        <tr class="text-center">
                            <!--Profile Picture, Account No., Category-->
                            <td rowspan="6" style="width: 15%">
                                <img src="https://iupac.org/wp-content/uploads/2018/05/default-avatar.png" style="border: 1px solid" /><br />
                                <br />
                                <h6><b>
                                    <asp:Literal runat="server" ID="getAccNo"></asp:Literal></b></h6>
                                <p style="font-size: 12px; font-weight: 500">
                                    <asp:Literal runat="server" ID="getCategory"></asp:Literal>
                                </p>
                            </td>
                            <!--Name-->
                            <td class="text-end" style="width: 150px">
                                <p>Name&nbsp&nbsp:</p>
                            </td>
                            <td>
                                <p class="w-auto text-start" style="font-weight: 500">
                                    <b>
                                        <asp:Literal runat="server" ID="getName"></asp:Literal></b>
                                </p>
                            </td>
                            <!--Queue No.-->
                            <td class="text-center border-custom" rowspan="6" style="width: 20%">
                                <h6>Queue No.</h6>
                                <p style="font-size: 50px; font-weight: bold">
                                    <asp:Literal runat="server" ID="getQueueNo"></asp:Literal>
                                </p>
                                <asp:Button runat="server" ID="BtnCheckOut" Text="Check-Out" OnClick="CheckOut" Width="120px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#ff0000" />
                            </td>
                        </tr>
                        <tr>
                            <!--DOB-->
                            <td class="text-end" style="width: 150px">
                                <p>Date of Birth&nbsp&nbsp:</p>
                            </td>
                            <td>
                                <p class="w-auto" style="font-weight: 500">
                                    <b>
                                        <asp:Literal runat="server" ID="getDOB"></asp:Literal></b>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <!--Age-->
                            <td class="text-end" style="width: 150px">
                                <p>Age&nbsp&nbsp:</p>
                            </td>
                            <td>
                                <p class="w-auto" style="font-weight: 500">
                                    <b>
                                        <asp:Literal runat="server" ID="getAge"></asp:Literal>&nbsp y/o</b>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <!--Matric/Staff No.-->
                            <td class="text-end" style="width: 150px">
                                <p>
                                    <asp:Literal runat="server" ID="getMatricStaffHeader"></asp:Literal>&nbsp&nbsp:
                                </p>
                            </td>
                            <td>
                                <p class="w-auto" style="font-weight: 500">
                                    <b>
                                        <asp:Literal runat="server" ID="getMatricStaff"></asp:Literal></b>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <!--Faculty/Department-->
                            <td class="text-end" style="width: 150px">
                                <p>
                                    <asp:Literal runat="server" ID="getFacDepHeader"></asp:Literal>&nbsp&nbsp:
                                </p>
                            </td>
                            <td>
                                <p class="w-auto" style="font-weight: 500">
                                    <b>
                                        <asp:Literal runat="server" ID="getFacDep"></asp:Literal></b>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <!--Remarks-->
                            <td class="text-end" style="width: 150px">
                                <p>Remarks&nbsp&nbsp:</p>
                            </td>
                            <td>
                                <p class="w-auto" style="font-weight: 500">
                                    <b>
                                        <asp:Literal runat="server" ID="getRemark"></asp:Literal></b>
                                </p>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <!--CONTENT 3-->
        <div class="row mt-3 mb-3" id="consult-content-3">
            <div class="col p-0 m-0">
                <div class="card p-0">
                    <ajaxOpd:TabContainer runat="server" ID="TabContainer1" ActiveTabIndex="0" BackColor="#0072c6">
                        <ajaxOpd:TabPanel runat="server" ID="TabPanel1" HeaderText="Clinical Info" BorderStyle="None">
                            <ContentTemplate>
                                <div runat="server" id="NoteClinical" class="row m-0 card-body pt-2 pb-1 ps-1 pe-1 note">
                                    <h5><b>Note</b></h5>
                                    <h6>Clinical Information for this patient has been submitted/updated.</h6>
                                </div>
                                <div class="card border-0 p-5" style="background-color: aliceblue">
                                    <table id="clinical-info-table">
                                        <tr>
                                            <td class="text-start">
                                                <input class="clinical-sign" value="Symptom" disabled>
                                                <ajaxOpd:ComboBox runat="server" ID="SymptomDdl" CssClass="combobox-custom" AppendDataBoundItems="True" Width="330px" Height="30px" AutoCompleteMode="Suggest" MaxLength="50">
                                                </ajaxOpd:ComboBox>
                                                <asp:Button runat="server" ID="BtnAdd1" Text="Add" OnClick="AddItemSymptom" Width="60px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#0066ff" />
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="SymptomDdl" ValidationGroup="TabGroup1"
                                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td class="text-start">
                                                <input class="clinical-sign" value="Sign" disabled>
                                                <ajaxOpd:ComboBox runat="server" ID="SignDdl" CssClass="combobox-custom" AppendDataBoundItems="True" Width="330px" Height="30px" AutoCompleteMode="Suggest" MaxLength="50">
                                                </ajaxOpd:ComboBox>
                                                <asp:Button runat="server" ID="BtnAdd2" Text="Add" OnClick="AddItemSign" Width="60px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#0066ff" />
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="SignDdl" ValidationGroup="TabGroup1"
                                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="text-start">
                                                <asp:TextBox runat="server" ID="getSymptomDesc" placeholder="Description..." CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="560px" Height="80px"></asp:TextBox>
                                            </td>
                                            <td class="text-start">
                                                <asp:TextBox runat="server" ID="getSignDesc" placeholder="Description..." CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="560px" Height="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="text-start">
                                                <input class="clinical-sign" value="Diagnosis" disabled>
                                                <ajaxOpd:ComboBox runat="server" ID="DiagnosisDdl" CssClass="combobox-custom" AppendDataBoundItems="True" Width="330px" Height="30px" AutoCompleteMode="Suggest" MaxLength="50">
                                                </ajaxOpd:ComboBox>
                                                <asp:Button runat="server" ID="BtnAdd3" Text="Add" OnClick="AddItemDiagnosis" Width="60px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#0066ff" />
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="DiagnosisDdl" ValidationGroup="TabGroup1"
                                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="text-start">
                                                <asp:TextBox runat="server" ID="getDiagnosisDesc" placeholder="Description..." CssClass="textbox-custom pt-1" TextMode="MultiLine" Width="560px" Height="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="m-2 p-2"></td>
                                        </tr>
                                        <tr>
                                            <td class="text-center" colspan="2">
                                                <asp:Button runat="server" ID="BtnReset" Text="Reset" OnClick="ResetClinicalInfo" Width="80px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#ff0000" />
                                                <asp:Button runat="server" ID="BtnSubmit" Text="Submit" OnClick="SubmitClinicalInfo" ValidationGroup="TabGroup1" Width="80px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#00cc00" />
                                                <asp:Button runat="server" ID="BtnUpdate" Text="Update" OnClick="UpdateClinicalInfo" ValidationGroup="TabGroup1" Width="80px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#00cc00" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </ajaxOpd:TabPanel>
                        <ajaxOpd:TabPanel runat="server" ID="TabPanel2" HeaderText="Electronic Medical Certificate" BorderStyle="None">
                            <ContentTemplate>
                                <div runat="server" id="NoteEMC" class="row m-0 card-body pt-2 pb-1 ps-1 pe-1 note">
                                    <h5><b>Note</b></h5>
                                    <h6>Electronic Medical Certificate for this patient has been generated/updated with the following dates and period.</h6>
                                </div>
                                <div class="card border-0 p-4" style="background-color: aliceblue">
                                    <table id="emc-info-table">
                                        <tr>
                                            <td class="text-end">Period:</td>
                                            <td class="text-start">&nbsp<asp:TextBox runat="server" ID="getPeriod" CssClass="textbox-custom text-center" ReadOnly="true" Enabled="false" Width="60px"></asp:TextBox>day(s)
                                            </td>
                                            <td class="text-end">From &nbsp<asp:TextBox runat="server" ID="getDateFrom" CssClass="textbox-custom" TextMode="Date"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="getDateFrom" ValidationGroup="TabGroup2"
                                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td class="text-start">&nbsp to &nbsp<asp:TextBox runat="server" ID="getDateTo" CssClass="textbox-custom" TextMode="Date"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="getDateTo" ValidationGroup="TabGroup2"
                                                    ErrorMessage="Required" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label runat="server" ID="ErrorDate" Visible="false" Text="Invalid" Font-Size="10px" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="m-2 p-2"></td>
                                        </tr>
                                        <tr>
                                            <td class="text-end" style="vertical-align: top;">Comment:</td>
                                            <td class="text-start" colspan="2">
                                                <asp:TextBox runat="server" ID="getComment" CssClass="textbox-custom pt-1" TextMode="MultiLine" Height="80px" Width="100%"
                                                    Placeholder="mengesahkan beliau tidak sihat untuk bertugas / belajar"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="MCPassword" visible="false">
                                            <td class="text-end">e-MC Password:</td>
                                            <td class="text-start" colspan="2">
                                                <asp:TextBox runat="server" ID="getPassword" CssClass="textbox-custom text-center" ReadOnly="true" Enabled="false" Width="120px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="m-3 p-3"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Button runat="server" ID="BtnGenerateMC" Text="Generate e-MC" OnClick="GenerateEMC" ValidationGroup="TabGroup2" Width="150px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#00cc00" />
                                                <asp:Button runat="server" ID="BtnUpdateMC" Text="Update e-MC" OnClick="UpdateEMCInfo" Width="150px" ValidationGroup="TabGroup2" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#00cc00" />
                                                <asp:Button runat="server" ID="BtnSendMC" Text="Send e-MC" OnClick="SendEMC" Width="150px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#ff0000" />
                                                <asp:Button runat="server" ID="BtnViewMC" Text="View e-MC" OnClick="ViewEMC" Width="150px" CssClass="btn-custom mt-2" ForeColor="White" BackColor="#999999" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </ajaxOpd:TabPanel>
                        <ajaxOpd:TabPanel runat="server" ID="TabPanel3" HeaderText="Clinical History" BorderStyle="None">
                            <ContentTemplate>
                                <div class="mt-4">
                                    <asp:GridView runat="server" ID="ClinicalHistoryTable" CssClass="display compact cell-border" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>No.</HeaderTemplate>
                                                <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="clinical_date" HeaderText="Date" SortExpression="clinical_date" />
                                            <asp:BoundField DataField="symptom" HeaderText="Diagnose" SortExpression="symptom" />
                                            <asp:BoundField DataField="ill_sign" HeaderText="Sign" SortExpression="ill_sign" />
                                            <asp:BoundField DataField="diagnosis" HeaderText="Diagnosis" SortExpression="diagnosis" />
                                            <asp:BoundField DataField="s_name" HeaderText="Doctor/PIC" SortExpression="s_name" />
                                        </Columns>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </ajaxOpd:TabPanel>
                        <ajaxOpd:TabPanel runat="server" ID="TabPanel4" HeaderText="e-MC History" BorderStyle="None">
                            <ContentTemplate>
                                <div class="mt-4">
                                    <asp:GridView runat="server" ID="MCHistoryTable" CssClass="display compact cell-border" OnRowDataBound="MCHistoryTable_RowDataBound" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>No.</HeaderTemplate>
                                                <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="date_created" HeaderText="Date & Time Created" SortExpression="date_created" />
                                            <asp:TemplateField HeaderText="e-MC" >
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server"
                                                        Text="View" CssClass="EMCViewBtn"
                                                        NavigateUrl='<%# base.ResolveUrl("~/e-MC.aspx?token=" + (String)Eval("url_hashed")) %>'>
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="emc_password" HeaderText="Password" SortExpression="emc_password" />
                                        </Columns>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </ajaxOpd:TabPanel>
                    </ajaxOpd:TabContainer>
                </div>
            </div>
        </div>

        <!--MODUL POPUP: POPUP MESSAGE-->
        <div style="display: none">
            <asp:Button runat="server" ID="BtnPopup" Text="Open Popup" CssClass="btn-custom mt-2" Width="100%" ForeColor="White" BackColor="#0066ff" />
        </div>
        <ajaxOpd:ModalPopupExtender runat="server" ID="ModalPopupMessage" PopupControlID="PanelPopup" TargetControlID="BtnPopup" CancelControlID="BtnOK" BackgroundCssClass="Background">
        </ajaxOpd:ModalPopupExtender>
        <asp:Panel runat="server" ID="PanelPopup" CssClass="Popup" Width="600px" Style="display: none">
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
                    <p>
                        <asp:Literal runat="server" ID="getMessage"></asp:Literal>
                    </p>
                </div>
                <div>
                    <!--OK BTN-->
                    <asp:Button runat="server" ID="BtnOK" Text="OK" CssClass="btn-custom" ForeColor="White" BackColor="#0A9E00" />
                </div>
            </div>
        </asp:Panel>
        <!--MODUL POPUP: CHECK-OUT MESSAGE-->        
        <div style="display: none">
            <asp:Button runat="server" ID="BtnPopupCheckOut" Text="Open Popup" CssClass="btn-custom mt-2" Width="100%" ForeColor="White" BackColor="#0066ff" />
        </div>
        <ajaxOpd:ModalPopupExtender runat="server" ID="ModalPopupCheckOut" PopupControlID="PanelPopupCheckOut" TargetControlID="BtnPopupCheckOut" CancelControlID="BtnCancel" BackgroundCssClass="Background">
        </ajaxOpd:ModalPopupExtender>
        <asp:Panel runat="server" ID="PanelPopupCheckOut" CssClass="Popup" Width="600px" Style="display: none">
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
                    <p>Are you sure want to <b>CHECK-OUT</b> this patient?</p>
                    <p runat="server" id="eMCNotGeneratedMsg"><b>The e-MC for this patient is not yet generated. Please ask them if they requires it.</b></p>
                </div>
                <div>
                    <!--CONFIRM BTN-->
                    <asp:Button runat="server" ID="BtnConfirm" Text="Confirm Check-Out" OnClick="CheckOutConfirm" CssClass="btn-custom mt-1 mb-2" ForeColor="White" BackColor="#ff0000"></asp:Button>
                    <!--CANCEL BTN-->
                    <asp:Button runat="server" ID="BtnCancel" Text="Cancel" CssClass="btn-custom" ForeColor="White" BackColor="#0A9E00" />
                </div>
            </div>
        </asp:Panel>
        <script type="text/javascript">
            $(function () {
                $("[id*=ClinicalHistoryTable]").DataTable({
                    lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                    language: {
                        searchPlaceholder: "",
                        search: "Search",
                    },
                });
                $("[id*=MCHistoryTable]").DataTable({
                    lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                    language: {
                        searchPlaceholder: "",
                        search: "Search",
                    },
                });
            });

            $(function () {
                $("#<%= getDateFrom.ClientID %>, #<%= getDateTo.ClientID %>").change(function () {
                    $("#<%= getPeriod.ClientID %>").val(_calculatePeriod());
                });
            });
            function _calculatePeriod() {
                var date1 = new Date(document.getElementById("<%= getDateFrom.ClientID %>").value);
                var date2 = new Date(document.getElementById("<%= getDateTo.ClientID %>").value);
                var TimeDiff = date2.getTime() - date1.getTime();
                var DayDiff = 1 + Math.round(TimeDiff / (1000 * 3600 * 24));
                return DayDiff;
            }
        </script>
    </div>
</asp:Content>
