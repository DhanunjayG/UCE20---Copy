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

public partial class SaveIMages : System.Web.UI.Page
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
            ArrayList str = new ArrayList();

            lblResult.Text = "Saving Data.....";
            string FileNameFullFolder = "CollectedImages/" + txtFolderNumber.Text;
            FileNameFullFolder = Server.MapPath(FileNameFullFolder);
            DirectoryInfo FileNameFullFolderDir = new DirectoryInfo(FileNameFullFolder);
            string[] validExtensionsTemp = new string[] { ".jpg", ".bmp", ".gif", ".png", ".jpeg" };
            int k = 0;

            foreach (FileInfo f in FileNameFullFolderDir.GetFiles())
            {
                for (int i = 0; i < validExtensionsTemp.Length; i++)
                {
                    if (f.Extension.ToString().ToLower() == validExtensionsTemp[i].ToLower())
                    {
                        // File.Move(f.FullName, Path.Combine(NewTempDir.FullName, FileNameFind));
                        string FileName = f.Name.ToString();
                        string FileNameFull = "CollectedImages/" + txtFolderNumber.Text;
                        //PicImg8 = FileNameFull + "/" + FileName;

                        DataSet dsImgID = objdropdownBL.USP_SavePictures(FileNameFull, FileName, f.Extension.ToString().ToLower(),0);
                        //str[k] = dsImgID.Tables[0].Rows[0]["picID"].ToString();
                        str.Add(dsImgID.Tables[0].Rows[0]["picID"].ToString());

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
                            string FileNameThumb = txtCarID.Text + "_Thumb.jpg";
                            string FileNameFullThumb = Server.MapPath("CollectedImages/" + txtFolderNumber.Text);
                            string FileNameSaveData = "CollectedImages/" + txtFolderNumber.Text;
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
                            FileNameFullThumb = FileNameFullThumb;
                            DataSet dsImgID = objdropdownBL.USP_SavePictures(FileNameSaveData, FileNameThumb, "jpg",0);
                            picID0 = dsImgID.Tables[0].Rows[0]["picID"].ToString();
                            break;
                        }


                    }
                }
            }

            objcarsInfo.Pic0 = picID0;
            objcarsInfo.CarID = Convert.ToInt32(txtCarID.Text);
            bool bnew = objdropdownBL.USP_UpdatePicturesById(objcarsInfo);


            lblResult.Text = "Saved Successfully.....";
            txtCarID.Text = "";
            txtFolderNumber.Text = "";

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

}
