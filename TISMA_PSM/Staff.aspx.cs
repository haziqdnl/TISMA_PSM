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
    public partial class Staff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BindGrid();
            }
        }

        private void BindGrid()
        {
            string constr1 = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(constr1))
            {
                using (SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM pku_staff ORDER BY s_name", con1))
                {
                    using (DataTable dt1 = new DataTable())
                    {
                        sda1.Fill(dt1);
                        DisplayRegisteredData.DataSource = dt1;
                        DisplayRegisteredData.DataBind();
                    }
                }
            }

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

            //Required for jQuery DataTables to work.
            DisplayRegisteredData.UseAccessibleHeader = true;
            DisplayRegisteredData.HeaderRow.TableSection = TableRowSection.TableHeader;
            DisplayUTMHRData.UseAccessibleHeader = true;
            DisplayUTMHRData.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}