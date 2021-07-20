using System;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    /**
     * This is a Controller Class for QMS
     * 
     * All SQL and public methods related only to QMS model 
     * that are used repeatedly are implemented in this Class.
     * 
     */
    public class ControllerQMS
    {
        /**
         * Validate if Queue No. has checked-in
         * Type: bool 
         */
        public static bool CheckIsQueueCheckedIn(string queueNo, DateTime dateGenerated)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckIsQueueCheckedIn", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@QueueNo", queueNo);
                        cmd.Parameters.AddWithValue("@DateGenerated", dateGenerated);
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
         * Validate if Queue No. has expired
         * Type: bool 
         */
        public static bool CheckIsQueueExpired(string queueNo, DateTime dateGenerated)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckIsQueueExpired", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@QueueNo", queueNo);
                        cmd.Parameters.AddWithValue("@DateGenerated", dateGenerated);
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
         * Validate if Queue No. not exist
         * Type: bool 
         */
        public static bool CheckIsQueueNotExist(string queueStr, DateTime dateGenerated)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckIsQueueNotExist", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@QueueNo", queueStr);
                        cmd.Parameters.AddWithValue("@DateGenerated", dateGenerated);
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
         * Validate if this patient owns this queue no.
         * Type: bool 
         */
        public static bool CheckIsPatientOwnsQueue(string icNo, string queueNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT fk_ic_no, queue_no FROM qms WHERE fk_ic_no = '" + icNo + "' AND queue_no = '" + queueNo + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
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
         * To convert queue no to proper format
         * Type: string
         */
        public static string ConvertQueueNo(int queueNo)
        {
            string queueStr;
            if (queueNo <= 9)
                queueStr = "00" + queueNo.ToString();
            else if (queueNo >= 10 && queueNo <= 99)
                queueStr = "0" + queueNo.ToString();
            else
                queueStr = queueNo.ToString();
            return queueStr;
        }
    }
}