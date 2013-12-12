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
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

public partial class SavePhotosFolders : System.Web.UI.Page
{
    LeadsBulkBL objdropdownBL = new LeadsBulkBL();
    DataSet dsDropDown = new DataSet();
    CarsInfo.CarsInfo objcarsInfo = new CarsInfo.CarsInfo();
    UsedCarsInfo objUsedCarsInfo = new UsedCarsInfo();
    public GeneralFunc objGeneralFunc = new GeneralFunc();
    public string ResultText = string.Empty;
    public ArrayList ArrResult = new ArrayList();
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
            lblCount.Text = ResultText;

        }
        catch (Exception ex)
        {
            //Response.Redirect("Error.aspx");
            lblError.Text = ex.ToString();
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
            //Response.Redirect("Error.aspx");
            lblError.Text = ex.ToString();
        }
        return true;


    }

    private void SaveData(DataSet ds)
    {
        try
        {
            for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
            {
                int PostingID = Convert.ToInt32(ds.Tables[0].Rows[r]["PostingID"].ToString());
                string SourcePicID = ds.Tables[0].Rows[r]["SourcePicID"].ToString();
                string Path = ds.Tables[0].Rows[r]["Path"].ToString();
                string FolderName = Path + "\\" + SourcePicID;
                if (System.IO.Directory.Exists(FolderName))
                {
                    DataSet dsCarsdetails = objdropdownBL.USP_GetDetailsBySourcePicID(PostingID);
                    if (dsCarsdetails.Tables[0].Rows.Count > 0)
                    {
                        DateTime PostedDate = Convert.ToDateTime(dsCarsdetails.Tables[0].Rows[0]["dateOfPosting"].ToString());
                        string PostedYear = PostedDate.Year.ToString();
                        string PostedMonth = PostedDate.Month.ToString();
                        string MakeName = dsCarsdetails.Tables[0].Rows[0]["make"].ToString();
                        string CarID = dsCarsdetails.Tables[0].Rows[0]["carid"].ToString();
                        string sFilePath = string.Empty;
                        sFilePath = "CollectedImages/" + PostedYear;
                        string sFilePathDir = Server.MapPath(sFilePath);
                        if (System.IO.Directory.Exists(sFilePathDir) == false)
                        {
                            System.IO.Directory.CreateDirectory(sFilePathDir);
                        }
                        string sFilePath2 = sFilePath + "/" + PostedMonth;
                        string sFilePath2Dir = Server.MapPath(sFilePath2);
                        if (System.IO.Directory.Exists(sFilePath2Dir) == false)
                        {
                            System.IO.Directory.CreateDirectory(sFilePath2Dir);
                        }
                        string sFilePath21 = sFilePath2 + "/" + MakeName;
                        string sFilePath2Dir2 = Server.MapPath(sFilePath21);
                        if (System.IO.Directory.Exists(sFilePath2Dir2) == false)
                        {
                            System.IO.Directory.CreateDirectory(sFilePath2Dir2);
                        }
                        string sFilePath3 = sFilePath21 + "/" + CarID.ToString() + "/";
                        string sFilePath3Dir = Server.MapPath(sFilePath3);
                        if (System.IO.Directory.Exists(sFilePath3Dir) == false)
                        {
                            System.IO.Directory.CreateDirectory(sFilePath3Dir);
                        }

                        ArrayList str = new ArrayList();
                        objcarsInfo.Pic0 = null;
                        objcarsInfo.Pic1 = null;
                        objcarsInfo.Pic2 = null;
                        objcarsInfo.Pic3 = null;
                        objcarsInfo.Pic4 = null;
                        objcarsInfo.Pic5 = null;
                        objcarsInfo.Pic6 = null;
                        objcarsInfo.Pic7 = null;
                        objcarsInfo.Pic8 = null;
                        objcarsInfo.Pic9 = null;
                        objcarsInfo.Pic10 = null;
                        objcarsInfo.Pic11 = null;
                        objcarsInfo.Pic12 = null;
                        objcarsInfo.Pic13 = null;
                        objcarsInfo.Pic14 = null;
                        objcarsInfo.Pic15 = null;
                        objcarsInfo.Pic16 = null;
                        objcarsInfo.Pic17 = null;
                        objcarsInfo.Pic18 = null;
                        objcarsInfo.Pic19 = null;
                        objcarsInfo.Pic20 = null;

                        DirectoryInfo FileNameFullFolderDir = new DirectoryInfo(FolderName);
                        string[] validExtensionsTemp = new string[] { ".jpg", ".bmp", ".gif", ".png", ".jpeg" };
                        int k = 1;

                        foreach (FileInfo f in FileNameFullFolderDir.GetFiles())
                        {
                            for (int i = 0; i < validExtensionsTemp.Length; i++)
                            {
                                if (f.Extension.ToString().ToLower() == validExtensionsTemp[i].ToLower())
                                {

                                    string FileName = k.ToString() + f.Extension.ToString().ToLower();
                                    string FileNameFull = Server.MapPath(sFilePath3);
                                    string Destination = FileNameFull + FileName;
                                    File.Copy(f.FullName, Destination, true);
                                    FileNameFull = sFilePath3;
                                    DataSet dsImgID = objdropdownBL.USP_SavePictures(FileNameFull, FileName, f.Extension.ToString().ToLower(),0);
                                    //str[k] = dsImgID.Tables[0].Rows[0]["picID"].ToString();
                                    str.Add(dsImgID.Tables[0].Rows[0]["picID"].ToString());
                                    k = k + 1;

                                }
                            }
                        }
                        for (int j = 0; j < str.Count; j++)
                        {
                            if (str[j].ToString() != "" && str[j].ToString() != "0" && str[j].ToString() != " ")
                            {
                                if (objcarsInfo.Pic1 == null)
                                {
                                    objcarsInfo.Pic1 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic2 == null)
                                {
                                    objcarsInfo.Pic2 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic3 == null)
                                {
                                    objcarsInfo.Pic3 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic4 == null)
                                {
                                    objcarsInfo.Pic4 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic5 == null)
                                {
                                    objcarsInfo.Pic5 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic6 == null)
                                {
                                    objcarsInfo.Pic6 = str[j].ToString();
                                }

                                else if (objcarsInfo.Pic7 == null)
                                {
                                    objcarsInfo.Pic7 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic8 == null)
                                {
                                    objcarsInfo.Pic8 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic9 == null)
                                {
                                    objcarsInfo.Pic9 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic10 == null)
                                {
                                    objcarsInfo.Pic10 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic11 == null)
                                {
                                    objcarsInfo.Pic11 = str[j].ToString();
                                }

                                else if (objcarsInfo.Pic12 == null)
                                {
                                    objcarsInfo.Pic12 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic13 == null)
                                {
                                    objcarsInfo.Pic13 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic14 == null)
                                {
                                    objcarsInfo.Pic14 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic15 == null)
                                {
                                    objcarsInfo.Pic15 = str[j].ToString();
                                }

                                else if (objcarsInfo.Pic16 == null)
                                {
                                    objcarsInfo.Pic16 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic17 == null)
                                {
                                    objcarsInfo.Pic17 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic18 == null)
                                {
                                    objcarsInfo.Pic18 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic19 == null)
                                {
                                    objcarsInfo.Pic19 = str[j].ToString();
                                }
                                else if (objcarsInfo.Pic20 == null)
                                {
                                    objcarsInfo.Pic20 = str[j].ToString();
                                }
                            }

                        }
                        string picID0 = string.Empty;
                        foreach (FileInfo f in FileNameFullFolderDir.GetFiles())
                        {
                            for (int i = 0; i < validExtensionsTemp.Length; i++)
                            {
                                if (f.Extension.ToString().ToLower() == validExtensionsTemp[i].ToLower())
                                {                        // File.Move(f.FullName, Path.Combine(NewTempDir.FullName, FileNameFind));

                                    if (picID0 == "")
                                    {
                                        string FileNameThumb = CarID + "_Thumb" + f.Extension.ToString().ToLower();
                                        string FileNameFullThumb = Server.MapPath(sFilePath3);
                                        //string FileNameFullThumb = Server.MapPath("CollectedImages/" + txtFolderNumber.Text);
                                        //  string FileNameSaveData = "CollectedImages/" + txtFolderNumber.Text;
                                        Bitmap originalBMP = new Bitmap(f.FullName);

                                        // Calculate the new image dimensions
                                        int origWidth = originalBMP.Width;
                                        int origHeight = originalBMP.Height;
                                        int sngRatio = origWidth / origHeight;
                                        int newWidth = 200;
                                        int newHeight = 122;
                                        // Create a new bitmap which will hold the previous resized bitmap
                                        Bitmap newBMP = new Bitmap(originalBMP, newWidth, newHeight);
                                        // Create a graphic based on the new bitmap
                                        Graphics oGraphics = Graphics.FromImage(newBMP);
                                        // Set the properties for the new graphic file
                                        oGraphics.SmoothingMode = SmoothingMode.AntiAlias; oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                        // Draw the new graphic based on the resized bitmap
                                        oGraphics.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                                        //flupImage20.PostedFile.SaveAs(FileNameFull + "/" + FileName);
                                        // Save the new graphic file to the server
                                        newBMP.Save(FileNameFullThumb + "/" + FileNameThumb);

                                        // Once finished with the bitmap objects, we deallocate them.
                                        originalBMP.Dispose();
                                        newBMP.Dispose();
                                        oGraphics.Dispose();
                                        FileNameFullThumb = sFilePath3;
                                        DataSet dsImgID = objdropdownBL.USP_SavePictures(FileNameFullThumb, FileNameThumb, "jpg",0);
                                        picID0 = dsImgID.Tables[0].Rows[0]["picID"].ToString();
                                        break;
                                    }


                                }
                            }
                        }
                        objcarsInfo.Pic0 = picID0;
                        objcarsInfo.CarID = Convert.ToInt32(CarID);
                        bool bnew = objdropdownBL.USP_UpdatePicturesById(objcarsInfo);

                    }
                    else
                    {
                        if (ResultText == "")
                        {
                            ResultText = "Not found any results for " + (r + 2).ToString() + " row";
                        }
                        else
                        {
                            ResultText = ResultText + ", " + "Not found any results for " + (r + 2).ToString() + " row";
                        }
                    }
                }
                else
                {
                    if (ResultText == "")
                    {
                        ResultText = "Not found any results for " + (r + 2).ToString() + " row";
                    }
                    else
                    {
                        ResultText = ResultText + ", " + "Not found any results for " + (r + 2).ToString() + " row";
                    }
                }
            }

        }
        catch (Exception ex)
        {
            // Response.Redirect("Error.aspx");
            //lblError.Text = ex.ToString();
        }
    }
}
