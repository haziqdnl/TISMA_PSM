using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using static TISMA_PSM.ControllerStaff;
using static TISMA_PSM.ControllerUtmHr;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Add_New_Staff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Verify user ACL based on session: 404 error
            if (!Convert.ToString(Session["UserRole"]).Equals("Admin"))
                Response.Redirect("PageNotFound.aspx");

            //- Get encrypted IC no. from URL param and decrypt it
            string icNo = DecryptURL(Request.QueryString["pid"]);

            //- Validate if decrypted IC no. is empty or null: 404 Error
            if (string.IsNullOrEmpty(icNo))
                Response.Redirect("PageNotFound.aspx");

            //- Validate if decrypted IC no. not exist: 404 Error
            if (CheckIcNoUtmHrNotExist(icNo).Equals(true))
                Response.Redirect("PageNotFound.aspx");

            //- Validate if staff already added to TISMA
            if (CheckIsStaffAddedToTisma(icNo).Equals(true))
                ModalPopupMessage.Show();

            if (!this.IsPostBack)
            {
                DisplayUtmHrInfo(icNo);
            }
        }

        protected void AddToTisma(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(getTismaRoleDdl.SelectedValue) && !getTismaRoleDdl.SelectedValue.Equals("Select"))
            {
                //- Object UTM-HR model
                ModelUtmHr hr = GetUtmHrInfoByIcNo(getIcNo.Text);

                //- Generate salt and hash password
                string[] encryptedUrl = Hashing(hr.sIcNo);
                string passwordHashed = encryptedUrl[0];
                string passwordSalt = encryptedUrl[1];

                //- DB Exception/Error handling
                try
                {
                    //- Identify TISMA selected ole
                    string role = getTismaRoleDdl.SelectedValue, spFunc = "";
                    if (role.Equals("Admin"))
                        spFunc = "AddToTismaPkuAdmin";
                    else if (role.Equals("Medical Officer"))
                        spFunc = "AddToTismaPkuMO";
                    else if (role.Equals("Receptionist"))
                        spFunc = "AddToTismaPkuReceptionist";
                    else
                        Response.Redirect("InternalServerError.aspx");

                    using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                    {
                        con.Open();
                        //- Insert Query
                        using (SqlCommand cmd = new SqlCommand(spFunc, con) { CommandType = CommandType.StoredProcedure })
                        {
                            //- Insert to table 'pku_staff'
                            cmd.Parameters.AddWithValue("@IcNo", hr.sIcNo);
                            cmd.Parameters.AddWithValue("@Passport", hr.sPassportNo);
                            cmd.Parameters.AddWithValue("@AccNo", getAccNo.Text);
                            cmd.Parameters.AddWithValue("@Username", getUsername.Text);
                            cmd.Parameters.AddWithValue("@Password", passwordHashed);
                            cmd.Parameters.AddWithValue("@PasswordSalt", passwordSalt);
                            cmd.Parameters.AddWithValue("@StaffId", hr.sStaffId);
                            cmd.Parameters.AddWithValue("@TelNo", hr.sTelNo);
                            cmd.Parameters.AddWithValue("@Email", hr.sEmail);
                            cmd.Parameters.AddWithValue("@Name", hr.sName);
                            cmd.Parameters.AddWithValue("@Dob", DateTime.ParseExact(getDob.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                            cmd.Parameters.AddWithValue("@Age", hr.sAge);
                            cmd.Parameters.AddWithValue("@Gender", hr.sGender);
                            cmd.Parameters.AddWithValue("@Marital", hr.sMaritalStat);
                            cmd.Parameters.AddWithValue("@Religion", hr.sReligion);
                            cmd.Parameters.AddWithValue("@Race", hr.sRace);
                            cmd.Parameters.AddWithValue("@Nationality", hr.sNationality);
                            cmd.Parameters.AddWithValue("@Address", hr.sAddress);
                            cmd.Parameters.AddWithValue("@Designation", hr.sDesignation);
                            cmd.Parameters.AddWithValue("@Department", hr.sDepartment);
                            cmd.Parameters.AddWithValue("@Session", hr.sSession);
                            cmd.Parameters.AddWithValue("@Category", hr.sCategory);
                            cmd.Parameters.AddWithValue("@Branch", hr.sBranch);

                            //- Insert to table 'pku_admin' or 'pku_medical_officer' or 'pku_receptionist'
                            cmd.Parameters.AddWithValue("@Role", 1);

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
        }

        private void DisplayUtmHrInfo(string icNo)
        {
            //- Object UTM-HR model
            ModelUtmHr hr = GetUtmHrInfoByIcNo(icNo);

            //- Parse and render data
            getBranch.SelectedValue = hr.sBranch;
            getCategory.Text = hr.sCategory;
            getName.Text = hr.sName;
            getStaffId.Text = hr.sStaffId;
            getIcNo.Text = hr.sIcNo;
            getPassportNo.Text = hr.sPassportNo;
            getDob.Text = hr.sDob;
            getAge.Text = hr.sAge.ToString();
            getGender.SelectedValue = hr.sGender;
            getMaritalStat.SelectedValue = hr.sMaritalStat;
            getReligion.SelectedValue = hr.sReligion;
            getRace.SelectedValue = hr.sRace;
            getNation.Text = hr.sNationality;
            getPhone.Text = hr.sTelNo;
            getEmail.Text = hr.sEmail;
            getDesignation.Text = hr.sDesignation;
            getFacDep.Text = hr.sDepartment;
            getAddress.Text = hr.sAddress;
            getSession.Text = hr.sSession;

            //- Generate a username based on email name
            MailAddress addr = new MailAddress(hr.sEmail);
            string username = addr.User;
            getUsername.Text = username;

            //- Generate account no.
            getAccNo.Text = GenerateAccNoStaff();
        }

        private static string GenerateAccNoStaff()
        {
            //- Step 1: Init Account No pattern
            string accNo = "STAFF";

            //- Step 2: Generate Date pattern and append to accNo
            string today = DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
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
                return accNo;
            else
                return GenerateAccNoStaff();
        }
    }
}