using System;
using System.Data.SqlClient;
using System.Web.Helpers;
using static TISMA_PSM.ControllerStaff;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Check user session if the user has logged in to skip Login page
            if (Convert.ToString(Session["UserGranted"]).Equals("true"))
                Response.Redirect("Dashboard.aspx");
        }

        protected void SignIn(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(getUsername.Text) && !String.IsNullOrEmpty(getPassword.Text))
            {
                if (CheckPatternIsUsername(getUsername.Text).Equals(true) && CheckPatternIsPassword(getPassword.Text).Equals(true))
                {
                    string passwordFromDB = "", salt = "", sessionRole = "", icNo = "", userAccNo = "", userName = "";

                    //- DB Exception/Error handling
                    try
                    {
                        using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                        {
                            con.Open();
                            //- Get user info
                            using (SqlCommand cmd = new SqlCommand("SELECT * FROM pku_staff WHERE s_username = '" + getUsername.Text + "'", con))
                            {
                                using (SqlDataReader sdr = cmd.ExecuteReader())
                                {
                                    if (sdr.HasRows)
                                    {
                                        //- Parse data
                                        sdr.Read();
                                        passwordFromDB = sdr["s_password"].ToString();
                                        salt = sdr["s_password_salt"].ToString();
                                        icNo = sdr["s_ic_no"].ToString();
                                        userAccNo = sdr["s_account_no"].ToString();
                                        userName = sdr["s_name"].ToString();
                                        UsernameIncorrectMsg.Visible = false;
                                    }
                                    else
                                    {
                                        UsernameIncorrectMsg.Visible = true;
                                        goto done;
                                    }
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        //- Display exception message
                        SqlExceptionMsg(ex);
                    }

                    //- Verify user role
                    if (CheckIsStaffAdmin(icNo).Equals(true))
                        sessionRole = "Admin";
                    else if (CheckIsStaffMO(icNo).Equals(true))
                        sessionRole = "Medical Officer";
                    else if (CheckIsStaffReceptionist(icNo).Equals(true))
                        sessionRole = "Receptionist";
                    else
                        Response.Redirect("InternalServerError.aspx"); //- In case no Role is identified for this user: Error 500

                    if (Crypto.VerifyHashedPassword(passwordFromDB, (salt + getPassword.Text)).Equals(true))
                    {
                        //- Accumulate online MO
                        if (sessionRole.Equals("Medical Officer"))
                        {
                            Application.Lock();
                            int applicationCount = Convert.ToInt32(Application["MOSessionCount"]);
                            applicationCount++;
                            Application["MOSessionCount"] = applicationCount;
                            Application.UnLock();
                        }

                        //- Assign session values
                        Session["UserName"] = userName;
                        Session["UserRole"] = sessionRole;
                        Session["UserAccNo"] = userAccNo;
                        Session["UserGranted"] = "true";
                        Response.Redirect("Dashboard.aspx");
                    }
                    else
                        PasswordIncorrectMsg.Visible = true;
                }
            }
        done:;
        }
    }
}