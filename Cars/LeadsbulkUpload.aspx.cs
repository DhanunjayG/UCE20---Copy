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

public partial class LeadsbulkUpload : System.Web.UI.Page
{
    LeadsBulkBL objdropdownBL = new LeadsBulkBL();
    DataSet dsDropDown = new DataSet();
    CarsInfo.CarsInfo objcarsInfo = new CarsInfo.CarsInfo();
    UsedCarsInfo objUsedCarsInfo = new UsedCarsInfo();
    public GeneralFunc objGeneralFunc = new GeneralFunc();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            // lblAlert.Visible = true;
            // lblAlert.Text = "Validated data will be inserted into the database. Do you want to continue?";
            // mdepAlert2.Show();


            lblResult.Text = "Saving Data.....";

            //bool bnew = objLead.Save_BatchDetails(Session["BatchDesc"].ToString(), grdDisplay.Rows.Count, ref Batch_ID);



            UploadExcel();

            lblResult.Text = "Saved Successfully.....";

        }
        catch (Exception ex)
        {
            Response.Redirect("Error.aspx");

        }
        finally
        {
            //mymail.Dispose();
            //smtpClient = null;
        }
    }
    private bool UploadExcel()
    {
        try
        {
            string FILENAME = string.Empty;
            string OpenPath = string.Empty;
            string SaveLoc = string.Empty;
            string FileExt = string.Empty;
            string hp = string.Empty;
            ExcelReading objExcelData = new ExcelReading();
            string sFileName = string.Empty;
            FILENAME = fupExcelFile.PostedFile.FileName;

            if (FILENAME.Contains("\\"))
            {
                string[] strFile = FILENAME.Split('\\');
                int max = strFile.Length - 1;
                FILENAME = strFile[max].ToString();
            }


            //It is date-username-filename.3digit unique for the day and user numeric ext .xls
            //20111103-admin-filenamemax15char.001.xls
            SaveLoc = Server.MapPath("LeadBulkFiles\\");



            if (System.IO.Directory.Exists(SaveLoc) == false)
            {
                System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(SaveLoc);
            }
            SaveLoc = Server.MapPath("LeadBulkFiles\\" + FILENAME);


            fupExcelFile.PostedFile.SaveAs(SaveLoc);

            DataSet ds = objExcelData.GetSalesExcelToDatasetLeads(SaveLoc);
            Session["ExcelDataset"] = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                SaveData(ds);
            }

        }

        catch (Exception ex)
        {
            Response.Redirect("Error.aspx");
        }
        return true;


    }

    private void SaveData(DataSet ds)
    {
        try
        {
            for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
            {
                objcarsInfo.YearOfMake = Convert.ToInt32(ds.Tables[0].Rows[r]["year"].ToString());
                objcarsInfo.MakeModelID = Convert.ToInt32(ds.Tables[0].Rows[r]["MakeModelID"].ToString());
                objcarsInfo.BodyTypeID = Convert.ToInt32(ds.Tables[0].Rows[r]["BodyTypeID"].ToString());
                objcarsInfo.CarID = Convert.ToInt32(0);
                if (ds.Tables[0].Rows[r]["Price"].ToString() == "")
                {
                    objcarsInfo.Price = "0";
                }
                else
                {
                    objcarsInfo.Price = ds.Tables[0].Rows[r]["Price"].ToString();
                }
                if (ds.Tables[0].Rows[r]["Mileage"].ToString() == "")
                {
                    objcarsInfo.Mileage = "0";
                }
                else
                {
                    objcarsInfo.Mileage = ds.Tables[0].Rows[r]["Mileage"].ToString();
                }
                objcarsInfo.ExteriorColor = ds.Tables[0].Rows[r]["ExteriorColor"].ToString();
                objcarsInfo.InteriorColor = ds.Tables[0].Rows[r]["InteriorColor"].ToString();
                objcarsInfo.Transmission = ds.Tables[0].Rows[r]["Transmission"].ToString();
                objcarsInfo.DriveTrain = ds.Tables[0].Rows[r]["Drivetrain"].ToString();
                objcarsInfo.NumberOfDoors = ds.Tables[0].Rows[r]["Doors"].ToString();
                objcarsInfo.VIN = ds.Tables[0].Rows[r]["VIN"].ToString();
                objcarsInfo.NumberOfCylinder = "Unspecified";
                objcarsInfo.FuelTypeID = Convert.ToInt32(ds.Tables[0].Rows[r]["FuelTypeID"].ToString());
                if (ds.Tables[0].Rows[r]["ZipCode"].ToString().Length == 4)
                {
                    objcarsInfo.Zipcode = "0" + ds.Tables[0].Rows[r]["ZipCode"].ToString();
                }
                else
                {
                    objcarsInfo.Zipcode = ds.Tables[0].Rows[r]["ZipCode"].ToString();
                }
                objcarsInfo.City = GeneralFunc.ToProper(ds.Tables[0].Rows[r]["City"].ToString());
                objcarsInfo.StateID = Convert.ToInt32(ds.Tables[0].Rows[r]["StateID"].ToString());
                string Condition = string.Empty;
                string CondiDescrip = string.Empty;
                Condition = GeneralFunc.ToProper(ds.Tables[0].Rows[r]["Description"].ToString());
                CondiDescrip = ds.Tables[0].Rows[r]["ConditionDescription"].ToString();

                DataSet dsCarsDetails = objdropdownBL.USP_SaveCarDetailsForBulkUpload(objcarsInfo, Condition, CondiDescrip);

                Session["CarID"] = Convert.ToInt32(dsCarsDetails.Tables[0].Rows[0]["CarID"].ToString());
                
                int CarIDs;
                
                objUsedCarsInfo.SellerName = GeneralFunc.ToProper(ds.Tables[0].Rows[r]["SellerName"].ToString());
                objUsedCarsInfo.Address1 = GeneralFunc.ToProper(ds.Tables[0].Rows[r]["Address"].ToString());
                objUsedCarsInfo.City = GeneralFunc.ToProper(ds.Tables[0].Rows[r]["City"].ToString());
                objUsedCarsInfo.State = ds.Tables[0].Rows[r]["State"].ToString();
                if (ds.Tables[0].Rows[r]["ZipCode"].ToString().Length == 4)
                {
                    objUsedCarsInfo.Zip = "0" + ds.Tables[0].Rows[r]["ZipCode"].ToString();
                }
                else
                {
                    objUsedCarsInfo.Zip = ds.Tables[0].Rows[r]["ZipCode"].ToString();
                }
                objUsedCarsInfo.Phone = ds.Tables[0].Rows[r]["Phone"].ToString();
                objUsedCarsInfo.Email = "";
                objUsedCarsInfo.SellerID = Convert.ToInt32(0);
                CarIDs = Convert.ToInt32(Session["CarID"].ToString());
                string Source = ds.Tables[0].Rows[r]["Url"].ToString();
                string SourcePicID = ds.Tables[0].Rows[r]["SourcePicID"].ToString();

                int SellerType = Convert.ToInt32(ds.Tables[0].Rows[r]["SellerType"].ToString());
                string HomeViewCount = ds.Tables[0].Rows[r]["HomeViewCount"].ToString();
             
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

                bool bnew = objdropdownBL.USP_SaveSellerInfoForBulkUpload(objUsedCarsInfo, CarIDs, Source, SourcePicID, SellerType, HomeViewCount);
                //for (int i = 1; i < 52; i++)
                //{
                //    if (Session["CarID"] == null)
                //    {
                //        CarIDs = 0;
                //    }
                //    else
                //    {
                //        CarIDs = Convert.ToInt32(Session["CarID"].ToString());
                //    }

                //    Isactive = 0;
                //    FeatureID = i;
                //    DataSet dsCarFeature = objdropdownBL.USP_SaveCarFeatures(CarIDs, FeatureID, Isactive);
                //    Session["CarID"] = Convert.ToInt32(dsCarFeature.Tables[0].Rows[0]["CarID"].ToString());
                //}
                lblCount.Text = r.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
