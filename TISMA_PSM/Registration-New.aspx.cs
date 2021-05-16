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
    public partial class Registration_New : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Retrieve pid (ic_no) and stat from URL param
            string ic_no = Request.QueryString["pid"];
            string tempStat = Request.QueryString["stat"];
            
            //- Status identifier
            String stat = IdentifyStatus(tempStat);

            //- Retrieve UTM-ACAD data
            if (stat.Equals("UTM-ACAD"))
            {
                string constr1 = ConfigurationManager.ConnectionStrings["utmacadConnectionString"].ConnectionString;
                using (SqlConnection con1 = new SqlConnection(constr1))
                {
                    using (SqlCommand cmd1 = new SqlCommand("SELECT * FROM utm_acad_tbl WHERE ic_no = '" + ic_no + "'"))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = con1;
                        con1.Open();
                        using (SqlDataReader sdr1 = cmd1.ExecuteReader())
                        {
                            sdr1.Read();

                            //- Parse Date value from SQL to DateTime obj
                            DateTime.TryParse(sdr1["dob"].ToString(), out DateTime dob);

                            getBranch.Text = sdr1["branch"].ToString();
                            getStat.Text = stat;
                            getAccNo.Text = GenerateAccNo(stat);
                            getCategory.Text = sdr1["category"].ToString();
                            getName.Text = sdr1["name"].ToString();
                            getMatricNo.Text = sdr1["matric_no"].ToString();
                            getIcNo.Text = sdr1["ic_no"].ToString();
                            getPassportNo.Text = sdr1["passport_no"].ToString();
                            getDob.Text = dob.ToString("dd/MM/yyyy");
                            getAge.Text = sdr1["age"].ToString();
                            getGender.Text = sdr1["gender"].ToString();
                            getMaritalStat.Text = sdr1["marital_stat"].ToString();
                            getReligion.Text = sdr1["religion"].ToString();
                            getRace.Text = sdr1["race"].ToString();
                            getNation.Text = sdr1["nationality"].ToString();
                            getAddress.Text = sdr1["student_address"].ToString();
                            getDesignation.Text = sdr1["designation"].ToString();
                            getFacDep.Text = sdr1["faculty"].ToString();
                            getCourse.Text = sdr1["course"].ToString();
                            getSem.Text = sdr1["semester"].ToString();
                            getPhone.Text = sdr1["tel_no"].ToString();
                            getSession.Text = sdr1["session_no"].ToString();
                            getEmail.Text = sdr1["email"].ToString();
                        }
                        con1.Close();
                    }
                }
            }
            //- Retrieve UTM-HR data
            else if (stat.Equals("UTM-HR"))
            {
                string constr2 = ConfigurationManager.ConnectionStrings["utmhrConnectionString"].ConnectionString;
                using (SqlConnection con2 = new SqlConnection(constr2))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT * FROM utm_hr_tbl WHERE ic_no = '" + ic_no + "'"))
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection = con2;
                        con2.Open();
                        using (SqlDataReader sdr2 = cmd2.ExecuteReader())
                        {
                            sdr2.Read();

                            // Parse Date value from SQL to DateTime obj
                            DateTime.TryParse(sdr2["dob"].ToString(), out DateTime dob);

                            getBranch.Text = sdr2["branch"].ToString();
                            getStat.Text = stat;
                            getAccNo.Text = GenerateAccNo(stat);
                            getCategory.Text = sdr2["category"].ToString();
                            getName.Text = sdr2["name"].ToString();
                            getMatricNo.Text = sdr2["staff_id"].ToString();
                            getIcNo.Text = sdr2["ic_no"].ToString();
                            getPassportNo.Text = sdr2["passport_no"].ToString();
                            getDob.Text = dob.ToString("dd/MM/yyyy");
                            getAge.Text = sdr2["age"].ToString();
                            getGender.Text = sdr2["gender"].ToString();
                            getMaritalStat.Text = sdr2["marital_stat"].ToString();
                            getReligion.Text = sdr2["religion"].ToString();
                            getRace.Text = sdr2["race"].ToString();
                            getNation.Text = sdr2["nationality"].ToString();
                            getAddress.Text = sdr2["staff_address"].ToString();
                            getDesignation.Text = sdr2["designation"].ToString();
                            getFacDep.Text = sdr2["department"].ToString();
                            getCourse.Text = "";
                            getSem.Text = "";
                            getPhone.Text = sdr2["tel_no"].ToString();
                            getSession.Text = sdr2["session_no"].ToString();
                            getEmail.Text = sdr2["email"].ToString();
                        }
                        con2.Close();
                    }
                }
            }
        }

        public static String IdentifyStatus(String tempStat)
        {
            String stat;
            if (tempStat.Equals("utmacad"))
                stat = "UTM-ACAD";
            else
                stat = "UTM-HR";

            return stat;
        } 

        public static String GenerateAccNo(String stat)
        {
            //- Step 1: Generate Date pattern
            String today = DateTime.Now.ToString("yyyy/MM/dd");
            today = today.Remove(7, 1); // yyyy/MM_dd
            today = today.Remove(4, 1); // yyyy_MMdd

            //- Step 2: Append status type to Date pattern as accNo
            String accNo;
            if (stat.Equals("UTM-ACAD"))
                accNo = today + 'A'; // A - ACAD
            else
                accNo = today + 'H'; // H - HR

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

            //-- Step 3.3: Identify the generated acc no. if it already existed in db
            if (CheckAccNotExist(accNo).Equals(true))
            {
                Debug.WriteLine("The generated Acc No. not existed yet.");
                return accNo;
            }
            else
            {
                Debug.WriteLine("The generated Acc No. already existed. Re-generating....");
                return GenerateAccNo(stat);
            }
        }

        public static bool CheckAccNotExist(String accNo)
        {
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckAccNotExist", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@AccNo", accNo.Trim());
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        protected void AddToTisma(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            //- Identify patient status
            String spFunc;
            if (getStat.Text.Equals("UTM-ACAD"))
                spFunc = "AddToTismaStudent";
            else
                spFunc = "AddToTismaStaff";

            SqlCommand cmd = new SqlCommand(spFunc, con)
            {
                CommandType = CommandType.StoredProcedure
            };
            //- Insert to table 'patient'
            cmd.Parameters.AddWithValue("@pIcNo", getIcNo.Text);
            cmd.Parameters.AddWithValue("@pAccNo", getAccNo.Text);
            cmd.Parameters.AddWithValue("@pPassport", getPassportNo.Text);
            cmd.Parameters.AddWithValue("@pTelNo", getPhone.Text);
            cmd.Parameters.AddWithValue("@pEmail", getEmail.Text);
            cmd.Parameters.AddWithValue("@pName", getName.Text);
            cmd.Parameters.AddWithValue("@Dob", Convert.ToDateTime(getDob.Text));
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

            if (getStat.Text.Equals("UTM-ACAD"))
            {
                //- Insert to table 'patient_student'
                cmd.Parameters.AddWithValue("@pMatric", getMatricNo.Text);
                cmd.Parameters.AddWithValue("@pFaculty", getFacDep.Text);
                cmd.Parameters.AddWithValue("@pCourse", getCourse.Text);
                cmd.Parameters.AddWithValue("@pSem", int.Parse(getSem.Text));
                cmd.Parameters.AddWithValue("@pAcadStat", 1); // 1 - True
            }
            else
            {
                //- Insert to table 'patient_staff'
                cmd.Parameters.AddWithValue("@pStaffId", getMatricNo.Text);
                cmd.Parameters.AddWithValue("@pDepartment", getFacDep.Text);
                cmd.Parameters.AddWithValue("@pHrStat", 1); // 1 - True
            }
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            Response.Redirect("Registration.aspx");
        }
    }
}