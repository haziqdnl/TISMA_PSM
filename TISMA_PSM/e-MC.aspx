<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="e-MC.aspx.cs" Inherits="TISMA_PSM.e_MC" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>e-MC :: TISMA</title>
    <!--CSS Bootstrap-->
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css" />
    <!--CSS Fontawesome-->
    <link rel="stylesheet" href="fontawesome/css/all.css" />
    <!--CSS DataTables-->
    <%--<link href="datatables/css/jquery.dataTables.min.css" rel="stylesheet" />--%>
    <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />

    <!--CSS Custom-->
    <link rel="stylesheet" href="custom/css/main.css" />

    <!--JS bootstrap-->
    <script src="bootstrap/js/bootstrap.bundle.min.js"></script>
    <!--jQuery-->
    <%--<script src="bootstrap/js/jquery-3.4.1.min.js"></script>--%>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <!--JS DataTables-->
    <%--<script src="datatables/js/jquery.dataTables.min.js"></script>--%>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/v/dt/dt-1.10.24/datatables.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div runat="server" id="PasswordCard" class="container d-flex justify-content-center align-items-center">
            <div class="card w-100">
                <p style="color:#717171">This is a confidential document</p>
                <p>Enter e-MC Password:</p>
                <asp:TextBox runat="server" ID="getPassword" TextMode="Password" CssClass="textbox-custom text-center float-start" ></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="getPassword" 
                    ValidationExpression="^[A-Z]{8}$" Display="Dynamic" SetFocusOnError="true"
                    ErrorMessage="Invalid Password" ForeColor="Red" Font-Size="10px">
                </asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="getPassword"
                    ErrorMessage="Please enter the password" ForeColor="Red" Display="Dynamic" Font-Size="10px">
                </asp:RequiredFieldValidator>
                <p runat="server" id="PasswordIncorrectMsg" visible="false" style="color:red; font-size:10px">Oops! Incorrect password, please try again.</p>
                <br />
                <div class="text-center">
                    <asp:Button runat="server" ID="btnPassword" Text="Proceed" OnClick="VerifyPassword" CssClass="btn-custom" ForeColor="White" BackColor="#0066ff" Width="100px"/>
                </div>
            </div>
        </div>
        <div runat="server" id="eMCCard" visible="false" class="container mt-5 d-flex justify-content-center align-items-center">
            <table runat="server" id="tableMC" style="border: 40px solid #fff; width: 21cm; height: auto; background-color: #fff; margin: 0; padding: 0; font-weight: 500; font-size: 12px">
                <tr>
                    <td class="text-center" colspan="3">
                        <img runat="server" id="logoPage" src="custom/image/pkulogo.png" width="319" height="89" style="text-align: center" />
                        <img runat="server" id="logoPKU" visible="false" src="custom/image/pkulogo.png" width="319" height="89" style="text-align: center" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="text-center">
                        <p style="text-align: center; font-size: 1.5em; font-weight: bolder; margin: 1rem 0 1rem 0"><b>SIJIL AKUAN SAKIT</b></p>
                    </td>
                </tr>
                <tr>
                    <td class="text-start" colspan="3">
                        <p>No. Siri:<asp:Literal runat="server" ID="getSerialNo"></asp:Literal></p>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr class="text-start">
                    <td>
                        <p>Nama</p>
                    </td>
                    <td colspan="2">
                        <p>: <asp:Literal runat="server" ID="getName"></asp:Literal></p>
                    </td>
                </tr>
                <tr class="text-start">
                    <td>
                        <p>No. Kad Pengenalan</p>
                    </td>
                    <td colspan="2">
                        <p>: <asp:Literal runat="server" ID="getIcNo"></asp:Literal></p>
                    </td>
                </tr>
                <tr class="text-start">
                    <td>
                        <p>No Matrik/Staff</p>
                    </td>
                    <td colspan="2">
                        <p>: <asp:Literal runat="server" ID="getMatricStaff"></asp:Literal></p>
                    </td>
                </tr>
                <tr>
                    <td class="text-start" colspan="3">
                        <br />
                        <p>
                            Saya mengesahkan telah memeriksa Encik / Puan / Cik yang disebut di atas dan mendapati beliau tidak sihat untuk bertugas / belajar selama
                        <b><u>
                            <asp:Literal runat="server" ID="getPeriod"></asp:Literal></u></b> hari, dari 
                        <b><u>
                            <asp:Literal runat="server" ID="getDateFrom"></asp:Literal></u></b> hingga <b>
                                <u><asp:Literal runat="server" ID="getDateTo"></asp:Literal></u></b>.
                        </p>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="text-start" colspan="3" style="width: 150px">
                        <p>Ulasan:</p>
                    </td>
                </tr>
                <tr>
                    <td class="text-start" colspan="3">
                        <p>
                            <asp:Literal runat="server" ID="getComment"></asp:Literal></p>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <p><br /></p>
                    </td>
                </tr>
                <tr>
                    <td class="text-start" style="width: 150px">
                        <p>Tarikh Dicetak:</p>
                    </td>
                    <td></td>
                    <td class="text-start" style="width:250px">
                        <p>Pengesahan Doktor:</p>
                    </td>
                </tr>
                <tr>
                    <td class="text-start">
                        <p><asp:Literal runat="server" ID="getDate"></asp:Literal></p>
                    </td>                    
                    <td></td>
                    <td class="text-start">
                        <p><asp:Literal runat="server" ID="getDrName"></asp:Literal></p>
                    </td>
                </tr>
                <tr>
                    <td class="text-start">
                        <p style="width: 150px">Masa Dicetak:</p>
                    </td>
                    <td></td>
                    <td class="text-start">
                        <p>Pusat Kesihatan UTM</p>
                    </td>
                </tr>
                <tr>
                    <td class="text-start">
                        <p>
                            <asp:Literal runat="server" ID="getTime"></asp:Literal></p>
                    </td>
                    <td></td>
                    <td class="text-start">
                        <p><b><i>True Copy</i></b></p>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <p>
                            <br />
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="text-center" colspan="3">
                        <p style="text-align: center; font-size:11px">
                            Sijil Akuan Sakit ini tidak memerlukan tandatangan kerana telah dihasilkan secara elektronik melalui<br />
                            <span style="color: blue; font-weight: 500;"><asp:Literal runat="server" ID="getUrl"></asp:Literal></span>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class="text-center" colspan="3">
                        <br />
                        <img runat="server" src="#" id="QrCodePdf" visible="false" width="150" height="150" style="text-align: center" />
                        <asp:PlaceHolder runat="server" ID="QrCodePage"></asp:PlaceHolder>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
        <div runat="server" id="DownloadCard" visible="false" class="container mt-5 d-flex justify-content-center align-items-center">
            <div class="text-center" style="margin-bottom: 80px">
                <asp:Button ID="btnDownload" runat="server" Text="Download e-MC" OnClick="ExportToPDF" CssClass="btn-custom" ForeColor="White" BackColor="#0066ff" />
            </div>
        </div>
    </form>
</body>
</html>
