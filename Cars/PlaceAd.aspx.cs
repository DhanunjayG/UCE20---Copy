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

public partial class Registar : System.Web.UI.Page
{
    DropdownBL objdropdownBL = new DropdownBL();
    DataSet dsDropDown = new DataSet();
    CarsInfo.CarsInfo objcarsInfo = new CarsInfo.CarsInfo();
    UsedCarsInfo objUsedCarsInfo = new UsedCarsInfo();
    public GeneralFunc objGeneralFunc = new GeneralFunc();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Constants.NAME] == null)
        {
            Response.Redirect("Default.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                Session["CurrentPage"] = "Home";
                Session["PageName"] = "";
                Session["CurrentPageConfig"] = null;

                GeneralFunc.SetPageDefaults(Page);
                ServiceReference objServiceReference = new ServiceReference();

                ScriptReference objScriptReference = new ScriptReference();

                objServiceReference.Path = "~/CarsService.asmx";

                objScriptReference.Path = "~/Static/Js/CarsJScriptNew.js";

                scrptmgr.Services.Add(objServiceReference);
                scrptmgr.Scripts.Add(objScriptReference);
                Session["UpDatePackageID"] = null;
                //lblUserName.Text = Session[Constants.NAME].ToString();
                dsDropDown = objdropdownBL.Usp_Get_DropDown();
                Session["DsDropDown"] = dsDropDown;
                DataSet dsUserInfoDetails = objdropdownBL.USP_GetUSerDetailsByUserID(Convert.ToInt32(Session[Constants.USER_ID].ToString()));

                GetAllModels();
                GetMakes();
                GetBody();
                FillYear();
                FillFuelTypes();
                FillStates();
                FillExteriorColor();
                FillInteriorColor();
                FillCylinders();
                FillDoors();
                FillTransmissions();
                FillDriveTrain();
                FillCondition();


                if ((Session["PostingID"] != null) && (Session["PostingID"].ToString() != ""))
                {
                    //btnUpdatePackage.Visible = true;
                    DataSet dsCarDetailsInfo = new DataSet();
                    dsCarDetailsInfo = objdropdownBL.USP_GetCustomerDetailsByPostingID(Convert.ToInt32(Session["PostingID"].ToString()));
                    if (dsCarDetailsInfo.Tables[0].Rows.Count > 0)
                    {
                        Session["CarID"] = dsCarDetailsInfo.Tables[0].Rows[0]["carid"].ToString();
                        Session["PackageID"] = dsCarDetailsInfo.Tables[0].Rows[0]["packageID"].ToString();
                        Session["PaymentID"] = dsCarDetailsInfo.Tables[0].Rows[0]["PaymentID"].ToString();
                        Session["UserPackID"] = dsCarDetailsInfo.Tables[0].Rows[0]["UserPackID"].ToString();
                        Session["SellerID"] = dsCarDetailsInfo.Tables[0].Rows[0]["SellerID"].ToString();

                        if (dsCarDetailsInfo.Tables[0].Rows[0]["IsActive"].ToString() == "True")
                        {
                            btnSold.Visible = true;
                            btnWithdraw.Visible = true;
                            btnActive.Visible = false;
                        }
                        else
                        {
                            btnSold.Visible = false;
                            btnWithdraw.Visible = false;
                            btnActive.Visible = true;
                        }


                        FillPackage();
                        ListItem list1 = new ListItem();
                        list1.Text = dsCarDetailsInfo.Tables[0].Rows[0]["yearOfMake"].ToString();
                        list1.Value = dsCarDetailsInfo.Tables[0].Rows[0]["yearOfMake"].ToString();
                        ddlYear.SelectedIndex = ddlYear.Items.IndexOf(list1);

                        ListItem list2 = new ListItem();
                        list2.Value = dsCarDetailsInfo.Tables[0].Rows[0]["MakeID"].ToString();
                        list2.Text = dsCarDetailsInfo.Tables[0].Rows[0]["make"].ToString();
                        ddlMake.SelectedIndex = ddlMake.Items.IndexOf(list2);
                        GetModelsInfo();

                        ListItem list3 = new ListItem();
                        list3.Text = dsCarDetailsInfo.Tables[0].Rows[0]["model"].ToString();
                        list3.Value = dsCarDetailsInfo.Tables[0].Rows[0]["makeModelID"].ToString();
                        ddlModel.SelectedIndex = ddlModel.Items.IndexOf(list3);

                        Session["SelYear"] = dsCarDetailsInfo.Tables[0].Rows[0]["yearOfMake"].ToString();
                        Session["SelMake"] = dsCarDetailsInfo.Tables[0].Rows[0]["make"].ToString();
                        Session["SelModel"] = dsCarDetailsInfo.Tables[0].Rows[0]["model"].ToString();
                        Session["MakeModelID"] = dsCarDetailsInfo.Tables[0].Rows[0]["makeModelID"].ToString();


                        ListItem list4 = new ListItem();
                        list4.Value = dsCarDetailsInfo.Tables[0].Rows[0]["bodyTypeID"].ToString();
                        list4.Text = dsCarDetailsInfo.Tables[0].Rows[0]["bodyType"].ToString();
                        ddlStyle.SelectedIndex = ddlStyle.Items.IndexOf(list4);

                        ListItem list5 = new ListItem();
                        list5.Value = dsCarDetailsInfo.Tables[0].Rows[0]["State_Id"].ToString();
                        list5.Text = dsCarDetailsInfo.Tables[0].Rows[0]["State"].ToString();
                        ddlLocationState.SelectedIndex = ddlLocationState.Items.IndexOf(list5);

                        ListItem list6 = new ListItem();
                        list6.Value = dsCarDetailsInfo.Tables[0].Rows[0]["fuelTypeID"].ToString();
                        list6.Text = dsCarDetailsInfo.Tables[0].Rows[0]["fuelType"].ToString();
                        ddlFuelType.SelectedIndex = ddlFuelType.Items.IndexOf(list6);

                        ListItem list7 = new ListItem();
                        list7.Value = dsCarDetailsInfo.Tables[0].Rows[0]["exteriorColor"].ToString();
                        list7.Text = dsCarDetailsInfo.Tables[0].Rows[0]["exteriorColor"].ToString();
                        ddlExteriorColor.SelectedIndex = ddlExteriorColor.Items.IndexOf(list7);


                        ListItem list8 = new ListItem();
                        list8.Text = dsCarDetailsInfo.Tables[0].Rows[0]["interiorColor"].ToString();
                        list8.Value = dsCarDetailsInfo.Tables[0].Rows[0]["interiorColor"].ToString();
                        ddlInteriorColor.SelectedIndex = ddlInteriorColor.Items.IndexOf(list8);

                        ListItem list9 = new ListItem();
                        list9.Value = dsCarDetailsInfo.Tables[0].Rows[0]["numberOfCylinder"].ToString();
                        list9.Text = dsCarDetailsInfo.Tables[0].Rows[0]["numberOfCylinder"].ToString();
                        ddlCylinderCount.SelectedIndex = ddlCylinderCount.Items.IndexOf(list9);

                        ListItem list10 = new ListItem();
                        list10.Value = dsCarDetailsInfo.Tables[0].Rows[0]["NumberOfDoors"].ToString();
                        list10.Text = dsCarDetailsInfo.Tables[0].Rows[0]["NumberOfDoors"].ToString();
                        ddlDoorCount.SelectedIndex = ddlDoorCount.Items.IndexOf(list10);

                        ListItem list11 = new ListItem();
                        list11.Value = dsCarDetailsInfo.Tables[0].Rows[0]["Transmission"].ToString();
                        list11.Text = dsCarDetailsInfo.Tables[0].Rows[0]["Transmission"].ToString();
                        ddlTransmission.SelectedIndex = ddlTransmission.Items.IndexOf(list11);

                        ListItem list12 = new ListItem();
                        list12.Value = dsCarDetailsInfo.Tables[0].Rows[0]["DriveTrain"].ToString();
                        list12.Text = dsCarDetailsInfo.Tables[0].Rows[0]["DriveTrain"].ToString();
                        ddlDriveTrain.SelectedIndex = ddlDriveTrain.Items.IndexOf(list12);

                        ListItem list13 = new ListItem();
                        list13.Value = dsCarDetailsInfo.Tables[0].Rows[0]["ConditionID"].ToString();
                        list13.Text = dsCarDetailsInfo.Tables[0].Rows[0]["ConditionDescription"].ToString();
                        ddlCondition.SelectedIndex = ddlCondition.Items.IndexOf(list13);

                        //txtSellerName.Text = dsCarDetailsInfo.Tables[0].Rows[0]["sellerName"].ToString();
                        //txtAddress.Text = GeneralFunc.ToProper(dsCarDetailsInfo.Tables[0].Rows[0]["address1"].ToString());
                        txtCity.Text = GeneralFunc.ToProper(dsCarDetailsInfo.Tables[0].Rows[0]["city"].ToString());
                        txtZip.Text = dsCarDetailsInfo.Tables[0].Rows[0]["zip"].ToString();
                        txtSellerPhone.Text = dsCarDetailsInfo.Tables[0].Rows[0]["phone"].ToString();
                        txtSellerEmail.Text = dsCarDetailsInfo.Tables[0].Rows[0]["email"].ToString();


                        if (dsCarDetailsInfo.Tables[0].Rows[0]["price"].ToString() == "0.0000")
                        {
                            txtAskingPrice.Text = "";
                        }
                        else
                        {
                            txtAskingPrice.Text = string.Format("{0:0.00}", Convert.ToDouble(dsCarDetailsInfo.Tables[0].Rows[0]["price"].ToString()));
                        }
                        if (txtAskingPrice.Text.Length > 6)
                        {
                            txtAskingPrice.Text = txtAskingPrice.Text.Substring(0, 6);
                        }

                        if (dsCarDetailsInfo.Tables[0].Rows[0]["mileage"].ToString() == "0.00")
                        {
                            txtMileage.Text = "";
                        }
                        else
                        {
                            txtMileage.Text = string.Format("{0:0.00}", Convert.ToDouble(dsCarDetailsInfo.Tables[0].Rows[0]["mileage"].ToString()));
                        }
                        if (txtMileage.Text.Length > 6)
                        {
                            txtMileage.Text = txtMileage.Text.Substring(0, 6);
                        }

                        txtVin.Text = dsCarDetailsInfo.Tables[0].Rows[0]["VIN"].ToString();
                        txtCondition.Text = dsCarDetailsInfo.Tables[0].Rows[0]["description"].ToString();
                        txtTitle.Text = dsCarDetailsInfo.Tables[0].Rows[0]["Title"].ToString();
                        for (int i = 1; i < 52; i++)
                        {
                            string ChkBoxID = "chkFeatures" + i.ToString();
                            CheckBox ChkedBox = (CheckBox)form1.FindControl(ChkBoxID);
                            if (dsCarDetailsInfo.Tables[3].Rows.Count >= i)
                            {
                                if (dsCarDetailsInfo.Tables[3].Rows[i - 1]["Isactive"].ToString() == "True")
                                {
                                    ChkedBox.Checked = true;
                                }
                                else
                                {
                                    ChkedBox.Checked = false;
                                }
                            }
                            else
                            {
                                ChkedBox.Checked = false;
                            }
                        }
                        if (dsCarDetailsInfo.Tables[4].Rows.Count > 0)
                        {
                            lblExistUrlRes.Visible = false;
                            divlblMultiSite.Style["display"] = "none";
                            grdMultiSites.Visible = true;
                            grdMultiSites.DataSource = dsCarDetailsInfo.Tables[4];
                            grdMultiSites.DataBind();

                        }
                        else
                        {
                            grdMultiSites.Visible = false;
                            lblExistUrlRes.Visible = true;
                            lblExistUrlRes.Text = "Not uploaded..!";
                            divlblMultiSite.Style["display"] = "block";
                        }
                    }
                }
            }

        }
    }

    private void FillYear()
    {
        try
        {
            DataSet dsYears = new DataSet();
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
            ddlYear.Items.Insert(0, new ListItem("Unspecified", "0"));
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
            ddlTransmission.DataSource = dsDropDown.Tables[7];
            ddlTransmission.DataTextField = "TransmissionName";
            ddlTransmission.DataValueField = "TransmissionName";
            ddlTransmission.DataBind();
            ddlTransmission.Items.Insert(0, new ListItem("Unspecified", "Unspecified"));
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
            ddlMake.Items.Insert(0, new ListItem("Unspecified", "0"));
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
            ddlStyle.Items.Insert(0, new ListItem("Unspecified", "0"));
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
            ddlModel.Items.Insert(0, new ListItem("Unspecified", "0"));
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
    //protected void btnSaveVehicleType_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        objcarsInfo.YearOfMake = Convert.ToInt32(ddlYear.SelectedItem.Text);
    //        objcarsInfo.MakeModelID = Convert.ToInt32(ddlModel.SelectedItem.Value);
    //        objcarsInfo.BodyTypeID = Convert.ToInt32(ddlStyle.SelectedItem.Value);
    //        if (Session["CarID"] == null)
    //        {
    //            objcarsInfo.CarID = Convert.ToInt32(0);
    //        }
    //        else
    //        {
    //            objcarsInfo.CarID = Convert.ToInt32(Session["CarID"].ToString());
    //        }
    //        DataSet dsSaveType = objdropdownBL.USP_SaveVehicleType(objcarsInfo);
    //        Session["CarID"] = Convert.ToInt32(dsSaveType.Tables[0].Rows[0]["CarID"].ToString());
    //        mpealteruser.Show();
    //        lblErr.Visible = true;
    //        lblErr.Text = "Vehicle type details saved successfully";
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void btnVehicleInfoSave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        objcarsInfo.Price = txtAskingPrice.Text;
    //        objcarsInfo.Mileage = txtMileage.Text;
    //        objcarsInfo.ExteriorColor = ddlExteriorColor.SelectedItem.Text;
    //        objcarsInfo.InteriorColor = ddlInteriorColor.SelectedItem.Text;
    //        objcarsInfo.Transmission = ddlTransmission.SelectedItem.Text;
    //        objcarsInfo.NumberOfDoors = ddlDoorCount.SelectedItem.Value;
    //        objcarsInfo.VIN = txtVin.Text;
    //        objcarsInfo.NumberOfCylinder = ddlCylinderCount.SelectedItem.Value;
    //        objcarsInfo.FuelTypeID = Convert.ToInt32(ddlFuelType.SelectedItem.Value);
    //        if (txtZip.Text.Length == 4)
    //        {
    //            objcarsInfo.Zipcode = "0" + txtZip.Text;
    //        }
    //        else
    //        {
    //            objcarsInfo.Zipcode = txtZip.Text;
    //        }
    //        objcarsInfo.City = txtCity.Text;
    //        objcarsInfo.StateID = Convert.ToInt32(ddlLocationState.SelectedItem.Value);
    //        if (Session["CarID"] == null)
    //        {
    //            objcarsInfo.CarID = Convert.ToInt32(0);
    //        }
    //        else
    //        {
    //            objcarsInfo.CarID = Convert.ToInt32(Session["CarID"].ToString());
    //        }
    //        DataSet dsVehicleInfo = objdropdownBL.USP_SaveVehicleInformation(objcarsInfo);
    //        Session["CarID"] = Convert.ToInt32(dsVehicleInfo.Tables[0].Rows[0]["CarID"].ToString());
    //        mpealteruser.Show();
    //        lblErr.Visible = true;
    //        lblErr.Text = "Vehicle information details saved successfully";
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void btnCarFeaturesSave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int Isactive;
    //        int CarIDs;
    //        int FeatureID;
    //        for (int i = 1; i < 31; i++)
    //        {
    //            if (Session["CarID"] == null)
    //            {
    //                CarIDs = 0;
    //            }
    //            else
    //            {
    //                CarIDs = Convert.ToInt32(Session["CarID"].ToString());
    //            }
    //            string ChkBoxID = "chkFeatures" + i.ToString();
    //            CheckBox ChkedBox = (CheckBox)form1.FindControl(ChkBoxID);
    //            if (ChkedBox.Checked == true)
    //            {
    //                Isactive = 1;
    //            }
    //            else
    //            {
    //                Isactive = 0;
    //            }
    //            FeatureID = i;
    //            DataSet dsCarFeature = objdropdownBL.USP_SaveCarFeatures(CarIDs, FeatureID, Isactive);
    //            Session["CarID"] = Convert.ToInt32(dsCarFeature.Tables[0].Rows[0]["CarID"].ToString());
    //        }
    //        CarIDs = Convert.ToInt32(Session["CarID"].ToString());
    //        string Condition = string.Empty;
    //        string CondiDescrip = string.Empty;
    //        Condition = txtCondition.Text;
    //        CondiDescrip = txtConditionDescrip.Text;
    //        bool bnew = objdropdownBL.USP_SaveCarFeaturesConditions(CarIDs, Condition, CondiDescrip);
    //        mpealteruser.Show();
    //        lblErr.Visible = true;
    //        lblErr.Text = "Vehicle features details saved successfully";
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    protected void btnSaveCarDetails_Click(object sender, EventArgs e)
    {
        try
        {            
            string NewMake = ddlMake.SelectedItem.Text;
            string NewModel = ddlModel.SelectedItem.Text;
            string NewYear = ddlYear.SelectedItem.Text;
            string OldMake = Session["SelMake"].ToString();
            string OldModel = Session["SelModel"].ToString();
            string OldYear = Session["SelYear"].ToString();
            if ((OldYear != NewYear) || (OldMake != NewMake) || (OldModel != NewModel))
            {
                MdepAlertSave.Show();
                lblAlertSave.Visible = true;
                lblAlertSave.Text = "Sorry we are not able change year,make and model of your car.<br />Remaining information will update<br />Please contact our customer service they will be able to help you update";
                SendStatusCarChangesInfoMail(OldYear, OldMake, OldModel, NewYear, NewMake, NewModel);
            }
            else
            {
                SaveDetails();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnAlertSaveOk_Click(object sender, EventArgs e)
    {
        try
        {
            SaveDetails();
        }
        catch (Exception ex)
        {
        }
    }

    private void SaveDetails()
    {
        try
        {
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            objcarsInfo.YearOfMake = Convert.ToInt32(Session["SelYear"].ToString());
            objcarsInfo.MakeModelID = Convert.ToInt32(Session["MakeModelID"].ToString());
            objcarsInfo.BodyTypeID = Convert.ToInt32(ddlStyle.SelectedItem.Value);
            if ((Session["CarID"] == null) || (Session["CarID"].ToString() == ""))
            {
                objcarsInfo.CarID = Convert.ToInt32(0);
            }
            else
            {
                objcarsInfo.CarID = Convert.ToInt32(Session["CarID"].ToString());
            }
            if (txtAskingPrice.Text == "")
            {
                objcarsInfo.Price = "0";
            }
            else
            {
                objcarsInfo.Price = txtAskingPrice.Text;
            }
            if (txtMileage.Text == "")
            {
                objcarsInfo.Mileage = "0";
            }
            else
            {
                objcarsInfo.Mileage = txtMileage.Text;
            }
            objcarsInfo.ExteriorColor = ddlExteriorColor.SelectedItem.Text;
            objcarsInfo.InteriorColor = ddlInteriorColor.SelectedItem.Text;
            objcarsInfo.Transmission = ddlTransmission.SelectedItem.Text;
            objcarsInfo.DriveTrain = ddlDriveTrain.SelectedItem.Value;
            objcarsInfo.NumberOfDoors = ddlDoorCount.SelectedItem.Value;
            objcarsInfo.VIN = txtVin.Text;
            objcarsInfo.NumberOfCylinder = ddlCylinderCount.SelectedItem.Value;
            objcarsInfo.FuelTypeID = Convert.ToInt32(ddlFuelType.SelectedItem.Value);
            if (txtZip.Text.Length == 4)
            {
                objcarsInfo.Zipcode = "0" + txtZip.Text;
            }
            else
            {
                objcarsInfo.Zipcode = txtZip.Text;
            }
            objcarsInfo.City = GeneralFunc.ToProper(txtCity.Text);
            objcarsInfo.StateID = Convert.ToInt32(ddlLocationState.SelectedItem.Value);
            string Condition = string.Empty;
            string CondiDescrip = string.Empty;
            Condition = GeneralFunc.ToProper(txtCondition.Text);
            string Title = txtTitle.Text;
            CondiDescrip = ddlCondition.SelectedItem.Text;

            DataSet dsCarsDetails = objdropdownBL.USP_SaveCarDetails(objcarsInfo, Condition, CondiDescrip, Title, UID);
            Session["CarID"] = Convert.ToInt32(dsCarsDetails.Tables[0].Rows[0]["CarID"].ToString());
            int Isactive;
            int CarIDs;
            int FeatureID;
            objUsedCarsInfo.SellerName = "";
            objUsedCarsInfo.Address1 = "";
            objUsedCarsInfo.City = GeneralFunc.ToProper(txtCity.Text);
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
            if ((Session["SellerID"] == null) || (Session["SellerID"].ToString() == ""))
            {
                objUsedCarsInfo.SellerID = Convert.ToInt32(0);
            }
            else
            {
                objUsedCarsInfo.SellerID = Convert.ToInt32(Session["SellerID"].ToString());
            }
            CarIDs = Convert.ToInt32(Session["CarID"].ToString());

            int PackageID;
            int PaymentID;
            int PostingID;
            //if (Session["PaymentID"] == null)
            //{
            //    PackageID = Convert.ToInt32(ddlPackage.SelectedItem.Value);
            //    PaymentID = Convert.ToInt32(0);
            //}
            //else if (Session["PaymentID"].ToString() == "")
            //{
            //    PackageID = Convert.ToInt32(ddlPackage.SelectedItem.Value);
            //    PaymentID = Convert.ToInt32(0);
            //}
            //else
            //{
            PackageID = Convert.ToInt32(Session["PackageID"]);
            if (Session["PaymentID"] == null)
            {
                PaymentID = Convert.ToInt32(0);
            }
            else if (Session["PaymentID"].ToString() == "")
            {
                PaymentID = Convert.ToInt32(0);
            }
            else
            {
                PaymentID = Convert.ToInt32(Session["PaymentID"]);
            }
            //}
            // int PackageID = Convert.ToInt32(Session["PackageID"]); //Convert.ToInt32(ddlPackage.SelectedItem.Value);
            Session["PackageID"] = PackageID;
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


            String strHostName = Request.UserHostAddress.ToString();
            string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();

            int UserPackID = Convert.ToInt32(Session["UserPackID"].ToString());
            DataSet dsposting = new DataSet();
            dsposting = objdropdownBL.USP_SaveSellerInfo(objUsedCarsInfo, CarIDs, UID, PackageID, PaymentID, UserPackID, PostingID, strIp);
            Session["PostingID"] = dsposting.Tables[0].Rows[0]["PostingID"].ToString();

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
                DataSet dsCarFeature = objdropdownBL.USP_SaveCarFeatures(CarIDs, FeatureID, Isactive, UID);
                Session["CarID"] = Convert.ToInt32(dsCarFeature.Tables[0].Rows[0]["CarID"].ToString());
            }
            //mpealteruser.Show();
            //lblErr.Visible = true;
            //lblErr.Text = "Car details saved successfully <br />Do you want to add photos now?";
            Response.Redirect("placeadPhotos.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
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
}
