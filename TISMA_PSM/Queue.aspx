<%@ Page Title="Queue List :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Queue.aspx.cs" Inherits="TISMA_PSM.Queue" %>

<asp:Content ID="Queue" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" style="padding: 1.5rem 1rem 1.5rem 1rem" id="q-list">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row" style="padding-left: 0; padding-right: 0;">
                <div class="col align-self-start">
                    <h5>Queue List</h5>
                </div>
                <div class="col align-self-end">
                    <div class="float-end " style="font-size: 12px; padding-bottom: 0.5rem;">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;></b>&nbsp;&nbsp;Queue List</span>
                    </div>
                </div>
            </div>
        </div>
        <!--CONTENT-->
        <div class="row justify-content-center mt-1" id="q-list-content">
            <!--HEADER-->
            <div class="card border-0">
                <div class="col">
                    <span>Today Registered List</span>
                </div>
            </div>
            <div class="card-body">
                <!--REFRESH BUTTON-->
                <button id="btn-refresh">
                    Refresh
                </button>
                <!--TABLE-->
                <div class="mt-3">
                    <table id="q-list-table" class="cell-border hover order-column mt-2" style="width: 100%;">
                        <thead>
                            <tr>
                                <th>No.</th>
                                <th>Queue No.</th>
                                <th>Account No.</th>
                                <th>Name</th>
                                <th>Category</th>
                                <th>Entered at</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>1.</td>
                                <td>000</td>
                                <td>123</td>
                                <td>Patient_0</td>
                                <td>Student</td>
                                <td>10/04/2021</td>
                            </tr>
                            <tr>
                                <td>2.</td>
                                <td>001</td>
                                <td>124</td>
                                <td>Patient_1</td>
                                <td>Student</td>
                                <td>10/04/2021</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('#q-list-table').DataTable();

            $("#refresh-btn").on("click", function () {
                $("#q-list-table").load("Queue-List.aspx #q-list-table");
            });
        });
    </script>
</asp:Content>
