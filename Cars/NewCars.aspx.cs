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

public partial class NewCars : System.Web.UI.Page
{
    NewCarsBl objNewCarsBl = new NewCarsBl();
    NewCarsInfo objNewCarsInfo = new NewCarsInfo();
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KeyWords.Addkeywordstags(Header);
            GeneralFunc.SaveSiteVisit();
            Session["CurrentPage"] = "New Cars";
            Session["PageName"] = "NewCars";

            Session["CurrentPageConfig"] = null;
            GeneralFunc.SaveSiteVisit();

            ServiceReference objServiceReference = new ServiceReference();

            ScriptReference objScriptReference = new ScriptReference();

            objServiceReference.Path = "~/CarsService.asmx";

            objScriptReference.Path = "~/Static/Js/CarsJScriptNew.js";

            scrptmgr.Services.Add(objServiceReference);
            scrptmgr.Scripts.Add(objScriptReference);

            GeneralFunc.SetPageDefaults(Page);
        }

    }


    protected void btnregister_Click(object sender, EventArgs e)
    {
        try
        {
            objNewCarsInfo.NewCarRequestName = GeneralFunc.ToProper(txtContactName.Text).Trim();
            objNewCarsInfo.NewCarReqPhoneNumber = txtphone.Text;
            objNewCarsInfo.NewCarReqEmail = txtemail.Text;
            DataSet dsNewCarRequest = new DataSet();
            dsNewCarRequest = objNewCarsBl.USP_SaveNewCarRequest(objNewCarsInfo);

            if (dsNewCarRequest.Tables[0].Rows.Count > 0)
            {
                string NewCarName = dsNewCarRequest.Tables[0].Rows[0]["NewCarRequestName"].ToString();
                string Phone = dsNewCarRequest.Tables[0].Rows[0]["NewCarReqPhoneNumber"].ToString();
                string Email = dsNewCarRequest.Tables[0].Rows[0]["NewCarReqEmail"].ToString();
                clsMailFormats format = new clsMailFormats();
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(CommonVariable.FromInfoMail);

                msg.To.Add(CommonVariable.ContactUsEMail);
                msg.CC.Add(CommonVariable.ContactUsEMailCC);
                msg.Subject = "Regarding new car request";
                msg.IsBodyHtml = true;
                string text = string.Empty;
                text = format.SendNewcarRequestDetails(NewCarName, Phone, Email, ref text);
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
            }
            mpealteruser.Show();

        }
        catch (Exception ex)
        {
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Default.aspx");
        }
        catch (Exception ex)
        {
        }
    }
}
