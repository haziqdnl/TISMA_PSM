using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace TISMA_PSM
{
    public partial class Patient_Info : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Retrieve pid (ic_no) and stat from URL param
            String acc_no = Request.QueryString["accno"];

            //- Local attribute
            String ic_no, category, noteStat = "";

            string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(constr1))
            {
                using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient WHERE p_account_no = '" + acc_no + "'"))
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
                        ic_no = sdr1["p_ic_no"].ToString();
                        category = sdr1["p_category"].ToString();

                        //- Parse data
                        getIcNo.Text = ic_no; 
                        getAccNo.Text = sdr1["p_account_no"].ToString(); displayAccNo.Text = sdr1["p_account_no"].ToString();
                        getPassportNo.Text = sdr1["p_passport_no"].ToString();
                        getPhone.Text = sdr1["p_tel_no"].ToString();
                        getEmail.Text = sdr1["p_email"].ToString();
                        getName.Text = sdr1["p_name"].ToString();
                        getDob.Text = dob.ToString("dd/MM/yyyy");
                        getAge.Text = sdr1["p_age"].ToString();
                        getGender.Text = sdr1["p_gender"].ToString();
                        getMaritalStat.Text = sdr1["p_marital_stat"].ToString();
                        getReligion.Text = sdr1["p_religion"].ToString();
                        getRace.Text = sdr1["p_race"].ToString();
                        getNation.Text = sdr1["p_nationality"].ToString();
                        getAddress.Text = sdr1["p_address"].ToString();
                        getDesignation.Text = sdr1["p_designation"].ToString();
                        getCategory.Text = category; displayCategory.Text = category;
                        getSession.Text = sdr1["p_session"].ToString();
                        getBranch.Text = sdr1["p_branch"].ToString();
                        getRemarks.Text = sdr1["p_remarks"].ToString();
                    }
                    con1.Close();
                }
                if (category.Equals("Student"))
                {
                    //- Display note
                    noteStat = "The basic information were reffered from UTMHR SYSTEM / ACAD SYSTEM";

                    //- Retrieve data
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient_student WHERE fk_ic_no = '" + ic_no + "'"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                        {
                            sdr1.Read();

                            //- Status activity
                            if (sdr1["utm_acad_stat"].ToString().Equals("True"))
                                statusText.Text = "Active";
                            else
                                statusText.Text = "Not-active";

                            //- Parse data
                            getMatricNo.Text = sdr1["matric_no"].ToString();
                            getFacDep.Text = sdr1["faculty"].ToString();
                            getCourse.Text = sdr1["course"].ToString();
                            getSem.Text = sdr1["semester"].ToString();
                            getStat.Text = "UTM-ACAD";
                        }
                        con1.Close();
                    }
                }
                else if (category.Equals("Staff"))
                {
                    //- Display note
                    noteStat = "The basic information were reffered from UTMHR SYSTEM / ACAD SYSTEM";

                    //- Retrieve data
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient_staff WHERE fk_ic_no = '" + ic_no + "'"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                        {
                            sdr1.Read();

                            //- Status activity
                            if (sdr1["utm_hr_stat"].ToString().Equals("True"))
                                statusText.Text = "Active";
                            else
                                statusText.Text = "Not-active";

                            //- Parse data
                            getMatricNo.Text = sdr1["staff_id"].ToString();
                            getFacDep.Text = sdr1["department"].ToString();
                            getStat.Text = "UTM-HR";
                        }
                        con1.Close();
                    }
                }
                else if (category.Equals("Public"))
                {
                    //- Display note
                    noteStat = "The basic information is registered as public";

                    //- Retrieve data
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM patient_public WHERE fk_ic_no = '" + ic_no + "'"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                        {
                            sdr1.Read();

                            //- Status activity
                            if (sdr1["public_stat"].ToString().Equals("True"))
                                statusText.Text = "Not-active";

                            //- Parse data
                            getStat.Text = "";
                        }
                        con1.Close();
                    }
                }
            }
            note.Text = noteStat;
        }
    }
}