using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Staff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Verify user ACL based on session: 404 error
            if (!Convert.ToString(Session["UserRole"]).Equals("Admin"))
                Response.Redirect("PageNotFound.aspx");

            if (!this.IsPostBack)
                this.BindGrid();
        }

        private void BindGrid()
        {
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query: TISMADB
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM pku_staff ORDER BY s_name", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                //- If records available
                                RegisteredStaffTable.DataSource = sdr;
                                RegisteredStaffTable.DataBind();
                            }
                            else
                            {
                                //- If no records found
                                DataTable dt = new DataTable();
                                RegisteredStaffTable.DataSource = dt;
                                RegisteredStaffTable.DataBind();
                            }
                        }
                    }
                }
                using (SqlConnection con = new SqlConnection(GetConnectionStringUtmHr()))
                {
                    con.Open();
                    //- Get Query: UTMHR
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM utm_hr_tbl ORDER BY name", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                //- If records available
                                DisplayUTMHRData.DataSource = sdr;
                                DisplayUTMHRData.DataBind();
                            }
                            else
                            {
                                //- If no records found
                                DataTable dt = new DataTable();
                                DisplayUTMHRData.DataSource = dt;
                                DisplayUTMHRData.DataBind();
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
            RegisteredStaffTable.UseAccessibleHeader = true;
            RegisteredStaffTable.HeaderRow.TableSection = TableRowSection.TableHeader;
            DisplayUTMHRData.UseAccessibleHeader = true;
            DisplayUTMHRData.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}