using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.ControllerPatient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Registration_New_Public : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //- Auto generate and render several data
                getNationDdl.DataSource = CountryList();
                getNationDdl.DataBind();
                getNationDdl.Items.Insert(0, "-Select-");
                getAccNo.Text = GenerateAccNoPatient("Public");
            }
        }

        protected void AddToTisma(object sender, EventArgs e)
        {
            //- Validate if Branch ddl is not selected
            if (getBranchDdl.SelectedValue.Equals("Select"))
                goto done;

            //- Validate if Name is empty or name pattern is false
            if (string.IsNullOrEmpty(getName.Text) || CheckPatternIsName(getName.Text).Equals(false))
                goto done;

            //- Validate if IC No. is empty or IC pattern is false
            if (string.IsNullOrEmpty(getIcNo.Text) || CheckPatternIsIcNo(getIcNo.Text).Equals(false))
                goto done;

            //- Validate if DOB is empty or null
            if (string.IsNullOrEmpty(getDob.Text))
                goto done;

            //- Validate if Gender ddl is not selected
            if (getGenderDdl.SelectedValue.Equals("Select"))
                goto done;

            //- Validate if Marital ddl is not selected
            if (getMaritalStatDdl.SelectedValue.Equals("Select"))
                goto done;

            //- Validate if Religion ddl is not selected
            if (getReligionDdl.SelectedValue.Equals("Select"))
                goto done;

            //- Validate if Race ddl is not selected
            if (getRaceDdl.SelectedValue.Equals("Select"))
                goto done;

            //- Validate if Nationality ddl is not selected
            if (getNationDdl.SelectedValue.Equals("-Select-"))
                goto done;

            //- Validate if Phone is empty or Phone pattern is false
            if (string.IsNullOrEmpty(getPhone.Text) || CheckPatternIsPhoneNo(getPhone.Text).Equals(false))
                goto done;

            //- Validate if Email is empty or Email pattern is false
            if (string.IsNullOrEmpty(getEmail.Text) || CheckPatternIsEmail(getEmail.Text).Equals(false))
                goto done;

            //- Validate if Designation is empty or text pattern is false
            if (string.IsNullOrEmpty(getDesignation.Text) || CheckPatternIsMultiLineText(getDesignation.Text).Equals(false))
                goto done;

            //- Validate if Address is empty or text pattern is false
            if (string.IsNullOrEmpty(getAddress.Text) || CheckPatternIsMultiLineText(getAddress.Text).Equals(false))
                goto done;

            //- Validate if Passport not empty but passport pattern is false
            if (!string.IsNullOrEmpty(getPassportNo.Text))
                if (CheckPatternIsPassport(getPassportNo.Text).Equals(false))
                    goto done;

            //- Validate if Remarks not empty but text pattern is false
            if (!string.IsNullOrEmpty(getRemarks.Text))
                if (CheckPatternIsMultiLineText(getRemarks.Text).Equals(false))
                    goto done;

            if (CheckIsPatientAddedToTisma(getIcNo.Text).Equals(true))
                ModalPopupMessage.Show();
            else
            {
                //- Calculate age
                DateTime dob = DateTime.ParseExact(getDob.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                int age = DateTime.Now.AddYears(-dob.Year).Year;

                //- Calculate session
                string session = DateTime.Now.AddYears(-1).ToString("yyyy", CultureInfo.InvariantCulture) + "/" + DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);

                //- DB Exception/Error handling
                try
                {
                    using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                    {
                        con.Open();
                        //- Insert data
                        using (SqlCommand cmd = new SqlCommand("AddToTismaPublic", con) { CommandType = CommandType.StoredProcedure })
                        {
                            //- Insert to table 'patient'
                            cmd.Parameters.AddWithValue("@pIcNo", getIcNo.Text);
                            cmd.Parameters.AddWithValue("@pAccNo", GenerateAccNoPatient("Public"));
                            cmd.Parameters.AddWithValue("@pPassport", getPassportNo.Text);
                            cmd.Parameters.AddWithValue("@pTelNo", getPhone.Text);
                            cmd.Parameters.AddWithValue("@pEmail", getEmail.Text);
                            cmd.Parameters.AddWithValue("@pName", getName.Text);
                            cmd.Parameters.AddWithValue("@Dob", DateTime.ParseExact(getDob.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                            cmd.Parameters.AddWithValue("@pAge", age);
                            cmd.Parameters.AddWithValue("@pGender", getGenderDdl.SelectedValue);
                            cmd.Parameters.AddWithValue("@pMarital", getMaritalStatDdl.SelectedValue);
                            cmd.Parameters.AddWithValue("@pReligion", getReligionDdl.SelectedValue);
                            cmd.Parameters.AddWithValue("@pRace", getRaceDdl.SelectedValue);
                            cmd.Parameters.AddWithValue("@pNationality", getNationDdl.SelectedValue);
                            cmd.Parameters.AddWithValue("@pAddress", getAddress.Text);
                            cmd.Parameters.AddWithValue("@pDesignation", getDesignation.Text);
                            cmd.Parameters.AddWithValue("@pCategory", getCategory.Text);
                            cmd.Parameters.AddWithValue("@pSession", session);
                            cmd.Parameters.AddWithValue("@pBranch", getBranchDdl.SelectedValue);
                            cmd.Parameters.AddWithValue("@pRemarks", getRemarks.Text);

                            //- Insert to table 'patient_public'
                            cmd.Parameters.AddWithValue("@pPublicStat", 1); // 1 - True

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    //- Display handling-error message
                    SqlExceptionMsg(ex);
                }
                Response.Redirect("Registration.aspx");
            }
            done:;
        }

        public static List<string> CountryList()
        {
            //- Creating List
            List<string> CultureList = new List<string>();

            //- Getting the specific CultureInfo from CultureInfo class
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo getCulture in getCultureInfo)
            {
                RegionInfo GetRegionInfo = new RegionInfo(getCulture.LCID);
                //- Add each country Name into the arraylist
                if (!(CultureList.Contains(GetRegionInfo.EnglishName)))
                {
                    CultureList.Add(GetRegionInfo.EnglishName);
                }
            }
            //- Sorting array by using sort method to get countries in order
            CultureList.Sort();
            return CultureList;
        }
    }
}