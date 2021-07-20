using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web;
using static TISMA_PSM.ControllerClinicalInfo;
using static TISMA_PSM.ControllerEMC;
using static TISMA_PSM.ControllerPatient;
using static TISMA_PSM.ControllerQMS;
using static TISMA_PSM.ControllerStaff;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Consultation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Verify user ACL based on session: 404 error
            if (!Convert.ToString(Session["UserRole"]).Equals("Admin") && !Convert.ToString(Session["UserRole"]).Equals("Medical Officer"))
                Response.Redirect("PageNotFound.aspx");

            //- Get account no. and queue no from URL param
            string accNo = DecryptURL(Request.QueryString["accno"]);
            string queueNo = Request.QueryString["queue"];

            //- Validate if decrypted account no. is empty or null: 404 Error
            if (string.IsNullOrEmpty(accNo))
                Response.Redirect("PageNotFound.aspx");

            //- Validate if decrypted account no. not exist: 404 Error
            if (CheckAccPatientNotExist(accNo).Equals(true))
                Response.Redirect("PageNotFound.aspx");

            //- Get patient info
            ModelPatient patient = GetPatientInfoByAccNo(accNo);

            //- Validate if queue no. is empty or null: 404 Error
            if (string.IsNullOrEmpty(queueNo))
                Response.Redirect("PageNotFound.aspx");

            //- Validate if queue no. not exist: 404 Error
            if (CheckIsQueueNotExist(queueNo, DateTime.Now).Equals(true))
                Response.Redirect("PageNotFound.aspx");

            //- Validate if queue no. not exist: 404 Error
            if (CheckIsPatientOwnsQueue(patient.pIcNo, queueNo).Equals(false))
                Response.Redirect("PageNotFound.aspx");

            //- Get extra URL param (msg) to display popup message
            if (!string.IsNullOrEmpty(Request.QueryString["msg"]))
            {
                //- Display popup message
                if (Request.QueryString["msg"].Equals("001"))
                    getMessage.Text = "The Clinical Information has been submitted successfully.";
                else if (Request.QueryString["msg"].Equals("002"))
                    getMessage.Text = "Changes on Clinical Information has been updated!";
                else if (Request.QueryString["msg"].Equals("003"))
                    getMessage.Text = "The Electronic Medical Certificate has been generated and sent to the patient.";
                else if (Request.QueryString["msg"].Equals("004"))
                    getMessage.Text = "The Electronic Medical Certificate has been sent to the patient's email address and SMS successfully.";
                else if (Request.QueryString["msg"].Equals("005"))
                    getMessage.Text = "Clinical Info not yet submitted. Please generate Clinical Info first before releasing the e-MC for this patient.";
                else if (Request.QueryString["msg"].Equals("006"))
                    getMessage.Text = "Changes on Electronic Medical Certificate has been updated!";
                else if (Request.QueryString["msg"].Equals("007"))
                    getMessage.Text = "Update failed. Please enter the date(s) of e-MC!";
                else if (Request.QueryString["msg"].Equals("008"))
                    getMessage.Text = "Check-Out failed. Please generate the Clinical Info for this patient first.";
                else if (Request.QueryString["msg"].Equals("009"))
                {
                    SymptomDdl.SelectedIndex = -1;
                    getSymptomDesc.Text = String.Empty;
                    SignDdl.SelectedIndex = -1;
                    getSignDesc.Text = String.Empty;
                    DiagnosisDdl.SelectedIndex = -1;
                    getDiagnosisDesc.Text = String.Empty;
                }

                if (!Request.QueryString["msg"].Equals("009"))
                    ModalPopupMessage.Show();
            }

            BindGrid(accNo);

            //- Parse and display queue no.
            getQueueNo.Text = queueNo;

            //- Block past date for date from and date to
            getDateFrom.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            getDateTo.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            //- Auto-set Date From to current date (if empty)
            if (string.IsNullOrEmpty(getDateFrom.Text))
                getDateFrom.Text = DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            if (!IsPostBack)
            {
                BindClinicalInfoDdl();

                //- Parse data
                getAccNo.Text = accNo;
                getCategory.Text = patient.pCategory;
                getName.Text = patient.pName;
                getDOB.Text = patient.pDob;
                getAge.Text = patient.pAge.ToString();
                getRemark.Text = patient.pRemarks;

                if (patient.pCategory.Equals("Student") || patient.pCategory.Equals("Staff"))
                {
                    getMatricStaffHeader.Text = "Matric/Staff No.";
                    getFacDepHeader.Text = "Faculty/Department";

                    if (patient.pCategory.Equals("Student"))
                    {
                        getMatricStaff.Text = patient.pMatricNo;
                        getFacDep.Text = patient.pFaculty;
                    }
                    else
                    {
                        getMatricStaff.Text = patient.pStaffId;
                        getFacDep.Text = patient.pDepartment;
                    }
                }
                else
                {
                    getMatricStaffHeader.Text = "IC/Passport No.";
                    getMatricStaff.Text = patient.pIcNo;
                    getFacDepHeader.Text = "Occupation";
                    getFacDep.Text = patient.pDesignation;
                }

                //- Check if patient's clinical info for today has been submitted TO GET CLINICAL INFO
                if (CheckClinicalSubmitted(patient.pIcNo).Equals(true))
                {
                    //- Get Clinical Info
                    ModelClinicalInfo clinical = GetClinicalInfoForTodayByIcNo(patient.pIcNo);
                    SymptomDdl.SelectedValue = clinical.symptom;
                    getSymptomDesc.Text = clinical.symptomDesc;
                    SignDdl.SelectedValue = clinical.sign;
                    getSignDesc.Text = clinical.signDesc;
                    DiagnosisDdl.SelectedValue = clinical.diagnosis;
                    getDiagnosisDesc.Text = clinical.diagnosisDesc;
                }
                //- Check if patient's e-MC has been generated with two specific dates and period TO GET e-MC INFO
                if (CheckEMCGenerated(patient.pIcNo).Equals(true))
                {
                    //- Get e-MC info
                    ModelEMC emc = GetEMCInfoForTodayByIcNo(patient.pIcNo);
                    getPassword.Text = DecryptEMCPassword(emc.password);
                    getDateFrom.Text = emc.dateFrom;
                    getDateTo.Text = emc.dateTo;
                    getPeriod.Text = emc.period.ToString();
                    getComment.Text = emc.comment;
                }
            }

            //- Check if patient's clinical info for today has been submitted TO HIDE/SHOW SEVERAL BUTTONS
            if (CheckClinicalSubmitted(patient.pIcNo).Equals(true))
            {
                //- Hide submit button and show update button
                BtnSubmit.Visible = false;
                BtnUpdate.Visible = true;
                //- Show note of submit/update
                NoteClinical.Visible = true;
            }
            else
            {
                //- Hide update button and show submit button
                BtnSubmit.Visible = true;
                BtnUpdate.Visible = false;
                //- Hide note of submit/update
                NoteClinical.Visible = false;
            }

            //- Check if patient's E-MC for today has been generated TO HIDE/SHOW SEVERAL BUTTONS
            if (CheckEMCGenerated(patient.pIcNo).Equals(true))
            {
                //- Show e-MC password for MO reference to view
                MCPassword.Visible = true;
                //- Hide submit button and show update/send/view e-MC button
                BtnGenerateMC.Visible = false;
                BtnUpdateMC.Visible = true;
                BtnSendMC.Visible = true;
                BtnViewMC.Visible = true;
                //- Show note of submit/update
                NoteEMC.Visible = true;
            }
            else
            {
                //- Show e-MC password for MO reference to view
                MCPassword.Visible = false;
                //- Hide submit button and show update/send/view e-MC button
                BtnGenerateMC.Visible = true;
                BtnUpdateMC.Visible = false;
                BtnSendMC.Visible = false;
                BtnViewMC.Visible = false;
                //- Show note of submit/update
                NoteEMC.Visible = false;
            }
        }

        protected void CheckOut(object sender, EventArgs e)
        {
            //- Get patient info
            ModelPatient patient = GetPatientInfoByAccNo(getAccNo.Text);

            if (CheckClinicalSubmitted(patient.pIcNo).Equals(false))
            {   
                //- Redirect (to prevent Form Resubmission)
                Response.Redirect("Consultation.aspx?accno=" + Request.QueryString["accno"] + "&queue=" + Request.QueryString["queue"] + "&msg=008");
            }
            else if (CheckEMCGenerated(patient.pIcNo).Equals(false))
            {
                //- Display popup message
                eMCNotGeneratedMsg.Visible = true;
                ModalPopupCheckOut.Show();
            }
            else
            {
                //- Display popup message
                eMCNotGeneratedMsg.Visible = false;
                ModalPopupCheckOut.Show();
            }
        }

        protected void CheckOutConfirm(object sender, EventArgs e)
        {
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Update Query
                    using (SqlCommand cmd = new SqlCommand("UPDATE qms SET is_checked_out = '1' WHERE queue_no = '" + getQueueNo.Text + "' AND date_generated = '" + DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "'", con))
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
            Response.Redirect("OPD.aspx");
        }

        protected void GenerateEMC(object sender, EventArgs e)
        {
            //- Get patient info
            ModelPatient patient = GetPatientInfoByAccNo(getAccNo.Text);
            //- Object e-MC model
            ModelEMC emc = new ModelEMC();

            if (CheckClinicalSubmitted(patient.pIcNo).Equals(false))
            {
                //- Redirect (to prevent Form Resubmission)
                Response.Redirect("Consultation.aspx?accno=" + Request.QueryString["accno"] + "&queue=" + Request.QueryString["queue"] + "&msg=005");
            }
            else
            {
                //- Step 1: Generate Serial No.
                emc.serialNo = GenerateSerialNo();

                //- Step 2: Encrypt and hash the Serial No. with salt
                string[] encryptedUrl = Hashing(emc.serialNo);
                emc.urlHashed = encryptedUrl[0];
                emc.urlSalt = encryptedUrl[1];

                //- Step 3: Generate QR Code
                emc.qrCode = GenerateQRCode(emc.urlHashed);

                //- Step 4: Generate password
                string[] generatedPassword = GeneratePasswordEMC();
                string plainPassword = generatedPassword[0];
                emc.password = generatedPassword[1];
                getPassword.Text = plainPassword;

                //- Step 5: Validate and calculate period
                DateTime dt1 = DateTime.ParseExact(getDateFrom.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dt2 = DateTime.ParseExact(getDateTo.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dtNow = DateTime.Now;

                //-- Step 5.1: Date from must not before the current date
                if (dt1.Date < dtNow.Date)
                {
                    //- Display popup message
                    ErrorDate.Visible = true;
                    getMessage.Text = "Error! The selected date (From) is a passed date.";
                    ModalPopupMessage.Show();
                    goto done;
                }

                //-- Step 5.2: Date after must not before the current date
                if (dt2.Date < dtNow.Date)
                {
                    //- Display popup message
                    ErrorDate.Visible = true;
                    getMessage.Text = "Error! The selected date (To) is a passed date.";
                    ModalPopupMessage.Show();
                    goto done;
                }

                //-- Step 5.3: Validate the e-MC date by calculating the period
                int period = 1 + (dt2 - dt1).Days;
                getPeriod.Text = period.ToString();
                if (period <= 0)
                {
                    //- Display popup message
                    ErrorDate.Visible = true;
                    getMessage.Text = "Error! The selected dates and period of e-MC are invalid.";
                    ModalPopupMessage.Show();
                    goto done;
                }

                //- Step 6: Validate if comment is empty or not,
                //--------- If not empty, validate the text pattern
                //--------- If empty, put '-'
                string comment = getComment.Text;
                if (!string.IsNullOrEmpty(comment)) 
                {
                    if (CheckPatternIsMultiLineText(comment).Equals(false))
                        goto done; 
                }
                else
                    comment = "-";

                //- Step 7: Get staff info
                //-- Get staff Account No. from session
                string staffAccNo = Convert.ToString(Session["UserAccNo"]);
                ModelStaff staff = GetStaffInfoByAccNo(staffAccNo);

                //- Step 9: Get clinical info
                ModelClinicalInfo clinical = GetClinicalInfoForTodayByIcNo(patient.pIcNo);

                //- Step 10: Database INSERT query
                //-- DB Exception/Error handling
                try
                {
                    using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                    {
                        con.Open();
                        //-- Insert Query
                        using (SqlCommand cmd = new SqlCommand("SubmitEMC", con) { CommandType = CommandType.StoredProcedure })
                        {
                            //-- Insert to table 'emc'
                            cmd.Parameters.AddWithValue("@SerialNo", emc.serialNo);
                            cmd.Parameters.AddWithValue("@UrlHashed", emc.urlHashed);
                            cmd.Parameters.AddWithValue("@UrlSalt", emc.urlSalt);
                            cmd.Parameters.AddWithValue("@Password", emc.password);
                            cmd.Parameters.AddWithValue("@DateFrom", DateTime.ParseExact(getDateFrom.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
                            cmd.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(getDateTo.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
                            cmd.Parameters.AddWithValue("@Period", period);
                            cmd.Parameters.AddWithValue("@TimeCreated", TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)));
                            cmd.Parameters.AddWithValue("@Comment", comment);
                            cmd.Parameters.AddWithValue("@PIcNo", patient.pIcNo);
                            cmd.Parameters.AddWithValue("@SIcNo", staff.sIcNo);
                            cmd.Parameters.AddWithValue("@ClinicalId", clinical.clinicalId);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    //- Display handling-error message
                    SqlExceptionMsg(ex);
                }

                //- Step 11: Auto-send e-MC to the patient via email
                EmailEMC(getAccNo.Text, plainPassword);

                //- Step 12: Auto-send e-MC to the patient via SMS
                string url = GetUrleMC() + emc.urlHashed;
                string message = HttpUtility.UrlEncode("RM0 TISMA: Confidential! Never share your e-MC with an untrusted party. You may access your e-MC via " + url + ". Password: " + plainPassword + ".");
                SendSMS(patient.pTelNo, message);

                //- Show e-MC password for MO reference to view
                MCPassword.Visible = true;
                //- Hide submit button and show update/send/view e-MC button
                BtnGenerateMC.Visible = false;
                BtnUpdateMC.Visible = true;
                BtnSendMC.Visible = true;
                BtnViewMC.Visible = true;
                //- Show note of submit/update/error
                NoteEMC.Visible = true;
                ErrorDate.Visible = false;
                
                //- Redirect (to prevent Form Resubmission)
                Response.Redirect("Consultation.aspx?accno=" + Request.QueryString["accno"] + "&queue=" + Request.QueryString["queue"] + "&msg=003");
            }
        done:;
        }

        protected void MCHistoryTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                string x = e.Row.Cells[3].Text;
                e.Row.Cells[3].Text = DecryptEMCPassword(x);
            }
        }

        protected void ResetClinicalInfo(object sender, EventArgs e)
        {
            //- Redirect (to prevent Form Resubmission)
            Response.Redirect("Consultation.aspx?accno=" + Request.QueryString["accno"] + "&queue=" + Request.QueryString["queue"] + "&msg=009");
        }

        protected void ReSendEMC(object sender, EventArgs e)
        {
            //- Get patient info
            ModelPatient patient = GetPatientInfoByAccNo(getAccNo.Text);
            //- Get e-MC info
            ModelEMC emc = GetEMCInfoByIcNo(patient.pIcNo);

            //- Send email
            EmailEMC(getAccNo.Text, getPassword.Text);
            //- Send sms
            string plainPassword = DecryptEMCPassword(emc.password);
            string url = GetUrleMC() + emc.urlHashed;
            string message = HttpUtility.UrlEncode("RM0 TISMA: Confidential! Never share your e-MC with an untrusted party. You may access your e-MC via " + url + ". Password: " + plainPassword + ".");
            SendSMS(patient.pTelNo, message);
            //- Redirect (to prevent Form Resubmission)
            Response.Redirect("Consultation.aspx?accno=" + Request.QueryString["accno"] + "&queue=" + Request.QueryString["queue"] + "&msg=004");
        }

        protected void SubmitClinicalInfo(object sender, EventArgs e)
        {
            //- Get staff Account No. from session
            string staffAccNo = Convert.ToString(Session["UserAccNo"]);

            //- Get patient info
            ModelPatient patient = GetPatientInfoByAccNo(getAccNo.Text);
            //- Get staff info
            ModelStaff staff = GetStaffInfoByAccNo(staffAccNo);

            //- Validate the clinical sign items
            string symptom, sign, diagnosis;
            if (!string.IsNullOrEmpty(SymptomDdl.Text))
                symptom = SymptomDdl.Text;
            else
                symptom = SymptomDdl.SelectedValue;
            if (!string.IsNullOrEmpty(SignDdl.Text))
                sign = SignDdl.Text;
            else
                sign = SignDdl.SelectedValue;
            if (!string.IsNullOrEmpty(DiagnosisDdl.Text))
                diagnosis = DiagnosisDdl.Text;
            else
                diagnosis = DiagnosisDdl.SelectedValue;

            //- Verify if item clinical info is new and not exist yet in the ddl, if yes, add and save into db
            if (CheckIsItemClinicalAdded("Diagnosis", diagnosis).Equals(false))
                AddDiagnosis(diagnosis);
            if (CheckIsItemClinicalAdded("Sign", sign).Equals(false))
                AddSign(sign);
            if (CheckIsItemClinicalAdded("Symptom", symptom).Equals(false))
                AddSymptom(symptom);

            //- Validate if description is not empty, but text pattern is false
            if (!string.IsNullOrEmpty(getSymptomDesc.Text))
                if (CheckPatternIsMultiLineText(getSymptomDesc.Text).Equals(false))
                    goto done;
            if (!string.IsNullOrEmpty(getSignDesc.Text))
                if (CheckPatternIsMultiLineText(getSignDesc.Text).Equals(false))
                    goto done;
            if (!string.IsNullOrEmpty(getDiagnosisDesc.Text))
                if (CheckPatternIsMultiLineText(getDiagnosisDesc.Text).Equals(false))
                    goto done;

            //- Proceed when all required input are filled
            if (!string.IsNullOrEmpty(symptom) && !string.IsNullOrEmpty(sign) && !string.IsNullOrEmpty(diagnosis))
            {
                //- DB Exception/Error handling
                try
                {
                    using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                    {
                        con.Open();
                        //- Insert data
                        SqlCommand cmd = new SqlCommand("SubmitClinicalInfo", con) { CommandType = CommandType.StoredProcedure };
                        cmd.Parameters.AddWithValue("@Symptom", symptom);
                        cmd.Parameters.AddWithValue("@SymptomDesc", getSymptomDesc.Text);
                        cmd.Parameters.AddWithValue("@Sign", sign);
                        cmd.Parameters.AddWithValue("@SignDesc", getSignDesc.Text);
                        cmd.Parameters.AddWithValue("@Diagnosis", diagnosis);
                        cmd.Parameters.AddWithValue("@DiagnosisDesc", getDiagnosisDesc.Text);
                        cmd.Parameters.AddWithValue("@PIcNo", patient.pIcNo);
                        cmd.Parameters.AddWithValue("@SIcNo", staff.sIcNo);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    //- Display handling-error message
                    SqlExceptionMsg(ex);
                }

                //- Hide submit button and show update button for Clinical Info
                BtnSubmit.Visible = false;
                BtnUpdate.Visible = true;
                //- Show note of submit/update
                NoteClinical.Visible = true;

                //- Redirect (to prevent Form Resubmission)
                Response.Redirect("Consultation.aspx?accno=" + Request.QueryString["accno"] + "&queue=" + Request.QueryString["queue"] + "&msg=001");
            }
        done:;
        }

        protected void UpdateClinicalInfo(object sender, EventArgs e)
        {
            //- Get patient info
            ModelPatient patient = GetPatientInfoByAccNo(getAccNo.Text);
            //- Get clinical info
            ModelClinicalInfo clinical = GetClinicalInfoForTodayByIcNo(patient.pIcNo);

            //- Validate the clinical sign items
            string symptom, sign, diagnosis;
            if (!string.IsNullOrEmpty(SymptomDdl.Text))
                symptom = SymptomDdl.Text;
            else
                symptom = SymptomDdl.SelectedValue;
            if (!string.IsNullOrEmpty(SignDdl.Text))
                sign = SignDdl.Text;
            else
                sign = SignDdl.SelectedValue;
            if (!string.IsNullOrEmpty(DiagnosisDdl.Text))
                diagnosis = DiagnosisDdl.Text;
            else
                diagnosis = DiagnosisDdl.SelectedValue;

            //- Verify if item clinical info is new and not exist yet in the ddl, if yes, add and save into db
            if (CheckIsItemClinicalAdded("Diagnosis", diagnosis).Equals(false))
                AddDiagnosis(diagnosis);
            if (CheckIsItemClinicalAdded("Sign", sign).Equals(false))
                AddSign(sign);
            if (CheckIsItemClinicalAdded("Symptom", symptom).Equals(false))
                AddSymptom(symptom);

            //- Validate if description is not empty, but text pattern is false
            if (!string.IsNullOrEmpty(getSymptomDesc.Text))
                if (CheckPatternIsMultiLineText(getSymptomDesc.Text).Equals(false))
                    goto done;
            if (!string.IsNullOrEmpty(getSignDesc.Text))
                if (CheckPatternIsMultiLineText(getSignDesc.Text).Equals(false))
                    goto done;
            if (!string.IsNullOrEmpty(getDiagnosisDesc.Text))
                if (CheckPatternIsMultiLineText(getDiagnosisDesc.Text).Equals(false))
                    goto done;

            //- Proceed when all required input are filled
            if (!string.IsNullOrEmpty(symptom) && !string.IsNullOrEmpty(sign) && !string.IsNullOrEmpty(diagnosis))
            {
                //- DB Exception/Error handling
                try
                {
                    using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                    {
                        con.Open();
                        //- Update Query
                        using (SqlCommand cmd = new SqlCommand("UPDATE clinical_info SET symptom = '" + symptom + "', symptom_desc = '" + getSymptomDesc.Text + "', " +
                                                                                     "ill_sign = '" + sign + "', ill_sign_desc = '" + getSignDesc.Text + "', " +
                                                                                     "diagnosis = '" + diagnosis + "', diagnosis_desc = '" + getDiagnosisDesc.Text + "' " +
                                                                                     "WHERE clinical_id = '" + clinical.clinicalId + "' AND fk_p_ic_no = '" + patient.pIcNo + "'", con))
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

                //- Hide submit button and show update button for Clinical Info
                BtnSubmit.Visible = false;
                BtnUpdate.Visible = true;
                //- Show note of submit/update
                NoteClinical.Visible = true;

                //- Redirect (to prevent Form Resubmission)
                Response.Redirect("Consultation.aspx?accno=" + Request.QueryString["accno"] + "&queue=" + Request.QueryString["queue"] + "&msg=002");
            }
        done:;
        }

        protected void UpdateEMCInfo(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(getDateFrom.Text) && !string.IsNullOrEmpty(getDateTo.Text))
            {
                //- Validate and calculate period
                DateTime dt1 = DateTime.ParseExact(getDateFrom.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dt2 = DateTime.ParseExact(getDateTo.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dtNow = DateTime.Now;

                //-- Date from must not before the current date
                if (dt1.Date < dtNow.Date)
                {
                    //- Display popup message
                    ErrorDate.Visible = true;
                    getMessage.Text = "Error! The selected date (From) is a passed date.";
                    ModalPopupMessage.Show();
                    goto done;
                }

                //-- Date after must not before the current date
                if (dt2.Date < dtNow.Date)
                {
                    //- Display popup message
                    ErrorDate.Visible = true;
                    getMessage.Text = "Error! The selected date (To) is a passed date.";
                    ModalPopupMessage.Show();
                    goto done;
                }

                //-- Validate the e-MC date by calculating the period
                int period = 1 + (dt2 - dt1).Days;
                getPeriod.Text = period.ToString();
                if (period <= 0)
                {
                    //- Display popup message
                    ErrorDate.Visible = true;
                    getMessage.Text = "Error! The selected dates and period of e-MC are invalid.";
                    ModalPopupMessage.Show();
                    goto done;
                }

                //- Validate if comment is empty or not,
                //- If not empty, validate the text pattern
                //- If empty, put '-'
                string comment = getComment.Text;
                if (!string.IsNullOrEmpty(comment))
                {
                    if (CheckPatternIsMultiLineText(comment).Equals(false))
                        goto done;
                }
                else
                    comment = "-";

                //- Get patient info
                ModelPatient patient = GetPatientInfoByAccNo(getAccNo.Text);
                //- Get e-MC info
                ModelEMC emc = GetEMCInfoByIcNo(patient.pIcNo);                

                //- DB Exception/Error handling
                try
                {
                    using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                    {
                        con.Open();
                        //- Update Query
                        using (SqlCommand cmd2 = new SqlCommand("UPDATE emc SET date_from = '" + getDateFrom.Text + "', date_to = '" + getDateTo.Text + "', " +
                                                                               "emc_period = '" + period + "', comment = '" + comment + "' " +
                                                                               "WHERE serial_no = '" + emc.serialNo + "' AND fk_p_ic_no = '" + patient.pIcNo + "'", con))
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

                //- Show e-MC password for MO reference to view
                MCPassword.Visible = true;
                //- Hide submit button and show update/send/view e-MC button
                BtnGenerateMC.Visible = false;
                BtnUpdateMC.Visible = true;
                BtnSendMC.Visible = true;
                BtnViewMC.Visible = true;
                //- Show note of submit/update/error
                NoteEMC.Visible = true;
                ErrorDate.Visible = false;

                //- Redirect (to prevent Form Resubmission)
                Response.Redirect("Consultation.aspx?accno=" + Request.QueryString["accno"] + "&queue=" + Request.QueryString["queue"] + "&msg=006");
            }
            else
            {
                //- Redirect (to prevent Form Resubmission)
                Response.Redirect("Consultation.aspx?accno=" + Request.QueryString["accno"] + "&queue=" + Request.QueryString["queue"] + "&msg=007");
            }
        done:;
        }

        protected void ViewEMC(object sender, EventArgs e)
        {
            //- Get patient info
            ModelPatient patient = GetPatientInfoByAccNo(getAccNo.Text);
            //- Get e-MC info
            ModelEMC emc = GetEMCInfoForTodayByIcNo(patient.pIcNo);

            //- Set the complete e-MC url
            string fullUrl = "e-MC.aspx?token=" + emc.urlHashed;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('" + fullUrl + "');", true);
        }

        private void BindClinicalInfoDdl()
        {
            //- DB Exception/Error handling
            try
            {
                //- Get Query
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Symptom Ddl
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM symptomDb ORDER BY symptom_name", con))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);
                                SymptomDdl.DataTextField = ds.Tables[0].Columns["symptom_name"].ToString();
                                SymptomDdl.DataValueField = ds.Tables[0].Columns["symptom_name"].ToString();
                                SymptomDdl.DataSource = ds.Tables[0];
                            }
                        }
                        SymptomDdl.DataBind();
                    }

                    //- Sign Ddl
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM signDb ORDER BY sign_name", con))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);
                                SignDdl.DataTextField = ds.Tables[0].Columns["sign_name"].ToString();
                                SignDdl.DataValueField = ds.Tables[0].Columns["sign_name"].ToString();
                                SignDdl.DataSource = ds.Tables[0];
                            }
                        }
                        SignDdl.DataBind();
                    }

                    //- Diagnosis Ddl
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM diagnosisDb ORDER BY diagnosis_name", con))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);
                                DiagnosisDdl.DataTextField = ds.Tables[0].Columns["diagnosis_name"].ToString();
                                DiagnosisDdl.DataValueField = ds.Tables[0].Columns["diagnosis_name"].ToString();
                                DiagnosisDdl.DataSource = ds.Tables[0];
                            }
                        }
                        DiagnosisDdl.DataBind();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }

        private void BindGrid(string accNo)
        {
            //- Get patient info
            ModelPatient patient = GetPatientInfoByAccNo(accNo);

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Retrieve Query: TISMADB
                    using (SqlCommand cmd = new SqlCommand("SELECT clinical_info.clinical_date, clinical_info.symptom, clinical_info.ill_sign, clinical_info.diagnosis, pku_staff.s_name " +
                                                           "FROM clinical_info LEFT OUTER JOIN pku_staff ON pku_staff.s_ic_no = clinical_info.fk_s_ic_no " +
                                                           "WHERE clinical_info.fk_p_ic_no = '" + patient.pIcNo + "' ORDER BY clinical_info.clinical_date", con))
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
                    using (SqlCommand cmd = new SqlCommand("SELECT date_created, url_hashed, emc_password FROM emc WHERE fk_p_ic_no = '" + patient.pIcNo + "' ORDER BY date_created", con))
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

            //- Render table
            ClinicalHistoryTable.UseAccessibleHeader = true;
            ClinicalHistoryTable.HeaderRow.TableSection = TableRowSection.TableHeader;
            MCHistoryTable.UseAccessibleHeader = true;
            MCHistoryTable.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        private void EmailEMC(string accNo, string emcPassword)
        {
            //- Get patient info
            ModelPatient patient = GetPatientInfoByAccNo(accNo);
            //- Get e-MC info
            ModelEMC emc = GetEMCInfoByIcNo(patient.pIcNo);

            //- Email content
            string subject = "Electronic Medical Certificate PKU: " + emc.serialNo;
            string url = GetUrleMC() + emc.urlHashed;
            string body = @"<html>
                                    <body style=""text-align:center; background-color: #fff"">
                                        <p style=""font-size:40px; font-weight:700; color:red"">TISMA<span style=""color: grey"">PKU</span></p>
                                        <h1>Electronic Medical Certificate</h1>
                                        <p>Thank you for visiting Pusat Kesihatan UTM (PKU) for a medical check-up. Here is the link to your <b>Electronic Medical Certificate (e-MC):</b></p>
                                        <a href='" + url + @"' style=""color:blue; font-size:18px; font-weight:700"">VIEW MY ELECTRONIC MEDICAL CERTIFICATE</a>
                                        <br />
                                        <p>e-MC Password:</p>
                                        <h1>" + emcPassword + @"</h1>
                                        <p>Please save or keep a copy of the Password and download the e-MC for future reference. Please do contact us immediately, if you have any issues with the e-MC.</p>
                                        <br />
                                        <p><b>RULES AND REGULATIONS (Please Read!)</b></p>
                                        <p>- This is a confidential document, only share with the party who requires it such as your employer or lecturer. -</p>
                                        <p>- <b>Do not share the URL</b> to public or untrusted party. -</p>
                                        <p>- <b>Do not share the PASSWORD</b> to public or untrusted party. -</p>
                                        <p>- You may have to provide/share the password to the party that requires this document in order to view it. -</p>
                                        <p>- This e-MC only valid within the given dates by your Medical Officer. -</p>
                                        <br /><hr />
                                        <p style=""color:grey; font-size: 11px"">This is an automatically generated email, please do not reply.</p>
                                    </body>
                                </html>";

            SendEmail(subject, body, patient.pEmail);
        }
    }
}