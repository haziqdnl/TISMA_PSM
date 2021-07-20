using System;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    /**
     * This is a Controller Class for Patient
     * 
     * All SQL and public methods related only to Patient model 
     * that are used repeatedly are implemented in this Class.
     * 
     */
    public class ControllerPatient
    {
        /**
         * Validate if patient account no. not exist
         * Type: bool
         */
        public static bool CheckAccPatientNotExist(string accNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckAccNotExist", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@AccNo", accNo.Trim());
                        status = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return status;
        }

        /**
         * Validate if this patient already added to TISMA
         * Type: bool
         */
        public static bool CheckIsPatientAddedToTisma(string icNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckIsPatientAddedToTisma", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@IcNo", icNo);
                        status = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return status;
        }

        /**
         * To generate Account No. for patient
         * Type: string
         */
        public static string GenerateAccNoPatient(string stat)
        {
            //- Step 1: Generate Date pattern
            string today = DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            today = today.Remove(7, 1); // yyyy/MM_dd
            today = today.Remove(4, 1); // yyyy_MMdd

            //- Step 2: Append status type to Date pattern as accNo
            string accNo;
            if (stat.Equals("UTM-ACAD"))
                accNo = today + 'A'; // A - ACAD
            else if (stat.Equals("UTM-HR"))
                accNo = today + 'H'; // H - HR
            else
                accNo = today + 'P'; // P - Public

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
            if (CheckAccPatientNotExist(accNo).Equals(true))
                return accNo;
            else
                return GenerateAccNoPatient(stat);
        }

        /**
         * Get patient info from DB as a ModelPatient object
         * Type: ModelPatient
         */
        public static ModelPatient GetPatientInfoByAccNo(string accNo)
        {
            //- Object Patient model
            ModelPatient patient = new ModelPatient();

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM patient WHERE p_account_no = '" + accNo + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();

                            //- Parse Date value from SQL to DateTime obj
                            DateTime.TryParse(sdr["p_dob"].ToString(), out DateTime dob);

                            //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
                            int age = DateTime.Now.AddYears(-dob.Year).Year;

                            patient.pAccountNo = sdr["p_account_no"].ToString();
                            patient.pAddress = sdr["p_address"].ToString();
                            patient.pAge = age;
                            patient.pBranch = sdr["p_branch"].ToString();
                            patient.pCategory = sdr["p_category"].ToString();
                            patient.pDesignation = sdr["p_designation"].ToString();
                            patient.pDob = dob.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            patient.pEmail = sdr["p_email"].ToString();
                            patient.pGender = sdr["p_gender"].ToString();
                            patient.pIcNo = sdr["p_ic_no"].ToString();
                            patient.pMaritalStat = sdr["p_marital_stat"].ToString();
                            patient.pName = sdr["p_name"].ToString();
                            patient.pNationality = sdr["p_nationality"].ToString();
                            patient.pPassportNo = sdr["p_passport_no"].ToString();
                            patient.pRace = sdr["p_race"].ToString();
                            patient.pReligion = sdr["p_religion"].ToString();
                            patient.pRemarks = sdr["p_remarks"].ToString();
                            patient.pSession = sdr["p_session"].ToString();
                            patient.pTelNo = sdr["p_tel_no"].ToString();
                        }
                    }
                    if (patient.pCategory.Equals("Student"))
                    {
                        //- Get Query
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM patient_student WHERE fk_ic_no = '" + patient.pIcNo + "'", con))
                        {
                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                //- Parse data
                                sdr.Read();

                                //- Status activity
                                if (sdr["utm_acad_stat"].ToString().Equals("True"))
                                    patient.utmAcadStat = true;
                                else
                                    patient.utmAcadStat = false;

                                patient.pFaculty = sdr["faculty"].ToString();
                                patient.pCourse = sdr["course"].ToString();
                                patient.pSemester = int.Parse(sdr["semester"].ToString());
                                patient.pMatricNo = sdr["matric_no"].ToString();
                            }
                        }
                    }
                    else if (patient.pCategory.Equals("Staff"))
                    {
                        //- Get Query
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM patient_staff WHERE fk_ic_no = '" + patient.pIcNo + "'", con))
                        {
                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                //- Parse data
                                sdr.Read();

                                //- Status activity
                                if (sdr["utm_hr_stat"].ToString().Equals("True"))
                                    patient.utmHrStat = true;
                                else
                                    patient.utmHrStat = false;

                                //- Parse data
                                patient.pStaffId = sdr["staff_id"].ToString();
                                patient.pDepartment = sdr["department"].ToString();
                            }
                        }
                    }
                    else if (patient.pCategory.Equals("Public"))
                    {
                        //- Get Query
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM patient_public WHERE fk_ic_no = '" + patient.pIcNo + "'", con))
                        {
                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                //- Parse data
                                sdr.Read();

                                //- Status activity
                                if (sdr["public_stat"].ToString().Equals("True"))
                                    patient.publicStat = true;
                                else
                                    patient.publicStat = false;
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
            return patient;
        }

        /**
         * Get patient info from DB as a ModelPatient object
         * Type: ModelPatient
         */
        public static ModelPatient GetPatientInfoByIcNo(string icNo)
        {
            //- Object Patient model
            ModelPatient patient = new ModelPatient();

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM patient WHERE p_ic_no = '" + icNo + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();

                            //- Parse Date value from SQL to DateTime obj
                            DateTime.TryParse(sdr["p_dob"].ToString(), out DateTime dob);

                            //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
                            int age = DateTime.Now.AddYears(-dob.Year).Year;

                            patient.pAccountNo = sdr["p_account_no"].ToString();
                            patient.pAddress = sdr["p_address"].ToString();
                            patient.pAge = age;
                            patient.pBranch = sdr["p_branch"].ToString();
                            patient.pCategory = sdr["p_category"].ToString();
                            patient.pDesignation = sdr["p_designation"].ToString();
                            patient.pDob = dob.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            patient.pEmail = sdr["p_email"].ToString();
                            patient.pGender = sdr["p_gender"].ToString();
                            patient.pIcNo = sdr["p_ic_no"].ToString();
                            patient.pMaritalStat = sdr["p_marital_stat"].ToString();
                            patient.pName = sdr["p_name"].ToString();
                            patient.pNationality = sdr["p_nationality"].ToString();
                            patient.pPassportNo = sdr["p_passport_no"].ToString();
                            patient.pRace = sdr["p_race"].ToString();
                            patient.pReligion = sdr["p_religion"].ToString();
                            patient.pRemarks = sdr["p_remarks"].ToString();
                            patient.pSession = sdr["p_session"].ToString();
                            patient.pTelNo = sdr["p_tel_no"].ToString();
                        }
                    }
                    if (patient.pCategory.Equals("Student"))
                    {
                        //- Get Query
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM patient_student WHERE fk_ic_no = '" + patient.pIcNo + "'", con))
                        {
                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                //- Parse data
                                sdr.Read();

                                //- Status activity
                                if (sdr["utm_acad_stat"].ToString().Equals("True"))
                                    patient.utmAcadStat = true;
                                else
                                    patient.utmAcadStat = false;

                                patient.pFaculty = sdr["faculty"].ToString();
                                patient.pCourse = sdr["course"].ToString();
                                patient.pSemester = int.Parse(sdr["semester"].ToString());
                                patient.pMatricNo = sdr["matric_no"].ToString();
                            }
                        }
                    }
                    else if (patient.pCategory.Equals("Staff"))
                    {
                        //- Get Query
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM patient_staff WHERE fk_ic_no = '" + patient.pIcNo + "'", con))
                        {
                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                //- Parse data
                                sdr.Read();

                                //- Status activity
                                if (sdr["utm_hr_stat"].ToString().Equals("True"))
                                    patient.utmHrStat = true;
                                else
                                    patient.utmHrStat = false;

                                //- Parse data
                                patient.pStaffId = sdr["staff_id"].ToString();
                                patient.pDepartment = sdr["department"].ToString();
                            }
                        }
                    }
                    else if (patient.pCategory.Equals("Public"))
                    {
                        //- Get Query
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM patient_public WHERE fk_ic_no = '" + patient.pIcNo + "'", con))
                        {
                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                //- Parse data
                                sdr.Read();

                                //- Status activity
                                if (sdr["public_stat"].ToString().Equals("True"))
                                    patient.publicStat = true;
                                else
                                    patient.publicStat = false;
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
            return patient;
        }
    }
}