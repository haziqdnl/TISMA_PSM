using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using static TISMA_PSM.Helper;
using static TISMA_PSM.ControllerEMC;

namespace TISMA_PSM
{
    public partial class Statistic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
            CalcDataOverall();
            CalcDataClinicalDaily();
            CalcDataClinicalMonthly();
            CalcDataEmcDaily();
            CalcDataEmcMonthly();
        }

        private void BindGrid()
        {
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT serial_no, date_created, emc_period, date_from, date_to, url_hashed, emc_password FROM emc ORDER BY date_created", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                //- If records available
                                MCHistoryTable.DataSource = sdr;
                                MCHistoryTable.DataBind();
                            }
                            else
                            {
                                //- If no records found
                                DataTable dt = new DataTable();
                                MCHistoryTable.DataSource = dt;
                                MCHistoryTable.DataBind();
                            }
                        }
                    }
                    using (SqlCommand cmd = new SqlCommand("SELECT clinical_info.clinical_date, patient.p_name, clinical_info.symptom, clinical_info.ill_sign, clinical_info.diagnosis, pku_staff.s_name " +
                                                           "FROM clinical_info LEFT OUTER JOIN pku_staff ON pku_staff.s_ic_no = clinical_info.fk_s_ic_no LEFT OUTER JOIN patient ON patient.p_ic_no = clinical_info.fk_p_ic_no " +
                                                           "ORDER BY clinical_info.clinical_date", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                //- If records available
                                ClinicalHistoryTable.DataSource = sdr;
                                ClinicalHistoryTable.DataBind();
                            }
                            else
                            {
                                //- If no records found
                                DataTable dt = new DataTable();
                                ClinicalHistoryTable.DataSource = dt;
                                ClinicalHistoryTable.DataBind();
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

            //- Table render;
            ClinicalHistoryTable.UseAccessibleHeader = true;
            ClinicalHistoryTable.HeaderRow.TableSection = TableRowSection.TableHeader;
            MCHistoryTable.UseAccessibleHeader = true;
            MCHistoryTable.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void MCHistoryTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                string x = e.Row.Cells[7].Text;
                e.Row.Cells[7].Text = DecryptEMCPassword(x);
            }
        }

        private void CalcDataOverall()
        {
            double totalEmc = 0, totalClinical = 0, emcOverClinical;

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Calc total registered patient
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM emc WHERE YEAR(CONVERT(DATE, date_created)) = '" + DateTime.Now.Year.ToString() + "'", con))
                    {
                        totalEmc = (int)cmd.ExecuteScalar();
                    }
                    //- Calc total registered patient
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM clinical_info WHERE YEAR(CONVERT(DATE, clinical_date)) = '" + DateTime.Now.Year.ToString() + "'", con))
                    {
                        totalClinical = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }

            //- Calculate the percentage
            emcOverClinical = totalEmc / totalClinical * 100;

            getTotalEmc.Text = totalEmc.ToString();
            getReleasedPerVisit.Text = string.Format("{0:0.00}", emcOverClinical) + "%";
            getAveEmcPerDay.Text = string.Format("{0:0.00}", totalEmc / 365);
            getAveEmcPerMonth.Text = string.Format("{0:0.00}", totalEmc / 12);
            getTotalVisit.Text = totalClinical.ToString();
            getAveDailyVisit.Text = string.Format("{0:0.00}", totalClinical / 365);
            getAveMonthlyVisit.Text = string.Format("{0:0.00}", totalClinical / 12);
        }

        private void CalcDataClinicalDaily()
        {
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get all existing dates where there is 1 or more Clinical Info has been generated within the system's current year
                    List<string> dateList = new List<string>();
                    using (SqlCommand cmd = new SqlCommand("SELECT clinical_date FROM clinical_info WHERE YEAR(CONVERT(DATE, clinical_date)) = '" + DateTime.Now.Year.ToString() + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                while (sdr.Read())
                                {
                                    DateTime.TryParse(sdr["clinical_date"].ToString(), out DateTime dateCreated);
                                    dateList.Add(dateCreated.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
                                }
                            }
                        }
                    }
                    //- Calculate the total e-MC based on the retrieved dates
                    int[] totalClinicalPerDate = new int[dateList.Count];
                    int i = 0;
                    foreach (string date in dateList)
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM clinical_info WHERE CONVERT(DATE, clinical_date) = '" + date + "'", con))
                        {
                            totalClinicalPerDate[i] = (int)cmd.ExecuteScalar();
                        }
                        i++;
                    }
                    //- Create Chart
                    CreateChart3(dateList.ToArray(), totalClinicalPerDate);
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }

        private void CalcDataClinicalMonthly()
        {
            string[] month = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            int[] totalEmcPerMonth = new int[month.Length];

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Calculate the monthly data
                    for (int i = 0; i < month.Length; i++)
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM clinical_info WHERE MONTH(CONVERT(DATE, clinical_date)) = '" + (i + 1) + "'", con))
                        {
                            totalEmcPerMonth[i] = (int)cmd.ExecuteScalar();
                        }
                    }
                    //- Create Chart
                    CreateChart2(month, totalEmcPerMonth);
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }

        private void CalcDataEmcDaily()
        {
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get all existing dates where there is 1 or more e-MC has been generated within the system's current year
                    List<string> dateList = new List<string>();
                    using (SqlCommand cmd = new SqlCommand("SELECT date_created FROM emc WHERE YEAR(CONVERT(DATE, date_created)) = '" + DateTime.Now.Year.ToString() + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                while (sdr.Read())
                                {
                                    DateTime.TryParse(sdr["date_created"].ToString(), out DateTime dateCreated);
                                    dateList.Add(dateCreated.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
                                }
                            }
                        }
                    }
                    //- Calculate the total e-MC based on the retrieved dates
                    int[] totalEmcPerDate = new int[dateList.Count];
                    int i = 0;
                    foreach (string date in dateList)
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM emc WHERE CONVERT(DATE, date_created) = '" + date + "'", con))
                        {
                            totalEmcPerDate[i] = (int)cmd.ExecuteScalar();
                        }
                        i++;
                    }
                    //- Create Chart
                    CreateChart1(dateList.ToArray(), totalEmcPerDate);
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }

        private void CalcDataEmcMonthly()
        {
            string[] month = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            int[] totalEmcPerMonth = new int[month.Length];

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Calculate the monthly data
                    for (int i = 0; i < month.Length; i++)
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM emc WHERE MONTH(CONVERT(DATE, date_created)) = '" + ( i + 1 ) + "'", con))
                        {
                            totalEmcPerMonth[i] = (int)cmd.ExecuteScalar();
                        }
                    }
                    //- Create Chart
                    CreateChart4(month, totalEmcPerMonth);
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
        }

        private void CreateChart1(string[] x, int[] y)
        {
            //- Set custom chart title
            Title title = new Title
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Total e-MC Generated Daily ( in " + DateTime.Now.Year.ToString() + " )"
            };
            ChartEmcDaily.Titles.Add(title);

            //- Series setup
            ChartEmcDaily.Series["SeriesEmcDaily"].Points.DataBindXY(x, y);

            //- Hide chart gridline
            ChartEmcDaily.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            ChartEmcDaily.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
        }

        private void CreateChart2(string[] x, int[] y)
        {
            //- Set custom chart title
            Title title = new Title
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Total e-MC Generated Monthly ( in " + DateTime.Now.Year.ToString() + " )"
            };
            ChartEmcMonthly.Titles.Add(title);

            //- Series setup
            ChartEmcMonthly.Series["SeriesEmcMonthly"].Points.DataBindXY(x, y);

            //- Hide chart gridline
            ChartEmcMonthly.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            ChartEmcMonthly.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            //- Display all value in the x-axis
            ChartEmcMonthly.ChartAreas["ChartAreaEmcMonthly"].AxisX.Interval = 1;
        }

        private void CreateChart3(string[] x, int[] y)
        {
            //- Set custom chart title
            Title title = new Title
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Total Daily Visit ( in " + DateTime.Now.Year.ToString() + " )"
            };
            ChartClinicalDaily.Titles.Add(title);

            //- Series setup
            ChartClinicalDaily.Series["SeriesClinicalDaily"].Points.DataBindXY(x, y);

            //- Hide chart gridline
            ChartClinicalDaily.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            ChartClinicalDaily.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
        }

        private void CreateChart4(string[] x, int[] y)
        {
            //- Set custom chart title
            Title title = new Title
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = "Total Monthly Visit ( in " + DateTime.Now.Year.ToString() + " )"
            };
            ChartClinicalMonthly.Titles.Add(title);

            //- Series setup
            ChartClinicalMonthly.Series["SeriesClinicalMonthly"].Points.DataBindXY(x, y);

            //- Hide chart gridline
            ChartClinicalMonthly.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            ChartClinicalMonthly.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            //- Display all value in the x-axis
            ChartClinicalMonthly.ChartAreas["ChartAreaClinicalMonthly"].AxisX.Interval = 1;
        }
    }
}