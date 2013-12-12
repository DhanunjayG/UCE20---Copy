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
using CarsBL.Dealer;
using System.Collections.Generic;


public partial class Login : System.Web.UI.Page
{
    UserRegistration objUserregBL = new UserRegistration();
    UserLogInfo UserLogInfo = new UserLogInfo();

    public string strUserSessionID;

    public string strUserAuthCookieID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GeneralFunc.SetPageDefaults(Page);
            //GeneralFunc.SaveSiteVisit();
            Session.Abandon();
            Session[Constants.USER_ID] = null;
            Session[Constants.USER_NAME] = null;
            txtEmail.Focus();




            Session["CurrentPage"] = "Home";
            Session["PageName"] = "";
            Session["CurrentPageConfig"] = null;


            ServiceReference objServiceReference = new ServiceReference();

            ScriptReference objScriptReference = new ScriptReference();

            objServiceReference.Path = "~/CarsService.asmx";

            objScriptReference.Path = "~/Static/Js/CarsJScriptNew.js";

            scrptmgr.Services.Add(objServiceReference);
            scrptmgr.Scripts.Add(objScriptReference);
            if (Session[Constants.USER_ID] != null)
            {
                //Request.UrlReferrer.AbsolutePath

            }


        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {

            ////if (delearChk.Checked == true)
            ////{

            DealerRegistration objUserregBL = new DealerRegistration();
            DataSet dsPerformLogin = new DataSet();

            dsPerformLogin = objUserregBL.PerformLogin(txtEmail.Text.Trim(), txtPassword.Text.Trim(), txtDealer.Text.Trim());

            if (dsPerformLogin != null)
            {
                if (dsPerformLogin.Tables.Count > 0)
                {
                    if (dsPerformLogin.Tables[0].Rows.Count > 0)
                    {
                        Session[Constants.USER_ID] = dsPerformLogin.Tables[0].Rows[0]["UId"];

                        Session[Constants.USER_NAME] = dsPerformLogin.Tables[0].Rows[0]["UserName"];

                        Session[Constants.NAME] = dsPerformLogin.Tables[0].Rows[0]["Name"];
                        Session[Constants.BusinessName] = dsPerformLogin.Tables[0].Rows[0]["BusinessName"];
                        Session[Constants.PhoneNumber] = dsPerformLogin.Tables[0].Rows[0]["PhoneNumber"];

                        Session[Constants.DealerCode] = txtDealer.Text;

                        CreateUserLog(1);

                        //Session["PackageID"] = dsPerformLogin.Tables[0].Rows[0]["PackageID"];
                        Response.Redirect("Dealer/Home.aspx");
                    }
                    else
                    {
                        lblError.Text = "Invalid dealercode or username or password!";
                        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "EmailNAClick();", true);
                        //Page.ClientScript.RegisterStartupScript(typeof(Page), "KyRST", "<script type='text/javascript' language='javascript'>pageLoad();</script>");
                        lblError.Visible = true;
                        txtDealer.Text = "";
                        txtEmail.Text = "";
                        txtPassword.Text = "";
                    }

                }
                else
                {
                    // CreateUserLog(7);
                    //  objUserlog.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), System.DateTime.Now, Convert.ToInt32(Session[Constants.USERLOG_ID]), 7);

                    lblError.Text = "Invalid username or password!";
                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "KyRST", "<script type='text/javascript' language='javascript'>pageLoad();</script>");
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "EmailNAClick();", true);
                    lblError.Visible = true;
                    txtDealer.Text = "";
                    txtEmail.Text = "";
                    txtPassword.Text = "";
                }

            }
            else
            {
                // CreateUserLog(7);
                //  objUserlog.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), System.DateTime.Now, Convert.ToInt32(Session[Constants.USERLOG_ID]), 7);

                lblError.Text = "Invalid username or password!";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "KyRST", "<script type='text/javascript' language='javascript'>pageLoad();</script>");
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "EmailNAClick();", true);
                lblError.Visible = true;
                txtDealer.Text = "";
                txtEmail.Text = "";
                txtPassword.Text = "";
            }
            //}
            //else
            //{
            //    DataSet dsPerformLogin = new DataSet();

            //    dsPerformLogin = objUserregBL.PerformLogin(txtEmail.Text, txtPassword.Text);
            //    if (dsPerformLogin != null)
            //    {
            //        if (dsPerformLogin.Tables.Count > 0)
            //        {
            //            if (dsPerformLogin.Tables[0].Rows.Count > 0)
            //            {
            //                Session[Constants.USER_ID] = dsPerformLogin.Tables[0].Rows[0]["UId"];

            //                Session[Constants.USER_NAME] = dsPerformLogin.Tables[0].Rows[0]["UserName"];

            //                Session[Constants.NAME] = dsPerformLogin.Tables[0].Rows[0]["Name"];

            //                Session[Constants.PhoneNumber] = dsPerformLogin.Tables[0].Rows[0]["PhoneNumber"];
            //                CreateUserLog(1);


            //                Session["PackageID"] = dsPerformLogin.Tables[0].Rows[0]["PackageID"];
            //                Response.Redirect("Account.aspx");

            //            }
            //            else
            //            {
            //                Session[Constants.USER_NAME] = txtEmail.Text;
            //                CreateUserLog(3);
            //                lblError.Text = "Invalid username or password!";
            //                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "EmailNAClick();", true); 
            //                Page.ClientScript.RegisterStartupScript(typeof(Page), "KyRST", "<script type='text/javascript' language='javascript'>pageLoad();</script>");
            //                lblError.Visible = true;
            //                txtEmail.Text = "";
            //                txtPassword.Text = "";
            //            }

            //        }
            //        else
            //        {
            //            // CreateUserLog(7);
            //            //  objUserlog.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), System.DateTime.Now, Convert.ToInt32(Session[Constants.USERLOG_ID]), 7);
            //            Session[Constants.USER_NAME] = txtEmail.Text;
            //            CreateUserLog(3);
            //            lblError.Text = "Invalid username or password!";
            //            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "EmailNAClick();", true); 
            //            lblError.Visible = true;
            //            txtEmail.Text = "";
            //            txtPassword.Text = "";
            //        }

            //    }
            //    else
            //    {
            //        // CreateUserLog(7);
            //        //  objUserlog.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), System.DateTime.Now, Convert.ToInt32(Session[Constants.USERLOG_ID]), 7);
            //        Session[Constants.USER_NAME] = txtEmail.Text;
            //        CreateUserLog(3);
            //        lblError.Text = "Invalid username or password!";
            //        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "EmailNAClick();", true); 
            //        lblError.Visible = true;
            //        txtEmail.Text = "";
            //        txtPassword.Text = "";
            //    }
            //}

        }
        catch (Exception ex)
        {

        }
    }
    protected void btnUserLogin_Click(object sender, EventArgs e)
    {
        DataSet dsPerformLogin = new DataSet();

        dsPerformLogin = objUserregBL.PerformLogin(txtCustLogUserID.Text, txtCustLogUserPwd.Text);
        if (dsPerformLogin != null)
        {
            if (dsPerformLogin.Tables.Count > 0)
            {
                if (dsPerformLogin.Tables[0].Rows.Count > 0)
                {
                    Session[Constants.USER_ID] = dsPerformLogin.Tables[0].Rows[0]["UId"];

                    Session[Constants.USER_NAME] = dsPerformLogin.Tables[0].Rows[0]["UserName"];

                    Session[Constants.NAME] = dsPerformLogin.Tables[0].Rows[0]["Name"];

                    Session[Constants.PhoneNumber] = dsPerformLogin.Tables[0].Rows[0]["PhoneNumber"];
                    CreateUserLog(1);


                    Session["PackageID"] = dsPerformLogin.Tables[0].Rows[0]["PackageID"];
                    Response.Redirect("Account.aspx");

                }
                else
                {
                    Session[Constants.USER_NAME] = txtCustLogUserID.Text;
                    CreateUserLog(3);
                    lblUserLogError.Text = "Invalid username or password!";
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "EmailNAClick();", true);
                    lblUserLogError.Visible = true;
                    txtCustLogUserID.Text = "";
                    txtCustLogUserPwd.Text = "";
                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "KyRST", "<script type='text/javascript' language='javascript'>pageLoad();</script>");
                }

            }
            else
            {
                // CreateUserLog(7);
                //  objUserlog.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), System.DateTime.Now, Convert.ToInt32(Session[Constants.USERLOG_ID]), 7);
                Session[Constants.USER_NAME] = txtCustLogUserID.Text;
                CreateUserLog(3);
                lblUserLogError.Text = "Invalid username or password!";
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "EmailNAClick();", true);
               // Page.ClientScript.RegisterStartupScript(typeof(Page), "KyRST", "<script type='text/javascript' language='javascript'>pageLoad();</script>");
                lblUserLogError.Visible = true;
                txtCustLogUserID.Text = "";
                txtCustLogUserPwd.Text = "";
            }

        }
        else
        {
            // CreateUserLog(7);
            //  objUserlog.Perform_LogOut(Convert.ToInt32(Session[Constants.USER_ID]), System.DateTime.Now, Convert.ToInt32(Session[Constants.USERLOG_ID]), 7);
            Session[Constants.USER_NAME] = txtCustLogUserID.Text;
            CreateUserLog(3);
            lblUserLogError.Text = "Invalid username or password!";
            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "EmailNAClick();", true);
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "KyRST", "<script type='text/javascript' language='javascript'>pageLoad();</script>");
            lblUserLogError.Visible = true;
            txtCustLogUserID.Text = "";
            txtCustLogUserPwd.Text = "";
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
            UserLogInfo.User_Id = int.Parse(Convert.ToInt64(Session[Constants.USER_ID]).ToString());
            UserLogInfo.Log_Status_Id = LogoutType;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, txtEmail.Text.Trim().ToLower(), DateTime.Now, DateTime.Now.AddMinutes(Convert.ToInt32(Constants.SESSIONEXPIRATIONTIME)), false, Session[Constants.USER_NAME].ToString().Trim(), FormsAuthentication.FormsCookiePath);

            // encrypt the ticket
            string sEncTicket = FormsAuthentication.Encrypt(ticket);

            // set the cookie
            HttpCookie httpAuthCookie;

            FormsAuthentication.SetAuthCookie(txtEmail.Text.Trim().ToLower(), false);

            httpAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, sEncTicket);

            Response.Cookies.Add(httpAuthCookie);

            strUserAuthCookieID = httpAuthCookie.Value;

            UserLogInfo.Session_Id = Session["SessionId"].ToString();
            UserLogInfo.CookieID = strUserAuthCookieID;
            UserLogInfo.Logout_time = Convert.ToDateTime("1/1/1990");
            //dsUserLog.Tables[0].Rows.Add(drUserLog);
            //blnSuccess = objUserLog.SaveUserLog(UserLogInfo, ref lngReturn, "");
            blnSuccess = objUserregBL.SaveUserLog(UserLogInfo, ref lngReturn, "");
            if (blnSuccess == true && lngReturn > 0)
            {
                Session[Constants.USERLOG_ID] = lngReturn;
                blnReturnValue = true;
            }
        }
        catch (Exception ex)
        {
            // Response.Redirect("Error.aspx");


        }

        return blnReturnValue;
    }


    protected void lnkRegister_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["PackageID"] != null)
            {
                Response.Redirect("Registration.aspx");
            }
            else
            {
                Response.Redirect("SellRegi.aspx");
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void btnSendPwd_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsUserPwd = objUserregBL.USP_GetPasswordByID(txtForgetUserName.Text);
            if (dsUserPwd.Tables.Count > 0)
            {
                if (dsUserPwd.Tables[0].Rows.Count > 0)
                {
                    clsMailFormats format = new clsMailFormats();
                    string LoginPassword = dsUserPwd.Tables[0].Rows[0]["Pwd"].ToString();
                    String LoginUserID = dsUserPwd.Tables[0].Rows[0]["UserID"].ToString();
                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(CommonVariable.FromInfoMail);
                    msg.To.Add(txtForgetUserName.Text);
                    msg.Bcc.Add(CommonVariable.ArchieveMail);
                    msg.Subject = "Forgot Password";
                    msg.IsBodyHtml = true;
                    string text = string.Empty;
                    text = format.SendForgetPassword(LoginUserID, LoginPassword, ref text);
                    msg.Body = text.ToString();
                    SmtpClient smtp = new SmtpClient();
                    //smtp.Host = "smtp.gmail.com";
                    //smtp.Port = 587;
                    //smtp.Credentials = new System.Net.NetworkCredential("satheesh.aakula@gmail.com", "hugomirad");
                    //smtp.EnableSsl = true;
                    //smtp.Send(msg);
                    smtp.Host = "127.0.0.1";
                    smtp.Port = 25;
                    smtp.Send(msg);
                    mpeChangePW.Hide();
                    mpealteruser.Show();
                    lblErr.Visible = true;
                    lblErr.Text = "Password send to your mail id";

                }
                else
                {
                    mpeChangePW.Hide();
                    mpealteruser.Show();
                    lblErr.Visible = true;
                    lblErr.Text = "No user exists with " + txtForgetUserName.Text;
                }
            }
            else
            {
                mpeChangePW.Hide();
                mpealteruser.Show();
                lblErr.Visible = true;
                lblErr.Text = "No user exists with " + txtForgetUserName.Text;
            }
        }
        catch (Exception ex)
        {
        }
    }
}
