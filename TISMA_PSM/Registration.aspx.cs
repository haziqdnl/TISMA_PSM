using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace TISMA_PSM
{
    public partial class Registration : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Filter"] = "All";
                BindGrid();
            }
        }

        private void BindGrid()
        {
            DataTable dt1 = new DataTable();

            //- DB Exception-Error handling
            try
            {
                //- Get Query: TISMADB
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                SqlConnection con1 = new SqlConnection(constr1);
                SqlDataAdapter sda1 = new SqlDataAdapter();
                SqlCommand cmd1 = new SqlCommand("GetTISMAPatient")
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd1.Parameters.AddWithValue("@Filter", ViewState["Filter"].ToString());
                cmd1.Connection = con1;
                sda1.SelectCommand = cmd1;
                sda1.Fill(dt1);
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                Debug.WriteLine("Database execution successful");
            }

            //- DataTable binding
            DisplayRegisteredData.DataSource = dt1;
            DisplayRegisteredData.DataBind();

            //- DB Exception-Error handling
            try
            {
                //- Get Query: UTMACAD
                string constr2 = ConfigurationManager.ConnectionStrings["utmacadConnectionString"].ConnectionString;
                using (SqlConnection con2 = new SqlConnection(constr2))
                {
                    using (SqlDataAdapter sda2 = new SqlDataAdapter("SELECT * FROM utm_acad_tbl", con2))
                    {
                        using (DataTable dt2 = new DataTable())
                        {
                            sda2.Fill(dt2);
                            DisplayUTMACADData.DataSource = dt2;
                            DisplayUTMACADData.DataBind();
                        }
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
                Debug.WriteLine("Database execution successful");
            }

            //- DB Exception-Error handling
            try
            {
                //- Get Query: UTMHR
                string constr3 = ConfigurationManager.ConnectionStrings["utmhrConnectionString"].ConnectionString;
                using (SqlConnection con3 = new SqlConnection(constr3))
                {
                    using (SqlDataAdapter sda3 = new SqlDataAdapter("SELECT * FROM utm_hr_tbl", con3))
                    {
                        using (DataTable dt3 = new DataTable())
                        {
                            sda3.Fill(dt3);
                            DisplayUTMHRData.DataSource = dt3;
                            DisplayUTMHRData.DataBind();
                        }
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
                Debug.WriteLine("Database execution successful");
            }

            //- Datatable render
            DisplayRegisteredData.UseAccessibleHeader = true;
            DisplayRegisteredData.HeaderRow.TableSection = TableRowSection.TableHeader;
            DisplayUTMACADData.UseAccessibleHeader = true;
            DisplayUTMACADData.HeaderRow.TableSection = TableRowSection.TableHeader;
            DisplayUTMHRData.UseAccessibleHeader = true;
            DisplayUTMHRData.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void CategoryChanged(object sender, EventArgs e)
        {
            DropDownList DdlPatientType = (DropDownList)sender;
            ViewState["Filter"] = DdlPatientType.SelectedValue;
            this.BindGrid();
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            DisplayRegisteredData.PageIndex = e.NewPageIndex;
            this.BindGrid();
        }

        public string EncryptURL(string url)
        {
            //- Custom key
            string EncryptionKey = "3NCRYPTTH1SURLP4R4M";

            //- Encryption logic
            String encrypted;
            byte[] clearBytes = Encoding.Unicode.GetBytes(url);
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