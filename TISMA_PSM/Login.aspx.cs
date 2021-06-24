using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TISMA_PSM
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SignIn(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(getUsername.Text) && String.IsNullOrEmpty(getPassword.Text))
                goto done;

            string passwordFromDB = "", salt = "", sessionRole = "", icNo = "", userAccNo = "", userName = "";
            //- DB Exception-Error handling
            try
            {
                //- Get Query
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr))
                {
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM pku_staff WHERE s_username = '" + getUsername.Text + "'", con1))
                    {
                        con1.Open();
                        SqlDataReader sdr1 = cmd1.ExecuteReader();
                        if ( sdr1.HasRows )
                        {
                            sdr1.Read();
                            passwordFromDB = sdr1["s_password"].ToString();
                            salt = sdr1["s_password_salt"].ToString();
                            icNo = sdr1["s_ic_no"].ToString();
                            userAccNo = sdr1["s_account_no"].ToString();
                            userName = sdr1["s_name"].ToString();
                            UsernameIncorrectMsg.Visible = false;
                        }
                        else
                        {
                            UsernameIncorrectMsg.Visible = true;
                            con1.Close();
                            goto done;
                        }
                        con1.Close();
                    }
                }
                using (SqlConnection con2 = new SqlConnection(constr))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT * FROM pku_admin WHERE fk_ic_no = '" + icNo + "' AND admin_role = '1'", con2))
                    {
                        con2.Open();
                        SqlDataReader sdr2 = cmd2.ExecuteReader();
                        if (sdr2.HasRows)
                            sessionRole = "Admin";
                        con2.Close();
                    }
                }
                using (SqlConnection con3 = new SqlConnection(constr))
                {
                    using (SqlCommand cmd3 = new SqlCommand("SELECT * FROM pku_medical_officer WHERE fk_ic_no = '" + icNo + "' AND mo_role = '1'", con3))
                    {
                        con3.Open();
                        SqlDataReader sdr3 = cmd3.ExecuteReader();
                        if (sdr3.HasRows)
                            sessionRole = "Medical Officer";
                        con3.Close();
                    }
                }
                using (SqlConnection con4 = new SqlConnection(constr))
                {
                    using (SqlCommand cmd4 = new SqlCommand("SELECT * FROM pku_receptionist WHERE fk_ic_no = '" + icNo + "' AND reception_role = '1'", con4))
                    {
                        con4.Open();
                        SqlDataReader sdr1 = cmd4.ExecuteReader();
                        if (sdr1.HasRows)
                            sessionRole = "Receptionist";
                        con4.Close();
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
                Debug.WriteLine("DB Execution Success: ");
            }

            if (Crypto.VerifyHashedPassword(passwordFromDB, (salt + getPassword.Text)).Equals(true))
            {
                if (sessionRole.Equals("Medical Officer"))
                {
                    Application.Lock();
                    int applicationCount = Convert.ToInt32(Application["MOSessionCount"]);
                    applicationCount++;
                    Application["MOSessionCount"] = applicationCount;
                    Application.UnLock();
                }

                Session["UserName"] = userName;
                Session["UserRole"] = sessionRole;
                Session["UserAccNo"] = userAccNo;
                Session["UserGranted"] = "true";
                Response.Redirect("Dashboard.aspx");
                PasswordIncorrectMsg.Visible = false;
            }
            else
            {
                PasswordIncorrectMsg.Visible = true;
                goto done;
            }
        done:;
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