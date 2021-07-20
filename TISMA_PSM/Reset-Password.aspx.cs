using System;
using System.Data.SqlClient;
using static TISMA_PSM.ControllerStaff;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Reset_Password : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Get reset password token from URL param
            string resetPasswordToken = Request.QueryString["token"];

            //- Validate if reset password token is empty or null: 404 Error
            if (string.IsNullOrEmpty(resetPasswordToken))
                Response.Redirect("PageNotFound.aspx");

            //- Validate if reset password token not exist: 404 Error
            if (CheckResetPasswordTokenNotExist(resetPasswordToken).Equals(true))
                Response.Redirect("PageNotFound.aspx");

            GetResetPasswordInfo(resetPasswordToken);
        }

        protected void ResetPassword(object sender, EventArgs e)
        {
            string newPassword = getNewPassword.Text;
            string confirmNewPassword = getConfirmNewPassword.Text;
            string resetPasswordToken = Request.QueryString["token"];
            string userEmail = "";

            if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmNewPassword))
            {
                if (CheckPatternIsNewPassword(newPassword).Equals(true))
                {
                    if (newPassword.Equals(confirmNewPassword))
                    {
                        //- Generate salt and hash password
                        string[] encryptedUrl = Hashing(newPassword);
                        string passwordHashed = encryptedUrl[0];
                        string passwordSalt = encryptedUrl[1];

                        //- DB Exception-Error handling
                        try
                        {
                            using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                            {
                                con.Open();
                                //- Get user email
                                using (SqlCommand cmd = new SqlCommand("SELECT * FROM forgot_password WHERE token = '" + resetPasswordToken + "'", con))
                                {
                                    using (SqlDataReader sdr = cmd.ExecuteReader())
                                    {
                                        if (sdr.HasRows)
                                        {
                                            //- Parse data
                                            sdr.Read();
                                            userEmail = sdr["email"].ToString();
                                        }
                                        else
                                            Response.Redirect("InternalServerError.aspx");
                                    }
                                }
                                //- Update Query: table 'forgot_password'
                                using (SqlCommand cmd = new SqlCommand("UPDATE forgot_password SET token_expired = '1', token_used = '1' WHERE token = '" + resetPasswordToken + "'", con))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                //- Update Query: table 'pku_staff'
                                using (SqlCommand cmd = new SqlCommand("UPDATE pku_staff SET s_password = '" + passwordHashed + "', s_password_salt = '" + passwordSalt + "' WHERE s_email = '" + userEmail + "'", con))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            //- Display handling-error message
                            SqlExceptionMsg(ex);
                        }
                        Response.Redirect("Reset-Password-Done.aspx");
                    }
                    else
                        PasswordNotMatchMsg.Visible = true;
                }
            }
        }

        private void GetResetPasswordInfo(string resetPasswordToken)
        {
            //- DB Exception-Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get reset password info
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM forgot_password WHERE token = '" + resetPasswordToken + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                //- Parse data
                                sdr.Read();
                                DateTime dateExpired = DateTime.Parse(sdr["date_expired"].ToString());
                                string tokenExpired = sdr["token_expired"].ToString();
                                string tokenUsed = sdr["token_used"].ToString();

                                //- Check token/link expiry (1)
                                if (tokenExpired.Equals("True"))
                                {
                                    ResetPasswordMain.Visible = false;
                                    ResetPasswordError.Visible = true;
                                    goto done;
                                }

                                //- Check token/link expiry (2)
                                if (DateTime.Now >= dateExpired)
                                {
                                    SetResetPasswordLinkExpired(resetPasswordToken);
                                    ResetPasswordMain.Visible = false;
                                    ResetPasswordError.Visible = true;
                                    goto done;
                                }

                                //- Check token used
                                if (tokenUsed.Equals("True"))
                                {
                                    ResetPasswordMain.Visible = false;
                                    ResetPasswordError.Visible = true;
                                    goto done;
                                }
                            }
                            else
                                Response.Redirect("InternalServerError.aspx");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        done:;
        }

        private static void SetResetPasswordLinkExpired(string resetPasswordToken)
        {
            //- DB Exception-Error handling
            try
            {
                //- Update Query
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Update Queury: table 'forgot_password'
                    using (SqlCommand cmd = new SqlCommand("UPDATE forgot_password SET token_expired = '1' WHERE token = '" + resetPasswordToken + "'", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }
    }
}