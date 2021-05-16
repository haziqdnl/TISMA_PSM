using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

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
            //- TISMADB
            DataTable dt1 = new DataTable();
            string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con1 = new SqlConnection(constr1);
            SqlDataAdapter sda1 = new SqlDataAdapter();
            SqlCommand cmd1 = new SqlCommand("GetTISMAPatient")
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd1.Parameters.AddWithValue("@Filter", ViewState["Filter"].ToString());
            cmd1.Connection = con1;
            sda1.SelectCommand = cmd1;
            sda1.Fill(dt1);
            DisplayRegisteredData.DataSource = dt1;
            DisplayRegisteredData.DataBind();

            //- UTMACAD
            string constr2 = ConfigurationManager.ConnectionStrings["utmacadConnectionString"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr2))
            {
                using (SqlDataAdapter sda2 = new SqlDataAdapter("SELECT * FROM utm_acad_tbl", con2))
                {
                    using (DataTable dt2 = new DataTable())
                    {
                        sda2.Fill(dt2);
                        DisplayUTMACADData.DataSource = dt2;
                        DisplayUTMACADData.DataBind();
                    }
                }
            }

            //- UTMHR
            string constr3 = ConfigurationManager.ConnectionStrings["utmhrConnectionString"].ConnectionString;
            using (SqlConnection con3 = new SqlConnection(constr3))
            {
                using (SqlDataAdapter sda3 = new SqlDataAdapter("SELECT * FROM utm_hr_tbl", con3))
                {
                    using (DataTable dt3 = new DataTable())
                    {
                        sda3.Fill(dt3);
                        DisplayUTMHRData.DataSource = dt3;
                        DisplayUTMHRData.DataBind();
                    }
                }
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