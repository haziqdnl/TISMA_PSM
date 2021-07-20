using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.ControllerQMS;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class QMS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindGrid();

            //- Get and display current date
            string today = DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            today = today.Remove(7, 1); // yyyy/MM_dd
            today = today.Remove(4, 1); // yyyy_MMdd
            getTodayDate.Text = today;
        }

        private void BindGrid()
        {
            //- Get today's date
            DateTime today = DateTime.Now;

            //- DB Exception/Error handling
            try
            {
                string query = "SELECT qms.queue_no, patient.p_name, qms.fk_ic_no, patient.p_category, patient.p_account_no, patient.p_remarks " +
                               "FROM qms LEFT OUTER JOIN patient " +
                               "ON qms.fk_ic_no = patient.p_ic_no " +
                               "WHERE qms.is_keyed_in = '1' AND qms.is_expired = '0' AND qms.is_checked_in = '0' AND qms.is_checked_out = '0' AND qms.date_generated = '" + today.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) +"' " +
                               "ORDER BY qms.queue_no"; 

                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
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
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }

            //- Datatable render
            QmsTable.UseAccessibleHeader = true;
            QmsTable.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void KeyInQueue(object sender, EventArgs e)
        {
            String queue = getQueueNo.Text;
            if (queue.Length == 1)
                queue = "00" + queue;
            else if (queue.Length == 2)
                queue = "0" + queue;

            //- Get today's date
            DateTime today = DateTime.Now;

            //- Condition check
            if (CheckIsQueueNotExist(queue, today).Equals(true))
            {
                getPopupTitle.Text = "Queue Number Not Exist or Expired";
                getPopupMessage.Text = "Unfortunately, the <b>queue number</b> you entered is <b>expired or not exist</b>. Please refer to the Receptionist at the reception counter";
                BindGrid();
                ModalPopupMessage.Show();
            }
            else if (CheckIsQueueCheckedIn(queue, today).Equals(true))
            {
                getPopupTitle.Text = "Queue Number Has Checked-In";
                getPopupMessage.Text = "Unfortunately, the <b>patient with this queue number has checked-in</b>. Please refer to the Receptionist at the reception counter";
                BindGrid();
                ModalPopupMessage.Show();
            }
            else if (CheckIsQueueExpired(queue, today).Equals(true))
            {
                getPopupTitle.Text = "Queue Number Already Expired";
                getPopupMessage.Text = "Unfortunately, the <b>queue number</b> you entered already <b>expired</b>. Please refer to the Receptionist at the reception counter";
                BindGrid();
                ModalPopupMessage.Show();
            }
            else
            {
                //- DB Exception/Error handling
                try
                {
                    using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                    {
                        con.Open();
                        //- Update Query
                        using (SqlCommand cmd = new SqlCommand("UPDATE qms SET is_keyed_in = '1' WHERE queue_no = '" + queue + "' AND date_generated = '" + today.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "'", con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    //- Display handling-error message
                    SqlExceptionMsg(ex);
                }
                Response.Redirect("QMS.aspx");
            }
        }
    }
}