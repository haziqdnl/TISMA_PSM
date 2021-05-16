﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace TISMA_PSM
{
    public partial class Registration_New_Public : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                getNationDdl.DataSource = CountryList();
                getNationDdl.DataBind();
                getNationDdl.Items.Insert(0, "-Select-");

                getAccNo.Text = GenerateAccNo();
            }

        }

        public static List<string> CountryList()
        {
            //Creating List
            List<string> CultureList = new List<string>();

            //getting the specific CultureInfo from CultureInfo class
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo getCulture in getCultureInfo)
            {
                //creating the object of RegionInfo class
                RegionInfo GetRegionInfo = new RegionInfo(getCulture.LCID);
                //adding each country Name into the arraylist
                if (!(CultureList.Contains(GetRegionInfo.EnglishName)))
                {
                    CultureList.Add(GetRegionInfo.EnglishName);
                }
            }

            //sorting array by using sort method to get countries in order
            CultureList.Sort();

            return CultureList;
        }

        public static String GenerateAccNo()
        {
            //- Step 1: Generate Date pattern
            String today = DateTime.Now.ToString("yyyy/MM/dd");
            today = today.Remove(7, 1); // yyyy/MM_dd
            today = today.Remove(4, 1); // yyyy_MMdd

            //- Step 2: Append status type to Date pattern as accNo
            String accNo;
            accNo = today + 'P'; // P - Public

            //- Step 3: Append unique no.
            //-- Step 3.1: Generate random no. between 0 to 999
            Random rand = new Random();
            int num = rand.Next(0, 999);

            //-- Step 3.2: Identify and parse random no. pattern as string, then append to accNo
            if (num <= 9)
                accNo = accNo + "00" + num.ToString();
            else if (num >= 10 && num <= 99)
                accNo = accNo + "0" + num.ToString();
            else
                accNo += num.ToString();

            //-- Step 3.3: Identify the generated acc no. if it already existed in db
            if (CheckAccNotExist(accNo).Equals(true))
            {
                Debug.WriteLine("The generated Acc No. not existed yet.");
                return accNo;
            }
            else
            {
                Debug.WriteLine("The generated Acc No. already existed. Re-generating....");
                return GenerateAccNo();
            }
        }

        public static bool CheckAccNotExist(String accNo)
        {
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("CheckAccNotExist", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@AccNo", accNo.Trim());
            con.Open();
            bool status = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();

            return status;
        }

        protected void AddToTisma(object sender, EventArgs e)
        {
            //- Calculate age
            DateTime dob = Convert.ToDateTime(getDob.Text);
            int age = DateTime.Now.AddYears(-dob.Year).Year;

            //- Calculate session
            String session = DateTime.Now.AddYears(-1).ToString("yyyy") + "/" + DateTime.Now.ToString("yyyy");
            
            //- Insert data
            string constr = ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("AddToTismaPublic", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            //- Insert to table 'patient'
            cmd.Parameters.AddWithValue("@pIcNo", getIcNo.Text);
            cmd.Parameters.AddWithValue("@pAccNo", GenerateAccNo());
            cmd.Parameters.AddWithValue("@pPassport", getPassportNo.Text);
            cmd.Parameters.AddWithValue("@pTelNo", getPhone.Text);
            cmd.Parameters.AddWithValue("@pEmail", getEmail.Text);
            cmd.Parameters.AddWithValue("@pName", getName.Text);
            cmd.Parameters.AddWithValue("@Dob", dob);
            cmd.Parameters.AddWithValue("@pAge", age);
            cmd.Parameters.AddWithValue("@pGender", getGenderDdl.SelectedValue);
            cmd.Parameters.AddWithValue("@pMarital", getMaritalStatDdl.SelectedValue);
            cmd.Parameters.AddWithValue("@pReligion", getReligionDdl.SelectedValue);
            cmd.Parameters.AddWithValue("@pRace", getRaceDdl.SelectedValue);
            cmd.Parameters.AddWithValue("@pNationality", getNationDdl.SelectedValue);
            cmd.Parameters.AddWithValue("@pAddress", getAddress.Text);
            cmd.Parameters.AddWithValue("@pDesignation", getDesignation.Text);
            cmd.Parameters.AddWithValue("@pCategory", getCategory.Text);
            cmd.Parameters.AddWithValue("@pSession", session);
            cmd.Parameters.AddWithValue("@pBranch", getBranchDdl.SelectedValue);
            cmd.Parameters.AddWithValue("@pRemarks", getRemarks.Text);

            //- Insert to table 'patient_public'
            cmd.Parameters.AddWithValue("@pPublicStat", 1); // 1 - True

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            Response.Redirect("Registration.aspx");
        }
    }
}