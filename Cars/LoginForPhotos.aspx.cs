
using System;
using System.Collections;
using System.Collections.Generic;
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
using CarsBL.CentralDBTransactions;

public partial class LoginForPhotos : System.Web.UI.Page
{
    SmartzUserRegBL objSmartzBL = new SmartzUserRegBL();
    SmartzUserRegInfo objSmartzInfo = new SmartzUserRegInfo();
    UserLogInfo UserLogInfo = new UserLogInfo();
    UserLogBL objUserLog = new UserLogBL();
    CentralDBMainBL objCentralDBBL = new CentralDBMainBL();

    public string strUserSessionID;

    public string strUserAuthCookieID;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["PhotoUpUserID"] = null;
            Session.Clear();
            Session.Abandon();
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtUserName.Focus();
            Session.Timeout = 180;
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsUserDetails = new DataSet();
            String strHostName = Request.UserHostAddress.ToString();
            string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            dsUserDetails = objCentralDBBL.PerformLogin(txtUserName.Text, txtPassword.Text, strIp);
            Session.Timeout = 180;
            if (dsUserDetails.Tables.Count > 0)
            {
                if (dsUserDetails.Tables[0].Rows.Count > 0)
                {
                    Session["PhotoUpUserID"] = dsUserDetails.Tables[0].Rows[0]["SmartzUID"].ToString();
                    Session["PhotoUpUserName"] = dsUserDetails.Tables[0].Rows[0]["SmartzUname"].ToString();
                    Session["PhotoUpAgentName"] = dsUserDetails.Tables[0].Rows[0]["SmartzFirstName"].ToString();
                    CreateUserLog(1);

                    Response.Redirect("CSRPhotoUpload.aspx?CID=0");
                }
                else
                {
                    Session["PhotoUpUserName"] = txtUserName.Text;
                    if (CreateUserLog(3))
                    {
                        lblError.Text = "Invalid username or password!";
                        lblError.Visible = true;
                        txtUserName.Text = "";
                        txtPassword.Text = "";
                    }
                }
            }
            else
            {
                Session["PhotoUpUserName"] = txtUserName.Text;
                if (CreateUserLog(3))
                {
                    lblError.Text = "Invalid username or password!";
                    lblError.Visible = true;
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    private Boolean CreateUserLog(int LogoutType)
    {
        Boolean blnReturnValue = false;

        Boolean blnSuccess;
        Int64 lngReturn = -1;


        String strHostName = Request.UserHostAddress.ToString();

        string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();


        try
        {

            UserLogInfo.Login_Ip = strIp;

            UserLogInfo.Login_DateTime = DateTime.Now;

            Session["SessionId"] = HttpContext.Current.Session.SessionID;


            //Set current Login 
            UserLogInfo.User_Id = int.Parse(Convert.ToInt64(Session["PhotoUpUserID"]).ToString());
            UserLogInfo.Log_Status_Id = LogoutType;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, txtUserName.Text.Trim().ToLower(), DateTime.Now, DateTime.Now.AddMinutes(Convert.ToInt32(Constants.SESSIONEXPIRATIONTIME)), false, Session["PhotoUpUserName"].ToString().Trim(), FormsAuthentication.FormsCookiePath);

            // encrypt the ticket
            string sEncTicket = FormsAuthentication.Encrypt(ticket);

            // set the cookie
            HttpCookie httpAuthCookie;

            FormsAuthentication.SetAuthCookie(txtUserName.Text.Trim().ToLower(), false);

            httpAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, sEncTicket);

            Response.Cookies.Add(httpAuthCookie);

            strUserAuthCookieID = httpAuthCookie.Value;

            UserLogInfo.Session_Id = Session["SessionId"].ToString();
            UserLogInfo.CookieID = strUserAuthCookieID;
            UserLogInfo.Logout_time = Convert.ToDateTime("1/1/1990");
            //dsUserLog.Tables[0].Rows.Add(drUserLog);
            //blnSuccess = objUserLog.SaveUserLog(UserLogInfo, ref lngReturn, "");
            blnSuccess = objCentralDBBL.SaveUserLog(UserLogInfo, ref lngReturn, "");
            if (blnSuccess == true && lngReturn > 0)
            {
                Session["PhotoUpUserLogID"] = lngReturn;
                blnReturnValue = true;
            }
        }
        catch (Exception ex)
        {
            // Response.Redirect("Error.aspx");


        }

        return blnReturnValue;
    }
}
