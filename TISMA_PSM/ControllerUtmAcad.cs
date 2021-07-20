using System;
using System.Data.SqlClient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    /**
     * This is a Controller Class for UTM-ACAD.
     * 
     * All SQL and public methods related only to UTM-ACAD model 
     * that are used repeatedly are implemented in this Class.
     * 
     */
    public class ControllerUtmAcad
    {
        /**
         * Validate if IC No in UTM-ACAD already exist
         * Type: bool
         */
        public static bool CheckIcNoUtmAcadNotExist(string IcNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringUtmAcad()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT ic_no FROM utm_acad_tbl WHERE ic_no = '" + IcNo + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                                status = false;
                            else
                                status = true;
                        }
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
         * Get a UTM-ACAD info from DB as a ModelUtmAcad object
         * Type: ModelUtmAcad
         */
        public static ModelUtmAcad GetUtmAcadInfoByIcNo(string icNo)
        {
            //- Object Staff model
            ModelUtmAcad acad = new ModelUtmAcad();

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringUtmAcad()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM utm_acad_tbl WHERE ic_no = '" + icNo + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();

                            //- Parse Date value from SQL to DateTime obj
                            DateTime.TryParse(sdr["dob"].ToString(), out DateTime dob);

                            acad.icNo = sdr["ic_no"].ToString();
                            acad.passportNo = sdr["passport_no"].ToString();
                            acad.matricNo = sdr["matric_no"].ToString();
                            acad.telNo = sdr["tel_no"].ToString();
                            acad.email = sdr["email"].ToString();
                            acad.name = sdr["name"].ToString();
                            acad.dob = dob.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            acad.age = int.Parse(sdr["age"].ToString());
                            acad.gender = sdr["gender"].ToString();
                            acad.maritalStat = sdr["marital_stat"].ToString();
                            acad.religion = sdr["religion"].ToString();
                            acad.race = sdr["race"].ToString();
                            acad.nationality = sdr["nationality"].ToString();
                            acad.address = sdr["student_address"].ToString();
                            acad.designation = sdr["designation"].ToString();
                            acad.course = sdr["course"].ToString();
                            acad.faculty = sdr["faculty"].ToString();
                            acad.semester = int.Parse(sdr["semester"].ToString());
                            acad.session = sdr["session_no"].ToString();
                            acad.category = sdr["category"].ToString();
                            acad.branch = sdr["branch"].ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return acad;
        }
    }
}