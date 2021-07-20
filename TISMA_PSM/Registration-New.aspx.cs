using System;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.ControllerPatient;
using static TISMA_PSM.ControllerUtmAcad;
using static TISMA_PSM.ControllerUtmHr;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Registration_New : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Get encrypted IC no. from URL param and decrypt it
            string icNo = DecryptURL(Request.QueryString["pid"]);
            string tempStat = Request.QueryString["stat"];

            //- Validate if status is empty or null: 404 Error
            if (string.IsNullOrEmpty(tempStat))
                Response.Redirect("PageNotFound.aspx");

            //- If status not empty, identify the status
            string stat = IdentifyStatus(tempStat);

            //- Validate if decrypted IC no. or identified status is empty or null: 404 Error
            if (string.IsNullOrEmpty(icNo) || string.IsNullOrEmpty(stat))
                Response.Redirect("PageNotFound.aspx");

            //- Validate if decrypted IC no. not exist in UTM-ACAD: 404 Error
            if (stat.Equals("UTM-ACAD"))
            {
                if (CheckIcNoUtmAcadNotExist(icNo).Equals(true))
                    Response.Redirect("PageNotFound.aspx");
            }

            //- Validate if decrypted IC no. not exist in UTM-HR: 404 Error
            if (stat.Equals("UTM-HR"))
            {
                if (CheckIcNoUtmHrNotExist(icNo).Equals(true))
                    Response.Redirect("PageNotFound.aspx");
            }

            //- Check is patient added to TISMA
            if (CheckIsPatientAddedToTisma(icNo).Equals(true))
                ModalPopupMessage.Show();

            if (!this.IsPostBack)
            {
                DisplayNewPatientInfo(icNo, stat);
            }
        }

        protected void AddToTisma(object sender, EventArgs e)
        {
            //- DB Exception/Error handling
            try
            {
                //- Identify patient status
                string spFunc = "";
                if (getStat.Text.Equals("UTM-ACAD"))
                    spFunc = "AddToTismaStudent";
                else if (getStat.Text.Equals("UTM-HR"))
                    spFunc = "AddToTismaStaff";
                else
                    Response.Redirect("InternalServerError.aspx");

                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Insert Query
                    using (SqlCommand cmd = new SqlCommand(spFunc, con) { CommandType = CommandType.StoredProcedure })
                    {
                        //- Insert to table 'patient'
                        cmd.Parameters.AddWithValue("@pIcNo", getIcNo.Text);
                        cmd.Parameters.AddWithValue("@pAccNo", getAccNo.Text);
                        cmd.Parameters.AddWithValue("@pPassport", getPassportNo.Text);
                        cmd.Parameters.AddWithValue("@pTelNo", getPhone.Text);
                        cmd.Parameters.AddWithValue("@pEmail", getEmail.Text);
                        cmd.Parameters.AddWithValue("@pName", getName.Text);
                        cmd.Parameters.AddWithValue("@Dob", DateTime.ParseExact(getDob.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                        cmd.Parameters.AddWithValue("@pAge", int.Parse(getAge.Text));
                        cmd.Parameters.AddWithValue("@pGender", getGender.Text);
                        cmd.Parameters.AddWithValue("@pMarital", getMaritalStat.Text);
                        cmd.Parameters.AddWithValue("@pReligion", getReligion.Text);
                        cmd.Parameters.AddWithValue("@pRace", getRace.Text);
                        cmd.Parameters.AddWithValue("@pNationality", getNation.Text);
                        cmd.Parameters.AddWithValue("@pAddress", getAddress.Text);
                        cmd.Parameters.AddWithValue("@pDesignation", getDesignation.Text);
                        cmd.Parameters.AddWithValue("@pCategory", getCategory.Text);
                        cmd.Parameters.AddWithValue("@pSession", getSession.Text);
                        cmd.Parameters.AddWithValue("@pBranch", getBranch.Text);
                        cmd.Parameters.AddWithValue("@pRemarks", getRemarks.Text);

                        //- Conditioning status to insert into different table
                        if (getStat.Text.Equals("UTM-ACAD"))
                        {
                            //- Insert to table 'patient_student'
                            cmd.Parameters.AddWithValue("@pMatric", getMatricNo.Text);
                            cmd.Parameters.AddWithValue("@pFaculty", getFacDep.Text);
                            cmd.Parameters.AddWithValue("@pCourse", getCourse.Text);
                            cmd.Parameters.AddWithValue("@pSem", int.Parse(getSem.Text));
                            cmd.Parameters.AddWithValue("@pAcadStat", 1); // 1 - True
                        }
                        else if (getStat.Text.Equals("UTM-HR"))
                        {
                            //- Insert to table 'patient_staff'
                            cmd.Parameters.AddWithValue("@pStaffId", getMatricNo.Text);
                            cmd.Parameters.AddWithValue("@pDepartment", getFacDep.Text);
                            cmd.Parameters.AddWithValue("@pHrStat", 1); // 1 - True
                        }
                        else
                            Response.Redirect("InternalServerError.aspx");

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

        private void DisplayNewPatientInfo(string icNo, string stat)
        {
            //- Parse and render data
            getStat.Text = stat;
            getAccNo.Text = GenerateAccNoPatient(stat);

            if (stat.Equals("UTM-ACAD"))
            {
                //- Object UTM-ACAD model
                ModelUtmAcad patient = GetUtmAcadInfoByIcNo(icNo);

                //- Parse and render data
                getBranch.Text = patient.branch;
                getCategory.Text = patient.category;
                getName.Text = patient.name;
                getMatricNo.Text = patient.matricNo;
                getIcNo.Text = patient.icNo;
                getPassportNo.Text = patient.passportNo;
                getDob.Text = patient.dob;
                getAge.Text = patient.age.ToString();
                getGender.Text = patient.gender;
                getMaritalStat.Text = patient.maritalStat;
                getReligion.Text = patient.religion;
                getRace.Text = patient.race;
                getNation.Text = patient.nationality;
                getAddress.Text = patient.address;
                getDesignation.Text = patient.designation;
                getFacDep.Text = patient.faculty;
                getCourse.Text = patient.course;
                getSem.Text = patient.semester.ToString();
                getPhone.Text = patient.telNo;
                getSession.Text = patient.session;
                getEmail.Text = patient.email;
            }
            else if (stat.Equals("UTM-HR"))
            {
                //- Object UTM-HR model
                ModelUtmHr hr = GetUtmHrInfoByIcNo(icNo);

                //- Parse and render data
                getBranch.SelectedValue = hr.sBranch;
                getCategory.Text = hr.sCategory;
                getName.Text = hr.sName;
                getMatricNo.Text = hr.sStaffId;
                getIcNo.Text = hr.sIcNo;
                getPassportNo.Text = hr.sPassportNo;
                getDob.Text = hr.sDob;
                getAge.Text = hr.sAge.ToString();
                getGender.SelectedValue = hr.sGender;
                getMaritalStat.SelectedValue = hr.sMaritalStat;
                getReligion.SelectedValue = hr.sReligion;
                getRace.SelectedValue = hr.sRace;
                getNation.Text = hr.sNationality;
                getAddress.Text = hr.sAddress;
                getDesignation.Text = hr.sDesignation;
                getFacDep.Text = hr.sDepartment;
                getCourse.Text = "";
                getSem.Text = "";
                getPhone.Text = hr.sTelNo;
                getSession.Text = hr.sSession;
                getEmail.Text = hr.sEmail;
            }
        }
    }
}