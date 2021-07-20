using System;
using System.Data.SqlClient;
using static TISMA_PSM.ControllerStaff;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Staff_Info : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Verify user ACL based on session: 404 error
            if (!Convert.ToString(Session["UserRole"]).Equals("Admin"))
                Response.Redirect("PageNotFound.aspx");

            //- Get encrypted account no. from URL param and decrypt it
            string accNo = DecryptURL(Request.QueryString["accno"]);

            //- Validate if decrypted account no. is empty or null: 404 Error
            if (string.IsNullOrEmpty(accNo))
                Response.Redirect("PageNotFound.aspx");

            //- Validate if decrypted account no. not exist: 404 Error
            if (CheckAccStaffNotExist(accNo).Equals(true))
                Response.Redirect("PageNotFound.aspx");

            if (!this.IsPostBack)
            {
                DisplayStaffInfo(accNo);
            }
        }

        protected void DeleteConfirmation(object sender, EventArgs e)
        {
            ModalPopupMessageDelete.Show();
        }

        protected void DeleteFromTisma(object sender, EventArgs e)
        {
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Delete Query
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM pku_staff WHERE s_ic_no = '" + getIcNo.Text + "'", con))
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
            Response.Redirect("Staff.aspx");
        }

        protected void UpdateToTisma(object sender, EventArgs e)
        {
            //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
            DateTime.TryParse(getDob.Text, out DateTime dob);
            int age = DateTime.Now.AddYears(-dob.Year).Year;

            //- Calculate current session when the staff data is updated
            String session = DateTime.Now.AddYears(-1).ToString("yyyy", System.Globalization.CultureInfo.InvariantCulture) + "/" + DateTime.Now.ToString("yyyy", System.Globalization.CultureInfo.InvariantCulture);

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
                getValidationMsg.Text = "The updated phone number already in use.";
                ModalPopupMessageValidation.Show();
            }
            else
            {
                //- DB Exception/Error handling
                try
                {
                    using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                    {
                        con.Open();
                        //- Update Query
                        using (SqlCommand cmd = new SqlCommand("UPDATE pku_staff SET s_passport_no = '" + getPassportNo.Text + "', " +
                                                                                    "s_marital_stat = '" + getMaritalStat.SelectedValue + "', " +
                                                                                    "s_religion = '" + getReligion.SelectedValue + "', " +
                                                                                    "s_tel_no = '" + getPhone.Text + "', " +
                                                                                    "s_address = '" + getAddress.Text + "', " +
                                                                                    "s_session = '" + session + "', " +
                                                                                    "s_age = '" + age + "' " +
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
                ModalPopupMessageUpdate.Show();
            }
        done:;
        }

        private void DisplayStaffInfo(string accNo)
        {
            //- Object Staff model
            ModelStaff staff = GetStaffInfoByAccNo(accNo);

            //- Parse and render data
            getBranch.SelectedValue = staff.sBranch;
            getCategory.Text = staff.sCategory;
            getAccNo.Text = staff.sAccountNo;
            displayAccNo.Text = staff.sAccountNo;
            getUsername.Text = staff.sUsername;
            displayUsername.Text = staff.sUsername;
            getName.Text = staff.sName;
            getStaffId.Text = staff.sStaffId;
            getIcNo.Text = staff.sIcNo;
            getPassportNo.Text = staff.sPassportNo;
            getDob.Text = staff.sDob;
            getAge.Text = staff.sAge.ToString();
            getGender.SelectedValue = staff.sGender;
            getMaritalStat.SelectedValue = staff.sMaritalStat;
            getReligion.SelectedValue = staff.sReligion;
            getRace.SelectedValue = staff.sRace;
            getNation.Text = staff.sNationality;
            getPhone.Text = staff.sTelNo;
            getEmail.Text = staff.sEmail;
            getDesignation.Text = staff.sDesignation;
            getFacDep.Text = staff.sDepartment;
            getAddress.Text = staff.sAddress;
            getSession.Text = staff.sSession;

            if (staff.adminRole.Equals(true))
                getTismaRoleDdl.SelectedValue = "Admin";
            else if (staff.moRole.Equals(true))
                getTismaRoleDdl.SelectedValue = "Medical Officer";
            else if (staff.receptionistRole.Equals(true))
                getTismaRoleDdl.SelectedValue = "Receptionist";
            else
                getTismaRoleDdl.SelectedValue = "Select";
        }
    }
}