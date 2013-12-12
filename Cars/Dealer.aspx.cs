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


public partial class Dealer : System.Web.UI.Page
{
    DealersInfo objDealersInfo = new DealersInfo();
    NewCarsBl objNewCarsBL = new NewCarsBl();
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GeneralFunc.SaveSiteVisit();
           
            Session["CurrentPage"] = "DealerReg";
            Session["PageName"] = "Packages";
            Session["CurrentPageConfig"] = null;
            GeneralFunc.SetPageDefaults(Page);
            KeyWords.Addkeywordstags(Header);
            ServiceReference objServiceReference = new ServiceReference();

            ScriptReference objScriptReference = new ScriptReference();

            objServiceReference.Path = "~/CarsService.asmx";

            objScriptReference.Path = "~/Static/Js/CarsJScriptNew.js";

            scrptmgr.Services.Add(objServiceReference);
            scrptmgr.Scripts.Add(objScriptReference);
        }
    }
    protected void btnregister_Click(object sender, EventArgs e)
    {
        try
        {
            objDealersInfo.DealerName = GeneralFunc.ToProper(txtContcname.Text).Trim();
            objDealersInfo.DealerPhone = txtphone.Text;
            objDealersInfo.DealerEmail = txtemail.Text;
            objDealersInfo.DealerAddress = txtRegAddress.Text;
            objDealersInfo.DealerCity = txtRegCity.Text;
            objDealersInfo.DealerShipName = txtDealerShipName.Text;
            objDealersInfo.DealerNotes = txtDealerNotes.Text;
            if (txtZip.Text.Length == 4)
            {
                objDealersInfo.DealerZip = "0" + txtZip.Text;
            }
            else
            {
                objDealersInfo.DealerZip = txtZip.Text;
            }
            DataSet dsDealerInfo = new DataSet();
            dsDealerInfo = objNewCarsBL.USP_SaveDealerRequest(objDealersInfo);
            if (dsDealerInfo.Tables[0].Rows.Count > 0)
            {
                string DealerName = dsDealerInfo.Tables[0].Rows[0]["DealerName"].ToString();
                string Phone = dsDealerInfo.Tables[0].Rows[0]["DealerPhone"].ToString();
                string Email = dsDealerInfo.Tables[0].Rows[0]["DealerEmail"].ToString();
                clsMailFormats format = new clsMailFormats();
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(CommonVariable.FromInfoMail);
                msg.To.Add(CommonVariable.ContactUsEMail);
                msg.CC.Add(CommonVariable.ContactUsEMailCC);
                msg.Bcc.Add(CommonVariable.ArchieveMail );
                msg.Subject = "Regarding Dealer request";
                msg.IsBodyHtml = true;
                string text = string.Empty;
                text = format.SendDealerRequestDetails(DealerName, Phone, Email, ref text);
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
