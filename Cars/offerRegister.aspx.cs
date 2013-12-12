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
using System.Collections.Generic;
using CarsBL.Masters;
using System.Net;

public partial class offerRegister : System.Web.UI.Page
{
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet dsDropDown = new DataSet();
    UserRegistration objUserregBL = new UserRegistration();
    UserRegistrationInfo objUserRegInfo = new UserRegistrationInfo();
    CarsInfo.CarsInfo objcarsInfo = new CarsInfo.CarsInfo();
    UsedCarsInfo objUsedCarsInfo = new UsedCarsInfo();
    public GeneralFunc objGeneralFunc = new GeneralFunc();


    BankDetailsBL objBankDetailsBL = new BankDetailsBL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dsDropDown = objdropdownBL.Usp_Get_DropDown();
            GetAllModels();
            GetMakes();

            DataSet dsYears = objBankDetailsBL.USP_GetNext12years();

            fillYears(dsYears);


            Session["DsDropDown"] = dsDropDown;

            FillStates();

        }
    }
    private void fillYears(DataSet dsYears)
    {
        try
        {
            CCExpiresYear.DataSource = dsYears.Tables[0];
            CCExpiresYear.DataTextField = "YearNum";
            CCExpiresYear.DataValueField = "YearValue";
            CCExpiresYear.DataBind();
            CCExpiresYear.Items.Insert(0, new ListItem("Select Year", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    private void FillStates()
    {
        try
        {
            ddlBillState.DataSource = dsDropDown.Tables[1];
            ddlBillState.DataTextField = "State_Code";
            ddlBillState.DataValueField = "State_ID";
            ddlBillState.DataBind();
            ddlBillState.Items.Insert(0, new ListItem("Unknown", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    private void GetAllModels()
    {
        try
        {
            DataSet dsAllModels = new DataSet();
            dsAllModels = objdropdownBL.USP_GetAllModels(0);
            Session["AllModel"] = dsAllModels;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void GetMakes()
    {
        try
        {
            var obj = new List<MakesInfo>();

            //MakesInfo objMakes = new MakesInfo();
            MakesBL objMakesBL = new MakesBL();

            obj = (List<MakesInfo>)objMakesBL.GetMakes();
            Session["Makes"] = obj;


            ddlMake.DataSource = obj;
            ddlMake.DataTextField = "Make";
            ddlMake.DataValueField = "MakeID";
            ddlMake.DataBind();
            ddlMake.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetModelsInfo();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void GetModelsInfo()
    {
        try
        {
            //var objModel = new List<MakesInfo>();
            //objModel = Session["AllModel"] as List<MakesInfo>;
            DataSet dsModels = Session["AllModel"] as DataSet;
            int makeid = Convert.ToInt32(ddlMake.SelectedItem.Value);
            DataView dvModel = new DataView();
            DataTable dtModel = new DataTable();
            dvModel = dsModels.Tables[0].DefaultView;
            dvModel.RowFilter = "MakeID='" + makeid.ToString() + "'";
            dtModel = dvModel.ToTable();
            ddlModel.DataSource = dtModel;
            ddlModel.Items.Clear();
            ddlModel.DataTextField = "Model";
            ddlModel.DataValueField = "MakeModelID";
            ddlModel.DataBind();
            ddlModel.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    //PaymentSucces.aspx
    private bool AuthorizePayment()
    {
        string sPrice = string.Empty;
        //CustomValidator1.ErrorMessage = "";
        string AuthNetVersion = "3.1"; // Contains CCV support
        string AuthNetLoginID = "9FtTpx88g879"; //Set your AuthNetLoginID here
        string AuthNetTransKey = "9Gp3Au9t97Wvb784";  // Get this from your authorize.net merchant interface

        sPrice = "9.99";

        WebClient webClientRequest = new WebClient();
        System.Collections.Specialized.NameValueCollection InputObject = new System.Collections.Specialized.NameValueCollection(30);
        System.Collections.Specialized.NameValueCollection ReturnObject = new System.Collections.Specialized.NameValueCollection(30);

        byte[] ReturnBytes;
        string[] ReturnValues;
        string ErrorString;
        //(TESTMODE) Bill To Company is required. (33) 

        InputObject.Add("x_version", AuthNetVersion);
        InputObject.Add("x_delim_data", "True");
        InputObject.Add("x_login", AuthNetLoginID);
        InputObject.Add("x_tran_key", AuthNetTransKey);
        InputObject.Add("x_relay_response", "False");

        //----------------------Set to False to go Live--------------------
        InputObject.Add("x_test_request", "False");
        //---------------------------------------------------------------------
        InputObject.Add("x_delim_char", ",");
        InputObject.Add("x_encap_char", "|");

        //Billing Address
        InputObject.Add("x_first_name", txtCardholderName.Text);
        InputObject.Add("x_last_name", LastNameTextBox.Text);
        InputObject.Add("x_phone", txtBillPhone.Text);
        InputObject.Add("x_address", AddressTextBox.Text);
        InputObject.Add("x_city", CityTextBox.Text);
        InputObject.Add("x_state", ddlBillState.SelectedItem.Text);
        InputObject.Add("x_zip", txtBillZip.Text);
        InputObject.Add("x_email", EmailTextBox.Text);
        InputObject.Add("x_email_customer", "TRUE");                     //Emails Customer
        InputObject.Add("x_merchant_email", "shravan@datumglobal.net");  //Emails Merchant
        InputObject.Add("x_country", CountryTextBox.Text);
        InputObject.Add("x_customer_ip", Request.UserHostAddress);  //Store Customer IP Address

        //Amount
        InputObject.Add("x_description", "Payment to Enhanced package $9.99 Got Discount package $40");

        //string.Format("{0:00}", Convert.ToDecimal(lblAdPrice2.Text))) + "Dollars
        //Description of Purchase

        //lblPackDescrip.Text 
        //Card Details
        InputObject.Add("x_card_num", txtCardNumber.Text);
        InputObject.Add("x_exp_date", ExpMon.SelectedItem.Text + "/" + CCExpiresYear.SelectedItem.Text);
        InputObject.Add("x_card_code", cvv.Text);

        InputObject.Add("x_method", "CC");
        InputObject.Add("x_type", "AUTH_CAPTURE");

        InputObject.Add("x_amount", string.Format("{0:c2}", Convert.ToDouble(sPrice)));

        //InputObject.Add("x_amount", string.Format("{0:c2}", lblAdPrice2));
        // Currency setting. Check the guide for other supported currencies
        InputObject.Add("x_currency_code", "USD");

        try
        {
            //Actual Server
            //Set above Testmode=off to go live
            webClientRequest.BaseAddress = "https://secure.authorize.net/gateway/transact.dll";

            ReturnBytes = webClientRequest.UploadValues(webClientRequest.BaseAddress, "POST", InputObject);
            ReturnValues = System.Text.Encoding.ASCII.GetString(ReturnBytes).Split(",".ToCharArray());

            if (ReturnValues[0].Trim(char.Parse("|")) == "1")
            {
                //AuthNetCodeLabel.Text = ReturnValues[4].Trim(char.Parse("|")); // Returned Authorisation Code
                //AuthNetTransIDLabel.Text = ReturnValues[6].Trim(char.Parse("|")); // Returned Transaction ID

                PaymentSuccess();

                SavePaymentDetails(sPrice, "USD", txtCardholderName.Text, ReturnValues[4].Trim(char.Parse("|")));


                Response.Redirect("PaymentSucces.aspx?NetCode=" + ReturnValues[4].Trim(char.Parse("|")) + "&tx=" + ReturnValues[6].Trim(char.Parse("|")) + "&amt=" + sPrice + "&item_number=" + Session["PackageID"].ToString() + "");

                lblErr.Text = "Authorisation Code" + ReturnValues[4].Trim(char.Parse("|")) + "</br>TransID=" + ReturnValues[6].Trim(char.Parse("|")); // Returned Authorisation Code;
                mpeAlert.Show();
                return true;
            }
            else
            {
                // Error!
                ErrorString = ReturnValues[3].Trim(char.Parse("|")) + " (" + ReturnValues[2].Trim(char.Parse("|")) + ")";

                if (ReturnValues[2].Trim(char.Parse("|")) == "44")
                {
                    // CCV transaction decline
                    ErrorString += "Credit Card Code Verification (CCV) returned the following error: ";

                    switch (ReturnValues[38].Trim(char.Parse("|")))
                    {
                        case "N":
                            ErrorString += "Card Code does not match.";
                            break;
                        case "P":
                            ErrorString += "Card Code was not processed.";
                            break;
                        case "S":
                            ErrorString += "Card Code should be on card but was not indicated.";
                            break;
                        case "U":
                            ErrorString += "Issuer was not certified for Card Code.";
                            break;
                    }
                }

                if (ReturnValues[2].Trim(char.Parse("|")) == "45")
                {
                    if (ErrorString.Length > 1)
                        ErrorString += "<br />n";

                    // AVS transaction decline
                    ErrorString += "Address Verification System (AVS) " +
                      "returned the following error: ";

                    switch (ReturnValues[5].Trim(char.Parse("|")))
                    {
                        case "A":
                            ErrorString += " the zip code entered does not match the billing address.";
                            break;
                        case "B":
                            ErrorString += " no information was provided for the AVS check.";
                            break;
                        case "E":
                            ErrorString += " a general error occurred in the AVS system.";
                            break;
                        case "G":
                            ErrorString += " the credit card was issued by a non-US bank.";
                            break;
                        case "N":
                            ErrorString += " neither the entered street address nor zip code matches the billing address.";
                            break;
                        case "P":
                            ErrorString += " AVS is not applicable for this transaction.";
                            break;
                        case "R":
                            ErrorString += " please retry the transaction; the AVS system was unavailable or timed out.";
                            break;
                        case "S":
                            ErrorString += " the AVS service is not supported by your credit card issuer.";
                            break;
                        case "U":
                            ErrorString += " address information is unavailable for the credit card.";
                            break;
                        case "W":
                            ErrorString += " the 9 digit zip code matches, but the street address does not.";
                            break;
                        case "Z":
                            ErrorString += " the zip code matches, but the address does not.";
                            break;
                    }
                }
                lblErr.Text = ErrorString;
                mpeAlert.Show();
                // ErrorString contains the actual error
                //CustomValidator1.ErrorMessage = ErrorString;
                return false;
            }
        }
        catch (Exception ex)
        {
            //CustomValidator1.ErrorMessage = ex.Message;
            return false;
        }
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        AuthorizePayment();
    }

    private void SavePaymentDetails(string amt, string sCurrency, string sUserName, string txtTran)
    {
        BankDetailsBL objBankDetailsBL = new BankDetailsBL();
        PmntDetailsinfo objPmntDetailsinfo = new PmntDetailsinfo();
        objPmntDetailsinfo.Currency = Request.QueryString["cc"];

        int CarID;

        if ((Session["CarID"] == null) || (Session["CarID"].ToString() == ""))
        {
            CarID = Convert.ToInt32(0);
        }
        else
        {
            CarID = Convert.ToInt32(Session["CarID"].ToString());
        }
        int UID = 0;

        Session["PayBy"] = CardType.SelectedItem.Value;

        if (Session["RegUSER_ID"] != null)
        {
            UID = Convert.ToInt32(Session["RegUSER_ID"].ToString());
        }
        objPmntDetailsinfo.TransactionID = Request.QueryString["NetCode"];

        objPmntDetailsinfo.PmntStatus = 2;
        objPmntDetailsinfo.Amount = amt;

        objPmntDetailsinfo.CardType = Session["PayBy"].ToString();
        objPmntDetailsinfo.PmntType = 1;


        String strHostName = Request.UserHostAddress.ToString();
        string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
        objPmntDetailsinfo.IPAddress = strIp;


        int PostingID = Convert.ToInt32(Session["PostingID"].ToString());

        int UserPackID = Convert.ToInt32(Session["RegUserPackID"].ToString());

        bool bnew = objBankDetailsBL.UpdatePmntStatus(objPmntDetailsinfo, 9, UID, CarID, UserPackID, PostingID);

    }
    private void PaymentSuccess()
    {

        try
        {
            objUserRegInfo.Name = GeneralFunc.ToProper(txtContactName.Text).Trim();
            objUserRegInfo.UserName = txtEmail.Text;
            objUserRegInfo.Pwd = txtPassword.Text;
            objUserRegInfo.PhoneNumber = txtPhone.Text;
            objUserRegInfo.CouponCode = "";
            objUserRegInfo.ReferCode = "";
            objUserRegInfo.Address = "";
            objUserRegInfo.City = "";
            objUserRegInfo.StateID = Convert.ToInt32(0);
            objUserRegInfo.BusinessName = "";
            objUserRegInfo.AltEmail = "";
            objUserRegInfo.AltPhone = "";

            objUserRegInfo.Zip = "";

            Session["PackageID"] = 9;
            objUserRegInfo.PackageID = Convert.ToInt32(Session["PackageID"].ToString());
            string UserID;
            string FistName = GeneralFunc.ToProper(txtContactName.Text).Trim();
            if (FistName.Length > 4)
            {
                FistName = FistName.Substring(0, 4);
            }
            string s = "";
            int j;
            Random random1 = new Random();
            for (j = 1; j < 6; j++)
            {
                s += random1.Next(0, 9).ToString();
            }
            UserID = FistName + s.ToString();
            int EmailExists = 1;
            DataSet dsUserExists = objUserregBL.USP_ChkUserExists(txtEmail.Text);
            if (dsUserExists.Tables.Count > 0)
            {
                if (dsUserExists.Tables[0].Rows.Count > 0)
                {
                    mpeAlert.Show();
                    lblErr.Visible = true;
                    lblErr.Text = "Email " + txtEmail.Text + " already exists";
                }
                else
                {
                    DataSet dsUserInfo = objUserregBL.Usp_Save_RegisterLogUser(objUserRegInfo, EmailExists, UserID);
                    Session["RegUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
                    Session["RegUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
                    Session["RegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
                    Session["RegPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
                    Session["PackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
                    Session["RegPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
                    Session["RegUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();

                    int UID = Convert.ToInt32(Session["RegUSER_ID"].ToString());
                    objcarsInfo.YearOfMake = Convert.ToInt32(ddlYear.SelectedItem.Text);
                    Session["SelYear"] = ddlYear.SelectedItem.Text;
                    Session["SelMake"] = ddlMake.SelectedItem.Text;
                    Session["SelModel"] = ddlModel.SelectedItem.Text;
                    objcarsInfo.MakeModelID = Convert.ToInt32(ddlModel.SelectedItem.Value);
                    objcarsInfo.BodyTypeID = Convert.ToInt32(0);
                    objcarsInfo.CarID = Convert.ToInt32(0);
                    if (txtPrice.Text == "")
                    {
                        objcarsInfo.Price = "0";
                    }
                    else
                    {
                        objcarsInfo.Price = txtPrice.Text;
                    }
                    objcarsInfo.Mileage = "0";
                    objcarsInfo.ExteriorColor = "Unspecified";
                    objcarsInfo.InteriorColor = "Unspecified";
                    objcarsInfo.Transmission = "Unspecified";
                    objcarsInfo.NumberOfDoors = "Unspecified";
                    objcarsInfo.DriveTrain = "Unspecified";
                    objcarsInfo.VIN = "";
                    objcarsInfo.NumberOfCylinder = "Unspecified";
                    objcarsInfo.FuelTypeID = Convert.ToInt32(0);
                    objcarsInfo.Zipcode = "";
                    objcarsInfo.City = "";
                    objcarsInfo.StateID = Convert.ToInt32(0);
                    string Condition = string.Empty;
                    string CondiDescrip = string.Empty;
                    Condition = "";
                    CondiDescrip = "Unspecified";
                    string Title = "";
                    DataSet dsCarsDetails = objdropdownBL.USP_SaveCarDetails(objcarsInfo, Condition, CondiDescrip, Title, UID);
                    Session["CarID"] = Convert.ToInt32(dsCarsDetails.Tables[0].Rows[0]["CarID"].ToString());
                    int Isactive;
                    int CarIDs;
                    int FeatureID;
                    objUsedCarsInfo.SellerName = "";
                    objUsedCarsInfo.Address1 = "";
                    objUsedCarsInfo.City = "";
                    objUsedCarsInfo.State = "Unspecified";
                    objUsedCarsInfo.Zip = "";
                    objUsedCarsInfo.Phone = txtPhone.Text;
                    objUsedCarsInfo.Email = txtEmail.Text;
                    objUsedCarsInfo.SellerID = Convert.ToInt32(0);
                    CarIDs = Convert.ToInt32(Session["CarID"].ToString());
                    int PackageID;
                    int PaymentID;
                    int UserPackID = Convert.ToInt32(Session["RegUserPackID"].ToString());
                    int PostingID;
                    if (Session["PostingID"] == null)
                    {
                        PostingID = Convert.ToInt32(0);
                    }
                    else if (Session["PostingID"].ToString() == "")
                    {
                        PostingID = Convert.ToInt32(0);
                    }
                    else
                    {
                        PostingID = Convert.ToInt32(Session["PostingID"]);
                    }
                    PackageID = Convert.ToInt32(Session["PackageID"]);
                    PaymentID = Convert.ToInt32(0);
                    Session["PackageID"] = PackageID;
                    DataSet dsCarFeature = new DataSet();
                    DataSet dsPosting = new DataSet();
                    String strHostName = Request.UserHostAddress.ToString();
                    string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                    dsPosting = objdropdownBL.USP_SaveSellerInfo(objUsedCarsInfo, CarIDs, UID, PackageID, PaymentID, UserPackID, PostingID, strIp);
                    Session["PostingID"] = dsPosting.Tables[0].Rows[0]["PostingID"].ToString();
                }
            }
            else
            {
                DataSet dsUserInfo = objUserregBL.Usp_Save_RegisterLogUser(objUserRegInfo, EmailExists, UserID);
                Session["RegUSER_ID"] = Convert.ToInt32(dsUserInfo.Tables[0].Rows[0]["UId"].ToString());
                Session["RegUserName"] = dsUserInfo.Tables[0].Rows[0]["UserName"].ToString();
                Session["RegName"] = dsUserInfo.Tables[0].Rows[0]["Name"].ToString();
                Session["RegPhoneNumber"] = dsUserInfo.Tables[0].Rows[0]["PhoneNumber"].ToString();
                Session["PackageID"] = dsUserInfo.Tables[0].Rows[0]["PackageID"].ToString();
                Session["RegPassword"] = dsUserInfo.Tables[0].Rows[0]["Pwd"].ToString();
                Session["RegUserPackID"] = dsUserInfo.Tables[0].Rows[0]["UserPackID"].ToString();

                int UID = Convert.ToInt32(Session["RegUSER_ID"].ToString());
                objcarsInfo.YearOfMake = Convert.ToInt32(ddlYear.SelectedItem.Text);
                Session["SelYear"] = ddlYear.SelectedItem.Text;
                Session["SelMake"] = ddlMake.SelectedItem.Text;
                Session["SelModel"] = ddlModel.SelectedItem.Text;
                objcarsInfo.MakeModelID = Convert.ToInt32(ddlModel.SelectedItem.Value);
                objcarsInfo.BodyTypeID = Convert.ToInt32(0);
                objcarsInfo.CarID = Convert.ToInt32(0);
                if (txtPrice.Text == "")
                {
                    objcarsInfo.Price = "0";
                }
                else
                {
                    objcarsInfo.Price = txtPrice.Text;
                }
                objcarsInfo.Mileage = "0";
                objcarsInfo.ExteriorColor = "Unspecified";
                objcarsInfo.InteriorColor = "Unspecified";
                objcarsInfo.Transmission = "Unspecified";
                objcarsInfo.NumberOfDoors = "Unspecified";
                objcarsInfo.DriveTrain = "Unspecified";
                objcarsInfo.VIN = "";
                objcarsInfo.NumberOfCylinder = "Unspecified";
                objcarsInfo.FuelTypeID = Convert.ToInt32(0);
                objcarsInfo.Zipcode = "";
                objcarsInfo.City = "";
                objcarsInfo.StateID = Convert.ToInt32(0);
                string Condition = string.Empty;
                string CondiDescrip = string.Empty;
                Condition = "";
                CondiDescrip = "Unspecified";
                string Title = "";
                DataSet dsCarsDetails = objdropdownBL.USP_SaveCarDetails(objcarsInfo, Condition, CondiDescrip, Title, UID);
                Session["CarID"] = Convert.ToInt32(dsCarsDetails.Tables[0].Rows[0]["CarID"].ToString());
                int Isactive;
                int CarIDs;
                int FeatureID;
                objUsedCarsInfo.SellerName = "";
                objUsedCarsInfo.Address1 = "";
                objUsedCarsInfo.City = "";
                objUsedCarsInfo.State = "Unspecified";
                objUsedCarsInfo.Zip = "";
                objUsedCarsInfo.Phone = txtPhone.Text;
                objUsedCarsInfo.Email = txtEmail.Text;
                objUsedCarsInfo.SellerID = Convert.ToInt32(0);
                CarIDs = Convert.ToInt32(Session["CarID"].ToString());
                int PackageID;
                int PaymentID;
                int UserPackID = Convert.ToInt32(Session["RegUserPackID"].ToString());
                int PostingID;
                if (Session["PostingID"] == null)
                {
                    PostingID = Convert.ToInt32(0);
                }
                else if (Session["PostingID"].ToString() == "")
                {
                    PostingID = Convert.ToInt32(0);
                }
                else
                {
                    PostingID = Convert.ToInt32(Session["PostingID"]);
                }
                PackageID = Convert.ToInt32(Session["PackageID"]);
                PaymentID = Convert.ToInt32(0);
                Session["Packag eID"] = PackageID;
                DataSet dsCarFeature = new DataSet();
                DataSet dsPosting = new DataSet();
                String strHostName = Request.UserHostAddress.ToString();
                string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
                dsPosting = objdropdownBL.USP_SaveSellerInfo(objUsedCarsInfo, CarIDs, UID, PackageID, PaymentID, UserPackID, PostingID, strIp);


                Session["PostingID"] = dsPosting.Tables[0].Rows[0]["PostingID"].ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
