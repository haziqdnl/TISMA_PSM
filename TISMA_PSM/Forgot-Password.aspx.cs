using System;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.ControllerStaff;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Forgot_Password : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ConfirmForgotPassword(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(getUsername.Text))
            {
                if (CheckPatternIsUsername(getUsername.Text).Equals(true))
                {
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
                                        string userEmail = sdr["s_email"].ToString();
                                        string resetPasswordToken = GenerateToken();
                                        while (CheckResetPasswordTokenNotExist(resetPasswordToken).Equals(false))
                                        {
                                            GenerateToken();
                                        }
                                        string resetPasswordUrl = GetUrlResetPassword() + resetPasswordToken;
                                        SaveForgotPasswordInfo(resetPasswordToken, userEmail);
                                        SendEmailResetPassword(resetPasswordUrl, userEmail);

                                        UsernameIncorrectMsg.Visible = false;
                                        ForgotPasswordRequest.Visible = false;
                                        ForgotPasswordSuccess.Visible = true;
                                    }
                                    else
                                        UsernameIncorrectMsg.Visible = true;
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        //- Display exception message
                        SqlExceptionMsg(ex);
                    }
                }
            }
        }

        private static void SaveForgotPasswordInfo(string token, string userEmail)
        {
            //- DB Exception-Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Insert Query
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO forgot_password (token, date_created, date_expired, email) VALUES (@Token, @DateCreated, @DateExpired, @UserEmail)", con))
                    {
                        cmd.Parameters.Add("@Token", SqlDbType.NVarChar, 70).Value = token;
                        cmd.Parameters.Add("@DateCreated", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@DateExpired", SqlDbType.DateTime).Value = DateTime.Now.AddHours(1);
                        cmd.Parameters.Add("@UserEmail", SqlDbType.NVarChar, 50).Value = userEmail;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display exception message
                SqlExceptionMsg(ex);
            }
        }

        private static void SendEmailResetPassword(string resetPasswordUrl, string userEmail)
        {
            string subject = "TISMA Password Reset";
            string body = @"<html>
                                <body style=""text-align:center; background-color: #fff"">
                                    <p style=""font-size:40px; font-weight:700; color:red"">TISMA<span style=""color: grey"">PKU</span></p>
                                    <h1>Forgot Your Password?</h1>
                                    <p>We were told that you forgot your password. To reset your password, please click on the link provided below:</b></p>
                                    <a href='" + resetPasswordUrl + @"' style=""color:blue; font-size:18px; font-weight:700"">RESET MY PASSWORD</a>
                                    <p style=""color:grey; font-size: 11px"">This link is valid for 1 hour from the time of request</p>
                                    <br />
                                    <p>If you didn't request a password reset, you can ignore this email</p>
                                    <br /><hr />
                                    <p style=""color:grey; font-size: 11px"">This is an automatically generated email, please do not reply.</p>
                                </body>
                            </html>";

            SendEmail(subject, body, userEmail);
        }
    }
}