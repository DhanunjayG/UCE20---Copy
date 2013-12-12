using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dealer_Details : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (!IsPostBack)
        {
            if (Session[Constants.USER_ID] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            else
            {
                //    ;

                //    Session[Constants.USER_NAME] ;

                //    Session[Constants.NAME];
         

                ServiceReference objServiceReference = new ServiceReference();

                ScriptReference objScriptReference = new ScriptReference();

                objServiceReference.Path = "~/CarsService.asmx";

                objScriptReference.Path = "~/Static/Js/CarsJScriptNew.js";

                scrptmgr.Services.Add(objServiceReference);
                scrptmgr.Scripts.Add(objScriptReference);

            }

        }
    }
}
