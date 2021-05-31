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
using System.Net.Mail;

namespace TISMA_PSM
{
    public partial class Add_New_Staff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Get pid (ic_no) and stat from URL param
            string ic_no = DecryptURL(Request.QueryString["pid"]);

            //- Check is patient added to TISMA
            if (CheckIsStaffAddedToTisma(ic_no).Equals(true))
                ModalPopupMessage.Show();

            //- DB Exception-Error handling
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["utmhrConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM utm_hr_tbl WHERE ic_no = '" + ic_no + "'"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            sdr.Read();

                            //- Parse Date value from SQL to DateTime obj
                            DateTime.TryParse(sdr["dob"].ToString(), out DateTime dob);

                            //- Parse Email value and generate Username pattern from Email name
                            string email = sdr["email"].ToString();
                            MailAddress addr = new MailAddress(email);
                            string username = addr.User;

                            getBranch.SelectedValue = sdr["branch"].ToString();
                            getCategory.Text = sdr["category"].ToString();
                            getAccNo.Text = GenerateAccNo();
                            getUsername.Text = username;
                            getPassword.Text = sdr["ic_no"].ToString();
                            getName.Text = sdr["name"].ToString();
                            getStaffId.Text = sdr["staff_id"].ToString();
                            getIcNo.Text = sdr["ic_no"].ToString();
                            getPassportNo.Text = sdr["passport_no"].ToString();
                            getDob.Text = dob.ToString("dd/MM/yyyy");
                            getAge.Text = sdr["age"].ToString();
                            getGender.SelectedValue = sdr["gender"].ToString();
                            getMaritalStat.SelectedValue = sdr["marital_stat"].ToString();
                            getReligion.SelectedValue = sdr["religion"].ToString();
                            getRace.SelectedValue = sdr["race"].ToString();
                            getNation.Text = sdr["nationality"].ToString();
                            getPhone.Text = sdr["tel_no"].ToString();
                            getEmail.Text = email;
                            getDesignation.Text = sdr["designation"].ToString();
                            getFacDep.Text = sdr["department"].ToString();
                            getAddress.Text = sdr["staff_address"].ToString();
                            getSession.Text = sdr["session_no"].ToString();
                        }
                        con.Close();
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
                Debug.WriteLine("DB Execution Success: Retrieve staff data from UTM-HR");
            }
        }

        protected void AddToTisma(object sender, EventArgs e)
        {
            //- DB Exception-Error handling
            try
            {
                //- Identify TISMA Role
                string role = getTismaRoleDdl.SelectedValue, spFunc;
                if (role.Equals("Admin"))
                    spFunc = "AddToTismaPkuAdmin";
                else if (role.Equals("Medical Officer"))
                    spFunc = "AddToTismaPkuMO";
                else
                    spFunc = "AddToTismaPkuReceptionist";

                //- Insert Query
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(spFunc, con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //- Insert to table 'pku_staff'
                cmd.Parameters.AddWithValue("@IcNo", getIcNo.Text);
                cmd.Parameters.AddWithValue("@Passport", getPassportNo.Text);
                cmd.Parameters.AddWithValue("@AccNo", getAccNo.Text);
                cmd.Parameters.AddWithValue("@Username", getUsername.Text);
                cmd.Parameters.AddWithValue("@Password", getPassword.Text);
                cmd.Parameters.AddWithValue("@StaffId", getStaffId.Text);
                cmd.Parameters.AddWithValue("@TelNo", getPhone.Text);
                cmd.Parameters.AddWithValue("@Email", getEmail.Text);
                cmd.Parameters.AddWithValue("@Name", getName.Text);
                cmd.Parameters.AddWithValue("@Dob", Convert.ToDateTime(getDob.Text));
                cmd.Parameters.AddWithValue("@Age", int.Parse(getAge.Text));
                cmd.Parameters.AddWithValue("@Gender", getGender.SelectedValue);
                cmd.Parameters.AddWithValue("@Marital", getMaritalStat.SelectedValue);
                cmd.Parameters.AddWithValue("@Religion", getReligion.SelectedValue);
                cmd.Parameters.AddWithValue("@Race", getRace.SelectedValue);
                cmd.Parameters.AddWithValue("@Nationality", getNation.Text);
                cmd.Parameters.AddWithValue("@Address", getAddress.Text);
                cmd.Parameters.AddWithValue("@Designation", getDesignation.Text);
                cmd.Parameters.AddWithValue("@Department", getFacDep.Text);
                cmd.Parameters.AddWithValue("@Session", getSession.Text);
                cmd.Parameters.AddWithValue("@Category", getCategory.Text);
                cmd.Parameters.AddWithValue("@Branch", getBranch.SelectedValue);

                //- Insert to table 'pku_admin' or 'pku_medical_officer' or 'pku_receptionist'
                cmd.Parameters.AddWithValue("@Role", 1);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Add to TISMA");
            }
            Response.Redirect("Staff.aspx");
        }

        public static bool CheckAccStaffNotExist(string accNo)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckAccStaffNotExist", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@AccNo", accNo.Trim());
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static bool CheckIsStaffAddedToTisma(string icNo)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckIsStaffAddedToTisma", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@IcNo", icNo);
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static string DecryptURL(string encryptedURL)
        {
            //- Custom key
            string EncryptionKey = "3NCRYPTTH1SURLP4R4M";

            //- Decrypt logic
            encryptedURL = encryptedURL.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(encryptedURL);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    encryptedURL = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return encryptedURL;
        }

        public static string GenerateAccNo()
        {
            //- Step 1: Init Account No pattern
            string accNo = "STAFF";

            //- Step 2: Generate Date pattern and append to accNo
            string today = DateTime.Now.ToString("yyyy/MM/dd");
            today = today.Remove(7, 1); // yyyy/MM_dd
            today = today.Remove(4, 1); // yyyy_MMdd            
            accNo += today; 

            //- Step 3: Append unique no.
            //-- Step 3.1: Generate random no. between 0 to 999
            Random rand = new Random();
            int num = rand.Next(0, 999);

            //-- Step 3.2: Identify and parse random no. pattern as string, then append to accNo
            if (num <= 9)
                accNo = accNo + "00" + num.ToString();
            else if (num >= 10 && num <= 99)
                accNo = accNo + "0" + num.ToString();
            else
                accNo += num.ToString();

            //- Step 4: Identify the generated acc no. if it already existed in db
            if (CheckAccStaffNotExist(accNo).Equals(true))
            {
                Debug.WriteLine("The generated Acc No. ["+accNo+"] not existed yet.");
                return accNo;
            }
            else
            {
                Debug.WriteLine("The generated Acc No. ["+accNo+"] already existed. Re-generating....");
                return GenerateAccNo();
            }
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