using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CarsInfo;
using System.Collections;
using CarsBL.Dealer;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using CarsBL.Transactions;
using System.Data;
using System.Web.UI.HtmlControls;




public partial class Dealer_DealerCarBulkUpload : System.Web.UI.Page
{
    public GeneralFunc GenFunc = new GeneralFunc();


    UsedCarsInfo objUsedCarsInfo = new UsedCarsInfo();
    public GeneralFunc objGeneralFunc = new GeneralFunc();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Constants.USER_ID] == null)
        {
            Response.Redirect("../Login.aspx");
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            SaveData();

            lblErrorMsg.Attributes.Add("Style", "color: #3079ed; display: inline-block;line-height: 30px;");

            lblErrorMsg.Text = grdIntroInfo.Rows.Count + " Cars Data Uploaded Successfully";
            txtRecordCount.Text = "0";
            grdIntroInfo.DataSource = null;
            grdIntroInfo.DataBind();

        }
        catch (Exception ex)
        {
            // Response.Redirect("Error.aspx");

        }
        finally
        {
            //mymail.Dispose();
            //smtpClient = null;
        }
    }


    private void SaveData()
    {
        UsedCarsInfo objcarsInfo = new UsedCarsInfo();
        DataSet ds = (DataSet)Session["ExcelData"];
        DealerActions objSaveCar = new DealerActions();

        try
        {
            int UID = Convert.ToInt32(Session[Constants.USER_ID].ToString());
            foreach (GridViewRow row in grdIntroInfo.Rows)
            {
                objcarsInfo.Make = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblMake")).Text).ToString();
                objcarsInfo.Model = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblModel")).Text).ToString();

                if (GenFunc.ExcelTextFormat(((Label)row.FindControl("lblPrice")).Text).ToString() == "")
                {
                    objcarsInfo.Price = Convert.ToDouble("0");
                }
                else
                {
                    objcarsInfo.Price = Convert.ToDouble(GenFunc.ExcelTextFormat(((Label)row.FindControl("lblPrice")).Text).ToString());
                }
                objcarsInfo.YearOfMake = Convert.ToInt32(GenFunc.ExcelTextFormat(((Label)row.FindControl("lblyear")).Text).ToString());
                objcarsInfo.Description = GenFunc.ExcelTextFormat(((HiddenField)row.FindControl("hdnDescription")).Value).ToString();
                if (GenFunc.ExcelTextFormat(((Label)row.FindControl("lblMileage")).Text).ToString().Replace("mi", "").Replace(",", "").Trim() == "")
                {
                    objcarsInfo.Mileage = "0";
                }
                else
                {
                    objcarsInfo.Mileage = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblMileage")).Text).ToString().Replace("mi", "").Replace(",", "").Trim();
                }
                objcarsInfo.Bodytype = GenFunc.ExcelTextFormat(((Label)row.FindControl("BodyType")).Text).ToString();
                objcarsInfo.ExteriorColor = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblExteriorColor")).Text).ToString();
                objcarsInfo.InteriorColor = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblInteriorColor")).Text).ToString();
                objcarsInfo.VIN = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblVIN")).Text).ToString();
                objcarsInfo.Fueltype = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblFuelType")).Text).ToString();
                objcarsInfo.NumberOfCylinder = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblNumberOfCylinder")).Text).ToString();
                objcarsInfo.Transmission = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblTransmission")).Text).ToString();
                //objcarsInfo.Wheelbase = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblWheelbase")).Text).ToString();
                objcarsInfo.NumberOfDoors = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblDoors")).Text).ToString();
                objcarsInfo.DriveTrain = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblDrivetrain")).Text).ToString();
                //objcarsInfo.VehicleConditionID = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblVehicleCondition")).Text).ToString();
                //objcarsInfo.NumberOfCylinder = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblVehicleCondition")).Text).ToString();

                objUsedCarsInfo.SellerID = 0;


                DataSet dsPackages = objSaveCar.USP_ChkPackageForAddCar(UID);

                int PaymentID;

                //**************************************************???///
                //during we need take user package from dealer package id 
                //**************************************************???///
                int UserPackID = 0;

                if (dsPackages.Tables[0].Rows.Count > 0)
                {
                    UserPackID = Convert.ToInt32(dsPackages.Tables[0].Rows[0]["UserPackID"].ToString());
                }
                else
                {
                    dsPackages = objSaveCar.USP_ChkExistingPackage(UID);
                    if (dsPackages.Tables[0].Rows.Count > 0)
                    {
                        UserPackID = Convert.ToInt32(dsPackages.Tables[0].Rows[0]["UserPackID"].ToString());
                        //objUsedCarsInfo.AdStatus = "6";

                    }
                }
                objUsedCarsInfo.AdStatus = "6";

                objcarsInfo.ConditionDescription = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblConditionDescription")).Text).ToString();

                string sDealerCarID = GenFunc.ExcelTextFormat(((HiddenField)row.FindControl("hndDealerUniqueID")).Value).ToString();

                objUsedCarsInfo.SellerID = Convert.ToInt32(0);

                string Source = "";
                string SourcePicID = "";

                string SellerType = "2";
                string SellerID = "0";

                objcarsInfo.DealerUniqueID = GenFunc.ExcelTextFormat(((HiddenField)row.FindControl("hndDealerUniqueID")).Value).ToString();

                objcarsInfo.Uid = Convert.ToInt32(Session[Constants.USER_ID].ToString());

                //UserControl UC = (UserControl)Page.LoadControl("Usercontrol/Header.ascx");

                //System.Web.UI.WebControls.Label lblDealerCode = (System.Web.UI.WebControls.Label)UC.FindControl("lblDealerCode");



                DataSet dsCarsDetails = objSaveCar.SaveCarDetailsForBulkUploadDealers(objcarsInfo, SellerID, SellerType, SourcePicID, Source, sDealerCarID, UserPackID.ToString(), Session[Constants.DealerCode].ToString());

                objcarsInfo.Carid = Convert.ToInt32(dsCarsDetails.Tables[0].Rows[0][0].ToString());


                //string sFilePath = GenFunc.ExcelTextFormat(((Label)row.FindControl("lblPicpath")).Text).ToString();



                //SaveImages(sFilePath, objcarsInfo.YearOfMake.ToString(), objcarsInfo.Make.ToString(), objcarsInfo.Model.ToString(), objcarsInfo.Carid.ToString());

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

                //}
                // int PackageID = Convert.ToInt32(Session["PackageID"]); //Convert.ToInt32(ddlPackage.SelectedItem.Value);



            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //    protected void btnUpload_Click(object sender, System.EventArgs e)
    //{
    //    if ((txtFilePath.HasFile)) {
    //        OleDbConnection conn = default(OleDbConnection);
    //        OleDbCommand cmd = default(OleDbCommand);
    //        OleDbDataAdapter da = default(OleDbDataAdapter);
    //        DataSet ds = null;
    //        string query = null;
    //        string connString = "";
    //        string strFileName = DateTime.Now.ToString("ddMMyyyy_HHmmss");
    //        string strFileType = System.IO.Path.GetExtension(txtFilePath.FileName).ToString().ToLower();

    //        //Check file type
    //        if (strFileType.Trim() == ".xls" | strFileType.Trim() == ".xlsx") {
    //            txtFilePath.SaveAs(Server.MapPath("~/UploadedExcel/" + strFileName + strFileType));
    //        } else {
    //            lblMessage.Text = "Only excel files allowed";
    //            lblMessage.ForeColor = System.Drawing.Color.Red;
    //            lblMessage.Visible = true;
    //            return;
    //        }

    //        string strNewPath = Server.MapPath("~/UploadedExcel/" + strFileName + strFileType);

    //        //Connection String to Excel Workbook
    //        if (strFileType.Trim() == ".xls") {
    //            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
    //        } else if (strFileType.Trim() == ".xlsx") {
    //            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
    //        }

    //        query = "SELECT * FROM [Sheet1$]";

    //        //Create the connection object
    //        conn = new OleDbConnection(connString);
    //        //Open connection
    //        if (conn.State == ConnectionState.Closed)
    //            conn.Open();
    //        //Create the command object
    //        cmd = new OleDbCommand(query, conn);
    //        da = new OleDbDataAdapter(cmd);
    //        ds = new DataSet();
    //        da.Fill(ds);

    //        grvExcelData.DataSource = ds.Tables[0];
    //        grvExcelData.DataBind();

    //        da.Dispose();
    //        conn.Close();
    //        conn.Dispose();
    //    } else {
    //        lblMessage.Text = "Please select an excel file first";
    //        lblMessage.ForeColor = System.Drawing.Color.Red;
    //        lblMessage.Visible = true;
    //    }
    //}



    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string FILENAME = string.Empty;
        string OpenPath = string.Empty;
        string SaveLoc = string.Empty;
        string FileExt = string.Empty;
        string hp = string.Empty;
        string sLoc = string.Empty;

        ArrayList RowNo = new ArrayList();
        ArrayList ColNo = new ArrayList();

        ArrayList SArray = new ArrayList();

        FileBL objFilesBL = new FileBL();

        IDataReader IdataReader = null;


        if (Page.IsValid)
        {
            try
            {


                FileExt = System.IO.Path.GetExtension(fuAttachments.PostedFile.FileName);
                //Get The Ext of File 
                //FileName = Server.MapPath(".") + fuAttachments.PostedFile.FileName;

                if (FileExt != ".xls" && FileExt != ".xlsx")
                {
                    lblErrorMsg.Text = "You have not selected a Excel File.Kindly select Excel File";
                    //grdErrors.DataSource = null;
                    //grdErrors.DataBind();

                    return;
                }
                if (fuAttachments.HasFile)
                {
                    OpenPath = fuAttachments.PostedFile.FileName;

                    FILENAME = OpenPath;
                    if (FILENAME.Contains("\\"))
                    {
                        string[] strFile = FILENAME.Split('\\');
                        int max = strFile.Length - 1;
                        FILENAME = strFile[max].ToString();
                    }

                    ViewState["FileName"] = FILENAME;

                    SaveLoc = Server.MapPath("DealerFileUpload\\");

                    if (System.IO.Directory.Exists(SaveLoc) == false)
                    {
                        // Try to create the directory. 
                        System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(SaveLoc);
                    }
                    string strFormat = String.Format("{0:MMMddyyyyhhmmss}", System.DateTime.Now);
                    SaveLoc = Server.MapPath("DealerFileUpload\\" + strFormat + FileExt);


                    IdataReader = objFilesBL.Get_FileByFileName(FILENAME);

                    while (IdataReader.Read())
                    {
                        lblErrorMsg.Text = "File Already Exists. Please Upload a new file";

                        btnUpload.Text = "Upload";

                        return;
                    }

                    fuAttachments.PostedFile.SaveAs(SaveLoc);

                    Session["SaveLocation"] = SaveLoc;

                    Session["FileName"] = FILENAME;

                    ReadExcelData(SaveLoc);
                }
                else
                {
                    btnSubmit.Text = "Process";
                    btnSubmit.Enabled = true;
                    btnUpload.Enabled = false;

                }
            }
            catch (Exception ex)
            {
                throw ex;
                //bool rethrow = ExceptionPolicy.HandleException(ex, ConstantClass.StrCRMUIPolicy);
                //if (rethrow)
                //    throw;
                //Redirecting to error message page
                //Response.Redirect(ConstantClass.StrErrorPageURL);
            }
        }
    }


    private void ReadExcelData(string sFileName)
    {
        ArrayList RowNo = new ArrayList();
        ArrayList ColNo = new ArrayList();

        ArrayList SArray = new ArrayList();

        ExcelReading objExcelData = new ExcelReading();

        DataSet ds = new DataSet();
        DataSet dsStatus = new DataSet();

        DataSet dsSales = new DataSet();
        bool bnew = false;

        grdErrors.DataSource = null;
        grdErrors.DataBind();

        try
        {

            lblErrorMsg.Text = "";

            ds = objExcelData.GetExcelToDataset(sFileName);

            Session["ExcelData"] = ds;

            DataSet dsError = new DataSet();

            dsError.Tables.Add();
            dsError.Tables["Table1"].Columns.Add("DealerUniqueID");
            dsError.Tables["Table1"].Columns.Add("RowNo");
            dsError.Tables["Table1"].Columns.Add("Error");

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("Price") &&
                        ds.Tables[0].Columns.Contains("year")
                        && ds.Tables[0].Columns.Contains("Make") &&
                        ds.Tables[0].Columns.Contains("Model") &&
                        ds.Tables[0].Columns.Contains("Description") &&
                        ds.Tables[0].Columns.Contains("Mileage") &&
                        ds.Tables[0].Columns.Contains("BodyStyle") &&
                        ds.Tables[0].Columns.Contains("ExteriorColor") &&
                        ds.Tables[0].Columns.Contains("InteriorColor") &&
                        ds.Tables[0].Columns.Contains("VIN") &&
                        ds.Tables[0].Columns.Contains("FuelType") &&
                        ds.Tables[0].Columns.Contains("Transmission") &&
                        ds.Tables[0].Columns.Contains("Wheelbase") &&
                        ds.Tables[0].Columns.Contains("Doors") &&
                        ds.Tables[0].Columns.Contains("Drivetrain") &&
                        ds.Tables[0].Columns.Contains("SellerNotes") &&
                        //ds.Tables[0].Columns.Contains("VehicleCondition") &&
                        ds.Tables[0].Columns.Contains("ConditionDescription") &&
                        ds.Tables[0].Columns.Contains("NumberOfCylinder")

                        )
                    {


                        if (ds != null)
                        {


                            if (ds.Tables[0].Rows.Count > 5000)
                            {
                                lblErrorMsg.Text = "Upload Only 5000 rows Only!";
                                return;
                            }
                            else if (Convert.ToInt32(txtRecordCount.Text) != Convert.ToInt32(ds.Tables[0].Rows.Count))
                            {
                                lblErrorMsg.Text = "Records Count Does Not Match!";
                                return;
                            }
                            else if (ds.Tables[0].Rows.Count > 0)
                            {

                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {

                                    if (ds.Tables[0].Rows[i]["Make"].ToString().Trim() == "")
                                    {
                                        dsError.Tables["Table1"].Rows.Add();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["DealerUniqueID"] = ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["RowNo"] = (i + 1).ToString();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["Error"] = "Please Enter Make in the Excel sheet";
                                    }
                                    else if (ds.Tables[0].Rows[i]["Model"].ToString().Trim() == "")
                                    {
                                        dsError.Tables["Table1"].Rows.Add();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["DealerUniqueID"] = ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["RowNo"] = (i + 1).ToString();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["Error"] = "Please Enter Valid Model in the Excel sheet";
                                    }
                                    else if (!GeneralFunc.IsNumeric(ds.Tables[0].Rows[i]["year"].ToString().Trim()))
                                    {
                                        dsError.Tables["Table1"].Rows.Add();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["DealerUniqueID"] = ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["RowNo"] = (i + 1).ToString();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["Error"] = "Please Enter Numeric Year in the Excel sheet";
                                    }
                                    else if (!GeneralFunc.CheckCurrentYear(ds.Tables[0].Rows[i]["year"].ToString().Trim()))
                                    {
                                        dsError.Tables["Table1"].Rows.Add();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["DealerUniqueID"] = ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["RowNo"] = (i + 1).ToString();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["Error"] = "Please Enter Year which should be less than or equal to current year in Excel Sheet ";
                                    }
                                        

                                    else if (!GeneralFunc.IsNumeric(ds.Tables[0].Rows[i]["Mileage"].ToString().Trim()))
                                    {
                                        dsError.Tables["Table1"].Rows.Add();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["DealerUniqueID"] = ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["RowNo"] = (i + 1).ToString();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["Error"] = "Please Enter Numeric Mileage in the Excel sheet";
                                    }
                                    if (ds.Tables[0].Rows[i]["Price"].ToString().Trim() == "")
                                    {
                                        dsError.Tables["Table1"].Rows.Add();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["DealerUniqueID"] = ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["RowNo"] = (i + 1).ToString();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["Error"] = "Please Enter Price in the Excel sheet";
                                    }
                                    else if (!GeneralFunc.IsNumeric(ds.Tables[0].Rows[i]["Price"].ToString().Trim()))
                                    {
                                        dsError.Tables["Table1"].Rows.Add();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["DealerUniqueID"] = ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["RowNo"] = (i + 1).ToString();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["Error"] = "Please Enter Numeric Price in the Excel sheet";
                                    }


                                    else if (ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim() == "")
                                    {
                                        dsError.Tables["Table1"].Rows.Add();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["DealerUniqueID"] = ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["RowNo"] = (i + 1).ToString();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["Error"] = "Please Enter Dealer UniqueID in the Excel sheet";
                                    }
                                    DealerActions objActions = new DealerActions();
                                    DataSet dsCheckUser = objActions.DealerCheckUniqueID(ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim(), Session[Constants.DealerCode].ToString());
                                    if (dsCheckUser.Tables[0].Rows.Count > 0)
                                    {
                                        dsError.Tables["Table1"].Rows.Add();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["DealerUniqueID"] = ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["RowNo"] = (i + 1).ToString();
                                        dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["Error"] = "Dealer Unique already exists";
                                    }
                                }
                                if (dsError.Tables[0].Rows.Count > 0)
                                {
                                    grdIntroInfo.DataSource = null;
                                    grdIntroInfo.DataBind();
                                    recGrid.Style.Add("display", "block");
                                    grdErrors.DataSource = dsError.Tables["Table1"].DefaultView;
                                    grdErrors.DataBind();
                                    //Header.Visible = false;
                                }
                                else
                                {
                                    grdErrors.DataSource = null;
                                    grdErrors.DataBind();
                                    Header.Visible = true;
                                    recGrid.Style.Add("display", "block");
                                    grdIntroInfo.DataSource = ds.Tables[0].DefaultView;
                                    grdIntroInfo.DataBind();
                                    btnSubmit.Enabled = false;
                                    btnUpload.Enabled = true;
                                }
                            }
                        }
                    }

                    else
                    {
                        btnUpload.Text = "Upload";
                        lblErrorMsg.Text = "Enter Valid Sales Excel Sheet Contain Proper Columns.";
                        btnSubmit.Enabled = true;
                    }

                }
                else
                {
                    btnUpload.Text = "Upload";
                    lblErrorMsg.Text = "No Records access from file!";
                    btnSubmit.Enabled = true;
                }
            }

        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
}



//DirectoryInfo FileNameFullFolderDir = new DirectoryInfo(ds.Tables[0].Columns.Contains("Picpath").ToString());
//string[] validExtensionsTemp = new string[] { ".jpg", ".bmp", ".gif", ".png", ".jpeg" };
//int k = 0;
//foreach (FileInfo f in FileNameFullFolderDir.GetFiles())
//{
//    for (int j = 0; j < validExtensionsTemp.Length; j++)
//    {
//        if (f.Extension.ToString().ToLower() == validExtensionsTemp[j].ToLower())
//        {

//        }
//    }
//}
//HttpFileCollection uploadFilCol = Request.Files;
//for (int k = 0; k < uploadFilCol.Count; i++)
//{
//    HttpPostedFile file = uploadFilCol[k];
//    string fileExt = Path.GetExtension(file.FileName).ToLower();
//    string fileName = Path.GetFileName(file.FileName);
//    if (fileName != string.Empty)
//    {
//        try
//        {
//            if (fileExt == ".jpg" || fileExt == ".gif")
//            {
//                file.SaveAs(Server.MapPath("./Images/") + fileName);
//            }
//            else
//            {
//                file.SaveAs(Server.MapPath("./Others/") + fileName);
//            }
//        }
//        catch (Exception ex)
//        {
//            throw ex;
//        }
//    }
//}

//filMyFile.Value = @"" + ds.Tables[0].Rows[i]["Picpath"].ToString();


// string FileNameFullPath = Server.MapPath("Dealer/DealerFileUpload");

//HttpPostedFile myFile = filMyFile.PostedFile;

//objfile.PostedFile = myFile;


//myFile.SaveAs(FileNameFullPath); 


//string file = System.IO.Path.GetFileName(uploads.FileName);

//if (System.IO.Directory.Exists(ds.Tables[0].Rows[i]["Picpath"].ToString()) == false)
//{
//    dsError.Tables["Table1"].Rows.Add();
//    dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["DealerUniqueID"] = ds.Tables[0].Rows[i]["DealerUniqueID"].ToString().Trim();
//    dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["RowNo"] = (i + 1).ToString();
//    dsError.Tables["Table1"].Rows[dsError.Tables["Table1"].Rows.Count - 1]["Error"] = "Please Enter Proper Picture Path ";

//}
