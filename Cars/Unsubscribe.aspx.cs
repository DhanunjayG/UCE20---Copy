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
using CarsBL.Transactions;
using CarsInfo;
using System.Net.Mail;
using CarsBL.Masters;

using System.Collections.Generic;

public partial class Unsubscribe : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["PageName"] = "Unsubscribe";

            ServiceReference objServiceReference = new ServiceReference();

            ScriptReference objScriptReference = new ScriptReference();

            objServiceReference.Path = "~/CarsService.asmx";

            objScriptReference.Path = "~/Static/Js/CarsJScriptNew.js";

            scrptmgr.Services.Add(objServiceReference);
            scrptmgr.Scripts.Add(objScriptReference);


            if (Request.QueryString.Keys.Count > 0)
            {
                string URL = string.Empty;
                string URL1 = string.Empty;
                if (Request.QueryString.Keys.Count > 0)
                {
                    if (URL != null)
                    {
                        PreferencesBL objPreferencesBL = new PreferencesBL();

                        DataSet dsPreferences = new DataSet();

                        dsPreferences = objPreferencesBL.GetEmailPreferences(Request.QueryString["PreferID"].ToString());
                        
                        if (dsPreferences.Tables[0].Rows.Count > 0)
                        {
                            if (dsPreferences.Tables[0].Rows[0]["IsActive"].ToString() == "True")
                            {
                                objPreferencesBL.SubScribeStatus(Request.QueryString["PreferID"].ToString(), 0);
                                
                                lblalert.Text = "Email Alerts has Unsubscribed Successfully..."; 
                            }
                            else
                            {
                                lblalert.Text = "Email Alerts has Already Unsubscribed "; 

                                //mpealteruser.Show(); 
                            }
                        }

                        Type cstype = GetType();
                        ClientScriptManager cs = Page.ClientScript;
                        cs.RegisterStartupScript(cstype, "KyRST", "<script type='text/javascript' language='javascript'>pageLoad();</script>");

                    }
                }
                
            }

        }
    }
    protected void btnSubscribe_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx"); 
    }
    protected void btnSubscribeCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmailPreferences.aspx"); 
    }
}
