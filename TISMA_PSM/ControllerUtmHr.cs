using System;
using System.Data.SqlClient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    /**
     * This is a Controller Class for UTM-HR.
     * 
     * All SQL and public methods related only to Patient model  
     * that are used repeatedly are implemented in this Class.
     * 
     */
    public class ControllerUtmHr
    {
        /**
         * Validate if IC No in UTM-HR already exist
         * Type: bool
         */
        public static bool CheckIcNoUtmHrNotExist(string IcNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringUtmHr()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT ic_no FROM utm_hr_tbl WHERE ic_no = '" + IcNo + "'", con))
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
         * Get a UTM-HR info from DB as a ModelUtmHr object
         * Type: ModelUtmHr
         */
        public static ModelUtmHr GetUtmHrInfoByIcNo(string icNo)
        {
            //- Object Staff model
            ModelUtmHr hr = new ModelUtmHr();

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringUtmHr()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM utm_hr_tbl WHERE ic_no = '" + icNo + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();

                            //- Parse Date value from SQL to DateTime obj
                            DateTime.TryParse(sdr["dob"].ToString(), out DateTime dob);

                            hr.sIcNo = sdr["ic_no"].ToString();
                            hr.sPassportNo = sdr["passport_no"].ToString();
                            hr.sStaffId = sdr["staff_id"].ToString();
                            hr.sTelNo = sdr["tel_no"].ToString();
                            hr.sEmail = sdr["email"].ToString(); ;
                            hr.sName = sdr["name"].ToString();
                            hr.sDob = dob.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            hr.sAge = int.Parse(sdr["age"].ToString());
                            hr.sGender = sdr["gender"].ToString();
                            hr.sMaritalStat = sdr["marital_stat"].ToString();
                            hr.sReligion = sdr["religion"].ToString();
                            hr.sRace = sdr["race"].ToString();
                            hr.sNationality = sdr["nationality"].ToString();
                            hr.sAddress = sdr["staff_address"].ToString();
                            hr.sDesignation = sdr["designation"].ToString();
                            hr.sDepartment = sdr["department"].ToString();
                            hr.sSession = sdr["session_no"].ToString();
                            hr.sCategory = sdr["category"].ToString();
                            hr.sBranch = sdr["branch"].ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return hr;
        }
    }
}