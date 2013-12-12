using System;
using System.Collections;
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

public partial class Mobile_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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

            Session["CurrentPageConfig"] = null;


            ServiceReference objServiceReference = new ServiceReference();

            ScriptReference objScriptReference = new ScriptReference();

            objServiceReference.Path = "~/CarsService.asmx";

            objScriptReference.Path = "~/Static/Js/CarsJScriptNew.js";

            scrptmgr.Services.Add(objServiceReference);
            scrptmgr.Scripts.Add(objScriptReference);





        }

    }
}
