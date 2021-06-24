using System;
using System.Web.UI;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Web;
using System.Security.Cryptography;

namespace TISMA_PSM
{
    public partial class e_MC : System.Web.UI.Page
    {
        //- Global attribute
        public static string encryptedPassword;
        public static byte[] qrCodeByte;
        public static string serialNo;

        protected void Page_Load(object sender, EventArgs e)
        {
            //- Get hashed url from URL param
            string eMCHashedUrl = Request.QueryString["token"];
            eMCHashedUrl = eMCHashedUrl.Replace(" ", "+");

            GetPatientEMCInfo(eMCHashedUrl);

            if (!this.IsPostBack)
            {
                
            }
        }

        [Obsolete]
        protected void ExportToPDF(object sender, EventArgs e)
        {
            //- Render PKU Logo
            logoPage.Visible = false;
            logoPKU.Visible = true;
            logoPKU.Src = GetUrl("custom/image/pkulogo.png");

            //- Render QR Code
            QrCodePage.Visible = false;
            QrCodePdf.Visible = true;
            System.Web.UI.WebControls.Image imgQRCode = new System.Web.UI.WebControls.Image
            {
                ImageUrl = "data:image/png;base64," + Convert.ToBase64String(qrCodeByte)
            };
            ;
            var imgUrl = imgQRCode.ImageUrl;
            File.WriteAllBytes(Server.MapPath("temp.png"), qrCodeByte);
            QrCodePdf.Src = GetUrl("temp.png");

            //- Generate PDF
            StringWriter s_tw = new StringWriter();
            HtmlTextWriter h_textw = new HtmlTextWriter(s_tw);
            tableMC.RenderControl(h_textw);
            StringReader s_tr = new StringReader(s_tw.ToString());            
            Document doc = new Document(PageSize.A4, 30f, 30f, 15f, 15f);
            PdfWriter.GetInstance(doc, Response.OutputStream);
            HTMLWorker html_worker = new HTMLWorker(doc);

            doc.Open();
            html_worker.Parse(s_tr);
            doc.Close();

            Response.ContentType = "Application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=" + serialNo + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(doc);
            File.Delete(Server.MapPath("temp.png"));
            imgQRCode.ImageUrl = imgUrl;
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        public void VerifyPassword(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(getPassword.Text))
            {
                string password = EncryptEMCPassword(getPassword.Text);

                if (password.Equals(encryptedPassword))
                {
                    PasswordCard.Visible = false;
                    eMCCard.Visible = true;
                    PasswordIncorrectMsg.Visible = false;
                    DownloadCard.Visible = true;
                }
                else
                {
                    PasswordIncorrectMsg.Visible = true;
                }
            }
        }

        protected void GetPatientEMCInfo(string eMCHashedUrl)
        {
            string category, icNo;

            //- DB Exception-Error handling
            try
            {
                //- Get Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    using (SqlCommand cmd1 = new SqlCommand("SELECT qr_code FROM emc WHERE url_hashed = '" + eMCHashedUrl + "'", con1))
                    {
                        con1.Open();
                        byte[] _qrCodeByte = (byte[])cmd1.ExecuteScalar();
                        MemoryStream ms = new MemoryStream();
                        ms.Write(_qrCodeByte, 0, _qrCodeByte.Length);
                        Bitmap bm = new Bitmap(ms);
                        qrCodeByte = _qrCodeByte;

                        System.Web.UI.WebControls.Image imgQRCode = new System.Web.UI.WebControls.Image
                        {
                            Height = 150,
                            Width = 150,
                            ImageUrl = "data:image/png;base64," + Convert.ToBase64String(_qrCodeByte)
                        };
                        QrCodePage.Controls.Add(imgQRCode);
                        con1.Close();
                    }

                    using (SqlCommand cmd1 = new SqlCommand("SELECT patient.p_name, patient.p_category, pku_staff.s_name, emc.* FROM emc " +
                                                            "LEFT JOIN patient ON patient.p_ic_no = emc.fk_p_ic_no " +
                                                            "LEFT JOIN pku_staff ON pku_staff.s_ic_no = emc.fk_s_ic_no " +
                                                            "WHERE url_hashed = '" + eMCHashedUrl + "'"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr = cmd1.ExecuteReader())
                        {
                            sdr.Read();

                            string url = "https://localhost:44316/e-MC.aspx?token=" + eMCHashedUrl;

                            //- Parse data
                            serialNo = sdr["serial_no"].ToString();
                            getSerialNo.Text = serialNo;
                            getName.Text = sdr["p_name"].ToString();
                            icNo = sdr["fk_p_ic_no"].ToString();
                            getIcNo.Text =  icNo;
                            encryptedPassword = sdr["emc_password"].ToString();
                            getPeriod.Text = sdr["emc_period"].ToString();
                            DateTime.TryParse(sdr["date_from"].ToString(), out DateTime dateFrom);
                            getDateFrom.Text = dateFrom.ToString("dd/MM/yyyy");
                            DateTime.TryParse(sdr["date_to"].ToString(), out DateTime dateTo);
                            getDateTo.Text = dateTo.ToString("dd/MM/yyyy");
                            getComment.Text = sdr["comment"].ToString();
                            DateTime.TryParse(sdr["date_created"].ToString(), out DateTime dateCreated);
                            getDate.Text = dateCreated.ToString("dd/MM/yyyy");
                            DateTime.TryParse(sdr["time_created"].ToString(), out DateTime timeCreated);
                            getTime.Text = timeCreated.ToString("hh:mm:ss tt");
                            getDrName.Text = sdr["s_name"].ToString();
                            getUrl.Text = url;
                            category = sdr["p_category"].ToString();
                        }
                        con1.Close();
                    }

                    if (category.Equals("Student"))
                    {
                        using (SqlCommand cmd1 = new SqlCommand("SELECT matric_no FROM patient_student WHERE fk_ic_no = '" + icNo + "'"))
                        {
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = con1;
                            con1.Open();
                            using (SqlDataReader sdr = cmd1.ExecuteReader())
                            {
                                sdr.Read();

                                getMatricStaff.Text = sdr["matric_no"].ToString();
                            }
                            con1.Close();
                        }
                    }
                    else if (category.Equals("Staff"))
                    {
                        using (SqlCommand cmd1 = new SqlCommand("SELECT staff_id FROM patient_staff WHERE fk_ic_no = '" + icNo + "'"))
                        {
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = con1;
                            con1.Open();
                            using (SqlDataReader sdr = cmd1.ExecuteReader())
                            {
                                sdr.Read();

                                getMatricStaff.Text = sdr["staff_id"].ToString();
                            }
                            con1.Close();
                        }
                    }
                    else
                    {
                        getMatricStaff.Text = "-";
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Get e-MC info");
            }
        }

        public static string EncryptEMCPassword(string password)
        {
            //- Custom key
            string EncryptionKey = "3NCRYPTP@55W0RD3MC";

            //- Encryption logic
            String encrypted;
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encrypted = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encrypted;
        }

        protected string GetUrl(string imagepath)
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

        public static void SqlExceptionMsg(SqlException ex)
        {
            StringBuilder errorMessages = new StringBuilder();
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + "\n" +
                "Message: " + ex.Errors[i].Message + "\n" +
                "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                "Source: " + ex.Errors[i].Source + "\n" +
                "Procedure: " + ex.Errors[i].Procedure + "\n");
            }
            Debug.WriteLine(errorMessages.ToString());
        }
    }
}