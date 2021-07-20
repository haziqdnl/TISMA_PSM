using System;

namespace TISMA_PSM
{
    public partial class InternalServerError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Check user session and get user info
            if (Convert.ToString(Session["UserGranted"]).Equals("true"))
            {
                btnBackToHome.Visible = true;
                btnBackToLogin.Visible = false;
            }
        }
    }
}