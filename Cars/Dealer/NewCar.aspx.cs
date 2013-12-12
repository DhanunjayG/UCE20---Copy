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
using System.Collections.Generic;
using CarsBL.Masters;
using System.Web.UI.MobileControls;
using System.Net.Mail;
using CarsBL.Dealer;

public partial class NewCar : System.Web.UI.Page
{
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet dsDropDown = new DataSet();

    UsedCarsInfo objUsedCarsInfo = new UsedCarsInfo();
    public GeneralFunc objGeneralFunc = new GeneralFunc();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Constants.NAME] == null)
        {
            Response.Redirect("../login.aspx");
        }
        else
            if (!IsPostBack)
            {
                Session["CarID"] = null;
                Session["PostingID"] = null;
                Session[Constants.SellerID] = null;

                Session["CurrentPage"] = "Home";
                Session["PageName"] = "";
                Session["CurrentPageConfig"] = null;

                //    Session[Constants.NAME];
                //lblDealerCode.Text = Session[Constants.DealerCode].ToString();
                //lblDealerID1.Text = Session[Constants.NAME].ToString();
                //lblDealerID.Text = Session[Constants.NAME].ToString();

                Session["UpDatePackageID"] = null;
                //lblUserName.Text = Session[Constants.NAME].ToString();
                if (Session ["DsDropDown"] == null)
                {
                    dsDropDown = objdropdownBL.Usp_Get_DropDown();
                    Session["DsDropDown"] = dsDropDown;
                }
                else
                {
                    dsDropDown = (DataSet)Session["DsDropDown"];
                }

                DataSet dsUserInfoDetails = objdropdownBL.USP_GetUSerDetailsByUserID(Convert.ToInt32(Session[Constants.USER_ID].ToString()));

                //lblDealerCode.Text 
                Session["UpDatePackageID"] = null;
                //lblUserName.Text = Session[Constants.NAME].ToString();
                dsDropDown = objdropdownBL.Usp_Get_DropDown();
                Session["DsDropDown"] = dsDropDown;
                // DataSet dsUserInfoDetails = objdropdownBL.USP_GetUSerDetailsByUserIDForRegLog(Convert.ToInt32(Session["RegUSER_ID"].ToString()));
                GetAllModels();
                GetMakes();
                GetBody();
                FillYear();
                FillFuelTypes();
                FillStates();
                FillPackage();
                FillExteriorColor();
                FillInteriorColor();
                FillCylinders();
                FillDoors();
                FillTransmissions();
                FillDriveTrain();
                FillCondition();
                DealerActions objDealerAction = new DealerActions();
                DataSet dsSellerInfo = objDealerAction.DealerDefaultSellerInfo(Session[Constants.DealerCode].ToString(), Convert.ToInt32(Session[Constants.USER_ID].ToString()));

                Session["SellerInfo"] = dsSellerInfo;
                
                if (dsSellerInfo.Tables[0].Rows.Count > 0)
                {
                    txtSellerName.Text = dsSellerInfo.Tables[0].Rows[0]["sellerName"].ToString();  //txta dsSellerInfo.Tables[0].Rows[0]["address1"]
                    txtSellerAddress.Text = dsSellerInfo.Tables[0].Rows[0]["address1"].ToString();
                    txtCity.Text = dsSellerInfo.Tables[0].Rows[0]["city"].ToString();
                    ddlLocationState.SelectedIndex = ddlLocationState.Items.IndexOf(ddlLocationState.Items.FindByValue(dsSellerInfo.Tables[0].Rows[0]["state"].ToString()));
                    txtZip.Text = dsSellerInfo.Tables[0].Rows[0]["zip"].ToString();
                    txtSellerPhone.Text = dsSellerInfo.Tables[0].Rows[0]["phone"].ToString();
                    txtSellerEmail.Text = dsSellerInfo.Tables[0].Rows[0]["email"].ToString();
                    chkbx.Checked = true; 
                }
                
                if ((Session["PostingID"] != null) && (Session["PostingID"].ToString() != ""))
                {
                    //btnUpdatePackage.Visible = true;
                    //FillUpdateInfo();
                }



            }
    }

    private void FillYear()
    {
        try
        {
            DataSet dsYears = new DataSet();

            ddlYear.DataSource = null;
            ddlYear.DataBind();
 
            if (Session["CarsYears"] == null)
            {
                dsYears = objdropdownBL.GetYears();
                Session["CarsYears"] = dsYears;
            }
            else
            {
                dsYears = Session["CarsYears"] as DataSet;
            }
            ddlYear.DataSource = dsYears.Tables[0];

            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem("Select", "-1"));
            
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

            if (Session["AllModel"] == null)
            {
                dsAllModels = objdropdownBL.USP_GetAllModels(0);
                Session["AllModel"] = dsAllModels;
            }
            else
            {
                dsAllModels = (DataSet)Session["AllModel"];
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void FillCondition()
    {
        try
        {
            ddlCondition.DataSource = dsDropDown.Tables[9];
            ddlCondition.DataTextField = "condition";
            ddlCondition.DataValueField = "conditionID";
            ddlCondition.DataBind();
            ddlCondition.SelectedIndex = 0;
            //ddlCondition.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillDriveTrain()
    {
        try
        {
            ddlDriveTrain.DataSource = dsDropDown.Tables[6];

            ddlDriveTrain.DataTextField = "NoOfDoorsName";
            ddlDriveTrain.DataValueField = "NoOfDoorsName";
            ddlDriveTrain.DataBind();
            ddlDriveTrain.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillTransmissions()
    {
        try
        {
            DataView dv = new DataView();
            DataTable dt = new DataTable();

            dv = dsDropDown.Tables[7].DefaultView;
            dv.RowFilter = "TransmissionID in (13,9,15,16)";
            dt = dv.ToTable();
            ddlTransmission.DataSource = dt;

            ddlTransmission.DataTextField = "TransmissionName";
            ddlTransmission.DataValueField = "TransmissionName";
            ddlTransmission.DataBind();
            ddlTransmission.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
            ddlTransmission.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
        }
    }

    private void FillDoors()
    {
        try
        {
            ddlDoorCount.DataSource = dsDropDown.Tables[8];
            ddlDoorCount.DataTextField = "DoorsCount";
            ddlDoorCount.DataValueField = "DoorsCount";
            ddlDoorCount.DataBind();
            ddlDoorCount.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillCylinders()
    {
        try
        {
            ddlCylinderCount.DataSource = dsDropDown.Tables[5];
            ddlCylinderCount.DataTextField = "CylindersName";
            ddlCylinderCount.DataValueField = "CylindersName";
            ddlCylinderCount.DataBind();
            ddlCylinderCount.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillInteriorColor()
    {
        try
        {
            ddlInteriorColor.DataSource = dsDropDown.Tables[4];
            ddlInteriorColor.DataTextField = "InteriorColorName";
            ddlInteriorColor.DataValueField = "InteriorColorName";
            ddlInteriorColor.DataBind();
            ddlInteriorColor.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillExteriorColor()
    {
        try
        {
            ddlExteriorColor.DataSource = dsDropDown.Tables[3];
            ddlExteriorColor.DataTextField = "ExteriorColorName";
            ddlExteriorColor.DataValueField = "ExteriorColorName";
            ddlExteriorColor.DataBind();
            ddlExteriorColor.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillPackage()
    {
        try
        {
            lblPackDiv.Style["display"] = "block";
            int RowNo = Convert.ToInt32(Session["PackageID"].ToString()) - 1;
            Double PackCost = new Double();
            PackCost = Convert.ToDouble(dsDropDown.Tables[2].Rows[RowNo]["Price"].ToString());
            string PackAmount = string.Format("{0:0.00}", PackCost).ToString();
            string PackName = dsDropDown.Tables[2].Rows[RowNo]["Description"].ToString();
            lblPackageName.Text = PackName + "($" + PackAmount + ")";
            //}

        }
        catch (Exception ex)
        {
        }
    }

    private void FillStates()
    {
        try
        {
            ddlLocationState.DataSource = dsDropDown.Tables[1];
            ddlLocationState.DataTextField = "State_Code";
            ddlLocationState.DataValueField = "State_ID";
            ddlLocationState.DataBind();
            ddlLocationState.Items.Insert(0, new ListItem("Unspecified", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    private void FillFuelTypes()
    {
        try
        {
            ddlFuelType.DataSource = dsDropDown.Tables[0];
            ddlFuelType.DataTextField = "FuelType";
            ddlFuelType.DataValueField = "FuelTypeID";
            ddlFuelType.DataBind();
        }
        catch (Exception ex)
        {
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
            ddlMake.Items.Insert(0, new ListItem("Select", "-1"));
        }
        catch (Exception ex)
        {
        }
    }
    public void GetBody()
    {
        try
        {
            var obj = new List<BodyInfo>();

            //MakesInfo objMakes = new MakesInfo();
            MakesBL objMakesBL = new MakesBL();

            obj = (List<BodyInfo>)objMakesBL.GetBodys();
            Session["Bodys"] = obj;


            ddlStyle.DataSource = obj;
            ddlStyle.DataTextField = "bodyType";
            ddlStyle.DataValueField = "bodyTypeID";
            ddlStyle.DataBind();
            
        }
        catch (Exception ex)
        {
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
            ddlModel.Items.Insert(0, new ListItem("Select", "-1"));

        }
        catch (Exception ex)
        {
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
        }
    }

    protected void btnSaveCarDetails_Click(object sender, EventArgs e)
    {
        DealerActions objActions = new DealerActions();
        try
        {
            DataSet dsCheckUser = objActions.DealerCheckUniqueID(txtDealerUniqueID.Text, Session[Constants.DealerCode].ToString());
            if (dsCheckUser.Tables[0].Rows.Count > 0)
            {

                MdepAlertSave.Show();
                lblAlertSave.Visible = true;
                lblAlertSave.Text = "Dealer inventory id already exist. Please enter a unique Dealer inventory id.";
                return;
            }

            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            objUsedCarsInfo.DealerUniqueID = txtDealerUniqueID.Text;
            objUsedCarsInfo.YearOfMake = Convert.ToInt32(ddlYear.SelectedItem.Text);
            Session["SelYear"] = ddlYear.SelectedItem.Text;
            Session["SelMake"] = ddlMake.SelectedItem.Text;
            Session["SelModel"] = ddlModel.SelectedItem.Text;
            objUsedCarsInfo.MakeModelID = Convert.ToInt32(ddlModel.SelectedItem.Value);

            if (ddlStyle.SelectedItem.Value == "-1")
            {
                objUsedCarsInfo.BodytypeID = 0;
            }
            else
            {
                objUsedCarsInfo.BodytypeID = Convert.ToInt32(ddlStyle.SelectedItem.Value);
            }

            

            //if ((Session["CarID"] == null) || (Session["CarID"].ToString() == ""))
            //{
            //    objUsedCarsInfo.Carid = Convert.ToInt32(0);
            //}
            //else
            //{
            //    objUsedCarsInfo.Carid = Convert.ToInt32(Session["CarID"].ToString());
            //}

            if (txtAskingPrice.Text == "")
            {
                objUsedCarsInfo.Price = 0.00;
            }
            else
            {
                objUsedCarsInfo.Price = Convert.ToDouble(txtAskingPrice.Text);
            }

            if (txtCurrentPrice.Text == "")
            {
                objUsedCarsInfo.CurrentPrice = "0";
            }
            else
            {
                objUsedCarsInfo.CurrentPrice = txtCurrentPrice.Text;
            }

            if (txtPurchaseCost.Text == "")
            {
                objUsedCarsInfo.PurchaseCost = "0";
            }
            else
            {
                objUsedCarsInfo.PurchaseCost = txtPurchaseCost.Text;
            }


            if (txtMileage.Text == "")
            {
                objUsedCarsInfo.Mileage = "0";
            }
            else
            {
                objUsedCarsInfo.Mileage = txtMileage.Text;
            }

            objUsedCarsInfo.ExteriorColor = ddlExteriorColor.SelectedItem.Text;
            objUsedCarsInfo.InteriorColor = ddlInteriorColor.SelectedItem.Text;
            objUsedCarsInfo.Transmission = ddlTransmission.SelectedItem.Text;
            objUsedCarsInfo.NumberOfDoors = ddlDoorCount.SelectedItem.Value;
            objUsedCarsInfo.DriveTrain = ddlDriveTrain.SelectedItem.Value;
            objUsedCarsInfo.VIN = txtVin.Text;
            objUsedCarsInfo.NumberOfCylinder = ddlCylinderCount.SelectedItem.Value;
            objUsedCarsInfo.FueltypeId = Convert.ToInt32(ddlFuelType.SelectedItem.Value);
            objUsedCarsInfo.CurrentPrice = txtCurrentPrice.Text;
            objUsedCarsInfo.PurchaseCost = txtPurchaseCost.Text;


            if (txtZip.Text.Length == 4)
            {
                objUsedCarsInfo.Zipcode = "0" + txtZip.Text;
            }
            else
            {
                objUsedCarsInfo.Zipcode = txtZip.Text;
            }
            objUsedCarsInfo.City = GeneralFunc.ToProper(txtCity.Text);
            objUsedCarsInfo.State = ddlLocationState.SelectedItem.Text;
            string Condition = string.Empty;
            string CondiDescrip = string.Empty;
            Condition = GeneralFunc.ToProper(txtCondition.Text);
            string Title = txtTitle.Text;
            CondiDescrip = ddlCondition.SelectedItem.Text;




            
            

            int Isactive;
            int CarIDs;
            int FeatureID;
            objUsedCarsInfo.SellerName = txtSellerName.Text ;
            objUsedCarsInfo.Address1 = txtSellerAddress.Text;
            objUsedCarsInfo.City = GeneralFunc.ToProper(txtCity.Text);
            objUsedCarsInfo.StateID = ddlLocationState.SelectedItem.Value.ToString();
            if (txtZip.Text.Length == 4)
            {
                objUsedCarsInfo.Zip = "0" + txtZip.Text;
            }
            else
            {
                objUsedCarsInfo.Zip = txtZip.Text;
            }
            objUsedCarsInfo.Phone = txtSellerPhone.Text;

            objUsedCarsInfo.State = ddlLocationState.SelectedItem.Text.ToString();

            if (txtZip.Text.Length == 4)
            {
                objUsedCarsInfo.Zip = "0" + txtZip.Text;
            }
            else
            {
                objUsedCarsInfo.Zip = txtZip.Text;
            }
            objUsedCarsInfo.Phone = txtSellerPhone.Text;
            objUsedCarsInfo.Email = txtSellerEmail.Text;

            if ((Session[Constants.SellerID] == null) || (Session[Constants.SellerID].ToString() == ""))
            {
                objUsedCarsInfo.SellerID = Convert.ToInt32(0);
            }
            else
            {
                objUsedCarsInfo.SellerID = Convert.ToInt32(Session[Constants.SellerID].ToString());
            }
            //CarIDs = Convert.ToInt32(Session["CarID"].ToString());


            DataSet dsPackages = objActions.USP_ChkPackageForAddCar(UID);

            int PaymentID;

            //**************************************************???///
            //during we need take user package from dealer package id 
            //**************************************************???///
            int UserPackID = 0;
            int PackageID = 0;
            if (dsPackages.Tables[0].Rows.Count > 0)
            {
                UserPackID = Convert.ToInt32(dsPackages.Tables[0].Rows[0]["UserPackID"].ToString());
                PackageID = Convert.ToInt32(dsPackages.Tables[0].Rows[0]["PackageID"].ToString());
                Session["PackageID"] = PackageID;

            }
            else
            {
                dsPackages = objActions.USP_ChkExistingPackage(UID);
                if (dsPackages.Tables[0].Rows.Count > 0)
                {
                    UserPackID = Convert.ToInt32(dsPackages.Tables[0].Rows[0]["UserPackID"].ToString());
                    PackageID = Convert.ToInt32(dsPackages.Tables[0].Rows[0]["PackageID"].ToString());
                    Session["PackageID"] = PackageID;
                }


            }
            objUsedCarsInfo.AdStatus = resAction1.SelectedItem.Value;

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

            PaymentID = Convert.ToInt32(0);

            

            if (PackageID == 1)
            {
                Session["isActive"] = false;
            }

            DataSet dsCarFeature = new DataSet();
            DataSet dsPosting = new DataSet();

            String strHostName = Request.UserHostAddress.ToString();
            string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();


            DataSet dsCarsDetails = objActions.SaveCarAndSellerInfo(objUsedCarsInfo, Condition, CondiDescrip, Title, UID, UID, PackageID, PaymentID, UserPackID, PostingID, strIp, Session[Constants.DealerCode].ToString());

            Session["CarID"] = Convert.ToInt32(dsCarsDetails.Tables[0].Rows[0]["CarID"].ToString());
            Session["PostingID"] = dsCarsDetails.Tables[0].Rows[0]["PostingID"].ToString();
            Session[Constants.SellerID] = dsCarsDetails.Tables[0].Rows[0]["sellerID"].ToString();

            for (int i = 1; i < 52; i++)
            {
                if (Session["CarID"] == null)
                {
                    CarIDs = 0;
                }
                else
                {
                    CarIDs = Convert.ToInt32(Session["CarID"].ToString());
                }
                string ChkBoxID = "chkFeatures" + i.ToString();
                CheckBox ChkedBox = (CheckBox)form1.FindControl(ChkBoxID);
                if (ChkedBox.Checked == true)
                {
                    Isactive = 1;
                }
                else
                {
                    Isactive = 0;
                }
                FeatureID = i;
                dsCarFeature = objActions.USP_SaveCarFeatures(CarIDs, FeatureID, Isactive, UID);
            }
            Session["CarID"] = Convert.ToInt32(dsCarFeature.Tables[0].Rows[0]["CarID"].ToString());

            //lblErrorMSg.Visible = true;

            //lblErrorMSg.Text = "You car Details Added";

            //MpeAlert.Show();
            Session["CarID"] = null;
            Session["PostingID"] = null;
            Session[Constants.SellerID] = null;
            Response.Redirect("PicsUpload.aspx?CarInventoryID=" + dsCarsDetails.Tables[0].Rows[0]["PostingID"].ToString() + "P" + txtDealerUniqueID.Text + "");

        }
        catch (Exception ex)
        {
        }
    }

    protected void btnAlertSaveOk_Click(object sender, EventArgs e)
    {
        try
        {
            //SaveDetails();
        }
        catch (Exception ex)
        {
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
    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            //int IsActive = 0;
            //int AdStatus = Convert.ToInt32(Session["ListStatusAd"].ToString());
            //int PostingID = Convert.ToInt32(Session["PostingID"].ToString());
            //bool bnew = objdropdownBL.USP_UpdateListingStatus(PostingID, IsActive, AdStatus);
            //mpealteruser.Hide();
            //lblErrorMSg.Visible = true;
            //lblErrorMSg.Text = "You are trying to change the vehicle you are advertising, Please contact our customer service they will be able to help you update"; 
            //MpeAlert.Show();


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnSold_Click(object sender, EventArgs e)
    {
        try
        {
            //Session["ListStatusAd"] = 4;
            //mpealteruser.Show();
            //lblErr.Visible = true;
            //lblErr.Text = "Do you want to make your listing sold?";
            SendStatusChangeMail("sold");
            lblErrorMSg.Visible = true;
            lblErrorMSg.Text = "You are trying to change the vehicle you are advertising, Please contact our customer service they will be able to help you update";
            MpeAlert.Show();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnWithdraw_Click(object sender, EventArgs e)
    {
        try
        {
            //Session["ListStatusAd"] = 3;
            //mpealteruser.Show();
            //lblErr.Visible = true;
            //lblErr.Text = "Do you want to withdraw your listing?";
            SendStatusChangeMail("withdraw");
            mpealteruser.Hide();
            lblErrorMSg.Visible = true;
            lblErrorMSg.Text = "You are trying to change the vehicle you are advertising, Please contact our customer service they will be able to help you update";
            MpeAlert.Show();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        try
        {
            SendStatusChangeMail("active");
            mdepActiveAd.Show();
            Page.ClientScript.RegisterStartupScript(typeof(Page), "KyRST", "<script type='text/javascript' language='javascript'>pageLoad();</script>");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnok1_Click(object sender, EventArgs e)
    {
        Response.Redirect("account.aspx");
    }
    protected void btnok1_Click1(object sender, EventArgs e)
    {
        Response.Redirect("account.aspx");
    }

    private void SendStatusChangeMail(string Status)
    {
        try
        {
            string UserEmail = Session[Constants.USER_NAME].ToString();
            string UserDisName = Session[Constants.NAME].ToString();
            clsMailFormats format = new clsMailFormats();
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(CommonVariable.FromInfoMail);
            msg.To.Add(CommonVariable.FromInfoMail);
            msg.Bcc.Add(CommonVariable.ArchieveMail);
            msg.Subject = "Regarding customer requested to change status of Car ID# " + Session["CarID"].ToString();
            msg.IsBodyHtml = true;
            string text = string.Empty;
            text = format.SendStatusChangeRequestMail(UserDisName, Status, ref text);
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
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private void SendStatusCarChangesInfoMail(string OldYear, string OldMake, string OldModel, string NewYear, string NewMake, string NewModel)
    {
        try
        {
            string UserEmail = Session[Constants.USER_NAME].ToString();
            string UserDisName = Session[Constants.NAME].ToString();
            clsMailFormats format = new clsMailFormats();
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(CommonVariable.FromInfoMail);
            msg.To.Add(CommonVariable.FromInfoMail);
            msg.Bcc.Add(CommonVariable.ArchieveMail);
            msg.Subject = "Regarding customer requested to change car details of Car ID# " + Session["CarID"].ToString();
            msg.IsBodyHtml = true;
            string text = string.Empty;
            text = format.SendCarInfoRequestMail(UserDisName, OldYear, OldMake, OldModel, NewYear, NewMake, NewModel, ref text);
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
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BtnSignout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("../login.aspx");
    }
    protected void chkbx_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbx.Checked == true)
        {
            DataSet dsSellerInfo = (DataSet)Session["SellerInfo"];
            if (dsSellerInfo.Tables[0].Rows.Count > 0)
            {
                txtSellerName.Text = dsSellerInfo.Tables[0].Rows[0]["sellerName"].ToString();  //txta dsSellerInfo.Tables[0].Rows[0]["address1"]
                txtSellerAddress.Text = dsSellerInfo.Tables[0].Rows[0]["address1"].ToString();
                txtCity.Text = dsSellerInfo.Tables[0].Rows[0]["city"].ToString();
                ddlLocationState.SelectedIndex = ddlLocationState.Items.IndexOf(ddlLocationState.Items.FindByText(dsSellerInfo.Tables[0].Rows[0]["state"].ToString()));
                txtZip.Text = dsSellerInfo.Tables[0].Rows[0]["zip"].ToString();
                txtSellerPhone.Text = dsSellerInfo.Tables[0].Rows[0]["phone"].ToString();
                txtSellerEmail.Text = dsSellerInfo.Tables[0].Rows[0]["email"].ToString();
                
            }
        }
        else
        {
            txtSellerAddress.Text = "";
            txtSellerName.Text = "";
            txtCity.Text = "";
            txtZip.Text = "";
            txtSellerPhone.Text = "";
            txtSellerEmail.Text = "";
            ddlLocationState.SelectedIndex = 0;
        }
    }
}
