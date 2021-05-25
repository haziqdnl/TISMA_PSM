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
    public partial class OPD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }

        }

        private void BindGrid()
        {
            //- Get today's date
            DateTime today = DateTime.Now;

            //- DB Exception-Error handling
            try
            {
                string query = "SELECT qms.queue_no, patient.p_name, qms.fk_ic_no, patient.p_category, patient.p_account_no, patient.p_remarks " +
                               "FROM qms LEFT OUTER JOIN patient " +
                               "ON qms.fk_ic_no = patient.p_ic_no " +
                               "WHERE qms.is_keyed_in = '" + 1 + "' AND qms.is_expired = '" + 0 + "' AND qms.is_checked_in = '" + 0 + "' AND qms.date_generated = '" + today.ToString("yyyyMMdd") + "'";

                //- Get Query: TISMADB
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.HasRows)
                    {
                        //- If records available
                        OpdTable.DataSource = sdr;
                        OpdTable.DataBind();
                    }
                    else
                    {
                        //- If no records found
                        DataTable dt = new DataTable();
                        OpdTable.DataSource = dt;
                        OpdTable.DataBind();
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
            OpdTable.UseAccessibleHeader = true;
            OpdTable.HeaderRow.TableSection = TableRowSection.TableHeader;
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