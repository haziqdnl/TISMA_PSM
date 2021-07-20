<%@ Page Title="Statistic :: TISMA" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Statistic.aspx.cs" Inherits="TISMA_PSM.Statistic" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxStatistics" %>

<asp:Content ID="Statistic" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid" style="padding: 1.5rem 1rem 1.5rem 1rem" id="statistic">
        <!--TITLE SECTION-->
        <div class="row justify-content-center">
            <div class="row" style="padding-left: 0; padding-right: 0;">
                <div class="col align-self-start">
                    <h5><span class="fa fa-chart-area me-2"></span>Data Statistics and Reporting ( Session <%: DateTime.Now.Year %> )</h5>
                </div>
                <div class="col align-self-end">
                    <div class="float-end " style="font-size: 12px; padding-bottom: 0.5rem;">
                        <i class="fas fa-home me-1"></i>
                        <span><b>Dashboard&nbsp;&nbsp;></b>&nbsp;&nbsp;Statistic</span>
                    </div>
                </div>
            </div>
        </div>
        <!--CONTENT-->
        <div class="row justify-content-center mt-1" id="statistic-content">
            <div class="card border-0">
                <ajaxStatistics:TabContainer runat="server" ID="TabContainer1" ActiveTabIndex="0" BackColor="#0072c6" ScrollBars="Auto">
                    <ajaxStatistics:TabPanel runat="server" ID="TabPanel1" HeaderText="Infographic of e-MC" BorderStyle="None" ScrollBars="Auto">
                        <ContentTemplate>
                            <!--DATA STATISTIC AND ANALYSIS-->
                            <div id="EmcChart" class="card-body text-center">
                                <!--DATA SUMMARY-->
                                <div class="mt-4">
                                    <h4>DATA STATISTIC AND ANALYSIS FOR e-MC</h4>
                                </div>
                                <br />
                                <div id="EmcStat" class="row justify-content-center mt-4">
                                    <div id="EmcPie1" class="ms-auto" style="background-color: #6abec7">
                                        <h1>
                                            <asp:Literal runat="server" ID="getTotalEmc"></asp:Literal></h1>
                                        <p>Total Generated</p>
                                    </div>
                                    <div id="EmcPie2" style="background-color: #c47ee2">
                                        <h1>
                                            <asp:Literal runat="server" ID="getReleasedPerVisit"></asp:Literal></h1>
                                        <p>Percentage per Visit</p>
                                    </div>
                                    <div id="EmcPie3" style="background-color: #f7b15b">
                                        <h1>
                                            <asp:Literal runat="server" ID="getAveEmcPerMonth"></asp:Literal></h1>
                                        <p>Average per Month</p>
                                    </div>
                                    <div id="EmcPie4" class="me-auto" style="background-color: #d07070">
                                        <h1>
                                            <asp:Literal runat="server" ID="getAveEmcPerDay"></asp:Literal></h1>
                                        <p>Average per Day</p>
                                    </div>
                                </div>
                                <br />
                                <br />
                                <br />
                                <!--CHARTS-->
                                <div id="EmcMonthly">
                                    <asp:Chart ID="ChartEmcMonthly" runat="server" Width="800px" Height="300px" BackColor="White" CssClass="ChartBackground">
                                        <Series>
                                            <asp:Series Name="SeriesEmcMonthly" ChartType="Line" Color="#5c001f" BorderWidth="5" YValuesPerPoint="1"></asp:Series>
                                        </Series>
                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartAreaEmcMonthly" Area3DStyle-Enable3D="false" BackColor="#fff"></asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>
                                </div>
                                <br />
                                <br />
                                <div id="EmcDaily" class="mb-5">
                                    <asp:Chart ID="ChartEmcDaily" runat="server" Width="800px" Height="300px" BackColor="White" CssClass="ChartBackground">
                                        <Series>
                                            <asp:Series Name="SeriesEmcDaily" ChartType="Line" Color="#5c001f" BorderWidth="5" YValuesPerPoint="1"></asp:Series>
                                        </Series>
                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartAreaEmcDaily" Area3DStyle-Enable3D="false" BackColor="#fff"></asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>
                                </div>
                            </div>
                        </ContentTemplate>
                    </ajaxStatistics:TabPanel>
                    <ajaxStatistics:TabPanel runat="server" ID="TabPanel2" HeaderText="List of e-MC" BorderStyle="None">
                        <ContentTemplate>
                            <div class="mt-4">
                                <asp:GridView runat="server" ID="MCHistoryTable" CssClass="display compact cell-border" OnRowDataBound="MCHistoryTable_RowDataBound" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" Width="100%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>No.</HeaderTemplate>
                                            <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="serial_no" HeaderText="Serial No." SortExpression="serial_no" />
                                        <asp:BoundField DataField="date_created" HeaderText="Date & Time Created" SortExpression="date_created" />
                                        <asp:BoundField DataField="emc_period" HeaderText="Period of MC" SortExpression="emc_period" />
                                        <asp:BoundField DataField="date_from" HeaderText="Date From" SortExpression="date_from" />
                                        <asp:BoundField DataField="date_to" HeaderText="Date To" SortExpression="date_to" />
                                        <asp:TemplateField HeaderText="e-MC Link">
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
                    </ajaxStatistics:TabPanel>
                    <ajaxStatistics:TabPanel runat="server" ID="TabPanel3" HeaderText="Infographic of OPD" BorderStyle="None">
                        <ContentTemplate>
                            <!--DATA STATISTIC AND ANALYSIS-->
                            <div id="ClinicalChart" class="card-body text-center">
                                <!--DATA SUMMARY-->
                                <div class="mt-4">
                                    <h4>DATA STATISTIC AND ANALYSIS FOR OPD</h4>
                                </div>
                                <br />
                                <div id="ClinicalStat" class="row justify-content-center mt-4">
                                    <div id="ClinicalPie1" class="ms-auto" style="background-color: #6abec7">
                                        <h1>
                                            <asp:Literal runat="server" ID="getTotalVisit"></asp:Literal></h1>
                                        <p>Total Visit</p>
                                    </div>
                                    <div id="ClinicalPie2" style="background-color: #c47ee2">
                                        <h1>
                                            <asp:Literal runat="server" ID="getAveMonthlyVisit"></asp:Literal></h1>
                                        <p>Monthly Visit Average</p>
                                    </div>
                                    <div id="ClinicalPie3" class="me-auto" style="background-color: #f7b15b">
                                        <h1>
                                            <asp:Literal runat="server" ID="getAveDailyVisit"></asp:Literal></h1>
                                        <p>Daily Visit Average</p>
                                    </div>
                                </div>
                                <br />
                                <br />
                                <br />
                                <!--CHARTS-->
                                <div id="ClinicalMonthly">
                                    <asp:Chart ID="ChartClinicalMonthly" runat="server" Width="800px" Height="300px" BackColor="White" CssClass="ChartBackground">
                                        <Series>
                                            <asp:Series Name="SeriesClinicalMonthly" ChartType="Line" Color="#5c001f" BorderWidth="5" YValuesPerPoint="1"></asp:Series>
                                        </Series>
                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartAreaClinicalMonthly" Area3DStyle-Enable3D="false" BackColor="#fff"></asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>
                                </div>
                                <br />
                                <br />
                                <div id="ClinicalDaily" class="mb-5">
                                    <asp:Chart ID="ChartClinicalDaily" runat="server" Width="800px" Height="300px" BackColor="White" CssClass="ChartBackground">
                                        <Series>
                                            <asp:Series Name="SeriesClinicalDaily" ChartType="Line" Color="#5c001f" BorderWidth="5" YValuesPerPoint="1"></asp:Series>
                                        </Series>
                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartAreaClinicalDaily" Area3DStyle-Enable3D="false" BackColor="#fff"></asp:ChartArea>
                                        </ChartAreas>
                                    </asp:Chart>
                                </div>
                            </div>
                        </ContentTemplate>
                    </ajaxStatistics:TabPanel>
                    <ajaxStatistics:TabPanel runat="server" ID="TabPanel4" HeaderText="List of Clinical Info (OPD)" BorderStyle="None">
                        <ContentTemplate>
                            <div class="mt-4">
                                <asp:GridView runat="server" ID="ClinicalHistoryTable" CssClass="display compact cell-border" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" Width="100%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>No.</HeaderTemplate>
                                            <ItemTemplate><%#Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="clinical_date" HeaderText="Date" SortExpression="clinical_date" />
                                        <asp:BoundField DataField="p_name" HeaderText="Patient Name" SortExpression="p_name" />
                                        <asp:BoundField DataField="symptom" HeaderText="Symptom" SortExpression="symptom" />
                                        <asp:BoundField DataField="ill_sign" HeaderText="Sign" SortExpression="ill_sign" />
                                        <asp:BoundField DataField="diagnosis" HeaderText="Diagnosis" SortExpression="diagnosis" />
                                        <asp:BoundField DataField="s_name" HeaderText="Doctor/PIC" SortExpression="s_name" />
                                    </Columns>
                                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </ajaxStatistics:TabPanel>
                </ajaxStatistics:TabContainer>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $("[id*=ClinicalHistoryTable]").DataTable({
                lengthMenu: [[20, 50, -1], [20, 50, "All"]],
                language: {
                    searchPlaceholder: "",
                    search: "Search",
                },
            });
            $("[id*=MCHistoryTable]").DataTable({
                lengthMenu: [[20, 50, -1], [20, 50, "All"]],
                language: {
                    searchPlaceholder: "",
                    search: "Search",
                },
            });
        });
    </script>
</asp:Content>
