using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Get number of online MO
            getDoctorAvailable.Text = Convert.ToString(Application["MOSessionCount"]);

            //- Get today's date and day
            DateTime today = DateTime.Now;
            getTodayDate.Text = today.ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            getTodayDayName.Text = today.ToString("dddd", System.Globalization.CultureInfo.InvariantCulture);

            if (!IsPostBack)
                CalculateData();
        }

        private void CalculateData()
        {
            //- Get today's date
            DateTime today = DateTime.Now;

            //- Local attribute
            int registered, waiting, inroom, checkedOut, emcTotal;

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Calc total registered patient
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM qms WHERE date_generated = '" + today.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "'", con))
                    {
                        registered = (int)cmd.ExecuteScalar();
                    }
                    //- Calc total registered patient
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM emc WHERE CONVERT(DATE, date_created) = convert(date, GETDATE())", con))
                    {
                        emcTotal = (int)cmd.ExecuteScalar();
                    }
                    //- Calc total waiting patient
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM qms WHERE date_generated = '" + today.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "' " +
                                                            "AND is_keyed_in = '1' AND is_checked_in = '0' AND is_checked_out = '0' AND is_expired = '0'", con))
                    {
                        waiting = (int)cmd.ExecuteScalar();
                    }
                    //- Calc total in-room patient
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM qms WHERE date_generated = '" + today.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "' " +
                                                            "AND is_keyed_in = '1' AND is_checked_in = '1' AND is_checked_out = '0' AND is_expired = '0'", con))
                    {
                        inroom = (int)cmd.ExecuteScalar();
                    }
                    //- Calc total checked-out patient
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM qms WHERE date_generated = '" + today.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "' " +
                                                            "AND is_keyed_in = '1' AND is_checked_in = '1' AND is_checked_out = '1' AND is_expired = '0'", con))
                    {
                        checkedOut = (int)cmd.ExecuteScalar();
                    }
                }

                //- Create chart based on the calculated data
                CreateChart(registered, waiting, inroom, checkedOut);

                //- Parse calculated data
                getPatientRegistered.Text = registered.ToString();
                getPatientWaiting.Text = waiting.ToString();
                getPatientCheckedIn.Text = inroom.ToString();
                getPatientCheckedOut.Text = checkedOut.ToString();
                getEMCGenerate.Text = emcTotal.ToString();

            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }

        private void CreateChart(int registered, int waiting, int inRoom, int checkedOut)
        {
            string[] xVal = { "Registered", "Waiting", "In Room", "Checked-Out" };
            int[] yVal = { registered, waiting, inRoom, checkedOut };

            //- Set custom chart title
            Title title = new Title
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Graphical Summary"
            };
            Chart1.Titles.Add(title);

            //- Series setup
            Chart1.Series["Series"].Points.DataBindXY(xVal, yVal);

            //- Set column color
            Chart1.Series["Series"].Points[1].Color = System.Drawing.Color.FromArgb(194, 55, 55);
            Chart1.Series["Series"].Points[0].Color = System.Drawing.Color.FromArgb(254, 140, 0);
            Chart1.Series["Series"].Points[2].Color = System.Drawing.Color.FromArgb(247, 177, 91);
            Chart1.Series["Series"].Points[3].Color = System.Drawing.Color.FromArgb(196, 99, 99);

            //- Hide chart gridline
            Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
        }
    }
}