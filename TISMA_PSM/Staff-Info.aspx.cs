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
using System.Windows;

namespace TISMA_PSM
{
    public partial class Staff_Info : System.Web.UI.Page
    {
        //- Global variable
        public static string currentPhone;
        public static string currentEmail;

        protected void Page_Load(object sender, EventArgs e)
        {
            //- Get pid (ic_no) and stat from URL param
            String acc_no = DecryptURL(Request.QueryString["accno"]);

            //- Local attribute
            String ic_no;
            int age;

            if (!this.IsPostBack)
            {
                //- DB Exception-Error handling
                try
                {
                    //- Get Query
                    string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                    using (SqlConnection con1 = new SqlConnection(constr1))
                    {
                        using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM pku_staff WHERE s_account_no = '" + acc_no + "'"))
                        {
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = con1;
                            con1.Open();
                            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                            {
                                sdr1.Read();

                                //- Parse Date value from SQL to DateTime obj
                                DateTime.TryParse(sdr1["s_dob"].ToString(), out DateTime dob);

                                //- Parse for local use
                                ic_no = sdr1["s_ic_no"].ToString();

                                //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
                                DateTime dobToAge = Convert.ToDateTime(dob);
                                age = DateTime.Now.AddYears(-dobToAge.Year).Year;

                                getBranch.SelectedValue = sdr1["s_branch"].ToString();
                                getCategory.Text = sdr1["s_category"].ToString();
                                getAccNo.Text = sdr1["s_account_no"].ToString(); displayAccNo.Text = sdr1["s_account_no"].ToString(); displayUsername.Text = sdr1["s_username"].ToString();
                                getUsername.Text = sdr1["s_username"].ToString();
                                getPassword.Text = sdr1["s_password"].ToString();
                                getName.Text = sdr1["s_name"].ToString();
                                getStaffId.Text = sdr1["s_staff_id"].ToString();
                                getIcNo.Text = sdr1["s_ic_no"].ToString();
                                getPassportNo.Text = sdr1["s_passport_no"].ToString();
                                getDob.Text = dob.ToString("dd/MM/yyyy");
                                getAge.Text = age.ToString();
                                getGender.SelectedValue = sdr1["s_gender"].ToString();
                                getMaritalStat.SelectedValue = sdr1["s_marital_stat"].ToString();
                                getReligion.SelectedValue = sdr1["s_religion"].ToString();
                                getRace.SelectedValue = sdr1["s_race"].ToString();
                                getNation.Text = sdr1["s_nationality"].ToString();
                                getPhone.Text = sdr1["s_tel_no"].ToString(); currentPhone = sdr1["s_tel_no"].ToString();
                                getEmail.Text = sdr1["s_email"].ToString(); currentEmail = sdr1["s_email"].ToString();
                                getDesignation.Text = sdr1["s_designation"].ToString();
                                getFacDep.Text = sdr1["s_department"].ToString();
                                getAddress.Text = sdr1["s_address"].ToString();
                                getSession.Text = sdr1["s_session"].ToString();
                            }
                            con1.Close();
                        }
                        if (CheckIsStaffAdmin(ic_no).Equals(true))
                        {
                            //- DB Exception-Error handling
                            try
                            {
                                //- Get Query
                                using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM pku_admin WHERE fk_ic_no = '" + ic_no + "'"))
                                {
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.Connection = con1;
                                    con1.Open();
                                    using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                                    {
                                        sdr1.Read();
                                        //- Parse data
                                        if(sdr1["admin_role"].ToString().Equals("True"))
                                            getTismaRoleDdl.SelectedValue = "Admin";
                                    }
                                    con1.Close();
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
                                Debug.WriteLine("DB Execution Success: Check role admin");
                            }
                        }
                        else if (CheckIsStaffMO(ic_no).Equals(true))
                        {
                            //- DB Exception-Error handling
                            try
                            {
                                //- Get Query
                                using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM pku_medical_officer WHERE fk_ic_no = '" + ic_no + "'"))
                                {
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.Connection = con1;
                                    con1.Open();
                                    using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                                    {
                                        sdr1.Read();
                                        //- Parse data
                                        if (sdr1["mo_role"].ToString().Equals("True"))
                                            getTismaRoleDdl.SelectedValue = "Medical Officer";
                                    }
                                    con1.Close();
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
                                Debug.WriteLine("DB Execution Success: Check role MO");
                            }
                        }
                        else if (CheckIsStaffReceptionist(ic_no).Equals(true))
                        {
                            //- DB Exception-Error handling
                            try
                            {
                                //- Get Query
                                using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM pku_receptionist WHERE fk_ic_no = '" + ic_no + "'"))
                                {
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.Connection = con1;
                                    con1.Open();
                                    using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                                    {
                                        sdr1.Read();
                                        //- Parse data
                                        if (sdr1["reception_role"].ToString().Equals("True"))
                                            getTismaRoleDdl.SelectedValue = "Receptionist";
                                    }
                                    con1.Close();
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
                                Debug.WriteLine("DB Execution Success: Check role Receptionist");
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
                    Debug.WriteLine("DB Execution Success: Retrieve staff data");
                }
            }
        }

        protected void DeleteConfirmation(object sender, EventArgs e)
        {
            ModalPopupMessageDelete.Show();
        }

        protected void DeleteFromTisma(object sender, EventArgs e)
        {
            //- DB Exception-Error handling
            try
            {
                //- Delete Query
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM pku_staff WHERE s_ic_no = '" + getIcNo.Text + "'", con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
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
                Debug.WriteLine("DB Execution Success: Staff deleted");
            }
            Response.Redirect("Staff.aspx");
        }

        protected void UpdateToTisma(object sender, EventArgs e)
        {
            //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
            DateTime.TryParse(getDob.Text, out DateTime dob);
            DateTime dobToAge = Convert.ToDateTime(dob);
            int age = DateTime.Now.AddYears(-dobToAge.Year).Year;

            //- Calculate the current session when the patient data is updated
            String session = DateTime.Now.AddYears(-1).ToString("yyyy") + "/" + DateTime.Now.ToString("yyyy");

            Debug.WriteLine(currentPhone);

            if (CheckIsPhoneStaffExist(getPhone.Text).Equals(true) && !getPhone.Text.Equals(currentPhone))
            {
                getValidationMsg.Text = "The updated phone number already in use.";
                ModalPopupMessageValidation.Show();
            }
            else if (CheckIsEmailStaffExist(getEmail.Text).Equals(true) && !getEmail.Text.Equals(currentEmail))
            {
                getValidationMsg.Text = "The updated email address already in use.";
                ModalPopupMessageValidation.Show();
            }
            else
            {
                //- DB Exception-Error handling
                try
                {
                    //- Update Query
                    string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        //- Table 'pku_staff'
                        using (SqlCommand cmd = new SqlCommand("UPDATE pku_staff " +
                                                               "SET s_password = '" + getPassword.Text + "', " +
                                                                   "s_passport_no = '" + getPassportNo.Text + "', " +
                                                                   "s_marital_stat = '" + getMaritalStat.SelectedValue + "', " +
                                                                   "s_religion = '" + getReligion.SelectedValue + "', " +
                                                                   "s_tel_no = '" + getPhone.Text + "', " +
                                                                   "s_email = '" + getEmail.Text + "', " +
                                                                   "s_address = '" + getAddress.Text + "', " +
                                                                   "s_session = '" + session + "', " +
                                                                   "s_age = '" + age + "' " +
                                                               "WHERE s_ic_no = '" + getIcNo.Text + "'", con))
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            con.Dispose();
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
                    Debug.WriteLine("DB Execution Success: Staff updated");
                }
                ModalPopupMessageUpdate.Show();
            }            
        }

        public static bool CheckIsEmailStaffExist(string email)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckIsEmailStaffExist", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Email", email);
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static bool CheckIsPhoneStaffExist(string telNo)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckIsPhoneStaffExist", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@TelNo", telNo);
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static bool CheckIsStaffAdmin(string icNo)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckIsStaffAdmin", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@IcNo", icNo);
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static bool CheckIsStaffMO(string icNo)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckIsStaffMO", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@IcNo", icNo);
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static bool CheckIsStaffReceptionist(string icNo)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckIsStaffReceptionist", con)
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