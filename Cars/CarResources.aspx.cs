using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GeneralFunc.SaveSiteVisit();
            //if (Session[Constants.NAME] == null)
            //{
            //    lnkLogin.Visible = true;
            //    lnkBtnLogout.Visible = false;
            //    lblUserName.Visible = false;
            //    lblWelcome.Visible = false;
            //}
            //else
            //{
            //    lnkLogin.Visible = false;
            //    lnkBtnLogout.Visible = true;
            //    lblUserName.Visible = true;
            //    lblWelcome.Visible = true;
            //    lblUserName.Text = Session[Constants.NAME].ToString();
            //}
            Session["CurrentPage"] = "Home";

            KeyWords.Addkeywordstags(Header);

            

            Session["CurrentPageConfig"] = null;


            ServiceReference objServiceReference = new ServiceReference();

            ScriptReference objScriptReference = new ScriptReference();

            objServiceReference.Path = "~/CarsService.asmx";

            objScriptReference.Path = "~/Static/Js/CarsJScript.js";

            scrptmgr.Services.Add(objServiceReference);
            scrptmgr.Scripts.Add(objScriptReference);





        }

    }
    protected void lnkBtnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }
        catch (Exception ex)
        {
        }
    }
}
