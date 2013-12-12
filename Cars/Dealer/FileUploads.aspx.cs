using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using CarsBL.Dealer;
using CarsInfo;

public partial class FileUploads : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session[Constants.NAME] == null)
        //{
        //    Response.Redirect("../login.aspx");
        //}        
        HttpPostedFile uploads = Request.Files["FileData"];
        //HttpContext.Current.Request["RequireUploadifySessionSync"]

        string FileExt = System.IO.Path.GetExtension(uploads.FileName);
        //Get The Ext of File 
        //FileName = Server.MapPath(".") + fuAttachments.PostedFile.FileName;

        /*
        if (FileExt != ".jpg" || FileExt != ".png"||FileExt != ".jpeg")
        {   

            //return "You have not selected a Excel File.Kindly select Excel File"; 
        }
         */

        string file = System.IO.Path.GetFileName(uploads.FileName);

        try
        {

            string[] Query1 = Request.QueryString[0].Split('*');

            string Make = Query1[0];
            string Model = Query1[1];
            Model = Model.Replace("/", "@");
            Model = Model.Replace("&", "@");
            string Year = Query1[2];
            string CarID = Query1[3];

            int k = 0;

            if (Session["k"] == null)
            {
                k = 0;
            }
            else
            {
                k = Convert.ToInt32(Session["k"].ToString()) + 1;
            }
            string Filepath = "CarImages//" + Year + "//" + Make + "//" + Model + "//" + CarID + "//";

            string FileNameFullPath = Server.MapPath("../CarImages/" + Year + "/" + Make + "/" + Model + "/" + CarID);

            Session["k"] = k;
            string SelModel = Model.Replace(" ", "-");
            string sNewFileName = Year + "_" + Make + "_" + SelModel + "_" + CarID + k + ".jpg";
            string sNewFileName1 = Year + "_" + Make + "_" + SelModel + "_" + CarID + k + "Thumb.jpg";


            string FileNameSaveData = FileNameFullPath + "//" + sNewFileName;
            string FileNameSaveData1 = FileNameFullPath + "//" + sNewFileName1;

            if (System.IO.Directory.Exists(FileNameFullPath) == false)
            {
                System.IO.Directory.CreateDirectory(FileNameFullPath);
            }



            uploads.SaveAs(FileNameSaveData);
            




            Bitmap oBitmap = default(Bitmap);
            oBitmap = new Bitmap(FileNameSaveData);
            Graphics oGraphic = default(Graphics);

            int newwidthimg = 600;
            // Here create a new bitmap object of the same height and width of the image.
            float AspectRatio = (float)oBitmap.Size.Width / (float)oBitmap.Size.Height;

            int newHeight = Convert.ToInt32(newwidthimg / AspectRatio);

            Bitmap bmpNew = new Bitmap(newwidthimg, newHeight);
            oGraphic = Graphics.FromImage(bmpNew);

            oGraphic.CompositingQuality = CompositingQuality.HighQuality;
            oGraphic.SmoothingMode = SmoothingMode.HighQuality;
            oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;


            oGraphic.DrawImage(oBitmap, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), 0, 0, oBitmap.Width, oBitmap.Height, GraphicsUnit.Pixel);
            // Release the lock on the image file. Of course,
            // image from the image file is existing in Graphics object
            oBitmap.Dispose();
            oBitmap = bmpNew;

            //SolidBrush oBrush = new SolidBrush(Color.Black);
            //Font ofont = new Font("Arial", 8);
            //oGraphic.DrawString("Some text to write", ofont, oBrush, 10, 10);
            //oGraphic.Dispose();
            //ofont.Dispose();
            //oBrush.Dispose();
            oBitmap.Save(FileNameSaveData, ImageFormat.Jpeg);

            oBitmap.Dispose();


            DealerActions objDealerActions = new DealerActions();

            
            DataSet dsImgID = objDealerActions.SavePictures(Filepath, sNewFileName, "Jpeg", Convert.ToInt32(Session[Constants.USER_ID].ToString()));

            ArrayList str = new ArrayList();

            //if (Session["Pics"] == null)
            //{
            //    Session["Pics"] = str;
            //}
            //else
            //{
            //    str = (ArrayList)Session["Pics"];
            //}

            //str[k] = dsImgID.Tables[0].Rows[0]["picID"].ToString();
            str.Add(dsImgID.Tables[0].Rows[0]["picID"].ToString());

            DataSet dsImagesData = objDealerActions.USP_GetImages(Convert.ToInt32(CarID), 7);

            //DataSet dsImagesData = Session["GetImages"] as DataSet;
            UsedCarsInfo objcarsInfo = new UsedCarsInfo();

            objcarsInfo.PIC0 = dsImagesData.Tables[0].Rows[0]["pic0"].ToString();
            objcarsInfo.PIC1 = dsImagesData.Tables[0].Rows[0]["pic1"].ToString();
            objcarsInfo.PIC2 = dsImagesData.Tables[0].Rows[0]["pic2"].ToString();
            objcarsInfo.PIC3 = dsImagesData.Tables[0].Rows[0]["pic3"].ToString();
            objcarsInfo.PIC4 = dsImagesData.Tables[0].Rows[0]["pic4"].ToString();
            objcarsInfo.PIC5 = dsImagesData.Tables[0].Rows[0]["pic5"].ToString();
            objcarsInfo.PIC6 = dsImagesData.Tables[0].Rows[0]["pic6"].ToString();
            objcarsInfo.PIC7 = dsImagesData.Tables[0].Rows[0]["pic7"].ToString();
            objcarsInfo.PIC8 = dsImagesData.Tables[0].Rows[0]["pic8"].ToString();
            objcarsInfo.PIC9 = dsImagesData.Tables[0].Rows[0]["pic9"].ToString();
            objcarsInfo.PIC10 = dsImagesData.Tables[0].Rows[0]["pic10"].ToString();
            objcarsInfo.PIC11 = dsImagesData.Tables[0].Rows[0]["pic11"].ToString();
            objcarsInfo.PIC12 = dsImagesData.Tables[0].Rows[0]["pic12"].ToString();
            objcarsInfo.PIC13 = dsImagesData.Tables[0].Rows[0]["pic13"].ToString();
            objcarsInfo.PIC14 = dsImagesData.Tables[0].Rows[0]["pic14"].ToString();
            objcarsInfo.PIC15 = dsImagesData.Tables[0].Rows[0]["pic15"].ToString();
            objcarsInfo.PIC16 = dsImagesData.Tables[0].Rows[0]["pic16"].ToString();
            objcarsInfo.PIC17 = dsImagesData.Tables[0].Rows[0]["pic17"].ToString();
            objcarsInfo.PIC18 = dsImagesData.Tables[0].Rows[0]["pic18"].ToString();
            objcarsInfo.PIC19 = dsImagesData.Tables[0].Rows[0]["pic19"].ToString();
            objcarsInfo.PIC20 = dsImagesData.Tables[0].Rows[0]["pic20"].ToString();


            for (int j = 0; j < str.Count; j++)
            {
                
                if (str[j].ToString() != "" && str[j].ToString() != "0" && str[j].ToString() != " ")
                {
                    
                    if (objcarsInfo.PIC1 == null || objcarsInfo.PIC1 == "" || objcarsInfo.PIC1 == "0")
                    {
                        objcarsInfo.PIC1 = str[j].ToString();
                        uploads.SaveAs(FileNameSaveData1);

                        Bitmap oBitmap1 = default(Bitmap);
                        oBitmap1 = new Bitmap(FileNameSaveData1);
                        Graphics oGraphic1 = default(Graphics);

                        int newwidthimg1 = 250;
                        // Here create a new bitmap object of the same height and width of the image.
                        float AspectRatio1 = (float)oBitmap1.Size.Width / (float)oBitmap1.Size.Height;

                        int newHeight1 = Convert.ToInt32(newwidthimg1 / AspectRatio);

                        Bitmap bmpNew1 = new Bitmap(newwidthimg1, newHeight1);
                        oGraphic1 = Graphics.FromImage(bmpNew1);

                        oGraphic1.CompositingQuality = CompositingQuality.HighQuality;
                        oGraphic1.SmoothingMode = SmoothingMode.HighQuality;
                        oGraphic1.InterpolationMode = InterpolationMode.HighQualityBicubic;


                        oGraphic1.DrawImage(oBitmap1, new Rectangle(0, 0, bmpNew1.Width, bmpNew1.Height), 0, 0, oBitmap1.Width, oBitmap1.Height, GraphicsUnit.Pixel);
                        // Release the lock on the image file. Of course,
                        // image from the image file is existing in Graphics object
                        oBitmap1.Dispose();
                        oBitmap1 = bmpNew1;

                        //SolidBrush oBrush = new SolidBrush(Color.Black);
                        //Font ofont = new Font("Arial", 8);
                        //oGraphic.DrawString("Some text to write", ofont, oBrush, 10, 10);
                        //oGraphic.Dispose();
                        //ofont.Dispose();
                        //oBrush.Dispose();
                        oBitmap1.Save(FileNameSaveData1, ImageFormat.Jpeg);

                        oBitmap.Dispose();


                        DealerActions objDealerActions1 = new DealerActions();


                        DataSet dsImgID1 = objDealerActions1.SavePictures(Filepath, sNewFileName, "Jpeg", Convert.ToInt32(Session[Constants.USER_ID].ToString()));

                        objcarsInfo.PIC0 = dsImgID1.Tables[0].Rows[0]["picID"].ToString();
                    }
                    else if (objcarsInfo.PIC2 == null || objcarsInfo.PIC2 == "" || objcarsInfo.PIC2 == "0")
                    {
                        objcarsInfo.PIC2 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC3 == null || objcarsInfo.PIC3 == "" || objcarsInfo.PIC3 == "0")
                    {
                        objcarsInfo.PIC3 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC4 == null || objcarsInfo.PIC4 == "" || objcarsInfo.PIC4 == "0")
                    {
                        objcarsInfo.PIC4 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC5 == null || objcarsInfo.PIC5 == "" || objcarsInfo.PIC5 == "0")
                    {
                        objcarsInfo.PIC5 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC6 == null || objcarsInfo.PIC6 == "" || objcarsInfo.PIC6 == "0")
                    {
                        objcarsInfo.PIC6 = str[j].ToString();
                    }

                    else if (objcarsInfo.PIC7 == null || objcarsInfo.PIC7 == "" || objcarsInfo.PIC7 == "0")
                    {
                        objcarsInfo.PIC7 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC8 == null || objcarsInfo.PIC8 == "" || objcarsInfo.PIC8 == "0")
                    {
                        objcarsInfo.PIC8 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC9 == null || objcarsInfo.PIC9 == "" || objcarsInfo.PIC9 == "0")
                    {
                        objcarsInfo.PIC9 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC10 == null || objcarsInfo.PIC10 == "" || objcarsInfo.PIC10 == "0")
                    {
                        objcarsInfo.PIC10 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC11 == null || objcarsInfo.PIC11 == "" || objcarsInfo.PIC11 == "0")
                    {
                        objcarsInfo.PIC11 = str[j].ToString();
                    }

                    else if (objcarsInfo.PIC12 == null || objcarsInfo.PIC12 == "" || objcarsInfo.PIC12 == "0")
                    {
                        objcarsInfo.PIC12 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC13 == null || objcarsInfo.PIC13 == "" || objcarsInfo.PIC13 == "0")
                    {
                        objcarsInfo.PIC13 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC14 == null || objcarsInfo.PIC14 == "" || objcarsInfo.PIC14 == "0")
                    {
                        objcarsInfo.PIC14 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC15 == null || objcarsInfo.PIC15 == "" || objcarsInfo.PIC15 == "0")
                    {
                        objcarsInfo.PIC15 = str[j].ToString();
                    }

                    else if (objcarsInfo.PIC16 == null || objcarsInfo.PIC16 == "" || objcarsInfo.PIC16 == "0")
                    {
                        objcarsInfo.PIC16 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC17 == null || objcarsInfo.PIC17 == "" || objcarsInfo.PIC17 == "0")
                    {
                        objcarsInfo.PIC17 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC18 == null || objcarsInfo.PIC18 == "" || objcarsInfo.PIC18 == "0")
                    {
                        objcarsInfo.PIC18 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC19 == null || objcarsInfo.PIC19 == "" || objcarsInfo.PIC19 == "0")
                    {
                        objcarsInfo.PIC19 = str[j].ToString();
                    }
                    else if (objcarsInfo.PIC20 == null || objcarsInfo.PIC20 == "" || objcarsInfo.PIC20 == "0")
                    {
                        objcarsInfo.PIC20 = str[j].ToString();
                    }
                }
                objcarsInfo.Carid = Convert.ToInt32(CarID);
            }

            bool bnew = objDealerActions.UpdatePicturesById(objcarsInfo, Convert.ToInt32(Session[Constants.USER_ID].ToString()));

        }
        catch(Exception ex)
        {

        }
    }

}
