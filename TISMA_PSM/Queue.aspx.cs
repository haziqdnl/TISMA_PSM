using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Queue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
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
                               "WHERE qms.is_keyed_in = '1' AND qms.is_expired = '0' AND qms.is_checked_in = '0' AND qms.is_checked_out = '0' AND qms.date_generated = '" + today.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "'";

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
                                QueueListTable.DataSource = sdr;
                                QueueListTable.DataBind();
                            }
                            else
                            {
                                //- If no records found
                                DataTable dt = new DataTable();
                                QueueListTable.DataSource = dt;
                                QueueListTable.DataBind();
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

            //- DataTable render
            QueueListTable.UseAccessibleHeader = true;
            QueueListTable.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}