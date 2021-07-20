using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using static TISMA_PSM.ControllerEMC;
using static TISMA_PSM.ControllerPatient;
using static TISMA_PSM.ControllerQMS;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Patient_Info : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Get encrypted account no. from URL param and decrypt it
            string accNo = DecryptURL(Request.QueryString["accno"]);

            //- Validate if decrypted account no. is empty or null: 404 Error
            if (string.IsNullOrEmpty(accNo))
                Response.Redirect("PageNotFound.aspx");

            //- Validate if decrypted account no. not exist: 404 Error
            if (CheckAccPatientNotExist(accNo).Equals(true))
                Response.Redirect("PageNotFound.aspx");

            if (!this.IsPostBack)
            {
                DisplayPatientInfo(accNo);
                GetLatestQueueInfo();
                BindGrid();
            }
            else
                BindGrid();
        }

        private void BindGrid()
        {
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT clinical_info.clinical_date, clinical_info.symptom, clinical_info.ill_sign, clinical_info.diagnosis, pku_staff.s_name " +
                                                           "FROM clinical_info LEFT OUTER JOIN pku_staff ON pku_staff.s_ic_no = clinical_info.fk_s_ic_no " +
                                                           "WHERE clinical_info.fk_p_ic_no = '" + getIcNo.Text + "' " +
                                                           "ORDER BY clinical_info.clinical_date", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
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
                    using (SqlCommand cmd = new SqlCommand("SELECT date_created, url_hashed, emc_password " +
                                                           "FROM emc WHERE fk_p_ic_no = '" + getIcNo.Text + "' " +
                                                           "ORDER BY date_created", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                //- If records available
                                MCHistoryTable.DataSource = sdr;
                                MCHistoryTable.DataBind();
                            }
                            else
                            {
                                //- If no records found
                                DataTable dt = new DataTable();
                                MCHistoryTable.DataSource = dt;
                                MCHistoryTable.DataBind();
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }

            //- Datatable render
            ClinicalHistoryTable.UseAccessibleHeader = true;
            ClinicalHistoryTable.HeaderRow.TableSection = TableRowSection.TableHeader;
            MCHistoryTable.UseAccessibleHeader = true;
            MCHistoryTable.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void MCHistoryTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                string x = e.Row.Cells[3].Text;
                e.Row.Cells[3].Text = DecryptEMCPassword(x);
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
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM qms WHERE fk_ic_no = '" + getIcNo.Text + "'", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM clinical_info WHERE fk_p_ic_no = '" + getIcNo.Text + "'", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM emc WHERE fk_p_ic_no = '" + getIcNo.Text + "'", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM patient WHERE p_ic_no = '" + getIcNo.Text + "'", con))
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
            Response.Redirect("Registration.aspx");
        }

        protected void GenerateQueue(object sender, EventArgs e)
        {
            //- Step 1: Get today's date
            DateTime dateGenerated = DateTime.Now;

            //- Step 2: Set expired date at
            DateTime dateExpired = DateTime.Today.AddDays(1);
            dateExpired = new DateTime(dateExpired.Year, dateExpired.Month, dateExpired.Day, 0, 0, 0);

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

            //- Step 4: Database INSERT query
            //-- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //-- Insert Query
                    using (SqlCommand cmd = new SqlCommand("AddToQMS", con) { CommandType = CommandType.StoredProcedure })
                    {
                        //-- Insert to table 'qms'
                        cmd.Parameters.AddWithValue("@QueueNo", ConvertQueueNo(queueNo));
                        cmd.Parameters.AddWithValue("@DateGenerated", dateGenerated);
                        cmd.Parameters.AddWithValue("@DateExpired", dateExpired);
                        cmd.Parameters.AddWithValue("@IsKeyIn", 0);
                        cmd.Parameters.AddWithValue("@IsCheckIn", 0);
                        cmd.Parameters.AddWithValue("@IsCheckOut", 0);
                        cmd.Parameters.AddWithValue("@IsExpired", 0);
                        cmd.Parameters.AddWithValue("@IcNo", getIcNo.Text);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
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

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 queue_no FROM qms WHERE date_generated = '" + today.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "' ORDER BY queue_no DESC", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.Read())
                                getLatestNo.Text = sdr["queue_no"].ToString();
                            else
                                getLatestNo.Text = "[No queue number generated yet]";
                        }
                    }
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 date_generated FROM qms WHERE fk_ic_no = '" + getIcNo.Text + "' ORDER BY fk_ic_no DESC", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.Read())
                            {
                                DateTime.TryParse(sdr["date_generated"].ToString(), out DateTime dateGenerated);
                                getLastVisited.Text = dateGenerated.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                                getLastVisited.Text = "- (First Time Visit)";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }

        protected void UpdateToTisma(object sender, EventArgs e)
        {
            //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
            DateTime dob =  DateTime.ParseExact(getDob.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            int age = DateTime.Now.AddYears(-dob.Year).Year;

            //- Calculate the current session when the patient data is updated
            string session = DateTime.Now.AddYears(-1).ToString("yyyy", System.Globalization.CultureInfo.InvariantCulture) + "/" + DateTime.Now.ToString("yyyy", System.Globalization.CultureInfo.InvariantCulture);

            //- Validate if Marital Stat ddl is not selected
            if (getMaritalStat.SelectedValue.Equals("Select"))
                goto done;

            //- Validate if Religion ddl is not selected
            if (getReligion.SelectedValue.Equals("Select"))
                goto done;

            //- Validate if Phone is empty or Phone pattern is false
            if (string.IsNullOrEmpty(getPhone.Text) || CheckPatternIsPhoneNo(getPhone.Text).Equals(false))
                goto done;

            //- Validate if Email is empty or Email pattern is false
            if (string.IsNullOrEmpty(getEmail.Text) || CheckPatternIsEmail(getEmail.Text).Equals(false))
                goto done;

            //- Validate if Email is empty or Email pattern is false
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

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Update Query
                    using (SqlCommand cmd = new SqlCommand("UPDATE patient SET p_passport_no = '" + getPassportNo.Text + "', " +
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
                        cmd.ExecuteNonQuery();
                    }
                    //- If category is 'Student', update 'semester'
                    if (getCategory.Text.Equals("Student"))
                    {
                        //- Table 'patient_student'
                        using (SqlCommand cmd = new SqlCommand("UPDATE patient_student SET semester = '" + getSem.Text + "' WHERE fk_ic_no = '" + getIcNo.Text + "'", con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            ModalPopupMessageUpdate.Show();
        done:;
        }

        private void DisplayPatientInfo(string accNo)
        {
            //- Object Patient model
            ModelPatient patient = GetPatientInfoByAccNo(accNo);

            //- Parse and render data
            getIcNo.Text = patient.pIcNo;
            getAccNo.Text = patient.pAccountNo; 
            displayAccNo.Text = patient.pAccountNo; 
            getAccNoQMS.Text = patient.pAccountNo;
            getPassportNo.Text = patient.pPassportNo;
            getPhone.Text = patient.pTelNo;
            getEmail.Text = patient.pEmail;
            getName.Text = patient.pName;
            getDob.Text = patient.pDob;
            getAge.Text = patient.pAge.ToString();
            getGender.SelectedValue = patient.pGender;
            getMaritalStat.SelectedValue = patient.pMaritalStat;
            getReligion.SelectedValue = patient.pReligion;
            getRace.SelectedValue = patient.pRace;
            getNation.Text = patient.pNationality;
            getAddress.Text = patient.pAddress;
            getDesignation.Text = patient.pDesignation;
            getCategory.Text = patient.pCategory; 
            displayCategory.Text = patient.pCategory;
            getSession.Text = patient.pSession;
            getBranch.SelectedValue = patient.pBranch;
            getRemarks.Text = patient.pRemarks;

            if (patient.pCategory.Equals("Student"))
            {
                //- Display note
                note.Text = "The basic information were reffered from UTMHR SYSTEM / ACAD SYSTEM";

                getMatricNo.Text = patient.pMatricNo;
                getFacDep.Text = patient.pFaculty;
                getCourse.Text = patient.pCourse;
                getSem.Text = patient.pSemester.ToString();
                getStat.Text = "UTM-ACAD";

                //- Status activity
                if (patient.utmAcadStat.Equals(true))
                    statusText.Text = "Active";
                else
                    statusText.Text = "Not-active";
            }
            else if (patient.pCategory.Equals("Staff"))
            {
                //- Display note
                note.Text = "The basic information were reffered from UTMHR SYSTEM / ACAD SYSTEM";

                //- Hide semester textbox
                semTextbox.Visible = false;
                semTitle.Visible = false;

                getMatricNo.Text = patient.pStaffId;
                getFacDep.Text = patient.pDepartment;
                getStat.Text = "UTM-HR";

                //- Status activity
                if (patient.utmHrStat.Equals(true))
                    statusText.Text = "Active";
                else
                    statusText.Text = "Not-active";
            }
            else if (patient.pCategory.Equals("Public"))
            {
                //- Display note
                note.Text = "The basic information is registered as public";

                //- Hide semester textbox
                semTextbox.Visible = false;
                semTitle.Visible = false;

                statusText.Text = "Not-active";
            }
        }
    }
}