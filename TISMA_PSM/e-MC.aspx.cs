using System;
using System.Web.UI;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web;
using static TISMA_PSM.ControllerEMC;
using static TISMA_PSM.ControllerPatient;
using static TISMA_PSM.ControllerStaff;
using static TISMA_PSM.Helper;
using System.Net;

namespace TISMA_PSM
{
    public partial class E_MC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Get hashed url from URL param
            string eMCHashedUrl = Request.QueryString["token"];

            //- Validate if decrypted account no. is empty or null: 404 Error
            if (string.IsNullOrEmpty(eMCHashedUrl))
                Response.Redirect("PageNotFound.aspx");

            eMCHashedUrl = eMCHashedUrl.Replace(" ", "+");

            //- Validate if e-MC hashed URL not exist: 404 Error
            if (CheckEMCHashedUrlNotExist(eMCHashedUrl).Equals(true))
                Response.Redirect("PageNotFound.aspx");

            DisplayEMCInfo(eMCHashedUrl);
        }

        [Obsolete]
        protected void ExportToPDF(object sender, EventArgs e)
        {
            //- Get hashed url from URL param
            string eMCHashedUrl = Request.QueryString["token"];

            //- Validate if decrypted account no. is empty or null: 404 Error
            if (string.IsNullOrEmpty(eMCHashedUrl))
                Response.Redirect("PageNotFound.aspx");

            eMCHashedUrl = eMCHashedUrl.Replace(" ", "+");

            //- Validate if e-MC hashed URL not exist: 404 Error
            if (CheckEMCHashedUrlNotExist(eMCHashedUrl).Equals(true))
                Response.Redirect("PageNotFound.aspx");

            //- Get e-MC info
            ModelEMC emc = GetEMCInfoByUrl(eMCHashedUrl);

            //- Regenerate the QR Code in case the domain changed
            emc.qrCode = GenerateQRCode(eMCHashedUrl);

            //- Render PKU Logo
            logoPage.Visible = false;
            logoPKU.Visible = true;
            logoPKU.Src = GetImageUrl("custom/image/pkulogo.png");

            //- Render QR Code
            QrCodePage.Visible = false;
            QrCodePdf.Visible = true;
            System.Web.UI.WebControls.Image imgQRCode = new System.Web.UI.WebControls.Image
            {
                ImageUrl = "data:image/png;base64," + Convert.ToBase64String(emc.qrCode)
            };
            ;
            var imgUrl = imgQRCode.ImageUrl;
            //- Create temp QR code
            File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("/custom/image/temp/qr_" + emc.serialNo + ".png"), emc.qrCode);
            QrCodePdf.Src = GetImageUrl("custom/image/temp/qr_" + emc.serialNo + ".png");

            //- Generate PDF
            try
            {
                Response.ContentType = "Application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + emc.serialNo + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                tableMC.RenderControl(htw);
                StringReader sr = new StringReader(sw.ToString());
                Document doc = new Document(PageSize.A4, 30f, 30f, 15f, 15f);
                HTMLWorker htmlWorker = new HTMLWorker(doc);
                PdfWriter.GetInstance(doc, Response.OutputStream);

                doc.Open();
                htmlWorker.Parse(sr);
                doc.Close();

                Response.Write(doc);
                Response.End();
            }
            catch (WebException ex)
            {
                string responseText;
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            //- Delete the temp QR code
            File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("/custom/image/temp/qr_" + emc.serialNo + ".png"));
            imgQRCode.ImageUrl = imgUrl;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        public void VerifyPassword(object sender, EventArgs e)
        {
            //- Get hashed url from URL param
            string eMCHashedUrl = Request.QueryString["token"];

            //- Validate if decrypted account no. is empty or null: 404 Error
            if (string.IsNullOrEmpty(eMCHashedUrl))
                Response.Redirect("PageNotFound.aspx");

            eMCHashedUrl = eMCHashedUrl.Replace(" ", "+");

            //- Validate if e-MC hashed URL not exist: 404 Error
            if (CheckEMCHashedUrlNotExist(eMCHashedUrl).Equals(true))
                Response.Redirect("PageNotFound.aspx");

            //- Get e-MC info
            ModelEMC emc = GetEMCInfoByUrl(eMCHashedUrl);

            if (!string.IsNullOrEmpty(getPassword.Text))
            {
                string password = EncryptEMCPassword(getPassword.Text);
                if (password.Equals(emc.password))
                {
                    PasswordCard.Visible = false;
                    eMCCard.Visible = true;
                    PasswordIncorrectMsg.Visible = false;
                    DownloadCard.Visible = true;
                }
                else
                    PasswordIncorrectMsg.Visible = true;
            }
        }

        protected void DisplayEMCInfo(string eMCHashedUrl)
        {
            //- Get e-MC info by url
            ModelEMC emc = GetEMCInfoByUrl(eMCHashedUrl);
            //- Get Patient info
            ModelPatient patient = GetPatientInfoByIcNo(emc.pIcNo);
            //- Get Staff info
            ModelStaff staff = GetStaffInfoByIcNo(emc.sIcNo);

            //- Regenerate the QR Code in case the domain changed
            emc.qrCode = GenerateQRCode(eMCHashedUrl);

            //- Parse and render QR Code
            System.Web.UI.WebControls.Image imgQRCode = new System.Web.UI.WebControls.Image
            {
                Height = 150,
                Width = 150,
                ImageUrl = "data:image/png;base64," + Convert.ToBase64String(emc.qrCode)
            };
            QrCodePage.Controls.Add(imgQRCode);

            string dateFromDD = emc.dateFrom.Remove(0, 8); // yyyy/MM/dd -> dd
            string dateFromMM = emc.dateFrom.Remove(0, 5); // yyyy/MM/dd -> MM/dd
            dateFromMM = dateFromMM.Remove(2); // MM/dd -> MM
            string dateFromYY = emc.dateFrom.Remove(4); // yyyy/MM/dd -> yyyy
            string dateFrom = dateFromDD + "-" + dateFromMM + "-" + dateFromYY;

            string dateToDD = emc.dateTo.Remove(0, 8); // yyyy/MM/dd -> dd
            string dateToMM = emc.dateTo.Remove(0, 5); // yyyy/MM/dd -> MM/dd
            dateToMM = dateToMM.Remove(2); // MM/dd -> MM
            string dateToYY = emc.dateTo.Remove(4); // yyyy/MM/dd -> yyyy
            string dateTo = dateToDD + "-" + dateToMM + "-" + dateToYY;

            //- Parse and render data
            getSerialNo.Text = emc.serialNo;
            getName.Text = patient.pName;
            getIcNo.Text = patient.pIcNo;
            getPeriod.Text = emc.period.ToString();
            getDateFrom.Text = dateFrom;
            getDateTo.Text = dateTo;
            getComment.Text = emc.comment;
            getDate.Text = emc.dateCreated;
            getTime.Text = emc.timeCreated;
            getDrName.Text = staff.sName;
            getUrl.Text = GetUrleMC() + eMCHashedUrl;

            if (patient.pCategory.Equals("Student"))
                getMatricStaff.Text = patient.pMatricNo;
            else if (patient.pCategory.Equals("Staff"))
                getMatricStaff.Text = patient.pStaffId;
            else
                getMatricStaff.Text = "-";
        }

        private string GetImageUrl(string imagepath)
        {
            string[] splits = Request.Url.AbsoluteUri.Split('/');
            if (splits.Length >= 2)
            {
                string url = splits[0] + "//";
                for (int i = 2; i < splits.Length - 1; i++)
                {
                    url += splits[i];
                    url += "/";
                }
                return url + imagepath;
            }
            return imagepath;
        }
    }
}