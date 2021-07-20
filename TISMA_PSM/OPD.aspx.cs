using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class OPD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Verify user ACL based on session: 404 error
            if (!Convert.ToString(Session["UserRole"]).Equals("Admin") && !Convert.ToString(Session["UserRole"]).Equals("Medical Officer"))
                Response.Redirect("PageNotFound.aspx");

            if (!IsPostBack)
                BindGrid();
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
                               "WHERE qms.is_keyed_in = '1' AND qms.is_expired = '0' AND qms.is_checked_in = '0' AND qms.is_checked_out = '0' AND qms.date_generated = '" + today.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "' " +
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
                                OpdTable.DataSource = sdr;
                                OpdTable.DataBind();
                            }
                            else
                            {
                                //- If no records found
                                DataTable dt = new DataTable();
                                OpdTable.DataSource = dt;
                                OpdTable.DataBind();
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
            OpdTable.UseAccessibleHeader = true;
            OpdTable.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void CheckInPatient(object sender, EventArgs e)
        {
            string encryptedAccNo = EncryptURL(getAccNo.Text);

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Update Query
                    using (SqlCommand cmd = new SqlCommand("UPDATE qms SET is_checked_in = '1' WHERE queue_no = '" + getQueueNo.Text + "' AND date_generated = '" + DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "'", con))
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
            Response.Redirect("Consultation.aspx?accno=" + encryptedAccNo + "&queue=" + getQueueNo.Text);
        }

        protected void ShowModalPopupConfirmation(object sender, EventArgs e)
        {
            //string[] arg = new string[2];
            Button btn = (Button)sender;
            string[] arg = btn.CommandArgument.ToString().Split(';');
            getAccNo.Text = arg[0];
            getQueueNo.Text = arg[1];
            ModalPopupConfirmation.Show();
            BindGrid();
        }
    }
}