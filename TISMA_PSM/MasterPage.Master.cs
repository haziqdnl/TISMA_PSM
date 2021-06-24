using System;
using System.Web;

namespace TISMA_PSM
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //- Check user session and retrieve user info
            if (!Convert.ToString(Session["UserGranted"]).Equals("true"))
                Response.Redirect("Login.aspx");

            //-- Ger user name
            string fullname = Convert.ToString(Session["UserName"]);
            var names = fullname.Split(' ');
            if (names.Length == 1)
                getUserName.Text = names[0];
            else if (names.Length == 2)
                getUserName.Text = names[0] + " " + names[1];
            else
                getUserName.Text = names[0] + " " + names[1] + " " + names[2];

            //-- Get user role
            getUserRole.Text = Convert.ToString(Session["UserRole"]);

            //-- User role ACL
            if (Convert.ToString(Session["UserRole"]).Equals("Receptionist"))
            {
                StaffLink.Visible = false;
                OPDLink.Visible = false;
            }
            if (Convert.ToString(Session["UserRole"]).Equals("Medical Officer"))
                StaffLink.Visible = false;
        }

        protected void Logout(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["UserRole"]).Equals("Medical Officer"))
            {
                Application.Lock();
                int applicationCount = Convert.ToInt32(Application["MOSessionCount"]);
                applicationCount--;
                Application["MOSessionCount"] = applicationCount;
                Application.UnLock();
            }
            if (Session["UserGranted"] != null)
            {
                Session.Clear();//clear session
                Session.Abandon();//Abandon session
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
            }
            Response.Redirect("Login.aspx");
        }
    }
}