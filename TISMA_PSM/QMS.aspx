<%@ Page Title="QMS :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="QMS.aspx.cs" Inherits="TISMA_PSM.QMS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxQms" %>

<asp:Content ID="QMS" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" style="padding: 1.5rem 1rem 1.5rem 1rem" id="qms">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row" style="padding-left: 0; padding-right: 0;">
                <div class="col align-self-start">
                    <h5>Module QMS</h5>
                </div>
                <div class="col align-self-end">
                    <div class="float-end " style="font-size: 12px; padding-bottom: 0.5rem;">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;>&nbsp;&nbsp;Modules&nbsp;&nbsp;></b>&nbsp;&nbsp;QMS</span>
                    </div>
                </div>
            </div>
        </div>
        <!--CONTENT 1-->
        <div class="row justify-content-center mt-1" id="qms-content-1">
            <!--HEADER-->
            <div class="card border-0">
                <div class="col">
                    <i class="fas fa-sign me-1"></i>
                    <span>Station</span>
                </div>
            </div>
            <div class="card-body text-center">
                <h3>MEDICAL CONSULTATION</h3>
            </div>
        </div>
        <!--CONTENT 2-->
        <div class="row justify-content-center mt-4" id="qms-content-2">
            <!--INSTRUCTION-->
            <div class="card border-0">
                <div class="col">
                    <span>Instruction: Key-in the 3 numbers, Ex: ' 001 '</span>
                </div>
            </div>
            <div class="card-body">
                <!--QUEUE NO.: Date-->
                <asp:TextBox runat="server" ID="getTodayDate" ReadOnly="true" Enabled="false" CssClass="textbox-custom" Width="100px" BackColor="White"></asp:TextBox>
                <!--QUEUE NO.: Number-->
                <asp:TextBox runat="server" ID="getQueueNo" AutoFocus="true" CssClass="textbox-custom" Placeholder="Your number here" Width="150px" BackColor="White" Required="true"></asp:TextBox>
                <!--SUBMIT BTN-->
                <asp:Button runat="server" ID="BtnEnter" Text="Enter" OnClick="KeyInQueue" CssClass="btn-custom mt-2" BorderStyle="None" Font-Size="15px" ForeColor="White" BackColor="#0068b4"
                    Style="padding: 0.3rem 1rem 0.3rem 1rem" />
                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="getQueueNo"
                    ValidationExpression="^[0-9]{3}$" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please enter the correct format" ForeColor="Red" Font-Size="10px">
                </asp:RegularExpressionValidator>
                <!--TABLE-->
                <div class="mt-3">
                    <asp:GridView runat="server" ID="QmsTable" CssClass="display compact cell-border" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>No.</HeaderTemplate>
                                <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="queue_no" HeaderText="Queue No." SortExpression="queue_no" />
                            <asp:BoundField DataField="p_account_no" HeaderText="Account No." SortExpression="p_account_no" />
                            <asp:BoundField DataField="p_name" HeaderText="Name" SortExpression="p_name" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="p_category" HeaderText="Category" SortExpression="p_category" />
                            <asp:BoundField DataField="p_remarks" HeaderText="Remarks" SortExpression="p_remarks" />
                        </Columns>
                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!--MODUL POPUP: POPUP MESSAGE-->
        <div style="display:none">
            <asp:Button runat="server" ID="BtnPopup" Text="Open Popup" CssClass="btn-custom mt-2" Width="100%" ForeColor="White" BackColor="#0066ff" />
        </div>
        <ajaxQms:ModalPopupExtender runat="server" ID="ModalPopupMessage" PopupControlID="PanelPopup" TargetControlID="BtnPopup" CancelControlID="BtnCancel" BackgroundCssClass="Background">
        </ajaxQms:ModalPopupExtender>
        <asp:Panel runat="server" ID="PanelPopup" CssClass="Popup" Width="600px" Style="display: none">
            <div class="card">
                <div class="row">
                    <div class="col align-self-center">
                        <span><asp:Literal runat="server" ID="getPopupTitle"></asp:Literal></span>
                    </div>
                    <div class="col align-self-end">
                        <div class="float-end">
                            <!--CANCEL BTN-->
                            <asp:Button runat="server" ID="BtnCancel" Font-Size="Larger" Text="X" Font-Bold="true" BorderStyle="None" CssClass="btn-custom" ForeColor="#808080" BackColor="White" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-body text-center">
                <p><asp:Literal runat="server" ID="getPopupMessage"></asp:Literal></p>
            </div>
        </asp:Panel>
    </div>
    <script>
        $(function () {
            $("[id*=QmsTable]").DataTable({
                lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                language: {
                    searchPlaceholder: "Search queue no...",
                    search: "",
                },
            });
        });
    </script>
</asp:Content>
