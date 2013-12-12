using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarsBL.Dealer;
using System.Data;

public partial class Dealer_Usercontrol_Header : System.Web.UI.UserControl
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
               
                DealerActions objdealeract = new DealerActions();

                DataSet dsUserInfoDetails = objdealeract.DealerDefaultSellerInfo(Session[Constants.DealerCode].ToString(), Convert.ToInt32(Session[Constants.USER_ID].ToString()));

                //DataSet dsUserInfoDetails = objdropdownBL.USP_GetUSerDetailsByUserID(UID);

                Session["getRegUserdata"] = dsUserInfoDetails;

                if (dsUserInfoDetails.Tables[1].Rows.Count>0)
                {
                    if (dsUserInfoDetails.Tables[1].Rows[0][0].ToString() == "")
                    {
                        btnManageAccount.Enabled = false;
                    }                
                }


                DataSet dsDelaerLogo = objdealeract.GetDealerLogo(dsUserInfoDetails.Tables[0].Rows[0]["sellerID"].ToString());

                if (dsDelaerLogo.Tables[0].Rows.Count > 0)
                {
                    lblImage.ImageUrl = "../" + dsDelaerLogo.Tables[0].Rows[0]["LogoPath"].ToString();
                    imgLogo.ImageUrl = "../" + dsDelaerLogo.Tables[0].Rows[0]["LogoPath"].ToString();
                }

                lblDealerCode.Text = Session[Constants.DealerCode].ToString();
                lblDealerID1.Text = GeneralFunc.ToProper(Session[Constants.NAME].ToString());
                lblDealerID.Text = Session[Constants.BusinessName].ToString();
                dealerLogo.Text = Session[Constants.BusinessName].ToString().ToUpper();


            }

        }
    }
    protected void BtnSignout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("../login.aspx");
    }
    protected void btnManageAccount_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageAccount.aspx");
    }
}
