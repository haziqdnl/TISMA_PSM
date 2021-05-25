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
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace TISMA_PSM
{
    public partial class QMS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }

            //- Get current date
            String today = DateTime.Now.ToString("yyyy/MM/dd");
            today = today.Remove(7, 1); // yyyy/MM_dd
            today = today.Remove(4, 1); // yyyy_MMdd
            getTodayDate.Text = today;
        }

        private void BindGrid()
        {
            //- Get today's date
            DateTime today = DateTime.Now;

            //- DB Exception-Error handling
            try
            {
                string query = "SELECT qms.queue_no, patient.p_name, qms.fk_ic_no, patient.p_category, patient.p_account_no, patient.p_remarks " +
                               "FROM qms LEFT OUTER JOIN patient " +
                               "ON qms.fk_ic_no = patient.p_ic_no " +
                               "WHERE qms.is_keyed_in = '"+1+"' AND qms.is_expired = '"+0+"' AND qms.is_checked_in = '"+0+"' AND qms.date_generated = '"+today.ToString("yyyyMMdd")+"'"; 

                //- Retrieve Query: TISMADB
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.HasRows)
                    {
                        //- If records available
                        QmsTable.DataSource = sdr;
                        QmsTable.DataBind();
                    }
                    else
                    {
                        //- If no records found
                        DataTable dt = new DataTable();
                        QmsTable.DataSource = dt;
                        QmsTable.DataBind();
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
                Debug.WriteLine("Database execution successful");
            }

            //- Datatable render
            QmsTable.UseAccessibleHeader = true;
            QmsTable.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void KeyInQueue(object sender, EventArgs e)
        {
            //- Get today's date
            DateTime today = DateTime.Now;

            //- Condition check
            if (CheckIsQueueNotExist(getQueueNo.Text, today).Equals(true))
            {
                getPopupTitle.Text = "Queue Number Not Exist or Expired";
                getPopupMessage.Text = "Unfortunately, the <b>queue number</b> you entered is <b>not exist or already expired</b>. Please refer to the Receptionist at the reception counter";
                ModalPopupMessage.Show();
            }
            else if (CheckIsQueueCheckedIn(getQueueNo.Text, today).Equals(true))
            {
                getPopupTitle.Text = "Queue Number Has Checked-In";
                getPopupMessage.Text = "Unfortunately, the <b>patient with this queue number has checked-in</b>. Please refer to the Receptionist at the reception counter";
                ModalPopupMessage.Show();
            }
            else if (CheckIsQueueExpired(getQueueNo.Text, today).Equals(true))
            {
                getPopupTitle.Text = "Queue Number Already Expired";
                getPopupMessage.Text = "Unfortunately, the <b>queue number</b> you entered already <b>expired</b>. Please refer to the Receptionist at the reception counter";
                ModalPopupMessage.Show();
            }
            else
            {
                //- DB Exception-Error handling
                try
                {
                    //- Update Query
                    string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        //- Table 'patient'
                        using (SqlCommand cmd = new SqlCommand("UPDATE qms SET is_keyed_in = '1' WHERE queue_no = '"+getQueueNo.Text+"' AND date_generated = '"+today.ToString("yyyyMMdd")+"'", con))
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            con.Dispose();
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
                    Debug.WriteLine("Database execution successful");
                }
                Server.TransferRequest(Request.Url.AbsolutePath, false);
            }
        }

        public static bool CheckIsQueueCheckedIn(string queueNo, DateTime dateGenerated)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckIsQueueCheckedIn", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@QueueNo", queueNo);
            cmd.Parameters.AddWithValue("@DateGenerated", dateGenerated);
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static bool CheckIsQueueExpired(string queueNo, DateTime dateGenerated)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckIsQueueExpired", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@QueueNo", queueNo);
            cmd.Parameters.AddWithValue("@DateGenerated", dateGenerated);
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        public static bool CheckIsQueueNotExist(string queueNo, DateTime dateGenerated)
        {
            //- Search Query
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckIsQueueNotExist", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@QueueNo", queueNo);
            cmd.Parameters.AddWithValue("@DateGenerated", dateGenerated);
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
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