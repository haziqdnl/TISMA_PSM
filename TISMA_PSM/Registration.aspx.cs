using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Filter"] = "All";
                BindGrid();
            }
        }

        private void BindGrid()
        {
            //- To create datatable for TISMADB
            DataTable dt1 = new DataTable();

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query: TISMADB
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        using (SqlCommand cmd = new SqlCommand("GetTISMAPatient", con) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.Parameters.AddWithValue("@Filter", ViewState["Filter"].ToString());
                            sda.SelectCommand = cmd;
                        }
                        sda.Fill(dt1);

                        //- DataTable binding
                        DisplayRegisteredData.DataSource = dt1;
                        DisplayRegisteredData.DataBind();
                    }
                }
                using (SqlConnection con = new SqlConnection(GetConnectionStringUtmAcad()))
                {
                    con.Open();
                    //- Get Query: UTMACAD
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM utm_acad_tbl", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                //- If records available
                                DisplayUTMACADData.DataSource = sdr;
                                DisplayUTMACADData.DataBind();
                            }
                            else
                            {
                                //- If no records found
                                DataTable dt = new DataTable();
                                DisplayUTMACADData.DataSource = dt;
                                DisplayUTMACADData.DataBind();
                            }
                        }
                    }
                }
                using (SqlConnection con = new SqlConnection(GetConnectionStringUtmHr()))
                {
                    con.Open();
                    //- Get Query: UTMHR
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM utm_hr_tbl", con))
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
            DisplayRegisteredData.UseAccessibleHeader = true;
            DisplayRegisteredData.HeaderRow.TableSection = TableRowSection.TableHeader;
            DisplayUTMACADData.UseAccessibleHeader = true;
            DisplayUTMACADData.HeaderRow.TableSection = TableRowSection.TableHeader;
            DisplayUTMHRData.UseAccessibleHeader = true;
            DisplayUTMHRData.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void CategoryChanged(object sender, EventArgs e)
        {
            DropDownList DdlPatientType = (DropDownList)sender;
            ViewState["Filter"] = DdlPatientType.SelectedValue;
            this.BindGrid();
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            DisplayRegisteredData.PageIndex = e.NewPageIndex;
            this.BindGrid();
        }        
    }
}