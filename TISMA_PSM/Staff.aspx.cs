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
    public partial class Staff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BindGrid();
            }
        }

        private void BindGrid()
        {
            //- DB Exception-Error handling
            try
            {
                //- Get Query: TISMADB
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    using (SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM pku_staff ORDER BY s_name", con1))
                    {
                        using (DataTable dt1 = new DataTable())
                        {
                            sda1.Fill(dt1);
                            RegisteredStaffTable.DataSource = dt1;
                            RegisteredStaffTable.DataBind();
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
                string constr2 = ConfigurationManager.ConnectionStrings["utmhrConnectionString"].ConnectionString;
                using (SqlConnection con2 = new SqlConnection(constr2))
                {
                    using (SqlDataAdapter sda2 = new SqlDataAdapter("SELECT * FROM utm_hr_tbl", con2))
                    {
                        using (DataTable dt2 = new DataTable())
                        {
                            sda2.Fill(dt2);
                            DisplayUTMHRData.DataSource = dt2;
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
            RegisteredStaffTable.UseAccessibleHeader = true;
            RegisteredStaffTable.HeaderRow.TableSection = TableRowSection.TableHeader;
            DisplayUTMHRData.UseAccessibleHeader = true;
            DisplayUTMHRData.HeaderRow.TableSection = TableRowSection.TableHeader;
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