using System;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    /**
     * This is a Controller Class for Staff/User.
     * 
     * All SQL and public methods related only to Staff model  
     * that are used repeatedly are implemented in this Class.
     * 
     */
    public class ControllerStaff
    {
        /**
         * Validate if staff account no. not exist
         * Type: bool
         */
        public static bool CheckAccStaffNotExist(string accNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckAccStaffNotExist", con) { CommandType = CommandType.StoredProcedure })
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
         * Validate if a phone no already exist
         * Type: bool
         */
        public static bool CheckIsPhoneStaffExist(string telNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckIsPhoneStaffExist", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@TelNo", telNo);
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
         * Validate if staff is already added into TISMA
         * Type: bool
         */
        public static bool CheckIsStaffAddedToTisma(string icNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckIsStaffAddedToTisma", con) { CommandType = CommandType.StoredProcedure })
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
         * Validate if staff is Admin
         * Type: bool
         */
        public static bool CheckIsStaffAdmin(string icNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckIsStaffAdmin", con) { CommandType = CommandType.StoredProcedure })
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
         * Validate if staff is MO
         * Type: bool
         */
        public static bool CheckIsStaffMO(string icNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckIsStaffMO", con) { CommandType = CommandType.StoredProcedure })
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
         * Validate if staff is Receptionist
         * Type: bool
         */
        public static bool CheckIsStaffReceptionist(string icNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckIsStaffReceptionist", con) { CommandType = CommandType.StoredProcedure })
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
         * Validate if reset password token not exist
         * Type: bool
         */
        public static bool CheckResetPasswordTokenNotExist(string token)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckResetPasswordTokenNotExist", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@Token", token.Trim());
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
         * Get user info from DB as a ModelStaff object
         * Type: ModelStaff
         */
        public static ModelStaff GetStaffInfoByAccNo(string accNo)
        {
            //- Object Staff model
            ModelStaff staff = new ModelStaff();

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM pku_staff WHERE s_account_no = '" + accNo + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();

                            //- Parse Date value from SQL to DateTime obj
                            DateTime.TryParse(sdr["s_dob"].ToString(), out DateTime dob);

                            //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
                            int age = DateTime.Now.AddYears(-dob.Year).Year;

                            staff.sIcNo = sdr["s_ic_no"].ToString();
                            staff.sPassportNo = sdr["s_passport_no"].ToString();
                            staff.sAccountNo = sdr["s_account_no"].ToString();
                            staff.sUsername = sdr["s_username"].ToString();
                            staff.sPasswordEncrypted = sdr["s_password"].ToString();
                            staff.sPasswordSalt = sdr["s_password_salt"].ToString();
                            staff.sStaffId = sdr["s_staff_id"].ToString();
                            staff.sTelNo = sdr["s_tel_no"].ToString();
                            staff.sEmail = sdr["s_email"].ToString();
                            staff.sName = sdr["s_name"].ToString();
                            staff.sDob = dob.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            staff.sAge = age;
                            staff.sGender = sdr["s_gender"].ToString();
                            staff.sMaritalStat = sdr["s_marital_stat"].ToString();
                            staff.sReligion = sdr["s_religion"].ToString();
                            staff.sRace = sdr["s_race"].ToString();
                            staff.sNationality = sdr["s_nationality"].ToString();
                            staff.sAddress = sdr["s_address"].ToString();
                            staff.sDesignation = sdr["s_designation"].ToString();
                            staff.sDepartment = sdr["s_department"].ToString();
                            staff.sSession = sdr["s_session"].ToString();
                            staff.sCategory = sdr["s_category"].ToString();
                            staff.sBranch = sdr["s_branch"].ToString();

                            if (CheckIsStaffAdmin(staff.sIcNo).Equals(true))
                            {
                                staff.adminRole = true;
                                staff.moRole = false;
                                staff.receptionistRole = false;
                            }
                            else if (CheckIsStaffMO(staff.sIcNo).Equals(true))
                            {
                                staff.adminRole = false;
                                staff.moRole = true;
                                staff.receptionistRole = false;
                            }
                            else if (CheckIsStaffReceptionist(staff.sIcNo).Equals(true))
                            {
                                staff.adminRole = false;
                                staff.moRole = false;
                                staff.receptionistRole = true;
                            }
                            else
                            {
                                staff.adminRole = false;
                                staff.moRole = false;
                                staff.receptionistRole = false;
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
            return staff;
        }

        /**
         * Get user info from DB as a ModelStaff object
         * Type: ModelStaff
         */
        public static ModelStaff GetStaffInfoByIcNo(string icNo)
        {
            //- Object Staff model
            ModelStaff staff = new ModelStaff();

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM pku_staff WHERE s_ic_no = '" + icNo + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();

                            //- Parse Date value from SQL to DateTime obj
                            DateTime.TryParse(sdr["s_dob"].ToString(), out DateTime dob);

                            //- Manually calculate age from the retrieved DOB value rather than retrieving the Age value from DB
                            int age = DateTime.Now.AddYears(-dob.Year).Year;

                            staff.sIcNo = sdr["s_ic_no"].ToString();
                            staff.sPassportNo = sdr["s_passport_no"].ToString();
                            staff.sAccountNo = sdr["s_account_no"].ToString();
                            staff.sUsername = sdr["s_username"].ToString();
                            staff.sPasswordEncrypted = sdr["s_password"].ToString();
                            staff.sPasswordSalt = sdr["s_password_salt"].ToString();
                            staff.sStaffId = sdr["s_staff_id"].ToString();
                            staff.sTelNo = sdr["s_tel_no"].ToString();
                            staff.sEmail = sdr["s_email"].ToString();
                            staff.sName = sdr["s_name"].ToString();
                            staff.sDob = dob.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            staff.sAge = age;
                            staff.sGender = sdr["s_gender"].ToString();
                            staff.sMaritalStat = sdr["s_marital_stat"].ToString();
                            staff.sReligion = sdr["s_religion"].ToString();
                            staff.sRace = sdr["s_race"].ToString();
                            staff.sNationality = sdr["s_nationality"].ToString();
                            staff.sAddress = sdr["s_address"].ToString();
                            staff.sDesignation = sdr["s_designation"].ToString();
                            staff.sDepartment = sdr["s_department"].ToString();
                            staff.sSession = sdr["s_session"].ToString();
                            staff.sCategory = sdr["s_category"].ToString();
                            staff.sBranch = sdr["s_branch"].ToString();

                            if (CheckIsStaffAdmin(staff.sIcNo).Equals(true))
                            {
                                staff.adminRole = true;
                                staff.moRole = false;
                                staff.receptionistRole = false;
                            }
                            else if (CheckIsStaffMO(staff.sIcNo).Equals(true))
                            {
                                staff.adminRole = false;
                                staff.moRole = true;
                                staff.receptionistRole = false;
                            }
                            else if (CheckIsStaffReceptionist(staff.sIcNo).Equals(true))
                            {
                                staff.adminRole = false;
                                staff.moRole = false;
                                staff.receptionistRole = true;
                            }
                            else
                            {
                                staff.adminRole = false;
                                staff.moRole = false;
                                staff.receptionistRole = false;
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
            return staff;
        }
    }
}