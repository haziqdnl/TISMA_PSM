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
    public partial class Patient_Info : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Show and Hide several div on Generate Queue function
            afterGenerate.Visible = false;

            //- Get pid (ic_no) and stat from URL param
            String acc_no = DecryptURL(Request.QueryString["accno"]);

            //- Local attribute
            String ic_no, category;
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
                        using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient WHERE p_account_no = '" + acc_no + "'"))
                        {
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = con1;
                            con1.Open();
                            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                            {
                                sdr1.Read();

                                //- Parse Date value from SQL to DateTime obj
                                DateTime.TryParse(sdr1["p_dob"].ToString(), out DateTime dob);

                                //- Parse for local use
                                ic_no = sdr1["p_ic_no"].ToString();
                                category = sdr1["p_category"].ToString();

                                //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
                                DateTime dobToAge = Convert.ToDateTime(dob);
                                age = DateTime.Now.AddYears(-dobToAge.Year).Year;

                                //- Parse data
                                getIcNo.Text = ic_no;
                                getAccNo.Text = sdr1["p_account_no"].ToString(); displayAccNo.Text = sdr1["p_account_no"].ToString(); getAccNoQMS.Text = sdr1["p_account_no"].ToString();
                                getPassportNo.Text = sdr1["p_passport_no"].ToString();
                                getPhone.Text = sdr1["p_tel_no"].ToString();
                                getEmail.Text = sdr1["p_email"].ToString();
                                getName.Text = sdr1["p_name"].ToString();
                                getDob.Text = dob.ToString("dd/MM/yyyy");
                                getAge.Text = age.ToString();
                                getGender.SelectedValue = sdr1["p_gender"].ToString();
                                getMaritalStat.SelectedValue = sdr1["p_marital_stat"].ToString();
                                getReligion.SelectedValue = sdr1["p_religion"].ToString();
                                getRace.SelectedValue = sdr1["p_race"].ToString();
                                getNation.Text = sdr1["p_nationality"].ToString();
                                getAddress.Text = sdr1["p_address"].ToString();
                                getDesignation.Text = sdr1["p_designation"].ToString();
                                getCategory.Text = category; displayCategory.Text = category;
                                getSession.Text = sdr1["p_session"].ToString();
                                getBranch.SelectedValue = sdr1["p_branch"].ToString();
                                getRemarks.Text = sdr1["p_remarks"].ToString();
                            }
                            con1.Close();
                        }
                        if (category.Equals("Student"))
                        {
                            //- Display note
                            note.Text = "The basic information were reffered from UTMHR SYSTEM / ACAD SYSTEM";

                            //- DB Exception-Error handling
                            try
                            {
                                //- Get Query
                                using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient_student WHERE fk_ic_no = '" + ic_no + "'"))
                                {
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.Connection = con1;
                                    con1.Open();
                                    using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                                    {
                                        sdr1.Read();

                                        //- Status activity
                                        if (sdr1["utm_acad_stat"].ToString().Equals("True"))
                                            statusText.Text = "Active";
                                        else
                                            statusText.Text = "Not-active";

                                        //- Parse data
                                        getMatricNo.Text = sdr1["matric_no"].ToString();
                                        getFacDep.Text = sdr1["faculty"].ToString();
                                        getCourse.Text = sdr1["course"].ToString();
                                        getSem.Text = sdr1["semester"].ToString();
                                        getStat.Text = "UTM-ACAD";
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
                                Debug.WriteLine("Database execution successful");
                            }
                        }
                        else if (category.Equals("Staff"))
                        {
                            //- Display note
                            note.Text = "The basic information were reffered from UTMHR SYSTEM / ACAD SYSTEM";

                            //- DB Exception-Error handling
                            try
                            {
                                //- Get Query
                                using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient_staff WHERE fk_ic_no = '" + ic_no + "'"))
                                {
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.Connection = con1;
                                    con1.Open();
                                    using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                                    {
                                        sdr1.Read();

                                        //- Status activity
                                        if (sdr1["utm_hr_stat"].ToString().Equals("True"))
                                            statusText.Text = "Active";
                                        else
                                            statusText.Text = "Not-active";

                                        //- Parse data
                                        getMatricNo.Text = sdr1["staff_id"].ToString();
                                        getFacDep.Text = sdr1["department"].ToString();
                                        getStat.Text = "UTM-HR";
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
                                Debug.WriteLine("Database execution successful");
                            }
                        }
                        else if (category.Equals("Public"))
                        {
                            //- Display note
                            note.Text = "The basic information is registered as public";

                            //- DB Exception-Error handling
                            try
                            {
                                //- Get Query
                                using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient_public WHERE fk_ic_no = '" + ic_no + "'"))
                                {
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.Connection = con1;
                                    con1.Open();
                                    using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                                    {
                                        sdr1.Read();

                                        //- Status activity
                                        if (sdr1["public_stat"].ToString().Equals("True"))
                                            statusText.Text = "Not-active";

                                        //- Parse data
                                        getStat.Text = "";
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
                                Debug.WriteLine("Database execution successful");
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
                GetLatestQueueInfo();

                if (!IsPostBack)
                {
                    BindGrid();
                }
            }
        }

        private void BindGrid()
        {
            //- DB Exception-Error handling
            try
            {
                string query = "SELECT clinical_info.clinical_date, emc.emc_url, pku_staff.s_name " +
                               "FROM clinical_info " +
                               "LEFT OUTER JOIN emc " +
                               "ON clinical_info.fk_p_ic_no = emc.fk_p_ic_no " +
                               "LEFT OUTER JOIN pku_staff " +
                               "ON clinical_info.fk_p_ic_no = pku_staff.s_ic_no " +
                               "WHERE clinical_info.fk_p_ic_no = '"+getIcNo.Text+"'";

                //- Retrieve Query: TISMADB
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.HasRows)
                    {
                        //- If records available
                        ClinicalHistoryTable.DataSource = sdr;
                        ClinicalHistoryTable.DataBind();
                    }
                    else
                    {
                        //- If no records found
                        DataTable dt = new DataTable();
                        ClinicalHistoryTable.DataSource = dt;
                        ClinicalHistoryTable.DataBind();
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
            ClinicalHistoryTable.UseAccessibleHeader = true;
            ClinicalHistoryTable.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void DeleteConfirmation(object sender, EventArgs e)
        {
            ModalPopupMessage.Show();
        }

        protected void DeleteFromTisma(object sender, EventArgs e)
        {
            //- DB Exception-Error handling
            try
            {
                //- Delete Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr1))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM qms WHERE fk_ic_no = '" + getIcNo.Text + "'", con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                    }
                }
                string constr2 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr2))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM clinical_info WHERE fk_ic_no = '" + getIcNo.Text + "'", con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                    }
                }
                string constr3 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr3))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM emc WHERE fk_p_ic_no = '" + getIcNo.Text + "'", con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                    }
                }
                string constr4 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr4))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM patient WHERE p_ic_no = '" + getIcNo.Text + "'", con))
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
                Debug.WriteLine("Database execution successful");
            }
            Response.Redirect("Registration.aspx");
        }

        protected void GenerateQueue(object sender, EventArgs e)
        {
            //- Step 1: Set today's date
            DateTime dateGenerated = DateTime.Now;
            Debug.WriteLine(dateGenerated.ToString("dd-MM-yyyy HH:mm:ss"));

            //- Step 2: Set expired date at
            DateTime dateExpired = DateTime.Today.AddDays(1);
            dateExpired = new DateTime(dateExpired.Year, dateExpired.Month, dateExpired.Day, 0, 0, 0);
            Debug.WriteLine(dateExpired.ToString("dd-MM-yyyy HH:mm:ss"));

            //- Step 3: Create Queue No.
            int queueNo = 1;
            string queueStr = ConvertQueueNo(queueNo);
            //-- Check if Queue No for current date is exist or not
            while (CheckIsQueueNotExist(queueStr, dateGenerated).Equals(false))
            {
                //-- If exist (the CheckIsQueueNotExist return false)
                queueNo += 1;
                queueStr = ConvertQueueNo(queueNo);
            }
            Debug.WriteLine(CheckIsQueueNotExist(ConvertQueueNo(queueNo), dateGenerated).Equals(true));
            Debug.WriteLine(ConvertQueueNo(queueNo));

            //- Step 4: Database INSERT query
            //-- DB Exception-Error handling
            try
            {
                //-- Insert Query
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("AddToQMS", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //-- Insert to table 'qms'
                cmd.Parameters.AddWithValue("@QueueNo", ConvertQueueNo(queueNo));
                cmd.Parameters.AddWithValue("@DateGenerated", dateGenerated);
                cmd.Parameters.AddWithValue("@DateExpired", dateExpired);
                cmd.Parameters.AddWithValue("@IsKeyIn", 0);
                cmd.Parameters.AddWithValue("@IsCheckIn", 0);
                cmd.Parameters.AddWithValue("@IsExpired", 0);
                cmd.Parameters.AddWithValue("@IcNo", getIcNo.Text);

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
                Debug.WriteLine("Database execution successful");
            }

            //- Pass data to front
            getLatestNo.Text = "";
            getLastVisited.Text = "";
            getQueueNo.Text = queueStr;

            //- Show and hide several div
            beforeGenerate.Visible = false;
            afterGenerate.Visible = true;
            ModalPopupGenerateQueue.Show();
        }

        public void GetLatestQueueInfo()
        {
            //- Get today's date
            DateTime today = DateTime.Now;

            //- DB Exception-Error handling
            try
            {
                //- Get Query
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 queue_no FROM qms WHERE date_generated = '"+today.ToString("yyyyMMdd")+"' ORDER BY queue_no DESC"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.Read())
                                getLatestNo.Text = sdr["queue_no"].ToString();
                            else
                                getLatestNo.Text = "[No queue number generated yet]";
                        }
                        con.Close();
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 date_generated FROM qms WHERE fk_ic_no = '" + getIcNo.Text + "' ORDER BY fk_ic_no DESC"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.Read())
                            {
                                DateTime.TryParse(sdr["date_generated"].ToString(), out DateTime dateGenerated);
                                getLastVisited.Text = dateGenerated.ToString("dd/MM/yyyy");
                            }
                            else
                                getLastVisited.Text = "- (First Time Visit)";
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
                Debug.WriteLine("Database execution successful");
            }
        }

        protected void UpdateToTisma(object sender, EventArgs e)
        {
            //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
            DateTime.TryParse(getDob.Text, out DateTime dob);
            DateTime dobToAge = Convert.ToDateTime(dob);
            int age = DateTime.Now.AddYears(-dobToAge.Year).Year;

            //- Calculate the current session when the patient data is updated
            String session = DateTime.Now.AddYears(-1).ToString("yyyy") + "/" + DateTime.Now.ToString("yyyy");

            //- DB Exception-Error handling
            try
            {
                //- Update Query
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    //- Table 'patient'
                    using (SqlCommand cmd = new SqlCommand("UPDATE patient " +
                                                           "SET p_passport_no = '" + getPassportNo.Text + "', " +
                                                               "p_age = '" + age + "', " +
                                                               "p_marital_stat = '" + getMaritalStat.SelectedValue + "', " +
                                                               "p_religion = '" + getReligion.SelectedValue + "', " +
                                                               "p_tel_no = '" + getPhone.Text + "', " +
                                                               "p_email = '" + getEmail.Text + "', " +
                                                               "p_designation = '" + getDesignation.Text + "', " +
                                                               "p_session = '" + session + "', " +
                                                               "p_address = '" + getAddress.Text + "', " +
                                                               "p_remarks = '" + getRemarks.Text + "' " +
                                                           "WHERE p_ic_no = '" + getIcNo.Text + "'", con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                    }
                    //- If category is 'Student', update 'semester'
                    if (getCategory.Text.Equals("Student"))
                    {
                        //- DB Exception-Error handling
                        try
                        {
                            //- Table 'patient_student'
                            using (SqlCommand cmd = new SqlCommand("UPDATE patient_student SET semester = '" + getSem.Text + "' WHERE p_ic_no = '" + getIcNo.Text + "'", con))
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                                con.Dispose();
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
        }

        public static bool CheckIsQueueNotExist(String queueStr, DateTime dateGenerated)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckIsQueueNotExist", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@QueueNo", queueStr);
            cmd.Parameters.AddWithValue("@DateGenerated", dateGenerated);
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static string ConvertQueueNo(int queueNo)
        {
            String queueStr;
            if (queueNo <= 9)
                queueStr = "00" + queueNo.ToString();
            else if (queueNo >= 10 && queueNo <= 99)
                queueStr = "0" + queueNo.ToString();
            else
                queueStr = queueNo.ToString();
            return queueStr;
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