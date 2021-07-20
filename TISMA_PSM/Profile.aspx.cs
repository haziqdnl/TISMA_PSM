using System;
using System.Data.SqlClient;
using System.Web.Helpers;
using static TISMA_PSM.ControllerStaff;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Check user session and retrieve user info
            if (!Convert.ToString(Session["UserGranted"]).Equals("true"))
                Response.Redirect("Login.aspx");

            //- Validate if this user account exist
            if (CheckAccStaffNotExist(Convert.ToString(Session["UserAccNo"])).Equals(true))
                Response.Redirect("PageNotFound.aspx");

            if (!IsPostBack)
                DisplayProfileInfo(Convert.ToString(Session["UserAccNo"]), Convert.ToString(Session["UserRole"]));
        }

        protected void ChangePassword(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(getCurrentPassword.Text) && !string.IsNullOrEmpty(getNewPassword.Text) && !string.IsNullOrEmpty(getConfirmNewPassword.Text))
            {
                if (VerifyCurrentPassword(getAccNo.Text, getCurrentPassword.Text).Equals(true))
                {
                    PasswordIncorrectMsg.Visible = false;

                    if (CheckPatternIsNewPassword(getNewPassword.Text).Equals(true))
                    {
                        if (getNewPassword.Text.Equals(getConfirmNewPassword.Text))
                        {
                            PasswordNotMatchMsg.Visible = false;

                            //- Produce salt and hash password
                            string[] encryptedUrl = Hashing(getNewPassword.Text);
                            string passwordHashed = encryptedUrl[0];
                            string passwordSalt = encryptedUrl[1];

                            //- DB Exception-Error handling
                            try
                            {
                                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                                {
                                    con.Open();
                                    //- Update Query
                                    using (SqlCommand cmd2 = new SqlCommand("UPDATE pku_staff SET s_password = '" + passwordHashed + "', s_password_salt = '" + passwordSalt + "' WHERE s_account_no = '" + getAccNo.Text + "'", con))
                                    {
                                        cmd2.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (SqlException ex)
                            {
                                //- Display handling-error message
                                SqlExceptionMsg(ex);
                            }
                            getMsgBody.Text = "Your new password has been saved!";
                            ModalPopupMessage.Show();
                        }
                        else
                            PasswordNotMatchMsg.Visible = true;
                    }
                }
                else
                    PasswordIncorrectMsg.Visible = true;
            }
        }

        protected void UpdateToTisma(object sender, EventArgs e)
        {
            //- Validate if passport not empty and if passport pattern is false
            if (!string.IsNullOrEmpty(getPassportNo.Text))
                if (CheckPatternIsPassport(getPassportNo.Text).Equals(false))
                    goto done;

            //- Validate if marital stat. ddl is not selected
            if (getMaritalStat.SelectedValue.Equals("Select"))
                goto done;

            //- Validate if religion ddl is not selected
            if (getReligion.SelectedValue.Equals("Select"))
                goto done;

            //- Validate if phone is empty or phone pattern is false
            if (string.IsNullOrEmpty(getPhone.Text) || CheckPatternIsPhoneNo(getPhone.Text).Equals(false))
                goto done;

            //- Validate if address is empty or text pattern is false
            if (string.IsNullOrEmpty(getAddress.Text) || CheckPatternIsMultiLineText(getAddress.Text).Equals(false))
                goto done;

            //- Get this staff's current phone no
            ModelStaff staff = GetStaffInfoByAccNo(getAccNo.Text);
            if (string.IsNullOrEmpty(staff.sTelNo))
                Response.Redirect("InternalServerError.aspx");

            //- Identify if data update has changes on the current phone no. and changed phone no. must not exist yet in db
            if (!getPhone.Text.Equals(staff.sTelNo) && CheckIsPhoneStaffExist(getPhone.Text).Equals(true))
            {
                getMsgHeader.Text = "FAILED to update.";
                getMsgBody.Text = "The updated phone number already in use.";
                ModalPopupMessage.Show();
            }
            else
            {
                //- DB Exception-Error handling
                try
                {
                    using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                    {
                        con.Open();
                        //- Update Query
                        using (SqlCommand cmd = new SqlCommand("UPDATE pku_staff SET s_passport_no = '" + getPassportNo.Text + "', s_tel_no = '" + getPhone.Text + "', " +
                                                                                    "s_address = '" + getAddress.Text + "', s_religion = '" + getReligion.SelectedValue + "', " +
                                                                                    "s_marital_stat = '" + getMaritalStat.SelectedValue + "' " +
                                                                                    "WHERE s_ic_no = '" + getIcNo.Text + "'", con))
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
                getMsgBody.Text = "Your profile has been updated!";
                ModalPopupMessage.Show();
            }
        done:;
        }

        private static bool VerifyCurrentPassword(string accNo, string currentPassword)
        {
            string hashedPasswordDb = "", salt = "";
            //- DB Exception-Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM pku_staff WHERE s_account_no = '" + accNo + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();
                            hashedPasswordDb = sdr["s_password"].ToString();
                            salt = sdr["s_password_salt"].ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }

            if (Crypto.VerifyHashedPassword(hashedPasswordDb, (salt + currentPassword)).Equals(true))
                return true;
            else
                return false;
        }

        private void DisplayProfileInfo(string accNo, string role)
        {
            //- Get Staff Info
            ModelStaff staff = GetStaffInfoByAccNo(accNo);

            //- Parse data
            getAccNo.Text = accNo;
            getUsername.Text = staff.sUsername;
            getRole.Text = role;
            getSession.Text = staff.sSession;
            getName.Text = staff.sName;
            getDOB.Text = staff.sDob;
            getIcNo.Text = staff.sIcNo;
            getPassportNo.Text = staff.sPassportNo;
            getStaffId.Text = staff.sStaffId;
            getEmail.Text = staff.sEmail;
            getPhone.Text = staff.sTelNo;
            getAddress.Text = staff.sAddress;
            getDesignation.Text = staff.sDesignation;
            getFacDep.Text = staff.sDepartment;
            getGender.Text = staff.sGender;
            getRace.Text = staff.sRace;
            getNationality.Text = staff.sNationality;
            getReligion.SelectedValue = staff.sReligion;
            getMaritalStat.SelectedValue = staff.sMaritalStat;
        }
    }
}