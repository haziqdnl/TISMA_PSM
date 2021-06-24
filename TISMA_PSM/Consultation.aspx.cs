using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using QRCoder;
using System.Web.Helpers;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace TISMA_PSM
{
    public partial class Consultation : System.Web.UI.Page
    {
        //- Global attribute
        public static string eMCHashedUrl;
        public static string patientEmail;

        protected void Page_Load(object sender, EventArgs e)
        {
            //- Get accno (accNo) and queue from URL param
            string accNo = DecryptURL(Request.QueryString["accno"]);
            string queueNo = Request.QueryString["queue"];
            BindGrid(accNo);

            if (!IsPostBack)
            {
                GetPatientInfo(accNo, queueNo);
                BindClinicalInfoDdl();

                //- Check if patient's clinical info for today has been submitted TO GET CLINICAL INFO
                if (CheckClinicalSubmitted(accNo).Equals(true))
                    GetPatientClinicalInfo(accNo);

                //- Check if patient's e-MC has been generated with two specific dates and period TO GET e-MC INFO
                if (CheckEMCGenerated(accNo).Equals(true))
                    GetPatientEMCInfo(accNo);
            }

            //- Check if patient's clinical info for today has been submitted TO HIDE/SHOW SEVERAL BUTTONS
            if (CheckClinicalSubmitted(accNo).Equals(true))
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

            //- Check if patient's E-MC has been generated with two specific dates and period TO HIDE/SHOW SEVERAL BUTTONS
            if (CheckEMCGenerated(getAccNo.Text).Equals(true))
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

        protected void AddItemDiagnosis(object sender, EventArgs e)
        {
            string diagnosis = DiagnosisDdl.Text;
            //- DB Exception-Error handling
            try
            {
                //- Insert Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO diagnosisDb (diagnosis_name) VALUES (@DiagnosisName)"))
                    {
                        cmd.Connection = con1;
                        cmd.Parameters.Add("@DiagnosisName", SqlDbType.NVarChar, 50).Value = diagnosis;
                        con1.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        con1.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Diagnosis Ddl updated");
            }
        }

        protected void AddItemSign(object sender, EventArgs e)
        {
            string sign = SignDdl.Text;
            //- DB Exception-Error handling
            try
            {
                //- Insert Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO signDb (sign_name) VALUES (@SignName)"))
                    {
                        cmd.Connection = con1;
                        cmd.Parameters.Add("@SignName", SqlDbType.NVarChar, 50).Value = sign;
                        con1.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        con1.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Sign Ddl updated");
            }
        }

        protected void AddItemSymptom(object sender, EventArgs e)
        {
            string symptom = SymptomDdl.Text;
            //- DB Exception-Error handling
            try
            {
                //- Insert Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO symptomDb (symptom_name) VALUES (@SymptomName)"))
                    {
                        cmd.Connection = con1;
                        cmd.Parameters.Add("@SymptomName", SqlDbType.NVarChar, 50).Value = symptom;
                        con1.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        con1.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Symptom Ddl updated");
            }
        }

        protected void CheckOut(object sender, EventArgs e)
        {
            if (CheckClinicalSubmitted(getAccNo.Text).Equals(false))
            {
                //- Display popup message
                getMessage.Text = "Check-Out failed. Please generate the Clinical Info for this patient first.";
                ModalPopupMessage.Show();
            }
            else if (CheckEMCGenerated(getAccNo.Text).Equals(false))
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
            /*
             * TODO: Logic to set checked-out to 1 from table qms
             */

            Response.Redirect("OPD.aspx");
        }

        protected void GenerateEMC(object sender, EventArgs e)
        {
            // conidtion check string tak empty: clinicalid

            if (CheckClinicalSubmitted(getAccNo.Text).Equals(false))
            {
                //- Display popup message
                getMessage.Text = "Clinical Info not yet submitted. Please generate Clinical Info first before releasing the e-MC for this patient.";
                ModalPopupMessage.Show();
            }
            else
            {
                //- Step 1: Generate Serial No.
                string _serialNo = GenerateSerialNo();

                //- Step 2: Encrypt and hash the Serial No. with salt
                string[] encryptedUrl = Hashing(_serialNo);
                string urlHashed = encryptedUrl[0];
                string urlSalt = encryptedUrl[1];

                //- Step 3: Generate QR Code
                byte[] qrCode = GenerateQRCode(urlHashed);

                //- Step 4: Generate password
                string[] generatedPassword = GeneratePassword();
                string password = generatedPassword[0];
                string encryptedPassword = generatedPassword[1];
                getPassword.Text = password;

                //- Step 5: Validate and calculate period
                DateTime dt1 = Convert.ToDateTime(getDateFrom.Text);
                DateTime dt2 = Convert.ToDateTime(getDateTo.Text);
                DateTime dtNow = DateTime.Now;

                if (dt2.Date < dtNow.Date)
                {
                    //- Display popup message
                    ErrorDate.Visible = true;
                    getMessage.Text = "The selected date is a passed date!";
                    ModalPopupMessage.Show();
                    goto done;
                }

                int period = 1 + (dt2 - dt1).Days;
                getPeriod.Text = period.ToString();
                //-- Step 5.1: Check if the e-MC selected date and period is valid
                if (period <= 0)
                {
                    //- Display popup message
                    ErrorDate.Visible = true;
                    getMessage.Text = "The selected dates and period of e-MC are invalid!";
                    ModalPopupMessage.Show();
                    goto done;
                }

                //- Step 6: Get comment
                string comment = getComment.Text;
                if (String.IsNullOrEmpty(comment))
                    comment = "-";

                /*
                 * TODO: Logic to obtain the current user (MO) IC No 
                 */
                //- Step 7: Get the MO's IC
                string staffIcNo = "981223144001";                

                //- Step 8: Database INSERT query
                //-- DB Exception-Error handling
                try
                {
                    //-- Insert Query
                    string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                    SqlConnection con = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand("SubmitEMC", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //-- Insert to table 'emc'
                    cmd.Parameters.AddWithValue("@SerialNo", _serialNo);
                    cmd.Parameters.AddWithValue("@UrlHashed", urlHashed);
                    cmd.Parameters.AddWithValue("@UrlSalt", urlSalt);
                    cmd.Parameters.AddWithValue("@QrCode", qrCode);
                    cmd.Parameters.AddWithValue("@Password", encryptedPassword);
                    cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(getDateFrom.Text));
                    cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(getDateTo.Text));
                    cmd.Parameters.AddWithValue("@Period", period);
                    cmd.Parameters.AddWithValue("@TimeCreated", TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss")));
                    cmd.Parameters.AddWithValue("@Comment", comment);
                    cmd.Parameters.AddWithValue("@PIcNo", GetPatientIcNo(getAccNo.Text));
                    cmd.Parameters.AddWithValue("@SIcNo", staffIcNo);
                    cmd.Parameters.AddWithValue("@ClinicalId", GetPatientClinicalId(getAccNo.Text));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    //- Display handling-error message
                    SqlExceptionMsg(ex);
                }
                finally
                {
                    //- Display success message
                    Debug.WriteLine("DB Execution Success: Generate Queue");
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

                //- Display popup message
                getMessage.Text = "The Electronic Medical Certificate has been generated successfully.";
                ModalPopupMessage.Show();
            }
        done:;
        }

        protected void ResetClinicalInfo(object sender, EventArgs e)
        {
            SymptomDdl.SelectedIndex = -1;
            getSymptomDesc.Text = String.Empty;
            SignDdl.SelectedIndex = -1;
            getSignDesc.Text = String.Empty;
            DiagnosisDdl.SelectedIndex = -1;
            getDiagnosisDesc.Text = String.Empty;
            Debug.WriteLine("All field input in Clinical Info has been reset.");
        }

        protected void SendEMC(object sender, EventArgs e)
        {
            if (CheckEMCGenerated(getAccNo.Text).Equals(true))
            {
                string url = "https://localhost:44316/e-MC.aspx?token=" + eMCHashedUrl;
                string body = @"<html>
                                    <body>
                                        <p>Dear patient,</p>
                                        <p>Thank you for visiting Pusat Kesihatan UTM (PKU) for a medical check-up. Here is the link to your <b>Electronic Medical Certificate (e-MC):</b></p>
                                        <p>" + url + @"</p>
                                        <br />
                                        <p>e-MC Password:</p>
                                        <h1>" + getPassword.Text + @"</h1>
                                        <p>Please save or keep a copy of the Password and download the e-MC for future reference. Please do contact us immediately, if you have any issues with the e-MC.</p>
                                        <br />
                                        <p><b>RULES AND REGULATIONS (Please Read!)</b></p>
                                        <p>1. This is a confidential document, only share with the party who requires it such as your employer or lecturer.</p>
                                        <p>2. <b>Do not share the URL</b> to public or untrusted party.</p>
                                        <p>3. <b>Do not share the PASSWORD</b> to public or untrusted party.</p>
                                        <p>4. You may have to provide/share the password to the party that requires this document in order to view it.</p>
                                        <p>5. This e-MC only valid within the given dates by your Medical Officer.</p>
                                    </body>
                                </html>";

                //- Email content
                MailMessage mailMsg = new MailMessage
                {
                    From = new MailAddress("psmtisma@gmail.com")
                };
                mailMsg.To.Add(patientEmail);
                mailMsg.Subject = "Electronic Medical Certificate PKU: " + GetPatientEMCSerialNo(getAccNo.Text);
                mailMsg.Body = body;
                mailMsg.IsBodyHtml = true;

                //- Email logic
                System.Net.NetworkCredential net = new System.Net.NetworkCredential
                {
                    UserName = "psmtisma@gmail.com",
                    Password = "psmtisma2021"
                };
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    UseDefaultCredentials = true,
                    Credentials = net,
                    Port = 587,
                    EnableSsl = true
                };
                smtp.Send(mailMsg);

                //- Display popup message
                getMessage.Text = "The Electronic Medical Certificate has been sent to the patient's email address successfully.";
                ModalPopupMessage.Show();
            }
            else
            {
                //- Display popup message
                getMessage.Text = "The e-MC is not generated yet.";
                ModalPopupMessage.Show();
            }
        }

        protected void SubmitClinicalInfo(object sender, EventArgs e)
        {
            /*
             * TODO: Logic to obtain the current user (MO) IC No 
             */
            string staffIcNo = "981223144001";

            //- Proceed when all required input are filled.
            if (!String.IsNullOrEmpty(SymptomDdl.Text) && !String.IsNullOrEmpty(SignDdl.Text) && !String.IsNullOrEmpty(DiagnosisDdl.Text))
            {
                //- DB Exception-Error handling
                try
                {
                    //- Insert data
                    string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                    SqlConnection con = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand("SubmitClinicalInfo", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Symptom", SymptomDdl.SelectedValue);
                    cmd.Parameters.AddWithValue("@SymptomDesc", getSymptomDesc.Text);
                    cmd.Parameters.AddWithValue("@Sign", SignDdl.SelectedValue);
                    cmd.Parameters.AddWithValue("@SignDesc", getSignDesc.Text);
                    cmd.Parameters.AddWithValue("@Diagnosis", DiagnosisDdl.SelectedValue);
                    cmd.Parameters.AddWithValue("@DiagnosisDesc", getDiagnosisDesc.Text);
                    cmd.Parameters.AddWithValue("@PIcNo", GetPatientIcNo(getAccNo.Text));
                    cmd.Parameters.AddWithValue("@SIcNo", staffIcNo);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    //- Display handling-error message
                    SqlExceptionMsg(ex);
                }
                finally
                {
                    //- Display success message
                    Debug.WriteLine("DB Execution Success: Add Clinical Info");
                }

                //- Hide submit button and show update button for Clinical Info
                BtnSubmit.Visible = false;
                BtnUpdate.Visible = true;

                //- Show note of submit/update
                NoteClinical.Visible = true;
            }

            //- Display popup message
            getMessage.Text = "The Clinical Information has been submitted successfully.";
            ModalPopupMessage.Show();
        }

        protected void UpdateClinicalInfo(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(SymptomDdl.SelectedValue) && !String.IsNullOrEmpty(SignDdl.SelectedValue) && !String.IsNullOrEmpty(DiagnosisDdl.SelectedValue))
            {
                //- DB Exception-Error handling
                try
                {
                    //- Update Query
                    string constr2 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                    using (SqlConnection con2 = new SqlConnection(constr2))
                    {
                        //- Table 'patient'
                        using (SqlCommand cmd2 = new SqlCommand("UPDATE clinical_info " +
                                                                "SET symptom = '" + SymptomDdl.SelectedValue + "', " +
                                                                    "symptom_desc = '" + getSymptomDesc.Text + "', " +
                                                                    "ill_sign = '" + SignDdl.SelectedValue + "', " +
                                                                    "ill_sign_desc = '" + getSignDesc.Text + "', " +
                                                                    "diagnosis = '" + DiagnosisDdl.SelectedValue + "', " +
                                                                    "diagnosis_desc = '" + getDiagnosisDesc.Text + "' " +
                                                                "WHERE clinical_id = '" + GetPatientClinicalId(getAccNo.Text) + "' AND fk_p_ic_no = '" + GetPatientIcNo(getAccNo.Text) + "'", con2))
                        {
                            con2.Open();
                            cmd2.ExecuteNonQuery();
                            con2.Close();
                            con2.Dispose();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    //- Display handling-error message
                    SqlExceptionMsg(ex);
                }
                finally
                {
                    //- Display success message
                    Debug.WriteLine("DB Execution Success: Update Clinical Info");
                }

                //- Hide submit button and show update button for Clinical Info
                BtnSubmit.Visible = false;
                BtnUpdate.Visible = true;

                //- Show note of submit/update
                NoteClinical.Visible = true;

                //- Display popup message
                getMessage.Text = "Changes on Clinical Information has been updated!";
                ModalPopupMessage.Show();
            }
            else
            {
                //- Display popup message
                getMessage.Text = "Update failed. Please enter the required input!";
                ModalPopupMessage.Show();
            }
        }

        protected void UpdateEMCInfo(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(getDateFrom.Text) && !String.IsNullOrEmpty(getDateTo.Text))
            {
                //- Validate date and calculate period
                DateTime dt1 = Convert.ToDateTime(getDateFrom.Text);
                DateTime dt2 = Convert.ToDateTime(getDateTo.Text);
                DateTime dtNow = DateTime.Now;

                if ( dt2.Date < dtNow.Date )
                {
                    //- Display popup message
                    ErrorDate.Visible = true;
                    getMessage.Text = "Update failed. The selected date is a passed date!";
                    ModalPopupMessage.Show();
                    goto done;
                }

                int period = 1 + (dt2 - dt1).Days;
                getPeriod.Text = period.ToString();
                //- Check if the e-MC selected date and period is valid
                if (period <= 0)
                {
                    //- Display popup message
                    ErrorDate.Visible = true;
                    getMessage.Text = "Update failed. The selected dates and period of e-MC are invalid!";
                    ModalPopupMessage.Show();
                    goto done;
                }

                //- Get comment
                string comment = getComment.Text;
                if (String.IsNullOrEmpty(comment))
                    comment = "-";

                //- DB Exception-Error handling
                try
                {
                    GetPatientEMCSerialNo(getAccNo.Text);
                    //- Update Query
                    string constr2 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                    using (SqlConnection con2 = new SqlConnection(constr2))
                    {
                        //- Table 'patient'
                        using (SqlCommand cmd2 = new SqlCommand("UPDATE emc " +
                                                                "SET date_from = '" + getDateFrom.Text + "', " +
                                                                    "date_to = '" + getDateTo.Text + "', " +
                                                                    "emc_period = '" + period + "', " +
                                                                    "comment = '" + comment + "' " +
                                                                "WHERE serial_no = '" + GetPatientEMCSerialNo(getAccNo.Text) + "' AND fk_p_ic_no = '" + GetPatientIcNo(getAccNo.Text) + "'", con2))
                        {
                            con2.Open();
                            cmd2.ExecuteNonQuery();
                            con2.Close();
                            con2.Dispose();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    //- Display handling-error message
                    SqlExceptionMsg(ex);
                }
                finally
                {
                    //- Display success message
                    Debug.WriteLine("DB Execution Success: Update e-MC Info");
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

                //- Display popup message
                getMessage.Text = "Changes on Electronic Medical Certificate has been updated!";
                ModalPopupMessage.Show();
            }
            else
            {
                //- Display popup message
                getMessage.Text = "Update failed. Please enter the date(s) of e-MC!";
                ModalPopupMessage.Show();
            }
        done:;
        }

        protected void ViewEMC(object sender, EventArgs e)
        {
            string fullUrl = "e-MC.aspx?token=" + eMCHashedUrl;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('" + fullUrl + "');", true);
        }

        protected void BindClinicalInfoDdl()
        {
            //- DB Exception-Error handling
            try
            {
                //- Get Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    //- Symptom Ddl
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM symptomDb"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();

                        SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        SymptomDdl.DataTextField = ds.Tables[0].Columns["symptom_name"].ToString();
                        SymptomDdl.DataValueField = ds.Tables[0].Columns["symptom_name"].ToString();
                        SymptomDdl.DataSource = ds.Tables[0];
                        SymptomDdl.DataBind();

                        con1.Close();
                    }

                    //- Sign Ddl
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM signDb"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();

                        SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        SignDdl.DataTextField = ds.Tables[0].Columns["sign_name"].ToString();
                        SignDdl.DataValueField = ds.Tables[0].Columns["sign_name"].ToString();
                        SignDdl.DataSource = ds.Tables[0];
                        SignDdl.DataBind();

                        con1.Close();
                    }

                    //- Diagnosis Ddl
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM diagnosisDb"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();

                        SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        DiagnosisDdl.DataTextField = ds.Tables[0].Columns["diagnosis_name"].ToString();
                        DiagnosisDdl.DataValueField = ds.Tables[0].Columns["diagnosis_name"].ToString();
                        DiagnosisDdl.DataSource = ds.Tables[0];
                        DiagnosisDdl.DataBind();

                        con1.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Retrieve patient data from DB");
            }
        }

        private void BindGrid(string accNo)
        {
            //- DB Exception-Error handling
            try
            {
                //- Retrieve Query: TISMADB
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr))
                {
                    SqlCommand cmd1 = new SqlCommand("SELECT clinical_info.clinical_date, clinical_info.symptom, clinical_info.ill_sign, clinical_info.diagnosis, pku_staff.s_name " +
                                                     "FROM clinical_info LEFT OUTER JOIN pku_staff ON pku_staff.s_ic_no = clinical_info.fk_s_ic_no " +
                                                     "WHERE clinical_info.fk_p_ic_no = '" + GetPatientIcNo(accNo) +"' " +
                                                     "ORDER BY clinical_info.clinical_date", con1);
                    con1.Open();
                    SqlDataReader sdr1 = cmd1.ExecuteReader();

                    if (sdr1.HasRows)
                    {
                        //- If records available
                        ClinicalHistoryTable.DataSource = sdr1;
                        ClinicalHistoryTable.DataBind();
                    }
                    else
                    {
                        //- If no records found
                        DataTable dt1 = new DataTable();
                        ClinicalHistoryTable.DataSource = dt1;
                        ClinicalHistoryTable.DataBind();
                    }
                    con1.Close();
                }
                using (SqlConnection con2 = new SqlConnection(constr))
                {
                    SqlCommand cmd2 = new SqlCommand("SELECT date_created, url_hashed, emc_password " +
                                                     "FROM emc WHERE fk_p_ic_no = '" + GetPatientIcNo(accNo) + "' " +
                                                     "ORDER BY date_created", con2);
                    con2.Open();
                    SqlDataReader sdr2 = cmd2.ExecuteReader();

                    if (sdr2.HasRows)
                    {
                        //- If records available
                        MCHistoryTable.DataSource = sdr2;
                        MCHistoryTable.DataBind();
                    }
                    else
                    {
                        //- If no records found
                        DataTable dt2 = new DataTable();
                        MCHistoryTable.DataSource = dt2;
                        MCHistoryTable.DataBind();
                    }
                    con2.Close();
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: ");
            }

            //- Render table
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

        protected static string GetPatientClinicalId(string accNo)
        {
            string clinicalId = "";
            //- DB Exception-Error handling
            try
            {
                //- Get Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    using (SqlCommand cmd1 = new SqlCommand("SELECT clinical_id FROM clinical_info " +
                                                           "WHERE fk_p_ic_no = '" + GetPatientIcNo(accNo) + "' AND Convert(date, clinical_date) = convert(date, GETDATE()) " +
                                                           "ORDER BY clinical_id DESC"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr = cmd1.ExecuteReader())
                        {
                            if (sdr.Read())
                                clinicalId = sdr["clinical_id"].ToString();
                        }
                        con1.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Get Clinical ID");
            }
            return clinicalId;
        }

        protected void GetPatientClinicalInfo(string accNo)
        {
            //- DB Exception-Error handling
            try
            {
                //- Get Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM clinical_info WHERE clinical_id = '" + GetPatientClinicalId(accNo) + "'"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr = cmd1.ExecuteReader())
                        {
                            sdr.Read();
                            
                            //- Parse data
                            SymptomDdl.SelectedValue = sdr["symptom"].ToString();
                            getSymptomDesc.Text = sdr["symptom_desc"].ToString();
                            SignDdl.SelectedValue = sdr["ill_sign"].ToString();
                            getSignDesc.Text = sdr["ill_sign_desc"].ToString();
                            DiagnosisDdl.SelectedValue = sdr["diagnosis"].ToString();
                            getDiagnosisDesc.Text = sdr["diagnosis_desc"].ToString();
                        }
                        con1.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Get Clinical Info");
            }
        }

        protected static string GetPatientEMCSerialNo(string accNo)
        {
            string serialNo = "";
            //- DB Exception-Error handling
            try
            {
                //- Get Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    using (SqlCommand cmd1 = new SqlCommand("SELECT serial_no FROM emc " +
                                                           "WHERE fk_p_ic_no = '" + GetPatientIcNo(accNo) + "' AND CONVERT(DATE, date_from) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, date_to) >= CONVERT(DATE, GETDATE()) " +
                                                           "ORDER BY date_created DESC"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr = cmd1.ExecuteReader())
                        {
                            if (sdr.Read())
                                serialNo = sdr["serial_no"].ToString();
                        }
                        con1.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Get Serial No");
            }
            return serialNo;
        }

        protected void GetPatientEMCInfo(string accNo)
        {
            //- DB Exception-Error handling
            try
            {
                //- Get Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM emc WHERE serial_no = '" + GetPatientEMCSerialNo(accNo) + "'"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr = cmd1.ExecuteReader())
                        {
                            sdr.Read();

                            //- Parse data
                            getPeriod.Text = sdr["emc_period"].ToString();
                            DateTime.TryParse(sdr["date_from"].ToString(), out DateTime dateFrom);
                            getDateFrom.Text = dateFrom.ToString("yyyy-MM-dd");
                            DateTime.TryParse(sdr["date_to"].ToString(), out DateTime dateTo);
                            getDateTo.Text = dateTo.ToString("yyyy-MM-dd");
                            getComment.Text = sdr["comment"].ToString();
                            string password = sdr["emc_password"].ToString();
                            getPassword.Text = DecryptEMCPassword(password);
                            eMCHashedUrl = sdr["url_hashed"].ToString();
                        }
                        con1.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Get e-MC info");
            }
        }

        protected static string GetPatientIcNo(string accNo)
        {
            string icNo = "";
            //- DB Exception-Error handling
            try
            {
                //- Get Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    //- Patient Info
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient WHERE p_account_no = '" + accNo + "'"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                        {
                            sdr1.Read();

                            //- Parse data
                            icNo = sdr1["p_ic_no"].ToString();
                        }
                        con1.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: ");
            }
            return icNo;
        }

        protected void GetPatientInfo(string accNo, string queueNo)
        {
            string designation, category;
            //- DB Exception-Error handling
            try
            {
                //- Get Query
                string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    //- Patient Info
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient WHERE p_account_no = '" + accNo + "'"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                        {
                            sdr1.Read();

                            //- Parse Date value from SQL to DateTime obj
                            DateTime.TryParse(sdr1["p_dob"].ToString(), out DateTime dob);

                            //- Parse for local use
                            category = sdr1["p_category"].ToString();

                            //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
                            DateTime dobToAge = Convert.ToDateTime(dob);
                            int age = DateTime.Now.AddYears(-dobToAge.Year).Year;

                            //- Parse data
                            getQueueNo.Text = queueNo;
                            getAccNo.Text = accNo;
                            getCategory.Text = category;
                            getName.Text = sdr1["p_name"].ToString();
                            getDOB.Text = dob.ToString("dd/MM/yyyy");
                            getAge.Text = age.ToString();
                            designation = sdr1["p_designation"].ToString();
                            patientEmail = sdr1["p_email"].ToString();
                            String remark = sdr1["p_remarks"].ToString();
                            if (String.IsNullOrEmpty(remark))
                                getRemark.Text = "-";
                            else
                                getRemark.Text = remark;
                        }
                        con1.Close();
                    }
                    if (category.Equals("Student"))
                    {
                        //- Get Query
                        using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient_student WHERE fk_ic_no = '" + GetPatientIcNo(accNo) + "'"))
                        {
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = con1;
                            con1.Open();
                            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                            {
                                sdr1.Read();

                                //- Parse data
                                getMatricStaffHeader.Text = "Matric/Staff No.";
                                getMatricStaff.Text = sdr1["matric_no"].ToString();
                                getFacDepHeader.Text = "Faculty/Department";
                                getFacDep.Text = sdr1["faculty"].ToString();
                            }
                            con1.Close();
                        }
                    }
                    else if (category.Equals("Staff"))
                    {
                        //- Get Query
                        using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient_staff WHERE fk_ic_no = '" + GetPatientIcNo(accNo) + "'"))
                        {
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Connection = con1;
                            con1.Open();
                            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                            {
                                sdr1.Read();

                                //- Parse data
                                getMatricStaffHeader.Text = "Matric/Staff No.";
                                getMatricStaff.Text = sdr1["staff_id"].ToString();
                                getFacDepHeader.Text = "Faculty/Department";
                                getFacDep.Text = sdr1["department"].ToString();
                            }
                            con1.Close();
                        }
                    }
                    else
                    {
                        getMatricStaffHeader.Text = "IC/Passport No.";
                        getMatricStaff.Text = GetPatientIcNo(accNo);
                        getFacDepHeader.Text = "Occupation";
                        getFacDep.Text = designation;
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            finally
            {
                //- Display success message
                Debug.WriteLine("DB Execution Success: Retrieve patient data from DB");
            }
        }

        public static bool CheckClinicalSubmitted(string accNo)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckClinicalSubmitted", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@IcNo", GetPatientIcNo(accNo).Trim());
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static bool CheckEMCGenerated(string accNo)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckEMCGenerated", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@IcNo", GetPatientIcNo(accNo).Trim());
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static bool CheckSerialNotExist(string _serialNo)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckSerialNotExist", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SerialNo", _serialNo.Trim());
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static string ConvertNumString(int num)
        {
            String str;
            if (num <= 9)
                str = "00" + num.ToString();
            else if (num >= 10 && num <= 99)
                str = "0" + num.ToString();
            else
                str = num.ToString();
            return str;
        }

        public static string DecryptURL(string encryptedURL)
        {
            //- Custom key
            string EncryptionKey = "3NCRYPTTH1SURLP4R4M";

            //- Decrypt logic
            encryptedURL = encryptedURL.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(encryptedURL);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    encryptedURL = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return encryptedURL;
        }

        public static string DecryptEMCPassword(string encryption)
        {
            //- Custom key
            string EncryptionKey = "3NCRYPTP@55W0RD3MC";

            //- Decrypt logic
            encryption = encryption.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(encryption);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    encryption = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return encryption;
        }

        public static string EncryptEMCPassword(string password)
        {
            //- Custom key
            string EncryptionKey = "3NCRYPTP@55W0RD3MC";

            //- Encryption logic
            String encrypted;
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encrypted = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encrypted;
        }

        public static string[] GeneratePassword()
        {
            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();
            int length = 8;
            char letter;
            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            string password = str_build.ToString();
            string encryptedPassword = EncryptEMCPassword(password);
            string[] generatedPassword = { password, encryptedPassword };

            return generatedPassword;
        }

        public static byte[] GenerateQRCode(string url)
        {
            string content = "https://localhost:44316/e-MC.aspx?token=" + url;
            byte[] byteImage = null;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);
            QRCode qrCode = new QRCode(qrCodeData);
            using (System.Web.UI.WebControls.Image imgQRCode = new System.Web.UI.WebControls.Image())
            {
                imgQRCode.Height = 150;
                imgQRCode.Width = 150;
            }
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byteImage = new byte[ms.ToArray().Length];
                    byteImage = ms.ToArray();
                    //imgQRCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage); This will be use for displaying the QR Code in e-MC
                }
            }
            return byteImage;
        }

        public static string GenerateSerialNo()
        {
            //- Step 1: Get today's date
            DateTime dateCreated = DateTime.Now;

            //- Step 2: Set today date pattern
            string today = dateCreated.ToString("yy/MM/dd");
            today = today.Remove(5, 1); // yyyy/MM_dd
            today = today.Remove(2, 1); // yyyy_MMdd

            //- Step 3: Append today date pattern to another string pattern
            string _serialNo;
            _serialNo = "PKUeMC" + today;

            //- Step 4: Generate and append a string no. to _serialNo
            //-- Step 4.1: Produce temp SerialNo and initialize a string no.
            int n = 1;
            string strNum = ConvertNumString(n);
            string tempSerialNo = _serialNo + strNum;

            //-- Step 4.2: Check is Serial No pattern not exist?
            while (CheckSerialNotExist(tempSerialNo).Equals(false))
            {
                //-- If exist (the CheckIsQueueNotExist return false)
                n += 1;
                strNum = ConvertNumString(n);
                tempSerialNo = _serialNo + strNum;
            }
            _serialNo = tempSerialNo;
            Debug.WriteLine("The generated Serial No. [" + _serialNo + "] not existed yet.");
            return _serialNo;
        }

        public static string[] Hashing(string _serialNo)
        {
            string salt = Crypto.GenerateSalt();
            string hashed = Crypto.HashPassword(salt + _serialNo);
            string[] encryption = { hashed, salt };

            return encryption;
        }

        public static void SqlExceptionMsg(SqlException ex)
        {
            StringBuilder errorMessages = new StringBuilder();
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + "\n" +
                "Message: " + ex.Errors[i].Message + "\n" +
                "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                "Source: " + ex.Errors[i].Source + "\n" +
                "Procedure: " + ex.Errors[i].Procedure + "\n");
            }
            Debug.WriteLine(errorMessages.ToString());
        }
    }
}