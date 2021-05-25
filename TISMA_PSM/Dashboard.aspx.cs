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

namespace TISMA_PSM
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CalcRegisteredPatient();
        }

        public void CalcRegisteredPatient()
        {
            //- Get today's date
            DateTime today = DateTime.Now;

            //- Local attribute
            int registered, waiting;

            //- DB Exception-Error handling
            try
            {
                //- Calculate Query
                string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    //- Calc total registered patient
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM qms WHERE date_generated = '"+today.ToString("yyyMMdd")+"'", con))
                    {
                        con.Open();
                        registered = (int)cmd.ExecuteScalar();
                        con.Close();
                    }
                    //- Calc total waiting patient
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM qms WHERE date_generated = '"+today.ToString("yyyMMdd")+ "' " +
                                                            "AND is_keyed_in = '"+1+"' AND is_checked_in = '"+0+"' AND is_expired = '"+0+"'", con))
                    {
                        con.Open();
                        waiting = (int)cmd.ExecuteScalar();
                        con.Close();
                    }
                }
                getPatientRegistered.Text = registered.ToString();
                getPatientWaiting.Text = waiting.ToString();
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



/*
 int result = DateTime.Compare(DateTime.Now, dateExpired);
            if (result < 0)
                Debug.WriteLine("Not Expired");
            else if (result == 0)
                Debug.WriteLine("Expired");
            else
                Debug.WriteLine("Expired");
 */

/*
         //- DB Exception-Error handling
            try
            {

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
         */