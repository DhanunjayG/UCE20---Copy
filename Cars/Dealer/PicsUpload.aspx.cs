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


public partial class _Default : System.Web.UI.Page
{
    DealerActions objDealerAction = new DealerActions();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Constants.NAME] == null)
        {
            Response.Redirect("../login.aspx");
        }

    }
    protected void imageOrder_Click(object sender, EventArgs e)
    {
        try
        {
            string CarID = hdnCarID.Value;
            string Make = hdnMake.Value;
            string Model = hdnModel.Value;
            string Year = hdnYear.Value;
            string Pic0 = hdnPic0.Value;
            string Pic1 = hdnPic1.Value;
            string Pic2 = hdnPic2.Value;
            string Pic3 = hdnPic3.Value;
            string Pic4 = hdnPic4.Value;
            string Pic5 = hdnPic5.Value;
            string Pic6 = hdnPic6.Value;
            string Pic7 = hdnPic7.Value;
            string Pic8 = hdnPic8.Value;
            string Pic9 = hdnPic9.Value;
            string Pic10 = hdnPic10.Value;
            string Pic11 = hdnPic11.Value;
            string Pic12 = hdnPic12.Value;
            string Pic13 = hdnPic13.Value;
            string Pic14 = hdnPic14.Value;
            string Pic15 = hdnPic15.Value;
            string Pic16 = hdnPic16.Value;
            string Pic17 = hdnPic17.Value;
            string Pic18 = hdnPic18.Value;
            string Pic19 = hdnPic19.Value;
            string Pic20 = hdnPic20.Value;

            DataSet dsImagesData = objDealerAction.USP_GetImages(Convert.ToInt32(CarID), 7);

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

            if ((Pic1 != null) && (Pic1 != "") && (Pic1 != "0"))
            {
                if (objcarsInfo.PIC1 != Pic1)
                {
                    DataSet dsdata = objDealerAction.GetPictureLocationByID(Pic1);
                    string FileNameSaveData1 = Server.MapPath(dsdata.Tables[0].Rows[0]["picturelocation"].ToString() + dsdata.Tables[0].Rows[0]["picturename"].ToString());
                    string FileNameSaveData = dsdata.Tables[0].Rows[0]["picturelocation"].ToString() + dsdata.Tables[0].Rows[0]["picturename"].ToString();
                    System.Web.UI.WebControls.Image Image1 = (System.Web.UI.WebControls.Image)form1.FindControl("Img1");
                    Image1.ImageUrl = "~/" + FileNameSaveData;
                    string SavePath = dsdata.Tables[0].Rows[0]["picturelocation"].ToString();
                    string path = Server.MapPath(Image1.ImageUrl);
                    //string path = Server.MapPath(ImageName.ImageUrl);
                    string SelModelName = Model.ToString();
                    SelModelName = SelModelName.Replace("/", "@");
                    SelModelName = SelModelName.Replace("&", "@");
                    SelModelName = SelModelName.Replace(" ", "-");
                    string FileNameThumb = Year + "_" + Make + "_" + SelModelName + "_" + CarID + "_Thumb.jpg";
                    string FileNameFullThumb = Server.MapPath("~/" + dsdata.Tables[0].Rows[0]["picturelocation"].ToString());
                    System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                    //int newwidthimg = 250;
                    //float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                    //int newHeight = Convert.ToInt32(newwidthimg / AspectRatio);
                    //Bitmap thumbnailBitmap = new Bitmap(newwidthimg, newHeight);
                    //Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                    //thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                    //thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                    //thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //var imageRectangle = new Rectangle(0, 0, newwidthimg, newHeight);
                    //thumbnailGraph.DrawImage(image, imageRectangle);
                    //thumbnailBitmap.Save(SavePath, ImageFormat.Jpeg);
                    //thumbnailGraph.Dispose();
                    //thumbnailBitmap.Dispose();
                    //image.Dispose();
                    //if (System.IO.Directory.Exists(FileNameSaveData1) == false)
                    //{
                    //    System.IO.Directory.CreateDirectory(FileNameSaveData1);
                    //}

                    Bitmap oBitmap1 = default(Bitmap);
                    oBitmap1 = new Bitmap(path);
                    Graphics oGraphic1 = default(Graphics);

                    int newwidthimg1 = 250;
                    // Here create a new bitmap object of the same height and width of the image.
                    //float AspectRatio = (float)oBitmap.Size.Width / (float)oBitmap.Size.Height;
                    float AspectRatio1 = (float)oBitmap1.Size.Width / (float)oBitmap1.Size.Height;

                    int newHeight1 = Convert.ToInt32(newwidthimg1 / AspectRatio1);

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
                    oBitmap1.Save(FileNameFullThumb + FileNameThumb, ImageFormat.Jpeg);

                    oBitmap1.Dispose();

                    DealerActions objDealerActions1 = new DealerActions();

                    DataSet dsImgID1 = objDealerActions1.SavePictures(SavePath, FileNameThumb, "Jpeg", Convert.ToInt32(Session[Constants.USER_ID].ToString()));

                    objcarsInfo.PIC0 = dsImgID1.Tables[0].Rows[0]["picID"].ToString();

                }
                objcarsInfo.PIC1 = Pic1;
            }
            else
            {
                objcarsInfo.PIC0 = "";
                objcarsInfo.PIC1 = "";
            }
            if ((Pic2 != null) && (Pic2 != "") && (Pic2 != "0"))
            {
                objcarsInfo.PIC2 = Pic2;
            }
            else
            {
                objcarsInfo.PIC2 = "";
            }
            if ((Pic3 != null) && (Pic3 != "") && (Pic3 != "0"))
            {
                objcarsInfo.PIC3 = Pic3;
            }
            else
            {
                objcarsInfo.PIC3 = "";
            }
            if ((Pic4 != null) && (Pic4 != "") && (Pic4 != "0"))
            {
                objcarsInfo.PIC4 = Pic4;
            }
            else
            {
                objcarsInfo.PIC4 = "";
            }
            if ((Pic5 != null) && (Pic5 != "") && (Pic5 != "0"))
            {
                objcarsInfo.PIC5 = Pic5;
            }
            else
            {
                objcarsInfo.PIC5 = "";
            }
            if ((Pic6 != null) && (Pic6 != "") && (Pic6 != "0"))
            {
                objcarsInfo.PIC6 = Pic6;
            }
            else
            {
                objcarsInfo.PIC6 = "";
            }
            if ((Pic7 != null) && (Pic7 != "") && (Pic7 != "0"))
            {
                objcarsInfo.PIC7 = Pic7;
            }
            else
            {
                objcarsInfo.PIC7 = "";
            }
            if ((Pic8 != null) && (Pic8 != "") && (Pic8 != "0"))
            {
                objcarsInfo.PIC8 = Pic8;
            }
            else
            {
                objcarsInfo.PIC8 = "";
            }
            if ((Pic9 != null) && (Pic9 != "") && (Pic9 != "0"))
            {
                objcarsInfo.PIC9 = Pic9;
            }
            else
            {
                objcarsInfo.PIC9 = "";
            }
            if ((Pic10 != null) && (Pic10 != "") && (Pic10 != "0"))
            {
                objcarsInfo.PIC10 = Pic10;
            }
            else
            {
                objcarsInfo.PIC10 = "";
            }
            if ((Pic11 != null) && (Pic11 != "") && (Pic11 != "0"))
            {
                objcarsInfo.PIC11 = Pic11;
            }
            else
            {
                objcarsInfo.PIC11 = "";
            }
            if ((Pic12 != null) && (Pic12 != "") && (Pic12 != "0"))
            {
                objcarsInfo.PIC12 = Pic10;
            }
            else
            {
                objcarsInfo.PIC12 = "";
            }
            if ((Pic13 != null) && (Pic13 != "") && (Pic13 != "0"))
            {
                objcarsInfo.PIC13 = Pic13;
            }
            else
            {
                objcarsInfo.PIC13 = "";
            }
            if ((Pic14 != null) && (Pic14 != "") && (Pic14 != "0"))
            {
                objcarsInfo.PIC14 = Pic14;
            }
            else
            {
                objcarsInfo.PIC14 = "";
            }
            if ((Pic15 != null) && (Pic15 != "") && (Pic15 != "0"))
            {
                objcarsInfo.PIC15 = Pic15;
            }
            else
            {
                objcarsInfo.PIC15 = "";
            }
            if ((Pic16 != null) && (Pic16 != "") && (Pic16 != "0"))
            {
                objcarsInfo.PIC16 = Pic16;
            }
            else
            {
                objcarsInfo.PIC16 = "";
            }
            if ((Pic17 != null) && (Pic17 != "") && (Pic17 != "0"))
            {
                objcarsInfo.PIC17 = Pic17;
            }
            else
            {
                objcarsInfo.PIC17 = "";
            }
            if ((Pic18 != null) && (Pic18 != "") && (Pic18 != "0"))
            {
                objcarsInfo.PIC18 = Pic18;
            }
            else
            {
                objcarsInfo.PIC18 = "";
            }
            if ((Pic19 != null) && (Pic19 != "") && (Pic19 != "0"))
            {
                objcarsInfo.PIC19 = Pic19;
            }
            else
            {
                objcarsInfo.PIC19 = "";
            }
            if ((Pic20 != null) && (Pic20 != "") && (Pic20 != "0"))
            {
                objcarsInfo.PIC20 = Pic20;
            }
            else
            {
                objcarsInfo.PIC20 = "";
            }
            objcarsInfo.Carid = Convert.ToInt32(CarID);
            bool bnew = objDealerAction.UpdatePicturesById(objcarsInfo, Convert.ToInt32(Session[Constants.USER_ID].ToString()));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}
