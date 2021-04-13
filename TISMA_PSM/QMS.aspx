<%@ Page Title="QMS :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="QMS.aspx.cs" Inherits="TISMA_PSM.QMS" %>

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
                    <span>Instruction: Key-in the last 3 numbers</span>
                </div>
            </div>
            <div class="card-body">
                <!--QUEUE NO.: Date-->
                <input readonly value="20210411" style="width: 6rem;" />
                <!--QUEUE NO.: Number-->
                <input autofocus required placeholder="Your number here" maxlength="3" minlength="3" style="width: 10rem;" />
                <!--SUBMIT BTN-->
                <button type="submit">Enter</button>
                <!--TABLE-->
                <div class="mt-3">
                    <table id="qms-table" class="cell-border hover order-column mt-2" style="width: 100%;">
                        <thead>
                            <tr>
                                <th>No.</th>
                                <th>Queue No.</th>
                                <th>Account No.</th>
                                <th>Name</th>
                                <th>Category</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('#qms-table').DataTable();
        });
    </script>
</asp:Content>
