using System;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    /**
     * This is a Controller Class for Clinical Info
     * 
     * All SQL and public methods related only to Clinical Info model 
     * that are used repeatedly are implemented in this Class.
     * 
     */
    public class ControllerClinicalInfo
    {
        /**
         * To add new Diagnosis item into DDL
         * Type: void
         */
        public static void AddDiagnosis(string diagnosis)
        {
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Insert Query
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO diagnosisDb (diagnosis_name) VALUES (@DiagnosisName)", con))
                    {
                        cmd.Parameters.Add("@DiagnosisName", SqlDbType.NVarChar, 50).Value = diagnosis;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }

        /**
         * To add new Sign item into DDL
         * Type: void
         */
        public static void AddSign(string sign)
        {
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Insert Query
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO signDb (sign_name) VALUES (@SignName)", con))
                    {
                        cmd.Parameters.Add("@SignName", SqlDbType.NVarChar, 50).Value = sign;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }

        /**
         * To add new Symptom item into DDL
         * Type: void
         */
        public static void AddSymptom(string symptom)
        {
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Insert Query
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO symptomDb (symptom_name) VALUES (@SymptomName)", con))
                    {
                        cmd.Parameters.Add("@SymptomName", SqlDbType.NVarChar, 50).Value = symptom;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }

        /**
         * Validate if Clinical has submitted
         * Type: bool 
         */
        public static bool CheckClinicalSubmitted(string icNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckClinicalSubmitted", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@IcNo", icNo.Trim());
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
         * Validate if e-MC has generated
         * Type: bool 
         */
        public static bool CheckIsItemClinicalAdded(string clinicalSign, string item)
        {
            string query = "";
            if (clinicalSign.Equals("Diagnosis"))
                query = "CheckIsItemDiagnosisAdded";
            else if (clinicalSign.Equals("Sign"))
                query = "CheckIsItemSignAdded";
            else if (clinicalSign.Equals("Symptom"))
                query = "CheckIsItemSymptomAdded";

            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand(query, con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@Item", item.Trim());
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
         * Get Clinical Info from DB as a ModelClinicalInfo object
         * Type: ModelClinicalInfo
         */
        public static ModelClinicalInfo GetClinicalInfoForTodayByIcNo(string icNo)
        {
            //- Object Clinical Info model
            ModelClinicalInfo clinical = new ModelClinicalInfo();

            //- DB Exception-Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM clinical_info WHERE fk_p_ic_no = '" + icNo + "' AND CONVERT(DATE, clinical_date) = CONVERT(DATE, GETDATE()) ORDER BY clinical_id DESC", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();
                            clinical.clinicalId = int.Parse(sdr["clinical_id"].ToString());
                            clinical.symptom = sdr["symptom"].ToString();
                            clinical.symptomDesc = sdr["symptom_desc"].ToString();
                            clinical.sign = sdr["ill_sign"].ToString();
                            clinical.signDesc = sdr["ill_sign_desc"].ToString();
                            clinical.diagnosis = sdr["diagnosis"].ToString();
                            clinical.diagnosisDesc = sdr["diagnosis_desc"].ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return clinical;
        }
    }
}